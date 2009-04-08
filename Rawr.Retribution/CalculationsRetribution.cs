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

                //hit
                int[] rigid = { 39915, 40014, 40125, 42156 };  // +hit

                //red
                int[] bold = { 39900, 39996, 40111, 42142 };  // +str

                //Orange
                //int[] inscribed = { 39947, 40037, 40142 };  // +str,+crit
                int[] etched = { 39948, 40038, 40143 };  // +str,+hit

                //Purple
                int[] sovereign = { 39934, 40022, 40129 }; // +str,+stam

                //Green
                int[] vivid = { 39975, 40088, 40166 }; // +hit,+stam

                //Meta
                int relentless = 41398;
                int chaotic = 41285;

                List<GemmingTemplate> retval = new List<GemmingTemplate>();

                string[] groupName = { "Uncommon", "Rare", "Epic" };
                for (int i = 0; i < 3; i++)
                {
                    retval.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupName[i],
                        RedId = bold[i],
                        YellowId = bold[i],
                        BlueId = bold[i],
                        PrismaticId = bold[i],
                        MetaId = relentless,
                        Enabled = i == 1
                    });
                    retval.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupName[i],
                        RedId = etched[i],
                        YellowId = rigid[i],
                        BlueId = vivid[i],
                        PrismaticId = rigid[i],
                        MetaId = relentless,
                        Enabled = i == 1
                    });
                    retval.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupName[i],
                        RedId = bold[i],
                        YellowId = etched[i],
                        BlueId = sovereign[i],
                        PrismaticId = bold[i],
                        MetaId = relentless,
                        Enabled = i == 1
                    });
                }

                retval.Add(new GemmingTemplate()
                {
                    Model = "Retribution",
                    Group = "Jeweler",
                    RedId = bold[1],
                    YellowId = bold[3],
                    BlueId = bold[3],
                    PrismaticId = bold[1],
                    MetaId = chaotic
                });
                retval.Add(new GemmingTemplate()
                {
                    Model = "Retribution",
                    Group = "Jeweler",
                    RedId = bold[3],
                    YellowId = bold[3],
                    BlueId = bold[3],
                    PrismaticId = bold[3],
                    MetaId = chaotic
                });


                return retval;
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
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of Endless Rage"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Fish Feast"));

            character.PaladinTalents.GlyphOfJudgement = true;
            character.PaladinTalents.GlyphOfConsecration = true;
            character.PaladinTalents.GlyphOfSenseUndead = true;
            character.PaladinTalents.GlyphOfExorcism = true;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Judgement");
                _relevantGlyphs.Add("Glyph of Exorcism");
                _relevantGlyphs.Add("Glyph of Sense Undead");
                _relevantGlyphs.Add("Glyph of Consecration");
                _relevantGlyphs.Add("Glyph of Seal of Blood");
                _relevantGlyphs.Add("Glyph of Seal of Command");
                _relevantGlyphs.Add("Glyph of Seal of Vengeance");
                _relevantGlyphs.Add("Glyph of Seal of Righteousness");
                _relevantGlyphs.Add("Glyph of Crusader Strike");
                _relevantGlyphs.Add("Glyph of Hammer of Wrath");
                _relevantGlyphs.Add("Glyph of Avenging Wrath");
            }
            return _relevantGlyphs;
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
                        "DPS Breakdown:Other*From trinket procs",
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
                    _customChartNames = new string[] { "Seals" };
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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            float fightLength = calcOpts.FightLength * 60f;
            PaladinTalents talents = character.PaladinTalents;
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsRetribution calc = new CharacterCalculationsRetribution();
            calc.BasicStats = GetCharacterStats(character, additionalItem, false);
            CombatStats combats = new CombatStats(character, stats);

            calc.AttackSpeed = combats.AttackSpeed;
            calc.WeaponDamage = combats.WeaponDamage;
            calc.ToMiss = CombatStats.GetMissChance(stats.PhysicalHit, calcOpts.TargetLevel);
            calc.ToDodge = CombatStats.GetDodgeChance(stats.Expertise, calcOpts.TargetLevel);
            calc.ToResist = CombatStats.GetResistChance(stats.SpellHit, calcOpts.TargetLevel);

            Rotation rot;
            if (calcOpts.SimulateRotation)
            {
                rot = new Simulator(combats);
            }
            else
            {
                rot = new EffectiveCooldown(combats);
            }
            calc.OtherDPS = stats.ArcaneDamage;
            rot.SetDPS(calc);
            calc.OverallPoints = calc.DPSPoints;

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
            return GetCharacterStats(character, additionalItem, true);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, bool computeAverageStats)
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
            statsRace.AttackPower = 220f;
            statsRace.Health = 6754f;
            statsRace.Mana = 4114;
            statsRace.PhysicalCrit = .0317f;
            statsRace.SpellCrit = .0317f;

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats stats = statsBaseGear + statsBuffs + statsRace;

            if (computeAverageStats)
            {
                Rotation rot;
                float libramAP = 0, libramCrit = 0;

                float berserkingAP = stats.BerserkingProc * 140f;

                stats.AttackPower += berserkingAP + libramAP;
                stats.CritRating += libramCrit;
                stats.Expertise += (talents.GlyphOfSealOfVengeance && calcOpts.Seal == Seal.Vengeance) ? 10f : 0;

                ConvertRatings(stats, talents, calcOpts.TargetLevel);

                CombatStats combats = new CombatStats(character, stats);
                if (calcOpts.SimulateRotation)
                {
                    rot = new Simulator(combats);
                    RotationSolution sol = ((Simulator)rot).Solution;

                    libramAP = stats.APCrusaderStrike_10 * (float)Math.Min(1f, 10f * sol.CrusaderStrike / sol.FightLength);
                    libramCrit = stats.CritJudgement_5 * 5f * sol.Judgement / sol.FightLength;

                }
                else
                {
                    rot = new EffectiveCooldown(combats);
                    libramAP = stats.APCrusaderStrike_10 * 10f / (float)Math.Max(10, calcOpts.CSCD);
                    libramCrit = stats.CritJudgement_5 * 5f / (calcOpts.JudgeCD * (1f - calcOpts.TimeUnder20) + calcOpts.JudgeCD20 * calcOpts.TimeUnder20);
                }

                Stats statsAverage = new Stats();
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (effect.Trigger == Trigger.Use)
                    {
                        statsAverage += effect.GetAverageStats();
                    }
                    else
                    {
                        float trigger = 0;
                        if (effect.Trigger == Trigger.MeleeCrit)
                        {
                            trigger = 1f / rot.GetMeleeCritsPerSec();
                        }
                        else if (effect.Trigger == Trigger.MeleeHit)
                        {
                            trigger = 1f / rot.GetMeleeAttacksPerSec();
                        }
                        else if (effect.Trigger == Trigger.PhysicalCrit)
                        {
                            trigger = 1f / rot.GetPhysicalCritsPerSec();
                        }
                        else if (effect.Trigger == Trigger.PhysicalHit)
                        {
                            trigger = 1f / rot.GetPhysicalAttacksPerSec();
                        }

                        if (effect.MaxStack > 1)
                        {
                            float timeToMax = (float)Math.Min(fightLength, effect.Chance * trigger * effect.MaxStack * (1f + calcOpts.StackTrinketReset));
                            statsAverage += effect.Stats * (effect.MaxStack * ((fightLength - .5f * timeToMax) / fightLength));
                        }
                        else
                        {
                            statsAverage += effect.GetAverageStats(trigger);
                        }
                    }
                }

                stats = statsBaseGear + statsBuffs + statsRace + statsAverage;
                stats.AttackPower += berserkingAP + libramAP;
                stats.Strength += stats.HighestStat;
                stats.CritRating += libramCrit;
                stats.Expertise += (talents.GlyphOfSealOfVengeance && calcOpts.Seal == Seal.Vengeance) ? 10f : 0;

                ConvertRatings(stats, talents, calcOpts.TargetLevel);
            }
            else
            {
                ConvertRatings(stats, talents, calcOpts.TargetLevel);
            }
            return stats;
        }

        private void ConvertRatings(Stats stats, PaladinTalents talents, int targetLevel)
        {
            stats.Strength *= (1 + stats.BonusStrengthMultiplier) * (1f + talents.DivineStrength * .03f);
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier) * (1f + talents.DivineIntellect * .03f);
            stats.AttackPower = (stats.AttackPower + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier);
            stats.Agility *= (1 + stats.BonusAgilityMultiplier);
            stats.Stamina *= (1 + stats.BonusStaminaMultiplier) * (1f + talents.SacredDuty * .04f) * (1f + talents.CombatExpertise * .02f);
            stats.Health += stats.Stamina * 10;

            stats.PhysicalHit += stats.HitRating / 3278.998947f;
            stats.SpellHit += stats.HitRating / 2623.199272f;
            stats.Expertise += talents.CombatExpertise * 2 + stats.ExpertiseRating * 4f / 32.78998947f;

            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctityOfBattle * .01f;
            stats.PhysicalCrit = stats.PhysicalCrit + stats.CritRating / 4590.598679f +
                stats.Agility / 5208.333333f + talentCrit - (targetLevel == 83 ? 0.048f : 0f);
            stats.SpellCrit = stats.SpellCrit + stats.CritRating / 4590.598679f + stats.Intellect / 16666.66709f +
                talentCrit - (targetLevel == 83 ? 0.03f : 0f);

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + stats.HasteRating * 1.3f / 3278.998947f) - 1f;

            stats.SpellPower += stats.Stamina * .1f * talents.TouchedByTheLight + stats.AttackPower * talents.SheathOfLight * .1f;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Seals")
            {

                CalculationOptionsRetribution initOpts = character.CalculationOptions as CalculationOptionsRetribution;

                Character baseChar = character.Clone();
                CalculationOptionsRetribution baseOpts = initOpts.Clone();
                baseChar.CalculationOptions = baseOpts;
                baseOpts.Seal = Seal.None;
                CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(baseChar);

                Character deltaChar = character.Clone();
                CalculationOptionsRetribution deltaOpts = initOpts.Clone();
                deltaChar.CalculationOptions = deltaOpts;

                ComparisonCalculationBase Blood;
                deltaOpts.Seal = Seal.Blood;
                Blood = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Blood", initOpts.Seal == Seal.Blood);
                Blood.Item = null;

                ComparisonCalculationBase Command;
                deltaOpts.Seal = Seal.Command;
                Command = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Command", initOpts.Seal == Seal.Command);
                Command.Item = null;

                ComparisonCalculationBase Righteousness;
                deltaOpts.Seal = Seal.Righteousness;
                Righteousness = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Righteousness", initOpts.Seal == Seal.Righteousness);
                Righteousness.Item = null;

                ComparisonCalculationBase Vengeance;
                deltaOpts.Seal = Seal.Vengeance;
                Vengeance = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Vengeance", initOpts.Seal == Seal.Vengeance);
                Vengeance.Item = null;

                return new ComparisonCalculationBase[] { Blood, Command, Righteousness, Vengeance };
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

        public override bool IsBuffRelevant(Buff buff)
        {
            Stats stats = buff.Stats;
            bool wantedStats = (stats.Strength + stats.Agility + stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier +
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
            Stats s = new Stats()
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
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit)
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public bool HasRelevantSpecialEffectStats(Stats stats)
        {
            return (stats.Strength + stats.Agility + stats.AttackPower + stats.CritRating + stats.ArmorPenetrationRating
            + stats.HasteRating + stats.ArcaneDamage + stats.HighestStat) > 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier +
                stats.CritJudgement_5 + stats.CrusaderStrikeDamage + stats.APCrusaderStrike_10 + stats.ConsecrationSpellPower +
                stats.JudgementCDReduction + stats.BerserkingProc + stats.DivineStormDamage + stats.DivineStormCrit + stats.BonusCritMultiplier +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit +
                stats.HammerOfWrathMultiplier) > 0;
            bool maybeStats = (stats.Agility + stats.Strength +
                stats.HitRating + stats.CritRating + stats.HasteRating + stats.SpellHit + stats.SpellPower +
                stats.BonusStaminaMultiplier + stats.BonusSpellCritMultiplier) > 0;
            bool ignoreStats = (stats.Mp5 + stats.SpellPower + stats.DefenseRating +
                stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.BlockValue) > 0;
            bool specialEffect = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit)
                {
                    specialEffect = HasRelevantSpecialEffectStats(effect.Stats);
                    if (specialEffect) break;
                }
            }
            return wantedStats || ((maybeStats || specialEffect) && !ignoreStats);
        }
    }
}
