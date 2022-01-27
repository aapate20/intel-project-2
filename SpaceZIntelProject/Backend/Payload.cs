using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    public class Payload
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Payload() { }
        public Payload(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }
}