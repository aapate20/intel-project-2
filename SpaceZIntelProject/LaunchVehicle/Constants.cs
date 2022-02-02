using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchVehicle
{
    // This file contains all the constants used in this project.
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
        public const string SERVICE_END_POINT = "CommunicationServiceEndpoint";

        public const string RECEIVE_COMMAND = "DSN: ";
        public const string SEND_COMMAND = "YOU: ";
        public const string START_TELEMETRY = "Start Telemetry";
        public const string STOP_TELEMETRY = "Stopping Telemetry";
        public const string SENDING_TELEMETRY = "Sending Telemetry";
        public const string LAUNCH_PAYLOAD = "Launch Payload";
        public const string LAUNCHING_PAYLOAD = "Launching Payload";
        public const string LAUNCH_PAYLOAD_SUCCSS = "Launched Payload Successfully.";
        public const string COMMAND_FAILURE = "Failure";
        public const string DEORBIT_VEHICLE = "Deorbit Payload";
        public const string DEORBITING_VEHICLE = "Deorbiting";
    }
}
