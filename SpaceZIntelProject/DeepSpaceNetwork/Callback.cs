using DeepSpaceNetwork.BackendServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeepSpaceNetwork
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Callback : BackendServiceReference.IBackendServicesCallback
    {
        public void ReceiveCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
