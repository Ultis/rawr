using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.DPSDK
{
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", CharacterClass.DeathKnight, CharacterRole.MeleeDPS)]
    public class CalculationsDPSDK : CalculationsBase
    {
        #region DPSDK Gemming Templates
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
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                const int Reverberating = 68779; // Meta

                string group; bool enabled;
                List<GemmingTemplate> templates = new List<GemmingTemplate>()
                    {
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = Reverberating, },
                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },

                        new GemmingTemplate() { Model = "DPSDK", Group = "Engineer", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = Reverberating, },
                    };

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
                enabled = true;
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
                enabled = true;
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
                        Model = "DPSDK",
                        Group = groupStr,
                        RedId = red[j] != 0 ? red[j] : org[j] != 0 ? org[j] : prp[j],
                        YellowId = ylw[j] != 0 ? ylw[j] : org[j] != 0 ? org[j] : grn[j],
                        BlueId = blu[j] != 0 ? blu[j] : prp[j] != 0 ? prp[j] : grn[j],
                        PrismaticId = red[j] != 0 ? red[j] : ylw[j] != 0 ? ylw[j] : blu[j],
                        //CogwheelId = cog[j],
                        HydraulicId = 0,
                        MetaId = chaotic,
                        Enabled = (enabled && j == 1)
                    });
                }
            }
        }
        #endregion

        public static float AddStatMultiplierStat(float statMultiplier, float newValue)
        {
            float updatedStatModifier = ((1f + statMultiplier) * (1f + newValue)) - 1f;
            return updatedStatModifier;
        }
        
        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,0,0,255));
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
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>();
                    string szPreBasic = "Basic Stats";
                    string[] szBasicStats = {
                        "Health",
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise",
                        "Haste Rating",
                        "Armor",
                        "Mastery",};
                    labels.AddRange(BuildLabels(szPreBasic, szBasicStats));
                    string szPreAdvanced = "Advanced Stats";
                    string[] szAdvStats = {
                        "Weapon Damage*Damage before misses and mitigation",
                        "Attack Speed",
                        "Crit Chance",
                        "Avoided Attacks",
                        "Enemy Mitigation",
                        "White HitChance",
                        "Yellow HitChance"};
                    labels.AddRange(BuildLabels(szPreAdvanced, szAdvStats));
                    string szPreDPSBreakdown = "DPS Breakdown";
                    string[] szDPSBreakdown = new string[EnumHelper.GetCount(typeof(DKability))];
                    foreach (int i in EnumHelper.GetValues(typeof(DKability)))
                    {
                        szDPSBreakdown[i] = Enum.GetName(typeof(DKability), i);
                    }
                    labels.AddRange(BuildLabels(szPreDPSBreakdown, szDPSBreakdown));
                    string szPreRotation = "Rotation Data";
                    string[] szRotation = {
                        "Rotation Duration*Duration of the total rotation cycle",
                        "Blood*Number of Runes consumed",
                        "Frost*Number of Runes consumed",
                        "Unholy*Number of Runes consumed",
                        "Death*Number of Runes consumed",
                        "Runic Power*Amount of Runic Power left after rotation.\nNegative values mean more RP generated than used.",
                        "RE Runes*Number of Runes Generated by Runic Empowerment.",
                        "Rune Cooldown*Duration for a single Rune to refresh.",};
                    labels.AddRange(BuildLabels(szPreRotation, szRotation));
                    string szPreDPU = "Damage Per Use";
                    string[] szDPU = {
                        "BB",
                        "BP",
                        "BS",
                        "DC",
                        "DnD",
                        "DS",
                        "Fest",
                        "FF",
                        "FS",
                        "HS",
                        "HB",
                        "IT*Not Including FF",
                        "NS",
                        "OB",
                        "PS*Not Including BP",
                        "RS",
                        "SS",};
                    labels.AddRange(BuildLabels(szPreDPU, szDPU));
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] BuildLabels(string szPrefix, string[] szBody)
        {
            List<string> labels = new List<string>();
            if (null != szBody && szBody.Length > 0)
            {
                foreach (string s in szBody)
                {
                    labels.Add(szPrefix + ":" + s);
                }
            }
            return labels.ToArray();
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// List<ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
        /// </summary>
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null) 
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Plate,
                        ItemType.Relic,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSDK(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSDK(); }
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsDPSDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSDK;
            return calcOpts;
        }

        private static bool HidingBadStuff { get { return HidingBadStuff_Def || HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        internal static bool HidingBadStuff_Def { get; set; }
        internal static bool HidingBadStuff_Spl { get; set; }
        internal static bool HidingBadStuff_PvP { get; set; }

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
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsDPSDK calc = new CharacterCalculationsDPSDK();
            if (character == null) { return calc; }
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            if (calcOpts == null) { return calc; }
            //
            StatsDK stats = new StatsDK();
            DeathKnightTalents talents = character.DeathKnightTalents;

            // Setup initial Boss data.
            // Get Boss from BossOptions data.
            BossOptions hBossOptions = character.BossOptions;
            if (hBossOptions == null) hBossOptions = new BossOptions(); 
            int targetLevel = hBossOptions.Level;

            stats = GetCharacterStats(character, additionalItem) as StatsDK;
            calc.BasicStats = stats.Clone() as StatsDK;
            ApplyRatings(calc.BasicStats);

            DKCombatTable combatTable = new DKCombatTable(character, calc.BasicStats, calc, calcOpts, hBossOptions);
            if (needsDisplayCalculations) combatTable.PostAbilitiesSingleUse(false);
            Rotation rot = new Rotation(combatTable);
            Rotation.Type RotT = rot.GetRotationType(character.DeathKnightTalents);

            // TODO: Fix this so we're not using pre-set rotations/priorities.
            if (RotT == Rotation.Type.Frost)
                rot.PRE_Frost();
            else if (RotT == Rotation.Type.Unholy)
                rot.PRE_Unholy();
            else if (RotT == Rotation.Type.Blood)
                rot.PRE_BloodDiseased();
            else
                rot.Solver();

            //TODO: This may need to be handled special since it's to update stats.
            AccumulateSpecialEffectStats(stats, character, calcOpts, combatTable, rot); // Now add in the special effects.
            ApplyRatings(stats);
            #region Cinderglacier
            if (stats.CinderglacierProc > 0)
            {
                // How many frost & shadow abilities do we have per min.?
                float CGabs = ((rot.m_FrostSpecials + rot.m_ShadowSpecials) / rot.CurRotationDuration) * 60f;
                float effCG = 0;
                if (CGabs > 0)
                    // Since 3 of those abilities get the 20% buff
                    // Get the effective ammount of CinderGlacier that would be applied across each ability.
                    // it is a proc after all.
                    effCG = 3 / CGabs;
                stats.BonusFrostDamageMultiplier += (.2f * effCG);
                stats.BonusShadowDamageMultiplier += (.2f * effCG);
            }
            #endregion

            // refresh w/ updated stats.
            combatTable = new DKCombatTable(character, stats, calc, calcOpts, hBossOptions);
            combatTable.PostAbilitiesSingleUse(false);
            rot = new Rotation(combatTable);
            RotT = rot.GetRotationType(character.DeathKnightTalents);

            // TODO: Fix this so we're not using pre-set rotations.
            if (RotT == Rotation.Type.Frost)
                rot.PRE_Frost();
            else if (RotT == Rotation.Type.Unholy)
                rot.PRE_Unholy();
            else if (RotT == Rotation.Type.Blood)
                rot.PRE_BloodDiseased();
            else
                rot.Solver();

            #region Pet Handling 
            // For UH, this is valid.  For Frost/Blood, we need to have this be 1/3 of the value since it has an uptime of 1 min for every 3.
            float ghouluptime = 1f;
            calc.dpsSub[(int)DKability.Gargoyle] = 0;
            if (RotT != Rotation.Type.Unholy) ghouluptime = 1f / 3f;
            else 
            {
                // Unholy will also have gargoyles.
                Pet Gar = new Gargoyle(stats, talents, hBossOptions, calcOpts.presence);
                float garuptime = .5f/3f;
                calc.dpsSub[(int)DKability.Gargoyle] = Gar.DPS * garuptime;
                calc.damSub[(int)DKability.Gargoyle] = Gar.DPS * 30f; // Duration 30 seconds.
            }
            Pet ghoul = new Ghoul(stats, talents, hBossOptions, calcOpts.presence);
            calc.dpsSub[(int)DKability.Ghoul] = ghoul.DPS * ghouluptime;
            calc.damSub[(int)DKability.Ghoul] = ghoul.DPS * 60f; // Duration 1 min.

            #endregion

            // Stats as Fire damage additive value proc.
            if (stats.ArcaneDamage > 1) calc.dpsSub[(int)DKability.OtherArcane] += stats.ArcaneDamage;
            if (stats.FireDamage > 1) calc.dpsSub[(int)DKability.OtherFire] += stats.FireDamage;
            if (stats.FrostDamage > 1) calc.dpsSub[(int)DKability.OtherFrost] += stats.FrostDamage;
            if (stats.HolyDamage > 1) calc.dpsSub[(int)DKability.OtherHoly] += stats.HolyDamage;
            if (stats.NatureDamage > 1) calc.dpsSub[(int)DKability.OtherNature] += stats.NatureDamage;
            if (stats.ShadowDamage > 1) calc.dpsSub[(int)DKability.OtherShadow] += stats.ShadowDamage;
            // Fire Dam Multiplier.

            calc.RotationTime = rot.CurRotationDuration;
            calc.Blood = rot.m_BloodRunes;
            calc.Frost = rot.m_FrostRunes;
            calc.Unholy = rot.m_UnholyRunes;
            calc.Death = rot.m_DeathRunes;
            calc.RP = rot.m_RunicPower;
            calc.FreeRERunes = rot.m_FreeRunesFromRE;

            calc.EffectiveArmor = stats.Armor;

            calc.OverallPoints = calc.DPSPoints = rot.m_DPS 
                // Add in supplemental damage from other sources
                + calc.dpsSub[(int)DKability.Ghoul] 
                + calc.dpsSub[(int)DKability.Gargoyle]
                + calc.dpsSub[(int)DKability.OtherArcane] + calc.dpsSub[(int)DKability.OtherFire] + calc.dpsSub[(int)DKability.OtherFrost] + calc.dpsSub[(int)DKability.OtherHoly] + calc.dpsSub[(int)DKability.OtherNature] + calc.dpsSub[(int)DKability.OtherShadow];
            if (needsDisplayCalculations)
            {
                AbilityDK_Base a = rot.GetAbilityOfType(DKability.White);
                if (rot.ml_Rot.Count > 1)
                {
                    AbilityDK_Base b;
                    b = rot.GetAbilityOfType(DKability.ScourgeStrike);
                    if (b == null) b = rot.GetAbilityOfType(DKability.FrostStrike);
                    if (b == null) b = rot.GetAbilityOfType(DKability.DeathStrike);
                    calc.YellowHitChance = b.HitChance;
                }
                calc.WhiteHitChance = (a == null ? 0 : a.HitChance + a.CritChance + .23f); // + glancing
                calc.MHWeaponDPS = (a == null ? 0 : rot.GetAbilityOfType(DKability.White).DPS);
                if (null != combatTable.MH)
                {
                    calc.MHWeaponDamage = combatTable.MH.damage;
                    calc.MHAttackSpeed = combatTable.MH.hastedSpeed;
                    calc.DodgedAttacks = combatTable.MH.chanceDodged;
                    calc.AvoidedAttacks = combatTable.MH.chanceDodged;
                    if (!hBossOptions.InBack)
                        calc.AvoidedAttacks += combatTable.MH.chanceParried;
                    calc.MissedAttacks = combatTable.MH.chanceMissed;
                }
                if (null != combatTable.OH)
                {
                    a = rot.GetAbilityOfType(DKability.WhiteOH);
                    calc.OHWeaponDPS = (a == null ? 0 : rot.GetAbilityOfType(DKability.WhiteOH).DPS);
                    calc.OHWeaponDamage = combatTable.OH.damage;
                    calc.OHAttackSpeed = combatTable.OH.hastedSpeed;
                }
                calcOpts.szRotReport = rot.ReportRotation();
                calc.m_RuneCD = (float)rot.m_SingleRuneCD / 1000;

                calc.DPSBreakdown(rot);
            }  

            return calc;
        }

        private Stats GetRaceStats(Character character) 
        {
            return BaseStats.GetBaseStats(character.Level, CharacterClass.DeathKnight, character.Race);
        }

        public static void AccumulatePresenceStats(StatsDK PresenceStats, Presence p, DeathKnightTalents t)
        {
            switch(p)
            {
                case Presence.Blood:
                {
                    if (t.ImprovedBloodPresence > 0)
                    {
                        PresenceStats.CritChanceReduction += 0.03f * t.ImprovedBloodPresence;
                        PresenceStats.BonusRuneRegeneration += .1f * t.ImprovedBloodPresence;
                    }
                    else if (t.ImprovedFrostPresence > 0)
                        PresenceStats.BonusRPMultiplier += .02f * t.ImprovedFrostPresence;
                    else if (t.ImprovedUnholyPresence == 1)
                        PresenceStats.MovementSpeed += .08f;
                    else if (t.ImprovedUnholyPresence == 2)
                        PresenceStats.MovementSpeed += .15f;
                    PresenceStats.BonusStaminaMultiplier += .08125f; 
                    if (Rawr.Properties.GeneralSettings.Default.PTRMode)
                        PresenceStats.BaseArmorMultiplier += 0.55f;
                    else
                        PresenceStats.BaseArmorMultiplier += 0.3f;

                    PresenceStats.DamageTakenReductionMultiplier = 1f - (1f - PresenceStats.DamageTakenReductionMultiplier) * (1f - 0.08f);
                    // Threat bonus.
                    PresenceStats.ThreatIncreaseMultiplier += 1f; 
                    break;
                }
                case Presence.Frost:
                {
                    if (t.ImprovedBloodPresence > 0)
                        PresenceStats.DamageTakenReductionMultiplier = 1f - (1f - PresenceStats.DamageTakenReductionMultiplier) * (1f - 0.02f * t.ImprovedBloodPresence);
                    else if (t.ImprovedUnholyPresence == 1)
                        PresenceStats.MovementSpeed += .08f;
                    else if (t.ImprovedUnholyPresence == 2)
                        PresenceStats.MovementSpeed += .15f;
                    PresenceStats.BonusDamageMultiplier += 0.1f;
                    PresenceStats.BonusRPMultiplier += 0.1f;  
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    break;
                }
                case Presence.Unholy:
                {
                    if (t.ImprovedBloodPresence > 0)
                        PresenceStats.DamageTakenReductionMultiplier = 1f - (1f - PresenceStats.DamageTakenReductionMultiplier) * (1f - 0.02f * t.ImprovedBloodPresence);
                    else if (t.ImprovedFrostPresence > 0)
                        PresenceStats.BonusRPMultiplier += .02f * t.ImprovedFrostPresence;
                    else if (t.ImprovedUnholyPresence > 0)
                        PresenceStats.PhysicalHaste = AddStatMultiplierStat(PresenceStats.PhysicalHaste, (.025f * t.ImprovedUnholyPresence));
                    PresenceStats.PhysicalHaste = AddStatMultiplierStat(PresenceStats.PhysicalHaste, .1f);
                    //PresenceStats.BonusRuneRegeneration += .1f;
                    PresenceStats.MovementSpeed += .15f;
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    break;
                }
            }
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
            StatsDK statsTotal = new StatsDK();
            if (null == character)
            {
#if DEBUG
                throw new Exception("Character is Null");
#else
                return statsTotal;
#endif
            }
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            if (null == calcOpts) { calcOpts = new CalculationOptionsDPSDK(); }
            DeathKnightTalents talents = character.DeathKnightTalents;
            if (null == talents) { return statsTotal; }

            statsTotal.Accumulate(GetRaceStats(character));
            AccumulateItemStats(statsTotal, character, additionalItem);
            statsTotal = GetRelevantStats(statsTotal) as StatsDK; // GetRel removes any stats specific to the StatsDK object.

            statsTotal.bDW = false;
            if (character.MainHand != null && character.OffHand != null) statsTotal.bDW = true;
            RemoveDuplicateRunes(statsTotal, character, statsTotal.bDW);
            AccumulateBuffsStats(statsTotal, character.ActiveBuffs);

            #region Tank
            #region T11
            int tierCount;
            if (character.SetBonusCount.TryGetValue("Magma Plated Battlearmor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T11_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T11_Tank = true; }
            }
            if (statsTotal.b4T11_Tank)
                statsTotal.AddSpecialEffect(_SE_IBF[1]);
            else
                statsTotal.AddSpecialEffect(_SE_IBF[0]);
            #endregion
            #region T12
            if (character.SetBonusCount.TryGetValue("Elementium Deathplate Battlearmor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T12_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T12_Tank = true; }
            }
            if (statsTotal.b2T12_Tank)
            {
                // Your melee attacks cause Burning Blood on your target, 
                // which deals 800 Fire damage every 2 for 6 sec and 
                // causes your abilities to behave as if you had 2 diseases 
                // present on the target.

                // Implemented in CombatState DiseaseCount
                statsTotal.FireDamage = 800 / 2;
            }
            if (statsTotal.b4T12_Tank)
            {
                // Your Dancing Rune Weapon grants 15% additional parry chance.
                // Implemented in DRW talent Static Special Effect.
            }
            #endregion
            #region T13
            if (character.SetBonusCount.TryGetValue("Necrotic Boneplate Armor", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T13_Tank = true; }
                if (tierCount >= 4) { statsTotal.b4T13_Tank = true; }
            }
            if (statsTotal.b2T13_Tank)
            {
                // When an attack drops your health below 35%, one of your Blood Runes 
                // will immediately activate and convert into a Death Rune for the next 
                // 20 sec. This effect cannot occur more than once every 45 sec.
            }
            if (statsTotal.b4T13_Tank)
            {
                // Your Vampiric Blood ability also affects all party and raid members 
                // for 50% of the effect it has on you.
            }
            #endregion
            #endregion
            #region DPS
            #region T11
            if (character.SetBonusCount.TryGetValue("Magma Plated Battlegear", out tierCount))
            {
                if (tierCount >= 2)
                {
                    statsTotal.b2T11_DPS = true;
                    if (tierCount >= 4) { statsTotal.b4T11_DPS = true; }
                }
                if (statsTotal.b2T11_DPS)
                {
                    // increase the crit chance of your DeathCoil & FS by 5%
                    statsTotal.BonusCritChanceDeathCoil += .05f;
                    statsTotal.BonusCritChanceFrostStrike += .05f;
                }
                if (statsTotal.b4T11_DPS)
                {
                    // Each time you gain a Death Rune or trigger your Killing Machine talent, 
                    // you also gain 1% increased attack power for 30 sec. Stacks up to 3 times.
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.DeathRuneGained,
                        new Stats() { BonusAttackPowerMultiplier = 0.01f, },
                        30, 0, 1f, 3));
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.KillingMachine,
                        new Stats() { BonusAttackPowerMultiplier = 0.01f, },
                        30, 0, 1f, 3));
                }
            }
            #endregion
            #region T12
            if (character.SetBonusCount.TryGetValue("Elementium Deathplate Battlegear", out tierCount))
            {
                if (tierCount >= 2)
                {
                    statsTotal.b2T12_DPS = true;
                    if (tierCount >= 4)
                    {
                        
                        statsTotal.b4T12_DPS = true;
                    }
                }
            }
            if (statsTotal.b2T12_DPS)
            {
                // Horn of Winter also grats 3 RPp5
                statsTotal.RPp5 += 3;
            }
            if (statsTotal.b4T12_DPS)
            {
                // Your Obliterate and Scourge Strike abilities instantly deal 6% additional damage as Fire damage.
                // Implemented in Oblit and SS classes.
            }
            #endregion
            #region T13
            if (character.SetBonusCount.TryGetValue("Necrotic Boneplate Battlegear", out tierCount))
            {
                if (tierCount >= 2) { statsTotal.b2T13_DPS = true; }
                if (tierCount >= 4) { statsTotal.b4T13_DPS = true; }
            }
            if (statsTotal.b2T13_DPS)
            {
                // Sudden Doom has a 30% chance and Rime has a 60% chance 
                // to grant 2 charges when triggered instead of 1.
            }
            if (statsTotal.b4T13_DPS)
            {
                // Runic Empowerment has a 25% chance and Runic Corruption 
                // has a 40% chance to also grant 710 mastery rating for 12 sec when activated.
            }
            #endregion
            #endregion

            AccumulateTalents(statsTotal, character);
            AccumulatePresenceStats(statsTotal, calcOpts.presence, talents);

            return (statsTotal);
        }

        private static void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup)
            {
                if (character.ActiveBuffs.Remove(b)) { removedBuffs.Add(b); }
            }
        }

        private void ApplyRatings(StatsDK statsTotal)
        {
            // Apply ratings.
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);

            statsTotal.Strength += statsTotal.HighestStat + statsTotal.Paragon;

            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= 1 + statsTotal.BonusHealthMultiplier;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower + statsTotal.Strength * 2);
            statsTotal.Armor = (float)Math.Floor(StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier) +
                                StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier));
            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            float HighestSecondaryStatValue = statsTotal.HighestSecondaryStat; // how much HighestSecondaryStat to add
            statsTotal.HighestSecondaryStat = 0f; // remove HighestSecondaryStat stat, since it's not needed
            if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating) {
                statsTotal.CritRating += HighestSecondaryStatValue;
            } else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating) {
                statsTotal.HasteRating += HighestSecondaryStatValue;
            } else /*if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating)*/ {
                statsTotal.MasteryRating += HighestSecondaryStatValue;
            }

            statsTotal.PhysicalHit += StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.DeathKnight);
            statsTotal.PhysicalHaste = AddStatMultiplierStat(statsTotal.PhysicalHaste, StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight));

            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);
        }

        private void AccumulateSpecialEffectStats(StatsDK s, Character c, CalculationOptionsDPSDK calcOpts, DKCombatTable t, Rotation rot)
        {
            StatsSpecialEffects se = new StatsSpecialEffects(t, rot, c.BossOptions );
            StatsDK statSE = new StatsDK();

            foreach (SpecialEffect effect in s.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    statSE = se.getSpecialEffects(effect);
                    s.Accumulate(statSE);
                }
            }
        }

        private void AccumulateGlyphStats(Stats s, DeathKnightTalents t)
        {
            if (t.GlyphofBoneShield)
                s.MovementSpeed = (float)Math.Max(s.MovementSpeed, 1.15f);
        }

        public static Rotation.Type GetSpec(DeathKnightTalents t)
        {
            return (Rotation.Type)t.HighestTree + 1;
        }

        /// <summary>
        /// Local version of GetItemStats()
        /// Includes the Armor style bonus.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats stats = base.GetItemStats(character, additionalItem);
            // Add in armor specialty
            if (GetQualifiesForArmorBonus(character, additionalItem))
            {
                stats.BonusStrengthMultiplier += .05f;
                stats.BonusStaminaMultiplier += .05f;
            }
            return stats;
        }


        public static bool GetQualifiesForArmorBonus(Character c, Item additionalItem)
        {
            // Easier to check if there is a DIS qualifying item than 
            // to ensure than every item matches what we expect.
            ItemTypeList list = new ItemTypeList();
            list.Add(ItemType.Cloth);
            list.Add(ItemType.Leather);
            list.Add(ItemType.Mail);
            if (additionalItem != null && list.Contains(additionalItem.Type))
                return false;
            else if ((c.Chest != null && list.Contains(c.Chest.Type))
                || (c.Feet != null && list.Contains(c.Feet.Type))
                || (c.Hands != null && list.Contains(c.Hands.Type))
                || (c.Head != null && list.Contains(c.Head.Type))
                || (c.Legs != null && list.Contains(c.Legs.Type))
                || (c.Neck != null && list.Contains(c.Neck.Type))
                || (c.Shoulders != null && list.Contains(c.Shoulders.Type))
                || (c.Waist != null && list.Contains(c.Waist.Type))
                || (c.Wrist != null && list.Contains(c.Wrist.Type))
                )
                return false;
            else
                return true;
        }
        /// <summary>Build the talent effects.</summary>
        public static void AccumulateTalents(StatsDK FullCharacterStats, Character character)
        {
            Stats newStats = new Stats();
            FullCharacterStats.Mastery += StatConversion.GetMasteryFromRating(FullCharacterStats.MasteryRating);
            // Runic Focus:
            FullCharacterStats.SpellHit += 0.09f;

            // Which talent tree focus?
            #region Talent Speciality
            Rotation.Type r = GetSpec(character.DeathKnightTalents);
            switch (r)
            {
                case Rotation.Type.Blood:
                    {
                        // Special abilities for being blood
                        // Heart Strike
                        // Veteran of the third war
                        // Stamina +9%
                        FullCharacterStats.BonusStaminaMultiplier += .09f;
                        // Expertise +6
                        FullCharacterStats.Expertise += 6;
                        // Blood Rites
                        // Whenever you hit with Death Strike or Obliterate, the Frost and Unholy Runes 
                        // will become Death Runes when they activate.  Death Runes count as a Blood, Frost or Unholy Rune.
                        // Vengence
                        // Each time you take damage, you gain 5% of the damage taken as attack power, up to a maximum of 10% of your health.
                        // Mastery: Blood Shield
                        // Each Time you heal yourself w/ DS you gain a shield worth 50% of the amount healed
                        // Each Point of Mastery increases the shield by 6.25%
                        break;
                    }
                case Rotation.Type.Frost:
                    {
                        // Special abilities for being Frost
                        // Frost Strike
                        // Icy Talons
                        // Melee Attack speed +20%
                        FullCharacterStats.PhysicalHaste = AddStatMultiplierStat(FullCharacterStats.PhysicalHaste, .2f);
                        FullCharacterStats.BonusRuneRegeneration -= .2f; // Only this haste doesn't affect Rune Regen.  

                        // Blood of the North
                        // Blood runes are death runes.
                        // Mastery: Frozen Heart
                        // Increases all frost damage by 16%.  
                        // Each point of mastery increases frost damage by an additional 2.0%
                        FullCharacterStats.BonusFrostDamageMultiplier += .16f + (.02f * FullCharacterStats.Mastery);
                        break;
                    }
                case Rotation.Type.Unholy:
                    {
                        // Special abilities for being Unholy
                        // Scourge Strike
                        // Master of Ghouls
                        // Reduces the CD on Raise dead by 60 sec.
                        // The ghoul summoned is considered your pet w/o a limited duration.
                        // Reaping
                        // Whenever you hit with Blood strike, pestilence, or Festering strike, the runes spent will 
                        // become death runes when they activate.
                        // Unholy Might
                        // Str +25%
                        FullCharacterStats.BonusStrengthMultiplier += .25f;
                        // Mastery: Dreadblade.
                        // Increases shadow damage by 20% + 
                        // Each point of mastery increases shadow damage by an additional 2.5%
                        FullCharacterStats.BonusShadowDamageMultiplier += .2f + (.025f * FullCharacterStats.Mastery);
                        break;
                    }
            }
            #endregion

            #region Blood Talents
            // Butchery
            // 1RPp5 per Point
            if (character.DeathKnightTalents.Butchery > 0)
            {
                FullCharacterStats.RPp5 += 1 * character.DeathKnightTalents.Butchery;
            }

            // Blade Barrier
            // Reduce damage by 2% per point for 10 sec.
            if (character.DeathKnightTalents.BladeBarrier > 0)
            {
                // If you don't have your Blood Runes on CD, you're doing it wrong. 
                FullCharacterStats.DamageTakenReductionMultiplier = 1f - (1f - FullCharacterStats.DamageTakenReductionMultiplier) * (1f - .02f * character.DeathKnightTalents.BladeBarrier);
            }

            // Bladed Armor
            // 2 AP per point per 180 Armor
            if (character.DeathKnightTalents.BladedArmor > 0)
            {
                // If you don't have your Blood Runes on CD, you're doing it wrong. 
                FullCharacterStats.AttackPower += (2 * character.DeathKnightTalents.BladedArmor) * (FullCharacterStats.Armor / 180);
            }

            // Improved Blood Tap
            // Reduces Blood tap CD by 15 sec * pts.
           
            // Scent of Blood
            // 15% after Dodge, Parry or damage received causing 1 melee hit per point to generate 10 runic power.
            // Implemented in WhiteSwing.

            // Scarlet Fever
            // Those hit by BB do have a 50/100% chance to reduce damage dealt by 10% for 30 sec.
            if (character.DeathKnightTalents.ScarletFever > 0)
            {
                // Like Blade Barrier, this should always be up.
                // Be sure to put this in the rotation solver for Tanking Rotations.
                // JOTHAY TODO: Isn't this conflicting with the Buffs pane?
                FullCharacterStats.DamageTakenReductionMultiplier = 1f - (1f - FullCharacterStats.DamageTakenReductionMultiplier) * (1f - .05f * character.DeathKnightTalents.ScarletFever);
            }

            // Hand of Doom
            // Reduces the CD for Strangulate by 30/60 sec.

            if (r == Rotation.Type.Blood)
            {
                // Blood-Caked Blade
                // 10% chance per point to cause Blood-Caked strike
                // Implmented in WhiteDamage ability file.
                
                // Bone Shield
                // 6 Bones 
                // Takes 20% less damage from all sources
                // Does 2% more damage to target
                // Each damaging attack consumes a bone.
                // Lasts 5 mins

                // Toughness
                // Increases Armor Value from items by 3% per point.
                if (character.DeathKnightTalents.Toughness > 0)
                {
                    FullCharacterStats.BaseArmorMultiplier = AddStatMultiplierStat(FullCharacterStats.BaseArmorMultiplier, (.03f * character.DeathKnightTalents.Toughness));
                }

                // Abominations Might
                // increase AP by 5%/10% of raid.
                // 1% per point increase to str.
                if (character.DeathKnightTalents.AbominationsMight > 0)
                {
                    // This happens no matter what:
                    FullCharacterStats.BonusStrengthMultiplier += (0.01f * character.DeathKnightTalents.AbominationsMight);
                    // This happens only if there isn't Trueshot Aura/Unleashed Rage/Abom's might  available:
                    if (!(character.ActiveBuffsContains("Trueshot Aura") || character.ActiveBuffsContains("Unleashed Rage") || character.ActiveBuffsContains("Abomination's Might")))
                    {
                        FullCharacterStats.BonusAttackPowerMultiplier += (.05f * character.DeathKnightTalents.AbominationsMight);
                    }
                }

                // Sanguine Fortitude
                // Buff's IBF:
                // While Active, your IBF reduces Dam taken by 15/30% and costs 50/100% less RP to activate.
                // CD duration? 3min suggested on pwnwear.
                // Cost?  This is a CD stacker.
                // TODO: look into CD stacking code./max v average values.

                // Blood Parasite
                // Melee Attacks have 5% * PTS chance of spawning a blood worm.
                // Blood worm attacks your enemies, gorging itself on blood
                // until it bursts to heal nearby allies. Lasts 20 sec.
                if (character.DeathKnightTalents.BloodParasite > 0)
                {
                    float fDamageDone = 200f; // TODO: Put this in a way to increase DPS.
                    float fBWAttackSpeed = 1.4f; // TODO: Validate this speed.
                    float fBWDuration = 20f;
                    float avgstacks = 5; // TODO: Figure out the best way to determine avg Stacks of BloodGorged
                    float WormHealth = (FullCharacterStats.Health * 0.35f);
                    _SE_Bloodworms = new SpecialEffect[] {
                        null,
                        new SpecialEffect(Trigger.MeleeAttack, new Stats() { Healed = ((avgstacks * WormHealth * .05f) / fBWDuration), PhysicalDamage = (fDamageDone/fBWAttackSpeed) }, fBWDuration, 0, .05f * 1),
                        new SpecialEffect(Trigger.MeleeAttack, new Stats() { Healed = ((avgstacks * WormHealth * .05f) / fBWDuration), PhysicalDamage = (fDamageDone/fBWAttackSpeed) }, fBWDuration, 0, .05f * 2),
                    };
                    FullCharacterStats.AddSpecialEffect(_SE_Bloodworms[character.DeathKnightTalents.BloodParasite]);
                }

                // Improved Blood Presence
                // Reduces chance to be critically hit while in blood presence by 3/6%
                // In addition while in Frost or Unholy, retain the 2/4% Dam reduction. 
                // Implemented in AccumulatePresenceStats()

                // Will of the Necropolis
                // Dam that takes you below 35% health or while at less than 35% is reduced by 5% per point.  
                if (character.DeathKnightTalents.WillOfTheNecropolis > 0)
                {
                    // Need to factor in the damage taken aspect of the trigger.
                    // Using the assumption that the tank will be at < 35% health about that % of the time.
                    FullCharacterStats.AddSpecialEffect(_SE_WillOfTheNecropolis[character.DeathKnightTalents.WillOfTheNecropolis]);
                }

                // Rune Tap
                // Convert 1 BR to 10% health.
                if (character.DeathKnightTalents.RuneTap > 0)
                {
                    FullCharacterStats.AddSpecialEffect(_SE_RuneTap);
                }

                // Vampiric Blood
                // temp 15% of max health and
                // increases health generated by 25% for 10 sec.
                // 1 min CD. 
                if (character.DeathKnightTalents.VampiricBlood > 0)
                {
                    FullCharacterStats.AddSpecialEffect(_SE_VampiricBlood[character.DeathKnightTalents.GlyphofVampiricBlood ? 1 : 0]);
                }

                // Improved Death Strike
                if (character.DeathKnightTalents.ImprovedDeathStrike > 0)
                {
                    FullCharacterStats.BonusDamageDeathStrike += (.40f * character.DeathKnightTalents.ImprovedDeathStrike);
                    // Also improves DS Healing.  Implemented in TankDK heals section.
                }

                // Crimson Scourge 
                // Increases the damage dealt by your Blood Boil by 20/40%, and when you Plague Strike a target that is already
                // infected with your Blood Plague, there is a 50/100% chance that your next Blood Boil will consume no runes.
                if (character.DeathKnightTalents.CrimsonScourge > 0)
                {
                    // Part 1 implmented in BB Ability
                    // TODO: Part 2 implement in rotation.
                }

                // Dancing Rune Weapon
                if (character.DeathKnightTalents.DancingRuneWeapon > 0)
                {
                    uint u4T12 = FullCharacterStats.b4T12_Tank ? 1u : 0u;
                    uint uGDRW = character.DeathKnightTalents.GlyphofDancingRuneWeapon ? 1u : 0u;
                    FullCharacterStats.AddSpecialEffect(_SE_DRW[u4T12][uGDRW]);
                }
            }
            #endregion

            #region Frost Talents

            // Runic Power Mastery
            // Increases Max RP by 10 per point
            if (character.DeathKnightTalents.RunicPowerMastery > 0)
            {
                FullCharacterStats.BonusMaxRunicPower += 10 * character.DeathKnightTalents.RunicPowerMastery;
            }

            // Icy Reach
            // Increases range of IT & CoI and HB by 5 yards per point.

            // Nerves of Cold Steel
            // Increase hit w/ 1H weapons by 1% per point
            // Increase damage done by off hand weapons by 8/16/25% per point
            // Implement off-hand weapon buff in combat shot roation
            if (character.MainHand != null && 
                (character.MainHand.Slot == ItemSlot.MainHand
                || character.MainHand.Slot == ItemSlot.OneHand))
            {
                FullCharacterStats.PhysicalHit += (character.DeathKnightTalents.NervesOfColdSteel * .01f);
            }

            // Lichborne
            // for 10 sec, immune to charm, fear, sleep
            // CD 2 Mins

            // On a Pale Horse
            // Reduce duration of Movement slowing effects by 15% per point
            // increase mount speed by 10% per point
            if (character.DeathKnightTalents.OnAPaleHorse > 0)
            {
                // Now w/ boss handler, reduce the duration of the movement slowing effects. 
            }

            // Endless Winter
            // Mind Freeze RP cost is reduced by 50% per point.
            if (character.DeathKnightTalents.EndlessWinter > 0)
            {  }

            if (r == Rotation.Type.Frost)
            {
                // Merciless Combat
                // addtional 6% per point damage for IT, HB, Oblit, and FS
                // on targets of less than 35% health.
                if (character.DeathKnightTalents.MercilessCombat > 0)
                {
                    // implemented in the abilities section (increase the damage done by 15% * 35%)
                }

                // Chill of the Grave
                // CoI, HB, IT and Oblit generate 5 RP per point.
                if (character.DeathKnightTalents.ChillOfTheGrave > 0)
                {
                    // Implemented in the abilities.
                }

                // Killing Machine
                // Melee attacks have a chance to make OB, or FS a crit.
                // increased proc per point.
                // Research Suggests 5 PPM at 3/3
                if (character.DeathKnightTalents.KillingMachine > 0)
                {
//                    FullCharacterStats.AddSpecialEffect(_KM[character.DeathKnightTalents.KillingMachine]);
                    // Implemented in Frost Rotation
                }


                // Rime
                // Oblit has a 15% per point your next IT or HB consumes no runes
                if (character.DeathKnightTalents.Rime > 0)
                {
                    // Implemented in FrostRotation.
                }

                // Pillar of Frost
                // 1 min CD, 20 sec dur
                // Str +20%
                if (character.DeathKnightTalents.PillarOfFrost > 0)
                {
                    FullCharacterStats.AddSpecialEffect(_SE_PillarOfFrost);
                }

                // Improved Icy Talons
                // increases the melee haste of the group/raid by 10%
                // increases your haste by 5% all the time.
                if (character.DeathKnightTalents.ImprovedIcyTalons > 0)
                {
                    FullCharacterStats.PhysicalHaste = AddStatMultiplierStat(FullCharacterStats.PhysicalHaste, 0.05f);
                    if (!character.ActiveBuffsContains("Improved Icy Talons")
                        && !character.ActiveBuffsContains("Windfury Totem"))
                    {
                        FullCharacterStats.PhysicalHaste = AddStatMultiplierStat(FullCharacterStats.PhysicalHaste, .1f);
                        FullCharacterStats.RangedHaste += .1f;
                    }
                }

                // Brittle Bones:
                // Str +2% per point
                // FF chills the bones of its victims increasing damage taken by 2% per point.
                if (character.DeathKnightTalents.BrittleBones > 0)
                {
                    FullCharacterStats.BonusStrengthMultiplier += .02f * character.DeathKnightTalents.BrittleBones;
                    FullCharacterStats.BonusDamageMultiplier += .02f * character.DeathKnightTalents.BrittleBones;
                }

                // Chilblains
                // FF victimes are movement reduced 25% per point

                // Hungering Cold
                // Spell that freezes all enemies w/ 10 yards.


                // Improved Frost Presence
                // Increases your bonus damage while in Frost Presence by an additional 2% per point.  
                // In addition, while in Blood Presence or Unholy Presence, you retain 2% per point increased runic power generation from Frost Presence.
                if (character.DeathKnightTalents.ImprovedFrostPresence > 0)
                {
                    FullCharacterStats.BonusDamageMultiplier += (0.02f * character.DeathKnightTalents.ImprovedFrostPresence);
                }

                // Threat of Thassarian: 
                // When dual-wielding, your Death Strikes, Obliterates, Plague Strikes, 
                // Blood Strikes and Frost Strikes and Rune Strike (as of 3.2.2) have a 30/60/100% chance 
                // to also deal damage with your off-hand weapon. 
                if (character.DeathKnightTalents.ThreatOfThassarian > 0)
                { 
                    // implemented in the abilities
                }

                // Might of the Frozen Wastes
                // When wielding a two-handed weapon, your autoattacks have a 15% chance to generate 10 Runic Power.
                if (character.DeathKnightTalents.MightOfTheFrozenWastes > 0)
                {
                    // WhiteDamage Bonus in WhiteDamage Ability.
                    if (character.MainHand != null && character.MainHand.Slot == ItemSlot.TwoHand)
                        FullCharacterStats.BonusPhysicalDamageMultiplier += (.1f/3) * character.DeathKnightTalents.MightOfTheFrozenWastes;
                }

                // Howling Blast.
            }
            #endregion

            #region UnHoly Talents
            // Unholy Command
            // reduces CD of DG by 5 sec per point

            // Virulence
            // 
            if (character.DeathKnightTalents.Virulence > 0)
            {
                FullCharacterStats.BonusDiseaseDamageMultiplier += (.1f * character.DeathKnightTalents.Virulence);
            }

            // Epidemic
            // Increases Duration of BP and FF by 4 sec per point
            // Implemented in the abilities page.

            // Desecration
            // PS and SS cause Desecrated Ground effect.
            // Targets are slowed by 25% per point
            // Not Implemented.
            
            // Resilient Infection
            // When your diseases are dispelled you have a 50/100% to activate a 
            // Frost rune if FF was dispelled
            // Unholy rune if BP was dispelled
            // Not Implemented

            // Morbidity
            // increases dam & healing of DC by 5% per point
            // increases dam of DnD by 10% sec per point
            if (character.DeathKnightTalents.Morbidity > 0)
            {
                // implemented in abilities.
            }

            if (r == Rotation.Type.Unholy)
            {
                // Runic Corruption
                // Reduces the cost of your Death Coil by 3 per point, and causes 
                // your Runic Empowerment ability to no longer refresh a depleted 
                // rune, but instead to increase your rune regeneration rate by 50/100% for 3 sec.
                if (character.DeathKnightTalents.RunicCorruption > 0)
                {
                    // Implmented in rotation.
                }

                // Unholy Frenzy
                // Induces a friendly unit into a killing frenzy for 30 sec.  
                // The target is Enraged, which increases their melee and ranged haste by 20%, 
                // but causes them to lose health equal to 2% of their maximum health every 3 sec.
                // TODO:

                // Contagion
                // Increases the damage of your diseases spread via Pestilence by 50/100%.
                if (character.DeathKnightTalents.Contagion > 0)
                    FullCharacterStats.BonusDiseaseDamageMultiplier += .5f * character.DeathKnightTalents.Contagion;

                // Shadow Infusion
                // When you cast Death Coil, you have a 33% per point chance to empower your active Ghoul, 
                // increasing its damage dealt by 10% for 30 sec.  Stacks up to 5 times.
                // TODO: Implement in Rotation & Ghoul

                // Magic Suppression
                // AMS absorbs additional 8, 16, 25% of spell damage.

                // Rage of Rivendare
                // Increases the damage of your Plague Strike, Scourge Strike, and Festering Strike abilities by 15% per point.
                if (character.DeathKnightTalents.RageOfRivendare > 0)
                {
                    // Implemented in the abilities.
                }

                // Unholy Blight
                // Causes the victims of your Death Coil to be surrounded by a vile swarm of unholy insects, 
                // taking 10% of the damage done by the Death Coil over 10 sec, and preventing any diseases on the victim from being dispelled.
                // Implemented in the DeathCoil ability.

                // AntiMagic Zone
                // Creates a zone where party/raid members take 75% less spell damage
                // Lasts 10 secs or X damage.  
                if (character.DeathKnightTalents.AntiMagicZone > 0)
                {
                    FullCharacterStats.AddSpecialEffect(_SE_AntiMagicZone);
                }

                // Improved Unholy Presence
                // Grants you an additional 2% haste while in Unholy Presence.  
                // In addition, while in Blood Presence or Frost Presence, you retain 8% increased movement speed from Unholy Presence.
                // Implemented in Presence Stats.

                // Dark Transformation
                // Consume 5 charges of Shadow Infusion on your Ghoul to transform it into a powerful 
                // undead monstrosity for 30 sec.  The Ghoul's abilities are empowered and take on new 
                // functions while the transformation is active.
                // TODO: implement in Ghoul

                // Ebon Plaguebringer
                // Your Plague Strike, Icy Touch, Chains of Ice, and Outbreak abilities also infect 
                // their target with Ebon Plague, which increases damage taken from your diseases 
                // by 15/30% and all magic damage taken by an additional 8%.
                if (character.DeathKnightTalents.EbonPlaguebringer > 0)
                {
                    if (!character.ActiveBuffsContains("Earth and Moon")
                        && !character.ActiveBuffsContains("Curse of the Elements")
                        && !character.ActiveBuffsContains("Ebon Plaguebringer"))
                    {
                        float fBonus = .08f;
                        FullCharacterStats.BonusArcaneDamageMultiplier += fBonus;
                        FullCharacterStats.BonusFireDamageMultiplier += fBonus;
                        FullCharacterStats.BonusFrostDamageMultiplier += fBonus;
                        FullCharacterStats.BonusHolyDamageMultiplier += fBonus;
                        FullCharacterStats.BonusNatureDamageMultiplier += fBonus;
                        FullCharacterStats.BonusShadowDamageMultiplier += fBonus;
                    }
                    FullCharacterStats.BonusDiseaseDamageMultiplier += (.15f * character.DeathKnightTalents.EbonPlaguebringer);
                }

                // Sudden Doom
                // Your auto attacks have a 5% per point chance to make your next Death Coil cost no runic power.
                // TODO: To Implment in DeathCoil

                // Summon Gargoyle
            }
            #endregion
        }

        #region Custom Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] 
                        { 
                            "Stats Graph", 
                            "Scaling vs Strength",
                            "Scaling vs Crit Rating",
                            "Scaling vs Haste Rating",
                            "Scaling vs Mastery Rating",
                            "Presences",
                        };
                }
                return _customChartNames;
            }
        }
        
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSDK baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

            string[] statList = new string[] 
            {
                "Strength",
                "Agility",
                "Attack Power",
                "Crit Rating",
                "Hit Rating",
                "Expertise Rating",
                "Haste Rating",
                "Mastery Rating",
            };

            switch (chartName)
            {
                case "Presences":
                    {
                        string[] listPresence = EnumHelper.GetNames(typeof(Presence));

                        // Set this to have no presence enabled.
                        Character baseCharacter = character.Clone();
                        baseCharacter.IsLoading = true;
                        (baseCharacter.CalculationOptions as CalculationOptionsDPSDK).presence = Presence.None;
                        baseCharacter.IsLoading = false;
                        // replacing pre-factored base calc since this is different than the Item budget lists. 
                        baseCalc = GetCharacterCalculations(baseCharacter, null, true, false, false) as CharacterCalculationsDPSDK;

                        // Set these to have the key presence enabled.
                        for (int index = 0; index < listPresence.Length; index++)
                        {
                            (character.CalculationOptions as CalculationOptionsDPSDK).presence = (Presence)index;
                            
                            calc = GetCharacterCalculations(character, null, false, true, true) as CharacterCalculationsDPSDK;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = listPresence[index];
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
                    }
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override System.Windows.Controls.Control GetCustomChartControl(string chartName)
        {
            switch (chartName)
            {
                case "Stats Graph":
                case "Scaling vs Strength":
                case "Scaling vs Crit Rating":
                case "Scaling vs Haste Rating":
                case "Scaling vs Mastery Rating":
                    return Graph.Instance;
                default:
                    return null;
            }
        }

        public override void UpdateCustomChartData(Character character, string chartName, System.Windows.Controls.Control control)
        {
            Color[] statColors = new Color[] { 
                Color.FromArgb(0xFF, 0xFF, 0, 0), 
                Color.FromArgb(0xFF, 0xFF, 165, 0), 
                Color.FromArgb(0xFF, 0x80, 0x80, 0x00), 
                Color.FromArgb(0xFF, 154, 205, 50), 
                Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF), 
                Color.FromArgb(0xFF, 0, 0, 0xFF), 
                Color.FromArgb(0xFF, 0x80, 0, 0xFF),
                Color.FromArgb(0xFF, 0, 0x80, 0xFF),
            };

            List<float> X = new List<float>();
            List<ComparisonCalculationBase[]> Y = new List<ComparisonCalculationBase[]>();

            float fMultiplier = 1;
            Stats[] statsList = new Stats[] {
                        new Stats() { Strength = fMultiplier },
                        new Stats() { Agility = fMultiplier },
                        new Stats() { AttackPower = fMultiplier },
                        new Stats() { CritRating = fMultiplier },
                        new Stats() { HitRating = fMultiplier },
                        new Stats() { ExpertiseRating = fMultiplier },
                        new Stats() { HasteRating = fMultiplier },
                        new Stats() { MasteryRating = fMultiplier },
                    };

            switch (chartName)
            {
                case "Stats Graph":
                    Graph.Instance.UpdateStatsGraph(character, statsList, statColors, 200, "", null);
                    break;
                case "Scaling vs Strength":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { Strength = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Crit Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { CritRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Haste Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { HasteRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
                case "Scaling vs Mastery Rating":
                    Graph.Instance.UpdateScalingGraph(character, statsList, new Stats() { MasteryRating = fMultiplier * 5 }, true, statColors, 100, "", null);
                    break;
            }
        }

        #endregion

        #region Relevant Stats?
        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand)
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            StatsDK s = new StatsDK()
            {
                // Core stats
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                ExpertiseRating = stats.ExpertiseRating,
                AttackPower = stats.AttackPower,
                MasteryRating = stats.MasteryRating,
                // Other Base Stats
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Resilience = stats.Resilience,

                // Secondary Stats
                Health = stats.Health,
                SpellHaste = stats.SpellHaste,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellPenetration = stats.SpellPenetration,

                // Dam stats
                WeaponDamage = stats.WeaponDamage,
                PhysicalDamage = stats.PhysicalDamage,
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,

                // Bonus to stat
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,

                // Bonus to Dam
                // *Dam
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                // +Dam
                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,
                BonusDamageScourgeStrike = stats.BonusDamageScourgeStrike,
                BonusDamageBloodStrike = stats.BonusDamageBloodStrike,
                BonusDamageDeathCoil = stats.BonusDamageDeathCoil, 
                BonusDamageDeathStrike =  stats.BonusDamageDeathStrike,
                BonusDamageFrostStrike = stats.BonusDamageFrostStrike,
                BonusDamageHeartStrike = stats.BonusDamageHeartStrike,
                BonusDamageIcyTouch = stats.BonusDamageIcyTouch,
                BonusDamageObliterate = stats.BonusDamageObliterate,
                // Crit
                BonusCritChanceDeathCoil = stats.BonusCritChanceDeathCoil,
                BonusCritChanceFrostStrike = stats.BonusCritChanceFrostStrike,
                BonusCritChanceObliterate = stats.BonusCritChanceObliterate,
                // Other
                CinderglacierProc = stats.CinderglacierProc,
                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                TargetArmorReduction = stats.TargetArmorReduction,
                // BossHandler
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.PhysicalAttack ||
                        effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.DeathRuneGained ||
                        effect.Trigger == Trigger.Use)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool bRelevant = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.DeathRuneGained ||
                        effect.Trigger == Trigger.KillingMachine ||
                        effect.Trigger == Trigger.RuneStrikeHit
                    )
                { bRelevant = true; }
                else if (relevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.PhysicalAttack ||
                        effect.Trigger == Trigger.Use)
                    {
                        foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                        {
                            if (!bRelevant)
                                bRelevant = HasRelevantStats(e.Stats);
                        }
                        if (!bRelevant)
                            bRelevant = relevantStats(effect.Stats);
                        
                    }
                }
            }
            if (!bRelevant)
                bRelevant = relevantStats(stats);
            return bRelevant;
        }

        private bool relevantStats(Stats stats)
        {
            bool bResults = false;
            // Core stats
            bResults |= (stats.Strength != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.AttackPower != 0);
            bool bHasCore = bResults; // if the above stats are 0, lets make sure we're not bringing in caster gear below.
            // Other Base Stats
            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.MasteryRating != 0);
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.Armor != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.Resilience != 0);

            // Secondary Stats
            bResults |= (stats.Health != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
            bResults |= (stats.SpellCrit != 0);
            bResults |= (stats.SpellCritOnTarget != 0);
            bResults |= (stats.SpellHit != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.SpellPenetration != 0);

            // Dam stats
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.PhysicalDamage != 0);
            bResults |= (stats.ShadowDamage != 0);
            bResults |= (stats.ArcaneDamage != 0);
            bResults |= (stats.FireDamage != 0);
            bResults |= (stats.FrostDamage) != 0;
            bResults |= (stats.HolyDamage) != 0;
            bResults |= (stats.NatureDamage) != 0;

            // Bonus to stat
            bResults |= (stats.BonusHealthMultiplier != 0);
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= ( stats.BonusStaminaMultiplier != 0);
            bResults |= ( stats.BonusAgilityMultiplier != 0);
            bResults |= ( stats.BonusCritDamageMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);

            // Bonus to Dam
            // *Dam
            bResults |= (stats.BonusWhiteDamageMultiplier != 0);
            bResults |= (stats.BonusDamageMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= ( stats.BonusShadowDamageMultiplier != 0);
            bResults |= ( stats.BonusFrostDamageMultiplier != 0);
            bResults |= ( stats.BonusDiseaseDamageMultiplier  != 0);
            // +Dam
            bResults |= (stats.BonusFrostWeaponDamage != 0);
            bResults |= (stats.BonusDamageScourgeStrike != 0);
            bResults |= (stats.BonusDamageBloodStrike != 0);
            bResults |= ( stats.BonusDamageDeathCoil != 0); 
            bResults |= ( stats.BonusDamageDeathStrike != 0);  
            bResults |= ( stats.BonusDamageFrostStrike   != 0); 
            bResults |= ( stats.BonusDamageHeartStrike != 0);  
            bResults |= ( stats.BonusDamageIcyTouch != 0);  
            bResults |= ( stats.BonusDamageObliterate != 0);
            // Crit
            bResults |= (stats.BonusCritDamageMultiplier != 0);
            bResults |= (stats.BonusCritChanceDeathCoil != 0);
            bResults |= ( stats.BonusCritChanceFrostStrike != 0); 
            bResults |= ( stats.BonusCritChanceObliterate != 0); 
            // Other
            bResults |= ( stats.CinderglacierProc != 0); 
            bResults |= ( stats.HighestStat != 0);
            bResults |= ( stats.HighestSecondaryStat != 0); 
            bResults |= ( stats.Paragon != 0); 
            bResults |= (stats.ThreatIncreaseMultiplier != 0);
            bResults |= (stats.ThreatReductionMultiplier != 0);
            bResults |= (stats.TargetArmorReduction != 0);
            // BossHandler
            bResults |= (stats.SnareRootDurReduc != 0); 
            bResults |= (stats.FearDurReduc != 0); 
            bResults |= (stats.StunDurReduc != 0); 
            bResults |= (stats.MovementSpeed != 0); 

            bResults |= !(HasIgnoreStats(stats));
            return bResults;
        }

        private bool HasIgnoreStats(Stats stats)
        {
            if (!HidingBadStuff) { return false; }
            bool retVal = false;
            retVal = (
                // Remove Spellcasting Stuff
                (HidingBadStuff_Spl ? stats.Mp5 + stats.SpellPower + stats.Mana + stats.ManaRestore + stats.Spirit + stats.Intellect
                                    + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier
                                    + stats.SpellPenetration + stats.BonusManaMultiplier
                                    : 0f)
                // Remove Defensive Stuff (until we do that special modeling)
                + (HidingBadStuff_Def ? stats.Dodge + stats.Parry
                                      + stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.Block
                                      + stats.SpellReflectChance
                                      : 0f)
                // Remove PvP Items
                + (HidingBadStuff_PvP ? stats.Resilience : 0f)
                ) > 0;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                //if (RelevantTriggers.Contains(effect.Trigger)) 
                //retVal |= !RelevantTriggers.Contains(effect.Trigger);
                retVal |= HasIgnoreStats(effect.Stats);
                if (retVal) break;
                //}
            }

            return retVal;
        }
        #endregion

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Crit Rating",
                        "Expertise Rating",
                        "Hit Rating",
                        "Haste Rating",
                        "Mastery",
                        "Target Miss %",
                        "Target Dodge %",
                        "Resilience",
                        "Spell Penetration"
                    };

                return _optimizableCalculationLabels;
            }
        }
        public override void SetDefaults(Character character)
        {
            // Need to be behind boss
            character.BossOptions.InBack = true;
            character.BossOptions.InBackPerc_Melee = 1.00d;
        }

        public static void RemoveDuplicateRunes(Stats statsBaseGear, Character character, bool bDW)
        {
            if (bDW // Check for DW.
                && (character.MainHandEnchant != null && character.OffHandEnchant != null) // Check that both weapons have enchants.
                && character.MainHandEnchant == character.OffHandEnchant) // check that we have duplicate enchants.
            {
                bool bEffect1Found = false;
                bool bEffect2Found = false;
                switch (character.MainHandEnchant.Id)
                {
                    case 3368: // FC
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_FC1))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (bEffect2Found && se1.Equals(_SE_FC2))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (se1.Equals(_SE_FC1))
                                bEffect1Found = true;
                            else if (se1.Equals(_SE_FC2))
                                bEffect2Found = true;
                        }
                        break;
                    case 3369: // Cinder
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_CG))
                                statsBaseGear.RemoveSpecialEffect(se1);
                            else if (se1.Equals(_SE_CG))
                                bEffect1Found = true;
                        }
                        break;
                    case 3370: // RazorIce
                        foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                        {
                            // if we've already found them, and we're seeing them again, then remove these repeats.
                            if (bEffect1Found && se1.Equals(_SE_RI))
                            {
                                statsBaseGear.BonusFrostWeaponDamage -= .02f;
                                statsBaseGear.RemoveSpecialEffect(se1);
                            }
                            else if (se1.Equals(_SE_RI))
                                bEffect1Found = true;
                        }
                        break;
                }

            }
        }

        #region Static SpecialEffects
        // Enchant: Rune of Fallen Crusader
        public static readonly SpecialEffect _SE_FC1 = new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1, false);
        public static readonly SpecialEffect _SE_FC2 = new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1, false);
        // Enchant: Rune of Razorice
        public static readonly SpecialEffect _SE_RI = new SpecialEffect(Trigger.MeleeHit, new Stats() { BonusFrostDamageMultiplier = 0.02f }, 20f, 0f, 1f, 5);
        // Enchant: Rune of Cinderglacier
        public static readonly SpecialEffect _SE_CG = new SpecialEffect(Trigger.DamageDone, new Stats() { CinderglacierProc = 2f }, 0f, 0f, -1.5f);
        // Icebound Fort
        private static readonly SpecialEffect[] _SE_IBF = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f }, 12 * 1.0f, 3 * 60  ), // Default IBF
            new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenReductionMultiplier = 0.20f }, 12 * 1.5f, 3 * 60  ), // IBF w/ 4T11
        };
        public static readonly SpecialEffect[] _SE_VampiricBlood = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, new Stats() {HealingReceivedMultiplier = .25f, BonusHealthMultiplier = .15f}, 10, 60f), // No Glyph
            new SpecialEffect(Trigger.Use, new Stats() {HealingReceivedMultiplier = .25f + .15f}, 10, 60f) // Glyphed
        };
        // Talent: Rune Tap
        public static readonly SpecialEffect _SE_RuneTap = new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .1f }, 0, 30f);
        // Talent: Killing Machine
        /*
        public static readonly SpecialEffect[] _KM = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.MeleeAttack, new Stats() { BonusCritChanceObliterate = 1f, BonusCritChanceFrostStrike = 1f }, 3f, 0f, (-5f * 1/3), false),
            new SpecialEffect(Trigger.MeleeAttack, new Stats() { BonusCritChanceObliterate = 1f, BonusCritChanceFrostStrike = 1f }, 3f, 0f, (-5f * 2/3), false),
            new SpecialEffect(Trigger.MeleeAttack, new Stats() { BonusCritChanceObliterate = 1f, BonusCritChanceFrostStrike = 1f }, 3f, 0f, (-5f * 3/3), false),
        };
         * */
        public static SpecialEffect[] _SE_Bloodworms = new SpecialEffect[3];
        /// <summary>
        /// When a damaging attack brings you below 30% of your maximum health, the cooldown on your Rune Tap
        /// ability is refreshed and your next Rune Tap has no cost, and all damage taken is reduced by [25/3*Pts]%
        /// for 8 sec. This effect cannot occur more than once every 45 seconds.
        /// </summary>
        public static readonly SpecialEffect[] _SE_WillOfTheNecropolis = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenReductionMultiplier = (0.25f / 3f * 1f) }, 8, 45, 0.30f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenReductionMultiplier = (0.25f / 3f * 2f) }, 8, 45, 0.30f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenReductionMultiplier = (0.25f / 3f * 3f) }, 8, 45, 0.30f),
        };
        public static readonly SpecialEffect[][] _SE_UnbreakableArmor = new SpecialEffect[][] {
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 0 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 0 * 10f),
            },
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 1 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 1 * 10f),
            },
        };
        public static readonly SpecialEffect _SE_AntiMagicZone = new SpecialEffect(Trigger.Use, new Stats() { SpellDamageTakenReductionMultiplier = 0.75f }, 10f, 2f * 60f);

        public static readonly SpecialEffect _SE_PillarOfFrost = new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = .2f }, 20f, 60);
        public static readonly SpecialEffect[][] _SE_DRW = new SpecialEffect[][] {
            new SpecialEffect[] { // No 4T12 Bonus
                new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.5f, Parry = .20f }, 12f, 1.5f * 60f), // Normal
                new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.5f, Parry = .20f, ThreatIncreaseMultiplier = 0.50f }, 12f, 1.5f * 60f), // Glyphed
            },
            new SpecialEffect[] { // 4T12 Bonus
                new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.5f, Parry = (.20f + .15f)/2 }, 12f * 2f, 1.5f * 60f), // Normal
                new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.5f, Parry = (.20f + .15f)/2 , ThreatIncreaseMultiplier = 0.50f }, 12f * 2f, 1.5f * 60f), // Glyphed
            }
        };

        #endregion
    }
}
