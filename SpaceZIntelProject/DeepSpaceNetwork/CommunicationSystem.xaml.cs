using System;
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
    /// Interaction logic for CommunicationSystem.xaml
    /// </summary>
    public partial class CommunicationSystem : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, BackendServiceReference.Vehicle> spacecraftDirectory = new Dictionary<string, BackendServiceReference.Vehicle>();
        public CommunicationSystem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Refresh_Window_After_Event();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Refresh_Window_After_Event()
        {
            LaunchSpacecraftList.Items.Clear();
            LaunchSpacecraftList.Items.Add("Select");
            LaunchSpacecraftList.SelectedIndex = 0;
            backendServicesClient = new BackendServiceReference.BackendServicesClient();

            try
            {
                Arr = backendServicesClient.GetAllLaunchSequenceSpacecraft();
                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    LaunchSpacecraftList.Items.Add(v.Name);
                    spacecraftDirectory[v.Name] = v;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAddedSpacecraft() Error.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Communication_Dashboard(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string selectedSpacecraft = LaunchSpacecraftList.SelectedItem.ToString();
                var communicationDashboard = new CommunicationDashboard(spacecraftDirectory[selectedSpacecraft]);
                communicationDashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Communication_Dashboard() error", ex);
                MessageBox.Show("Oops, Some problem with the software!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LaunchSpacecraftList.SelectedIndex = 0;
                this.Refresh_Window_After_Event();
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
