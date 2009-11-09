using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    [Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_CrusaderStrike", CharacterClass.Paladin)]
    public class CalculationsRetribution : CalculationsBase
    {

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs

                //Prismatic
                int[] tear = { 42701, 42702, 49110 }; //+stats

                //Yellow
                int[] rigid = { 39915, 40014, 40125, 42156 };  // +hit

                //Red
                int[] bold = { 39900, 39996, 40111, 42142 };  // +str

                //Orange
                int[] inscribed = { 39947, 40037, 40142 };  // +str,+crit
                int[] etched = { 39948, 40038, 40143 };  // +str,+hit

                //Purple
                int[] sovereign = { 39934, 40022, 40129 }; // +str,+stam

                //Green
                int[] vivid = { 39975, 40088, 40166 }; // +hit,+stam

                //Meta
                int relentless = 41398;
                //int chaotic = 41285;

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
                        Enabled = i == 2
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
                        Enabled = i == 2
                    });
                    retval.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupName[i],
                        RedId = bold[i],
                        YellowId = inscribed[i],
                        BlueId = sovereign[i],
                        PrismaticId = bold[i],
                        MetaId = relentless,
                        Enabled = i == 2
                    });
                    retval.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupName[i],
                        RedId = bold[i],
                        YellowId = tear[i],
                        BlueId = tear[i],
                        PrismaticId = bold[i],
                        MetaId = relentless,
                        Enabled = i == 2
                    });
                }

                retval.Add(new GemmingTemplate()
                {
                    Model = "Retribution",
                    Group = "Jeweler",
                    RedId = bold[3],
                    YellowId = bold[2],
                    BlueId = bold[2],
                    PrismaticId = bold[3],
                    MetaId = relentless
                });
                retval.Add(new GemmingTemplate()
                {
                    Model = "Retribution",
                    Group = "Jeweler",
                    RedId = bold[3],
                    YellowId = bold[3],
                    BlueId = bold[3],
                    PrismaticId = bold[3],
                    MetaId = relentless
                });


                return retval;
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Strength of Earth Totem"));
            character.ActiveBuffsAdd(("Blessing of Might"));
            character.ActiveBuffsAdd(("Improved Blessing of Might"));
            character.ActiveBuffsAdd(("Unleashed Rage"));
            character.ActiveBuffsAdd(("Sanctified Retribution"));
            character.ActiveBuffsAdd(("Swift Retribution"));
            character.ActiveBuffsAdd(("Arcane Intellect"));
            character.ActiveBuffsAdd(("Commanding Shout"));
            character.ActiveBuffsAdd(("Leader of the Pack"));
            character.ActiveBuffsAdd(("Windfury Totem"));
            character.ActiveBuffsAdd(("Elemental Oath"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Improved Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Improved Mark of the Wild"));
            character.ActiveBuffsAdd(("Sunder Armor"));
            character.ActiveBuffsAdd(("Faerie Fire"));
            character.ActiveBuffsAdd(("Heart of the Crusader"));
            character.ActiveBuffsAdd(("Blood Frenzy"));
            character.ActiveBuffsAdd(("Improved Scorch"));
            character.ActiveBuffsAdd(("Curse of the Elements"));
            character.ActiveBuffsAdd(("Misery"));
            character.ActiveBuffsAdd(("Blessing of Kings"));
            character.ActiveBuffsAdd(("Flask of Endless Rage"));
            character.ActiveBuffsAdd(("Fish Feast"));
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

        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the stats available to the Optimizer 
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Melee Avoid %",
					};
                return _optimizableCalculationLabels;
            }
        }
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
#if RAWR3
        private Dictionary<string, System.Windows.Media.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Colors.Red);
                }
                return _subPointNameColors;
            }
        }
#else
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(255, 255, 0, 0));
                }
                return _subPointNameColors;
            }
        }
#endif

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
                        "DPS Breakdown:Hand of Reckoning",
                        "DPS Breakdown:Other*From trinket procs",
                        "Rotation Info:Average SoV Stack",
                        "Rotation Info:SoV Overtake*How long you need to dps a target for SoV to do more DPS then SoR",
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
                    _customChartNames = new string[] { "Seals", "Weapon Speed" };
                }
                return _customChartNames;
            }
        }

