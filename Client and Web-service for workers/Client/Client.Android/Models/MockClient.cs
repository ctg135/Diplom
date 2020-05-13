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
using Client.ViewModels;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Droid.Models
{
    public class MockClient : IClientModel
    {
        public string Session { get; set; }
        public string Server { get; set; }

        public Task<AuthorizationResult> Authorization(string Login, string Password)
        {
            System.Diagnostics.Debug.WriteLine($"Паролимся {Login}, {Password}");
            if (Login == Password) return Task.FromResult(AuthorizationResult.Ok);
            else return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> Authorization()
        {
            System.Diagnostics.Debug.WriteLine($"Неработящая авторизация");
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<AuthorizationResult> CheckConnect()
        {
            System.Diagnostics.Debug.WriteLine($"Проверка подключения");
            return Task.FromResult(AuthorizationResult.Error);
        }

        public Task<StatusCode> GetLastStatusCode()
        {
            System.Diagnostics.Debug.WriteLine($"Получаем последний статус");
            return Task.FromResult(new StatusCode() { Code = "2", LastUpdate = DateTime.Now.ToString() });
        }

        public Task<List<Plan1>> GetPlans(DateTime Start, DateTime End, PlanTypes[] Filter)
        {
            System.Diagnostics.Debug.WriteLine($"Выводим планы {Start} - {End}, {JsonConvert.SerializeObject(Filter)}");

            List<Plan1> plans = new List<Plan1>();
            List<Plan1> templateplans = new List<Plan1>()
            {
                new Plan1()
                {
                    TypePlan = "1",
                    DateSet = Start.ToString("dd.MM.yyyy"),
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "2",
                    DateSet = End.ToString("dd.MM.yyyy"),
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "3",
                    DateSet = End.ToString("dd.MM.yyyy"),
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
                new Plan1()
                {
                    TypePlan = "4",
                    DateSet = End.ToString("dd.MM.yyyy"),
                    StartDay = "8:30",
                    EndDay = "10:30"
                },
            };

            if (Filter.Length == 0)
            {
                plans = templateplans;
            }
            else 
            {
                foreach(var filter in Filter)
                {
                    if (filter == PlanTypes.DayOff)
                    {
                        plans.Add(templateplans[1]);
                    }
                    else if (filter == PlanTypes.Holiday)
                    {
                        plans.Add(templateplans[3]);
                    }
                    else if (filter == PlanTypes.Hospital)
                    {
                        plans.Add(templateplans[2]);
                    }
                    else if (filter == PlanTypes.Working)
                    {
                        plans.Add(templateplans[0]);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Неизвестная дрянь '{filter}'");
                    }
                }
            }
            return Task.FromResult(plans);
        }

        public Task<List<Status>> GetStatuses()
        {
            System.Diagnostics.Debug.WriteLine($"Получаем типы статусов");
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
            System.Diagnostics.Debug.WriteLine($"План на сегодня");
            return Task.FromResult(new Plan1() { DateSet = DateTime.Now.ToString("dd.MM.yyyy"), StartDay = "8:00", EndDay = "16:00", TypePlan = "1" });
        }

        public Task<List<PlanType>> GetPlanTypes()
        {
            System.Diagnostics.Debug.WriteLine($"Типы планов");

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
            System.Diagnostics.Debug.WriteLine($"Инфо о работнике");

            return Task.FromResult(new Worker1() { Name = "Имя", Patronymic = "Фамилия", Surname = "Отчество",  Department ="Программистический", Position = "Программист" });
        }

        public Task<bool> IsSetStatusClientError(string ErrorMessage)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка ли в установке статуса?");

            throw new NotImplementedException();
        }

        public Task SetStatus(string Code)
        {
            System.Diagnostics.Debug.WriteLine($"Установка нового статуса ${Code}");

            throw new NotImplementedException();
        }

        public Task<List<Tasks.Task>> GetTasks(TaskStages[] Filter)
        {
            System.Diagnostics.Debug.WriteLine($"Получение задач по {Filter} ПОКА БЕЗ ДАТЫ!1!");

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
                        DateFinished = DateTime.Now.ToString("dd.MM.yyyy"),
                        Description = "Курсач"
                    });
                }
            }

            return Task.FromResult(tasks);
        }

        public Task<List<TaskStage>> GetTaskStages()
        {
            System.Diagnostics.Debug.WriteLine($"Список готовых стадий");

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