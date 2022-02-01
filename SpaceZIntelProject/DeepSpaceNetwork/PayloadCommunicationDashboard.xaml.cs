using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for PayloadCommunicationDashboard.xaml
    /// </summary>
    public partial class PayloadCommunicationDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.Vehicle vehicle;
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        private BackendServiceReference.Telemetry telemetry;
        private BackendServiceReference.Spy spy;
        private BackendServiceReference.Weather weather;
        private BackendServiceReference.Comm comm;
        private DispatcherTimer telemetryTimer;
        private DispatcherTimer dataTimer;
        public PayloadCommunicationDashboard(BackendServiceReference.Vehicle vehicle)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Thread.Sleep(1000);
            InitializeComponent();
            this.vehicle = vehicle;
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            this.Configured_Window();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Configured_Window()
        {
            try
            {
                this.telemetry = new BackendServiceReference.Telemetry();
                this.PayloadCommunicationSystemLabel.Content = this.vehicle.Payload.Name + " " + "Communication Dashboard";
                this.PayloadType.Content = this.vehicle.Payload.Type;
                this.telemetryTimer = new DispatcherTimer();
                this.dataTimer = new DispatcherTimer();

                if (Constants.PAYLOAD_TYPE_SCIENTIFIC.Equals(this.vehicle.Payload.Type))
                {
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(5);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(5);
                }
                else if (Constants.PAYLOAD_TYPE_COMMUNICATION.Equals(this.vehicle.Payload.Type))
                {
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(10);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(10);
                }
                else if (Constants.PAYLOAD_TYPE_SPY.Equals(this.vehicle.Payload.Type))
                {
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(30);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(30);
                }
                this.telemetryTimer.Tick += TelemetryTicker;
                this.dataTimer.Tick += DataTicker;
                this.StartDataBtn.IsEnabled = true;
                this.StartTelemetryBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Payload Window", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TelemetryTicker(object sender, EventArgs e)
        {
            this.UpdateTelemetry();
        }

        private void DataTicker(object sender, EventArgs e)
        {
            if (Constants.PAYLOAD_TYPE_SCIENTIFIC.Equals(this.vehicle.Payload.Type))
                this.UpdateScientific();
            if (Constants.PAYLOAD_TYPE_COMMUNICATION.Equals(this.vehicle.Payload.Type))
                this.UpdateCommunication();
            if (Constants.PAYLOAD_TYPE_SPY.Equals(this.vehicle.Payload.Type))
                this.UpdateSpy();
        }

        private void UpdateTelemetry()
        {
            try
            {
                this.telemetry = this.backendService.GetTelemetryOfVehicle(this.vehicle.Payload.Name);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Altitude").Append("\":  ").Append(this.telemetry.Altitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.telemetry.Longitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.telemetry.Latitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Temperature").Append("\":  ").Append(this.telemetry.Temperature).Append("\n");
                sb.Append("\t").Append("\"").Append("TimeToOrbit").Append("\":  ").Append(this.telemetry.TimeToOrbit).Append("\n");
                sb.Append('}').Append("\n");

                this.TelemetryBox.Text += sb.ToString();
                this.TelemetryBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Telemetry Object", ex);
            }
        }

        private void UpdateScientific()
        {
            try
            {
                this.weather = this.backendService.GetWeatherDataOfVehicle(this.vehicle.Payload.Name);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Rainfall").Append("\":  ").Append(this.weather.Rainfall).Append(" mm").Append("\n");
                sb.Append("\t").Append("\"").Append("Humidity").Append("\":  ").Append(this.weather.Humidity).Append(" %").Append("\n");
                sb.Append("\t").Append("\"").Append("Snow").Append("\":  ").Append(this.weather.Snow).Append(" in").Append("\n");
                sb.Append('}').Append("\n");

                this.DataBox.Text += sb.ToString();
                this.DataBox.ScrollToEnd();

            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Scientific Object", ex);
            }
        }

        private void UpdateCommunication()
        {
            try
            {
                this.comm = this.backendService.GetCommDataOfVehicle(this.vehicle.Payload.Name);
                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Uplink").Append("\":  ").Append(this.comm.Uplink).Append(" MBps").Append("\n");
                sb.Append("\t").Append("\"").Append("Downlink").Append("\":  ").Append(this.comm.Downlink).Append(" MBps").Append("\n");
                sb.Append("\t").Append("\"").Append("ActiveTransponder").Append("\":  ").Append(this.comm.ActiveTransponder).Append("\n");
                sb.Append('}').Append("\n");

                this.DataBox.Text += sb.ToString();
                this.DataBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Comm Object", ex);
            }
        }

        private void UpdateSpy()
        {
            try
            {
                this.spy = this.backendService.GetSpyDataOfVehicle(this.vehicle.Payload.Name);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.spy.Latitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.spy.Longitude).Append("\n");
                sb.Append('}').Append("\n");

                this.DataBox.Text += sb.ToString();
                this.DataBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Spy Object", ex);
            }
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
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Payload.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_DECOMMISSION,
                    Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, Constants.STATUS_OFFLINE);
                    MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Payload.Name, out Process currentProcess);
                    if (currentProcess != null && !currentProcess.HasExited)
                    {
                        currentProcess.CloseMainWindow();
                        currentProcess.Close();
                        MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Payload.Name);
                    }

                    this.Decommision_Command_Window_Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error("Decommision_Payload(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Decommision_Command_Window_Refresh()
        {
            this.StartTelemetryBtn.IsEnabled = false;
            this.StopTelemetryBtn.IsEnabled = false;
            this.DecommisionBtn.IsEnabled = false;
            this.StartDataBtn.IsEnabled = false;
            this.StopDataBtn.IsEnabled = false;
            this.TelemetryBox.Text = "";
            this.DataBox.Text = "";
            this.CommunicationBox.Text = "Lost Connection to Payload";
            this.TelemetryBox.IsEnabled = false;
            this.DataBox.IsEnabled = false;
            this.CommunicationBox.IsEnabled = false;
        }

        private void Start_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
            if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status) ||
                Constants.STATUS_OFFLINE.Equals(this.vehicle.Payload.PayloadStatus))
            {
                this.Decommision_Command_Window_Refresh();
            }
            else
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Payload.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.START_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.backendService.SendCommandToPayloadVehicle(this.vehicle.Payload.Name, Constants.START_TELEMETRY);
                    this.telemetryTimer.Start();
                    this.StopTelemetryBtn.IsEnabled = true;
                    this.StartTelemetryBtn.IsEnabled = false;
                }
            }
            
        }

        private void Stop_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
            if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status) ||
                Constants.STATUS_OFFLINE.Equals(this.vehicle.Payload.PayloadStatus))
            {
                this.Decommision_Command_Window_Refresh();
            }
            else
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Payload.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.STOP_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.backendService.SendCommandToPayloadVehicle(this.vehicle.Payload.Name, Constants.STOP_TELEMETRY);
                    this.telemetryTimer.Stop();
                    this.StartTelemetryBtn.IsEnabled = true;
                    this.StopTelemetryBtn.IsEnabled = false;
                }
            }
            
        }

        private void Start_Data_Function(object sender, RoutedEventArgs e)
        {
            this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
            if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status) ||
                Constants.STATUS_OFFLINE.Equals(this.vehicle.Payload.PayloadStatus))
            {
                this.Decommision_Command_Window_Refresh();
            }
            else
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Payload.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.START_DATA + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.backendService.SendCommandToPayloadVehicle(this.vehicle.Payload.Name, Constants.START_DATA);
                    this.dataTimer.Start();
                    this.StopDataBtn.IsEnabled = true;
                    this.StartDataBtn.IsEnabled = false;
                }
            }
            
        }

        private void Stop_Data_Function(object sender, RoutedEventArgs e)
        {
            this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
            if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status) ||
                Constants.STATUS_OFFLINE.Equals(this.vehicle.Payload.PayloadStatus))
            {
                this.Decommision_Command_Window_Refresh();
            }
            else
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Payload.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.STOP_DATA + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.backendService.SendCommandToPayloadVehicle(this.vehicle.Payload.Name, Constants.STOP_DATA);
                    this.dataTimer.Stop();
                    this.StartDataBtn.IsEnabled = true;
                    this.StopDataBtn.IsEnabled = false;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Payload.Name, out Process currentProcess);
                if (currentProcess != null && !currentProcess.HasExited)
                {
                    currentProcess.CloseMainWindow();
                    currentProcess.Close();
                    MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Payload.Name);
                }
            }
            catch (Exception ex)
            {
                log.Error("Window_Closed_Spacecraft_Communication_Error(), Did not receive Vehicle object.", ex);
            }
        }

    }
}
