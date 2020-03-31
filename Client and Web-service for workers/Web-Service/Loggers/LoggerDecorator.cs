using System;
using log4net;
using log4net.Core;

namespace Web_Service.Loggers
{
    public abstract class LoggerDecorator : ILog
    {
        protected ILog Base { get; set; }
        public LoggerDecorator(ILog BaseLogger)
        {
            Base = BaseLogger;
        }
        public bool IsDebugEnabled => Base.IsDebugEnabled;

        public bool IsInfoEnabled => Base.IsInfoEnabled;

        public bool IsWarnEnabled => Base.IsWarnEnabled;

        public bool IsErrorEnabled => Base.IsErrorEnabled;

        public bool IsFatalEnabled => Base.IsFatalEnabled;

        public ILogger Logger => Base.Logger;

        public void Debug(object message)
        {
            Base.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            Base.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Base.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            Base.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            Base.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.DebugFormat(provider, format, args);
        }

        public void Error(object message)
        {
            Base.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            Base.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Base.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            Base.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            Base.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            Base.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            Base.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            Base.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            Base.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            Base.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.FatalFormat(provider, format, args);
        }

        public void Info(object message)
        {
            Base.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            Base.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Base.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            Base.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            Base.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            Base.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            Base.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Base.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            Base.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            Base.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Base.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Base.WarnFormat(provider, format, args);
        }
    }

}