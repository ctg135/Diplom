using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModels
{
    class TaskDetailsPageViewModel : BaseViewModel
    {
        private IClientModel Client { get; set; }
        public Tasks.Task Item { get; set; }
        public ICommand ButtonCommand { get; set; }
        public string ButtonText { get; set; }
        public bool ButtonVisibility { get; set; }

        public event EventHandler Exit;
        public TaskDetailsPageViewModel(Tasks.Task Item)
        {
            this.Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<IClientModel>();
            this.Client.Session = Globals.Config.GetItem("Session").Result;
            this.Client.Server = Globals.Config.GetItem("Server").Result;
            this.Item = Item;
            ButtonVisibility = new bool();
            ButtonText = string.Empty;
            if (Item.Stage == Globals.TaskStages["1"]) // Если задача непринята
            {
                ButtonCommand = new Command(AcceptTask);
                ButtonText = "Принять задачу";
                ButtonVisibility = true;
            }
            else if (Item.Stage == Globals.TaskStages["2"]) // Если задача выполняется
            {
                ButtonCommand = new Command(CompleteTask);
                ButtonText = "Завершить задачу";
                ButtonVisibility = true;
            }
            else if (Item.Stage == Globals.TaskStages["3"]) // Если задача завершена
            {
                ButtonVisibility = false;
            }
            else
            {
                throw new Exception($"Необработанная стадия '{Item.Stage}' задачи '{Item.Id}'");
            }
        }
        private void AcceptTask(object param)
        {
            Client.AcceptTask(Item.Id);
            Exit(this, new EventArgs());
        }
        private void CompleteTask(object param)
        {
            Client.CompleteTask(Item.Id);
            Exit(this, new EventArgs());
        }
    }
}
