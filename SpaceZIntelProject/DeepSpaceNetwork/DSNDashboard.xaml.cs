using System;
using System.Collections;
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
    /// Interaction logic for DSNDashboard.xaml
    /// </summary>
    public partial class DSNDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, BackendServiceReference.Vehicle> SpaceCraftDir;   
        public DSNDashboard()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SpaceCraftDir = new Dictionary<string, BackendServiceReference.Vehicle>();
            backendServicesClient = new BackendServiceReference.BackendServicesClient();
            ActiveSpaceCraftList.Items.Clear();
            NewSpaceCraftList.Items.Clear();
            try
            {
                Arr = backendServicesClient.GetAllSpacecraft();

                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    SpaceCraftDir[v.Name] = v;
                    if (Constants.STATUS_ADDED.Equals(v.Status) || Constants.STATUS_LAUNCHED.Equals(v.Status))
                    {
                        NewSpaceCraftList.Items.Add(v.Name);
                    }
                    else
                    {
                        ActiveSpaceCraftList.Items.Add(v.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to GetAllSpacecraft()", ex);
            }
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); //create your new form.
            missionControlSystem.Show(); //show the new form.
            this.Close();
        }

        private void Get_Spacecraft_details(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if(ActiveSpaceCraftList.SelectedItem == null && NewSpaceCraftList.SelectedItem == null)
                {
                    throw new Exception("Please select one item from either list.");
                }
                else 
                {
                    if(ActiveSpaceCraftList.SelectedItem != null)
                    {
                        SpaceCraftDir.TryGetValue(ActiveSpaceCraftList.SelectedItem.ToString(), out BackendServiceReference.Vehicle vehicle1);
                        this.Append_In_SB(sb, vehicle1);
                    }

                    if (NewSpaceCraftList.SelectedItem != null)
                    {
                        SpaceCraftDir.TryGetValue(NewSpaceCraftList.SelectedItem.ToString(), out BackendServiceReference.Vehicle vehicle2);
                        this.Append_In_SB(sb, vehicle2);
                    }

                    SpacecraftDetails.Text = sb.ToString();
                    SpacecraftDetails.ScrollToEnd();
                    sb.Clear();
                }
            }
            catch(Exception ex)
            {
                log.Error("Error in Get_Spacecraft_details() Event.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SpacecraftDetails.Text = "";
            }
            finally
            {
                ActiveSpaceCraftList.SelectedItem = null;
                NewSpaceCraftList.SelectedItem = null;
            }
        }

        private void Append_In_SB(StringBuilder sb, BackendServiceReference.Vehicle vehicle)
        {
            if (vehicle != null)
            {
                sb.Append("Name: " + vehicle.Name + "\n");
                sb.Append("Orbit Radius: " + vehicle.OrbitRadius + "\n");
                sb.Append("Spacecraft Status: " + vehicle.Status + "\n");
                sb.Append("Payload Name: " + vehicle.Payload.Name + "\n");
                sb.Append("Payload Type: " + vehicle.Payload.Type + "\n");
                sb.Append("Payload Status: " + vehicle.Payload.Status + "\n");
                sb.Append("----------------\n\n");
            }
        }
    }
}
