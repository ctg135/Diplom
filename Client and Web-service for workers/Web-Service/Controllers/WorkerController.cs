using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_Service.Controllers
{
    public class WorkerController : ApiController
    {
        // GET: api/Worker
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Worker/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Worker
        /// <summary>
        /// Создание сотрудника
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            // Выбор данных { Основные данные по таблице, хэш пароля, ифна о девайсе }

            // Вставка данных

            return response;
        }

        // PUT: api/Worker/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Worker/5
        public void Delete(int id)
        {
        }
    }
}
