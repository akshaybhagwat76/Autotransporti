using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;
using System.Xml.Linq;

public partial class admin_Users : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PageUtility.bindControls(new List<Control> { lnkSave });
                loadRoles();
                loadData();
            }
        }
        catch (Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }
    private void loadRoles()
    {
        dropRole.Items.Add(new ListItem(UserRole.Admin.ToString().ToUpper(), UserRole.Admin));
        dropRole.Items.Add(new ListItem(UserRole.Operatore.ToString().ToUpper(), UserRole.Operatore));

    }
    private void loadData()
    {
        rpTable.DataSource = Operatore.getList();
        rpTable.DataBind();
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string id = lnk.Attributes["idc"];

            Operatore.delete(id);

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
            ViewState.Remove("password");
            txtName.Value = "";
            txtEmail.Value = "";
            txtUsername.Value = "";
            txtPassword.Value = PageUtility.RandomString(new Random(), 8, true);
            dropRole.SelectedValue = UserRole.Admin;

            BootstrapUI.triggerModal(this, "modalEdit");
        }
        catch (Exception ex)
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

            var item = Operatore.getDetail(id);
            if (item != null)
            {
                ViewState["itemID"] = id;
                ViewState["password"] = item.Password;
                txtName.Value = item.Name;
                txtEmail.Value = item.Email;
                txtUsername.Value = item.Username;
                txtPassword.Value = Crypto.Decrypt(item.Password, true);
                dropRole.SelectedValue = item.Role;

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
            if (!checkForm())
            {
                BootstrapUI.triggerModal(this, "modalEdit");
                return;
            }


            Operatore item = new Operatore();

            if (ViewState["itemID"] != null)
                item.Id = ViewState["itemID"].ToString();
            item.Name = txtName.Value;
            item.Username = txtUsername.Value;
            item.Email = txtEmail.Value;
            item.Password = ViewState["password"] != null ? ViewState["password"].ToString() : Crypto.Encrypt(txtPassword.Value, true);
            item.Role = dropRole.SelectedValue;
            item.insertOrUpdate();

            BootstrapUI.showNotification(this, "", "Salvataggio dei dati avvenuto con successo!", "success", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEdit", "hide");

            loadData();
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEdit");
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }

    private bool checkForm()
    {
        bool result = true;
        string message = "";
        var list = Operatore.getList();
        string userID = ViewState["itemID"] == null ? null : ViewState["itemID"].ToString();
        var f = list.Where(x => x.Email == txtEmail.Value && (userID == null || x.Id != userID)).FirstOrDefault();
        if (f != null)
        {
            result = false;
            message += "- L'indirizzo email inserito è già in uso";
        }
        f = list.Where(x => x.Username == txtUsername.Value && (userID == null || x.Id != userID)).FirstOrDefault();
        if (f != null)
        {
            result = false;
            message += "<br>- Lo username inserito è già in uso";
        }
        if (!string.IsNullOrEmpty(message))
            BootstrapUI.showNotification(this, "Attenzione", message, "error", "toast-top-center");
        return result;
    }

}