using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Web_Service.DataBase;

namespace Web_Service.Controllers
{
    public class StatusController : ApiController
    {
        /// <summary>
        /// Контроллер для получения всех статусов
        /// <code>GET: api/Status</code>
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сообщение-ответ</returns>
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.StatusLog.Info($"GET Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string statuses = JsonConvert.SerializeObject(DBClient.GetStatuses());
                response.Content = new StringContent(statuses);
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error("GET Ошибка получения статусов", exc);
                return MessageTemplate.BadProcessingMessage;
            }

            Logger.StatusLog.Info($"GET Отправка ответа {ClientInfo}");
            return response;
        }

        /// <summary>
        /// Контроллер для получения статуса работника
        /// <code>POST: api/Status</code>
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сообщение-ответ</returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.StatusLog.Info($"POST Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();

            string Session = string.Empty;

            try
            {
                var json = JObject.Parse(await request.Content.ReadAsStringAsync());
                Session = (string)json["Session"];
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error("POST Ошибка сериализации", exc);
                return MessageTemplate.BadMessage;
            }

            if (string.IsNullOrEmpty(Session))
            {
                Logger.StatusLog.Error("POST Пустой номер сессии");
                return MessageTemplate.BadMessage;
            }
            string WorkerId = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(Session);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Error("POST Ошибка поиска сотрудника", exc);
            }
            
            Logger.StatusLog.Debug("POST Просмотр статуса для #" + WorkerId);

            switch (Authentication.Authenticate(Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Info("POST Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Info("POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            try
            {
                var status = DBClient.GetStatus(WorkerId, DateTime.Now);
                response.Content = new StringContent(JsonConvert.SerializeObject(status));
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Error("Ошибка получения статуса", exc);
                return MessageTemplate.BadProcessingMessage;
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
                Logger.StatusLog.Error("PUT Пустой номер сессии");
                return MessageTemplate.BadMessage;
            }

            string WorkerId = DBClient.GetWorkerId(Session);

            switch (Authentication.Authenticate(Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Info("PUT Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Info("PUT Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.StatusLog.Debug($"PUT установка статуса '{NewStatusWorker}' для #{WorkerId}");

            if(DBClient.IsLongStatus(NewStatusWorker))
            {
                Logger.StatusLog.Warn("PUT Попытка установить длительный статус");
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Недостаточно прав для установки статуса\""),
                    StatusCode = HttpStatusCode.BadRequest              
                };
            }

            if(DBClient.State_Finished == DBClient.GetStatus(WorkerId, DateTime.Now).Code)
            {
                Logger.StatusLog.Warn("PUT Попытка продолжтить рабочий день");
                return new HttpResponseMessage()
                {
                    Content = new StringContent("{\"Message\":\"Рабочий день закончен!\""),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            try
            {
                DBClient.LogStatus(WorkerId, NewStatusWorker, DateTime.Now);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Error("PUT Ошибка установки статуса", exc);
                return MessageTemplate.BadSetStatusMessage;
            }

            Logger.StatusLog.Info($"PUT Отправка ответа {ClientInfo}");
            return response;
        }
    }
}
