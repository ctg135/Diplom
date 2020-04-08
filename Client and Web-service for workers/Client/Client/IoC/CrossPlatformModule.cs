using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Client.ViewModels;

namespace Client.IoC
{
    class CrossPlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AuthoPageViewModel>();
            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<GraphicPageViewModel>();
        }
    }
}
