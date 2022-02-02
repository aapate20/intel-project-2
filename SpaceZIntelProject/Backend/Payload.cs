using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    // Class to hold Payload Data for Spacecraft.
    public class Payload
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string PayloadStatus { get; set; }

        public Payload() { }

        public Payload(string Name, string Type, String Status, String PayloadStatus)
        {
            this.Name = Name;
            this.Type = Type;
            this.Status = Status;
            this.PayloadStatus = PayloadStatus;
        }
    }
}