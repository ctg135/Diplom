using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Autofac;

namespace Client.iOS.IoC
{
    class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}