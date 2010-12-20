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
            OKButton.IsEnabled = isValidVersion();
            LB_OutOfDateWarning.Visibility = OKButton.IsEnabled ? Visibility.Collapsed : Visibility.Visible; //use newly set IsEnabled to prevent calling isValidVersion twice
        }

        private bool isValidVersion()
        {   // Make sure it's not empty and they are using the current version
            if (TB_XMLDump.Text == string.Empty)
                return false;
            float version = 0f;
            int rawrBuild = int.MaxValue;
            Regex r1 = new Regex(@"<Version>([0-9\.]+)</Version>");
            Regex r2 = new Regex(@"<RawrBuild>([0-9]+)</RawrBuild>");
            try
            {
                Match match = r1.Match(TB_XMLDump.Text);
                if (match.Success)
                    version = float.Parse(match.Groups[1].Value);
                match = r2.Match(TB_XMLDump.Text);
                if (match.Success)
                    rawrBuild = int.Parse(match.Groups[1].Value);
            }
            catch
            {
                return false;
            }
            // these need to be set to min addon version supported and current rawr build number
            // only needs to be changed when something changes in import routines
            return (version >= 0.11f) && (rawrBuild <= 56325); // TODO would be better to automatically pickup build number from current build
        }
    }
}

