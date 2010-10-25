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

namespace Rawr.UI
{
    public partial class WelcomeWindow : ChildWindow
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            MessageText.Text = @"Welcome to a *very* early beta of Rawr4! Rawr4 is designed for level 85 content in WoW 4.0.3. It's very early. Some classes/specs aren't supported yet. Most are barely functional, if at all.";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Rawr.Properties.GeneralSettings.Default.WelcomeScreenSeen = true;
            this.DialogResult = true;
        }
    }
}

