using PayloadSystem.BackendServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayloadSystem
{
    public class Callback : BackendServiceReference.IBackendServicesCallback
    {

        void IBackendServicesCallback.ReceiveCommand(Vehicle vehicle, string command)
        {
            throw new NotImplementedException();
        }

        void IBackendServicesCallback.SendCommand(Vehicle vehicle, string command)
        {
            throw new NotImplementedException();
        }
    }
}
