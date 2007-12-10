using System;
using System.Collections.Generic;

namespace Rawr
{
    public class Calculations
    {
        //my insides all turned to ash / so slow
        //and blew away as i collapsed / so cold
        //public static float ValueOfArmor = 1.77f;
        private static CharacterCalculation _cachedCharacterStatsWithSlotEmpty = null;
        private static Character.CharacterSlot _cachedSlot = Character.CharacterSlot.Shirt;
        private static Character _cachedCharacter = null;

        public static void ClearCache()
        {
            _cachedCharacter = null;
        }

        public static ItemBuffEnchantCalculation GetItemCalculations(Item item, Character character,
                                                                     Character.CharacterSlot slot)
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


            CharacterCalculation characterStatsWithSlotEmpty;
            if (useCache)
                characterStatsWithSlotEmpty = _cachedCharacterStatsWithSlotEmpty;
            else
            {
                characterStatsWithSlotEmpty =
                    GetCharacterCalculationsFromBasicStats(GetCharacterStats(characterWithSlotEmpty));
                _cachedCharacter = character;
                _cachedSlot = slot;
                _cachedCharacterStatsWithSlotEmpty = characterStatsWithSlotEmpty;
            }


            Item additionalItem = null;
            if (item.FitsInSlot(Character.CharacterSlot.Gems) || item.FitsInSlot(Character.CharacterSlot.Metas))
                additionalItem = item;
            CharacterCalculation characterStatsWithNewItem =
                GetCharacterCalculationsFromBasicStats(GetCharacterStats(characterWithNewItem, additionalItem));

            ItemBuffEnchantCalculation itemCalc = new ItemBuffEnchantCalculation();
            itemCalc.Item = item;
            itemCalc.Name = item.Name;
            itemCalc.Equipped = character[slot] == item;
            itemCalc.OverallPoints = characterStatsWithNewItem.OverallPoints - characterStatsWithSlotEmpty.OverallPoints;
            itemCalc.MitigationPoints = characterStatsWithNewItem.MitigationPoints -
                                        characterStatsWithSlotEmpty.MitigationPoints;
            itemCalc.SurvivalPoints = characterStatsWithNewItem.SurvivalPoints -
                                      characterStatsWithSlotEmpty.SurvivalPoints;

            characterStatsWithNewItem.ToString();

            return itemCalc;
        }

