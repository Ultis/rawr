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
        private BossHandler _BossOptions;
        public BossHandler BossOptions {
            get { return _BossOptions; }
            set {
                if (_BossOptions != null) {
                    _BossOptions.PropertyChanged -= new PropertyChangedEventHandler(bossOpts_PropertyChanged);
                }
                _BossOptions = value;
                DataContext = _BossOptions;
                CB_Level.SelectedIndex = _BossOptions.Level - 80;
                for (int i=0; i < 4; i++) {
                    if(_BossOptions.Armor == StatConversion.NPC_ARMOR[i]) {
                        CB_Armor.SelectedIndex = i;
                        break;
                    }
                }
                _BossOptions.PropertyChanged += new PropertyChangedEventHandler(bossOpts_PropertyChanged);
                bossOpts_PropertyChanged(this, null);
            }
        }

        private Character _char;
        public Character Character {
            get { return _char; }
            set {
                if (_char != null) {
                    _char.ClassChanged -= new EventHandler(character_ClassChanged);
                }
                _char = value;
                if (_char != null) {
                    _char.ClassChanged += new EventHandler(character_ClassChanged);
                    character_ClassChanged(this, EventArgs.Empty);
                    BossOptions = Character.BossOptions;
                }
            }
        }

        public void RefreshBoss() { character_ClassChanged(this, EventArgs.Empty); }
        private void character_ClassChanged(object sender, EventArgs e)
        {
            UpdateSavedBosses();
        }

        private bool isLoading = false;
        public void bossOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Target Armor/Level
            if (!isLoading && CB_Level.SelectedIndex == -1) { CB_Level.SelectedIndex = 0; }
            if (!isLoading && CB_Armor.SelectedIndex == -1) { CB_Armor.SelectedIndex = 0; }
            // Fix the enables
            CB_InBackPerc.IsEnabled = (bool)CK_InBack.IsChecked;
            CB_MultiTargsMax.IsEnabled = (bool)CK_MultiTargs.IsChecked;
            CB_MultiTargsPerc.IsEnabled = (bool)CK_MultiTargs.IsChecked;
            BT_Moves.IsEnabled = (bool)CK_MovingTargs.IsChecked;
            BT_Stuns.IsEnabled = (bool)CK_StunningTargs.IsChecked;
            BT_Fears.IsEnabled = (bool)CK_FearingTargs.IsChecked;
            BT_Roots.IsEnabled = (bool)CK_RootingTargs.IsChecked;
            BT_Disarms.IsEnabled = (bool)CK_DisarmTargs.IsChecked;
            BT_Attacks.IsEnabled = (bool)CK_Attacks.IsChecked;
            //
            Character.OnCalculationsInvalidated();
        }

        public BossPicker()
        {
            InitializeComponent();
        }

        public void BossAspectsChanged(object sender, EventArgs e)
        {
            UpdateSavedBosses();
            Character.OnCalculationsInvalidated();
        }

        // Stolen from TalentPicker:
        // This is for saving multiple custom bosses and
        // recalling them, doesn't do anything right now
        public bool HasCustomBoss { get; private set; }

        //private bool updating;
        private void UpdateSavedBosses()
        {
            /*SavedTalentSpecList savedSpecs = SavedTalentSpec.SpecsFor(Character.Class);
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
            updating = false;*/
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
            }*/
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            /*if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateSavedSpecs();
            }*/
        }

        private void SavedCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (!updating)
            {
                SavedTalentSpec newSpec = SavedCombo.SelectedItem as SavedTalentSpec;
                Character.CurrentTalents = newSpec.TalentSpec();
                character_ClassChanged(this, EventArgs.Empty);
                Character.OnCalculationsInvalidated();
            }*/
        }

        private void CB_Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BossOptions.Level = 80 + CB_Level.SelectedIndex;
        }

        private void CB_Armor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BossOptions.Armor = int.Parse((string)((ComboBoxItem)(CB_Armor.SelectedItem)).Content);
        }

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

        private void BT_Attacks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DG_BossSitChanges_Closed(object sender, EventArgs e)
        {
            if ((bool)Box.DialogResult)
            {
                switch (Box.Flag) {
                    case DG_BossSitChanges.Flags.Stun: {
                        Character.BossOptions.Stuns = Box.TheList;
                        BT_Stuns.Content = Character.BossOptions.Stuns.Count == 0 ?
                            "None" : Character.BossOptions.DynamicCompiler_Stun.ToString();
                        break;
                    }
                    case DG_BossSitChanges.Flags.Move: {
                        Character.BossOptions.Moves = Box.TheList;
                        BT_Moves.Content = Character.BossOptions.Moves.Count == 0 ?
                            "None" : Character.BossOptions.DynamicCompiler_Move.ToString();
                        break;
                    }
                    case DG_BossSitChanges.Flags.Fear: {
                        Character.BossOptions.Fears = Box.TheList;
                        BT_Fears.Content = Character.BossOptions.Fears.Count == 0 ?
                            "None" : Character.BossOptions.DynamicCompiler_Fear.ToString();
                        break;
                    }
                    case DG_BossSitChanges.Flags.Root: {
                        Character.BossOptions.Roots = Box.TheList;
                        BT_Roots.Content = Character.BossOptions.Roots.Count == 0 ?
                            "None" : Character.BossOptions.DynamicCompiler_Root.ToString();
                        break;
                    }
                    default: {
                        Character.BossOptions.Disarms = Box.TheList;
                        BT_Disarms.Content = Character.BossOptions.Disarms.Count == 0 ?
                            "None" : Character.BossOptions.DynamicCompiler_Disarm.ToString();
                        break;
                    }
                }
                //CB_BossList_SelectedIndexChanged(null, null);
            }
        }
    }
}
