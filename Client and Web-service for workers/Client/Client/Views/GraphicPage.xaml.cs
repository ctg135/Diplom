using Client.ViewModels;
using CommonServiceLocator;
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
            this.BindingContext = ServiceLocator.Current.GetInstance<GraphicPageViewModel>();
            (BindingContext as GraphicPageViewModel).ViewPlans += GraphicPage_ViewPlans;
        }

        private void GraphicPage_ViewPlans(object sender, ViewPlansEventArgs args)
        {
            Navigation.PushAsync(new ViewPlansPage(args.Plans));
        }
    }
}