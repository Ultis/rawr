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
	public partial class ItemFilterPopup : UserControl
	{
		public ItemFilterPopup()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void HidePopup(object sender, RoutedEventArgs e)
		{
            FilterPopup.IsOpen = false;
		}

		private void ShowPopup(object sender, RoutedEventArgs e)
		{
            FilterTree.ItemsSource = ItemFilter.FilterList.FilterList;
            FilterPopup.IsOpen = true;
		}

        private void FocusLost(object sender, RoutedEventArgs e)
        {
            FrameworkElement focus = (FocusManager.GetFocusedElement() as FrameworkElement);
            DependencyObject parent = VisualTreeHelper.GetParent(focus);
            while (parent != null && parent != FilterTree) parent = VisualTreeHelper.GetParent(parent);
            if (parent == null)
            {
                Toggle.IsChecked = false;
            }
        }
	}
}