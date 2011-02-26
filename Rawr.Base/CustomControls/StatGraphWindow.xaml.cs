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

namespace Rawr
{
    public partial class StatGraphWindow : ChildWindow
    {
        public StatGraphWindow()
        {
            InitializeComponent();
        }

        public Graph GetGraph { get { return TheGraph; } }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

