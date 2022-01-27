﻿using System;
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
using System.Windows.Shapes;

namespace DeepSpaceNetwork
{
    /// <summary>
    /// Interaction logic for LaunchSpacecraft.xaml
    /// </summary>
    public partial class LaunchSpacecraft : Window
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private BackendServiceReference.BackendServicesClient backendServicesClient;
        private BackendServiceReference.Vehicle[] Arr;
        private Dictionary<string, Process> processDirectory;
        public LaunchSpacecraft()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            InitializeComponent();
            processDirectory = new Dictionary<string, Process>();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Refresh_Window_After_Event();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Refresh_Window_After_Event()
        {
            SpacecraftList.Items.Clear();
            SpacecraftList.Items.Add("Select");
            SpacecraftList.SelectedIndex = 0;
            backendServicesClient = new BackendServiceReference.BackendServicesClient();

            try
            {
                Arr = backendServicesClient.GetAddedSpacecraft();
                foreach (BackendServiceReference.Vehicle v in Arr)
                {
                    SpacecraftList.Items.Add(v.Name);
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAddedSpacecraft() Error.", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Go_Back(object sender, RoutedEventArgs e)
        {
            var missionControlSystem = new MissionControlSystem(); //create your new form.
            missionControlSystem.Show(); //show the new form.
            this.Close();
        }

        private void Launch_Spacecraft(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string selectedSpacecraft = SpacecraftList.SelectedItem.ToString();
                if ("Select".Equals(selectedSpacecraft))
                {
                    throw new Exception("Please select one from available spacecraft.");
                }
                else
                {
                    processDirectory.TryGetValue(selectedSpacecraft, out Process currentProcess);
                    if (currentProcess == null || currentProcess.HasExited)
                    {
                        Process process = new Process();
                        string currentDirectory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
                        string parentDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(currentDirectory).ToString()).ToString();
                        string finalLocation = System.IO.Path.Combine(parentDirectory, Constants.LAUNCH_VEHICLE_DIRECTORY).ToString();
                        process.StartInfo = new ProcessStartInfo(finalLocation);
                        processDirectory[selectedSpacecraft] = process;
                        process.Start();
                    }
                    else
                    {
                        MessageBox.Show("Luanch Vehicle: " + selectedSpacecraft + " is currently running");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Launch_Spacecraft() error", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SpacecraftList.SelectedIndex = 0;
            }
            finally{
                this.Refresh_Window_After_Event();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

    }
}
