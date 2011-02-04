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
    public partial class ItemDisplay : UserControl
    {
        public ItemInstance DisplayedItem { get { return DataContext as ItemInstance; } }

        public ItemDisplay(ItemInstance itemInstance)
        {
            DataContext = itemInstance;
            InitializeComponent();
            FixGemCountVisibilities(itemInstance);
        }

        private void FixGemCountVisibilities(ItemInstance itemInstance) {
            Name_Gem1.Visibility = Image_Gem1.Visibility = Border_Gem1.Visibility = Visibility.Collapsed;
            Name_Gem2.Visibility = Image_Gem2.Visibility = Border_Gem2.Visibility = Visibility.Collapsed;
            Name_Gem3.Visibility = Image_Gem3.Visibility = Border_Gem3.Visibility = Visibility.Collapsed;

            bool bssocketmaybe = false;
            if (itemInstance.Slot == ItemSlot.Hands || itemInstance.Slot == ItemSlot.Wrist || itemInstance.Slot == ItemSlot.Waist) { bssocketmaybe = true; }

            if (itemInstance.Item.SocketColor1 != ItemSlot.None || bssocketmaybe) { Name_Gem1.Visibility = Image_Gem1.Visibility = Border_Gem1.Visibility = Visibility.Visible; bssocketmaybe = false; }
            if (itemInstance.Item.SocketColor2 != ItemSlot.None || bssocketmaybe) { Name_Gem2.Visibility = Image_Gem2.Visibility = Border_Gem2.Visibility = Visibility.Visible; bssocketmaybe = false; }
            if (itemInstance.Item.SocketColor3 != ItemSlot.None || bssocketmaybe) { Name_Gem3.Visibility = Image_Gem3.Visibility = Border_Gem3.Visibility = Visibility.Visible; bssocketmaybe = false; }
        }

        private void ShowTooltip(object sender, MouseEventArgs e)
        {
            MainPage.Tooltip.ItemInstance = DisplayedItem;
            MainPage.Tooltip.Show(this, ActualWidth, 0);
        }

        private void HideTooltip(object sender, MouseEventArgs e)
        {
            MainPage.Tooltip.Hide();
        }
    }
}