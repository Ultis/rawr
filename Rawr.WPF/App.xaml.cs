using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Rawr.UI;

namespace Rawr.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Rawr.UI.App
    {

        public App()
        {
            InitializeComponent();

            App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            Rawr4ArmoryService versionChecker = new Rawr4ArmoryService(true);
            versionChecker.GetVersionCompleted += new EventHandler<EventArgs<string> >(_timerCheckForUpdates_Callback);
            versionChecker.GetVersionAsync();
        }

        public bool GetNewVersionStatus(string version) {
            if (!string.IsNullOrEmpty(version))
            {
                string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Match currentMatch = Regex.Match(currentVersion, @"(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)(\.(?<revsn>\d+))?");
                Match otherMatch   = Regex.Match(version,        @"(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)(\.(?<revsn>\d+))?");
                //
                int valueMajorC = 0; int valueMajorO = 0;
                int valueMinorC = 0; int valueMinorO = 0;
                int valueBuildC = 0; int valueBuildO = 0;
                int valueRevsnC = 0; int valueRevsnO = 0;

                if (currentMatch.Groups["major"].Value != "" && otherMatch.Groups["major"].Value != "") {
                    valueMajorC = int.Parse(currentMatch.Groups["major"].Value) * 100000 * 100000 * 100000;
                    valueMajorO = int.Parse(otherMatch.Groups["major"].Value)   * 100000 * 100000 * 100000;
                    if (valueMajorO > valueMajorC) { return true; }
                }
                if (currentMatch.Groups["minor"].Value != "" && otherMatch.Groups["minor"].Value != "") {
                    valueMinorC = int.Parse(currentMatch.Groups["minor"].Value) * 100000 * 100000;
                    valueMinorO = int.Parse(otherMatch.Groups["minor"].Value)   * 100000 * 100000;
                    if (valueMinorO > valueMinorC) { return true; }
                }
                if (currentMatch.Groups["build"].Value != "" && otherMatch.Groups["build"].Value != "") {
                    valueBuildC = int.Parse(currentMatch.Groups["build"].Value) * 100000;
                    valueBuildO = int.Parse(otherMatch.Groups["build"].Value)   * 100000;
                    if (valueBuildO > valueBuildC) { return true; }
                }
                if (currentMatch.Groups["revsn"].Value != "" && otherMatch.Groups["revsn"].Value != "") {
                    valueRevsnC = int.Parse(currentMatch.Groups["revsn"].Value);
                    valueRevsnO = int.Parse(otherMatch.Groups["revsn"].Value);
                    if (valueRevsnO > valueRevsnC) { return true; }
                }
            }
            return false;
        }

        void _timerCheckForUpdates_Callback(object sender, EventArgs<string> version) {
            if (!string.IsNullOrEmpty(version.Value)) {
                string releaseType = "None";
                string[] versions = version.Value.Split('|');
                if (GetNewVersionStatus(versions[0])) { releaseType = "Release";
                } else if (GetNewVersionStatus(versions[1]) && Rawr.Properties.GeneralSettings.Default.AlertMeToBetaReleases) { releaseType = "Beta"; }

                if (releaseType != "None") {
                    if (MessageBox.Show(string.Format("A new {1} version of Rawr has been released, version {0}! Would you like to go to the Rawr site to download the new version?{2}",
                        releaseType == "Release" ? versions[0] : versions[1], releaseType, releaseType == "Beta" ? "\r\n\r\nYou can turn off Beta version alerts in the Options dialog" : ""),
                        "New Version Released!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        string uri = "http://rawr.codeplex.com/";
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(uri));
                    }
                }
            }
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            new Base.ErrorBox()
            {
                Title = "Error Performing Action",
                Function = "Unknown Function",
                TheException = e.Exception,
            }.Show();
            e.Handled = true;
        }

        /*private void App_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
                MessageBox.Show("A new version of Rawr has automatically been downloaded and installed! Relaunch Rawr, at your leisure, to use it!", "New version installed", MessageBoxButton.OK);
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            LoadScreen.SaveFiles();
        }*/

        public override void OpenNewWindow(string title, System.Windows.Controls.Control control)
        {
            WindowChild window = new WindowChild();
            window.RootVisual.Children.Add(control);
            window.Title = title;
            window.Show();
        }
    }
}
