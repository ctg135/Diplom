using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_Service.Controllers
{
    public class AuthoController : ApiController
    {
        // GET: api/Autho
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            return response;
        }

        // GET: api/Autho/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Autho
        public HttpResponseMessage Post(HttpRequestMessage request)
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
