using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Client.ViewModels;
using Client.DataModels;

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