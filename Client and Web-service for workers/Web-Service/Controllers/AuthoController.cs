using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    public class AuthoController : ApiController
    {
        /// <summary>
        /// Авторизация по паролю и логину
        /// <code>POST: api/Autho</code>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.AuthoLog.Info($"POST Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();

            var data = new Data.Request.AuthoLogin();

            DateTime dateOfCreation = DateTime.Now;
            string sessionHash = string.Empty;
            string WorkerId    = string.Empty;

            try
            {
                data = JsonConvert.DeserializeObject<Data.Request.AuthoLogin>(await request.Content.ReadAsStringAsync());
                Logger.AuthoLog.Debug($"POST Login:'{data.Login}', Password:'{data.Password}'");
            }
            catch(Exception exc)
            {
                Logger.AuthoLog.Error($"POST Ошибка сериализации полученного сообщения: {exc.Message} в \"{await request.Content.ReadAsStringAsync()}\"");
                return MessageTemplate.SerializationError;
            }

            if(string.IsNullOrEmpty(data.Login) || string.IsNullOrEmpty(data.Password))
            {
                Logger.AuthoLog.Warn("POST Пустые данные авторизации");
            }
            Logger.AuthoLog.Trace("POST Создание сессии");
            try
            {
                sessionHash = Authentication.CreateSession(
                    data.Login,
                    data.Password,
                    ClientInfo,
                    dateOfCreation
                );
            }
            catch (AuthenticationExcecption exc)
            {
                Logger.AuthoLog.Error($"POST Ошибка авторизаци: {exc.Message}");
                return MessageTemplate.ClientNotFound;
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Fatal(exc, $"POST Не удалось создать сессию для #{WorkerId}");
                return MessageTemplate.SessionNotCreated;
            }

            Logger.AuthoLog.Debug($"POST Сессия {sessionHash} в базе данных");
            
            response.Content = new StringContent(JsonConvert.SerializeObject(new Data.Response.AuthoResult() { Session = sessionHash }));
            response.StatusCode = HttpStatusCode.OK;

            Logger.AuthoLog.Info($"POST Отправка ответа {ClientInfo}");

            return response;
        }
        /// <summary>
        /// Авторизация с использованием сессии
        /// </summary>
        /// <param name="request">Сообщение-запрос</param>
        /// <returns>Сообщение-ответ</returns>
        public async Task<HttpResponseMessage> Put(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.AuthoLog.Info($"PUT Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();

            var req = new Data.Request.AuthoSession();
            var authores = new AuthenticationResult();

            try
            {
                req = JsonConvert.DeserializeObject<Data.Request.AuthoSession>(await request.Content.ReadAsStringAsync());
                Logger.AuthoLog.Debug($"Session:'{req.Session}'");
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Error(exc, "PUT Ошибка сериализации");
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.AuthoLog.Warn("PUT Пустой номер сессии");
            }

            try
            {
                Logger.AuthoLog.Trace("Авторизация сессии");
                authores = Authentication.Authenticate(req.Session, ClientInfo);
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Fatal(exc, "PUT Ошибка обновления сессии");
                return MessageTemplate.InternalError;
            }

            switch(authores)
            {
                case AuthenticationResult.Ok:
                    Logger.AuthoLog.Debug($"PUT Сессия {req.Session} авторизирована");
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
                case AuthenticationResult.ClientNotFound:
                    Logger.AuthoLog.Debug("PUT Клиент не был найден");
                    return MessageTemplate.ClientNotFound;
                case AuthenticationResult.SessionNotFound:
                    Logger.AuthoLog.Debug("PUT Сессия не была найдена");
                    return MessageTemplate.SessionNotFound;
                default:
                    Logger.AuthoLog.Fatal("PUT Необработанная ошибка");
                    return MessageTemplate.InternalError;
            }
        }
    }
}
