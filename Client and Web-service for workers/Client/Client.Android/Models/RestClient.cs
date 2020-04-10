using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using ClientData = Client.DataModels;
using Client.Models;
using ServerData = Client.Droid.Models.ServerModels;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Client.Droid.Models
{
    public class RestClient : IClientModel
    {

        #region Controllers consts
        /// <summary>
        /// Постоянная для контроллера авторизации
        /// </summary>
        private const string ControllerAuthorization = "Autho";

        /// <summary>
        /// Постоянная для контроллера проверки подключения
        /// </summary>
        private const string ControllerConnection    = "Connection";

        /// <summary>
        /// Постоянная для контроллера планов
        /// </summary>
        private const string ControllerPlan          = "Plan";

        /// <summary>
        /// Постоянная для контроллера статусов
        /// </summary>
        private const string ControllerStatus        = "Status";

        /// <summary>
        /// Постоянная для контроллера работников
        /// </summary>
        private const string ControllerWorker        = "Worker";

        #endregion

        /// <summary>
        /// Строка с информацией о клиенте
        /// </summary>
        private string ClientInfo { get; set; }

        public RestClient()
        {
            ClientInfo = $"{DeviceInfo.Manufacturer} {DeviceInfo.Model} {DeviceInfo.Platform} {DeviceInfo.VersionString}";
        }

        /// <summary>
        /// Метод создания настроенного <c>HttpClient</c>
        /// </summary>
        /// <returns>HttpClient с настроенными заголовками</returns>
        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", ClientInfo);
            return client;
        }

        /// <summary>
        /// Производит асинхронную отправку сообщения
        /// </summary>
        /// <param name="Content">Содержимое сообщения</param>
        /// <param name="Method">Метод сообщения</param>
        /// <param name="Controller">Контроллер сообщения</param>
        /// <returns>Сообщение ответ от сервера</returns>
        private async Task<HttpResponseMessage> SendMessageAsync(string Content, HttpMethod Method, string Controller)
        {
            var request = new HttpRequestMessage(Method, Server + Controller);

            request.Headers.Add("Accept", "application/json");

            if (!string.IsNullOrEmpty(Content))
            {
                request.Content = new StringContent(Content);
            }

            return await GetHttpClient().SendAsync(request);
        }
        
        /// <summary>
        /// Возвращает хэш входной строки
        /// </summary>
        /// <param name="Input">Входная строка</param>
        /// <returns>Хэш</returns>
        string GetHashString(string Input)
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


        #region Implementation of IClientModel
        /// <summary>
        /// Сессия клиента
        /// </summary>
        public string Session { get; set; }

        /// <summary>
        /// Сервер клиента
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Авторизация клиента
        /// </summary>
        /// <param name="Login">Логин клиента</param>
        /// <param name="Password">Пароль клиента</param>
        /// <returns>Результат авторизации</returns>
        public async Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            // Пока тестим
            //string json = JsonConvert.SerializeObject(new ServerData.Autho() { Login = Login, Password = GetHashString(Password)});
            string json = JsonConvert.SerializeObject(new ServerData.Autho() { Login = Login, Password = Password});

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerAuthorization);
            switch(res.StatusCode)
            {
                case HttpStatusCode.OK:
                    var resJson = JObject.Parse(await res.Content.ReadAsStringAsync());
                    Session = (string)resJson["Session"];
                    return await Task.FromResult(AuthorizationResult.Ok);

                default:
                    return await Task.FromResult(AuthorizationResult.Error);
            }
        }
        /// <summary>
        /// Авторизация по сессии
        /// </summary>
        /// <returns>Результат авторизации</returns>
        public async Task<AuthorizationResult> Authorization()
        {
            // Допилить контроллер
            return await Task.FromResult(AuthorizationResult.Error);
        }
        /// <summary>
        /// Фукнция для проверки подключения к серверу
        /// </summary>
        /// <returns>Результат проверки</returns>
        public async Task<AuthorizationResult> CheckConnect()
        {
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerConnection);
            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await Task.FromResult(AuthorizationResult.Ok);
                default:
                    return await Task.FromResult(AuthorizationResult.Error);
            }
        }
        /// <summary>
        /// Функция получения последнего статуса работника
        /// </summary>
        /// <returns>Статус работника</returns>
        public async Task<ClientData.StatusCode> GetLastStatusCode()
        {
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerStatus); 
            var content = await res.Content.ReadAsStringAsync();

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception(res.StatusCode.ToString() + " " + (string)contentjson["Message"]);
            }

            return await Task.FromResult(JsonConvert.DeserializeObject<ClientData.StatusCode>(content));
        }
        /// <summary>
        /// Функция получения планов
        /// </summary>
        /// <param name="Start">Начальная дата</param>
        /// <param name="End">Конечная дата</param>
        /// <returns>Список планов</returns>
        public async Task<List<ClientData.Plan>> GetPlans(DateTime Start, DateTime End)
        {
            string query = JsonConvert.SerializeObject(new ServerData.DateQuery() { StartDate = Start.ToString("dd.MM.yyyy"), EndDate = End.ToString("dd.MM.yyyy") });
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session, Query = query });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerPlan);
            var content = await res.Content.ReadAsStringAsync();

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception(res.StatusCode.ToString() + " " + (string)contentjson["Message"]);
            }

            var listContent = JsonConvert.DeserializeObject<List<ServerData.Plan>>(content);

            var listClient = new List<ClientData.Plan>();

            foreach(var clientPlan in listContent)
            {
                listClient.Add(new ClientData.Plan()
                {
                    Id = clientPlan.Id,
                    Date = clientPlan.Date,
                    StartOfDay = clientPlan.StartOfDay,
                    EndOfDay = clientPlan.EndOfDay
                });
            }

            return await Task.FromResult(listClient);
        }
        /// <summary>
        /// Функция получения статусов
        /// </summary>
        /// <returns>Список статусов</returns>
        public async Task<List<ClientData.Status>> GetStatuses()
        {
            var res = await SendMessageAsync(null, HttpMethod.Get, ControllerStatus);
            var content = await res.Content.ReadAsStringAsync();

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception(res.StatusCode.ToString() + " " + (string)contentjson["Message"]);
            }

            return await Task.FromResult(JsonConvert.DeserializeObject<List<ClientData.Status>>(content));
        }
        /// <summary>
        /// Функция получения нового плана
        /// </summary>
        /// <returns>План на сегодня</returns>
        public async Task<ClientData.Plan> GetTodayPlan()
        {
            var plans = await GetPlans(DateTime.Now, DateTime.Now);
            if (plans == null) 
                return await Task.FromResult(new ClientData.Plan());
            return await Task.FromResult (plans[0]);
        }
        /// <summary>
        /// Функция получения информации о работнике
        /// </summary>
        /// <returns>Информация о работнике</returns>
        public async Task<ClientData.Worker> GetWorkerInfo()
        {
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerConnection);
            var content = await res.Content.ReadAsStringAsync();
            
            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:

                    break;
                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception(res.StatusCode.ToString() + " " + (string)contentjson["Message"]);
            }

            return await Task.FromResult(JsonConvert.DeserializeObject<ClientData.Worker>(content));
        }
        /// <summary>
        /// Функция установки статуса
        /// </summary>
        /// <param name="Code">Код статуса</param>
        /// <returns></returns>
        public async Task SetStatus(string Code)
        {
            string query = JsonConvert.SerializeObject(new ServerData.StatusCodeQuery() { StatusCode = Code });
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session, Query =  query});

            var res = await SendMessageAsync(json, HttpMethod.Put, ControllerStatus);

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:

                    break;
                default:
                    var content = await res.Content.ReadAsStringAsync();
                    var contentjson = JObject.Parse(content);
                    throw new Exception(res.StatusCode.ToString() + " " + (string)contentjson["Message"]);
            }
        }
        #endregion
    }
}