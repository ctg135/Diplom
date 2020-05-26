using Client.Views;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
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
