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
				comparisonGraph1.Character = _character;
			}
		}

		public ItemComparison()
		{
			InitializeComponent();
		}

		public void LoadGearBySlot(Character.CharacterSlot slot)
		{
			List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
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

			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
			comparisonGraph1.EquipSlot = slot == Character.CharacterSlot.Gems || slot == Character.CharacterSlot.Metas ?
				Character.CharacterSlot.None : slot;
		}

		public void LoadEnchantsBySlot(Item.ItemSlot slot, CharacterCalculationsBase currentCalculations)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetEnchantCalculations(slot, Character, currentCalculations).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadBuffs(CharacterCalculationsBase currentCalculations, Buff.BuffType buffType, bool activeOnly)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetBuffCalculations(Character, currentCalculations, buffType, activeOnly).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadCurrentGearEnchantsBuffs(CharacterCalculationsBase currentCalculations)
		{
			List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
			if (Items != null && Character != null)
			{
				foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
					if (Character[slot] != null)
						itemCalculations.Add(Calculations.GetItemCalculations(Character[slot], Character, slot));

				foreach (ComparisonCalculationBase calc in Calculations.GetEnchantCalculations(Item.ItemSlot.None, Character, currentCalculations))
					if (calc.Equipped)
						itemCalculations.Add(calc);

				foreach (ComparisonCalculationBase calc in Calculations.GetBuffCalculations(Character, currentCalculations, Buff.BuffType.All, true))
					itemCalculations.Add(calc);
			}

			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}

		public void LoadCustomChart(string chartName)
		{
			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = Calculations.GetCustomChartData(Character, chartName);
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}
	}
}
