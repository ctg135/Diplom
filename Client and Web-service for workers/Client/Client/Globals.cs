using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private static async void OnTimer(object parameter)
        {
            var result = new AuthorizationResult();

            try
            {
                result = await TimerClient.CheckConnect();
            }
            catch(Exception exc)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Ошибка подключения", exc.Message, "Ок");
                Xamarin.Forms.Application.Current.MainPage = new Xamarin.Forms.NavigationPage(new Views.AuthoPage());
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
        public static void SetUpConnectionChecker(int Period, IClientModel Client)
        {
            TimerClient = Client;
            ConnectionCheckerTimer = new Timer(OnTimer, null, 60 * 1000, Period);
        }
        public static void DestroyConnectionChecker()
        {
            ConnectionCheckerTimer.Dispose();
            ConnectionCheckerTimer = null;
            TimerClient = null;
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
