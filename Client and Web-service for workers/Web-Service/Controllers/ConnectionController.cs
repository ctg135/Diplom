using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_Service.DataBase;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
                Logger.ConnectionLog.Error("POST Пустой номер сессии");
                return MessageTemplate.BadMessage;
            }

            try
            {
                DBClient.UpdateSession(Session, DateTime.Now);
            }
            catch(Exception exc)
            {
                Logger.ConnectionLog.Error("POST Ошибка обновления сессии", exc);
                return MessageTemplate.BadProcessingMessage;
            }

            Logger.ConnectionLog.Info($"POST Статус обновлён для {ClientInfo}");
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }
    }
}
