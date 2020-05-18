using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Client.DataModels;
using Client.Models;
using Client.ViewModels;
using System.Net.Http;
using Xamarin.Essentials;
using System.Security.Cryptography;
using Request = Web_Service.Data.Request;
using Response = Web_Service.Data.Response;

namespace Client.Droid.Models
{
    /// <summary>
    /// Класс для работы с сервером
    /// </summary>
    class RestClient : IClientModel
    {
        /// <summary>
        /// Контроллеры
        /// </summary>
        private class Controller
        {
            public const string Autho = "Autho";
            public const string Status = "Status";
            public const string Plan = "Plan";
            public const string Task = "Task";
            public const string Worker = "Worker";
        }
        /// <summary>
        /// Стадии задач
        /// </summary>
        private class TaskStageData
        {
            public const string NotAccepted = "1";
            public const string Processing = "2";
            public const string Completed = "3";
        }
        private class Plans
        {
            public const string Working = "1";
            public const string DayOff = "2";
            public const string Hospital = "3";
            public const string Holiday = "4";
        }
        /// <summary>
        /// Информация о клиенте
        /// </summary>
        private string ClientInfo { get; set; }

        public RestClient()
        {
            ClientInfo = $"{DeviceInfo.Manufacturer} {DeviceInfo.Model} {DeviceInfo.Platform} {DeviceInfo.VersionString}";
        }
        /// <summary>
        /// Функция создания <see cref="HttpClient"/> с параметрами по умолчанию
        /// </summary>
        /// <returns><see cref="HttpClient"/> со стандартными параметрами</returns>
        private HttpClient CreateDefaultClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", ClientInfo);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        /// <summary>
        /// Производит отправку сообщения с помощью стандартного <see cref="HttpClient"/>
        /// <para>Смотри <see cref="CreateDefaultClient"/></para>
        /// </summary>
        /// <exception cref="HttpRequestException">Ошибка отправки сообщения</exception>
        /// <param name="Controller">Адресс контроллера</param>
        /// <param name="Method">Метод сообщения</param>
        /// <param name="Content">Контент, может быть <see cref="null"/></param>
        /// <returns><see cref="HttpResponseMessage"/> как результат запроса</returns>
        private async Task<HttpResponseMessage> SendMessage(string Controller, HttpMethod Method, string Content = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = Method,
                RequestUri = new Uri(Server + Controller),
                Content = new StringContent(Content ?? "")
            })
            using (var client = CreateDefaultClient())
            {
                response = await client.SendAsync(request);
            }

