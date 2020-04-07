using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.DataModels;
using Client.Models;

namespace Client.Models
{
    class PlansMock : IPlansModel
    {
        private Plan STDPLAN
        {
            get
            {
                return new Plan()
                {
                    Id = "1",
                    StartOfDay = "9:00",
                    EndOfDay = "18:00",
                    Total = "9"
                };
            }
        }
        public async Task<List<Plan>> GetPlans(DateTime Start, DateTime End)
        {
            int diff = (Start - End).Days;
            if (diff < 0) return new List<Plan>();
            List<Plan> plans = new List<Plan>();
            for(int i = 0; i < diff; i++)
            {
                var temp = STDPLAN;
                temp.Date = Start.AddDays(i).ToString("d");
                temp.Id = (1 + i).ToString();
                plans.Add(temp);
            }
            return plans;
        }
        public async Task<Plan> GetTodayPlan()
        {
            var plan = STDPLAN;
            plan.Date = DateTime.Now.ToString("d");
            return plan;
        }
    }
}
