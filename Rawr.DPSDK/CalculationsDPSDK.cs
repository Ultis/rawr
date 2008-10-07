using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.DPSDK
{
    //[Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", Character.CharacterClass.Paladin)]  wont work until wotlk goes live on wowhead
    [Rawr.Calculations.RawrModelInfo("DPS", "spell_shadow_deathcoil", Character.CharacterClass.DeathKnight)]
	class CalculationsDPSDK : CalculationsBase
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
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
					    "Basic Stats:Armor Penetration",
					    "Advanced Stats:Weapon Damage*Damage before misses and mitigation",
					    "Advanced Stats:Attack Speed",
					    "Advanced Stats:Crit Chance",
					    "Advanced Stats:Avoided Attacks",
					    "Advanced Stats:Enemy Mitigation",
                        "DPS Breakdown:White",
                        "DPS Breakdown:Seal",
                        "DPS Breakdown:Windfury",
					    "DPS Breakdown:Crusader Strike",
                        "DPS Breakdown:Judgement",
                        "DPS Breakdown:Consecration",
                        "DPS Breakdown:Exorcism",
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
                    _customChartNames = new string[] { "Item Budget" };
                }
                return _customChartNames;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
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
                        Item.ItemType.Sigil,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword
					}));
            }
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationDPSDK();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsDPSDK();
        }


		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSDK));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsDPSDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSDK;
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
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            GetTalents(character);
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            calcs.BasicStats = stats;
            calcs.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
 
            return calcs;
        }


        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    { Strength = 123f, Agility = 79f, Stamina = 118f, Intellect = 87f, Spirit = 88f };
                    break;
                case Character.CharacterRace.Draenei: // Relevant racials: +1% hit
                    statsRace = new Stats()
                    { Strength = 127f, Agility = 74f, Stamina = 119f, Intellect = 84f, Spirit = 89f, Hit = .01f };
                    break;
                case Character.CharacterRace.Human: // Relevant racials: +10% spirit, +5 expertise when wielding mace or sword
                    statsRace = new Stats()
                    { Strength = 126f, Agility = 77f, Stamina = 120f, Intellect = 83f, Spirit = 89f, BonusSpiritMultiplier = 0.1f, };
                    //Expertise for Humans
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.TwoHandMace || character.MainHand.Type == Item.ItemType.TwoHandSword))
                        statsRace.Expertise = 5f;
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    { Strength = 128f, Agility = 73f, Stamina = 120f, Intellect = 83f, Spirit = 89f, };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            // Derived stats base amount, common to all races
            statsRace.AttackPower = 190f;
            statsRace.Health = 3197f;
            statsRace.Mana = 2673f;

            return statsRace;
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
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
           
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = new Stats();
            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();
            return (statsTotal);
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSDK baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetration = 66.67f } },
                        new Item() { Stats = new Stats() { SpellPower = 11.7f } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration",
                        "Spell Damage",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

                    for (int index = 0; index < itemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSDK;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
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
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,

                //CritRating = stats.CritRating,
                //HitRating = stats.HitRating,
                SpellPower = stats.SpellPower,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,

                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,

                LotPCritRating = stats.LotPCritRating,
                CritMeleeRating = stats.CritMeleeRating,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                Bloodlust = stats.Bloodlust,
                ExposeWeakness = stats.ExposeWeakness,
                DrumsOfBattle = stats.DrumsOfBattle,
                DrumsOfWar = stats.DrumsOfWar,
                ShatteredSunMightProc = stats.ShatteredSunMightProc,
                ExecutionerProc = stats.ExecutionerProc,
                MongooseProc = stats.MongooseProc,

                BonusCrusaderStrikeDamageMultiplier = stats.BonusCrusaderStrikeDamageMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ExpertiseRating + stats.HasteRating + stats.WeaponDamage + 
                stats.CritRating + stats.HitRating + stats.SpellPower + stats.SpellDamageFromSpiritPercentage +
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusSpellPowerMultiplier +
                stats.CritMeleeRating + stats.LotPCritRating + stats.WindfuryAPBonus + stats.Bloodlust + stats.ExposeWeakness +
                stats.DrumsOfBattle + stats.DrumsOfWar + stats.ShatteredSunMightProc + stats.ExecutionerProc + stats.MongooseProc +
                stats.BonusCrusaderStrikeDamageMultiplier) != 0;
        }


        /// <summary>
        /// Saves the talents for the character
        /// </summary>
        /// <param name="character">The character for whom the talents should be saved</param>
        public void GetTalents(Character character)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            calcOpts.talents = character.DeathKnightTalents;
        }
    }
}
