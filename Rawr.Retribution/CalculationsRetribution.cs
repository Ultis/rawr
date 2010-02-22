using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Rawr.Retribution
{
    [Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_CrusaderStrike", CharacterClass.Paladin)]
    public class CalculationsRetribution : CalculationsBase
    {
        #region Model Properties
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

        /// <summary>
        /// Buffs that will be enabled by default in the given character object
        /// </summary>
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Strength of Earth Totem");
            character.ActiveBuffsAdd("Blessing of Might");
            character.ActiveBuffsAdd("Improved Blessing of Might");
            character.ActiveBuffsAdd("Unleashed Rage");
            character.ActiveBuffsAdd("Sanctified Retribution");
            character.ActiveBuffsAdd("Swift Retribution");
            character.ActiveBuffsAdd("Arcane Intellect");
            character.ActiveBuffsAdd("Commanding Shout");
            character.ActiveBuffsAdd("Leader of the Pack");
            character.ActiveBuffsAdd("Windfury Totem");
            character.ActiveBuffsAdd("Elemental Oath");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Improved Power Word: Fortitude");
            character.ActiveBuffsAdd("Mark of the Wild");
            character.ActiveBuffsAdd("Improved Mark of the Wild");
            character.ActiveBuffsAdd("Sunder Armor");
            character.ActiveBuffsAdd("Faerie Fire");
            character.ActiveBuffsAdd("Heart of the Crusader");
            character.ActiveBuffsAdd("Blood Frenzy");
            character.ActiveBuffsAdd("Improved Scorch");
            character.ActiveBuffsAdd("Curse of the Elements");
            character.ActiveBuffsAdd("Misery");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Totem of Wrath (Spell Power)");
            character.ActiveBuffsAdd("Blessing of Kings (Str/Sta Bonus)");
            character.ActiveBuffsAdd("Flask of Endless Rage");
            character.ActiveBuffsAdd("Fish Feast");
        }

        private static List<string> _relevantGlyphs;
        /// <summary>
        /// List of glyphs that will be available in the Glyph subtab of the Talents tab.
        /// </summary>
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
        /// Labels of the stats available to the Optimizer.
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
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Red);
                }
                return _subPointNameColors;
            }
        }
