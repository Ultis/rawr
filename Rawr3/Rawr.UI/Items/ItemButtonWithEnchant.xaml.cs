using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using Rawr;

namespace Rawr.UI
{
    public partial class ItemButtonWithEnchant : UserControl
    {

        private CharacterSlot slot;
        public CharacterSlot Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                ComparisonItemList.Slot = slot;
                //ComparisonListReforge.Slot = slot;
                ComparisonListEnchant.Slot = slot;
            }
        }

        public ItemInstance Item;
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) character.AvailableItemsChanged -= new EventHandler(character_CalculationsInvalidated);
                character = value;
                DataContext = character;
                ComparisonItemList.Character = character;
                ComparisonItemListGem1.Character = character;
                ComparisonItemListGem2.Character = character;
                ComparisonItemListGem3.Character = character;
                ComparisonListReforge.Character = character;
                ComparisonListEnchant.Character = character;
                if (character != null)
                {
                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                    character_CalculationsInvalidated(null, null);
                }
            }
        }

        private ItemInstance gear;
        private Item enchant;
        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            if (character != null)
            {
                Item = character[Slot];
                if (Item == null) {
                    IconImage.Source = null;
                    EnchantButton.Content = "";
                    gear = null;
                    gear = null;
                    IconImageGem1.Source = null; GemButton1.IsEnabled = false;
                    IconImageGem2.Source = null; GemButton2.IsEnabled = false;
                    IconImageGem3.Source = null; GemButton3.IsEnabled = false;
                } else {
                    IconImage.Source = Icons.ItemIcon(Item.Item.IconPath);
                    EnchantButton.Content = Item.Enchant.ShortName;
                    gear = Item;

                    Item eItem = new Item();
                    eItem.Name = Item.Enchant.Name;
                    eItem.Quality = ItemQuality.Temp;
                    eItem.Stats = Item.Enchant.Stats;
                    enchant = eItem;

                    // There are several special sockets, we need to account for them
                    int nonBSSocketCount = (Item.Item.SocketColor1 != ItemSlot.None ? 1 : 0)
                                         + (Item.Item.SocketColor2 != ItemSlot.None ? 1 : 0)
                                         + (Item.Item.SocketColor3 != ItemSlot.None ? 1 : 0);
                    bool BSSlot_Wrist = Slot == CharacterSlot.Wrist && Character.WristBlacksmithingSocketEnabled && (Character.PrimaryProfession == Profession.Blacksmithing || Character.SecondaryProfession == Profession.Blacksmithing);
                    bool BSSlot_Glove = Slot == CharacterSlot.Hands && Character.HandsBlacksmithingSocketEnabled && (Character.PrimaryProfession == Profession.Blacksmithing || Character.SecondaryProfession == Profession.Blacksmithing);
                    bool BSSlot_Waist = Slot == CharacterSlot.Waist && Character.WaistBlacksmithingSocketEnabled;
                    // If there is no gem socket there, hide the selector
                    GemButton1.IsEnabled = Item.Item.SocketColor1 != ItemSlot.None || (nonBSSocketCount == 0 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    GemButton2.IsEnabled = Item.Item.SocketColor2 != ItemSlot.None || (nonBSSocketCount == 1 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    GemButton3.IsEnabled = Item.Item.SocketColor3 != ItemSlot.None || (nonBSSocketCount == 2 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    // Use the Gem's image if it exists
                    IconImageGem1.Source = Item.Gem1 != null ? Icons.ItemIcon(Item.Gem1.IconPath) : null;
                    IconImageGem2.Source = Item.Gem2 != null ? Icons.ItemIcon(Item.Gem2.IconPath) : null;
                    IconImageGem3.Source = Item.Gem3 != null ? Icons.ItemIcon(Item.Gem3.IconPath) : null;
                    // Remove any previously registered Events that can be fired
                    ComparisonItemListGem1.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem1_SelectedItemsGemChanged);
                    ComparisonItemListGem2.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem2_SelectedItemsGemChanged);
                    ComparisonItemListGem3.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem3_SelectedItemsGemChanged);
                    // Set up the selector so that it uses the right slot type
                    ComparisonItemListGem1.Slot = Item.Item.SocketColor1 == ItemSlot.Meta ? CharacterSlot.Metas : CharacterSlot.Gems;
                    ComparisonItemListGem2.Slot = CharacterSlot.Gems;
                    ComparisonItemListGem3.Slot = CharacterSlot.Gems;
                    // Set the Gem lists' selected items to the core item's gems
                    ComparisonItemListGem1.SelectedItem = Item.Gem1;
                    ComparisonItemListGem2.SelectedItem = Item.Gem2;
                    ComparisonItemListGem3.SelectedItem = Item.Gem3;
                    // Let the comp lists know they need to behave differently from a normal list
                    ComparisonItemListGem1.IsAnItemsGem = true;
                    ComparisonItemListGem2.IsAnItemsGem = true;
                    ComparisonItemListGem3.IsAnItemsGem = true;
                    // Register the Events that can be fired
                    ComparisonItemListGem1.SelectedItemsGemChanged += new EventHandler(ComparisonItemListGem1_SelectedItemsGemChanged);
                    ComparisonItemListGem2.SelectedItemsGemChanged += new EventHandler(ComparisonItemListGem2_SelectedItemsGemChanged);
                    ComparisonItemListGem3.SelectedItemsGemChanged += new EventHandler(ComparisonItemListGem3_SelectedItemsGemChanged);
                }
            }
        }

        public ItemButtonWithEnchant()
        {
            // Required to initialize variables
            InitializeComponent();
            PopupUtilities.RegisterPopup(this, ListPopup, EnchantPopup, Close);
            ComparisonListEnchant.IsEnchantList = true;
            ComparisonListReforge.IsReforgeList = true;
            PopupUtilities.RegisterPopup(this, ListPopupGem1, Close);
            PopupUtilities.RegisterPopup(this, ListPopupGem2, Close);
            PopupUtilities.RegisterPopup(this, ListPopupGem3, Close);
            PopupUtilities.RegisterPopup(this, ReforgePopup, Close);
            UpdateButtonOptions();
        }

        private void Close()
        {
            if (ListPopup.IsOpen) { ListPopup.IsOpen = false; }
            if (EnchantPopup.IsOpen) { EnchantPopup.IsOpen = false; }
            if (ListPopupGem1.IsOpen) { ListPopupGem1.IsOpen = false; }
            if (ListPopupGem2.IsOpen) { ListPopupGem2.IsOpen = false; }
            if (ListPopupGem3.IsOpen) { ListPopupGem3.IsOpen = false; }
            if (ReforgePopup.IsOpen) { ReforgePopup.IsOpen = false; }
        }

        #region Popup Lists
        private void EnsurePopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.DesiredSize.Height - transform.Transform(new Point(0, ComparisonItemList.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0) {
                ListPopup.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
            distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, 66 + ComparisonListEnchant.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0) {
                EnchantPopup.VerticalOffset = 66 + distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }
        private void EnsureGem1PopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, ComparisonItemListGem1.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ListPopupGem1.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }
        private void EnsureGem2PopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, ComparisonItemListGem2.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ListPopupGem2.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }
        private void EnsureGem3PopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, ComparisonItemListGem3.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ListPopupGem3.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }
        private void EnsureReforgePopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, ComparisonListReforge.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ReforgePopup.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }

        private void MainButton_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsurePopupsVisible();
            ComparisonItemList.IsShown = true;
            ListPopup.IsOpen = true;
            ComparisonItemList.Focus();
        }
        private void GemButton1_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsureGem1PopupsVisible();
            ComparisonItemListGem1.IsShown = true;
            ListPopupGem1.IsOpen = true;
            ComparisonItemListGem1.Focus();
        }
        private void GemButton2_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsureGem2PopupsVisible();
            ComparisonItemListGem2.IsShown = true;
            ListPopupGem2.IsOpen = true;
            ComparisonItemListGem2.Focus();
        }
        private void GemButton3_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsureGem3PopupsVisible();
            ComparisonItemListGem3.IsShown = true;
            ListPopupGem3.IsOpen = true;
            ComparisonItemListGem3.Focus();
        }
        private void EnchantButton_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsurePopupsVisible();
            ComparisonListEnchant.IsShown = true;
            EnchantPopup.IsOpen = true;
            ComparisonListEnchant.Focus();
        }
        private void ReforgeButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            EnsureReforgePopupsVisible();
            ComparisonListReforge.IsShown = true;
            ReforgePopup.IsOpen = true;
            ComparisonListReforge.Focus();
        }
        #endregion

        #region Returning Gem/Reforge Events
        public void ComparisonItemListGem1_SelectedItemsGemChanged(object sender, EventArgs e) { if (Item != null) Item.Gem1 = ComparisonItemListGem1.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        public void ComparisonItemListGem2_SelectedItemsGemChanged(object sender, EventArgs e) { if (Item != null) Item.Gem2 = ComparisonItemListGem2.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        public void ComparisonItemListGem3_SelectedItemsGemChanged(object sender, EventArgs e) { if (Item != null) Item.Gem3 = ComparisonItemListGem3.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        //public void ComparisonListReforge_SelectedItemsGemChanged(object sender, EventArgs e) { if (Item != null) Item.Reforging = ComparisonListReforge.SelectedReforging; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        #endregion

        #region ToolTips
        private void MainButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen)
            {
                MainPage.Tooltip.ItemInstance = gear;
                MainPage.Tooltip.Show(MainButton, 72, 0);
            }
        }
        private void GemButton1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = Item.Gem1;
                MainPage.Tooltip.Show(GemButton1, 22, 0);
            }
        }
        private void GemButton2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = Item.Gem2;
                MainPage.Tooltip.Show(GemButton2, 22, 0);
            }
        }
        private void GemButton3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = Item.Gem3;
                MainPage.Tooltip.Show(GemButton3, 22, 0);
            }
        }
        private void EnchantButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!EnchantPopup.IsOpen)
            {
                MainPage.Tooltip.Item = enchant;
                MainPage.Tooltip.Show(EnchantButton, 72, 0);
            }
        }
        private void ReforgeButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen && !ReforgePopup.IsOpen)
            {
                MainPage.Tooltip.Item = Item.Reforging;
                MainPage.Tooltip.Show(GemButton3, 22, 0);
            }*/
        }

        private void AnyButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) { MainPage.Tooltip.Hide(); }
        #endregion

        private void BSSocket_Checked(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
            BSSocketIsChecked = CK_BSSocket.IsChecked.GetValueOrDefault(false);
        }

        private bool _ShowGems = true; public bool ShowGems { get { return _ShowGems; } set { _ShowGems = value; UpdateButtonOptions(); } }
        private bool _ShowGem3 = true; public bool ShowGem3 { get { return _ShowGem3; } set { _ShowGem3 = value; UpdateButtonOptions(); } }
        private bool _ShowEnchant = true; public bool ShowEnchant { get { return _ShowEnchant; } set { _ShowEnchant = value; UpdateButtonOptions(); } }
        private bool _ShowReforge = true; public bool ShowReforge { get { return _ShowReforge; } set { _ShowReforge = value; UpdateButtonOptions(); } }
        private bool _ShowBSSocket = false; public bool ShowBSSocket { get { return _ShowBSSocket; } set { _ShowBSSocket = value; UpdateButtonOptions(); } }
        private bool _EnableBSSocket = true; public bool EnableBSSocket { get { return _EnableBSSocket; } set { _EnableBSSocket = value; UpdateButtonOptions(); } }

        public Boolean BSSocketIsChecked {
            get {
                Boolean retVal = (Boolean)this.GetValue(BSSocketProperty);
                //if (CK_BSSocket.IsChecked != retVal) { CK_BSSocket.IsChecked = retVal; }
                return retVal;
            }
            set {
                this.SetValue(BSSocketProperty, value);
                if (CK_BSSocket.IsChecked != value) { CK_BSSocket.IsChecked = value; }
            }
        }
        public static readonly DependencyProperty BSSocketProperty = DependencyProperty.Register(
            "BSSocketIsChecked", typeof(Boolean), typeof(ItemButtonWithEnchant), new PropertyMetadata(false));

        public void SetButtonOptions(bool showReforge, bool showEnchant, bool showGems, bool showGem3, bool showBSSocket, bool enableBSSocket)
        {
            ShowGems = showGems;
            ShowGem3 = showGem3;
            ShowEnchant = showEnchant;
            ShowReforge = showReforge;
            ShowBSSocket = showBSSocket;
            EnableBSSocket = enableBSSocket;
            //
            UpdateButtonOptions();
        }
        public void UpdateButtonOptions() {
            GemButton1.Visibility = ShowGems ? Visibility.Visible : Visibility.Collapsed;
            GemButton2.Visibility = ShowGems ? Visibility.Visible : Visibility.Collapsed;
            GemButton3.Visibility = (ShowGems && ShowGem3) ? Visibility.Visible : Visibility.Collapsed;
            EnchantButton.Visibility = ShowEnchant ? Visibility.Visible : Visibility.Collapsed;
            ReforgeButton.Visibility = ShowReforge ? Visibility.Visible : Visibility.Collapsed;
            CK_BSSocket.Visibility = ShowBSSocket ? Visibility.Visible : Visibility.Collapsed;
            CK_BSSocket.IsEnabled = EnableBSSocket;
            if (CK_BSSocket.IsChecked != BSSocketIsChecked)
            {
                CK_BSSocket.IsChecked = BSSocketIsChecked;
            }
        }
    }
}