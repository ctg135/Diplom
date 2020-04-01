using log4net;
using log4net.Config;

namespace Web_Service.Loggers
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
        public static ILog AuthoLog { get; } = new FatalLogger(
            BaseLogger:   LogManager.GetLogger("LOGGER_AUTHO"),
            CommonLogger: Log,
            Controller:   "api/Autho");
        /// <summary>
        /// Логгер для контроллера api/Worker
        /// </summary>
        public static ILog WorkerLog { get; } = new FatalLogger(
            BaseLogger:   LogManager.GetLogger("LOGGER_WORKER"),
            CommonLogger: Log,
            Controller:   "api/Worker");
        /// <summary>
        /// Логгер для контроллера api/Status
        /// </summary>
        public static ILog StatusLog { get; } = new FatalLogger(
            BaseLogger:   LogManager.GetLogger("LOGGER_STATUS"),
            CommonLogger: Log,
            Controller:   "api/Status");
        /// <summary>
        /// Логгер для контроллера api/Plan
        /// </summary>
        public static ILog PlanLog { get; } = new FatalLogger(
            BaseLogger:   LogManager.GetLogger("LOGGER_PLAN"),
            CommonLogger: Log,
            Controller:   "api/Plan");
        /// <summary>
        /// Логгер для контроллера api/Connection
        /// </summary>
        public static ILog ConnectionLog { get; } = new FatalLogger(
            BaseLogger:   LogManager.GetLogger("LOGGER_CONNECTION"),
            CommonLogger: Log,
            Controller:   "api/Connection");
        /// <summary>
        /// Инициализация логгеров
        /// </summary>
        public static void InitLogger()
        {
            // Выгрузка информации из Web.config
            XmlConfigurator.Configure();
        }
    }
}