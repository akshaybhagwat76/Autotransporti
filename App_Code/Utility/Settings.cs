using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Utility
{
    public static class Settings
    {
        public static string SiteHost = ConfigurationManager.AppSettings["SiteHost"].ToString();
        public static string RecaptchaSiteKey = ConfigurationManager.AppSettings["GoogleCaptchaSiteKey"].ToString();
        public static string ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString();

        public static readonly string K_LOGGED_USER = "kLoggedUser";

        public static string getMimeTypeForExtension(string extension)
        {
            switch (extension)
            {
                case "pdf":
                    return "application/pdf";
                case "xml":
                    return "application/xml";
                case "zip":
                    return "application/octet-stream";
                case "doc":
                    return "application/msword";
                case "xls":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.ms-excel";//"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "docx":
                    return "application/msword";
                case "csv":
                    return "text/csv";

            }
            return "application/octet-stream";
        }
    }

    public static class UserRole
    {
        public static readonly string Superadmin = "superadmin";
        public static readonly string Admin = "admin";
        public static readonly string Operatore = "operatore";
    }

    public static class TipoMezzo
    {
        public static readonly string Motrice = "Motrice";
        public static readonly string Bilico = "Bilico";
        public static readonly string Rimorchio = "Rimorchio";
    }

    public static class TipoGiornata
    {
        public static readonly string Feriale = "Feriale";
        public static readonly string Festiva = "Festiva";
        public static readonly string FestivaSpeciale = "Festiva speciale";
        public static readonly string Partenza = "Partenza";
        public static readonly string Rientro = "Rientro";
    } 
}