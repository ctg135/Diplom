using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Client.DataModels;
using Client.Models;
using Client.Views;
using System.Globalization;

namespace Client.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Клиент для работы с сервером
        /// </summary>
        private IClientModel Client { get; set; }
        /// <summary>
        /// Команда установки нового статуса
        /// </summary>
        public ICommand SetNewStatus { get; private set; }
        /// <summary>
        /// Команда обновления информации на страницы
        /// </summary>
        public ICommand UpdateData { get; private set; }
        /// <summary>
        /// Событие для просмотра задач в подробностях
        /// </summary>

        public event EventHandler NavigateToTaskDetail;
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
        /// График на сегодня
        /// </summary>
        public Plan1 PlanToday { get; set; }
        /// <summary>
        /// Список задач
        /// </summary>
        public Tasks Tasks { get; set; }
        /// <summary>
        /// Может ли статус быть обновлён
        /// </summary>
        public bool IsStatusUpdateable { get; set; }
        public MainPageViewModel(IClientModel Client)
        {
            SetNewStatus = new Command(SetStatus);
            UpdateData = new Command(UpdateProps);

            this.Client = Client;
            this.Client.Session = Globals.Config.GetItem("Session").Result;
            this.Client.Server = Globals.Config.GetItem("Server").Result;
            Tasks = new Tasks();
            PlanToday = Plan1.Empty();
            Globals.WorkerStatus = StatusCode.Empty();
        }
        /// <summary>
        /// Команда установки статуса
        /// </summary>
        private async void SetStatus()
        {
            string[] buts = new string[3]
            {
                Globals.StatusCodes["2"].Title, // На работе
                Globals.StatusCodes["3"].Title, // На перерыве
                Globals.StatusCodes["5"].Title  // Рабочий день закончен
            };
            string butCancel = "Отмена";
            
            var res = await Application.Current.MainPage.DisplayActionSheet("Выбор статуса", butCancel, null, buts);

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
                await Application.Current.MainPage.DisplayAlert("Ошибка установки статуса", exc.Message, "Ок");
                return;
            }

            NotifyPropertyChanged(nameof(Status));
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
                string type = PlanToday.TypePlan;
                PlanToday.TypePlan = Globals.PlanTypes[type].ToLower();
                if (type == "1")
                {
                    PlanToday.TypePlan += $" с {PlanToday.StartDay} до {PlanToday.EndDay}";
                }

                foreach (var task in Tasks.Items)
                {
                    task.OpeningDetails -= Task_OpeningDetails;
                }
                Tasks = new Tasks( await Client.GetTasks(DateTime.MinValue, new TaskStages[] { TaskStages.NotAccepted, TaskStages.Processing }));
                foreach (var task in Tasks.Items)
                {
                    task.OpeningDetails += Task_OpeningDetails;
                    task.Stage = Globals.TaskStages[task.Stage];
                }
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }

            if (Globals.WorkerStatus.Code == "1" || Globals.WorkerStatus.Code == "2" || Globals.WorkerStatus.Code == "3")
            {
                IsStatusUpdateable = true;
            }
            else
            {
                IsStatusUpdateable = false;
            }

            NotifyPropertyChanged(nameof(IsStatusUpdateable));
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(PlanToday));
            NotifyPropertyChanged(nameof(Tasks));
        }

        private void Task_OpeningDetails(object sender, EventArgs e)
        {
            NavigateToTaskDetail(sender, new EventArgs());
        }
    }
}
