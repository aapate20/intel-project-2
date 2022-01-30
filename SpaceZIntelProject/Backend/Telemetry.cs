using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    public class Telemetry
    {
        public double Altitude { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Temperature { get; set; }

        public int TimeToOrbit { get; set; }

        public Telemetry()
        {
            Altitude = 0;
            Longitude = 0; 
            Latitude = 0;
            Temperature = 340;
            TimeToOrbit = 0;
        }
    }
}