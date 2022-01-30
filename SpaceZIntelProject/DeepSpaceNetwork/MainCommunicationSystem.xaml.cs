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
    /// Interaction logic for MainCommunicationSystem.xaml
    /// </summary>
    public partial class MainCommunicationSystem : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        public MainCommunicationSystem()
        {
            Mouse.OverrideCursor = Cursors.Wait;    
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Go_to_Spacecraft_Communication_System(object sender, RoutedEventArgs e)
        {
            var communicationSystem = new CommunicationSystem();
            communicationSystem.Show();
            this.Close();
        }

        private void Go_to_Payload_Communication_System(object sender, RoutedEventArgs e)
        {
            var payloadCommunicationSystem = new PayloadCommunicationSystem();
            payloadCommunicationSystem.Show();
            this.Close();
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
