    using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{
	[Rawr.Calculations.RawrModelInfo("Healadin", "Spell_Holy_HolyBolt", Character.CharacterClass.Paladin)]
	public class CalculationsHealadin : CalculationsBase
    {

        public CalculationsHealadin()
            : base()
        {
            _subPointNameColorsMana = new Dictionary<string, System.Drawing.Color>();
            _subPointNameColorsMana.Add("Mana", System.Drawing.Color.FromArgb(0, 0, 255));

            _subPointNameColorsRating = new Dictionary<string, System.Drawing.Color>();
            _subPointNameColorsRating.Add("Fight Healing", System.Drawing.Color.Red);
            _subPointNameColorsRating.Add("Burst Healing", System.Drawing.Color.CornflowerBlue);

            _subPointNameColors = _subPointNameColorsRating;
        }

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs
                //Green
                int[] dazzling = { 39984, 40094, 40175 };

                //Yellow
                int[] brilliant = { 39912, 40012, 40123, 42148 };

                //Orange
                int[] luminous = { 39946, 40047, 40151 };

                //Meta
                int insightful = 41401;
                // int revitalizing = 41376;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Healadin", Group = "Uncommon",
						RedId = brilliant[0], YellowId = brilliant[0], BlueId = brilliant[0], PrismaticId = brilliant[0], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Uncommon",
						RedId = luminous[0], YellowId = brilliant[0], BlueId = dazzling[0], PrismaticId = brilliant[0], MetaId = insightful },

					new GemmingTemplate() { Model = "Healadin", Group = "Rare", Enabled = true,
						RedId = brilliant[1], YellowId = brilliant[1], BlueId = brilliant[1], PrismaticId = brilliant[1], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Rare", Enabled = true,
						RedId = luminous[1], YellowId = brilliant[1], BlueId = dazzling[1], PrismaticId = brilliant[1], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Epic",
						RedId = brilliant[2], YellowId = brilliant[2], BlueId = brilliant[2], PrismaticId = brilliant[2], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Epic",
						RedId = luminous[2], YellowId = brilliant[2], BlueId = dazzling[2], PrismaticId = brilliant[2], MetaId = insightful },
						
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = brilliant[3], YellowId = brilliant[1], BlueId = brilliant[3], PrismaticId = brilliant[1], MetaId = insightful },
					new GemmingTemplate() { Model = "Healadin", Group = "Jeweler",
						RedId = brilliant[3], YellowId = brilliant[3], BlueId = brilliant[3], PrismaticId = brilliant[3], MetaId = insightful },
				};
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Swift Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Arcane Intellect"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Judgements of the Wise"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Elemental Oath"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Wrath of Air Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath (Spell Power)"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of the Frost Wyrm"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Fish Feast"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Tree of Life Aura"));

            character.PaladinTalents.GlyphOfHolyLight = true;
            character.PaladinTalents.GlyphOfDivinity = true;
            character.PaladinTalents.GlyphOfSealOfWisdom = true;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Seal of Wisdom");
                _relevantGlyphs.Add("Glyph of Seal of Light");
                _relevantGlyphs.Add("Glyph of Holy Light");
                _relevantGlyphs.Add("Glyph of Flash of Light");
                _relevantGlyphs.Add("Glyph of Holy Shock");
                _relevantGlyphs.Add("Glyph of Divinity");
                _relevantGlyphs.Add("Glyph of Beacon of Light");
                _relevantGlyphs.Add("Glyph of the Wise");
                _relevantGlyphs.Add("Glyph of Lay on Hands");
            }
            return _relevantGlyphs;
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHealadin();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spell Power",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",

					"Cycle Stats:Total Healed",
					"Cycle Stats:Total Mana",
					"Cycle Stats:Average Healing per sec",
					"Cycle Stats:Average Healing per mana",

                    "Rotation Info:Holy Light Time",
                    "Rotation Info:Flash of Light Time",
                    "Rotation Info:Holy Shock Time",
                    "Rotation Info:Sacred Shield Time",
                    "Rotation Info:Beacon of Light Time",
                    "Rotation Info:Judgement Time",

                    "Healing Breakdown:Holy Light Healed",
                    "Healing Breakdown:Flash of Light Healed",
                    "Healing Breakdown:Holy Shock Healed",
                    "Healing Breakdown:Sacred Shield Healed",
                    "Healing Breakdown:Beacon of Light Healed",
                    "Healing Breakdown:Glyph of HL Healed",
                    "Healing Breakdown:Other Healed*From trinekt procs",

                    "Holy Light:HL Average Heal",
                    "Holy Light:HL Crit",
                    "Holy Light:HL Cast Time",
                    "Holy Light:HL Averege Cost",
                    "Holy Light:HL Healing per sec",
                    "Holy Light:HL Healing per mana",

                    "Flash of Light:FoL Average Heal",
                    "Flash of Light:FoL Crit",
                    "Flash of Light:FoL Cast Time",
                    "Flash of Light:FoL Averege Cost",
                    "Flash of Light:FoL Healing per sec",
                    "Flash of Light:FoL Healing per mana",

                    "Holy Shock:HS Average Heal",
                    "Holy Shock:HS Crit",
                    "Holy Shock:HS Cast Time",
                    "Holy Shock:HS Averege Cost",
                    "Holy Shock:HS Healing per sec",
                    "Holy Shock:HS Healing per mana",

                    "Sacred Shield:SS Average Absorb",
                    "Sacred Shield:SS Healing per sec",
                    "Sacred Shield:SS Healing per mana",

				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Mana Pool Breakdown",
                    "Mana Usage Breakdown",
					};
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColorsMana = null;
        private Dictionary<string, System.Drawing.Color> _subPointNameColorsRating = null;
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;

        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                Dictionary<string, System.Drawing.Color> ret = _subPointNameColors;
                _subPointNameColors = _subPointNameColorsRating;
                return ret;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
                        Item.ItemType.Plate,
                        Item.ItemType.Mail,
                        Item.ItemType.Leather,
                        Item.ItemType.Cloth,
                        Item.ItemType.None,
						Item.ItemType.Shield,
						Item.ItemType.Libram,
						Item.ItemType.OneHandAxe,
						Item.ItemType.OneHandMace,
						Item.ItemType.OneHandSword,
						Item.ItemType.TwoHandAxe,
						Item.ItemType.TwoHandMace,
						Item.ItemType.TwoHandSword
					});
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if ((slot == Item.ItemSlot.OffHand && enchant.Slot != Item.ItemSlot.OffHand) || slot == Item.ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot);
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHealadin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHealadin(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHealadin));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsHealadin calcOpts = serializer.Deserialize(reader) as CalculationOptionsHealadin;
			return calcOpts;
		}

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            Stats stats;
            CharacterCalculationsHealadin calc = null;
            PaladinTalents talents = character.PaladinTalents;

            for (int i = 0; i < 5; i++)
            {
                stats = GetCharacterStats(character, additionalItem, true, calc);
                calc = new CharacterCalculationsHealadin();

                Rotation rot = new Rotation(character, stats);
                calc.OverallPoints = rot.CalculateHealing(calc);
            }
            calc.BasicStats = GetCharacterStats(character, additionalItem, false, null);

            return calc;
            #region Old
