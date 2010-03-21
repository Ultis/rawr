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
    public partial class ArmoryLoadDialog : ChildWindow
    {
        private static List<string> ServerNames;

        static ArmoryLoadDialog()
        {
            #region Server Names
            ServerNames = new List<string>() {
                "Caelestrasz",
                "Dath'Remar",
                "Khaz'goroth",
                "Nagrand",
                "Saurfang",
                "Barthilas",
                "Dreadmaul",
                "Frostmourne",
                "Gundrak",
                "Jubei'Thos",
                "Thaurissan",
                "Aerie Peak",
                "Anvilmar",
                "Arathor",
                "Antonidas",
                "Azuremyst",
                "Baelgun",
                "Blade's Edge",
                "Bladefist",
                "Bronzebeard",
                "Cenarius",
                "Draenor",
                "Dragonblight",
                "Echo Isles",
                "Galakrond",
                "Gnomeregan",
                "Hyjal",
                "Kilrogg",
                "Korialstrasz",
                "Lightbringer",
                "Misha",
                "Moonrunner",
                "Nordrassil",
                "Proudmoore",
                "Shadowsong",
                "Shu'Halo",
                "Silvermoon",
                "Skywall",
                "Suramar",
                "Uldum",
                "Uther",
                "Velen",
                "Windrunner",
                "Blackrock",
                "Blackwing Lair",
                "Bonechewer",
                "Boulderfist",
                "Coilfang",
                "Crushridge",
                "Daggerspine",
                "Dark Iron",
                "Darrowmere",
                "Destromath",
                "Dethecus",
                "Dragonmaw",
                "Dunemaul",
                "Frostwolf",
                "Gorgonnash",
                "Gurubashi",
                "Kalecgos",
                "Kil'Jaeden",
                "Lethon",
                "Maiev",
                "Nazjatar",
                "Ner'zhul",
                "Onyxia",
                "Rivendare",
                "Shattered Halls",
                "Spinebreaker",
                "Spirestone",
                "Stonemaul",
                "Stormscale",
                "Tichondrius",
                "Ursin",
                "Vashj",
                "Blackwater Raiders",
                "Cenarion Circle",
                "Feathermoon",
                "Sentinels",
                "Silver Hand",
                "The Scryers",
                "Wyrmrest Accord",
                "The Venture Co.",
                "Azjol-Nerub",
                "Doomhammer",
                "Icecrown",
                "Perenolde",
                "Terenas",
                "Zangarmarsh",
                "Kel'Thuzad",
                "Darkspear",
                "Deathwing",
                "Bloodscalp",
                "Nathrezim",
                "Shadow Council",
                "Aggramar",
                "Alexstrasza",
                "Alleria",
                "Blackhand",
                "Borean Tundra",
                "Cairne",
                "Dawnbringer",
                "Draka",
                "Eitrigg",
                "Fizzcrank",
                "Garona",
                "Ghostlands",
                "Greymane",
                "Grizzly Hills",
                "Hellscream",
                "Hydraxis",
                "Kael'thas",
                "Khaz Modan",
                "Kul Tiras",
                "Madoran",
                "Malfurion",
                "Malygos",
                "Muradin",
                "Nesingwary",
                "Quel'Dorei",
                "Ravencrest",
                "Rexxar",
                "Runetotem",
                "Sen'Jin",
                "Staghelm",
                "Terokkar",
                "Thunderhorn",
                "Vek'nilash",
                "Whisperwind",
                "Winterhoof",
                "Aegwynn",
                "Agamaggan",
                "Akama",
                "Archimonde",
                "Azgalor",
                "Azshara",
                "Balnazzar",
                "Blood Furnace",
                "Burning Legion",
                "Cho'gall",
                "Chromaggus",
                "Detheroc",
                "Drak'tharon",
                "Drak'thul",
                "Frostmane",
                "Garithos",
                "Gul'dan",
                "Hakkar",
                "Illidan",
                "Korgath",
                "Laughing Skull",
                "Mal'Ganis",
                "Malorne",
                "Mug'thol",
                "Stormreaver",
                "Sargeras",
                "The Underbog",
                "Thunderlord",
                "Wildhammer",
                "Farstriders",
                "Kirin Tor",
                "Moon Guard",
                "Scarlet Crusade",
                "Sisters of Elune",
                "Thorium Brotherhood",
                "Emerald Dream",
                "Lightninghoof",
                "Maelstrom",
                "Twisting Nether",
                "Area 52",
                "Arygos",
                "Bloodhoof",
                "Dalaran",
                "Drenden",
                "Durotan",
                "Duskwood",
                "Eldre'Thalas",
                "Elune",
                "Eonar",
                "Exodar",
                "Fenris",
                "Garrosh",
                "Gilneas",
                "Grizzly Hills",
                "Kargath",
                "Khadgar",
                "Llane",
                "Lothar",
                "Medivh",
                "Nazgrel",
                "Norgannon",
                "Shandris",
                "Stormrage",
                "Tanaris",
                "Thrall",
                "Trollbane",
                "Turalyon",
                "Uldaman",
                "Undermine",
                "Ysera",
                "Zul'jin",
                "Altar of Storms",
                "Alterac Mountains",
                "Andorhal",
                "Anetheron",
                "Anub'arak",
                "Arthas",
                "Auchindoun",
                "Black Dragonflight",
                "Bleeding Hollow",
                "Burning Blade",
                "Dalvengyr",
                "Demon Soul",
                "Dentarg",
                "Eredar",
                "Executus",
                "Firetree",
                "Gorefiend",
                "Haomarush",
                "Jaedenar",
                "Lightning's Blade",
                "Mannoroth",
                "Magtheridon",
                "Scilla",
                "Shadowmoon",
                "Shattered Hand",
                "Skullcrusher",
                "Smolderthorn",
                "The Forgotten Coast",
                "Tortheldrin",
                "Warsong",
                "Ysondre",
                "Zuluhed",
                "Argent Dawn",
                "Earthen Ring",
                "Steamwheedle Cartel",
                "Ravenholdt",
                "Quel'Thalas",
                "Drakkari",
                "Ragnaros"
            };
            ServerNames.Sort();
            #endregion
        }

		public Character Character { get; private set; }
		private Rawr.ElitistArmoryService _armoryService = new ElitistArmoryService();

        public ArmoryLoadDialog()
        {
            InitializeComponent();

			_armoryService.ProgressChanged += new EventHandler<EventArgs<string>>(_armoryService_ProgressChanged);
			_armoryService.GetCharacterCompleted += new EventHandler<EventArgs<Character>>(_armoryService_GetCharacterCompleted);

            if (Rawr.Properties.RecentSettings.Default.RecentChars != null) {
                int count = Rawr.Properties.RecentSettings.Default.RecentChars.Count;
                if (count > 0) {
                    string[] autocomplete = new string[count];
                    Rawr.Properties.RecentSettings.Default.RecentChars.CopyTo(autocomplete, 0);
                    NameText.IsTextCompletionEnabled = true;
                    NameText.ItemsSource = autocomplete;
                    NameText.Text = Rawr.Properties.RecentSettings.Default.RecentChars[count - 1];
                }
            } else { Rawr.Properties.RecentSettings.Default.RecentChars = new List<string>() { }; }
            if (Rawr.Properties.RecentSettings.Default.RecentServers == null)
            {
                Rawr.Properties.RecentSettings.Default.RecentServers = new List<string>() { }; 
            }
            List<string> autocompletelist = new List<string>(ServerNames);
            bool dirty = false;
            foreach (var server in Rawr.Properties.RecentSettings.Default.RecentServers)
            {
                if (!autocompletelist.Contains(server))
                {
                    autocompletelist.Add(server);
                    dirty = true;
                }
            }
            if (dirty)
            {
                autocompletelist.Sort();
            }
            RealmText.IsTextCompletionEnabled = true;
            RealmText.ItemsSource = autocompletelist;
            int c = Rawr.Properties.RecentSettings.Default.RecentServers.Count;
            if (c > 0)
            {
                RealmText.Text = Rawr.Properties.RecentSettings.Default.RecentServers[c - 1];
            }

            if (Rawr.Properties.RecentSettings.Default.RecentRegion != null) {
                RegionCombo.SelectedItem = Rawr.Properties.RecentSettings.Default.RecentRegion;
            } else { Rawr.Properties.RecentSettings.Default.RecentRegion = "US"; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
			_armoryService.GetCharacterAsync((CharacterRegion)Enum.Parse(typeof(CharacterRegion),
				((ComboBoxItem)RegionCombo.SelectedItem).Content.ToString(), false), 
				RealmText.Text, NameText.Text, ForceRefreshCheckBox.IsChecked.Value);

			ProgressBarStatus.IsIndeterminate = true;
			OKButton.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = false;
		}

#if !SILVERLIGHT
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
#endif

		void _armoryService_ProgressChanged(object sender, EventArgs<string> e)
		{
			string[] progress = e.Value.Split('|');
			TextBlockStatus.Text = progress[0];
			if (progress.Length > 1)
			{
				ToolTipStatus.Visibility = Visibility.Visible;
				ToolTipStatus.Content = progress[1];
			}
			else
				ToolTipStatus.Visibility = Visibility.Collapsed;
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
#if SILVERLIGHT
			RegionCombo.SelectedItem = RegionCombo.Items.FirstOrDefault(i => ((ComboBoxItem)i).Content.ToString() == region.ToString());
#else
            RegionCombo.SelectedItem = RegionCombo.Items.Cast<object>().FirstOrDefault(i => ((ComboBoxItem)i).Content.ToString() == region.ToString());
#endif
			OKButton_Click(null, null);
		}
	}
}

