using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Client.DataModels;

namespace Client.Droid.Models
{
    class MockPlanLoader : Client.Models.IPlanLoader
    {
        public Task ClearPlans()
        {
            throw new NotImplementedException();
        }

        public Task<List<Plan1>> GetPlans()
        {
            List<Plan1> plans = new List<Plan1>()
            {
                new Plan1()
                {
                    DateSet = "08.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:00",
                    TypePlan = "Будний"
                },
                new Plan1()
                {
                    DateSet = "09.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:00",
                    TypePlan = "Будний"
                },
                new Plan1()
                {
                    DateSet = "10.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:00",
                    TypePlan = "Будний"
                },
                new Plan1()
                {
                    DateSet = "11.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:00",
                    TypePlan = "Будний"
                },
                new Plan1()
                {
                    DateSet = "12.05.2020",
                    TypePlan = "Выхоной"
                }
            };

            return Task.FromResult(plans);
        }

        public Task SetPlans(List<Plan1> Plans)
        {
            throw new NotImplementedException();
        }
    }
}