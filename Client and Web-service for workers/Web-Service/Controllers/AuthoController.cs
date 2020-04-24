using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Web_Service.DataBase;
using Web_Service.Loggers;
using Web_Service.Models;
using Web_Service.Data;

namespace Web_Service.Controllers
{
    public class AuthoController : ApiController
    {
        /// <summary>
        /// Авторизация
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
                Logger.AuthoLog.Error($"Ошибка авторизации");
                return MessageTemplate.ClientNotFound;
            }
            catch (Exception exc)
            {
                Logger.AuthoLog.Fatal($"POST Не удалось создать сессию для #{WorkerId}", exc);
                return MessageTemplate.SessionNotCreated;
            }

            Logger.AuthoLog.Info($"POST Сессия {sessionHash} в базе данных");
            
            response.Content = new StringContent("{\"Session\":\"" + sessionHash + "\"}");
            response.StatusCode = System.Net.HttpStatusCode.OK;

            Logger.AuthoLog.Info($"POST Отправка ответа {ClientInfo}");

            return response;
        }
    }
}
