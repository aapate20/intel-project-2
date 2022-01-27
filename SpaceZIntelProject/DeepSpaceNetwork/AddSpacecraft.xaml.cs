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

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for AddSpacecraft.xaml
    /// </summary>
    public partial class AddSpacecraft : Window
    {
        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle vehicle;
        private BackendServiceReference.Payload payload;
        public AddSpacecraft()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            backendServicesClient = new BackendServiceReference.BackendServicesClient();
            vehicle = new BackendServiceReference.Vehicle();
            payload = new BackendServiceReference.Payload();
        }

        private void Add_Spacecraft_in_DB(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (spaceCraftName.Text != "" && orbitRadius.Text != "" && payload != null && payload.Name != null
                && payload.Name != "" && payload.Type != null && payload.Type != "")
                {
                    vehicle.Name = spaceCraftName.Text;
                    long count = backendServicesClient.CheckSpacecraftExists(vehicle.Name);
                    if (count > 0)
                    {
                        throw new Exception("Spacecraft already exist. " + count.ToString());
                    } 
                    bool success = Int32.TryParse(orbitRadius.Text, out int OrbitIntInput);
                    if (success)
                    {
                        vehicle.OrbitRadius = OrbitIntInput;
                    }
                    else
                    {
                        throw new Exception("Please enter integer value in Orbit Radius field.");
                    }
                    vehicle.Payload = payload;
                    vehicle.Status = "Added";
                    string msg = backendServicesClient.AddSpaceCraft(vehicle);
                    MessageBox.Show(msg + " " + count.ToString(), "Status", MessageBoxButton.OK, MessageBoxImage.Information);
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
                vehicle = new BackendServiceReference.Vehicle();
                payload = new BackendServiceReference.Payload();
                spaceCraftName.Text = string.Empty;
                orbitRadius.Text = string.Empty;
                selectedFileName.Text = string.Empty;
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
                    selectedFileName.Text = System.IO.Path.GetFileName(ofd.FileName);
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
                        payload.Name = data.Value.ToString();
                    }
                    else if ("Type".Equals(data.Key.ToString()))
                    {
                        payload.Type = data.Value.ToString();
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
