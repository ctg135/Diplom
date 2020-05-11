using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Client.Models;
using Client.DataModels;
using System.Threading.Tasks;
using Client.ViewModels;

namespace Client.Droid.Models
{
    public class MockClient : IClientModel
    {
        public string Session { get; set; }
        public string Server { get; set; }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            if (Login == Password) return Task.FromResult(AuthorizationResult.Ok);
            else return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> Authorization()
        {
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> CheckConnect()
        {
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<StatusCode> GetLastStatusCode()
        {
            return Task.FromResult(new StatusCode() { Code = "2", LastUpdate = DateTime.Now.ToString() });
        }

        public Task<List<Plan>> GetPlans(DateTime Start, DateTime End)
        {
            throw new NotImplementedException();

            List<Plan> plans = new List<Plan>();

            for (int i = 0; Start < End; i++, Start = Start.AddDays(1))
            {
                plans.Add(new Plan() { Date = Start.ToString(), Id = i.ToString(), StartOfDay = "8:30", EndOfDay = "9:30", Total = "1" });
            }

            return Task.FromResult(plans);
        }

        public Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = new List<Status>() 
            {
                new Status()
                {
                    Code = "1",
                    Title = "Не установлен"
                },
                new Status()
                {
                    Code = "2",
                    Title = "На работе"
                },
                new Status()
                {
                    Code = "3",
                    Title = "На перерыве"
                },
                new Status()
                {
                    Code = "4",
                    Title = "В отпуске"
                },
                new Status()
                {
                    Code = "5",
                    Title = "Рабочий день закончен"
                },
                new Status()
                {
                    Code = "6",
                    Title = "На больничном"
                },
                new Status()
                {
                    Code = "7",
                    Title = "На выходном"
                },
            };

            return Task.FromResult(statuses);
        }

        public Task<Plan1> GetTodayPlan()
        {
            return Task.FromResult(new Plan1() { DateSet = DateTime.Now.ToString("dd.MM.yyyy"), StartDay = "8:00", EndDay = "16:00", TypePlan = "1" });
        }

        public Task<List<PlanType>> GetPlanTypes()
        {
            List<PlanType> planTypes = new List<PlanType>() 
            {
                new PlanType()
                {
                    Code = "1",
                    Title = "Рабочий день"
                },
                new PlanType()
                {
                    Code = "2",
                    Title = "Выходной"
                },
                new PlanType()
                {
                    Code = "3",
                    Title = "Больничный"
                },
                new PlanType()
                {
                    Code = "4",
                    Title = "Отпускной"
                },
                new PlanType()
                {
                    Code = "0",
                    Title = "-"
                }
            };

            return Task.FromResult(planTypes);
        }

        public Task<Worker1> GetWorkerInfo()
        {
            return Task.FromResult(new Worker1() { Name = "Имя", Patronymic = "Фамилия", Surname = "Отчество",  Department ="Программистический", Position = "Программист" });
        }

        public Task<bool> IsSetStatusClientError(string ErrorMessage)
        {
            throw new NotImplementedException();
        }

        public Task SetStatus(string Code)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks.Task>> GetTasks(TaskStages[] Filter)
        {
            var tasks = new List<Tasks.Task>();

            foreach (var stage in Filter )
            {
                if (stage == TaskStages.NotAccepted)
                {
                    tasks.Add(new Tasks.Task()
                    {
                        Id = "1",
                        Stage = "1",
                        Boss = "Сам я",
                        DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                        Description = "Копать картоху"
                    });
                }
                if (stage == TaskStages.Processing)
                {
                    tasks.Add(new Tasks.Task()
                    {
                        Id = "2",
                        Stage = "2",
                        Boss = "Сам я",
                        DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                        Description = "Деплом"
                    });
                }
                if (stage == TaskStages.Completed)
                {
                    tasks.Add(new Tasks.Task()
                    {
                        Id = "3",
                        Stage = "3",
                        Boss = "Сам я",
                        DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                        Description = "Курсач"
                    });
                }
            }

            return Task.FromResult(tasks);
        }

        public Task<List<TaskStage>> GetTaskStages()
        {
            return Task.FromResult(new List<TaskStage>() 
            {  
                new TaskStage()
                {
                    Code = "1",
                    Title = "Ожидает принятия"
                },
                new TaskStage()
                {
                    Code = "2",
                    Title = "Принят к выполнению"
                },
                new TaskStage()
                {
                    Code = "3",
                    Title = "Выполнено"
                },
            });
        }

        public Task AcceptTask(string TaskId)
        {
            System.Diagnostics.Debug.WriteLine($"Задача №{TaskId} принята");
            return Task.CompletedTask;
        }

        public Task CompleteTask(string TaskId)
        {
            System.Diagnostics.Debug.WriteLine($"Задача №{TaskId} завершена");
            return Task.CompletedTask;
        }
    }
}