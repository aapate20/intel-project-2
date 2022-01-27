using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;
using log4net;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {

            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging DSN =============        ");
            base.OnStartup(e);
        }
    }
}
