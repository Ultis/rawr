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
					new GemmingTemplate() { Model = "Retribution", Group = "Jeweler",
						RedId = bold[1], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[1], MetaId = chaotic },
				};
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Strength of Earth Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Might"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Blessing of Might"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Unleashed Rage"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Sanctified Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Swift Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Arcane Intellect"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Commanding Shout"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Leader of the Pack"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Windfury Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Elemental Oath"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Sunder Armor"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Faerie Fire"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Heart of the Crusader"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blood Frenzy"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Scorch"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Curse of the Elements"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Misery"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));

            character.PaladinTalents.GlyphOfJudgement = true;
            character.PaladinTalents.GlyphOfConsecration = true;
            character.PaladinTalents.GlyphOfSenseUndead = true;
            character.PaladinTalents.GlyphOfExorcism = true;
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
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
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
                        "DPS Breakdown:Total DPS",
                        "DPS Breakdown:White",
                        "DPS Breakdown:Seal",
					    "DPS Breakdown:Crusader Strike",
                        "DPS Breakdown:Judgement",
                        "DPS Breakdown:Consecration",
                        "DPS Breakdown:Exorcism",
                        "DPS Breakdown:Divine Storm",
                        "DPS Breakdown:Hammer of Wrath",
					    "Rotation Info:Crusader Strike CD",
                        "Rotation Info:Judgement CD",
                        "Rotation Info:Consecration CD",
                        "Rotation Info:Exorcism CD",
                        "Rotation Info:Divine Storm CD",
                        "Rotation Info:Hammer of Wrath CD",
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


        private readonly float[] DodgeChance = { 0.05f, 0.055f, 0.06f, 0.065f };
        private readonly float[] MissChance = { 0.05f, 0.052f, 0.054f, 0.08f };
        private readonly float[] ResistChance = { 0.04f, 0.05f, 0.06f, 0.17f };

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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            float fightLength = calcOpts.FightLength * 60f;
            PaladinTalents talents = character.PaladinTalents;
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRetribution calc = new CharacterCalculationsRetribution();
            calc.BasicStats = stats;

            //damage multipliers
            float twoHandedSpec = 1f + 0.02f * talents.TwoHandedWeaponSpecialization;
            float crusade = (1f + (calcOpts.MobType < 3 ? .02f : .01f) * talents.Crusade)
                * (talents.GlyphOfSenseUndead && calcOpts.MobType == 0 ? 1.01f : 1f);
            float vengeance = 1f + 0.03f * talents.Vengeance;
            float talentMulti = twoHandedSpec * crusade * vengeance;
            float critBonus = 2f * (1f + stats.BonusCritMultiplier);
            float spellCritBonus = 1.5f * (1f + stats.BonusSpellCritMultiplier);
            float aow = 1f + .05f * talents.TheArtOfWar;
            float rightVen = 1f + .1f * talents.RighteousVengeance;
            float sanctBattle = 1f + 0.05f * talents.SanctityOfBattle;

            float awTimes = (float)Math.Ceiling((fightLength - 20f) / (180f - talents.SanctifiedWrath * 30f));
            float aw = 1f + ((awTimes * 20f) / fightLength) * .2f;

            float spellPowerMulti = (1f + stats.BonusHolyDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            float physPowerMulti = (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier);
            const float partialResist = 0.953f; // Average of 4.7% damage lost to partial resists on spells

            calc.ToMiss = (float)Math.Max(MissChance[calcOpts.TargetLevel - 80] - stats.PhysicalHit, 0f);
            calc.ToDodge = (float)Math.Max(DodgeChance[calcOpts.TargetLevel - 80] - stats.Expertise * .0025f, 0f);
            calc.ToResist = (float)Math.Max(ResistChance[calcOpts.TargetLevel - 80] - stats.SpellHit, 0f);

            float meleeAvoid = 1f - calc.ToMiss - calc.ToDodge;
            float rangeAvoid = 1f - calc.ToMiss;
            float spellAvoid = 1f - calc.ToResist;

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
            float csHitDamage = (normalizedWeaponDamage * 1.1f + stats.CrusaderStrikeDamage) *
                talentMulti * physPowerMulti * armorReduction * aow * aw * sanctBattle * (1f + stats.CrusaderStrikeMultiplier);
            float csAvgDamage = csHitDamage * ((1f - csCrit) + csCrit * critBonus * rightVen) * meleeAvoid;
            calc.CrusaderStrikeDPS = csAvgDamage / 6f;
            #endregion

            #region Divine Storm
            float dsCrit = stats.PhysicalCrit + stats.DivineStormCrit;
            float dsHitDamage = (normalizedWeaponDamage + stats.DivineStormDamage) * 1.1f *
                talentMulti * physPowerMulti * armorReduction * aow * (1f + stats.DivineStormMultiplier) * aw;
            float dsAvgDamage = dsHitDamage * ((1f - dsCrit) + dsCrit * critBonus * rightVen) * meleeAvoid;
            calc.DivineStormDPS = dsAvgDamage / 10f;
            #endregion

            #region Judgement
            float judgeCrit = stats.PhysicalCrit + .06f * talents.Fanaticism;
            float judgeHitDamage = (calc.WeaponDamage * .26f + .18f * stats.SpellPower + .11f * stats.AttackPower) * aw
                * spellPowerMulti * talentMulti * partialResist * aow * (talents.GlyphOfJudgement ? 1.1f : 1f);
            float judgeAvgDamage = judgeHitDamage * ((1f - judgeCrit) + judgeCrit * critBonus * rightVen) * rangeAvoid;
            calc.JudgementDPS = judgeAvgDamage / (8f - stats.JudgementCDReduction);
            #endregion

            #region Consecration
            calc.ConsecrationDPS = (72f + .04f * (stats.SpellPower + stats.ConsecrationSpellPower) + .04f * stats.AttackPower)
                * vengeance * crusade * spellPowerMulti * partialResist * aw * spellAvoid;
            float consAvgDamage = calc.ConsecrationDPS * (talents.GlyphOfConsecration ? 10f : 8f);
            #endregion

            #region Exorcism
            float exoCrit = (calcOpts.MobType < 2 ? 1f : stats.SpellCrit);
            float exoHitDamage = (1087f + .42f * stats.SpellPower) * vengeance * crusade * spellPowerMulti * partialResist *
                aw * sanctBattle * (1f + stats.ExorcismMultiplier) * (talents.GlyphOfExorcism ? 1.2f : 1f);
            float exoAvgDamage = exoHitDamage * ((1f - exoCrit) + exoCrit * spellCritBonus) * spellAvoid;
            calc.ExorcismDPS = exoAvgDamage / 15f;
            #endregion

            #region Hammer of Wrath
            float howCrit = (float)Math.Min(1f, stats.PhysicalCrit + .25f * talents.SanctifiedWrath);
            float howHitDamage = (1198f + .15f * stats.SpellPower + .15f * stats.AttackPower) * spellPowerMulti * talentMulti * partialResist * aw * (1f + stats.HammerOfWrathMultiplier);
            float howAvgDamage = howHitDamage * ((1f - howCrit) + howCrit * critBonus) * rangeAvoid;
            calc.HammerOfWrathDPS = howAvgDamage / 6f;
            #endregion

            
            float sealHitDamage = calc.WeaponDamage * .48f * spellPowerMulti * talentMulti * partialResist * aw;
            float sealAvgDamage = sealHitDamage * ((1f - stats.PhysicalCrit) + stats.PhysicalCrit * critBonus) * meleeAvoid;

            if (calcOpts.SimulateRotation)
            {
                Rotation rotation = new Rotation(calcOpts.Priorities, calcOpts.TimeUnder20, calcOpts.Wait, calcOpts.Delay,
                    stats.JudgementCDReduction > 0 ? true : false, talents.GlyphOfConsecration);
                RotationSolution sol = RotationSimulator.SimulateRotation(rotation);
                calc.Rotation = sol;

                float sealProcs = (sol.FightLength / calc.AttackSpeed + sol.CrusaderStrike + sol.DivineStorm) * meleeAvoid;
                calc.SealDPS = sealAvgDamage * sealProcs / sol.FightLength;

                calc.OverallPoints = calc.DPSPoints =
                    calc.WhiteDPS +
                    calc.SealDPS +
                    (judgeAvgDamage * sol.Judgement +
                    csAvgDamage * sol.CrusaderStrike +
                    dsAvgDamage * sol.DivineStorm +
                    exoAvgDamage * sol.Exorcism +
                    consAvgDamage * sol.Consecration +
                    howAvgDamage * sol.HammerOfWrath) / sol.FightLength;

            }
            else
            {
                float sealProcs = fightLength * (1f / calc.AttackSpeed + 1f / calcOpts.CSCD + 1f / calcOpts.DSCD) * meleeAvoid;
                calc.SealDPS = sealAvgDamage * sealProcs / fightLength;
                float sealProcs20 = fightLength * (1f / calc.AttackSpeed + 1f / calcOpts.CSCD20 + 1f / calcOpts.DSCD20) * meleeAvoid;
                float sealDPS20 = sealAvgDamage * sealProcs20 / fightLength;

                calc.Rotation = new RotationSolution();
                calc.Rotation.JudgementCD = calcOpts.JudgeCD * (1f - calcOpts.TimeUnder20) + calcOpts.JudgeCD20 * calcOpts.TimeUnder20;
                calc.Rotation.CrusaderStrikeCD = calcOpts.CSCD * (1f - calcOpts.TimeUnder20) + calcOpts.CSCD20 * calcOpts.TimeUnder20;
                calc.Rotation.DivineStormCD = calcOpts.DSCD * (1f - calcOpts.TimeUnder20) + calcOpts.DSCD20 * calcOpts.TimeUnder20;
                calc.Rotation.ConsecrationCD = calcOpts.ConsCD * (1f - calcOpts.TimeUnder20) + calcOpts.ConsCD20 * calcOpts.TimeUnder20;
                calc.Rotation.ExorcismCD = calcOpts.ExoCD * (1f - calcOpts.TimeUnder20) + calcOpts.ExoCD20 * calcOpts.TimeUnder20;
                calc.Rotation.HammerOfWrathCD = calcOpts.HoWCD20;

                float dps = calc.WhiteDPS +
                    calc.SealDPS +
                    judgeAvgDamage / calcOpts.JudgeCD +
                    csAvgDamage / calcOpts.CSCD +
                    dsAvgDamage / calcOpts.DSCD +
                    exoAvgDamage / calcOpts.ExoCD +
                    consAvgDamage / calcOpts.ConsCD;

                float dps20 = calc.WhiteDPS +
                    sealDPS20 +
                    judgeAvgDamage / calcOpts.JudgeCD20 +
                    csAvgDamage / calcOpts.CSCD20 +
                    dsAvgDamage / calcOpts.DSCD20 +
                    exoAvgDamage / calcOpts.ExoCD20 +
                    consAvgDamage / calcOpts.ConsCD20 +
                    howAvgDamage / calcOpts.HoWCD20;

                calc.OverallPoints = calc.DPSPoints = dps * (1f - calcOpts.TimeUnder20) + dps20 * calcOpts.TimeUnder20;
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
            statsRace.PhysicalCrit = .0317f;
            statsRace.SpellCrit = .0317f;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats stats = statsBaseGear + statsBuffs + statsRace;

            float libramAP, libramCrit;
            if (calcOpts.SimulateRotation)
            {
                Rotation rot = new Rotation(calcOpts.Priorities, calcOpts.TimeUnder20, calcOpts.Wait, calcOpts.Delay,
                    stats.JudgementCDReduction > 0 ? true : false, talents.GlyphOfConsecration);
                RotationSolution sol = RotationSimulator.SimulateRotation(rot);

                libramAP = stats.APCrusaderStrike_10 * (float)Math.Min(1f, 10f * sol.CrusaderStrike / sol.FightLength);
                libramCrit = stats.CritJudgement_5 * 5f * sol.Judgement / sol.FightLength;
            }
            else
            {
                libramAP = stats.APCrusaderStrike_10 * 10f / (float)Math.Max(10, calcOpts.CSCD);
                libramCrit = stats.CritJudgement_5 * 5f / (calcOpts.JudgeCD * (1f - calcOpts.TimeUnder20) + calcOpts.JudgeCD20 * calcOpts.TimeUnder20);
            }

            float berserkingAP = stats.BerserkingProc * 140f;

            float greatnessStr = stats.GreatnessProc * ((float)Math.Floor(fightLength / 50f) * 15f + (float)Math.Min(fightLength % 50f, 15f)) / fightLength;
            stats.Strength = (stats.Strength + greatnessStr) * (1 + stats.BonusStrengthMultiplier) * (1f + talents.DivineStrength * .03f);
            stats.Intellect = stats.Intellect * (1 + stats.BonusIntellectMultiplier) * (1f + talents.DivineIntellect * .03f);
            stats.AttackPower = (stats.AttackPower + berserkingAP + libramAP + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier);
            stats.Agility = stats.Agility * (1 + stats.BonusAgilityMultiplier);
            stats.Stamina = stats.Stamina * (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .04f) * (1f + talents.CombatExpertise * .02f);
            stats.Health += stats.Stamina * 10;

            stats.PhysicalHit += stats.HitRating / 3278.998947f;
            stats.SpellHit += stats.HitRating / 2623.199272f;
            stats.Expertise += talents.CombatExpertise * 2 + stats.ExpertiseRating * 4f / 32.78998947f;// *1.25f;
            // Haste trinket (Meteorite Whetstone)
            stats.HasteRating += stats.HasteRatingOnPhysicalAttack * 10 / 45;

            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctityOfBattle * .01f;
            stats.PhysicalCrit = stats.PhysicalCrit + (stats.CritRating + libramCrit) / 4590.598679f + 
                stats.Agility / 5208.333333f + talentCrit - (calcOpts.TargetLevel == 83 ? 0.048f : 0f);
            stats.SpellCrit = stats.SpellCrit + (stats.CritRating + libramCrit) / 4590.598679f + stats.Intellect / 16666.66709f +
                talentCrit - (calcOpts.TargetLevel == 83 ? 0.03f : 0f);

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + stats.HasteRating * 1.3f / 3278.998947f) - 1f;

            stats.SpellPower += stats.Stamina * .1f * talents.TouchedByTheLight + stats.AttackPower * talents.SheathOfLight * .1f;

            return stats;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == Item.ItemSlot.OffHand ||
                (item.Slot == Item.ItemSlot.Ranged && item.Type != Item.ItemType.Libram))
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff)
        {
            Stats stats = buff.Stats;
            bool wantedStats = (stats.Strength + stats.Agility + stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.GreatnessProc + 
                stats.CritJudgement_5 + stats.CrusaderStrikeDamage + stats.APCrusaderStrike_10 + stats.ConsecrationSpellPower +
                stats.JudgementCDReduction + stats.BerserkingProc + stats.DivineStormDamage + stats.DivineStormCrit +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit +
                stats.HammerOfWrathMultiplier + stats.SpellPower + stats.BonusIntellectMultiplier + stats.Intellect +
                stats.Health + stats.Stamina + stats.SpellCrit + stats.BonusCritMultiplier + 
                stats.HitRating + stats.CritRating + stats.HasteRating + stats.SpellHit + stats.SpellPower +
                stats.BonusStaminaMultiplier + stats.BonusSpellCritMultiplier) > 0;
            return wantedStats;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Strength = stats.Strength,
                Agility = stats.Agility,
                Intellect = stats.Intellect,
                Stamina = stats.Stamina,
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
                CritJudgement_5 = stats.CritJudgement_5,
                CrusaderStrikeDamage = stats.CrusaderStrikeDamage,
                APCrusaderStrike_10 = stats.APCrusaderStrike_10,
                ConsecrationSpellPower = stats.ConsecrationSpellPower,
                JudgementCDReduction = stats.JudgementCDReduction,
                DivineStormDamage = stats.DivineStormDamage,
                DivineStormCrit = stats.DivineStormCrit,
                CrusaderStrikeCrit = stats.CrusaderStrikeCrit,
                ExorcismMultiplier = stats.ExorcismMultiplier,
                HammerOfWrathMultiplier = stats.HammerOfWrathMultiplier,
                CrusaderStrikeMultiplier = stats.CrusaderStrikeMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.GreatnessProc + 
                stats.CritJudgement_5 + stats.CrusaderStrikeDamage + stats.APCrusaderStrike_10 + stats.ConsecrationSpellPower +
                stats.JudgementCDReduction + stats.BerserkingProc + stats.DivineStormDamage + stats.DivineStormCrit + stats.BonusCritMultiplier +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit +
                stats.HammerOfWrathMultiplier) > 0;
            bool maybeStats = (stats.Agility + stats.Strength + 
                stats.HitRating + stats.CritRating + stats.HasteRating + stats.SpellHit + stats.SpellPower +
                stats.BonusStaminaMultiplier + stats.BonusSpellCritMultiplier) > 0;
            bool ignoreStats = (stats.Mp5 + stats.SpellPower + stats.DefenseRating +
                stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.BlockValue) > 0;
            return wantedStats || (maybeStats && ! ignoreStats);
        }
    }
}
