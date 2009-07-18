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

namespace Rawr.Silverlight
{
    public partial class ArmoryLoadDialog : ChildWindow
    {

        public string CharacterName { get; private set; }
        public string Realm { get; private set; }
        public CharacterRegion Region { get; private set; }

        public ArmoryLoadDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            CharacterName = NameText.Text;
            Realm = RealmText.Text;
            Region = (CharacterRegion)Enum.Parse(typeof(CharacterRegion),
                ((ComboBoxItem)RegionCombo.SelectedItem).Content.ToString(), false);

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

