using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Client.Views;

namespace Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AuthoPage());
            //MainPage = new AuthoPage();
            
            //(MainPage.BindingContext as ViewModels.AuthoPageViewModel).Authorized += App_Authorized;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