//            }

//            const float base_mana = 4394;            
//            float fightLength = calcOpts.Length * 60;
//            float activeLength = fightLength * calcOpts.Activity;

//            float divine_pleas = (float)Math.Ceiling((fightLength - 60f) / (60f * calcOpts.DivinePlea));
//            float glyph_sow = (talents.GlyphOfSealOfWisdom ? .95f : 1f);
//            float heal_multi = (talents.GlyphOfSealOfLight ? 1.05f : 1f) * (1f + stats.HealingReceivedMultiplier)
//                * (1f - .5f * divine_pleas * 15f / fightLength) * (1f + talents.Divinity * .01f);


//            calc.ManaBase = stats.Mana;
//            calc.ManaLayOnHands = 1950 * ((talents.GlyphOfDivinity ? 1 : 0) + (calcOpts.LoHSelf ? 1 : 0)) * (talents.GlyphOfDivinity ? 2f : 1f);
//            calc.ManaArcaneTorrent = (character.Race == Character.CharacterRace.BloodElf ? stats.Mana * .06f * (float)Math.Ceiling(fightLength / 120f - .25f) : 0);
//            calc.ManaDivinePlea = stats.Mana * .25f * divine_pleas;
//            calc.ManaMp5 = fightLength * stats.Mp5 / 5;
//            calc.ManaPotion = (1 + stats.BonusManaPotion) * calcOpts.ManaAmt;
//            calc.ManaReplenishment = stats.ManaRestoreFromMaxManaPerSecond * stats.Mana * fightLength * calcOpts.Replenishment;
//            calc.ManaOther += stats.ManaRestore;
//            if (stats.HighestStat > 0)
//            {
//                float greatnessMana = stats.HighestStat * 15;
//                calc.ManaReplenishment += stats.ManaRestoreFromMaxManaPerSecond * fightLength * greatnessMana * calcOpts.Replenishment; // Replenishment
//                calc.ManaDivinePlea += divine_pleas * greatnessMana * .25f; // Divine Plea
//            }
//            calc.TotalMana = calc.ManaBase + calc.ManaDivinePlea + calc.ManaMp5 + calc.ManaOther + calc.ManaPotion + 
//                calc.ManaReplenishment + calc.ManaLayOnHands;

