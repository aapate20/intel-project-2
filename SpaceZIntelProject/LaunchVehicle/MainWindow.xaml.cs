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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LaunchVehicle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private string spaceCraftName;
        private BackendServiceReference.BackendServicesClient client;
        private BackendServiceReference.Vehicle vehicle;
        private DispatcherTimer dt;
        private int decrement = 10;
        private int timeToOrbitRadius = 0;
        public MainWindow(string spaceCraftName)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            client = new BackendServiceReference.BackendServicesClient();
            this.spaceCraftName = spaceCraftName;
            SpaceCraftLabel.Content = this.spaceCraftName + " Dashboard";
            try
            {
                vehicle = client.GetSpacecraft(spaceCraftName);
                if(vehicle != null)
                {
                    orbitRadius.Content = vehicle.OrbitRadius;
                    payloadName.Content = vehicle.Payload.Name;
                    payloadType.Content = vehicle.Payload.Type;
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAddedSpacecraft() Error.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += launchSequenceTicker;
            dt.Start();
        }

        
        private void launchSequenceTicker(object sender, EventArgs e)
        {
            this.decrement--;
            launchSequence.Content = this.decrement.ToString();

            if(this.decrement == 0)
            {
                dt.Stop();
            }
        }
    }
}