#endif

        /// <summary>
        /// Creates the CalculationOptionPanel
        /// </summary>
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
        /// 
        /// Values used here need to be defined via the GetCharacterDisplayCalculationValues() member
        /// in CharacterCalculationsRetribution.cs
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
                        "Basic Stats:Mana",
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
                        "Rotation Info:Chosen Rotation",
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
        #endregion

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

            Rotation rot = Rotation.Create(combats);

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

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            // Adjust expertise for racial passive and for using Seal of Vengeance combined with the SoV glyph.
            statsRace.Expertise += BaseStats.GetRacialExpertise(character, ItemSlot.MainHand);
            if (talents.GlyphOfSealOfVengeance && calcOpts.Seal == SealOf.Vengeance)
                statsRace.Expertise += 10f;

            // Combine stats
            Stats stats = statsBaseGear + statsBuffs + statsRace;
         
            // If wanted, Average out any Proc and OnUse effects into the stats
            if (computeAverageStats)
            {
                float fightLength = calcOpts.FightLength * 60f;
                CombatStats combats = new CombatStats(character, stats);
                Rotation rot = Rotation.Create(combats);

                // Average out proc effects, and add to global stats.
                Stats statsAverage = new Stats();
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    statsAverage.Accumulate(ProcessSpecialEffect(effect, rot, calcOpts.Seal, combats.BaseWeaponSpeed, fightLength, calcOpts.StackTrinketReset));
                }
                stats += statsAverage;

                // Death's Verdict/Vengeance (TOTC)
                // Known issue: We haven't yet accounted for bonus multipliers on str and agi
                if (stats.Strength > stats.Agility)
                    stats.Strength += stats.HighestStat + stats.Paragon;
                else
                    stats.Agility += stats.HighestStat + stats.Paragon;

                // Deathbringer's Will (ICC, Saurfang)
                // Paladins get str, hasterating and crit procs.  They're all equally likely so we'll divide the proc by 3
                stats.Strength += stats.DeathbringerProc / 3f;
                stats.HasteRating += stats.DeathbringerProc / 3f;
                stats.CritRating += stats.DeathbringerProc / 3f;
            }

            // ConvertRatings needs to be done AFTER accounting for the averaged stats, since stat multipliers 
            // should affect the averaged stats also.
            ConvertRatings(stats, talents, calcOpts.TargetLevel);

            return stats;
        }

        private Stats ProcessSpecialEffect(
            SpecialEffect effect, 
            Rotation rot, 
            SealOf seal, 
            float baseWeaponSpeed, 
            float fightLength, 
            int stackTrinketReset)
        {
            float trigger = 0f; 
            float procChance = 1f;

            switch (effect.Trigger)
            {
                case Trigger.MeleeCrit:
                    trigger = 1f / rot.GetMeleeCritsPerSec();
                    break;

                case Trigger.MeleeHit:
                    trigger = 1f / rot.GetMeleeAttacksPerSec();
                    break;

                case Trigger.PhysicalCrit:
                    trigger = 1f / rot.GetPhysicalCritsPerSec();
                    break;

                case Trigger.PhysicalHit:
                    trigger = 1f / rot.GetPhysicalAttacksPerSec();
                    break;

                case Trigger.DamageDone:
                    trigger = 1f / rot.GetAttacksPerSec();
                    break;

                case Trigger.DamageOrHealingDone:
                    // Need to add Self-heals
                    trigger = 1f / rot.GetAttacksPerSec();
                    break;

                case Trigger.CrusaderStrikeHit:
                    trigger = rot.GetCrusaderStrikeCD();
                    procChance = rot.CS.ChanceToLand();
                    break;

                case Trigger.JudgementHit:
                    trigger = rot.GetJudgementCD();
                    procChance = rot.Judge.ChanceToLand();
                    break;

                case Trigger.SealOfVengeanceTick:
                    if (seal == SealOf.Vengeance)
                    {
                        trigger = 3f;
                        procChance = 1f;
                    }
                    else
                    {
                        return new Stats();
                    }
                    break;

                case Trigger.Use:
                    trigger = 0f;
                    procChance = 1f;
                    break;

                default:
                    return new Stats();
            }

            if (effect.MaxStack > 1)
            {
                if (effect.Stats.MoteOfAnger > 0)
                {
                    // When in effect stats, MoteOfAnger is % of melee hits
                    // When in character stats, MoteOfAnger is average procs per second
                    return new Stats() { MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(trigger, procChance, baseWeaponSpeed, fightLength) / effect.MaxStack };
                }
                else
                {
                    Stats tempStats = null;
                    foreach (SpecialEffect subeffect in effect.Stats.SpecialEffects())
                    {
                        tempStats = ProcessSpecialEffect(subeffect, rot, seal, baseWeaponSpeed, effect.Duration, 0);
                    }

                    if (tempStats != null) return tempStats * effect.GetAverageStackSize(trigger, procChance, baseWeaponSpeed, fightLength, stackTrinketReset);
                    else return effect.Stats * effect.GetAverageStackSize(trigger, procChance, baseWeaponSpeed, fightLength, stackTrinketReset);
                }
            }
            else return effect.GetAverageStats(trigger, procChance, baseWeaponSpeed, fightLength);
        }

        // Combine talents and buffs into primary and secondary stats.
        // Convert ratings into their percentage values.
        private void ConvertRatings(Stats stats, PaladinTalents talents, int targetLevel)
        {
            // Primary stats
            stats.Strength *= (1 + stats.BonusStrengthMultiplier) * (1f + talents.DivineStrength * .03f);
            stats.Agility *= (1 + stats.BonusAgilityMultiplier);
            stats.Stamina *= (1 + stats.BonusStaminaMultiplier) * 
                (1f + talents.SacredDuty * .04f) * 
                (1f + talents.CombatExpertise * .02f);
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier) * (1f + talents.DivineIntellect * .02f);

            // Secondary stats
            // GetManaFromIntellect/GetHealthFromStamina account for the fact 
            // that the first 20 Int/Sta only give 1 Mana/Health each.
            stats.Mana += StatConversion.GetManaFromIntellect(stats.Intellect, CharacterClass.Paladin) * 
                (1f + stats.BonusManaMultiplier);
            stats.Health += StatConversion.GetHealthFromStamina(stats.Stamina, CharacterClass.Paladin);
            stats.AttackPower = (stats.AttackPower + stats.Strength * 2) * 
                (1 + stats.BonusAttackPowerMultiplier);

            // Combat ratings
            stats.Expertise += talents.CombatExpertise * 2 + 
                StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Paladin);
            stats.PhysicalHit += StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Paladin);
            stats.SpellHit += StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Paladin);

            float talentCrit = talents.CombatExpertise * .02f + talents.Conviction * .01f + talents.SanctityOfBattle * .01f;
            stats.PhysicalCrit += StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Paladin)
                                + StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Paladin)
                                + talentCrit
                                - StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel-80];    // Mob crit suppression
            stats.SpellCrit += stats.SpellCritOnTarget
                            + StatConversion.GetSpellCritFromRating(stats.CritRating, CharacterClass.Paladin)
                            + StatConversion.GetSpellCritFromIntellect(stats.Intellect, CharacterClass.Paladin)
                            + talentCrit
                            - StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80];    // Mob crit suppression

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Paladin)) * (1f + stats.Bloodlust) - 1f;
            stats.SpellHaste = (1f + stats.SpellHaste) * (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating, CharacterClass.Paladin)) * (1f + stats.Bloodlust) - 1f;

            stats.SpellPower += stats.Strength * talents.TouchedByTheLight * .2f 
                              + stats.AttackPower * talents.SheathOfLight * .1f;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsRetribution calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }
            // Removes the Hunter's Mark Buff and it's Children 'Glyphed', 'Improved' and 'Both' if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
                hasRelevantBuff =  character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
                }
            }
            /* [More Buffs to Come to this method]
             * Ferocious Inspiration | Sanctified Retribution
             * Hunting Party | Judgements of the Wise, Vampiric Touch, Improved Soul Leech, Enduring Winter
             * Acid Spit | Expose Armor, Sunder Armor (requires BM & Worm Pet)
             */
            #endregion

            #region Special Pot Handling
            /*foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }


        #region Relevancy Methods
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

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand ||
            (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Libram))
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            Stats stats = buff.Stats;
            bool wantedStats = (stats.Strength + stats.Agility + stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration +
                stats.ArmorPenetrationRating + stats.ExpertiseRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.PhysicalHit +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.Bloodlust + stats.MoteOfAnger +
                stats.CrusaderStrikeDamage + stats.ConsecrationSpellPower + stats.JudgementCrit + stats.RighteousVengeanceCanCrit +
                stats.JudgementCDReduction + stats.DivineStormDamage + stats.DivineStormCrit + stats.Paragon +
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit + stats.SpellCritOnTarget +
                stats.HammerOfWrathMultiplier + stats.SpellPower + stats.BonusIntellectMultiplier + stats.Intellect +
                stats.Health + stats.Stamina + stats.SpellCrit + stats.BonusCritMultiplier + stats.DeathbringerProc +
                stats.BonusSealOfCorruptionDamageMultiplier + stats.BonusSealOfRighteousnessDamageMultiplier +  stats.BonusSealOfVengeanceDamageMultiplier +
                stats.HitRating + stats.CritRating + stats.HasteRating + stats.SpellHit + stats.SpellPower +
                stats.SealMultiplier + stats.JudgementMultiplier + stats.DivineStormRefresh +
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
                SpellCritOnTarget = stats.SpellCritOnTarget,
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
                SealMultiplier = stats.SealMultiplier,
                JudgementMultiplier = stats.JudgementMultiplier,
                DivineStormRefresh = stats.DivineStormRefresh,
                DeathbringerProc = stats.DeathbringerProc,
                MoteOfAnger = stats.MoteOfAnger
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
                || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.DamageOrHealingDone || effect.Trigger == Trigger.JudgementHit)
            {
                Stats stats = effect.Stats;
                foreach (SpecialEffect subeffect in stats.SpecialEffects())
                {
                    if (HasRelevantSpecialEffect(subeffect)) return true;
                }
                return (stats.Strength + stats.Agility + stats.AttackPower + stats.CritRating + stats.MoteOfAnger
                    + stats.ArmorPenetrationRating + stats.Paragon + stats.HasteRating + stats.DeathbringerProc
                    + stats.ArcaneDamage + stats.HighestStat + stats.FireDamage + stats.ShadowDamage) > 0;
            }
            return false;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.AttackPower + stats.DivineStormMultiplier + stats.ArmorPenetration + stats.MoteOfAnger +
                stats.ArmorPenetrationRating + stats.PhysicalHaste + stats.PhysicalCrit + stats.DivineStormRefresh +
                stats.BonusStrengthMultiplier + stats.BonusAgilityMultiplier + stats.BonusDamageMultiplier + stats.BonusAttackPowerMultiplier +
                stats.BonusPhysicalDamageMultiplier + stats.BonusHolyDamageMultiplier + stats.Paragon + stats.DeathbringerProc +
                stats.BonusSealOfCorruptionDamageMultiplier + stats.BonusSealOfRighteousnessDamageMultiplier + stats.BonusSealOfVengeanceDamageMultiplier +
                stats.CrusaderStrikeDamage + stats.ConsecrationSpellPower + stats.JudgementCrit + stats.RighteousVengeanceCanCrit +
                stats.JudgementCDReduction + stats.DivineStormDamage + stats.DivineStormCrit + stats.BonusCritMultiplier + 
                stats.CrusaderStrikeCrit + stats.ExorcismMultiplier + stats.CrusaderStrikeMultiplier + stats.SpellCrit + stats.SpellCritOnTarget +
                stats.HammerOfWrathMultiplier + stats.Bloodlust + stats.SealMultiplier + stats.JudgementMultiplier) > 0;
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
        #endregion

        #region Custom Charts
        // Custom charts are extra charts which can be defined per model.
        // The charts are available via the "Slot" menu of the righthand Rawr chart panel.

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { "Seals", "Weapon Speed", "Rotations" };
                }
                return _customChartNames;
            }
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
            if (chartName == "Rotations")
            {
                List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                Character baseChar = character.Clone();
                CalculationOptionsRetribution baseOpts = ((CalculationOptionsRetribution)character.CalculationOptions).Clone();
                baseChar.CalculationOptions = baseOpts;

                Ability[] selectedRotation = 
                    ((CharacterCalculationsRetribution)Calculations.GetCharacterCalculations(character))
                        .Rotation;

                IEnumerable<Ability[]> rotations =
                    baseOpts.Experimental.IndexOf("<AllRotations>") == -1 ?
                    baseOpts.Rotations :
                    Rotation.GetAllRotations();

                foreach (Ability[] rotation in rotations)
                {
                    // Force this rotation rather than having the calculations try all and use the best one.
                    // We don't need to set this back to off, 
                    // since we're working in a cloned CalculationOptionsRetribution
                    baseOpts.ForceRotation = rotation; 
                                                
                    ComparisonCalculationBase compare = Calculations.GetCharacterComparisonCalculations(
                        Calculations.GetCharacterCalculations(baseChar),
                        RotationParameters.RotationString(rotation), 
                        Utilities.AreArraysEqual(rotation, selectedRotation));
                    compare.Item = null;
                    comparisons.Add(compare);
                }

                return comparisons.ToArray();
            }
            if (chartName == "Weapon Speed")
            {
                if (character.MainHand == null)
                    return new ComparisonCalculationBase[] { };

                return new ComparisonCalculationBase[] 
                { 
                    GetWeaponSpeedComparison(character, 3.3f),
                    GetWeaponSpeedComparison(character, 3.4f),
                    GetWeaponSpeedComparison(character, 3.5f),
                    GetWeaponSpeedComparison(character, 3.6f),
                    GetWeaponSpeedComparison(character, 3.7f)
                };
            }
            else
            {
                return new ComparisonCalculationBase[0];
            }

        }


        private ComparisonCalculationBase GetWeaponSpeedComparison(Character character, float speed)
        {
            Character adjustedCharacter = character.Clone();
            adjustedCharacter.IsLoading = true;
            adjustedCharacter.MainHand = new ItemInstance(
                AdjustWeaponSpeed(character.MainHand.Item, speed), 
                character.MainHand.Gem1, 
                character.MainHand.Gem2, 
                character.MainHand.Gem3, 
                character.MainHand.Enchant);

            var comparison = Calculations.GetCharacterComparisonCalculations(
                Calculations.GetCharacterCalculations(character), 
                adjustedCharacter, 
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:0.0} Speed", 
                    speed),
                character.MainHand.Item.Speed == speed);
            comparison.Item = null;

            return comparison;
        }

        private Item AdjustWeaponSpeed(Item weapon, float speed)
        {
            Item adjustedWeapon = weapon.Clone();
            adjustedWeapon.MinDamage = (int)Math.Round(weapon.MinDamage / weapon.Speed * speed);
            adjustedWeapon.MaxDamage = (int)Math.Round(weapon.MaxDamage / weapon.Speed * speed);
            adjustedWeapon.Speed = speed;

            return adjustedWeapon;
        }

        #endregion
    }
}
