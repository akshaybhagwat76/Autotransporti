using System;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Data;
using Utility;

/// <summary>
/// Summary description for EmailHandler
/// </summary>
public class EmailHandler
{
    NetworkCredential Credenziali;
    SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["EmailSMTP"].ToString());
    string Email = ConfigurationManager.AppSettings["EmailMainBox"].ToString();
    string EmailAdmin = ConfigurationManager.AppSettings["EmailAdmin"].ToString();
    string SiteHost = ConfigurationManager.AppSettings["SiteHost"].ToString();
    StreamReader reader = null;

    public EmailHandler()
    {
        Credenziali = new NetworkCredential(ConfigurationManager.AppSettings["EmailSMTPuser"].ToString(), ConfigurationManager.AppSettings["EmailSMTPpwd"].ToString());
        smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailSMTPport"]);
        smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailSMTPssl"]);
        //******* Disabilitato per aruba
        smtp.Credentials = Credenziali;
    }

    

    private void ReportErrorToAdmin(Exception ex, MailMessage mail)
    {
        try
        {
            MailMessage msg = new MailMessage();
            NetworkCredential credenziali = (NetworkCredential)smtp.Credentials;
            string innerExc = "";
            if (ex.InnerException != null)
                innerExc = ex.InnerException.Message;

            string body = "Errore nell'invio dell'email:\r\n\r\nMessaggio: " + ex.Message +
                "\r\n\r\nStack Trace: " + ex.StackTrace + "\r\n\r\n" + "InnerExecption: " + innerExc +
                "\r\n\r\nParametri dell'email:\r\n\r\nFrom: " + mail.From.Address + "\r\n\r\nTo: " + mail.To[0].Address + "\r\n\r\nSmtp Host: " + smtp.Host + " - Porta: " + smtp.Port.ToString() +
                " - SSL: " + smtp.EnableSsl.ToString() + " - User: " + credenziali.UserName + " - Pwd: " + credenziali.Password;

            SmtpClient SmtpAdmin = new SmtpClient("mail.appgear.it");
            NetworkCredential CredenzialiAdmin = new NetworkCredential("postmaster@appgear.it", "Vge17Dar");
            SmtpAdmin.Port = 25;
            SmtpAdmin.EnableSsl = false;
            SmtpAdmin.Credentials = CredenzialiAdmin;

            msg.From = new MailAddress("postmaster@appgear.it");
            msg.To.Add(new MailAddress(EmailAdmin));
            msg.Subject = "Errore invio email " + SiteHost;
            msg.Body = body;

            SmtpAdmin.Send(msg);
        }
        catch (Exception newex)
        {

        }
    }
    public void ReportErrorToAdmin(Exception ex)
    {
        try
        {
            MailMessage msg = new MailMessage();

            string body = "Errore nell'invio dell'email:\r\n\r\nMessaggio: " + ex.Message +
                "\r\n\r\nStack Trace: " + ex.StackTrace + "\r\n\r\n";
            if (ex.InnerException != null)
            {
                body += "InnerExecption: " + ex.InnerException.Message;
            }

            SmtpClient SmtpAdmin = new SmtpClient("mail.appgear.it");
            NetworkCredential CredenzialiAdmin = new NetworkCredential("postmaster@appgear.it", "Vge17Dar");
            SmtpAdmin.Port = 25;
            SmtpAdmin.EnableSsl = false;
            SmtpAdmin.Credentials = CredenzialiAdmin;

            msg.From = new MailAddress("postmaster@appgear.it");
            msg.To.Add(new MailAddress(EmailAdmin));
            msg.Subject = "Errore portale " + SiteHost;
            msg.Body = body;

            SmtpAdmin.Send(msg);
        }
        catch (Exception newex)
        {

        }
    }
}