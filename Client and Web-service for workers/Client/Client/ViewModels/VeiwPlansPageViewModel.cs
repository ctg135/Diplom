using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Client.DataModels;
using Client.Views;
using Xamarin.Forms;

namespace Client.ViewModels
{
    class VeiwPlansPageViewModel : BaseViewModel
    {
        public class MyPlan
        {
            public string DateSet { get; set; }
            public string TypePlan { get; set; }
            public string Range { get; set; }
        }
        public List<MyPlan> Plans { get; set; }
        public VeiwPlansPageViewModel(List<Plan1> PlanList)
        {
            Plans = new List<MyPlan>();

            foreach (var plan in PlanList)
            {
                if (string.IsNullOrEmpty(plan.StartDay))
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
