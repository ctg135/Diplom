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
        // POST: api/Autho
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            Logger.AuthoLog.Info($"POST Получено сообщение от {request.Headers.UserAgent.ToString()}");
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

            if(WorkerId == string.Empty)
            {
                Logger.AuthoLog.Error("POST Работник не найден");
                return MessageTemplate.UserNotFound;
            }

            DateTime dateOfCreation = DateTime.Now;

            string sessionHash = Authentication.CreateNewSession(
                data.Login,
                data.Password,
                request.Headers.UserAgent.ToString(),
                dateOfCreation
            );

            try
            {
                DBClient.CreateSession(WorkerId, sessionHash, request.Headers.UserAgent.ToString(), dateOfCreation);
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Error($"api/Autho POST Не удалось создать сессию: {exc.Message}");
                return MessageTemplate.SessionNotCreated;
            }

            Logger.AuthoLog.Info($"api/Autho POST создана сессия {sessionHash} для #{WorkerId}");
            response.Content = new StringContent(sessionHash);
            Logger.AuthoLog.Info($"api/Autho POST Отправка ответа {request.Headers.UserAgent.ToString()}");
            return response;
        }

        // GET: api/Autho/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Autho
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }

        // PUT: api/Autho/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Autho/5
        public void Delete(int id)
        {
        }
    }
}
