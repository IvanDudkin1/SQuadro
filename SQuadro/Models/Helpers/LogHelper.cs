using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQuadro.Service;

namespace SQuadro.Models
{
    public static class LogHelper
    {
        private static ILogger ILogger = Logger.GetLogger(Constants.LogFilePath);
 
        public static void Log(string text, LogType logType = LogType.Exception, Exception exception = null)
        {
            ILogger.Log(text, logType, exception);
        }
    }
}