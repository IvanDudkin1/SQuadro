using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQuadro.Models
{
    public static class ExceptionsHelper
    {
        public static string ProcessException(Exception exception)
        {
            if (exception is UserException)
                return exception.Message;
            else
            {
                try
                {
                    LogHelper.Log(exception.GetMessage(), SQuadro.Service.LogType.Exception, exception);
                }
                catch { }
                return "Oops, something went wrong.";
            }
        }
    }

    public class UserException : Exception
    {
        public UserException()
        {
        }

        public UserException(string message)
            : base(message)
        {
        }

        public UserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}