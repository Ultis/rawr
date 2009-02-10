using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    [Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_CrusaderStrike", Character.CharacterClass.Paladin)]
	class CalculationsRetribution : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
            }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    List<string> labels = new List<string>(new string[]
                    {
                        "Basic Stats:Health",
					    "Basic Stats:Strength",
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Chance",
					    "Basic Stats:Miss Chance",
					    "Basic Stats:Dodge Chance",
					    "Basic Stats:Melee Haste",
					    "Basic Stats:Weapon Damage",
					    "Basic Stats:Attack Speed",
                        "DPS Breakdown:White",
                        "DPS Breakdown:Seal",
					    "DPS Breakdown:Crusader Strike",
                        "DPS Breakdown:Judgement",
                        "DPS Breakdown:Consecration",
                        "DPS Breakdown:Exorcism",
                        "DPS Breakdown:Divine Storm",
                        "DPS Breakdown:Hammer of Wrath",
                        "DPS Breakdown:Total DPS"
                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelRetribution()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Libram,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword
					}));
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationRetribution();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRetribution();
        }


		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRetribution));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsRetribution calcOpts = serializer.Deserialize(reader) as CalculationOptionsRetribution;
			return calcOpts;
		}


        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            PaladinTalents talents = character.PaladinTalents;
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRetribution calc = new CharacterCalculationsRetribution();
            calc.BasicStats = stats;

            //damage multipliers
            float twoHandedSpec = 1f + 0.02f * talents.TwoHandedWeaponSpecialization;
            float crusade = (1f + (calcOpts.MobType < 3 ? .02f : .01f) * talents.Crusade)
                * (calcOpts.GlyphSenseUndead && calcOpts.MobType == 0 ? 1.01f : 1f);
            float vengeance = 1f + 0.03f * talents.Vengeance;
            float talentMulti = twoHandedSpec * crusade * vengeance;
            float critBonus = 2f * (1f + stats.BonusCritMultiplier);
            float spellCritBonus = 1.5f * (1f + stats.BonusSpellCritMultiplier);
            float aow = 1f + .05f * talents.TheArtOfWar;
            float rightVen = .08f * talents.RighteousVengeance;

            float spellPowerMulti = (1f + stats.BonusHolyDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            float physPowerMulti = (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            const float partialResist = 0.953f; // Average of 4.7% damage lost to partial resists on spells

            calc.ToMiss = (float)Math.Max(0.08f - stats.PhysicalHit, 0f);
            calc.ToDodge = (float)Math.Max(0.065f - stats.Expertise * .0025f, 0f);
            calc.ToResist = (float)Math.Max(0.17f - stats.SpellHit, 0f);

            #region Mitigation
            float targetArmor = (13083 - stats.ArmorPenetration) * (1f - stats.ArmorPenetrationRating / 1539.529991f);
            //TODO: Check this out, make sure its right
            float armorReduction = 1f - targetArmor / ((467.5f * character.Level) + targetArmor - 22167.5f);
            #endregion

            #region Weapon Damage
            float baseSpeed = character.MainHand == null ? 3.5f : character.MainHand.Speed;
            float baseWeaponDamage = character.MainHand == null ? 371.5f : (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f;
            calc.AttackSpeed = baseSpeed / ( 1f + stats.PhysicalHaste );
            calc.WeaponDamage = baseWeaponDamage + stats.AttackPower * baseSpeed / 14f;
            float normalizedWeaponDamage = calc.WeaponDamage * 3.3f / baseSpeed;

            float whiteDamage = calc.WeaponDamage * talentMulti * physPowerMulti * armorReduction;

            const float glancingAmount = 1f - 0.35f;
            const float glanceChance = .24f;

            float avgWhiteHit = whiteDamage * (glanceChance * glancingAmount + stats.PhysicalCrit * critBonus + (1f - stats.PhysicalCrit - glanceChance - calc.ToMiss - calc.ToDodge));
            calc.WhiteDPS = avgWhiteHit / calc.AttackSpeed;
            float sealProcs = 1f / calc.AttackSpeed * (1f - calc.ToDodge - calc.ToMiss);
            float sealProcs20 = 1f / calc.AttackSpeed * (1f - calc.ToDodge - calc.ToMiss);
            #endregion

            #region Crusader Strike
            if ( talents.CrusaderStrike > 0 )
            {
                float csDamage = normalizedWeaponDamage * 1.1f * talentMulti * physPowerMulti * armorReduction * aow;
                float csAvgHit = csDamage * (1f + stats.PhysicalCrit * critBonus - stats.PhysicalCrit - calc.ToMiss - calc.ToDodge);
                calc.CrusaderStrikeDPS = csAvgHit / calcOpts.CrusaderStrikeCD;
                sealProcs += 1f / calcOpts.CrusaderStrikeCD * (1f - calc.ToDodge - calc.ToMiss);
                calc.CrusaderStrikeDPS20 = csAvgHit / calcOpts.CrusaderStrikeCD20;
                sealProcs20 += 1f / calcOpts.CrusaderStrikeCD20 * (1f - calc.ToDodge - calc.ToMiss);
            }
            #endregion

            #region Divine Storm
            if ( talents.DivineStorm > 0 )
            {
                float dsDamage = normalizedWeaponDamage * talentMulti * physPowerMulti * armorReduction * aow * (1f + stats.DivineStormMultiplier);
                float dsAvgHit = dsDamage * (1f + stats.PhysicalCrit * critBonus - stats.PhysicalCrit - calc.ToMiss - calc.ToDodge);
                float dsRightVen = dsDamage * critBonus * rightVen * spellPowerMulti * talentMulti * partialResist * stats.PhysicalCrit;
                calc.DivineStormDPS = (dsAvgHit + dsRightVen) / calcOpts.DivineStormCD;
                sealProcs += 1f / calcOpts.DivineStormCD * (1f - calc.ToDodge - calc.ToMiss);
                calc.DivineStormDPS20 = (dsAvgHit + dsRightVen) / calcOpts.DivineStormCD20;
                sealProcs20 += 1f / calcOpts.DivineStormCD20 * (1f - calc.ToDodge - calc.ToMiss);
            }
            #endregion
            
            #region Seal
            float sealDamage = calc.WeaponDamage * .27f * spellPowerMulti * talentMulti * partialResist;
            float sealAvgHit = sealDamage * (1f + stats.PhysicalCrit * critBonus - stats.PhysicalCrit - calc.ToMiss - calc.ToDodge);
            calc.SealDPS = sealAvgHit * sealProcs * (1f - calc.ToMiss - calc.ToDodge);
            calc.SealDPS20 = sealAvgHit * sealProcs20 * (1f - calc.ToMiss - calc.ToDodge);
            #endregion

            #region Judgement
            float judgeCrit = stats.PhysicalCrit + .05f * talents.Fanaticism;
            float judgeDamage = (calc.WeaponDamage * .36f + .25f * stats.SpellPower + .16f * stats.AttackPower)
                * spellPowerMulti * talentMulti * partialResist * aow * (calcOpts.GlyphJudgement ? 1.1f : 1f);
            float judgeAvgHit = judgeDamage * (1f + judgeCrit * critBonus - judgeCrit - calc.ToMiss);
            float judgeRightVen = judgeDamage * critBonus * rightVen * spellPowerMulti * talentMulti * partialResist * judgeCrit;
            calc.JudgementDPS = (judgeAvgHit + judgeRightVen) / calcOpts.JudgementCD;
            calc.JudgementDPS20 = (judgeAvgHit + judgeRightVen) / calcOpts.JudgementCD20;
            #endregion

            #region Consecration
            float consDamage = (72f + .04f * stats.SpellPower + .04f * stats.AttackPower) * vengeance * crusade * spellPowerMulti * partialResist;
            calc.ConsecrationDPS = consDamage * (calcOpts.GlyphConsecration ? 10f : 8f) / calcOpts.ConescrationCD;
            calc.ConsecrationDPS20 = consDamage * (calcOpts.GlyphConsecration ? 10f : 8f) / calcOpts.ConescrationCD20;
            #endregion

            #region Exorcism
            if (calcOpts.MobType < 2 || calcOpts.Mode31)
            {
                float exoCrit = (calcOpts.Mode31 && calcOpts.MobType < 2 ? 1f : stats.SpellCrit);
                float exoDamage = (1087f + .42f * stats.SpellPower) * vengeance * crusade * spellPowerMulti * partialResist;
                float exoAvgHit = exoDamage * (1f + exoCrit * spellCritBonus - exoCrit - calc.ToResist);
                calc.ExorcismDPS = exoAvgHit / calcOpts.ExorcismCD;
                calc.ExorcismDPS20 = exoAvgHit / calcOpts.ExorcismCD20;
            }
            #endregion
            float howCrit = stats.PhysicalCrit + .25f * talents.SanctifiedRetribution;
            float howDamage = (1198f + .15f * stats.SpellPower + .15f * stats.AttackPower) * spellPowerMulti * talentMulti * partialResist;
            float howAvgHit = howDamage * (1f + howCrit * critBonus - howCrit - calc.ToMiss);
            calc.HammerOfWrathDPS20 = howAvgHit / calcOpts.HammerOfWrathCD20;
            #region HammerOfWrath

            #endregion

            calc.OverallPoints = calc.DPSPoints = calc.WhiteDPS + (calc.CrusaderStrikeDPS + calc.DivineStormDPS + calc.JudgementDPS
                + calc.SealDPS + calc.ConsecrationDPS + calc.ExorcismDPS) * (1f - calcOpts.TimeUnder20) +
                (calc.CrusaderStrikeDPS20 + calc.DivineStormDPS20 + calc.JudgementDPS20 + calc.HammerOfWrathDPS20
                + calc.SealDPS20 + calc.ConsecrationDPS20 + calc.ExorcismDPS20) * calcOpts.TimeUnder20;

            return calc;
        }

        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            PaladinTalents talents = character.PaladinTalents;
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats() { Strength = 19f, Agility = 22f, Stamina = 20f, Intellect = 24f, Spirit = 20f };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Strength = 23f, Agility = 17f, Stamina = 21f, Intellect = 21f, Spirit = 20f, PhysicalHit = .01f, SpellHit = .01f };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats() { Strength = 22f, Agility = 20f, Stamina = 22f, Intellect = 20f, Spirit = 22f, BonusSpiritMultiplier = 0.1f, };
                    //Expertise for Humans
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.OneHandMace || character.MainHand.Type == Item.ItemType.OneHandSword))
                        statsRace.Expertise = 3f;
                    break;
                default: //defaults to Dwarf stats
                    statsRace = new Stats() { Strength = 24f, Agility = 16f, Stamina = 25f, Intellect = 19f, Spirit = 20f, };
                    if (character.MainHand != null && character.MainHand.Type == Item.ItemType.OneHandMace)
                        statsRace.Expertise = 5f;
                    break;
            }
            statsRace.Strength += 129f;
            statsRace.Agility += 70f;
            statsRace.Stamina += 122f;
            statsRace.Intellect += 78f;
            statsRace.Spirit += 82f;
            statsRace.Dodge = .032685f;
            statsRace.Parry = .05f;
            statsRace.AttackPower = 258f;
            statsRace.Health = 6754f;
            statsRace.Mana = 4114;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats stats = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            Stats statsOther = statsBaseGear + statsEnchants + statsBuffs;
            stats.Strength = (float)Math.Floor(statsOther.Strength * (1 + stats.BonusStrengthMultiplier)) * (1f + talents.DivineStrength * .03f) + (float)Math.Floor(statsRace.Strength * (1 + stats.BonusStrengthMultiplier)) * (1f + talents.DivineStrength * .03f);
            stats.AttackPower = (float)Math.Floor((stats.AttackPower + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier));
            stats.Agility = (float)Math.Floor(statsOther.Agility * (1 + stats.BonusAgilityMultiplier)) + (float)Math.Floor(statsRace.Agility * (1 + stats.BonusAgilityMultiplier));
            stats.Stamina = (float)Math.Floor(statsOther.Stamina * (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .04f) * (1f + talents.CombatExpertise * .02f))
                + (float)Math.Floor(statsRace.Stamina * (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .04f) * (1f + talents.CombatExpertise * .02f));
            stats.Health = (float)Math.Round(stats.Health + stats.Stamina * 10);

            stats.PhysicalHit += character.StatConversion.GetHitFromRating(stats.HitRating) * .01f;
            stats.SpellHit += character.StatConversion.GetSpellHitFromRating(stats.HitRating) * .01f;
            stats.Expertise += talents.CombatExpertise * 2 + character.StatConversion.GetExpertiseFromRating(stats.ExpertiseRating);
            // Haste trinket (Meteorite Whetstone)
            stats.HasteRating += stats.HasteRatingOnPhysicalAttack * 10 / 45;

            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctifiedSeals * .01f;
            stats.PhysicalCrit = stats.PhysicalCrit + character.StatConversion.GetCritFromRating(stats.CritRating) * .01f +
                character.StatConversion.GetCritFromAgility(stats.Agility) * .01f + talentCrit;
            stats.SpellCrit = stats.SpellCrit + character.StatConversion.GetSpellCritFromRating(stats.CritRating) * .01f
                + character.StatConversion.GetSpellCritFromIntellect(stats.Intellect) * .01f + talentCrit;

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + character.StatConversion.GetHasteFromRating(stats.HasteRating) * .01f) - 1f;

            stats.SpellPower += (float)Math.Floor(stats.Stamina * .1f * talents.TouchedByTheLight + stats.AttackPower * talents.SheathOfLight * .1f);

            return stats;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            //List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            //CharacterCalculationsRetribution baseCalc, calc;
            //ComparisonCalculationBase comparison;
            //float[] subPoints;

            //switch (chartName)
            //{
            //    case "Item Budget":
            //        Item[] itemList = new Item[] {
            //            new Item() { Stats = new Stats() { Strength = 10 } },
            //            new Item() { Stats = new Stats() { Agility = 10 } },
            //            new Item() { Stats = new Stats() { AttackPower = 20 } },
            //            new Item() { Stats = new Stats() { CritRating = 10 } },
            //            new Item() { Stats = new Stats() { HitRating = 10 } },
            //            new Item() { Stats = new Stats() { ExpertiseRating = 10 } },
            //            new Item() { Stats = new Stats() { HasteRating = 10 } },
            //            new Item() { Stats = new Stats() { ArmorPenetration = 66.67f } },
            //            new Item() { Stats = new Stats() { SpellPower = 11.7f } },
            //        };
            //        string[] statList = new string[] {
            //            "Strength",
            //            "Agility",
            //            "Attack Power",
            //            "Crit Rating",
            //            "Hit Rating",
            //            "Expertise Rating",
            //            "Haste Rating",
            //            "Armor Penetration",
            //            "Spell Damage",
            //        };

            //        baseCalc = GetCharacterCalculations(character) as CharacterCalculationsRetribution;

            //        for (int index = 0; index < itemList.Length; index++)
            //        {
            //            calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsRetribution;

            //            comparison = CreateNewComparisonCalculation();
            //            comparison.Name = statList[index];
            //            comparison.Equipped = false;
            //            comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
            //            subPoints = new float[calc.SubPoints.Length];
            //            for (int i = 0; i < calc.SubPoints.Length; i++)
            //            {
            //                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
            //            }
            //            comparison.SubPoints = subPoints;

            //            comparisonList.Add(comparison);
            //        }
            //        return comparisonList.ToArray();
            //    default:
                    return new ComparisonCalculationBase[0];
 //           }
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == Item.ItemSlot.OffHand ||
                (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Libram))
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,

                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
				ArmorPenetration = stats.ArmorPenetration,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                SpellCrit = stats.SpellCrit,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
				PhysicalHit = stats.PhysicalHit,
				SpellHit = stats.SpellHit,
                Expertise = stats.Expertise,
                SpellPower = stats.SpellPower,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                DivineStormMultiplier = stats.DivineStormMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower + stats.SpellCrit + stats.DivineStormMultiplier +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.HasteRating +
                stats.CritRating + stats.HitRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit + stats.SpellHit + stats.SpellPower+
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier + stats.BonusDamageMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.BonusSpellCritMultiplier
                ) != 0;
        }
    }
}
