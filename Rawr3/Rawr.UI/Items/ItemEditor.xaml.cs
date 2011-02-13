using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class ItemEditor : ChildWindow
    {
        private Stats clonedStats;
        private Item currentItem;
        public Item CurrentItem
        {
            get { return currentItem; }
            set
            {
                currentItem = value;
                DataContext = currentItem;

                Title = "Item Editor - " + currentItem.Name;

                clonedStats = currentItem.Stats.Clone();
                ItemStats.CurrentStats = clonedStats;
                UpdateEffectList();

                TypeCombo.SelectedIndex = (int)CurrentItem.Type;
                SlotCombo.SelectedIndex = (int)CurrentItem.Slot;
                BindCombo.SelectedIndex = (int)CurrentItem.Bind;
                QualityCombo.SelectedIndex = (int)CurrentItem.Quality;
                DamageTypeComboBox.SelectedIndex = (int)CurrentItem.DamageType;

                CostText.Value = (double)CurrentItem.Cost;

                MinDamageNum.Value = (double)CurrentItem.MinDamage;
                MaxDamageNum.Value = (double)CurrentItem.MaxDamage;
                IlvlNum.Value = (double)CurrentItem.ItemLevel;

                if (currentItem.SocketColor1 == ItemSlot.Meta) Gem1Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor1 == ItemSlot.Red) Gem1Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor1 == ItemSlot.Yellow) Gem1Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor1 == ItemSlot.Blue) Gem1Combo.SelectedIndex = 4;
                else if (currentItem.SocketColor1 == ItemSlot.Cogwheel) Gem1Combo.SelectedIndex = 5;
                else if (currentItem.SocketColor1 == ItemSlot.Hydraulic) Gem1Combo.SelectedIndex = 6;
                else if (currentItem.SocketColor1 == ItemSlot.Prismatic) Gem1Combo.SelectedIndex = 7;
                else Gem1Combo.SelectedIndex = 0;
                if (currentItem.SocketColor2 == ItemSlot.Meta) Gem2Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor2 == ItemSlot.Red) Gem2Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor2 == ItemSlot.Yellow) Gem2Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor2 == ItemSlot.Blue) Gem2Combo.SelectedIndex = 4;
                else if (currentItem.SocketColor2 == ItemSlot.Cogwheel) Gem2Combo.SelectedIndex = 5;
                else if (currentItem.SocketColor2 == ItemSlot.Hydraulic) Gem2Combo.SelectedIndex = 6;
                else if (currentItem.SocketColor2 == ItemSlot.Prismatic) Gem2Combo.SelectedIndex = 7;
                else Gem2Combo.SelectedIndex = 0;
                if (currentItem.SocketColor3 == ItemSlot.Meta) Gem3Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor3 == ItemSlot.Red) Gem3Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor3 == ItemSlot.Yellow) Gem3Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor3 == ItemSlot.Blue) Gem3Combo.SelectedIndex = 4;
                else if (currentItem.SocketColor3 == ItemSlot.Cogwheel) Gem3Combo.SelectedIndex = 5;
                else if (currentItem.SocketColor3 == ItemSlot.Hydraulic) Gem3Combo.SelectedIndex = 6;
                else if (currentItem.SocketColor3 == ItemSlot.Prismatic) Gem3Combo.SelectedIndex = 7;
                else Gem3Combo.SelectedIndex = 0;

                if (currentItem.Faction == ItemFaction.Neutral) CB_Faction.SelectedIndex = 0;
                else if (currentItem.Faction == ItemFaction.Alliance) CB_Faction.SelectedIndex = 1;
                else if (currentItem.Faction == ItemFaction.Horde) CB_Faction.SelectedIndex = 2;

                /* This check shouldn't need to happen
                if (currentItem.LocationInfo != null && currentItem.LocationInfo.Count > 1 && currentItem.LocationInfo[1] == null)
                {
                    currentItem.LocationInfo = new ItemLocationList() { currentItem.LocationInfo[0], };
                }*/

                /*currentItem.LocationInfo.RemoveAll(null);
                for (int i = 0; i < currentItem.LocationInfo.Count; )
                {
                    if (currentItem.LocationInfo[i] == null) {
                    } else {
                        i++;
                    }
                }*/
                UpdateSourcesBox(currentItem.LocationInfo);

                foreach (CheckBox cb in ClassCheckBoxes.Values) cb.IsChecked = false;
                if (!string.IsNullOrEmpty(currentItem.RequiredClasses))
                {
                    foreach (string c in currentItem.RequiredClasses.Split('|'))
                    {
                        CheckBox checkBox;
                        if (ClassCheckBoxes.TryGetValue(c, out checkBox))
                        {
                            checkBox.IsChecked = true;
                        }
                    }
                }

                var nonZeroStats = currentItem.SocketBonus.Values(x => x != 0);
                bool statFound = false;
                foreach (PropertyInfo info in nonZeroStats.Keys)
                {
                    BonusAmount.Text = nonZeroStats[info].ToString();
                    BonusStat.Tag = info;
                    BonusStat.SelectedItem = Extensions.DisplayName(info);
                    statFound = true;
                    break;
                }
                if (!statFound)
                {
                    PropertyInfo info = typeof(Stats).GetProperty("Stamina");
                    BonusAmount.Text = ((float)info.GetGetMethod().Invoke(CurrentItem.SocketBonus, null)).ToString();
                    BonusStat.Tag = info;
                    BonusStat.SelectedItem = Extensions.DisplayName(info);
                }
            }
        }

        private bool SourcesChanged = false;

        private void UpdateEffectList()
        {
            SpecialEffectList.Items.Clear();
            foreach (SpecialEffect eff in clonedStats.SpecialEffects())
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = eff.ToString();
                cbi.Tag = eff;
                SpecialEffectList.Items.Add(cbi);
            }
            if (SpecialEffectList.Items.Count > 0)
            {
                SpecialEffectList.IsEnabled = true;
                EditSpecialButton.IsEnabled = true;
                DeleteSpecialButton.IsEnabled = true;
                SpecialEffectList.SelectedIndex = 0;
            }
            else
            {
                SpecialEffectList.IsEnabled = false;
                EditSpecialButton.IsEnabled = false;
                DeleteSpecialButton.IsEnabled = false;
            }
        }

        public void Show(Item item)
        {
            CurrentItem = item;
            Show();
        }

        private Dictionary<string, CheckBox> ClassCheckBoxes;
        public ItemEditor()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            TypeCombo.ItemsSource = EnumHelper.GetValues<ItemType>().Select(e => e.ToString());
            QualityCombo.ItemsSource = EnumHelper.GetValues<ItemQuality>().Where(iq => iq != ItemQuality.Temp).Select(e => e.ToString());
            SlotCombo.ItemsSource = EnumHelper.GetValues<ItemSlot>().Select(e => e.ToString());
            DamageTypeComboBox.ItemsSource = EnumHelper.GetValues<ItemDamageType>().Select(e => e.ToString());
            BonusStat.ItemsSource = Stats.StatNames;

            ClassCheckBoxes = new Dictionary<string, CheckBox>();
            ClassCheckBoxes["DeathKnight"] = DeathKnightCheckBox;
            ClassCheckBoxes["Druid"] = DruidCheckBox;
            ClassCheckBoxes["Hunter"] = HunterCheckBox;
            ClassCheckBoxes["Mage"] = MageCheckBox;
            ClassCheckBoxes["Paladin"] = PaladinCheckBox;
            ClassCheckBoxes["Priest"] = PriestCheckBox;
            ClassCheckBoxes["Rogue"] = RogueCheckBox;
            ClassCheckBoxes["Shaman"] = ShamanCheckBox;
            ClassCheckBoxes["Warlock"] = WarlockCheckBox;
            ClassCheckBoxes["Warrior"] = WarriorCheckBox;
        }

        #region Item Source
        private void BT_ItemSourceEdit_Click(object sender, RoutedEventArgs e)
        {
            ItemSourceEditor itemSourceEditor = new ItemSourceEditor(CurrentItem);
            itemSourceEditor.Closed += new EventHandler(itemSourceEditor_Closed);
            itemSourceEditor.Show();
        }
        ItemLocationList tempSources = null;
        public void itemSourceEditor_Closed(object sender, EventArgs e)
        {
            if ((sender as ItemSourceEditor).DialogResult.GetValueOrDefault(false))
            {
                if (CurrentItem.LocationInfo != (sender as ItemSourceEditor).ItemSources)
                {
                    tempSources = (sender as ItemSourceEditor).ItemSources;
                    UpdateSourcesBox(tempSources);
                    SourcesChanged = true;
                }
            }
        }
        private void UpdateSourcesBox(ItemLocationList newList) {
            if (newList != null && newList.Count > 0) {
                if (newList.Count > 0) {
                    TB_Source.Text = TB_SourceNote.Text = "";
                    foreach (ItemLocation il in newList) {
                        if (il == null) { continue; }
                        TB_Source.Text += il.Description + "\n";
                        TB_SourceNote.Text += il.Note + "\n";
                    }
                    TB_Source.Text = TB_Source.Text.TrimEnd('\n');
                    TB_SourceNote.Text = TB_SourceNote.Text.TrimEnd('\n');
                } else if (newList[0] != null){
                    TB_Source.Text = newList[0].Description;
                    TB_SourceNote.Text = newList[0].Note;
                } else {
                    TB_Source.Text = "";
                    TB_SourceNote.Text = "";
                }
            }
            else {
                TB_Source.Text = "";
                TB_SourceNote.Text = "";
            }
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem != null)
            {
                CurrentItem.Name = NameText.Text;
                CurrentItem.SetName = SetNameText.Text;
                CurrentItem.IconPath = IconPathText.Text;
                CurrentItem.Unique = UniqueCheck.IsChecked.GetValueOrDefault(false);
                CurrentItem.ItemLevel = (int)IlvlNum.Value;
                CurrentItem.Id = (int)IdNum.Value;
                CurrentItem.MinDamage = (int)MinDamageNum.Value;
                CurrentItem.MaxDamage = (int)MaxDamageNum.Value;
                CurrentItem.Speed = (float)SpeedNum.Value;
                CurrentItem.Stats = clonedStats;
                CurrentItem.Type = (ItemType)TypeCombo.SelectedIndex;
                CurrentItem.Slot = (ItemSlot)SlotCombo.SelectedIndex;
                CurrentItem.Bind = (BindsOn)BindCombo.SelectedIndex;
                CurrentItem.Quality = (ItemQuality)QualityCombo.SelectedIndex;
                CurrentItem.DamageType = (ItemDamageType)DamageTypeComboBox.SelectedIndex;
                CurrentItem.Cost = (float)CostText.Value;

                if (Gem1Combo.SelectedIndex == 1) CurrentItem.SocketColor1 = ItemSlot.Meta;
                else if (Gem1Combo.SelectedIndex == 2) CurrentItem.SocketColor1 = ItemSlot.Red;
                else if (Gem1Combo.SelectedIndex == 3) CurrentItem.SocketColor1 = ItemSlot.Yellow;
                else if (Gem1Combo.SelectedIndex == 4) CurrentItem.SocketColor1 = ItemSlot.Blue;
                else if (Gem1Combo.SelectedIndex == 5) CurrentItem.SocketColor1 = ItemSlot.Cogwheel;
                else if (Gem1Combo.SelectedIndex == 6) CurrentItem.SocketColor1 = ItemSlot.Hydraulic;
                else if (Gem1Combo.SelectedIndex == 7) CurrentItem.SocketColor1 = ItemSlot.Prismatic;
                else CurrentItem.SocketColor1 = ItemSlot.None;
                if (Gem2Combo.SelectedIndex == 1) CurrentItem.SocketColor2 = ItemSlot.Meta;
                else if (Gem2Combo.SelectedIndex == 2) CurrentItem.SocketColor2 = ItemSlot.Red;
                else if (Gem2Combo.SelectedIndex == 3) CurrentItem.SocketColor2 = ItemSlot.Yellow;
                else if (Gem2Combo.SelectedIndex == 4) CurrentItem.SocketColor2 = ItemSlot.Blue;
                else if (Gem2Combo.SelectedIndex == 5) CurrentItem.SocketColor2 = ItemSlot.Cogwheel;
                else if (Gem2Combo.SelectedIndex == 6) CurrentItem.SocketColor2 = ItemSlot.Hydraulic;
                else if (Gem2Combo.SelectedIndex == 7) CurrentItem.SocketColor2 = ItemSlot.Prismatic;
                else CurrentItem.SocketColor2 = ItemSlot.None;
                if (Gem3Combo.SelectedIndex == 1) CurrentItem.SocketColor3 = ItemSlot.Meta;
                else if (Gem3Combo.SelectedIndex == 2) CurrentItem.SocketColor3 = ItemSlot.Red;
                else if (Gem3Combo.SelectedIndex == 3) CurrentItem.SocketColor3 = ItemSlot.Yellow;
                else if (Gem3Combo.SelectedIndex == 4) CurrentItem.SocketColor3 = ItemSlot.Blue;
                else if (Gem3Combo.SelectedIndex == 5) CurrentItem.SocketColor3 = ItemSlot.Cogwheel;
                else if (Gem3Combo.SelectedIndex == 6) CurrentItem.SocketColor3 = ItemSlot.Hydraulic;
                else if (Gem3Combo.SelectedIndex == 7) CurrentItem.SocketColor3 = ItemSlot.Prismatic;
                else CurrentItem.SocketColor3 = ItemSlot.None;

                if (SourcesChanged/* && tempSources != null*/) {
                    CurrentItem.LocationInfo = tempSources;
                }

                if      (CB_Faction.SelectedIndex == 0) currentItem.Faction = ItemFaction.Neutral;
                else if (CB_Faction.SelectedIndex == 1) currentItem.Faction = ItemFaction.Alliance;
                else if (CB_Faction.SelectedIndex == 2) currentItem.Faction = ItemFaction.Horde;
                else currentItem.Faction = ItemFaction.Neutral;

                foreach (PropertyInfo info in Stats.PropertyInfoCache)
                {
                    if (Extensions.DisplayName(info).Equals(BonusStat.SelectedItem))
                    {
                        PropertyInfo oldStat = BonusStat.Tag as PropertyInfo;
                        object[] param = new object[1] { 0 };
                        oldStat.GetSetMethod().Invoke(CurrentItem.SocketBonus, param);
                        param = new object[1] { float.Parse(BonusAmount.Text) };
                        info.GetSetMethod().Invoke(CurrentItem.SocketBonus, param);
                        BonusStat.Tag = info;
                        break;
                    }
                }

                string req = null;
                foreach (KeyValuePair<string, CheckBox> kvp in ClassCheckBoxes)
                {
                    if (kvp.Value.IsChecked.GetValueOrDefault(false))
                    {
                        if (req == null) req = kvp.Key;
                        else req += "|" + kvp.Key;
                    }
                }
                CurrentItem.RequiredClasses = req;
                CurrentItem.InvalidateCachedData();
                ItemCache.OnItemsChanged();
            }
            LoadScreen.SaveFiles();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void AddSpecial_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SpecialEffectEditor see = new SpecialEffectEditor();
            see.Closed += new EventHandler(Add_Closed);
            see.Show();
        }

        private void Add_Closed(object sender, EventArgs e)
        {
            SpecialEffectEditor editor = (SpecialEffectEditor)sender;
            if (editor.DialogResult.GetValueOrDefault())
            {
                clonedStats.AddSpecialEffect(editor.SpecialEffect);
                UpdateEffectList();
            }
        }

        private void DeleteSpecialButton_Click(object sender, RoutedEventArgs e)
        {
            SpecialEffect eff = (SpecialEffect)((ComboBoxItem)SpecialEffectList.SelectedItem).Tag;
            clonedStats.RemoveSpecialEffect(eff);
            UpdateEffectList();
        }

        private void EditSpecialButton_Click(object sender, RoutedEventArgs e)
        {
            SpecialEffect eff = (SpecialEffect)((ComboBoxItem)SpecialEffectList.SelectedItem).Tag;
            SpecialEffectEditor see = new SpecialEffectEditor(eff);
            see.Closed += new EventHandler(Edit_Closed);
            see.Show();
        }

        private void Edit_Closed(object sender, EventArgs e)
        {
            UpdateEffectList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ItemCache.DeleteItem(CurrentItem);
            this.DialogResult = true;
        }

        private void WowheadButton_Click(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.wowhead.com/?item=" + CurrentItem.Id), "_blank");
#else
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://www.wowhead.com/?item=" + CurrentItem.Id));
#endif
		}

        private void RandomSuffixButton_Click(object sender, RoutedEventArgs e)
        {
            RandomSuffixEditor see = new RandomSuffixEditor(CurrentItem);
            see.Show();
        }
    }
}

