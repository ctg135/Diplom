using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Client.DataModels;
using Client.Views;
using Xamarin.Forms;

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
        /// <summary>
        /// Список плавно для отображения
        /// </summary>
        public List<MyPlan> Plans { get; set; }
        /// <summary>
        /// Создание модели
        /// </summary>
        /// <param name="PlanList">Список планов для отображения</param>
        public VeiwPlansPageViewModel(List<Plan1> PlanList)
        {
            Plans = new List<MyPlan>();

            foreach (var plan in PlanList)
            {
                if (string.IsNullOrEmpty(plan.StartDay) || string.IsNullOrEmpty(plan.EndDay))
                {
                    Plans.Add(new MyPlan() { DateSet = plan.DateSet, TypePlan = plan.TypePlan, Range = "" });
                }
                else
                {
                    Plans.Add(new MyPlan() { DateSet = plan.DateSet, TypePlan = plan.TypePlan, Range = $"С {plan.StartDay} по {plan.EndDay}" });
                }

            }
        }
    }
}
