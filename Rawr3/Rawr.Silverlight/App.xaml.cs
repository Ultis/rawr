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
using System.Text.RegularExpressions;

namespace Rawr.Silverlight
{
    public partial class App : UI.App
    {
        private MainPage _mainPage = null;

        private Dictionary<Control, string> _windows = new Dictionary<Control, string>();

        public App()
        {
#if DEBUG
#if SILVERLIGHT
            //Application.Current.Host.Settings.EnableFrameRateCounter = true;
            //Application.Current.Host.Settings.EnableRedrawRegions = true;
            //Application.Current.Host.Settings.EnableCacheVisualization = true;
#endif
#endif
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
            //_mainPage.WindowsComboBox.Items.Add(new ComboBoxItem() { Content = "Character", Tag = _mainPage });
            //_mainPage.WindowsComboBox.SelectionChanged += new SelectionChangedEventHandler(WindowsComboBox_SelectionChanged);
            _windows[_mainPage] = "Character";
            g.Children.Add(_mainPage);
            ProcessBookmark();
            if (true/*!Rawr.Properties.GeneralSettings.Default.WelcomeScreenSeen*/)
            {
                new WelcomeWindow().Show();
            }
            if (!Rawr.Properties.GeneralSettings.Default.HasSeenHelpWindow)
            {
                MessageBox.Show(
@"Rawr has added a new feature, the Rawr Web Help Window. The Web Help Window is designed to provide fast access to common help topics.

In the Downloadable Version of Rawr (WPF), you will be provided with a mini-webbrowser which displays relevant topics as seen on our Documentation pages. In Silverlight, the online version of Rawr, you will be prompted with quick links to open the content in your browser. We are working to provide web content directly in here as well.

You will now be taken to this window to become familiar with it.",
                    "New Information!", MessageBoxButton.OK);
                Rawr.Properties.GeneralSettings.Default.HasSeenHelpWindow = true;
                new WebHelp("WhereToStart", "").Show();
            }
            this.CheckAndDownloadUpdateAsync();
        }

        /*void WindowsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mainPage.WindowsComboBox != null)
            {
                int newIndex = _mainPage.WindowsComboBox.SelectedIndex;
                if (newIndex > 0)
                {
                    Control window = (Control)((ComboBoxItem)_mainPage.WindowsComboBox.SelectedItem).Tag;
                    _mainPage.WindowsComboBox.IsDropDownOpen = false;
                    _mainPage.WindowsComboBox.SelectedIndex = 0;
                    ShowWindow(window);
                }
            }
        }*/

