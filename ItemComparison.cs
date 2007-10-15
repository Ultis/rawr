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

			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
			comparisonGraph1.EquipSlot = slot == Character.CharacterSlot.Gems || slot == Character.CharacterSlot.Metas ?
				Character.CharacterSlot.None : slot;
		}

		public void LoadEnchantsBySlot(Item.ItemSlot slot, CalculatedStats currentCalculations)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetEnchantCalculations(slot, Character, currentCalculations).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadBuffs(CalculatedStats currentCalculations, Buffs.BuffCatagory buffCategory)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetBuffCalculations(Character, currentCalculations, buffCategory).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadCurrentGearEnchantsBuffs(CalculatedStats currentCalculations)
		{
			List<ItemCalculation> itemCalculations = new List<ItemCalculation>();
			if (Items != null && Character != null)
			{
				foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
					if (Character[slot] != null)
						itemCalculations.Add(Calculations.GetItemCalculations(Character[slot], Character, slot));

				foreach (ItemCalculation calc in Calculations.GetEnchantCalculations(Item.ItemSlot.None, Character, currentCalculations))
					if (calc.Equipped)
						itemCalculations.Add(calc);

				foreach (ItemCalculation calc in Calculations.GetBuffCalculations(Character, currentCalculations, Buffs.BuffCatagory.CurrentBuffs))
					itemCalculations.Add(calc);
			}

			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}

		public void LoadCombatTable(CalculatedStats currentCalculations)
		{
			ItemCalculation calcMiss = new ItemCalculation();
			ItemCalculation calcDodge = new ItemCalculation();
			ItemCalculation calcCrit = new ItemCalculation();
			ItemCalculation calcCrush = new ItemCalculation();
			ItemCalculation calcHit = new ItemCalculation();
			calcMiss.ItemName =  "    Miss    ";
			calcDodge.ItemName = "   Dodge   ";
			calcCrit.ItemName =  "  Crit  ";
			calcCrush.ItemName = " Crush ";
			calcHit.ItemName =   "Hit";

			float crits = 2.6f - currentCalculations.CappedCritReduction;
			float crushes = Math.Max(Math.Min(100f - (crits + (currentCalculations.DodgePlusMiss)), 15f), 0f);
			float hits = Math.Max(100f - (crits + crushes + (currentCalculations.DodgePlusMiss)), 0f);
			
			calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculations.Miss;
			calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculations.Dodge;
			calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
			calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
			calcHit.OverallPoints = calcHit.SurvivalPoints = hits;

			comparisonGraph1.RoundValues = false;
			comparisonGraph1.ItemCalculations = new ItemCalculation[] { calcMiss, calcDodge, calcCrit, calcCrush, calcHit };
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}
	}
}
