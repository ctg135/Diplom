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
using Newtonsoft.Json;

namespace Client.Droid.Models
{
    class MockPlanLoader : Client.Models.IPlanLoader
    {
        public Task ClearPlans()
        {
            System.Diagnostics.Debug.WriteLine($"Очищаем сохраненки");

            throw new NotImplementedException();
        }

        public Task<List<Plan1>> GetPlans()
        {
            System.Diagnostics.Debug.WriteLine($"Получаем сохраненки");
            List<Plan1> plans = new List<Plan1>()
            {
                new Plan1()
                {
                    TypePlan = "ваорш",
                    DateSet = "08.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "ваорш",
                    DateSet = "09.05.2020y",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "ваорш",
                    DateSet = "10.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "ваорш",
                    DateSet = "11.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
            };

            return Task.FromResult(plans);
        }

        public Task SetPlans(List<Plan1> Plans)
        {
            System.Diagnostics.Debug.WriteLine($"Устанавливаем сохраненку {JsonConvert.SerializeObject(Plans)}");

            throw new NotImplementedException();
        }
    }
}