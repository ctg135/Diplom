using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;


namespace Web_Service.Controllers
{
    public class ConnectionController : ApiController
    {
        /// <summary>
        /// <code>GET: api/Connection</code>
        /// Производит обновление времени жизни сессии
        /// </summary>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.ConnectionLog.Info($"POST Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();

            string Session = string.Empty;

            try
            {
                var json = JObject.Parse(await request.Content.ReadAsStringAsync());
                Session = (string)json["Session"];
            }
            catch (Exception exc)
            {
                Logger.ConnectionLog.Error("POST Ошибка сериализации", exc);
                return MessageTemplate.BadMessage;
            }

            if (string.IsNullOrEmpty(Session))
            {
                Logger.ConnectionLog.Warn("POST Пустой номер сессии");
            }

            try
            {
                DBClient.UpdateSession(Session, DateTime.Now);
            }
            catch(Exception exc)
            {
                Logger.ConnectionLog.Fatal("POST Ошибка обновления сессии", exc);
                return MessageTemplate.InternalError;
            }

            Logger.ConnectionLog.Info($"POST Сессия {Session} обновлена");
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }
    }
}
