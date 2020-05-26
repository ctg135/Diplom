using Client.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Droid.Models
{
    class MockPlanLoader : Client.Models.IPlanLoader
    {
        public Task ClearPlans()
        {
            System.Diagnostics.Debug.WriteLine($"Очищаем сохраненки");

            throw new NotImplementedException();
        }

        public Task<List<Plan>> GetPlans()
        {
            System.Diagnostics.Debug.WriteLine($"Получаем сохраненки");
            List<Plan> plans = new List<Plan>()
            {
                new Plan()
                {
                    TypePlan = "ваорш",
                    DateSet = "08.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan()
                {
                    TypePlan = "ваорш",
                    DateSet = "09.05.2020y",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan()
                {
                    TypePlan = "ваорш",
                    DateSet = "10.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan()
                {
                    TypePlan = "ваорш",
                    DateSet = "11.05.2020",
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
            };

            return Task.FromResult(plans);
        }

        public Task SetPlans(List<Plan> Plans)
        {
            System.Diagnostics.Debug.WriteLine($"Устанавливаем сохраненку {JsonConvert.SerializeObject(Plans)}");

            throw new NotImplementedException();
        }
    }
}