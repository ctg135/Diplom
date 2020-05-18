using Newtonsoft.Json;
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
                Content = new StringContent(Content)
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

        public Task AcceptTask(string TaskId)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationResult> Authorization()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationResult> CheckConnect()
        {
            throw new NotImplementedException();
        }

        public Task CompleteTask(string TaskId)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode> GetLastStatusCode()
        {
            throw new NotImplementedException();
        }

        public Task<List<Plan1>> GetPlans(DateTime Start, DateTime End, PlanTypes[] Filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlanType>> GetPlanTypes()
        {
            throw new NotImplementedException();
        }

        public Task<List<Status>> GetStatuses()
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks.Task>> GetTasks(DateTime Created, TaskStages[] Filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskStage>> GetTaskStages()
        {
            throw new NotImplementedException();
        }

        public Task<Plan1> GetTodayPlan()
        {
            throw new NotImplementedException();
        }

        public Task<Worker1> GetWorkerInfo()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSetStatusClientError(string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        public Task SetStatus(string Code)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}