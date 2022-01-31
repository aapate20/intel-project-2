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
        string spaceCraftName;
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
                this.vehicle = this.backendService.GetSpacecraft(spaceCraftName);
                this.PayloadLabel.Content = this.vehicle.Payload.Name + " Dashboard";
                this.PayloadType.Content = this.vehicle.Payload.Type;
            }
            catch (Exception ex)
            {
                log.Error("Error Initializing Payload Window", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
