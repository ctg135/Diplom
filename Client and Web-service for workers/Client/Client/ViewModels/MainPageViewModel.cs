using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Client.DataModels;
using Client.Models;
using Client.Views;

namespace Client.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        private IClientModel Client { get; set; }
        public ICommand ExitAcc { get; private set; }
        public ICommand SetNewStatus { get; private set; }
        public ICommand UpdateData { get; private set; }
        /// <summary>
        /// Статус работника
        /// </summary>
        public string Status
        {
            get
            {
                if (Globals.WorkerStatus.Code == StatusCode.Empty().Code) return "-";
                return Globals.StatusCodes[Globals.WorkerStatus.Code].Title;
            }
        }
        /// <summary>
        /// Обновление даты статуса
        /// </summary>
        public string LastUpdate
        {
            get
            {
                return Globals.WorkerStatus.LastUpdate;
            }
        }
        /// <summary>
        /// График на сегодня
        /// </summary>
        public Plan PlanToday { get; set; }
        public MainPageViewModel(IClientModel Client)
        {
            ExitAcc = new Command(UnAutho);
            SetNewStatus = new Command(SetStatus);
            UpdateData = new Command(UpdateProps);

            this.Client = Client;
            this.Client.Session = Globals.Config.GetItem("Session").Result;
            this.Client.Server = Globals.Config.GetItem("Server").Result;

            PlanToday = Plan.Empty();
            Globals.WorkerStatus = StatusCode.Empty();
        }
        /// <summary>
        /// Комманда выхода
        /// </summary>
        private async void UnAutho()
        {
            Globals.Clear();
            Application.Current.MainPage = new NavigationPage(new AuthoPage());
        }
        /// <summary>
        /// Команда установки статуса
        /// </summary>
        private async void SetStatus()
        {
            List<string> buts = new List<string>(Globals.Statuses.Keys);
            string butCancel = "Отмена";
            
            var res = await Application.Current.MainPage.DisplayActionSheet("Выбор статуса", butCancel, null, buts.ToArray());

            if (res == butCancel || res == null)
            {
                return;
            }

            try
            {
                await Client.SetStatus(Globals.Statuses[res].Code);
                Globals.WorkerStatus = await Client.GetLastStatusCode();
            }
            catch (Exception exc)
            {
                if(await Client.IsSetStatusClientError(exc.Message))
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка установки статуса", exc.Message, "Ок");
                }
                else
                {
                    await FatalError(exc.Message);
                }
                return;
            }
            
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(LastUpdate));
        }
        /// <summary>
        /// Команда обноления планов и статуса на форме
        /// </summary>
        public async void UpdateProps()
        {
            try
            {
                Globals.WorkerStatus = await Client.GetLastStatusCode();
                PlanToday = await Client.GetTodayPlan();
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }

            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(LastUpdate));
            NotifyPropertyChanged(nameof(PlanToday));
        }
    }
}
