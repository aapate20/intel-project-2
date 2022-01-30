using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PayloadSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        void App_Startup(object sender, StartupEventArgs e)
        {

            string spaceCraftName = "";
            try
            {
                spaceCraftName = e.Args[0];
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging - Payload: " + spaceCraftName + " =============        ");
            MainWindow mainWindow = new MainWindow(spaceCraftName);
            mainWindow.Show();
        }
    }
}
