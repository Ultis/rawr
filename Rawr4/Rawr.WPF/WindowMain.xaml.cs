using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rawr.UI;

namespace Rawr.WPF
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        public static WindowMain Instance;
        public WindowMain()
        {
            Instance = this;
            Activated += new EventHandler(WindowMain_Activated);
            Closing += new System.ComponentModel.CancelEventHandler(WindowMain_Closing);
            Application.Current.Exit += new ExitEventHandler(Current_Exit);

            InitializeComponent();

            Height = Rawr.WPF.Properties.Settings.Default.WindowHeight;
            Width = Rawr.WPF.Properties.Settings.Default.WindowWidth;
            Top = Rawr.WPF.Properties.Settings.Default.WindowTop;
            Left = Rawr.WPF.Properties.Settings.Default.WindowLeft;
            WindowState = Rawr.WPF.Properties.Settings.Default.WindowState;

            LoadScreen.StartLoading(new EventHandler(LoadFinished));
        }

        void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState != System.Windows.WindowState.Minimized)
            {
                Rawr.WPF.Properties.Settings.Default.WindowHeight = Height;
                Rawr.WPF.Properties.Settings.Default.WindowWidth = Width;
                Rawr.WPF.Properties.Settings.Default.WindowTop = Top;
                Rawr.WPF.Properties.Settings.Default.WindowLeft = Left;
                Rawr.WPF.Properties.Settings.Default.WindowState = WindowState;
                Rawr.WPF.Properties.Settings.Default.Save();
            }
        }

        void WindowMain_Activated(object sender, EventArgs e)
        {
            Activated -= new EventHandler(WindowMain_Activated);
            if (true/*!Rawr.Properties.GeneralSettings.Default.WelcomeScreenSeen*/)
            {
                ChildWindow window = new WelcomeWindow();
                window.Owner = this;
                window.ShowDialog();
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            LoadScreen.SaveFiles();
        }

        private void LoadFinished(object sender, EventArgs e)
        {
            LoadScreen.Visibility = System.Windows.Visibility.Collapsed;
            RootVisual.Children.Add(new MainPage());
        }
    }
}
