using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Data;

namespace Rawr.UI
{
    public partial class WebHelp : ChildWindow
    {
        public WebHelp(string relativeUri, string overrideText = "")
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif
            runlink(relativeUri, overrideText);
        }
        private void runlink(string relativeUri, string overrideText = "")
        {
            uri = relativeUri;
            UpdateCurrentButtonColor();
            overridetext = overrideText;
            BT_Open.NavigateUri = new Uri(BT_Open.NavigateUri + relativeUri);
#if !SILVERLIGHT
            if (overrideText != "") {
                //TB_View.Text = overrideText;
                Web_View.NavigateToString(overrideText);
            } else {
                // Let the user know we need to download it
                Web_View.NavigateToString("Loading...");
                // Give it the right callback so it knows what to do when we get it
                _webHelpService.GetWebHelpCompleted += new EventHandler<EventArgs<String>>(_webHelpService_GetWebHelpCompleted);
                // Call the page
                _webHelpService.GetWebHelpAsync(uri, true);
                //Web_View.Navigate(new Uri(string.Format(WebHelpService.URL_WEBHELP, uri)));
            }
#else
            if (overrideText != "") {
                TB_View.Text = overrideText;
                //Web_View.NavigateToString(overrideText);
            } else if (Application.Current.IsRunningOutOfBrowser) {
                // Let the user know we can't download it due to cross domain access
                // TODO: Make it call through Rawr4.com instead to see if that gets around it
                overrideText = "Silverlight does not function with this feature yet due to security issues."
                    + " We are working on a way around this."
                    + " In the meantime, please use the link below to open it in your browser."
                    + " You can also select a different content on the left before opening in your browser.";
                TB_View.Text = overrideText;
                //Web_View.NavigateToString(overrideText);
                //_webHelpService.GetWebHelpAsync(uri, true);
            } else {
                //Web_View.MaxHeight = 0;
                //Web_View.Visibility = Visibility.Collapsed;
                //this.LayoutRoot.Children.Remove(Web_View);
                //TB_View.Visibility = Visibility.Visible;
                TB_View.Text = "Silverlight does not function with this feature yet due to security issues."
                    + " We are working on a way around this."
                    + " In the meantime, please use the link below to open it in your browser."
                    + " You can also select a different content on the left before opening in your browser.";
            }
#endif
        }

        public string uri = "";
        public string overridetext = "";
        WebHelpService _webHelpService = new WebHelpService();

#if !SILVERLIGHT
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
#endif

        void _webHelpService_GetWebHelpCompleted(object sender, EventArgs<String> e)
        {
#if SILVERLIGHT
            TB_View.Text = e.Value;
            //Web_View.NavigateToString(e.Value);
#else
            Web_View.Visibility = Visibility.Visible;
            Web_View.NavigateToString(e.Value);
            /* The following code is to save the file locally as cached. It does work but I found the
             * NavigateToString function and would rather just use that.
            string RootDir = AppDomain.CurrentDomain.BaseDirectory;
            string pagelocation = string.Format(RootDir + "helppages\\{0}.html", uri);
            Uri uriLocal = new Uri(pagelocation, UriKind.Absolute);
            // Ensure directory paths
            if (!Directory.Exists(RootDir + "helppages\\")) { Directory.CreateDirectory(RootDir + "helppages\\"); }

            try {
                using (var filestream = new StreamWriter(pagelocation))
                {
                    filestream.Write(e.Value);
                    filestream.Close();
                }
                Web_View.Navigate(uriLocal);
            } catch (System.IO.FileFormatException) {
                // ignore it, we couldn't save the file because the web server has a corrupted image
                Web_View.Navigate("about:blank");
            } catch (Exception) {
                // for now, ignore it
                Web_View.Navigate("about:blank");
            }*/
#endif
        }

        private void UpdateCurrentButtonColor() {
            foreach (UIElement uie in TopicsStack.Children) {
                if (uie is Button) {
                    if (((uie as Button).Tag as string) == uri) {
                        (uie as Button).Background = new SolidColorBrush(Colors.Blue);
#if SILVERLIGHT
                        (uie as Button).Foreground = new SolidColorBrush(Colors.Black);
#else
                        (uie as Button).Foreground = new SolidColorBrush(Colors.White);
#endif
                    } else {
                        (uie as Button).Background = new SolidColorBrush(Colors.White);
                        (uie as Button).Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }

        private void Nav_Click(object sender, RoutedEventArgs e) {
            string newUri = (sender as Button).Tag as string;
            uri = newUri;
            runlink(uri, overridetext);
        }

        private void BT_Open_Click(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(string.Format(WebHelpService.URL_WEBHELP, uri), UriKind.Absolute), "_blank");
#else
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(uri));
#endif
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = true; }
    }
}

