using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web;
using Newtonsoft.Json;
using NLog;

namespace Utility
{
    public static class PageUtility
    {


        public static void bindControls(List<Control> controls)
        {
            foreach (var c in controls)
                c.DataBind();
        }
        public static string cleanString(string input)
        {
            Regex rgx = new Regex("[^0-9a-zA-Z]");
            var result = rgx.Replace(input, "-");
            return result.Replace(" ", "_");
        }
        public static string decimalString(decimal? value)
        {
            if (value.HasValue)
                return value.Value.ToString("0.00");
            else
                return "";
        }
        public static decimal? decimalValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return Convert.ToDecimal(value.Replace(".", ","));
        }

        public static double getFilesize(string path)
        {
            try
            {
                double length = new System.IO.FileInfo(path).Length / 1024.0 / 1024.0;
                return length;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string RandomString(Random r, int length, bool includeSpecialChar = false)
        {
            string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            if (includeSpecialChar)
                valid += "!@?_-$";
            StringBuilder res = new StringBuilder();

            while (0 < length--)
                res.Append(valid[r.Next(valid.Length)]);
            return res.ToString();
        }

        public static void logError(Logger logger, LogLevel level, Exception ex = null, string message = null)
        {
            var msg = new LogEventInfo(level, "", message ?? "");
            if (ex != null)
                msg.Exception = ex;
            logger.Error(msg);

        }
        public static void logInfo(Logger logger, LogLevel level, string message = null)
        {
            var msg = new LogEventInfo(level, "", message ?? "");
            logger.Info(msg);

        }

        public static Operatore getLoggedUser()
        {
            if (HttpContext.Current.Session[Settings.K_LOGGED_USER] == null)
                return null;
            return (Operatore)HttpContext.Current.Session[Settings.K_LOGGED_USER];
        }
    }
}