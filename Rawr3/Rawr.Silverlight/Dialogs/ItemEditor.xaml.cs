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

namespace Rawr.Silverlight
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
                QualityCombo.SelectedIndex = (int)CurrentItem.Quality;
                DamageTypeComboBox.SelectedIndex = (int)CurrentItem.DamageType;

                if (currentItem.SocketColor1 == Item.ItemSlot.Meta) Gem1Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor1 == Item.ItemSlot.Red) Gem1Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor1 == Item.ItemSlot.Yellow) Gem1Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor1 == Item.ItemSlot.Blue) Gem1Combo.SelectedIndex = 4;
                else Gem1Combo.SelectedIndex = 0;
                if (currentItem.SocketColor2 == Item.ItemSlot.Meta) Gem2Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor2 == Item.ItemSlot.Red) Gem2Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor2 == Item.ItemSlot.Yellow) Gem2Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor2 == Item.ItemSlot.Blue) Gem2Combo.SelectedIndex = 4;
                else Gem2Combo.SelectedIndex = 0;
                if (currentItem.SocketColor3 == Item.ItemSlot.Meta) Gem3Combo.SelectedIndex = 1;
                else if (currentItem.SocketColor3 == Item.ItemSlot.Red) Gem3Combo.SelectedIndex = 2;
                else if (currentItem.SocketColor3 == Item.ItemSlot.Yellow) Gem3Combo.SelectedIndex = 3;
                else if (currentItem.SocketColor3 == Item.ItemSlot.Blue) Gem3Combo.SelectedIndex = 4;
                else Gem3Combo.SelectedIndex = 0;
                
                foreach (CheckBox cb in ClassCheckBoxes.Values) cb.IsChecked = false;
                if (currentItem.RequiredClasses != null)
                {
                    foreach (string c in currentItem.RequiredClasses.Split('|')) ClassCheckBoxes[c].IsChecked = true;
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

            TypeCombo.ItemsSource = EnumHelper.GetValues<Item.ItemType>().Select(e => e.ToString());
            QualityCombo.ItemsSource = EnumHelper.GetValues<Item.ItemQuality>().Where(iq => iq != Item.ItemQuality.Temp).Select(e => e.ToString());
            SlotCombo.ItemsSource = EnumHelper.GetValues<Item.ItemSlot>().Select(e => e.ToString());
            DamageTypeComboBox.ItemsSource = EnumHelper.GetValues<Item.ItemDamageType>().Select(e => e.ToString());
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
                CurrentItem.Type = (Item.ItemType)TypeCombo.SelectedIndex;
                CurrentItem.Slot = (Item.ItemSlot)SlotCombo.SelectedIndex;
                CurrentItem.Quality = (Item.ItemQuality)QualityCombo.SelectedIndex;
                CurrentItem.DamageType = (Item.ItemDamageType)DamageTypeComboBox.SelectedIndex;

                if (Gem1Combo.SelectedIndex == 1) CurrentItem.SocketColor1 = Item.ItemSlot.Meta;
                else if (Gem1Combo.SelectedIndex == 2) CurrentItem.SocketColor1 = Item.ItemSlot.Red;
                else if (Gem1Combo.SelectedIndex == 3) CurrentItem.SocketColor1 = Item.ItemSlot.Yellow;
                else if (Gem1Combo.SelectedIndex == 4) CurrentItem.SocketColor1 = Item.ItemSlot.Blue;
                else CurrentItem.SocketColor1 = Item.ItemSlot.None;
                if (Gem2Combo.SelectedIndex == 1) CurrentItem.SocketColor1 = Item.ItemSlot.Meta;
                else if (Gem2Combo.SelectedIndex == 2) CurrentItem.SocketColor1 = Item.ItemSlot.Red;
                else if (Gem2Combo.SelectedIndex == 3) CurrentItem.SocketColor1 = Item.ItemSlot.Yellow;
                else if (Gem2Combo.SelectedIndex == 4) CurrentItem.SocketColor1 = Item.ItemSlot.Blue;
                else CurrentItem.SocketColor1 = Item.ItemSlot.None;
                if (Gem3Combo.SelectedIndex == 1) CurrentItem.SocketColor1 = Item.ItemSlot.Meta;
                else if (Gem3Combo.SelectedIndex == 2) CurrentItem.SocketColor1 = Item.ItemSlot.Red;
                else if (Gem3Combo.SelectedIndex == 3) CurrentItem.SocketColor1 = Item.ItemSlot.Yellow;
                else if (Gem3Combo.SelectedIndex == 4) CurrentItem.SocketColor1 = Item.ItemSlot.Blue;
                else CurrentItem.SocketColor1 = Item.ItemSlot.None;

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
                ItemCache.OnItemsChanged();
            }
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
            clonedStats.AddSpecialEffect(((SpecialEffectEditor)sender).SpecialEffect);
            UpdateEffectList();
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
    }
}

