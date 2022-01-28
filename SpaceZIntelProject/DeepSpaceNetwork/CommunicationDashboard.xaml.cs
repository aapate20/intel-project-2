using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for CommunicationDashboard.xaml
    /// </summary>
    public partial class CommunicationDashboard : Window
    {

        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle vehicle;
        public CommunicationDashboard(BackendServiceReference.Vehicle vehicle)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            this.vehicle = vehicle;
            backendServicesClient = new BackendServiceReference.BackendServicesClient();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            spaceCraftName.Content = vehicle.Name + " " + "Communication Dashboard";
            payloadName.Content = vehicle.Payload.Name;
            payloadType.Content = vehicle.Payload.Type;
            if (Constants.STATUS_ADDED.Equals(vehicle.Payload.Status))
            {
                customPayloadBtn.Content = "Launch Payload";
            }
            else
            {
                customPayloadBtn.Content = "Payload Dashboard";
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void DeOrbit_Spacecraft(object sender, RoutedEventArgs e)
        {

        }
    }
}
