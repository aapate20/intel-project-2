using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    // Class to store Weather Data for Payload type Scientific.
    public class Weather
    {
        public int Rainfall { get; set; }

        public int Humidity { get; set; }

        public int Snow { get; set; }
    }
}