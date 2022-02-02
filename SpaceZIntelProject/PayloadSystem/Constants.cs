using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadSystem
{
    // This file contains all the constants used in this project.
    public class Constants
    {
        public const string SERVICE_END_POINT = "CommunicationServiceEndpoint";
        public const string PAYLOAD_TYPE_SCIENTIFIC = "Scientific";
        public const string PAYLOAD_TYPE_COMMUNICATION = "Communication";
        public const string PAYLOAD_TYPE_SPY = "Spy";
        public const string RECEIVE_COMMAND = "DSN: ";
        public const string SEND_COMMAND = "YOU: ";
        public const string SENDING_TELEMETRY = "Sending Telemetry";
        public const string COMMAND_FAILURE = "Failure";
        public const string START_TELEMETRY = "Start Telemetry";
        public const string STOP_TELEMETRY = "Stop Telemetry";
        public const string START_DATA = "Request Data";
        public const string STARTING_DATA = "Sending Data";
        public const string STOP_DATA = "Stop Data Loading";
        public const string STOPIING_DATA = "Terminating Data Transfer";
    }
}
