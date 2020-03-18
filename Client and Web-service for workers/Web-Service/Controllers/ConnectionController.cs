using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_Service.Controllers
{
    public class ConnectionController : ApiController
    {
        /// <summary>
        /// <code>GET: api/Connection</code>
        /// Производит проверку подключения к серверу
        /// </summary>
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent("Ok") };
        }
    }
}
