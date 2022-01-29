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
    /// Interaction logic for PayloadCommunicationDashboard.xaml
    /// </summary>
    public partial class PayloadCommunicationDashboard : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly BackendServiceReference.BackendServicesClient backendServicesClient;
        private readonly BackendServiceReference.Payload payload;
        public PayloadCommunicationDashboard(BackendServiceReference.Payload payload)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            this.payload = payload;
            this.backendServicesClient = new BackendServiceReference.BackendServicesClient();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
