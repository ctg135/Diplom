using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Web_Service.Models;
using Web_Service.DataBase;
using System.Threading.Tasks;


namespace Web_Service.Controllers
{
    public class AuthoController : ApiController
    {
        /// <summary>
        /// Авторизация
        /// <br><c>POST: api/Autho</c></br>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.AuthoLog.Info($"POST Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();
            
            Autho data = new Autho();
            try
            {
                data = JsonConvert.DeserializeObject<Autho>(await request.Content.ReadAsStringAsync());
            }
            catch(Exception exc)
            {
                Logger.AuthoLog.Error($"POST Ошибка сериализации полученного сообщения: {exc.Message} в \"{await request.Content.ReadAsStringAsync()}\"");
                return MessageTemplate.BadProcessingMessage;
            }

            if(string.IsNullOrEmpty(data.Login) && string.IsNullOrEmpty(data.Password))
            {
                Logger.AuthoLog.Error("POST Пустые данные авторизации");
                return MessageTemplate.BadMessage;
            }

            string WorkerId = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(data.Login, data.Password);
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Error($"POST Ошибка поиска сотрудника: {exc.Message}");
                return MessageTemplate.UserNotFound;
            }

            if(string.IsNullOrEmpty(WorkerId))
            {
                Logger.AuthoLog.Error("POST Работник не найден");
                return MessageTemplate.UserNotFound;
            }

            DateTime dateOfCreation = DateTime.Now;

            string sessionHash = Authentication.CreateSessionHash(
                data.Login,
                data.Password,
                ClientInfo,
                dateOfCreation
            );

            try
            {
                DBClient.CreateSession(WorkerId, sessionHash, ClientInfo, dateOfCreation);
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Error($"api/Autho POST Не удалось создать сессию: {exc.Message}");
                return MessageTemplate.SessionNotCreated;
            }

            Logger.AuthoLog.Info($"api/Autho POST создана сессия {sessionHash} для #{WorkerId}");
            response.Content = new StringContent(sessionHash);
            Logger.AuthoLog.Info($"api/Autho POST Отправка ответа {ClientInfo}");
            return response;
        }
    }
}
