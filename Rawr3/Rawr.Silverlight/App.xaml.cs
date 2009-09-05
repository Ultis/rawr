using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr.UI;
using System.Windows.Browser;

namespace Rawr.Silverlight
{
    public partial class App : UI.App
    {
		private MainPage _mainPage = null;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
			this.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(App_CheckAndDownloadUpdateCompleted);

			InitializeComponent();
        }

		private void Application_Startup(object sender, StartupEventArgs e)
        {
            Properties.NetworkSettings.UseAspx = e.InitParams.ContainsKey("UseAspx");
            Grid g = new Grid();
            LoadScreen ls = new LoadScreen();
            g.Children.Add(ls);
            RootVisual = g;
            ls.StartLoading(new EventHandler(LoadFinished));
        }

        private void LoadFinished(object sender, EventArgs e)
        {
            Grid g = RootVisual as Grid;
            g.Children.RemoveAt(0);
			_mainPage = new MainPage();
            g.Children.Add(_mainPage);
			ProcessBookmark();
		
			this.CheckAndDownloadUpdateAsync();
		}

		private void ProcessBookmark()
		{
			string bookmark = HtmlPage.Window.CurrentBookmark;
			if (!string.IsNullOrEmpty(bookmark))
			{
				if (bookmark.StartsWith("~"))
				{
				}
				else if (bookmark.Contains("@") && bookmark.Contains("-"))
				{
					string characterName = bookmark.Substring(0, bookmark.IndexOf("@"));
					string realm = bookmark.Substring(bookmark.IndexOf("@") + 1);
					CharacterRegion region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), realm.Substring(0, 2), true);
					realm = realm.Substring(3);

					_mainPage.LoadFromArmory(characterName, region, realm);
				}
				else if (Calculations.Models.ContainsKey(bookmark))
				{
					Calculations.LoadModel(Calculations.Models[bookmark]);
				}
			}
		}

		private void App_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
		{
			if (e.UpdateAvailable)
				MessageBox.Show("A new version of Rawr has automatically been downloaded and installed! Relaunch Rawr, at your leisure, to use it!", "New version installed", MessageBoxButton.OK);
		}

		private void Application_Exit(object sender, EventArgs e)
        {
            LoadScreen.SaveFiles();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
