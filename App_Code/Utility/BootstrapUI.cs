using System.Web.UI;
using System.Web;

namespace Utility
{
    public static class BootstrapUI
    {
        public static string redirectURL(string paramID, string result)
        {
            string redirect = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString()) ? string.Format("{0}?id={1}", HttpContext.Current.Request.RawUrl, paramID.ToString()) : HttpContext.Current.Request.RawUrl;
            if (HttpContext.Current.Request.QueryString["save"] == null)
                redirect += "&save=" + result;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString()) && HttpContext.Current.Request.QueryString["id"] == null)
                redirect += "&id=" + paramID.ToString();
            return redirect;
        }
        public static void showNotification(Control control, string title, string message, string type, string position)
        {
            ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "notification", string.Format("showNotification('{0}','{1}','{2}','{3}');", title, message.Replace("'", @"\'"), type, position), true);
        }
        public static void showNotification(Control control, string title, string message, string type, string position, bool pageLoad)
        {
            if (pageLoad)
                ScriptManager.RegisterStartupScript(control, control.GetType(), "notification", string.Format("showNotification('{0}','{1}','{2}','{3}');", title, message.Replace("'", @"\'"), type, position), true);
            else
                showNotification(control, title, message, type, position);
        }

        public static void triggerModal(Control control, string modalID, string action = "show", bool pageLoad = false)
        {
            if (pageLoad)
                ScriptManager.RegisterStartupScript(control, control.GetType(), "modal" + System.DateTime.Now.ToString("ddMMyyyyHHmmss"), string.Format("triggerModal('{0}','{1}');", modalID, action), true);
            else
                ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "modal" + System.DateTime.Now.ToString("ddMMyyyyHHmmss"), string.Format("triggerModal('{0}','{1}');", modalID, action), true);
        }
    }
}