using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;

namespace LaunchVehicle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        void App_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging - Launch Vehicle =============        ");
            string launchSpacecraftName = "";
            try
            {
                launchSpacecraftName = e.Args[0];
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            MainWindow mainWindow = new MainWindow(launchSpacecraftName);
            mainWindow.Show();
        }
    }
}
