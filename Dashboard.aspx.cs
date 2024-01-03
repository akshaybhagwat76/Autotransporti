using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                loadStats();
            }
        }catch(Exception ex)
        {

        }
    }

    private void loadStats()
    {
        var trips = Viaggio.getList();
        var validTrips = trips.Where(x => x.Rifornimenti != null).ToList();

        //************ Consumo totale
        var refuels = validTrips.SelectMany(x => x.Rifornimenti).ToList();
        var totaLiter = refuels.Sum(x => x.LtCarburante);
        var totalKm = 0;
        foreach (var trip in validTrips)
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

        spTripCounter.InnerText = trips.Count.ToString();
    }
}