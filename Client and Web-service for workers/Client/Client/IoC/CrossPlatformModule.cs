using Autofac;
using Client.ViewModels;
using Client.Views;

namespace Client.IoC
{
    class CrossPlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<LoadingPageViewModel>();
            builder.RegisterType<AuthoPageViewModel>();
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<GraphicPageViewModel>();
            builder.RegisterType<SettingsPageViewModel>();
            builder.RegisterType<TaskDetailsPageViewModel>();
            builder.RegisterType<TaskListPageViewModel>();
            builder.RegisterType<VeiwPlansPageViewModel>();
        }
    }
}
