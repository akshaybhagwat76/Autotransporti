using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using Newtonsoft.Json;
using Utility;


public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Operatore loggedUser = PageUtility.getLoggedUser();
        if (loggedUser == null)
            Response.Redirect("~/Default.aspx");

        try
        {
            if (!IsPostBack)
            {
                litCurrentUser.Text = loggedUser.Name;

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Remove(Settings.K_LOGGED_USER);
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center", true);
        }
        
    }

}