//            float benediction = 1f - talents.Benediction * .02f;

//            #region Maintaining JotP
//            if (calcOpts.JotP && talents.JudgementsOfThePure > 0)
//            {
////                float oldhaste = fightLength * (1f + stats.SpellHaste);
////                stats.SpellHaste = ((1 + stats.SpellHaste) * (1 + talents.JudgementsOfThePure * .03f)) - 1;

////              float seals_cast = (float)Math.Ceiling((fight_length - 60f) / 120f);
////              calc.JotPCasts += seals_cast;
////              calc.JotPUsage += ((float)Math.Round(base_mana * .14f) - ied) * seals_cast;

//                float miss_chance = (float)Math.Max(0f, .09f - talents.EnlightenedJudgements * .02f - stats.PhysicalHit);
//                float average_casts = 1 / (1f - miss_chance);
//                float judgements_cast = (float)Math.Ceiling(fightLength / 60f);
//                calc.JotPCasts += judgements_cast * average_casts;
//                calc.JotPUsage += (float)Math.Round(base_mana * .05f) * judgements_cast * average_casts;

// //               float newhaste = fightLength * (1f + stats.SpellHaste) - (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste)) * calc.JotPCasts;
// //               calc.JotPHaste = 1f - oldhaste / newhaste;
//            }
//            #endregion

//            #region Maintaining BoL
//            if (talents.BeaconOfLight > 0)
//            {
//                calc.BoLCasts = (float)Math.Ceiling(fightLength * calcOpts.BoLUp / (talents.GlyphOfBeaconOfLight ? 90f : 60f));
//                calc.BoLUsage = calc.BoLCasts * ((float)Math.Round(base_mana * .35f * benediction) - stats.SpellsManaReduction);
//            }
//            #endregion

//            calc.HealedOther += stats.Healed;

//            #region Flash of Light
//            const float fol_coef = 1.5f / 3.5f * 66f / 35f * 1.25f;
//            calc.FoLAvgHeal = (835.5f + (stats.SpellPower + stats.FlashOfLightSpellPower) * fol_coef) * (1f + talents.HealingLight * .04f) * (1f + stats.FlashOfLightMultiplier) * heal_multi;
//            float fol_baseMana = (int)(base_mana * .07f);
//            calc.FoLCrit = stats.SpellCrit + stats.FlashOfLightCrit + talents.HolyPower * .01f + (talents.GlyphOfFlashOfLight ? .05f : 0f);
//            calc.FoLCost = fol_baseMana * glyph_sow - fol_baseMana * .12f * talents.Illumination * calc.FoLCrit - stats.SpellsManaReduction;
//            float fol_dimana = fol_baseMana * glyph_sow * .5f - fol_baseMana * .12f * talents.Illumination * calc.FoLCrit;
//            float fol_heal = calc.FoLAvgHeal * ((1 - calc.FoLCrit) + 1.5f  * (1f + stats.BonusCritHealMultiplier) * calc.FoLCrit);
//            calc.FoLCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
//            calc.FoLHPS = fol_heal / calc.FoLCastTime;
//            float fol_mps = calc.FoLCost / calc.FoLCastTime;
//            float fol_dimps = fol_dimana / calc.FoLCastTime;
//            calc.FoLHPM = fol_heal / calc.FoLCost;
//            #endregion

