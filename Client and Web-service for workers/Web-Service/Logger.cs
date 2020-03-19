using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace Web_Service
{
    public static class Logger
    {
        public static ILog Log { get; } = LogManager.GetLogger("LOGGER");
        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}