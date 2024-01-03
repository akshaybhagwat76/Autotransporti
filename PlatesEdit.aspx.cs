
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

public partial class PlatesEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PageUtility.bindControls(new List<Control>() { lnkSave, lnkSaveTyre });
            if (!IsPostBack)
            {
                bindTypes();

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
    private void bindTypes()
    {
        dropType.Items.Add(new ListItem(TipoMezzo.Motrice, TipoMezzo.Motrice));
        dropType.Items.Add(new ListItem(TipoMezzo.Bilico, TipoMezzo.Bilico));
        dropType.Items.Add(new ListItem(TipoMezzo.Rimorchio, TipoMezzo.Rimorchio));
    }
    private void loadData()
    {
        var item = Targa.getDetail(Request.QueryString["id"]);
        if (item == null)
        {
            Response.Redirect("~/Plates.aspx");
            return;
        }

        litPageTitle.Text = item.Codice;
        txtCode.Value = item.Codice;
        txtCode.Disabled = true;
        txtKm.Value = item.KmTotali.ToString();
        dropType.SelectedValue = item.TipoMezzo;
        chkEnabled.Checked = item.Attiva;

        lnkAddAxis.Visible = true;
        bindAxis(item.Assi);
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] == null && Targa.isCodeUsed(txtCode.Value))
                throw new Exception("Questa targa risultà già registrata!");

            var item = Request.QueryString["id"] == null ? new Targa() : Targa.getDetail(Request.QueryString["id"]);
            item.Codice = txtCode.Value;
            item.KmTotali = Convert.ToInt32(txtKm.Value);
            item.TipoMezzo = dropType.SelectedValue;
            item.Attiva = chkEnabled.Checked;

            item.insertOrUpdate();

            if (Request.QueryString["id"] == null)
                Response.Redirect("~/PlatesEdit.aspx?s=1&id=" + item.Id);
            else
                BootstrapUI.showNotification(this, "", "Dati correttamente salvati!", "success", "toast-top-center");

        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    #region Assi
    private void bindAxis(List<Asse> source)
    {
        rpAxis.DataSource = source;
        rpAxis.DataBind();
    }
    protected void lnkDeleteAxis_Click(object sender, EventArgs e)
    {
        try
        {
            var idx = Convert.ToInt16(((LinkButton)sender).Attributes["idx"]);
            var item = Targa.getDetail(Request.QueryString["id"]);
            item.Assi.RemoveAt(idx);
            item.insertOrUpdate();

            bindAxis(item.Assi);
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    protected void lnkAddAxis_Click(object sender, EventArgs e)
    {
        try
        {
            dropAxisTyre.SelectedValue = "1";

            BootstrapUI.triggerModal(this, "modalNewAxis");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }
    protected void lnkConfirmNewAxis_Click(object sender, EventArgs e)
    {
        try
        {
            var item = Targa.getDetail(Request.QueryString["id"]);
            if (item.Assi == null)
                item.Assi = new List<Asse>();

            var tyres = new List<Pneumatico>();
            for (int idx = 0; idx <= (Convert.ToInt32(dropAxisTyre.SelectedValue) * 2) - 1; idx++)
                tyres.Add(new Pneumatico() { Posizione = idx});
            item.Assi.Add(new Asse() { Pneumatici = tyres});

            item.insertOrUpdate();

            bindAxis(item.Assi);

            BootstrapUI.triggerModal(this, "modalNewAxis", "hide");
            
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalNewAxis");
        }
    }
    #endregion

    #region Tyres
    protected void lnkTyreDetail_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            var idxAxis = Convert.ToInt16(lnk.Attributes["idaxis"]);
            var idx = Convert.ToInt16(lnk.Attributes["idx"]);

            var item = Targa.getDetail(Request.QueryString["id"]);
            var tyre = item.Assi[idxAxis].Pneumatici[idx];


            txtTyreBrand.Value = tyre.Marca;
            txtTyreKm.Value = tyre.KmTotali.ToString();
            chkEnabled.Checked = tyre.InFunzione;

            hdnAxisIdx.Value = idxAxis.ToString();
            hdnTyreIdx.Value = idx.ToString();

            BootstrapUI.triggerModal(this, "modalEditTyre");

        }catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }
    protected void lnkSaveTyre_Click(object sender, EventArgs e)
    {
        try
        {
            var item = Targa.getDetail(Request.QueryString["id"]);
            var tyre = item.Assi[Convert.ToInt16(hdnAxisIdx.Value)].Pneumatici[Convert.ToInt16(hdnTyreIdx.Value)];

            tyre.Marca = txtTyreBrand.Value;
            tyre.KmTotali = Convert.ToInt32(txtTyreKm.Value);
            tyre.InFunzione = chkTyreActive.Checked;

            item.insertOrUpdate();
            bindAxis(item.Assi);

            BootstrapUI.triggerModal(this, "modalEditTyre", "hide");

        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
            BootstrapUI.triggerModal(this, "modalEditTyre");
        }
    }
    protected void lnkUpdateTyrePosition_Click(object sender, EventArgs e)
    {
        Targa item = null;
        try
        {
            item = Targa.getDetail(Request.QueryString["id"]);
            var draggablePos = hdnDragTyre.Value.Split(new char[] { '|' });
            var droppablePos = hdnDropTyre.Value.Split(new char[] { '|' });
            int dragAxis = Convert.ToInt32(draggablePos[0]), dragTyre = Convert.ToInt32(draggablePos[1]);
            int dropAxis = Convert.ToInt32(droppablePos[0]), dropTyre = Convert.ToInt32(droppablePos[1]);

            var draggableTyre = item.Assi[dragAxis].Pneumatici[dragTyre];
            draggableTyre.Posizione = dropTyre;
            var droppableTyre = item.Assi[dropAxis].Pneumatici[dropTyre];
            droppableTyre.Posizione = dragTyre;

            if (dragAxis != dropAxis)
            {
                item.Assi[dragAxis].Pneumatici.RemoveAt(dragTyre);
                item.Assi[dropAxis].Pneumatici.RemoveAt(dropTyre);
                item.Assi[dropAxis].Pneumatici.Insert(dropTyre, draggableTyre);
                item.Assi[dragAxis].Pneumatici.Insert(dragTyre, droppableTyre);
            }
            else
            {
                Swap(item.Assi[dragAxis].Pneumatici, dragTyre, dropTyre);
                //item.Assi[dragAxis].Pneumatici.RemoveAt(dragTyre);
                //item.Assi[dropAxis].Pneumatici.Insert(dropTyre, draggableTyre);
            }

            item.insertOrUpdate();

            bindAxis(item.Assi);
        }
        catch(Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
        finally { bindAxis(item.Assi); }
    }
    #endregion



    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
