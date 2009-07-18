using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Rawr;

namespace Rawr.Silverlight
{
    public partial class MainPage : UserControl
    {

        public static ItemTooltip Tooltip { get; private set; }
        public static MainPage Instance { get; private set; }


        private Status status;
        public Status Status
        {
            set { status = value; }
            get
            {
                if (status == null) status = new Status();
                return status;
            }
        }

        private ItemBrowser itemSearch = null;
        public ItemBrowser ItemSearch
        {
            get
            {
                if (itemSearch == null) itemSearch = new ItemBrowser();
                return itemSearch;
            }
        }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (value != null)
                {
                    if (character != null)
                    {
                        character.CalculationsInvalidated -= new EventHandler(character_CalculationsInvalidated);
                        character.ClassChanged -= new EventHandler(character_ClassChanged);
                    }

                    character = value;
                    character.IsLoading = true;
                    DataContext = character;
                    Calculations.LoadModel(Calculations.Models[character.CurrentModel]);
                    Calculations.ClearCache();
                    Calculations.CalculationOptionsPanel.Character = character;
                    HeadButton.Character = character;
                    NeckButton.Character = character;
                    ShoulderButton.Character = character;
                    BackButton.Character = character;
                    ChestButton.Character = character;
                    ShirtButton.Character = character;
                    TabardButton.Character = character;
                    WristButton.Character = character;
                    HandButton.Character = character;
                    BeltButton.Character = character;
                    LegButton.Character = character;
                    FeetButton.Character = character;
                    Ring1Button.Character = character;
                    Ring2Button.Character = character;
                    Trinket1Button.Character = character;
                    Trinket2Button.Character = character;
                    MainHandButton.Character = character;
                    OffHandButton.Character = character;
                    RangedButton.Character = character;
                    TalentPicker.Character = character;

                    BuffControl.Character = character;
                    ComparisonGraph.Character = character;

                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                    character.ClassChanged += new EventHandler(character_ClassChanged);

                    character_ClassChanged(this, EventArgs.Empty);

                    character_CalculationsInvalidated(this, EventArgs.Empty);
                    ItemCache.OnItemsChanged();
                    Character.IsLoading = false;
                }
            }
        }

        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Wait;
            CharacterCalculationsBase calcs = Calculations.GetCharacterCalculations(character, null, true, true, true);
            CalculationDisplay.SetCalculations(calcs.GetCharacterDisplayCalculationValues());
            this.Cursor = Cursors.Arrow;
        }

        public MainPage()
        {
            Instance = this;
            InitializeComponent();
			if (Application.Current.IsRunningOutOfBrowser) OfflineInstallButton.Visibility = Visibility.Collapsed;

            Tooltip = ItemTooltip;

            HeadButton.Slot = CharacterSlot.Head;
            NeckButton.Slot = CharacterSlot.Neck;
            ShoulderButton.Slot = CharacterSlot.Shoulders;
            BackButton.Slot = CharacterSlot.Back;
            ChestButton.Slot = CharacterSlot.Chest;
            ShirtButton.Slot = CharacterSlot.Shirt;
            TabardButton.Slot = CharacterSlot.Tabard;
            WristButton.Slot = CharacterSlot.Wrist;
            HandButton.Slot = CharacterSlot.Hands;
            BeltButton.Slot = CharacterSlot.Waist;
            LegButton.Slot = CharacterSlot.Legs;
            FeetButton.Slot = CharacterSlot.Feet;
            Ring1Button.Slot = CharacterSlot.Finger1;
            Ring2Button.Slot = CharacterSlot.Finger2;
            Trinket1Button.Slot = CharacterSlot.Trinket1;
            Trinket2Button.Slot = CharacterSlot.Trinket2;
            MainHandButton.Slot = CharacterSlot.MainHand;
            OffHandButton.Slot = CharacterSlot.OffHand;
            RangedButton.Slot = CharacterSlot.Ranged;

            ModelCombo.ItemsSource = Calculations.Models.Keys;
            Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);

            Character c = new Character();
            c.CurrentModel = ConfigModel;
            c.Class = Calculations.ModelClasses[c.CurrentModel];
            Character = c;

            Calculations.ModelChanging += new EventHandler(Calculations_ModelChanging);
            ItemCache.Instance.ItemsChanged += new EventHandler(Instance_ItemsChanged);

            StatusMessaging.Ready = true;
        }

        private string ConfigModel { get { return "Healadin"; } }

        private void Calculations_ModelChanged(object sender, EventArgs e)
        {
            foreach (string item in ModelCombo.Items)
            {
                if (item == Character.CurrentModel)
                {
                    ModelCombo.SelectedItem = item;
                }
            }
            OptionsView.Content = Calculations.CalculationOptionsPanel;
            if (!Character.IsLoading) Character = Character;
        }

        private void Calculations_ModelChanging(object sender, EventArgs e)
        {
            Character.SerializeCalculationOptions();
        }


        private void ModelCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Character.IsLoading)
            {
                Character.CurrentModel = ModelCombo.SelectedItem as string;
                Character.Class = Calculations.ModelClasses[Character.CurrentModel];
                Calculations.LoadModel(Calculations.GetModel(Character.CurrentModel));
            }
        }


        private void Instance_ItemsChanged(object sender, EventArgs e)
        {
            Character.InvalidateItemInstances();
            Character.OnCalculationsInvalidated();
        }

        private void InstallOffline(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Application.Current.InstallState == InstallState.NotInstalled)
            {
                Application.Current.Install();
            }
        }

        private void NewCharacter()
        {
            Character c = new Character();
            c.CurrentModel = Character.CurrentModel;
            c.Class = Character.Class;
            c.Race = Character.Race;
            Character = c;
        }

        private void OpenCharacter()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "character file (*.xml)|*.xml";
            if (ofd.ShowDialog().GetValueOrDefault(false))
            {
                using (StreamReader reader = ofd.File.OpenText())
                {
                    Character = Character.LoadFromXml(reader.ReadToEnd());
                }
            }
        }

        private void ItemsAreLoaded(Character character)
        {
            Status.Close();
            Character = character;
        }

        private void SaveCharacter()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "character file (*.xml)|*.xml";
            if (sfd.ShowDialog().GetValueOrDefault(false))
            {
                Character.Save(sfd.OpenFile());
            }
        }

        private void LoadFromArmory()
        {
            ArmoryLoadDialog armoryLoad = new ArmoryLoadDialog();
            armoryLoad.Show();
            armoryLoad.Closed += new EventHandler(armoryLoad_Closed);
        }

        private void armoryLoad_Closed(object sender, EventArgs e)
        {
            ArmoryLoadDialog ald = sender as ArmoryLoadDialog;
            if (((ArmoryLoadDialog)sender).DialogResult.GetValueOrDefault(false))
            {
                Status.Show();
                Armory.GetCharacter(ald.CharacterName, ald.Realm, ald.Region, armory_ResultReady);
            }
        }

        private void armory_ResultReady(Character newChar)
        {
            if (newChar != null)
            {
                Character = newChar;
            }
            Status = null;
        }

        private void ShowItemRefinement()
        {
            new RelevantItemRefinement().Show();
        }

        private void ShowItemEditor()
        {
            ItemSearch.Show();
        }

        private void ShowGemmingTemplates()
        {
            new GemmingTemplates(Character).Show();
        }

        private void FileMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FileMenu != null)
            {
                int newIndex = FileMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    FileMenu.IsDropDownOpen = false;
                    FileMenu.SelectedIndex = 0;
                    if (newIndex == 1) NewCharacter();
                    else if (newIndex == 2) OpenCharacter();
                    else if (newIndex == 3) SaveCharacter();
                    else if (newIndex == 4) OpenSavedUpgradeList();
                    else if (newIndex == 6) LoadFromArmory();
                    //else if (newIndex == 7) LoadFromProfiler(0;
                    else new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }
        }

        private void OpenSavedUpgradeList()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rawr Upgrade List Files|*.xml";
            if (ofd.ShowDialog().GetValueOrDefault())
            {
                new UpgradesComparison(ofd.File.OpenText()).Show();
            }
        }

        private void ImportMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImportMenu != null)
            {
                int newIndex = ImportMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    ImportMenu.IsDropDownOpen = false;
                    ImportMenu.SelectedIndex = 0;
                    new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }
        }

        private void ToolsMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToolsMenu != null)
            {
                int newIndex = ToolsMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    ToolsMenu.IsDropDownOpen = false;
                    ToolsMenu.SelectedIndex = 0;
                    if (newIndex == 1) ShowItemEditor();
                    else if (newIndex == 2) ShowGemmingTemplates();
                    else if (newIndex == 3) ShowItemRefinement();
                    else if (newIndex == 4) ShowItemFilters();
                    else if (newIndex == 6) ShowOptimizer();
                    else new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }

        }

        private void ShowItemFilters()
        {
            new EditItemFilter().Show();
        }

        private void ShowOptimizer()
        {
            new OptimizeWindow(Character).Show();
        }

        private void ShowHelp(string uri)
        {
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(uri, UriKind.Absolute), "_blank");
        }

        private void HelpMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HelpMenu != null)
            {
                int newIndex = HelpMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    HelpMenu.IsDropDownOpen = false;
                    HelpMenu.SelectedIndex = 0;
                    if (newIndex == 1) ShowHelp("http://www.codeplex.com/Rawr/Wiki/View.aspx?title=Help");
                    else if (newIndex == 2) ShowHelp("http://www.youtube.com/watch?v=OjRM5SUoOoQ");
                    else if (newIndex == 3) ShowHelp("http://www.codeplex.com/Rawr/Wiki/View.aspx?title=Gemmings");
                    else if (newIndex == 4) ShowHelp("http://www.codeplex.com/Rawr/Wiki/View.aspx?title=GearOptimization");
                    else if (newIndex == 5) ShowHelp("http://www.codeplex.com/Rawr/Wiki/View.aspx?title=ItemFiltering");
                    else if (newIndex == 7) ShowHelp("http://rawr.codeplex.com/");
                    else if (newIndex == 8) ShowHelp("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2451163");
                    else new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }
        }

        private void OptionsMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OptionsMenu != null)
            {
                int newIndex = OptionsMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    OptionsMenu.IsDropDownOpen = false;
					OptionsMenu.SelectedIndex = 0;
                    if (newIndex == 1) ShowOptions();
                    else if (newIndex == 3) ResetItemCache();
                    else if (newIndex == 4) ResetAllCaches();
                    else new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }

        }

        private void ShowOptions()
        {
            new OptionsDialog().Show();
        }

		private void ResetAllCaches()
		{
			ConfirmationWindow.ShowDialog("Are you sure you'd like to clear and redownload all caches?\r\n\r\nWARNING: This will also unload the current character, so be sure to save first!",
				new EventHandler(ResetAllCaches_Confirmation));
		}

		private void ResetAllCaches_Confirmation(object sender, EventArgs e)
		{
			if (((ConfirmationWindow)sender).DialogResult == true)
			{
				Character = new Character();
				new FileUtils("BuffCache.xml").Delete();
				new FileUtils("EnchantCache.xml").Delete();
				new FileUtils("ItemCache.xml").Delete();
				new FileUtils("Talents.xml").Delete();
				new FileUtils("ItemSource.xml").Delete();
				new FileUtils("ItemFilter.xml").Delete();
				new FileUtils("Settings.xml").Delete();
				LoadScreen ls = new LoadScreen();
				(App.CurrentApplication.RootVisual as Grid).Children.Add(ls);
				this.Visibility = Visibility.Collapsed;
				ls.StartLoading(new EventHandler(ResetCaches_Finished));
				Character = new Character();
			}
		}
		
		private void ResetItemCache()
		{
			ConfirmationWindow.ShowDialog("Are you sure you'd like to clear and redownload the item cache?\r\n\r\nWARNING: This will also unload the current character, so be sure to save first!",
				new EventHandler(ResetItemCaches_Confirmation));
		}

		private void ResetItemCaches_Confirmation(object sender, EventArgs e)
		{
			if (((ConfirmationWindow)sender).DialogResult == true)
			{
				Character = new Character();
				new FileUtils("ItemCache.xml").Delete();
				LoadScreen ls = new LoadScreen();
				(App.CurrentApplication.RootVisual as Grid).Children.Add(ls);
				this.Visibility = Visibility.Collapsed;
				ls.StartLoading(new EventHandler(ResetCaches_Finished));
				Character = new Character();
			}
		}

		private void ResetCaches_Finished(object sender, EventArgs e)
		{
			(App.CurrentApplication.RootVisual as Grid).Children.Remove(sender as LoadScreen);
			this.Visibility = Visibility.Visible;
		}

        private void character_ClassChanged(object sender, EventArgs e)
        {
            ClassCombo.SelectedIndex = Character.ClassIndex;
        }

        private void ClassCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Character.ClassIndex = ClassCombo.SelectedIndex;
        }

    }
}
