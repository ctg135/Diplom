using System;
using System.Collections.Generic;
using System.Text;
using Client.DataModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Globalization;
using System.Windows.Input;
using Client.Models;
using Client.Views;

namespace Client.ViewModels
{
    class GraphicPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Клиент для работы с сервером
        /// </summary>
        public IClientModel Client { get; private set; }
        /// <summary>
        /// Коллекция планов
        /// </summary>
        public List<Plan> Plans { get; private set; }
        /// <summary>
        /// Начальная дата для отбора
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Конечная дата для отбора
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Команда для загрузки планов
        /// </summary>
        public ICommand ShowPlans { get; private set; }
        /// <summary>
        /// Команда выхода из аккаунта
        /// </summary>
        public ICommand Exit { get; private set; }

        public GraphicPageViewModel()
        {
            ShowPlans = new Command(LoadPlans);
            Exit = new Command(UnAutho);
            Client = new ClientMock();
            Plans = new List<Plan>();
            StartDate = EndDate = DateTime.Parse( DateTime.Now.ToString("d") );
        }
        private async void LoadPlans()
        {
            if (StartDate > EndDate)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка ввода", "Начальная дата не может быть больше конечной", "Ок");
                return;
            }
             
            try
            {
                Plans = await Client.GetPlans(StartDate, EndDate);
            }
            catch(Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }

            NotifyPropertyChanged(nameof(Plans));
        }

        private async void UnAutho()
        {
            Globals.Clear();
            Application.Current.MainPage = new NavigationPage(new AuthoPage());
        }
    }
}
