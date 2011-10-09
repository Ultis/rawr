using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace Rawr.Rogue
{
    [Rawr.Calculations.RawrModelInfo("Rogue", "Ability_Rogue_SliceDice", CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase
    {
        #region Variables and Properties
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get {
                // Relevant Gem IDs for Rogues
                //Red
                int[] delicate = { 52082, 52212, /*, 52258*/ }; // Agi

                //Purple
                int[] glinting = { 52102, 52220 }; //Agi/Hit

                //Blue
                int[] rigid = { 52089, 52235, /*, 52264*/ }; // Hit

                //Green
                int[] lightning = { 52125, 52225 }; // Haste/Hit
                int[] piercing = { 52122, 52228 }; // Crit/Hit
                int[] sensei = { 52128, 52237 }; // Mast/Hit

                //Yellow
                int[] fractured = { 52049, 52219, /*, 52269*/ }; // Mast
                int[] quick = { 52093, 52232, /*, 52268*/ }; // Haste
                int[] smooth = { 52091, 52241, /*, 52266*/ }; // Crit

                //Orange
                int[] adept = { 52118, 52204 }; // Agi/Mast
                int[] deadly = { 52109, 52209 }; // Agi/Crit
                int[] deft = { 52112, 52211 }; // Agi/Haste
                int[] keen = { 52118, 52224 }; // Exp/Mast

                //Meta
                int agile = 68778; // Agi/Crit dmg

                List<GemmingTemplate> list = new List<GemmingTemplate>();
                for (int tier = 0; tier < 2; tier++)
                {
                    list.AddRange(new GemmingTemplate[]
                        {
                            CreateRogueGemmingTemplate(tier, delicate, delicate, delicate, delicate, agile),
                            CreateRogueGemmingTemplate(tier, delicate,    adept, glinting, delicate, agile),
                            CreateRogueGemmingTemplate(tier, delicate,     deft, glinting, delicate, agile),
                        });
                }
                return list;
            }
        }

        private const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateRogueGemmingTemplate(int tier, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate()
            {
                Model = "Rogue",
                Group = (new string[] { "Uncommon", "Rare", "Epic", "Jewelcrafter" })[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta,
            };
        }

        private CalculationOptionsPanelRogue _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelRogue()); } }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Summary:Overall Points*Sum of your DPS Points and Survivability Points.",
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
                    "Basic Stats:Mastery Rating",
                    "Basic Stats:Weapon Damage",
                    
                    "Complex Stats:Avoided White Attacks",
                    "Complex Stats:Avoided Yellow Attacks",
                    "Complex Stats:Avoided Poison Attacks",
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
                    "Abilities:Revealing Strike",
                    "Abilities:Slice and Dice",
                    "Abilities:Rupture",
                    "Abilities:Eviscerate",
                    "Abilities:Envenom",
                    "Abilities:Instant Poison",
                    "Abilities:Deadly Poison",
                    "Abilities:Wound Poison",
                    "Abilities:Venomous Wounds",
                    "Abilities:Main Gauche"
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
                        "Avoided Yellow Attacks %",
                        "Avoided Poison Attacks %",
                        "Avoided White Attacks %",
                        "Custom Rotation DPS",
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

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
                }
                return _subPointNameColors;
            }
        }

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
        public bool PTRMode = false;
        
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            CalculationOptionsRogue calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsRogue calc = new CharacterCalculationsRogue();
            if (character == null) { return calc; }
            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            if (calcOpts == null) { return calc; }
            //
            BossOptions bossOpts = character.BossOptions;
            RogueTalents talents = character.RogueTalents;
            PTRMode = calcOpts.PTRMode;
            #region Spec determination
            int spec;
            int assCounter = 0, combatCounter = 0, subtCounter = 0;
            for (int i = 0; i <= 18; i++) assCounter += int.Parse(talents.ToString()[i].ToString());
            for (int i = 19; i <= 37; i++) combatCounter += int.Parse(talents.ToString()[i].ToString());
            for (int i = 38; i <= 56; i++) subtCounter += int.Parse(talents.ToString()[i].ToString());
            if (assCounter > combatCounter && assCounter > subtCounter) spec = 0;
            else if (combatCounter > subtCounter) spec = 1;
            else spec = 2;
            #endregion
            int targetLevel = bossOpts.Level;
            float targetArmor = bossOpts.Armor;
            bool targetPoisonable = calcOpts.TargetPoisonable;
            WeightedStat[] critRatingUptimes;
            Stats stats = GetCharacterStatsWithTemporaryEffects(character, additionalItem, out critRatingUptimes);
            calc.BasicStats = stats;
            calc.TargetLevel = targetLevel;
            calc.Spec = spec;
            Item mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
            Item offHand = character.OffHand == null ? new Knuckles() : character.OffHand.Item;
            
            #region Basic Chances and Constants
            #region Constants from talents
            float dmgBonusOnGarrRuptTickChance = RV.Talents.VenomousWoundsProcChance * talents.VenomousWounds;
            float cPGCritDmgMult = RV.Talents.LethalityCritMult * talents.Lethality;
            float ambushBSCostReduc = RV.Talents.SlaughterFTShadowsBSAmbushCostReduc[talents.SlaughterFromTheShadows];
            float ambushCPBonus = RV.Talents.InitiativeChance * talents.Initiative;
            float ambushCritBonus = RV.Talents.ImpAmbushCritBonus * talents.ImprovedAmbush;
            float ambushDmgMult = RV.Talents.ImpAmbushDmgMult * talents.ImprovedAmbush + RV.Talents.OpportunityDmgMult * talents.Opportunity;
            float bSDmgMult = RV.Talents.AggressionDmgMult[talents.Aggression] + RV.Talents.OpportunityDmgMult * talents.Opportunity + (spec == 2 ? RV.Mastery.SinisterCallingMult: 0f);
            float bSCritBonus = talents.PuncturingWounds * RV.Talents.PuncturingWoundsBSCritMult + (stats.Rogue_T11_2P > 0 ? RV.Set.T112CritBonus : 0f);
            float evisCritBonus = (talents.GlyphOfEviscerate ? RV.Glyph.EvisCritMult : 0f);
            float mutiCritBonus = talents.PuncturingWounds * RV.Talents.PuncturingWoundsMutiCritMult + (stats.Rogue_T11_2P > 0 ? RV.Set.T112CritBonus : 0f);
            float ruptDmgMult = (spec == 2 ? RV.Mastery.Executioner + RV.Mastery.ExecutionerPerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f);
            float ruptDurationBonus = talents.GlyphOfRupture ? RV.Glyph.RuptBonusDuration : 0;
            float snDDurationBonus = talents.GlyphOfSliceandDice ? RV.Glyph.SnDBonusDuration : 0;
            float exposeDurationBonus = talents.GlyphOfExposeArmor ? RV.Glyph.ExposeBonusDuration : 0;
            float snDDurationMult = RV.Talents.ImpSliceAndDice * talents.ImprovedSliceAndDice;
            float sStrikeCritBonus = (stats.Rogue_T11_2P > 0 ? RV.Set.T112CritBonus : 0f);
            float cPonCPGCritChance = RV.Talents.SealFateChance * talents.SealFate;
            float evisDmgMult = (1f + RV.Talents.AggressionDmgMult[talents.Aggression] + RV.Talents.CoupDeGraceMult[talents.CoupDeGrace]) * (1f + (spec == 2 ? RV.Mastery.Executioner + RV.Mastery.ExecutionerPerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f)) - 1f;
            float envenomDmgMult = (1f + RV.Talents.CoupDeGraceMult[talents.CoupDeGrace]) * (1f + (spec == 2 ? RV.Mastery.Executioner + RV.Mastery.ExecutionerPerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f)) - 1f;
            float hemoCostReduc = RV.Talents.SlaughterFTShadowsHemoCostReduc * talents.SlaughterFromTheShadows;
            float hemoDmgMult = (spec == 2 ? RV.Mastery.SinisterCallingMult : 0f);
            float meleeDmgMult = spec == 0 && mainHand.Type == ItemType.Dagger && offHand.Type == ItemType.Dagger ? RV.Mastery.AssassinsResolveMeleeDmgBonus : 0f;
            float meleeSpeedMult = (1f + RV.Talents.LightningReflexesSpeedMult * talents.LightningReflexes) * (1f + RV.AR.MeleeSpeedMult * RV.AR.Duration / RV.AR.CD * talents.AdrenalineRush) * (1f + RV.SnD.SpeedBonus) - 1f;
            float mutiCostReduc = talents.GlyphOfMutilate ? RV.Glyph.MutiCostReduc : 0;
            float mutiDmgMult = RV.Talents.OpportunityDmgMult * talents.Opportunity;
            float oHDmgMult = (1f + RV.OHDmgReduc) * (1f + (spec == 1 ? RV.Mastery.AmbidexterityDmgMult : 0f)) - 1f;
            float potentPoisonsMult = (spec == 0 ? RV.Mastery.PotentPoisonsDmgMult + RV.Mastery.PotentPoisonsDmgMultPerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f);
            float spellDmgMult = character.ActiveBuffs.Contains(Buff.GetBuffByName("Lightning Breath")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Fire Breath")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Master Poisoner")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Ebon Plaguebringer")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Earth and Moon")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Curse of the Elements")) ? 0f : (calcOpts.TargetPoisonable ? RV.Talents.MasterPoisonerSpellDmgMult * talents.MasterPoisoner : 0f);
            float natureDmgMult = (1f + spellDmgMult) * (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + potentPoisonsMult) - 1f;
            float poisonDmgMult = (1f + spellDmgMult) * (1f + stats.BonusNatureDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + potentPoisonsMult + RV.Talents.VilePoisonsDmgMult[talents.VilePoisons]) - 1f;
            float sSCostReduc = RV.Talents.ImpSinisterStrikeCostReduc * talents.ImprovedSinisterStrike;
            float sSDmgMult = (1f + RV.Talents.ImpSinisterStrikeDmgMult * talents.ImprovedSinisterStrike) * (1f + RV.Talents.AggressionDmgMult[talents.Aggression]) - 1f;
            #endregion

            float exposeArmor = character.ActiveBuffs.Contains(Buff.GetBuffByName("Corrosive Spit")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Tear Armor")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Sunder Armor")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Faerie Fire")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Expose Armor")) ? 0f : RV.Expose.ArmorReduc;
            float findWeakness = talents.FindWeakness * RV.Talents.FindWeaknessArmorIgnore * ((RV.SD.Duration + RV.Talents.FindWeaknessDuration) / RV.SD.CD +
                (RV.Talents.FindWeaknessDuration / (RV.Vanish.CD - RV.Talents.ElusivenessVanishCDReduc * talents.Elusiveness) +
                (talents.Preparation > 0 ? RV.Talents.FindWeaknessDuration / (RV.Talents.PreparationCD * talents.Preparation) : 0f)));
            float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Armor, stats.TargetArmorReduction + exposeArmor, 0f) * (1f - findWeakness);
            float critMultiplier = RV.CritDmgMult * (1f + stats.BonusCritDamageMultiplier);
            float critMultiplierPoison = RV.CritDmgMultPoison * (1f + stats.BonusCritDamageMultiplier);
            float hasteBonus = (1f + StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Rogue)) * (1f + stats.PhysicalHaste) - 1f;
            float speedModifier = 1f / (1f + hasteBonus) / (1f + meleeSpeedMult);
            float mainHandSpeed = mainHand == null ? 0f : mainHand._speed * speedModifier;
            float offHandSpeed = offHand == null ? 0f : offHand._speed * speedModifier;

            float mainHandSpeedNorm = mainHand.Type == ItemType.Dagger ? RV.WeapSpeedNormDagger : RV.WeapSpeedNorm;
            float offHandSpeedNorm = mainHand.Type == ItemType.Dagger ? RV.WeapSpeedNormDagger : RV.WeapSpeedNorm;

            float hitBonus = stats.PhysicalHit + StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Rogue);
            float spellHitBonus = stats.SpellHit + StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Rogue);
            float expertiseMHBonus = ((character.Race == CharacterRace.Human && (mainHand.Type == ItemType.OneHandSword || mainHand.Type == ItemType.OneHandMace)) ? RV.Racial.HumanExpBonus : 0) +
                                        ((character.Race == CharacterRace.Dwarf && (mainHand.Type == ItemType.OneHandMace)) ? RV.Racial.DwarfExpBonus : 0) +
                                        ((character.Race == CharacterRace.Orc && (mainHand.Type == ItemType.OneHandAxe || mainHand.Type == ItemType.FistWeapon)) ? RV.Racial.OrcExpBonus : 0);
            float expertiseOHBonus = ((character.Race == CharacterRace.Human && (offHand.Type == ItemType.OneHandSword || offHand.Type == ItemType.OneHandMace)) ? RV.Racial.HumanExpBonus : 0) +
                                        ((character.Race == CharacterRace.Dwarf && (offHand.Type == ItemType.OneHandMace)) ? RV.Racial.DwarfExpBonus : 0) +
                                        ((character.Race == CharacterRace.Orc && (offHand.Type == ItemType.OneHandAxe || offHand.Type == ItemType.FistWeapon)) ? RV.Racial.OrcExpBonus : 0);
            expertiseMHBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Rogue) + stats.Expertise + expertiseMHBonus, CharacterClass.Rogue);
            expertiseOHBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Rogue) + stats.Expertise + expertiseOHBonus, CharacterClass.Rogue);

            float chanceMHDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - character.Level] - expertiseMHBonus);
            float chanceOHDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - character.Level] - expertiseOHBonus);
            float chanceParry = 0f; //Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - character.Level] - expertiseBonus);
            float chanceWhiteMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP_DW[targetLevel - character.Level] - hitBonus);
            float chanceMiss = Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[targetLevel - character.Level] - hitBonus);
            float chancePoisonMiss = Math.Max(0f, StatConversion.GetSpellMiss(character.Level - targetLevel, false) - spellHitBonus);

            float glanceMultiplier = RV.GlanceMult;
            float chanceWhiteMHAvoided = chanceWhiteMiss + chanceMHDodge + chanceParry;
            float chanceWhiteOHAvoided = chanceWhiteMiss + chanceOHDodge + chanceParry;
            float chanceMHAvoided = chanceMiss + chanceMHDodge + chanceParry;
            float chanceOHAvoided = chanceMiss + chanceOHDodge + chanceParry;
            float chanceFinisherAvoided = chanceMiss + chanceMHDodge + chanceParry;
            float chancePoisonAvoided = chancePoisonMiss;
            float chanceWhiteMHNonAvoided = 1f - chanceWhiteMHAvoided;
            float chanceWhiteOHNonAvoided = 1f - chanceWhiteOHAvoided;
            float chanceMHNonAvoided = 1f - chanceMHAvoided;
            float chanceOHNonAvoided = 1f - chanceOHAvoided;
            float chancePoisonNonAvoided = 1f - chancePoisonAvoided;

            ////Crit Chances
            float chanceCritYellow = 0f;
            float chanceHitYellow = 0f;
            float chanceCritAmbush = 0f;
            float chanceHitAmbush = 0f;
            float chanceCritBackstab = 0f;
            float chanceHitBackstab = 0f;
            float cpPerBackstab = 0f;
            float chanceCritMuti = 0f;
            float chanceHitMuti = 0f;
            float cpPerMuti = 0f;
            float chanceCritSStrike = 0f;
            float chanceHitSStrike = 0f;
            float cpPerSStrike = 0f;
            float chanceCritHemo = 0f;
            float chanceHitHemo = 0f;
            float cpPerHemo = 0f;
            float chanceCritRStrike = 0f;
            float chanceHitRStrike = 0f;
            float cpPerRStrike = 0f;
            float chanceCritEvis = 0f;
            float chanceHitEvis = 0f;
            //float chanceCritBleed = 0f;
            float chanceGlance = StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - character.Level];
            float chanceCritWhiteMainTotal = 0f;
            float chanceCritWhiteMain = 0f;
            float chanceHitWhiteMain = 0f;
            float chanceCritWhiteOffTotal = 0f;
            float chanceCritWhiteOff = 0f;
            float chanceHitWhiteOff = 0f;
            float chanceCritPoison = 0f;
            float chanceHitPoison = 0f;

            for (int i = 0; i < critRatingUptimes.Length; i++)
            { //Sum up the weighted chances for each crit value
                WeightedStat iStat = critRatingUptimes[i];
                //Yellow - 2 Roll, so total of X chance to avoid, total of 1 chance to crit and hit when not avoided
                float chanceCritYellowTemp = Math.Min(1f, StatConversion.GetCritFromRating(stats.CritRating + iStat.Value, CharacterClass.Rogue)
                    + StatConversion.GetCritFromAgility(stats.Agility - RV.BaseStatCalcReduc, CharacterClass.Rogue)
                    + stats.PhysicalCrit
                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - character.Level]
                    );
                float chanceHitYellowTemp = 1f - chanceCritYellowTemp;
                float chanceCritPoisonTemp = Math.Min(1f, StatConversion.GetSpellCritFromRating(stats.CritRating + iStat.Value)
                    + stats.SpellCrit
                    + stats.SpellCritOnTarget
                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - character.Level]);
                float chanceHitPoisonTemp = 1f - chanceCritPoisonTemp;

                //Ambush - Identical to Yellow, but higher crit chance
                float chanceCritAmbushTmp = Math.Min(1f, chanceCritYellowTemp + ambushCritBonus);
                float chanceHitAmbushTmp = 1f - chanceCritAmbushTmp;
                float cpPerAmbushTmp = (chanceHitAmbushTmp + chanceCritAmbushTmp * (1f + cPonCPGCritChance)) * (1f + ambushCPBonus);

                //Backstab - Identical to Yellow, with higher crit chance
                float chanceCritBackstabTemp = Math.Min(1f, chanceCritYellowTemp + bSCritBonus);
                float chanceHitBackstabTemp = 1f - chanceCritBackstabTemp;
                float cpPerBackstabTemp = (chanceHitBackstabTemp + chanceCritBackstabTemp * (1f + cPonCPGCritChance));

                //Mutilate - Identical to Yellow, with higher crit chance
                float chanceCritMutiTemp = Math.Min(1f, chanceCritYellowTemp + mutiCritBonus);
                float chanceHitMutiTemp = 1f - chanceCritMutiTemp;
                float cpPerMutiTemp = (1 + chanceHitMutiTemp * chanceHitMutiTemp + (1 - chanceHitMutiTemp * chanceHitMutiTemp) * (1f + cPonCPGCritChance));

                //Sinister Strike - Identical to Yellow, with higher crit chance
                float chanceCritSStrikeTemp = Math.Min(1f, chanceCritYellowTemp + sStrikeCritBonus);
                float chanceHitSStrikeTemp = 1f - chanceCritSStrikeTemp;
                float cpPerSStrikeTemp = (chanceHitSStrikeTemp + chanceCritSStrikeTemp * (1f + cPonCPGCritChance)) * (1f + (talents.GlyphOfSinisterStrike ? RV.Glyph.SSCPBonusChance : 0f));

                //Hemorrhage - Identical to Yellow, with higher crit chance
                float chanceCritHemoTemp = Math.Min(1f, chanceCritYellowTemp);
                float chanceHitHemoTemp = 1f - chanceCritHemoTemp;
                float cpPerHemoTemp = (chanceHitHemoTemp + chanceCritHemoTemp * (1f + cPonCPGCritChance));

                //Revealing Strike - Identical to Yellow, with higher crit chance
                float chanceCritRStrikeTemp = Math.Min(1f, chanceCritYellowTemp);
                float chanceHitRStrikeTemp = 1f - chanceCritRStrikeTemp;
                float cpPerRStrikeTemp = chanceHitRStrikeTemp + chanceCritRStrikeTemp;

                //Eviscerate - Identical to Yellow, with higher crit chance
                float chanceCritEvisTemp = Math.Min(1f, chanceCritYellowTemp + evisCritBonus);
                float chanceHitEvisTemp = 1f - chanceCritEvisTemp;

                //White Mainhand
                float chanceCritWhiteMainTotalTemp = chanceCritYellowTemp;
                float chanceCritWhiteMainTemp = Math.Min(chanceCritYellowTemp, 1f - chanceGlance - chanceWhiteMHAvoided);
                float chanceHitWhiteMainTemp = 1f - chanceCritWhiteMainTemp - chanceWhiteMHAvoided - chanceGlance;
