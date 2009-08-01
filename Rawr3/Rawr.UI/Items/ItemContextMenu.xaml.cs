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
	public partial class ItemContextMenu : UserControl
	{

        public bool IsShown { get { return ContextPopup.IsOpen; } }

        public CharacterSlot Slot { get; set; }

        private Character character;
        public Character Character
        {
            get { return character; }
            set { character = value; Hide(); }
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

                    ContextItemName.Content = selectedItemInstance.Item.Name;
                }
            }
        }

		public ItemContextMenu()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        public void Show(UIElement relativeTo) { Show(relativeTo, 0, 0); }
        public void Show(UIElement relativeTo, double offsetX, double offsetY)
        {
            ContextList.SelectedIndex = -1;

            GeneralTransform gt = relativeTo.TransformToVisual((UIElement)this.Parent);
            Point offset = gt.Transform(new Point(offsetX, offsetY));
            ContextPopup.VerticalOffset = offset.Y;
            ContextPopup.HorizontalOffset = offset.X;
            ContextPopup.IsOpen = true;

            ContextGrid.Measure(Application.Current.RootVisual.DesiredSize);

            GeneralTransform transform = relativeTo.TransformToVisual(Application.Current.RootVisual);
            double distBetweenBottomOfPopupAndBottomOfWindow =
                Application.Current.RootVisual.RenderSize.Height - offsetY -
                transform.Transform(new Point(0, ContextGrid.DesiredSize.Height)).Y;
            if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
            {
                ContextPopup.VerticalOffset += distBetweenBottomOfPopupAndBottomOfWindow;
            }
            ContextList.Focus();
        }

        public void Hide()
        {
            ContextPopup.IsOpen = false;
        }

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement focus = (FocusManager.GetFocusedElement() as FrameworkElement);
            DependencyObject parent = VisualTreeHelper.GetParent(focus);
            while (parent != null && parent != ContextGrid) parent = VisualTreeHelper.GetParent(parent);
            if (parent == null)
            {
                Hide();
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
                else if (contextItem == ContextOpenWowhead)
                {
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.wowhead.com/?item=" + SelectedItemInstance.Id), "_blank");
                }
                else if (contextItem == ContextAddCustom)
                {
                    CustomItemInstance custom = new CustomItemInstance(Character, SelectedItemInstance);
                    custom.Closed += new EventHandler(custom_Closed);
                    custom.Show();
                }
                else if (contextItem == ContextRefreshArmory)
                {
                    Item.LoadFromId(SelectedItemInstance.Id, true, true, false);
                }
                else if (contextItem == ContextRefreshWowhead)
                {
                    Item.LoadFromId(SelectedItemInstance.Id, true, true, true);
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
                Hide();
            }
        }

        private void custom_Closed(object sender, EventArgs e)
        {
            CustomItemInstance custom = sender as CustomItemInstance;
            if (custom.DialogResult.GetValueOrDefault())
            {

            }
        }
	}
}