using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;
using System.Xml.Linq;

public partial class admin_Maintenance : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PageUtility.bindControls(new List<Control> { lnkSave });

                loadData();
            }
        }catch(Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }

    private void loadData()
    {
        rpTable.DataSource = Manutenzione.getList();
        rpTable.DataBind();
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string id = lnk.Attributes["idc"];

            Manutenzione.delete(id);

            loadData();
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }

    protected void lnkNew_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState.Remove("itemID");
            txtDescription.Value = "";
            txtKmFrom.Value = "";
            txtKmTo.Value = "";
            
            BootstrapUI.triggerModal(this, "modalEdit");
        }catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string id = lnk.Attributes["idc"];

            var item = Manutenzione.getDetail(id);
            if (item != null)
            {
                ViewState["itemID"] = id;
                txtDescription.Value = item.Descrizione;
                txtKmFrom.Value = item.KmDa.ToString();
                txtKmTo.Value = item.KmA.ToString();
                
                BootstrapUI.triggerModal(this, "modalEdit");
            }

        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }

    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {

            Manutenzione item = new Manutenzione();

            if (ViewState["itemID"] != null)
                item.Id = ViewState["itemID"].ToString();
            item.Descrizione = txtDescription.Value;
            item.KmDa = Convert.ToInt32(txtKmFrom.Value);
            item.KmA = Convert.ToInt32(txtKmTo.Value);

            item.insertOrUpdate();

            BootstrapUI.showNotification(this, "", "Salvataggio dei dati avvenuto con successo!", "success", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEdit", "hide");

            loadData();
        }
        catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEdit");
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }

}