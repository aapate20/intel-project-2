using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Backend
{
    public interface IBackendCallback
    {
        [OperationContract]
        void ReceiveCommand(Vehicle vehicle, string command);

        [OperationContract]
        void SendCommand(Vehicle vehicle, string command);
    }
}