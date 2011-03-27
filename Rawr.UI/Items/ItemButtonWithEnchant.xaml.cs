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
        public ItemButtonWithEnchant()
        {
            // Required to initialize variables
            InitializeComponent();
            PopupUtilities.RegisterPopup(this, ListPopup, EnchantPopup, Close);
            ComparisonListEnchant.IsEnchantList = true;
            ComparisonListReforge.IsReforgeList = true;
            ComparisonListTinker.IsTinkeringList = true;
            PopupUtilities.RegisterPopup(this, ListPopupGem1, Close);
            PopupUtilities.RegisterPopup(this, ListPopupGem2, Close);
            PopupUtilities.RegisterPopup(this, ListPopupGem3, Close);
            PopupUtilities.RegisterPopup(this, ReforgePopup, Close);
            PopupUtilities.RegisterPopup(this, TinkerPopup, Close);
            UpdateButtonOptions();
        }

        private CharacterSlot slot;
        public CharacterSlot Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                ComparisonItemList.Slot = slot;
                ComparisonItemListGem1.Slot = slot;
                ComparisonItemListGem2.Slot = slot;
                ComparisonItemListGem3.Slot = slot;
                ComparisonListReforge.Slot = slot;
                ComparisonListEnchant.Slot = slot;
                ComparisonListTinker.Slot = slot;
            }
        }

        public ItemInstance TheItem;
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
                ComparisonListTinker.Character = character;
                if (character != null)
                {
                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                    character_CalculationsInvalidated(null, null);
                }
            }
        }

        private ItemInstance gear;
        private Item enchant;
        private Item reforge;
        private Item tinkering;
        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            if (character != null)
            {
                TheItem = character[Slot];
                if (TheItem == null || TheItem.Item == null) {
                    IconImage.Source = null;
                    EnchantButton.Content = "";
                    TinkerButton.Content = "";
                    gear = null;
                    IconImageGem1.Source = null; GemButton1.IsEnabled = false; SetSocketColor(GemButton1, ItemSlot.None);
                    IconImageGem2.Source = null; GemButton2.IsEnabled = false; SetSocketColor(GemButton2, ItemSlot.None);
                    IconImageGem3.Source = null; GemButton3.IsEnabled = false; SetSocketColor(GemButton3, ItemSlot.None);
                } else {
                    IconImage.Source = Icons.AnIcon(TheItem.Item.IconPath);
                    EnchantButton.Content = TheItem.Enchant.ShortName;
                    ReforgeButton.Content = TheItem.Reforging != null ? TheItem.Reforging.VeryShortName : "NR";
                    TinkerButton.Content = TheItem.Tinkering.ShortName;
                    gear = TheItem;

                    Item eItem = new Item();
                    if (TheItem.Enchant != null) {
                        eItem.Name = TheItem.Enchant.Name;
                        eItem.Quality = ItemQuality.Temp;
                        eItem.Stats = TheItem.Enchant.Stats;
                    }
                    enchant = eItem;

                    Item tItem = new Item();
                    if (TheItem.Tinkering != null)
                    {
                        tItem.Name = TheItem.Tinkering.Name;
                        tItem.Quality = ItemQuality.Temp;
                        tItem.Stats = TheItem.Tinkering.Stats;
                    }
                    tinkering = tItem;

                    Item rItem = new Item();
                    if (TheItem.Reforging != null) {
                        rItem.Name = TheItem.Reforging.ToString();
                        rItem.Quality = ItemQuality.Temp;
                    }
                    reforge = rItem;

                    // There are several special sockets, we need to account for them
                    int nonBSSocketCount = (TheItem.Item.SocketColor1 != ItemSlot.None ? 1 : 0)
                                         + (TheItem.Item.SocketColor2 != ItemSlot.None ? 1 : 0)
                                         + (TheItem.Item.SocketColor3 != ItemSlot.None ? 1 : 0);
                    bool BSSlot_Wrist = Slot == CharacterSlot.Wrist && Character.WristBlacksmithingSocketEnabled && (Character.PrimaryProfession == Profession.Blacksmithing || Character.SecondaryProfession == Profession.Blacksmithing);
                    bool BSSlot_Glove = Slot == CharacterSlot.Hands && Character.HandsBlacksmithingSocketEnabled && (Character.PrimaryProfession == Profession.Blacksmithing || Character.SecondaryProfession == Profession.Blacksmithing);
                    bool BSSlot_Waist = Slot == CharacterSlot.Waist && Character.WaistBlacksmithingSocketEnabled;
                    // If there is no gem socket there, hide the selector
                    GemButton1.IsEnabled = TheItem.Item.SocketColor1 != ItemSlot.None || (nonBSSocketCount == 0 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    GemButton2.IsEnabled = TheItem.Item.SocketColor2 != ItemSlot.None || (nonBSSocketCount == 1 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    GemButton3.IsEnabled = TheItem.Item.SocketColor3 != ItemSlot.None || (nonBSSocketCount == 2 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist));
                    // Use the Gem's image if it exists
                    IconImageGem1.Source = TheItem.Gem1 != null ? Icons.AnIcon(TheItem.Gem1.IconPath) : null;
                    IconImageGem2.Source = TheItem.Gem2 != null ? Icons.AnIcon(TheItem.Gem2.IconPath) : null;
                    IconImageGem3.Source = TheItem.Gem3 != null ? Icons.AnIcon(TheItem.Gem3.IconPath) : null;
                    // Remove any previously registered Events that can be fired
                    ComparisonItemListGem1.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem1_SelectedItemsGemChanged);
                    ComparisonItemListGem2.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem2_SelectedItemsGemChanged);
                    ComparisonItemListGem3.SelectedItemsGemChanged -= new EventHandler(ComparisonItemListGem3_SelectedItemsGemChanged);
                    // Set up the selector so that it uses the right slot type
                    ComparisonItemListGem1.GemIndex = 1;
                    ComparisonItemListGem2.GemIndex = 2;
                    ComparisonItemListGem3.GemIndex = 3;
                    ComparisonItemListGem1.GemSlot = GetProperGemSlot(TheItem.Item.SocketColor1);
                    ComparisonItemListGem2.GemSlot = GetProperGemSlot(TheItem.Item.SocketColor2);
                    ComparisonItemListGem3.GemSlot = GetProperGemSlot(TheItem.Item.SocketColor3);
                    // Update the socket colors
                    SetSocketColors();
                    // Set the Gem lists' selected items to the core item's gems
                    ComparisonItemListGem1.SelectedItem = TheItem.Gem1;
                    ComparisonItemListGem2.SelectedItem = TheItem.Gem2;
                    ComparisonItemListGem3.SelectedItem = TheItem.Gem3;
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

        private CharacterSlot GetProperGemSlot(ItemSlot s) {
            return (s == ItemSlot.Meta ? CharacterSlot.Metas :
                   (s == ItemSlot.Cogwheel ? CharacterSlot.Cogwheels :
                   (s == ItemSlot.Hydraulic ? CharacterSlot.Hydraulics :
                   (CharacterSlot.Gems))));
        }

        private void SetSocketColor(Button button, ItemSlot slot) {
            Brush b;
            switch (slot) {
                // Site for colors: http://www.thereforesystems.com/silverlight-predefined-colors-what-color-are-they/
                case ItemSlot.Blue:      b = new SolidColorBrush(Color.FromArgb(255,000,000,139)); break; // Dark Blue
                case ItemSlot.Red:       b = new SolidColorBrush(Color.FromArgb(255,139,000,000)); break; // Dark Red
                case ItemSlot.Yellow:    b = new SolidColorBrush(Color.FromArgb(255,218,165,032)); break; // Goldenrod
                case ItemSlot.Meta:      b = new SolidColorBrush(Color.FromArgb(255,059,059,059)); break; // Very Dark Gray
                case ItemSlot.Prismatic: b = new SolidColorBrush(Color.FromArgb(255,097,117,132)); break; // Gray
                case ItemSlot.Cogwheel:  b = new SolidColorBrush(Color.FromArgb(255,000,000,000)); break; // Black
                case ItemSlot.Hydraulic: b = new SolidColorBrush(Color.FromArgb(255,255,255,0255)); break; // White
                default:                 b = new SolidColorBrush(SystemColors.ControlColor); break;
            }
            /*b = new LinearGradientBrush(new GradientStopCollection() {
                new GradientStop(){ Color = Color.FromArgb(255,000,000,139), Offset = 0 },
                new GradientStop(){ Color = Color.FromArgb(255,000,000,000), Offset = 1 },
            }, 90);*/
            button.Background = b;
            button.BorderBrush = b;
            button.BorderThickness = new Thickness(2);
        }
        private void SetSocketColors()
        {
            SetSocketColor(GemButton1, TheItem.Item.SocketColor1 != ItemSlot.None ? TheItem.Item.SocketColor1 : ((GetWhichIsBSSocket() == 1) ? ItemSlot.Prismatic : ItemSlot.None));
            SetSocketColor(GemButton2, TheItem.Item.SocketColor2 != ItemSlot.None ? TheItem.Item.SocketColor2 : ((GetWhichIsBSSocket() == 2) ? ItemSlot.Prismatic : ItemSlot.None));
            SetSocketColor(GemButton3, TheItem.Item.SocketColor3 != ItemSlot.None ? TheItem.Item.SocketColor3 : ((GetWhichIsBSSocket() == 3) ? ItemSlot.Prismatic : ItemSlot.None));
        }

        private int GetWhichIsBSSocket() {
            int retVal = -1;
            //
            int nonBSSocketCount = (TheItem.Item.SocketColor1 != ItemSlot.None ? 1 : 0)
                                 + (TheItem.Item.SocketColor2 != ItemSlot.None ? 1 : 0)
                                 + (TheItem.Item.SocketColor3 != ItemSlot.None ? 1 : 0);
            //
            bool BSSlot_Wrist = Slot == CharacterSlot.Wrist && Character.WristBlacksmithingSocketEnabled && Character.HasProfession(Profession.Blacksmithing);
            bool BSSlot_Glove = Slot == CharacterSlot.Hands && Character.HandsBlacksmithingSocketEnabled && Character.HasProfession(Profession.Blacksmithing);
            bool BSSlot_Waist = Slot == CharacterSlot.Waist && Character.WaistBlacksmithingSocketEnabled;
            // If there is no gem socket there, hide the selector
            if (nonBSSocketCount == 0 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist)) { retVal = 1; }
            if (nonBSSocketCount == 1 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist)) { retVal = 2; }
            if (nonBSSocketCount == 2 && (BSSlot_Wrist || BSSlot_Glove || BSSlot_Waist)) { retVal = 3; }
            //
            return retVal;
        }

        private void Close()
        {
            if (ListPopup.IsOpen) { ListPopup.IsOpen = false; }
            if (EnchantPopup.IsOpen) { EnchantPopup.IsOpen = false; }
            if (ListPopupGem1.IsOpen) { ListPopupGem1.IsOpen = false; }
            if (ListPopupGem2.IsOpen) { ListPopupGem2.IsOpen = false; }
            if (ListPopupGem3.IsOpen) { ListPopupGem3.IsOpen = false; }
            if (ReforgePopup.IsOpen) { ReforgePopup.IsOpen = false; }
            if (TinkerPopup.IsOpen) { TinkerPopup.IsOpen = false; }
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
        private void EnsureTinkerPopupsVisible()
        {
            GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow = App.Current.RootVisual.RenderSize.Height - transform.Transform(new Point(0, ComparisonListTinker.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                TinkerPopup.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
            }
        }

        #region Context Menu
        private void MainButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //TODO: *Usually* this is enough, but sometimes the positioning gets off, and it stays where the last context menu was.
            //Not really sure why, but I guess we should set the context menu location here too?
            ContextMenuService.GetContextMenu(MainButton).IsOpen = true;
            if (e != null) { e.Handled = true; }
        }
        private void ContextMenuItem_Opened(object sender, RoutedEventArgs e) {
            if (TheItem == null) { ContextMenuItem.IsOpen = false; }
            else { ContextItemName.Header = TheItem.Name; }
        }
        private void EditItem(object sender, RoutedEventArgs e)
        {
            new ItemEditor() { CurrentItem = TheItem.Item }.Show();
        }
#if SILVERLIGHT
        public class MyHyperlinkButton : HyperlinkButton
        {
            public MyHyperlinkButton(string navigateUri)
            {
                base.NavigateUri = new Uri(navigateUri);
                TargetName = "_blank";
            }
            public void ClickMe() { base.OnClick(); }
        }
#endif
        private void OpenInWowhead(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            if (App.Current.IsRunningOutOfBrowser) {
                MyHyperlinkButton button = new MyHyperlinkButton("http://www.wowhead.com/?item=" + TheItem.Id);
                button.ClickMe();
            } else {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.wowhead.com/?item=" + TheItem.Id), "_blank");
            }
#else
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://www.wowhead.com/?item=" + TheItem.Id));
#endif
        }
        private void RefreshItemFromWowhead(object sender, RoutedEventArgs e)
        {
            Item.LoadFromId(TheItem.Id, true, true, true, false);
        }
        #endregion
        private void MainButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsurePopupsVisible();
#else
            ListPopup.PlacementTarget = LayoutRoot;
            ListPopup.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonItemList.IsShown = true;
            ListPopup.IsOpen = true;
            ComparisonItemList.Focus();
        }
        private void GemButton1_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsureGem1PopupsVisible();
