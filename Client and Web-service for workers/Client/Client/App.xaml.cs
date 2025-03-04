﻿using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Client.IoC;
using Client.Views;
using CommonServiceLocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client
{
    public partial class App : Application
    {
        public App(Module PlatformModule)
        {
            InitializeComponent();
            InitializeDependencies(PlatformModule);

            Globals.Config = ServiceLocator.Current.GetService<Models.IConfigManager>();

            Current.MainPage = new LoadingPage();
        }

        private void InitializeDependencies(Module PlatformModule)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CrossPlatformModule());
            builder.RegisterModule(PlatformModule);

            var locator = new AutofacServiceLocator(builder.Build());
            ServiceLocator.SetLocatorProvider(() => locator);
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
