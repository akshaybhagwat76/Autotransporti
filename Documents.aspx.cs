using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;

public partial class admin_Documents : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtStart.Value = DateTime.Today.AddDays(-15).ToShortDateString();
                txtEnd.Value = DateTime.Today.ToShortDateString();
                loadData();
            }
        }catch(Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }
    private void loadData()
    {
        if (string.IsNullOrEmpty(txtStart.Value) || string.IsNullOrEmpty(txtEnd.Value))
            throw new Exception("Inserire un intervallo di date per visualizzare i documenti!");

        var source = Cliente.getList().SelectMany(x => x.DDT)
            .Where(x => x.Data >= Convert.ToDateTime(txtStart.Value) && x.Data <= Convert.ToDateTime(txtEnd.Value).AddDays(1))
            .OrderByDescending(x => x.Data)
            .ToList();
        rpDocuments.DataSource = source;
        rpDocuments.DataBind();
    }


    protected void lnkApplyFilter_Click(object sender, EventArgs e)
    {
        try
        {
            loadData();
        }catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }
}