#if RAWR3
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelRetribution()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
                {
                    ItemType.None,
                    ItemType.Leather,
                    ItemType.Mail,
                    ItemType.Plate,
                    ItemType.Libram,
                    ItemType.Polearm,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandMace,
                    ItemType.TwoHandSword
                }));
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Paladin; } }
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
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
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
            calc.OtherDPS = new MagicDamage(combats, stats.ArcaneDamage).AverageDamage()
                + new MagicDamage(combats, stats.FireDamage).AverageDamage()
                + new MagicDamage(combats, stats.ShadowDamage).AverageDamage();
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

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            if (character.Race == CharacterRace.Dwarf && character.MainHand != null &&
                    character.MainHand.Type == ItemType.TwoHandMace)
                statsRace.Expertise += 5f;
            if (character.Race == CharacterRace.Human && character.MainHand != null &&
                    (character.MainHand.Type == ItemType.TwoHandMace || character.MainHand.Type == ItemType.TwoHandSword))
                statsRace.Expertise += 3f;
            statsRace.Health -= 180f;
            statsRace.Mana -= 280f;

            Stats stats = statsBaseGear + statsBuffs + statsRace;
            
            stats.Expertise += (talents.GlyphOfSealOfVengeance && calcOpts.Seal == SealOf.Vengeance) ? 10f : 0;
            ConvertRatings(stats, talents, calcOpts.TargetLevel);

            if (computeAverageStats)
            {
                Rotation rot;

                CombatStats combats = new CombatStats(character, stats);
                if (calcOpts.SimulateRotation) rot = new Simulator(combats);
                else rot = new EffectiveCooldown(combats);

                Stats statsAverage = new Stats();
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {

                    float trigger = 0f; float procChance = 1f;
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
                    else if (effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DamageDone)
                    {
                        trigger = 1f / rot.GetPhysicalAttacksPerSec();
                    }
                    else if (effect.Trigger == Trigger.CrusaderStrikeHit)
                    {
                        trigger = rot.GetCrusaderStrikeCD();
                        procChance = rot.CS.ChanceToLand();
                    }
                    else if (effect.Trigger == Trigger.JudgementHit)
                    {
                        trigger = rot.GetJudgementCD();
                        procChance = rot.Judge.ChanceToLand();
                    }
                    else if (effect.Trigger == Trigger.SealOfVengeanceTick)
                    {
                        if (calcOpts.Seal == SealOf.Vengeance)
                        {
                            trigger = 3f;
                            procChance = 1f;
                        }
                        else continue;
                    }
                    else if (effect.Trigger == Trigger.Use)
                    {
                        trigger = 0f;
                        procChance = 1f;
                    }
                    else continue;
                    if (effect.MaxStack > 1) statsAverage += effect.Stats * effect.GetAverageStackSize(trigger, procChance,
                        combats.BaseWeaponSpeed, fightLength, calcOpts.StackTrinketReset);
                    else statsAverage += effect.GetAverageStats(trigger, procChance, combats.BaseWeaponSpeed, fightLength);

                    float chance = effect.GetAverageUptime(trigger, procChance, combats.BaseWeaponSpeed, fightLength);
                }

                stats = statsBaseGear + statsBuffs + statsRace + statsAverage;

                if (stats.Strength > stats.Agility)  stats.Strength += stats.HighestStat + stats.Paragon;
                else stats.Agility += stats.HighestStat + stats.Paragon;
                
                stats.Expertise += (talents.GlyphOfSealOfVengeance && calcOpts.Seal == SealOf.Vengeance) ? 10f : 0;

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

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + stats.HasteRating * 1.3f / 3278.998947f) * (1f + stats.Bloodlust) - 1f;
            
            stats.SpellPower += stats.Strength * .2f * talents.TouchedByTheLight + stats.AttackPower * talents.SheathOfLight * .1f;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Seals")
            {

                CalculationOptionsRetribution initOpts = character.CalculationOptions as CalculationOptionsRetribution;

                Character baseChar = character.Clone();
                CalculationOptionsRetribution baseOpts = initOpts.Clone();
                baseChar.CalculationOptions = baseOpts;
                baseOpts.Seal = SealOf.None;
                CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(baseChar);

                Character deltaChar = character.Clone();
                CalculationOptionsRetribution deltaOpts = initOpts.Clone();
                deltaChar.CalculationOptions = deltaOpts;

                ComparisonCalculationBase Command;
                deltaOpts.Seal = SealOf.Command;
                Command = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Command", initOpts.Seal == SealOf.Command);
                Command.Item = null;

                ComparisonCalculationBase Righteousness;
                deltaOpts.Seal = SealOf.Righteousness;
                Righteousness = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Righteousness", initOpts.Seal == SealOf.Righteousness);
                Righteousness.Item = null;

                ComparisonCalculationBase Vengeance;
                deltaOpts.Seal = SealOf.Vengeance;
                Vengeance = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Vengeance", initOpts.Seal == SealOf.Vengeance);
                Vengeance.Item = null;

                return new ComparisonCalculationBase[] { Command, Righteousness, Vengeance };
            }
            if (chartName == "Weapon Speed")
            {

                CalculationOptionsRetribution initOpts = character.CalculationOptions as CalculationOptionsRetribution;

                CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(character);

                Item newMH;
                float baseSpeed = character.MainHand.Speed;
                int minDamage = character.MainHand.MinDamage;
                int maxDamage = character.MainHand.MaxDamage;

                Character deltaChar = character.Clone();
                deltaChar.IsLoading = true;

                ComparisonCalculationBase three;
                newMH = character.MainHand.Item.Clone();
                newMH.MinDamage = (int)Math.Round(minDamage / baseSpeed * 3.3f);
                newMH.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * 3.3f);
                newMH.Speed = 3.3f;
                deltaChar.MainHand = new ItemInstance(newMH, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
                three = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "3.3 Speed", baseSpeed == newMH.Speed);
                three.Item = null;

                ComparisonCalculationBase four;
                newMH = character.MainHand.Item.Clone();
                newMH.MinDamage = (int)Math.Round(minDamage / baseSpeed * 3.4f);
                newMH.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * 3.4f);
                newMH.Speed = 3.4f;
                deltaChar.MainHand = new ItemInstance(newMH, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
                four = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "3.4 Speed", baseSpeed == newMH.Speed);
                four.Item = null;

                ComparisonCalculationBase five;
                newMH = character.MainHand.Item.Clone();
                newMH.MinDamage = (int)Math.Round(minDamage / baseSpeed * 3.5f);
                newMH.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * 3.5f);
                newMH.Speed = 3.5f;
                deltaChar.MainHand = new ItemInstance(newMH, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
                five = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "3.5 Speed", baseSpeed == newMH.Speed);
                five.Item = null;
                
                ComparisonCalculationBase six;
                newMH = character.MainHand.Item.Clone();
                newMH.MinDamage = (int)Math.Round(minDamage / baseSpeed * 3.6f);
                newMH.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * 3.6f);
                newMH.Speed = 3.6f;
                deltaChar.MainHand = new ItemInstance(newMH, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
                six = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "3.6 Speed", baseSpeed == newMH.Speed);
                six.Item = null;
                
                ComparisonCalculationBase seven;
                newMH = character.MainHand.Item.Clone();
                newMH.MinDamage = (int)Math.Round(minDamage / baseSpeed * 3.7f);
                newMH.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * 3.7f);
                newMH.Speed = 3.7f;
                deltaChar.MainHand = new ItemInstance(newMH, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
                seven = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "3.7 Speed", baseSpeed == newMH.Speed);
                seven.Item = null;

                
                return new ComparisonCalculationBase[] { three, four, five, six, seven };
            }
            else
            {
                return new ComparisonCalculationBase[0];
            }

        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand ||
            (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Libram))
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff)
        {
            Stats stats = buff.Stats;
            bool wantedStats = (stats.Strength + stats.Agility + stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.Bloodlust +
                stats.CrusaderStrikeDamage + stats.ConsecrationSpellPower + stats.JudgementCrit + stats.RighteousVengeanceCanCrit +
                stats.JudgementCDReduction + stats.DivineStormDamage + stats.DivineStormCrit + stats.Paragon +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit +
                stats.HammerOfWrathMultiplier + stats.SpellPower + stats.BonusIntellectMultiplier + stats.Intellect +
                stats.Health + stats.Stamina + stats.SpellCrit + stats.BonusCritMultiplier +
                stats.BonusSealOfCorruptionDamageMultiplier + stats.BonusSealOfRighteousnessDamageMultiplier +  stats.BonusSealOfVengeanceDamageMultiplier +
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
                Bloodlust = stats.Bloodlust,
                Expertise = stats.Expertise,
                Paragon = stats.Paragon,
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
                BonusSealOfCorruptionDamageMultiplier = stats.BonusSealOfCorruptionDamageMultiplier,
                BonusSealOfRighteousnessDamageMultiplier = stats.BonusSealOfRighteousnessDamageMultiplier,
                BonusSealOfVengeanceDamageMultiplier = stats.BonusSealOfVengeanceDamageMultiplier,
                CrusaderStrikeDamage = stats.CrusaderStrikeDamage,
                ConsecrationSpellPower = stats.ConsecrationSpellPower,
                JudgementCDReduction = stats.JudgementCDReduction,
                DivineStormDamage = stats.DivineStormDamage,
                DivineStormCrit = stats.DivineStormCrit,
                CrusaderStrikeCrit = stats.CrusaderStrikeCrit,
                ExorcismMultiplier = stats.ExorcismMultiplier,
                HammerOfWrathMultiplier = stats.HammerOfWrathMultiplier,
                CrusaderStrikeMultiplier = stats.CrusaderStrikeMultiplier,
                JudgementCrit = stats.JudgementCrit,
                RighteousVengeanceCanCrit = stats.RighteousVengeanceCanCrit,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantSpecialEffect(effect)) s.AddSpecialEffect(effect);
            }
            return s;
        }

        public bool HasRelevantSpecialEffect(SpecialEffect effect)
        {
            if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit|| effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.CrusaderStrikeHit || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.SealOfVengeanceTick
                || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.JudgementHit)
            {
                Stats stats = effect.Stats;
                return (stats.Strength + stats.Agility + stats.AttackPower + stats.CritRating
                    + stats.ArmorPenetrationRating + stats.Paragon + stats.HasteRating
                    + stats.ArcaneDamage + stats.HighestStat + stats.FireDamage + stats.ShadowDamage) > 0;
            }
            return false;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.PhysicalHaste + stats.PhysicalCrit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.Paragon +
                stats.BonusSealOfCorruptionDamageMultiplier + stats.BonusSealOfRighteousnessDamageMultiplier + stats.BonusSealOfVengeanceDamageMultiplier +
                stats.CrusaderStrikeDamage + stats.ConsecrationSpellPower + stats.JudgementCrit + stats.RighteousVengeanceCanCrit +
                stats.JudgementCDReduction + stats.DivineStormDamage + stats.DivineStormCrit + stats.BonusCritMultiplier +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit +
                stats.HammerOfWrathMultiplier + stats.Bloodlust) > 0;
            bool maybeStats = (stats.Agility + stats.Strength + stats.ExpertiseRating + stats.PhysicalHit +
                stats.HitRating + stats.CritRating + stats.HasteRating + stats.SpellHit + stats.SpellPower +
                stats.BonusStaminaMultiplier + stats.BonusSpellCritMultiplier) > 0;
            bool ignoreStats = (stats.Mp5 + stats.SpellPower + stats.DefenseRating +
                stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.BlockValue) > 0;
            bool specialEffect = false;
            bool hasSpecialEffect = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                hasSpecialEffect = true;
                specialEffect = false;
                if (HasRelevantSpecialEffect(effect))
                {
                    specialEffect = true;
                    break;
                }
            }
            return wantedStats || (specialEffect && !ignoreStats) || (maybeStats && !ignoreStats && (!hasSpecialEffect || specialEffect));
        }
    }
}
