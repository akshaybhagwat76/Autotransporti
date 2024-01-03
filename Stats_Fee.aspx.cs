using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;
using System.Globalization;

public partial class admin_Stats_Fee : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                bindDrop();
                dropMonth.SelectedValue = DateTime.Today.Month.ToString();
                dropYear.SelectedValue = DateTime.Today.Year.ToString();
                loadData();
            }
        }catch(Exception ex)
        {
            PageUtility.logError(LogManager.GetCurrentClassLogger(), LogLevel.Error, ex);
        }
    }
    private void bindDrop()
    {
        for (int idx = 1; idx <= 12; idx++)
            dropMonth.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(idx).ToUpper(), idx.ToString()));

        for (int idx= DateTime.Today.Year; idx >= DateTime.Today.Year - 5; idx--)
            dropYear.Items.Add(new ListItem(idx.ToString(), idx.ToString()));
    }
    private void loadData()
    {
        var start = Convert.ToDateTime(String.Format("01/{0}/{1}", dropMonth.SelectedValue, dropYear.SelectedValue));
        var end = Convert.ToDateTime(String.Format("{0}/{1}/{2}", DateTime.DaysInMonth(Convert.ToInt16(dropYear.SelectedValue), Convert.ToInt16(dropMonth.SelectedValue)), dropMonth.SelectedValue, dropYear.SelectedValue));
        var trips = Viaggio.getList(null, null, start, end.AddDays(1));
        var drivers = Autista.getList();
        var groupedTrips = trips.GroupBy(x => x.IdAutista).Select(s => new
        {
            driverId = s.Key,
            driver = string.Format("{0} {1}", drivers.First(x => x.Id == s.Key).Cognome, drivers.First(x => x.Id == s.Key).Nome),
            totale = s.Sum(c => c.Giornate.Sum(g => g.Tariffa))
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

            var start = Convert.ToDateTime(String.Format("01/{0}/{1}", dropMonth.SelectedValue, dropYear.SelectedValue));
            var end = Convert.ToDateTime(String.Format("{0}/{1}/{2}", DateTime.DaysInMonth(Convert.ToInt16(dropYear.SelectedValue), Convert.ToInt16(dropMonth.SelectedValue)), dropMonth.SelectedValue, dropYear.SelectedValue));
            var trips = Viaggio.getList(lnk.Attributes["did"], null, start, end.AddDays(1)).SelectMany(s => s.Giornate).OrderByDescending(x => x.Data);
            rpFee.DataSource = trips;
            rpFee.DataBind();

            litFeeTotalDetail.Text = trips.Sum(x => x.Tariffa).ToString("c");

            BootstrapUI.triggerModal(this, "modalDetail");
        }
        catch (Exception ex)
        {
            BootstrapUI.showNotification(this, "Attenzione", ex.Message, "error", "toast-top-center");
        }
    }

}