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
    public partial class CommunicationDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.Vehicle vehicle;
        private DispatcherTimer refreshTimer;
        private DispatcherTimer telemetryTimer;
        private int refreshSecond = 1;
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
            this.Configured_Dashboard();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.spaceCraftName.Content = vehicle.Name + " " + "Communication Dashboard";
            this.payloadName.Content = vehicle.Payload.Name;
            this.payloadType.Content = vehicle.Payload.Type;
            this.Refresh_Window();
            this.telemetryTimer = new DispatcherTimer();
            this.telemetryTimer.Interval = TimeSpan.FromSeconds(1);
            this.telemetryTimer.Tick += Telemetry_Window_Ticker;

            if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
            {
                this.stopTelemetry.IsEnabled = false;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Refresh_Window()
        {

            this.refreshTimer = new DispatcherTimer();
            this.refreshTimer.Interval = TimeSpan.FromSeconds(1);
            this.refreshTimer.Tick += Refresh_Window_Ticker;
            this.refreshTimer.Start();
        }

        private void Refresh_Window_Ticker(object sender, EventArgs e)
        {
            try
            {
                this.refreshSecond++;
                if (this.refreshSecond % 5 == 0)
                {
                    this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
                    this.Configured_Dashboard();
                }
            }
            catch (Exception ex)
            {
                log.Error("refreshWindowTicker(), Did not receive Vehicle object.", ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Configured_Dashboard()
        {
            try
            {
                if ((Constants.STATUS_LAUNCH_INITIATED.Equals(vehicle.Payload.Status) && Constants.STATUS_ONLINE.Equals(vehicle.Payload.PayloadStatus)) 
                    || Constants.STATUS_ADDED.Equals(vehicle.Payload.Status) || Constants.STATUS_AIR.Equals(vehicle.Payload.Status)) 
                    this.customPayloadBtn.Content = Constants.LAUNCH_PAYLOAD;
                else if (Constants.STATUS_LAUNCHED.Equals(vehicle.Payload.Status) && Constants.STATUS_ONLINE.Equals(vehicle.Payload.PayloadStatus))
                {
                    this.customPayloadBtn.Content = Constants.PAYLOAD_DASHBOARD;
                    this.refreshTimer.Stop();
                    log.Info("Spacecraft CommunicationDashboard: Payload(launched)  -> Refresh Screen Timer Stoped.");
                }
                else 
                    this.customPayloadBtn.IsEnabled = false;

                if(Constants.STATUS_OFFLINE.Equals(this.vehicle.SpacecraftStatus) || Constants.STATUS_DEORBIT.Equals(this.vehicle.Status))
                {
                    this.startTelemetry.IsEnabled = false;
                    this.stopTelemetry.IsEnabled = false;
                    this.refreshTimer.Stop();
                    log.Info("Spacecraft CommunicationDashboard -> Refresh Screen Timer Stoped.");
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
                this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);

                if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status)) 
                    MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
                else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status)) 
                    MessageBox.Show("Operation Not Allowed, Spacecraft not reached orbit yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
                else if(Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                {
                    log.Info("Spacecraft Deorbited: " + this.vehicle.Name);
                    this.deorbitSpacecraft.IsEnabled = false;
                    this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_OFFLINE, 
                        Constants.COLUMN_STATUS, Constants.STATUS_DEORBIT);

                    MainWindow.processDirectorySpacecraft.TryGetValue(this.vehicle.Name, out Process currentProcess);
                    if (currentProcess != null && !currentProcess.HasExited)
                    {
                        currentProcess.Kill();
                        MainWindow.processDirectorySpacecraft.Remove(this.vehicle.Name);
                    }

                    this.startTelemetry.IsEnabled = false;
                    this.stopTelemetry.IsEnabled = false;

                    if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Payload.Status))
                    {
                        this.customPayloadBtn.IsEnabled = false;
                        this.payloadWarning.Content = "Cannot launch Payload!, Spacecraft Deorbited.";
                        this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, 
                            Constants.STATUS_OFFLINE, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_DECOMMISSION);
                    }

                    this.TelemetryBox.Text = "";
                    this.CommunicationBox.Text = "Lost Connection to spacecraft.";
                    this.TelemetryBox.IsEnabled = false;
                    this.CommunicationBox.IsEnabled = false;
                }
                else 
                    MessageBox.Show("Operation Not Allowed, Unknown Status: " + this.vehicle.SpacecraftStatus, "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
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
                this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
                if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status))
                    MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status))
                    MessageBox.Show("Operation Not Allowed, Spacecraft not reached orbit yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status) && Constants.STATUS_ONLINE.Equals(this.vehicle.Payload.PayloadStatus)) {
                    log.Info("Launched Payload: " + this.vehicle.Payload.Name);
                    this.Launch_Payload();
                }
                else if(Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status))
                {
                    this.customPayloadBtn.IsEnabled = false;
                    MessageBox.Show("Payload has been decommissioned.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else 
                    MessageBox.Show("Operation Not Allowed, Unknown Status: " + this.vehicle.SpacecraftStatus, "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
            }
            catch(Exception ex)
            {
                log.Error("Custom_Payload_Function(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Launch_Payload()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                this.customPayloadBtn.Content = Constants.PAYLOAD_DASHBOARD;
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

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show();
            this.Close();
        }

        private void Start_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status) && Constants.STATUS_ONLINE.Equals(this.vehicle.SpacecraftStatus))
            {
                this.backendService.SendCommandToVehicle(this.vehicle, "Start Telemetry.");
                this.telemetryTimer.Start();
            } 
            else if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status)) 
                MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
            else  
                MessageBox.Show("Operation Not Allowed, Unknown Status: " + this.vehicle.SpacecraftStatus, "Message", MessageBoxButton.OK, MessageBoxImage.Warning); 
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
                    this.startTelemetry.IsEnabled = false;
                    this.stopTelemetry.IsEnabled = false;   
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

        private void Stop_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            this.telemetryTimer.Stop();
        }

        public void UpdateCommunicationBoard(string message, string vehicleName)
        {
            this.CommunicationBox.Text += vehicleName + ": " + message;
            this.CommunicationBox.ScrollToEnd();
        }
    }
}
