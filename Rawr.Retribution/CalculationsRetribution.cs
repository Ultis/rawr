using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Retribution
{
    [Rawr.Calculations.RawrModelInfo("Retribution", "Spell_Holy_CrusaderStrike", CharacterClass.Paladin)]
    public class CalculationsRetribution : CalculationsBase
    {
        #region Model Properties
        #region DPSWarr Gemming Templates
        // Ok... I broke the templates when I was working on replacing them w/ the new Cata gems.
        // Stealing this from DPSwarr, and everything works.  THANK YOU DPSWarr folks.
        // Ideally, Rawr.Base should handle 0's in the template w/o the special work required.
        // Or at least so it doesn't cause a model to break.
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for DPSWarrs
                //               common uncommon rare  jewel |  fills in gaps if it can
                // Red slots
                int[] red_str = { 52081, 52206, 00000, 52255 }; fixArray(red_str);
                int[] red_exp = { 52085, 52230, 00000, 52260 }; fixArray(red_exp);
                int[] red_hit = { 00000, 00000, 00000, 00000 }; fixArray(red_hit);
                int[] red_mst = { 00000, 00000, 00000, 00000 }; fixArray(red_mst);
                int[] red_crt = { 00000, 00000, 00000, 00000 }; fixArray(red_crt);
                int[] red_has = { 00000, 00000, 00000, 00000 }; fixArray(red_has);
                // Orange slots
                int[] org_str = { 52114, 52240, 00000, 00000 }; fixArray(org_str);
                int[] org_exp = { 52118, 52224, 00000, 00000 }; fixArray(org_exp);
                int[] org_hit = { 00000, 00000, 00000, 00000 }; fixArray(org_hit);
                int[] org_mst = { 52114, 52240, 00000, 00000 }; fixArray(org_mst);
                int[] org_crt = { 52108, 52222, 00000, 00000 }; fixArray(org_crt);
                int[] org_has = { 52111, 52214, 00000, 00000 }; fixArray(org_has);
                // Yellow slots
                int[] ylw_str = { 00000, 00000, 00000, 00000 }; fixArray(ylw_str);
                int[] ylw_exp = { 00000, 00000, 00000, 00000 }; fixArray(ylw_exp);
                int[] ylw_hit = { 00000, 00000, 00000, 00000 }; fixArray(ylw_hit);
                int[] ylw_mst = { 52094, 52219, 00000, 52269 }; fixArray(ylw_mst);
                int[] ylw_crt = { 52091, 52241, 00000, 52266 }; fixArray(ylw_crt);
                int[] ylw_has = { 52093, 52232, 00000, 52268 }; fixArray(ylw_has);
                // Green slots
                int[] grn_str = { 00000, 00000, 00000, 00000 }; fixArray(grn_str);
                int[] grn_exp = { 00000, 00000, 00000, 00000 }; fixArray(grn_exp);
                int[] grn_hit = { 52128, 52237, 00000, 00000 }; fixArray(grn_hit);
                int[] grn_mst = { 52126, 52231, 00000, 00000 }; fixArray(grn_mst);
                int[] grn_crt = { 52121, 52223, 00000, 00000 }; fixArray(grn_crt);
                int[] grn_has = { 52124, 52218, 00000, 00000 }; fixArray(grn_has);
                // Blue slots
                int[] blu_str = { 00000, 00000, 00000, 00000 }; fixArray(blu_str);
                int[] blu_exp = { 00000, 00000, 00000, 00000 }; fixArray(blu_exp);
                int[] blu_hit = { 52089, 52235, 00000, 52264 }; fixArray(blu_hit);
                int[] blu_mst = { 00000, 00000, 00000, 00000 }; fixArray(blu_mst);
                int[] blu_crt = { 00000, 00000, 00000, 00000 }; fixArray(blu_crt);
                int[] blu_has = { 00000, 00000, 00000, 00000 }; fixArray(blu_has);
                // Purple slots
                int[] ppl_str = { 52095, 52243, 00000, 00000 }; fixArray(ppl_str);
                int[] ppl_exp = { 52105, 52203, 00000, 00000 }; fixArray(ppl_exp);
                int[] ppl_hit = { 52101, 52213, 00000, 00000 }; fixArray(ppl_hit);
                int[] ppl_mst = { 00000, 00000, 00000, 00000 }; fixArray(ppl_mst);
                int[] ppl_crt = { 00000, 00000, 00000, 00000 }; fixArray(ppl_crt);
                int[] ppl_has = { 00000, 00000, 00000, 00000 }; fixArray(ppl_has);
                // Cogwheels
                int[] cog_str = { 00000, 00000, 00000, 00000 }; fixArray(cog_str);
                int[] cog_exp = { 59489, 59489, 00000, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 00000, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 00000, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 00000, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 00000, 59479 }; fixArray(cog_has);

                string group; bool enabled;
                List<GemmingTemplate> templates = new List<GemmingTemplate>();

                #region Strength
                enabled = true;
                group = "Strength";
                // Straight
                AddTemplates(templates,
                    red_str, red_str, red_str,
                    red_str, red_str, red_str,
                    red_str, cog_mst, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_str, ylw_str, blu_str,
                    org_str, ppl_str, grn_str,
                    red_str, cog_mst, group, enabled);
                #endregion

                #region Expertise
                group = "Expertise";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    red_exp, red_exp, red_exp,
                    red_exp, red_exp, red_exp,
                    red_exp, cog_exp, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_exp, ylw_exp, blu_exp,
                    org_exp, ppl_exp, grn_exp,
                    red_exp, cog_exp, group, enabled);
                #endregion

                #region Hit
                group = "Hit";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    blu_hit, blu_hit, blu_hit,
                    blu_hit, blu_hit, blu_hit,
                    blu_hit, cog_hit, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_hit, ylw_hit, blu_hit,
                    org_hit, ppl_hit, grn_hit,
                    blu_hit, cog_hit, group, enabled);
                #endregion

                #region Mastery
                enabled = true;
                group = "Mastery";
                // Straight
                AddTemplates(templates,
                    ylw_mst, ylw_mst, ylw_mst,
                    ylw_mst, ylw_mst, ylw_mst,
                    ylw_mst, cog_mst, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_mst, ylw_mst, blu_mst,
                    org_mst, ppl_mst, grn_mst,
                    ylw_mst, cog_mst, group, enabled);
                #endregion

                #region Crit
                group = "Crit";
                enabled = false;
                // Straight
                AddTemplates(templates,
                    ylw_crt, ylw_crt, ylw_crt,
                    ylw_crt, ylw_crt, ylw_crt,
                    ylw_crt, cog_crt, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_crt, ylw_crt, blu_crt,
                    org_crt, ppl_crt, grn_crt,
                    red_crt, cog_crt, group, enabled);
                #endregion

                #region Haste
                group = "Haste";
                enabled = false;
                // Straight
                AddTemplates(templates,
                    ylw_has, ylw_has, ylw_has,
                    ylw_has, ylw_has, ylw_has,
                    ylw_has, cog_has, group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_has, ylw_has, blu_has,
                    org_has, ppl_has, grn_has,
                    red_has, cog_has, group, enabled);
                #endregion
                return templates;
            }
        }
        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green, but no Blue
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue (or Green as set above), but no Purple
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple (or Blue/Green as set above), but no Jewel
        }
        private static void AddTemplates(List<GemmingTemplate> templates, int[] red, int[] ylw, int[] blu, int[] org, int[] prp, int[] grn, int[] pris, int[] cog, string group, bool enabled)
        {
            const int chaotic = 52291; // Meta
            const string groupFormat = "{0} {1}";
            string[] quality = new string[] { "Uncommon", "Rare", "Epic", "Jewelcrafter" };
            for (int j = 0; j < 4; j++)
            {
                // Check to make sure we're not adding the same gem template twice due to repeating JC gems
                if (j != 3 || !(red[j] == red[j - 1] && blu[j] == blu[j - 1] && ylw[j] == ylw[j - 1]))
                {
                    string groupStr = String.Format(groupFormat, quality[j], group);
                    templates.Add(new GemmingTemplate()
                    {
                        Model = "Retribution",
                        Group = groupStr,
                        RedId = red[j] != 0 ? red[j] : org[j] != 0 ? org[j] : prp[j],
                        YellowId = ylw[j] != 0 ? ylw[j] : org[j] != 0 ? org[j] : grn[j],
                        BlueId = blu[j] != 0 ? blu[j] : prp[j] != 0 ? prp[j] : grn[j],
                        PrismaticId = red[j] != 0 ? red[j] : ylw[j] != 0 ? ylw[j] : blu[j],
                        CogwheelId = cog[j],
                        HydraulicId = 0,
                        MetaId = chaotic,
                        Enabled = (enabled && j == 1)
                    });
                }
            }
        }
        #endregion

        /// <summary>
        /// Buffs that will be enabled by default in the given character object
        /// </summary>
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Horn of Winter");
            character.ActiveBuffsAdd("Blessing of Might");
            character.ActiveBuffsAdd("Elemental Oath");
            character.ActiveBuffsAdd("Arcane Tactics");
            character.ActiveBuffsAdd("Improved Icy Talons");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Arcane Brilliance");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Sunder Armor");
            character.ActiveBuffsAdd("Blood Frenzy");
            character.ActiveBuffsAdd("Shadow and Flame");
            character.ActiveBuffsAdd("Curse of the Elements");
            character.ActiveBuffsAdd("Strength Food");
            character.ActiveBuffsAdd("Heroism/Bloodlust");

            if (character.PrimaryProfession == Profession.Alchemy || character.SecondaryProfession == Profession.Alchemy)
                character.ActiveBuffsAdd("Flask of Endless Rage (Mixology)");
            else
                character.ActiveBuffsAdd("Flask of Endless Rage");
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
                _relevantGlyphs.Add("Glyph of Crusader Strike");
                _relevantGlyphs.Add("Glyph of Exorcism");
                _relevantGlyphs.Add("Glyph of Judgement");
                _relevantGlyphs.Add("Glyph of Seal of Truth");
                _relevantGlyphs.Add("Glyph of Templar's Verdict");
                _relevantGlyphs.Add("Glyph of Consecration");
                _relevantGlyphs.Add("Glyph of Hammer of Wrath");
                _relevantGlyphs.Add("Glyph of Rebuke");
                _relevantGlyphs.Add("Glyph of Ascetic Crusader");
            }
            return _relevantGlyphs;
        }

        private string[] _optimizableCalculationLabels = null;
        /// <summary>
        /// Labels of the stats available to the Optimizer.
        /// </summary>
        /// The list of labels listed here needs to match with the list in GetOptimizableCalculationValue override in CharacterCalculationsRetribution.cs
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Health",
                    "% Chance to Miss (Melee)",
                    "% Chance to Miss (Spells)",
                    "% Chance to be Dodged",
                    "% Chance to be Parried",
                    "% Chance to be Avoided (Melee/Dodge)",
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

        /// <summary>
        /// Creates the CalculationOptionPanel
        /// </summary>
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
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
                        "Basic Stats:Mastery",
                        "Basic Stats:Melee Haste",
                        "Basic Stats:Weapon Damage",
                        "Basic Stats:Attack Speed",
                        "DPS Breakdown:Total DPS",
                        "DPS Breakdown:White",
                        "DPS Breakdown:Seal",
                        "DPS Breakdown:Seal (Dot)",
                        "DPS Breakdown:Seal of Command",
                        "DPS Breakdown:Crusader Strike",
                        "DPS Breakdown:Templars Verdict",
                        "DPS Breakdown:Hand of Light", 
                        "DPS Breakdown:Exorcism",
                        "DPS Breakdown:Hammer of Wrath",
                        "DPS Breakdown:Holy Wrath",
                        "DPS Breakdown:Judgement",
                        "DPS Breakdown:Consecration",
                        "DPS Breakdown:Other*From trinket procs",
                        //"Rotation Info:Average SoT Stack",
                        "Rotation Info:Inqusition Uptime",
                        "Rotation Info:Crusader Strike Usage",
                        "Rotation Info:Templar's Verdict Usage",
                        "Rotation Info:Exorcism Usage",
                        "Rotation Info:Hammer of Wrath Usage",
                        "Rotation Info:Judgement Usage",
                        "Rotation Info:Holy Wrath Usage",
                        "Rotation Info:Consecration Usage",
                        
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
        public override CharacterCalculationsBase GetCharacterCalculations(
            Character character, 
            Item additionalItem, 
            bool referenceCalculation, 
            bool significantChange, 
            bool needsDisplayCalculations)
        {
            return GetCharacterCalculations(
                character,
                additionalItem,
                GetCharacterRotation(character, additionalItem));
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, RotationCalculation rot)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsRetribution calc = new CharacterCalculationsRetribution();
            if (character == null) { return calc; }
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            if (calcOpts == null) { return calc; }
            if (rot == null) { return calc; }
            CombatStats combats = rot.Combats;
            Stats stats = combats.Stats;

            calc.Combatstats = combats;
            calc.BasicStats = GetCharacterStats(character, additionalItem, false);
            calc.OtherDPS = new MagicDamage(combats, stats.ArcaneDamage).AverageDamage()
                          + new MagicDamage(combats, stats.FireDamage).AverageDamage()
                          + new MagicDamage(combats, stats.ShadowDamage).AverageDamage()
                          + new MagicDamage(combats, stats.FrostDamage).AverageDamage()
                          + new MagicDamage(combats, stats.NatureDamage).AverageDamage()
                          + new MagicDamage(combats, stats.HolyDamage).AverageDamage();
            rot.SetDPS(calc);
            calc.OverallPoints = calc.DPSPoints;

            return calc;
        }

        public RotationCalculation GetCharacterRotation(Character character, Item additionalItem)
        {
            // First things first, we need to ensure that we aren't using bad data
            if (character == null) { return null; }
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            if (calcOpts == null) { return null; }
            //
            return CreateRotation(
                new CombatStats(
                    character,
                    GetCharacterStats(character, additionalItem, true)));
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
            return GetCharacterRotation(character, additionalItem).Combats.Stats;
        }

        public RotationCalculation CreateRotation(CombatStats combats)
        {
            return new RotationCalculation(combats);
        }

        public Stats GetCharacterStats(
            Character character, 
            Item additionalItem, 
            bool computeAverageStats)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Paladin, character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            // Adjust expertise for racial passive
            statsRace.Expertise += BaseStats.GetRacialExpertise(character, ItemSlot.MainHand);

            // Combine stats
            Stats stats = statsBaseGear + statsBuffs + statsRace;
            // If wanted, Average out any Proc and OnUse effects into the stats
            if (computeAverageStats)
            {
                Stats statsTmp = stats.Clone();
                ConvertRatings(statsTmp, talents, calcOpts.TargetLevel, character.Level);                // Convert ratings so we have right value for haste, weaponspeed and talents etc.

                float fightLength = calcOpts.FightLength * 60f;
                CombatStats combats = new CombatStats(character, statsTmp);
                RotationCalculation rot = CreateRotation(combats);

                // Average out proc effects, and add to global stats.
                Stats statsAverage = new Stats();
                foreach (SpecialEffect effect in stats.SpecialEffects())
                    statsAverage.Accumulate(
                        ProcessSpecialEffect(
                            effect, 
                            rot, 
                            calcOpts.Seal, 
                            combats.BaseWeaponSpeed, 
                            fightLength));
                stats.Accumulate(statsAverage);
            }

            // No negative values (from possible charts)
            if (stats.Strength < 0)
                stats.Strength = 0;
            if (stats.Agility < 0)
                stats.Agility = 0;
            if (stats.AttackPower < 0)
                stats.AttackPower = 0;
            if (stats.ExpertiseRating < 0)
                stats.ExpertiseRating = 0;
            if (stats.HitRating < 0)
                stats.HitRating = 0;
            if (stats.CritRating < 0)
                stats.CritRating = 0;
            if (stats.HasteRating < 0)
                stats.HasteRating = 0;
            if (stats.SpellPower < 0)
                stats.SpellPower = 0;
            if (stats.MasteryRating < 0)
                stats.MasteryRating = 0;
        
            // ConvertRatings needs to be done AFTER accounting for the averaged stats, since stat multipliers 
            // should affect the averaged stats also.
            ConvertRatings(stats, talents, calcOpts.TargetLevel, character.Level);

            return stats;
        }

        private Stats ProcessSpecialEffect(
            SpecialEffect effect,
            RotationCalculation rot, 
            SealOf seal, 
            float baseWeaponSpeed, 
            float fightLength)
        {
            float trigger = 0f; 
            float procChance = 1f;

            switch (effect.Trigger)
            {
                case Trigger.MeleeCrit:
                    trigger = (float) (1f / rot.GetMeleeCritsPerSec());
                    break;

                case Trigger.MeleeHit:
                    trigger = (float)(1f / rot.GetMeleeAttacksPerSec());
                    break;

                case Trigger.MeleeAttack:   // [Tiny Abomination in a Jar] and [Shadowmourne]
                    double MeleeAttackPerSec =  rot.GetMeleeAttacksPerSec() +                   // Meleehit
                                                rot.SealProcsPerSec(rot.Seal) +                 // Seal hit
                                                rot.GetAbilityHitsPerSecond(rot.Judge) +
                                                rot.SealDotProcPerSec(rot.Seal);         // Judgement debuf application (100% on J Hit)
                    trigger = (float) (1f / MeleeAttackPerSec);
                    break;

                case Trigger.PhysicalCrit:
                    trigger = (float) (1f / rot.GetPhysicalCritsPerSec());
                    break;

                case Trigger.PhysicalHit:
                    trigger = (float) (1f / rot.GetPhysicalAttacksPerSec());
                    break;

                case Trigger.DamageDone:
                    trigger = (float) (1f / rot.GetAttacksPerSec());
                    break;

                case Trigger.SpellHit:                  // This technically counts heals also, but we aren't modelling ret doing heals
                case Trigger.DamageSpellHit:
                    trigger = (float) (1f / rot.GetSpellAttacksPerSec());
                    break;

                case Trigger.SpellCrit:                 // This technically counts heals also, but we aren't modelling ret doing heals
                case Trigger.DamageSpellCrit:
                    trigger = (float) (1f / rot.GetSpellCritsPerSec()); 
                    break;

                case Trigger.DamageOrHealingDone:
                    // Need to add Self-heals
                    trigger = (float) (1f / rot.GetAttacksPerSec());
                    break;

                case Trigger.CrusaderStrikeHit:
                    trigger = rot.CS.GetCooldown();
                    procChance = rot.CS.ChanceToLand();
                    break;

                case Trigger.JudgementHit:
                    trigger = rot.Judge.GetCooldown();
                    procChance = rot.Judge.ChanceToLand();
                    break;

                case Trigger.Use:
                    trigger = 0f;
                    procChance = 1f;
                    break;

                default:
                    return new Stats();
            }

            if (effect.Trigger == Trigger.Use && effect.Stats._rawSpecialEffectData != null)
            {
                // Run Recursive Effects (like Victor's Call)
                Stats SubStats = ProcessSpecialEffect(effect.Stats._rawSpecialEffectData[0],rot,seal,baseWeaponSpeed,fightLength);
                float upTime = effect.GetAverageUptime(trigger,procChance);
                Stats retVal = new Stats();
                retVal.Accumulate(SubStats, upTime);
                return retVal;
            }
            else if (effect.MaxStack > 1)
            {
                Stats tempStats = null;
                foreach (SpecialEffect subeffect in effect.Stats.SpecialEffects())
                {
                    tempStats = ProcessSpecialEffect(subeffect, rot, seal, baseWeaponSpeed, effect.Duration);
                }
                if (tempStats != null) 
                    return tempStats * effect.GetAverageStackSize(trigger, procChance, baseWeaponSpeed, fightLength);
                else 
                    return effect.Stats * effect.GetAverageStackSize(trigger, procChance, baseWeaponSpeed, fightLength);
            }
            else return effect.GetAverageStats(trigger, procChance, baseWeaponSpeed, fightLength);
        }

        // Combine talents and buffs into primary and secondary stats.
        // Convert ratings into their percentage values.
        private void ConvertRatings(Stats stats, PaladinTalents talents, int targetLevel, int charlevel)
        {
            // Primary stats
            stats.Strength += stats.HighestStat;
            stats.Strength *= (1 + stats.BonusStrengthMultiplier);
            stats.Agility *= (1 + stats.BonusAgilityMultiplier);
            stats.Stamina *= (1 + stats.BonusStaminaMultiplier);
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier);

            // Secondary stats
            // GetManaFromIntellect/GetHealthFromStamina account for the fact 
            // that the first 20 Int/Sta only give 1 Mana/Health each.
            stats.Mana += StatConversion.GetManaFromIntellect(stats.Intellect, CharacterClass.Paladin) * (1f + stats.BonusManaMultiplier);
            stats.Health += StatConversion.GetHealthFromStamina(stats.Stamina, CharacterClass.Paladin);
            stats.AttackPower = (stats.AttackPower + stats.Strength * 2) * (1 + stats.BonusAttackPowerMultiplier);

            // Combat ratings
            if (stats.HighestSecondaryStat > 0)
            {
                if (stats.CritRating > stats.MasteryRating)
                    if (stats.HasteRating > stats.CritRating)
                        stats.HasteRating += stats.HighestSecondaryStat;
                    else
                        stats.CritRating += stats.HighestSecondaryStat;
                else
                    if (stats.HasteRating > stats.MasteryRating)
                        stats.HasteRating += stats.HighestSecondaryStat;
                    else
                        stats.MasteryRating += stats.HighestSecondaryStat;
            }
            stats.Expertise += (talents.GlyphOfSealOfTruth ? 10f : 0) + StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Paladin);
            stats.PhysicalHit += StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Paladin);
            stats.SpellHit += StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Paladin) + .08f;

            stats.PhysicalCrit += 
                StatConversion.GetPhysicalCritFromRating(stats.CritRating, CharacterClass.Paladin) + 
                StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.Paladin) + 
                StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - charlevel]; // Mob crit suppression
            stats.SpellCrit += stats.SpellCritOnTarget + 
                StatConversion.GetSpellCritFromRating(stats.CritRating, CharacterClass.Paladin) + 
                StatConversion.GetSpellCritFromIntellect(stats.Intellect, CharacterClass.Paladin) + 
                StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - charlevel]; // Mob crit suppression

            stats.PhysicalHaste = (1f + stats.PhysicalHaste) * (1f + StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Paladin)) - 1f;
            stats.SpellHaste = (1f + stats.SpellHaste) * (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating, CharacterClass.Paladin)) - 1f;
            
            stats.BonusDamageMultiplier += PaladinConstants.TWO_H_SPEC + 
                                           talents.Communion * .02f;

            stats.SpellPower += stats.AttackPower * .3f;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsRetribution calcOpts) 
        {
            List<Buff> buffs = new List<Buff>(character.ActiveBuffs);

            var buffStats = GetBuffsStats(buffs);
            return buffStats;
        }

        #region Relevancy Methods
        /// <summary>
        /// List of itemtypes that are relevant for retribution
        /// </summary>
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[]
                {
                    ItemType.None,
                    ItemType.Plate,
                    ItemType.Libram,ItemType.Relic,
                    ItemType.Polearm,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandMace,
                    ItemType.TwoHandSword
                }));
            }
        }

        /// <summary>
        /// List of SpecialEffect Triggers that are relevant for retribution model
        /// Ever trigger listed here needs an implementation in ProcessSpecialEffects()
        /// A trigger not listed here should not appear in ProcessSpecialEffects()
        /// </summary>
        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers
        {
            get
            {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                            Trigger.Use, 
                            Trigger.SpellCrit,        
                            Trigger.SpellHit,             
                            Trigger.DamageSpellCrit,        
                            Trigger.DamageSpellHit,        
                            Trigger.PhysicalCrit,
                            Trigger.PhysicalHit,
                            Trigger.MeleeCrit,
                            Trigger.MeleeHit,
                            Trigger.MeleeAttack,
                            Trigger.DamageDone,
                            Trigger.DamageOrHealingDone, 
                            Trigger.DoTTick,
                            Trigger.JudgementHit,
                            Trigger.CrusaderStrikeHit,
                        });
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            // First we let normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsItemRelevant(item);

            // Next we use our special stat relevancy filtering.
            if (relevant)
                relevant = HasPrimaryStats(item.Stats) || (HasSecondaryStats(item.Stats) && !HasUnwantedStats(item.Stats));

            return relevant;
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            // First we let normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsBuffRelevant(buff, character);

            // Temporary FIX (?): buf.AllowedClasses is not currently being tested as part of base.IsBuffRelevant(). So we'll do it ourselves.
            if (relevant && !buff.AllowedClasses.Contains(CharacterClass.Paladin))
                relevant = false;

            // Next we use our special stat relevancy filtering on consumables. (party buffs only need filtering on relevant stats)
            if (relevant && (buff.Group == "Elixirs and Flasks" || buff.Group == "Potion" || buff.Group == "Food" || buff.Group == "Scrolls" || buff.Group == "Temporary Buffs"))
                relevant = HasPrimaryStats(buff.Stats) || (HasSecondaryStats(buff.Stats) && !HasUnwantedStats(buff.Stats));

            // Remove bloodlust, we have our own processing for it.
            if (relevant && buff.Name == "Heroism/Bloodlust")
                relevant = false;

            return relevant;
        }

        public override bool IsEnchantRelevant(Enchant enchant, Character character)
        {
            // First we let the normal rules (profession, class, relevant stats) decide
            bool relevant = base.IsEnchantRelevant(enchant, character);

            // Next we use our special stat relevancy filtering.
            if (relevant)
                relevant = HasPrimaryStats(enchant.Stats) || (HasSecondaryStats(enchant.Stats) && !HasUnwantedStats(enchant.Stats));

            return relevant;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Health = stats.Health,
                Mana = stats.Mana,
                Strength = stats.Strength,
                HighestStat = stats.HighestStat,
                Agility = stats.Agility,
                Intellect = stats.Intellect,
                Stamina = stats.Stamina,
                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                ArmorPenetration = stats.ArmorPenetration,
                TargetArmorReduction = stats.TargetArmorReduction,
                ExpertiseRating = stats.ExpertiseRating,
                HasteRating = stats.HasteRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                Expertise = stats.Expertise,
                MasteryRating = stats.MasteryRating,
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
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                DivineStormMultiplier = stats.DivineStormMultiplier,
                BonusSealOfRighteousnessDamageMultiplier = stats.BonusSealOfRighteousnessDamageMultiplier,
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
                SealMultiplier = stats.SealMultiplier,
                JudgementMultiplier = stats.JudgementMultiplier,
                DivineStormRefresh = stats.DivineStormRefresh,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                ArcaneDamage = stats.ArcaneDamage,
                ShadowDamage = stats.ShadowDamage,
                NatureDamage = stats.NatureDamage,
                HolyDamage = stats.HolyDamage,
                MovementSpeed = stats.MovementSpeed,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                    s.AddSpecialEffect(effect);
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return HasPrimaryStats(stats) || HasSecondaryStats(stats) || HasExtraStats(stats);
        }

        /// <summary>
        /// HasPrimaryStats() should return true if the Stats object has any stats that define the item
        /// as being 'for your class/spec'. For melee classes this is typical melee stats like Strength, 
        /// Agility, AP, Expertise... For casters it would be spellpower, intellect, ...
        /// As soon as an item/enchant/buff has any of the stats listed here, it will be assumed to be 
        /// relevant unless explicitely filtered out.
        /// Stats that could be usefull for both casters and melee such as HitRating, CritRating and Haste
        /// don't belong here, but are SecondaryStats. Specific melee versions of these do belong here 
        /// for melee, spell versions would fit here for casters.
        /// </summary>
        public bool HasPrimaryStats(Stats stats)
        {
            bool PrimaryStats = // Base stats
                                stats.Strength != 0 ||
                                stats.HighestStat != 0 ||
                                stats.Agility != 0 ||
                                stats.AttackPower != 0 ||
                                stats.ArmorPenetration != 0 ||
                                stats.TargetArmorReduction != 0 ||
                                stats.Expertise != 0 ||
                // Combat ratings
                                stats.ExpertiseRating != 0 ||
                                stats.PhysicalHit != 0 ||
                                stats.PhysicalCrit != 0 ||
                                stats.PhysicalHaste != 0 ||
                                stats.MasteryRating != 0 ||
                // Stat and damage multipliers
                                stats.BonusStrengthMultiplier != 0 ||
                                stats.BonusAgilityMultiplier != 0 ||
                                stats.BonusAttackPowerMultiplier != 0 ||
                                stats.BonusPhysicalDamageMultiplier != 0 ||
                                stats.BonusHolyDamageMultiplier != 0 ||
                                stats.BonusDamageMultiplier != 0 ||
                                stats.BonusWhiteDamageMultiplier != 0 ||
                // Paladin specific stats (set bonusses)
                                stats.DivineStormMultiplier != 0 ||
                                stats.BonusSealOfRighteousnessDamageMultiplier != 0 ||
                                stats.CrusaderStrikeDamage != 0 ||
                                stats.ConsecrationSpellPower != 0 ||
                                stats.JudgementCDReduction != 0 ||
                                stats.DivineStormDamage != 0 ||
                                stats.DivineStormCrit != 0 ||
                                stats.CrusaderStrikeCrit != 0 ||
                                stats.ExorcismMultiplier != 0 ||
                                stats.HammerOfWrathMultiplier != 0 ||
                                stats.CrusaderStrikeMultiplier != 0 ||
                                stats.JudgementCrit != 0 ||
                                stats.SealMultiplier != 0 ||
                                stats.JudgementMultiplier != 0 ||
                                stats.DivineStormRefresh != 0;
                                // Item proc effects
            if (!PrimaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasPrimaryStats(effect.Stats))
                    {
                        PrimaryStats = true;
                        break;
                    }
                }
            }
            return PrimaryStats;
        }

        /// <summary>
        /// HasSecondaryStats() should return true if the Stats object has any stats that are relevant for the 
        /// model but only to a smaller degree, so small that you wouldn't typically consider the item.
        /// Stats that are usefull to both melee and casters (HitRating, CritRating & Haste) fit in here also.
        /// An item/enchant/buff having these stats would be considered only if it doesn't have any of the 
        /// unwanted stats.  Group/Party buffs are slighly different, they would be considered regardless if 
        /// they have unwanted stats.
        /// Note that a stat may be listed here since it impacts the model, but may also be listed as an unwanted stat.
        /// </summary>
        public bool HasSecondaryStats(Stats stats)
        {
            bool SecondaryStats = // Caster stats
                                  stats.Intellect != 0 ||                // Intellect increases spellcrit, so it contributes to DPS.
                                  stats.SpellCrit != 0 ||                // Exorcism can crit
                                  stats.SpellCritOnTarget != 0 ||        // Exorcism
                                  stats.SpellHit != 0 ||                 // Exorcism & Consecration (1st tick)
                                  stats.SpellPower != 0 ||               // All holy damage effects benefit from spellpower
                                  stats.BonusIntellectMultiplier != 0 || // See intellect
                                  stats.BonusSpellCritMultiplier != 0 || // See spellcrit
                                  // Generic DPS stats, useful for casters and melee.
                                  stats.HitRating != 0 ||
                                  stats.CritRating != 0 ||
                                  stats.HasteRating != 0 ||
                                  stats.BonusCritMultiplier != 0 ||
                                  // Damage procs
                                  stats.FireDamage != 0 ||
                                  stats.FrostDamage != 0 ||
                                  stats.ArcaneDamage != 0 ||
                                  stats.ShadowDamage != 0 ||
                                  stats.NatureDamage != 0 ||
                                  stats.HolyDamage != 0 ||
                                  // Special (unmodelled)
                                  stats.MovementSpeed != 0 ||
                                  stats.SnareRootDurReduc != 0 ||
                                  stats.FearDurReduc != 0 ||
                                  stats.StunDurReduc != 0;
            if (!SecondaryStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasSecondaryStats(effect.Stats))
                    {
                        SecondaryStats = true;
                        break;
                    }
                }
            }
            return SecondaryStats;
        }

        /// <summary>
        /// Return true if the Stats object has any stats that don't influence the model but that you do want 
        /// to display in tooltips and in calculated summary values.
        /// </summary>
        public bool HasExtraStats(Stats stats)
        {
            bool ExtraStats =   stats.Health != 0 ||
                                stats.Mana != 0 ||
                                stats.Stamina != 0 ||
                                stats.BonusStaminaMultiplier != 0;
            if (!ExtraStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger) && HasExtraStats(effect.Stats))
                    {
                        ExtraStats = true;
                        break;
                    }
                }
            }
            return ExtraStats;
        }

        /// <summary>
        /// Return true if the Stats object contains any stats that are making the item undesired.
        /// Any item having only Secondary stats would be removed if it also has one of these.
        /// </summary>
        public bool HasUnwantedStats(Stats stats)
        {
            /// List of stats that will filter out some buffs (Flasks, Elixirs & Scrolls), Enchants and Items.
            bool UnwantedStats = stats.SpellPower != 0 ||
                                 stats.Intellect != 0 ||
                                 stats.Spirit != 0 ||
                                 stats.Mp5 != 0 ||
                                 stats.ParryRating != 0 ||
                                 stats.DodgeRating != 0 ||
                                 stats.BlockRating != 0;
            if (!UnwantedStats)
            {
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (/*RelevantTriggers.Contains(effect.Trigger) && */HasUnwantedStats(effect.Stats))    // An unwanted stat could be behind a trigger we don't model.
                    {
                        UnwantedStats = true;
                        break;
                    }
                }
            }
            return UnwantedStats;
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
                    _customChartNames = new string[] 
                    { 
                        "Seals", 
                        "Weapon Speed"
                    };
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

                ComparisonCalculationBase Righteousness;
                deltaOpts.Seal = SealOf.Righteousness;
                Righteousness = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Righteousness", initOpts.Seal == SealOf.Righteousness);
                Righteousness.Item = null;

                ComparisonCalculationBase Truth;
                deltaOpts.Seal = SealOf.Truth;
                Truth = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Truth", initOpts.Seal == SealOf.Truth);
                Truth.Item = null;

                return new ComparisonCalculationBase[] { Righteousness, Truth };
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
                    GetWeaponSpeedComparison(character, 3.7f),
                    GetWeaponSpeedComparison(character, 3.8f)
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
                character.MainHand.RandomSuffixId,
                character.MainHand.Gem1, 
                character.MainHand.Gem2, 
                character.MainHand.Gem3, 
                character.MainHand.Enchant,
                character.MainHand.Reforging,
                character.MainHand.Tinkering);

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
