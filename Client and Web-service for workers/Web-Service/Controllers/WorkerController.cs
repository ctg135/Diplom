using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web_Service.DataBase;
using Web_Service.Loggers;

namespace Web_Service.Controllers
{
    public class WorkerController : ApiController
    {
        /// <summary>
        /// Контроллер получения информации о сотруднике
        /// <br>POST: api/Worker</br>
        /// </summary>
        /// <param name="request">Соообщение-запрос</param>
        /// <returns>Информация о сотруднике</returns>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.WorkerLog.Info($"POST Получено сообщение от {ClientInfo}");
            HttpResponseMessage response = new HttpResponseMessage();

            var req = new Data.Request.WorkerInfoRequest();

            try
            {
                req = JsonConvert.DeserializeObject<Data.Request.WorkerInfoRequest>(await request.Content.ReadAsStringAsync());
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Error("POST Ошибка сериализации", exc);
                return MessageTemplate.SerializationError;
            }

            if(string.IsNullOrEmpty(req.Session))
            {
                Logger.WorkerLog.Warn("POST Пустой номер сессии");
            }

            string WorkerId = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Fatal("POST Ошибка поиска сотрудника", exc);
                return MessageTemplate.UserNotFound;
            }

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.WorkerLog.Info("POST Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.WorkerLog.Info("POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            var worker = new Data.Response.WorkerInfo();

            try
            {
                worker = DBClient.GetWorker(WorkerId);
            }
            catch (Exception exc)
            {
                Logger.WorkerLog.Fatal("POST Ошибка поиска работника", exc);
                return MessageTemplate.WorkerNotFound;
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(worker));

            Logger.WorkerLog.Info($"POST Отправка ответа {ClientInfo}");
            return response;
        }
    }
}
