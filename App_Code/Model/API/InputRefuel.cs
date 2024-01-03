using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API
{
    public class InputRefuel
    {
        public string JourneyId { get; set; }
        public int Km { get; set; }
        public decimal Lt { get; set; }
        public decimal? Cost { get; set; }
        public InputRefuel()
        { }
    }
}