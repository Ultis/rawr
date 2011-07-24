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
using System.Threading;

namespace Rawr.UI
{
    public partial class ConfirmationWindow : ChildWindow
    {
		public static void ShowDialog(string message, string title, EventHandler callback, bool cancelButton = false)
		{
			ConfirmationWindow window = new ConfirmationWindow() { Message = message, Title = title };
            if (cancelButton) window.CancelButton.Visibility = Visibility.Visible;
			if (callback != null) window.Closed += callback;
			window.Show();
		}

        public MessageBoxResult MessageBoxResult { get; set; }

		public string Message
        {
            get { return MessageText.Text; }
            set { MessageText.Text = value; }
        }

		public ConfirmationWindow()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            this.DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.No;
            this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            this.Close();
        }
    }
}

