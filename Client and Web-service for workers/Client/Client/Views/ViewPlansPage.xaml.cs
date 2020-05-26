using Client.DataModels;
using Client.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPlansPage : ContentPage
    {
        public ViewPlansPage(List<Plan> PlanList)
        {
            this.BindingContext = new VeiwPlansPageViewModel(PlanList);
            InitializeComponent();
        }

        private void OnLeftSwipeCalendar(object sender, SwipedEventArgs e)
        {
            calendar.NextMonth();
        }
        private void OnRightSwipeCalendar(object sender, SwipedEventArgs e)
        {
            calendar.PreviousMonth();
        }
    }
}