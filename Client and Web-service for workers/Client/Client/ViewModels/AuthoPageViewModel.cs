using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using Client.Views;
using Client.Models;
using Client.DataModels;
using Plugin.Settings;

namespace Client.ViewModels
{
    class AuthoPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Команда авторизации
        /// </summary>
        public ICommand Authorize { get; set; }
        /// <summary>
        /// Команда открытия настроек
        /// </summary>
        public ICommand OpenSettings { get; set; }
        /// <summary>
        /// Клиент для получения данных
        /// </summary>
        public IClientModel Client { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login    { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        public AuthoPageViewModel(IClientModel Client)
        {
            this.Client = Client;
            Authorize = new Command(Autho);
            OpenSettings = new Command(OpenSettingsPageAsync);
        }
        /// <summary>
        /// Команда авторизации
        /// </summary>
        private async void Autho()
        {
            var result = await Client.Authorization.Authorization(Login, Password);
            switch(result)
            {
                case AuthorizationResult.Ok:
                    Globals.Config.SetItem("Session", Client.Authorization.Session);


                    Dictionary<string, Status> statuses = new Dictionary<string, Status>();
                    Dictionary<string, Status> statusCodes = new Dictionary<string, Status>();


                    foreach(var status in await Client.GetStatuses())
                    {
                        statuses.Add(status.Title, status);
                        statusCodes.Add(status.Code, status);
                    }

                    Globals.Statuses = statuses;
                    Globals.StatusCodes = statusCodes;

                    Application.Current.MainPage = new MainMenuPage();
                    break;
                case AuthorizationResult.Error:
                    await Application.Current.MainPage.DisplayAlert("Ошибка авторизации", "", "Ок");
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Ошибка авторизации", "Непридвиденная ошибка", "Ок");
                    break;
            }
        }
        /// <summary>
        /// Команда открытия страницы настроек
        /// </summary>
        private async void OpenSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }
    }
}
