using Client.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XamForms.Controls;

namespace Client.ViewModels
{
    /// <summary>
    /// Модель окна просмотра списка планов
    /// </summary>
    class VeiwPlansPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Класс для хранения планов
        /// </summary>
        public class MyPlan
        {
            /// <summary>
            /// Время установки
            /// </summary>
            public string DateSet { get; set; }
            /// <summary>
            /// Тип плана
            /// </summary>
            public string TypePlan { get; set; }
            /// <summary>
            /// Отрезок времени для для рабочего дня в виде <c>"с {StartDate} по {EndDate}"</c>
            /// </summary>
            public string Range { get; set; }
        }
        #region ViewData
        /// <summary>
        /// Дата сегодня
        /// </summary>
        public DateTime TodayDate { get; private set; }
        /// <summary>
        /// Список дат для отображения
        /// </summary>
        public ObservableCollection<SpecialDate> Dates { get; private set; }
        /// <summary>
        /// Комманда для получения информации о дне
        /// </summary>
        public ICommand DayInfo { get; private set; }
        #endregion
        private Dictionary<DateTime, MyPlan> dates { get; set; }

        /// <summary>
        /// Создание модели
        /// </summary>
        /// <param name="PlanList">Список планов для отображения</param>
        public VeiwPlansPageViewModel(List<Plan> PlanList)
        {
            DayInfo = new Command(ShowDayInfo);
            TodayDate = DateTime.Now;

            var specials = new List<SpecialDate>();
            dates = new Dictionary<DateTime, MyPlan>();

            foreach (var plan in PlanList)
            {
                MyPlan newp = new MyPlan() { DateSet = plan.DateSet, TypePlan = plan.TypePlan };

                if (plan.TypePlan == Globals.PlanTypes["1"]) // Если рабочий
                {
                    newp.Range = $"С {plan.StartDay} по {plan.EndDay}";
                    specials.Add(SpecialDateFabric.GetWorking(DateTime.Parse(plan.DateSet)));
                } 
                else if (plan.TypePlan == Globals.PlanTypes["2"]) // Если выходной
                {
                    specials.Add(SpecialDateFabric.GetDayOff(DateTime.Parse(plan.DateSet)));
                }
                else if (plan.TypePlan == Globals.PlanTypes["3"]) // Если больничный 
                {
                    specials.Add(SpecialDateFabric.GetHospital(DateTime.Parse(plan.DateSet)));
                }
                else if (plan.TypePlan == Globals.PlanTypes["4"]) // Если отпуск
                {
                    specials.Add(SpecialDateFabric.GetHoliday(DateTime.Parse(plan.DateSet)));
                }

                dates.Add(DateTime.Parse(plan.DateSet), newp);
            }

            Dates = new ObservableCollection<SpecialDate>(specials);
        }
        private async void ShowDayInfo(object param)
        {
            var plan = new MyPlan();
            try
            {
                plan = dates[(DateTime)param];
            }
            catch (Exception)
            {
                plan = new MyPlan() { TypePlan = "График не задан", Range = "" };
            }
            finally
            {
                string message = plan.TypePlan;
                if (!string.IsNullOrEmpty(plan.Range))
                {
                    message += $"\n{plan.Range}";
                }
                await Application.Current.MainPage.DisplayAlert(((DateTime)param).ToString("dd MMMM, dddd"), message, "Ок");
            }
        }
        private class SpecialDateFabric
        {
            public static SpecialDate GetWorking(DateTime Date)
            {
                return new SpecialDate(Date)
                {
                    BackgroundColor = Color.FromHex("95e089"),
                    TextColor = Color.Black,
                    Selectable = true
                };
            }
            public static SpecialDate GetHospital(DateTime Date)
            {
                return new SpecialDate(Date)
                {
                    BackgroundColor = Color.FromHex("b647df"),
                    TextColor = Color.White,
                    Selectable = true
                };
            }
            public static SpecialDate GetHoliday(DateTime Date)
            {
                return new SpecialDate(Date)
                {
                    BackgroundColor = Color.FromHex("f66b30"),
                    TextColor = Color.Black,
                    Selectable = true
                };
            }
            public static SpecialDate GetDayOff(DateTime Date)
            {
                return new SpecialDate(Date)
                {
                    BackgroundColor = Color.FromHex("66f9ff"),
                    TextColor = Color.Black,
                    Selectable = true
                };
            }
        }
    }
}
