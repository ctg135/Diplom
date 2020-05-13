using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Client.ViewModels;
using Client.Views;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class Tasks : BaseViewModel
    {
        /// <summary>
        /// Класс для хранения задач
        /// </summary>
        public class Task
        {
            /// <summary>
            /// Id записи
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// Стадия задачи
            /// </summary>
            public string Stage { get; set; }
            /// <summary>
            /// Дата установки
            /// </summary>
            public string DateSetted { get; set; }
            /// <summary>
            /// Кем была поставлена задача
            /// </summary>
            public string Boss { get; set; }
            /// <summary>
            /// Дата окончания выполнения задачи
            /// </summary>
            public string DateFinished { get; set; }
            /// <summary>
            /// Команда открытия команды
            /// </summary>
            public ICommand OpenDetails { get; set; }
            /// <summary>
            /// Собыие открытия подробностей
            /// </summary>
            public event EventHandler OpeningDetails;
            public Task()
            {
                OpenDetails = new Command(OpenDetailsCommand);
            }
            /// <summary>
            /// Команда открытия деталей
            /// </summary>
            /// <param name="param"></param>
            private void OpenDetailsCommand(object param)
            {
                OpeningDetails(this, new EventArgs());
            }
        }
        /// <summary>
        /// Список всех задач
        /// </summary>
        public ObservableCollection<Task> Items { get; private set; }
        /// <summary>
        /// Конструктор класса задач
        /// </summary>
        public Tasks()
        {
            Items = new ObservableCollection<Task>();
        }
        /// <summary>
        /// Конструктор коллекции задач
        /// </summary>
        /// <param name="ListTasks">Список задач</param>
        public Tasks(List<Task> ListTasks)
        {
            Items = new ObservableCollection<Task>(ListTasks);
        }
    }
}