        private void ProcessBookmark()
        {
            if (HtmlPage.IsEnabled)
            {
                string bookmark = HtmlPage.Window.CurrentBookmark;
                if (!string.IsNullOrEmpty(bookmark))
                {
                    if (bookmark.StartsWith("~"))
                    {
                        string identifier = bookmark.Substring(1);
                        _mainPage.LoadCharacterFromRawr4Repository(identifier);
                    }
                    else if (bookmark.Contains("@") && bookmark.Contains("-"))
                    {
                        string characterName = bookmark.Substring(0, bookmark.IndexOf("@"));
                        string realm = bookmark.Substring(bookmark.IndexOf("@") + 1);
                        CharacterRegion region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion), realm.Substring(0, 2), true);
                        realm = realm.Substring(3);

                        _mainPage.LoadCharacterFromBNet(characterName, region, realm);
                    }
                    else if (Calculations.Models.ContainsKey(bookmark))
                    {
                        Calculations.LoadModel(Calculations.Models[bookmark]);
                    }
                }
            }
        }

        private bool AutoUpdateWorked = false;
        private void App_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable) {
                AutoUpdateWorked = true;
                MessageBox.Show("A new version of Rawr has automatically been downloaded and installed! Relaunch Rawr, at your leisure, to use it!", "New version installed", MessageBoxButton.OK);
            } else {
                AutoUpdateWorked = false;
                Rawr4ArmoryService versionChecker = new Rawr4ArmoryService(true);
                versionChecker.GetVersionCompleted += new EventHandler<EventArgs<string>>(_timerCheckForUpdates_Callback);
                versionChecker.GetVersionAsync();
            }
        }

        void _timerCheckForUpdates_Callback(object sender, EventArgs<string> version)
        {
            if (AutoUpdateWorked) { return; } // User was already prompted about the update
            if (!string.IsNullOrEmpty(version.Value))
            {
                string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Match currentMatch = Regex.Match(currentVersion, @"(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)\.?(?<revsn>\d+)?");
                Match otherMatch = Regex.Match(version.Value, @"(?<major>\d+)\.(?<minor>\d+)\.(?<build>\d+)\.?(?<revsn>\d+)?");
                //
                bool NewRelAvail = false;

                int valueMajorC = 0; int valueMajorO = 0;
                int valueMinorC = 0; int valueMinorO = 0;
                int valueBuildC = 0; int valueBuildO = 0;
                int valueRevsnC = 0; int valueRevsnO = 0;

                if (currentMatch.Groups["major"].Value != "" && otherMatch.Groups["major"].Value != "")
                {
                    valueMajorC = int.Parse(currentMatch.Groups["major"].Value) * 100 * 100 * 100;
                    valueMajorO = int.Parse(otherMatch.Groups["major"].Value) * 100 * 100 * 100;
                }
                if (currentMatch.Groups["minor"].Value != "" && otherMatch.Groups["minor"].Value != "")
                {
                    valueMinorC = int.Parse(currentMatch.Groups["minor"].Value) * 100 * 100;
                    valueMinorO = int.Parse(otherMatch.Groups["minor"].Value) * 100 * 100;
                }
                if (currentMatch.Groups["build"].Value != "" && otherMatch.Groups["build"].Value != "")
                {
                    valueBuildC = int.Parse(currentMatch.Groups["build"].Value) * 100;
                    valueBuildO = int.Parse(otherMatch.Groups["build"].Value) * 100;
                }
                if (currentMatch.Groups["revsn"].Value != "" && otherMatch.Groups["revsn"].Value != "")
                {
                    valueRevsnC = int.Parse(currentMatch.Groups["revsn"].Value);
                    valueRevsnO = int.Parse(otherMatch.Groups["revsn"].Value);
                }

                int valueCurrent = valueMajorC + valueMinorC + valueBuildC + valueRevsnC;
                int valueOther = valueMajorO + valueMinorO + valueBuildO + valueRevsnO;

                NewRelAvail = valueOther > valueCurrent;

                if (NewRelAvail)
                {
                    if (MessageBox.Show(string.Format("A new version of Rawr has been released, version {0}! Unfortunately, the program did not or could not auto-update. Would you like to go to the Rawr site to download the new version?",
                        version.Value), "New Version Released!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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

        private void Application_Exit(object sender, EventArgs e)
        {
            LoadScreen.SaveFiles();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            /*if (!System.Diagnostics.Debugger.IsAttached)
            {
                ChildWindow errorWin = new ChildWindow()
                {
                    Content = new StackPanel()
                };
                (errorWin.Content as StackPanel).Children.Add(
                    new TextBlock() { Text = "An error has occurred. Please check the Issue Tracker on Rawr's development website (http://rawr.codeplex.com) for a solution, or report it there if it hasn't been reported:" });

                string errorString = string.Empty;
                Exception ex = e.ExceptionObject;
                do
                {
                    errorString += ex.Message + "\r\n\r\n" + (ex.InnerException != null ? ex.InnerException.Message + "\r\n\r\n" : "") + ex.StackTrace;
                    ex = ex.InnerException;
                } while (ex != null);

                (errorWin.Content as StackPanel).Children.Add(
                    new TextBox() { Text = errorString });

                errorWin.Show();

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                //Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
            else
            {*/
            new Base.ErrorBox()
            {
                Title = "Error Performing Action",
                Function = "Unknown Function",
                TheException = e.ExceptionObject,
            }.Show();
            e.Handled = true;
            //}
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

        public override void OpenNewWindow(string title, Control control)
        {
            _windows[control] = title;
            //_mainPage.WindowsComboBox.Items.Add(new ComboBoxItem() { Content = title, Tag = control });
            ShowWindow(control);
        }

        public override void ShowWindow(Control control)
        {
            Grid g = RootVisual as Grid;
            g.Children.RemoveAt(0);
            g.Children.Add(control);
        }

        public override void CloseWindow(Control control)
        {
            Grid g = RootVisual as Grid;
            if (g.Children[0] == control && control != _mainPage)
            {
                g.Children.RemoveAt(0);
                g.Children.Add(_mainPage);
            }
        }
    }
}
