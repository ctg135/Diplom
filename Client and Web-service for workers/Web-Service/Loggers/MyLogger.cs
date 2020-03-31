using System;
using log4net;

namespace Web_Service.Loggers
{
    /// <summary>
    /// Класс логгера-декоратора для вывода ошибок в общий лог
    /// </summary>
    public class MyLogger : LoggerDecorator
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
        /// Конструктор логгера-декоратора с выводом ошибок в общий лог
        /// </summary>
        /// <param name="BaseLogger">"Обёртываемый" логгер</param>
        /// <param name="CommonLogger">Общий логгер</param>
        /// <param name="Controller">Название контроллера</param>
        public MyLogger(ILog BaseLogger, ILog CommonLogger, string Controller) : base(BaseLogger)
        {
            this.CommonLogger = CommonLogger;
            this.Controller = Controller;
        }
        public override void Error(object message)
        {
            base.Error(message);
            CommonLogger.Error($"Произошла ошибка в {Controller} {message}");
        }
    
        public override void Error(object message, Exception exc)
        {
            base.Error(message, exc);
            CommonLogger.Error($"Произошла ошибка в {Controller} {exc.Message}");
        }
    
        public override void Fatal(object message)
        {
            base.Fatal(message);
            CommonLogger.Fatal($"Произошла ошибка в {Controller} {message}");
        }
    
        public override void Fatal(object message, Exception exc)
        {
            base.Fatal(message, exc);
            CommonLogger.Fatal($"Произошла ошибка в {Controller} {exc.Message}");
        }
    }
}