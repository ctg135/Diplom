using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Создание окна настроек с параметрами по умолчанию
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel();
            (BindingContext as SettingsPageViewModel).ViewSaved += SettingsPage_ViewSaved;
        }
        /// <summary>
        /// Создание окна настроек
        /// </summary>
        /// <param name="AuthoButtonVisivle">При значении <c>true</c> отображает кнопку выхода из аккаунта</param>
        public SettingsPage(bool AuthoButtonVisivle = true)
        {
            InitializeComponent();
            BindingContext = new SettingsPageViewModel(AuthoButtonVisivle);
            (BindingContext as SettingsPageViewModel).ViewSaved += SettingsPage_ViewSaved;
        }
        /// <summary>
        /// Обработчик события просмотра планов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SettingsPage_ViewSaved(object sender, ViewPlansEventArgs args)
        {
            Navigation.PushAsync(new ViewPlansPage(args.Plans));
        }
    }
}