        public static List<ItemBuffEnchantCalculation> GetEnchantCalculations(Item.ItemSlot slot, Character character,
                                                                              CharacterCalculation currentCalcs)
        {
            List<ItemBuffEnchantCalculation> enchantCalcs = new List<ItemBuffEnchantCalculation>();
            CharacterCalculation calcsEquipped;
            CharacterCalculation calcsUnequipped;
            foreach (Enchant enchant in Enchant.FindEnchants(slot))
            {
                //if (enchantCalcs.ContainsKey(enchant.Id)) continue;

                bool isEquipped = character.GetEnchantBySlot(enchant.Slot) == enchant;
                if (isEquipped)
                {
                    calcsEquipped = currentCalcs;
                    Character charUnequipped = character.Clone();
                    charUnequipped.SetEnchantBySlot(enchant.Slot, null);
                    calcsUnequipped = GetCharacterCalculationsFromBasicStats(GetCharacterStats(charUnequipped));
                }
                else
                {
                    Character charUnequipped = character.Clone();
                    Character charEquipped = character.Clone();
                    charUnequipped.SetEnchantBySlot(enchant.Slot, null);
                    charEquipped.SetEnchantBySlot(enchant.Slot, enchant);
                    calcsUnequipped = GetCharacterCalculationsFromBasicStats(GetCharacterStats(charUnequipped));
                    calcsEquipped = GetCharacterCalculationsFromBasicStats(GetCharacterStats(charEquipped));
                }
                ItemBuffEnchantCalculation enchantCalc = new ItemBuffEnchantCalculation();
                enchantCalc.Name = enchant.Name;
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

        public static List<ItemBuffEnchantCalculation> GetBuffCalculations(Character character,
                                                                           CharacterCalculation currentCalcs,
                                                                           Buff.BuffType buffType, bool activeOnly)
        {
            List<ItemBuffEnchantCalculation> buffCalcs = new List<ItemBuffEnchantCalculation>();
            CharacterCalculation calcsEquipped;
            CharacterCalculation calcsUnequipped;
            foreach (Buff buff in Buff.GetBuffsByType(buffType))
            {
                if (!activeOnly || character.ActiveBuffs.Contains(buff.Name))
                {
                    Character charUnequipped = character.Clone();
                    Character charEquipped = character.Clone();
                    if (charUnequipped.ActiveBuffs.Contains(buff.Name))
                        charUnequipped.ActiveBuffs.Remove(buff.Name);
                    if (string.IsNullOrEmpty(buff.RequiredBuff))
                    {
                        if (charUnequipped.ActiveBuffs.Contains("Improved " + buff.Name))
                            charUnequipped.ActiveBuffs.Remove("Improved " + buff.Name);
                    }
                    else if (charUnequipped.ActiveBuffs.Contains(buff.RequiredBuff))
                        charUnequipped.ActiveBuffs.Remove(buff.RequiredBuff);

                    if (!charEquipped.ActiveBuffs.Contains(buff.Name))
                        charEquipped.ActiveBuffs.Add(buff.Name);
                    if (string.IsNullOrEmpty(buff.RequiredBuff))
                    {
                        if (charEquipped.ActiveBuffs.Contains("Improved " + buff.Name))
                            charEquipped.ActiveBuffs.Remove("Improved " + buff.Name);
                    }
                    else if (!charEquipped.ActiveBuffs.Contains(buff.RequiredBuff))
                        charEquipped.ActiveBuffs.Add(buff.RequiredBuff);

                    calcsUnequipped = GetCharacterCalculationsFromBasicStats(GetCharacterStats(charUnequipped));
                    calcsEquipped = GetCharacterCalculationsFromBasicStats(GetCharacterStats(charEquipped));

                    ItemBuffEnchantCalculation buffCalc = new ItemBuffEnchantCalculation();
                    buffCalc.Name = buff.Name;
                    buffCalc.Equipped = character.ActiveBuffs.Contains(buff.Name);
                    buffCalc.OverallPoints = calcsEquipped.OverallPoints - calcsUnequipped.OverallPoints;
                    buffCalc.MitigationPoints = calcsEquipped.MitigationPoints - calcsUnequipped.MitigationPoints;
                    buffCalc.SurvivalPoints = calcsEquipped.SurvivalPoints - calcsUnequipped.SurvivalPoints;
                    buffCalcs.Add(buffCalc);
                }
            }
            return buffCalcs;
        }

        public static CharacterCalculation GetCharacterCalculations(Character character)
        {
            return GetCharacterCalculationsFromBasicStats(GetCharacterStats(character));
        }

        private static CharacterCalculation GetCharacterCalculationsFromBasicStats(Stats stats)
        {
            CharacterCalculation calculatedStats = new CharacterCalculation();
            calculatedStats.BasicStats = stats;
            calculatedStats.Miss = 5 + stats.DefenseRating/60f + stats.Miss - 0.6f;
            calculatedStats.Dodge =
                Math.Min(100f - calculatedStats.Miss,
                         stats.Agility/14.7059f + stats.DodgeRating/18.9231f + stats.DefenseRating/59.134615f - 0.6f);
            calculatedStats.Mitigation = (stats.Armor/(stats.Armor + 11959.5f))*100f;
            calculatedStats.CappedMitigation = Math.Min(75f, calculatedStats.Mitigation);
            calculatedStats.DodgePlusMiss = calculatedStats.Dodge + calculatedStats.Miss;
            calculatedStats.CritReduction = stats.DefenseRating/60f + stats.Resilience/39.423f;
            calculatedStats.CappedCritReduction = Math.Min(2.6f, calculatedStats.CritReduction);
            //Out of 100 attacks, you'll take...
            float crits = 2.6f - calculatedStats.CappedCritReduction;
            float crushes = Math.Max(Math.Min(100f - (crits + (calculatedStats.DodgePlusMiss)), 15f), 0f);
            float hits = Math.Max(100f - (crits + crushes + (calculatedStats.DodgePlusMiss)), 0f);
            //Apply armor and multipliers for each attack type...
            crits *= (100f - calculatedStats.CappedMitigation)*.02f;
            crushes *= (100f - calculatedStats.CappedMitigation)*.015f;
            hits *= (100f - calculatedStats.CappedMitigation)*.01f;
            calculatedStats.DamageTaken = hits + crushes + crits;
            calculatedStats.TotalMitigation = 100f - calculatedStats.DamageTaken;

            calculatedStats.SurvivalPoints = (stats.Health/(1f - (calculatedStats.CappedMitigation/100f)));
            // / (buffs.ShadowEmbrace ? 0.95f : 1f);
            calculatedStats.MitigationPoints = (7000f*(1f/(calculatedStats.DamageTaken/100f)));
            // / (buffs.ShadowEmbrace ? 0.95f : 1f);
            calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints;
            return calculatedStats;
        }

        public static Stats GetCharacterStats(Character character)
        {
            return GetCharacterStats(character, null);
        }

        public static Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = character.Race == Character.CharacterRace.NightElf
                                  ? new Stats(0, 3434, 75, 82, 59, 0, 0)
                                  : new Stats(0, 3434, 64, 85, 40, 0, 0);
            Stats statsBaseGear = new Stats();
            List<Item> items = new List<Item>(new Item[]
                                                  {
                                                      character.Back, character.Chest, character.Feet, character.Finger1
                                                      ,
                                                      character.Finger2, character.Hands, character.Head, character.Idol
                                                      , character.Legs, character.Neck,
                                                      character.Shirt, character.Shoulders, character.Tabard,
                                                      character.Trinket1, character.Trinket2,
                                                      character.Waist, character.Weapon, character.Wrist
                                                  });
            if (additionalItem != null) items.Add(additionalItem);

            foreach (Item item in items)
                if (item != null)
                    statsBaseGear += item.GetTotalStats();

            Stats statsEnchants = GetEnchantsStats(character);
            statsBaseGear.Agility += statsEnchants.Agility;
            statsBaseGear.DefenseRating += statsEnchants.DefenseRating;
            statsBaseGear.DodgeRating += statsEnchants.DodgeRating;
            statsBaseGear.Resilience += statsEnchants.Resilience;
            statsBaseGear.Stamina += statsEnchants.Stamina;

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            statsBuffs.Health += statsEnchants.Health;
            statsBuffs.Armor += statsEnchants.Armor;

            float agiBase = (float) Math.Floor(statsRace.Agility*1.03f);
            float agiBonus = (float) Math.Floor((statsBaseGear.Agility + statsBuffs.Agility)*1.03f);
            float staBase = (float) Math.Floor(statsRace.Stamina*1.03f*1.25f);
            float staBonus = (statsBaseGear.Stamina + statsBuffs.Stamina)*1.03f*1.25f;
            float staHotW = (statsRace.Stamina*1.03f*1.25f + staBonus)*0.2f;
            staBonus = (float) Math.Round(Math.Floor(staBonus) + staHotW);

            Stats statsTotal = new Stats();
            statsTotal.Agility = agiBase +
                                 (float)
                                 Math.Floor((agiBase*statsBuffs.BonusAgilityMultiplier) +
                                            agiBonus*(1 + statsBuffs.BonusAgilityMultiplier));
            statsTotal.Stamina = staBase +
                                 (float)
                                 Math.Round((staBase*statsBuffs.BonusStaminaMultiplier) +
                                            staBonus*(1 + statsBuffs.BonusStaminaMultiplier));
            statsTotal.DefenseRating = statsRace.DefenseRating + statsBaseGear.DefenseRating + statsBuffs.DefenseRating;
            statsTotal.DodgeRating = statsRace.DodgeRating + statsBaseGear.DodgeRating + statsBuffs.DodgeRating;
            statsTotal.Resilience = statsRace.Resilience + statsBaseGear.Resilience + statsBuffs.Resilience;
            statsTotal.Health =
                (float)
                Math.Round(((statsRace.Health + statsBaseGear.Health + statsBuffs.Health + (statsTotal.Stamina*10f))*
                            (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Armor =
                (float)
                Math.Round(((statsBaseGear.Armor*5.5f) + statsRace.Armor + statsBuffs.Armor + (statsTotal.Agility*2f))*
                           (1 + statsBuffs.BonusArmorMultiplier));
            statsTotal.Miss = statsBuffs.Miss;

            return statsTotal;
        }

        public static Stats GetEnchantsStats(Character character)
        {
            Stats statsTotal = new Stats();
            Stats[] enchantStatses = new Stats[]
                {
                    character.BackEnchant.Stats, character.ChestEnchant.Stats, character.FeetEnchant.Stats,
                    character.Finger1Enchant.Stats, character.Finger2Enchant.Stats, character.HandsEnchant.Stats,
                    character.HeadEnchant.Stats, character.LegsEnchant.Stats, character.ShouldersEnchant.Stats,
                    character.WeaponEnchant.Stats, character.WristEnchant.Stats
                };
            foreach (Stats enchantStats in enchantStatses)
                statsTotal += enchantStats;

            return statsTotal;
        }

        public static Stats GetBuffsStats(List<string> buffs)
        {
            Stats totalStats = new Stats();
            foreach (string buffName in buffs)
                totalStats += Buff.GetBuffByName(buffName).Stats;

            return totalStats;
        }
    }

    public class CharacterCalculation
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
    }

    public class ItemBuffEnchantCalculation
    {
        public string Name = string.Empty;
        public float OverallPoints = 0f;
        public float MitigationPoints = 0f;
        public float SurvivalPoints = 0f;
        public Item Item = null;
        public bool Equipped = false;

        public override string ToString()
        {
            return
                string.Format("{0}: ({1}O {2}M {3}S)", Name, Math.Round(OverallPoints), Math.Round(MitigationPoints),
                              Math.Round(SurvivalPoints));
        }
    }
}