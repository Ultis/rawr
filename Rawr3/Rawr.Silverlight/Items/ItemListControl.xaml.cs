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
using System.Collections.ObjectModel;

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
                    if (IsShown) BuildListItems();
                }
            }
        }

        public bool IsEnchantList { get; set; }

		private Character.CharacterSlot _slot;
		public Character.CharacterSlot Slot
		{
			get { return _slot; }
			set
			{
				_slot = value;
				IsPopulated = false;
			}
		}

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
				if (isShown) listBoxItems.Focus();
            }
        }

		// Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register("Items", 
			typeof(ItemListItemCollection), 
			typeof(ItemListControl),
			new PropertyMetadata(null));
		public ItemListItemCollection Items
		{
			get
			{
				ItemListItemCollection items = (ItemListItemCollection)GetValue(ItemsProperty);
				if (items == null)
				{
					Items = items = new ItemListItemCollection();
				}
				return items;
			}
			set { SetValue(ItemsProperty, value); }
		}
        #endregion

        private void BuildListItems()
        {
            if (Character == null) return;
            bool seenEquippedItem = (Character[Slot] == null);

            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
            if (IsEnchantList)
            {
                CharacterCalculationsBase current = Calculations.GetCharacterCalculations(Character);
                if (Character != null && current != null)
                {
                    itemCalculations = Calculations.GetEnchantCalculations(Item.GetItemSlotByCharacterSlot(Slot), Character, current);
                }
            }
            else
            {
				List<ItemInstance> relevantItemInstances = Character.GetRelevantItemInstances(Slot);
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

            float maxValue = itemCalculations.Count == 0 ? 100f : itemCalculations.Max(c => c.OverallPoints);

			Items.Clear();
			List<ItemListItem> itemListItems = new List<ItemListItem>();
			ItemListItem selectedItem = null;
			int selectedEnchantId = 0;
			if (IsEnchantList)
			{
				Enchant selectedEnchant = Character.GetEnchantBySlot(Slot);
				if (selectedEnchant != null) selectedEnchantId = selectedEnchant.Id;
			}
			else
			{
				ComparisonCalculationBase emptyCalcs = Calculations.CreateNewComparisonCalculation();
				emptyCalcs.Name = "Empty";
				emptyCalcs.Item = new Item();
				emptyCalcs.Item.Name = "Empty";
				emptyCalcs.ItemInstance = new ItemInstance();
				emptyCalcs.ItemInstance.Id = -1;
				emptyCalcs.Equipped = Character[Slot] == null;
				itemCalculations.Add(emptyCalcs);
			}
			foreach (ComparisonCalculationBase calc in itemCalculations)
			{
				ItemListItem itemListItem = new ItemListItem(calc, maxValue, 344d);
				itemListItems.Add(itemListItem);
				if (IsEnchantList)
				{
					if (itemListItem.EnchantId == selectedEnchantId)
						selectedItem = itemListItem;
				}
				else
				{
					if (calc.ItemInstance == Character[Slot] ||
						(calc.ItemInstance.Id == -1 && Character[Slot] == null))
						selectedItem = itemListItem;
				}
			}
			Items.AddRange(itemListItems);
			listBoxItems.SelectedItem = selectedItem;
			listBoxItems.Focus();
            IsPopulated = true;
        }

		private void BuildSorts()
		{
			comboBoxSort.Items.Clear();
			comboBoxSort.Items.Add("Alphabetical");
			comboBoxSort.Items.Add("Overall");
            if (Calculations.Instance != null)
            {
                foreach (var kvp in Calculations.SubPointNameColors) comboBoxSort.Items.Add(kvp.Key);
            }
			comboBoxSort.SelectedIndex = 1;
		}

        private void item_MouseLeave(object sender, MouseEventArgs e)
        {
			FrameworkElement fe = sender as FrameworkElement;
			ItemTooltip.Hide();
        }

        private void item_MouseEnter(object sender, MouseEventArgs e)
        {
			FrameworkElement fe = sender as FrameworkElement;
			ItemListItem listItem = fe.DataContext as ItemListItem;

			if (listItem.ItemInstance != null) ItemTooltip.ItemInstance = listItem.ItemInstance;
			else ItemTooltip.Item = listItem.Item;

			ItemTooltip.Show(fe, 367, 0);
        }

		public ItemListControl()
		{
			// Required to initialize variables
			InitializeComponent();
			this.DataContext = this;
			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
			BuildSorts();
		}

		private void Calculations_ModelChanged(object sender, EventArgs e)
		{
			BuildSorts();
		}

        private void listBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IsEnchantList)
                {
                    ItemListItem listItem = ((ListBox)sender).SelectedItem as ItemListItem;
                    ItemInstance copy = Character[Slot].Clone();
                    copy.EnchantId = listItem.EnchantId;
                    IsShown = false;
                    IsPopulated = false;
                    Character[Slot] = copy;
                }
                else
                {
                    ItemListItem listItem = ((ListBox)sender).SelectedItem as ItemListItem;
                    IsShown = false;
                    IsPopulated = false;
					Character[Slot] = listItem.ItemInstance.Id == -1 ? null : listItem.ItemInstance;
                }
				this.Close();
            }
            catch { }
        }

		private void UserControl_LostFocus(object sender, RoutedEventArgs e)
		{
			FrameworkElement focus = (FocusManager.GetFocusedElement() as FrameworkElement);
			if (focus is ComboBoxItem && comboBoxSort.Items.Contains(focus.DataContext))
				focus = comboBoxSort;
			DependencyObject parent = VisualTreeHelper.GetParent(focus);
			while (parent != null && parent != this)
				parent = VisualTreeHelper.GetParent(parent);
			if (parent == null)
			{
				//focus isn't within this control
				this.Close();
			}
		}

		private void Close()
		{
			this.IsShown = false;
			if (this.Parent is Popup)
				(this.Parent as Popup).IsOpen = false;
		}

		private void Filter_TextChanged(object sender, TextChangedEventArgs e)
		{
			(sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}

		private void comboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboBoxSort.SelectedIndex >= 0)
				Items.Sort = (ComparisonGraph.ComparisonSort)(comboBoxSort.SelectedIndex - 2);
		}
	}
}