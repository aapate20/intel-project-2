using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PayloadSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        private BackendServiceReference.Vehicle vehicle;
        private BackendServiceReference.Telemetry telemetry;
        private BackendServiceReference.Spy spy;
        private BackendServiceReference.Weather weather;
        private BackendServiceReference.Comm comm;
        string spaceCraftName;
        public string command = "";
        private DispatcherTimer telemetryTimer;
        private DispatcherTimer dataTimer;
        private int telemetrySecond = 0;
        private int dataSecond = 0;
        Random random;
        public MainWindow(string Spacecraft)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            spaceCraftName = Spacecraft;
            this.Configured_Window();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Configured_Window()
        {
            try
            {
                random = new Random();
                this.vehicle = this.backendService.GetSpacecraft(spaceCraftName);
                this.telemetry = new BackendServiceReference.Telemetry();
                this.backendService.ConnectToBackend(this.vehicle.Payload.Name);
                this.PayloadLabel.Content = this.vehicle.Payload.Name + " Dashboard";
                this.PayloadType.Content = this.vehicle.Payload.Type;
                this.telemetryTimer = new DispatcherTimer();
                this.dataTimer = new DispatcherTimer();

                if (Constants.PAYLOAD_TYPE_SCIENTIFIC.Equals(this.vehicle.Payload.Type))
                {
                    this.weather = new BackendServiceReference.Weather();
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(5);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(5);
                }  
                else if (Constants.PAYLOAD_TYPE_COMMUNICATION.Equals(this.vehicle.Payload.Type))
                {
                    this.comm = new BackendServiceReference.Comm();
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(10);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(10);
                } 
                else if (Constants.PAYLOAD_TYPE_SPY.Equals(this.vehicle.Payload.Type))
                {
                    this.spy = new BackendServiceReference.Spy();
                    this.telemetryTimer.Interval = TimeSpan.FromSeconds(30);
                    this.dataTimer.Interval = TimeSpan.FromSeconds(30);
                }

                this.telemetryTimer.Tick += TelemetryTicker;
                this.dataTimer.Tick += DataTicker;
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
                this.telemetry.Altitude = random.Next(Convert.ToInt32(this.vehicle.OrbitRadius), Convert.ToInt32(this.vehicle.OrbitRadius) + 1000);
                this.telemetry.Longitude = random.Next(-90, 90);
                this.telemetry.Latitude = random.Next(-180, 180);
                this.telemetry.Temperature = random.Next(-400, -200);
                this.telemetry.TimeToOrbit = 0;

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Altitude").Append("\":  ").Append(this.telemetry.Altitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.telemetry.Longitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.telemetry.Latitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Temperature").Append("\":  ").Append(this.telemetry.Temperature).Append("\n");
                sb.Append("\t").Append("\"").Append("TimeToOrbit").Append("\":  ").Append(this.telemetry.TimeToOrbit).Append("\n");
                sb.Append('}').Append("\n");

                this.TelemetryBox.Text = sb.ToString();
                this.TelemetryBox.ScrollToEnd();
                
                this.backendService.UpdateTelemetryMap(this.vehicle.Payload.Name, this.telemetry);
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
                this.weather.Rainfall = random.Next(0, 25000);
                this.weather.Humidity = random.Next(20, 60);
                this.weather.Snow= random.Next(0, 1000);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Rainfall").Append("\":  ").Append(this.weather.Rainfall).Append(" mm").Append("\n");
                sb.Append("\t").Append("\"").Append("Humidity").Append("\":  ").Append(this.weather.Humidity).Append(" %").Append("\n");
                sb.Append("\t").Append("\"").Append("Snow").Append("\":  ").Append(this.weather.Snow).Append(" in").Append("\n");
                sb.Append('}').Append("\n");

                this.TelemetryBox.Text = sb.ToString();
                this.TelemetryBox.ScrollToEnd();

                this.backendService.UpdateWeatherMap(this.vehicle.Payload.Name, this.weather);
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
                this.comm.Uplink = random.Next(100, 1000);
                this.comm.Downlink = random.Next(1000, 10000);
                this.comm.ActiveTransponder = random.Next(1, 100);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Uplink").Append("\":  ").Append(this.comm.Uplink).Append(" MBps").Append("\n");
                sb.Append("\t").Append("\"").Append("Downlink").Append("\":  ").Append(this.comm.Downlink).Append(" MBps").Append("\n");
                sb.Append("\t").Append("\"").Append("ActiveTransponder").Append("\":  ").Append(this.comm.ActiveTransponder).Append("\n");
                sb.Append('}').Append("\n");

                this.TelemetryBox.Text = sb.ToString();
                this.TelemetryBox.ScrollToEnd();

                this.backendService.UpdateCommMap(this.vehicle.Payload.Name, this.comm);
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
                this.spy.Latitude = random.Next(0, 25000);
                this.spy.Longitude = random.Next(20, 60);

                StringBuilder sb = new StringBuilder();
                sb.Append('{').Append("\n");
                sb.Append("\t").Append("\"").Append("Latitude").Append("\":  ").Append(this.spy.Latitude).Append("\n");
                sb.Append("\t").Append("\"").Append("Longitude").Append("\":  ").Append(this.spy.Longitude).Append("\n");
                sb.Append('}').Append("\n");

                this.TelemetryBox.Text = sb.ToString();
                this.TelemetryBox.ScrollToEnd();

                this.backendService.UpdateSpyMap(this.vehicle.Payload.Name, this.spy);
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Spy Object", ex);
            }
        }

        public void UpdateCommunicationBoard(string command)
        {
            this.command = command;
            this.CommunicationBox.Text += Constants.RECEIVE_COMMAND + command + "\n";
            this.CommunicationBox.ScrollToEnd();
            this.ProcessCommand(command);
        }

        private void ProcessCommand(string command)
        {
            try
            {
                if (Constants.START_TELEMETRY.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Payload.Name + ": " + Constants.SENDING_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.telemetrySecond = 0;
                    this.telemetryTimer.Start();
                }
                else if (Constants.STOP_TELEMETRY.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Payload.Name + ": " + Constants.STOP_TELEMETRY + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.telemetryTimer.Stop();
                }
                else if (Constants.START_DATA.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Payload.Name + ": " + Constants.STARTING_DATA + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.dataSecond = 0;
                    this.dataTimer.Start();
                }
                else if (Constants.STOP_DATA.Equals(command))
                {
                    this.CommunicationBox.Text += this.vehicle.Payload.Name + ": " + Constants.STOPIING_DATA + "\n";
                    this.CommunicationBox.ScrollToEnd();
                    this.dataTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.TelemetryBox.Text = "";
                this.CommunicationBox.Text += Constants.SEND_COMMAND + ": " + Constants.COMMAND_FAILURE + "\n";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the window?", "Close?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question,
                MessageBoxResult.Cancel) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                this.backendService.DisconnectFromBackend(this.vehicle.Payload.Name);
            }
        }
    }
}
