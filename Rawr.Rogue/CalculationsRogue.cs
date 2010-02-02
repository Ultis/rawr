using System;
using System.Collections.Generic;
#if RAWR3
using System.Windows.Media;
#endif
using System.Text;

namespace Rawr.Rogue {
    //[Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", CharacterClass.Rogue)]
    [Rawr.Calculations.RawrModelInfo("Rogue", "Ability_Rogue_SliceDice", CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase
    {
        #region Variables and Properties

        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
                ////Relevant Gem IDs for Rogues
                //Red
                int[] bold      = { 39900, 39996, 40111, 42142 };
                int[] delicate  = { 39905, 39997, 40112, 42143 };

                //Purple
                int[] shifting  = { 39935, 40023, 40130 };
                int[] sovereign = { 39934, 40022, 40129 };

                //Blue
                int[] solid     = { 39919, 40008, 40119, 36767 };

                //Green
                int[] enduring  = { 39976, 40089, 40167 };

                //Yellow
                int[] thick     = { 39916, 40015, 40126, 42157 };

                //Orange
                int[] etched    = { 39948, 40038, 40143 };
                int[] fierce    = { 39951, 40041, 40146 };
                int[] glinting  = { 39953, 40044, 40148 };
                int[] stalwart  = { 39964, 40056, 40160 };
                int[] deadly    = { 39952, 40043, 40147 };

                //Meta
                // int austere = 41380;
                int relentless = 41398;

                return new List<GemmingTemplate> {
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Max Agility
					    RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Agi/Crit
					    RedId = delicate[0], YellowId = deadly[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare",  //Max Agility
					    RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Rare",  //Agi/Crit 
					    RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", Enabled = true, //Max Agility
					    RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Epic", Enabled = true, //Agi/Crit 
					    RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Max Agility
					    RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
				    new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Agility Heavy
					    RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
			    };
            }
        }

        private CalculationOptionsPanelRogue _calculationOptionsPanel = null;
        #if RAWR3
        public override ICalculationOptionsPanel CalculationOptionsPanel
        #else
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        #endif
        {
            get {
                if (_calculationOptionsPanel == null) { _calculationOptionsPanel = new CalculationOptionsPanelRogue(); }
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
					"Summary:Overall Points*Sum of your DPS Points and Survivability Points",
					"Summary:DPS Points*DPS Points is your theoretical DPS.",
					"Summary:Survivability Points*One hundreth of your health.",

					"Basic Stats:Health",
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Strength",
					"Basic Stats:Crit Rating",
					"Basic Stats:Hit Rating",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Weapon Damage",
					
					"Complex Stats:Avoided Attacks",
					"Complex Stats:Crit Chance",
					"Complex Stats:MainHand Speed",
					"Complex Stats:OffHand Speed",
					"Complex Stats:Armor Mitigation",
					
					"Abilities:Optimal Rotation",
					"Abilities:Optimal Rotation DPS",
					"Abilities:Custom Rotation DPS",
					"Abilities:MainHand",
					"Abilities:OffHand",
					"Abilities:Backstab",
					"Abilities:Hemorrhage",
					"Abilities:Sinister Strike",
					"Abilities:Mutilate",
					"Abilities:Slice and Dice",
					"Abilities:Eviscerate",
					"Abilities:Envenom",
                    "Abilities:Instant Poison",
                    "Abilities:Deadly Poison",
                    "Abilities:Wound Poison",
                    "Abilities:Anesthetic Poison",
					//"Abilities:Melee Usage",
					//"Abilities:Melee Stats",
					//"Abilities:Mangle Usage",
					//"Abilities:Mangle Stats",
					//"Abilities:Shred Usage",
					//"Abilities:Shred Stats",
					//"Abilities:Rake Usage",
					//"Abilities:Rake Stats",
					//"Abilities:Rip Usage",
					//"Abilities:Rip Stats",
					//"Abilities:Bite Usage",
					//"Abilities:Bite Stats",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Armor",
                        "Strength",
                        "Attack Power",
                        "Agility",
                        "Crit %",
                        "Haste %",
					    "Armor Penetration %",
                        "% Chance to Miss (White)",
                        "% Chance to Miss (Yellow)",
                        "% Chance to be Dodged",
                        "% Chance to be Parried",
                        "% Chance to be Avoided (Yellow/Dodge)",
					};
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Combat Table",
                    };
                }
                return _customChartNames;
            }
        }

#if RAWR3
        private Dictionary<string, Color> _subPointNameColors = null;
		public override Dictionary<string, Color> SubPointNameColors
		{
			get
			{
				if (_subPointNameColors == null)
				{
					_subPointNameColors = new Dictionary<string, Color>();
					_subPointNameColors.Add("DPS", Color.FromArgb(255, 160, 0, 224));
					_subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
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
                    _subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
                }
                return _subPointNameColors;
            }
        }
