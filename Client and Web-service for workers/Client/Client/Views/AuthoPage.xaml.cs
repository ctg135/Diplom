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
    public partial class AuthoPage : ContentPage
    {
        public AuthoPage()
        {
            InitializeComponent();
            BindingContext = new AuthoPageViewModel(new Models.ClientMock());
        }
    }
}