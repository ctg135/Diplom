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
            public string Id { get; set; }
            public string Description { get; set; }
            public string Stage { get; set; }
            public string DateSetted { get; set; }
            public string Boss { get; set; }
            public ICommand OpenDetails { get; set; }

            public event EventHandler OpeningDetails;
            public Task()
            {
                OpenDetails = new Command(OpenDetailsCommand);
            }
            private void OpenDetailsCommand(object param)
            {
                OpeningDetails(this, new EventArgs());
            }
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
    }
}
