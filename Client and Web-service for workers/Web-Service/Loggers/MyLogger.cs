using System;
using log4net;

namespace Web_Service.Loggers
{
    public class MyLogger : LoggerDecorator
    {
        public ILog CommonLogger { get; private set; }
        public string Controller { get; private set; }
        public MyLogger(ILog BaseLogger, ILog CommonLogger, string Controller) : base(BaseLogger)
        {
            this.CommonLogger = CommonLogger;
            this.Controller = Controller;
        }
        public void Error(object message)
        {
            base.Error(message);
            CommonLogger.Error($"В {Controller} произошла ошибка: {message}");
        }
    
        public void Error(object message, Exception exc)
        {
            base.Error(message, exc);
            CommonLogger.Error($"В {Controller} произошла ошибка: {exc.Message}");
        }
    
        public void Fatal(object message)
        {
            base.Fatal(message);
            CommonLogger.Fatal($"В {Controller} произошла фатальная ошибка: {message}");
        }
    
        public void Fatal(object message, Exception exc)
        {
            base.Fatal(message, exc);
            CommonLogger.Fatal($"В {Controller} произошла фатальная ошибка: {exc.Message}");
        }
    }
}