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
        /// <summary>
        /// <code>POST: api/Task</code>
        /// Контроллер для просмотра задач
        /// </summary>
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            string ClientInfo = request.Headers.UserAgent.ToString();
            Logger.TaskLog.Info($"POST Получено сообщение от {ClientInfo}");

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            var req = new Data.Request.TaskRequest();

            try
            {
                var a = await request.Content.ReadAsStringAsync();
                Logger.TaskLog.Debug($"POST Содержимое сообщения {a}");
                req = JsonConvert.DeserializeObject<Data.Request.TaskRequest>(a);
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Error(exc, $"POST Ошибка сериализации в {await request.Content.ReadAsStringAsync()}");
                return MessageTemplate.SerializationError;
            }

            Logger.TaskLog.Trace($"POST Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.TaskLog.Debug($"POST Сессия авторизована");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.PlanLog.Error("POST Сессия не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.PlanLog.Error("POST Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            Logger.TaskLog.Trace("POST Определение работника по сессии");

            string WorkerId = string.Empty;

            try
            {
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.TaskLog.Debug($"POST Определен работник #{WorkerId}");
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, "POST Работник не найден");
                return MessageTemplate.InternalError;
            }

            if (req.TaskStages != null)
            {
                foreach(string stage in req.TaskStages)
                {
                    if (!DBClient.TaskStages.Contains(stage))
                    {
                        Logger.TaskLog.Info($"POST Стадия '{stage}' не сущетствует");
                        return MessageTemplate.BadStagesGiven;
                    }
                }
            }

            Logger.TaskLog.Trace($"POST Выполнение выборки");

            try
            {
                Logger.TaskLog.Trace($"POST Поиск задач по дате {req.DateCreation} и стадиям {req.TaskStages.ToArray()}");
                var tasks = DBClient.GetTasks(WorkerId, req.DateCreation, req.TaskStages);
                Logger.TaskLog.Debug($"POST найдено {tasks.ToList().Count} задач");
                response.Content = new StringContent(JsonConvert.SerializeObject(tasks));
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, $"POST Ошибка выборки задач по [{req.TaskStages}] и {req.DateCreation}");
                return MessageTemplate.InternalError;
            }

            return response;
        }

        /// <summary>
        /// <code>PUT: api/Task</code>
        /// Контроллер для установки стадий задачам
        /// </summary>
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
                var a = await request.Content.ReadAsStringAsync();
                Logger.TaskLog.Debug($"PUT Полученное сообщение");
                req = JsonConvert.DeserializeObject<Data.Request.TaskNewStage>(a);
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

            Logger.TaskLog.Trace($"PUT Авторизация сессии {req.Session}");

            switch (Authentication.Authenticate(req.Session, ClientInfo))
            {
                case AuthenticationResult.Ok:
                    Logger.TaskLog.Debug($"PUT Сессия авторизована");
                    break;

                case AuthenticationResult.SessionNotFound:
                    Logger.TaskLog.Info($"PUT Сессия {req.Session} не найдена");
                    return MessageTemplate.SessionNotFound;

                case AuthenticationResult.ClientNotFound:
                    Logger.TaskLog.Info($"PUT Клиент не найден");
                    return MessageTemplate.ClientNotFound;
            }

            try
            {
                Logger.TaskLog.Trace($"PUT Поиск работника по сессии");
                WorkerId = DBClient.GetWorkerId(req.Session);
                Logger.TaskLog.Debug($"PUT Авторизирован #{WorkerId}");
                Logger.TaskLog.Trace($"PUT Поиск выполняемого для задачи '{req.TaskId}'");
                SettedWorkerId = DBClient.GetWorkerIdFromTask(req.TaskId);
                Logger.TaskLog.Debug($"PUT Назначен #{SettedWorkerId}");
                if (WorkerId != SettedWorkerId) throw new Exception("Работник изменяет не свою задачу");
            }
            catch (Exception exc)
            {
                Logger.TaskLog.Fatal(exc, "PUT Ошибка поиска сотрудника или нахождения номера сотрудника по задаче");
                return MessageTemplate.InternalError;
            }

            if (DBClient.GetStatus(WorkerId, DateTime.Now).StatusCode != DBClient.State_Working)
            {
                Logger.TaskLog.Error("Сотрудник не на рабочем месте");
                return MessageTemplate.BadStatusWorker;
            }

            Logger.TaskLog.Trace($"PUT Проврка стадии задачи на возможность установить");

            if (req.Stage == DBClient.TaskStage_NotAccepted)
            {
                Logger.TaskLog.Error("PUT Нельзя установить задаче стадии непринятой");
                return MessageTemplate.BadTaskStage;
            }
            else if (!(req.Stage == DBClient.TaskStage_Completed || req.Stage == DBClient.TaskStage_Accepted))
            {
                Logger.TaskLog.Error($"PUT Некорректный номер стадии '{req.Stage}'");
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
                Logger.TaskLog.Error("PUT Стадии совпали");
                return response;
            }
            else if (PreviousStage == DBClient.TaskStage_Completed)
            {
                Logger.TaskLog.Error("PUT Задача уже завершена");
                return MessageTemplate.BadTaskStage;
            }
            else if (PreviousStage == DBClient.TaskStage_NotAccepted && req.Stage != DBClient.TaskStage_Accepted)
            {
                Logger.TaskLog.Error("PUT Задача ещё не принята");
                return MessageTemplate.BadTaskStage;
            }
            else if (PreviousStage == DBClient.TaskStage_Accepted && req.Stage != DBClient.TaskStage_Completed)
            {
                Logger.TaskLog.Error("PUT Задача должна быть выполнена");
                return MessageTemplate.BadTaskStage;
            }

            Logger.TaskLog.Debug("PUT Стадия может быть обновлена");

            try
            {
                Logger.TaskLog.Trace("PUT Обновление стадии");
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
