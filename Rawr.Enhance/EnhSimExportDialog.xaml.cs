using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Rawr.Enhance
{
    public partial class EnhSimExportDialog : ChildWindow
    {
        public EnhSimExportDialog()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            Export_Dump.Text = text;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

