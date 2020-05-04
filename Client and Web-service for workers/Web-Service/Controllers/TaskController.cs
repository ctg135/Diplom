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
        public async Task<HttpResponseMessage> Put(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.TaskLog.Info($"PUT Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            var req = new Data.Request.TaskNewStage();
            string WorkerId;
            string SettedWorkerId;

            try
            {
                req = JsonConvert.DeserializeObject<Data.Request.TaskNewStage>(await request.Content.ReadAsStringAsync());
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Error(exc, "PUT Ошибка сериализации");
                return MessageTemplate.SerializationError;
            }

            if (string.IsNullOrEmpty(req.Session))
            {
                Logger.TaskLog.Warn("PUT Пустой номер сессии");
            }

            Logger.TaskLog.Debug($"PUT Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.TaskLog.Info($"PUT Сессия {req.Session} не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.TaskLog.Info($"PUT Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            // Проверка работника

            try
            {
                Logger.TaskLog.Debug($"PUT Поиск работника по сессии '{req.Session}'");
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.TaskLog.Trace($"PUT Авторизирован #{WorkerId}");
                Logger.TaskLog.Debug($"PUT Поиск выполняемого для задачи '{req.TaskId}'");
                SettedWorkerId = DBClient.GetWorkerIdFromTask(req.TaskId);
                Logger.TaskLog.Trace($"PUT Назначен #{SettedWorkerId}");
                if (WorkerId != SettedWorkerId) throw new Exception("Работник изменяет не свою задачу");
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, "PUT Ошибка поиска сотрудника или нахождения номера сотрудника по задаче");
                return MessageTemplate.InternalError;
            }

            if (string.IsNullOrEmpty(SettedWorkerId))
            {
                Logger.TaskLog.Fatal("PUT Задача не имеет назначенного работника");
                return MessageTemplate.InternalError;
            }

            if (DBClient.GetStatus(WorkerId, DateTime.Now).StatusCode != DBClient.State_Working)
            {
                Logger.TaskLog.Info("Сотрудник не на рабочем месте");
                return MessageTemplate.BadStatusWorker;
            }

            // Проверка стадии

            if (req.Stage == DBClient.TaskStage_NotAccepted)
            {
                Logger.TaskLog.Info("PUT Нельзя установить задаче стадии непринятой");
                return MessageTemplate.BadTaskStage;
            }
            else if (!(req.Stage == DBClient.TaskStage_Completed || req.Stage == DBClient.TaskStage_Accepted))
            {
                Logger.TaskLog.Info($"PUT Некорректный номер стадии '{req.Stage}'");
                return MessageTemplate.BadTaskStage;
            }

            string PreviousStage;

            Logger.TaskLog.Debug($"PUT Проверка задачи '{req.TaskId}' на корректность установки стадии");

            try
            {
                PreviousStage = DBClient.GetTaskStage(req.TaskId);
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, $"PUT Ошибка поиска стадии задачи '{req.TaskId}'");
                return MessageTemplate.InternalError;
            }

            Logger.TaskLog.Debug($"PUT Задача '{req.TaskId}' имеет '{PreviousStage}' статус, попытка установки '{req.Stage}'");

            if (PreviousStage == req.Stage)
            {
                Logger.TaskLog.Info("PUT Стадии совпали");
                return response;
            }
            else if (PreviousStage == DBClient.TaskStage_Completed)
            {
                Logger.TaskLog.Info("PUT Задача уже завершена");
                return MessageTemplate.BadTaskStage;
            }
            else if (PreviousStage == DBClient.TaskStage_NotAccepted && req.Stage != DBClient.TaskStage_Accepted)
            {
                Logger.TaskLog.Info("PUT Задача ещё не принята");
                return MessageTemplate.BadTaskStage;
            }
            else if (PreviousStage == DBClient.TaskStage_Accepted && req.Stage != DBClient.TaskStage_Completed)
            {
                Logger.TaskLog.Info("PUT Задача должна быть выполнена");
                return MessageTemplate.BadTaskStage;
            }

            Logger.TaskLog.Debug("PUT Стадия может быть обновлена");

            try
            {
                DBClient.UpdateTaskStage(req.TaskId, req.Stage);
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, $"PUT Ошибка изменения стадии задачи '{req.TaskId}' на '{req.Stage}'");
                return MessageTemplate.InternalError;
            }

            Logger.TaskLog.Info($"PUT Отправка ответа {ClientInfo}");
            return await Task.FromResult(response);
        }
    }
}
