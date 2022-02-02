using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for DSNDashboard.xaml
    /// </summary>
    
    /* 
     * In this window, logic is added to see information of history data of all spacecraft.
     */
    public partial class DSNDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, BackendServiceReference.Vehicle> SpaceCraftDir;
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        public DSNDashboard()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SpaceCraftDir = new Dictionary<string, BackendServiceReference.Vehicle>();
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();
            this.ActiveSpaceCraftList.Items.Clear();
            this.NewSpaceCraftList.Items.Clear();
            try
            {
                Arr = this.backendService.GetAllSpacecraft();

                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    SpaceCraftDir[v.Name] = v;
                    if (Constants.STATUS_ADDED.Equals(v.Status) || Constants.STATUS_LAUNCHED.Equals(v.Status))
                    {
                        this.NewSpaceCraftList.Items.Add(v.Name);
                    }
                    else
                    {
                        this.ActiveSpaceCraftList.Items.Add(v.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to GetAllSpacecraft()", ex);
            }
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        // Function to get spacecraft details.
        private void Get_Spacecraft_details(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if(this.ActiveSpaceCraftList.SelectedItem == null && this.NewSpaceCraftList.SelectedItem == null)
                {
                    throw new Exception("Please select one item from either list.");
                }
                else 
                {
                    if(this.ActiveSpaceCraftList.SelectedItem != null)
                    {
                        SpaceCraftDir.TryGetValue(this.ActiveSpaceCraftList.SelectedItem.ToString(), out BackendServiceReference.Vehicle vehicle1);
                        this.Append_In_SB(sb, vehicle1);
                    }

                    if (this.NewSpaceCraftList.SelectedItem != null)
                    {
                        SpaceCraftDir.TryGetValue(this.NewSpaceCraftList.SelectedItem.ToString(), out BackendServiceReference.Vehicle vehicle2);
                        this.Append_In_SB(sb, vehicle2);
                    }

                    this.SpacecraftDetails.Text = sb.ToString();
                    this.SpacecraftDetails.ScrollToEnd();
                    sb.Clear();
                }
            }
            catch(Exception ex)
            {
                log.Error("Error in Get_Spacecraft_details() Event.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SpacecraftDetails.Text = "";
            }
            finally
            {
                this.ActiveSpaceCraftList.SelectedItem = null;
                this.NewSpaceCraftList.SelectedItem = null;
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

        // Function to go back to mission control system.
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); //create your new form.
            missionControlSystem.Show(); //show the new form.
            this.Close();
        }
    }
}
