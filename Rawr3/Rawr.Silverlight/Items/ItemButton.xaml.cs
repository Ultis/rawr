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

namespace Rawr.Silverlight
{
	public partial class ItemButton : UserControl
	{

        private Character.CharacterSlot slot;
        public Character.CharacterSlot Slot
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
			GeneralTransform transform = TransformToVisual(Application.Current.RootVisual);
			double distBetweenBottomOfPopupAndBottomOfWindow =
                Application.Current.RootVisual.RenderSize.Height -
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

	}
}