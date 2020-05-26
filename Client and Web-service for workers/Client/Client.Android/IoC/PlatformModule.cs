using Autofac;
using Client.Models;

namespace Client.Droid.IoC
{
    class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Models.XmlPlanLoader>().As<IPlanLoader>();
            builder.RegisterType<Models.RestClient>().As<IClientModel>();
            builder.RegisterType<Models.XmlConfigManager>().As<IConfigManager>();
        }
    }
}