//                float chanceCritWhiteMainTemp = Math.Min(chanceCritYellowTemp, 1f - chanceGlance);
//                float chanceHitWhiteMainTemp = 1f - chanceCritWhiteMainTemp - chanceGlance;
                //White Offhand
                float chanceCritWhiteOffTotalTemp = chanceCritYellowTemp;
                float chanceCritWhiteOffTemp = Math.Min(chanceCritYellowTemp, 1f - chanceGlance - chanceWhiteOHAvoided);
                float chanceHitWhiteOffTemp = 1f - chanceCritWhiteOffTemp - chanceWhiteOHAvoided - chanceGlance;
//                float chanceCritWhiteOffTemp = Math.Min(chanceCritYellowTemp, 1f - chanceGlance);
//                float chanceHitWhiteOffTemp = 1f - chanceCritWhiteOffTemp - chanceGlance;

                chanceCritYellow += iStat.Chance * chanceCritYellowTemp;
                chanceHitYellow += iStat.Chance * chanceHitYellowTemp;
                chanceCritAmbush += iStat.Chance * chanceCritAmbushTmp;
                chanceHitAmbush += iStat.Chance * chanceHitAmbushTmp;
                chanceCritBackstab += iStat.Chance * chanceCritBackstabTemp;
                chanceHitBackstab += iStat.Chance * chanceHitBackstabTemp;
                cpPerBackstab += iStat.Chance * cpPerBackstabTemp;
                chanceCritMuti += iStat.Chance * chanceCritMutiTemp;
                chanceHitMuti += iStat.Chance * chanceHitMutiTemp;
                cpPerMuti += iStat.Chance * cpPerMutiTemp;
                chanceCritSStrike += iStat.Chance * chanceCritSStrikeTemp;
                chanceHitSStrike += iStat.Chance * chanceHitSStrikeTemp;
                cpPerSStrike += iStat.Chance * cpPerSStrikeTemp;
                chanceCritHemo += iStat.Chance * chanceCritHemoTemp;
                chanceHitHemo += iStat.Chance * chanceHitHemoTemp;
                cpPerHemo += iStat.Chance * cpPerHemoTemp;
                chanceCritRStrike += iStat.Chance * chanceCritRStrikeTemp;
                chanceHitRStrike += iStat.Chance * chanceHitRStrikeTemp;
                cpPerRStrike += iStat.Chance * cpPerRStrikeTemp;
                chanceCritEvis += iStat.Chance * chanceCritEvisTemp;
                chanceHitEvis += iStat.Chance * chanceHitEvisTemp;
                chanceCritWhiteMainTotal += iStat.Chance * chanceCritWhiteMainTotalTemp;
                chanceCritWhiteMain += iStat.Chance * chanceCritWhiteMainTemp;
                chanceHitWhiteMain += iStat.Chance * chanceHitWhiteMainTemp;
                chanceCritWhiteOffTotal += iStat.Chance * chanceCritWhiteOffTotalTemp;
                chanceCritWhiteOff += iStat.Chance * chanceCritWhiteOffTemp;
                chanceHitWhiteOff += iStat.Chance * chanceHitWhiteOffTemp;
                chanceCritPoison += iStat.Chance * chanceCritPoisonTemp;
                chanceHitPoison += iStat.Chance * chanceHitPoisonTemp;
            }

            calc.DodgedMHAttacks = chanceMHDodge * 100f;
            calc.DodgedOHAttacks = chanceOHDodge * 100f;
            calc.ParriedAttacks = chanceParry * 100f;
            calc.MissedWhiteAttacks = chanceWhiteMiss * 100f;
            calc.MissedAttacks = chanceMiss * 100f;
            calc.MissedPoisonAttacks = chancePoisonMiss * 100f;

            float timeToReapplyDebuffs = 1f / (1f - chanceMHAvoided) - 1f;
            float lagVariance = (float)calcOpts.Latency / 1000f;
            float ruptDurationUptime = RV.Rupt.BaseDuration + ruptDurationBonus;
            float ruptDurationAverage = ruptDurationUptime + timeToReapplyDebuffs + lagVariance;
            float snDBonusDuration = snDDurationBonus - lagVariance;
            float recupBonusDuration = -lagVariance;
            float eABonusDuration = exposeDurationBonus - lagVariance;
            #endregion

            #region Attack Damages
            float DPSfromAP = stats.AttackPower / RV.APperDPS;
            float baseDamage = (mainHand == null ? 0f : mainHand._speed) * DPSfromAP + stats.WeaponDamage + (mainHand.MinDamage + mainHand.MaxDamage) / 2f;
            float baseDamageNorm = mainHandSpeedNorm * DPSfromAP + stats.WeaponDamage + (mainHand.MinDamage + mainHand.MaxDamage) / 2f;
            float baseOffDamage = ((offHand == null ? 0f : offHand._speed) * DPSfromAP + stats.WeaponDamage + (offHand.MinDamage + offHand.MaxDamage) / 2f) * (1f + oHDmgMult);
            float baseOffDamageNorm = (offHandSpeedNorm * DPSfromAP + stats.WeaponDamage + (offHand.MinDamage + offHand.MaxDamage) / 2f) * (1f + oHDmgMult);
            float meleeBonus = (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + meleeDmgMult);
            float meleeDamageRaw = baseDamage * meleeBonus * modArmor;
            float meleeOffDamageRaw = baseOffDamage * meleeBonus * modArmor;
            float meleeDamageNormRaw = baseDamageNorm * meleeBonus * modArmor;
            float meleeOffDamageNormRaw = baseOffDamageNorm * meleeBonus * modArmor;
            float backstabDamageRaw = (baseDamageNorm * RV.BS.WeapDmgMult + RV.BS.BonusDmg) * meleeBonus * (1f + bSDmgMult) * modArmor;
            backstabDamageRaw *= (mainHand._type == ItemType.Dagger ? 1f : 0f);
            float hemoDamageRaw = (baseDamageNorm * RV.Hemo.WeapDmgMult * (mainHand._type == ItemType.Dagger ? RV.Hemo.DaggerDmgMult : 0f)) * meleeBonus * (1f + hemoDmgMult) * modArmor;
            hemoDamageRaw *= (talents.Hemorrhage > 0 ? 1f: 0f);
            float sStrikeDamageRaw = (baseDamageNorm + RV.SS.BonusDmg) * meleeBonus * (1f + sSDmgMult) * modArmor;
            float mutiDamageMainRaw = (baseDamageNorm * RV.Muti.WeapDmgMult + RV.Muti.BonusDmg) * meleeBonus * (1f + mutiDmgMult) * (1f + (targetPoisonable ? 0.2f : 0f)) * modArmor;
            mutiDamageMainRaw *= ((spec == 0 && mainHand._type == ItemType.Dagger && offHand._type == ItemType.Dagger) ? 1f : 0f);
            float mutiDamageOffRaw = (baseOffDamageNorm * RV.Muti.WeapDmgMult + RV.Muti.BonusDmg) * meleeBonus * (1f + mutiDmgMult) * (1f + (targetPoisonable ? 0.2f : 0f)) * modArmor;
            mutiDamageOffRaw *= ((spec == 0 && mainHand._type == ItemType.Dagger && offHand._type == ItemType.Dagger) ? 1f : 0f);
            float mutiDamageRaw = mutiDamageMainRaw + mutiDamageOffRaw;
            float rStrikeDamageRaw = (baseDamage * RV.RS.WeapDmgMult) * meleeBonus * modArmor;
            rStrikeDamageRaw *= talents.RevealingStrike > 0 ? 1f : 0f;
            float ambushDmgRaw = (baseDamageNorm + RV.Ambush.BonusDmg) * RV.Ambush.WeapDmgMult * (mainHand._type == ItemType.Dagger ? RV.Ambush.DaggerDmgMult : 1f) * (1f + ambushDmgMult);
            float[] ruptDamageRaw = new float[] {0,
                (RV.Rupt.BaseDmg + 1 * RV.Rupt.TickBaseDmg + RV.Rupt.TickAPMult[1] * stats.AttackPower) * (3f + 1f + ruptDurationBonus / RV.Rupt.TickTime) * meleeBonus * (1f + stats.BonusBleedDamageMultiplier),
                (RV.Rupt.BaseDmg + 2 * RV.Rupt.TickBaseDmg + RV.Rupt.TickAPMult[2] * stats.AttackPower) * (3f + 2f + ruptDurationBonus / RV.Rupt.TickTime) * meleeBonus * (1f + stats.BonusBleedDamageMultiplier),
                (RV.Rupt.BaseDmg + 3 * RV.Rupt.TickBaseDmg + RV.Rupt.TickAPMult[3] * stats.AttackPower) * (3f + 3f + ruptDurationBonus / RV.Rupt.TickTime) * meleeBonus * (1f + stats.BonusBleedDamageMultiplier),
                (RV.Rupt.BaseDmg + 4 * RV.Rupt.TickBaseDmg + RV.Rupt.TickAPMult[4] * stats.AttackPower) * (3f + 4f + ruptDurationBonus / RV.Rupt.TickTime) * meleeBonus * (1f + stats.BonusBleedDamageMultiplier),
                (RV.Rupt.BaseDmg + 5 * RV.Rupt.TickBaseDmg + RV.Rupt.TickAPMult[5] * stats.AttackPower) * (3f + 5f + ruptDurationBonus / RV.Rupt.TickTime) * meleeBonus * (1f + stats.BonusBleedDamageMultiplier)};
            float evisBaseDamageRaw = RV.Evis.BaseAvgDmg * meleeBonus * (1f + evisDmgMult) * modArmor;
            float evisCPDamageRaw = (RV.Evis.CPBaseDmg + RV.Evis.CPAPMult * stats.AttackPower) * meleeBonus * (1f + evisDmgMult) * modArmor;
            float envenomBaseDamageRaw = RV.Envenom.BaseDmg * (1f + natureDmgMult) * (1f + envenomDmgMult) * (1f + meleeDmgMult);
            float envenomCPDamageRaw = (RV.Envenom.CPBaseDmg + RV.Envenom.CPAPMult * stats.AttackPower) * (1f + natureDmgMult) * (1f + envenomDmgMult) * (1f + meleeDmgMult);
            float iPDamageRaw = (RV.IP.BaseAvgDmg + RV.IP.APMult * stats.AttackPower) * (1f + poisonDmgMult);
            float dPDamageRaw = (RV.DP.BaseDmg + RV.DP.APMult * stats.AttackPower) * (1f + poisonDmgMult) * RV.DP.TickTime / RV.DP.Duration;
            float wPDamageRaw = (RV.WP.BaseDmg + RV.WP.APMult * stats.AttackPower) * (1f + poisonDmgMult);
            float venomousWoundsRaw = dmgBonusOnGarrRuptTickChance * (RV.Talents.VenomousWoundsBonusDmg + RV.Talents.VenomousWoundsAPMult * stats.AttackPower) * (1f + natureDmgMult);

            float meleeDamageAverage = (chanceGlance * glanceMultiplier + chanceCritWhiteMain * critMultiplier + chanceHitWhiteMain) * meleeDamageRaw;
            float meleeOffDamageAverage = (chanceGlance * glanceMultiplier + chanceCritWhiteOff * critMultiplier + chanceHitWhiteOff) * meleeOffDamageRaw;
            float meleeDamageNormAverage = (chanceGlance * glanceMultiplier + chanceCritWhiteMain * critMultiplier + chanceHitWhiteMain) * meleeDamageNormRaw;
            float meleeOffDamageNormAverage = (chanceGlance * glanceMultiplier + chanceCritWhiteOff * critMultiplier + chanceHitWhiteOff) * meleeOffDamageNormRaw;
            float mainGaucheDmgAvg = (1f - chanceCritYellow) * meleeDamageRaw + chanceCritYellow * meleeDamageRaw * critMultiplier;
            float backstabDamageAverage = (1f - chanceCritBackstab) * backstabDamageRaw + chanceCritBackstab * backstabDamageRaw * (critMultiplier + cPGCritDmgMult);
            float hemoDamageAverage = (1f - chanceCritHemo) * hemoDamageRaw + chanceCritHemo * hemoDamageRaw * (critMultiplier + cPGCritDmgMult);
            float sStrikeDamageAverage = (1f - chanceCritSStrike) * sStrikeDamageRaw + chanceCritSStrike * sStrikeDamageRaw * (critMultiplier + cPGCritDmgMult);
            float mutiDamageAverage = chanceHitMuti * chanceHitMuti * mutiDamageRaw +
                chanceCritMuti * chanceHitMuti * (mutiDamageMainRaw * (critMultiplier + cPGCritDmgMult) + mutiDamageOffRaw) +
                chanceHitMuti * chanceCritMuti * (mutiDamageMainRaw + mutiDamageOffRaw * (critMultiplier + cPGCritDmgMult)) +
                chanceCritMuti * chanceCritMuti * (mutiDamageMainRaw + mutiDamageOffRaw) * (critMultiplier + cPGCritDmgMult);
            float rStrikeDamageAverage = (1f - chanceCritRStrike) * rStrikeDamageRaw + chanceCritRStrike * rStrikeDamageRaw * critMultiplier;
            float ambushDmgAvg = (1f - chanceCritAmbush) * ambushDmgRaw + chanceCritAmbush * ambushDmgRaw * critMultiplier;
            float[] ruptDamageAverage = new float[] {0,
                ((1f - chanceCritYellow) * ruptDamageRaw[1] + chanceCritYellow * ruptDamageRaw[1] * critMultiplier),
                ((1f - chanceCritYellow) * ruptDamageRaw[2] + chanceCritYellow * ruptDamageRaw[2] * critMultiplier),
                ((1f - chanceCritYellow) * ruptDamageRaw[3] + chanceCritYellow * ruptDamageRaw[3] * critMultiplier),
                ((1f - chanceCritYellow) * ruptDamageRaw[4] + chanceCritYellow * ruptDamageRaw[4] * critMultiplier),
                ((1f - chanceCritYellow) * ruptDamageRaw[5] + chanceCritYellow * ruptDamageRaw[5] * critMultiplier)};
            float evisBaseDamageAverage = (1f - chanceCritEvis) * evisBaseDamageRaw + chanceCritEvis * evisBaseDamageRaw * critMultiplier;
            float evisCPDamageAverage = (1f - chanceCritEvis) * evisCPDamageRaw + chanceCritEvis * evisCPDamageRaw * critMultiplier;
            float envenomBaseDamageAverage = (1f - chanceCritYellow) * envenomBaseDamageRaw + chanceCritYellow * envenomBaseDamageRaw * critMultiplier;
            float envenomCPDamageAverage = (1f - chanceCritYellow) * envenomCPDamageRaw + chanceCritYellow * envenomCPDamageRaw * critMultiplier;
            float iPDamageAverage = (1f - chanceCritPoison) * iPDamageRaw + chanceCritPoison * iPDamageRaw * critMultiplierPoison;
            float dPDamageAverage = (1f - chanceCritPoison) * dPDamageRaw + chanceCritPoison * dPDamageRaw * critMultiplierPoison;
            float wPDamageAverage = (1f - chanceCritPoison) * wPDamageRaw + chanceCritPoison * wPDamageRaw * critMultiplierPoison;
            float venomousWoundsAverage = (1f - chanceCritPoison) * venomousWoundsRaw + chanceCritPoison * venomousWoundsRaw * critMultiplierPoison;
            #endregion

            #region Energy Costs
            float ambushEnergyRaw = RV.Ambush.Cost - ambushBSCostReduc;
            //float garrEnergyRaw = RV.Garrote.Cost;
            float backstabEnergyRaw = RV.BS.Cost - ambushBSCostReduc - (talents.GlyphOfBackstab ? chanceCritBackstab * RV.Glyph.BSEnergyOnCrit : 0f);
            float hemoEnergyRaw = RV.Hemo.Cost - hemoCostReduc;
            float sStrikeEnergyRaw = RV.SS.Cost - sSCostReduc;
            float mutiEnergyRaw = RV.Muti.Cost - mutiCostReduc;
            float rSEnergyRaw = RV.RS.Cost;
            float ruptEnergyRaw = RV.Rupt.Cost;
            float evisEnergyRaw = RV.Evis.Cost;
            float envenomEnergyRaw = RV.Envenom.Cost;
            float snDEnergyRaw = RV.SnD.Cost;
            float recupEnergyRaw = RV.Recup.Cost;
            float exposeEnergyRaw = RV.Expose.Cost;

            //[rawCost + ((1/chance_to_land) - 1) * rawCost/5] 
            float energyCostMultiplier = 1f + ((1f / chanceMHNonAvoided) - 1f) * (1 - RV.EnergyReturnOnAvoid);
            float backstabEnergyAverage = backstabEnergyRaw * energyCostMultiplier;
            float hemoEnergyAverage = hemoEnergyRaw * energyCostMultiplier;
            float sStrikeEnergyAverage = sStrikeEnergyRaw * energyCostMultiplier;
            float mutiEnergyAverage = mutiEnergyRaw * energyCostMultiplier;
            float rSEnergyAverage = rSEnergyRaw * energyCostMultiplier;
            float ruptEnergyAverage = ruptEnergyRaw * energyCostMultiplier;
            float evisEnergyAverage = evisEnergyRaw * energyCostMultiplier;
            float envenomEnergyAverage = envenomEnergyRaw * energyCostMultiplier;
            float snDEnergyAverage = snDEnergyRaw * energyCostMultiplier;
            float recupEnergyAverage = recupEnergyRaw * energyCostMultiplier;
            float eAEnergyAverage = exposeEnergyRaw * energyCostMultiplier;
            #endregion

            #region Ability Stats
            RogueAbilityStats mainHandStats = new RogueMHStats()
            {
                DamagePerHit = meleeDamageRaw,
                DamagePerSwing = meleeDamageAverage,
                Weapon = mainHand,
                CritChance = chanceCritWhiteMain,
            };
            RogueAbilityStats offHandStats = new RogueOHStats()
            {
                DamagePerHit = meleeOffDamageRaw,
                DamagePerSwing = meleeOffDamageAverage,
                Weapon = offHand,
                CritChance = chanceCritWhiteOff,
            };
            RogueAbilityStats mainGaucheStats = new RogueMainGaucheStats()
            {
                DamagePerHit = meleeDamageNormRaw,
                DamagePerSwing = mainGaucheDmgAvg,
                Weapon = mainHand,
                CritChance = chanceCritYellow,
            };
            RogueAbilityStats backstabStats = new RogueBackstabStats()
            {
                DamagePerHit = backstabDamageRaw,
                DamagePerSwing = backstabDamageAverage,
                EnergyCost = backstabEnergyAverage,
                CritChance = chanceCritBackstab,
                CPPerSwing = cpPerBackstab,
            };
            RogueAbilityStats hemoStats = new RogueHemoStats()
            {
                DamagePerHit = hemoDamageRaw,
                DamagePerSwing = hemoDamageAverage,
                EnergyCost = hemoEnergyAverage,
                CritChance = chanceCritHemo,
                CPPerSwing = cpPerHemo,
            };
            RogueAbilityStats sStrikeStats = new RogueSStrikeStats()
            {
                DamagePerHit = sStrikeDamageRaw,
                DamagePerSwing = sStrikeDamageAverage,
                EnergyCost = sStrikeEnergyAverage,
                CritChance = chanceCritSStrike,
                CPPerSwing = cpPerSStrike,
            };
            RogueAbilityStats mutiStats = new RogueMutiStats()
            {
                DamagePerHit = mutiDamageRaw,
                DamagePerSwing = mutiDamageAverage,
                EnergyCost = mutiEnergyAverage,
                CritChance = chanceCritMuti,
                CPPerSwing = cpPerMuti,
            };
            RogueAbilityStats rStrikeStats = new RogueRStrikeStats()
            {
                DamagePerHit = rStrikeDamageRaw,
                DamagePerSwing = rStrikeDamageAverage,
                EnergyCost = rSEnergyAverage,
                CritChance = chanceCritRStrike,
                CPPerSwing = cpPerRStrike,
            };
            RogueAbilityStats ruptStats = new RogueRuptStats()
            {
                DamagePerHitArray = new float[] {0,
                    ruptDamageRaw[1],
                    ruptDamageRaw[2],
                    ruptDamageRaw[3],
                    ruptDamageRaw[4],
                    ruptDamageRaw[5]},
                DamagePerSwingArray = new float[] {0,
                    ruptDamageAverage[1],
                    ruptDamageAverage[2],
                    ruptDamageAverage[3],
                    ruptDamageAverage[4],
                    ruptDamageAverage[5]},
                DurationUptime = ruptDurationUptime,
                DurationAverage = ruptDurationAverage,
                DurationPerCP = RV.Rupt.DurationPerCP,
                EnergyCost = ruptEnergyAverage,
            };
            RogueAbilityStats evisStats = new RogueEvisStats()
            {
                DamagePerHit = evisBaseDamageRaw,
                DamagePerSwing = evisBaseDamageAverage,
                DamagePerHitPerCP = evisCPDamageRaw,
                DamagePerSwingPerCP = evisCPDamageAverage,
                EnergyCost = evisEnergyAverage,
                CritChance = chanceCritEvis,
                DamagePerCrit = evisBaseDamageRaw * critMultiplier,
                DamagePerCritPerCP = evisCPDamageRaw * critMultiplier,
            };
            RogueAbilityStats envenomStats = new RogueEnvenomStats()
            {
                DamagePerHit = envenomBaseDamageRaw,
                DamagePerSwing = envenomBaseDamageAverage,
                DamagePerHitPerCP = envenomCPDamageRaw,
                DamagePerSwingPerCP = envenomCPDamageAverage,
                EnergyCost = envenomEnergyAverage,
                CritChance = chanceCritYellow,
                DamagePerCrit = envenomBaseDamageRaw * critMultiplier,
                DamagePerCritPerCP = envenomCPDamageRaw * critMultiplier,
            };
            RogueAbilityStats snDStats = new RogueSnDStats()
            {
                DurationUptime = snDBonusDuration * (1f + snDDurationMult),
                DurationAverage = (RV.SnD.BaseDuration + snDBonusDuration) * (1f + snDDurationMult),
                EnergyCost = snDEnergyAverage,
                DurationPerCP = RV.SnD.DurationPerCP,
            };
            RogueAbilityStats recupStats = new RogueRecupStats()
            {
                DurationUptime = recupBonusDuration,
                DurationAverage = RV.Recup.BaseDuration + recupBonusDuration,
                EnergyCost = recupEnergyAverage,
                DurationPerCP = RV.Recup.DurationPerCP,
            };
            RogueAbilityStats exposeStats = new RogueExposeStats()
            {
                DurationUptime = eABonusDuration,
                DurationAverage = RV.Expose.BaseDuration + eABonusDuration,
                EnergyCost = eAEnergyAverage,
                DurationPerCP = RV.Expose.DurationPerCP,
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
            RogueAbilityStats venomousWoundsStats = new RogueVenomousWoundsStats()
            {
                DamagePerHit = venomousWoundsRaw,
                DamagePerSwing = venomousWoundsAverage,
            };
            #endregion

            #region Rotations
            RogueRotationCalculator rotationCalculator;
            RogueRotationCalculator.RogueRotationCalculation rotationCalculationOptimal;
//            BonusPhysicalDamageMultiplier = character.ActiveBuffs.Contains(Buff.GetBuffByName("Ravage")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Acid Spit")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Brittle Bones")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Blood Frenzy")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Savage Combat")) ? 0f : RV.Talents.SavageCombatMult * talents.SavageCombat,
            #region Assassination
            if (spec == 0)
            {
                rotationCalculator = new RogueRotationCalculatorAss(character, stats, bossOpts, calcOpts,
                    hasteBonus, mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, chanceWhiteMHAvoided, chanceWhiteOHAvoided, chanceMHAvoided, chanceOHAvoided,
                    chanceFinisherAvoided, chancePoisonAvoided, chanceCritYellow * cPonCPGCritChance, (1f - chanceHitMuti * chanceHitMuti) * cPonCPGCritChance, mainHandStats,
                    offHandStats, backstabStats, mutiStats, ruptStats, envenomStats, snDStats, exposeStats, iPStats, dPStats, wPStats, venomousWoundsStats);
                rotationCalculationOptimal = new RogueRotationCalculatorAss.RogueRotationCalculation();

                bool segmentedOptimize = talents.MurderousIntent > 0;
                int numberOfSegments = segmentedOptimize ? 2 : 1;
                float durationMultiplier = 1f;
                if (!calcOpts.ForceCustom)
                {
                    while (numberOfSegments > 0)
                    {
                        if (segmentedOptimize && numberOfSegments == 2) durationMultiplier = 1 - RV.Talents.MurderousIntentThreshold;
                        else if (segmentedOptimize && numberOfSegments == 1) durationMultiplier = RV.Talents.MurderousIntentThreshold;
                        RogueRotationCalculator.RogueRotationCalculation rotationCalculationDPS = new RogueRotationCalculatorAss.RogueRotationCalculation();
                        for (int snDCP = 4; snDCP < 6; snDCP++)
                            for (int finisherCP = 4; finisherCP < 6; finisherCP++)
                                for (int CPG = 0; CPG < 2; CPG++)
                                {
                                    if (CPG == 1 && (!calcOpts.EnableBS || backstabStats.DamagePerSwing == 0)) continue;
                                    for (int ruptCP = 3; ruptCP < 6; ruptCP++)
                                    {
                                        if (ruptCP > 3 && !calcOpts.EnableRupt) continue;
                                        for (int mHPoison = 1; mHPoison < 3; mHPoison++)
                                        {
                                            if (mainHand == null) break;
                                            if ((mHPoison == 1 && !calcOpts.EnableIP) ||
                                                (mHPoison == 2 && !calcOpts.EnableDP)) continue;
                                            for (int oHPoison = 1; oHPoison < 3; oHPoison++)
                                            {
                                                if (offHand == null) break;
                                                if ((oHPoison == 1 && !calcOpts.EnableIP) ||
                                                    (oHPoison == 2 && !calcOpts.EnableDP)) continue;
                                                bool useTotT = false;
                                                RogueRotationCalculatorAss.RogueRotationCalculation rotationCalculation =
                                                    rotationCalculator.GetRotationCalculations(durationMultiplier, CPG, 0, (ruptCP == 3 ? 0 : ruptCP), false, 0, finisherCP, snDCP, mHPoison, oHPoison, useTotT, (int)exposeArmor, PTRMode);
                                                if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
                                                    rotationCalculationDPS = rotationCalculation;
                                            }
                                        }
                                    }
                                }
                        if (numberOfSegments == 2) rotationCalculationOptimal = rotationCalculationDPS;
                        else if (segmentedOptimize) rotationCalculationOptimal += rotationCalculationDPS;
                        else rotationCalculationOptimal = rotationCalculationDPS;
                        numberOfSegments--;
                    }
                }

                numberOfSegments = segmentedOptimize ? 2 : 1;
                durationMultiplier = 1f;
                while (numberOfSegments > 0)
                {
                    if (segmentedOptimize && numberOfSegments == 2) durationMultiplier = 1 - RV.Talents.MurderousIntentThreshold;
                    else if (segmentedOptimize && numberOfSegments == 1) durationMultiplier = RV.Talents.MurderousIntentThreshold;
                    RogueRotationCalculator.RogueRotationCalculation rotationCalculationDPS = new RogueRotationCalculatorAss.RogueRotationCalculation();
                    rotationCalculationDPS = rotationCalculator.GetRotationCalculations(
                        durationMultiplier, calcOpts.CustomCPG, calcOpts.CustomRecupCP, calcOpts.CustomRuptCP, calcOpts.CustomUseRS, calcOpts.CustomFinisher, calcOpts.CustomCPFinisher, calcOpts.CustomCPSnD, calcOpts.CustomMHPoison, calcOpts.CustomOHPoison, calcOpts.CustomUseTotT, (int)exposeArmor, PTRMode);
                    if (numberOfSegments == 2) calc.CustomRotation = rotationCalculationDPS;
                    else if (segmentedOptimize) calc.CustomRotation += rotationCalculationDPS;
                    else calc.CustomRotation = rotationCalculationDPS;
                    numberOfSegments--;
                }
            }
            #endregion
            #region Combat
            else if (spec == 1)
            {
                rotationCalculator = new RogueRotationCalculatorCombat(character, stats, bossOpts, calcOpts, hasteBonus, mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm,
                    chanceWhiteMHAvoided, chanceWhiteOHAvoided, chanceMHAvoided, chanceOHAvoided, chanceFinisherAvoided, chancePoisonAvoided, chanceCritYellow * cPonCPGCritChance,
                    mainHandStats, offHandStats, mainGaucheStats, sStrikeStats, rStrikeStats, ruptStats, evisStats, snDStats, exposeStats, iPStats, dPStats, wPStats);
                rotationCalculationOptimal = new RogueRotationCalculatorCombat.RogueRotationCalculation();

                if (!calcOpts.ForceCustom)
                {
                    RogueRotationCalculator.RogueRotationCalculation rotationCalculationDPS = new RogueRotationCalculatorCombat.RogueRotationCalculation();
                    for (int snDCP = 4; snDCP < 6; snDCP++)
                        for (int finisherCP = 4; finisherCP < 6; finisherCP++)
                            for (int ruptCP = 3; ruptCP < 6; ruptCP++)
                                for (int useRS = 0; useRS < 2; useRS++)
                                {
                                    if (useRS == 1 && (!calcOpts.EnableRS || rStrikeStats.DamagePerSwing == 0)) continue;
                                    for (int mHPoison = 1; mHPoison < 4; mHPoison++)
                                    {
                                        if (!targetPoisonable || mainHand == null) break;
                                        if ((mHPoison == 1 && !calcOpts.EnableIP) ||
                                            (mHPoison == 2 && !calcOpts.EnableDP) ||
                                            (mHPoison == 3 && !calcOpts.EnableWP)) continue;
                                        for (int oHPoison = 1; oHPoison < 4; oHPoison++)
                                        {
                                            if (!targetPoisonable || offHand == null) break;
                                            if ((oHPoison == 1 && !calcOpts.EnableIP) ||
                                                (oHPoison == 2 && !calcOpts.EnableDP) ||
                                                (oHPoison == 3 && !calcOpts.EnableWP)) continue;
                                            bool useTotT = false;
                                            RogueRotationCalculator.RogueRotationCalculation rotationCalculation =
                                                rotationCalculator.GetRotationCalculations(0, 0, 0, (ruptCP == 3 ? 0 : ruptCP), useRS == 1, 0, finisherCP, snDCP, mHPoison, oHPoison, useTotT, (int)exposeArmor, PTRMode);
                                            if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
                                                rotationCalculationDPS = rotationCalculation;
                                        }
                                    }
                                }
                    rotationCalculationOptimal = rotationCalculationDPS;
                }
                calc.CustomRotation = rotationCalculator.GetRotationCalculations(0, calcOpts.CustomCPG, calcOpts.CustomRecupCP, calcOpts.CustomRuptCP, calcOpts.CustomUseRS, calcOpts.CustomFinisher,
                    calcOpts.CustomCPFinisher, calcOpts.CustomCPSnD, calcOpts.CustomMHPoison, calcOpts.CustomOHPoison, calcOpts.CustomUseTotT, (int)exposeArmor, PTRMode);
            }
            #endregion
            #region Subtlety
            else
            {
                rotationCalculator = new RogueRotationCalculatorSubt(character, stats, bossOpts, calcOpts, hasteBonus, mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, chanceWhiteMHAvoided,
                    chanceWhiteOHAvoided, chanceMHAvoided, chanceOHAvoided, chanceFinisherAvoided, chancePoisonAvoided, chanceCritYellow * cPonCPGCritChance, mainHandStats, offHandStats,
                    backstabStats, hemoStats, ruptStats, evisStats, snDStats, recupStats, exposeStats, iPStats, dPStats, wPStats);
                rotationCalculationOptimal = new RogueRotationCalculatorSubt.RogueRotationCalculation();
                bool useHemo = !(character.ActiveBuffs.Contains(Buff.GetBuffByName("Mangle")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Hemorrhage")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Blood Frenzy")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Gore")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Stampede")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Tendon Rip")));

                if (!calcOpts.ForceCustom)
                {
                    RogueRotationCalculator.RogueRotationCalculation rotationCalculationDPS = new RogueRotationCalculatorSubt.RogueRotationCalculation();
                    for (int snDCP = 4; snDCP < 6; snDCP++)
                        for (int finisherCP = 4; finisherCP < 6; finisherCP++)
                            for (int CPG = 0; CPG < 2; CPG++)
                            {
                                if ((CPG == 0 && (!calcOpts.EnableBS || backstabStats.DamagePerSwing == 0)) ||
                                    (CPG == 1 && (!calcOpts.EnableHemo || hemoStats.DamagePerSwing == 0))) continue;
                                for (int ruptCP = 3; ruptCP < 6; ruptCP++)
                                {
                                    if (ruptCP > 3 && !calcOpts.EnableRupt) continue;
                                    for (int recupCP = 3; recupCP < 6; recupCP++)
                                    {
                                        if (recupCP > 3 && !calcOpts.EnableRecup) continue;
                                        for (int mHPoison = 1; mHPoison < 3; mHPoison++)
                                        {
                                            if (!targetPoisonable || mainHand == null) break;
                                            if ((mHPoison == 1 && !calcOpts.EnableIP) ||
                                                (mHPoison == 2 && !calcOpts.EnableDP)) continue;
                                            for (int oHPoison = 1; oHPoison < 3; oHPoison++)
                                            {
                                                if (!targetPoisonable || offHand == null) break;
                                                if ((oHPoison == 1 && !calcOpts.EnableIP) ||
                                                    (oHPoison == 2 && !calcOpts.EnableDP)) continue;
                                                bool useTotT = false;
                                                RogueRotationCalculator.RogueRotationCalculation rotationCalculation =
                                                    rotationCalculator.GetRotationCalculations(0, CPG, (recupCP == 3 ? 0 : recupCP), (ruptCP == 3 ? 0 : ruptCP), useHemo, 0, finisherCP, snDCP, mHPoison, oHPoison, useTotT, (int)exposeArmor, PTRMode);
                                                if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
                                                    rotationCalculationDPS = rotationCalculation;
                                            }
                                        }
                                    }
                                }
                            }
                    rotationCalculationOptimal = rotationCalculationDPS;
                }
                calc.CustomRotation = rotationCalculator.GetRotationCalculations(0, calcOpts.CustomCPG, calcOpts.CustomRecupCP, calcOpts.CustomRuptCP, calcOpts.CustomUseRS, calcOpts.CustomFinisher,
                    calcOpts.CustomCPFinisher, calcOpts.CustomCPSnD, calcOpts.CustomMHPoison, calcOpts.CustomOHPoison, calcOpts.CustomUseTotT, (int)exposeArmor, PTRMode);
            }
            #endregion
            
            calc.HighestDPSRotation = calcOpts.ForceCustom == false ? rotationCalculationOptimal : calc.CustomRotation;
            #endregion

            calc.AvoidedWhiteMHAttacks = chanceWhiteMHAvoided * 100f;
            calc.AvoidedWhiteOHAttacks = chanceWhiteOHAvoided * 100f;
            calc.AvoidedAttacks = chanceMHAvoided * 100f;
            calc.AvoidedFinisherAttacks = chanceFinisherAvoided * 100f;
            calc.AvoidedPoisonAttacks = chancePoisonAvoided * 100f;
            calc.DodgedMHAttacks = chanceMHDodge * 100f;
            calc.ParriedAttacks = chanceParry * 100f;
            calc.MissedAttacks = chanceMiss * 100f;
            calc.CritChanceYellow = chanceCritYellow * 100f;
            calc.CritChanceMHTotal = chanceCritWhiteMainTotal * 100f;
            calc.CritChanceMH = chanceCritWhiteMain * 100f;
            calc.CritChanceOHTotal = chanceCritWhiteOffTotal * 100f;
            calc.CritChanceOH = chanceCritWhiteOff * 100f;
            calc.MainHandSpeed = mainHandSpeed;
            calc.OffHandSpeed = offHandSpeed;
            calc.ArmorMitigation = (1f - modArmor) * 100f;
            calc.Duration = bossOpts.BerserkTimer;

            calc.MainHandStats = mainHandStats;
            calc.OffHandStats = offHandStats;
            calc.MainGaucheStats = mainGaucheStats;
            calc.BackstabStats = backstabStats;
            calc.HemoStats = hemoStats;
            calc.SStrikeStats = sStrikeStats;
            calc.MutiStats = mutiStats;
            calc.RStrikeStats = rStrikeStats;
            calc.RuptStats = ruptStats;
            calc.SnDStats = snDStats;
            calc.EvisStats = evisStats;
            calc.EnvenomStats = envenomStats;
            calc.IPStats = iPStats;
            calc.DPStats = dPStats;
            calc.WPStats = wPStats;
            calc.VenomousWoundsStats = venomousWoundsStats;

            float magicDPS = 0f; // (stats.ShadowDamage + stats.ArcaneDamage) * (1f + chanceCritYellow);
            calc.DPSPoints = calc.HighestDPSRotation.DPS + magicDPS;
            calc.SurvivabilityPoints = stats.Health / 100f;
            calc.OverallPoints = calc.DPSPoints + calc.SurvivabilityPoints;
            return calc;
        }

        private Stats GetCharacterStatsWithTemporaryEffects(Character character, Item additionalItem, /*out WeightedStat[] armorPenetrationUptimes,*/ out WeightedStat[] critRatingUptimes)
        {
            RogueTalents talents = character.RogueTalents;
            #region Spec determination
            int spec;
            int assCounter = 0, combatCounter = 0, subtCounter = 0;
            for (int i = 0; i <= 18; i++) assCounter += int.Parse(talents.ToString()[i].ToString());
            for (int i = 19; i <= 37; i++) combatCounter += int.Parse(talents.ToString()[i].ToString());
            for (int i = 38; i <= 56; i++) subtCounter += int.Parse(talents.ToString()[i].ToString());
            if (assCounter > combatCounter && assCounter > subtCounter) spec = 0;
            else if (combatCounter > subtCounter) spec = 1;
            else spec = 2;
            #endregion

            CalculationOptionsRogue calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            BossOptions bossOpts = character.BossOptions;
            int targetLevel = bossOpts.Level;
            bool targetPoisonable = calcOpts.TargetPoisonable;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);

            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            Stats statsTalents = new Stats()
            {
                BonusAgilityMultiplier = (1f + (spec == 2 ? RV.Mastery.SinisterCallingMult : 0f)) * (1f + RV.LeatherSpecialization) - 1f,
                BonusAttackPowerMultiplier = (1f + (spec == 1 ? RV.Mastery.VitalityAPMult : 0f)) * (1f + RV.Talents.SavageCombatMult * talents.SavageCombat) - 1f,
                BonusCritChance = character.ActiveBuffs.Contains(Buff.GetBuffByName("Rampage")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Leader of the Pack")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Honor Among Thieves")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Terrifying Roar")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Furious Howl")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Elemental Oath")) ? 0f : RV.Talents.HonorAmongThievesCritBonus * talents.HonorAmongThieves,
                BonusDamageMultiplier = RV.Vendetta.DmgMult * talents.Vendetta * (RV.Vendetta.Duration * (talents.GlyphOfVendetta ? 1f + RV.Glyph.VendettaDurationMult : 1f)) / RV.Vendetta.CD +
                    talents.SanguinaryVein * RV.Talents.SanguinaryVein +
                    RV.Mastery.MasterOfSubtletyDmgMult * RV.Mastery.MasterOfSubtletyDuration / (RV.Vanish.CD - RV.Talents.ElusivenessVanishCDReduc * talents.Elusiveness) +
                    (talents.Preparation > 0 ? RV.Mastery.MasterOfSubtletyDmgMult * RV.Mastery.MasterOfSubtletyDuration / (RV.Talents.PreparationCD * talents.Preparation) : 0f),
                BonusPhysicalDamageMultiplier = character.ActiveBuffs.Contains(Buff.GetBuffByName("Ravage")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Acid Spit")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Brittle Bones")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Blood Frenzy")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Savage Combat")) ? 0f : RV.Talents.SavageCombatMult * talents.SavageCombat,
                BonusBleedDamageMultiplier = character.ActiveBuffs.Contains(Buff.GetBuffByName("Mangle")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Hemorrhage")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Blood Frenzy")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Gore")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Stampede")) || character.ActiveBuffs.Contains(Buff.GetBuffByName("Tendon Rip")) ? 0f : RV.Hemo.BleedDmgMult * talents.Hemorrhage,
                PhysicalHit = RV.Talents.PrecisionMult * talents.Precision,
                SpellHit = RV.Talents.PrecisionMult * talents.Precision,
            };

            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems;
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsTalents);

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility - statsRace.Agility) * (1f + statsTotal.BonusAgilityMultiplier)) + statsRace.Agility;
            statsTotal.AttackPower += (statsTotal.Strength - RV.BaseStatCalcReduc / 2) + RV.APperAgi * (statsTotal.Agility - RV.BaseStatCalcReduc) + RV.BaseStatCalcReduc;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - RV.BaseStatCalcReduc) * RV.HPPerStam + RV.BaseStatCalcReduc);
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff;

            float hasteBonus = (1f + StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Rogue)) * (1f + statsTotal.PhysicalHaste) - 1f;
            float speedBonus = (1f + hasteBonus) * (1f + RV.SnD.SpeedBonus) * (1f + (spec == 2 ? RV.Mastery.Executioner + RV.Mastery.ExecutionerPerMast * StatConversion.GetMasteryFromRating(statsTotal.MasteryRating) : 0f)) - 1f;
            float mHSpeed = (character.MainHand == null ? 2 : character.MainHand.Speed);
            float oHSpeed = (character.OffHand == null ? 2 : character.OffHand.Speed);
            float meleeHitInterval = 1f / ((mHSpeed + oHSpeed) / speedBonus);

            //To calculate the Poison hit interval only white attacks are taken into account, IP is assumed on the slowest and DP on the fastest weapon
            float dPPS = bossOpts.BerserkTimer / (Math.Min(mHSpeed, oHSpeed) / speedBonus) * RV.DP.Chance + (spec == 0 ? RV.Mastery.ImprovedPoisonsDPBonus : 0);
            float poisonHitInterval = 1 / (Math.Max(mHSpeed, mHSpeed) * RV.IP.Chance * (1f + RV.Mastery.ImprovedPoisonsIPFreqMult) / RV.IP.NormWeapSpeed + dPPS);
            
            float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
            float spellHitBonus = StatConversion.GetSpellHitFromRating(statsTotal.HitRating) + statsTotal.SpellHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating) + statsTotal.Expertise);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - character.Level] - expertiseBonus);
            float chanceParry = 0f; //Math.Max(0f, StatConversion.WHITE_PARRY_CHANCE_CAP[targetLevel - character.Level] - expertiseBonus);
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[targetLevel - character.Level] - hitBonus);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;
            float chancePoisonAvoided = Math.Max(0f, StatConversion.GetSpellMiss(character.Level - targetLevel, false) - spellHitBonus);

            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating)
                                + StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Rogue)
                                + statsTotal.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - character.Level];
            float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
            float chanceHit = 1f - chanceAvoided;

            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
            triggerIntervals[Trigger.Use] = 0f;
            triggerIntervals[Trigger.MeleeHit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalHit] = meleeHitInterval;
            triggerIntervals[Trigger.MeleeAttack] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalAttack] = meleeHitInterval;
            triggerIntervals[Trigger.MeleeCrit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalCrit] = meleeHitInterval;
            triggerIntervals[Trigger.DoTTick] = 0f;
            triggerIntervals[Trigger.DamageDone] = meleeHitInterval / 2f;
            triggerIntervals[Trigger.DamageOrHealingDone] = meleeHitInterval / 2f; // Need to add Self-heals
            triggerIntervals[Trigger.SpellHit] = poisonHitInterval;
            triggerIntervals[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 4f; // Approximating as 80% chance every 4 seconds. TODO: Actually model this
            triggerChances[Trigger.Use] = 1f;
            triggerChances[Trigger.MeleeHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.PhysicalHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.PhysicalAttack] = triggerChances[Trigger.MeleeAttack] = 1f;
            triggerChances[Trigger.MeleeCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.PhysicalCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.DoTTick] = 1f;
            triggerChances[Trigger.DamageDone] = 1f - chanceAvoided / 2f;
            triggerChances[Trigger.DamageOrHealingDone] = 1f - chanceAvoided / 2f; // Need to add Self-heals
            triggerChances[Trigger.SpellHit] = Math.Max(0f, 1f - chancePoisonAvoided);
            triggerChances[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 0.80f; // Approximating as 80% chance every 4 seconds. TODO: Actually model this

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
                        triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer);
                    statsProcs.Accumulate(effect.Stats._rawSpecialEffectData[0].GetAverageStats(
                        triggerIntervals[effect.Stats._rawSpecialEffectData[0].Trigger],
                        triggerChances[effect.Stats._rawSpecialEffectData[0].Trigger], 1f, bossOpts.BerserkTimer),
                        upTime);
                }
                else if (effect.Stats.MoteOfAnger > 0)
                {
                    // When in effect stats, MoteOfAnger is % of melee hits
                    // When in character stats, MoteOfAnger is average procs per second
                    statsProcs.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(triggerIntervals[effect.Trigger],
                        triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer) / effect.MaxStack;
                }
                else
                {
                    statsProcs.Accumulate(effect.GetAverageStats(triggerIntervals[effect.Trigger],
                        triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer),
                        1f);
                }
            }

            statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon;
            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength + RV.APperAgi * statsProcs.Agility;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * RV.HPPerStam);
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));

            float HighestSecondaryStatValue = statsProcs.HighestSecondaryStat; // how much HighestSecondaryStat to add
            statsProcs.HighestSecondaryStat = 0f; // remove HighestSecondaryStat stat, since it's not needed
            if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating) {
                statsProcs.CritRating += HighestSecondaryStatValue;
            } else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating) {
                statsProcs.HasteRating += HighestSecondaryStatValue;
            } else /*if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating)*/ {
                statsProcs.MasteryRating += HighestSecondaryStatValue;
            }

            //Agility is only used for crit from here on out; we'll be converting Agility to CritRating, 
            //and calculating CritRating separately, so don't add any Agility or CritRating from procs here.
            statsProcs.CritRating = statsProcs.Agility = 0;
            statsTotal.Accumulate(statsProcs);

            //Handle Crit procs
            critRatingUptimes = new WeightedStat[0];
            List<SpecialEffect> tempCritEffects = new List<SpecialEffect>();
            List<float> tempCritEffectIntervals = new List<float>();
            List<float> tempCritEffectChances = new List<float>();
            List<float> tempCritEffectScales = new List<float>();

            foreach (SpecialEffect effect in statsTotal.SpecialEffects(se => triggerIntervals.ContainsKey(se.Trigger) && (se.Stats.CritRating + se.Stats.Agility + se.Stats.HighestStat + se.Stats.Paragon) > 0))
            {
                tempCritEffects.Add(effect);
                tempCritEffectIntervals.Add(triggerIntervals[effect.Trigger]);
                tempCritEffectChances.Add(triggerChances[effect.Trigger]);
                tempCritEffectScales.Add(1f);
            }

            if (tempCritEffects.Count == 0)
            {
                critRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = 1f, Value = 0f } };
            }
            else if (tempCritEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempCritEffects[0];
                float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer) * tempCritEffectScales[0];
                float totalAgi = (effect.Stats.Agility + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                critRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = uptime, Value = 
                        effect.Stats.CritRating + StatConversion.GetCritFromAgility(totalAgi - 10,
                        CharacterClass.Rogue) * StatConversion.RATING_PER_PHYSICALCRIT },
                    new WeightedStat() { Chance = 1f - uptime, Value = 0f }};
            }
            else if (tempCritEffects.Count > 1)
            {
                List<float> tempCritEffectsValues = new List<float>();
                foreach (SpecialEffect effect in tempCritEffects)
                {
                    float totalAgi = (float)effect.MaxStack * (effect.Stats.Agility + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                    tempCritEffectsValues.Add(effect.Stats.CritRating +
                        StatConversion.GetCritFromAgility(totalAgi - 10, CharacterClass.Rogue) *
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
                WeightedStat[] critWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempCritEffects.ToArray(), intervals, chances, offset, tempCritEffectScales.ToArray(), 1f, bossOpts.BerserkTimer, tempCritEffectsValues.ToArray());
                critRatingUptimes = critWeights;
            }
            return statsTotal;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            WeightedStat[] critRatingUptimes;
            return GetCharacterStatsWithTemporaryEffects(character, additionalItem, out critRatingUptimes);
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
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

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats relevantStats = new Stats {
               Agility = stats.Agility,
               Strength = stats.Strength,
               AttackPower = stats.AttackPower,
               CritRating = stats.CritRating,
               HitRating = stats.HitRating,
               Stamina = stats.Stamina,
               HasteRating = stats.HasteRating,
               ExpertiseRating = stats.ExpertiseRating,
               MasteryRating = stats.MasteryRating,
               TargetArmorReduction = stats.TargetArmorReduction,
               WeaponDamage = stats.WeaponDamage,
               BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
               BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
               BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
               BonusDamageMultiplier = stats.BonusDamageMultiplier,
               BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
               BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
               Health = stats.Health,
               ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
               PhysicalHaste = stats.PhysicalHaste,
               PhysicalHit = stats.PhysicalHit,
               PhysicalCrit = stats.PhysicalCrit,
               HighestStat = stats.HighestStat,
               MoteOfAnger = stats.MoteOfAnger,

               BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
               BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
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

               // BossHandler
               SnareRootDurReduc = stats.SnareRootDurReduc,
               FearDurReduc = stats.FearDurReduc,
               StunDurReduc = stats.StunDurReduc,
               MovementSpeed = stats.MovementSpeed,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.DamageOrHealingDone || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.MeleeAttack || effect.Trigger == Trigger.PhysicalAttack || effect.Trigger == Trigger.EnergyOrFocusDropsBelow20PercentOfMax)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        relevantStats.AddSpecialEffect(effect);
                    }
                }
            }

            return relevantStats;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool relevant = (
                    stats.Agility +
                    stats.Strength +
                    stats.AttackPower +
                    stats.CritRating +
                    stats.HitRating +
                    stats.HasteRating +
                    stats.ExpertiseRating +
                    stats.MasteryRating +
                    stats.TargetArmorReduction +
                    stats.WeaponDamage +
                    stats.BonusAgilityMultiplier +
                    stats.BonusAttackPowerMultiplier +
                    stats.BonusCritDamageMultiplier +
                    stats.BonusDamageMultiplier +
                    stats.BonusStaminaMultiplier +
                    stats.BonusStrengthMultiplier +
                    stats.ThreatReductionMultiplier +
                    stats.PhysicalHaste +
                    stats.PhysicalHit +
                    stats.PhysicalCrit +
                    stats.HighestStat +

                    // Set bonuses
                    stats.Rogue_T11_2P +
                    stats.Rogue_T11_4P +

                    stats.BonusPhysicalDamageMultiplier +
                    stats.BonusBleedDamageMultiplier + 
                    stats.SpellHit +
                    stats.SpellCrit +
                    stats.SpellCritOnTarget +

                    // Trinket Procs
                    stats.Paragon +
                    stats.MoteOfAnger +

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
                    stats.BonusFireDamageMultiplier +

                    // BossHandler
                    stats.SnareRootDurReduc +
                    stats.FearDurReduc +
                    stats.StunDurReduc +
                    stats.MovementSpeed
                ) != 0 || (stats.Stamina > 0 && stats.SpellPower == 0);

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use
                    || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.MeleeCrit
                    || effect.Trigger == Trigger.MeleeAttack
                    || effect.Trigger == Trigger.PhysicalHit
                    || effect.Trigger == Trigger.PhysicalCrit
                    || effect.Trigger == Trigger.PhysicalAttack
                    || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone
                    || effect.Trigger == Trigger.DamageOrHealingDone
                    || effect.Trigger == Trigger.SpellHit
                    || effect.Trigger == Trigger.EnergyOrFocusDropsBelow20PercentOfMax) // For Poison Hits
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) { break; }
                }
            }
            return relevant;
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand && item.Type == ItemType.None)
                return false;
            return base.IsItemRelevant(item);
        }
        
        public Stats GetBuffsStats(Character character, CalculationOptionsRogue calcOpts)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

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

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd(("Horn of Winter"));
            character.ActiveBuffsAdd(("Battle Shout"));
            character.ActiveBuffsAdd(("Unleashed Rage"));
            character.ActiveBuffsAdd(("Improved Moonkin Form"));
            character.ActiveBuffsAdd(("Leader of the Pack"));
            character.ActiveBuffsAdd(("Improved Icy Talons"));
            character.ActiveBuffsAdd(("Power Word: Fortitude"));
            character.ActiveBuffsAdd(("Mark of the Wild"));
            character.ActiveBuffsAdd(("Blessing of Kings"));
            character.ActiveBuffsAdd(("Sunder Armor"));
            character.ActiveBuffsAdd(("Faerie Fire"));
            character.ActiveBuffsAdd(("Totem of Wrath"));
            character.ActiveBuffsAdd(("Flask of Endless Rage"));
            character.ActiveBuffsAdd(("Agility Food"));
            character.ActiveBuffsAdd(("Heroism/Bloodlust"));

            if (character.PrimaryProfession == Profession.Alchemy ||
                character.SecondaryProfession == Profession.Alchemy)
                character.ActiveBuffsAdd(("Flask of Endless Rage (Mixology)"));

            // Need to be behind boss
            character.BossOptions.InBack = true;
            character.BossOptions.InBackPerc_Melee = 1.00d;
        }
    }

    public class Knuckles : Item { public Knuckles() { Speed = 2f; } }

    public class ComparisonCalculationsRogue : ComparisonCalculationBase
    {
        private string _name = string.Empty;
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SurvivabilityPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        private Item _item = null;
        public override Item Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance
        {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }

        private bool _equipped = false;
        public override bool Equipped
        {
            get { return _equipped; }
            set { _equipped = value; }
        }

        public override bool PartEquipped { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
        }
    }
}
