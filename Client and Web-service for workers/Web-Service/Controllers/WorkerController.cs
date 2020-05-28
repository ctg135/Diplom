using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    public class WorkerController : ApiController
    {
        /// <summary>
        /// Контроллер получения информации о сотруднике
        /// <br>POST: api/Worker</br>
        /// </summary>
        /// <param name="request">Соообщение-запрос</param>
        /// <returns>Информация о сотруднике</returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.WorkerLog.Info($"POST Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();

            var req = new Data.Request.WorkerInfoRequest();
            string WorkerId = string.Empty;

            try
            {
                var a = await request.Content.ReadAsStringAsync();
                Logger.WorkerLog.Debug($"POST Содержимое сообщения {a}");
                req = JsonConvert.DeserializeObject<Data.Request.WorkerInfoRequest>(a);
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Error(exc, "POST Ошибка сериализации");
                return MessageTemplate.SerializationError;
            }

            if(string.IsNullOrEmpty(req.Session))
            {
                Logger.WorkerLog.Warn("POST Пустой номер сессии");
            }

            Logger.StatusLog.Trace($"POST Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.WorkerLog.Debug($"POST Авторизация пройдена");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.WorkerLog.Info("POST Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.WorkerLog.Info("POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.WorkerLog.Trace($"POST Поиск работника по сессии");

            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.WorkerLog.Debug($"POST Найден #{WorkerId}");
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Fatal(exc, "POST Ошибка поиска сотрудника");
                return MessageTemplate.InternalError;
            }

            Logger.WorkerLog.Trace("Получение инфомрации о работнике");
            var worker = new Data.Response.WorkerInfo();

            try
            {
                worker = DBClient.GetWorker(WorkerId);
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Fatal(exc, $"POST Ошибка поиска информации о работнике #{WorkerId}");
                return MessageTemplate.WorkerNotFound;
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(worker));

            Logger.WorkerLog.Info($"POST Отправка ответа {ClientInfo}");
            return response;
        }
    }
}
