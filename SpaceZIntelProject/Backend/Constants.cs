using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    // This file contains all the constants used in this project.
    public class Constants
    {
        public const string DATABASE_SPACEZ = "SpaceZ";
        public const string COLLECTION_SPACECRAFT = "Spacecraft";
        public const string STATUS_LAUNCH_INITIATED = "Launch Initiated";
        public const string STATUS_LAUNCHED = "Launched";
        public const string STATUS_ONLINE = "Online";
        public const string STATUS_OFFLINE = "Offline";
        public const string STATUS_DEORBIT = "Deorbit";
        public const string STATUS_DECOMMISSION = "Decommission";
        public const string STATUS_ADDED = "Added";
        public const string COLUMN_STATUS = "Status";
        public const string COLUMN_SPACECRAFT_STATUS = "SpacecraftStatus";
        public const string COLUMN_NAME = "Name";
        public const string COLUMN_PAYLOAD_NAME = "Payload.Name";
        public const string COLUMN_PAYLOAD_STATUS = "Payload.Status";
        public const string COLUMN_PAYLOAD_PAYLOAD_STATUS = "Payload.PayloadStatus";
    }
}