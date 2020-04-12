using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonServiceLocator;

using Client.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<LoadingPageViewModel>();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ((LoadingPageViewModel)BindingContext).Authorize.Execute(new object());
        }
    }
}