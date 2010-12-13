using System;
using System.Collections.Generic;
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
    public partial class ItemSourceEditorChild : ChildWindow
    {
        #region Constructors
        public ItemSourceEditorChild()
        {
            InitializeComponent();
            HideAllPoints();
        }
        public ItemSourceEditorChild(ItemLocation sourceToAddorEdit)
        {
            NewSource = sourceToAddorEdit;
            InitializeComponent();
            HideAllPoints();
            isChanging = true;
            CB_Type.SelectedItem = LocTypeToString(sourceToAddorEdit);
            PopulateEach(sourceToAddorEdit);
            isChanging = false;
        }
        #endregion

        #region Variables
        private bool isChanging = false;
        private ItemLocation _NewSource = null;
        public ItemLocation NewSource {
            get { return _NewSource ?? (_NewSource = new UnknownItem()); }
            set { if (_NewSource != value) { _NewSource = value; } }
        }
        #endregion

        #region Conversion Functions
        private String LocTypeToString(ItemLocation src)
        {
            string retVal = "";
            if (src is NoSource) { retVal = "None"; }
            else if (src is UnknownItem) { retVal = "Unknown"; }
            else if (src is VendorItem) { retVal = "Vendor"; }
            else if (src is FactionItem) { retVal = "Faction"; }
            else if (src is PvpItem) { retVal = "PvP"; }
            else if (src is StaticDrop) { retVal = "Static Drop"; }
            else if (src is WorldDrop) { retVal = "World Drop"; }
            else if (src is CraftedItem) { retVal = "Crafted"; }
            else if (src is QuestItem) { retVal = "Quest"; }
            else if (src is ContainerItem) { retVal = "Container"; }
            else if (src is ItemLocation) { retVal = "Not Found"; }
            return retVal;
        }
        #endregion

        #region Utility/Shared Functions
        private void PopulateEach(ItemLocation sourceToAddorEdit)
        {
            NotFound_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Unknown_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Vendor_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Faction_PopulateInfo(sourceToAddorEdit); isChanging = false;
            PvP_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Crafted_PopulateInfo(sourceToAddorEdit); isChanging = false;
            WorldDrop_PopulateInfo(sourceToAddorEdit); isChanging = false;
            StaticDrop_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Quest_PopulateInfo(sourceToAddorEdit); isChanging = false;
            Container_PopulateInfo(sourceToAddorEdit); isChanging = false;
        }
        private void HideAllPoints()
        {
            try
            {
                St_None.Visibility = Visibility.Collapsed;
                St_Unknown.Visibility = Visibility.Collapsed;
                St_Vendor.Visibility = Visibility.Collapsed;
                St_Faction.Visibility = Visibility.Collapsed;
                St_PvP.Visibility = Visibility.Collapsed;
                St_Crafted.Visibility = Visibility.Collapsed;
                St_WorldDrop.Visibility = Visibility.Collapsed;
                St_StaticDrop.Visibility = Visibility.Collapsed;
                St_Quest.Visibility = Visibility.Collapsed;
                St_Container.Visibility = Visibility.Collapsed;
            }
            catch (Exception) { }
        }
        private void UpdateString()
        {
            LB_String.Text = NewSource.Description;
        }
        private void CB_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HideAllPoints();
                if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
                switch (CB_Type.SelectedItem as String)
                {
                    case "None": {
                        St_None.Visibility = Visibility.Visible;
                        NewSource = new NoSource() { };
                        break;
                    }
                    case "Unknown": {
                        St_Unknown.Visibility = Visibility.Visible;
                        NewSource = new UnknownItem() { };
                        break;
                    }
                    case "Vendor": {
                        St_Vendor.Visibility = Visibility.Visible;
                        NewSource = new VendorItem() { VendorName = "", VendorArea = "", Cost = 0, };
                        break;
                    }
                    case "Faction": {
                        St_Faction.Visibility = Visibility.Visible;
                        NewSource = new FactionItem() { FactionName = "A Faction", Level = ReputationLevel.Honored, Cost = 0, };
                        break;
                    }
                    case "PvP": {
                        St_PvP.Visibility = Visibility.Visible;
                        NewSource = new PvpItem() { TokenCount = 0, TokenType = "", Points = 0, PointType = "Honor Points", };
                        break;
                    }
                    case "Crafted": {
                        St_Crafted.Visibility = Visibility.Visible;
                        NewSource = new CraftedItem() { Bind = BindsOn.BoE, Skill = "Unknown", SpellName = "", Level = 525 };
                        break;
                    }
                    case "World Drop": {
                        St_WorldDrop.Visibility = Visibility.Visible;
                        NewSource = new WorldDrop() { Heroic = false, Location = "Unknown Zone" };
                        break;
                    }
                    case "Static Drop": {
                        St_StaticDrop.Visibility = Visibility.Visible;
                        NewSource = new StaticDrop() { Heroic = false, Area = "Unknown Instance", Boss = "Unknown Boss", };
                        break;
                    }
                    case "Quest": {
                        St_Quest.Visibility = Visibility.Visible;
                        NewSource = new QuestItem() { Area = "Unknown Zone", MinLevel = 85, Quest = "Unknown Quest", Party = 5 };
                        break; 
                    }
                    case "Container": {
                        St_Container.Visibility = Visibility.Visible;
                        NewSource = new ContainerItem() { Area = "Unknown Zone", Container = "Unknown Container", Heroic = false, MinLevel = 85, Party = 5 };
                        break;
                    }
                    default: { /* it broke */ break; }
                }
            }
            catch (Exception) { /*Do nothing*/ }
            PopulateEach(NewSource);
        }
        #endregion

        #region ItemLocation Functions
        #region No Source or Not Found or None
        private void NotFound_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Not Found" && (CB_Type.SelectedItem as String) != "None") { return; }
            //
            isChanging = true;
            //
            NewSource = new NoSource() { };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void NotFound_PopulateInfo(ItemLocation src)
        {
            if ((src is NoSource) == false) { return; }
            NoSource topop = src as NoSource;
            isChanging = false;
            NotFound_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region Unknown
        private void Unknown_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Unknown") { return; }
            //
            isChanging = true;
            //
            NewSource = new UnknownItem() { };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Unknown_PopulateInfo(ItemLocation src)
        {
            if ((src is UnknownItem) == false) { return; }
            UnknownItem topop = src as UnknownItem;
            isChanging = false;
            Unknown_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region VendorItem
        private void Vendor_InfoChanged_Text(object sender, TextChangedEventArgs e) { Vendor_InfoChanged(); }
        private void Vendor_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { Vendor_InfoChanged(); }
        private void Vendor_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { Vendor_InfoChanged(); }
        private void Vendor_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Vendor") { return; }
            //
            isChanging = true;
            //
            int total = 0;
            int gold = (int)TB_Vendor_Money_1.Value; total += gold * 10000;
            int silver = (int)TB_Vendor_Money_2.Value; total += silver * 100;
            int copper = (int)TB_Vendor_Money_3.Value; total += copper;
            NewSource = new VendorItem()
            {
                VendorName = TB_Vendor_Name.Text,
                VendorArea = TB_Vendor_Area.Text,
                Cost = total,
                TokenMap = (TB_Vendor_Token_1.SelectedItem as String) != ""
                    ? new SerializableDictionary<string, int>() { { TB_Vendor_Token_1.SelectedItem as String, (int)TB_Vendor_Token_2.Value } }
                    : new SerializableDictionary<string, int>(),
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Vendor_PopulateInfo(ItemLocation src) {
            if ((src is VendorItem) == false) { return; }
            //
            VendorItem topop = src as VendorItem;
            // Name & Area
            if (topop.VendorName == null) { topop.VendorName = ""; }
            if (topop.VendorArea == null) { topop.VendorArea = ""; }
            TB_Vendor_Name.Text = topop.VendorName;
            TB_Vendor_Area.Text = topop.VendorArea;
            // Money Cost
            int total = topop.Cost;
            int gold = total / 10000;
            total -= gold * 10000;
            int silver = total / 100;
            total -= silver * 100;
            TB_Vendor_Money_1.Value = gold;
            TB_Vendor_Money_2.Value = silver;
            TB_Vendor_Money_3.Value = total;
            // Token Map Cost (TODO)
            TB_Vendor_Token_1.SelectedItem = (topop.TokenMap != null && topop.TokenMap.Keys.Count > 0 ? topop.TokenMap.Keys.ToList()[0] : "");
            TB_Vendor_Token_2.Value        = (topop.TokenMap != null && topop.TokenMap.Keys.Count > 0 ? topop.TokenMap.Values.ToList()[0] : 0);
            //
            isChanging = false;
            Vendor_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region FactionItem
        private void Faction_InfoChanged_Text(object sender, TextChangedEventArgs e) { Faction_InfoChanged(); }
        private void Faction_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { Faction_InfoChanged(); }
        private void Faction_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { Faction_InfoChanged(); }
        private void Faction_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Faction") { return; }
            //
            isChanging = true;
            //
            int total = 0;
            int gold = (int)TB_Faction_Money_1.Value; total += gold * 10000;
            int silver = (int)TB_Faction_Money_2.Value; total += silver * 100;
            int copper = (int)TB_Faction_Money_3.Value; total += copper;
            //
            NewSource = new FactionItem() {
                Cost = total,
                FactionName = TB_Faction_Name.Text,
                Level = (ReputationLevel)CB_Faction_Level.SelectedIndex,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Faction_PopulateInfo(ItemLocation src)
        {
            if ((src is FactionItem) == false) { return; }
            //
            FactionItem topop = src as FactionItem;
            // Name & Area
            TB_Faction_Name.Text = topop.FactionName;
            CB_Faction_Level.SelectedIndex = (int)topop.Level;
            // Money Cost
            int total = topop.Cost;
            int gold = total / 10000;
            total -= gold * 10000;
            int silver = total / 100;
            total -= silver * 100;
            TB_Faction_Money_1.Value = gold;
            TB_Faction_Money_2.Value = silver;
            TB_Faction_Money_3.Value = total;
            // Token Map Cost (TODO)
            //
            isChanging = false;
            Faction_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region PvPItem
        private void PvP_InfoChanged_Text(object sender, TextChangedEventArgs e) { PvP_InfoChanged(); }
        private void PvP_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { PvP_InfoChanged(); }
        private void PvP_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { PvP_InfoChanged(); }
        private void PvP_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "PvP") { return; }
            //
            isChanging = true;
            //
            NewSource = new PvpItem() {
                PointType = TB_PvP_Name.Text,
                Points = (int)TB_PvP_Money_1.Value,
                TokenType = TB_PvP_Token_1.SelectedItem as String,
                TokenCount = (int)TB_PvP_Money_2.Value,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void PvP_PopulateInfo(ItemLocation src)
        {
            if ((src is PvpItem) == false) { return; }
            //
            PvpItem topop = src as PvpItem;
            // Points Type and Cost
            TB_PvP_Name.Text = topop.PointType;
            TB_PvP_Money_1.Value = topop.Points;
            // Token Map Cost (TODO)
            TB_PvP_Token_1.SelectedItem = topop.TokenType;
            TB_PvP_Money_2.Value = topop.TokenCount;
            //
            isChanging = false;
            PvP_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region CraftedItem
        private void Crafted_InfoChanged_Text(object sender, TextChangedEventArgs e) { Crafted_InfoChanged(); }
        private void Crafted_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { Crafted_InfoChanged(); }
        private void Crafted_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { Crafted_InfoChanged(); }
        private void Crafted_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Crafted") { return; }
            //
            isChanging = true;
            //
            NewSource = new CraftedItem()
            {
                Skill = TB_Crafted_Name.Text,
                Level = (int)TB_Crafted_Money_1.Value,
                SpellName = TB_Crafted_Token_1.Text,
                Bind = (BindsOn)TB_Crafted_Token_2.SelectedIndex,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Crafted_PopulateInfo(ItemLocation src)
        {
            if ((src is CraftedItem) == false) { return; }
            //
            CraftedItem topop = src as CraftedItem;
            // Points Type and Cost
            TB_Crafted_Name.Text = topop.Skill;
            TB_Crafted_Money_1.Value = topop.Level;
            // Token Map Cost (TODO)
            TB_Crafted_Token_1.Text = topop.SpellName;
            TB_Crafted_Token_2.SelectedIndex = (int)topop.Bind;
            //
            isChanging = false;
            Crafted_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region WorldDrop
        private void WorldDrop_InfoChanged_Text(object sender, TextChangedEventArgs e) { WorldDrop_InfoChanged(); }
        private void WorldDrop_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { WorldDrop_InfoChanged(); }
        private void WorldDrop_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { WorldDrop_InfoChanged(); }
        private void WorldDrop_CheckedChanged(object sender, RoutedEventArgs e) { WorldDrop_InfoChanged(); }
        private void WorldDrop_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "World Drop") { return; }
            //
            isChanging = true;
            //
            NewSource = new WorldDrop()
            {
                Heroic = TB_WorldDrop_Money_1.IsChecked.GetValueOrDefault(false),
                Location = TB_WorldDrop_Name.Text,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void WorldDrop_PopulateInfo(ItemLocation src)
        {
            if ((src is WorldDrop) == false) { return; }
            //
            WorldDrop topop = src as WorldDrop;
            // Heroic Mode and Location
            if (topop.Location == null) { topop.Location = "Unknown Zone"; }
            TB_WorldDrop_Name.Text = topop.Location;
            TB_WorldDrop_Money_1.IsChecked = topop.Heroic;
            //
            isChanging = false;
            WorldDrop_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region StaticDrop
        private void StaticDrop_InfoChanged_Text(object sender, TextChangedEventArgs e) { StaticDrop_InfoChanged(); }
        private void StaticDrop_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { StaticDrop_InfoChanged(); }
        private void StaticDrop_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { StaticDrop_InfoChanged(); }
        private void StaticDrop_CheckedChanged(object sender, RoutedEventArgs e) { StaticDrop_InfoChanged(); }
        private void StaticDrop_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Static Drop") { return; }
            //
            isChanging = true;
            //
            NewSource = new StaticDrop()
            {
                Heroic = TB_StaticDrop_Money_1.IsChecked.GetValueOrDefault(false),
                Area = TB_StaticDrop_Name.Text,
                Boss = TB_StaticDrop_Token_1.Text,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void StaticDrop_PopulateInfo(ItemLocation src)
        {
            if ((src is StaticDrop) == false) { return; }
            //
            StaticDrop topop = src as StaticDrop;
            // Points Type and Cost
            TB_StaticDrop_Name.Text = topop.Area;
            TB_StaticDrop_Money_1.IsChecked = topop.Heroic;
            TB_StaticDrop_Token_1.Text = topop.Boss;
            //
            isChanging = false;
            StaticDrop_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region QuestItem
        private void Quest_InfoChanged_Text(object sender, TextChangedEventArgs e) { Quest_InfoChanged(); }
        private void Quest_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { Quest_InfoChanged(); }
        private void Quest_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { Quest_InfoChanged(); }
        private void Quest_CheckedChanged(object sender, RoutedEventArgs e) { Quest_InfoChanged(); }
        private void Quest_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Quest") { return; }
            //
            isChanging = true;
            //
            NewSource = new QuestItem()
            {
                Area = TB_Quest_Name.Text,
                Quest = TB_Quest_Money_1.Text,
                Party = (int)TB_Quest_Money_2.Value,
                MinLevel = (int)TB_Quest_Money_3.Value,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Quest_PopulateInfo(ItemLocation src)
        {
            if ((src is QuestItem) == false) { return; }
            //
            QuestItem topop = src as QuestItem;
            // Zone, Name, Party Size, Minimum Level
            TB_Quest_Name.Text = topop.Area;
            TB_Quest_Money_1.Text = topop.Quest;
            TB_Quest_Money_2.Value = topop.Party;
            TB_Quest_Money_3.Value = topop.MinLevel;
            //
            isChanging = false;
            Quest_InfoChanged();
            isChanging = true;
        }
        #endregion
        #region ContainerItem
        private void Container_InfoChanged_Text(object sender, TextChangedEventArgs e) { Container_InfoChanged(); }
        private void Container_InfoChanged_NUD(object sender, RoutedPropertyChangedEventArgs<double> e) { Container_InfoChanged(); }
        private void Container_InfoChanged_CB(object sender, SelectionChangedEventArgs e) { Container_InfoChanged(); }
        private void Container_CheckedChanged(object sender, RoutedEventArgs e) { Container_InfoChanged(); }
        private void Container_InfoChanged()
        {
            if (isChanging) { return; }
            if (CB_Type == null || CB_Type.SelectedItem == null) { return; }
            if ((CB_Type.SelectedItem as String) != "Container") { return; }
            //
            isChanging = true;
            //
            NewSource = new ContainerItem()
            {
                Area = TB_Container_Name.Text,
                Container = TB_Container_Money_1.Text,
                Heroic = TB_Container_Token_1.IsChecked.GetValueOrDefault(false),
                MinLevel = (int)TB_Container_Money_2.Value,
                Party = (int)TB_Container_Money_3.Value,
            };
            //
            UpdateString();
            //
            isChanging = false;
        }
        private void Container_PopulateInfo(ItemLocation src)
        {
            if ((src is ContainerItem) == false) { return; }
            //
            ContainerItem topop = src as ContainerItem;
            // Points Type and Cost
            TB_Container_Name.Text = topop.Area;
            TB_Container_Money_1.Text = topop.Container;
            TB_Container_Token_1.IsChecked = topop.Heroic;
            TB_Container_Money_2.Value = topop.MinLevel;
            TB_Container_Money_3.Value = topop.Party;
            //
            isChanging = false;
            Container_InfoChanged();
            isChanging = true;
        }
        #endregion
        #endregion

        #region Dialog Interaction
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}