//            #region Holy Light
//            const float hl_coef = 2.5f / 3.5f * 66f / 35f * 1.25f;
//            calc.HLAvgHeal = (5166f + (stats.HolyLightSpellPower + stats.SpellPower) * hl_coef) * (1f + talents.HealingLight * .04f) * heal_multi;
//            float hl_baseMana = (float)Math.Round(base_mana * .29f);
//            calc.HLCrit = stats.SpellCrit + stats.HolyLightCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f;
//            calc.HLCost = hl_baseMana * glyph_sow * (1f - stats.HolyLightPercentManaReduction)
//                - hl_baseMana * .12f * talents.Illumination * calc.HLCrit - stats.HolyLightManaCostReduction - stats.SpellsManaReduction;
//            float hl_dimana = hl_baseMana * glyph_sow * (1f - stats.HolyLightPercentManaReduction) * 0.5f
//                - hl_baseMana * .12f * talents.Illumination * calc.HLCrit - stats.HolyLightManaCostReduction;
//            float hl_heal = calc.HLAvgHeal * ((1 - calc.HLCrit) + 1.5f * (1f + stats.BonusCritHealMultiplier) * calc.HLCrit);
//            calc.HLCastTime = (2.5f - .5f / 3 * talents.LightsGrace) / (1f + stats.SpellHaste);
//            calc.HLHPS = hl_heal / calc.HLCastTime;
//            float hl_mps = calc.HLCost / calc.HLCastTime;
//            float hl_dimps = hl_dimana / calc.HLCastTime;
//            calc.HLHPM = hl_heal / calc.HLCost;
//            #endregion

//            #region Holy Shock
//            const float hs_coef = 1.5f / 3.5f * 66f / 35f;
//            calc.HSAvgHeal = (2500f + stats.SpellPower * hs_coef) * (1f + talents.HealingLight * .04f) * heal_multi;
//            float hs_baseMana = (float)Math.Round(base_mana * .18f * benediction);
//            calc.HSCrit = stats.SpellCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f + stats.HolyShockCrit;
//            calc.HSCost = hs_baseMana * glyph_sow - hs_baseMana * .12f * talents.Illumination * calc.HSCrit - stats.SpellsManaReduction;
//            float hs_heal = calc.HSAvgHeal * ((1 - calc.HSCrit) + 1.5f * (1f + stats.BonusCritHealMultiplier) * calc.HSCrit);
//            calc.HSCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
//            calc.HSHPS = hs_heal / calc.HSCastTime;
//            float hs_mps = calc.HSCost / calc.HSCastTime;
//            calc.HSHPM = hs_heal / calc.HSCost;
//            #endregion

//            //#region Sacred Shield
//            //const float SSUptime = 1f;
//            //calc.SSAvgAbsorb = 500f + .75f * stats.SpellPower;
//            //calc.SSCasts = (float)Math.Ceiling(fightLength / 30f) * SSUptime;
//            //float ss_baseMana = (float)Math.Round(.12f * base_mana) * benediction - stats.SpellsManaReduction;
//            //calc.SSUsage = calc.SSCasts * ss_baseMana;
//            //calc.SSAbsorbed = (float)Math.Floor(fightLength / 6f) * calc.SSAvgAbsorb;
//            //calc.SSHPM = calc.SSAbsorbed / calc.SSUsage;
//            //calc.SSHPS = calc.SSAbsorbed / (calc.SSCasts * calc.HSCastTime);
//            //#endregion

