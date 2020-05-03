using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;


namespace Web_Service.Controllers
{
    public class TaskController : ApiController
    {
        /// <summary>
        /// <code>api/Task GET</code>
        /// Контроллер, возвращающий спсиоу стадий задач
        /// </summary>
        /// <param name="request">Сообщение запрос</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.TaskLog.Info($"GET Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string tasks = JsonConvert.SerializeObject(DBClient.GetTaskStages());
                response.Content = new StringContent(tasks);
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, "GET Ошибка получения статусов");
                return MessageTemplate.InternalError;
            }

            Logger.TaskLog.Info($"GET Отправка ответа {ClientInfo}");
            response.StatusCode = HttpStatusCode.OK;
            return await Task.FromResult(response);
        }
        // POST: api/Task
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Task/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
