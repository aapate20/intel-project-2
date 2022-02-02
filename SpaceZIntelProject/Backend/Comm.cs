using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    // Class to store Communication Data for Payload type comunication.
    public class Comm
    {
        public int Uplink { get; set; }

        public int Downlink { get; set; }

        public int ActiveTransponder { get; set; }

    }
}