//            if (talents.HolyShock > 0 && calcOpts.HolyShock > 0)
//            {
//                calc.HSTime = (float)(Math.Floor(fightLength * calcOpts.HolyShock / (talents.GlyphOfHolyShock ? 5f : 6f)) * calc.HSCastTime);
//                calc.HSHealed = calc.HSTime * calc.HSHPS;
//                calc.HSUsage = calc.HSTime * hs_mps;
//            }
//            float df_casts = 0, df_manaCost = 0, df_manaSaved = 0, df_healing = 0;
//            if (talents.DivineFavor > 0)
//            {
//                df_casts = (float)Math.Ceiling((fightLength - .5f) / 120f);
//                df_manaCost = (float)Math.Round(base_mana * .03f) * df_casts * benediction - stats.SpellsManaReduction;
//                df_manaSaved = hl_baseMana * .6f * df_casts * (1f - calc.HLCrit);
//                df_healing = calc.HLAvgHeal * (1f - calc.HLCrit) * (1.5f * (1f + stats.BonusCritHealMultiplier) - 1f);
//            }
//            float di_time = 0, di_manausage = 0, di_healing = 0;
//            if (talents.DivineIllumination > 0)
//            {
//                di_time = (float)Math.Ceiling((fightLength - 1f) / 180f) * 15f * calcOpts.Activity;
//                di_manausage = di_time * hl_dimps;
//                di_healing = di_time * calc.HLHPS;
//            }

//            float healing_mana = calc.TotalMana - calc.BoLUsage - calc.JotPUsage - calc.HSUsage - calc.SSUsage - df_manaCost + df_manaSaved - di_manausage;
//            float healing_time = activeLength - (calc.HSCastTime * (calc.BoLCasts + calc.JotPCasts + calc.SSCasts)) - calc.HSTime - di_time;
//            calc.HLTime = Math.Min(healing_time, Math.Max(0, (healing_mana - (healing_time * fol_mps)) / (hl_mps - fol_mps)));
//            calc.FoLTime = healing_time - calc.HLTime;
//            if (calc.HLTime == 0)
//            {
//                calc.FoLTime = Math.Min(healing_time, healing_mana / fol_mps);
//            }

//            calc.FoLUsage = calc.FoLTime * fol_mps;
//            calc.HLUsage = calc.HLTime * hl_mps;

//            calc.FoLHealed = calc.FoLTime * calc.FoLHPS;
//            calc.HLHealed = calc.HLTime * calc.HLHPS + di_healing + df_healing;
//            calc.HLTime += di_time;
//            if (talents.GlyphOfHolyLight) calc.HLGlyph = calc.HLHealed * calcOpts.GHL_Targets * 0.1f * heal_multi;// *
//                //((1 - stats.SpellCrit - talents.HolyPower * .01f) + 1.5f * (1f + stats.BonusCritHealMultiplier) * (stats.SpellCrit + talents.HolyPower * .01f));

//            calc.TotalHealed = calc.FoLHealed + calc.HLHealed + calc.HSHealed;
//            if (talents.BeaconOfLight > 0) calc.TotalHealed += calc.BoLHealed = calcOpts.BoLEff * calcOpts.BoLUp * calc.TotalHealed;
//            calc.TotalHealed += calc.SSAbsorbed + calc.HLGlyph + calc.HealedOther;

//            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;
//            calc.AvgHPS = calc.TotalHealed / fightLength;

//            calc.FightPoints = calc.AvgHPS * (1f - calcOpts.BurstScale);
//            calc.BurstPoints = calc.HLHPS * calcOpts.BurstScale;
//            calc.OverallPoints = calc.FightPoints + calc.BurstPoints; 

