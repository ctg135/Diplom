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
using Client.Models;

namespace Client.Droid.Models
{
    class AndroidMockClient : IClientModel
    {
        public AndroidMockClient()
        {

        }
        public async Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = new List<Status>();
            statuses.Add(new Status() { Title = "Не установлен", Code = "1", Description = "...." });
            statuses.Add(new Status() { Title = "На работе", Code = "2", Description = "...." });
            statuses.Add(new Status() { Title = "Перерыв", Code = "3", Description = "...." });
            statuses.Add(new Status() { Title = "Рабочий день закончен", Code = "5", Description = "...." });
            return await Task.FromResult(statuses);
        }

        public async Task<Worker> GetWorkerInfo()
        {
            return await Task.FromResult(new Worker()
            {
                Name = "Тостер",
                Surname = "Тестер",
                BirthDate = "11.11.1111",
                AccessLevel = "4",
                Mail = "toster@mail.ru",
                Patronymic = "Тостерович",
                Position = "Программист",
                Rate = "123"
            });
        }

        private StatusCode LastStatus { get; set; } = new StatusCode() { Code = "1", LastUpdate = DateTime.Now.ToString("g") };
        public string Session { get; set; }
        public string Server { get; set; }

        public async Task SetStatus(string Code)
        {
            LastStatus = new StatusCode() { Code = Code, LastUpdate = DateTime.Now.ToString("g") };
        }
        public async Task<StatusCode> GetLastStatusCode()
        {
            return await Task.FromResult(LastStatus);
        }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            if (Login == Password)
            {
                this.Session = "1234567890987654321";
                return Task.FromResult(AuthorizationResult.Ok);
            }
            else
            {
                return Task.FromResult(AuthorizationResult.Error);
            }
        }

        public Task<AuthorizationResult> Authorization()
        {
            return Task.FromResult(AuthorizationResult.Ok);
        }
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
            var diff = End - Start;

            if (diff.TotalSeconds < 0) return new List<Plan>();

            List<Plan> plans = new List<Plan>();
            int j = 1;
            for (DateTime i = Start; i <= End; i = i.AddDays(1))
            {
                var temp = STDPLAN;
                temp.Date = i.ToString("d");
                temp.Id = (1 + j).ToString();
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

        public async Task<AuthorizationResult> CheckConnect()
        {
            return await Task.FromResult(AuthorizationResult.Ok);
        }
    }
}