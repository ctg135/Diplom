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