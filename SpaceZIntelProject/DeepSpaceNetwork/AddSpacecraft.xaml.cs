using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.ServiceModel;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for AddSpacecraft.xaml
    /// </summary>
    public partial class AddSpacecraft : Window
    {
        private BackendServiceReference.Vehicle vehicle;
        private BackendServiceReference.Payload payload;
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        public AddSpacecraft()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            vehicle = new BackendServiceReference.Vehicle();
            payload = new BackendServiceReference.Payload();
        }

        private void Add_Spacecraft_in_DB(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (this.SpaceCraftName.Text != "" && this.OrbitRadius.Text != "" && this.payload != null && this.payload.Name != null
                && this.payload.Name != "" && this.payload.Type != null && this.payload.Type != "")
                {
                    if(!Constants.PAYLOAD_TYPE_COMMUNICATION.Equals(this.payload.Type) 
                        && !Constants.PAYLOAD_TYPE_SPY.Equals(this.payload.Type)
                        && !Constants.PAYLOAD_TYPE_SCIENTIFIC.Equals(this.payload.Type))
                    {
                        throw new Exception("Payload type should be one of following: " + 
                            Constants.PAYLOAD_TYPE_SCIENTIFIC + ", " + Constants.PAYLOAD_TYPE_SPY + ", " 
                            + Constants.PAYLOAD_TYPE_COMMUNICATION);
                    }
                    this.vehicle.Name = SpaceCraftName.Text;
                    long count = backendService.CheckSpacecraftExists(this.vehicle.Name, this.payload.Name);
                    if (count > 0)
                    {
                        throw new Exception("Spacecraft or Payload already exist. ");
                    } 
                    bool success = Double.TryParse(this.OrbitRadius.Text, out double orbitDoubleInput);
                    if (success)
                    {
                        this.vehicle.OrbitRadius = Math.Round(orbitDoubleInput, 3);
                    }
                    else
                    {
                        throw new Exception("Please enter integer value in Orbit Radius field.");
                    }
                    this.vehicle.Status = Constants.STATUS_ADDED;
                    this.payload.Status = Constants.STATUS_ADDED;
                    this.vehicle.SpacecraftStatus = Constants.STATUS_OFFLINE;
                    this.payload.PayloadStatus = Constants.STATUS_OFFLINE;
                    this.vehicle.Payload = this.payload;
                    
                    string msg = backendService.AddSpaceCraft(this.vehicle);
                    MessageBox.Show(msg, "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    var missionControlSystem = new MissionControlSystem(); //create your new form.
                    missionControlSystem.Show(); //show the new form.
                    this.Close();
                }
                else
                {
                    throw new Exception("All fields are required!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.vehicle = new BackendServiceReference.Vehicle();
                this.payload = new BackendServiceReference.Payload();
                this.SpaceCraftName.Text = string.Empty;
                this.OrbitRadius.Text = string.Empty;
                this.SelectedFileName.Text = string.Empty;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var missionControlWindow = new MissionControlSystem(); //create your new form.
            missionControlWindow.Show(); //show the new form.
            this.Close();
        }
        private void Open_File_Dialog(object sender, RoutedEventArgs e)
        {
            string payloadData = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    this.SelectedFileName.Text = System.IO.Path.GetFileName(ofd.FileName);
                    payloadData = File.ReadAllText(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if(payloadData != null)
            {
                JObject json = JObject.Parse(payloadData);
                foreach(var data in json)
                {
                    if ("Name".Equals(data.Key.ToString()))
                    {
                        this.payload.Name = data.Value.ToString();
                    }
                    else if ("Type".Equals(data.Key.ToString()))
                    {
                        this.payload.Type = data.Value.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter correct configuration file!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
