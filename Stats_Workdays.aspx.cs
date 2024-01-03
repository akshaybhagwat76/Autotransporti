using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;

public partial class admin_Stats_Workdays : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtStart.Value = String.Format("01/{0}/{1}", DateTime.Today.Month, DateTime.Today.Year);
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
            throw new Exception("Inserire un intervallo di date per visualizzare le statistiche!");

        var trips = Viaggio.getList(null, null, Convert.ToDateTime(txtStart.Value), Convert.ToDateTime(txtEnd.Value).AddDays(1));
        var drivers = Autista.getList();
        var groupedTrips = trips.GroupBy(x => x.IdAutista).Select(s => new
        {
            driverId = s.Key,
            driver = string.Format("{0} {1}", drivers.First(x => x.Id == s.Key).Cognome, drivers.First(x => x.Id == s.Key).Nome),
            partenze = s.Sum(c => c.Giornate.Count(x => x.Tipo == TipoGiornata.Partenza)),
            feriali = s.Sum(c => c.Giornate.Count(x => x.Tipo == TipoGiornata.Feriale)),
            festive = s.Sum(c => c.Giornate.Count(x => x.Tipo == TipoGiornata.Festiva)),
            festiveSpeciali = s.Sum(c => c.Giornate.Count(x => x.Tipo == TipoGiornata.FestivaSpeciale)),
            rientri = s.Sum(c => c.Giornate.Count(x => x.Tipo == TipoGiornata.Rientro)),
            km = s.Sum(c => c.Rifornimenti.Count() == 0 ? 0 : c.Rifornimenti.Last().Km - c.KmInizialiTarga)
        });
        
        rpStats.DataSource = groupedTrips;
        rpStats.DataBind();
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

    protected void lnkDetail_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            litDetailDriver.Text = lnk.Attributes["dname"];

            var trips = Viaggio.getList(lnk.Attributes["did"], null, Convert.ToDateTime(txtStart.Value), Convert.ToDateTime(txtEnd.Value).AddDays(1));
            rpDriverDetails.DataSource = trips;
            rpDriverDetails.DataBind();

            BootstrapUI.triggerModal(this, "modalDetail");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

    protected void rpDriverDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Viaggio data = (Viaggio)e.Item.DataItem;
            Literal litdays = (Literal)e.Item.FindControl("litWorkingDays");
            Literal litfestivi = (Literal)e.Item.FindControl("litWorkingFestivi");
            Literal litspeciali = (Literal)e.Item.FindControl("litWorkingFestiviSpeciali");
            Literal litrientro = (Literal)e.Item.FindControl("litWorkingRientro");
            Literal litfee = (Literal)e.Item.FindControl("litTotalFee");

            litdays.Text = data.Giornate.Count(x => x.Tipo == TipoGiornata.Feriale).ToString();
            litfestivi.Text = data.Giornate.Count(x => x.Tipo == TipoGiornata.Festiva).ToString();
            litspeciali.Text = data.Giornate.Count(x => x.Tipo == TipoGiornata.FestivaSpeciale).ToString();
            litrientro.Text = data.Giornate.Count(x => x.Tipo == TipoGiornata.Rientro).ToString();
            litfee.Text = data.Giornate.Sum(x => x.Tariffa).ToString("c");

        }
    }
}