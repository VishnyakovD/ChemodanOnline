using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Shop.Logger
{
    public interface ILogger
    {
        void Debug(object message);
        void DebugFormat(string format, params object[] args);
        void Info(object message);
        void InfoFormat(string format, params object[] args);
        void Warn(object message);
        void WarnFormat(string format, object arg0);
        void Error(object message);
        void Error(string message, Exception exception);
        void ErrorFormat(string format, params object[] args);
        void Fatal(object message);
        void Fatal(string message, Exception exception);
        void FatalFormat(string format, params object[] args);
    }

    public class Logger : ILogger
    {
        private readonly NLog.Logger _log;

        public Logger(Type logType)
        {
            _log = LogManager.GetLogger(logType.FullName);
        }

        public void Debug(object message)
        {
           _log.Debug(message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.Debug(format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);
  
        }

        public void InfoFormat(string format, params object[] args)
        {

            _log.Info(format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void WarnFormat(string format, object arg0)
        {
            _log.Warn(format, arg0);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.Error(format, args);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _log.Fatal(format, args);
        }
    }
}
