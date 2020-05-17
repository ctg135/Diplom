using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Client.ViewModels;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            InitializeComponent();
            BindingContext = new TaskListPageViewModel();
            (BindingContext as TaskListPageViewModel).OpenDetailsPage += async (object sender, EventArgs args) => { await Navigation.PushAsync(new TaskDetailsPage(sender as Tasks.Task)); };
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            (BindingContext as TaskListPageViewModel).LoadTasks.Execute(new object());
        }
    }
}