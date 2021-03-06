using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend
{
    // Class to store Spacecraft Data.
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public double OrbitRadius { get; set; }

        public Payload Payload { get; set; }

        public string Status { get; set; }

        public string SpacecraftStatus { get; set; }

        public Vehicle() { }

        public Vehicle(string Name, int OrbitRadius, Payload Payload, string Status, string SpacecraftStatus)
        {
            this.Name = Name;
            this.OrbitRadius = OrbitRadius; 
            this.Payload = Payload; 
            this.Status = Status;
            this.SpacecraftStatus = SpacecraftStatus;
        }
    }
}