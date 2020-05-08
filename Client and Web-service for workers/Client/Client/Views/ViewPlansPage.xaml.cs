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
        public ViewPlansPage(List<Plan1> PlanList)
        {
            this.BindingContext = new VeiwPlansPageViewModel(PlanList);
            InitializeComponent();
            Title = $"Всего {PlanList.Count.ToString()}";
        }
    }
}