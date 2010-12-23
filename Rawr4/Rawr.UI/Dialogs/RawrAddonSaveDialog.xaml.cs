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

namespace Rawr.UI
{
    public partial class RawrAddonSaveDialog : ChildWindow
    {
        public RawrAddonSaveDialog(Character character)
        {
            InitializeComponent();
            BuildExportLua(character);
        }

        private void BuildExportLua(Character character)
        {
            // this routine will build a LUA representation of the character for Rawr.Addon 
            // and populate the textbox with that for cut and paste into addon
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

