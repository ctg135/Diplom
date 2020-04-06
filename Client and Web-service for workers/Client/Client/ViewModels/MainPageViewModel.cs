using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Client.DataModels;

namespace Client.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        public ICommand Exit { get; set; } = new Command(Exitting);
        public ICommand SetNewStatus { get; set; } = new Command(SetStatus);
        public string Status { get; set; } = "На работе";
        public string LastUpdate { get; set; } = "11.12.2002 12:30";
        public Plan PlanToday { get; set; } = new Plan() { StartOfDay = "8:30" };

        private static void Exitting()
        {

        }
        private static void SetStatus()
        {
            string[] buts = { "One", "Two", "Threea" };
            Application.Current.MainPage.DisplayActionSheet("Выбор статуса", "Отмена", null, buts);
        }
    }
}
