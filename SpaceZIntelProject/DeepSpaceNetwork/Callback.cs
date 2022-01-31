using DeepSpaceNetwork.BackendServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceNetwork
{
    public class Callback : BackendServiceReference.IBackendServicesCallback
    {
        public void ReceiveCommand(Vehicle vehicle, string command)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(Vehicle vehicle, string command)
        {
            throw new NotImplementedException();
        }
    }
}
