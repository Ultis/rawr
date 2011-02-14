using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
#if SILVERLIGHT
    public partial class ItemContextMenu : UserControl
#else
    public partial class ItemContextMenu : ContextMenu
#endif
    {
#if SILVERLIGHT
        public bool IsOpen 
        { 
            get { return ContextPopup.IsOpen; }
            set { ContextPopup.IsOpen = value; }
        }
#endif

        public CharacterSlot Slot { get; set; }

        private Character character;
        public Character Character
        {
            get { return character; }
            set 
            { 
                character = value;
                IsOpen = false;
            }
        }

        private ItemInstance selectedItemInstance;
        public ItemInstance SelectedItemInstance
        {
            get { return selectedItemInstance; }
            set
            {
                selectedItemInstance = value;
                if (selectedItemInstance != null)
                {
                    if (Character.CustomItemInstances.Contains(selectedItemInstance)) ContextDeleteCustom.Visibility = Visibility.Visible;
                    else ContextDeleteCustom.Visibility = Visibility.Collapsed;

#if SILVERLIGHT
                    ContextItemName.Content = selectedItemInstance.Item.Name;
                    ContextOpenWowhead.NavigateUri = new Uri("http://www.wowhead.com/?item=" + selectedItemInstance.Id);
#else
                    ContextItemName.Header = selectedItemInstance.Item.Name;
#endif
                }
            }
        }

        public ItemContextMenu()
        {
            // Required to initialize variables
            InitializeComponent();
        }

#if SILVERLIGHT
        public void Show(UIElement relativeTo) { Show(relativeTo, 0, 0); }
        public void Show(UIElement relativeTo, double offsetX, double offsetY)
        {            
            ContextList.SelectedIndex = -1;

            GeneralTransform gt = relativeTo.TransformToVisual((UIElement)this.Parent);
            Point offset = gt.Transform(new Point(offsetX, offsetY));
            ContextPopup.VerticalOffset = offset.Y;
            ContextPopup.HorizontalOffset = offset.X;
            ContextPopup.IsOpen = true;

            ContextGrid.Measure(App.Current.RootVisual.DesiredSize);

            GeneralTransform transform = relativeTo.TransformToVisual(App.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow =
                App.Current.RootVisual.RenderSize.Height - offsetY -
                transform.Transform(new Point(0, ContextGrid.DesiredSize.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ContextPopup.VerticalOffset += distBetweenBottomOfPopupAndBottomOfWindow;
            }
            ContextList.Focus();
        }

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement focus = (App.GetFocusedElement() as FrameworkElement);
            DependencyObject parent = VisualTreeHelper.GetParent(focus);
            while (parent != null && parent != ContextGrid) parent = VisualTreeHelper.GetParent(parent);
            if (parent == null)
            {
                IsOpen = false;
            }
        }

        private void ContextList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem contextItem = ContextList.SelectedItem as ListBoxItem;
            if (ContextList.SelectedIndex > 1)
            {
                if (contextItem == ContextEdit)
                {
                    new ItemEditor() { CurrentItem = SelectedItemInstance.Item }.Show();
                }
                else if (contextItem == ContextEquip)
                {
                    Character[Slot] = SelectedItemInstance;
                }
                /*else if (contextItem == ContextOpenWowhead)
                {
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.wowhead.com/?item=" + SelectedItemInstance.Id), "_blank");
                }*/
                else if (contextItem == ContextAddCustom)
                {
                    CustomItemInstance custom = new CustomItemInstance(Character, SelectedItemInstance);
                    custom.Closed += new EventHandler(custom_Closed);
                    custom.Show();
                }
                else if (contextItem == ContextRefreshArmory)
                {
                    Item.LoadFromId(SelectedItemInstance.Id, true, true, false, false);
                }
                else if (contextItem == ContextRefreshWowhead)
                {
                    Item.LoadFromId(SelectedItemInstance.Id, true, true, true, false);
                }
                else if (contextItem == ContextDeleteCustom)
                {
                    Character.CustomItemInstances.Remove(selectedItemInstance);
                    ItemCache.OnItemsChanged();
                }
                else if (contextItem == ContextEvaluateUpgrade)
                {
                    OptimizeWindow optimizer = new OptimizeWindow(Character);
                    optimizer.Show();
                    optimizer.EvaluateUpgrades(SelectedItemInstance.Item);
                }
                IsOpen = false;
            }
        }
#endif

        private void EditItem(object sender, RoutedEventArgs e)
        {
            new ItemEditor() { CurrentItem = SelectedItemInstance.Item }.Show();
        }

        private void OpenInWowhead(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.wowhead.com/?item=" + SelectedItemInstance.Id), "_blank");
#else
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://www.wowhead.com/?item=" + SelectedItemInstance.Id));
#endif
        }

        private void RefreshItemFromArmory(object sender, RoutedEventArgs e)
        {
            Item.LoadFromId(SelectedItemInstance.Id, true, true, false, false);
        }

        private void RefreshItemFromWowhead(object sender, RoutedEventArgs e)
        {
            Item.LoadFromId(SelectedItemInstance.Id, true, true, true, false);
        }

        private void EquipItem(object sender, RoutedEventArgs e)
        {
            Character[Slot] = SelectedItemInstance;
        }

        private void AddCustomGemming(object sender, RoutedEventArgs e)
        {
            CustomItemInstance custom = new CustomItemInstance(Character, SelectedItemInstance);
            custom.Closed += new EventHandler(custom_Closed);
            custom.Show();
        }

        private void DeleteCustomGemming(object sender, RoutedEventArgs e)
        {
            Character.CustomItemInstances.Remove(selectedItemInstance);
            ItemCache.OnItemsChanged();
        }

        private void EvaluateUpgrade(object sender, RoutedEventArgs e)
        {
            OptimizeWindow optimizer = new OptimizeWindow(Character);
            optimizer.Show();
            optimizer.EvaluateUpgrades(new Optimizer.SuffixItem() { Item = SelectedItemInstance.Item, RandomSuffixId = SelectedItemInstance.RandomSuffixId });
        }

        private void custom_Closed(object sender, EventArgs e)
        {
            CustomItemInstance custom = sender as CustomItemInstance;
            if (custom.DialogResult.GetValueOrDefault(false))
            {

            }
        }
    }
}