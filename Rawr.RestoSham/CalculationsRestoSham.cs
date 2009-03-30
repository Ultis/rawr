using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    [Rawr.Calculations.RawrModelInfo("RestoSham", "Spell_Nature_Magicimmunity", Character.CharacterClass.Shaman)]
    class CalculationsRestoSham : CalculationsBase
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
						RedId = Runed[0], YellowId = Reckless[0], BlueId = Energized[0], PrismaticId = Quick[0], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //SP Crit
						RedId = Runed[0], YellowId = Potent[0], BlueId = Sundered[0], PrismaticId = Smooth[0], MetaId = Ember },

					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Runed[1], YellowId = Runed[1], BlueId = Runed[1], PrismaticId = Runed[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Runed[1], YellowId = Luminous[1], BlueId = Glowing[1], PrismaticId = Runed[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Royal[1], YellowId = Dazzling[1], BlueId = Dazzling[1], PrismaticId = Lustrous[1], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Runed[1], YellowId = Dazzling[1], BlueId = Royal[1], PrismaticId = Runed[1], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Runed[1], YellowId = Reckless[1], BlueId = Energized[1], PrismaticId = Quick[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Runed[1], YellowId = Potent[1], BlueId = Sundered[1], PrismaticId = Smooth[1], MetaId = Ember },

					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Max Spellpower
						RedId = Runed[2], YellowId = Runed[2], BlueId = Runed[2], PrismaticId = Runed[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //SP Int
						RedId = Runed[2], YellowId = Luminous[2], BlueId = Glowing[2], PrismaticId = Runed[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Heavy MP5
						RedId = Royal[2], YellowId = Dazzling[2], BlueId = Dazzling[2], PrismaticId = Lustrous[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //SP MP5
						RedId = Runed[2], YellowId = Dazzling[2], BlueId = Royal[2], PrismaticId = Runed[2], MetaId = Insightful },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //SP Haste
						RedId = Runed[2], YellowId = Reckless[2], BlueId = Energized[2], PrismaticId = Quick[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //SP Crit
						RedId = Runed[2], YellowId = Potent[2], BlueId = Sundered[2], PrismaticId = Smooth[2], MetaId = Ember },

					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Spellpower
						RedId = Runed[3], YellowId = Runed[3], BlueId = Runed[3], PrismaticId = Runed[3], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Crit
						RedId = Smooth[3], YellowId = Smooth[3], BlueId = Smooth[3], PrismaticId = Smooth[3], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Haste
						RedId = Quick[3], YellowId = Quick[3], BlueId = Quick[3], PrismaticId = Quick[3], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Jeweler", //Max Int
						RedId = Brilliant[3], YellowId = Brilliant[3], BlueId = Brilliant[3], PrismaticId = Brilliant[3], MetaId = Ember },
				};
            }
        }
        #endregion
        #region Colors of the ratings we track
        private Dictionary<string, System.Drawing.Color> _subpointColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subpointColors == null)
                {
                    _subpointColors = new Dictionary<string, System.Drawing.Color>();
                    _subpointColors.Add("Healing", System.Drawing.Color.Green);
                    _subpointColors.Add("Till OOM", System.Drawing.Color.Blue);
                }
                return _subpointColors;
            }
        }
        #endregion
        #region Labels and Charts info
        //
        // Character calulcations display labels:
        //
        private string[] _characterDisplayCalcLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalcLabels == null)
                {
                    _characterDisplayCalcLabels = new string[] {
                          "Basic Stats:Health",
                          "Basic Stats:Mana",
                          "Basic Stats:Stamina",
                          "Basic Stats:Intellect",
                          "Basic Stats:Spell Power",
                          "Basic Stats:MP5*Mana regeneration while casting",
                          "Basic Stats:Heal Spell Crit*This includes all static talents including those that are not shown on the in-game character pane",
                          "Basic Stats:Spell Haste",
                          "Totals:Total HPS*This is averaged including the time you are OOM and your overhealing %, acutal HPS during is below",
                          "Totals:Time to OOM*In Seconds",
                          "Totals:Total Healed*Includes Burst and Sustained",
                          "Healing Style Breakdowns:Chosen Sequence",
                          "Healing Style Breakdowns:RT+HW Spam HPS",
                          "Healing Style Breakdowns:RT+HW Spam MPS",
                          "Healing Style Breakdowns:RT+LHW Spam HPS",
                          "Healing Style Breakdowns:RT+LHW Spam MPS",
                          "Healing Style Breakdowns:RT+CH Spam HPS",
                          "Healing Style Breakdowns:RT+CH Spam MPS",
                          "Healing Style Breakdowns:HW Spam HPS",
                          "Healing Style Breakdowns:HW Spam MPS",
                          "Healing Style Breakdowns:LHW Spam HPS",
                          "Healing Style Breakdowns:LHW Spam MPS",
                          "Healing Style Breakdowns:CH Spam HPS",
                          "Healing Style Breakdowns:CH Spam MPS"};
                }
                return _characterDisplayCalcLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Time to OOM",
                    "Total HPS",
                    "Health",
                    "Haste %",
                    "Crit %",
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
                return new string[]{"Stat Relative Weights"};
            }
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot);
        }

        //
        // Calculations options panel:
        //
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelRestoSham();
                return _calculationOptionsPanel;
            }
        }

        #endregion
        #region Item types we're interested in.
        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[] {
                         Item.ItemType.None,
                         Item.ItemType.Cloth,
                         Item.ItemType.Leather,
                         Item.ItemType.Mail,
                         Item.ItemType.Totem,
                         Item.ItemType.OneHandMace,
                         Item.ItemType.OneHandAxe,
                         Item.ItemType.Shield,
                         Item.ItemType.Staff,
                         Item.ItemType.FistWeapon,
                         Item.ItemType.Dagger });
                }
                return _relevantItemTypes;
            }
        }
        #endregion
        #region Model Verification and prepare Calculations
        //
        // This model is for shammies!
        //
        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Shaman; }
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
        #region Calculations
        //
        // Do the actual calculations:
        //
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            return GetCharacterCalculations(character, additionalItem, null);
        }

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
            float Time = (options.FightLength * 60f);
            #endregion
            #region Intellect and MP5 Based Calcs
            float onUse = 0.0f;
            if (options.ManaPotAmount > 0)
                onUse += (options.ManaPotAmount * (1 + stats.BonusManaPotion)) / (options.FightLength * 60 / 5);
            stats.Mp5 += (float)Math.Round((stats.Intellect * ((character.ShamanTalents.UnrelentingStorm / 3) * .1f)), 0);
            calcStats.TotalManaPool = (((((float)Math.Truncate(options.FightLength / 5.025f) + 1) * ((stats.Mana * (1 + stats.BonusManaMultiplier)) * (.24f +
                ((options.ManaTidePlus ? .04f : 0))))) * character.ShamanTalents.ManaTideTotem) * (options.ManaTideEveryCD ? 1 : 0)) + stats.Mana + onUse + ((stats.ManaRestoreFromMaxManaPerSecond * stats.Mana) * ((options.FightLength * 60f)) *
                (options.BurstPercentage * .01f));
            calcStats.SpellCrit = .022f + StatConversion.GetSpellCritFromIntellect(stats.Intellect)
                + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit +
                (.01f * (character.ShamanTalents.TidalMastery + character.ShamanTalents.ThunderingStrikes +
                (character.ShamanTalents.BlessingOfTheEternals * 2)));
            float preserve = stats.ManacostReduceWithin15OnHealingCast * .02f;
            float Critical = 1f + ((calcStats.SpellCrit + stats.BonusCritHealMultiplier) / 2f);
            #endregion
            #region Water Shield and Mana Calculations
            float WSC = (float)Math.Max((1.6 * (1 - (calcStats.SpellHaste))), 1.1f);
            float Orb = ((400 * (1 + (character.ShamanTalents.ImprovedShields * .05f))) * (1 + stats.WaterShieldIncrease)) + (options.TotemWS1 ? 27 : 0);
            float Orbs = 3 + (options.WaterShield2 ? 1 : 0);
            #endregion
            #region Totem Stats
            float TotemHW1 = (options.TotemHW1 ? 88 : 0);  // +88 Healing to HW
            float TotemHW2 = (options.TotemHW2 ? 24 : 0);  // -24 Cost of HW
            float TotemHW3 = (options.TotemHW3 ? 79 : 0);  // -79 Cost of HW
            float TotemCH1 = (options.TotemCH1 ? 78 : 0);  // -78 Cost of CH
            float TotemCH2 = (options.TotemCH2 ? 102 : 0);  // +102 Base Heal CH
            float TotemCH3 = (options.TotemCH3 ? 20 : 0);  // -20 Cost of CH
            float TotemCH4 = (options.TotemCH4 ? 87 : 0);  // +87 Base Heal of CH
            float TotemLHW1 = (options.TotemLHW1 ? 1 : 0); // +79 Spellpower to LHW
            float TotemWS1 = (options.TotemWS1 ? 1 : 0);  // Totem of Thunderhead: 27 per orb trigger (untested for IWS), 2 MP5
            #endregion
            #region Variable from Options
            float OverHeal = options.OverhealingPercentage * .01f;
            float TrueHealing = (1 - OverHeal);
            #endregion
            #region Earthliving Weapon Calculations
            float ELWHPS = 0;
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                ELWHPS = (652 + (Healing * (5 / 11)) * (12 / 15)) * (1f + ((character.ShamanTalents.Purification) * .02f));
            float ExtraELW = (options.TankHeal ? 0 : 1) + (options.ELWGlyph ? .5f : 0);
            #endregion
            #region Earth Shield Calculations
            float ESTimer = 24 + (character.ShamanTalents.ImprovedEarthShield * 4);
            if (options.ESInterval < ESTimer)
                if (options.ESInterval > 0)
                    options.ESInterval = ESTimer;
            float ESUptime = 0;
            float ESCasts = (Time / options.ESInterval);
            if (options.ESInterval > 0)
                ESUptime = (ESTimer) / (options.ESInterval);
            float ESHPS = (float)Math.Round((((((((2022f + (Healing * 3f)) * (1f + (.05f * (character.ShamanTalents.ImprovedShields
                + character.ShamanTalents.ImprovedEarthShield)))) / 6f * (6f + character.ShamanTalents.ImprovedEarthShield))) 
                 * (1f + ((character.ShamanTalents.Purification) * .02f))) * ESUptime) / Time),2);
            float ESMPS = 0;
            if (options.ESInterval > 0)
                ESMPS = (ESCasts * ((float)Math.Round(660f * (1f - ((character.ShamanTalents.TidalFocus) * .01f))))) / Time;
            float MTime = Time - (((float)Math.Max(((1.5f * (1 - (calcStats.SpellHaste)))), 1.1f)) * ESCasts);
            float RCP = MTime / Time;

            #endregion
            #region Chain Heal Calculations
            float TankCH = (options.TankHeal ? 1 : (1.75f + (options.GlyphCH ? .125f : 0)));
            float CHMana = (((835 - ((TotemCH1 + TotemCH3) + (preserve * (options.TankHeal ? 1 : (3f + (options.GlyphCH ? 1f : 0)))))) * 
                (1f - ((character.ShamanTalents.TidalFocus) * .01f))));
            float CHCast = (float)Math.Max(((2.6 - stats.CHCTDecrease) * (1 - (calcStats.SpellHaste))), 1.1f);
            float CHHeal = ((((((1130 + TotemCH2 + TotemCH4) + (Healing * (2.5f / 3.5f))) * (1f + (character.ShamanTalents.ImprovedChainHeal * .02f)) *
                (1f + ((character.ShamanTalents.Purification) * .02f))) * Critical) * TankCH)  + (ExtraELW * ELWHPS * CHCast / 2f)) * (1f + stats.CHHWHealIncrease);
            float CHHPS = CHHeal / CHCast;
            float CHMPS = CHMana / CHCast;
            #endregion
            #region Lesser Healing Wave Calculations
            float LHWMana = (((550 - preserve) * (1f - ((character.ShamanTalents.TidalFocus * .01f)))) - ((Orb * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .1f)));
            float LHWCast = (float)Math.Max(((1.5f * (1 - (calcStats.SpellHaste)))), 1.1f) + ((WSC / Orbs * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .2f));
            float LHWHeal = ((((((1720 + (1.88f * (TotemLHW1 * (1 + (.02f * character.ShamanTalents.TidalWaves)))) + ((Healing * (1 + (.02f * 
                character.ShamanTalents.TidalWaves)))) * (1.5f / 3.5f))) * (1f + ((character.ShamanTalents.Purification) * .02f))))
                * ((options.LHWPlus ? (options.TankHeal ? 1.2f : 1) : 1))) 
                );
            float LHWHPS = LHWHeal / LHWCast;
            float LHWMPS = LHWMana / LHWCast;
            #endregion
            #region Healing Wave Calculations
            float HWMana = (((1099 - (TotemHW2 + TotemHW3) - preserve) * (1f - ((character.ShamanTalents.TidalFocus * .01f)))) - ((Orb * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield / 3)));
            float HWCast = (float)Math.Max((((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) * (1 - (calcStats.SpellHaste)))), 1.1f)
                + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float HWHeal = (((((3250 + (1.88f * (TotemHW1 * (1 + (.04f * character.ShamanTalents.TidalWaves)))) + ((Healing * (1 + (.04f * 
                character.ShamanTalents.TidalWaves)))) * ((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) / 3.5f))) * 
                (1f + ((character.ShamanTalents.Purification) * .02f)))) ) * (1 + (character.ShamanTalents.HealingWay * .06f));
            float HWHPS = HWHeal / HWCast;
            float HWMPS = HWMana / HWCast;
            #endregion
            #region Lesser Healing Wave Calculations with Tidal Waves up
            float LHWTCMana = (((550 - preserve) * (1f - ((character.ShamanTalents.TidalFocus * .01f)))) - ((Orb * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .2f)));
            float LHWTCCast = (float)Math.Max((((1.5f * (1 - (character.ShamanTalents.TidalWaves * .06f))) * (1 - (calcStats.SpellHaste)))), 1.1f) + ((WSC / Orbs * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .2f));
            float LHWTCHeal = ((((((1720 + (1.88f * (TotemLHW1 * (1 + (.02f * character.ShamanTalents.TidalWaves)))) + ((Healing * (1 + (.02f *
                character.ShamanTalents.TidalWaves)))) * (1.5f / 3.5f))) * (1f + (character.ShamanTalents.Purification * .02f))))
                * ((options.LHWPlus ? (options.TankHeal ? 1.2f : 1) : 1))));
            float LHWTCHPS = LHWTCHeal / LHWTCCast;
            float LHWTCMPS = LHWTCMana / LHWTCCast;
            #endregion
            #region Healing Wave Calculations with Tidal Waves up
            float HWTCMana = (((1099 - (TotemHW2 + TotemHW3) - preserve) * (1f - ((character.ShamanTalents.TidalFocus * .01f)))) - ((Orb * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .1f)));
            float HWTCCast = (float)Math.Max((((((3 - (character.ShamanTalents.ImprovedHealingWave * .1f))) * (1 - (character.ShamanTalents.TidalWaves * .06f)))
                * (1 - (calcStats.SpellHaste)))), 1.1f) + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float HWTCHeal = (((((3250 + (1.88f * (TotemHW1 * (1 + (.04f * character.ShamanTalents.TidalWaves)))) + ((Healing * (1 + (.04f *
                character.ShamanTalents.TidalWaves)))) * ((3 - (character.ShamanTalents.ImprovedHealingWave * .1f)) / 3.5f))) *
                (1f + ((character.ShamanTalents.Purification) * .02f))))) * (1 + (character.ShamanTalents.HealingWay * .06f));
            float HWTCHPS = HWTCHeal / HWTCCast;
            float HWTCMPS = HWTCMana / HWTCCast;
            #endregion
            #region Riptide Calculations
            float RTMana = (((792 - preserve) * (1f - ((character.ShamanTalents.TidalFocus * .01f)))) - ((Orb * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield * .1f)));
            float RTCast = (float)Math.Max(((1.5f * (1 - (calcStats.SpellHaste)))), 1.1f) + ((WSC / Orbs * stats.SpellCrit) *
                (character.ShamanTalents.ImprovedWaterShield / 3));
            float RTHeal = ((((1670f + (Healing * .5f)) * (1f + ((character.ShamanTalents.Purification) * .02f))))) * 
                character.ShamanTalents.Riptide;
            float RTHPS = RTHeal / RTCast;
            float RTHOTHPS = ((((1670f + (Healing * .5f)) * (1f + ((character.ShamanTalents.Purification) * .02f))) / 15f)) * 
                character.ShamanTalents.Riptide;
            float RTMPS = RTMana / RTCast;
            #endregion
            #region Ancestral Awakening Calcs
            float AACalc = stats.SpellCrit * 1.5f * (.1f * character.ShamanTalents.AncestralAwakening);
            #endregion
            #region Sequence Construction
            #region  RT + HW Spam, sequence reconstructs with haste
            #region Determine which rotation for HW based on Haste.
            float HWPerRotation = 0;
            float HWR1 = (RTCast + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))));
            float HWR2 = (RTCast + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + HWCast);
            float HWR3 = (RTCast + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + HWCast + HWCast);
            if (HWR1 > (6 - stats.RTCDDecrease))
                HWPerRotation = 2;
            if (HWR1 < (6 - stats.RTCDDecrease))
                if (HWR2 > (6 - stats.RTCDDecrease))
                    HWPerRotation = 3;
            if (HWR1 < (6 - stats.RTCDDecrease))
                if (HWR2 < (6 - stats.RTCDDecrease))
                    if (HWR3 > (6 - stats.RTCDDecrease))
                        HWPerRotation = 4;
            if (HWR1 < (6 - stats.RTCDDecrease))
                if (HWR2 < (6 - stats.RTCDDecrease))
                    if (HWR3 < (6 - stats.RTCDDecrease))
                        HWPerRotation = 5;
            #endregion
            float RTHWRotLength = RTCast + ((HWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) * 2) + (HWCast * (HWPerRotation - 2));
            float RTPerc = RTCast / RTHWRotLength;
            float RTHWHWTCPerc = (2 * HWTCCast) / RTHWRotLength;
            float RTHWHWPerc = ((HWPerRotation - 2) * HWCast) / RTHWRotLength;
            float RTHWAA = (RTHPS * RTPerc) + (HWHPS * RTHWHWPerc) + (RTHWHWTCPerc * HWTCHPS) * AACalc;
            calcStats.RTHWHPS = (RTHPS * RTPerc * (1 + stats.SpellCrit)) + (HWHPS * RTHWHWPerc * (1 + stats.SpellCrit)) + (RTHWHWTCPerc * HWTCHPS *
                (1 + stats.SpellCrit)) + RTHWAA + RTHOTHPS + ESHPS + ELWHPS;
            calcStats.RTHWMPS = (RTMPS * RTPerc) + (HWMPS * RTHWHWPerc) + (RTHWHWTCPerc * HWTCMPS) + ESMPS;
            #endregion
            #region  RT + LHW Spam, sequence reconstructs with haste
            #region Determine which rotation for LHW based on Haste.
            float LHWPerRotation = 0;
            float LHWR1 = (RTCast + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))));
            float LHWR2 = (RTCast + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + LHWCast);
            float LHWR3 = (RTCast + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + (LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) + LHWCast + LHWCast);
            if (LHWR1 > (6 - stats.RTCDDecrease))
                LHWPerRotation = 2;
            if (LHWR1 < (6 - stats.RTCDDecrease))
                if (LHWR2 > (6 - stats.RTCDDecrease))
                    LHWPerRotation = 3;
            if (LHWR1 < (6 - stats.RTCDDecrease))
                if (LHWR2 < (6 - stats.RTCDDecrease))
                    if (LHWR3 > (6 - stats.RTCDDecrease))
                        LHWPerRotation = 4;
            if (LHWR1 < (6 - stats.RTCDDecrease))
                if (LHWR2 < (6 - stats.RTCDDecrease))
                    if (LHWR3 < (6 - stats.RTCDDecrease))
                        LHWPerRotation = 5;
            #endregion
            float RTLHWRotLength = RTCast + ((LHWCast * (1 - (character.ShamanTalents.TidalWaves * .06f))) * 2) + (LHWCast * (LHWPerRotation - 2));
            float RTPerc2 = RTCast / RTLHWRotLength;
            float RTLHWLHWTCPerc = (2 * LHWTCCast) / RTLHWRotLength;
            float RTLHWLHWPerc = ((LHWPerRotation - 2) * LHWCast) / RTLHWRotLength;
            float RTLHWAA = ((RTHPS * RTPerc2) + (LHWHPS * RTLHWLHWPerc) + (RTLHWLHWTCPerc * LHWTCHPS)) * AACalc;
            calcStats.RTLHWHPS = (RTHPS * RTPerc2 * (1 + stats.SpellCrit)) + (LHWHPS * RTLHWLHWPerc * (1 + stats.SpellCrit)) + 
                (RTLHWLHWTCPerc * LHWTCHPS * (1 + stats.SpellCrit)) + RTLHWAA + RTHOTHPS + ESHPS + ELWHPS;
            calcStats.RTLHWMPS = (RTMPS * RTPerc2) + (LHWMPS * RTLHWLHWPerc) + (RTLHWLHWTCPerc * LHWTCMPS) + ESMPS;
            #endregion
            #region  RT + CH Spam, sequence reconstructs with haste
            #region Determine which rotation for CH based on Haste.
            float CHPerRotation = 0;
            float CHR1 = RTCast + (CHCast * 2);
            float CHR2 = RTCast + (CHCast * 3);
            float CHR3 = RTCast + (CHCast * 4);
            if (CHR1 > (6 - stats.RTCDDecrease))
                CHPerRotation = 2;
            if (CHR1 < (6 - stats.RTCDDecrease))
                if (CHR2 > (6 - stats.RTCDDecrease))
                    CHPerRotation = 3;
            if (CHR1 < (6 - stats.RTCDDecrease))
                if (CHR2 < (6 - stats.RTCDDecrease))
                    if (CHR3 > (6 - stats.RTCDDecrease))
                        CHPerRotation = 4;
            if (CHR1 < (6 - stats.RTCDDecrease))
                if (CHR2 < (6 - stats.RTCDDecrease))
                    if (CHR3 < (6 - stats.RTCDDecrease))
                        CHPerRotation = 5;
            #endregion
            float RTCHRotLength = RTCast + (CHCast * CHPerRotation);
            float RTPerc3 = RTCast / RTCHRotLength;
            float RTCHCHPerc = (CHPerRotation * CHCast) / RTCHRotLength;
            float RTCHAA = (RTHPS * RTPerc3) * AACalc;
            calcStats.RTCHHPS = (RTHPS * RTPerc3 * (1 + stats.SpellCrit)) + (RTCHCHPerc * (((CHHPS * (CHPerRotation - 1)) + (CHHPS * 1.2f)) / CHPerRotation) * (1 + stats.SpellCrit)) + RTCHAA + ESHPS + ELWHPS;
            calcStats.RTCHMPS = (RTMPS * RTPerc3) + (CHMPS * RTCHCHPerc) + ESMPS;
            #endregion
            #region HW Spam
            calcStats.HWSpamHPS = (HWHPS * (1 + stats.SpellCrit)) + (HWHPS * AACalc) + ESHPS + ELWHPS;
            calcStats.HWSpamMPS = HWMPS + ESMPS;
            #endregion
            #region LHW Spam
            calcStats.LHWSpamHPS = (LHWHPS * (1 + stats.SpellCrit)) + (LHWHPS * AACalc) + ESHPS + ELWHPS;
            calcStats.LHWSpamMPS = LHWMPS + ESMPS;
            #endregion
            #region CH Spam
            calcStats.CHSpamHPS = CHHPS + ESHPS + ELWHPS;
            calcStats.CHSpamMPS = CHMPS + ESMPS;
            #endregion
            calcStats.ChosenSequence = options.HealingStyle;
            calcStats.FightHPS = 0;
            calcStats.FightMPS = 0;
            if (options.HealingStyle.Equals("CH Spam"))
                calcStats.FightHPS = (calcStats.CHSpamHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("CH Spam"))
                calcStats.FightMPS = (calcStats.CHSpamMPS * RCP) + ESMPS;
            if (options.HealingStyle.Equals("HW Spam"))
                calcStats.FightHPS = (calcStats.HWSpamHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("HW Spam"))
                calcStats.FightMPS = (calcStats.HWSpamMPS * RCP) + ESMPS;
            if (options.HealingStyle.Equals("LHW Spam"))
                calcStats.FightHPS = (calcStats.LHWSpamHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("LHW Spam"))
                calcStats.FightMPS = (calcStats.LHWSpamMPS * RCP) + ESMPS;
            if (options.HealingStyle.Equals("RT+HW"))
                calcStats.FightHPS = (calcStats.RTHWHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("RT+HW"))
                calcStats.FightMPS = (calcStats.RTHWMPS * RCP) + ESMPS;
            if (options.HealingStyle.Equals("RT+LHW"))
                calcStats.FightHPS = (calcStats.RTLHWHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("RT+LHW"))
                calcStats.FightMPS = (calcStats.RTLHWMPS * RCP) + ESMPS;
            if (options.HealingStyle.Equals("RT+CH"))
                calcStats.FightHPS = (calcStats.RTCHHPS * TrueHealing) + (TrueHealing * ESHPS) + ELWHPS;
            if (options.HealingStyle.Equals("RT+CH"))
                calcStats.FightMPS = (calcStats.RTCHMPS * RCP) + ESMPS;
            #endregion
            #region Final Stats
            calcStats.TillOOM = (calcStats.TotalManaPool + (stats.Mp5 / 5 * 60 * options.FightLength)) / calcStats.FightMPS;
            if (calcStats.TillOOM > (60 * options.FightLength))
                calcStats.TillOOM = 60 * options.FightLength;
            calcStats.TotalHPS = calcStats.FightHPS;
            calcStats.TotalHealed = (calcStats.FightHPS * (calcStats.TillOOM / (options.FightLength * 60f))) * (options.FightLength * 60f);
            calcStats.OverallPoints = calcStats.TotalHealed / 10f;
            calcStats.SubPoints[0] = calcStats.TotalHealed / 10f;

            return calcStats;
            #endregion
        }
        #endregion
        
        //
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
            #region Create the statistics for a given character:
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 135, Intellect = 141, Spirit = 145 };
                    break;

                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 135, Spirit = 145 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;

                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 137, Spirit = 146 };
                    break;

                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 137, Intellect = 124, Spirit = 144 };
                    break;

                default:
                    statsRace = new Stats();
                    break;
            }
            #endregion
            #region Other Final Stats
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            //Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace;
            if (statModifier != null)
                statsTotal += statModifier;

            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            float IntMultiplier = (1 + statsTotal.BonusIntellectMultiplier) * (1 + (float)Math.Round(.02f * character.ShamanTalents.AncestralKnowledge, 2));
            statsTotal.Intellect = (float)Math.Floor((statsRace.Intellect) * IntMultiplier) + 
                (float)Math.Floor((statsBaseGear.Intellect + statsBuffs.Intellect/* + statsEnchants.Intellect*/) * IntMultiplier);
            statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower) + (float)Math.Floor(statsTotal.Intellect * .05f * character.ShamanTalents.NaturesBlessing);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1 + statsTotal.BonusHealthMultiplier);

            // Fight options:

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            float OrbRegen = (options.WaterShield3 ? 130 : 100);
            statsTotal.Mp5 += ((options.WaterShield ? OrbRegen : 0)) + (options.TotemWS1 ? 2 : 0) + (options.WaterShield ? (100f * statsTotal.WaterShieldIncrease) : 0);

            return statsTotal;
            #endregion
        }
        #region Chart data area
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
                case "Stat Relative Weights":
                    StatRelativeWeight[] stats = new StatRelativeWeight[] {
                      new StatRelativeWeight("Int", new Stats() { Intellect = 10f }),
                      new StatRelativeWeight("Haste", new Stats() { HasteRating = 10f }),
                      new StatRelativeWeight("+Heal", new Stats() { SpellPower = 10f}),
                      new StatRelativeWeight("Mp5", new Stats() { Mp5 = 10f }),
                      new StatRelativeWeight("Spell Crit", new Stats() { CritRating = 10f })};

                    // Get the percentage total healing is changed by a change in a single stat:

                    float healPct = 0f;
                    foreach (StatRelativeWeight weight in stats)
                    {
                        CharacterCalculationsRestoSham statCalc = (CharacterCalculationsRestoSham)GetCharacterCalculations(character, null, weight.Stat);
                        weight.PctChange = ((statCalc.TotalHealed - calc.TotalHealed) / calc.TotalHealed);
                        if (weight.Name == "+Heal")
                            healPct = weight.PctChange;
                    }

                    // Create the chart data points:

                    foreach (StatRelativeWeight weight in stats)
                    {
                        ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(weight.Name);
                        comp.OverallPoints = weight.PctChange / healPct;
                        comp.SubPoints[0] = comp.OverallPoints;
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
            return SpecialEffects.GetRelevantStats(stats) + new Stats()
            {
                #region Base Stats
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                Earthliving = stats.Earthliving,
                #endregion
                #region Trinkets
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                BonusManaPotion = stats.BonusManaPotion,
                #endregion
                #region Totems and Sets
                WaterShieldIncrease = stats.WaterShieldIncrease,
                CHHWHealIncrease = stats.CHHWHealIncrease,
                CHCTDecrease = stats.CHCTDecrease,
                RTCDDecrease = stats.RTCDDecrease,
                #endregion
                #region Gems
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier
                #endregion
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.HasteRating +
                stats.BonusIntellectMultiplier + stats.BonusCritHealMultiplier + stats.BonusManaPotion + stats.ManaRestoreOnCast_5_15 +
                stats.ManaRestoreFromMaxManaPerSecond + stats.CHHWHealIncrease + stats.WaterShieldIncrease + stats.SpellHaste +
                stats.BonusIntellectMultiplier + stats.BonusManaMultiplier + stats.ManacostReduceWithin15OnHealingCast + stats.CHCTDecrease +
                stats.RTCDDecrease + stats.Earthliving) > 0;
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