//            return calc;
            #endregion
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, true, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, bool computeAverageStats, CharacterCalculationsHealadin calc)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            float fightLength = calcOpts.Length * 60;

            Stats statsRace;
            if (character.Race == Character.CharacterRace.Draenei)
            {
                statsRace = new Stats() { Health = 6754, Mana = 4114, Stamina = 146, Intellect = 99 };
            }
            else if (character.Race == Character.CharacterRace.Dwarf)
            {
                statsRace = new Stats() { Health = 6754, Mana = 4114, Stamina = 152, Intellect = 97 };
            }
            else if (character.Race == Character.CharacterRace.Human)
            {
                statsRace = new Stats() { Health = 6754, Mana = 4114, Stamina = 149, Intellect = 98 };
            }
            else
            {
                statsRace = new Stats() { Health = 6754, Mana = 4114, Stamina = 141, Intellect = 102 };
            }


            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats stats = statsBaseGear + statsBuffs + statsRace;
            ConvertRatings(stats, talents, calcOpts);
            if (computeAverageStats)
            {
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
                        if (calc == null)
                        {
                            trigger = 1.5f / calcOpts.Activity / (1f + stats.SpellHaste);
                            if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit) { trigger *= stats.SpellCrit; }
                        }
                        else
                        {
                            if (effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellHit)
                            {
                                trigger = 1f / Rotation.GetHealingCastsPerSec(calc);
                            }
                            else if (effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.SpellCrit)
                            {
                                trigger = 1f / Rotation.GetHealingCritsPerSec(calc);
                            }
                            else
                            {
                                trigger = 1f / Rotation.GetSpellCastsPerSec(calc);
                            }
                        }

                        if (effect.MaxStack > 1)
                        {
                            float timeToMax = (float)Math.Min(fightLength, effect.Chance * trigger * effect.MaxStack);
                            statsAverage += effect.Stats * (effect.MaxStack * ((fightLength - .5f * timeToMax) / fightLength));
                        }
                        else
                        {
                            statsAverage += effect.GetAverageStats(trigger);
                        }
                    }
                }
                statsAverage.ManaRestore *= fightLength;
                statsAverage.Healed *= fightLength;

                stats = statsBaseGear + statsBuffs + statsRace + statsAverage;
                ConvertRatings(stats, talents, calcOpts);
            }

            return stats;
        }

        private void ConvertRatings(Stats stats, PaladinTalents talents, CalculationOptionsHealadin calcOpts)
        {
            stats.Stamina *= 1 + stats.BonusStaminaMultiplier;
            stats.Intellect *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f);
            stats.HighestStat *= (1 + stats.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f);
            stats.SpellPower += 0.04f * (stats.Intellect + stats.HighestStat) * talents.HolyGuidance;
            stats.SpellCrit = .03336f + stats.SpellCrit + (stats.Intellect + stats.HighestStat) / 16666.66709f
                + stats.CritRating / 4590.598679f + talents.SanctityOfBattle * .01f + talents.Conviction * .01f;

            stats.SpellHaste = (1f + talents.JudgementsOfThePure * (calcOpts.JotP ? .03f : 0f))
                * (1f + stats.SpellHaste)
                * (1f + stats.HasteRating / 3278.998947f)
                - 1f;

            stats.Mana = (stats.Mana + stats.Intellect * 15) * (1f + stats.BonusManaMultiplier);
            stats.Health = stats.Health + stats.Stamina * 10f;
            stats.PhysicalHit += stats.HitRating / 3278.998947f;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Mana Pool Breakdown")
            {
                _subPointNameColors = _subPointNameColorsMana;
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin Base = new ComparisonCalculationHealadin("Base");
                ComparisonCalculationHealadin Mp5 = new ComparisonCalculationHealadin("Mp5");
                ComparisonCalculationHealadin Potion = new ComparisonCalculationHealadin("Potion");
                ComparisonCalculationHealadin Replenishment = new ComparisonCalculationHealadin("Replenishment");
                ComparisonCalculationHealadin ArcaneTorrent = new ComparisonCalculationHealadin("Arcane Torrent");
                ComparisonCalculationHealadin DivinePlea = new ComparisonCalculationHealadin("Divine Plea");
                ComparisonCalculationHealadin LoH = new ComparisonCalculationHealadin("Lay on Hands");
                ComparisonCalculationHealadin Other = new ComparisonCalculationHealadin("Other");

                Base.OverallPoints = Base.ThroughputPoints = calc.ManaBase;
                Mp5.OverallPoints = Mp5.ThroughputPoints = calc.ManaMp5;
                LoH.OverallPoints = LoH.ThroughputPoints = calc.ManaLayOnHands;
                Potion.OverallPoints = Potion.ThroughputPoints = calc.ManaPotion;
                Replenishment.OverallPoints = Replenishment.ThroughputPoints = calc.ManaReplenishment;
                ArcaneTorrent.OverallPoints = ArcaneTorrent.ThroughputPoints = calc.ManaArcaneTorrent;
                DivinePlea.OverallPoints = DivinePlea.ThroughputPoints = calc.ManaDivinePlea;
                Other.OverallPoints = Other.ThroughputPoints = calc.ManaOther;

                return new ComparisonCalculationBase[] { Base, Mp5, Potion, Replenishment, LoH, ArcaneTorrent, DivinePlea, Other };
            }
            else if (chartName == "Mana Usage Breakdown")
            {
                _subPointNameColors = _subPointNameColorsMana;
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");
                ComparisonCalculationHealadin SS = new ComparisonCalculationHealadin("Sacred Shield");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.UsageFoL;
                HL.OverallPoints = HL.ThroughputPoints = calc.UsageHL;
                HS.OverallPoints = HS.ThroughputPoints = calc.UsageHS;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.UsageJotP;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.UsageBoL;
                SS.OverallPoints = SS.ThroughputPoints = calc.UsageSS;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL, SS };
            }
            return new ComparisonCalculationBase[] {};
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                PhysicalHit = stats.PhysicalHit,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,
                FlashOfLightCrit = stats.FlashOfLightCrit,
                FlashOfLightSpellPower = stats.FlashOfLightSpellPower,
                FlashOfLightMultiplier = stats.FlashOfLightMultiplier,
                HolyShockCrit = stats.HolyShockCrit,
                HolyLightSpellPower = stats.HolyLightSpellPower,
                HolyLightCrit = stats.HolyLightCrit,
                HolyLightManaCostReduction = stats.HolyLightManaCostReduction,
                HolyLightPercentManaReduction = stats.HolyLightPercentManaReduction,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                // Gear Procs
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                SpellsManaReduction = stats.SpellsManaReduction,
                SacredShieldICDReduction = stats.SacredShieldICDReduction,
                HolyShockHoTOnCrit = stats.HolyShockHoTOnCrit
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use
                    || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit)
                {
                    if (HasRelevantSpecialEffectStats(effect.Stats)) s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        public bool HasRelevantSpecialEffectStats(Stats stats)
        {
            return (stats.Intellect + stats.SpellPower + stats.CritRating + stats.HasteRating + stats.ManaRestore + stats.Mp5 + stats.Healed + stats.HighestStat) > 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.Intellect + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.SpellCrit + stats.SpellHaste
                + stats.HitRating + stats.PhysicalHit + stats.GreatnessProc + stats.Heal1Min + stats.BonusHoTOnDirectHeals + stats.Mana
                + stats.HasteRating + stats.BonusIntellectMultiplier + stats.HolyLightPercentManaReduction + stats.HolyShockCrit + 
                + stats.BonusManaPotion + stats.FlashOfLightMultiplier + stats.FlashOfLightSpellPower + stats.FlashOfLightCrit + stats.HolyLightManaCostReduction
                + stats.HolyLightCrit + stats.HolyLightSpellPower + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusManaMultiplier
                + stats.HealingReceivedMultiplier + stats.BonusCritHealMultiplier + stats.SpellsManaReduction
                + stats.SacredShieldICDReduction + stats.HolyShockHoTOnCrit) > 0;
            bool survivalStats = (stats.Stamina + stats.Health) > 0;
            bool ignoreStats = (stats.Agility + stats.Strength + stats.AttackPower + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry
                + stats.HitRating + stats.ArmorPenetrationRating + stats.Spirit + stats.DodgeRating + stats.ParryRating
                + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue) > 0;
            bool specialEffect = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use
                    || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.HealingSpellCast || effect.Trigger == Trigger.HealingSpellCrit || effect.Trigger == Trigger.HealingSpellHit)
                {
                    specialEffect = HasRelevantSpecialEffectStats(effect.Stats);
                    if (specialEffect) break;
                }
            }
            return (wantedStats || ((survivalStats || specialEffect) && !ignoreStats));
        }
    }
}