#endif

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
                        ItemType.None,
                        ItemType.Leather,
                        ItemType.Bow,
                        ItemType.Crossbow,
                        ItemType.Gun,
                        ItemType.Thrown,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Rogue; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsRogue(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsRogue(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            CalculationOptionsRogue calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            if (calcOpts == null) calcOpts = new CalculationOptionsRogue();
            int targetLevel = calcOpts.TargetLevel;
            float targetArmor = calcOpts.TargetArmor;
            bool targetPoisonable = calcOpts.TargetPoisonable;
            bool maintainBleed = false;
            WeightedStat[] arPenUptimes, critRatingUptimes;
            Stats stats = GetCharacterStatsWithTemporaryEffects(character, additionalItem, out arPenUptimes, out critRatingUptimes);
            float levelDifference = (targetLevel - 80f) * 0.2f;
            CharacterCalculationsRogue calculatedStats = new CharacterCalculationsRogue();
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;
            stats.BonusBleedDamageMultiplier = 0.3f;
            Item mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
            Item offHand = character.OffHand == null ? new Knuckles() : character.OffHand.Item;

            #region Basic Chances and Constants
            float modArmor = 0f;
            for (int i = 0; i < arPenUptimes.Length; i++)
            {
                modArmor += arPenUptimes[i].Chance * StatConversion.GetArmorDamageReduction(character.Level, calcOpts.TargetArmor, stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating + arPenUptimes[i].Value);
            }
            float ArmorConstant = 400 + 85 * character.Level + 4.5f * 85 * (character.Level - 59);
            float tempTargetArmor = modArmor * ArmorConstant / (1 - modArmor);
            modArmor = 1f - ArmorConstant / (ArmorConstant + (tempTargetArmor - stats.ArmorReduction));
            modArmor = 1f - modArmor;
            float mainHandModArmor = mainHand.Type != ItemType.OneHandMace ? modArmor : Math.Min(1f, modArmor - stats.BonusMaceArP);
            float offHandModArmor = offHand.Type != ItemType.OneHandMace ? modArmor : Math.Min(1f, modArmor - stats.BonusMaceArP);
            float critMultiplier = 2f * (1f + stats.BonusCritMultiplier);
            float critMultiplierBleed = 2f * (1f + stats.BonusCritMultiplier);
            float critMultiplierPoison = 2f * (1f + stats.BonusCritMultiplier);
            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Rogue);
            hasteBonus = (1f + hasteBonus) * (1f + stats.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
            float speedModifier = 1f / (1f + hasteBonus) / (1f + stats.PhysicalHaste);
            float mainHandSpeed = mainHand == null ? 0f : mainHand._speed * speedModifier;
            float offHandSpeed = offHand == null ? 0f : offHand._speed * speedModifier;

            float normalizedMainHandSpeed = mainHand.Type == ItemType.Dagger ? 1.7f : 2.4f;
            float normalizedOffHandSpeed = mainHand.Type == ItemType.Dagger ? 1.7f : 2.4f;

            float hitBonus = stats.PhysicalHit + StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Rogue);
            float spellHitBonus = stats.SpellHit + StatConversion.GetHitFromRating(stats.HitRating, CharacterClass.Rogue);
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Rogue) + stats.Expertise, CharacterClass.Rogue);

            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceParry = 0f; //Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceWhiteMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP_DW[targetLevel - 80] - hitBonus);
            float chanceMiss = Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[targetLevel - 80] - hitBonus);
            float chancePoisonMiss = Math.Max(0f, StatConversion.GetSpellMiss(80 - targetLevel, false) - spellHitBonus);

            float glanceMultiplier = 0.7f;
            float chanceWhiteAvoided = chanceWhiteMiss + chanceDodge + chanceParry;
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;
            float chancePoisonAvoided = chancePoisonMiss;
            float chanceWhiteNonAvoided = 1f - chanceWhiteAvoided;
            float chanceNonAvoided = 1f - chanceAvoided;
            float chancePoisonNonAvoided = 1f - chancePoisonAvoided;

            ////Crit Chances
            float chanceCritYellow = 0f;
            float chanceHitYellow = 0f;
            float cpPerCPG = 0f;
            float chanceCritBackstab = 0f;
            float chanceHitBackstab = 0f;
            float chanceCritMuti = 0f;
            float chanceHitMuti = 0f;
            float chanceCritRupt = 0f;
            float chanceCritEvis = 0f;
            float chanceCritEnvenom = 0f;
            //float chanceCritBleed = 0f;
            float chanceGlance = 0f;
            float chanceCritWhiteMain = 0f;
            float chanceHitWhiteMain = 0f;
            float chanceCritWhiteOff = 0f;
            float chanceHitWhiteOff = 0f;
            float chanceCritPoison = 0f;
            float chanceHitPoison = 0f;

            for (int i = 0; i < critRatingUptimes.Length; i++)
            { //Sum up the weighted chances for each crit value
                WeightedStat iStat = critRatingUptimes[i];
                //Yellow - 2 Roll, so total of X chance to avoid, total of 1 chance to crit and hit when not avoided
                float chanceCritYellowTemp = Math.Min(1f, StatConversion.GetCritFromRating(stats.CritRating + iStat.Value, CharacterClass.Rogue)
                    + StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Rogue)
                    + stats.PhysicalCrit
                    + (mainHand.Type == ItemType.Dagger || mainHand.Type == ItemType.FistWeapon ? stats.BonusDaggerFistCrit : 0)
                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80])
                    + stats.BonusMainHandCrit;
                float chanceHitYellowTemp = 1f - chanceCritYellowTemp;
                float chanceCritPoisonTemp = Math.Min(1f, StatConversion.GetCritFromRating(stats.CritRating + iStat.Value, CharacterClass.Rogue)
                    + StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Rogue)
                    + stats.SpellCrit
                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80]);
                float chanceHitPoisonTemp = 1f - chanceCritPoisonTemp;
                float cpPerCPGTemp = (chanceHitYellowTemp + chanceCritYellowTemp * (1f + stats.BonusCPOnCrit)) / chanceNonAvoided;

                //Backstab - Identical to Yellow, with higher crit chance
                float chanceCritBackstabTemp = Math.Min(1f, chanceCritYellowTemp + stats.BonusBackstabCrit);
                float chanceHitBackstabTemp = 1f - chanceCritBackstabTemp;

                //Mutilate - Identical to Yellow, with higher crit chance
                float chanceCritMutiTemp = Math.Min(1f, chanceCritYellowTemp + stats.BonusMutiCrit);
                float chanceHitMutiTemp = 1f - chanceCritMutiTemp;

                //Bleeds - 1 Roll, no avoidance, total of 1 chance to crit and hit
                /*float chanceCritBleedTemp = character.DruidTalents.PrimalGore > 0 ? chanceCritYellowTemp : 0f;
                float chanceCritRipTemp = Math.Min(1f, chanceCritBleedTemp > 0f ? chanceCritBleedTemp + stats.BonusRipCrit : 0f);
                float chanceCritRakeTemp = stats.BonusRakeCrit > 0 ? chanceCritBleedTemp : 0;*/

                //White
                float chanceGlanceTemp = StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 80];
                //White Mainhand
                float chanceCritWhiteMainTemp = Math.Min(chanceCritYellowTemp + (mainHand.Type == ItemType.Dagger || mainHand.Type == ItemType.FistWeapon ? stats.BonusDaggerFistCrit : 0), 1f - chanceGlanceTemp - chanceWhiteAvoided + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80]);
                float chanceHitWhiteMainTemp = 1f - chanceCritWhiteMainTemp - chanceWhiteAvoided - chanceGlanceTemp;
                //White Offhand
                float chanceCritWhiteOffTemp = Math.Min(chanceCritYellowTemp + (offHand.Type == ItemType.Dagger || offHand.Type == ItemType.FistWeapon ? stats.BonusDaggerFistCrit : 0), 1f - chanceGlanceTemp - chanceWhiteAvoided + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80]);
                float chanceHitWhiteOffTemp = 1f - chanceCritWhiteOffTemp - chanceWhiteAvoided - chanceGlanceTemp;

                chanceCritYellow += iStat.Chance * chanceCritYellowTemp;
                chanceHitYellow += iStat.Chance * chanceHitYellowTemp;
                cpPerCPG += iStat.Chance * cpPerCPGTemp;
                chanceCritBackstab += iStat.Chance * chanceCritBackstabTemp;
                chanceHitBackstab += iStat.Chance * chanceHitBackstabTemp;
                chanceCritMuti += iStat.Chance * chanceCritMutiTemp;
                chanceHitMuti += iStat.Chance * chanceHitMutiTemp;
                //chanceCritBleed += iStat.Chance * chanceCritBleedTemp;
                chanceGlance += iStat.Chance * chanceGlanceTemp;
                chanceCritWhiteMain += iStat.Chance * chanceCritWhiteMainTemp;
                chanceHitWhiteMain += iStat.Chance * chanceHitWhiteMainTemp;
                chanceCritWhiteOff += iStat.Chance * chanceCritWhiteOffTemp;
                chanceHitWhiteOff += iStat.Chance * chanceHitWhiteOffTemp;
                chanceCritPoison += iStat.Chance * chanceCritPoisonTemp;
                chanceHitPoison += iStat.Chance * chanceHitPoisonTemp;
            }

            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
            calculatedStats.MissedAttacks = chanceMiss * 100f;

            float timeToReapplyDebuffs = 1f / (1f - chanceAvoided) - 1f;
            float lagVariance = (float)calcOpts.LagVariance / 1000f;
            //float mangleDurationUptime = (character.DruidTalents.GlyphOfMangle ? 18f : 12f);
            //float mangleDurationAverage = mangleDurationUptime - timeToReapplyDebuffs - lagVariance;
            //float rakeDurationUptime = 9f + stats.BonusRakeDuration;
            //float rakeDurationAverage = rakeDurationUptime + timeToReapplyDebuffs + lagVariance;
            float ruptDurationUptime = (character.RogueTalents.GlyphOfRupture ? 12f : 8f);
            float ruptDurationAverage = ruptDurationUptime + timeToReapplyDebuffs + lagVariance;
            float snDBonusDuration = 0f - lagVariance;
            //float berserkDuration = character.DruidTalents.Berserk > 0 ? (character.DruidTalents.GlyphOfBerserk ? 20f : 15f) : 0f;
            #endregion

            #region Attack Damages
            float baseDamage = (stats.AttackPower / 14f) + stats.WeaponDamage + (mainHand.MinDamage + mainHand.MaxDamage) / 2f;
            float baseOffDamage = ((stats.AttackPower / 14f) + stats.WeaponDamage + (offHand.MinDamage + offHand.MaxDamage) / 2f) + (1f + stats.BonusOffHandDamageMultiplier);
            float meleeDamageRaw = (baseDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
            float meleeOffDamageRaw = (baseOffDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
            float baseYellowDamage = baseDamage * (1f + stats.BonusYellowDamageMultiplier);
            float baseYellowOffDamage = baseOffDamage * (1f + stats.BonusYellowDamageMultiplier);
            float backstabDamageRaw = (baseYellowDamage * 1.5f + 465f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusBackstabDamageMultiplier) * modArmor;
            backstabDamageRaw *= (mainHand._type == ItemType.Dagger ? 1f : 0f);
            float hemoDamageRaw = (baseYellowDamage * 1.1f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusHemoDamageMultiplier) * (1f /*+ stats.BonusBleedDamageMultiplier*/) * modArmor;
            float sStrikeDamageRaw = (baseYellowDamage * 1f + 180f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f /*+ stats.BonusShredDamageMultiplier*/) * (1f + stats.BonusSStrikeDamageMultiplier) * modArmor;
            float mutiDamageRaw = (baseYellowDamage * 1f + 181f + baseYellowOffDamage * 1f + 181f * (1f + stats.BonusOffHandDamageMultiplier)) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMutiDamageMultiplier) * (1f + (targetPoisonable ? 0.2f : 0f)) * modArmor;
            mutiDamageRaw *= (mainHand._type == ItemType.Dagger && offHand._type == ItemType.Dagger ? 1f : 0f);
            float ruptDamageRaw = (1736f + stats.AttackPower * 0.3f /*+ (stats.BonusRuptDamagePerCPPerTick * 5f * 8f)*/) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRuptDamageMultiplier);
            float evisBaseDamageRaw = (127f + 381f) / 2f * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusEvisDamageMultiplier) * modArmor;
            float evisCPDamageRaw = ((370f + stats.AttackPower * 0.03f) + (370f + stats.AttackPower * 0.07f)) / 2f * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusEvisDamageMultiplier) * modArmor;
            float envenomBaseDamageRaw = 0f;
            float envenomCPDamageRaw = (215f + stats.AttackPower * 0.09f) * (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusEnvenomDamageMultiplier);
            float iPDamageRaw = ((300f + 400f) / 2f + stats.AttackPower * 0.09f) * (1f + stats.BonusPoisonDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier);
            float dPDamageRaw = ((80f + 296f) / 2f + stats.AttackPower * 0.108f) * (1f + stats.BonusPoisonDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier) * 5f; //*5 for the 5 stack for now
            float wPDamageRaw = ((78f + 231f) / 2f + stats.AttackPower * 0.036f) * (1f + stats.BonusPoisonDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier);
            float aPDamageRaw = ((218f + 280f) / 2f) + (1f + stats.BonusPoisonDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier);

            float meleeDamageAverage = chanceGlance * meleeDamageRaw * glanceMultiplier +
                                        chanceCritWhiteMain * meleeDamageRaw * critMultiplier +
                                        chanceHitWhiteMain * meleeDamageRaw;
            float meleeOffDamageAverage = chanceGlance * meleeOffDamageRaw * glanceMultiplier +
                                           chanceCritWhiteOff * meleeOffDamageRaw * critMultiplier +
                                           chanceHitWhiteOff * meleeOffDamageRaw;
            //??? check numbers
            float backstabDamageAverage = (1f - chanceCritBackstab) * backstabDamageRaw + chanceCritBackstab * backstabDamageRaw * (critMultiplier + stats.BonusCPGCritDamageMultiplier);
            float hemoDamageAverage = (1f - chanceCritYellow) * hemoDamageRaw + chanceCritYellow * hemoDamageRaw * (critMultiplier + stats.BonusCPGCritDamageMultiplier);
            float sStrikeDamageAverage = (1f - chanceCritYellow) * sStrikeDamageRaw + chanceCritYellow * sStrikeDamageRaw * (critMultiplier + stats.BonusCPGCritDamageMultiplier);
            float mutiDamageAverage = (1f - chanceCritMuti) * mutiDamageRaw + chanceCritMuti * mutiDamageRaw * (critMultiplier + stats.BonusCPGCritDamageMultiplier);
            float ruptDamageAverage = ((1f - chanceCritRupt) * ruptDamageRaw + chanceCritRupt * ruptDamageRaw * critMultiplierBleed);
            float evisBaseDamageAverage = (1f - chanceCritEvis) * evisBaseDamageRaw + chanceCritEvis * evisBaseDamageRaw * critMultiplier;
            float evisCPDamageAverage = (1f - chanceCritEvis) * evisCPDamageRaw + chanceCritEvis * evisCPDamageRaw * critMultiplier;
            float envenomBaseDamageAverage = (1f - chanceCritEnvenom) * envenomBaseDamageRaw + chanceCritEnvenom * envenomBaseDamageRaw * critMultiplier;
            float envenomCPDamageAverage = (1f - chanceCritEnvenom) * envenomCPDamageRaw + chanceCritEnvenom * envenomCPDamageRaw * critMultiplier;
            float iPDamageAverage = (1f - chanceCritPoison) * iPDamageRaw + chanceCritPoison * iPDamageRaw * critMultiplierPoison;
            float dPDamageAverage = dPDamageRaw;
            float wPDamageAverage = (1f - chanceCritPoison) * wPDamageRaw + chanceCritPoison * wPDamageRaw * critMultiplierPoison;
            float aPDamageAverage = (1f - chanceCritPoison) * aPDamageRaw + chanceCritPoison * aPDamageRaw * critMultiplierPoison;

            //if (needsDisplayCalculations)
            //{
            //    Console.WriteLine("White:    {0:P} Avoided, {1:P} Glance,      {2:P} Hit, {3:P} Crit - Swing: {4}", chanceAvoided, chanceGlance, chanceHitWhite, chanceCritWhite, meleeDamageAverage);
            //    Console.WriteLine("Yellow:   {0:P} Avoided, {1:P} NonAvoided,  {2:P} Hit, {3:P} Crit - Swing: {4}", chanceAvoided, chanceNonAvoided, 1f - chanceCritYellow, chanceCritYellow, mangleDamageAverage);
            //    Console.WriteLine("Bite:     {0:P} Avoided, {1:P} NonAvoided,  {2:P} Hit, {3:P} Crit - Swing: {4}", chanceAvoided, chanceNonAvoided, 1f - chanceCritBite, chanceCritBite, biteBaseDamageAverage);
            //    Console.WriteLine("RipBleed:                                      {0:P} Hit, {1:P} Crit - Swing: {2}", 1f - chanceCritRip, chanceCritRip, ripDamageAverage);
            //    Console.WriteLine();
            //}
            #endregion

            #region Energy Costs
            float ambushEnergyRaw = 60f - stats.AmbushBackstabCostReduction;
            float garrEnergyRaw = 50f - stats.GarrCostReduction;
            float backstabEnergyRaw = 60f - stats.AmbushBackstabCostReduction;
            float hemoEnergyRaw = 35f - stats.HemoCostReduction;
            float sStrikeEnergyRaw = 45f - stats.SStrikeCostReduction;
            float mutiEnergyRaw = 60f;
            float ruptEnergyRaw = 25f;
            float evisEnergyRaw = 35f;
            float envenomEnergyRaw = 35f;
            float snDEnergyRaw = 25f;

            //[rawCost + ((1/chance_to_land) - 1) * rawCost/5] 
            float cpgEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * 0.2f;
            float finisherEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * (1f - stats.FinisherEnergyOnAvoid);
            float backstabEnergyAverage = backstabEnergyRaw * cpgEnergyCostMultiplier;
            float hemoEnergyAverage = hemoEnergyRaw * cpgEnergyCostMultiplier;
            float sStrikeEnergyAverage = sStrikeEnergyRaw * cpgEnergyCostMultiplier;
            float mutiEnergyAverage = mutiEnergyRaw * cpgEnergyCostMultiplier;
            float ruptEnergyAverage = ruptEnergyRaw * finisherEnergyCostMultiplier;
            float evisEnergyAverage = evisEnergyRaw * finisherEnergyCostMultiplier;
            float envenomEnergyAverage = envenomEnergyRaw * finisherEnergyCostMultiplier;
            float snDEnergyAverage = snDEnergyRaw;
            #endregion

            #region Ability Stats
            RogueAbilityStats mainHandStats = new RogueMHStats()
            {
                DamagePerHit = meleeDamageRaw,
                DamagePerSwing = meleeDamageAverage,
            };
            RogueAbilityStats offHandStats = new RogueOHStats()
            {
                DamagePerHit = meleeOffDamageRaw,
                DamagePerSwing = meleeOffDamageAverage,
            };
            RogueAbilityStats backstabStats = new RogueBackstabStats()
            {
                DamagePerHit = backstabDamageRaw,
                DamagePerSwing = backstabDamageAverage,
                EnergyCost = backstabEnergyAverage,
            };
            RogueAbilityStats hemoStats = new RogueHemoStats()
            {
                DamagePerHit = hemoDamageRaw,
                DamagePerSwing = hemoDamageAverage,
                EnergyCost = hemoEnergyAverage,
            };
            RogueAbilityStats sStrikeStats = new RogueSStrikeStats()
            {
                DamagePerHit = sStrikeDamageRaw,
                DamagePerSwing = sStrikeDamageAverage,
                EnergyCost = sStrikeEnergyAverage,
            };
            RogueAbilityStats mutiStats = new RogueMutiStats()
            {
                DamagePerHit = mutiDamageRaw,
                DamagePerSwing = mutiDamageAverage,
                EnergyCost = mutiEnergyAverage,
            };
            RogueAbilityStats ruptStats = new RogueRuptStats()
            {
                DamagePerHit = ruptDamageRaw,
                DamagePerSwing = ruptDamageAverage,
                DurationUptime = ruptDurationUptime,
                DurationAverage = ruptDurationAverage,
                DurationPerCP = 2f,
                EnergyCost = ruptEnergyAverage,
            };
            RogueAbilityStats evisStats = new RogueEvisStats()
            {
                DamagePerHit = evisBaseDamageRaw,
                DamagePerSwing = evisBaseDamageAverage,
                DamagePerHitPerCP = evisCPDamageRaw,
                DamagePerSwingPerCP = evisCPDamageAverage,
                EnergyCost = evisEnergyAverage,
            };
            RogueAbilityStats envenomStats = new RogueEnvenomStats()
            {
                DamagePerHit = envenomBaseDamageRaw,
                DamagePerSwing = envenomBaseDamageAverage,
                DamagePerHitPerCP = envenomCPDamageRaw,
                DamagePerSwingPerCP = envenomCPDamageAverage,
                EnergyCost = envenomEnergyAverage,
            };
            RogueAbilityStats snDStats = new RogueSnDStats()
            {
                DurationUptime = snDBonusDuration * (1f + stats.BonusSnDDurationMultiplier),
                DurationAverage = (6f + snDBonusDuration) * (1f + stats.BonusSnDDurationMultiplier),
                EnergyCost = snDEnergyAverage,
                DurationPerCP = 3f,
            };
            RogueAbilityStats iPStats = new RogueIPStats()
            {
                DamagePerHit = iPDamageRaw,
                DamagePerSwing = iPDamageAverage,
            };
            RogueAbilityStats dPStats = new RogueDPStats()
            {
                DamagePerHit = dPDamageRaw,
                DamagePerSwing = dPDamageAverage,
            };
            RogueAbilityStats wPStats = new RogueWPStats()
            {
                DamagePerHit = wPDamageRaw,
                DamagePerSwing = wPDamageAverage,
            };
            RogueAbilityStats aPStats = new RogueAPStats()
            {
                DamagePerHit = aPDamageRaw,
                DamagePerSwing = aPDamageAverage,
            };
            #endregion

            #region Rotations
            RogueRotationCalculator rotationCalculator = new RogueRotationCalculator(stats, calcOpts.Duration, cpPerCPG,
                maintainBleed, /*berserkDuration,*/ mainHandSpeed, offHandSpeed, 
                chanceWhiteAvoided, chanceAvoided, chancePoisonAvoided, chanceCritYellow * stats.BonusCPOnCrit, chanceCritMuti * stats.BonusCPOnCrit,
                mainHandStats, offHandStats, backstabStats, hemoStats, sStrikeStats, mutiStats,
                ruptStats, evisStats, envenomStats, snDStats, iPStats, dPStats, wPStats, aPStats);
            RogueRotationCalculator.RogueRotationCalculation rotationCalculationDPS = new RogueRotationCalculator.RogueRotationCalculation();

            bool usePoisons = targetPoisonable && (mainHand != null || offHand != null);
            bool haSMH = mainHand._type == ItemType.OneHandSword || mainHand._type == ItemType.OneHandAxe;
            bool haSOH = offHand._type == ItemType.OneHandSword || offHand._type == ItemType.OneHandAxe;

            for (int snDCP = 1; snDCP < 6; snDCP++)
                for (int finisher = 0; finisher < 3; finisher++)
                    for (int finisherCP = 1; finisherCP < 6; finisherCP++)
                        for (int CPG = (character.RogueTalents.Mutilate > 0 ? 0 : 1); CPG < (character.RogueTalents.Hemorrhage > 0 ? 4 : 3); CPG++)
                            for (int mHPoison = 0 ; usePoisons ? mHPoison < 5 : mHPoison < 1 ; mHPoison++)
                                for (int oHPoison = 0; usePoisons ? oHPoison < 5 : oHPoison < 1; oHPoison++)
                                    for (int useRupt = 0; useRupt < 2; useRupt++)
                                        {
                                            RogueRotationCalculator.RogueRotationCalculation rotationCalculation =
                                                rotationCalculator.GetRotationCalculations(
                                                CPG, useRupt == 1, finisher, finisherCP, snDCP, mHPoison, oHPoison, haSMH, haSOH);
                                            if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
                                                rotationCalculationDPS = rotationCalculation;
                                        }

            calculatedStats.HighestDPSRotation = rotationCalculationDPS;
            calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(
                calcOpts.CustomCPG, calcOpts.CustomUseRupt, calcOpts.CustomFinisher, calcOpts.CustomCPFinisher, calcOpts.CustomCPSnD, calcOpts.CustomMHPoison, calcOpts.CustomOHPoison, haSMH, haSOH);

            /*if (character.DruidTalents.GlyphOfShred && rotationCalculationDPS.ShredCount > 0)
            {
                ripStats.DurationUptime += 6f;
                ripStats.DurationAverage += 6f;
            }*/
            ruptStats.DamagePerHit *= (ruptStats.DurationUptime + 5 * ruptStats.DurationPerCP) / 12f;
            ruptStats.DamagePerSwing *= (ruptStats.DurationUptime + 5 * ruptStats.DurationPerCP) / 12f;
            #endregion

            calculatedStats.AvoidedWhiteAttacks = chanceWhiteAvoided * 100f;
            calculatedStats.AvoidedAttacks = chanceAvoided * 100f;
            calculatedStats.AvoidedPoisonAttacks = chancePoisonAvoided * 100f;
            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.ParriedAttacks = chanceParry * 100f;
            calculatedStats.MissedAttacks = chanceMiss * 100f;
            calculatedStats.CritChance = chanceCritYellow * 100f;
            calculatedStats.MainHandSpeed = mainHandSpeed;
            calculatedStats.OffHandSpeed = offHandSpeed;
            calculatedStats.ArmorMitigation = (1f - modArmor) * 100f;
            calculatedStats.Duration = calcOpts.Duration;

            calculatedStats.MainHandStats = mainHandStats;
            calculatedStats.OffHandStats = offHandStats;
            calculatedStats.BackstabStats = backstabStats;
            calculatedStats.HemoStats = hemoStats;
            calculatedStats.SStrikeStats = sStrikeStats;
            calculatedStats.MutiStats = mutiStats;
            calculatedStats.RuptStats = ruptStats;
            calculatedStats.SnDStats = snDStats;
            calculatedStats.EvisStats = evisStats;
            calculatedStats.EnvenomStats = envenomStats;
            calculatedStats.IPStats = iPStats;
            calculatedStats.DPStats = dPStats;
            calculatedStats.WPStats = wPStats;
            calculatedStats.APStats = aPStats;

            float magicDPS = (stats.ShadowDamage + stats.ArcaneDamage) * (1f + chanceCritYellow);
            calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS + magicDPS;
            calculatedStats.SurvivabilityPoints = stats.Health / 100f;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
            return calculatedStats;
        }

        private Stats GetCharacterStatsWithTemporaryEffects(Character character, Item additionalItem, out WeightedStat[] armorPenetrationUptimes, out WeightedStat[] critRatingUptimes)
        {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            int targetLevel = calcOpts.TargetLevel;
            bool targetPoisonable = calcOpts.TargetPoisonable;

            Stats statsRace = BaseStats.GetBaseStats(80, character.Class, character.Race);
            statsRace.PhysicalHaste = 0.4f; //Slice and Dice

            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            RogueTalents talents = character.RogueTalents;

            Stats statsTalents = new Stats()
            {
                AmbushBackstabCostReduction = 3 * talents.SlaughterFromTheShadows,
                ArmorReduction = talents.SerratedBlades > 2 ? 8f * character.Level : talents.SerratedBlades > 1 ? 5.34f * character.Level : talents.SerratedBlades > 0 ? 2.67f * character.Level : 0,
                BonusAgilityMultiplier = 0.03f * talents.SinisterCalling,
                //BonusAmbushCrit = 0.25f * talents.ImprovedAmbush,
                //BonusAmbushDamageMultiplier = 0.1f * talents.Opportunity,
                BonusAttackPowerMultiplier = (1f + 0.02f * talents.SavageCombat) * (1f + 0.02f * talents.Deadliness) - 1f,
                BonusBackstabCrit = 0.1f * talents.PuncturingWounds,
                BonusBackstabDamageMultiplier = (1f + 0.03f * talents.Aggression) * (1f + 0.05f * talents.BladeTwisting) * (1f + 0.1f * talents.SurpriseAttacks) * (1f + 0.1f * talents.Opportunity) * (1f + 0.02f * talents.SinisterCalling) - 1f,
                BonusCPGCritDamageMultiplier = 0.06f * talents.Lethality,
                BonusCPOnCrit = 0.2f * talents.SealFate,
                //BonusCPOnGroupCrit = talents.HonorAmongThieves > 2 ? 1 : 0.33f * talents.HonorAmongThieves,
                BonusCritChance = (1f + 0.01f * talents.Malice) * (1f + 0.04f * talents.PreyOnTheWeak) * (1f + (targetPoisonable ? 0.03f * talents.MasterPoisoner : 0)) - 1f,
                BonusMainHandCrit = (character.MainHand != null) ? ((character.MainHand.Type == ItemType.Dagger || character.MainHand.Type == ItemType.FistWeapon) ? 0.02f * talents.CloseQuartersCombat : 0f) : 0f,
                BonusDamageMultiplier = 0.02f * talents.Murder,
                //BonusDPApplyChance = 0.04f * talents.ImprovedPoisons,
                BonusEnergyRegenMultiplier = 0.08f * talents.Vitality,
                BonusEnvenomDamageMultiplier = 0.07f * talents.VilePoisons,
                BonusEvisDamageMultiplier = (1f + 0.07f * talents.ImprovedEviscerate) + (1f + 0.03f * talents.Aggression) - 1f,
                //BonusGarrDamageMultiplier = (1f + 0.15f * talents.BloodSpatter) + (1f + 0.1f * talents.Opportunity) -1f,
                //BonusGougeDamageMultiplier = 0.1f * talents.SurpriseAttacks,
                BonusHemoDamageMultiplier = (1f + 0.1f * talents.SurpriseAttacks) + (1f + 0.02f * talents.SinisterCalling) - 1f,
                BonusIPFrequencyMultiplier = 0.1f * talents.ImprovedPoisons,
                BonusMaceArP = 0.03f * talents.MaceSpecialization,
                BonusMaxEnergy = 10 * talents.Vigor,
                BonusMutiCrit = 0.05f * talents.PuncturingWounds,
                BonusMutiDamageMultiplier = 0.1f * talents.Opportunity,
                BonusOffHandCrit = (character.OffHand != null) ? ((character.OffHand.Type == ItemType.Dagger || character.OffHand.Type == ItemType.FistWeapon) ? 0.02f * talents.CloseQuartersCombat : 0f) : 0f,
                BonusOffHandDamageMultiplier = 0.1f * talents.DualWieldSpecialization,
                //BonusParryMultiplier = 0.02f * talents.Deflection,
                BonusPoisonDamageMultiplier = 0.07f * talents.VilePoisons,
                BonusRuptDamageMultiplier = (1f + 0.15f * talents.BloodSpatter) * (1f + 0.1f * talents.SerratedBlades) - 1f,
                //BonusShivDamageMultiplier = 0.1f * talents.SurpriseAttacks,
                BonusSnDDurationMultiplier = 0.25f * talents.ImprovedSliceAndDice,
                BonusSStrikeDamageMultiplier = (1f + 0.03f * talents.Aggression) * (1f + 0.05f * talents.BladeTwisting) * (1f + 0.1f * talents.SurpriseAttacks) - 1f,
                BonusStaminaMultiplier = 0.02f * talents.Endurance,
                //BonusStealthDamageMultiplier = 0.04f * talents.MasterOfSubtlety,
                //BonusStealthEnergyRegen = 0.3f * talents.Overkill,
                //BonusYellowDamageBelow35 = 0.1f * talents.DirtyDeeds,
                BonusYellowDamageMultiplier = 0.02f * talents.FindWeakness,
                //ChanceFinisherDodgedMultiplier = talents.SurpriseAttacks >0 ? 0 : 1,
                //ChanceOnCPOnAmbushGarrCS = talents.Initiative > 2 ? 1 : 0.33f * talents.Initiative,
                //ChanceOnEnergyOnCrit = talents.FocusedAttacks > 2 ? 1f : (0.33f * talents.FocusedAttacks),
                //ChanceOnEnergyOnOHAttack = 3 * 0.2f * talents.CombatPotency,
                //ChanceOnEnergyPerCPFinisher = 25 * 0.04 * talents.RelentlessStrikes,
                ChanceOnMHAttackOnSwordAxeHit = 0.01f * talents.HackAndSlash,
                //ChanceOnNoDPConsumeOnEvenom = talents.MasterPoisoner == 3 ? 1f : (0.33f * talents.MasterPoisoner),
                //CDOnExtraVanish = 8 * talents.Preparation,
                //CPGCritIncreaseOnRaidAvoid = 0.02f * talents.TurnTheTables,
                CPOnFinisher = 0.2f * talents.Ruthlessness,
                Dodge = 0.02f * talents.LightningReflexes,
                Expertise = 5 * talents.WeaponExpertise,
                //ExposeCostReduction = 5 * talents.ImprovedExposeArmor,
                FinisherEnergyOnAvoid = 0.4f * talents.QuickRecovery,
                GarrCostReduction = 10 * talents.DirtyDeeds,
                HemoCostReduction = 1 * talents.SlaughterFromTheShadows,
                //PercentSnDMaxDurationReset = 0.2f * talents.CutToTheChase,
                PhysicalHit = 0.01f * talents.Precision,
                PhysicalHaste = 0.04f * talents.LightningReflexes,
                //PrepCDReduction = 90 * talents.FilthyTricks,
                SpellHit = 0.01f * talents.Precision,
                SStrikeCostReduction = 3 * talents.ImprovedSinisterStrike,
                //VanishCDReduction = 30 * talents.Elusiveness,
            };

            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            statsGearEnchantsBuffs.Agility += statsGearEnchantsBuffs.AverageAgility;

            Stats statsTotal = statsRace + statsItems;
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsTalents);

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.AttackPower += statsTotal.Strength + statsTotal.Agility;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Armor += 2f * statsTotal.Agility;
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;

            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            hasteBonus = (1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) * (1f + statsTotal.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
            float meleeHitInterval = 1f / ((character.MainHand == null ? 2 : character.MainHand.Speed + hasteBonus) + (character.OffHand == null ? 2 : character.OffHand.Speed + hasteBonus));
            float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating, CharacterClass.Druid) + statsTotal.Expertise, CharacterClass.Druid);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceParry = 0f; //Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - 80] - expertiseBonus);
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[targetLevel - 80] - hitBonus);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating)
                                + StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Rogue)
                                + statsTotal.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 80];
            float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
            float chanceHit = 1f - chanceAvoided;

            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
            triggerIntervals[Trigger.Use] = 0f;
            triggerIntervals[Trigger.MeleeHit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalHit] = meleeHitInterval;
            triggerIntervals[Trigger.MeleeCrit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalCrit] = meleeHitInterval;
            triggerIntervals[Trigger.DoTTick] = 0f;
            triggerIntervals[Trigger.DamageDone] = meleeHitInterval / 2f;
            triggerChances[Trigger.Use] = 1f;
            triggerChances[Trigger.MeleeHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.PhysicalHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.MeleeCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.PhysicalCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.DoTTick] = 1f;
            triggerChances[Trigger.DamageDone] = 1f - chanceAvoided / 2f;

            // Handle Trinket procs
            Stats statsProcs = new Stats();
            foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger)))
            {
                // JOTHAY's NOTE: The following is an ugly hack to add Recursive Effects to Cat
                // so Victor's Call and similar trinkets can be given more appropriate value
                if (effect.Trigger == Trigger.Use && effect.Stats._rawSpecialEffectDataSize == 1
                    && triggerIntervals.ContainsKey(effect.Stats._rawSpecialEffectData[0].Trigger))
                {
                    float upTime = effect.GetAverageUptime(triggerIntervals[effect.Trigger],
                        triggerChances[effect.Trigger], 1f, calcOpts.Duration);
                    statsProcs.Accumulate(effect.Stats._rawSpecialEffectData[0].GetAverageStats(
                        triggerIntervals[effect.Stats._rawSpecialEffectData[0].Trigger],
                        triggerChances[effect.Stats._rawSpecialEffectData[0].Trigger], 1f, calcOpts.Duration),
                        upTime);
                }
                else
                {
                    statsProcs.Accumulate(effect.GetAverageStats(triggerIntervals[effect.Trigger],
                        triggerChances[effect.Trigger], 1f, calcOpts.Duration),
                        effect.Stats.DeathbringerProc > 0 ? 1f / 3f : 1f);
                }
            }

            statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon + statsProcs.DeathbringerProc;
            statsProcs.Strength += statsProcs.DeathbringerProc;
            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.HasteRating += statsProcs.DeathbringerProc;
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
            statsProcs.Armor += 2f * statsProcs.Agility;
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));

            //Agility is only used for crit from here on out; we'll be converting Agility to CritRating, 
            //and calculating CritRating separately, so don't add any Agility or CritRating from procs here.
            //Also calculating ArPen separately, so don't add that either.
            statsProcs.CritRating = statsProcs.Agility = statsProcs.ArmorPenetrationRating = 0;
            statsTotal.Accumulate(statsProcs);

            //Handle Crit procs
            critRatingUptimes = new WeightedStat[0];
            List<SpecialEffect> tempCritEffects = new List<SpecialEffect>();
            List<float> tempCritEffectIntervals = new List<float>();
            List<float> tempCritEffectChances = new List<float>();
            List<float> tempCritEffectScales = new List<float>();

            foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger) && (se.Stats.CritRating + se.Stats.Agility + se.Stats.DeathbringerProc + se.Stats.HighestStat + se.Stats.Paragon) > 0))
            {
                tempCritEffects.Add(effect);
                tempCritEffectIntervals.Add(triggerIntervals[effect.Trigger]);
                tempCritEffectChances.Add(triggerChances[effect.Trigger]);
                tempCritEffectScales.Add(effect.Stats.DeathbringerProc > 0 ? 1f / 3f : 1f);
            }

            if (tempCritEffects.Count == 0)
            {
                critRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = 1f, Value = 0f } };
            }
            else if (tempCritEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempCritEffects[0];
                float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, calcOpts.Duration) * tempCritEffectScales[0];
                float totalAgi = (effect.Stats.Agility + effect.Stats.DeathbringerProc + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                critRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = uptime, Value = 
						effect.Stats.CritRating + StatConversion.GetCritFromAgility(totalAgi,
						CharacterClass.Rogue) * StatConversion.RATING_PER_PHYSICALCRIT },
					new WeightedStat() { Chance = 1f - uptime, Value = 0f }};
            }
            else if (tempCritEffects.Count > 1)
            {
                List<float> tempCritEffectsValues = new List<float>();
                foreach (SpecialEffect effect in tempCritEffects)
                {
                    float totalAgi = (float)effect.MaxStack * (effect.Stats.Agility + effect.Stats.DeathbringerProc + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                    tempCritEffectsValues.Add(effect.Stats.CritRating +
                        StatConversion.GetCritFromAgility(totalAgi, CharacterClass.Rogue) *
                        StatConversion.RATING_PER_PHYSICALCRIT);
                }

                float[] intervals = new float[tempCritEffects.Count];
                float[] chances = new float[tempCritEffects.Count];
                float[] offset = new float[tempCritEffects.Count];
                for (int i = 0; i < tempCritEffects.Count; i++)
                {
                    intervals[i] = triggerIntervals[tempCritEffects[i].Trigger];
                    chances[i] = triggerChances[tempCritEffects[i].Trigger];
                }
                if (tempCritEffects.Count >= 2)
                {
                    offset[0] = calcOpts.TrinketOffset;
                }
                WeightedStat[] critWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempCritEffects.ToArray(), intervals, chances, offset, tempCritEffectScales.ToArray(), 1f, calcOpts.Duration, tempCritEffectsValues.ToArray());
                critRatingUptimes = critWeights;
            }

            //Handle ArPen procs
            armorPenetrationUptimes = new WeightedStat[0];
            List<SpecialEffect> tempArPenEffects = new List<SpecialEffect>();
            List<float> tempArPenEffectIntervals = new List<float>();
            List<float> tempArPenEffectChances = new List<float>();
            List<float> tempArPenEffectScales = new List<float>();

            foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger) && se.Stats.ArmorPenetrationRating > 0))
            {
                tempArPenEffects.Add(effect);
                tempArPenEffectIntervals.Add(triggerIntervals[effect.Trigger]);
                tempArPenEffectChances.Add(triggerChances[effect.Trigger]);
                tempArPenEffectScales.Add(effect.Stats.DeathbringerProc > 0 ? 1f / 3f : 1f);
            }

            if (tempArPenEffects.Count == 0)
            {
                armorPenetrationUptimes = new WeightedStat[] { new WeightedStat() { Chance = 1f, Value = 0f } };
            }
            else if (tempArPenEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempArPenEffects[0];
                float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, calcOpts.Duration) * tempArPenEffectScales[0];
                armorPenetrationUptimes = new WeightedStat[] { new WeightedStat() { Chance = uptime, Value = effect.Stats.ArmorPenetrationRating + effect.Stats.DeathbringerProc },
					new WeightedStat() { Chance = 1f - uptime, Value = 0f }};
            }
            else if (tempArPenEffects.Count > 1)
            {
                List<float> tempArPenEffectValues = new List<float>();
                foreach (SpecialEffect effect in tempArPenEffects)
                {
                    tempArPenEffectValues.Add(effect.Stats.ArmorPenetrationRating);
                }

                float[] intervals = new float[tempArPenEffects.Count];
                float[] chances = new float[tempArPenEffects.Count];
                float[] offset = new float[tempArPenEffects.Count];
                for (int i = 0; i < tempArPenEffects.Count; i++)
                {
                    intervals[i] = triggerIntervals[tempArPenEffects[i].Trigger];
                    chances[i] = triggerChances[tempArPenEffects[i].Trigger];
                }
                if (tempArPenEffects.Count >= 2)
                {
                    offset[0] = calcOpts.TrinketOffset;
                }
                WeightedStat[] arPenWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempArPenEffects.ToArray(), intervals, chances, offset, tempArPenEffectScales.ToArray(), 1f, calcOpts.Duration, tempArPenEffectValues.ToArray());
                armorPenetrationUptimes = arPenWeights;
            }

            return statsTotal;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            RogueTalents talents = character.RogueTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Rogue, character.Race);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsOptionsPanel = new Stats() {
                // handle boss level difference
                PhysicalCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
                SpellCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
            };
			Stats statsTalents = new Stats()
			{
				BonusAgilityMultiplier = talents.SinisterCalling * 0.03f,
				PhysicalHit = talents.Precision * 0.01f,
                SpellHit = talents.Precision * 0.01f,
				BonusAttackPowerMultiplier = talents.Deadliness * 0.02f
										   + talents.SavageCombat * 0.02f,
				/*  PhysicalCrit = talents.Malice * 0.01f +
								((character.ActiveBuffs.FindAll(buff => buff.Group == "Critical Strike Chance Taken").Count == 0)
									? talents.MasterPoisoner * 0.01f : 0f), */
                PhysicalCrit = talents.Malice * 0.01f,
                SpellCrit = talents.Malice * 0.01f,
				Dodge = talents.LightningReflexes * 0.02f,
				Parry = talents.Deflection * 0.02f,
                PhysicalHaste = (float)Math.Round((double)(0.033*talents.LightningReflexes),2),
                /*
				PhysicalHaste = (1f + talents.BladeFlurry * 0.20f)
                              * (1f + (float)Math.Ceiling((10f/3f*talents.LightningReflexes)/100f)) - 1f,
                 */
			};
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents + statsOptionsPanel;
            Stats statsProcs = new Stats();

            // Stamina
            float totalBSTAM = statsTotal.BonusStaminaMultiplier;
            float staBase = (float)Math.Floor((1f + totalBSTAM) * statsRace.Stamina);
            float staBonus = (float)Math.Floor((1f + totalBSTAM) * statsGearEnchantsBuffs.Stamina);
            statsTotal.Stamina = staBase + staBonus;

            // Health
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;

            // Strength
            float totalBSM = statsTotal.BonusStrengthMultiplier;
            float strBase  = (float)Math.Floor((1f + totalBSM) * statsRace.Strength);
            float strBonus = (float)Math.Floor((1f + totalBSM) * statsGearEnchantsBuffs.Strength);
            statsTotal.Strength = strBase + strBonus;

            // Agility
            float totalBAM = statsTotal.BonusAgilityMultiplier;
            float agiBase  = (float)Math.Floor((1f + totalBAM) * statsRace.Agility);
            float agiBonus = (float)Math.Floor((1f + totalBAM) * statsGearEnchantsBuffs.Agility);
            statsTotal.Agility = agiBase + agiBonus;

            // Attack Power
            float totalBAPM        = statsTotal.BonusAttackPowerMultiplier;
            float apBase           = (1f + totalBAPM) * (statsRace.AttackPower);
            float apBonusAGI       = (1f + totalBAPM) * (statsTotal.Agility);
            float apBonusSTR       = (1f + totalBAPM) * (statsTotal.Strength);
            float apBonusOther     = (1f + totalBAPM) * (statsGearEnchantsBuffs.AttackPower);
            statsTotal.AttackPower = (float)Math.Floor(apBase + apBonusAGI + apBonusSTR + apBonusOther);

            // Armor (Not currently being used in Rogue)
            /*statsTotal.Armor       = (float)Math.Floor(statsTotal.Armor      * (1f + statsTotal.BaseArmorMultiplier ));
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor  = (float)Math.Floor(statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.Armor      += statsTotal.BonusArmor;*/

            /*statsTotal.PhysicalHaste *= (1f + StatConversion.GetHasteFromRating(statsTotal.HasteRating,CharacterClass.Rogue))
                                     *  (1f + Talents.BladeFlurry.Haste.Bonus)
                                     *  (1f + Talents.LightningReflexes.Haste.Bonus);*/
            /*
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + ratingHasteBonus);
            */
            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, CharacterClass.Rogue);

            // Defensive Stats
            statsTotal.Dodge += StatConversion.GetDodgeFromAgility(statsTotal.Agility, CharacterClass.Rogue);
            statsTotal.Parry += 5f;

            /*//Berserking
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789) {
                statsTotal.AttackPower += 135f;  //taken straight from Elitist Jerks  
            }
            if (character.OffHand != null && character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789) {
                statsTotal.AttackPower += 135f;  //taken straight from Elitist Jerks  
            }*/

            // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
            CombatFactors combatFactors = new CombatFactors(character, statsTotal);
            WhiteAttacks whiteAttacks = new WhiteAttacks(character, statsTotal, combatFactors);

            float fightDuration = 600f;//calcOpts.Duration;

            float mhHitsPerSecond = 0f; float ohHitsPerSecond = 0f;
            mhHitsPerSecond = 1f / (1.5f/* + calcOpts.GetLatency()*/) * 0.9f * combatFactors.ProbYellowHit;
            // White Hits per second uses hasted numbers, not un-hasted
            if (combatFactors.MH.Speed > 0f) { mhHitsPerSecond += whiteAttacks.MhHits; }
            if (combatFactors.OH.Speed > 0f) { ohHitsPerSecond += whiteAttacks.OhHits; }

            float mhHitInterval = 1f / mhHitsPerSecond;
            float ohHitInterval = 1f / ohHitsPerSecond;
            float bothHitInterval = 1f / (mhHitsPerSecond + ohHitsPerSecond);
            //float bleedHitInterval = 1f / (calcOpts.FuryStance ? 1f : 4f / 3f); // 4/3 ticks per sec with deep wounds and rend both going, 1 tick/sec with just deep wounds
            float dmgDoneInterval = 1f / (mhHitsPerSecond + ohHitsPerSecond /*+ (calcOpts.FuryStance ? 1f : 4f / 3f)*/);
            float poisonHitInterval = 1f;

            SpecialEffect bersMainHand = null;
            SpecialEffect bersOffHand = null;

            // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
            if (character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789) { // berserker enchant id
                Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();

                if (mhEffects.MoveNext()) {
                    bersMainHand = mhEffects.Current;
                    statsProcs += bersMainHand.GetAverageStats(mhHitInterval, 1f, combatFactors.MH.Speed, fightDuration);
                }
            }
            if (character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789) {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();

                if (ohEffects.MoveNext()) {
                    bersOffHand = ohEffects.Current;
                    statsProcs += bersOffHand.GetAverageStats(ohHitInterval, 1f, combatFactors.OH.Speed, fightDuration);
                }
            }
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                if (effect != bersMainHand && effect != bersOffHand) // bersStats is null if the char doesn't have berserking enchant
                {
                    float oldArp = effect.Stats.ArmorPenetrationRating;
                    if (effect.Stats.ArmorPenetrationRating > 0)
                    {
                        float arpenBuffs = 0;
                        float currentArp = arpenBuffs + StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating);
                        float arpToHardCap = (1f - currentArp) * StatConversion.RATING_PER_ARMORPENETRATION;
                        if (arpToHardCap < effect.Stats.ArmorPenetrationRating) effect.Stats.ArmorPenetrationRating = arpToHardCap;
                    }
                    switch (effect.Trigger)
                    {
                        case Trigger.Use:
                            statsProcs += effect.GetAverageStats(0f, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                        case Trigger.MeleeHit:
                        case Trigger.PhysicalHit:
                            statsProcs += effect.GetAverageStats(bothHitInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                        case Trigger.MeleeCrit:
                        case Trigger.PhysicalCrit:
                            statsProcs += effect.GetAverageStats(bothHitInterval, combatFactors.ProbMhCrit, combatFactors.MHSpeed, fightDuration);
                            break;
                        /*case Trigger.DoTTick:
                            statsProcs += effect.GetAverageStats(bleedHitInterval, 1f, combatFactors.MHSpeed, fightDuration); // 1/sec DeepWounds, 1/3sec Rend
                            break;*/
                        case Trigger.DamageDone: // physical and dots
                            statsProcs += effect.GetAverageStats(dmgDoneInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                        case Trigger.SpellHit:
                            statsProcs += effect.GetAverageStats(poisonHitInterval, 1f, combatFactors.MHSpeed, fightDuration);
                            break;
                    }
                    effect.Stats.ArmorPenetrationRating = oldArp;
                }
            }

            statsProcs.Stamina   = (float)Math.Floor(statsProcs.Stamina * (1f + totalBSTAM) * (1f + statsProcs.BonusStaminaMultiplier));
            statsProcs.Strength  = (float)Math.Floor(statsProcs.Strength * (1f + totalBSM) * (1f + statsProcs.BonusStrengthMultiplier));
            statsProcs.Agility  += statsProcs.Paragon;
            statsProcs.Agility   = (float)Math.Floor(statsProcs.Agility * (1f + totalBAM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Agility  += (float)Math.Floor(statsProcs.HighestStat * (1f + totalBAM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Health   += (float)Math.Floor(statsProcs.Stamina * 10f);

            // Armor
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BaseArmorMultiplier + statsProcs.BaseArmorMultiplier));
            statsProcs.BonusArmor += statsProcs.Agility * 2f;
            statsProcs.BonusArmor = (float)Math.Floor(statsProcs.BonusArmor * (1f + statsTotal.BonusArmorMultiplier + statsProcs.BonusArmorMultiplier));
            statsProcs.Armor += statsProcs.BonusArmor;

            // Attack Power
            float totalBAPMProcs    = (1f + statsTotal.BonusAttackPowerMultiplier) * (1f + statsProcs.BonusAttackPowerMultiplier) - 1f;
            float apBonusAGIProcs   = (1f + totalBAPM) * (statsProcs.Agility);
            float apBonusSTRProcs   = (1f + totalBAPM) * (statsProcs.Strength);
            float apBonusOtherProcs = (1f + totalBAPM) * (statsProcs.AttackPower);
            statsProcs.AttackPower  = (float)Math.Floor(apBonusSTRProcs + apBonusAGIProcs + apBonusOtherProcs);

            // Haste
            statsProcs.PhysicalHaste = StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, CharacterClass.Rogue);

            statsTotal += statsProcs;

            // Haste
            statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste) *
                                       (1f + 0.2f * character.RogueTalents.BladeFlurry * 15f / 120f) *
                                       (1f + ratingHasteBonus);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            switch (chartName) {
                case "Combat Table":
                    var currentCalculationsRogue = GetCharacterCalculations(character) as CharacterCalculationsRogue;
                    var calcMiss = new ComparisonCalculationsRogue();
                    var calcDodge = new ComparisonCalculationsRogue();
                    var calcParry = new ComparisonCalculationsRogue();
                    var calcBlock = new ComparisonCalculationsRogue();
                    var calcGlance = new ComparisonCalculationsRogue();
                    var calcCrit = new ComparisonCalculationsRogue();
                    var calcHit = new ComparisonCalculationsRogue();

                    if (currentCalculationsRogue != null)
                    {
                        calcMiss.Name = "    Miss    ";
                        calcDodge.Name = "   Dodge   ";
                        calcGlance.Name = " Glance ";
                        calcCrit.Name = "  Crit  ";
                        calcHit.Name = "Hit";

                        calcMiss.OverallPoints = 0f;
                        calcDodge.OverallPoints = 0f;
                        calcParry.OverallPoints = 0f;
                        calcBlock.OverallPoints = 0f;
                        calcGlance.OverallPoints = 0f;
                        calcCrit.OverallPoints = 0f;
                        calcHit.OverallPoints = 0f;
                    }
                    return new ComparisonCalculationBase[] {calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit};

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        /*public override bool IsBuffRelevant(Buff buff)
        {
            if (!buff.AllowedClasses.Contains(CharacterClass.Rogue)) { return false; }
            if (buff.Name == "Focus Magic") { return false; }

            if (buff.SetName == "Strength of the Clefthoof") { return false; }
            if (buff.SetName == "Skyshatter Regalia") { return false; }

            return base.IsBuffRelevant(buff);
        }*/

        public override bool IsEnchantRelevant(Enchant enchant, Character character)
        {
            string name = enchant.Name;
            if (name.Contains("Rune of"))
            {
                return false; // Bad DK Enchant, Bad!
            }
            return base.IsEnchantRelevant(enchant, character);
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsRogue calcOpts) {
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

        public override Stats GetRelevantStats(Stats stats) {
            Stats relevantStats = new Stats {
               Agility = stats.Agility,
               Strength = stats.Strength,
               AttackPower = stats.AttackPower,
               CritRating = stats.CritRating,
               HitRating = stats.HitRating,
               Stamina = stats.Stamina,
               HasteRating = stats.HasteRating,
               ExpertiseRating = stats.ExpertiseRating,
               ArmorPenetration = stats.ArmorPenetration,
               ArmorPenetrationRating = stats.ArmorPenetrationRating,
               BloodlustProc = stats.BloodlustProc,
               WeaponDamage = stats.WeaponDamage,
               BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
               BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
               BonusCritMultiplier = stats.BonusCritMultiplier,
               BonusDamageMultiplier = stats.BonusDamageMultiplier,
               BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
               BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
               Health = stats.Health,
               Bloodlust = stats.Bloodlust,
               ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
               PhysicalHaste = stats.PhysicalHaste,
               PhysicalHit = stats.PhysicalHit,
               PhysicalCrit = stats.PhysicalCrit,
               HighestStat = stats.HighestStat,
               
               /*AllResist = stats.AllResist,
               ArcaneResistance = stats.ArcaneResistance,
               NatureResistance = stats.NatureResistance,
               FireResistance = stats.FireResistance,
               FrostResistance = stats.FrostResistance,
               ShadowResistance = stats.ShadowResistance,
               ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
               NatureResistanceBuff = stats.NatureResistanceBuff,
               FireResistanceBuff = stats.FireResistanceBuff,
               FrostResistanceBuff = stats.FrostResistanceBuff,
               ShadowResistanceBuff = stats.ShadowResistanceBuff,*/
               
               BonusSnDDuration = stats.BonusSnDDuration,
               CPOnFinisher = stats.CPOnFinisher,
               BonusEvisEnvenomDamage = stats.BonusEvisEnvenomDamage,
               BonusFreeFinisher = stats.BonusFreeFinisher,
               BonusCPGDamage = stats.BonusCPGDamage,
               BonusSnDHaste = stats.BonusSnDHaste,
               BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
               RogueT7TwoPieceBonus = stats.RogueT7TwoPieceBonus,
               RogueT7FourPieceBonus = stats.RogueT7FourPieceBonus,
               RogueT8TwoPieceBonus = stats.RogueT8TwoPieceBonus,
               RogueT8FourPieceBonus = stats.RogueT8FourPieceBonus,
               ReduceEnergyCostFromRupture = stats.ReduceEnergyCostFromRupture,
               BonusCPGCritChance = stats.BonusCPGCritChance,
               BonusToTTEnergy = stats.BonusToTTEnergy,
               ChanceOn3CPOnFinisher = stats.ChanceOn3CPOnFinisher,

               BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
               SpellHit = stats.SpellHit,
               SpellCrit = stats.SpellCrit,
               SpellCritOnTarget = stats.SpellCritOnTarget,

                // Damage Procs
               ShadowDamage = stats.ShadowDamage,
               ArcaneDamage = stats.ArcaneDamage,
               HolyDamage = stats.HolyDamage,
               NatureDamage = stats.NatureDamage,
               FrostDamage = stats.FrostDamage,
               FireDamage = stats.FireDamage,
               BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
               BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
               BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
               BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
               BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
               BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.SpellHit)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        relevantStats.AddSpecialEffect(effect);
                    }
                }
            }

            return relevantStats;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = (
                    stats.Agility +
                    stats.Strength +
                    stats.AttackPower +
                    stats.CritRating +
                    stats.HitRating +
                    //stats.Stamina +
                    stats.HasteRating +
                    stats.ExpertiseRating +
                    stats.ArmorPenetration +
                    stats.ArmorPenetrationRating +
                    stats.BloodlustProc +
                    stats.WeaponDamage +
                    stats.BonusAgilityMultiplier +
                    stats.BonusAttackPowerMultiplier +
                    stats.BonusCritMultiplier +
                    stats.BonusDamageMultiplier +
                    stats.BonusStaminaMultiplier +
                    stats.BonusStrengthMultiplier +
                    //stats.Health +
                    stats.Bloodlust +
                    stats.ThreatReductionMultiplier +
                    stats.PhysicalHaste +
                    stats.PhysicalHit +
                    stats.PhysicalCrit +
                    stats.HighestStat +

                    /*stats.AllResist +
                    stats.ArcaneResistance +
                    stats.NatureResistance +
                    stats.FireResistance +
                    stats.FrostResistance +
                    stats.ShadowResistance +
                    stats.ArcaneResistanceBuff +
                    stats.NatureResistanceBuff +
                    stats.FireResistanceBuff +
                    stats.FrostResistanceBuff +
                    stats.ShadowResistanceBuff +*/

                    stats.BonusSnDDuration +
                    stats.CPOnFinisher +
                    stats.BonusEvisEnvenomDamage +
                    stats.BonusFreeFinisher +
                    stats.BonusCPGDamage +
                    stats.BonusSnDHaste +
                    stats.BonusBleedDamageMultiplier +
                    stats.RogueT7TwoPieceBonus +
                    stats.RogueT7FourPieceBonus +
                    stats.RogueT8TwoPieceBonus +
                    //stats.RuptureCrit +
                    stats.RogueT8FourPieceBonus +
                    stats.ReduceEnergyCostFromRupture +
                    stats.BonusCPGCritChance +
                    stats.BonusToTTEnergy +
                    stats.ChanceOn3CPOnFinisher +

                    stats.BonusPhysicalDamageMultiplier +
                    stats.SpellHit +
                    stats.SpellCrit +
                    stats.SpellCritOnTarget +

                    // Damage Procs
                    stats.ShadowDamage +
                    stats.ArcaneDamage +
                    stats.HolyDamage +
                    stats.NatureDamage +
                    stats.FrostDamage +
                    stats.FireDamage +
                    stats.BonusShadowDamageMultiplier +
                    stats.BonusArcaneDamageMultiplier +
                    stats.BonusHolyDamageMultiplier +
                    stats.BonusNatureDamageMultiplier +
                    stats.BonusFrostDamageMultiplier +
                    stats.BonusFireDamageMultiplier
                ) > 0 && stats.SpellPower == 0;

            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use
                    || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.MeleeCrit
                    || effect.Trigger == Trigger.PhysicalHit
                    || effect.Trigger == Trigger.PhysicalCrit
                    || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone
                    || effect.Trigger == Trigger.SpellHit) // For Poison Hits
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) { break; }
                }
            }
            return relevant;
        }

        private static List<string> _relevantGlyphs = null;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Backstab");
                _relevantGlyphs.Add("Glyph of Eviscerate");
                _relevantGlyphs.Add("Glyph of Mutilate");
                _relevantGlyphs.Add("Glyph of Hunger For Blood");
                _relevantGlyphs.Add("Glyph of Sinister Strike");
                _relevantGlyphs.Add("Glyph of Slice and Dice");
                _relevantGlyphs.Add("Glyph of Feint");
                _relevantGlyphs.Add("Glyph of Rupture");
                _relevantGlyphs.Add("Glyph of Blade Flurry");
                _relevantGlyphs.Add("Glyph of Adrenaline Rush");
                _relevantGlyphs.Add("Glyph of Killing Spree");
                _relevantGlyphs.Add("Glyph of Vigor");
                _relevantGlyphs.Add("Glyph of Fan of Knives");
                _relevantGlyphs.Add("Glyph of Expose Armor");
                _relevantGlyphs.Add("Glyph of Ghostly Strike");
                _relevantGlyphs.Add("Glyph of Tricks of the Trade");
            }
            return _relevantGlyphs;
        }
    }
    public class Knuckles : Item { public Knuckles() { Speed = 2f; } }
}