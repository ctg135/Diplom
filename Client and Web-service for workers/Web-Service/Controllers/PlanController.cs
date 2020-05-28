using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    public class PlanController : ApiController
    {
        /// <summary>
        /// <code>api/Post GET</code>
        /// Контроллер получения типов планов
        /// </summary>
        /// <param name="request">Сообщение-запрос</param>
        /// <returns>Список типов планов</returns>
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.PlanLog.Info($"GET Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var pl = DBClient.GetPlanTypes();
                string plans = JsonConvert.SerializeObject(pl);
                response.Content = new StringContent(plans);
                Logger.PlanLog.Debug($"GET Всего планов:'{pl.ToArray().Length}'");
            }
            catch (Exception exc)
            {
                Logger.PlanLog.Fatal(exc, "GET Ошибка получения статусов");
                return MessageTemplate.InternalError;
            }

            response.StatusCode = HttpStatusCode.OK;
            return await Task.FromResult(response);
        }
        /// <summary>
        /// КОнтроллер для получения планов на выбранное количество дней 
        /// <code>POST: api/Plan</code>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.PlanLog.Info($"POST Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();

            var req = new Data.Request.PlansRequest();
            string WorkerId = string.Empty;
            DateTime StartDate;
            DateTime EndDate;

            try
            {
                Logger.PlanLog.Debug($"POST Содержимое сообщения '{request.Content.ReadAsStringAsync()}'");
                req = JsonConvert.DeserializeObject<Data.Request.PlansRequest>(await request.Content.ReadAsStringAsync());
                StartDate = DateTime.Parse(req.StartDate);
                EndDate = DateTime.Parse(req.EndDate);
            }
            catch (Exception exc)
            {
                Logger.PlanLog.Error(exc, $"POST Ошибка сериализации в {await request.Content.ReadAsStringAsync()}");
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.PlanLog.Warn("POST Пустая строка сессии");
            }

            Logger.PlanLog.Trace("POST Проверка на значение даты");

            if (StartDate > EndDate)
            {
                Logger.PlanLog.Error("POST Некорректный дипазон дат");
                return MessageTemplate.BadDatesGived;
            }

            Logger.PlanLog.Trace("POST Авторизация сессии");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.PlanLog.Debug($"POST Сессия {req.Session} авторизована");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.PlanLog.Error("POST Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.PlanLog.Error("POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }
            Logger.PlanLog.Trace("POST Поиск работника по сессии");
            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
            }
            catch(Exception exc)
            {
                Logger.PlanLog.Fatal(exc, "POST Работник не найден");
                return MessageTemplate.InternalError;
            }
            Logger.PlanLog.Debug($"POST Найден работник #{WorkerId}");
            try
            {
                Logger.PlanLog.Trace($"POST Поиск планов");
                Logger.PlanLog.Debug($"POST От {StartDate:dd.MM.yyyy} по {EndDate:dd.MM.yyyy} типов {req.PlanCodes.ToArray()}");

                var plans = new List<Data.Response.Plan>();
                Logger.PlanLog.Trace($"POST Опредление списка планов");
                if (req.PlanCodes == null || req.PlanCodes.ToArray().Length == 0)
                {
                    Logger.PlanLog.Trace($"POST Поиск без фильтра");
                    plans = new List<Data.Response.Plan>(DBClient.GetPlans(WorkerId, StartDate, EndDate));
                }
                else
                {
                    Logger.PlanLog.Trace($"POST Поиск с фильтром");
                    plans = new List<Data.Response.Plan>(DBClient.GetPlans(WorkerId, StartDate, EndDate, new List<string>(req.PlanCodes)));
                }
                Logger.PlanLog.Debug($"POST Всего найдено {plans.Count}");
                response.Content = new StringContent(JsonConvert.SerializeObject(plans));
            }
            catch (Exception exc)
            {
                Logger.PlanLog.Error(exc, "Ошибка получения планов");
                return MessageTemplate.InternalError;
            }

            Logger.PlanLog.Info($"POST Отправка ответа {ClientInfo}");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
