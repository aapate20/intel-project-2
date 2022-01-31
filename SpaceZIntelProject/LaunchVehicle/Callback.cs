using LaunchVehicle.BackendServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaunchVehicle
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Callback : BackendServiceReference.IBackendServicesCallback
    {

        public void ReceiveCommand(Vehicle vehicle, string command)
        {
            ((MainWindow)Application.Current.MainWindow).UpdateCommunicationBoard(command, vehicle.Name);
        }

        public void SendCommand(Vehicle vehicle, string command)
        {
            throw new NotImplementedException();
        }
    }
}
