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
    /// Interaction logic for PayloadCommunicationSystem.xaml
    /// </summary>
    public partial class PayloadCommunicationSystem : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, BackendServiceReference.Payload> payloadDirectory = new Dictionary<string, BackendServiceReference.Payload>();
        public PayloadCommunicationSystem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Refresh_Window_After_Event();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Refresh_Window_After_Event()
        {
            this.LaunchPayloadList.Items.Clear();
            LaunchPayloadList.Items.Add("Select");
            LaunchPayloadList.SelectedIndex = 0;
            backendServicesClient = new BackendServiceReference.BackendServicesClient();

            try
            {
                Arr = backendServicesClient.GetAllOnlinePayload();
                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    LaunchPayloadList.Items.Add(v.Payload.Name);
                    payloadDirectory[v.Payload.Name] = v.Payload;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAddedSpacecraft() Error.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Payload_Communication_Dashboard(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string selectedSpacecraft = LaunchPayloadList.SelectedItem.ToString();
                var payloadCommunicationDashboard = new PayloadCommunicationDashboard(payloadDirectory[selectedSpacecraft]);
                payloadCommunicationDashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Communication_Dashboard() error", ex);
                MessageBox.Show("Oops, Some problem with the software!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LaunchPayloadList.SelectedIndex = 0;
                this.Refresh_Window_After_Event();
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var mainCommunicationSystem = new MainCommunicationSystem();
            mainCommunicationSystem.Show();
            this.Close();
        }
    }
}
