using System;
#if RAWR3 || RAWR4
using System.Windows.Media;
#else
using System.Drawing;
#endif
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    [Rawr.Calculations.RawrModelInfo("RestoSham", "Spell_Nature_Magicimmunity", CharacterClass.Shaman)]
    public class CalculationsRestoSham : CalculationsBase
    {
        #region Base mana at level 80 and 85
        const float BaseMana = 4396f;
        #endregion
        #region Carry over calculations
        public float HealPerSec { get; set; }
        public float HealHitPerSec { get; set; }
        public float CritPerSec { get; set; }
        public float FightSeconds { get; set; }
        public float castingActivity { get; set; }
        #endregion
        #region Setup Character Defaults (Buffs, Gem Templates)
        /// <summary>
        /// Sets the defaults for a RestoShaman character
        /// </summary>
        /// <param name="character">The character object to set the defaults for</param>
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Improved Moonkin Form"));
            character.ActiveBuffsAdd(("Tree of Life Aura"));
            character.ActiveBuffsAdd(("Arcane Intellect"));
            character.ActiveBuffsAdd(("Vampiric Touch"));
            character.ActiveBuffsAdd(("Mana Spring Totem"));
            character.ActiveBuffsAdd(("Restorative Totems"));
            character.ActiveBuffsAdd(("Moonkin Form"));
            character.ActiveBuffsAdd(("Wrath of Air Totem"));
            character.ActiveBuffsAdd(("Totem of Wrath (Spell Power)"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Improved Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Improved Mark of the Wild"));
            character.ActiveBuffsAdd(("Blessing of Kings"));
            character.ActiveBuffsAdd(("Flask of the Frost Wyrm"));
            character.ActiveBuffsAdd(("Spell Power Food"));
        }
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for Resto Shaman - Goes Normal, Rare, Epic, and JC
                //Red
                int[] Runed = { 39911, 39998, 40113, 42144 };

                //Purple
                int[] Royal = { 39943, 40027, 40134 };
                int[] Glowing = { 39936, 40025, 40132 };

                //Blue
                int[] Lustrous = { 39927, 40010, 40121, 42146 };

                //Green
                int[] Dazzling = { 39984, 40094, 40175 };
                int[] Energized = { 39989, 40105, 40179 };
                int[] Sundered = { 39985, 40096, 40176 };

                //Yellow
                int[] Brilliant = { 39912, 40012, 40123, 42148 };
                int[] Quick = { 39918, 40017, 40128, 42150 };
                int[] Smooth = { 39914, 40013, 40124, 42149 };

                //Orange
                int[] Luminous = { 39946, 40047, 40151 };
                int[] Reckless = { 39959, 40051, 40155 };
                int[] Potent = { 39956, 40048, 40152 };

                //Meta
                int Ember = 41333;
                int Insightful = 41401;
                int Revitalizing = 41376;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //Max Spellpower
						RedId = Runed[0], YellowId = Runed[0], BlueId = Runed[0], PrismaticId = Runed[0], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //SP Int
						RedId = Runed[0], YellowId = Luminous[0], BlueId = Glowing[0], PrismaticId = Runed[0], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //Heavy MP5
						RedId = Royal[0], YellowId = Dazzling[0], BlueId = Dazzling[0], PrismaticId = Lustrous[0], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //SP MP5
						RedId = Runed[0], YellowId = Dazzling[0], BlueId = Royal[0], PrismaticId = Runed[0], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //SP Haste
						RedId = Runed[0], YellowId = Reckless[0], BlueId = Energized[0], PrismaticId = Quick[0], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //SP Crit
						RedId = Runed[0], YellowId = Potent[0], BlueId = Sundered[0], PrismaticId = Smooth[0], MetaId = Revitalizing },

					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Runed[1], YellowId = Runed[1], BlueId = Runed[1], PrismaticId = Runed[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Runed[1], YellowId = Luminous[1], BlueId = Glowing[1], PrismaticId = Runed[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Royal[1], YellowId = Dazzling[1], BlueId = Dazzling[1], PrismaticId = Lustrous[1], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Runed[1], YellowId = Dazzling[1], BlueId = Royal[1], PrismaticId = Runed[1], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Runed[1], YellowId = Reckless[1], BlueId = Energized[1], PrismaticId = Quick[1], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare",
						RedId = Runed[1], YellowId = Potent[1], BlueId = Sundered[1], PrismaticId = Smooth[1], MetaId = Revitalizing },

					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Max Haste + Insight
						RedId = Quick[2], YellowId = Quick[2], BlueId = Quick[2], PrismaticId = Quick[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Int + Insight
						RedId = Luminous[2], YellowId = Luminous[2], BlueId = Dazzling[2], PrismaticId = Luminous[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Heavy Haste + Insight
						RedId = Reckless[2], YellowId = Quick[2], BlueId = Energized[2], PrismaticId = Quick[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Crit + Insight
						RedId = Runed[2], YellowId = Potent[2], BlueId = Sundered[2], PrismaticId = Smooth[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Max Haste + Revitalizing
						RedId = Quick[2], YellowId = Quick[2], BlueId = Quick[2], PrismaticId = Quick[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Int + Revitalizing
						RedId = Luminous[2], YellowId = Luminous[2], BlueId = Dazzling[2], PrismaticId = Luminous[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Heavy Haste + Revitalizing
						RedId = Reckless[2], YellowId = Quick[2], BlueId = Energized[2], PrismaticId = Quick[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Crit + Revitalizing
						RedId = Runed[2], YellowId = Potent[2], BlueId = Sundered[2], PrismaticId = Smooth[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //SP MP5
						RedId = Runed[2], YellowId = Dazzling[2], BlueId = Royal[2], PrismaticId = Runed[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Heavy MP5
						RedId = Royal[2], YellowId = Dazzling[2], BlueId = Dazzling[2], PrismaticId = Lustrous[2], MetaId = Insightful },

					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Haste + Insight
						RedId = Quick[3], YellowId = Quick[3], BlueId = Quick[3], PrismaticId = Quick[3], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //SP Int + Insight
						RedId = Runed[3], YellowId = Brilliant[3], BlueId = Dazzling[2], PrismaticId = Luminous[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Heavy Haste + Insight
						RedId = Reckless[2], YellowId = Quick[3], BlueId = Energized[2], PrismaticId = Quick[3], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //SP Crit + Insight
						RedId = Runed[3], YellowId = Smooth[3], BlueId = Sundered[2], PrismaticId = Smooth[3], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Haste + Revitalizing
						RedId = Quick[3], YellowId = Quick[3], BlueId = Quick[3], PrismaticId = Quick[3], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //SP Int + Revitalizing
						RedId = Runed[3], YellowId = Brilliant[3], BlueId = Dazzling[2], PrismaticId = Luminous[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Heavy Haste + Revitalizing
						RedId = Reckless[2], YellowId = Quick[3], BlueId = Energized[2], PrismaticId = Quick[3], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //SP Crit + Revitalizing
						RedId = Runed[3], YellowId = Smooth[3], BlueId = Sundered[2], PrismaticId = Smooth[3], MetaId = Revitalizing },
				};
            }
        }
        public override List<ItemType> RelevantItemTypes
        {
            get { return Relevants.RelevantItemTypes; }
        }
        #endregion
        #region Labels and Charts Overrides
        public override Dictionary<string, Color> SubPointNameColors
        {
            get { return RestoShamConfiguration.SubPointNameColors; }
        }
        public override string[] CharacterDisplayCalculationLabels
        {
            get { return RestoShamConfiguration.CharacterDisplayCalculationLabels; }
        }
        public override string[] OptimizableCalculationLabels
        {
            get { return RestoShamConfiguration.OptimizableCalculationLabels; }
        }
        public override string[] CustomChartNames
        {
            get { return CustomCharts.CustomChartNames; }
        }
#if !RAWR3 && !RAWR4
        // for RAWR3 || RAWR4 include all charts in CustomChartNames
        public override string[] CustomRenderedChartNames
        {
            get { return CustomCharts.CustomRenderedChartNames; }
        }
#endif
        #endregion
        #region Set calculation options and item usable options
#if RAWR3 || RAWR4
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
		public override ICalculationOptionsPanel CalculationOptionsPanel
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
#endif
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelRestoSham();
                return _calculationOptionsPanel;
            }
        }
        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }
        #endregion
        #region Model Verification and prepare Calculations
        //
        // This model is for shammies!
        //
        public override CharacterClass TargetClass
        {
            get { return CharacterClass.Shaman; }
        }


        //
        // Get instances of our calculation classes:
        //
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationRestoSham();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRestoSham();
        }
        #endregion
        #region Calculations Begin
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            return GetCharacterCalculations(character, additionalItem, null);
        }
        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, Stats statModifier)
        {
            Stats stats = GetCharacterStats(character, additionalItem, statModifier);
            CharacterCalculationsRestoSham calcStats = new CharacterCalculationsRestoSham();
            calcStats.BasicStats = stats;
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            if (options == null)
                return calcStats;

            FightSeconds = options.FightLength * 60f;
            castingActivity = 1f;
            #region Spell Power and Haste Based Calcs
            stats.SpellPower += stats.Intellect - 10f;
            stats.SpellPower += 1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f);
            stats.SpellHaste = (1 + (stats.HasteRating / 3279f)) * (1 + stats.SpellHaste) - 1;  // 80 Haste Setting  (PENGUIN)
            /*stats.SpellHaste = (1 + StatConversion.GetSpellHasteFromRating(stats.HasteRating)) * (1 + stats.SpellHaste) - 1;*/ // 85 Haste Setting (PENGUIN)
            calcStats.SpellHaste = stats.SpellHaste;
            float Healing = 1.88f * stats.SpellPower;
            float LBSpellPower = Healing -  (1 * ((1 + character.ShamanTalents.ElementalWeapons * .2f) * 150f));
            #endregion
            #region Overhealing/Latency/Interval/CH jumps values
            float GcdLatency = options.Latency / 1000;
            float Latency = Math.Max(Math.Min(GcdLatency, 0.275f) - 0.14f, 0) + Math.Max(GcdLatency - 0.275f, 0);
#if true
            float ESOverheal = 0;
            float ELWOverheal = 0;
            float CHOverheal = 0;
            float RTOverheal = 0;
            float HWOverheal = 0;
            float HWSelfOverheal = 0;
            float GHWOverheal = 0;
            float GHWSelfOverheal = 0;
            float HSrgOverheal = 0;
            float AAOverheal = 0;
            float HSTOverheal = 0;
            float ESInterval = 0;
#else
            float ESOverheal = 0.1f;
            float ELWOverheal = 0.7f;
            float CHOverheal = 0.7f;
            float RTOverheal = 0.8f;
            float HWOverheal = 0.4f;
            float HWSelfOverheal = 0.9f;
            float HSrgOverheal = 0.75f;
            float AAOverheal = 0.6f;
            float HSTOverheal = 0.9f;
            float ESInterval = 6;
#endif
            bool TankHeal = options.Targets == "Tank";
            bool RaidHeal = options.Targets == "Heavy Raid";
            bool SelfHeal = RaidHeal || options.Targets == "Self";
            calcStats.DeepHeals = .2f + ((stats.MasteryRating / 6.34f) * .025f); // %Health_Deficit*Mastery% = Additional Healing
            #region Theoretical deep healing based off oposite of over-heal.  Will require tweaking.  Much, Much tweaking.
            float DeepHeal = calcStats.DeepHeals * (1 - (CHOverheal + RTOverheal + HWOverheal + HWSelfOverheal + HSrgOverheal) / 5);
            #endregion
            float ELWOverwriteScale = RaidHeal ? 0.875f : TankHeal ? 0.5f : 0.6f;
            float CHRTConsumption = RaidHeal ? 0.07f : TankHeal ? 0.5f : 0.19f;
            float CHJumps = RaidHeal ? 4 : SelfHeal ? 1.73f : TankHeal ? 1.86f : 2.5f;
            float HSTTargets = RaidHeal ? 5f : 1f;
            #endregion
            #region Intellect, Spell Crit, and MP5 Based Calcs
            stats.Mp5 += (float)Math.Round((5f * (0.001f + (float)Math.Sqrt(stats.Intellect) * stats.Spirit * 0.005575f) * 0.60f) / 2); // Level 80 Mana Regen (PENGUIN)
            /*stats.Mp5 += (StatConversion.GetSpiritRegenSec(stats.Spirit, stats.Intellect)) * 2.5f;*/ //Level 85 Regen to be enabled (PENGUIN)
            float CritPenalty = 1f - (((CHOverheal + RTOverheal + HWOverheal + HWSelfOverheal + HSrgOverheal + AAOverheal) / 6f) / 2f);
            stats.SpellCrit = .022f + ((stats.Intellect / (166 + (2 / 3))) / 100) + (stats.CritRating / 4591f) + stats.SpellCrit + (.01f * (character.ShamanTalents.Acuity)); //Level 80 Crit (PENGUIN)
            /*stats.SpellCrit = .022f + StatConversion.GetSpellCritFromIntellect(stats.Intellect)
                + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit +
                (.01f * (character.ShamanTalents.Acuity));*/ //Level 85 Crit (PENGUIN)
            calcStats.SpellCrit = stats.SpellCrit;
            float CriticalScale = 1.5f * (1 + stats.BonusCritHealMultiplier);
            float CriticalChance = calcStats.SpellCrit;
            float Critical = 1f + ((CriticalChance * Math.Min(CriticalScale - 1, 1)) * (CritPenalty));  //  The penalty is set to ensure that while no crit will ever be valued less then a full heal, it will however be reduced more so due to overhealing.  The average currently works out close to current HEP reports and combat logs.
            float ChCritical = 1f + (((CriticalChance + (stats.RestoSham4T9 * .05f)) * Math.Min(CriticalScale - 1, 1)) * (CritPenalty));

            #endregion
            #region Healing Bonuses and scales
            //  Cost scale
            float CostScale = 1f - character.ShamanTalents.TidalFocus * .02f;
            //  Healing scale from Purification
            float PurificationScale = 1.1f + (character.ShamanTalents.SparkOfLife * .02f) + (TankHeal ? .15f : 0f);
            //  AA scale
            float AAScale = CriticalScale * character.ShamanTalents.AncestralAwakening * .1f * PurificationScale;
            //  TW chance
            float TWChance = character.ShamanTalents.TidalWaves * 0.1f;
            #endregion
            #region Water Shield and Mana Calculations
            float Orb;
            /*Create addition to orb if 85 (PENGUIN)*/
            if (options.WaterShield)
            {
                stats.Mp5 += (character.ShamanTalents.GlyphofWaterMastery ? 150 : 100) + 100f * stats.WaterShieldIncrease;
                Orb = (428 * (1 + (character.ShamanTalents.ImprovedShields * .05f))) * (1 + stats.WaterShieldIncrease);
                Orb = Orb * character.ShamanTalents.ImprovedWaterShield / 3;
            }
            else
                Orb = 0;
            #endregion
            #region Earthliving Weapon Calculations
            //  ELW bonus healing = spell power
            float ELWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            ELWBonusHealing *= 1.88f * 0.3636f;
            //  ELW healing scale = purification scale
            float ELWHealingScale = PurificationScale + (character.ShamanTalents.GlyphofEarthlivingWeapon ? .2f : 0);
            float ELWHPS = ((652 + ELWBonusHealing) * ELWHealingScale / 12) * (1 + stats.BonusHealingDoneMultiplier);
            float ELWChance = 1 * ELWOverwriteScale;
            #endregion
            #region Earth Shield Calculations
            bool UseES = (options.EarthShield ); // Wether or not to use ES at all - Make sure the option and the talent are on.
            float ESBonusHealing = stats.SpellPower;  //  ES bonus healing = spell power
            ESBonusHealing *= 1.88f * (1f / 3.5f);  //  ... * generic healing scale * HoT scale
            float ESHealingScale = PurificationScale;  //  ES healing scale = purification scale
            //  ... + 5%/10% Improved Earth Shield + 5%/10%/15% Improved Shields
            ESHealingScale *= 1 + character.ShamanTalents.ImprovedShields * 0.05f;
            //  ... + 20% if Glyphed
            if (character.ShamanTalents.GlyphofEarthShield)
                ESHealingScale *= 1.2f;
            float ESChargeHeal = (337 + ESBonusHealing) * ESHealingScale * (UseES ? 1 : 0); //  Heal per ES Charge
            float ESHeal = ESChargeHeal * 9;  //  ES if all charges heal
            float ESCost = (float)Math.Round(.15 * BaseMana) * CostScale * (UseES ? 1 : 0);
            float ESTimer = 9 * Math.Max(ESInterval, 4);
            calcStats.ESHPS = ((ESHeal * Critical) / ESTimer) * (1 + stats.BonusHealingDoneMultiplier);
            float ESMPS = ESCost / ESTimer;
            #endregion
            #region Base Variables ( Heals per sec and Crits per sec )
            float RTPerSec = 0;
            float RTTicksPerSec = 0;
            float HWPerSec = 0;
            float GHWPerSec = 0;
            float GHWCPerSec = 0;
            float CHPerSec = 0;
            float CHHitsPerSec = 0;
            float HSrgPerSec = 0;
            float HSrgCPerSec = 0f;
            float CHCPerSec = 0f;
            float CHCHitsPerSec = 0;
            float HWCPerSec = 0f;
            float RTCPerSec = 0f;
            float AAsPerSec = 0f;
            float ELWTicksPerSec = 0;
            #endregion
            #region Base Speeds ( Hasted / RTCast / HSrgCast / HWCast / CHCast )
            float HasteScale = 1f / (1f + calcStats.SpellHaste);
            float RTHaste = stats.RestoSham2T10 * 0.2f;
            float RTCast = (float)Math.Max(1.5f * HasteScale + Latency, 1f + GcdLatency);
            float RTCD = 6 - stats.RTCDDecrease;
            float RTCDCast = RTCD + GcdLatency;
            float RTDuration = 15 + (character.ShamanTalents.GlyphofRiptide ? 6 : 0);
            float HRCast = (float)Math.Max(1.5f * HasteScale + Latency, 1f + GcdLatency);
            float HRCD = 6 - stats.RTCDDecrease;
            float HRCDCast = HRCD + GcdLatency;
            float HRDuration = 15;
            float ELWDuration = 12;
            float HWCastBase = 2.5f;
            float GHWCastBase = 2.5f;
            calcStats.RealHWCast = HWCastBase * HasteScale;
            float HWCast = (float)Math.Max(HWCastBase * HasteScale + Latency, 1f + GcdLatency);
            float HWCastTWLatency = (Latency * 0.25f + GcdLatency * 0.75f) * TWChance + (Latency * 0.5f + GcdLatency * 0.5f) * (1 - TWChance);
            float HWCastTW = (float)Math.Max(HWCastBase * HasteScale * 0.7f + HWCastTWLatency, 1f + GcdLatency);
            float HWCast_RT = (float)Math.Max(HWCastBase / (1f + calcStats.SpellHaste + RTHaste), 1f) + GcdLatency;
            float HWCastTW_RT = (float)Math.Max(HWCastBase / (1f + calcStats.SpellHaste + RTHaste) * 0.7f + HWCastTWLatency, 1f + GcdLatency);
            calcStats.RealGHWCast = GHWCastBase * HasteScale;
            float GHWCast = (float)Math.Max(GHWCastBase * HasteScale + Latency, 1f + GcdLatency);
            float GHWCastTWLatency = (Latency * 0.25f + GcdLatency * 0.75f) * TWChance + (Latency * 0.5f + GcdLatency * 0.5f) * (1 - TWChance);
            float GHWCastTW = (float)Math.Max(GHWCastBase * HasteScale * 0.7f + GHWCastTWLatency, 1f + GcdLatency);
            float GHWCast_RT = (float)Math.Max(GHWCastBase / (1f + calcStats.SpellHaste + RTHaste), 1f) + GcdLatency;
            float GHWCastTW_RT = (float)Math.Max(GHWCastBase / (1f + calcStats.SpellHaste + RTHaste) * 0.7f + GHWCastTWLatency, 1f + GcdLatency);
            calcStats.RealHSrgCast = 1.5f * HasteScale;
            float HSrgCast = (float)Math.Max(1.5f * HasteScale, 1f) + GcdLatency;
            float HSrgCast_RT = (float)Math.Max(1.5f / (1f + calcStats.SpellHaste + RTHaste), 1f) + GcdLatency;
            float CHCastBase = 2.5f - stats.CHCTDecrease;
            calcStats.RealCHCast = CHCastBase * HasteScale;
            float CHCast = (float)Math.Max(CHCastBase * HasteScale + Latency, 1f + GcdLatency);
            float CHCast_RT = (float)Math.Max(CHCastBase / (1f + calcStats.SpellHaste + RTHaste), 1f) + GcdLatency;
            // This totally heals the boss backwards! Yeah! :D
            // Don't worry about this messing with procs or anything, it's just to show on the stats page. :)
            calcStats.LBCast = (float)Math.Max(2.5f * HasteScale, 1f);
            calcStats.LBRestore = (((((645 + 735) / 2) + LBSpellPower) * 1.08f) * (.2f * character.ShamanTalents.TelluricCurrents)) - (BaseMana * .06f); //Make an 85 Version (719+831) (PENGUIN)
            #endregion
            #region Base Spells ( TankCH / RTHeal / HSrgHeal / GHWHeal / HWHeal / CHHeal )
            #region Riptide area
            //  RT bonus healing = spell power
            float RTBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            RTBonusHealing *= 1.88f * (1f / 3.5f);
            //  RT healing scale = purification scale
            float RTHealingScale = PurificationScale + DeepHeal;
            //  ... + 20% 2pc T9 bonus
            RTHealingScale *= 1 + stats.RestoSham2T9 * .2f;
            //  ... set to zero if RT talent is not taken
            float RTHeal = (1670 + RTBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            //  RT HoT bonus healing = spell power
            float RTHotBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            RTHotBonusHealing *= 1.88f * 0.5f;
            float RTHotHeal = (1670 + RTHotBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            float RTHotTickHeal = RTHotHeal / 5;
            RTHotHeal = RTDuration / 3 * RTHotTickHeal;
            float RTHotHPS = RTHotTickHeal / 3;
            #endregion
            #region Healing Rains
            //  RT bonus healing = spell power
            float HRBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            HRBonusHealing *= 1.88f * (1f / 3.5f);
            //  RT healing scale = purification scale
            float HRHealingScale = PurificationScale;
            //  ... + 20% 2pc T9 bonus
            HRHealingScale *= 1f;
            //  ... set to zero if RT talent is not taken
            float HRHeal = (1670 + HRBonusHealing) * HRHealingScale;
            //  RT HoT bonus healing = spell power
            float HRHotBonusHealing = stats.SpellPower;
            //  ... * generic healing scale * HoT scale
            HRHotBonusHealing *= 1.88f * 0.5f;
            float HRHotHeal = (1670 + RTHotBonusHealing) * RTHealingScale * character.ShamanTalents.Riptide;
            float HRHotTickHeal = HRHotHeal / 5;
            HRHotHeal = HRDuration / 3 * HRHotTickHeal;
            float HRHotHPS = HRHotTickHeal / 3;
            #endregion
            #region Healing Surge Area
            //  HSrg bonus healing = spell power + totem spell power bonus
            float HSrgBonusHealing = stats.SpellPower;
            //  ... * generic healing scale + bonus from TW
            HSrgBonusHealing *= 1.88f * (1.5f / 3.5f);
            //  HSrg healing scale = purification scale
            float HSrgHealingScale = PurificationScale + DeepHeal;
            float HSrgHeal = (1720 + HSrgBonusHealing) * HSrgHealingScale;
            #endregion
            #region Healing Wave Area
            //  HW bonus healing = spell power + totem spell power bonus
            float HWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale + bonus from TW
            HWBonusHealing *= 1.88f * (3.0f / 3.5f) + character.ShamanTalents.TidalWaves * .04f;
            //  HW healing scale = purification scale
            float HWHealingScale = PurificationScale + DeepHeal;
            //  ... + 8%/16%/25% Healing Way bonus
            HWHealingScale *= 1f / 3f;
            //  ... + 5% 4pc T7 bonus
            HWHealingScale *= 1 + stats.CHHWHealIncrease;
            float HWHeal = (3250 + HWBonusHealing) * HWHealingScale;
            //  HW self-healing scale = 20% if w/Glyph (no longer benefits from Purification since patch 3.2)
            float HWSelfHealingScale = SelfHeal && character.ShamanTalents.GlyphofHealingWave ? 0.2f : 0;
            //      * correction due to the fact it's just not smart to use GoHW for self-healing if you're _really_ hammered down
            HWSelfHealingScale *= 1f / HWHealingScale;
            #endregion
            #region Greater Healing Wave Area
            //  HW bonus healing = spell power
            float GHWBonusHealing = stats.SpellPower;
            //  ... * generic healing scale
            GHWBonusHealing *= 1.88f * (3.0f / 3.5f);
            //  HW healing scale = purification scale
            float GHWHealingScale = PurificationScale + DeepHeal;
            GHWHealingScale *= 1f / 3f;
            //  ... + 5% 4pc T7 bonus
            GHWHealingScale *= 1 + stats.CHHWHealIncrease;
            float GHWHeal = (3250 + GHWBonusHealing) * GHWHealingScale;
            //  HW self-healing scale = 20% if w/Glyph (no longer benefits from Purification since patch 3.2)
            float GHWSelfHealingScale = SelfHeal && character.ShamanTalents.GlyphofHealingWave ? 0.2f : 0;
            //      * correction due to the fact it's just not smart to use GoHW for self-healing if you're _really_ hammered down
            GHWSelfHealingScale *= 1f / GHWHealingScale;
            #endregion
            #region Chain Heal Area
            //  CH bonus healing = spell power
            float CHBonusHealing = stats.SpellPower;
            //  ... * generic healing scale
            CHBonusHealing *= 1.88f * (2.5f / 3.5f);
            float CHHealingScale = PurificationScale + DeepHeal;
            CHHealingScale *= 1f;
            //  ... + 5% 4pc T7 + HoT 4pc T10
            CHHealingScale *= 1f + stats.CHHWHealIncrease + .25f * CriticalChance * stats.RestoSham4T10;
            float CHHeal = (1130 + CHBonusHealing) * (CHHealingScale - (character.ShamanTalents.GlyphofChainHeal ? .1f : 0));
            float CHJumpHeal = 0;
            float scale = 1f;
            int jump;
            for (jump = 0; jump < CHJumps; jump++)
            {
                CHJumpHeal += scale;
                scale *= 0.3f + (character.ShamanTalents.GlyphofChainHeal ? .15f : 0);
            }
            CHJumpHeal += scale * (CHJumps - jump);
            CHJumpHeal *= CHHeal;
            #endregion
            #region Healing Stream Totem Area
            //  HST bonus healing = spell power
            float HSTBonusHealing = stats.SpellPower;
            //      * generic healing scale * HoT scale
            HSTBonusHealing *= 1.88f * 0.044f;
            //  HST healing scale = purification scale
            float HSTHealingScale = PurificationScale;
            //      + 25%/50% Healing Rains
            HSTHealingScale *= 1 + (.25f * character.ShamanTalents.SoothingRains);
            #endregion
            #endregion
            #region Base Costs ( Preserve / RTCost / HSrgCost / CHCost )
            float Preserve = stats.ManacostReduceWithin15OnHealingCast * .02f;
            float RTCost = ((float)Math.Round(BaseMana * .18f) - Preserve) * CostScale;
            float HRCost = ((float)Math.Round(BaseMana * .46f) - Preserve) * CostScale;
            float HSrgCost = ((float)Math.Round(BaseMana * .27f) - Preserve) * CostScale;
            float HWCost = ((float)Math.Round(BaseMana * .09f) - Preserve) * CostScale;
            float GHWCost = ((float)Math.Round(BaseMana * .30f) - Preserve) * CostScale;
            float HRNCost = ((float)Math.Round(BaseMana * .46f) - Preserve) * CostScale;
            float DecurseCost = ((float)Math.Round(BaseMana * .14f) - Preserve) * CostScale;
            float CHCost = ((float)Math.Round(BaseMana * .17f) - Preserve) * CostScale;
            #endregion
            #region RT + HSrg Rotation (RTHSrgMPS / RTHSrgHPS / RTHSrgTime)  (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTHSrgTime = RTCast;
                float RTHSrgRemainingTime = RTCDCast - RTHSrgTime;
                int RTHSrgHSrgCasts = 0;
                if (RTHSrgRemainingTime > GcdLatency)
                {
                    RTHSrgRemainingTime -= HSrgCast_RT;
                    RTHSrgTime += HSrgCast_RT;
                    ++RTHSrgHSrgCasts;
                    if (RTHSrgRemainingTime > GcdLatency)
                    {
                        int RTHSrgHSrgRemainingCasts = (int)Math.Ceiling((RTHSrgRemainingTime - GcdLatency) / HSrgCast);
                        RTHSrgTime += RTHSrgHSrgRemainingCasts * HSrgCast;
                        RTHSrgHSrgCasts += RTHSrgHSrgRemainingCasts;
                    }
                }
                float RTHSrgHSrgCrits = Math.Min(2, RTHSrgHSrgCasts) * Math.Min(CriticalChance + TWChance * 0.25f, 1) + (RTHSrgHSrgCasts - 2) * CriticalChance;
                float RTHSrgRTHeal = RTHeal * Critical;
                float RTHSrgHSrgCritHeal = HSrgHeal * Critical;
                float RTHSrgHSrgHeal = (HSrgHeal * (RTHSrgHSrgCasts - RTHSrgHSrgCrits)) + (RTHSrgHSrgCrits * RTHSrgHSrgCritHeal);
                float RTHSrgAA = (RTHeal * CriticalChance + HSrgHeal * RTHSrgHSrgCrits) * AAScale;
                float RTTargets = TankHeal ? 1 : RTDuration / RTHSrgTime;
                float RTHSrgELWTargets = ELWChance * (TankHeal ? 1 : RTHSrgHSrgCasts * ELWDuration / RTHSrgTime);
                calcStats.RTHSrgHPS = (((RTHSrgRTHeal * (1 - RTOverheal) + RTHSrgHSrgHeal * (1 - HSrgOverheal) + RTHSrgAA * (1 - AAOverheal)) / RTHSrgTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTHSrgELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calcStats.RTHSrgMPS = ((RTCost + (HSrgCost * RTHSrgHSrgCasts)) / RTHSrgTime) * castingActivity;
                if (options.SustStyle.Equals("RT+HSrg"))
                {
                    RTPerSec = 1f / RTHSrgTime;
                    RTTicksPerSec = RTTargets / 3;
                    HSrgPerSec = RTHSrgHSrgCasts / RTHSrgTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    HSrgCPerSec = RTHSrgHSrgCrits / RTHSrgTime;
                    AAsPerSec += (CriticalChance + RTHSrgHSrgCrits) / RTHSrgTime;
                    ELWTicksPerSec += RTHSrgELWTargets;
                }
            }
            #endregion
            #region RT + HW Rotation (RTHWMPS / RTHWHPS / RTHWTime) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTHWTime = RTCast;
                float RTHWRemainingTime = RTCDCast - RTHWTime;
                int RTHWHWCasts = 0;
                if (RTHWRemainingTime > GcdLatency)
                {
                    float RTHWHWCastTW_RT = HWCastTW_RT * TWChance + HWCast_RT * (1 - TWChance);
                    RTHWRemainingTime -= RTHWHWCastTW_RT;
                    RTHWTime += RTHWHWCastTW_RT;
                    ++RTHWHWCasts;
                    if (RTHWRemainingTime > GcdLatency)
                    {
                        float RTHWHWCastTW = HWCastTW * TWChance + HWCast * (1 - TWChance);
                        RTHWRemainingTime -= RTHWHWCastTW;
                        RTHWTime += RTHWHWCastTW;
                        ++RTHWHWCasts;
                        if (RTHWRemainingTime > GcdLatency)
                        {
                            int RTHWHWRemainingCasts = (int)Math.Ceiling((RTHWRemainingTime - GcdLatency) / HWCast);
                            RTHWTime += RTHWHWRemainingCasts * HWCast;
                            RTHWHWCasts += RTHWHWRemainingCasts;
                        }
                    }
                }
                float RTHWRTHeal = RTHeal * Critical;
                float RTHWHWHCrits = RTHWHWCasts * CriticalChance;
                float RTHWHWHeal = HWHeal * RTHWHWCasts * Critical;
                float RTHWHWSelfHeal = RTHWHWHeal * HWSelfHealingScale * Critical;
                float RTHWAA = (RTHeal * CriticalChance + HWHeal * RTHWHWHCrits) * AAScale;
                //  Multi-target ELW handling not in yet due to low priority
                float RTTargets = TankHeal ? 1 : RTDuration / RTHWTime;
                float RTHWELWTargets = ELWChance * (TankHeal ? 1 : RTHWHWCasts * ELWDuration / RTHWTime);
                calcStats.RTHWHPS = (((RTHWRTHeal * (1 - RTOverheal) + RTHWHWHeal * (1 - HWOverheal) + RTHWHWSelfHeal * (1 - HWSelfOverheal) + RTHWAA * (1 - AAOverheal)) / RTHWTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTHWELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calcStats.RTHWMPS = ((RTCost + (HWCost * RTHWHWCasts)) / RTHWTime) * castingActivity;
                if (options.SustStyle.Equals("RT+HW"))
                {
                    RTPerSec = 1f / RTHWTime;
                    RTTicksPerSec = RTTargets / 3;
                    HWPerSec = RTHWHWCasts / RTHWTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    HWCPerSec = RTHWHWHCrits / RTHWTime;
                    AAsPerSec += (CriticalChance + RTHWHWHCrits) / RTHWTime;
                    ELWTicksPerSec += RTHWELWTargets;
                }
            }
            #endregion
            #region RT + GHW Rotation (RTGHWMPS / RTGHWHPS / RTGHWTime) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTGHWTime = RTCast;
                float RTGHWRemainingTime = RTCDCast - RTGHWTime;
                int RTGHWGHWCasts = 0;
                if (RTGHWRemainingTime > GcdLatency)
                {
                    float RTGHWGHWCastTW_RT = GHWCastTW_RT * TWChance + GHWCast_RT * (1 - TWChance);
                    RTGHWRemainingTime -= RTGHWGHWCastTW_RT;
                    RTGHWTime += RTGHWGHWCastTW_RT;
                    ++RTGHWGHWCasts;
                    if (RTGHWRemainingTime > GcdLatency)
                    {
                        float RTGHWGHWCastTW = GHWCastTW * TWChance + GHWCast * (1 - TWChance);
                        RTGHWRemainingTime -= RTGHWGHWCastTW;
                        RTGHWTime += RTGHWGHWCastTW;
                        ++RTGHWGHWCasts;
                        if (RTGHWRemainingTime > GcdLatency)
                        {
                            int RTGHWGHWRemainingCasts = (int)Math.Ceiling((RTGHWRemainingTime - GcdLatency) / GHWCast);
                            RTGHWTime += RTGHWGHWRemainingCasts * GHWCast;
                            RTGHWGHWCasts += RTGHWGHWRemainingCasts;
                        }
                    }
                }
                float RTGHWRTHeal = RTHeal * Critical;
                float RTGHWGHWHCrits = RTGHWGHWCasts * CriticalChance;
                float RTGHWGHWHeal = GHWHeal * RTGHWGHWCasts * Critical;
                float RTGHWGHWSelfHeal = RTGHWGHWHeal * GHWSelfHealingScale * Critical;
                float RTGHWAA = (RTHeal * CriticalChance + GHWHeal * RTGHWGHWHCrits) * AAScale;
                //  Multi-target ELW handling not in yet due to low priority
                float RTTargets = TankHeal ? 1 : RTDuration / RTGHWTime;
                float RTGHWELWTargets = ELWChance * (TankHeal ? 1 : RTGHWGHWCasts * ELWDuration / RTGHWTime);
                calcStats.RTGHWHPS = (((RTGHWRTHeal * (1 - RTOverheal) + RTGHWGHWHeal * (1 - GHWOverheal) + RTGHWGHWSelfHeal * (1 - GHWSelfOverheal) + RTGHWAA * (1 - AAOverheal)) / RTGHWTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTGHWELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calcStats.RTGHWMPS = ((RTCost + (GHWCost * RTGHWGHWCasts)) / RTGHWTime) * castingActivity;
                if (options.SustStyle.Equals("RT+GHW"))
                {
                    RTPerSec = 1f / RTGHWTime;
                    RTTicksPerSec = RTTargets / 3;
                    GHWPerSec = RTGHWGHWCasts / RTGHWTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    GHWCPerSec = RTGHWGHWHCrits / RTGHWTime;
                    AAsPerSec += (CriticalChance + RTGHWGHWHCrits) / RTGHWTime;
                    ELWTicksPerSec += RTGHWELWTargets;
                }
            }
            #endregion
            #region RT + CH Rotation (RTCHMPS / RTCHHPS / RTCHTime / TankCH) (Adjusted based on Casting Activity)
            if (character.ShamanTalents.Riptide != 0)
            {
                float RTCHTime = RTCast;
                float RTCHRemainingTime = RTCDCast - RTCHTime;
                int RTCHCHCasts = 0;
                if (RTCHRemainingTime > GcdLatency)
                {
                    RTCHRemainingTime -= CHCast_RT;
                    RTCHTime += CHCast_RT;
                    ++RTCHCHCasts;
                    if (RTCHRemainingTime > GcdLatency)
                    {
                        int RTCHCHRemainingCasts = (int)Math.Ceiling((RTCHRemainingTime - GcdLatency) / CHCast);
                        RTCHTime += RTCHCHRemainingCasts * CHCast;
                        RTCHCHCasts += RTCHCHRemainingCasts;
                    }
                }
                float RTCHRTHeal = RTHeal * Critical;
                float RTCHCHHeal = (CHJumpHeal + CHHeal * CHRTConsumption * .25f) * RTCHCHCasts * ChCritical;
                float RTCHAA = RTHeal * CriticalChance * AAScale;
                float RTTargets = TankHeal ? Math.Max(RTDuration / RTCHTime - CHRTConsumption, 0) : (RTCast + (RTDuration - RTCast) * (1 - CHRTConsumption)) / RTCHTime;
                float RTCHELWTargets = ELWChance * (CHJumps * RTCHCHCasts + RTTargets) * ELWDuration / RTCHTime;
                calcStats.RTCHHPS = (((RTCHRTHeal * (1 - RTOverheal) + RTCHCHHeal * (1 - CHOverheal) + RTCHAA * (1 - AAOverheal)) / RTCHTime + RTTargets * RTHotHPS * (1 - RTOverheal) + RTCHELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
                calcStats.RTCHMPS = ((RTCost + (CHCost * RTCHCHCasts)) / RTCHTime) * castingActivity;
                if (options.SustStyle.Equals("RT+CH"))
                {
                    RTPerSec = 1f / RTCHTime;
                    RTCPerSec = RTPerSec * CriticalChance;
                    RTTicksPerSec = RTTargets / 3;
                    CHPerSec = RTCHCHCasts / RTCHTime;
                    CHHitsPerSec = CHPerSec * CHJumps;
                    CHCPerSec = RTCHCHCasts * (CriticalChance + (stats.RestoSham4T9 * .05f)) / RTCHTime;
                    CHCHitsPerSec = CHCPerSec * CHJumps;
                    AAsPerSec += CriticalChance / RTCHTime;
                    ELWTicksPerSec += RTCHELWTargets;
                }
            }
            #endregion
            #region CH Spam (CHHPS / CHMPS)
            float CHELWTargets = ELWChance * CHJumps * ELWDuration / CHCast;
            calcStats.CHSpamHPS = ((CHJumpHeal * ChCritical * (1 - CHOverheal) / CHCast + CHELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calcStats.CHSpamMPS = (CHCost / CHCast) * castingActivity;
            if (options.SustStyle.Equals("CH Spam"))
            {
                CHPerSec = 1f / CHCast;
                CHHitsPerSec = CHPerSec * CHJumps;
                CHCPerSec = (CriticalChance + (stats.RestoSham4T9 * .05f)) / CHCast;
                CHCHitsPerSec = CHCPerSec * CHJumps;
                ELWTicksPerSec += CHELWTargets;
            }
            #endregion
            #region HSrg Spam (HSrgHPS / HSrgMPS)
            float HSrgHSrgHeal = HSrgHeal * Critical;
            float HSrgAA = HSrgHeal * CriticalChance * AAScale;
            float HSrgELWTargets = ELWChance * ELWDuration / HSrgCast;
            calcStats.HSrgSpamHPS = (((HSrgHSrgHeal * (1 - HSrgOverheal) + HSrgAA * (1 - AAOverheal)) / HSrgCast + HSrgELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calcStats.HSrgSpamMPS = (HSrgCost / HSrgCast) * castingActivity;
            if (options.SustStyle.Equals("HSrg Spam"))
            {
                HSrgPerSec = 1f / HSrgCast;
                HSrgCPerSec = CriticalChance / HSrgCast;
                ELWTicksPerSec += HSrgELWTargets;
            }
            #endregion
            #region HW Spam (HWHPS / HWMPS)
            float HWHWHeal = HWHeal * Critical;
            float HWHWSelfHeal = HWHWHeal * HWSelfHealingScale * Critical;
            float HWAA = HWHeal * CriticalChance * AAScale;
            float HWELWTargets = ELWChance * ELWDuration / HWCast;
            calcStats.HWSpamHPS = (((HWHWHeal * (1 - HWOverheal) + HWHWSelfHeal * (1 - HWSelfOverheal) + HWAA * (1 - AAOverheal)) / HWCast + HWELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calcStats.HWSpamMPS = (HWCost / HWCast) * castingActivity;
            if (options.SustStyle.Equals("HW Spam"))
            {
                HWPerSec = 1f / HWCast;
                HWCPerSec = CriticalChance / HWCast;
                ELWTicksPerSec += HWELWTargets;
            }
            #endregion
            #region GHW Spam (GHWHPS / GHWMPS)
            float GHWGHWHeal = GHWHeal * Critical;
            float GHWGHWSelfHeal = GHWGHWHeal * GHWSelfHealingScale * Critical;
            float GHWAA = GHWHeal * CriticalChance * AAScale;
            float GHWELWTargets = ELWChance * ELWDuration / GHWCast;
            calcStats.GHWSpamHPS = (((GHWGHWHeal * (1 - GHWOverheal) + GHWGHWSelfHeal * (1 - GHWSelfOverheal) + GHWAA * (1 - AAOverheal)) / GHWCast + GHWELWTargets * ELWHPS * (1 - ELWOverheal)) * castingActivity) * (1 + stats.BonusHealingDoneMultiplier);
            calcStats.GHWSpamMPS = (GHWCost / GHWCast) * castingActivity;
            if (options.SustStyle.Equals("GHW Spam"))
            {
                GHWPerSec = 1f / GHWCast;
                GHWCPerSec = CriticalChance / GHWCast;
                ELWTicksPerSec += GHWELWTargets;
            }
            #endregion
            #region Variables if Riptide not taken in talents
            if (character.ShamanTalents.Riptide == 0)
            {
                calcStats.RTHSrgHPS = calcStats.HSrgSpamHPS;
                calcStats.RTHSrgMPS = calcStats.HSrgSpamMPS;
                calcStats.RTHWHPS = calcStats.HWSpamHPS;
                calcStats.RTHWMPS = calcStats.HWSpamMPS;
                calcStats.RTGHWHPS = calcStats.GHWSpamHPS;
                calcStats.RTGHWMPS = calcStats.GHWSpamMPS;
                calcStats.RTCHHPS = calcStats.CHSpamHPS;
                calcStats.RTCHMPS = calcStats.CHSpamMPS;
            }
            #endregion
            #region Create Final calcs via spell cast (HealPerSec/HealHitPerSec/CritPerSec)
            HealPerSec = (RTPerSec + HSrgPerSec + HWPerSec + CHPerSec + GHWPerSec) * castingActivity;
            HealHitPerSec = (RTPerSec + RTTicksPerSec + HSrgPerSec + HWPerSec + CHHitsPerSec + AAsPerSec + ELWTicksPerSec + GHWPerSec) * castingActivity;
            CritPerSec = (RTCPerSec + HSrgCPerSec + HWCPerSec + CHCPerSec + GHWCPerSec) * castingActivity;
            #endregion
            #region Proc Handling for Mana Restore only
            Stats statsProcs2 = new Stats();
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case (Trigger.HealingSpellCast):
                        if (HealPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / HealPerSec), 1f, 0f, FightSeconds);
                        break;
                    case (Trigger.HealingSpellHit):
                        if (HealHitPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / HealHitPerSec), 1f, 0f, FightSeconds);
                        break;
                    case (Trigger.HealingSpellCrit):
                        if (CritPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / CritPerSec), 1f, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCast):
                        if (HealPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / HealPerSec), 1f, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellHit):
                        if (HealHitPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / HealHitPerSec), 1f, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCrit):
                        if (CritPerSec != 0)
                            effect.AccumulateAverageStats(statsProcs2, (1f / CritPerSec), 1f, 0f, FightSeconds);
                        break;
                    case Trigger.Use:
                        effect.AccumulateAverageStats(statsProcs2, 0f, 1f, 0f, FightSeconds);
                        break;
                }
            }
            #endregion
            #region Calculate Sequence HPS/MPS
            float HSTHPS = (25 + HSTBonusHealing) * HSTHealingScale / 2f * (1 - HSTOverheal);
            calcStats.HSTHeals = HSTHPS * HSTTargets;

            // Burst
            calcStats.BurstSequence = options.BurstStyle;
            float BurstHPS = 0;
            float BurstMUPS = 0;
            switch (options.BurstStyle)
            {
                case "CH Spam":
                    BurstHPS = calcStats.CHSpamHPS;
                    BurstMUPS = calcStats.CHSpamMPS;
                    break;
                case "HW Spam":
                    BurstHPS = calcStats.HWSpamHPS;
                    BurstMUPS = calcStats.HWSpamMPS;
                    break;
                case "GHW Spam":
                    BurstHPS = calcStats.GHWSpamHPS;
                    BurstMUPS = calcStats.GHWSpamMPS;
                    break;
                case "HSrg Spam":
                    BurstHPS = calcStats.HSrgSpamHPS;
                    BurstMUPS = calcStats.HSrgSpamMPS;
                    break;
                case "RT+HW":
                    BurstHPS = calcStats.RTHWHPS;
                    BurstMUPS = calcStats.RTHWMPS;
                    break;
                case "RT+GHW":
                    BurstHPS = calcStats.RTGHWHPS;
                    BurstMUPS = calcStats.RTGHWMPS;
                    break;
                case "RT+HSrg":
                    BurstHPS = calcStats.RTHSrgHPS;
                    BurstMUPS = calcStats.RTHSrgMPS;
                    break;
                case "RT+CH":
                    BurstHPS = calcStats.RTCHHPS;
                    BurstMUPS = calcStats.RTCHMPS;
                    break;
            }

            // Sustained
            calcStats.SustainedSequence = options.SustStyle;
            float SustHPS = 0;
            float SustMUPS = 0;
            switch (options.SustStyle)
            {
                case "CH Spam":
                    SustHPS = calcStats.CHSpamHPS;
                    SustMUPS = calcStats.CHSpamMPS;
                    break;
                case "HW Spam":
                    SustHPS = calcStats.HWSpamHPS;
                    SustMUPS = calcStats.HWSpamMPS;
                    break;
                case "GHW Spam":
                    SustHPS = calcStats.GHWSpamHPS;
                    SustMUPS = calcStats.GHWSpamMPS;
                    break;
                case "HSrg Spam":
                    SustHPS = calcStats.HSrgSpamHPS;
                    SustMUPS = calcStats.HSrgSpamMPS;
                    break;
                case "RT+HW":
                    SustHPS = calcStats.RTHWHPS;
                    SustMUPS = calcStats.RTHWMPS;
                    break;
                case "RT+GHW":
                    SustHPS = calcStats.RTGHWHPS;
                    SustMUPS = calcStats.RTGHWMPS;
                    break;
                case "RT+HSrg":
                    SustHPS = calcStats.RTHSrgHPS;
                    SustMUPS = calcStats.RTHSrgMPS;
                    break;
                case "RT+CH":
                    SustHPS = calcStats.RTCHHPS;
                    SustMUPS = calcStats.RTCHMPS;
                    break;
            }

            calcStats.BurstHPS += calcStats.HSTHeals;
            SustHPS += calcStats.HSTHeals;
            calcStats.MUPS = ((SustMUPS * options.ActivityPerc) + (BurstMUPS * (100 - options.ActivityPerc))) * .01f;
            calcStats.MUPS += (DecurseCost * options.Decurse) / FightSeconds;
            #endregion
            #region Final Stats
            calcStats.LBNumber = options.LBUse;
            float ESUsage = UseES ? (float)Math.Round((FightSeconds / ESTimer), 0) : 0;
            float ESDowntime = (FightSeconds - (RTCast * ESUsage) - 3) / FightSeconds;  // Rip tide cast time is used to simulate ES cast time, as they are exactly the same.  The 3 Simulates the time of two full totem drops.
            calcStats.MAPS = ((stats.Mana) / (FightSeconds))
                + (stats.ManaRestore / FightSeconds)
                + (stats.ManaRestoreFromMaxManaPerSecond * stats.Mana)
                + (stats.Mp5 / 5f)
                + (options.Innervates * 7866f / FightSeconds)
                + statsProcs2.ManaRestore
                + ((RTCPerSec * Orb) * castingActivity * ESDowntime)
                + ((HSrgCPerSec * Orb * .6f) * castingActivity * ESDowntime)
                + ((HWCPerSec * Orb) * castingActivity * ESDowntime)
                + ((GHWCPerSec * Orb) * castingActivity * ESDowntime)
                + ((CHCHitsPerSec * Orb * .3f) * castingActivity * ESDowntime)
                + (calcStats.LBRestore * calcStats.LBNumber)
                - ESMPS;
            if (options.WSPops > 0)
                calcStats.MAPS += ((options.WSPops * Orb) / 60);
            calcStats.ManaUsed = calcStats.MAPS * FightSeconds;
            float MAPSConvert = (float)Math.Min((calcStats.MAPS / ((calcStats.MUPS) * ESDowntime)), 1);
            float HealedHPS = stats.Healed * (1 + stats.BonusHealingDoneMultiplier);
            calcStats.BurstHPS = (BurstHPS * ESDowntime) + calcStats.ESHPS * (1 - ESOverheal) + HealedHPS;
            calcStats.SustainedHPS = (SustHPS * MAPSConvert) + calcStats.ESHPS * (1 - ESOverheal) + HealedHPS;
            calcStats.Survival = (calcStats.BasicStats.Health + calcStats.BasicStats.Hp5) * (options.SurvivalPerc * .01f);
            calcStats.OverallPoints = calcStats.BurstHPS + calcStats.SustainedHPS + calcStats.Survival;
            calcStats.SubPoints[0] = calcStats.BurstHPS;
            calcStats.SubPoints[1] = calcStats.SustainedHPS;
            calcStats.SubPoints[2] = calcStats.Survival;

            return calcStats;
            #endregion
        }
        #endregion
        #region Character Stats and other Final Stats
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, null);
        }
        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
            CalculationOptionsRestoSham calcOpts = character.CalculationOptions as CalculationOptionsRestoSham;
            Stats statsRace = GetRaceStats(character.Race);
            #region Other Final Stats
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace;
            if (statModifier != null)
                statsTotal += statModifier;
            statsTotal.Accumulate(GetProcStats(statsTotal.SpecialEffects()));
            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));

            float IntMultiplier = (1f + statsTotal.BonusIntellectMultiplier);
            if (IntMultiplier > 1)
            {
                statsTotal.Intellect = (float)Math.Floor((statsRace.Intellect) * IntMultiplier) +
                    (float)Math.Floor((statsBaseGear.Intellect + statsBuffs.Intellect/* + statsEnchants.Intellect*/) * IntMultiplier);
                if (statModifier != null && statModifier.Intellect > 0)
                    statsTotal.Intellect += (float)Math.Floor((statModifier.Intellect) * IntMultiplier);
            }

            statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower) + (float)Math.Floor(statsTotal.Intellect * 0.05f * character.ShamanTalents.NaturesBlessing);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1f + statsTotal.BonusHealthMultiplier);
            #endregion
            return statsTotal;
        }
        #region Basic Stats Methods
        
        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>
        /// A filtered Stats object containing only the stats relevant to the model.
        /// </returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats relevantStats = new Stats();
            Type statsType = typeof(Stats);

            foreach (string relevantStat in Relevants.RelevantStats)
            {
                float v = (float)statsType.GetProperty(relevantStat).GetValue(stats, null);
                if (v > 0)
                {
                    statsType.GetProperty(relevantStat).SetValue(relevantStats, v, null);
                }
            }

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.HealingSpellCast ||
                    effect.Trigger == Trigger.HealingSpellCrit ||
                    effect.Trigger == Trigger.HealingSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.Use)
                {
                    if (HasRelevantStats(effect.Stats))
                        relevantStats.AddSpecialEffect(effect);
                }
            }

            return relevantStats;
        }
        
        /// <summary>
        /// Tests whether there are positive relevant stats in the Stats object.
        /// </summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>
        /// True if any of the positive stats in the Stats are relevant.
        /// </returns>
        public override bool HasRelevantStats(Stats stats)
        {
            // Accumulate the "base" stats with the special effect stats.
            Stats comparison = new Stats();
            comparison.Accumulate(stats);
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                comparison.Accumulate(effect.Stats);
            }

            float statTotal = 0f;

            // Loop over each relevant stat and get its value
            Type statsType = typeof(Stats);
            foreach (string relevantStat in Relevants.RelevantStats)
            {
                float v = (float)statsType.GetProperty(relevantStat).GetValue(comparison, null);
                if (v > 0)
                    statTotal += v;
            }

            // if statTotal > 0 then we have relevant stats
            return statTotal > 0;
        }
        
        /// <summary>
        /// Gets the stats of the buffs currently active on a character
        /// </summary>
        /// <param name="character">The character to evaluate</param>
        /// <returns>Stats object containing the stats of all the active buffs on the character</returns>
        private Stats GetBuffsStats(Character character)
        {
            return GetBuffsStats(character.ActiveBuffs);
        }
        
        /// <summary>
        /// Gets the stats of procs or special effects
        /// </summary>
        /// <param name="specialEffects">List of special effects to aggregate</param>
        /// <returns>Stats object containing the accumulated stats from the provided special effects</returns>
        private Stats GetProcStats(Stats.SpecialEffectEnumerator specialEffects)
        {
            Stats statsProcs = new Stats();
            foreach (SpecialEffect effect in specialEffects)
            {
                statsProcs.Accumulate(GetProcStats_Inner(effect));
            }
            return statsProcs;
        }

        private Stats GetProcStats_Inner(SpecialEffect effect) {
            Stats procStats = new Stats();
            switch (effect.Trigger)
            {
                case (Trigger.HealingSpellCast):
                    if (HealPerSec != 0)
                        procStats = effect.GetAverageStats((1f / HealPerSec), 1f, 0f, FightSeconds);
                    break;
                case (Trigger.HealingSpellHit):
                    if (HealHitPerSec != 0)
                        procStats = effect.GetAverageStats((1f / HealHitPerSec), 1f, 0f, FightSeconds);
                    break;
                case (Trigger.HealingSpellCrit):
                    if (CritPerSec != 0)
                        procStats = effect.GetAverageStats((1f / CritPerSec), 1f, 0f, FightSeconds);
                    break;
                case (Trigger.SpellCast):
                    if (HealPerSec != 0)
                        procStats = effect.GetAverageStats((1f / HealPerSec), 1f, 0f, FightSeconds);
                    break;
                case (Trigger.SpellHit):
                    if (HealHitPerSec != 0)
                        procStats = effect.GetAverageStats((1f / HealHitPerSec), 1f, 0f, FightSeconds);
                    break;
                case (Trigger.SpellCrit):
                    if (CritPerSec != 0)
                        procStats = effect.GetAverageStats((1f / CritPerSec), 1f, 0f, FightSeconds);
                    break;
                case Trigger.Use:
                    if (effect.Stats._rawSpecialEffectData != null) {
                        // Handles Recursive Effects
                        Stats SubStats = GetProcStats_Inner(effect.Stats._rawSpecialEffectData[0]);
                        float upTime = effect.GetAverageUptime(0f, 1f, 0f, FightSeconds);
                        procStats.Accumulate(SubStats,upTime);
                    } else {
                        procStats = effect.GetAverageStats(0f, 1f, 0f, FightSeconds);
                    }
                    break;
            }
            return procStats;
        }
        
        /// <summary>
        /// Create the base stats for a given character based on their race.
        /// </summary>
        /// <param name="race">The race of the character</param>
        /// <returns>Stats object containing the base race stats</returns>
        private static Stats GetRaceStats(CharacterRace race)
        {
            #region Create the statistics for a given character
            Stats statsRace;
            switch (race)
            {
                case CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = BaseMana, Stamina = 135, Intellect = 141, Spirit = 145 };
                    break;

                case CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = BaseMana, Stamina = 138, Intellect = 135, Spirit = 145 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;

                case CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = BaseMana, Stamina = 138, Intellect = 137, Spirit = 146 };
                    break;

                case CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = BaseMana, Stamina = 137, Intellect = 124, Spirit = 144 };
                    break;

                default:
                    statsRace = new Stats();
                    break;
            }
            return statsRace;
            #endregion
        }
        #endregion
        #endregion

        #region Chart data area
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
#if !RAWR3 && !RAWR4
            ChartCalculator chartCalc = CustomCharts.GetChartCalculator(chartName);
            if (chartCalc == null)
                return new ComparisonCalculationBase[0];

            ICollection<ComparisonCalculationBase> list = chartCalc(character, this);
            ComparisonCalculationBase[] retVal = new ComparisonCalculationBase[list.Count];
            if (list.Count > 0)
                list.CopyTo(retVal, 0);
            return retVal;
#else
			return new ComparisonCalculationBase[0];
#endif
        }
#if !RAWR3 && !RAWR4
        public override void RenderCustomChart(Character character, string chartName, Graphics g, int width, int height)
        {
            string calc = chartName.Substring(0, chartName.IndexOf("Stats Graph") - 1);
            Base.Graph.RenderStatsGraph(g, width, height, character,
                CustomCharts.StatsGraphStatsList, CustomCharts.StatsGraphColors,
                200, "", calc, Base.Graph.Style.DpsWarr);
        }
#endif

        #endregion

        #region Retrieve our options from XML:
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsRestoSham calcOpts = serializer.Deserialize(reader) as CalculationOptionsRestoSham;
            return calcOpts;
        }
        #endregion
    }
}
