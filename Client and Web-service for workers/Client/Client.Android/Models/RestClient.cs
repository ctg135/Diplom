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
            string json = JsonConvert.SerializeObject(new ServerData.Autho() { Login = Login, Password = GetHashString(Password)});

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
            return await CheckConnect();
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
                    throw new Exception((string)contentjson["Message"]);
            }

            ServerData.StatusCode status = JsonConvert.DeserializeObject<ServerData.StatusCode>(content);

            return await Task.FromResult(new ClientData.StatusCode()
            {
                Code = status.Code ?? "",
                LastUpdate = status.LastUpdate ?? ""
            }) ;
        }
        /// <summary>
        /// Функция получения планов
        /// </summary>
        /// <param name="Start">Начальная дата</param>
        /// <param name="End">Конечная дата</param>
        /// <returns>Список планов</returns>
        public async Task<List<ClientData.Plan>> GetPlans(DateTime Start, DateTime End)
        {
            var query = new ServerData.DateQuery() { StartDate = Start.ToString("dd.MM.yyyy"), EndDate = End.ToString("dd.MM.yyyy") };
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session, Query = query });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerPlan);
            var content = await res.Content.ReadAsStringAsync();

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;

                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception((string)contentjson["Message"]);
            }

            var listContent = JsonConvert.DeserializeObject<List<ServerData.Plan>>(content);

            var listClient = new List<ClientData.Plan>();

            foreach(var clientPlan in listContent)
            {
                listClient.Add(new ClientData.Plan()
                {
                    Id = clientPlan.Id ?? "",
                    Date = clientPlan.Date ?? "",
                    StartOfDay = clientPlan.StartOfDay ?? "",
                    EndOfDay = clientPlan.EndOfDay ?? "",
                    Total = clientPlan.Total ?? ""
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
                    throw new Exception((string)contentjson["Message"]);
            }

            List<ServerData.Status> serverStatuses = JsonConvert.DeserializeObject<List<ServerData.Status>>(content);
            var statuses = new List<ClientData.Status>();

            foreach(var status in serverStatuses)
            {
                statuses.Add(new ClientData.Status()
                {
                    Code = status.Code ?? "",
                    Description = status.Description ?? "",
                    Title = status.Title ?? ""
                });
            }

            return await Task.FromResult(statuses);
        }
        /// <summary>
        /// Функция получения нового плана
        /// </summary>
        /// <returns>План на сегодня</returns>
        public async Task<ClientData.Plan> GetTodayPlan()
        {
            var plans = await GetPlans(DateTime.Now, DateTime.Now);
            if (plans.Count == 0) 
                return await Task.FromResult(ClientData.Plan.Empty());
            return await Task.FromResult (plans[0]);
        }
        /// <summary>
        /// Функция получения информации о работнике
        /// </summary>
        /// <returns>Информация о работнике</returns>
        public async Task<ClientData.Worker> GetWorkerInfo()
        {
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session });

            var res = await SendMessageAsync(json, HttpMethod.Post, ControllerWorker);
            var content = await res.Content.ReadAsStringAsync();
            
            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:

                    break;
                default:
                    var contentjson = JObject.Parse(content);
                    throw new Exception((string)contentjson["Message"]);
            }

            var serverWorker = JsonConvert.DeserializeObject<ServerData.Worker>(content);

            return await Task.FromResult(new ClientData.Worker()
            {
                AccessLevel = serverWorker.AccessLevel ?? "",
                BirthDate   = serverWorker.BirthDate ?? "",
                Mail        = serverWorker.Mail ?? "",
                Name        = serverWorker.Name ?? "",
                Patronymic  = serverWorker.Patronymic ?? "",
                Position    = serverWorker.Position ?? "",
                Rate        = serverWorker.Rate ?? "",
                Surname     = serverWorker.Surname ?? ""
            });
        }
        /// <summary>
        /// Функция установки статуса
        /// </summary>
        /// <param name="Code">Код статуса</param>
        /// <returns></returns>
        public async Task SetStatus(string Code)
        {
            var query = new ServerData.StatusCodeQuery() { Code = Code };
            string json = JsonConvert.SerializeObject(new ServerData.Request() { Session = Session, Query =  query});

            var res = await SendMessageAsync(json, HttpMethod.Put, ControllerStatus);

            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:

                    break;
                default:
                    var content = await res.Content.ReadAsStringAsync();
                    var contentjson = JObject.Parse(content);
                    throw new Exception((string)contentjson["Message"]);
            }
        }
        #endregion
    }
}