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
            InitializeComponent();
			if (Application.Current.RunningOffline) OfflineInstallButton.Visibility = Visibility.Collapsed;

            Tooltip = ItemTooltip;

            HeadButton.Slot = Character.CharacterSlot.Head;
            NeckButton.Slot = Character.CharacterSlot.Neck;
            ShoulderButton.Slot = Character.CharacterSlot.Shoulders;
            BackButton.Slot = Character.CharacterSlot.Back;
            ChestButton.Slot = Character.CharacterSlot.Chest;
            ShirtButton.Slot = Character.CharacterSlot.Shirt;
            TabardButton.Slot = Character.CharacterSlot.Tabard;
            WristButton.Slot = Character.CharacterSlot.Wrist;
            HandButton.Slot = Character.CharacterSlot.Hands;
            BeltButton.Slot = Character.CharacterSlot.Waist;
            LegButton.Slot = Character.CharacterSlot.Legs;
            FeetButton.Slot = Character.CharacterSlot.Feet;
            Ring1Button.Slot = Character.CharacterSlot.Finger1;
            Ring2Button.Slot = Character.CharacterSlot.Finger2;
            Trinket1Button.Slot = Character.CharacterSlot.Trinket1;
            Trinket2Button.Slot = Character.CharacterSlot.Trinket2;
            MainHandButton.Slot = Character.CharacterSlot.MainHand;
            OffHandButton.Slot = Character.CharacterSlot.OffHand;
            RangedButton.Slot = Character.CharacterSlot.Ranged;

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
            if (Application.Current.ExecutionState == ExecutionStates.RunningOnline)
            {
                Application.Current.Detach();
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
            new GemmingTemplates().Show();
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
                    else if (newIndex == 5) LoadFromArmory();
                    //else if (newIndex == 6) LoadFromProfiler(0;
                    else new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
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

        private void HelpMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HelpMenu != null)
            {
                int newIndex = HelpMenu.SelectedIndex;
                if (newIndex > 0)
                {
                    HelpMenu.IsDropDownOpen = false;
                    HelpMenu.SelectedIndex = 0;
                    new ErrorWindow() { Message = "Not yet implemented." }.Show();
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
                    new ErrorWindow() { Message = "Not yet implemented." }.Show();
                }
            }

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
