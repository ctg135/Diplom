using Client.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            InitializeComponent();
            BindingContext = CommonServiceLocator.ServiceLocator.Current.GetInstance<TaskListPageViewModel>();
            (BindingContext as TaskListPageViewModel).OpenDetailsPage += async (object sender, EventArgs args) => { await Navigation.PushAsync(new TaskDetailsPage(sender as Tasks.Task)); };
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            (BindingContext as TaskListPageViewModel).LoadTasks.Execute(new object());
        }
    }
}