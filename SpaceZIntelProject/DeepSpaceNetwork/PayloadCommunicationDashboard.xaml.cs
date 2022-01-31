using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for PayloadCommunicationDashboard.xaml
    /// </summary>
    public partial class PayloadCommunicationDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly BackendServiceReference.Vehicle vehicle;
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        public PayloadCommunicationDashboard(BackendServiceReference.Vehicle vehicle)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            this.vehicle = vehicle;
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            this.payloadCommunicationSystemLabel.Content = this.vehicle.Payload.Name + " " + "Communication Dashboard";
            this.payloadType.Content = this.vehicle.Payload.Type;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var payloadCommunicationSystem = new PayloadCommunicationSystem();
            payloadCommunicationSystem.Show();
            this.Close();
        }

        private void Decommision_Payload(object sender, RoutedEventArgs e)
        {
            try
            {
                this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_DECOMMISSION, 
                    Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, Constants.STATUS_OFFLINE);
                MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Payload.Name, out Process currentProcess);
                if (currentProcess != null && !currentProcess.HasExited)
                {
                    currentProcess.Kill();
                    MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Payload.Name);
                }
                startTelemetryBtn.IsEnabled = false;
                stopTelemetryBtn.IsEnabled = false;
                decommisionBtn.IsEnabled = false;
                startDataBtn.IsEnabled = false;
                stopDataBtn.IsEnabled = false;
                this.telemetryBox.Text = "";
                this.dataBox.Text = "";
                this.communicationBox.Text = "Lost Connection to Payload";
                this.telemetryBox.IsEnabled = false;
                this.dataBox.IsEnabled = false;
                this.communicationBox.IsEnabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Decommision_Payload(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
