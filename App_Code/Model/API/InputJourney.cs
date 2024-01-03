using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API
{
    public class InputJourney
    {
        public string PlateCode { get; set; }
        public int PlateKm { get; set; }
        public string TrailerCode { get; set; }
        public int TrailerKm { get; set; }
        public string DriverId { get; set; }
        public InputJourney()
        { }
    }
}