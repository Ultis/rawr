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
        private delegate void LoadGearBySlotCallback(Character.CharacterSlot slot);
	    
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
            //Check for thread context
            if (InvokeRequired)
            {
                LoadGearBySlotCallback d = new LoadGearBySlotCallback(LoadGearBySlot);
                Invoke(d, new object[] { slot });
            }
		    
			List<ItemBuffEnchantCalculation> itemCalculations = new List<ItemBuffEnchantCalculation>();
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

		public void LoadEnchantsBySlot(Item.ItemSlot slot, CharacterCalculation currentCalculations)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetEnchantCalculations(slot, Character, currentCalculations).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadBuffs(CharacterCalculation currentCalculations, Buff.BuffType buffType, bool activeOnly)
		{
			if (Items != null && Character != null)
			{
				comparisonGraph1.RoundValues = true;
				comparisonGraph1.ItemCalculations = Calculations.GetBuffCalculations(Character, currentCalculations, buffType, activeOnly).ToArray();
				comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
			}
		}

		public void LoadCurrentGearEnchantsBuffs(CharacterCalculation currentCalculations)
		{
			List<ItemBuffEnchantCalculation> itemCalculations = new List<ItemBuffEnchantCalculation>();
			if (Items != null && Character != null)
			{
				foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
					if (Character[slot] != null)
						itemCalculations.Add(Calculations.GetItemCalculations(Character[slot], Character, slot));

				foreach (ItemBuffEnchantCalculation calc in Calculations.GetEnchantCalculations(Item.ItemSlot.None, Character, currentCalculations))
					if (calc.Equipped)
						itemCalculations.Add(calc);

				foreach (ItemBuffEnchantCalculation calc in Calculations.GetBuffCalculations(Character, currentCalculations, Buff.BuffType.All, true))
					itemCalculations.Add(calc);
			}

			comparisonGraph1.RoundValues = true;
			comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}

		public void LoadCombatTable(CharacterCalculation currentCalculations)
		{
			ItemBuffEnchantCalculation calcMiss = new ItemBuffEnchantCalculation();
			ItemBuffEnchantCalculation calcDodge = new ItemBuffEnchantCalculation();
			ItemBuffEnchantCalculation calcCrit = new ItemBuffEnchantCalculation();
			ItemBuffEnchantCalculation calcCrush = new ItemBuffEnchantCalculation();
			ItemBuffEnchantCalculation calcHit = new ItemBuffEnchantCalculation();
			calcMiss.Name =  "    Miss    ";
			calcDodge.Name = "   Dodge   ";
			calcCrit.Name =  "  Crit  ";
			calcCrush.Name = " Crush ";
			calcHit.Name =   "Hit";

			float crits = 2.6f - currentCalculations.CappedCritReduction;
			float crushes = Math.Max(Math.Min(100f - (crits + (currentCalculations.DodgePlusMiss)), 15f), 0f);
			float hits = Math.Max(100f - (crits + crushes + (currentCalculations.DodgePlusMiss)), 0f);
			
			calcMiss.OverallPoints = calcMiss.MitigationPoints = currentCalculations.Miss;
			calcDodge.OverallPoints = calcDodge.MitigationPoints = currentCalculations.Dodge;
			calcCrit.OverallPoints = calcCrit.SurvivalPoints = crits;
			calcCrush.OverallPoints = calcCrush.SurvivalPoints = crushes;
			calcHit.OverallPoints = calcHit.SurvivalPoints = hits;

			comparisonGraph1.RoundValues = false;
			comparisonGraph1.ItemCalculations = new ItemBuffEnchantCalculation[] { calcMiss, calcDodge, calcCrit, calcCrush, calcHit };
			comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
		}
	}
}
