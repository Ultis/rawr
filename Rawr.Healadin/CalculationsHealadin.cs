    using System;
using System.Collections.Generic;

namespace Rawr.Healadin
{
	[Rawr.Calculations.RawrModelInfo("Healadin", "Spell_Holy_HolyBolt", Character.CharacterClass.Paladin)]
	public class CalculationsHealadin : CalculationsBase
    {

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
                int revitalizing = 41376;

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
						RedId = brilliant[3], YellowId = brilliant[3], BlueId = brilliant[3], PrismaticId = brilliant[3], MetaId = insightful },
				};
            }
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
					"Cycle Stats:Other Heals*From Trinket Procs",
                    "Holy Light:HL Average Heal*Average non crit heal",
                    "Holy Light:HL Crit",
                    "Holy Light:HL Cast Time",
                    "Holy Light:HL Healing per sec",
                    "Holy Light:HL Healing per mana",
                    "Holy Light:HL Rotation Time",
                    "Holy Light:HL Healed",
                    "Holy Light:HL Mana Usage",
                    "Holy Light:Glyph of HL Healed",
                    "Flash of Light:FoL Average Heal*Average non crit heal",
                    "Flash of Light:FoL Crit",
                    "Flash of Light:FoL Cast Time",
                    "Flash of Light:FoL Healing per sec",
                    "Flash of Light:FoL Healing per mana",
                    "Flash of Light:FoL Rotation Time",
                    "Flash of Light:FoL Healed",
                    "Flash of Light:FoL Mana Usage",
                    "Holy Shock:HS Average Heal*Average non crit heal",
                    "Holy Shock:HS Crit",
                    "Holy Shock:HS Cast Time",
                    "Holy Shock:HS Healing per sec",
                    "Holy Shock:HS Healing per mana",
                    "Holy Shock:HS Rotation Time",
                    "Holy Shock:HS Healed",
                    "Holy Shock:HS Mana Usage",
                    "Sacred Shield:SS Average Absorb",
                    "Sacred Shield:SS Casts",
                    "Sacred Shield:SS Healing per sec",
                    "Sacred Shield:SS Healing per mana",
                    "Sacred Shield:SS Absorbed",
                    "Sacred Shield:SS Mana Usage",
                    "Beacon of Light:BoL Healed",
                    "Beacon of Light:BoL Casts",
                    "Beacon of Light:BoL Mana Usage",
                    "Judgement:JotP Effective Haste*Effective haste after discounting GCDs used to keep the buff up",
                    "Judgement:JotP Casts",
                    "Judgement:JotP Mana Usage"
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
                    "Glyphs"
					};
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Fight Healing", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Burst Healing", System.Drawing.Color.CornflowerBlue);
                }
                return _subPointNameColors;
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            CharacterCalculationsHealadin calc = new CharacterCalculationsHealadin();
            calc.BasicStats = stats;
            PaladinTalents talents = character.PaladinTalents;

			CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
            if (calcOpts == null) calcOpts = new CalculationOptionsHealadin();

            const float base_mana = 4394;            
            float fight_length = calcOpts.Length * 60;
			float active_length = fight_length * calcOpts.Activity;

            float divine_pleas = (float)Math.Ceiling((fight_length - 60f) / (60f * calcOpts.DivinePlea));
            float glyph_sow = (calcOpts.GlyphSealOfWisdom ? .95f : 1f);
            float heal_multi = (calcOpts.GlyphSealOfLight ? 1.05f : 1f) * (1f + stats.HealingReceivedMultiplier)
                * (1f - .5f * divine_pleas * 15f / fight_length) * (1f + talents.Divinity * .01f);


            calc.ManaBase = stats.Mana;
            calc.ManaLayOnHands = 1950 * ((calcOpts.GlyphDivinity ? 1 : 0) + (calcOpts.LoHSelf ? 1 : 0)) * (calcOpts.GlyphDivinity ? 2f : 1f);
            calc.ManaArcaneTorrent = (character.Race == Character.CharacterRace.BloodElf ? stats.Mana * .06f * (float)Math.Ceiling(fight_length / 120f - .25f) : 0);
            calc.ManaDivinePlea = stats.Mana * .25f * divine_pleas;
            calc.ManaMp5 = fight_length * stats.Mp5 / 5;
            calc.ManaPotion = (1 + stats.BonusManaPotion) * calcOpts.ManaAmt;
            calc.ManaReplenishment = stats.ManaRestoreFromMaxManaPerSecond * stats.Mana * fight_length * calcOpts.Replenishment;
            calc.ManaSpiritual = calcOpts.Spiritual;
            if (stats.ManaRestoreOnCast_10_45 > 0)
            {
                calc.ManaOther += (float)Math.Ceiling(fight_length / 60f - .25f) * stats.ManaRestoreOnCast_10_45;
            }
            if (stats.ManaRestoreOnCrit_25_45 > 0)
            {
                calc.ManaOther += (float)Math.Ceiling(fight_length / (45f + 6f / stats.SpellCrit)) * stats.ManaRestoreOnCrit_25_45;
            }
            if (stats.ManaRestore5min > 0)
            {
                calc.ManaOther += (float)Math.Ceiling((fight_length - 30f) / 300f) * stats.ManaRestore5min;
			}
			if (stats.GreatnessProc > 0)
			{
				float intMultiple = (1f + (GetItemStats(character, additionalItem)/* + GetEnchantsStats(character)*/ + GetBuffsStats(character.ActiveBuffs)).BonusIntellectMultiplier)
					* (1 + .03f * talents.DivineIntellect);
				float greatnessInt = stats.GreatnessProc * intMultiple;
				float greatnessMana = greatnessInt * 15;
				float greatnessProcs = (float)Math.Ceiling((fight_length - 15f) / 45f);
				calc.ManaOther += stats.ManaRestoreFromMaxManaPerSecond * 15f * greatnessMana * calcOpts.Replenishment * greatnessProcs; // Replenishment
				calc.ManaOther += (divine_pleas * 15f / fight_length) * greatnessMana * .25f * greatnessProcs; // Divine Plea
			}
            calc.TotalMana = calc.ManaBase + calc.ManaDivinePlea + calc.ManaMp5 + calc.ManaOther + calc.ManaPotion + 
                calc.ManaReplenishment + calc.ManaSpiritual + calc.ManaLayOnHands;

            float benediction = 1f - talents.Benediction * .02f;
            float ied = stats.ManaRestoreOnCast_5_15 * .035f;

            #region Maintaining JotP
            if (calcOpts.JotP && talents.JudgementsOfThePure > 0)
            {
                float oldhaste = fight_length * (1f + stats.SpellHaste);
                stats.SpellHaste = ((1 + stats.SpellHaste) * (1 + talents.JudgementsOfThePure * .03f)) - 1;

//              float seals_cast = (float)Math.Ceiling((fight_length - 60f) / 120f);
//              calc.JotPCasts += seals_cast;
//              calc.JotPUsage += ((float)Math.Round(base_mana * .14f) - ied) * seals_cast;

                float miss_chance = (float)Math.Max(0f, .09f - talents.EnlightenedJudgements * .02f - stats.PhysicalHit);
                float average_casts = 1 / (1f - miss_chance);
                float judgements_cast = (float)Math.Ceiling(fight_length / 60f);
                calc.JotPCasts += judgements_cast * average_casts;
                calc.JotPUsage += ((float)Math.Round(base_mana * .05f) - ied) * judgements_cast * average_casts;

                float newhaste = fight_length * (1f + stats.SpellHaste) - (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste)) * calc.JotPCasts;
                calc.JotPHaste = 1f - oldhaste / newhaste;
            }
            #endregion

            #region Maintaining BoL
            if (talents.BeaconOfLight > 0)
            {
                calc.BoLCasts = (float)Math.Ceiling(fight_length * calcOpts.BoLUp / (calcOpts.GlyphBeaconOfLight ? 90f : 60f));
                calc.BoLUsage = calc.BoLCasts * ((float)Math.Round(base_mana * .35f * benediction) - ied - stats.SpellsManaReduction);
            }
            #endregion

            #region Trinket Procs
            if (stats.Heal1Min > 0) { calc.HealedOther += stats.Heal1Min * (float)Math.Ceiling((fight_length - 15f) / 60f); }
            if (stats.BonusHoTOnDirectHeals > 0) { calc.HealedOther += stats.BonusHoTOnDirectHeals * 60f * (float)Math.Ceiling((fight_length - 15f) / 55f); }
            #endregion

            #region Flash of Light
            const float fol_coef = 1.5f / 3.5f * 66f / 35f * 1.25f;
            calc.FoLAvgHeal = (835.5f + (stats.SpellPower + stats.FlashOfLightSpellPower) * fol_coef) * (1f + talents.HealingLight * .04f) * (1f + stats.FlashOfLightMultiplier) * heal_multi;
            float fol_baseMana = (int)(base_mana * .07f);
            calc.FoLCrit = stats.SpellCrit + stats.FlashOfLightCrit + talents.HolyPower * .01f + (calcOpts.GlyphFlashOfLight ? .05f : 0f);
            calc.FoLCost = fol_baseMana * glyph_sow - fol_baseMana * .12f * talents.Illumination * calc.FoLCrit - ied - stats.SpellsManaReduction;
            float fol_dimana = fol_baseMana * glyph_sow * .5f - fol_baseMana * .12f * talents.Illumination * calc.FoLCrit - ied;
            float fol_heal = calc.FoLAvgHeal * ((1 - calc.FoLCrit) + 1.5f  * (1f + stats.BonusCritHealMultiplier) * calc.FoLCrit);
            calc.FoLCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
            calc.FoLHPS = fol_heal / calc.FoLCastTime;
            float fol_mps = calc.FoLCost / calc.FoLCastTime;
            float fol_dimps = fol_dimana / calc.FoLCastTime;
            calc.FoLHPM = fol_heal / calc.FoLCost;
            #endregion

            #region Holy Light
            const float hl_coef = 2.5f / 3.5f * 66f / 35f * 1.25f;
            calc.HLAvgHeal = (5166f + (stats.HolyLightSpellPower + stats.SpellPower) * hl_coef) * (1f + talents.HealingLight * .04f) * heal_multi;
            float hl_baseMana = (float)Math.Round(base_mana * .29f);
            calc.HLCrit = stats.SpellCrit + stats.HolyLightCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f;
            calc.HLCost = hl_baseMana * glyph_sow * (1f - stats.HolyLightPercentManaReduction)
                - hl_baseMana * .12f * talents.Illumination * calc.HLCrit - stats.HolyLightManaCostReduction - ied - stats.SpellsManaReduction;
            float hl_dimana = hl_baseMana * glyph_sow * (1f - stats.HolyLightPercentManaReduction) * 0.5f
                - hl_baseMana * .12f * talents.Illumination * calc.HLCrit - stats.HolyLightManaCostReduction - ied;
            float hl_heal = calc.HLAvgHeal * ((1 - calc.HLCrit) + 1.5f * (1f + stats.BonusCritHealMultiplier) * calc.HLCrit);
            calc.HLCastTime = (2.5f - .5f / 3 * talents.LightsGrace) / (1f + stats.SpellHaste);
            calc.HLHPS = hl_heal / calc.HLCastTime;
            float hl_mps = calc.HLCost / calc.HLCastTime;
            float hl_dimps = hl_dimana / calc.HLCastTime;
            calc.HLHPM = hl_heal / calc.HLCost;
            #endregion

            #region Holy Shock
            const float hs_coef = 1.5f / 3.5f * 66f / 35f;
            calc.HSAvgHeal = (2500f + stats.SpellPower * hs_coef) * (1f + talents.HealingLight * .04f) * heal_multi;
            float hs_baseMana = (float)Math.Round(base_mana * .18f * benediction);
            calc.HSCrit = stats.SpellCrit + talents.HolyPower * .01f + talents.SanctifiedLight * .02f + stats.HolyShockCrit;
            calc.HSCost = hs_baseMana * glyph_sow - hs_baseMana * .12f * talents.Illumination * calc.HSCrit - ied - stats.SpellsManaReduction;
            float hs_heal = calc.HSAvgHeal * ((1 - calc.HSCrit) + 1.5f * (1f + stats.BonusCritHealMultiplier) * calc.HSCrit);
            calc.HSCastTime = (float)Math.Max(1f, 1.5f / (1f + stats.SpellHaste));
            calc.HSHPS = hs_heal / calc.HSCastTime;
            float hs_mps = calc.HSCost / calc.HSCastTime;
            calc.HSHPM = hs_heal / calc.HSCost;
            #endregion

            #region Sacred Shield
            const float SSUptime = 1f;
            calc.SSAvgAbsorb = 500f + .75f * stats.SpellPower;
            calc.SSCasts = (float)Math.Ceiling(fight_length / 30f) * SSUptime;
            float ss_baseMana = (float)Math.Round(.12f * base_mana) * benediction - stats.SpellsManaReduction;
            calc.SSUsage = calc.SSCasts * ss_baseMana;
            calc.SSAbsorbed = (float)Math.Floor(fight_length / 6f) * calc.SSAvgAbsorb;
            calc.SSHPM = calc.SSAbsorbed / calc.SSUsage;
            calc.SSHPS = calc.SSAbsorbed / (calc.SSCasts * calc.HSCastTime);
            #endregion

            if (talents.HolyShock > 0 && calcOpts.HolyShock > 0)
            {
                calc.HSTime = (float)(Math.Floor(fight_length * calcOpts.HolyShock / (calcOpts.GlyphHolyShock ? 5f : 6f)) * calc.HSCastTime);
                calc.HSHealed = calc.HSTime * calc.HSHPS;
                calc.HSUsage = calc.HSTime * hs_mps;
            }
            float df_casts = 0, df_manaCost = 0, df_manaSaved = 0, df_healing = 0;
            if (talents.DivineFavor > 0)
            {
                df_casts = (float)Math.Ceiling((fight_length - .5f) / 120f);
                df_manaCost = (float)Math.Round(base_mana * .03f) * df_casts * benediction - stats.SpellsManaReduction;
                df_manaSaved = hl_baseMana * .6f * df_casts * (1f - calc.HLCrit);
                df_healing = calc.HLAvgHeal * (1f - calc.HLCrit) * (1.5f * (1f + stats.BonusCritHealMultiplier) - 1f);
            }
            float di_time = 0, di_manausage = 0, di_healing = 0;
            if (talents.DivineIllumination > 0)
            {
                di_time = (float)Math.Ceiling((fight_length - 1f) / 180f) * 15f * calcOpts.Activity;
                di_manausage = di_time * hl_dimps;
                di_healing = di_time * calc.HLHPS;
            }

            float healing_mana = calc.TotalMana - calc.BoLUsage - calc.JotPUsage - calc.HSUsage - calc.SSUsage - df_manaCost + df_manaSaved - di_manausage;
            float healing_time = active_length - (calc.HSCastTime * (calc.BoLCasts + calc.JotPCasts + calc.SSCasts)) - calc.HSTime - di_time;
            calc.HLTime = Math.Min(healing_time, Math.Max(0, (healing_mana - (healing_time * fol_mps)) / (hl_mps - fol_mps)));
            calc.FoLTime = healing_time - calc.HLTime;
            if (calc.HLTime == 0)
            {
                calc.FoLTime = Math.Min(healing_time, healing_mana / fol_mps);
            }

            calc.FoLUsage = calc.FoLTime * fol_mps;
            calc.HLUsage = calc.HLTime * hl_mps;

            calc.FoLHealed = calc.FoLTime * calc.FoLHPS;
            calc.HLHealed = calc.HLTime * calc.HLHPS + di_healing + df_healing;
            calc.HLTime += di_time;
            if (calcOpts.GlyphHolyLight) calc.HLGlyph = calc.HLHealed * calcOpts.GHL_Targets * 0.1f * heal_multi *
                ((1 - stats.SpellCrit - talents.HolyPower * .01f) + 1.5f * (1f + stats.BonusCritHealMultiplier) * (stats.SpellCrit + talents.HolyPower * .01f));

            calc.TotalHealed = calc.FoLHealed + calc.HLHealed + calc.HSHealed + calc.HLGlyph;
            if (talents.BeaconOfLight > 0) calc.TotalHealed += calc.BoLHealed = calcOpts.BoLEff * calcOpts.BoLUp * calc.TotalHealed;
            calc.TotalHealed += calc.SSAbsorbed + calc.HLGlyph + calc.HealedOther;

            calc.AvgHPM = calc.TotalHealed / calc.TotalMana;
            calc.AvgHPS = calc.TotalHealed / fight_length;

            calc.FightPoints = calc.AvgHPS * (1f - calcOpts.BurstScale);
            calc.BurstPoints = calc.HLHPS * calcOpts.BurstScale;
            calc.OverallPoints = calc.FightPoints + calc.BurstPoints; 

            return calc;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;

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
            Stats statsTotal = statsBaseGear + statsBuffs + statsRace;

            statsTotal.Stamina *= 1 + statsTotal.BonusStaminaMultiplier;
            statsTotal.Intellect *= (1 + statsTotal.BonusIntellectMultiplier) * (1 + talents.DivineIntellect * .03f);
            statsTotal.SpellPower += 0.04f * statsTotal.Intellect * talents.HolyGuidance;
            statsTotal.SpellCrit = .03336f + statsTotal.SpellCrit + statsTotal.Intellect / 16666.66709f
                + statsTotal.CritRating / 4590.598679f + talents.SanctityOfBattle * .01f + talents.Conviction * .01f;
            statsTotal.SpellHaste += statsTotal.HasteRating / 3278.998947f;
            statsTotal.Mana = (statsTotal.Mana + statsTotal.Intellect * 15) * (1f + statsBaseGear.BonusManaMultiplier);
            statsTotal.Health = statsTotal.Health + statsTotal.Stamina * 10f;
            statsTotal.PhysicalHit += statsTotal.HitRating / 3278.998947f;
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            if (chartName == "Mana Pool Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin Base = new ComparisonCalculationHealadin("Base");
                ComparisonCalculationHealadin Mp5 = new ComparisonCalculationHealadin("Mp5");
                ComparisonCalculationHealadin Potion = new ComparisonCalculationHealadin("Potion");
                ComparisonCalculationHealadin Replenishment = new ComparisonCalculationHealadin("Replenishment");
                ComparisonCalculationHealadin ArcaneTorrent = new ComparisonCalculationHealadin("Arcane Torrent");
                ComparisonCalculationHealadin DivinePlea = new ComparisonCalculationHealadin("Divine Plea");
                ComparisonCalculationHealadin Spiritual = new ComparisonCalculationHealadin("Spiritual Atunement");
                ComparisonCalculationHealadin LoH = new ComparisonCalculationHealadin("Lay on Hands");
                ComparisonCalculationHealadin Other = new ComparisonCalculationHealadin("Other");

                Base.OverallPoints = Base.ThroughputPoints = calc.ManaBase;
                Mp5.OverallPoints = Mp5.ThroughputPoints = calc.ManaMp5;
                LoH.OverallPoints = LoH.ThroughputPoints = calc.ManaLayOnHands;
                Potion.OverallPoints = Potion.ThroughputPoints = calc.ManaPotion;
                Replenishment.OverallPoints = Replenishment.ThroughputPoints = calc.ManaReplenishment;
                ArcaneTorrent.OverallPoints = ArcaneTorrent.ThroughputPoints = calc.ManaArcaneTorrent;
                DivinePlea.OverallPoints = DivinePlea.ThroughputPoints = calc.ManaDivinePlea;
                Spiritual.OverallPoints = Spiritual.ThroughputPoints = calc.ManaSpiritual;
                Other.OverallPoints = Other.ThroughputPoints = calc.ManaOther;

                return new ComparisonCalculationBase[] { Base, Mp5, Potion, Replenishment, LoH, ArcaneTorrent, DivinePlea, Spiritual, Other };
            }
            else if (chartName == "Mana Usage Breakdown")
            {
                CharacterCalculationsHealadin calc = GetCharacterCalculations(character) as CharacterCalculationsHealadin;
                if (calc == null) calc = new CharacterCalculationsHealadin();

                ComparisonCalculationHealadin FoL = new ComparisonCalculationHealadin("Flash of Light");
                ComparisonCalculationHealadin HL = new ComparisonCalculationHealadin("Holy Light");
                ComparisonCalculationHealadin HS = new ComparisonCalculationHealadin("Holy Shock");
                ComparisonCalculationHealadin JotP = new ComparisonCalculationHealadin("Judgements and Seals");
                ComparisonCalculationHealadin BoL = new ComparisonCalculationHealadin("Beacon of Light");
                ComparisonCalculationHealadin SS = new ComparisonCalculationHealadin("Sacred Shield");

                FoL.OverallPoints = FoL.ThroughputPoints = calc.FoLUsage;
                HL.OverallPoints = HL.ThroughputPoints = calc.HLUsage;
                HS.OverallPoints = HS.ThroughputPoints = calc.HSUsage;
                JotP.OverallPoints = JotP.ThroughputPoints = calc.JotPUsage;
                BoL.OverallPoints = BoL.ThroughputPoints = calc.BoLUsage;
                SS.OverallPoints = SS.ThroughputPoints = calc.SSUsage;

                return new ComparisonCalculationBase[] { FoL, HL, HS, JotP, BoL, SS };
            }
            else if (chartName == "Glyphs")
            {

                CalculationOptionsHealadin initOpts = character.CalculationOptions as CalculationOptionsHealadin;

                Character baseChar = character.Clone();
                CalculationOptionsHealadin baseOpts = initOpts.Clone();
                baseChar.CalculationOptions = baseOpts;

                Character deltaChar = character.Clone();
                CalculationOptionsHealadin deltaOpts = initOpts.Clone();
                deltaChar.CalculationOptions = deltaOpts;

                CharacterCalculationsBase baseCalc;

                ComparisonCalculationBase HolyLight;
                baseOpts.GlyphHolyLight = false;
                deltaOpts.GlyphHolyLight = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                HolyLight = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Holy Light", initOpts.GlyphHolyLight);
                deltaOpts.GlyphHolyLight = baseOpts.GlyphHolyLight = initOpts.GlyphHolyLight;
                HolyLight.Item = null;

                ComparisonCalculationBase FlashOfLight;
                baseOpts.GlyphFlashOfLight = false;
                deltaOpts.GlyphFlashOfLight = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                FlashOfLight = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Flash of Light", initOpts.GlyphFlashOfLight);
                deltaOpts.GlyphFlashOfLight = baseOpts.GlyphFlashOfLight = initOpts.GlyphFlashOfLight;
                FlashOfLight.Item = null;

                ComparisonCalculationBase Divinity;
                baseOpts.GlyphDivinity = false;
                deltaOpts.GlyphDivinity = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                Divinity = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Divnity", initOpts.GlyphDivinity);
                deltaOpts.GlyphDivinity = baseOpts.GlyphDivinity = initOpts.GlyphDivinity;
                Divinity.Item = null;

                ComparisonCalculationBase SealOfWisdom;
                baseOpts.GlyphSealOfWisdom = false;
                deltaOpts.GlyphSealOfWisdom = true;
                baseOpts.GlyphSealOfLight = deltaOpts.GlyphSealOfLight = false;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                SealOfWisdom = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Wisdom", initOpts.GlyphSealOfWisdom);
                deltaOpts.GlyphSealOfWisdom = baseOpts.GlyphSealOfWisdom = initOpts.GlyphSealOfWisdom;
                deltaOpts.GlyphSealOfLight = baseOpts.GlyphSealOfLight = initOpts.GlyphSealOfLight;
                SealOfWisdom.Item = null;

                ComparisonCalculationBase SealOfLight;
                baseOpts.GlyphSealOfLight = false;
                deltaOpts.GlyphSealOfLight = true;
                baseOpts.GlyphSealOfWisdom = deltaOpts.GlyphSealOfWisdom = false;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                SealOfLight = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Seal of Light", initOpts.GlyphSealOfLight);
                deltaOpts.GlyphSealOfLight = baseOpts.GlyphSealOfLight = initOpts.GlyphSealOfLight;
                deltaOpts.GlyphSealOfWisdom = baseOpts.GlyphSealOfWisdom = initOpts.GlyphSealOfWisdom;
                SealOfLight.Item = null;

                ComparisonCalculationBase HolyShock;
                baseOpts.GlyphHolyShock = false;
                deltaOpts.GlyphHolyShock = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                HolyShock = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Holy Shock", initOpts.GlyphHolyShock);
                deltaOpts.GlyphHolyShock = baseOpts.GlyphHolyShock = initOpts.GlyphHolyShock;
                HolyShock.Item = null;

                ComparisonCalculationBase BeaconOfLight;
                baseOpts.GlyphBeaconOfLight = false;
                deltaOpts.GlyphBeaconOfLight = true;
                baseCalc = Calculations.GetCharacterCalculations(baseChar);
                BeaconOfLight = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, "Beacon of Light", initOpts.GlyphBeaconOfLight);
                deltaOpts.GlyphBeaconOfLight = baseOpts.GlyphBeaconOfLight = initOpts.GlyphBeaconOfLight;
                BeaconOfLight.Item = null;

                return new ComparisonCalculationBase[] { HolyLight, FlashOfLight, Divinity, SealOfWisdom, 
                    SealOfLight, HolyShock, BeaconOfLight };
            }
            return new ComparisonCalculationBase[] {};
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
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
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                // Gear Procs
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                ManaRestoreOnCrit_25_45 = stats.ManaRestoreOnCrit_25_45,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,
                ManaRestore5min = stats.ManaRestore5min,
                GreatnessProc = stats.GreatnessProc,
                Heal1Min = stats.Heal1Min,
                BonusHoTOnDirectHeals = stats.BonusHoTOnDirectHeals,
                SpellsManaReduction = stats.SpellsManaReduction
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool wantedStats = (stats.Intellect + stats.Mp5 + stats.SpellPower + stats.CritRating + stats.SpellCrit + stats.SpellHaste
                + stats.HitRating + stats.PhysicalHit + stats.GreatnessProc + stats.Heal1Min + stats.BonusHoTOnDirectHeals + stats.Mana
                + stats.HasteRating + stats.BonusIntellectMultiplier + stats.HolyLightPercentManaReduction + stats.HolyShockCrit + stats.ManaRestoreOnCrit_25_45
                + stats.BonusManaPotion + stats.FlashOfLightMultiplier + stats.FlashOfLightSpellPower + stats.FlashOfLightCrit + stats.HolyLightManaCostReduction
                + stats.HolyLightCrit + stats.HolyLightSpellPower + stats.ManaRestoreOnCast_10_45 + stats.ManaRestoreFromMaxManaPerSecond + stats.BonusManaMultiplier
                + stats.HealingReceivedMultiplier + stats.ManaRestoreOnCast_5_15 + stats.BonusCritHealMultiplier + stats.ManaRestore5min
                + stats.SpellsManaReduction) > 0;
            bool survivalStats = (stats.Stamina + stats.Health) > 0;
            bool ignoreStats = (stats.Agility + stats.Strength + stats.AttackPower + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry
                + stats.HitRating + stats.ArmorPenetrationRating + stats.Spirit + stats.DodgeRating + stats.ParryRating
                + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue) > 0;
            return (wantedStats || (survivalStats && !ignoreStats));
        }
    }
}
