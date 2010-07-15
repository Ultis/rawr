using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr.Base;

namespace Rawr.UI
{
    public partial class BossPicker : UserControl
    {
        private BossOptions _BossOptions;
        public BossOptions BossOptions {
            get { return _BossOptions; }
            set {
                if (_BossOptions != null) {
                    _BossOptions.PropertyChanged -= new PropertyChangedEventHandler(bossOpts_PropertyChanged);
                }
                _BossOptions = value;
                DataContext = _BossOptions;
                _BossOptions.PropertyChanged += new PropertyChangedEventHandler(bossOpts_PropertyChanged);
                bossOpts_PropertyChanged(this, null);
            }
        }

        private bool isLoading = false;
        private bool firstload = true;
        private BossList bosslist = null;

        private Character _char;
        public Character Character {
            get { return _char; }
            set {
                if (_char != null) {
                    //_char.ClassChanged -= new EventHandler(character_ClassChanged);
                }
                _char = value;
                if (_char != null) {
                    //_char.ClassChanged += new EventHandler(character_ClassChanged);
                    //character_ClassChanged(this, EventArgs.Empty);
                    BossOptions = Character.BossOptions;
                }
            }
        }

        public void bossOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isLoading) { return; }
            // Basics
            CB_Level.SelectedItem = BossOptions.Level;
            CB_Armor.SelectedItem = BossOptions.Armor;
            NUD_Duration.Value = BossOptions.BerserkTimer;
            NUD_DurationSpeed.Value = BossOptions.SpeedKillTimer;
            NUD_TargHP.Value = BossOptions.Health;
            NUD_Under35Perc.Value = BossOptions.Under35Perc * 100;
            NUD_Under20Perc.Value = BossOptions.Under20Perc * 100;
            CB_InBackPerc.IsEnabled = (bool)(CK_InBack.IsChecked = BossOptions.InBack);
            CB_InBackPerc.Value = BossOptions.InBackPerc_Melee * 100d;
            CB_MaxPlayers.SelectedItem = BossOptions.Max_Players;
            CB_MinTanks.SelectedItem = BossOptions.Min_Tanks;
            CB_MinHealers.SelectedItem = BossOptions.Min_Healers;
            // Offensive
            CB_MultiTargsMax.IsEnabled = (bool)(CK_MultiTargs.IsChecked = BossOptions.MultiTargs);
            CB_MultiTargsPerc.IsEnabled = (bool)(CK_MultiTargs.IsChecked = BossOptions.MultiTargs);
            CB_MultiTargsMax.Value = BossOptions.MaxNumTargets;
            CB_MultiTargsPerc.Value = BossOptions.MultiTargsPerc * 100d;
            BT_Attacks.IsEnabled = (bool)(CK_Attacks.IsChecked = BossOptions.DamagingTargs);
            BT_Attacks.Content = BossOptions.DynamicCompiler_Attacks.ToString();
            // Defensive
            NUD_Resist_Physical.Value = BossOptions.Resist_Physical * 100d;
            NUD_Resist_Frost.Value = BossOptions.Resist_Frost * 100d;
            NUD_Resist_Fire.Value = BossOptions.Resist_Fire * 100d;
            NUD_Resist_Nature.Value = BossOptions.Resist_Nature * 100d;
            NUD_Resist_Arcane.Value = BossOptions.Resist_Arcane * 100d;
            NUD_Resist_Shadow.Value = BossOptions.Resist_Shadow * 100d;
            NUD_Resist_Holy.Value = BossOptions.Resist_Holy * 100d;
            // The Impedance Checks
            BT_Moves.IsEnabled = (bool)(CK_MovingTargs.IsChecked = BossOptions.MovingTargs);
            BT_Stuns.IsEnabled = (bool)(CK_StunningTargs.IsChecked = BossOptions.StunningTargs);
            BT_Fears.IsEnabled = (bool)(CK_FearingTargs.IsChecked = BossOptions.FearingTargs);
            BT_Roots.IsEnabled = (bool)(CK_RootingTargs.IsChecked = BossOptions.RootingTargs);
            BT_Disarms.IsEnabled = (bool)(CK_DisarmTargs.IsChecked = BossOptions.DisarmingTargs);
            // The Impedance Buttons
            BT_Stuns.Content = BossOptions.DynamicCompiler_Stun.ToString();
            BT_Moves.Content = BossOptions.DynamicCompiler_Move.ToString();
            BT_Fears.Content = BossOptions.DynamicCompiler_Fear.ToString();
            BT_Roots.Content = BossOptions.DynamicCompiler_Root.ToString();
            BT_Disarms.Content = BossOptions.DynamicCompiler_Disarm.ToString();
            // Summary
            TB_BossInfo.Text = BossOptions.GenInfoString();
            //
            if (CB_BossList.SelectedIndex == -1) { CB_BossList.SelectedIndex = 0; } // Sets it to Custom
            //
            Character.OnCalculationsInvalidated();
        }

        public BossPicker()
        {
            isLoading = true;
            try {
                InitializeComponent();
                //TheScroll.SetIsMouseWheelScrollingEnabled(true);

                // Create our local Boss List
                if (bosslist == null) { bosslist = new BossList(); }
                // Populate the Boss List ComboBox
                if (CB_BossList.Items.Count < 1) { CB_BossList.Items.Add("Custom"); }
                if (CB_BossList.Items.Count < 2) {
                    foreach (string s in bosslist.GetBetterBossNamesAsArray()) { CB_BossList.Items.Add(s); }
                }
                // Set the default Filter Type
                if (CB_BL_FilterType.SelectedIndex == -1) { CB_BL_FilterType.SelectedIndex = 0; }
                // Set the Default filter to All and Populate the list based upon the Filter Type
                // E.g.- If the type is Content, the Filter List will be { "All", "T7", "T7.5",... }
                // E.g.- If the type is Version, the Filter List will be { "All", "10 Man", "25 man", "25 Man Heroic",... }
                if (CB_BL_Filter.Items.Count < 1) { CB_BL_Filter.Items.Add("All"); }
                if (CB_BL_Filter.SelectedItem == null) { CB_BL_Filter.SelectedIndex = 0; }
                bosslist.GenCalledList(BossList.FilterType.Content, (string)((ComboBoxItem)(CB_BL_Filter.SelectedItem)).Content);
                if (CB_BL_Filter.Items.Count < 2) {
                    foreach (string s in bosslist.GetFilterListAsArray((BossList.FilterType)(CB_BL_FilterType.SelectedIndex))) {
                        CB_BL_Filter.Items.Add(s);
                    }
                }

                if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                CB_BossList.Items.Add("Custom");

                foreach (string s in bosslist.GetBetterBossNamesAsArray()) { CB_BossList.Items.Add(s); }
            } catch (Exception ex) {
                new ErrorBox("Error in creating the BossTab Pane",
                    ex.Message, "BossPicker()", ex.InnerException.Message, ex.StackTrace);
            }
            isLoading = false;
        }

        /* Stolen from TalentPicker:
        // This is for saving multiple custom bosses and recalling them, doesn't do anything right now
        public bool HasCustomBoss { get; private set; }
        //private bool updating;
        private void UpdateSavedBosses()
        {
            SavedTalentSpecList savedSpecs = SavedTalentSpec.SpecsFor(Character.Class);
            SavedTalentSpec current = null;
            updating = true;
            foreach (SavedTalentSpec sts in savedSpecs)
            {
                if (sts.Equals(Character.CurrentTalents))
                {
                    current = sts;
                    break;
                }
            }

            if (current != null)
            {
                HasCustomSpec = false;
                SavedCombo.ItemsSource = savedSpecs;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Delete";
            }
            else
            {
                HasCustomSpec = true;
                current = new SavedTalentSpec("Custom", Character.CurrentTalents, Tree1.Points(), Tree2.Points(), Tree3.Points());
                SavedTalentSpecList currentList = new SavedTalentSpecList();
                currentList.AddRange(savedSpecs);
                currentList.Add(current);
                SavedCombo.ItemsSource = null;
                SavedCombo.ItemsSource = currentList;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Save";
            }
            updating = false;
        }
        private void SaveDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            /*SavedTalentSpec currentSpec = SavedCombo.SelectedItem as SavedTalentSpec;
            if (HasCustomSpec)
            {
                SaveTalentSpecDialog dialog = new SaveTalentSpecDialog(currentSpec.TalentSpec(),
                    currentSpec.Tree1, currentSpec.Tree2, currentSpec.Tree3);
                dialog.Closed += new EventHandler(dialog_Closed);
                dialog.Show();
            }
            else
            {
                SavedTalentSpec.AllSpecs.Remove(currentSpec);
                UpdateSavedSpecs();
            }
        }
        private void dialog_Closed(object sender, EventArgs e)
        {
            if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateSavedSpecs();
            }
        }
        private void SavedCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updating)
            {
                SavedTalentSpec newSpec = SavedCombo.SelectedItem as SavedTalentSpec;
                Character.CurrentTalents = newSpec.TalentSpec();
                character_ClassChanged(this, EventArgs.Empty);
                Character.OnCalculationsInvalidated();
            }
        }
        public void BossAspectsChanged(object sender, EventArgs e)
        {
            UpdateSavedBosses();
            Character.OnCalculationsInvalidated();
        }
        public void RefreshBoss() { character_ClassChanged(this, EventArgs.Empty); }
        private void character_ClassChanged(object sender, EventArgs e)
        {
            UpdateSavedBosses();
        }
        */

        // Boss Selector
        private void CB_BL_FilterType_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (!isLoading) {
                    isLoading = true;
                    // Use Filter Type Box to adjust Filter Box
                    if (CB_BL_Filter.Items.Count > 0) { CB_BL_Filter.Items.Clear(); }
                    if (CB_BL_Filter.Items.Count < 1) { CB_BL_Filter.Items.Add("All"); }
                    CB_BL_Filter.SelectedItem = "All";
                    BossList.FilterType ftype = (BossList.FilterType)(CB_BL_FilterType.SelectedIndex);
                    bosslist.GenCalledList(ftype, CB_BL_Filter.SelectedItem.ToString());
                    foreach (string s in bosslist.GetFilterListAsArray(ftype)) { CB_BL_Filter.Items.Add(s); }
                    CB_BL_Filter.SelectedItem = "All";
                    // Now edit the Boss List to the new filtered list of bosses
                    if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                    CB_BossList.Items.Add("Custom");
                    foreach (string s in bosslist.GetBetterBossNamesAsArray()) { CB_BossList.Items.Add(s); }
                    CB_BossList.SelectedItem = "Custom";
                    // Save the new names
                    if (!firstload) {
                        BossOptions.FilterType = (BossList.FilterType)((ComboBoxItem)(CB_BL_FilterType.SelectedItem)).Content;
                        BossOptions.Filter = CB_BL_Filter.SelectedItem.ToString();
                        BossOptions.BossName = CB_BossList.SelectedItem.ToString();
                    }
                    //
                    Character.OnCalculationsInvalidated();
                    isLoading = false;
                }
            }catch (Exception ex){
                new ErrorBox("Error in the Boss Options",
                    ex.Message, "CB_BL_FilterType_SelectedIndexChanged()",
                    "No Additional Info", ex.StackTrace);
            }
        }
        private void CB_BL_Filter_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (!isLoading) {
                    isLoading = true;
                    // Use Filter Type Box to adjust Filter Box
                    BossList.FilterType ftype = (BossList.FilterType)(CB_BL_FilterType.SelectedIndex);
                    //ComboBoxItem a = (ComboBoxItem)CB_BL_Filter.SelectedItem;
                    bosslist.GenCalledList(ftype, CB_BL_Filter.SelectedItem.ToString());//a.Content.ToString());
                    // Now edit the Boss List to the new filtered list of bosses
                    if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                    CB_BossList.Items.Add("Custom");
                    foreach (string s in bosslist.GetBetterBossNamesAsArray()) { CB_BossList.Items.Add(s); }
                    CB_BossList.SelectedItem = "Custom";
                    // Save the new names
                    if (!firstload) {
                        BossOptions.FilterType = (BossList.FilterType)CB_BL_FilterType.SelectedItem;
                        BossOptions.Filter = CB_BL_Filter.SelectedItem.ToString();
                        BossOptions.BossName = CB_BossList.SelectedItem.ToString();
                    }
                    //
                    Character.OnCalculationsInvalidated();
                    isLoading = false;
                }
            }catch (Exception ex){
                new ErrorBox("Error in the Boss Options",
                    ex.Message, "CB_BL_Filter_SelectedIndexChanged()",
                    "No Additional Info", ex.StackTrace);
            }
        }
        private void CB_BossList_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            string addInfo = "No Additional Info";
            try {
                if (!isLoading) {
                    addInfo = "Not Loading";
                    if (CB_BossList.SelectedIndex != 0) { // "Custom"
                        addInfo += "\r\nCB_BossList.SelectedIndex != 0";
                        isLoading = true;
                        // Get Values
                        BossHandler boss = bosslist.GetBossFromBetterName(CB_BossList.SelectedItem.ToString()).Clone(); // "T7 : Naxxramas : 10 man : Patchwerk"
                        BossOptions.CloneThis(boss);
                        addInfo += "\r\nBoss Info Set";

                        // Set Controls to those Values
                        TB_BossInfo.Text = boss.GenInfoString();

                        // Save the new names
                        if (!firstload) {
                            addInfo += "\r\n!firstlost";
                            BossOptions.FilterType = (BossList.FilterType)CB_BL_FilterType.SelectedItem;
                            BossOptions.Filter = CB_BL_Filter.SelectedItem.ToString();
                            BossOptions.BossName = CB_BossList.SelectedItem.ToString();
                        }
                        isLoading = false;
                    } else {
                        addInfo += "\r\nCB_BossList.SelectedIndex == 0";
                        isLoading = true;
                        BossHandler boss = new BossHandler();
                        //
                        boss.Name = "Custom";
                        BossOptions.BossName = boss.Name;
                        //
                        TB_BossInfo.Text = boss.GenInfoString();
                        isLoading = false;
                    }
                }
            } catch (Exception ex) {
                new ErrorBox("Error in setting BossPicker Character settings from Boss",
                    ex.Message, "CB_BossList_SelectedIndexChanged()",
                    addInfo, ex.StackTrace);
            }
            isLoading = false;
            bossOpts_PropertyChanged(null, null);
        }

        // Impedences
        DG_BossSitChanges Box = null;
        private void BT_Stuns_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Stuns, DG_BossSitChanges.Flags.Stun);
            Box.Closed += new EventHandler(DG_BossSitChanges_Closed);
            Box.Show();
        }
        private void BT_Moves_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Moves, DG_BossSitChanges.Flags.Move);
            Box.Closed += new EventHandler(DG_BossSitChanges_Closed);
            Box.Show();
        }
        private void BT_Fears_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Fears, DG_BossSitChanges.Flags.Fear);
            Box.Closed += new EventHandler(DG_BossSitChanges_Closed);
            Box.Show();
        }
        private void BT_Roots_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Roots, DG_BossSitChanges.Flags.Root);
            Box.Closed += new EventHandler(DG_BossSitChanges_Closed);
            Box.Show();
        }
        private void BT_Disarms_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Disarms, DG_BossSitChanges.Flags.Disarm);
            Box.Closed += new EventHandler(DG_BossSitChanges_Closed);
            Box.Show();
        }
        private void DG_BossSitChanges_Closed(object sender, EventArgs e)
        {
            if ((bool)Box.DialogResult)
            {
                switch (Box.Flag) {
                    case DG_BossSitChanges.Flags.Stun: { Character.BossOptions.Stuns = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Move: { Character.BossOptions.Moves = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Fear: { Character.BossOptions.Fears = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Root: { Character.BossOptions.Roots = Box.TheList; break; }
                    default: { Character.BossOptions.Disarms = Box.TheList; break; }
                }
                bossOpts_PropertyChanged(null, null);
            }
        }

        // Offensive
        DG_BossAttacks BoxA = null;
        private void BT_Attacks_Click(object sender, RoutedEventArgs e)
        {
            BoxA = new DG_BossAttacks(Character.BossOptions.Attacks);
            BoxA.Closed += new EventHandler(DG_BossAttacks_Closed);
            BoxA.Show();
        }
        private void DG_BossAttacks_Closed(object sender, EventArgs e)
        {
            if ((bool)BoxA.DialogResult)
            {
                Character.BossOptions.Attacks = BoxA.TheList;
                bossOpts_PropertyChanged(null, null);
            }
        }
    }
}
