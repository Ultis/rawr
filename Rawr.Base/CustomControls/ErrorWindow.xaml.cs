using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

namespace Rawr
{
    public partial class ErrorWindow : ChildWindow
    {
        private string _ErrorMessage = "";
        private string _StackTrace = "";
        private string _SuggestedFix = "";
        private string _Info = "";

        public string ErrorMessage
        {
            get { return _ErrorMessage != "" ? _ErrorMessage : "No Error Message"; }
            set
            {
                _ErrorMessage = value;
                TB_ErrorMessage.Text = ErrorMessage;
            }
        }
        public string StackTrace
        {
            get { return _StackTrace != "" ? _StackTrace : "No Stack Trace"; }
            set
            {
                _StackTrace = value;
                TB_StackTrace.Text = StackTrace;
                //if (StackTrace == "No Stack Trace") { LB_StackTrace.Visibility = TB_StackTrace.Visibility = Visibility.Collapsed; }
            }
        }
        public string SuggestedFix
        {
            get { return _SuggestedFix != "" ? _SuggestedFix : "No Suggested Fix"; }
            set
            {
                _SuggestedFix = value;
                TB_SuggestedFix.Text = SuggestedFix;
            }
        }
        public string Info
        {
            get { return _Info != "" ? _Info : "No Additional Info"; }
            set
            {
                _Info = value;
                TB_Info.Text = Info;
            }
        }

        public ErrorWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #region Copy to Clipboard
        ChildWindow c = new ChildWindow();
        TextBox tb = null;
        private void BT_CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Grid g = new Grid();
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            // Generate Children
            TextBlock t = new TextBlock() { Text = "Please enter the steps you have taken", Margin = new Thickness(2), };
            tb = new TextBox() {
                Text = "", // "[Please fill in steps here]",
                Margin = new Thickness(2),
                MinHeight = 150, MaxHeight = 150, VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                MinWidth  = 250, MaxWidth  = 250, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };
            TextBlock i = new TextBlock() { Margin = new Thickness(2), MaxWidth = 250, TextWrapping = TextWrapping.Wrap,
                Text = "Adding this information will allow the developers to work your issue more quickly and effectively.", };
            Button BT_OK = new Button() { Content = "OK", MinWidth = 75, Margin = new Thickness(2), HorizontalAlignment = HorizontalAlignment.Right }; BT_OK.Click += new RoutedEventHandler(BT_OK_Click);
            Button BT_Cancel = new Button() { Content = "Cancel", MinWidth = 75, Margin = new Thickness(2), }; BT_Cancel.Click += new RoutedEventHandler(BT_Cancel_Click);
            // Add Children
            g.Children.Add(t);
            g.Children.Add(tb);
            g.Children.Add(BT_OK);
            g.Children.Add(BT_Cancel);
            Grid.SetRow(t, 0);
            Grid.SetRow(tb, 1);
            Grid.SetRow(i, 2);
            Grid.SetRow(BT_OK, 3);
            Grid.SetRow(BT_Cancel, 3);
            Grid.SetColumnSpan(t, 2);
            Grid.SetColumnSpan(tb, 2);
            Grid.SetColumn(BT_Cancel, 1);
            // Push to dialog
            c.Content = g; c.Title = "Reproduction Steps";
            c.Closed += new EventHandler(c_Closed);
            c.Show();
        }
        void BT_OK_Click(object sender, RoutedEventArgs e) { c.DialogResult = true; }
        void BT_Cancel_Click(object sender, RoutedEventArgs e) { c.DialogResult = false; }
        void c_Closed(object sender, EventArgs e)
        {
            string steps = "[Please fill in steps here]";
            if ((bool)c.DialogResult == true)
            {
                steps = tb.Text.Trim();
            }

            #region Read Assembly for program version
            bool readIt = true;
            string version = "";
            try {
                Assembly asm = Assembly.GetExecutingAssembly();
                version = "Debug";
                if (asm.FullName != null) {
                    string[] parts = asm.FullName.Split(',');
                    version = parts[1];
                    while (version.Contains("Version=")) { version = version.Replace("Version=", ""); }
                    while (version.Contains(" ")) { version = version.Replace(" ", ""); }
                }
                version = string.Format("{0}", version);
            } catch (Exception) { readIt = false; }
            #endregion

            Clipboard.SetText(string.Format("I have performed the Suggested Fix and continue to receive this error. "
                + "If I am posting this message without having performed the Suggested Fix I am aware that "
                + "I am consuming the Developer(s) time in managing my Issue unnecessarily."
                #if SILVERLIGHT
                + (readIt ? string.Format("\r\n\r\n== Version: {0} SL ==", version) : "")
                #else
                + (readIt ? string.Format("\r\n\r\n== Version: {0} WPF ==", version) : "")
                #endif
                + "\r\n\r\n== Error Message ==\r\n{0}"
                + "\r\n\r\n== StackTrace ==\r\n{1}"
                + "\r\n\r\n== These are the Steps that I have tried ==\r\n" + steps,
                ErrorMessage, StackTrace));
        }
        #endregion
    }
}

