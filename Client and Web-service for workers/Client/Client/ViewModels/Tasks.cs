using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Client.ViewModels;

namespace Client.ViewModels
{
    public class Tasks : BaseViewModel
    {
        /// <summary>
        /// Класс для хранения задач
        /// </summary>
        public class Task 
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public string Stage { get; set; }
            public string DateSetted { get; set; }
            public string Boss { get; set; }
        }
        /// <summary>
        /// Список всех задач
        /// </summary>
        public ObservableCollection<Task> Items { get; private set; }
        public Tasks()
        {
            Items = new ObservableCollection<Task>();
        }
        public Tasks(List<Task> ListTasks)
        {
            Items = new ObservableCollection<Task>(ListTasks);
        }
        public void UpLoadDefault()
        {
            Items = new ObservableCollection<Task>()
            {
                new Task()
                {
                    Id = "1",
                    Description = "Капать картошку",
                    Stage = "Выполняется",
                    DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                    Boss = "Супер маслята"
                },
                new Task()
                {
                    Id = "2",
                    Description = "ТЯВА ТЯЯЯВА",
                    Stage = "0.0",
                    DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                    Boss = "Супер маслята"
                },
                new Task()
                {
                    Id = "3",
                    Description = "Делай дипломчег Делай дипломчег Делай дипломчег Делай дипломчег Делай дипломчег Делай дипломчегДелай дипломчег",
                    Stage = "Выполняется",
                    DateSetted = DateTime.Now.ToString("dd.MM.yyyy"),
                    Boss = "Супер маслята"
                }
            };
        }
    }
}
