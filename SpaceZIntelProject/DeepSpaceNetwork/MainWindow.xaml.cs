using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Extensions.Logging;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        public static Dictionary<string, Process> processDirectory = new Dictionary<string, Process>();
         public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            log.Info("Application started");
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Go_to_DSN_Dashboard(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); 
            missionControlSystem.Show();
            this.Close();
        }

        private void Go_to_Communication_System(object sender, RoutedEventArgs e)
        {
            var mianCommunicationSystem = new MainCommunicationSystem();
            mianCommunicationSystem.Show(); 
            this.Close();
        }

    }
}
