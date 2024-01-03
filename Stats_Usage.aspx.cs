using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using NLog;
using System.Globalization;

public partial class admin_Stats_Usage : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtStart.Value = DateTime.Today.AddDays(-30).ToShortDateString();
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
        var trips = Viaggio.getList();
        var validTrips = trips.Where(x => x.Rifornimenti != null).ToList();

        //************ Consumo totale
        var refuels = validTrips.SelectMany(x => x.Rifornimenti).ToList();
        var totaLiter = refuels.Sum(x => x.LtCarburante);
        var totalKm = 0;
        foreach(var trip in validTrips)
        {
            var lastkm = trip.KmInizialiTarga;
            foreach (var r in trip.Rifornimenti)
            {
                totalKm += r.Km - lastkm;
                lastkm = r.Km;
            }
        }
        var average = totaLiter * 100 / totalKm;
        spTotalAverage.InnerText = Math.Round(average, 2).ToString();

        //*********** Consumi del periodo
        validTrips = trips.Where(x => x.Inizio >= Convert.ToDateTime(txtStart.Value) && x.Inizio <= Convert.ToDateTime(txtEnd.Value).AddDays(1) && x.Rifornimenti != null).ToList();
        refuels = validTrips.SelectMany(x => x.Rifornimenti).ToList();
        totaLiter = refuels.Sum(x => x.LtCarburante);
        totalKm = 0;
        foreach (var trip in validTrips)
        {
            var lastkm = trip.KmInizialiTarga;
            foreach (var r in trip.Rifornimenti)
            {
                totalKm += r.Km - lastkm;
                lastkm = r.Km;
            }
        }
        average = totaLiter * 100 / totalKm;
        spPeriodAverage.InnerText = Math.Round(average, 2).ToString();

        //*********** Report autisti
        var drivers = Autista.getList();
        List<dynamic> list = new List<dynamic>();
        foreach(var g in drivers)
        {
            var f = validTrips.Where(x => x.IdAutista == g.Id);
            refuels = f.SelectMany(x => x.Rifornimenti).ToList();
            totaLiter = refuels.Sum(x => x.LtCarburante);
            totalKm = 0;
            foreach (var trip in f)
            {
                var lastkm = trip.KmInizialiTarga;
                foreach (var r in trip.Rifornimenti)
                {
                    totalKm += r.Km - lastkm;
                    lastkm = r.Km;
                }
            }
            list.Add(new
            {
                driverId = g.Id,
                km = totalKm,
                driver = string.Format("{0} {1}", g.Cognome, g.Nome),
                average = Math.Round(totaLiter * 100 / totalKm, 2)
            });
        }
        rpStats.DataSource = list.ToList().OrderBy(x => x.average);
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

}