            return await Task.FromResult(response);
        }
        /// <summary>
        /// Возвращает хэш входной строки
        /// </summary>
        /// <param name="Input">Входная строка</param>
        /// <returns>Хэш-строку</returns>
        private string GetHashString(string Input)
        {
            var a = SHA1.Create();
            var h = a.ComputeHash(Encoding.UTF8.GetBytes(Input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < h.Length; i++)
            {
                builder.Append(h[i].ToString("x2"));
            }
            return builder.ToString();
        }
        #region Impementation of IClientModel
        public string Session { get; set; }
        public string Server { get; set; }

        public async Task AcceptTask(string TaskId)
        {
            var reqData = new Request.TaskNewStage() { Session = Session, TaskId = TaskId, Stage = TaskStageData.Processing };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Task, HttpMethod.Put, reqContent);

            switch(res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }
        }

        public async Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            var reqData = new Request.AuthoLogin() { Login = Login, Password = GetHashString(Password) };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Autho, HttpMethod.Post, reqContent);
            
            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    return AuthorizationResult.Error;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }
            var resContent = JsonConvert.DeserializeObject<Response.AuthoResult>(await res.Content.ReadAsStringAsync());
            Session = resContent.Session;
            return AuthorizationResult.Ok;
        }

        public async Task<AuthorizationResult> Authorization()
        {
            var reqData = new Request.AuthoSession() { Session = Session };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Autho, HttpMethod.Put, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return AuthorizationResult.Ok;
                case System.Net.HttpStatusCode.Unauthorized:
                    return AuthorizationResult.Error;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }
        }

        public async Task CompleteTask(string TaskId)
        {
            var reqData = new Request.TaskNewStage() { Session = Session, TaskId = TaskId, Stage = TaskStageData.Completed };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Task, HttpMethod.Put, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }
        }

        public async Task<StatusCode> GetLastStatusCode()
        {
            var reqData = new Request.StatusUserRequest() { Session = Session };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Status, HttpMethod.Post, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<Response.Status>(await res.Content.ReadAsStringAsync());

            return new StatusCode()
            {
                Code = resContent.StatusCode ?? "",
                LastUpdate = resContent.Updated ?? ""
            };
        }

        public async Task<List<Plan>> GetPlans(DateTime Start, DateTime End, PlanTypes[] Filter)
        {
            var filter = new List<string>();
            foreach (var filt in Filter)
            {
                if (filt == PlanTypes.DayOff)
                {
                    filter.Add(Plans.DayOff);
                }
                else if (filt == PlanTypes.Holiday)
                {
                    filter.Add(Plans.Holiday);
                }
                else if (filt == PlanTypes.Hospital)
                {
                    filter.Add(Plans.Hospital);
                }
                else if (filt == PlanTypes.Working)
                {
                    filter.Add(Plans.Working);
                }
            }

            var reqData = new Request.PlansRequest() { Session = Session, StartDate = Start.ToString("dd.MM.yyyy"), EndDate = End.ToString("dd.MM.yyyy"), PlanCodes = filter.ToArray() };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Plan, HttpMethod.Post, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<List<Response.Plan>>(await res.Content.ReadAsStringAsync());
            var result = new List<Plan>();

            foreach (var plan in resContent)
            {
                result.Add(new Plan()
                {
                    DateSet = plan.Date ?? "",
                    TypePlan = plan.PlanCode ?? "",
                    StartDay = plan.DayStart ?? "",
                    EndDay = plan.DayEnd ?? ""
                });
            }

            return result;
        }

        public async Task<List<PlanType>> GetPlanTypes()
        {
            var res = await SendMessage(Controller.Plan, HttpMethod.Get);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<List<Response.PlanType>>(await res.Content.ReadAsStringAsync());
            var result = new List<PlanType>();

            foreach (var type in resContent)
            {
                result.Add(new PlanType()
                {
                    Code = type.PlanCode ?? "",
                    Title = type.Title?? ""
                });
            }

            return result;
        }

        public async Task<List<Status>> GetStatuses()
        {
            var res = await SendMessage(Controller.Status, HttpMethod.Get);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<List<Response.StatusType>>(await res.Content.ReadAsStringAsync());
            var result = new List<Status>();

            foreach (var type in resContent)
            {
                result.Add(new Status()
                {
                    Code = type.StatusCode ?? "",
                    Title = type.Title ?? ""
                });
            }

            return result;
        }

        public async Task<List<Tasks.Task>> GetTasks(DateTime Created, TaskStages[] Filter)
        {
            var filter = new List<string>();

            foreach(var filt in Filter)
            {
                if (filt == TaskStages.NotAccepted)
                {
                    filter.Add(TaskStageData.NotAccepted);
                }
                else if (filt == TaskStages.Processing)
                {
                    filter.Add(TaskStageData.Processing);
                }
                else if (filt == TaskStages.Completed)
                {
                    filter.Add(TaskStageData.Completed);
                }
            }

            var reqData = new Request.TaskRequest() 
            { 
                Session = Session,
                DateCreation = Created == DateTime.MinValue ? "" : Created.ToString("dd.MM.yyyy"),
                TaskStages = filter
            };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Task, HttpMethod.Post, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<List<Response.Task>>(await res.Content.ReadAsStringAsync());
            var result = new List<Tasks.Task>();

            foreach (var type in resContent)
            {
                result.Add(new Tasks.Task()
                {
                    Id = type.TaskId ?? "",
                    Boss = type.SetterWorkerName ?? "",
                    DateSetted = type.Created ?? "",
                    DateFinished = type.Finished ?? "",
                    Description = type.Description ?? "",
                    Stage = type.Stage ?? ""
                });
            }

            return result;
        }

        public async Task<List<TaskStage>> GetTaskStages()
        {
            var res = await SendMessage(Controller.Task, HttpMethod.Get);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<List<Response.TaskStage>>(await res.Content.ReadAsStringAsync());
            var result = new List<TaskStage>();

            foreach (var type in resContent)
            {
                result.Add(new TaskStage()
                {
                    Code = type.Stage ?? "",
                    Title = type.Title ?? ""
                });
            }

            return result;
        }

        public async Task<Plan> GetTodayPlan()
        {
            var res = Plan.Empty();
            var resl = await GetPlans(DateTime.Now, DateTime.Now, new PlanTypes[0]);
            if (resl.Count > 0)
                res = resl[0];
            return res;
        }

        public async Task<Worker> GetWorkerInfo()
        {
            var reqData = new Request.WorkerInfoRequest() { Session = Session };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Worker, HttpMethod.Post, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }

            var resContent = JsonConvert.DeserializeObject<Response.WorkerInfo>(await res.Content.ReadAsStringAsync());

            return new Worker()
            {
                Name = resContent.Name ?? "",
                Surname = resContent.Surname ?? "",
                Patronymic = resContent.Patronymic ?? "",
                Position = resContent.Position ?? "",
                Department = resContent.Department ?? ""
            };
        }
        public async Task SetStatus(string Code)
        {
            var reqData = new Request.NewStatusCode() { Session = Session, StatusCode = Code };
            string reqContent = JsonConvert.SerializeObject(reqData);

            var res = await SendMessage(Controller.Status, HttpMethod.Put, reqContent);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    break;
                default:
                    throw new Exception(JObject.Parse(await res.Content.ReadAsStringAsync())["Message"].ToString());
            }
        }
        #endregion
    }
}