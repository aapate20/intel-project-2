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
    [ServiceContract]
    public interface BackendServices
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
        void UpdateSpacecraft(string vehicleName, string column, string status);

        [OperationContract]
        List<Vehicle> GetAllOnlineSpacecraft();

        [OperationContract]
        Vehicle GetSpacecraft(string vehicleName);

        [OperationContract]
        List<Vehicle> GetAllOnlinePayload();
    }
}
