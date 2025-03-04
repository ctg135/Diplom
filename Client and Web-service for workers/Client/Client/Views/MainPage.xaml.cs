﻿using Client.ViewModels;
using CommonServiceLocator;
using System;
using System.ComponentModel;
using Xamarin.Forms;

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
            (this.BindingContext as MainPageViewModel).NavigateToTaskDetail += MainPage_NavigateToTaskDetail;
        }

        private async void MainPage_NavigateToTaskDetail(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaskDetailsPage(sender as Tasks.Task));
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ((ViewModels.MainPageViewModel)BindingContext).UpdateData.Execute(new object());
        }
    }
}
