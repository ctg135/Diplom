using NLog;

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
        public static ILogger Log { get; } = LogManager.GetLogger("common");
        /// <summary>
        /// Логгер для контроллера api/Autho
        /// </summary>
        public static ILogger AuthoLog { get; } = LogManager.GetLogger("api/Autho");
        /// <summary>
        /// Логгер для контроллера api/Worker
        /// </summary>
        public static ILogger WorkerLog { get; } = LogManager.GetLogger("api/Worker");
        /// <summary>
        /// Логгер для контроллера api/Status
        /// </summary>
        public static ILogger StatusLog { get; } = LogManager.GetLogger("api/Status");
        /// <summary>
        /// Логгер для контроллера api/Plan
        /// </summary>
        public static ILogger PlanLog { get; } = LogManager.GetLogger("api/Plan");
    }
}