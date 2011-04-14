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
                    _char.ClassChanged -= new EventHandler(character_ClassChanged);
                    _char.CalculationsInvalidated -= new EventHandler(character_CalculationsInvalidated);
                }
                _char = value;
                if (_char != null) {
                    _char.ClassChanged += new EventHandler(character_ClassChanged);
                    _char.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                    BossOptions = Character.BossOptions;
                    character_ClassChanged(this, EventArgs.Empty);
                    character_CalculationsInvalidated(this, EventArgs.Empty);
                }
            }
        }

        private void character_CalculationsInvalidated(object sender, EventArgs e) {
            if(BossOptions == null) return;
            TB_BossInfo.Text = BossOptions.GenInfoString(Character);
        }

        public void bossOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isLoading) { return; }
            isLoading = true;
            // Basics
            CB_Level.SelectedItem = BossOptions.Level;
            CB_Armor.SelectedItem = BossOptions.Armor;
            CB_MobType.SelectedIndex = BossOptions.MobType;
            NUD_Duration.Value = BossOptions.BerserkTimer;
            NUD_DurationSpeed.Value = BossOptions.SpeedKillTimer;
            NUD_TargHP.Value = BossOptions.Health;
            NUD_Under35Perc.Value = BossOptions.Under35Perc * 100;
            NUD_Under20Perc.Value = BossOptions.Under20Perc * 100;
            CB_InBackPerc_Melee.IsEnabled = (bool)(CK_InBack.IsChecked = BossOptions.InBack);
            CB_InBackPerc_Melee.Value = BossOptions.InBackPerc_Melee * 100d;
            CB_InBackPerc_Ranged.IsEnabled = (bool)(CK_InBack.IsChecked = BossOptions.InBack);
            CB_InBackPerc_Ranged.Value = BossOptions.InBackPerc_Ranged * 100d;
            CB_MaxPlayers.SelectedItem = BossOptions.Max_Players;
            CB_MinTanks.SelectedItem = BossOptions.Min_Tanks;
            CB_MinHealers.SelectedItem = BossOptions.Min_Healers;
            // Offensive
            BT_MultiTargs.IsEnabled = /*LB_MultiTargs.IsEnabled =*/ (bool)(CK_MultiTargs.IsChecked = (bool?)BossOptions.MultiTargs);
            BT_Attacks.IsEnabled = /*LB_Attacks.IsEnabled =*/ (bool)(CK_Attacks.IsChecked = (bool?)BossOptions.DamagingTargs);
            BT_BuffStates.IsEnabled = /*LB_BuffStates.IsEnabled =*/ (bool)(CK_BuffStates.IsChecked = (bool?)BossOptions.HasBuffStates);
            LB_MultiTargs.Text = BossOptions.DynamicString_MultiTargs.ToString();
            LB_Attacks.Text = BossOptions.DynamicString_Attacks.ToString();
            LB_BuffStates.Text = BossOptions.DynamicString_BuffStates.ToString();
            // Defensive
            NUD_Resist_Physical.Value = BossOptions.Resist_Physical * 100d;
            NUD_Resist_Frost.Value = BossOptions.Resist_Frost * 100d;
            NUD_Resist_Fire.Value = BossOptions.Resist_Fire * 100d;
            NUD_Resist_Nature.Value = BossOptions.Resist_Nature * 100d;
            NUD_Resist_Arcane.Value = BossOptions.Resist_Arcane * 100d;
            NUD_Resist_Shadow.Value = BossOptions.Resist_Shadow * 100d;
            NUD_Resist_Holy.Value = BossOptions.Resist_Holy * 100d;
            // The Impedance Checks
            BT_Stuns.IsEnabled = /*LB_Stuns.IsEnabled =*/ (bool)(CK_Stuns.IsChecked = BossOptions.StunningTargs);
            BT_Moves.IsEnabled = /*LB_Moves.IsEnabled =*/ (bool)(CK_Moves.IsChecked = BossOptions.MovingTargs);
            BT_Fears.IsEnabled = /*LB_Fears.IsEnabled =*/ (bool)(CK_Fears.IsChecked = BossOptions.FearingTargs);
            BT_Roots.IsEnabled = /*LB_Roots.IsEnabled =*/ (bool)(CK_Roots.IsChecked = BossOptions.RootingTargs);
            BT_Slncs.IsEnabled = /*LB_Slncs.IsEnabled =*/ (bool)(CK_Slncs.IsChecked = BossOptions.SilencingTargs);
            BT_Dsrms.IsEnabled = /*LB_Dsrms.IsEnabled =*/ (bool)(CK_Dsrms.IsChecked = BossOptions.DisarmingTargs);
            // The Impedance Buttons
            LB_Stuns.Text = BossOptions.DynamicString_Stun.ToString();
            LB_Moves.Text = BossOptions.DynamicString_Move.ToString();
            LB_Fears.Text = BossOptions.DynamicString_Fear.ToString();
            LB_Roots.Text = BossOptions.DynamicString_Root.ToString();
            LB_Slncs.Text = BossOptions.DynamicString_Slnc.ToString();
            LB_Dsrms.Text = BossOptions.DynamicString_Dsrm.ToString();
            // Summary
            TB_BossInfo.Text = BossOptions.GenInfoString(Character);
            if (origbrush == null) { origbrush = TB_BossInfo.Foreground; }
            if (TB_BossInfo.Text.Contains("ALERT")) {
                TB_BossInfo.Foreground = new SolidColorBrush(Colors.Red);
            } else {
                TB_BossInfo.Foreground = origbrush;
            }
            //
            if (CB_BossList.SelectedIndex == -1) { CB_BossList.SelectedIndex = 0; } // Sets it to Custom
            isLoading = false;
            //
            Character.OnCalculationsInvalidated();
        }
        private Brush origbrush = null;

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
                bosslist.GenCalledList(BossList.FilterType.Content, CB_BL_Filter.SelectedItem as String);
                if (CB_BL_Filter.Items.Count < 2) {
                    foreach (string s in bosslist.GetFilterListAsArray((BossList.FilterType)(CB_BL_FilterType.SelectedIndex))) {
                        CB_BL_Filter.Items.Add(s);
                    }
                }

                if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                CB_BossList.Items.Add("Custom");

                foreach (string s in bosslist.GetBetterBossNamesAsArray()) { CB_BossList.Items.Add(s); }
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in creating the BossTab Pane",
                    Function = "BossPicker()",
                    TheException = ex,
                }.Show();
            }
            isLoading = false;
        }

        private void character_ClassChanged(object sender, EventArgs e)
        {
            //UpdateSavedBosses();

            // The following code is a system by which we can hide UI for options that a model doesn't implement

            LB_Level.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Level"] ? Visibility.Visible : Visibility.Collapsed;
            CB_Level.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Level"] ? Visibility.Visible : Visibility.Collapsed;

            LB_Armor.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Armor"] ? Visibility.Visible : Visibility.Collapsed;
            CB_Armor.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Armor"] ? Visibility.Visible : Visibility.Collapsed;

            LB_MobType.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["MobType"] ? Visibility.Visible : Visibility.Collapsed;
            CB_MobType.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["MobType"] ? Visibility.Visible : Visibility.Collapsed;

            LB_Duration.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;
            NUD_Duration.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Duration2.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;
            LB_DurationSpeed.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;
            NUD_DurationSpeed.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Duration2Speed.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Timers"] ? Visibility.Visible : Visibility.Collapsed;

            LB_TargHP.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Health"] ? Visibility.Visible : Visibility.Collapsed;
            NUD_TargHP.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Health"] ? Visibility.Visible : Visibility.Collapsed;

            LB_Under.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["TimeSub35"]
                || BossOptions.MyModelSupportsThis[Character.CurrentModel]["TimeSub20"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Under35Perc.Visibility  = 
            NUD_Under35Perc.Visibility = 
            LB_Under35Perc2.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["TimeSub35"] ? Visibility.Visible : Visibility.Collapsed;

            LB_Under20Perc.Visibility  = 
            NUD_Under20Perc.Visibility = 
            LB_Under20Perc2.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["TimeSub20"] ? Visibility.Visible : Visibility.Collapsed;

            CK_InBack.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Melee"]
                                || BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Ranged"]
                    ? Visibility.Visible : Visibility.Collapsed;
            CB_InBackPerc_Melee.Visibility  = BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Melee"] ? Visibility.Visible : Visibility.Collapsed;
            CB_InBackPerc_Ranged.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Ranged"] ? Visibility.Visible : Visibility.Collapsed;
            LB_InBehindPerc.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Melee"]
                                      || BossOptions.MyModelSupportsThis[Character.CurrentModel]["InBack_Ranged"]
                    ? Visibility.Visible : Visibility.Collapsed;

            LB_MaxPlayers.Visibility = 
            CB_MaxPlayers.Visibility = 
            LB_MinTanks.Visibility   = 
            CB_MinTanks.Visibility   = 
            LB_MinHealers.Visibility = 
            CB_MinHealers.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["RaidSize"] ? Visibility.Visible : Visibility.Collapsed;

            // Offensive
            LB_MultiTargs.Visibility = 
            CK_MultiTargs.Visibility = 
            BT_MultiTargs.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["TargetGroups"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Attacks.Visibility    = 
            CK_Attacks.Visibility    = 
            BT_Attacks.Visibility    = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Attacks"] ? Visibility.Visible : Visibility.Collapsed;
            LB_BuffStates.Visibility = 
            CK_BuffStates.Visibility = 
            BT_BuffStates.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["BuffStates"] ? Visibility.Visible : Visibility.Collapsed;

            // Defensive
            Tab_Def.Visibility             = 
            LB_Resistances.Visibility      = 
            LB_Resist_Physical.Visibility  = 
            NUD_Resist_Physical.Visibility = 
            LB_Resist_Physical2.Visibility = 
            LB_Resist_Frost.Visibility     = 
            NUD_Resist_Frost.Visibility    = 
            LB_Resist_Fire.Visibility      = 
            NUD_Resist_Fire.Visibility     = 
            LB_Resist_Nature.Visibility    = 
            NUD_Resist_Nature.Visibility   = 
            LB_Resist_Arcane.Visibility    = 
            NUD_Resist_Arcane.Visibility   = 
            LB_Resist_Shadow.Visibility    = 
            NUD_Resist_Shadow.Visibility   = 
            LB_Resist_Holy.Visibility      = 
            NUD_Resist_Holy.Visibility     = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Defensive"] ? Visibility.Visible : Visibility.Collapsed;

            // Impedances
            Tab_Imp.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Moves"]
                              || BossOptions.MyModelSupportsThis[Character.CurrentModel]["Stuns"]
                              || BossOptions.MyModelSupportsThis[Character.CurrentModel]["Fears"]
                              || BossOptions.MyModelSupportsThis[Character.CurrentModel]["Roots"]
                              || BossOptions.MyModelSupportsThis[Character.CurrentModel]["Disarms"]
                               ? Visibility.Visible : Visibility.Collapsed;
            LB_Moves.Visibility =
            CK_Moves.Visibility =
            BT_Moves.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Moves"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Stuns.Visibility =
            CK_Stuns.Visibility =
            BT_Stuns.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Stuns"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Fears.Visibility =
            CK_Fears.Visibility =
            BT_Fears.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Fears"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Roots.Visibility =
            CK_Roots.Visibility =
            BT_Roots.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Roots"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Slncs.Visibility =
            CK_Slncs.Visibility =
            BT_Slncs.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Silences"] ? Visibility.Visible : Visibility.Collapsed;
            LB_Dsrms.Visibility =
            CK_Dsrms.Visibility =
            BT_Dsrms.Visibility = BossOptions.MyModelSupportsThis[Character.CurrentModel]["Disarms"] ? Visibility.Visible : Visibility.Collapsed;
        }

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
                new Base.ErrorBox()
                {
                    Title = "Error in the Boss Options",
                    Function = "CB_BL_FilterType_SelectedIndexChanged()",
                    TheException = ex,
                }.Show();
                isLoading = false;
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
                        BossOptions.FilterType = (BossList.FilterType)CB_BL_FilterType.SelectedIndex;
                        BossOptions.Filter = CB_BL_Filter.SelectedItem.ToString();
                        BossOptions.BossName = CB_BossList.SelectedItem.ToString();
                    }
                    //
                    Character.OnCalculationsInvalidated();
                    isLoading = false;
                }
            }catch (Exception ex){
                new Base.ErrorBox()
                {
                    Title = "Error in the Boss Options",
                    Function = "CB_BL_Filter_SelectedIndexChanged()",
                    TheException = ex,
                }.Show();
                isLoading = false;
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
                        TB_BossInfo.Text = boss.GenInfoString(Character);

                        // Save the new names
                        if (!firstload) {
                            addInfo += "\r\n!firstlost";
                            BossOptions.FilterType = (BossList.FilterType)CB_BL_FilterType.SelectedIndex;
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
                        TB_BossInfo.Text = boss.GenInfoString(Character);
                        isLoading = false;
                    }
                    bossOpts_PropertyChanged(null, null);
                }
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in setting BossPicker Character settings from Boss",
                    Function = "CB_BossList_SelectedIndexChanged()",
                    TheException = ex,
                }.Show();
                isLoading = false;
            }
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
        private void BT_Silences_Click(object sender, RoutedEventArgs e)
        {
            Box = new DG_BossSitChanges(Character.BossOptions.Silences, DG_BossSitChanges.Flags.Silence);
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
                switch (Box.Flag) {case DG_BossSitChanges.Flags.Stun: { Character.BossOptions.Stuns = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Move: { Character.BossOptions.Moves = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Fear: { Character.BossOptions.Fears = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Root: { Character.BossOptions.Roots = Box.TheList; break; }
                    case DG_BossSitChanges.Flags.Silence: { Character.BossOptions.Silences = Box.TheList; break; }
                    default: { Character.BossOptions.Disarms = Box.TheList; break; }
                }
                bossOpts_PropertyChanged(null, null);
            }
        }

        // Offensive
        DG_BossAttacks BoxA = null;
        DG_BossTargetGroups BoxB = null;
        DG_BossBuffStates BoxC = null;
        private void BT_Attacks_Click(object sender, RoutedEventArgs e)
        {
            BoxA = new DG_BossAttacks(Character.BossOptions.Attacks);
            BoxA.Closed += new EventHandler(DG_BossAttacks_Closed);
            BoxA.Show();
        }
        private void BT_MultiTargs_Click(object sender, RoutedEventArgs e)
        {
            BoxB = new DG_BossTargetGroups(Character.BossOptions.Targets);
            BoxB.Closed += new EventHandler(DG_MultiTargs_Closed);
            BoxB.Show();
        }
        private void BT_BuffStates_Click(object sender, RoutedEventArgs e)
        {
            BoxC = new DG_BossBuffStates(Character.BossOptions.BuffStates);
            BoxC.Closed += new EventHandler(DG_BuffStates_Closed);
            BoxC.Show();
        }
        private void DG_BossAttacks_Closed(object sender, EventArgs e)
        {
            if ((bool)BoxA.DialogResult)
            {
                Character.BossOptions.Attacks = BoxA.TheList;
                bossOpts_PropertyChanged(null, null);
            }
        }
        private void DG_MultiTargs_Closed(object sender, EventArgs e)
        {
            if ((bool)BoxB.DialogResult)
            {
                Character.BossOptions.Targets = BoxB.TheList;
                bossOpts_PropertyChanged(null, null);
            }
        }
        private void DG_BuffStates_Closed(object sender, EventArgs e)
        {
            if ((bool)BoxC.DialogResult)
            {
                Character.BossOptions.BuffStates = BoxC.TheList;
                bossOpts_PropertyChanged(null, null);
            }
        }

        private void BT_GetHelp_Click(object sender, RoutedEventArgs e)
        {
            WebHelp webHelp = new WebHelp("BossHandler");
            webHelp.Show();
        }
    }
}
