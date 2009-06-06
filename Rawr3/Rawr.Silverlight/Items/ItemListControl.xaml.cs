using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Silverlight
{
	public partial class ItemListControl : UserControl
    {

        #region Getters/Setters
        private bool isPopulated;
        public bool IsPopulated
        {
            get { return isPopulated; }
            set
            {
                isPopulated = value;
                if (!isPopulated)
                {
                    ListControl.Items.Clear();
                    if (IsShown) BuildListItems();
                }
            }
        }

        public bool Enchant { get; set; }

        public Character.CharacterSlot Slot { get; set; }

        public Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) character.CalculationsInvalidated -= new EventHandler(character_CalculationsInvalidated);
                character = value;
                character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                IsPopulated = false;
            }
        }

        private void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            IsPopulated = false;
        }

        private bool isShown;
        public bool IsShown
        {
            get { return isShown; }
            set
            {
                isShown = value;
                if (isShown && !IsPopulated) BuildListItems();
            }
        }
        #endregion

        private void BuildListItems()
        {
            if (Character == null) return;
            bool seenEquippedItem = (Character[Slot] == null);

            List<ItemInstance> relevantItemInstances = Character.GetRelevantItemInstances(Slot);
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>(relevantItemInstances.Count + 1);
            if (Enchant)
            {
                CharacterCalculationsBase current = Calculations.GetCharacterCalculations(Character);
                if (Character != null && current != null)
                {
                    itemCalculations = Calculations.GetEnchantCalculations(Item.GetItemSlotByCharacterSlot(Slot), Character, current);
                }
            }
            else
            {
                if (relevantItemInstances.Count > 0)
                {
                    foreach (ItemInstance item in relevantItemInstances)
                    {
                        if (!seenEquippedItem && Character[Slot].Equals(item)) seenEquippedItem = true;
                        itemCalculations.Add(Calculations.GetItemCalculations((ItemInstance)item, Character, Slot));
                    }
                }
                if (!seenEquippedItem) itemCalculations.Add(Calculations.GetItemCalculations(Character[Slot], Character, Slot));
            }

            float maxPoints = itemCalculations.Max(c => c.OverallPoints);

            foreach (ComparisonCalculationBase calc in itemCalculations.OrderByDescending(c => c.OverallPoints))
            {
                ListBoxItem item = new ListBoxItem();
                item.MouseEnter += new MouseEventHandler(item_MouseEnter);
                item.MouseLeave += new MouseEventHandler(item_MouseLeave);
                StackPanel spMain = new StackPanel();
                item.Content = spMain;

                StackPanel spName = new StackPanel();
                spName.Orientation = Orientation.Horizontal;

                if (calc.ItemInstance != null)
                {
                    item.Tag = calc.ItemInstance;
                    if (calc.ItemInstance == Character[Slot])
                        item.Background = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));

                    Image itemIcon = new Image();
                    itemIcon.Width = 32; itemIcon.Height = 32;
                    itemIcon.Margin = new Thickness(0, 0, 4, 2);
                    itemIcon.Source = Icons.ItemIcon(calc.ItemInstance.Item.IconPath);
                    spName.Children.Add(itemIcon);
                }
                else
                {
                    item.Tag = calc.Item;
                    if (Math.Abs(calc.Item.Id % 10000) == Character[Slot].EnchantId)
                        item.Background = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
                }

                TextBlock itemName = new TextBlock();
                itemName.VerticalAlignment = VerticalAlignment.Center;
                itemName.Text = calc.Name;
                itemName.TextWrapping = TextWrapping.Wrap;
                itemName.MaxWidth = ListControl.Width - 66;
                spName.Children.Add(itemName);

                spMain.Children.Add(spName);

                StackPanel spCalcs = new StackPanel();
                spCalcs.Orientation = Orientation.Horizontal;

                int i = 0;
                foreach (Color subColor in Calculations.SubPointNameColors.Values)
                {
                    Rectangle r = new Rectangle();
                    r.Height = 2;
                    r.Width = calc.SubPoints[i++] / maxPoints * (ListControl.Width - 30);
                    r.Fill = new SolidColorBrush(subColor);
                    spCalcs.Children.Add(r);
                }
                spMain.Children.Add(spCalcs);

                ListControl.Items.Add(item);
            }
            IsPopulated = true;
        }

        private void item_MouseLeave(object sender, MouseEventArgs e)
        {
            ItemTooltip.Hide();
        }

        private void item_MouseEnter(object sender, MouseEventArgs e)
        {
            ItemInstance ii = ((ListBoxItem)sender).Tag as ItemInstance;
            Item i = ((ListBoxItem)sender).Tag as Item;

            if (ii != null) ItemTooltip.ItemInstance = ii;
            else ItemTooltip.Item = i;

            ItemTooltip.Show((ListBoxItem)sender, 267, 0);
        }

		public ItemListControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void ListControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Enchant)
                {
                    Item item = (Item)((ListBoxItem)((ListBox)sender).SelectedItem).Tag;
                    ItemInstance copy = Character[Slot].Clone();
                    copy.EnchantId = item == null ? 0 : Math.Abs(item.Id % 10000);
                    IsShown = false;
                    IsPopulated = false;
                    Character[Slot] = copy;
                }
                else
                {
                    ItemInstance newItem = (ItemInstance)((ListBoxItem)((ListBox)sender).SelectedItem).Tag;
                    IsShown = false;
                    IsPopulated = false;
                    Character[Slot] = newItem;
                }
            }
            catch { }
        }
		
	}
}