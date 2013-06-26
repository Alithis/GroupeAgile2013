using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RateYourRent.Helper
{
    public static class ExceptionEx
    {
        public static void Log(this Exception exception)
        {

        }

        public static void LogJS(string name, string method, string message)
        {
            System.IO.File.AppendAllLines(HttpContext.Current.Server.MapPath("/log.txt"),
                new string[] { string.Format("{0} - {1}.{2} : {3}", DateTime.Now, method, name, message) });
        }
    }
}