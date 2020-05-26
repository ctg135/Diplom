using Client.Models;
using Client.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModels
{
    class LoadingPageViewModel : BaseViewModel
    {
        private IClientModel Client { get; set; }
        public ICommand Authorize { get; private set; }
        public LoadingPageViewModel(IClientModel client)
        {
            Client = client;
            Authorize = new Command(Autho);
        }
        private async void Autho()
        {
            string session = await Globals.Config.GetItem("Session");
            string server = await Globals.Config.GetItem("Server");

            if (string.IsNullOrEmpty(session) || string.IsNullOrEmpty(server))
            {
                Application.Current.MainPage = new NavigationPage(new AuthoPage());
            }
            else
            {
                Client.Server = server;
                Client.Session = session;
                var result = new AuthorizationResult();
                try
                {
                    result = await Client.Authorization();
                }
                catch(Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Возникла ошибка при подключении к серверу", "Проверьте настройки подключения", "Ок");
                    Application.Current.MainPage = new NavigationPage(new AuthoPage());
                    return;
                }
                
                switch (result)
                {
                    case AuthorizationResult.Ok:
                        await Globals.Load(session, server);
                        Application.Current.MainPage = new MainMenuPage();
                        break;
                    case AuthorizationResult.Error:
                        Application.Current.MainPage = new NavigationPage(new AuthoPage());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
