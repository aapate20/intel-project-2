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
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle vehicle;
        public CommunicationDashboard(BackendServiceReference.Vehicle vehicle)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            this.vehicle = vehicle;
            this.backendServicesClient = new BackendServiceReference.BackendServicesClient();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Configured_Dashboard();            
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        private void Configured_Dashboard()
        {
            try
            {
                this.spaceCraftName.Content = vehicle.Name + " " + "Communication Dashboard";
                this.payloadName.Content = vehicle.Payload.Name;
                this.payloadType.Content = vehicle.Payload.Type;
                if ((Constants.STATUS_LAUNCH_INITIATED.Equals(vehicle.Payload.Status) && Constants.STATUS_OFFLINE.Equals(vehicle.Payload.PayloadStatus))
                    || Constants.STATUS_ADDED.Equals(vehicle.Payload.Status) || Constants.STATUS_AIR.Equals(vehicle.Payload.Status))
                {
                    this.customPayloadBtn.Content = Constants.LAUNCH_PAYLOAD;
                }
                else if (Constants.STATUS_ONLINE.Equals(vehicle.Payload.PayloadStatus))
                {
                    this.customPayloadBtn.Content = Constants.PAYLOAD_DASHBOARD;
                }
                else
                {
                    this.customPayloadBtn.Visibility = Visibility.Hidden;
                }

                if(Constants.STATUS_OFFLINE.Equals(this.vehicle.SpacecraftStatus) || Constants.STATUS_DEORBIT.Equals(this.vehicle.Status))
                {
                    this.startTelemetry.Visibility = Visibility.Hidden;
                    this.stopTelemetry.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception ex)
            {
                log.Error("CommunicationDashboard(), Did not receive Vehicle object.", ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeOrbit_Spacecraft(object sender, RoutedEventArgs e)
        {
            try
            {
                this.vehicle = backendServicesClient.GetSpacecraft(this.vehicle.Name);
                backendServicesClient.UpdateSpacecraft(this.vehicle.Name, Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_OFFLINE);
                backendServicesClient.UpdateSpacecraft(this.vehicle.Name, Constants.COLUMN_STATUS, Constants.STATUS_DEORBIT);

            }
            catch(Exception ex) {
                log.Error("DeOrbit_Spacecraft(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Custom_Payload_Function(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Constants.LAUNCH_PAYLOAD.Equals(this.customPayloadBtn.Content))
                {
                    this.launchPayload();
                }
                else if (Constants.PAYLOAD_DASHBOARD.Equals(this.customPayloadBtn.Content))
                {
                    var payloadCommunicationDashboard = new PayloadCommunicationDashboard(this.vehicle.Payload);
                    payloadCommunicationDashboard.Show();
                }
                else
                {
                    throw new Exception("Software crash! Cannot assign correct status for Payload button. Contact Admin!");
                }
            }
            catch(Exception ex)
            {
                log.Error("Custom_Payload_Function(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void launchPayload()
        {

        }

    }
}
