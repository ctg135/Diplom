using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                    new MainMenuItem { Id = 0, Title = "Главная",   TargetType = typeof(MainPage) },
                    new MainMenuItem { Id = 1, Title = "Задания",   TargetType = typeof(TaskListPage) },
                    new MainMenuItem { Id = 2, Title = "График",    TargetType = typeof(GraphicPage) },
                    new MainMenuItem { Id = 3, Title = "Настройки", TargetType = typeof(SettingsPage) },
                    //new MainMenuItem { Id = 3, Title = "Задания",   TargetType = typeof(TasksViewPage) }
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