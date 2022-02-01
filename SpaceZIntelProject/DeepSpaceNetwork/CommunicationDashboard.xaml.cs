﻿using System;
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
                this.spaceCraftName.Content = vehicle.Name + " " + "Communication Dashboard";
                this.payloadName.Content = vehicle.Payload.Name;
                this.payloadType.Content = vehicle.Payload.Type;

                this.telemetryTimer = new DispatcherTimer();
                this.telemetryTimer.Interval = TimeSpan.FromSeconds(1);
                this.telemetryTimer.Tick += Telemetry_Window_Ticker;
                
                if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                {
                    this.startTelemetry.IsEnabled = false;
                    this.stopTelemetry.IsEnabled = false;
                }
                if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Payload.Status) ||
                    Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status))
                {
                    this.customPayloadBtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Configured_Window() Error: " + ex.Message, ex);
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
                else if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                {
                    this.backendService.SendCommandToVehicle(this.vehicle, Constants.DEORBIT_VEHICLE);
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
                        this.payloadWarning.Content = "Cannot launch Payload!, Spacecraft Deorbited.";
                        this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS,
                            Constants.STATUS_OFFLINE, Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_DECOMMISSION);
                    }

                    this.TelemetryBox.Text = "";
                    this.CommunicationBox.Text = "Lost Connection to spacecraft.";
                    this.TelemetryBox.IsEnabled = false;
                    this.CommunicationBox.IsEnabled = false;
                    this.customPayloadBtn.IsEnabled = false;
                }
                else
                    MessageBox.Show("Operation Not Allowed, Unknown Status: " + this.vehicle.SpacecraftStatus, "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                log.Error("DeOrbit_Spacecraft(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Custom_Payload_Function(object sender, RoutedEventArgs e)
        {
            try
            {

                this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
                if (Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status) 
                    && Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Payload.Status)
                    && Constants.STATUS_ONLINE.Equals(this.vehicle.Payload.PayloadStatus))
                {
                    log.Info("Launched Payload: " + this.vehicle.Payload.Name);
                    this.Launch_Payload();
                    this.customPayloadBtn.IsEnabled = false;
                }
                else if (Constants.STATUS_LAUNCH_INITIATED.Equals(this.vehicle.Status))
                    MessageBox.Show("Operation Not Allowed, Spacecraft not luanched yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status))
                    MessageBox.Show("Operation Not Allowed, Spacecraft not reached orbit yet!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (Constants.STATUS_LAUNCHED.Equals(this.vehicle.Payload.Status))
                {
                    this.customPayloadBtn.IsEnabled = false;
                    MessageBox.Show("Payload Already Launched in the orbit.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Constants.STATUS_DECOMMISSION.Equals(this.vehicle.Payload.Status))
                {
                    this.customPayloadBtn.IsEnabled = false;
                    MessageBox.Show("Payload has been decommissioned.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Unhandled Status: " + this.vehicle.Status + " : " + this.vehicle.Payload.Status, "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                log.Error("Custom_Payload_Function(): " + ex.Message, ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show();
            this.Close();
        }

        private void Start_Telemetry_Function(object sender, RoutedEventArgs e)
        {
            this.vehicle = this.backendService.GetSpacecraft(this.vehicle.Name);
            if ((Constants.STATUS_LAUNCHED.Equals(this.vehicle.Status) 
                || Constants.STATUS_ORBIT_REACHED.Equals(this.vehicle.Status))
                && Constants.STATUS_ONLINE.Equals(this.vehicle.SpacecraftStatus))
            {
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
            this.CommunicationBox.Text += Constants.SEND_COMMAND + Constants.STOP_TELEMETRY + "\n";
            this.CommunicationBox.ScrollToEnd();
            this.backendService.SendCommandToVehicle(this.vehicle, Constants.STOP_TELEMETRY);
            this.telemetryTimer.Stop();
        }
    }
}
