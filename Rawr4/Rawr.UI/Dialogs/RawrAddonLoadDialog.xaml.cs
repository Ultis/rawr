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

namespace Rawr.UI
{
    public partial class RawrAddonLoadDialog : ChildWindow
    {
        public RawrAddonLoadDialog()
        {
            InitializeComponent();
        }

        public RawrAddonImportType ImportType {
            get {
                if (RB_Equipped.IsChecked.GetValueOrDefault(false)) { return RawrAddonImportType.Equipped; }
                if (RB_EquippedBags.IsChecked.GetValueOrDefault(false)) { return RawrAddonImportType.EquippedBags; }
                return RawrAddonImportType.EquippedBagsBank;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TB_XMLDump_TextChanged(object sender, TextChangedEventArgs e)
        {
            OKButton.IsEnabled = (TB_XMLDump.Text != "" && TB_XMLDump.Text.Contains("0.09")); // Make sure it's not empty and they are using the current version
            LB_OutOfDateWarning.Visibility = (!TB_XMLDump.Text.Contains("0.09")) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

