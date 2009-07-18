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
                ComparisonEnchantList.Slot = slot;
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
                ComparisonEnchantList.Character = character;
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
                item = character[Slot];
                if (item == null)
                {
                    IconImage.Source = null;
                    EnchantButton.Content = "";
                    gear = null;
                    gear = null;
                }
                else
                {
                    IconImage.Source = Icons.ItemIcon(item.Item.IconPath);
                    EnchantButton.Content = item.Enchant.ShortName;
                    gear = item;

                    Item eItem = new Item();
                    eItem.Name = item.Enchant.Name;
                    eItem.Quality = ItemQuality.Temp;
                    eItem.Stats = item.Enchant.Stats;
                    enchant = eItem;
                }
            }
        }

		public ItemButtonWithEnchant()
		{
			// Required to initialize variables
            InitializeComponent();
            ComparisonEnchantList.IsEnchantList = true;
		}

		private void EnsurePopupsVisible()
		{
			GeneralTransform transform = TransformToVisual(Application.Current.RootVisual);
			double distBetweenBottomOfPopupAndBottomOfWindow =
				Application.Current.RootVisual.DesiredSize.Height -
				transform.Transform(new Point(0, ComparisonItemList.Height)).Y;
			if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
			{
				ListPopup.VerticalOffset = distBetweenBottomOfPopupAndBottomOfWindow;
			} 
			distBetweenBottomOfPopupAndBottomOfWindow =
                 Application.Current.RootVisual.RenderSize.Height -
				 transform.Transform(new Point(0, 66 + ComparisonEnchantList.Height)).Y;
			if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
			{
				EnchantPopup.VerticalOffset = 66 + distBetweenBottomOfPopupAndBottomOfWindow;
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

        private void MainButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ListPopup.IsOpen)
            {
                MainPage.Tooltip.ItemInstance = gear;
                MainPage.Tooltip.Show(MainButton, 72, 0);
            }
        }

        private void MainButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MainPage.Tooltip.Hide();
        }

        private void EnchantButton_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            MainPage.Tooltip.Hide();
			EnsurePopupsVisible();
			ComparisonEnchantList.IsShown = true;
			EnchantPopup.IsOpen = true;
			ComparisonEnchantList.Focus();
        }

		private void EnchantButton_MouseEnter(object sender, MouseEventArgs e)
		{
            if (!EnchantPopup.IsOpen)
            {
                MainPage.Tooltip.Item = enchant;
                MainPage.Tooltip.Show(EnchantButton, 72, 0);
            }
		}

		private void EnchantButton_MouseLeave(object sender, MouseEventArgs e)
		{
            MainPage.Tooltip.Hide();
		}

	}
}