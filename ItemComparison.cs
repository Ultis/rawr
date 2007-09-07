using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ItemComparison : UserControl
	{
		public Item[] Items;

		public ComparisonGraph.ComparisonSort Sort
		{
			get { return comparisonGraph1.Sort; }
			set { comparisonGraph1.Sort = value; }
		}

		private Character _character;
		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
			}
		}

		public ItemComparison()
		{
			InitializeComponent();
		}

		public void LoadGearBySlot(Character.CharacterSlot slot)
		{
			List<ItemCalculation> itemCalculations = new List<ItemCalculation>();
			if (Items != null && Character != null)
			{
				foreach (Item item in Items)
				{
					if (item.FitsInSlot(slot))
					{
						itemCalculations.Add(Calculations.GetItemCalculations(item, Character, slot));
					}
				}
			}

			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
		}

		public void LoadEnchantsBySlot(Item.ItemSlot slot, CalculatedStats currentCalculations)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.ItemCalculations = Calculations.GetEnchantCalculations(slot, Character, currentCalculations).ToArray();
			}
		}

		public void LoadBuffs(CalculatedStats currentCalculations)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.ItemCalculations = Calculations.GetBuffCalculations(Character, currentCalculations).ToArray();
			}
		}
	}
}
