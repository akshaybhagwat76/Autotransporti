using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API
{
    public class InputDDT
    {
        public string DriverId { get; set; }
        public string CustomerId { get; set; }
        public string File { get; set; }
        public string FileExt { get; set; }

        public InputDDT()
        { }
    }
}