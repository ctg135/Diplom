using log4net;
using System;

namespace Web_Service.Loggers
{
    /// <summary>
    /// Класс логгера-декоратора для вывода фатальных ошибок в общий лог
    /// </summary>
    public class FatalLogger : LoggerDecorator
    {
        /// <summary>
        /// Общий логгер
        /// </summary>
        public ILog CommonLogger { get; private set; }
        /// <summary>
        /// Название контроллера
        /// </summary>
        public string Controller { get; private set; }
        /// <summary>
        /// Конструктор логгера-декоратора с фатальных выводом ошибок в общий лог
        /// </summary>
        /// <param name="BaseLogger">"Обёртываемый" логгер</param>
        /// <param name="CommonLogger">Общий логгер</param>
        /// <param name="Controller">Название контроллера</param>
        public FatalLogger(ILog BaseLogger, ILog CommonLogger, string Controller) : base(BaseLogger)
        {
            this.CommonLogger = CommonLogger;
            this.Controller = Controller;
        }
    
        public override void Fatal(object message)
        {
            CommonLogger.Fatal($"Произошла фатальная ошибка в {Controller} {message}");
            base.Fatal(message);
        }
    
        public override void Fatal(object message, Exception exc)
        {
            CommonLogger.Fatal($"Произошла фатальная ошибка в {Controller} {message}");
            base.Fatal(message, exc);
        }
    }
}