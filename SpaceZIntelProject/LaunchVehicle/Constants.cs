﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchVehicle
{
    public class Constants
    {
        public const string STATUS_LAUNCHED = "Launched";
        public const string STATUS_LAUNCH_INITIATED = "Launch Initiated";
        public const string STATUS_AIR = "Air";
        public const string STATUS_DEORBIT = "Deorbit";
        public const string STATUS_DECOMMISSION = "Decommission";
        public const string STATUS_ADDED = "Added";
        public const string STATUS_MISSION_ABORTED = "Mission Aborted";
        public const string STATUS_ORBIT_REACHED = "Orbit Reached";
        public const string COLUMN_STATUS = "Status";
        public const string COLUMN_PAYLOAD_STATUS = "Payload.Status";
        public const string COLUMN_PAYLOAD_PAYLOAD_STATUS = "Payload.PayloadStatus";
        public const string STATUS_ONLINE = "Online";
        public const string STATUS_OFFLINE = "Offline";
        public const string COLUMN_SPACECRAFT_STATUS = "SpacecraftStatus";
        public const string LAUNCH_VEHICLE_DIRECTORY = "LaunchVehicle\\bin\\Debug\\LaunchVehicle.exe";
    }
}
