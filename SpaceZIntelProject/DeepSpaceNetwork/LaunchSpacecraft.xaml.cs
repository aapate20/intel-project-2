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

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for LaunchSpacecraft.xaml
    /// </summary>
    
    /*
     * After adding Spacecraft, one can launch spacecraft from this window.
     */
    public partial class LaunchSpacecraft : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, BackendServiceReference.Vehicle> spacecraftDirectory = new Dictionary<string, BackendServiceReference.Vehicle>();
        private BackendServiceReference.IBackendServices backendService;
        private static DuplexChannelFactory<BackendServiceReference.IBackendServices> duplexChannelFactory;
        public LaunchSpacecraft()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Refresh_Window_After_Event();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Refresh_Window_After_Event()
        {
            this.SpacecraftList.Items.Clear();
            this.SpacecraftList.Items.Add("Select");
            this.SpacecraftList.SelectedIndex = 0;
            duplexChannelFactory = new DuplexChannelFactory<BackendServiceReference.IBackendServices>(new Callback(), Constants.SERVICE_END_POINT);
            this.backendService = duplexChannelFactory.CreateChannel();

            try
            {
                Arr = this.backendService.GetAddedSpacecraft();
                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    this.SpacecraftList.Items.Add(v.Name);
                    spacecraftDirectory[v.Name] = v;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAddedSpacecraft() Error.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
         * Function to launch Spacecraft and its DSN communication dashboard.
         */
        private void Launch_Spacecraft(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string selectedSpacecraft = this.SpacecraftList.SelectedItem.ToString();
                if ("Select".Equals(selectedSpacecraft))
                {
                    throw new Exception("Please select one from available spacecraft.");
                }
                else
                {
                    MainWindow.processDirectorySpacecraft.TryGetValue(selectedSpacecraft, out Process currentProcess);
                    if (currentProcess == null || currentProcess.HasExited)
                    {
                        Process process = new Process();
                        string currentDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
                        string parentDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(currentDirectory).ToString()).ToString();
                        string finalLocation = System.IO.Path.Combine(parentDirectory, Constants.LAUNCH_VEHICLE_DIRECTORY).ToString();
                        process.StartInfo = new ProcessStartInfo(finalLocation);
                        process.StartInfo.Arguments = selectedSpacecraft;
                        MainWindow.processDirectorySpacecraft[selectedSpacecraft] = process;
                        process.Start();
                        this.backendService.LaunchSpacecraft(selectedSpacecraft, Constants.SPACECRAFT_PREFIX + selectedSpacecraft);
                    }
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Launch_Spacecraft() error", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SpacecraftList.SelectedIndex = 0;
                this.Refresh_Window_After_Event();
            }
            finally{
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        // Function to go back to Mission Control System Window.
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); //create your new form.
            missionControlSystem.Show(); //show the new form.
            this.Close();
        }

    }
}