#else
            ListPopupGem1.PlacementTarget = GemButton1;
            ListPopupGem1.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonItemListGem1.IsShown = true;
            ListPopupGem1.IsOpen = true;
            ComparisonItemListGem1.Focus();
        }
        private void GemButton2_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsureGem2PopupsVisible();
#else
            ListPopupGem2.PlacementTarget = GemButton2;
            ListPopupGem2.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonItemListGem2.IsShown = true;
            ListPopupGem2.IsOpen = true;
            ComparisonItemListGem2.Focus();
        }
        private void GemButton3_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsureGem3PopupsVisible();
#else
            ListPopupGem3.PlacementTarget = GemButton3;
            ListPopupGem3.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonItemListGem3.IsShown = true;
            ListPopupGem3.IsOpen = true;
            ComparisonItemListGem3.Focus();
        }
        private void EnchantButton_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsurePopupsVisible();
#else
            EnchantPopup.PlacementTarget = LayoutRoot;
            EnchantPopup.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonListEnchant.IsShown = true;
            EnchantPopup.IsOpen = true;
            ComparisonListEnchant.Focus();
        }
        private void ReforgeButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsureReforgePopupsVisible();
#else
            ReforgePopup.PlacementTarget = ReforgeButton;
            ReforgePopup.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonListReforge.IsShown = true;
            ReforgePopup.IsOpen = true;
            ComparisonListReforge.Focus();
        }
        private void TinkerButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
