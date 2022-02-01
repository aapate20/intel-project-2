using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Backend
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "BackendServices" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IBackendCallback))]
    public interface IBackendServices
    {
        [OperationContract]
        string AddSpaceCraft(Vehicle vehicle);

        [OperationContract]
        List<Vehicle> GetAllSpacecraft();

        [OperationContract]
        long CheckSpacecraftExists(string vehicleName, string payloadName);

        [OperationContract]
        List<Vehicle> GetAddedSpacecraft();

        [OperationContract]
        void UpdateSpacecraft1(string vehicleName, string column, string status);

        [OperationContract]
        void UpdateSpacecraft2(string vehicleName, string column1, string value1, string column2, string value2);

        [OperationContract]
        void UpdateSpacecraft3(string vehicleName, string column1, string value1, string column2, string value2, string column3, string value3);

        [OperationContract]
        void LaunchSpacecraft(string vehicleName, string dsnDashboardName);

        [OperationContract]
        List<Vehicle> GetAllOnlineSpacecraft();

        [OperationContract]
        Vehicle GetSpacecraft(string vehicleName);

        [OperationContract]
        List<Vehicle> GetAllOnlinePayload();

        [OperationContract]
        void UpdateTelemetryMap(string vehicleName, Telemetry telemetry);

        [OperationContract]
        Telemetry GetTelemetryOfVehicle(string vehicleName);
        
        [OperationContract]
        void UpdateCommMap(string vehicleName, Comm comm);

        [OperationContract]
        Comm GetCommDataOfVehicle(string vehicleName);
        
        [OperationContract]
        void UpdateWeatherMap(string vehicleName, Weather weather);

        [OperationContract]
        Weather GetWeatherDataOfVehicle(string vehicleName);
        
        [OperationContract]
        void UpdateSpyMap(string vehicleName, Spy spy);

        [OperationContract]
        Spy GetSpyDataOfVehicle(string vehicleName);

        [OperationContract]
        void SendCommandToVehicle(Vehicle vehicle, string command);

        [OperationContract]
        void SendCommandToPayloadVehicle(string vehicleName, string command);

        [OperationContract]
        void ConnectToBackend(string vehicleName);

        [OperationContract]
        void DisconnectFromBackend(string vehicleName);
    }
}
