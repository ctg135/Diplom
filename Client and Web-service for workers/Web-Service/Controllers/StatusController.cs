using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    public class StatusController : ApiController
    {
        /// <summary>
        /// Контроллер для получения всех статусов
        /// <code>GET: api/Status</code>
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сообщение-ответ со списком всех возможных статусов</returns>
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.StatusLog.Info($"GET Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string statuses = JsonConvert.SerializeObject(DBClient.GetStatusTypes());
                response.Content = new StringContent(statuses);
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Fatal("GET Ошибка получения статусов", exc);
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Info($"GET Отправка ответа {ClientInfo}");
            response.StatusCode = HttpStatusCode.OK;
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Контроллер для получения статуса работника
        /// <code>POST: api/Status</code>
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сообщение-ответ со статусом пользвателя</returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.StatusLog.Info($"POST Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();

            string WorkerId = string.Empty;
            var req = new Data.Request.StatusUserRequest();

            try
            {
                req = JsonConvert.DeserializeObject<Data.Request.StatusUserRequest>(await request.Content.ReadAsStringAsync());                
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error("POST Ошибка сериализации", exc);
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.StatusLog.Warn("POST Пустой номер сессии");
            }

            Logger.StatusLog.Debug($"POST Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Info($"POST Сессия {req.Session} не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Info($"POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.StatusLog.Debug($"POST Поиск работника по сессии {req.Session}");

            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal("POST Ошибка поиска сотрудника", exc);
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Debug("POST Просмотр статуса для #" + WorkerId);

            try
            {
                var status = DBClient.GetStatus(WorkerId, DateTime.Now);
                response.Content = new StringContent(JsonConvert.SerializeObject(status));
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal("Ошибка получения статуса", exc);
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Info($"POST Отправка ответа {ClientInfo}");
            return response;
        }

        /// <summary>
        /// Контроллер для установки статуса
        /// <code>PUT: api/Status/</code>
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сообщение-ответ</returns>
        public async Task<HttpResponseMessage> Put(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.StatusLog.Info($"PUT Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();

            string Session         = string.Empty;
            string NewStatusWorker = string.Empty;

            try
            {
                var json = JObject.Parse(await request.Content.ReadAsStringAsync());
                Session         = (string)json["Session"];
                NewStatusWorker = (string)json["Query"]["Code"];
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error("PUT Ошибка сериализации", exc);
                return MessageTemplate.BadMessage;
            }

            if (string.IsNullOrEmpty(Session))
            {
                Logger.StatusLog.Warn("PUT Пустой номер сессии");
            }

            string WorkerId = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(Session);
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Fatal("POST Ошибка поиска сотрудника", exc);
                return MessageTemplate.UserNotFound;
            }

            switch (Authentication.Authenticate(Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Error("PUT Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Error("PUT Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.StatusLog.Debug($"PUT установка статуса '{NewStatusWorker}' для #{WorkerId}");

            var oldStatus = DBClient.GetStatus(WorkerId, DateTime.Now).StatusCode;

            if (oldStatus == NewStatusWorker)
            {
                Logger.StatusLog.Error($"PUT Такой статус {NewStatusWorker} уже установлен");
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Статус уже установлен\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (oldStatus == DBClient.State_Finished)
            {
                Logger.StatusLog.Error("PUT Попытка продолжтить рабочий день");
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Рабочий день закончен!\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (oldStatus == DBClient.State_NotState)
            {
                var plans = new List<Models.Plan>(DBClient.GetPlans(WorkerId, DateTime.Now, DateTime.Now));
                if (plans.Count < 1)
                {
                    Logger.StatusLog.Error("PUT Попытка выйти на работу в выходной");
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("{\"Message\":\"На сегодня нету графика\"}"),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            if (DBClient.IsLongStatus(NewStatusWorker) || DBClient.State_NotState == NewStatusWorker)
            {
                Logger.StatusLog.Error("PUT Попытка установить длительный статус");
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Недостаточно прав для установки статуса\"}"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            try
            {
                DBClient.LogStatus(WorkerId, NewStatusWorker, DateTime.Now);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal("PUT Ошибка установки статуса", exc);
                return MessageTemplate.BadSetStatusMessage;
            }

            Logger.StatusLog.Info($"PUT Отправка ответа {ClientInfo}");
            return response;
        }
    }
}
