using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public class Calculations
    {
        //my insides all turned to ash / so slow
        //and blew away as i collapsed / so cold
		//public static float ValueOfArmor = 1.77f;
		private static CalculatedStats _cachedCharacterStatsWithSlotEmpty = null;
		private static Character.CharacterSlot _cachedSlot = Character.CharacterSlot.Shirt;
		private static Character _cachedCharacter = null;

		public static void ClearCache()
		{
			_cachedCharacter = null;
		}

		public static ItemCalculation GetItemCalculations(Item item, Character character, Character.CharacterSlot slot)
		{
			bool useCache = character == _cachedCharacter && slot == _cachedSlot;
			Character characterWithSlotEmpty = null;

			if (!useCache)
				characterWithSlotEmpty = character.Clone();
			Character characterWithNewItem = character.Clone();

			if (slot != Character.CharacterSlot.Metas && slot != Character.CharacterSlot.Gems)
			{
				if (!useCache) characterWithSlotEmpty[slot] = null;
				characterWithNewItem[slot] = item;
			}
			

			CalculatedStats characterStatsWithSlotEmpty;
			if (useCache)
				characterStatsWithSlotEmpty = _cachedCharacterStatsWithSlotEmpty;
			else
			{
				characterStatsWithSlotEmpty = GetCalculatedStatsFromBasicStats(characterWithSlotEmpty.Buffs, GetCharacterStats(characterWithSlotEmpty));
				_cachedCharacter = character;
				_cachedSlot = slot;
				_cachedCharacterStatsWithSlotEmpty = characterStatsWithSlotEmpty;
			}
			
			
			Item additionalItem = null;
			if (item.FitsInSlot(Character.CharacterSlot.Gems) || item.FitsInSlot(Character.CharacterSlot.Metas)) additionalItem = item;
			CalculatedStats characterStatsWithNewItem = GetCalculatedStatsFromBasicStats(characterWithNewItem.Buffs, GetCharacterStats(characterWithNewItem, additionalItem));

			ItemCalculation itemCalc = new ItemCalculation();
			itemCalc.Item = item;
			itemCalc.ItemName = item.Name;
			itemCalc.Equipped = character[slot] == item;
			itemCalc.OverallPoints = characterStatsWithNewItem.OverallPoints - characterStatsWithSlotEmpty.OverallPoints;
			itemCalc.MitigationPoints = characterStatsWithNewItem.MitigationPoints - characterStatsWithSlotEmpty.MitigationPoints;
			itemCalc.SurvivalPoints = characterStatsWithNewItem.SurvivalPoints - characterStatsWithSlotEmpty.SurvivalPoints;

			characterStatsWithNewItem.ToString();

			return itemCalc;
		}

		public static List<ItemCalculation> GetEnchantCalculations(Item.ItemSlot slot, Character character, CalculatedStats currentCalcs)
		{
			List<ItemCalculation> enchantCalcs = new List<ItemCalculation>();
			CalculatedStats calcsEquipped = null;
			CalculatedStats calcsUnequipped = null;
			foreach (Enchant enchant in Enchant.FindEnchants(slot))
			{
				//if (enchantCalcs.ContainsKey(enchant.Id)) continue;

				bool isEquipped = character.GetEnchantBySlot(enchant.Slot) == enchant;
				if (isEquipped)
				{
					calcsEquipped = currentCalcs;
					Character charUnequipped = character.Clone();
					charUnequipped.SetEnchantBySlot(enchant.Slot, null);
					calcsUnequipped = GetCalculatedStatsFromBasicStats(character.Buffs, GetCharacterStats(charUnequipped));
				}
				else
				{
					Character charUnequipped = character.Clone();
					Character charEquipped = character.Clone();
					charUnequipped.SetEnchantBySlot(enchant.Slot, null);
					charEquipped.SetEnchantBySlot(enchant.Slot, enchant);
					calcsUnequipped = GetCalculatedStatsFromBasicStats(character.Buffs, GetCharacterStats(charUnequipped));
					calcsEquipped = GetCalculatedStatsFromBasicStats(character.Buffs, GetCharacterStats(charEquipped));
				}
				ItemCalculation enchantCalc = new ItemCalculation();
				enchantCalc.ItemName = enchant.Name;
				enchantCalc.Item = new Item();
				enchantCalc.Item.Name = enchant.Name;
				enchantCalc.Item.Stats = enchant.Stats;
				enchantCalc.Equipped = isEquipped;
				enchantCalc.OverallPoints = calcsEquipped.OverallPoints - calcsUnequipped.OverallPoints;
				enchantCalc.MitigationPoints = calcsEquipped.MitigationPoints - calcsUnequipped.MitigationPoints;
				enchantCalc.SurvivalPoints = calcsEquipped.SurvivalPoints - calcsUnequipped.SurvivalPoints;
				enchantCalcs.Add(enchantCalc);
			}
			return enchantCalcs;
		}

		public static List<ItemCalculation> GetBuffCalculations(Character character, CalculatedStats currentCalcs, Buffs.BuffCatagory buffCategory)
		{
			List<ItemCalculation> buffCalcs = new List<ItemCalculation>();
			CalculatedStats calcsEquipped = null;
			CalculatedStats calcsUnequipped = null;
			string[] buffs = buffCategory == Buffs.BuffCatagory.AllLongDurationBuffsNoDW ? Buffs.AllLongDurationBuffsNoDW : Buffs.AllBuffs;
			foreach (string buff in buffs)
			{
				if (buffCategory != Buffs.BuffCatagory.CurrentBuffs || character.Buffs[buff])
				{
					Character charUnequipped = character.Clone();
					Character charEquipped = character.Clone();
					charUnequipped.Buffs[buff] = false;
					charUnequipped.Buffs[buff.Replace("Improved ", "")] = false;
					charUnequipped.Buffs["Improved " + buff] = false;
					charEquipped.Buffs[buff] = true;
					charEquipped.Buffs[buff.Replace("Improved ", "")] = true;
					charEquipped.Buffs["Improved " + buff] = false;
					calcsUnequipped = GetCalculatedStatsFromBasicStats(charUnequipped.Buffs, GetCharacterStats(charUnequipped));
					calcsEquipped = GetCalculatedStatsFromBasicStats(charEquipped.Buffs, GetCharacterStats(charEquipped));
					
					ItemCalculation buffCalc = new ItemCalculation();
					buffCalc.ItemName = buff;
					buffCalc.Equipped = character.Buffs[buff];
					buffCalc.OverallPoints = calcsEquipped.OverallPoints - calcsUnequipped.OverallPoints;
					buffCalc.MitigationPoints = calcsEquipped.MitigationPoints - calcsUnequipped.MitigationPoints;
					buffCalc.SurvivalPoints = calcsEquipped.SurvivalPoints - calcsUnequipped.SurvivalPoints;
					buffCalcs.Add(buffCalc);
				}
			}
			return buffCalcs;
		}

		public static Stats GetItemTotalStats(Item item)
		{
			Stats totalItemStats = new Stats();
			Item gem1 = item.Gem1;
			Item gem2 = item.Gem2;
			Item gem3 = item.Gem3;
			bool eligibleForSocketBonus = Item.GemMatchesSlot(gem1, item.Sockets.Color1) &&
				Item.GemMatchesSlot(gem2, item.Sockets.Color2) &&
				Item.GemMatchesSlot(gem3, item.Sockets.Color3);
			Stats[] itemStatses = new Stats[5];
			itemStatses[0] = item.Stats;
			if (gem1 != null) itemStatses[1] = gem1.Stats;
			if (gem2 != null) itemStatses[2] = gem2.Stats;
			if (gem3 != null) itemStatses[3] = gem3.Stats;
			if (eligibleForSocketBonus) itemStatses[4] = item.Sockets.Stats;

			foreach (Stats itemStats in itemStatses)
			{
				if (itemStats != null)
				{
					totalItemStats.Agility += itemStats.Agility;
					totalItemStats.Armor += itemStats.Armor;
					totalItemStats.DefenseRating += itemStats.DefenseRating;
					totalItemStats.DodgeRating += itemStats.DodgeRating;
					totalItemStats.Health += itemStats.Health;
					totalItemStats.Resilience += itemStats.Resilience;
					totalItemStats.Stamina += itemStats.Stamina;
				}
			}
			return totalItemStats;
		}

        public static CalculatedStats CalculateStats(Character character)
        {
			return GetCalculatedStatsFromBasicStats(character.Buffs, GetCharacterStats(character));
			//CharacterCalculations characterCalcs = new CharacterCalculations();
			//characterCalcs.CalculatedStats = GetCalculatedStatsFromBasicStats(character.Buffs, GetCharacterStats(character));
			//characterCalcs.EnchantCalculations = GetEnchantCalculations(character, characterCalcs.CalculatedStats);
			//characterCalcs.BuffCalculations = GetBuffCalculations(character, characterCalcs.CalculatedStats);

			//return characterCalcs;
        }

		private static CalculatedStats GetCalculatedStatsFromBasicStats(Buffs buffs, Stats stats)
		{
			CalculatedStats calculatedStats = new CalculatedStats();
			calculatedStats.BasicStats = stats;
			calculatedStats.Miss = 5 + stats.DefenseRating / 60f + (buffs.DualWieldingMob ? 20f : 0f) + (buffs.ScorpidSting ? 5f : 0f) + (buffs.InsectSwarm ? 2f : 0f) - 0.6f;
			calculatedStats.Dodge = Math.Min(100f - calculatedStats.Miss, stats.Agility / 14.7059f + stats.DodgeRating / 18.9231f + stats.DefenseRating / 59.134615f -0.6f);
			calculatedStats.Mitigation = (stats.Armor / (stats.Armor + 11959.5f)) * 100f;
			calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
			calculatedStats.DodgePlusMiss = calculatedStats.Dodge + calculatedStats.Miss;
			calculatedStats.CritReduction = stats.DefenseRating / 60f + stats.Resilience / 39.423f;
			calculatedStats.CappedCritReduction = Math.Min(2.6f, calculatedStats.CritReduction);
			//Out of 100 attacks, you'll take...
			float crits = 2.6f - calculatedStats.CappedCritReduction;
			float crushes = Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMiss)), 15f), 0f);
			float hits = Math.Max(100f - (crits + crushes + (calculatedStats.DodgePlusMiss)), 0f);
			//Apply armor and multipliers for each attack type...
			crits *= (100f - calculatedStats.CappedMitigation) * .02f;
			crushes *= (100f - calculatedStats.CappedMitigation) * .015f;
			hits *= (100f - calculatedStats.CappedMitigation) * .01f;
			calculatedStats.DamageTaken = hits + crushes + crits;//100f * (1f - (calculatedStats.DodgePlusMiss / 100f)) * (1f - (calculatedStats.CappedMitigation / 100f));
			calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;
			
			calculatedStats.SurvivalPoints = (stats.Health / (1f - (calculatedStats.CappedMitigation / 100f))) / (buffs.ShadowEmbrace ? 0.95f : 1f);

			calculatedStats.MitigationPoints = (7000f * (1f / (calculatedStats.DamageTaken / 100f))) / (buffs.ShadowEmbrace ? 0.95f : 1f);
			//calculatedStats.MitigationPoints = 5360f * ((1f - (calculatedStats.DamageTaken / 100f)) / (calculatedStats.DamageTaken / 100f));
			calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints;
			return calculatedStats;
		}

		public static Stats GetCharacterStats(Character character) { return GetCharacterStats(character, null); }
		public static Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = character.Race == Character.CharacterRace.NightElf ? new Stats(0, 3434, 75, 82, 59, 0, 0) : new Stats(0, 3434, 64, 85, 40, 0, 0);
			Stats statsBaseGear = new Stats();
			List<Item> items = new List<Item>(new Item[] {character.Back, character.Chest, character.Feet, character.Finger1,
                character.Finger2, character.Hands, character.Head, character.Idol, character.Legs, character.Neck,
                character.Shirt, character.Shoulders, character.Tabard, character.Trinket1, character.Trinket2,
                character.Waist, character.Weapon, character.Wrist});
			if (additionalItem != null) items.Add(additionalItem);
			foreach (Item item in items)
			{
				if (item != null)
				{
					Stats itemStats = GetItemTotalStats(item);
					statsBaseGear.Agility += itemStats.Agility;
					statsBaseGear.Armor += itemStats.Armor;
					statsBaseGear.DefenseRating += itemStats.DefenseRating;
					statsBaseGear.DodgeRating += itemStats.DodgeRating;
					statsBaseGear.Health += itemStats.Health;
					statsBaseGear.Resilience += itemStats.Resilience;
					statsBaseGear.Stamina += itemStats.Stamina;
				}
			}
            Stats statsEnchants = GetEnchantsStats(character);
            statsBaseGear.Agility += statsEnchants.Agility;
            statsBaseGear.DefenseRating += statsEnchants.DefenseRating;
            statsBaseGear.DodgeRating += statsEnchants.DodgeRating;
            statsBaseGear.Resilience += statsEnchants.Resilience;
            statsBaseGear.Stamina += statsEnchants.Stamina;

            Stats statsBuffs = GetBuffsStats(character.Buffs);
            statsBuffs.Health += statsEnchants.Health;
            statsBuffs.Armor += statsEnchants.Armor;

			float agiBase = (float)Math.Floor(statsRace.Agility * 1.03f);
			float agiBonus = (float)Math.Floor((statsBaseGear.Agility + statsBuffs.Agility) * 1.03f);
			float staBase = (float)Math.Floor(statsRace.Stamina * 1.03f * 1.25f);
			float staBonus = (statsBaseGear.Stamina + statsBuffs.Stamina) * 1.03f * 1.25f;
			float staHotW = (statsRace.Stamina * 1.03f * 1.25f + staBonus) * 0.2f;
			staBonus = (float)Math.Round(Math.Floor(staBonus) + staHotW);

            Stats statsTotal = new Stats();
			statsTotal.Agility = agiBase + (character.Buffs.BlessingOfKings ? (float)Math.Floor((agiBase * 0.1f) + agiBonus * 1.1f) : agiBonus);
			statsTotal.Stamina = staBase + (character.Buffs.BlessingOfKings ? (float)Math.Round((staBase * 0.1f) + staBonus * 1.1f) : staBonus);
			//statsTotal.Agility = (float)Math.Round((statsBaseGear.Agility + statsBuffs.Agility) * (character.Buffs.BlessingOfKings ? 1.133f : 1.03f));
            //statsTotal.Stamina = (float)Math.Round((((statsBaseGear.Stamina + statsBuffs.Stamina) * (character.Buffs.BlessingOfKings ? 1.133f : 1.03f)) * 1.5f) - 1f);
			statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
			statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
			statsTotal.Resilience = statsRace.Resilience + statsBaseGear.Resilience + statsBuffs.Resilience;
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsBaseGear.Health + statsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
			statsTotal.Armor = (float)Math.Round(((statsBaseGear.Armor * 5.5f) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility * 2f)) * (character.Buffs.AncestralFortitude ? 1.25f : 1f) * (character.Buffs.ImprovedLayOnHands ? 1.3f : 1f));

            return statsTotal;
        }

        public static Stats GetEnchantsStats(Character character)
        {
            Stats statsTotal = new Stats();
			Stats[] enchantStatses = new Stats[] {character.BackEnchant.Stats, character.ChestEnchant.Stats, character.FeetEnchant.Stats, character.Finger1Enchant.Stats, character.Finger2Enchant.Stats, character.HandsEnchant.Stats, 
				character.HeadEnchant.Stats, character.LegsEnchant.Stats, character.ShouldersEnchant.Stats, character.WeaponEnchant.Stats, character.WristEnchant.Stats};
            foreach (Stats enchantStats in enchantStatses)
            {
                statsTotal.Agility += enchantStats.Agility;
                statsTotal.Armor += enchantStats.Armor;
                statsTotal.DefenseRating += enchantStats.DefenseRating;
                statsTotal.DodgeRating += enchantStats.DodgeRating;
                statsTotal.Health += enchantStats.Health;
                statsTotal.Resilience += enchantStats.Resilience;
                statsTotal.Stamina += enchantStats.Stamina;
            }
            return statsTotal;
        }

        public static Stats GetBuffsStats(Buffs buffs)
        {
            Stats statsTotal = new Stats();
            statsTotal.Health =
                (buffs.CommandingShout ?            1080f : 0) +
                (buffs.ImprovedCommandingShout ?    (float)Math.Floor(1080f * 0.25f) : 0) +
                (buffs.HeroicPotion ?				700f : 0) +
                (buffs.NightmareSeed ?				2000f : 0) +
                (buffs.Heroic1750HealthTrinket ?			1750f : 0) +
                (buffs.ArgussianCompass ?			1150f : 0) +
                (buffs.BroochOfTheImmortalKing ?    1250f : 0) +
                (buffs.ElixirOfMajorFortitude ?     250f : 0) +
                (buffs.FlaskOfFortification ?       500f : 0);
            statsTotal.Armor =
                (buffs.MarkOfTheWild ?              340f : 0) +
                (buffs.ImprovedMarkOfTheWild ?      (float)Math.Floor(340f * 0.35f) : 0) +
                (buffs.ElixirOfMajorDefense ?		550f : 0) +
                (buffs.ScrollOfProtection	?		300f : 0) +
                (buffs.Malorne4PieceBonus ?			1400f : 0) +
                (buffs.IronshieldPotion ?			2500f : 0) +
                (buffs.LivingRootOfTheWildheart ?	4070f : 0) +
                (buffs.AdamantiteFigurine ?			1280f : 0) +
                (buffs.DevotionAura ?				861f : 0) +
                (buffs.ImprovedDevotionAura ?		(float)Math.Floor(861f * 0.4f) : 0);
            statsTotal.Agility =
                (buffs.MarkOfTheWild ?				14f : 0) +
				(buffs.ImprovedMarkOfTheWild ?		(float)Math.Floor(14f * 0.35f) : 0) +
                (buffs.AgilityFood ?				20f : 0) +
                (buffs.GraceOfAirTotem ?			77f : 0) +
                (buffs.ImprovedGraceOfAirTotem ?	(float)Math.Floor(77f * 0.15f) : 0) +
                (buffs.ElixirOfMajorAgility ?		35f : 0) + 
                (buffs.FlaskOfChromaticWonder ?		18f : 0) + 
                (buffs.ElixirOfMastery ?			15f : 0) + 
                (buffs.BadgeOfTenacity ?			150f : 0) + 
                (buffs.IdolOfTerror ?				65f : 0) + 
                (buffs.ScrollOfAgility ?			20f : 0);
            statsTotal.Stamina =
                (buffs.PowerWordFortitude ?			79f : 0) +
                (buffs.ImprovedPowerWordFortitude ?	(float)Math.Floor(79f * 0.3f) : 0) +
                (buffs.MarkOfTheWild ?				14f : 0) +
                (buffs.ImprovedMarkOfTheWild ?		(float)Math.Floor(14f * 0.35f) : 0) +
                (buffs.BloodPact ?					66f : 0) +
                (buffs.ImprovedBloodPact ?			(float)Math.Floor(66f * 0.3f) : 0) +
                (buffs.ElixirOfMastery ?			15f : 0) + 
                (buffs.FlaskOfChromaticWonder ?		18f : 0) + 
                (buffs.StaminaFood ?				30f : 0);
            statsTotal.DodgeRating =
                (buffs.MoongladeRejuvination ?		35f : 0) + 
                (buffs.DawnstoneCrab ?				125f : 0) + 
                (buffs.MoroesLuckyPocketWatch ?		300f : 0);
			statsTotal.DefenseRating =
                (buffs.FlaskOfFortification ?		10f : 0);
			statsTotal.Resilience =
				(buffs.GladiatorResilience ?		35f : 0) +
                (buffs.Season3ResilienceRelic ?				31f : 0) +
                (buffs.ElixirOfIronskin ?			30f : 0);
            return statsTotal;
        }
    }

	//public class CharacterCalculations
	//{
	//    public CalculatedStats CalculatedStats;
	//    public Dictionary<int, ItemCalculation> EnchantCalculations;
	//    public Dictionary<string, ItemCalculation> BuffCalculations;

	//    public CharacterCalculations() { }
	//}

    public class CalculatedStats
    {
        public Stats BasicStats;
        public float Dodge;
        public float Miss;
		public float Mitigation;
		public float CappedMitigation;
		public float DodgePlusMiss;
        public float TotalMitigation;
        public float DamageTaken;
		public float CritReduction;
		public float CappedCritReduction;
		public float OverallPoints;
        public float MitigationPoints;
        public float SurvivalPoints;

        public CalculatedStats() { }
    }

	public class ItemCalculation
	{
		public string ItemName = string.Empty;
		public float OverallPoints = 0f;
		public float MitigationPoints = 0f;
		public float SurvivalPoints = 0f;
		public Item Item = null;
		public bool Equipped = false;

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}M {3}S)", ItemName, Math.Round(OverallPoints), Math.Round(MitigationPoints), Math.Round(SurvivalPoints));
		}
	}
}
