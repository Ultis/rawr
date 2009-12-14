using System;
#if RAWR3
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
        #region Gemming Template Area
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
                int[] Reckless = { 39959, 40051, 40152 };
                int[] Potent = { 39956, 40048, 40155 };

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

					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Max Spellpower
						RedId = Runed[2], YellowId = Runed[2], BlueId = Runed[2], PrismaticId = Runed[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Int
						RedId = Runed[2], YellowId = Luminous[2], BlueId = Glowing[2], PrismaticId = Runed[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //Heavy MP5
						RedId = Royal[2], YellowId = Dazzling[2], BlueId = Dazzling[2], PrismaticId = Lustrous[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP MP5
						RedId = Runed[2], YellowId = Dazzling[2], BlueId = Royal[2], PrismaticId = Runed[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Haste
						RedId = Runed[2], YellowId = Reckless[2], BlueId = Energized[2], PrismaticId = Quick[2], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", Enabled = true, //SP Crit
						RedId = Runed[2], YellowId = Potent[2], BlueId = Sundered[2], PrismaticId = Smooth[2], MetaId = Revitalizing },

					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //JC - SP
						RedId = Runed[3], YellowId = Smooth[3], BlueId = Lustrous[3], PrismaticId = Runed[3], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //JC - Haste
						RedId = Runed[3], YellowId = Quick[3], BlueId = Lustrous[3], PrismaticId = Quick[3], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //JC - Crit
						RedId = Runed[3], YellowId = Smooth[3], BlueId = Lustrous[3], PrismaticId = Smooth[3], MetaId = Revitalizing },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //JC - Int
						RedId = Runed[3], YellowId = Brilliant[3], BlueId = Lustrous[3], PrismaticId = Brilliant[3], MetaId = Ember },
				};
            }
        }
        #endregion
        #region Labels and Charts info
        //
        // Character calulcations display labels:
        //
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
            character.ActiveBuffsAdd(("Earthliving Weapon"));
        }
        private string[] _characterDisplayCalcLabels = null;


        private Dictionary<string, Color> _subpointColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subpointColors == null)
                {
                    _subpointColors = new Dictionary<string, Color>();
                    _subpointColors.Add("Burst", Color.FromArgb(255, 255, 0, 0));
					_subpointColors.Add("Sustained", Color.FromArgb(255, 0, 0, 255));
					_subpointColors.Add("Survival", Color.FromArgb(255, 0, 128, 0));
                }
                return _subpointColors;
            }
        }

        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalcLabels == null)
                {
                    _characterDisplayCalcLabels = new string[] {
                          "Totals:HPS - Burst",
                          "Totals:HPS - Sustained",
                          "Totals:Survival",
                          "Basic Stats:Health",
                          "Basic Stats:Mana",
                          "Basic Stats:Stamina",
                          "Basic Stats:Intellect",
                          "Basic Stats:Spell Power",
                          "Basic Stats:MP5*Mana regeneration while casting",
                          "Basic Stats:Heal Spell Crit*This includes all static talents including those that are not shown on the in-game character pane",
                          "Basic Stats:Spell Haste",
                          "Healing Style Breakdowns:Burst Sequence",
                          "Healing Style Breakdowns:Sustained Sequence",
                          "Healing Style Breakdowns:Mana Available per Second",
                          "Healing Style Breakdowns:Mana Used per Second*This is the mana used per second by your chosen sustained sequence",
                          "Healing Style Breakdowns:Healing Stream HPS",
                          "Healing Style Breakdowns:RT+HW HPS",
                          "Healing Style Breakdowns:RT+LHW HPS",
                          "Healing Style Breakdowns:RT+CH HPS",
                          "Healing Style Breakdowns:RT+CH+LHW HPS",
                          "Healing Style Breakdowns:HW Spam HPS",
                          "Healing Style Breakdowns:LHW Spam HPS",
                          "Healing Style Breakdowns:CH Spam HPS"};
                }
                return _characterDisplayCalcLabels;
            }
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Healing Wave");
                _relevantGlyphs.Add("Glyph of Water Shield");
                _relevantGlyphs.Add("Glyph of Water Mastery");
                _relevantGlyphs.Add("Glyph of Chain Heal");
                _relevantGlyphs.Add("Glyph of Earth Shield");
                _relevantGlyphs.Add("Glyph of Lesser Healing Wave");
                _relevantGlyphs.Add("Glyph of Earthliving Weapon");
                _relevantGlyphs.Add("Glyph of Mana Tide Totem");
                _relevantGlyphs.Add("Glyph of Healing Stream Totem");
                _relevantGlyphs.Add("Glyph of Riptide");

            }
            return _relevantGlyphs;
        }
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Haste %",
                    "Crit %",
                    "Mana Usable per Second",
					};
                return _optimizableCalculationLabels;
            }
        }
        //
        // Custom chart names:
        //
        public override string[] CustomChartNames
        {
            get
            {
                return new string[]{"Mana Available per Second - NYI"};
            }
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        //
        // Calculations options panel:
        //
#if RAWR3
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

        #endregion
        #region Item types we're interested in
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[] {
                         ItemType.None,
                         ItemType.Cloth,
                         ItemType.Leather,
                         ItemType.Mail,
                         ItemType.Totem,
                         ItemType.OneHandMace,
                         ItemType.OneHandAxe,
                         ItemType.Shield,
                         ItemType.Staff,
                         ItemType.FistWeapon,
                         ItemType.Dagger });
                }
                return _relevantItemTypes;
            }
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
            cacheChar = character;
            return GetCharacterCalculations(character, additionalItem, null);
        }
        #region Calculations Variables to be Carried Over
        public float HealPerSec = 0;
        public float CritPerSec = 0;
        public float FightSeconds = 0;
        public float ELWHPS = 0;
        public float HeroismMana = 0;
        public float TotalManaPool = 0;
        #endregion
        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem,
                                                                  Stats statModifier)
        {
            Stats stats = GetCharacterStats(character, additionalItem, statModifier);
            CharacterCalculationsRestoSham calcStats = new CharacterCalculationsRestoSham();
            calcStats.BasicStats = stats;
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            #region Spell Power and Haste Based Calcs
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                stats.SpellPower += (character.ShamanTalents.ElementalWeapons * .1f * 150f);
            calcStats.SpellHaste = (stats.HasteRating / 3270) + stats.SpellHaste;
            float Healing = 1.88f * stats.SpellPower;
            #endregion
            #region Intellect and MP5 Based Calcs
            stats.Mp5 += (float)Math.Round((stats.Intellect * ((character.ShamanTalents.UnrelentingStorm / 3) * .1f)), 0) + (stats.TotemThunderhead * 2);
            float OrbRegen = (character.ShamanTalents.GlyphofWaterMastery ? 130 : 100);
            stats.Mp5 += ((options.WaterShield ? OrbRegen : 0)) + (stats.TotemThunderhead * 2) + (options.WaterShield ? (100f * stats.WaterShieldIncrease) : 0);
            if (options.Heroism.Equals("Me"))
                HeroismMana = .19f * 4396f;
            if (options.Heroism.Equals("Me"))
                stats.SpellHaste = .3f / (60 * options.FightLength);
            if (options.Heroism.Equals("Yes"))
                stats.SpellHaste = .3f / (60 * options.FightLength);
            calcStats.TotalManaPool = (((((float)Math.Floor(options.FightLength / 5.025f) + 1) * ((stats.Mana * (1 + stats.BonusManaMultiplier)) * (.24f +
                ((character.ShamanTalents.GlyphofManaTideTotem ? 0.4f : 0)))))) * (options.ManaTideEveryCD ? 1 : 0)) + 
                stats.Mana + ((stats.ManaRestoreFromMaxManaPerSecond * stats.Mana) * ((options.FightLength * 60f)) *
                (options.BurstPercentage * .01f))  - HeroismMana;
            calcStats.SpellCrit = .022f + StatConversion.GetSpellCritFromIntellect(stats.Intellect)
                + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit +
                (.01f * (character.ShamanTalents.TidalMastery + character.ShamanTalents.ThunderingStrikes +
                (character.ShamanTalents.BlessingOfTheEternals * 2)));
            float CriticalChance = calcStats.SpellCrit + stats.BonusCritHealMultiplier;
            float Critical = 1f + (CriticalChance / 2f);
            #endregion
            #region Water Shield and Mana Calculations
            //// Code Flag: Dodger = Re-cast not needed in 3.2
            //float WSC = (float)Math.Max((1.5 * (1 - (calcStats.SpellHaste))), 1f) / 4;
            float Thunderhead = stats.TotemThunderhead * 27;
            float Orb = ((400 * (1 + (character.ShamanTalents.ImprovedShields * .05f))) * (1 + stats.WaterShieldIncrease)) + Thunderhead;
            #endregion
            #region Earthliving Weapon Calculations
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                ELWHPS = (652 + (Healing * (5 / 11)) * (12 / 15)) * (1f + ((character.ShamanTalents.Purification) * .02f));
            float ExtraELW = (options.TankHeal ? 0 : 1) + (character.ShamanTalents.GlyphofEarthlivingWeapon ? .5f : 0);
            #endregion
            #region Earth Shield Calculations
            float ESUse = (options.EarthShield ? 1 : 0);
            float ESTimer = 24 + (character.ShamanTalents.ImprovedEarthShield * 4);
            float ESHPS = (float)Math.Round(((((((2022f + (Healing * 3f)) * (1f + (.05f * (character.ShamanTalents.ImprovedShields + character.ShamanTalents.ImprovedEarthShield)))) 
                / 6f * (6f + character.ShamanTalents.ImprovedEarthShield))) * (1f + ((character.ShamanTalents.Purification) * .02f))) * (character.ShamanTalents.GlyphofEarthShield ? 1.2 : 1)), 0)
                * character.ShamanTalents.EarthShield / ESTimer * ESUse;
            float ESMPS = ((float)Math.Round((.15 * 4396), 0) * (1f - ((character.ShamanTalents.TidalFocus) * .01f))) / ESTimer * ESUse;
            #endregion
            #region Base Variables ( Heals per sec and Crits per sec )
            float RTPerSec = 0;
            float HWPerSec = 0;
            float CHPerSec = 0;
            float LHWPerSec = 0;
            float LHWCPerSec = 0f;
            float CHCPerSec = 0f;
            float HWCPerSec = 0f;
            float RTCPerSec = 0f;
            float HealsPerSec = 0;
            float CritsPerSec = 0;
            #endregion
            #region Base Speeds ( Hasted / RTCast / LHWCast / HWCast / CHCast )
            float Hasted = (1 + ((stats.HasteRating / 3270) + stats.SpellHaste));
            float RTCast = (float)Math.Max((1.5 * (1 + ((stats.HasteRating / 3270) + stats.SpellHaste))), 1f);
            float HWCast = ((float)Math.Max((((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) * (1 - (calcStats.SpellHaste)))), 1f)
                * ((1 + ((stats.HasteRating / 3270) + stats.SpellHaste))));
            float LHWCast = (float)Math.Max(((1.5f * (1 - (calcStats.SpellHaste)))), 1.1f);
            float CHCast = (float)Math.Max(((2.6 - stats.CHCTDecrease) * (1 - (calcStats.SpellHaste))), 1f);
            #endregion
            #region Base Spells ( TankCH / RTHeal / LHWHeal / HWHeal / CHHeal )
            float TankCH = (options.TankHeal ? 1 : (1.75f + (character.ShamanTalents.GlyphofChainHeal ? .125f : 0)));
            float RTHeal = (((1670f + (Healing * .5f)) * (1f + ((character.ShamanTalents.Purification) * .02f)))) * (1 + (.2f * stats.RestoSham2T9));
            float LHWHeal = (1720 + (Healing + (((stats.TotemLHWSpellpower) * (.02f * character.ShamanTalents.TidalWaves)) * (1.5f / 3.5f)) + ((1.88f * stats.TotemLHWSpellpower) *
                (1.5f / 3.5f)))) * (options.TankHeal ? (character.ShamanTalents.GlyphofLesserHealingWave ? 1.2f : 1) : 1);
            float HWHeal = ((3250 + (Healing + (((stats.TotemHWSpellpower) * (.04f * character.ShamanTalents.TidalWaves)) * ((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) / 3.5f)) 
                + (((1.88f * stats.TotemHWSpellpower)) * ((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) / 3.5f)))) 
                * (1f + (character.ShamanTalents.Purification * .02f)) * (character.ShamanTalents.GlyphofHealingWave ? 1.2f : 1)) * (1 + (.25f * character.ShamanTalents.HealingWay / 3));
            float CHHeal = (((((((1130 + stats.TotemCHBaseHeal) + ((Healing + stats.RestoShamRelicT9) * (2.5f / 3.5f))) * (1f + (character.ShamanTalents.ImprovedChainHeal * .02f))
                * (1f + ((character.ShamanTalents.Purification) * .02f))) * (Critical + (.05f * stats.RestoSham4T9))) * TankCH) + (ExtraELW * ELWHPS * (2.5f * ((1 + (stats.HasteRating
                / 3270) + stats.SpellHaste))) / 2f)) * (1f + stats.CHHWHealIncrease + (.25f * CriticalChance * stats.RestoSham2T10)));
            #endregion
            #region Base Costs ( Preserve / RTCost / LHWCost / CHCost )
            float Preserve = stats.ManacostReduceWithin15OnHealingCast * .02f;
            float RTCost = ((((float)Math.Round(4396 * .18f, 0)) - Preserve) * (1f - (character.ShamanTalents.TidalFocus * .01f)));
            float LHWCost = ((((float)Math.Round(4396 * .15f, 0)) - Preserve) * (1f - (character.ShamanTalents.TidalFocus * .01f)));
            float HWCost = ((((float)Math.Round(4396 * .32f, 0)) - Preserve - stats.TotemHWBaseCost) * (1f - (character.ShamanTalents.TidalFocus * .01f)));
            float CHCost = ((((float)Math.Round(4396 * .19f, 0)) - Preserve - stats.TotemCHBaseCost) * (1f - (character.ShamanTalents.TidalFocus * .01f)));
            #endregion
            #region RT + LHW Rotation (RTLHWMPS / RTLHWHPS / RTLHWTime) Needs Heals and Crits per Second
            float RTLHWCPER = (float)Math.Round(((6.25f - RTCast) - (LHWCast * 2)));
            float RTLHWTime = RTCast + (LHWCast * 2) + (RTLHWCPER * LHWCast);
            float RTLHWAA = (((((((LHWHeal * (2))))) / RTLHWTime * (Critical + (.5f / 5 * character.ShamanTalents.TidalWaves))) + ((RTHeal + (((LHWHeal * (RTLHWCPER))))) / RTLHWTime * Critical))) * (character.ShamanTalents.AncestralAwakening * .1f);
            calcStats.RTLHWHPS = (((((((LHWHeal * (2))))) / RTLHWTime * (Critical+(.5f / 5 * character.ShamanTalents.TidalWaves))) + ((RTHeal + (((LHWHeal * (RTLHWCPER))))) / RTLHWTime * Critical)) + ELWHPS) + RTLHWAA;
            calcStats.RTLHWMPS = ((RTCost) + (LHWCost * (RTLHWCPER + 2))) / RTLHWTime;
            if (options.SustStyle.Equals("RT+LHW"))
                RTPerSec = 1f / RTLHWTime;
            if (options.SustStyle.Equals("RT+LHW"))
                LHWPerSec = (RTLHWCPER + 2) / RTLHWTime;
            if (options.SustStyle.Equals("RT+LHW"))
                RTCPerSec = (1f / RTLHWTime) * CriticalChance;
            if (options.SustStyle.Equals("RT+LHW"))
                LHWCPerSec = ((RTLHWCPER + 2) / RTLHWTime) * (CriticalChance + ((.5f / 5 * character.ShamanTalents.TidalWaves) * (2 / (2 + RTLHWCPER))));
            #endregion
            #region RT + HW Rotation (RTHWMPS / RTHWHPS / RTHWTime) Needs Heals and Crits per Second
            float RTHWCPER = (float)Math.Round(((6.25f - RTCast) - ((HWCast / (Hasted) * (Hasted + ((.3f / 5 * character.ShamanTalents.TidalWaves))))) * 2) / HWCast);
            float RTHWTime = RTCast + (((HWCast / (Hasted) * (Hasted + ((.3f / 5 * character.ShamanTalents.TidalWaves))))) * 2) + (RTHWCPER * HWCast);
            float RTHWAA = ((((RTHeal + (((HWHeal * (2 + RTHWCPER))))) / RTHWTime * Critical))) * (character.ShamanTalents.AncestralAwakening * .1f);
            calcStats.RTHWHPS = (((RTHeal + (((HWHeal * (2 + RTHWCPER))))) / RTHWTime * Critical) + ELWHPS) + RTHWAA;
            calcStats.RTHWMPS = ((RTCost) + (HWCost * (RTHWCPER + 2))) / RTHWTime;
            if (options.SustStyle.Equals("RT+HW"))
                RTPerSec = 1f / RTHWTime;
            if (options.SustStyle.Equals("RT+HW"))
                HWPerSec = (RTHWCPER + 2) / RTHWTime;
            if (options.SustStyle.Equals("RT+HW"))
                RTCPerSec = (1f / RTHWTime) * CriticalChance;
            if (options.SustStyle.Equals("RT+HW"))
                HWCPerSec = ((RTHWCPER + 2) / RTHWTime) * CriticalChance;
            #endregion
            #region RT + CH Rotation (RTCHMPS / RTCHHPS / RTCHTime / TankCH) Needs Heals and Crits per Second
            float RTCHCPER = (float)Math.Round(4.75f / CHCast);
            float RTCHTime = RTCast + (CHCast * RTCHCPER);
            float RTCHAA = (RTHeal / RTCHTime * Critical) * (.1f * character.ShamanTalents.AncestralAwakening);
            calcStats.RTCHHPS = (((RTHeal + (CHHeal * 1.2f) + (CHHeal * (RTCHCPER - 1))) / RTCHTime * Critical) + ELWHPS) + RTCHAA;
            calcStats.RTCHMPS = (((RTCost + (CHCost * (RTCHCPER))) / RTCHTime));
            if (options.SustStyle.Equals("RT+CH"))
                RTPerSec = 1f / RTCHTime;
            if (options.SustStyle.Equals("RT+CH"))
                CHPerSec = RTCHCPER / RTCHTime;
            if (options.SustStyle.Equals("RT+CH"))
                RTCPerSec = (1f / RTCHTime) * CriticalChance;
            if (options.SustStyle.Equals("RT+CH"))
                CHCPerSec = (RTCHCPER / RTCHTime) * CriticalChance;
            #endregion
            #region RT + CH + LHW2 Rotation (RTCHLHW2MPS / RTCHLHW2HPS / RTCHLHW2Time) Needs Heals and Crits per Second
            float RTCHLHW2CPER = (float)Math.Max(((float)Math.Round(((6.25f - (RTCast + CHCast)) - (LHWCast) * 2) / LHWCast)), 2);
            float RTCHLHW2Time = RTCast + CHCast + (LHWCast * 2) + (float)Math.Max(((RTCHLHW2CPER - 2) * LHWCast), 0);
            float RTCHLHW2AA = (((((((LHWHeal * (RTCHLHW2CPER))))) / RTCHLHW2Time * (Critical + (.5f / 5 * character.ShamanTalents.TidalWaves)))) + (((RTHeal) / RTCHLHW2Time * Critical))) * (character.ShamanTalents.AncestralAwakening * .1f);
            calcStats.RTCHLHW2HPS = ((((((((LHWHeal * (RTCHLHW2CPER))))) / RTCHLHW2Time * (Critical + (.5f / 5 * character.ShamanTalents.TidalWaves)))) + (((RTHeal + CHHeal) / RTCHLHW2Time * Critical))) + ELWHPS) + RTCHLHW2AA;
            calcStats.RTCHLHW2MPS = ((RTCost) + CHCost + (LHWCost * (RTCHLHW2CPER))) / RTCHLHW2Time;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                RTPerSec = 1f / RTCHLHW2Time;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                CHPerSec = 1f / RTCHLHW2Time;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                LHWPerSec = RTCHLHW2CPER / RTCHLHW2Time;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                RTCPerSec = (1f / RTCHLHW2Time) * CriticalChance;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                CHCPerSec = (1f / RTCHLHW2Time) * CriticalChance;
            if (options.SustStyle.Equals("RT+CH+LHW"))
                LHWCPerSec = (RTCHLHW2CPER / RTCHLHW2Time) * (CriticalChance + ((.5f / 5 * character.ShamanTalents.TidalWaves) * (2 / (RTCHLHW2CPER))));
            #endregion
            #region CH Spam (CHHPS / CHMPS) Needs Heals and Crits per Second
            calcStats.CHSpamHPS = (CHHeal / CHCast + ELWHPS);
            calcStats.CHSpamMPS = (CHCost / CHCast);
            if (options.SustStyle.Equals("CH Spam"))
                CHPerSec = 1f / CHCast;
            if (options.SustStyle.Equals("CH Spam"))
                CHCPerSec = (1f / CHCast) * CriticalChance;
            #endregion
            #region LHW Spam (LHWHPS / LHWMPS) Needs Heals and Crits per Second
            float LHWAA = LHWHeal * (.1f * character.ShamanTalents.AncestralAwakening);
            calcStats.LHWSpamHPS = (((LHWHeal * (Critical)) / (LHWCast/* + (WSC * CriticalChance * .6f * (character.ShamanTalents.ImprovedWaterShield / 3))*/)) + ELWHPS) + LHWAA;
            calcStats.LHWSpamMPS = LHWCost / (LHWCast);
            if (options.SustStyle.Equals("LHW Spam"))
                LHWPerSec = 1f / LHWCast;
            if (options.SustStyle.Equals("LHW Spam"))
                LHWCPerSec = (1f / LHWCast) * CriticalChance;
            #endregion
            #region HW Spam (HWHPS / HWMPS) Needs Heals and Crits per Second
            float HWAA = HWHeal * (.1f * character.ShamanTalents.AncestralAwakening);
            calcStats.HWSpamHPS = (((HWHeal * (Critical)) / (HWCast/* + (WSC * CriticalChance * (character.ShamanTalents.ImprovedWaterShield / 3))*/)) + ELWHPS) + HWAA;
            calcStats.HWSpamMPS = HWCost / (HWCast);
            if (options.SustStyle.Equals("HW Spam"))
                HWPerSec = 1f / HWCast;
            if (options.SustStyle.Equals("HW Spam"))
                HWCPerSec = (1f / HWCast) * CriticalChance;
            #endregion
            #region Create Final calcs via spell cast (Improve Water Shield Mana Return)
            HealsPerSec = RTPerSec + LHWPerSec + HWPerSec + CHPerSec;
            CritsPerSec = RTCPerSec + LHWCPerSec + HWCPerSec + CHCPerSec;
            HealPerSec = HealsPerSec;
            CritPerSec = CritsPerSec;
            FightSeconds = options.FightLength * 60f;
            if (RTPerSec > 0)
                stats.SpellHaste += stats.RestoSham2T10 * (.2f / 6f);
            if (RTPerSec > 0)
                stats.SpellPower += stats.RestoShamRelicT10;

            #region Proc Handling for Mana Restore only
            Stats statsProcs2 = new Stats();
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case (Trigger.HealingSpellCast):
                        effect.AccumulateAverageStats(statsProcs2, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.HealingSpellCrit):
                        effect.AccumulateAverageStats(statsProcs2, (1f / CritPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCast):
                        effect.AccumulateAverageStats(statsProcs2, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCrit):
                        effect.AccumulateAverageStats(statsProcs2, (1f / CritPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellHit):
                        effect.AccumulateAverageStats(statsProcs2, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case Trigger.Use:
                        effect.AccumulateAverageStats(statsProcs2, effect.Cooldown, 1f, 0f, effect.Duration);
                        break;
                }
            }
            stats.Mp5 += stats.ManaRestoreOnCrit_25_45 / (0.5f / (CritsPerSec * .25f) + 45) * 5; // Mana per Crit, to be removed when Special effects are implimented
            stats.Mp5 += stats.ManaRestoreOnCast_5_15 / (0.5f / (HealsPerSec * .05f) + 15) * 5; // Mana per Cast, to be removed when Special effects are implimented
            stats.Mp5 += stats.ManaRestoreOnCast_10_45 / (0.5f / (HealsPerSec * .1f) + 45) * 5; // Mana per Cast, to be removed when Special effects are implimented
            stats.ManaRestore += statsProcs2.ManaRestore * FightSeconds;
            #endregion
            stats.Mp5 += (RTCPerSec * (Orb * (character.ShamanTalents.ImprovedWaterShield / 3)) * 5) + (LHWCPerSec * (Orb * (character.ShamanTalents.ImprovedWaterShield / 3)) * 5 * .6f) 
                + (HWCPerSec * (Orb * (character.ShamanTalents.ImprovedWaterShield / 3)) * 5)+ (CHCPerSec * (Orb * (character.ShamanTalents.ImprovedWaterShield / 3)) * 5 * .3f);
            #endregion
            #region Calculate Sequence HPS/MPS
            calcStats.HSTHeals = (((stats.SpellPower * .044f) + 25) * (1f + (character.ShamanTalents.RestorativeTotems * .15f)) * (1f + (character.ShamanTalents.GlyphofHealingStreamTotem ? .2f : 0))) / 2;
            calcStats.BurstSequence = options.BurstStyle;
            if (options.BurstStyle.Equals("CH Spam"))
                calcStats.BurstHPS = (calcStats.CHSpamHPS);
            if (options.BurstStyle.Equals("HW Spam"))
                calcStats.BurstHPS = (calcStats.HWSpamHPS);
            if (options.BurstStyle.Equals("LHW Spam"))
                calcStats.BurstHPS = (calcStats.LHWSpamHPS);
            if (options.BurstStyle.Equals("RT+HW"))
                calcStats.BurstHPS = (calcStats.RTHWHPS);
            if (options.BurstStyle.Equals("RT+LHW"))
                calcStats.BurstHPS = (calcStats.RTLHWHPS);
            if (options.BurstStyle.Equals("RT+CH"))
                calcStats.BurstHPS = (calcStats.RTCHHPS);
            if (options.BurstStyle.Equals("RT+CH+LHW"))
                calcStats.BurstHPS = (calcStats.RTCHLHW2HPS);
            calcStats.SustainedSequence = options.SustStyle;
            float SustHPS = 0;
            if (options.SustStyle.Equals("CH Spam"))
                SustHPS = (calcStats.CHSpamHPS);
            if (options.SustStyle.Equals("CH Spam"))
                calcStats.MUPS = (calcStats.CHSpamMPS);
            if (options.SustStyle.Equals("HW Spam"))
                SustHPS = (calcStats.HWSpamHPS);
            if (options.SustStyle.Equals("HW Spam"))
                calcStats.MUPS = (calcStats.HWSpamMPS);
            if (options.SustStyle.Equals("LHW Spam"))
                SustHPS = (calcStats.LHWSpamHPS);
            if (options.SustStyle.Equals("LHW Spam"))
                calcStats.MUPS = (calcStats.LHWSpamMPS);
            if (options.SustStyle.Equals("RT+HW"))
                SustHPS = (calcStats.RTHWHPS);
            if (options.SustStyle.Equals("RT+HW"))
                calcStats.MUPS = (calcStats.RTHWMPS);
            if (options.SustStyle.Equals("RT+LHW"))
                SustHPS = (calcStats.RTLHWHPS);
            if (options.SustStyle.Equals("RT+LHW"))
                calcStats.MUPS = (calcStats.RTLHWMPS);
            if (options.SustStyle.Equals("RT+CH"))
                SustHPS = (calcStats.RTCHHPS);
            if (options.SustStyle.Equals("RT+CH"))
                calcStats.MUPS = (calcStats.RTCHMPS);
            if (options.SustStyle.Equals("RT+CH+LHW"))
                SustHPS = (calcStats.RTCHLHW2HPS);
            if (options.SustStyle.Equals("RT+CH+LHW"))
                calcStats.MUPS = (calcStats.RTCHLHW2MPS);
            calcStats.BurstHPS += calcStats.HSTHeals;
            SustHPS += calcStats.HSTHeals;
            #endregion
            #region Final Stats
            float ESUsage = (float)Math.Round(((options.FightLength * 60) / ESTimer), 0);
            float Time = (options.FightLength * 60) - (1.5f * ESUsage) - 4;
            float Converter = Time / (options.FightLength * 60);
            calcStats.MAPS = (((calcStats.TotalManaPool + stats.ManaRestore) + (stats.Mp5 / 5 * 60 * options.FightLength)) / (60f * options.FightLength) * Converter) - ESMPS;
            float MAPSConvert = (float)Math.Min((calcStats.MAPS / (calcStats.MUPS * Converter)), 1);
            calcStats.BurstHPS = (calcStats.BurstHPS * Converter) + ESHPS;
            calcStats.SustainedHPS = ((SustHPS * Converter) * MAPSConvert) + ESHPS;
            calcStats.Survival = calcStats.BasicStats.Health * (options.SurvivalPerc * .01f);
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
            cacheChar = character;
            return GetCharacterStats(character, additionalItem, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
            CalculationOptionsRestoSham calcOpts = character.CalculationOptions as CalculationOptionsRestoSham;
            #region Create the statistics for a given character
            Stats statsRace;
            switch (character.Race)
            {
                case CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 135, Intellect = 141, Spirit = 145 };
                    break;

                case CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 135, Spirit = 145 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;

                case CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 137, Spirit = 146 };
                    break;

                case CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 137, Intellect = 124, Spirit = 144 };
                    break;

                default:
                    statsRace = new Stats();
                    break;
            }
            #endregion
            #region Other Final Stats
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace;
            if (statModifier != null)
                statsTotal += statModifier;
            #region Proc Handling
            Stats statsProcs = new Stats();
            // Jothay's Note:
            // Added these two lines for a just in case scenario,
            // otherwise you might have (1f/0f) which is bad
            if (HealPerSec == 0) HealPerSec = 1;
            if (CritPerSec == 0) CritPerSec = 1;
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case (Trigger.HealingSpellCast):
                        effect.AccumulateAverageStats(statsProcs, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.HealingSpellCrit):
                        effect.AccumulateAverageStats(statsProcs, (1f / CritPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCast):
                        effect.AccumulateAverageStats(statsProcs, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellCrit):
                        effect.AccumulateAverageStats(statsProcs, (1f / CritPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case (Trigger.SpellHit):
                        effect.AccumulateAverageStats(statsProcs, (1f / HealPerSec), effect.Chance, 0f, FightSeconds);
                        break;
                    case Trigger.Use:
                        effect.AccumulateAverageStats(statsProcs, effect.Cooldown, 1f, 0f, effect.Duration);
                        break;
                }
            }
            #endregion
            statsTotal += statsProcs;
            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            float IntMultiplier = (1 + statsTotal.BonusIntellectMultiplier) * (1 + (float)Math.Round(.02f * character.ShamanTalents.AncestralKnowledge, 2));
            statsTotal.Intellect = (float)Math.Floor((statsRace.Intellect) * IntMultiplier) + 
                (float)Math.Floor((statsBaseGear.Intellect + statsBuffs.Intellect/* + statsEnchants.Intellect*/) * IntMultiplier);
            statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower) + (float)Math.Floor(statsTotal.Intellect * .05f * character.ShamanTalents.NaturesBlessing);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1 + statsTotal.BonusHealthMultiplier);

            // Fight options:
            #endregion
            return statsTotal;
        }
        #endregion
        #region Chart data area: Code Flag = Penguin (Model MAPS)
        //
        // Class used by stat relative weights custom chart.
        //
        class StatRelativeWeight
        {
            public StatRelativeWeight(string name, Stats stat)
            {
                this.Stat = stat;
                this.Name = name;
            }

            public Stats Stat;
            public string Name;
            public float PctChange;
        }


        //
        // Data for custom charts:
        //
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsRestoSham calc = GetCharacterCalculations(character) as CharacterCalculationsRestoSham;
            if (calc == null)
                calc = new CharacterCalculationsRestoSham();

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            if (options == null)
                options = new CalculationOptionsRestoSham();

            List<ComparisonCalculationBase> list = new List<ComparisonCalculationBase>();
            switch (chartName)
            {
                case "Mana Usable per Second - NYI":
                    StatRelativeWeight[] stats = new StatRelativeWeight[] {
                      new StatRelativeWeight("10 Intellect", new Stats() { Intellect = 10f }),
                      new StatRelativeWeight("10 Haste Rating", new Stats() { HasteRating = 10f }),
                      new StatRelativeWeight("10 Spellpower", new Stats() { SpellPower = 10f}),
                      new StatRelativeWeight("10 MP5", new Stats() { Mp5 = 10f }),
                      new StatRelativeWeight("10 Crit Rating", new Stats() { CritRating = 10f })};

                    // Get the percentage total healing is changed by a change in a single stat:

                    float mpsPct = 0f;
                    foreach (StatRelativeWeight weight in stats)
                    {
                        CharacterCalculationsRestoSham statCalc = (CharacterCalculationsRestoSham)GetCharacterCalculations(character, null, weight.Stat);
                        weight.PctChange = ((statCalc.MAPS - calc.MAPS) / calc.MAPS);
                        if (weight.Name == "+MAPS")
                            mpsPct = weight.PctChange;
                    }

                    // Create the chart data points:

                    foreach (StatRelativeWeight weight in stats)
                    {
                        ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(weight.Name);
                        comp.OverallPoints = weight.PctChange / mpsPct;
                        list.Add(comp);
                    }

                    break;



            }

            ComparisonCalculationBase[] retVal = new ComparisonCalculationBase[list.Count];
            if (list.Count > 0)
                list.CopyTo(retVal);
            return retVal;
        }

        #endregion
        #region Relevant Stats
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                Earthliving = stats.Earthliving,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                BonusManaPotion = stats.BonusManaPotion,
                WaterShieldIncrease = stats.WaterShieldIncrease,
                CHHWHealIncrease = stats.CHHWHealIncrease,
                CHCTDecrease = stats.CHCTDecrease,
                RTCDDecrease = stats.RTCDDecrease,
                TotemCHBaseHeal = stats.TotemCHBaseHeal,
                TotemHWBaseCost = stats.TotemHWBaseCost,
                TotemCHBaseCost = stats.TotemCHBaseCost,
                TotemHWSpellpower = stats.TotemHWSpellpower,
                TotemLHWSpellpower = stats.TotemLHWSpellpower,
                TotemThunderhead = stats.TotemThunderhead,
                ManaRestore = stats.ManaRestore,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                RestoSham2T9 = stats.RestoSham2T9,
                RestoSham4T9 = stats.RestoSham4T9,
                RestoSham2T10 = stats.RestoSham2T10,
                RestoSham4T10 = stats.RestoSham4T10,
                RestoShamRelicT9 = stats.RestoShamRelicT9,
                RestoShamRelicT10 = stats.RestoShamRelicT10

            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.HealingSpellCast ||
                    effect.Trigger == Trigger.HealingSpellCrit ||
                    effect.Trigger == Trigger.HealingSpellHit ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.Use)
                {
                    if (HasRelevantStats(effect.Stats))
                        s.AddSpecialEffect(effect);
                }
            }

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.HasteRating +
                stats.BonusIntellectMultiplier + stats.BonusCritHealMultiplier + stats.BonusManaPotion + stats.ManaRestoreOnCast_5_15 +
                stats.ManaRestoreOnCrit_25_45 + stats.ManaRestoreOnCast_10_45 + stats.ManaRestore + stats.HighestStat +
                stats.ManaRestoreFromMaxManaPerSecond + stats.CHHWHealIncrease + stats.WaterShieldIncrease + stats.SpellHaste +
                stats.BonusIntellectMultiplier + stats.BonusManaMultiplier + stats.ManacostReduceWithin15OnHealingCast + stats.CHCTDecrease +
                stats.RTCDDecrease + stats.Earthliving + stats.TotemCHBaseHeal + stats.TotemHWBaseCost + stats.TotemCHBaseCost +
                stats.TotemHWSpellpower + stats.TotemLHWSpellpower + stats.TotemThunderhead + stats.RestoSham2T9 + stats.RestoSham4T9 +
                stats.RestoSham2T10 + stats.RestoSham4T10 + stats.RestoShamRelicT9 + stats.RestoShamRelicT10) > 0;
        }
        public Stats GetBuffsStats(Character character, CalculationOptionsRestoSham calcOpts)
        {
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
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*/
            /* }
// If we have an enhanced Hunter's Mark, kill the Buff
if (hasRelevantBuff > 0) {
if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*/
            /* }
if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*/
            /* }
if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*/
            /* }
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

            foreach (Buff b in removedBuffs)
            {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs)
            {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        #endregion
        #region Retrieve our options from XML:
        //
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
