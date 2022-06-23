using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SQuadro.Service
{
    public class Logger: ILogger
    {
        private Logger(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    var file = File.CreateText(path);
                    file.Close();
                }
                this.path = path;
            }
            catch
            {
                throw new InvalidOperationException("Initialization failed");
            }
        }
        
        private string path;

        public static ILogger GetLogger(string path)
        {
            return new Logger(path);
        }

        public void Log(string text, LogType logType, Exception exception)
        {
            using (var file = File.AppendText(this.path))
            {
                var logLine = String.Format("{0:f}: {1}: {2}", DateTimeOffset.Now, logType.ToString(), text);
                if (exception != null) logLine += String.Format(": {0}", exception);
                file.WriteLine(logLine);
            }
        }
    }

    public interface ILogger
    {
        void Log(string text, LogType logType = LogType.Information, Exception exception = null);
    }

    public enum LogType
    {
        Exception,
        Information,
        Debug
    }
}
