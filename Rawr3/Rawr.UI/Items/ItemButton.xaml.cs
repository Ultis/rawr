using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr;
using System.Windows.Controls.Primitives;

namespace Rawr.UI
{
	public partial class ItemButton : UserControl
	{

        private CharacterSlot slot;
        public CharacterSlot Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                ComparisonItemList.Slot = slot;
            }
        }

        private ItemInstance item;
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) character.AvailableItemsChanged -= new EventHandler(character_CalculationsInvalidated);
                character = value;
                ComparisonItemList.Character = character;
                if (character != null)
                {
                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                    character_CalculationsInvalidated(null, null);
                }
            }
        }

        public void character_CalculationsInvalidated(object sender, EventArgs e)
		{
            if (character != null)
            {
                item = character[Slot];
                if (item == null)
                {
                    IconImage.Source = null;
                }
                else
                {
                    IconImage.Source = Icons.ItemIcon(item.Item.IconPath);
                }
            }
        }

		public ItemButton()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void EnsurePopupsVisible()
		{
			GeneralTransform transform = TransformToVisual(App.Current.RootVisual);
			double distBetweenBottomOfPopupAndBottomOfWindow =
                App.Current.RootVisual.RenderSize.Height -
				transform.Transform(new Point(0, ComparisonItemList.Height)).Y;
			if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
			{
				ListPopup.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
			}
		}

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
			EnsurePopupsVisible();
            ComparisonItemList.IsShown = true;
            ListPopup.IsOpen = true;
			ComparisonItemList.Focus();
		}

        private void MainButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen)
            {
                MainPage.Tooltip.ItemInstance = item;
                MainPage.Tooltip.Show(MainButton, 72, 0);
            }
        }

        private void MainButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MainPage.Tooltip.Hide();
        }

        #region WPF Popup Handling
#if !SILVERLIGHT
        private void Close()
        {
            if (ListPopup.IsOpen)
            {
                ListPopup.IsOpen = false;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured == this && e.OriginalSource == this)
            {
                Close();
            }
        }

        private bool IsDescendant(DependencyObject reference, DependencyObject node)
        {
            DependencyObject o = node;
            while (o != null)
            {
                if (o == reference)
                {
                    return true;
                }
                Popup popup = o as Popup;
                if (popup != null)
                {
                    o = popup.Parent;
                    if (o == null)
                    {
                        o = popup.PlacementTarget;
                    }
                }
                else
                {
                    //handle content elements separately
                    ContentElement contentElement = o as ContentElement;
                    if (contentElement != null)
                    {
                        DependencyObject parent = ContentOperations.GetParent(contentElement);
                        if (parent != null)
                        {
                            o = parent;
                        }
                        FrameworkContentElement fce = contentElement as FrameworkContentElement;
                        if (fce != null)
                        {
                            o = fce.Parent;
                        }
                        else
                        {
                            o = null;
                        }
                    }
                    else
                    {
                        //also try searching for parent in framework elements (such as DockPanel, etc)
                        FrameworkElement frameworkElement = o as FrameworkElement;
                        if (frameworkElement != null)
                        {
                            DependencyObject parent = frameworkElement.Parent;
                            if (parent != null)
                            {
                                o = parent;
                                continue;
                            }
                        }

                        //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
                        o = VisualTreeHelper.GetParent(o);
                    }
                }
            }
            return false;
        }

        private void UserControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != this)
            {
                if (e.OriginalSource == this)
                {
                    // If capture is null or it's not below the combobox, close. 
                    if (Mouse.Captured == null || !IsDescendant(this, Mouse.Captured as DependencyObject))
                    {
                        Close();
                    }
                }
                else
                {
                    if (IsDescendant(this, e.OriginalSource as DependencyObject))
                    {
                        // Take capture if one of our children gave up capture
                        if (ListPopup.IsOpen && Mouse.Captured == null)
                        {
                            Mouse.Capture(this, CaptureMode.SubTree);
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }
#endif
        #endregion
	}
}