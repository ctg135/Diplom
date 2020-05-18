using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Models;
using Client.DataModels;

namespace Client
{
    public static class Globals
    {
        /// <summary>
        /// Экхепляр для работы с конфигурацией приложения
        /// </summary>
        public static IConfigManager Config { get; set; }
        /// <summary>
        /// Информация о работнике
        /// </summary>
        public static Worker WorkerInfo { get; set; }
        /// <summary>
        /// Активный статус работника
        /// </summary>
        public static StatusCode WorkerStatus { get; set; }
        /// <summary>
        /// {Название статуса - Статус}
        /// </summary>
        public static Dictionary<string, Status> Statuses { get; set; }
        /// <summary>
        /// {Код статуса - Статус}
        /// </summary>
        public static Dictionary<string, Status> StatusCodes { get; set; }
        /// <summary>
        /// Таймер для проверки подключения
        /// </summary>
        private static Timer ConnectionCheckerTimer { get; set; }
        /// <summary>
        /// Клиент для таймера
        /// </summary>
        private static IClientModel TimerClient { get; set; }
        /// <summary>
        /// Словарь типов планов <code>{Код - Полное название}</code>
        /// </summary>
        public static Dictionary<string, string> PlanTypes { get; set; }
        /// <summary>
        /// Словарь стадий задач <code>{Код - Стадия}</code>
        /// </summary>
        public static Dictionary<string, string> TaskStages { get; set; }
        /// <summary>
        /// Функция таймера для проверки подключения
        /// </summary>
        /// <param name="parameter"></param>
        private static async void OnTimer(object parameter)
        {
            var result = new AuthorizationResult();

            try
            {
                result = await TimerClient.Authorization();
            }
            catch(Exception exc)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Ошибка подключения", exc.Message, "Ок");
                Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.AuthoPage());
                return;
            }

            switch(result)
            {
                case AuthorizationResult.Ok:

                    break;
                case AuthorizationResult.Error:
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Ошибка подключения", "Нет подключения к серверу", "Ок");
                    Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.AuthoPage());
                    break;
            }
        }
        /// <summary>
        /// Установка процедуры проверки подключения
        /// </summary>
        /// <param name="Period">Период проверки</param>
        /// <param name="Client">Клиент для подключения</param>
        public static void SetUpConnectionChecker(int Period, IClientModel Client)
        {
            TimerClient = Client;
            ConnectionCheckerTimer = new Timer(OnTimer, null, 60 * 1000, Period);
        }
        /// <summary>
        /// Освобождает ресурсы тпймера
        /// </summary>
        public static void DestroyConnectionChecker()
        {
            //ConnectionCheckerTimer.Dispose();
            TimerClient = null;
        }
        /// <summary>
        /// Функция загрузки данных в <c>Globals</c>
        /// </summary>
        /// <param name="Session">Сессия для загрузки в <c>Config</c></param>
        /// <param name="Server">Сервер для загрузки в <c>Config</c></param>
        public static async Task Load(string Session, string Server)
        {
            var Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<IClientModel>();

            Client.Session = Session;
            Client.Server = Server;

            await Config.SetItem("Session", Session);
            await Config.SetItem("Server", Server);

            Dictionary<string, Status> statuses = new Dictionary<string, Status>();
            Dictionary<string, Status> statusCodes = new Dictionary<string, Status>();

            foreach (var status in await Client.GetStatuses())
            {
                statuses.Add(status.Title, status);
                statusCodes.Add(status.Code, status);
            }

            Statuses = statuses;
            StatusCodes = statusCodes;

            PlanTypes = new Dictionary<string, string>();

            foreach (var type in await Client.GetPlanTypes())
            {
                PlanTypes.Add(type.Code, type.Title);
            }

            WorkerInfo = await Client.GetWorkerInfo();

            var stages = await Client.GetTaskStages();
            TaskStages = new Dictionary<string, string>();
            foreach(var stage in stages)
            {
                TaskStages.Add(stage.Code, stage.Title);
            }

            //WorkerStatus = await Client.GetLastStatusCode();

            //SetUpConnectionChecker(2 * 60 * 1000, Client);
        }
        /// <summary>
        /// Очистка всех данных
        /// </summary>
        public static void Clear()
        {
            StatusCodes = null;
            Statuses = null;
            WorkerInfo = null;
            WorkerStatus = null;
            Config.SetItem("Session", "");
            DestroyConnectionChecker();
        }
    }
}
