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
using System.Windows.Threading;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for CommunicationDashboard.xaml
    /// </summary>
    /* 
     * DSN Spacecraft Communication Dashboard used to communicate with Launched Spacecraft.
     */
    public partial class CommunicationDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.Vehicle vehicle;
        private DispatcherTimer telemetryTimer;
        private BackendServiceReference.Telemetry telemetryObject;
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        public CommunicationDashboard(BackendServiceReference.Vehicle vehicle)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            this.vehicle = vehicle;
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Configured_Window();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Configured_Window()
        {
            try
            {
                this.SpaceCraftName.Content = vehicle.Name + " " + "Communication Dashboard";
                this.PayloadName.Content = vehicle.Payload.Name;
                this.PayloadType.Content = vehicle.Payload.Type;

                this.telemetryTimer = new DispatcherTimer();
                this.telemetryTimer.Interval = TimeSpan.FromSeconds(1);
                this.telemetryTimer.Tick += Telemetry_Window_Ticker;
                
                if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                {
                    this.StartTelemetry.IsEnabled = false;
                    this.StopTelemetry.IsEnabled = false;
                }
                if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Payload.Status) ||
                    Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status))
                {
                    this.CustomPayloadBtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Configured_Window() Error: " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
         * Function to deorbit (send spacecraft out or orbit) spacecraft.
         */
        private void DeOrbit_Spacecraft(object sender, RoutedEventArgs e)
        {
            try
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);

                    if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status))
                        MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status))
                        MessageBox.Show("Operation Not Allowed, Spacecraft not reached orbit yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                    {
                        this.backendService.SendCommandToVehicle(this.vehicle, Constants.DEORBIT_VEHICLE);
                        log.Info("Spacecraft Deorbited: " + this.vehicle.Name);
                        this.DeorbitSpacecraft.IsEnabled = false;
                        this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_OFFLINE,
                            Constants.COLUMN_STATUS, Constants.STATUS_DEORBIT);

                        MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Name, out Process currentProcess);
                        if (currentProcess != null && !currentProcess.HasExited)
                        {
                            currentProcess.CloseMainWindow();
                            currentProcess.Close();
                            MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Name);
                        }

                        this.StartTelemetry.IsEnabled = false;
                        this.StopTelemetry.IsEnabled = false;

                        if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Payload.Status))
                        {
                            this.PayloadWarning.Content = "Cannot launch Payload!, Spacecraft Deorbited.";
                            this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS,
                                Constants.STATUS_OFFLINE, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_DECOMMISSION);
                        }

                        this.TelemetryBox.Text = "";
                        this.CommunicationBox.Text = "Lost Connection to spacecraft.";
                        this.TelemetryBox.IsEnabled = false;
                        this.CommunicationBox.IsEnabled = false;
                        this.CustomPayloadBtn.IsEnabled = false;
                    }
                    else
                        MessageBox.Show("Operation Not Allowed, Unknown Status: " + this.vehicle.SpacecraftStatus, "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
            catch (Exception ex)
            {
                log.Error("DeOrbit_Spacecraft(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /* 
         * Function to Launch Payload
         */
        private void Custom_Payload_Function(object sender, RoutedEventArgs e)
        {
            try
            {
                bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Name);
                if (!status)
                {
                    MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
                    if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status)
                        && Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Payload.Status)
                        && Constants.STATUS_ONLINE.Equals(this.vehicle.Payload.PayloadStatus))
                    {
                        log.Info("Launched Payload: " + this.vehicle.Payload.Name);
                        this.Launch_Payload();
                        this.CustomPayloadBtn.IsEnabled = false;
                    }
                    else if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status))
                        MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status))
                        MessageBox.Show("Operation Not Allowed, Spacecraft not reached orbit yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Payload.Status))
                    {
                        this.CustomPayloadBtn.IsEnabled = false;
                        MessageBox.Show("Payload Already Launched in the orbit.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status))
                    {
                        this.CustomPayloadBtn.IsEnabled = false;
                        MessageBox.Show("Payload has been decommissioned.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Unhandled Status: " + this.vehicle.Status + " : " + this.vehicle.Payload.Status, "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
            catch (Exception ex)
            {
                log.Error("Custom_Payload_Function(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /* 
         * Function to Launch Payload
         */
        private void Launch_Payload()
        {
            this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.LAUNCH_PAYLOAD + "\n";
            this.CommunicationBox.ScrollToEnd();
            this.backendService.SendCommandToVehicle(this.vehicle, Constants.LAUNCH_PAYLOAD);
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string spaceCraftName = this.vehicle.Name;
                string spacecraftPayloadName = this.vehicle.Payload.Name;
                MainWindow.processDirectorySpacecraft.TryGetValue(spacecraftPayloadName, out Process currentProcess);
                if (currentProcess == null || currentProcess.HasExited)
                {
                    Process process = new Process();
                    string currentDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
                    string parentDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(currentDirectory).ToString()).ToString();
                    string finalLocation = System.IO.Path.Combine(parentDirectory, Constants.PAYLOAD_DIRECTORY).ToString();
                    process.StartInfo = new ProcessStartInfo(finalLocation);
                    process.StartInfo.Arguments = spaceCraftName;
                    MainWindow.processDirectorySpacecraft[spacecraftPayloadName] = process;
                    process.Start();
                    this.backendService.UpdateSpacecraft2(spaceCraftName, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_LAUNCHED,
                        Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, Constants.STATUS_ONLINE);
                }
                else
                {
                    log.Info("Process already running: " + spacecraftPayloadName);
                }
                var payloadCommunicationDashboard = new PayloadCommunicationDashboard(this.vehicle);
                payloadCommunicationDashboard.Show();
            }
            catch (Exception ex)
            {
                log.Error("payloadDashboardFunction() error", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }


        /*
         * Function to get telemetry data from spacecraft.
         * Used thread that will update the UI with telemetry data from payload.
         */
        private void Start_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Name);
            if (!status)
            {
                MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
            else
            {
                this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
                if ((Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status)
                    || Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                    && Constants.STATUS_ONLINE.Equals(this.vehicle.SpacecraftStatus))
                {
                    this.StartTelemetry.IsEnabled = false;
                    this.StopTelemetry.IsEnabled = true;
                    this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.START_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.backendService.SendCommandToVehicle(this.vehicle, Constants.START_TELEMETRY);
                    this.telemetryTimer.Start();
                }
                else if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status))
                    MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (Constants.STATUS_DEORBIT.Equals(this.vehicle.Status)
                    || Constants.STATUS_OFFLINE.Equals(this.vehicle.SpacecraftStatus))
                    MessageBox.Show("Spacecraft Deorbited", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show("Unhandled Status: " + this.vehicle.Status + " , " + this.vehicle.SpacecraftStatus,
                        "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Telemetry_Window_Ticker(object sender, EventArgs e)
        {
            try
            {
                this.telemetryObject = this.backendService.GetTelemetryOfVehicle(this.vehicle.Name);
                StringBuilder sb = new StringBuilder();

                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Altitude").Append("\":  ").Append(this.telemetryObject.Altitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.telemetryObject.Longitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.telemetryObject.Latitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Temperature").Append("\":  ").Append(this.telemetryObject.Temperature).Append("\n");

                if (telemetryObject.TimeToOrbit == 0)
                {
                    this.telemetryTimer.Stop();
                    sb.Append("\t").Append("\"").Append("TimeToOrbit").Append("\":  ").Append("Orbit Reached").Append("\n");
                    this.StartTelemetry.IsEnabled = false;
                    this.StopTelemetry.IsEnabled = false;
                }
                else
                    sb.Append("\t").Append("\"").Append("TimeToOrbit").Append("\":  ").Append(this.telemetryObject.TimeToOrbit).Append("\n");

                sb.Append('}').Append("\n");
                this.TelemetryBox.Text += sb.ToString();
                this.TelemetryBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                log.Error("Telemetry_Window_Ticker(), Did not receive Vehicle object.", ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
         * Stop thread that is running in background 
         * Stop getting telemetry data from spacecraft.
         */
        private void Stop_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            bool status = this.backendService.CheckVehicleConnectedToBackend(this.vehicle.Name);
            if (!status)
            {
                MessageBox.Show("Vehicle is Disconnected. Please login again!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
            else
            {
                this.StartTelemetry.IsEnabled = true;
                this.StopTelemetry.IsEnabled = false;
                this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.STOP_TELEMETRY + "\n";
                this.CommunicationBox.ScrollToEnd();
                this.backendService.SendCommandToVehicle(this.vehicle, Constants.STOP_TELEMETRY);
                this.telemetryTimer.Stop();
            }
        }

        // Function to go back to Spacecraft communication system.
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show();
            this.Close();
        }

        /* This function will close communication channel with spacecraft and stop the process of Launch Vehicle if it is running.
         * Disconnecting client from backend if window is closed so that callback function work properly.
         */
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Name, out Process currentProcess);
                if (currentProcess != null && !currentProcess.HasExited)
                {
                    currentProcess.CloseMainWindow();
                    currentProcess.Close();
                    MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Name);
                }
            }
            catch (Exception ex)
            {
                log.Error("Window_Closed_Spacecraft_Communication_Error(), Did not receive Vehicle object.", ex);
            }
            
        }
    }
}
