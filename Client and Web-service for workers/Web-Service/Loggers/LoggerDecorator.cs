using log4net;
using log4net.Core;
using System;

namespace Web_Service.Loggers
{
    /// <summary>
    /// Декоратор для логгера (Обёртка)
    /// </summary>
    public abstract class LoggerDecorator : ILog
    {
        /// <summary>
        /// "Обёртываемый" логгер
        /// </summary>
        protected ILog Base { get; set; }
        /// <summary>
        /// Конструктор для декоратора логгера
        /// </summary>
        /// <param name="BaseLogger">"Обёртываемый" логгер</param>
        public LoggerDecorator(ILog BaseLogger)
        {
            Base = BaseLogger;
        }
        public virtual bool IsDebugEnabled => Base.IsDebugEnabled;

        public virtual bool IsInfoEnabled => Base.IsInfoEnabled;

        public virtual bool IsWarnEnabled => Base.IsWarnEnabled;

        public virtual  bool IsErrorEnabled => Base.IsErrorEnabled;

        public virtual bool IsFatalEnabled => Base.IsFatalEnabled;

        public virtual ILogger Logger => Base.Logger;

        public virtual void Debug(object message)
        {
            Base.Debug(message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            Base.Debug(message, exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            Base.DebugFormat(format, args);
        }

        public virtual void DebugFormat(string format, object arg0)
        {
            Base.DebugFormat(format, arg0);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1)
        {
            Base.DebugFormat(format, arg0, arg1);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.DebugFormat(format, arg0, arg1, arg2);
        }

        public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.DebugFormat(provider, format, args);
        }

        public virtual void Error(object message)
        {
            Base.Error(message);
        }

        public virtual void Error(object message, Exception exception)
        {
            Base.Error(message, exception);
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            Base.ErrorFormat(format, args);
        }

        public virtual void ErrorFormat(string format, object arg0)
        {
            Base.ErrorFormat(format, arg0);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1)
        {
            Base.ErrorFormat(format, arg0, arg1);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.ErrorFormat(format, arg0, arg1, arg2);
        }

        public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.ErrorFormat(provider, format, args);
        }

        public virtual void Fatal(object message)
        {
            Base.Fatal(message);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            Base.Fatal(message, exception);
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            Base.FatalFormat(format, args);
        }

        public virtual void FatalFormat(string format, object arg0)
        {
            Base.FatalFormat(format, arg0);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1)
        {
            Base.FatalFormat(format, arg0, arg1);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.FatalFormat(format, arg0, arg1, arg2);
        }

        public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.FatalFormat(provider, format, args);
        }

        public virtual void Info(object message)
        {
            Base.Info(message);
        }

        public virtual void Info(object message, Exception exception)
        {
            Base.Info(message, exception);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            Base.InfoFormat(format, args);
        }

        public virtual void InfoFormat(string format, object arg0)
        {
            Base.InfoFormat(format, arg0);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1)
        {
            Base.InfoFormat(format, arg0, arg1);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.InfoFormat(format, arg0, arg1, arg2);
        }

        public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.InfoFormat(provider, format, args);
        }

        public virtual void Warn(object message)
        {
            Base.Warn(message);
        }

        public virtual void Warn(object message, Exception exception)
        {
            Base.Warn(message, exception);
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            Base.WarnFormat(format, args);
        }

        public virtual void WarnFormat(string format, object arg0)
        {
            Base.WarnFormat(format, arg0);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1)
        {
            Base.WarnFormat(format, arg0, arg1);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.WarnFormat(format, arg0, arg1, arg2);
        }

        public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.WarnFormat(provider, format, args);
        }
    }

}