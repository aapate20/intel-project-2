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
    /// Interaction logic for MissionControlSystem.xaml
    /// </summary>
    public partial class MissionControlSystem : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        public MissionControlSystem()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Redirect to Add Spacecraft Window
        private void Add_Spacecraft(object sender, RoutedEventArgs e)
        {
            Window addSpacecraft = new AddSpacecraft();
            addSpacecraft.Show();
            this.Close();
        }

        // Redirect to Launch Spacecraft Window
        private void Launch_Spacecraft(object sender, RoutedEventArgs e)
        {
            Window launchSpacecraft = new LaunchSpacecraft(); 
            launchSpacecraft.Show(); 
            this.Close();
        }

        // Redirect to DSN Dashboard Window
        private void Go_to_DSN_Dashboard(object sender, RoutedEventArgs e)
        {
            Window dsnDashboard = new DSNDashboard(); 
            dsnDashboard.Show(); 
            this.Close();
        }

        // Function to go Previous Window
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            Window backWindow = new MainWindow();
            backWindow.Show();
            this.Close();
        }
    }
}
