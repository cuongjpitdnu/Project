using System;
using System.Runtime.CompilerServices;
using GPMain.Common.Interface;
using Serilog;

namespace GPMain.Common.Helper
{
    /// <summary>
    /// Meno        : Support write log in System
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    public class LogHelper : ILog
    {
        private const string FORMAT_MSG = "{0}.{1} {2}"; // {className}.{methodName} {msgContent}
        private ILogger _logger;

        private enum LevelLog
        {
            Info,
            Debug,
            Error,
        }

        public LogHelper(string pathFile)
        {
            _logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(pathFile, rollingInterval: RollingInterval.Day, outputTemplate: @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}", shared: true).CreateLogger();
        }

        public void Info(Type type, string msg = "", [CallerMemberName] string memberName = "")
        {
            WriteLog(LevelLog.Info, type, null, msg, memberName);
        }

        public void Debug(Type type, string msg = "", [CallerMemberName] string memberName = "")
        {
            WriteLog(LevelLog.Debug, type, null, msg, memberName);
        }

        public void Error(Type type, Exception ex, string msg = "", [CallerMemberName] string memberName = "")
        {
            WriteLog(LevelLog.Error, type, ex, msg, memberName);
        }

        private void WriteLog(LevelLog levelLog, Type type, Exception ex, string msg = "", string memberName = "")
        {
            var className = type != null ? type.FullName : "";
            switch (levelLog)
            {
                case LevelLog.Info:
                    _logger.Information(string.Format(FORMAT_MSG, className, memberName, msg).Trim());
                    break;
                case LevelLog.Debug:
                    _logger.Debug(string.Format(FORMAT_MSG, className, memberName, msg).Trim());
                    break;
                case LevelLog.Error:
                    _logger.Error(ex, string.Format(FORMAT_MSG, className, memberName, msg).Trim());
                    break;
            }
        }
    }
}
