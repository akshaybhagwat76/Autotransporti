
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

public partial class DriversEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageUtility.bindControls(new List<Control>() { lnkSave, lnkSaveNote });
            if (!IsPostBack)
            {
                bindFees();
                txtPassword.Value = PageUtility.RandomString(new Random(), 8, true);

                if (Request.QueryString["id"] != null)
                    loadData();

                if (Request.QueryString["s"] != null)
                    BootstrapUI.showNotification(this, "", "Dati correttamente salvati!", "success", "toast-top-center", true);
            }
        }
        catch (Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }

    private void bindFees()
    {
        List<Tariffa> source = new List<Tariffa>()
        {
            new Tariffa(){ Tipo = TipoGiornata.Partenza},
            new Tariffa(){ Tipo = TipoGiornata.Feriale},
            new Tariffa(){ Tipo = TipoGiornata.Festiva},
            new Tariffa(){ Tipo = TipoGiornata.FestivaSpeciale},
            new Tariffa(){ Tipo = TipoGiornata.Rientro}
        };
        rpFee.DataSource = source;
        rpFee.DataBind();
    }

    private void loadData()
    {
        var item = Autista.getDetail(Request.QueryString["id"]);
        if (item == null)
        {
            Response.Redirect("~/Drivers.aspx");
            return;
        }

        litPageTitle.Text = String.Format("{0} {1}", item.Cognome, item.Nome);
        txtName.Value = item.Nome;
        txtSurname.Value = item.Cognome;
        txtPhone.Value = item.Telefono;
        txtUsername.Value = item.Username;
        txtUsername.Disabled = true;
        txtPassword.Value = Crypto.Decrypt(item.Password, true);
        chkEnabled.Checked = item.Abilitato;

        rpFee.DataSource = item.Tariffe;
        rpFee.DataBind();

        bindNotes(item.Note);
        lnkAddNote.Visible = true;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] == null && Autista.isUsernameUsed(txtUsername.Value))
                throw new Exception("Username già utilizzato per un altro account!");

            var item = Request.QueryString["id"] == null ? new Autista() : Autista.getDetail(Request.QueryString["id"]);
            item.Nome = txtName.Value;
            item.Cognome = txtSurname.Value;
            item.Telefono = txtPhone.Value;
            item.Username = txtUsername.Value;
            item.Password = Crypto.Encrypt(txtPassword.Value, true);
            item.Abilitato = chkEnabled.Checked;

            var fee = new List<Tariffa>();
            foreach (RepeaterItem rpitem in rpFee.Items)
            {
                HtmlInputText txt = (HtmlInputText)rpitem.FindControl("txtValue");
                fee.Add(new Tariffa() { Tipo = txt.Attributes["tp"], Valore = Convert.ToDecimal(txt.Value) });
            }
            item.Tariffe = fee;

            item.insertOrUpdate();

            if (Request.QueryString["id"] == null)
                Response.Redirect("~/DriversEdit.aspx?s=1&id=" + item.Id);
            else
                BootstrapUI.showNotification(this, "", "Dati correttamente salvati!", "success", "toast-top-center");

        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    #region Note
    private void bindNotes(List<Nota> source)
    {
        if (source != null)
            source = source.OrderByDescending(x => x.Data).ToList();
        rpNotes.DataSource = source;
        rpNotes.DataBind();
    }
    protected void lnkDeleteNote_Click(object sender, EventArgs e)
    {
        try
        {
            var idx = Convert.ToInt16(((LinkButton)sender).Attributes["idx"]);
            var item = Autista.getDetail(Request.QueryString["id"]);
            item.Note.RemoveAt(idx);
            item.insertOrUpdate();

            bindNotes(item.Note);
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    protected void lnkEditNote_Click(object sender, EventArgs e)
    {
        try
        {
            var idx = Convert.ToInt16(((LinkButton)sender).Attributes["idx"]);
            var item = Autista.getDetail(Request.QueryString["id"]).Note[idx];

            txtNoteText.Value = item.Testo;
            litNoteDate.Text = String.Format("Registrata il {0}", item.Data.ToLocalTime().ToString("dd.MM.yyyy HH:mm"));
            chkNotePublic.Checked = item.Pubblica;
            ViewState["editingNoteIdx"] = idx;

            BootstrapUI.triggerModal(this, "modalEditNote");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    protected void lnkAddNote_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState.Remove("editingNoteIdx");
            txtNoteText.Value = "";
            chkNotePublic.Checked = false;
            litNoteDate.Text = "";

            BootstrapUI.triggerModal(this, "modalEditNote");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }
    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {
        try
        {
            var item = Autista.getDetail(Request.QueryString["id"]);
            if (ViewState["editingNoteIdx"] == null)
            {
                if (item.Note == null)
                    item.Note = new List<Nota>();
                item.Note.Add(new Nota() { Data = DateTime.Now, Pubblica = chkNotePublic.Checked, Testo = txtNoteText.Value });
            }
            else
            {
                var note = item.Note[Convert.ToInt16(ViewState["editingNoteIdx"])];
                note.Testo = txtNoteText.Value;
                note.Pubblica = chkNotePublic.Checked;
            }
            item.insertOrUpdate();

            bindNotes(item.Note);

            BootstrapUI.triggerModal(this, "modalEditNote", "hide");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEditNote");
        }
    }
    #endregion


}