#if SILVERLIGHT
            EnsureTinkerPopupsVisible();
#else
            TinkerPopup.PlacementTarget = LayoutRoot;
            TinkerPopup.Placement = PlacementMode.RelativePoint;
#endif
            ComparisonListTinker.IsShown = true;
            TinkerPopup.IsOpen = true;
            ComparisonListTinker.Focus();
        }
        #endregion

        #region Returning Gem Events
        public void ComparisonItemListGem1_SelectedItemsGemChanged(object sender, EventArgs e) { if (TheItem != null) TheItem.Gem1 = ComparisonItemListGem1.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        public void ComparisonItemListGem2_SelectedItemsGemChanged(object sender, EventArgs e) { if (TheItem != null) TheItem.Gem2 = ComparisonItemListGem2.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        public void ComparisonItemListGem3_SelectedItemsGemChanged(object sender, EventArgs e) { if (TheItem != null) TheItem.Gem3 = ComparisonItemListGem3.SelectedItem; character.OnCalculationsInvalidated(); character_CalculationsInvalidated(null, null); }
        #endregion

        #region ToolTips
        private void MainButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ListPopup.IsOpen)
            {
                MainPage.Tooltip.ItemInstance = gear;
                MainPage.Tooltip.Show(MainButton, 72, 0);
            }
        }
        private void GemButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = TheItem.Gem1;
                MainPage.Tooltip.Show(GemButton1, 22, 0);
            }
        }
        private void GemButton2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = TheItem.Gem2;
                MainPage.Tooltip.Show(GemButton2, 22, 0);
            }
        }
        private void GemButton3_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen)
            {
                MainPage.Tooltip.Item = TheItem.Gem3;
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
        private void ReforgeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ListPopup.IsOpen && !ListPopupGem1.IsOpen && !ListPopupGem2.IsOpen && !ListPopupGem3.IsOpen && !ReforgePopup.IsOpen)
            {
                MainPage.Tooltip.Item = reforge;
                MainPage.Tooltip.Show(ReforgeButton, 22, 0);
            }
        }
        private void TinkerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!TinkerPopup.IsOpen)
            {
                MainPage.Tooltip.Item = tinkering;
                MainPage.Tooltip.Show(TinkerButton, 72, 0);
            }
        }

        private void AnyButton_MouseLeave(object sender, MouseEventArgs e) { MainPage.Tooltip.Hide(); }
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
        private bool _ShowTinker = false; public bool ShowTinker { get { return _ShowTinker; } set { _ShowTinker = value; UpdateButtonOptions(); } }
        private bool _ShowBSSocket = false; public bool ShowBSSocket { get { return _ShowBSSocket; } set { _ShowBSSocket = value; UpdateButtonOptions(); } }
        private bool _EnableBSSocket = true; public bool EnableBSSocket { get { return _EnableBSSocket; } set { _EnableBSSocket = value; UpdateButtonOptions(); } }

        public Boolean BSSocketIsChecked {
            get { return (Boolean)this.GetValue(BSSocketProperty); }
            set {
                // Set the Value
                this.SetValue(BSSocketProperty, value);
                // Update Character's slot check, if need be. The if is to prevent stack overflow
                if (Slot == CharacterSlot.Wrist && Character.WristBlacksmithingSocketEnabled != value) { Character.WristBlacksmithingSocketEnabled = value; }
                else if (Slot == CharacterSlot.Hands && Character.HandsBlacksmithingSocketEnabled != value) { Character.HandsBlacksmithingSocketEnabled = value; }
                else if (Slot == CharacterSlot.Waist && Character.WaistBlacksmithingSocketEnabled != value) { Character.WaistBlacksmithingSocketEnabled = value; }
                // Update the Checkbox, if need be. The if is to prevent stack overflow
                if (CK_BSSocket.IsChecked != value) { CK_BSSocket.IsChecked = value; }
            }
        }
        public static readonly DependencyProperty BSSocketProperty = DependencyProperty.Register(
            "BSSocketIsChecked", typeof(Boolean), typeof(ItemButtonWithEnchant), new PropertyMetadata(false));

        public void SetButtonOptions(bool showReforge, bool showEnchant, bool showGems, bool showGem3, bool showBSSocket, bool enableBSSocket, bool showTinker)
        {
            _ShowGems = showGems;
            _ShowGem3 = showGem3;
            _ShowEnchant = showEnchant;
            _ShowReforge = showReforge;
            _ShowTinker = showTinker;
            _ShowBSSocket = showBSSocket;
            _EnableBSSocket = enableBSSocket;
            //
            UpdateButtonOptions();
        }
        public void UpdateButtonOptions() {
            GemButton1.Visibility = ShowGems ? Visibility.Visible : Visibility.Collapsed;
            GemButton2.Visibility = ShowGems ? Visibility.Visible : Visibility.Collapsed;
            GemButton3.Visibility = (ShowGems && ShowGem3) ? Visibility.Visible : Visibility.Collapsed;
            EnchantButton.Visibility = ShowEnchant ? Visibility.Visible : Visibility.Collapsed;
            ReforgeButton.Visibility = ShowReforge ? Visibility.Visible : Visibility.Collapsed;
            TinkerButton.Visibility = ShowTinker ? Visibility.Visible : Visibility.Collapsed;
            CK_BSSocket.Visibility = ShowBSSocket ? Visibility.Visible : Visibility.Collapsed;
            CK_BSSocket.IsEnabled = EnableBSSocket;
            if (CK_BSSocket.IsChecked != BSSocketIsChecked)
            {
                CK_BSSocket.IsChecked = BSSocketIsChecked;
            }
        }
    }
}
