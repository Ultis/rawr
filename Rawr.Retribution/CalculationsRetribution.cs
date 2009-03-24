using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    [Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_CrusaderStrike", Character.CharacterClass.Paladin)]
	class CalculationsRetribution : CalculationsBase
    {

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs

                //red
                int[] bold = { 39900, 39996, 40111, 42142 };  // +str

                //Orange
                int[] inscribed = { 39947, 40037, 40142 };  // +str,+crit
                int[] etched = { 39948, 40038, 40143 };  // +str,+hit

                //Purple
                int[] sovereign = { 39934, 40022, 40129 }; // +str,+stam

                //Meta
                int relentless = 41398;
                int chaotic = 41285;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Retribution", Group = "Uncommon",
						RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Uncommon",
						RedId = bold[0], YellowId = inscribed[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Uncommon",
						RedId = bold[0], YellowId = etched[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = relentless },

					new GemmingTemplate() { Model = "Retribution", Group = "Rare", Enabled = true,
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Rare", Enabled = true,
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Rare", Enabled = true,
						RedId = bold[1], YellowId = etched[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Retribution", Group = "Epic",
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Epic",
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = relentless },
					new GemmingTemplate() { Model = "Retribution", Group = "Epic",
						RedId = bold[2], YellowId = etched[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = relentless },
						
					new GemmingTemplate() { Model = "Retribution", Group = "Jeweler",
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
				};
            }
        }

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
                    _subPointNameColors.Add("Unlimited DPS", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Limited DPS", System.Drawing.Color.Blue);
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
                    _customChartNames = new string[] { "Glyphs" };
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
            float fightLength = calcOpts.FightLength * 60f;
            PaladinTalents talents = character.PaladinTalents;
            Stats stats = GetCharacterStats(character, additionalItem);

            Rotation unlimitedRot = new Rotation(calcOpts.UnlimitedPriorities, calcOpts.TimeUnder20, stats.JudgementCDReduction > 0 ? true : false,
                calcOpts.GlyphConsecration);
            Rotation limitedRot = new Rotation(calcOpts.LimitedPriorities, calcOpts.TimeUnder20, stats.JudgementCDReduction > 0 ? true : false,
                 calcOpts.GlyphConsecration);
            RotationSolution unlimited = RotationSimulator.SimulateRotation(unlimitedRot);
            RotationSolution limited = RotationSimulator.SimulateRotation(limitedRot);

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
            float rightVen = .1f * talents.RighteousVengeance;
            float sanctBattle = 1f + 0.05f * talents.SanctityOfBattle;

            float awTimes = (float)Math.Ceiling((fightLength - 20f) / (180f - talents.SanctifiedWrath * 30f));
            float aw = 1f + ((awTimes * 20f) / fightLength) * .2f;

            float spellPowerMulti = (1f + stats.BonusHolyDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            float physPowerMulti = (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            const float partialResist = 0.953f; // Average of 4.7% damage lost to partial resists on spells

            calc.ToMiss = (float)Math.Max(0.08f - stats.PhysicalHit, 0f);
            calc.ToDodge = (float)Math.Max(0.065f - stats.Expertise * .0025f, 0f);
            calc.ToResist = (float)Math.Max(0.17f - stats.SpellHit, 0f);

            int targetArmor = 10645;
			float armorReduction = 1f - ArmorCalculations.GetDamageReduction(character.Level, targetArmor,
                stats.ArmorPenetration, stats.ArmorPenetrationRating);

            #region Weapon Damage
            float bloodlustUptime = ((float)Math.Floor(fightLength / 300f) * 40f + (float)Math.Min(fightLength % 300f, 40f)) / fightLength;
            float bloodlustHaste = 1f + (bloodlustUptime * .3f);

            float baseSpeed = character.MainHand == null ? 3.5f : character.MainHand.Speed;
            float baseWeaponDamage = character.MainHand == null ? 371.5f : (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f;
            calc.AttackSpeed = baseSpeed / ( (1f + stats.PhysicalHaste) * bloodlustHaste );
            calc.WeaponDamage = baseWeaponDamage + stats.AttackPower * baseSpeed / 14f;
            float normalizedWeaponDamage = calc.WeaponDamage * 3.3f / baseSpeed;

            float whiteDamage = calc.WeaponDamage * talentMulti * physPowerMulti * armorReduction * aw;

            const float glancingAmount = 1f - 0.35f;
            const float glanceChance = .24f;

            float avgWhiteHit = whiteDamage * (glanceChance * glancingAmount + stats.PhysicalCrit * critBonus + (1f - stats.PhysicalCrit - glanceChance - calc.ToMiss - calc.ToDodge));
            calc.WhiteDPS = avgWhiteHit / calc.AttackSpeed;
            #endregion

            #region Crusader Strike
            float csCrit = stats.PhysicalCrit + stats.CrusaderStrikeCrit;
            float csDamage = (normalizedWeaponDamage * 1.1f + stats.CrusaderStrikeDamage) *
                talentMulti * physPowerMulti * armorReduction * aow * aw * sanctBattle;
            float csAvgHit = csDamage * (1f + csCrit * critBonus - csCrit - calc.ToMiss - calc.ToDodge);
            float csRightVen = csDamage * critBonus * rightVen;
            calc.CrusaderStrikeDPS = (csAvgHit + csRightVen) / 6f;
            #endregion

            #region Divine Storm
            float dsCrit = stats.PhysicalCrit + stats.DivineStormCrit;
            float dsDamage = (normalizedWeaponDamage + stats.DivineStormDamage) * 1.1f *
                talentMulti * physPowerMulti * armorReduction * aow * (1f + stats.DivineStormMultiplier) * aw;
            float dsAvgHit = dsDamage * (1f + dsCrit * critBonus - dsCrit - calc.ToMiss - calc.ToDodge);
            float dsRightVen = dsDamage * critBonus * rightVen;
            calc.DivineStormDPS = (dsAvgHit + dsRightVen) / 10f;
            #endregion

            #region Judgement
            float judgeCrit = stats.PhysicalCrit + .06f * talents.Fanaticism;
            float judgeDamage = (calc.WeaponDamage * .36f + .25f * stats.SpellPower + .16f * stats.AttackPower) * aw
                * spellPowerMulti * talentMulti * partialResist * aow * (calcOpts.GlyphJudgement ? 1.1f : 1f);
            float judgeAvgHit = judgeDamage * (1f + judgeCrit * critBonus - judgeCrit - calc.ToMiss);
            float judgeRightVen = judgeDamage * critBonus * rightVen;
            calc.JudgementDPS = (judgeAvgHit + judgeRightVen) / (8f - stats.JudgementCDReduction);
            #endregion

            #region Consecration
            calc.ConsecrationDPS = (72f + .04f * (stats.SpellPower + stats.ConsecrationSpellPower) + .04f * stats.AttackPower) * vengeance * crusade * spellPowerMulti * partialResist * aw;
            float consAvgHit = calc.ConsecrationDPS * (calcOpts.GlyphConsecration ? 10f : 8f);
            #endregion

            #region Exorcism
            float exoCrit = (calcOpts.MobType < 2 ? 1f : stats.SpellCrit);
            float exoDamage = (1087f + .42f * stats.SpellPower) * vengeance * crusade * spellPowerMulti * partialResist * 
                aw * sanctBattle * (1f + stats.ExorcismMultiplier) * (calcOpts.GlyphExorcism ? 1.2f : 1f); 
            float exoAvgHit = exoDamage * (1f + exoCrit * spellCritBonus - exoCrit - calc.ToResist);
            calc.ExorcismDPS = exoAvgHit / 15f;
            #endregion

            #region Hammer of Wrath
            float howCrit = stats.PhysicalCrit + .25f * talents.SanctifiedRetribution;
            float howDamage = (1198f + .15f * stats.SpellPower + .15f * stats.AttackPower) * spellPowerMulti * talentMulti * partialResist * aw * (1f + stats.HammerOfWrathMultiplier);
            float howAvgHit = howDamage * (1f + howCrit * critBonus - howCrit - calc.ToMiss);
            calc.HammerOfWrathDPS = howAvgHit / 6f;
            #endregion

            #region Seal
            float sealDamage = calc.WeaponDamage * .27f * spellPowerMulti * talentMulti * partialResist * aw;
            float sealAvgHit = sealDamage * (1f + stats.PhysicalCrit * critBonus - stats.PhysicalCrit - calc.ToMiss - calc.ToDodge);
            float sealProcs = (unlimited.FightLength / calc.AttackSpeed + unlimited.CrusaderStrike + unlimited.DivineStorm) * (1f - calc.ToMiss - calc.ToDodge);
            calc.SealDPS = sealAvgHit * sealProcs * (1f - calc.ToMiss - calc.ToDodge) / unlimited.FightLength;
            #endregion

            float unlimitedDps = calc.WhiteDPS + calc.SealDPS + ((judgeAvgHit + judgeRightVen) * unlimited.Judgement +
                (csAvgHit + csRightVen) * unlimited.CrusaderStrike + (dsAvgHit + dsRightVen) * unlimited.DivineStorm + exoAvgHit * unlimited.Exorcism +
                consAvgHit * unlimited.Consecration + howAvgHit * unlimited.HammerOfWrath) / unlimited.FightLength;

            if (calcOpts.SimulateMana)
            {
                const float baseMana = 4394f;
                float benediction = 1f - .02f * talents.Benediction;
                float csMana = baseMana * -.08f * benediction * (calcOpts.GlyphCrusaderStrike ? .8f : 1f);
                float judgeMana = baseMana * -.05f * benediction +
                    (1f - calc.ToMiss) * 25f / 3f * baseMana * talents.JudgementsOfTheWise;
                float dsMana = baseMana * -.12f * benediction;
                float howMana = baseMana * -.12f * benediction * (calcOpts.GlyphHammerOfWrath ? .5f : 1f);
                float exoMana = baseMana * -.08f * benediction;
                float consMana = baseMana * -.22f * benediction;

                float unlimitedUsage = csMana / unlimited.CrusaderStrikeCD + judgeMana / unlimited.JudgementCD + dsMana / unlimited.DivineStormCD +
                    howMana / unlimited.HammerOfWrathCD + exoMana / unlimited.ExorcismCD + consMana / unlimited.ConsecrationCD;

                float limitedDps = calc.WhiteDPS + calc.SealDPS + ((judgeAvgHit + judgeRightVen) * limited.Judgement +
                    (csAvgHit + csRightVen) * limited.CrusaderStrike + (dsAvgHit + dsRightVen) * limited.DivineStorm + exoAvgHit * limited.Exorcism +
                    consAvgHit * limited.Consecration + howAvgHit * limited.HammerOfWrath) / limited.FightLength;
                float limitedUsage = csMana / limited.CrusaderStrikeCD + judgeMana / limited.JudgementCD + dsMana / limited.DivineStormCD +
                    howMana / limited.HammerOfWrathCD + exoMana / limited.ExorcismCD + consMana / limited.ConsecrationCD;
                //float limitedDps = calc.WhiteDPS + calc.SealDPS + ((judgeAvgHit + judgeRightVen) * unlimited.Judgement +
                //    csAvgHit * unlimited.CrusaderStrike + (dsAvgHit + dsRightVen) * unlimited.DivineStorm +
                //    howAvgHit * unlimited.HammerOfWrath) / unlimited.FightLength;
                //float limitedUsage = csMana / unlimited.CrusaderStrikeCD + judgeMana / unlimited.JudgementCD + dsMana / unlimited.DivineStormCD +
                //    howMana / unlimited.HammerOfWrathCD;

                float divinePleas = (float)Math.Ceiling((fightLength - 30) / 60);
                float jowProcs = fightLength * (1 / calc.AttackSpeed + 1 / unlimited.CrusaderStrikeCD + 1 / unlimited.DivineStormCD +
                    1 / unlimited.JudgementCD) * (1f - calc.ToMiss - calc.ToDodge);
                float totalMana = stats.Mana + stats.Mana * .25f * divinePleas + jowProcs * stats.ManaRestoreFromBaseManaPerHit * baseMana +
                    stats.ManaRestoreFromMaxManaPerSecond * stats.Mana * fightLength + stats.Mp5 * fightLength / 5f +
                    (calcOpts.GlyphDivinity ? (float)Math.Min(stats.Mana, 7800) : 1950f);

                float unlimitedTime = (totalMana + fightLength * limitedUsage) / (limitedUsage - unlimitedUsage);
                float limitedTime;
                if (unlimitedTime > fightLength)
                {
                    unlimitedTime = fightLength;
                    limitedTime = 0;
                }
                else if (unlimitedTime < 0)
                {
                    unlimitedTime = 0;
                    limitedTime = totalMana / -limitedUsage;
                }
                else
                {
                    limitedTime = fightLength - unlimitedTime;
                }

                float mixedDps = (unlimitedDps * unlimitedTime + limitedDps * limitedTime) / fightLength;

                calc.LimitedPoints = mixedDps - unlimitedDps;
                calc.UnlimitedPoints = unlimitedDps;
                calc.OverallPoints = calc.LimitedPoints + calc.UnlimitedPoints;
            }
            else
            {
                calc.LimitedPoints = 0;
                calc.OverallPoints = calc.UnlimitedPoints = unlimitedDps;
            }

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
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            float fightLength = calcOpts.FightLength * 60f;
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats() { Strength = 19f, Agility = 22f, Stamina = 20f, Intellect = 24f, Spirit = 20f };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Strength = 23f, Agility = 17f, Stamina = 21f, Intellect = 21f, Spirit = 20f };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats() { Strength = 22f, Agility = 20f, Stamina = 22f, Intellect = 20f, Spirit = 22f };
                    if (character.MainHand != null && (character.MainHand.Type == Item.ItemType.TwoHandMace || character.MainHand.Type == Item.ItemType.TwoHandSword))
                        statsRace.Expertise = 3f;
                    break;
                default: //defaults to Dwarf stats
                    statsRace = new Stats() { Strength = 24f, Agility = 16f, Stamina = 25f, Intellect = 19f, Spirit = 20f, };
                    if (character.MainHand != null && character.MainHand.Type == Item.ItemType.TwoHandMace)
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
            statsRace.AttackPower = 220f;
            statsRace.Health = 6754f;
            statsRace.Mana = 4114;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats stats = statsBaseGear + statsBuffs + statsRace;

            Rotation rot = new Rotation(calcOpts.UnlimitedPriorities, calcOpts.TimeUnder20, stats.JudgementCDReduction > 0 ? true : false,
                calcOpts.GlyphConsecration);
            RotationSolution sol = RotationSimulator.SimulateRotation(rot);

            float berserkingAP = stats.BerserkingProc * 140f;

            float greatnessStr = stats.GreatnessProc * ((float)Math.Floor(fightLength / 50f) * 15f + (float)Math.Min(fightLength % 50f, 15f)) / fightLength;
            stats.Strength = (stats.Strength + greatnessStr) * (1 + stats.BonusStrengthMultiplier) * (1f + talents.DivineStrength * .03f);
            stats.Intellect = stats.Intellect * (1 + stats.BonusIntellectMultiplier) * (1f + talents.DivineIntellect * .03f);
            float libramAP = stats.APCrusaderStrike_6 * 6f * sol.CrusaderStrike / fightLength;
            stats.AttackPower = (stats.AttackPower + berserkingAP + libramAP + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier);
            stats.Agility = stats.Agility * (1 + stats.BonusAgilityMultiplier);
            stats.Stamina = stats.Stamina * (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .04f) * (1f + talents.CombatExpertise * .02f);
            stats.Health += stats.Stamina * 10;
            stats.Mana += stats.Intellect * 15f;

            stats.PhysicalHit += stats.HitRating / 3278.998947f;
            stats.SpellHit += stats.HitRating / 2623.199272f;
            stats.Expertise += talents.CombatExpertise * 2 + stats.ExpertiseRating * 4f / 32.78998947f;// *1.25f;
            // Haste trinket (Meteorite Whetstone)
            stats.HasteRating += stats.HasteRatingOnPhysicalAttack * 10 / 45;

            float libramCrit = stats.CritJudgement_5 * 5f * sol.Judgement / fightLength
                + stats.CritDivineStorm_8 * 8f * sol.DivineStorm / fightLength;
            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctityOfBattle * .01f;
            stats.PhysicalCrit = stats.PhysicalCrit + (stats.CritRating + libramCrit) / 4590.598679f + stats.Agility / 5208.333333f + talentCrit;
            stats.SpellCrit = stats.SpellCrit + (stats.CritRating + libramCrit) / 4590.598679f + stats.Intellect / 16666.66709f + talentCrit;

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + stats.HasteRating * 1.3f / 3278.998947f) - 1f;

            stats.SpellPower += stats.Stamina * .1f * talents.TouchedByTheLight + stats.AttackPower * talents.SheathOfLight * .1f;

            return stats;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Glyphs")
            {
                
                CalculationOptionsRetribution initOpts = character.CalculationOptions as CalculationOptionsRetribution;

                Character baseChar = character.Clone();
                CalculationOptionsRetribution baseOpts = initOpts.Clone();
                baseChar.CalculationOptions = baseOpts;

                Character deltaChar = character.Clone();
                CalculationOptionsRetribution deltaOpts = initOpts.Clone();
                deltaChar.CalculationOptions = deltaOpts;

                CharacterCalculationsBase baseCalc;

                ComparisonCalculationBase Judgement;
                baseOpts.GlyphJudgement = false;
                deltaOpts.GlyphJudgement = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                Judgement = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Judgement", initOpts.GlyphJudgement);
                deltaOpts.GlyphJudgement = baseOpts.GlyphJudgement = initOpts.GlyphJudgement;

                ComparisonCalculationBase Consecration;
                baseOpts.GlyphConsecration = false;
                deltaOpts.GlyphConsecration = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                Consecration = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Consecration", initOpts.GlyphConsecration);
                deltaOpts.GlyphConsecration = baseOpts.GlyphConsecration = initOpts.GlyphConsecration;

                ComparisonCalculationBase SenseUndead;
                baseOpts.GlyphSenseUndead = false;
                deltaOpts.GlyphSenseUndead = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                SenseUndead = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Sense Undead", initOpts.GlyphSenseUndead);
                deltaOpts.GlyphSenseUndead = baseOpts.GlyphSenseUndead = initOpts.GlyphSenseUndead;

                ComparisonCalculationBase CrusaderStrike;
                baseOpts.GlyphCrusaderStrike = false;
                deltaOpts.GlyphCrusaderStrike = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                CrusaderStrike = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Crusader Strike", initOpts.GlyphCrusaderStrike);
                deltaOpts.GlyphCrusaderStrike = baseOpts.GlyphCrusaderStrike = initOpts.GlyphCrusaderStrike;

                ComparisonCalculationBase Exorcism;
                baseOpts.GlyphExorcism = false;
                deltaOpts.GlyphExorcism = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                Exorcism = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Exorcism", initOpts.GlyphExorcism);
                deltaOpts.GlyphExorcism = baseOpts.GlyphExorcism = initOpts.GlyphExorcism;

                ComparisonCalculationBase HammerOfWrath;
                baseOpts.GlyphHammerOfWrath = false;
                deltaOpts.GlyphHammerOfWrath = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                HammerOfWrath = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Hammer of Wrath", initOpts.GlyphHammerOfWrath);
                deltaOpts.GlyphHammerOfWrath = baseOpts.GlyphHammerOfWrath = initOpts.GlyphHammerOfWrath;

                ComparisonCalculationBase Divinity;
                baseOpts.GlyphDivinity = false;
                deltaOpts.GlyphDivinity = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                Divinity = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Divinity", initOpts.GlyphDivinity);
                deltaOpts.GlyphDivinity = baseOpts.GlyphDivinity = initOpts.GlyphDivinity;

                return new ComparisonCalculationBase[] { Judgement, Consecration, SenseUndead, Exorcism, 
                    CrusaderStrike, HammerOfWrath, Divinity };
            }
            else
            {
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
                Mana = stats.Mana,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
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
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                DivineStormMultiplier = stats.DivineStormMultiplier,
                BerserkingProc = stats.BerserkingProc,
                GreatnessProc = stats.GreatnessProc,
                CritDivineStorm_8 = stats.CritDivineStorm_8,
                CritJudgement_5 = stats.CritJudgement_5,
                CrusaderStrikeDamage = stats.CrusaderStrikeDamage,
                APCrusaderStrike_6 = stats.APCrusaderStrike_6,
                ConsecrationSpellPower = stats.ConsecrationSpellPower,
                JudgementCDReduction = stats.JudgementCDReduction,
                DivineStormDamage = stats.DivineStormDamage,
                DivineStormCrit = stats.DivineStormCrit,
                CrusaderStrikeCrit = stats.CrusaderStrikeCrit,
                ExorcismMultiplier = stats.ExorcismMultiplier,
                HammerOfWrathMultiplier = stats.HammerOfWrathMultiplier,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Health + stats.Strength + stats.Agility + stats.Stamina + stats.Spirit + stats.AttackPower + stats.SpellCrit + stats.DivineStormMultiplier +
                stats.HitRating + stats.CritRating + stats.ArmorPenetration + stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.HasteRating +
                stats.CritRating + stats.HitRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit + stats.SpellHit + stats.SpellPower+
                stats.BonusStrengthMultiplier + stats.BonusStaminaMultiplier + stats.BonusAgilityMultiplier + stats.BonusCritMultiplier + stats.BonusDamageMultiplier +
                stats.BonusAttackPowerMultiplier + stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.BonusSpellCritMultiplier +
                stats.GreatnessProc + stats.CritDivineStorm_8 + stats.CritJudgement_5 + stats.CrusaderStrikeDamage + stats.APCrusaderStrike_6 + stats.ConsecrationSpellPower +
                stats.JudgementCDReduction + stats.BerserkingProc + stats.Mp5 + stats.ManaRestoreFromMaxManaPerSecond + stats.ManaRestoreFromBaseManaPerHit + stats.Mana +
                stats.BonusIntellectMultiplier + stats.DivineStormDamage + stats.DivineStormCrit + stats.CrusaderStrikeCrit + stats.ExorcismMultiplier +
                stats.HammerOfWrathMultiplier
                ) != 0;
        }
    }
}
