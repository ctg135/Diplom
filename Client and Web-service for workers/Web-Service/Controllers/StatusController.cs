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
                var a = DBClient.GetStatusTypes();
                Logger.StatusLog.Debug($"GET Найдено {a.ToArray().Length} статусов");
                string statuses = JsonConvert.SerializeObject(a);
                response.Content = new StringContent(statuses);
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Fatal(exc, "GET Ошибка получения статусов");
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
                var a = await request.Content.ReadAsStringAsync();
                Logger.StatusLog.Debug($"POST Содержимое сообщения '{a}'");
                req = JsonConvert.DeserializeObject<Data.Request.StatusUserRequest>(a);                
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error(exc, "POST Ошибка сериализации");
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.StatusLog.Warn("POST Пустой номер сессии");
            }

            Logger.StatusLog.Trace($"POST Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.StatusLog.Debug("POST Сессия авторизована");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Info($"POST Сессия {req.Session} не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Info($"POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.StatusLog.Trace($"POST Поиск работника по сессии {req.Session}");

            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.StatusLog.Debug($"Надйен #{WorkerId}");
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal(exc, "POST Ошибка поиска сотрудника");
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Trace("POST Просмотр статуса");

            try
            {
                var status = DBClient.GetStatus(WorkerId, DateTime.Now);
                var con = JsonConvert.SerializeObject(status);
                Logger.StatusLog.Debug($"POST Найден статус {con}");
                response.Content = new StringContent(con);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal(exc, "Ошибка получения статуса");
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

            var req = new Data.Request.NewStatusCode();

            try
            {
                var a = await request.Content.ReadAsStringAsync();
                Logger.StatusLog.Debug($"PUT Содержиоме:{a}");
                req = JsonConvert.DeserializeObject<Data.Request.NewStatusCode>(a);
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Error(exc, "PUT Ошибка сериализации");
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.StatusLog.Warn("PUT Пустой номер сессии");
            }

            Logger.StatusLog.Trace($"POST Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.StatusLog.Debug($"PUT Сессия авторизована");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.StatusLog.Error("PUT Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.StatusLog.Error("PUT Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            string WorkerId = string.Empty;

            try
            {
                Logger.StatusLog.Trace($"PUT Поиск работника");
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.StatusLog.Debug($"PUT Найден работник #{WorkerId}");
            }
            catch (Exception exc)
            {
                Logger.StatusLog.Fatal(exc, "POST Ошибка поиска сотрудника");
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Trace($"PUT установка статуса '{req.StatusCode}'");

            var currentStatus = DBClient.GetStatus(WorkerId, DateTime.Now).StatusCode;
            Logger.StatusLog.Debug($"PUT Установленный статус {currentStatus}");
            Logger.StatusLog.Trace("PUT Проверка статуса на корректность");
            if (currentStatus == req.StatusCode)
            {
                Logger.StatusLog.Info($"PUT Такой статус {req.StatusCode} уже установлен");
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }

            if (!DBClient.IsStatusCanBeUpdated(currentStatus))
            {
                Logger.StatusLog.Info("PUT Статус не может быть обновлён");
                return MessageTemplate.StatusSet_StatusNotUpdateble;
            }

            if (!DBClient.IsStatusCanBeSetted(req.StatusCode))
            {
                Logger.StatusLog.Info("PUT Статус не может быть установлен");
                return MessageTemplate.StatusSet_BadStatusCodeGiven;
            }

            if (currentStatus == DBClient.State_NotState && req.StatusCode == DBClient.State_Finished)
            {
                Logger.StatusLog.Info("PUT Рабочий день не начался");
                return MessageTemplate.StatusSet_BadStatusCodeGiven;
            }

            Logger.StatusLog.Debug($"PUT Статус может быть установлен");

            try
            {
                Logger.StatusLog.Trace($"PUT Ведение журнала");
                DBClient.LogStatus(WorkerId, req.StatusCode, DateTime.Now);
            }
            catch(Exception exc)
            {
                Logger.StatusLog.Fatal(exc, "PUT Ошибка установки статуса");
                return MessageTemplate.InternalError;
            }

            Logger.StatusLog.Info($"PUT Отправка ответа {ClientInfo}");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
