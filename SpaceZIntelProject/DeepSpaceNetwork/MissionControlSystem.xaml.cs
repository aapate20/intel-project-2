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

        private void Add_Spacecraft(object sender, RoutedEventArgs e)
        {
            var addSpacecraft = new AddSpacecraft(); //create your new form.
            addSpacecraft.Show(); //show the new form.
            this.Close();
        }

        private void Launch_Spacecraft(object sender, RoutedEventArgs e)
        {
            var launchSpacecraft = new LaunchSpacecraft(); //create your new form.
            launchSpacecraft.Show(); //show the new form.
            this.Close();
        }

        private void Go_to_DSN_Dashboard(object sender, RoutedEventArgs e)
        {
            var dsnDashboard = new DSNDashboard(); //create your new form.
            dsnDashboard.Show(); //show the new form.
            this.Close();
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(); //create your new form.
            mainWindow.Show(); //show the new form.
            this.Close();
        }
    }
}
