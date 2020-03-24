using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace Web_Service
{
    /// <summary>
    /// Логгер
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Общий логгер
        /// </summary>
        public static ILog Log { get; } = LogManager.GetLogger("LOGGER");
        /// <summary>
        /// Логгер для контроллера api/Autho
        /// </summary>
        public static ILog AuthoLog { get; } = LogManager.GetLogger("LOGGER_AUTHO");
        public static ILog WorkerLog { get; } = LogManager.GetLogger("LOGGER_WORKER");
        /// <summary>
        /// Инициализация логгеров
        /// </summary>
        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}