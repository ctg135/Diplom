using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPageMaster : ContentPage
    {
        public ListView ListView;

        public MainMenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainMenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMenuPageMasterViewModel : INotifyPropertyChanged
        {
            public string WorkerName
            {
                get
                {
                    return Globals.WorkerInfo.Name + " " + Globals.WorkerInfo.Surname;
                }
            }
            public string WorkerPosition
            {
                get
                {
                    return Globals.WorkerInfo.Position;
                }
            }

            public ObservableCollection<MainMenuItem> MenuItems { get; set; }

            public MainMenuPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMenuItem>(new[]
                {
                    new MainMenuItem { Id = 0, Title = "Главная",   TargetType = typeof(MainPage), IconSource = ImageSource.FromResource("Client.Images.MainMenuIcon.home.png") },
                    new MainMenuItem { Id = 1, Title = "Задания",   TargetType = typeof(TaskListPage), IconSource = ImageSource.FromResource("Client.Images.MainMenuIcon.tasks.png") },
                    new MainMenuItem { Id = 2, Title = "График",    TargetType = typeof(GraphicPage), IconSource = ImageSource.FromResource("Client.Images.MainMenuIcon.calendar.png") },
                    new MainMenuItem { Id = 3, Title = "Настройки", TargetType = typeof(SettingsPage), IconSource = ImageSource.FromResource("Client.Images.MainMenuIcon.cog.png") },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}