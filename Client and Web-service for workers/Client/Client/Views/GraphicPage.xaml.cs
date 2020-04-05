using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphicPage : ContentPage
    {
        public GraphicPage()
        {
            InitializeComponent();
            this.BindingContext = new GraphicPageViewModel();
        }
    }
}