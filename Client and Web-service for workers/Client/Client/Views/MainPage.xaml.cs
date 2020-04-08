using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Client.ViewModels;

using CommonServiceLocator;

namespace Client.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = ServiceLocator.Current.GetInstance<MainPageViewModel>();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ((ViewModels.MainPageViewModel)BindingContext).UpdateData.Execute(new object());
        }
    }
}
