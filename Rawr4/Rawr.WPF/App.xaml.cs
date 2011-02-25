using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            //this.Exit += this.Application_Exit;
            
            InitializeComponent();

            App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            Rawr4ArmoryService versionChecker = new Rawr4ArmoryService(true);
            versionChecker.GetVersionCompleted += new EventHandler<EventArgs<string> >(_timerCheckForUpdates_Callback);
            versionChecker.GetVersionAsync();
            //this.MainWindow = new WindowMain();
        }

        void _timerCheckForUpdates_Callback(object sender, EventArgs<string> version)
        {
            if (!string.IsNullOrEmpty(version.Value))
            {
                string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (currentVersion != version.Value)
                {
                    if (MessageBox.Show(string.Format("A new version of Rawr has been released, version {0}! Would you like to go to the Rawr site to download the new version?",
                        version.Value), "New Version Released!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        string uri = "http://rawr.codeplex.com/";
#if SILVERLIGHT
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(uri, UriKind.Absolute), "_blank");
#else
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(uri));
#endif
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
