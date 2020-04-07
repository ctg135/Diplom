using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Xamarin.Forms;
using System.Threading.Tasks;
using Client.Views;

namespace Client.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        /// <summary>
        /// Вывод сообщения об ошибке и выход на страницу авторизации с очисткой <c>Globals</c>
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected async Task FatalError(string Message)
        {
            var rootpage = Application.Current.MainPage;
            await rootpage.DisplayAlert("Ошибка", Message, "Ок");
            Globals.Clear();
            rootpage = new NavigationPage(new AuthoPage());
        }
    }
}
