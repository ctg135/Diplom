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
    class SettingsPageViewModel : BaseViewModel
    {
        public bool ButtonLogOutVisible { get; set; }
        public ICommand LogOut { get; private set; }
        public ICommand ViewSavedPlans { get; private set; }
        public ICommand ClearSavedPlans { get; private set; }
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
        public SettingsPageViewModel(bool ButtonLogOutVisible = true)
        {
            this.ButtonLogOutVisible = ButtonLogOutVisible;
            LogOut = new Command(LogOuted);
            ViewSavedPlans = new Command(ViewPlans);
            ClearSavedPlans = new Command(ClearPlans);
        }
        private void LogOuted(object param)
        {
            Globals.Clear();
            Application.Current.MainPage = new NavigationPage(new AuthoPage());
        }
        private async void ViewPlans(object param)
        {
            IPlanLoader Plans = CommonServiceLocator.ServiceLocator.Current.GetInstance<IPlanLoader>();
            var plans = await Plans.GetPlans();
            await Application.Current.MainPage.Navigation.PushAsync(new ViewPlansPage(plans));
        }
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
