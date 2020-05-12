using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Client.ViewModels;
using Client.Models;
namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel();
        }
        public SettingsPage(bool AuthoButtonVisivle = true)
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel(AuthoButtonVisivle);
        }
    }
}