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
    /// <summary>
    /// Модель для просмотра графиков
    /// </summary>
    partial class GraphicPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Клиент для работы с сервером
        /// </summary>
        public IClientModel Client { get; private set; }
        /// <summary>
        /// Загрузчик сохраненных планов
        /// </summary>
        public IPlanLoader PlanLoader { get; private set; }
        #region xamlData
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
        /// Команда для сохранения планов
        /// </summary>
        public ICommand SavePlans { get; private set; }
        /// <summary>
        /// Команда просмотра сохраненных планов
        /// </summary>
        public ICommand OpenSavedPlans { get; private set; }
        /// <summary>
        /// Значение выбранного рабочего дня в фильтрах
        /// </summary>
        public bool WorkDaysSelected { get; set; }
        /// <summary>
        /// Значение выбранного выходного дня в фильтрах
        /// </summary>
        public bool DaysOffSelected { get; set; }
        /// <summary>
        /// Значение выбранного отпускного дня в фильтрах
        /// </summary>
        public bool HolidayDaysSelected { get; set; }
        /// <summary>
        /// Значение выбранного больничного в фильтрах
        /// </summary>
        public bool HospitalDaysSelected { get; set; }
        #endregion
        /// <summary>
        /// Событие просмотра графиков
        /// </summary>
        public event ViewPlansEvent ViewPlans;
        /// <summary>
        /// Создание модели
        /// </summary>
        /// <param name="Client"></param>
        public GraphicPageViewModel(IClientModel Client, IPlanLoader PlanLoader)
        {
            ShowPlans = new Command(LoadPlans);
            SavePlans = new Command(SaveSelectedPlans);
            OpenSavedPlans = new Command(OpenSaved);

            this.Client = Client;
            this.Client.Session = Globals.Config.GetItem("Session").Result;
            this.Client.Server = Globals.Config.GetItem("Server").Result;

            this.PlanLoader = PlanLoader;

            StartDate = EndDate = DateTime.Parse( DateTime.Now.ToString("d") );
        }
        /// <summary>
        /// Загрузка планов и их просмотр
        /// </summary>
        private async void LoadPlans()
        {
            var Plans = new List<Plan1>();
            if (StartDate > EndDate)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка ввода", "Начальная дата не может быть больше конечной", "Ок");
                return;
            }

            List<PlanTypes> filter = new List<PlanTypes>();
            if (WorkDaysSelected) filter.Add(PlanTypes.Working);
            if (DaysOffSelected) filter.Add(PlanTypes.DayOff);
            if (HolidayDaysSelected) filter.Add(PlanTypes.Holiday);
            if (HospitalDaysSelected) filter.Add(PlanTypes.Hospital);

            try
            {
                Plans = await Client.GetPlans(StartDate, EndDate, filter.ToArray());
                foreach (var plan in Plans)
                {
                    plan.TypePlan = Globals.PlanTypes[plan.TypePlan];
                }
                System.Diagnostics.Debug.WriteLine("Выводим графики");
                ViewPlans(this, new ViewPlansEventArgs(Plans));
            }
            catch(Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }
        }
        private async void SaveSelectedPlans(object param)
        {
            string msg = "Сохраненныё графики будут очищены", title = "Вы уверены?", butOk = "Да", butCancel = "Нет";
            var res = await Application.Current.MainPage.DisplayAlert(title, msg, butOk, butCancel);
            if (!res) return;

            var Plans = new List<Plan1>();
            if (StartDate > EndDate)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка ввода", "Начальная дата не может быть больше конечной", "Ок");
                return;
            }

            List<PlanTypes> filter = new List<PlanTypes>();
            if (WorkDaysSelected) filter.Add(PlanTypes.Working);
            if (DaysOffSelected) filter.Add(PlanTypes.DayOff);
            if (HolidayDaysSelected) filter.Add(PlanTypes.Holiday);
            if (HospitalDaysSelected) filter.Add(PlanTypes.Hospital);

            try
            {
                Plans = await Client.GetPlans(StartDate, EndDate, filter.ToArray());
                foreach (var plan in Plans)
                {
                    plan.TypePlan = Globals.PlanTypes[plan.TypePlan];
                }
                await PlanLoader.SetPlans(Plans);
                await Application.Current.MainPage.DisplayAlert("Все готово!", "Планы сохранены в памяти", "Ок");
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }
        }
        private async void OpenSaved(object param)
        {
            try
            {
                var Plans = await PlanLoader.GetPlans();
                ViewPlans(this, new ViewPlansEventArgs(Plans));
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }
        }
    }
}
