using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Client.Models;

namespace Client.ViewModels
{
    class TaskListPageViewModel : BaseViewModel
    {
        #region xamlData
        public bool IsDateRequired { get; set; }
        public bool NotAcceptedFilter { get; set; }
        public bool AcceptedFilter { get; set; }
        public bool CompletedFilter { get; set; }
        public DateTime SearchDate { get; set; }
        public Tasks TaskList { get; private set; }
        public ICommand OpenDetails { get; private set; }
        public ICommand LoadTasks { get; private set; }
        #endregion
        private IClientModel Client { get; set; }

        public event EventHandler OpenDetailsPage;
        private void OnTaskDetailsView(object sender, EventArgs args)
        {
            OpenDetailsPage(sender, args);
        }

        public TaskListPageViewModel()
        {
            LoadTasks = new Command(UpdateTaskList);
            this.Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<IClientModel>();

            IsDateRequired = false;
            NotAcceptedFilter = true;
            AcceptedFilter = true;
            CompletedFilter = false;
            SearchDate = DateTime.Now;

            TaskList = new Tasks();
        }

        private async void UpdateTaskList(object param)
        {
            foreach (var item in TaskList.Items)
            {
                item.OpeningDetails -= OnTaskDetailsView;
            }

            List<TaskStages> filter = new List<TaskStages>();
            if (NotAcceptedFilter) filter.Add(TaskStages.NotAccepted);
            if (AcceptedFilter) filter.Add(TaskStages.Processing);
            if (CompletedFilter) filter.Add(TaskStages.Completed);

            string sarg = IsDateRequired ? SearchDate.ToString() : DateTime.MinValue.ToString();

            try
            {
                TaskList = new Tasks(await Client.GetTasks(DateTime.Parse(sarg), filter.ToArray()));
            }
            catch (Exception exc)
            {
                await FatalError(exc.Message);
                return;
            }

            foreach (var item in TaskList.Items)
            {
                item.DateSetted = "Выдано: " + item.DateSetted;
                if (!string.IsNullOrWhiteSpace(item.DateFinished)) item.DateFinished = "Закончено " + item.DateFinished;
                item.OpeningDetails += OnTaskDetailsView;
                item.Stage = Globals.TaskStages[item.Stage];
            }

            NotifyPropertyChanged(nameof(TaskList));
        }

    }
}
