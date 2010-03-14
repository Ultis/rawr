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
		public Character Character { get; private set; }
		private Rawr.ElitistArmoryService _armoryService = new ElitistArmoryService();

        public ArmoryLoadDialog()
        {
            InitializeComponent();

			_armoryService.GetCharacterProgressChanged += new EventHandler<EventArgs<string>>(_armoryService_GetCharacterProgressChanged);
			_armoryService.GetCharacterCompleted += new EventHandler<EventArgs<Character>>(_armoryService_GetCharacterCompleted);

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
			_armoryService.GetCharacterAsync((CharacterRegion)Enum.Parse(typeof(CharacterRegion),
				((ComboBoxItem)RegionCombo.SelectedItem).Content.ToString(), false), 
				RealmText.Text, NameText.Text, ForceRefreshCheckBox.IsChecked.Value);

			ProgressBarStatus.IsIndeterminate = true;
			OKButton.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = false;
		}

		void _armoryService_GetCharacterProgressChanged(object sender, EventArgs<string> e)
		{
			TextBlockStatus.Text = e.Value;
		}

		void _armoryService_GetCharacterCompleted(object sender, EventArgs<Character> e)
		{
			ProgressBarStatus.IsIndeterminate = true;
			ProgressBarStatus.Value = ProgressBarStatus.Maximum;
			Character = e.Value;
			this.DialogResult = true;
		}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
			if (OKButton.IsEnabled)
			{
				this.DialogResult = false;
			}
			else
			{
				_armoryService.CancelAsync();
				OKButton.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = true;
				ProgressBarStatus.IsIndeterminate = false;
				TextBlockStatus.Text = string.Empty;
			}
        }

		public void Load(string characterName, CharacterRegion region, string realm)
		{
			NameText.Text = characterName;
			RealmText.Text = realm;
			RegionCombo.SelectedItem = RegionCombo.Items.FirstOrDefault(i => ((ComboBoxItem)i).Content.ToString() == region.ToString());
			OKButton_Click(null, null);
		}
	}
}

