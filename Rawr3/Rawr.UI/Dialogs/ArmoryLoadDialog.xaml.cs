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
using Manas.Silverlight;

namespace Rawr.UI
{
    public partial class ArmoryLoadDialog : ChildWindow
    {

        public string CharacterName { get; private set; }
        public string Realm { get; private set; }
        public CharacterRegion Region { get; private set; }

        public ArmoryLoadDialog()
        {
            InitializeComponent();

            if (Rawr.Properties.RecentSettings.Default.RecentChars != null) {
                int count = Rawr.Properties.RecentSettings.Default.RecentChars.Count;
                if (count > 0) {
                    string[] autocomplete = new string[count];
                    Rawr.Properties.RecentSettings.Default.RecentChars.CopyTo(autocomplete, 0);
                    //textBoxName.AutoCompleteCustomSource.AddRange(autocomplete);
                    NameText.Text = Rawr.Properties.RecentSettings.Default.RecentChars[count - 1];
                }
            } else { Rawr.Properties.RecentSettings.Default.RecentChars = new List<string>() { }; }
            if (Rawr.Properties.RecentSettings.Default.RecentServers != null) {
                int count = Rawr.Properties.RecentSettings.Default.RecentServers.Count;
                if (count > 0) {
                    string[] autocomplete = new string[count];
                    Rawr.Properties.RecentSettings.Default.RecentServers.CopyTo(autocomplete, 0);
                    //textBoxRealm.AutoCompleteCustomSource.AddRange(autocomplete);
                    RealmText.Text = Rawr.Properties.RecentSettings.Default.RecentServers[count - 1];
                }
            } else { Rawr.Properties.RecentSettings.Default.RecentServers = new List<string>() { }; }
            if (Rawr.Properties.RecentSettings.Default.RecentRegion != null) {
                RegionCombo.SelectedItem = Rawr.Properties.RecentSettings.Default.RecentRegion;
            } else { Rawr.Properties.RecentSettings.Default.RecentRegion = "US"; }
        }

        // The method we have to implement to offer suggestions
        public void DoSuggestName(string text, SuggestCallback callback)
        {
            // Don't suggest if there's no text
            if (text.Length == 0)
            {
                callback(this, null);
                return;
            }

            List<Suggestion> result = new List<Suggestion>();

            // See which options have as a prefix the text entered by the user
            foreach (string option in Rawr.Properties.RecentSettings.Default.RecentChars)
            {
                if (option.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(new Suggestion() { DisplayString = option, ReplaceString = option });
                }
            }

            callback(this, result.ToArray());
        }
        // The method we have to implement to offer suggestions
        public void DoSuggestRealm(string text, SuggestCallback callback)
        {
            // Don't suggest if there's no text
            if (text.Length == 0)
            {
                callback(this, null);
                return;
            }

            var result = new List<Suggestion>();

            // See which options have as a prefix the text entered by the user
            foreach (var option in Rawr.Properties.RecentSettings.Default.RecentServers)
            {
                if (option.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(new Suggestion() { DisplayString = option, ReplaceString = option });
                }
            }

            callback(this, result.ToArray());
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

