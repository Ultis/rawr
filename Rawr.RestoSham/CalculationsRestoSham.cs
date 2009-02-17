using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    [Rawr.Calculations.RawrModelInfo("RestoSham", "Spell_Nature_Magicimmunity", Character.CharacterClass.Shaman)]
    class CalculationsRestoSham : CalculationsBase
    {
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
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //Max Crit
						RedId = Potent[0], YellowId = Smooth[0], BlueId = Sundered[0], PrismaticId = Smooth[0], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //Max Haste
						RedId = Reckless[0], YellowId = Quick[0], BlueId = Energized[0], PrismaticId = Quick[0], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Uncommon", //Max Int
						RedId = Luminous[0], YellowId = Brilliant[0], BlueId = Dazzling[0], PrismaticId = Brilliant[0], MetaId = Ember },


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
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Potent[1], YellowId = Smooth[1], BlueId = Sundered[1], PrismaticId = Smooth[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Reckless[1], YellowId = Quick[1], BlueId = Energized[1], PrismaticId = Quick[1], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Rare", Enabled = true,
						RedId = Luminous[1], YellowId = Brilliant[1], BlueId = Dazzling[1], PrismaticId = Brilliant[1], MetaId = Ember },


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
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Max Crit
						RedId = Potent[2], YellowId = Smooth[2], BlueId = Sundered[2], PrismaticId = Smooth[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Max Haste
						RedId = Reckless[2], YellowId = Quick[2], BlueId = Energized[2], PrismaticId = Quick[2], MetaId = Ember },
					new GemmingTemplate() { Model = "RestoSham", Group = "Epic", //Max Int
						RedId = Luminous[2], YellowId = Brilliant[2], BlueId = Dazzling[2], PrismaticId = Brilliant[2], MetaId = Ember },


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

        //
        // Colors of the ratings we track:
        //
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
                          // "Basic Stats:Spirit",
                          "Basic Stats:Spell Power",
                          "Basic Stats:MP5*Mana regeneration while casting",
                          // "Basic Stats:MP5 (outside FSR)*Mana regeneration while not casting (outside the 5-second rule)",
                          "Basic Stats:Heal Spell Crit*This includes all static talents including those that are not shown on the in-game character pane",
                          "Basic Stats:Spell Haste",
                          "Totals:Total HPS*Includes Burst and Sustained",
                          "Totals:Time to OOM*In Seconds",
                          "Totals:Total Healed*Includes Burst and Sustained",
                          "Fight Details:Fight HPS" ,
                          "Fight Details:ES + LHW HPS",
                          "Fight Details:ES + LHW OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + HW HPS",
                          "Fight Details:ES + HW OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + CH HPS",
                          "Fight Details:ES + CH OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + RT + CHx2 HPS",
                          "Fight Details:ES + RT + CHx2 OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Burst Details:Burst HPS",
                          "Burst Details:RT + LHWx2 + CH HPS",
                          "Burst Details:RT + LHWx2 + CH OOM*Longest time you can keep this burst up",
                          "Burst Details:RT + HWx2 + CH HPS",
                          "Burst Details:RT + HWx2 + CH OOM*Longest time you can keep this burst up",
                          "Burst Details:RT + LHWx4 HPS",
                          "Burst Details:RT + LHWx4 OOM*Longest time you can keep this burst up",
                          "Burst Details:RT + LHWx3 HPS",
                          "Burst Details:RT + LHWx3 OOM*Longest time you can keep this burst up" };
                }
                return _characterDisplayCalcLabels;
            }
        }

        //
        // Custom chart names:
        //
        public override string[] CustomChartNames
        {
            get
            {
                return new string[]{"Time to OOM",
                                      "Stat Relative Weights"};
            }
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


        //
        // Item types we're interested in:
        //
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


        //
        // Do the actual calculations:
        //
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            return GetCharacterCalculations(character, additionalItem, null);
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem,
                                                                  Stats statModifier)
        {
            Stats stats = GetCharacterStats(character, additionalItem, statModifier);
            CharacterCalculationsRestoSham calcStats = new CharacterCalculationsRestoSham();
            calcStats.BasicStats = stats;

            // calcStats.Mp5OutsideFSR = 5f * (.001f + (float)Math.Sqrt((double)stats.Intellect) * stats.Spirit * .009327f) + stats.Mp5;
            calcStats.SpellCrit = .022f + character.StatConversion.GetSpellCritFromIntellect(stats.Intellect) / 100f
                + character.StatConversion.GetSpellCritFromRating(stats.CritRating) / 100f + stats.SpellCrit + 
                (.01f * (character.ShamanTalents.TidalMastery + character.ShamanTalents.ThunderingStrikes + 
                (character.ShamanTalents.BlessingOfTheEternals * 2)));
            
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;

            // Total Mana Pool for the fight:

            float onUse = 0.0f;
            if (options.ManaPotAmount > 0)
                onUse += (options.ManaPotAmount * (1 + stats.BonusManaPotion)) / (options.FightLength * 60 / 5);
            if (options.ManaTideEveryCD)
                onUse += (((float)Math.Truncate(options.FightLength / 5.025f) + 1) *
					(stats.Mana * (.24f + ((options.ManaTidePlus ? .04f : 0))))) * character.ShamanTalents.ManaTideTotem;

            float mp5 = (stats.Mp5 * (1f - (options.OutsideFSRPct / 100f)));
            // Out of Five Second Rule regen currently removed.
            // mp5 += (calcStats.Mp5OutsideFSR * (options.OutsideFSRPct / 100f));
            mp5 += (float)Math.Round((stats.Intellect * ((character.ShamanTalents.UnrelentingStorm / 3) * .1f)), 0);
            calcStats.TotalManaPool = stats.Mana + onUse + (mp5 * (60f / 5f) * options.FightLength) +
                ((stats.ManaRestoreFromMaxManaPerSecond * stats.Mana) * ((options.FightLength * 60f)) * .85f);
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                stats.SpellPower += (character.ShamanTalents.ElementalWeapons * .1f * 150f);
            if (character.ActiveBuffsContains("Flametongue Totem"))
                stats.SpellPower += (character.ShamanTalents.EnhancingTotems * .05f * 144);


            // Stats, Talents, and Options
            float CurrentMana = calcStats.TotalManaPool;
            float Awaken = (.1f * character.ShamanTalents.AncestralAwakening);
            float TankCH = (options.TankHeal ? 1 : (1.75f + (options.GlyphCH ? .125f : 0)));
            float ExtraELW = (options.TankHeal ? 0 : 1) + (options.ELWGlyph ? .5f : 0);
            float Redux = (1f - ((character.ShamanTalents.TidalFocus) * .01f));
            float Time = (options.FightLength * 60f);
            float EFL = Time - (1.5f * (Time / options.ESInterval));
            float Critical = 1f + ((calcStats.SpellCrit + stats.BonusCritHealMultiplier) / 2f);
            if (character.ShamanTalents.TidalForce > 0)
                Critical += (1.2f / 180) / 2;
            float Purify = (1f + ((character.ShamanTalents.Purification) * .02f));
            float Healing = 1.88f * stats.SpellPower;
            float Hasted = 1 - (stats.HasteRating / 3279);
            float OverHeal = options.OverhealingPercentage * .01f;
            float Burst = options.BurstPercentage * .01f;
            float Sustained = 1 - Burst;
            float TrueHealing = (1 - OverHeal);
            float ELWHPS = 0;
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                ELWHPS = (652 + (Healing * (5 / 11)) * (12 / 15)) * Purify;

            // Water Shield Variables
            float Orb = 400 * (1 + (character.ShamanTalents.ImprovedShields * .05f));
            float Orbs = 3 + (options.WaterShield2 ? 1 : 0);

            // Spell Casting Times
            float WSC = 1.5f * Hasted;
            float LHWC = (1.5f * Hasted) + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f));
            float HWC = ((3f - (.1f * character.ShamanTalents.ImprovedHealingWave)) * Hasted) + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float CHC = 2.5f * Hasted;
            float RTC = ((1.5f * Hasted) + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3))) * character.ShamanTalents.Riptide;
            if (WSC < 1)
                WSC = 1;
            if (LHWC < (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f))))
                LHWC = 1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f));
            if (HWC < (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3))))
                HWC = (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3)));
            if (CHC < 1.5f)
                CHC = 1.5f;
            if (character.ShamanTalents.Riptide > 0)
                if (RTC < (1 + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3))))
                    RTC = 1 + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float TidalWaves = 1;
            if (character.ShamanTalents.TidalWaves > 0)
                TidalWaves += 1 - ((.2f * character.ShamanTalents.TidalWaves) * .3f);
            float LHWTC = LHWC * TidalWaves;
            float HWTC = HWC * TidalWaves;
            if (LHWTC < (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f))))
                LHWTC = 1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f));
            if (HWTC < (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3))))
                HWTC = (1.5f + ((WSC / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3)));

            // Spell Mana Costs
            float LHWM = ((550 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .1f)));
            float HWM = ((1099 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3)));
            float CHM = ((835 * Redux));
            float RTM = (((792 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3)))) * character.ShamanTalents.Riptide;

            // Spell Healing Amounts
            float LHWHeal = ((((1720f + (Healing * (LHWC / 3.5f))) * Purify) * (Critical + Awaken)) * ((options.LHWPlus ? (options.TankHeal ? 1.2f : 1) : 1))) * TrueHealing;
            float HWHeal = ((((3250f + (Healing * (HWC / 3.5f))) * (1 + (.06f * character.ShamanTalents.HealingWay)) * Purify)) * (Critical + Awaken)) * TrueHealing;
            float CHHeal = ((((1130f + (Healing * (CHC / 3.5f))) * (1f + (((character.ShamanTalents.ImprovedChainHeal / 2f)) * .02f)) * Purify) * Critical) * TankCH) * TrueHealing + (ExtraELW * ELWHPS * CHC);
            float CHRTHeal = 0;
            if (character.ShamanTalents.Riptide > 0)
                CHRTHeal = ((((1130f + (Healing * (CHC / 3.5f))) * (1f + (((character.ShamanTalents.ImprovedChainHeal / 2f)) * .02f)) * Purify) * 1.2f * Critical) * TankCH) * TrueHealing + (ExtraELW * ELWHPS * CHC);
            if (character.ShamanTalents.Riptide < 1)
                CHRTHeal = CHHeal;
            float RTHeal = ((((1670f + (Healing * .5f)) * Purify) * (Critical + Awaken)) * TrueHealing) * character.ShamanTalents.Riptide;
            float RTHot = ((((1670f + (Healing * .5f)) * Purify) / 15f) * TrueHealing) * character.ShamanTalents.Riptide;

            // Earth Shield Specific Calcs
            if (options.ESInterval < 32)
                options.ESInterval = 32;
            float ESC = 0;
            float ESCMPS = 0;
            if (character.ShamanTalents.EarthShield > 0)
                ESC = ((((Time / options.ESInterval) * (((2022f + (Healing * 3f)) * (1f + (.05f * (character.ShamanTalents.ImprovedShields + character.ShamanTalents.ImprovedEarthShield)))) / 6f * (6f + character.ShamanTalents.ImprovedEarthShield))) / Time) * Purify) * TrueHealing;
            if (character.ShamanTalents.EarthShield > 0)
                ESCMPS = (((Time / options.ESInterval) * (660f * Redux)));

            // MPS Calculation Variables
            float RTMPS = 0;
            if (character.ShamanTalents.Riptide > 0)
                RTMPS = ((RTM / RTC)) * character.ShamanTalents.Riptide;
            float RTSequence = 3 + character.ShamanTalents.Riptide;
            float RTLWH2CHRTMPSMT = ((RTMPS + ((LHWM / LHWTC) * 2) + (CHM / CHC)) / RTSequence);
            float RTWH2CHRTMPSMT = ((RTMPS + ((HWM / HWC) * 2) + (CHM / CHC)) / RTSequence);
            float RTLWH4MPSMT = ((RTMPS + ((LHWM / LHWTC) * 2) + ((LHWM / LHWC) * 2)) / (RTSequence + 1));
            float RTWH3MPSMT = ((RTMPS + ((HWM / HWTC) * 2) + ((HWM / HWC))) / RTSequence);
            float ESLHWMPSMT = (ESCMPS + ((EFL / LHWC) * LHWM)) / Time;
            float ESHWMPSMT = (ESCMPS + ((EFL / (HWC)) * HWM)) / Time;
            float ESCHMPSMT = (ESCMPS + ((EFL / CHC) * CHM)) / Time;

            // Calculate Best HPS
            float RTHPS = 0;
            if (character.ShamanTalents.Riptide > 0)
                RTHPS = (RTHeal / RTC) * character.ShamanTalents.Riptide;
            calcStats.RTLWH2CHRTHPSMT = (((RTHPS + ((LHWHeal / LHWC) * 2) + (CHRTHeal / CHC)) / RTSequence) * (Math.Min(((CurrentMana / RTLWH2CHRTMPSMT) / Time), 1)));
            calcStats.RTWH2CHRTHPSMT = (((RTHPS + ((HWHeal / HWC) * 2) + (CHRTHeal / CHC)) / RTSequence) * (Math.Min(((CurrentMana / RTLWH2CHRTMPSMT) / Time), 1)));
            calcStats.RTLWH4HPSMT = (((RTHPS + ((LHWHeal / LHWC) * 4)) / (RTSequence + 1)) * (Math.Min(((CurrentMana / RTLWH2CHRTMPSMT) / Time), 1)));
            calcStats.RTWH3HPSMT = (((RTHPS + ((HWHeal / HWC) * 3)) / RTSequence) * (Math.Min(((CurrentMana / RTLWH2CHRTMPSMT) / Time), 1)));
            calcStats.ESLHWHPSMT = (ESC + ((LHWHeal * (EFL / (LHWC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESLHWMPSMT) / Time), 1));
            calcStats.ESHWHPSMT = (ESC + ((HWHeal * (EFL / ((HWC) * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESHWMPSMT) / Time), 1));
            calcStats.ESCHHPSMT = (ESC + ((CHHeal * (EFL / (CHC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESCHMPSMT) / Time), 1));

            if (character.ShamanTalents.Riptide > 0)
                calcStats.ESRTCHCHHPSMT = (((ESC + ((RTHeal * (EFL / (RTC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / RTMPS) / Time), 1)) / 3) + (calcStats.ESCHHPSMT / 3 * 2)) * character.ShamanTalents.Riptide;
            if (character.ShamanTalents.Riptide < 1)
                calcStats.ESRTCHCHHPSMT = calcStats.ESCHHPSMT - 1;
            if (calcStats.ESCHHPSMT > calcStats.ESHWHPSMT)
                if (calcStats.ESCHHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESCHHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESCHHPSMT;
            if (calcStats.ESHWHPSMT > calcStats.ESCHHPSMT)
                if (calcStats.ESHWHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESHWHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESHWHPSMT;
            if (calcStats.ESLHWHPSMT > calcStats.ESCHHPSMT)
                if (calcStats.ESLHWHPSMT > calcStats.ESHWHPSMT)
                    if (calcStats.ESLHWHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESLHWHPSMT;
            if (calcStats.ESRTCHCHHPSMT > calcStats.ESHWHPSMT)
                if (calcStats.ESRTCHCHHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESRTCHCHHPSMT > calcStats.ESCHHPSMT)
                        calcStats.FightHPS = calcStats.ESRTCHCHHPSMT;
            if (calcStats.RTLWH2CHRTHPSMT > calcStats.RTWH2CHRTHPSMT)
                if (calcStats.RTLWH2CHRTHPSMT > calcStats.RTLWH4HPSMT)
                    if (calcStats.RTLWH2CHRTHPSMT > calcStats.RTWH3HPSMT)
                        calcStats.BurstHPS = calcStats.RTLWH2CHRTHPSMT;
            if (calcStats.RTWH2CHRTHPSMT > calcStats.RTLWH2CHRTHPSMT)
                if (calcStats.RTWH2CHRTHPSMT > calcStats.RTLWH4HPSMT)
                    if (calcStats.RTWH2CHRTHPSMT > calcStats.RTWH3HPSMT)
                        calcStats.BurstHPS = calcStats.RTWH2CHRTHPSMT;
            if (calcStats.RTLWH4HPSMT > calcStats.RTLWH2CHRTHPSMT)
                if (calcStats.RTLWH4HPSMT > calcStats.RTWH2CHRTHPSMT)
                    if (calcStats.RTLWH4HPSMT > calcStats.RTWH3HPSMT)
                        calcStats.BurstHPS = calcStats.RTLWH4HPSMT;
            if (calcStats.RTWH3HPSMT > calcStats.RTWH2CHRTHPSMT)
                if (calcStats.RTWH3HPSMT > calcStats.RTLWH4HPSMT)
                    if (calcStats.RTWH3HPSMT > calcStats.RTLWH2CHRTHPSMT)
                        calcStats.BurstHPS = calcStats.RTWH3HPSMT;

            
            // Calculate Tim till OOM
            calcStats.ESLHWMPSMT = CurrentMana / ESLHWMPSMT;
            calcStats.ESHWMPSMT = CurrentMana / ESHWMPSMT;
            calcStats.ESCHMPSMT = CurrentMana / ESCHMPSMT;
            if (character.ShamanTalents.Riptide > 0)
                calcStats.ESRTCHCHMPSMT = CurrentMana / ((RTMPS + ESCHMPSMT + ESCHMPSMT) / 3);
            if (character.ShamanTalents.Riptide < 1)
                calcStats.ESRTCHCHMPSMT = calcStats.ESCHMPSMT - 1;
            calcStats.RTLWH2CHRTMPSMT = CurrentMana / RTLWH2CHRTMPSMT;
            calcStats.RTWH2CHRTMPSMT = CurrentMana / RTWH2CHRTMPSMT;
            calcStats.RTLWH4MPSMT = CurrentMana / RTLWH4MPSMT;
            calcStats.RTWH3MPSMT = CurrentMana / RTWH3MPSMT;
            if (calcStats.ESCHMPSMT > calcStats.ESHWMPSMT)
                if (calcStats.ESCHMPSMT > calcStats.ESLHWMPSMT)
                    if (calcStats.ESCHMPSMT > calcStats.ESRTCHCHMPSMT)
                        calcStats.FightMPS = calcStats.ESCHMPSMT;
            if (calcStats.ESHWMPSMT > calcStats.ESCHMPSMT)
                if (calcStats.ESHWMPSMT > calcStats.ESLHWMPSMT)
                    if (calcStats.ESHWMPSMT > calcStats.ESRTCHCHMPSMT)
                        calcStats.FightMPS = calcStats.ESHWMPSMT;
            if (calcStats.ESLHWMPSMT > calcStats.ESCHMPSMT)
                if (calcStats.ESLHWMPSMT > calcStats.ESHWMPSMT)
                    if (calcStats.ESLHWMPSMT > calcStats.ESRTCHCHMPSMT)
                        calcStats.FightMPS = calcStats.ESLHWMPSMT;
            if (calcStats.ESRTCHCHMPSMT > calcStats.ESHWMPSMT)
                if (calcStats.ESRTCHCHMPSMT > calcStats.ESLHWMPSMT)
                    if (calcStats.ESRTCHCHMPSMT > calcStats.ESCHMPSMT)
                        calcStats.FightMPS = calcStats.ESRTCHCHMPSMT;
            if (calcStats.RTLWH2CHRTMPSMT > calcStats.RTWH2CHRTMPSMT)
                if (calcStats.RTLWH2CHRTMPSMT > calcStats.RTLWH4MPSMT)
                    if (calcStats.RTLWH2CHRTMPSMT > calcStats.RTWH3MPSMT)
                        calcStats.BurstMPS = calcStats.RTLWH2CHRTMPSMT;
            if (calcStats.RTWH2CHRTMPSMT > calcStats.RTLWH2CHRTMPSMT)
                if (calcStats.RTWH2CHRTMPSMT > calcStats.RTLWH4MPSMT)
                    if (calcStats.RTWH2CHRTMPSMT > calcStats.RTWH3MPSMT)
                        calcStats.BurstMPS = calcStats.RTWH2CHRTMPSMT;
            if (calcStats.RTLWH4MPSMT > calcStats.RTLWH2CHRTMPSMT)
                if (calcStats.RTLWH4MPSMT > calcStats.RTWH2CHRTMPSMT)
                    if (calcStats.RTLWH4MPSMT > calcStats.RTWH3MPSMT)
                        calcStats.BurstMPS = calcStats.RTLWH4MPSMT;
            if (calcStats.RTWH3MPSMT > calcStats.RTWH2CHRTMPSMT)
                if (calcStats.RTWH3MPSMT > calcStats.RTLWH4MPSMT)
                    if (calcStats.RTWH3MPSMT > calcStats.RTLWH2CHRTMPSMT)
                        calcStats.BurstMPS = calcStats.RTWH3MPSMT;

            // Final Stats
            calcStats.TillOOM = (Burst * calcStats.BurstMPS) + (Sustained * calcStats.FightMPS);
            calcStats.TotalHPS = (Burst * calcStats.BurstHPS) + (Sustained * calcStats.FightHPS) + ELWHPS;
            calcStats.TotalHealed = calcStats.TotalHPS * (options.FightLength * 60f);
            calcStats.OverallPoints = calcStats.TotalHealed / 10f;
            calcStats.SubPoints[0] = calcStats.TotalHealed / 10f;

            return calcStats;
        }


        //
        // Create the statistics for a given character:
        //
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
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
            // statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower) + (float)Math.Floor(statsTotal.Intellect * .05f * character.ShamanTalents.NaturesBlessing);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1 + statsTotal.BonusHealthMultiplier);


            // Fight options:

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
			float OrbRegen = (options.WaterShield3 ? 130 : 100);
			statsTotal.Mp5 += (options.WaterShield ? OrbRegen : 0);

            return statsTotal;
        }


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
                      new StatRelativeWeight("Int", new Stats() { Intellect = 1f }),
                      new StatRelativeWeight("Haste", new Stats() { HasteRating = 1f }),
                      new StatRelativeWeight("+Heal", new Stats() { SpellPower = 1f}),
                      new StatRelativeWeight("Mp5", new Stats() { Mp5 = 1f }),
                      new StatRelativeWeight("Spell Crit", new Stats() { CritRating = 1f })};

                    // Get the percentage total healing is changed by a change in a single stat:

                    float healPct = 0f;
                    foreach (StatRelativeWeight weight in stats)
                    {
                        CharacterCalculationsRestoSham statCalc = (CharacterCalculationsRestoSham)GetCharacterCalculations(character, null, weight.Stat);
                        weight.PctChange = (statCalc.TotalHealed - calc.TotalHealed) / calc.TotalHealed;
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


        //
        // Get stats which are relevant to resto shammies:
        //
        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                // Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
				BonusCritHealMultiplier = stats.BonusCritHealMultiplier
			};
        }


        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.HasteRating + 
                stats.BonusIntellectMultiplier + stats.BonusCritHealMultiplier + stats.BonusManaPotion + stats.ManaRestoreOnCast_5_15 + 
                stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }


        //
        // Retrieve our options from XML:
        //
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsRestoSham calcOpts = serializer.Deserialize(reader) as CalculationOptionsRestoSham;
            return calcOpts;
        }
    }
}
