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
        public ICommand Authorize { get; private set; }
        /// <summary>
        /// Команда открытия настроек
        /// </summary>
        public ICommand OpenSettings { get; private set; }
        /// <summary>
        /// Команда открытия списка сохраненных планов
        /// </summary>
        public ICommand OpenSavedPlans { get; private set; }
        /// <summary>
        /// Команда обноления настроек
        /// </summary>
        public ICommand UpdateSettings { get; private set; }
        /// <summary>
        /// Клиент для получения данных
        /// </summary>
        public IClientModel Client { get; private set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Событие отобржения списка планов
        /// </summary>
        public event ViewPlansEvent ViewPlans;

        public AuthoPageViewModel(IClientModel Client)
        {
            this.Client = Client;
            Authorize = new Command(Autho);
            OpenSavedPlans = new Command(OpenSaved);
            OpenSettings = new Command(OpenSettingsPageAsync);
            UpdateSettings = new Command(UpdateSets);
        }
        /// <summary>
        /// Команда авторизации
        /// </summary>
        private async void Autho()
        {
            var result = new AuthorizationResult();
            try
            {
                result = await Client.Authorization(Login, Password);
            }
            catch(Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка авторизации", "Ошибка подключения", "Ок");
                return;
            }
            switch(result)
            {
                case AuthorizationResult.Ok:
                    await Globals.Load(Client.Session, Client.Server);
                    Application.Current.MainPage = new MainMenuPage();
                    break;
                case AuthorizationResult.Error:
                    await Application.Current.MainPage.DisplayAlert("Ошибка авторизации", "Неверное имя пользователя или пароль", "Ок");
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Ошибка авторизации", "Непридвиденная ошибка", "Ок");
                    break;
            }
        }
        private async void OpenSaved()
        {
            IPlanLoader planLoader = ServiceLocator.Current.GetInstance<IPlanLoader>();

            this.ViewPlans(this, new ViewPlansEventArgs(await planLoader.GetPlans()));
        }
        /// <summary>
        /// Команда открытия страницы настроек
        /// </summary>
        private async void OpenSettingsPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage(false));
        }
        /// <summary>
        /// Команда для обновления настроек
        /// </summary>
        private async void UpdateSets()
        {
            Client.Server  = await Globals.Config.GetItem("Server");
            Client.Session = await Globals.Config.GetItem("Session");
        }
    }
}
