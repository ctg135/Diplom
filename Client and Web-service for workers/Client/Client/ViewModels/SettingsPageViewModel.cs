using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Client.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Client.Views;

namespace Client.ViewModels
{
    /// <summary>
    /// Модель окна настроек
    /// </summary>
    class SettingsPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Свойство отображаемости кнопки выхода из аккаунта
        /// </summary>
        public bool ButtonLogOutVisible { get; set; }
        /// <summary>
        /// Комманда для выхода из аккаунта
        /// </summary>
        public ICommand LogOut { get; private set; }
        /// <summary>
        /// Просмотреть сохраненные планы
        /// </summary>
        public ICommand ViewSavedPlans { get; private set; }
        /// <summary>
        /// Очистить сохраненные планы
        /// </summary>
        public ICommand ClearSavedPlans { get; private set; }
        /// <summary>
        /// Событие для просмотра планов
        /// </summary>
        public event ViewPlansEvent ViewSaved;
        /// <summary>
        /// Строка с сохраненным сервером
        /// </summary>
        public string Server
        {
            get
            {
                return Globals.Config.GetItem("Server").Result;
            }
            set
            {
                Globals.Config.SetItem("Server", value);
            }
        }
        /// <summary>
        /// Созданиеи модели окна настроек
        /// </summary>
        /// <param name="ButtonLogOutVisible">Отображение кнопки авторизации</param>
        public SettingsPageViewModel(bool ButtonLogOutVisible = true)
        {
            this.ButtonLogOutVisible = ButtonLogOutVisible;
            LogOut = new Command(LogOuted);
            ViewSavedPlans = new Command(ViewPlans);
            ClearSavedPlans = new Command(ClearPlans);
        }
        /// <summary>
        /// Функция при выходе из аккаунта
        /// </summary>
        /// <param name="param"></param>
        private void LogOuted(object param)
        {
            Globals.Clear();
            Application.Current.MainPage = new NavigationPage(new AuthoPage());
        }
        /// <summary>
        /// Функция просмотра сохраненных планов
        /// </summary>
        /// <param name="param"></param>
        private async void ViewPlans(object param)
        {
            IPlanLoader Plans = CommonServiceLocator.ServiceLocator.Current.GetInstance<IPlanLoader>();
            var plans = await Plans.GetPlans();
            //await Application.Current.MainPage.Navigation.PushAsync(new ViewPlansPage(plans));
            ViewSaved(this, new ViewPlansEventArgs(plans));
        }
        /// <summary>
        /// Функция очистки сохраненных планов
        /// </summary>
        /// <param name="param"></param>
        private async void ClearPlans(object param)
        {
            string title = "Потдтверждение";
            string message = "Вы уверены, что хотите очистить сохраненные дни графика?";
            string butAccept = "Да";
            string butCancel = "Нет";

            var res = await Application.Current.MainPage.DisplayAlert(title, message, butAccept, butCancel);

            if (res == true)
            {
                IPlanLoader Plans = CommonServiceLocator.ServiceLocator.Current.GetInstance<IPlanLoader>();
                await Plans.ClearPlans();
            }
        }
    }
}
