using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LaunchVehicle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /* 
     * This window will open if client launch the spacecraft or create communication channel to get telemetry data.
     */
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        private readonly string spaceCraftName;
        private readonly BackendServiceReference.Vehicle vehicle;
        private readonly BackendServiceReference.Telemetry telemetry;
        private DispatcherTimer launchTimer;
        private DispatcherTimer orbitTimer;
        private int launchSequenceSecond = 10;
        private int timeToOrbitSeconds = 0;
        Random random;
        private double distanceCoveredinOneSec = 0;
        private double temperatureReduceInOneSec = 0;
        public MainWindow(string spaceCraftName)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            this.telemetry = new BackendServiceReference.Telemetry();
            this.telemetry.Temperature = 340;
            random = new Random();
            this.spaceCraftName = spaceCraftName;
            this.SpaceCraftLabel.Content = this.spaceCraftName + " Dashboard";
            this.backendService.ConnectToBackend(spaceCraftName);
            Thread.Sleep(500);
            try
            {
                this.vehicle = this.backendService.GetSpacecraft(spaceCraftName);
                if(vehicle != null)
                {
                    this.OrbitRadius.Content = vehicle.OrbitRadius;
                    this.PayloadName.Content = vehicle.Payload.Name;
                    this.PayloadType.Content = vehicle.Payload.Type;
                    if (Constants.STATUS_LAUNCH_INITIATED.Equals(vehicle.Status))
                    {
                        this.CalculateTimeToOrbit();
                        this.Start_Launch_Sequence();
                    }
                    else
                    {
                        this.TimeToOrbit.Content = Constants.STATUS_ORBIT_REACHED;
                        this.LaunchLabel.Visibility = Visibility.Hidden;
                        this.LaunchSequence.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    throw new Exception("Cannot fetch Spacecraft data from database.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing LaunchVehicle Window.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        // Start Launch sequence -> 10, 9, 8, ...  
        private void Start_Launch_Sequence()
        {
            this.launchTimer = new DispatcherTimer();
            this.launchTimer.Interval = TimeSpan.FromSeconds(1);
            this.launchTimer.Tick += LaunchSequenceTicker;
            this.launchTimer.Start();
        }

        
        private void LaunchSequenceTicker(object sender, EventArgs e)
        {
            this.launchSequenceSecond--;
            this.LaunchSequence.Content = this.launchSequenceSecond.ToString();

            if(this.launchSequenceSecond == 0)
            {
                this.launchTimer.Stop();
                this.LaunchSequence.Content = "Spacecraft Launched!";
                this.LaunchSpacecraft();
            }
        }

        // Function to Launch Spacecraft after launch sequence  = 0.
        private void LaunchSpacecraft()
        {
            try
            {
                this.LaunchLabel.Visibility= Visibility.Hidden;
                this.LaunchSequence.Visibility= Visibility.Hidden;
                this.backendService.UpdateSpacecraft3(this.vehicle.Name, Constants.COLUMN_STATUS, Constants.STATUS_LAUNCHED, 
                    Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_ONLINE, 
                    Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_AIR);
                this.orbitTimer = new DispatcherTimer();
                this.orbitTimer.Interval = TimeSpan.FromSeconds(1);
                this.orbitTimer.Tick += TimeToReachOrbitTicker;
                this.orbitTimer.Start();
            }
            catch(Exception ex)
            {
                log.Error("LaunchVehicle! Did not receive Vehicle object.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        // Start counter for time to reach orbit after spacecraft is launched.
        private void TimeToReachOrbitTicker(object sender, EventArgs e)
        {
            this.timeToOrbitSeconds--;
            this.TimeToOrbit.Content = this.timeToOrbitSeconds.ToString();

            if(timeToOrbitSeconds == 0)
            {
                this.TimeToOrbit.Content = Constants.STATUS_ORBIT_REACHED;
                this.orbitTimer.Stop();
                this.LaunchPayload();
            }

            this.UpdateTelemetry();
        }

        // Function to update telemetry board (UI).
        private void UpdateTelemetry()
        {
            this.telemetry.Altitude += this.distanceCoveredinOneSec;
            this.telemetry.Longitude = random.Next(-90, 90);
            this.telemetry.Latitude = random.Next(-180, 180);
            this.telemetry.Temperature -= this.temperatureReduceInOneSec;
            this.telemetry.TimeToOrbit = this.timeToOrbitSeconds;

            StringBuilder sb = new StringBuilder();
            sb.Append('{').Append("\n");
            sb.Append("\t").Append("\"").Append("Altitude").Append("\":  ").Append(this.telemetry.Altitude).Append("\n");
            sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.telemetry.Longitude).Append("\n");
            sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.telemetry.Latitude).Append("\n");
            sb.Append("\t").Append("\"").Append("Temperature").Append("\":  ").Append(this.telemetry.Temperature).Append("\n");
            sb.Append("\t").Append("\"").Append("TimeToOrbit").Append("\":  ").Append(this.TimeToOrbit.Content).Append("\n");
            sb.Append('}').Append("\n");

            this.TelemetryBox.Text = sb.ToString();
            this.TelemetryBox.ScrollToEnd();
            this.backendService.UpdateTelemetryMap(this.vehicle.Name, this.telemetry);
        }

        // Function to launch payload
        private void LaunchPayload()
        {
            try
            {
                this.backendService.UpdateSpacecraft3(vehicle.Name, Constants.COLUMN_STATUS, Constants.STATUS_ORBIT_REACHED, 
                    Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_LAUNCH_INITIATED, 
                    Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, Constants.STATUS_ONLINE);
            }
            catch (Exception ex)
            {
                log.Error("LaunchVehicle! Did not receive Vehicle object.", ex);
                MessageBox.Show("Some problem in the software!, Please contact Admin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        // Function to calculate time to reach orbit.
        private void CalculateTimeToOrbit()
        {
            try
            {
                this.timeToOrbitSeconds = (int)Math.Ceiling((this.vehicle.OrbitRadius / 3600) + 10);
                this.temperatureReduceInOneSec = 340 / this.timeToOrbitSeconds;
                this.distanceCoveredinOneSec = this.vehicle.OrbitRadius / this.timeToOrbitSeconds;
                this.TimeToOrbit.Content = this.timeToOrbitSeconds.ToString();
            }
            catch (Exception ex)
            {
                log.Error("LaunchVehicle! Did not receive Vehicle object.", ex);
                MessageBox.Show("Some problem in the software!, Please contact Admin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Function to update communication board (UI).
        public void UpdateCommunicationBoard(string command)
        {
            this.CommunicationBox.Text += Constants.RECEIVE_COMMAND + command + "\n";
            this.CommunicationBox.ScrollToEnd();
            this.ProcessCommand(command);
        }

        // Function to process command get from DSN.
        private void ProcessCommand(string command)
        {
            try
            {
                if (Constants.START_TELEMETRY.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Name + ": " + Constants.SENDING_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                }
                else if (Constants.LAUNCH_PAYLOAD.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Name + ": " + Constants.LAUNCHING_PAYLOAD + "\n";
                    this.CommunicationBox.ScrollToEnd();
                }
                else if (Constants.STOP_TELEMETRY.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Name + ": " + Constants.STOP_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                }
                else if (Constants.DEORBIT_VEHICLE.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Name + ": " + Constants.DEORBITING_VEHICLE + "\n";
                    this.CommunicationBox.ScrollToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.TelemetryBox.Text = "";
                this.CommunicationBox.Text += Constants.SEND_COMMAND + ": " + Constants.COMMAND_FAILURE + "\n";
            }
        }

        // Function to disconnect if client disconnected.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.backendService.DisconnectFromBackend(this.vehicle.Name);
        }

        // Function to take step if spacecraft is disconnected while in Launching state or Reaching the orbit.
        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.launchSequenceSecond > 0)
            {
                this.launchTimer.Stop();
                if (this.vehicle != null)
                {
                    this.backendService.UpdateSpacecraft2(this.vehicle.Name, Constants.COLUMN_STATUS, Constants.STATUS_ADDED,
                        Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_OFFLINE);
                }
            }
            else if (this.timeToOrbitSeconds > 0)
            {
                this.orbitTimer.Stop();
                this.backendService.UpdateSpacecraft3(this.vehicle.Name, Constants.COLUMN_STATUS, Constants.STATUS_MISSION_ABORTED,
                    Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_OFFLINE,
                    Constants.COLUMN_PAYLOAD_STATUS, Constants.STATUS_MISSION_ABORTED);
            }
        }

    }
}
