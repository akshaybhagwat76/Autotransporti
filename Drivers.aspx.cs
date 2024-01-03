using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;

public partial class admin_Drivers : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                loadData();
            }
        }catch(Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }
    private void loadData()
    {
        rpTable.DataSource = Autista.getList();
        rpTable.DataBind();
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string id = lnk.Attributes["idc"];

            Autista.delete(id);

            loadData();
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }


}