using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using Client.Views;
using Client.Models;
using Client.DataModels;
using Plugin.Settings;
using CommonServiceLocator;

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
        /// Команда обноления настроек
        /// </summary>
        public ICommand UpdateSettings { get; set; }
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
            UpdateSettings = new Command(UpdateSets);
        }
        /// <summary>
        /// Команда авторизации
        /// </summary>
        private async void Autho()
        {
            var result = await Client.Authorization(Login, Password);
            switch(result)
            {
                case AuthorizationResult.Ok:
                    await Globals.Load(Client.Session, Client.Server);
                    Application.Current.MainPage = new MainMenuPage();
                    break;
                case AuthorizationResult.Error:
                    await Application.Current.MainPage.DisplayAlert("", "Ошибка авторизации", "Ок");
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
        /// <summary>
        /// Команда для обновления настроек
        /// </summary>
        private async void UpdateSets()
        {
            Client.Server = await Globals.Config.GetItem("Server");
        }
    }
}
