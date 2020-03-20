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


namespace Web_Service.Controllers
{
    public class ConnectionController : ApiController
    {
        /// <summary>
        /// <code>GET: api/Connection</code>
        /// Производит проверку подключения к серверу
        /// </summary>
        public async Task<HttpResponseMessage> Get(HttpRequestMessage message)
        {
            Logger.Log.Debug("api/Connection GET Получено сообщение");
            Logger.Log.Info("api/Connection GET Сообщение обработано");

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(await message.Content.ReadAsStringAsync()) };
        }
    }
}
