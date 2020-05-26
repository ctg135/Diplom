using Client.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetailsPage : ContentPage
    {
        public TaskDetailsPage(Tasks.Task Item)
        {
            InitializeComponent();
            BindingContext = new TaskDetailsPageViewModel(Item);
            (BindingContext as TaskDetailsPageViewModel).Exit += TaskDetailsPage_Exit;
            Title = $"Задача №{Item.Id}";
        }

        private async void TaskDetailsPage_Exit(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}