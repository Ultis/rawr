using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Rawr.Cat
{
    [Rawr.Calculations.RawrModelInfo("Cat", "Ability_Druid_CatForm", CharacterClass.Druid)]
    public class CalculationsCat : CalculationsBase
    {
        #region Gemming Templates
        //my insides all turned to ash / so slow
        //and blew away as i collapsed / so cold
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for Cats
                //Red
                int[] delicate = { 52082, 52212 }; // Agi

                //Purple
                int[] shifting = { 52096, 52238 }; // Agi/Sta
                int[] accurate = { 52105, 52203 }; // Exp/Hit
                int[] glinting = { 52102, 52220 }; // Agi/Hit
                
                //Blue
                int[] solid = { 52086, 52242 }; // Sta
                
                //Green
                int[] jagged = { 52121, 52223 }; // Crit/Sta
                int[] nimble = { 52120, 52227 }; // Dodge/Hit
                int[] piercing = { 52122, 52228 }; // Crit/Hit
                int[] puissant = { 52126, 52231 }; // Mastery/Sta
                int[] regal = { 52119, 52233 }; // Dodge/Sta
                int[] senseis = { 52128, 52233 }; // Mastery/Hit
                
                //Yellow
                int[] fractured = { 52094, 52219 }; // Mastery
                int[] subtle = { 52090, 52247 }; // Dodge
                
                //Orange
                int[] adept = { 52115, 52204 }; // Agi/Mastery
                int[] deadly = { 52109, 52209 }; // Agi/Crit
                int[] deft = { 52112, 52211 }; // Agi/Haste
                int[] keen = { 52118, 52224 }; // Exp/Mastery
                int[] polished = { 52106, 52229 }; // Agi/Dodge
                int[] resolute = { 52107, 52249 }; // Exp/Dodge

                //Prismatic

                //Meta
                int fleet   = 52289; // 54 Mastery, 8% Runspeed - 2Y
                int chaotic = 52291; // 54 Crit,    3% Crit Dmg - 3R
                int agile   = 68778; // 54 Agility, 3% Crit Dmg - 3R

                int[] metas = { fleet, chaotic, agile };

                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                List<GemmingTemplate> list = new List<GemmingTemplate>();
                for (int tier = 0; tier < 2; tier++)
                {
                    for (int meta = 0; meta < 3; meta++)
                    {
                        list.AddRange(new GemmingTemplate[]
                        {
                            CreateCatGemmingTemplate(tier,	 delicate,   delicate, 	delicate,	delicate,	metas[meta]), 
                            CreateCatGemmingTemplate(tier,	 delicate,   adept, 	glinting,	delicate,	metas[meta]),

                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_pry[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_ddg[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_pry[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_ddg[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = metas[meta], },
                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_pry[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_ddg[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_pry[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_ddg[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_ddg[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_spr[0], MetaId = metas[meta], },

                            //new GemmingTemplate() { Model = "Cat", Group = "Cogwheels", Enabled = false, CogwheelId = cog_spr[0], Cogwheel2Id = cog_has[0], MetaId = metas[meta], },
                        });
                    }
                }

                return list;
            }
        }
        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green Rarity, but no Blue Rarity
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue Rarity (or Green Rarity as set above), but no Purple Rarity
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple Rarity (or Blue Rarity/Green Rarity as set above), but no Jewel
        }
        private const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateCatGemmingTemplate(int tier, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate()
            {
                Model = "Cat",
                Group = (new string[] { "Uncommon", "Rare", "Epic", "Jeweler" })[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta
            };
        }
        #endregion

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelCat();
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
                    "Basic Stats:Mastery Rating",
                    //"Basic Stats:Armor Penetration Rating",
                    //"Basic Stats:Weapon Damage",
                    
                    "Complex Stats:Avoided Attacks",
                    "Complex Stats:Crit Chance",
                    "Complex Stats:Attack Speed",
                    "Complex Stats:Armor Mitigation",
                    
                    "Abilities:Optimal Rotation",
                    //"Abilities:Optimal Rotation DPS",
                    //"Abilities:Custom Rotation DPS",
                    "Abilities:Melee",
                    "Abilities:Mangle",
                    "Abilities:Shred",
                    "Abilities:Ravage",
                    "Abilities:Rake",
                    "Abilities:Rip",
                    "Abilities:Bite",
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
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    //"Custom Rotation DPS",
                    "Health",
                    "Avoided Attacks %",
                    "Avoided Interrupts %",
                    "Nature Resist",
                    "Fire Resist",
                    "Frost Resist",
                    "Shadow Resist",
                    "Arcane Resist",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                        //"Hit Test",
                    };
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
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 160, 0, 224));
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
                }
                return _subPointNameColors;
            }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Leather,
                        ItemType.Idol, 
                        ItemType.Relic,
                        ItemType.Staff,
                        ItemType.TwoHandMace,
                        ItemType.Polearm
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Druid; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationCat(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsCat(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsCat));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsCat calcOpts = serializer.Deserialize(reader) as CalculationOptionsCat;
            return calcOpts;
        }

        private CatRotationCalculator rotationCalculator;
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsCat calculatedStats = new CharacterCalculationsCat();
            if (character == null) { return calculatedStats; }
            CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat;
            if (calcOpts == null) { return calculatedStats; }
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) { return calculatedStats; }
            
            int targetLevel = bossOpts.Level;
            int characterLevel = character.Level;
            float lagVariance = (float)calcOpts.LagVariance / 1000f;
            StatsCat stats = GetCharacterStats(character, additionalItem) as StatsCat;
            int levelDifference = (targetLevel - characterLevel);
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = targetLevel;
            bool maintainMangleBecauseNooneElseIs = !character.ActiveBuffsConflictingBuffContains("Bleed Damage");
            int T11Count;
            character.SetBonusCount.TryGetValue("Stormrider's Battlegarb", out T11Count);
            MangleUsage mangleusage = maintainMangleBecauseNooneElseIs ? MangleUsage.MaintainMangle : MangleUsage.None;
            if (calcOpts.CustomUseMangle && T11Count >= 4) { mangleusage = MangleUsage.Maintain4T11; }
            if (mangleusage != MangleUsage.None) {
                stats.BonusBleedDamageMultiplier = (1f + stats.BonusBleedDamageMultiplier)
                    * (1f + (maintainMangleBecauseNooneElseIs ? 0.30f : 0f)) // if someone else is putting up mangle, don't try to add to it here as it won't stack
                    - 1f;
            }

            #region Basic Chances and Constants
            float modArmor = 1f - StatConversion.GetArmorDamageReduction(character.Level, bossOpts.Armor,
                stats.TargetArmorReduction, stats.ArmorPenetration);
            
            float critMultiplier = 2f * (1f + stats.BonusCritDamageMultiplier);
            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(stats.HasteRating, CharacterClass.Druid);
            float attackSpeed = (1f / (1f + hasteBonus)) / (1f + stats.PhysicalHaste);
            float energyPerSecond = 10f * (1f + hasteBonus);

            float hitBonus = stats.PhysicalHit + StatConversion.GetPhysicalHitFromRating(stats.HitRating, CharacterClass.Druid);
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Druid) + stats.Expertise, CharacterClass.Druid);
            float masteryBonus = StatConversion.GetMasteryFromRating(stats.MasteryRating, CharacterClass.Druid) + 8f;
            stats.NonShredBleedDamageMultiplier = masteryBonus * 0.031f;

            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - 85] - expertiseBonus);
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[targetLevel - 85] - hitBonus);
            float chanceAvoided = chanceMiss + chanceDodge;

            //Crit Chances
            float chanceCritYellow = 0f;
            float chanceCritBite = 0f;
            float chanceCritRip = 0f;
            float chanceCritRake = 0f;
            float chanceGlance = 0f;
            float chanceCritWhite = 0f;

            for (int i = 0; i < stats.TemporaryCritRatingUptimes.Length; i++)
            { //Sum up the weighted chances for each crit value
                WeightedStat iStat = stats.TemporaryCritRatingUptimes[i];

                //Yellow - 2 Roll, so total of X chance to avoid, total of 1 chance to crit and hit when not avoided
                float chanceCritYellowTemp = Math.Min(1f, StatConversion.GetCritFromRating(stats.CritRating + iStat.Value, CharacterClass.Druid)
                    + StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Druid)
                    + stats.PhysicalCrit
                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 85]);
                //float chanceHitYellowTemp = 1f - chanceCritYellowTemp;
                //float cpPerCPG = chanceHitYellow + chanceCritYellow * (1f + stats.CPOnCrit);

                //Bite - Identical to Yellow, with higher crit chance
                float chanceCritBiteTemp = Math.Min(1f, chanceCritYellowTemp + character.DruidTalents.RendAndTear * 0.25f / 3f);
                //float chanceHitBiteTemp = 1f - chanceCritBiteTemp;

                //Bleeds - 1 Roll, no avoidance, total of 1 chance to crit and hit
                float chanceCritRipTemp = Math.Min(1f, chanceCritYellowTemp);
                float chanceCritRakeTemp = Math.Min(1f, chanceCritYellowTemp);

                //White
                float chanceGlanceTemp = StatConversion.WHITE_GLANCE_CHANCE_CAP[targetLevel - 85];
                float chanceCritWhiteTemp = Math.Min(chanceCritYellowTemp, 1f - chanceGlanceTemp - chanceAvoided);
                //float chanceHitWhiteTemp = 1f - chanceCritWhiteTemp - chanceAvoided - chanceGlanceTemp;


                chanceCritYellow += iStat.Chance * chanceCritYellowTemp;
                chanceCritBite += iStat.Chance * chanceCritBiteTemp;
                chanceCritRip += iStat.Chance * chanceCritRipTemp;
                chanceCritRake += iStat.Chance * chanceCritRakeTemp;
                chanceGlance += iStat.Chance * chanceGlanceTemp;
                chanceCritWhite += iStat.Chance * chanceCritWhiteTemp;
            }
            #endregion

            #region Attack Damages
            /* 
            float baseDamage = 55f + (stats.AttackPower / 14f) + stats.WeaponDamage;
            float meleeDamageRaw = (baseDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * modArmor;
            float mangleDamageRaw = (baseDamage * 2f + 566f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusMangleDamageMultiplier) * modArmor;
            float shredDamageRaw = (baseDamage * 2.25f + 666f + stats.BonusShredDamage) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusShredDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * modArmor;
            float rakeDamageRaw = (176f + stats.AttackPower * 0.01f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
            float rakeDamageDot = (1074f + stats.AttackPower * 0.18f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRakeDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier) * ((9f + stats.BonusRakeDuration) / 9f);
            float ripDamageRaw = (3006f + stats.AttackPower * 0.3f + (stats.BonusRipDamagePerCPPerTick * 5f * 6f)) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusRipDamageMultiplier) * (1f + stats.BonusBleedDamageMultiplier);
            float biteBaseDamageRaw = 190f * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;
            float biteCPDamageRaw = (290f + stats.AttackPower * 0.07f) * (1f + stats.BonusPhysicalDamageMultiplier) * (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFerociousBiteDamageMultiplier) * modArmor;

            float meleeDamageAverage = chanceGlance * meleeDamageRaw * glanceMultiplier +
                                        chanceCritWhite * meleeDamageRaw * critMultiplier +
                                        chanceHitWhite * meleeDamageRaw;
            float mangleDamageAverage = (1f - chanceCritYellow) * mangleDamageRaw + chanceCritYellow * mangleDamageRaw * critMultiplier;
            float shredDamageAverage = (1f - chanceCritYellow) * shredDamageRaw + chanceCritYellow * shredDamageRaw * critMultiplier;
            float rakeDamageAverage = ((1f - chanceCritYellow) * rakeDamageRaw + chanceCritYellow * rakeDamageRaw * critMultiplier) + ((1f - chanceCritRake) * rakeDamageDot + chanceCritRake * rakeDamageDot * critMultiplierBleed);
            float ripDamageAverage = ((1f - chanceCritRip) * ripDamageRaw + chanceCritRip * ripDamageRaw * critMultiplierBleed);
            float biteBaseDamageAverage = (1f - chanceCritBite) * biteBaseDamageRaw + chanceCritBite * biteBaseDamageRaw * critMultiplier;
            float biteCPDamageAverage = (1f - chanceCritBite) * biteCPDamageRaw + chanceCritBite * biteCPDamageRaw * critMultiplier;

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
            float mangleEnergyRaw = 45f - stats.MangleCatCostReduction;
            float shredEnergyRaw = 60f - stats.ShredCostReduction;
            float rakeEnergyRaw = 40f - stats.RakeCostReduction;
            float ripEnergyRaw = 30f - stats.RipCostReduction;
            float biteEnergyRaw = 35f; //Assuming no wasted energy
            float roarEnergyRaw = 25f;

            //[rawCost + ((1/chance_to_land) - 1) * rawCost/5] 
            float cpgEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * 0.2f;
            float finisherEnergyCostMultiplier = 1f + ((1f / chanceNonAvoided) - 1f) * (1f - stats.FinisherEnergyOnAvoid);
            float mangleEnergyAverage = mangleEnergyRaw * cpgEnergyCostMultiplier;
            float shredEnergyAverage = shredEnergyRaw * cpgEnergyCostMultiplier;
            float rakeEnergyAverage = rakeEnergyRaw * cpgEnergyCostMultiplier;
            float ripEnergyAverage = ripEnergyRaw * finisherEnergyCostMultiplier;
            float biteEnergyAverage = biteEnergyRaw * finisherEnergyCostMultiplier;
            float roarEnergyAverage = roarEnergyRaw;
            #endregion

            #region Ability Stats
            CatAbilityStats meleeStats = new CatMeleeStats()
            {
                DamagePerHit = meleeDamageRaw,
                DamagePerSwing = meleeDamageAverage,
            };
            CatAbilityStats mangleStats = new CatMangleStats()
            {
                DamagePerHit = mangleDamageRaw,
                DamagePerSwing = mangleDamageAverage,
                DurationUptime = mangleDurationUptime,
                DurationAverage = mangleDurationAverage,
                EnergyCost = mangleEnergyAverage,
            };
            CatAbilityStats shredStats = new CatShredStats()
            {
                DamagePerHit = shredDamageRaw,
                DamagePerSwing = shredDamageAverage,
                EnergyCost = shredEnergyAverage,
            };
            CatAbilityStats rakeStats = new CatRakeStats()
            {
                DamagePerHit = rakeDamageRaw + rakeDamageDot,
                DamagePerSwing = rakeDamageAverage,
                DurationUptime = rakeDurationUptime,
                DurationAverage = rakeDurationAverage,
                EnergyCost = rakeEnergyAverage,
            };
            CatAbilityStats ripStats = new CatRipStats()
            {
                DamagePerHit = ripDamageRaw,
                DamagePerSwing = ripDamageAverage,
                DurationUptime = ripDurationUptime,
                DurationAverage = ripDurationAverage,
                EnergyCost = ripEnergyAverage,
            };
            CatAbilityStats biteStats = new CatBiteStats()
            {
                DamagePerHit = biteBaseDamageRaw,
                DamagePerSwing = biteBaseDamageAverage,
                DamagePerHitPerCP = biteCPDamageRaw,
                DamagePerSwingPerCP = biteCPDamageAverage,
                EnergyCost = biteEnergyAverage,
            };
            CatAbilityStats roarStats = new CatRoarStats()
            {
                DurationUptime = roarBonusDuration,
                DurationAverage = 9f + roarBonusDuration,
                EnergyCost = roarEnergyAverage,
                DurationPerCP = 5f,
            };
            */
            #endregion

            #region Rotations
            CatAbilityBuilder abilities = new CatAbilityBuilder(stats, character.DruidTalents,
                character.MainHand == null ? 0.75f : 
                ((character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f) / character.MainHand.Speed,
                attackSpeed, modArmor, hasteBonus, critMultiplier, chanceAvoided, chanceCritWhite, chanceCritYellow, chanceCritYellow,
                chanceCritYellow, chanceCritYellow, chanceCritRake, chanceCritRip, chanceCritBite, chanceGlance);
            rotationCalculator = new CatRotationCalculator(abilities, bossOpts.BerserkTimer, mangleusage);
            var optimalRotation = rotationCalculator.GetOptimalRotation(); //TODO: Check for 4T11, maintain it if so
            calculatedStats.Abilities = abilities;
            calculatedStats.HighestDPSRotation = optimalRotation;

            //CatRotationCalculator rotationCalculator = new CatRotationCalculator(stats, calcOpts.Duration, cpPerCPG,
            //    maintainMangle, berserkDuration, attackSpeed,
            //    true,
            //    character.DruidTalents.GlyphOfShred, chanceAvoided, chanceCritYellow * stats.BonusCPOnCrit,
            //    cpgEnergyCostMultiplier, stats.ClearcastOnBleedChance, meleeStats, mangleStats, shredStats,
            //    rakeStats, ripStats, biteStats, roarStats);
            //CatRotationCalculator.CatRotationCalculation rotationCalculationDPS = new CatRotationCalculator.CatRotationCalculation();

            //for (int roarCP = 1; roarCP < 6; roarCP++)
            //    for (int biteCP = 0; biteCP < 6; biteCP++)
            //        for (int useRake = 0; useRake < 2; useRake++)
            //            for (int useShred = 0; useShred < 2; useShred++)
            //                for (int useRip = 0; useRip < 2; useRip++)
            //                {
            //                    CatRotationCalculator.CatRotationCalculation rotationCalculation =
            //                        rotationCalculator.GetRotationCalculations(
            //                        useRake == 1, useShred == 1, useRip == 1, biteCP, roarCP);
            //                    if (rotationCalculation.DPS > rotationCalculationDPS.DPS)
            //                        rotationCalculationDPS = rotationCalculation;
            //                }

            //calculatedStats.HighestDPSRotation = rotationCalculationDPS;
            //calculatedStats.CustomRotation = rotationCalculator.GetRotationCalculations(
            //    calcOpts.CustomUseRake, calcOpts.CustomUseShred, calcOpts.CustomUseRip, calcOpts.CustomCPFerociousBite, calcOpts.CustomCPSavageRoar);

            //if (character.DruidTalents.GlyphOfShred && rotationCalculationDPS.ShredCount > 0)
            //{
            //    ripStats.DurationUptime += 6f;
            //    ripStats.DurationAverage += 6f;
            //}
            //ripStats.DamagePerHit *= ripStats.DurationUptime / 12f;
            //ripStats.DamagePerSwing *= ripStats.DurationUptime / 12f;
            #endregion

            calculatedStats.AvoidedAttacks = chanceAvoided * 100f;
            calculatedStats.DodgedAttacks = chanceDodge * 100f;
            calculatedStats.MissedAttacks = chanceMiss * 100f;
            calculatedStats.CritChance = chanceCritYellow * 100f;
            calculatedStats.AttackSpeed = attackSpeed;
            calculatedStats.ArmorMitigation = (1f - modArmor) * 100f;
            calculatedStats.Duration = bossOpts.BerserkTimer;


            float magicDPS = (stats.ShadowDamage + stats.ArcaneDamage + stats.NatureDamage + stats.FireDamage + stats.FrostDamage) * (1f + chanceCritYellow);
            float abomDPS = (stats.MoteOfAnger * abilities.MeleeStats.DamageAverage);
            calculatedStats.DPSPoints = calculatedStats.HighestDPSRotation.DPS + magicDPS + abomDPS;
            calculatedStats.SurvivabilityPoints = stats.Health / 100f;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints + calculatedStats.SurvivabilityPoints;
            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsCat calcOpts = character.CalculationOptions as CalculationOptionsCat ?? new CalculationOptionsCat();
            DruidTalents talents = character.DruidTalents;
            BossOptions bossOpts = character.BossOptions;

            bool hasCritBuff = false;
            foreach (Buff buff in character.ActiveBuffs)
            {
                if (buff.Group == "Critical Strike Chance")
                {
                    hasCritBuff = true;
                    break;
                }
            }

            StatsCat statsTotal = new StatsCat()
            {
                BonusAgilityMultiplier = Character.ValidateArmorSpecialization(character, ItemType.Leather) ? 0.05f : 0f,
                BonusAttackPowerMultiplier = (1f + 0.25f) * (1f + talents.HeartOfTheWild * 0.1f / 3f) - 1f,
                //BonusBleedDamageMultiplier = (character.ActiveBuffsConflictingBuffContains("Bleed Damage") ? 0f : 0.3f),

                MovementSpeed = 0.15f * talents.FeralSwiftness,
                RavageCritChanceOnTargetsAbove80Percent = 0.25f * talents.PredatoryStrikes,
                FurySwipesChance = 0.05f * talents.FurySwipes,
                CPOnCrit = 0.5f * talents.PrimalFury,
                FerociousBiteDamageMultiplier = 0.05f* talents.FeralAggression,
                EnergyOnTigersFury = 20f * talents.KingOfTheJungle,
                FreeRavageOnFeralChargeChance = 0.5f * talents.Stampede,
                PhysicalCrit = (hasCritBuff ? 0f : 0.05f * talents.LeaderOfThePack)
                                + (talents.MasterShapeshifter == 1 ? 0.04f : 0f),
                MaxEnergyOnTigersFuryBerserk = 10f * talents.PrimalMadness,
                RipRefreshChanceOnFerociousBiteOnTargetsBelow25Percent = 0.5f * talents.BloodInTheWater,
                ShredDamageMultiplier = talents.RendAndTear * 0.2f / 3f,

                BonusBerserkDuration = (talents.Berserk > 0 ? 15f + (talents.GlyphOfBerserk ? 5f : 0f) : 0f),
                MangleDamageMultiplier = talents.GlyphOfMangle ? 0.1f : 0f,
                SavageRoarDamageMultiplierIncrease = talents.GlyphOfSavageRoar ? 0.05f : 0f,
                FeralChargeCatCooldownReduction = talents.GlyphOfFeralCharge ? 2f : 0f,
                FerociousBiteMaxExtraEnergyReduction = talents.GlyphOfFerociousBite ? 35f : 0f,
            };

            #region Set Bonuses
            int T11Count;
            character.SetBonusCount.TryGetValue("Stormrider's Battlegarb", out T11Count);
            if (T11Count >= 2) {
                statsTotal.BonusDamageMultiplierRakeTick = (1f + statsTotal.BonusDamageMultiplierRakeTick) * (1f + 0.10f) - 1f;
                statsTotal.BonusDamageMultiplierLacerate = (1f + statsTotal.BonusDamageMultiplierLacerate) * (1f + 0.10f) - 1f;
            }
            if (T11Count >= 4) {
                statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.MangleCatAttack,
                    new Stats() { BonusAttackPowerMultiplier = 0.01f, },
                    30, 0, 1f, 3));
                //statsTotal.BonusSurvivalInstinctsDurationMultiplier = (1f + statsTotal.BonusSurvivalInstinctsDurationMultiplier) * (1f + 0.50f) - 1f;
            }
            int T12Count;
            character.SetBonusCount.TryGetValue("Obsidian Arborweave Battlegarb", out T12Count);
            if (T12Count >= 2) {
                statsTotal.MangleDamageMultiplier = (1f + statsTotal.MangleDamageMultiplier) * (1f + 0.10f) - 1f;
                statsTotal.ShredDamageMultiplier = (1f + statsTotal.ShredDamageMultiplier) * (1f + 0.10f) - 1f;
            }
            if (T12Count >= 4) {
                // Assume that all Finishing Moves are used at 5 combo points
                SpecialEffect primary = new SpecialEffect(Trigger.Berserk, new Stats(), rotationCalculator.BerserkDuration(), rotationCalculator.BerserkCooldown());
                // This is the Inner Eye stacking buff.
                SpecialEffect secondary = new SpecialEffect(Trigger.FinishingMove,
                    new StatsCat() { BonusBerserkDuration = 2f, },
                    0, 0, 1f);
                primary.Stats.AddSpecialEffect(secondary);
                statsTotal.AddSpecialEffect(primary);
            }
            #endregion

            statsTotal.Accumulate(BaseStats.GetBaseStats(character.Level, character.Class, character.Race, BaseStats.DruidForm.Cat));
            statsTotal.Accumulate(GetItemStats(character, additionalItem));
            AccumulateBuffsStats(statsTotal, character.ActiveBuffs);

            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsTotal.AttackPower += statsTotal.Strength * 2f + statsTotal.Agility * 2f - 20f; //-20 to account for the first 20 str and first 20 agi only giving 1ap each
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - 20f) * 10f + 20f);
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff;

            int targetLevel = bossOpts.Level;
            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Druid);
            hasteBonus = (1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) - 1f;
            float meleeHitInterval = 1f / ((1f + hasteBonus) + 1f / (3.5f / hasteBonus));
            float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
            float expertiseBonus = StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating, CharacterClass.Druid) + statsTotal.Expertise, CharacterClass.Druid);
            float chanceDodge = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[targetLevel - 85] - expertiseBonus);
            float chanceParry = 0f;
            float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[targetLevel - 85] - hitBonus);
            float chanceAvoided = chanceMiss + chanceDodge + chanceParry;

            float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating)
                                + StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.Druid)
                                + statsTotal.PhysicalCrit
                                + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - 85];
            float chanceCrit = rawChanceCrit * (1f - chanceAvoided);
            float chanceHit = 1f - chanceAvoided;
            bool usesMangle = (!character.ActiveBuffsContains("Mangle") && !character.ActiveBuffsContains("Trauma"));

            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
            triggerIntervals[Trigger.Use] = 0f;
            triggerIntervals[Trigger.MeleeAttack] = meleeHitInterval;
            triggerIntervals[Trigger.MeleeHit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalHit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalAttack] = meleeHitInterval;
            triggerIntervals[Trigger.MeleeCrit] = meleeHitInterval;
            triggerIntervals[Trigger.PhysicalCrit] = meleeHitInterval;
            triggerIntervals[Trigger.DoTTick] = 1.5f;
            triggerIntervals[Trigger.DamageDone] = meleeHitInterval / 2f;
            triggerIntervals[Trigger.DamageOrHealingDone] = meleeHitInterval / 2f; // Need to Add Self-Heals
            triggerIntervals[Trigger.RakeTick] = 3f;
            if (usesMangle) {
                triggerIntervals[Trigger.MangleCatHit] = 60f;
                triggerIntervals[Trigger.MangleCatAttack] = 60f;
            }
            triggerIntervals[Trigger.MangleCatOrShredHit] = usesMangle ? 3.76f : 3.87f;
            triggerIntervals[Trigger.MangleCatOrShredOrInfectedWoundsHit] = triggerIntervals[Trigger.MangleCatOrShredHit] / ((talents.InfectedWounds > 0) ? 2f : 1f);
            triggerIntervals[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 4f; // doing 80% chance every 4 seconds per Astry
            triggerIntervals[Trigger.FinishingMove] = rotationCalculator.RipBiteUptime();
            triggerIntervals[Trigger.Berserk] = rotationCalculator.berserkUptime;
            triggerChances[Trigger.Use] = 1f;
            triggerChances[Trigger.MeleeAttack] = 1f;
            triggerChances[Trigger.MeleeHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.PhysicalHit] = Math.Max(0f, chanceHit);
            triggerChances[Trigger.PhysicalAttack] = 1f;
            triggerChances[Trigger.MeleeCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.PhysicalCrit] = Math.Max(0f, chanceCrit);
            triggerChances[Trigger.DoTTick] = 1f;
            triggerChances[Trigger.DamageDone] = 1f - chanceAvoided / 2f;
            triggerChances[Trigger.DamageOrHealingDone] = 1f - chanceAvoided / 2f; // Need to Add Self-Heals
            triggerChances[Trigger.RakeTick] = 1f;
            if (usesMangle) {
                triggerChances[Trigger.MangleCatAttack] = 1f;
                triggerChances[Trigger.MangleCatHit] = chanceHit;
            }
            triggerChances[Trigger.MangleCatOrShredHit] = chanceHit;
            triggerChances[Trigger.MangleCatOrShredOrInfectedWoundsHit] = chanceHit;
            triggerChances[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 0.80f; // doing 80% chance every 4 seconds per Astry

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
                } else if (effect.Stats.MoteOfAnger > 0) {
                    // When in effect stats, MoteOfAnger is % of melee hits
                    // When in character stats, MoteOfAnger is average procs per second
                    statsProcs.MoteOfAnger = effect.Stats.MoteOfAnger * effect.GetAverageProcsPerSecond(triggerIntervals[effect.Trigger],
                        triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer) / effect.MaxStack;
                } else {
                    statsProcs.Accumulate(effect.GetAverageStats(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer));
                }
            }

            statsProcs.Agility += statsProcs.HighestStat + statsProcs.Paragon;
            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility * 2f;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
            statsProcs.Armor += 2f * statsProcs.Agility;
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
            if (statsProcs.HighestSecondaryStat > 0)
            {
                if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating) {
                    statsProcs.CritRating += statsProcs.HighestSecondaryStat; // this will be invalidated after this, but I'm at least putting it in for now
                } else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating) {
                    statsProcs.HasteRating += statsProcs.HighestSecondaryStat;
                } else if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating) {
                    statsProcs.MasteryRating += statsProcs.HighestSecondaryStat;
                }
                statsProcs.HighestSecondaryStat = 0;
            }


            //Agility is only used for crit from here on out; we'll be converting Agility to CritRating, 
            //and calculating CritRating separately, so don't add any Agility or CritRating from procs here.
            statsProcs.CritRating = statsProcs.Agility = 0;
            statsTotal.Accumulate(statsProcs);

            //Handle Crit procs
            statsTotal.TemporaryCritRatingUptimes = new WeightedStat[0];
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
                statsTotal.TemporaryCritRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = 1f, Value = 0f } };
            }
            else if (tempCritEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempCritEffects[0];
                float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], 1f, bossOpts.BerserkTimer) * tempCritEffectScales[0];
                float totalAgi = (float)effect.MaxStack * (effect.Stats.Agility + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                statsTotal.TemporaryCritRatingUptimes = new WeightedStat[] { new WeightedStat() { Chance = uptime, Value = 
                            effect.Stats.CritRating + StatConversion.GetCritFromAgility(totalAgi,
                            CharacterClass.Druid) * StatConversion.RATING_PER_PHYSICALCRIT },
                        new WeightedStat() { Chance = 1f - uptime, Value = 0f }};
            }
            else if (tempCritEffects.Count > 1)
            {
                List<float> tempCritEffectsValues = new List<float>();
                foreach (SpecialEffect effect in tempCritEffects)
                {
                    float totalAgi = (float)effect.MaxStack * (effect.Stats.Agility + effect.Stats.HighestStat + effect.Stats.Paragon) * (1f + statsTotal.BonusAgilityMultiplier);
                    tempCritEffectsValues.Add(effect.Stats.CritRating +
                        StatConversion.GetCritFromAgility(totalAgi, CharacterClass.Druid) *
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
                WeightedStat[] critWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempCritEffects.ToArray(), intervals, chances, offset,
                    tempCritEffectScales.ToArray(), 1f, bossOpts.BerserkTimer, tempCritEffectsValues.ToArray());
                statsTotal.TemporaryCritRatingUptimes = critWeights;
            }

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                /*case "White Combat Table":
                    CharacterCalculationsCat calcs = (CharacterCalculationsCat)GetCharacterCalculations(character);
                    float[] ct = null;//calcs.MeleeStats.CombatTable;
                    return new ComparisonCalculationBase[]
                    {
                        new ComparisonCalculationCat() { Name = "Miss", OverallPoints = ct[0], DPSPoints = ct[0]},
                        new ComparisonCalculationCat() { Name = "Dodge", OverallPoints = ct[1], DPSPoints = ct[1]},
                        new ComparisonCalculationCat() { Name = "Parry", OverallPoints = ct[2], DPSPoints = ct[2]},
                        new ComparisonCalculationCat() { Name = "Glance", OverallPoints = ct[3], DPSPoints = ct[3]},
                        new ComparisonCalculationCat() { Name = "Hit", OverallPoints = ct[4], DPSPoints = ct[4]},
                        new ComparisonCalculationCat() { Name = "Crit", OverallPoints = ct[5], DPSPoints = ct[5]},
                    };
                    */
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand ||
                (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Idol && item.Type != ItemType.Relic) ||
                item.Stats.SpellPower > 0) 
                return false;
            foreach (var effect in item.Stats.SpecialEffects(s => s.Stats.SpellPower > 0))
                return false;
            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff, Character character) {
            if (buff != null
                && !string.IsNullOrEmpty(buff.SetName)
                && buff.SetName == "Stormrider's Battlegarb"
                && buff.SetName == "Obsidian Arborweave Battlegarb")
            { return true; }
            return base.IsBuffRelevant(buff, character);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
                {
                    Agility = stats.Agility,
                    Strength = stats.Strength,
                    AttackPower = stats.AttackPower,
                    CritRating = stats.CritRating,
                    HitRating = stats.HitRating,
                    Stamina = stats.Stamina,
                    HasteRating = stats.HasteRating,
                    ExpertiseRating = stats.ExpertiseRating,
                    MasteryRating = stats.MasteryRating,
                    ArmorPenetration = stats.ArmorPenetration,
                    TargetArmorReduction = stats.TargetArmorReduction,
                    WeaponDamage = stats.WeaponDamage,
                    BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                    BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                    BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                    BonusDamageMultiplier = stats.BonusDamageMultiplier,
                    BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                    BonusHealthMultiplier = stats.BonusHealthMultiplier,
                    BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                    BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    Health = stats.Health,
                    ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                    PhysicalHaste = stats.PhysicalHaste,
                    PhysicalHit = stats.PhysicalHit,
                    BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                    PhysicalCrit = stats.PhysicalCrit,
                    ArcaneDamage = stats.ArcaneDamage,
                    ShadowDamage = stats.ShadowDamage,
                    HighestStat = stats.HighestStat,
                    Paragon = stats.Paragon,
                    MoteOfAnger = stats.MoteOfAnger,
                    BonusDamageMultiplierRakeTick = stats.BonusDamageMultiplierRakeTick,

                    ArcaneResistance = stats.ArcaneResistance,
                    NatureResistance = stats.NatureResistance,
                    FireResistance = stats.FireResistance,
                    FrostResistance = stats.FrostResistance,
                    ShadowResistance = stats.ShadowResistance,
                    ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                    NatureResistanceBuff = stats.NatureResistanceBuff,
                    FireResistanceBuff = stats.FireResistanceBuff,
                    FrostResistanceBuff = stats.FrostResistanceBuff,
                    ShadowResistanceBuff = stats.ShadowResistanceBuff,

                    SnareRootDurReduc = stats.SnareRootDurReduc,
                    FearDurReduc = stats.FearDurReduc,
                    StunDurReduc = stats.StunDurReduc,
                    MovementSpeed = stats.MovementSpeed,
                };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.PhysicalAttack || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.MangleCatHit || effect.Trigger == Trigger.MangleCatAttack || effect.Trigger == Trigger.RakeTick
                    || effect.Trigger == Trigger.MangleCatOrShredHit || effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.DamageOrHealingDone
                    || effect.Trigger == Trigger.EnergyOrFocusDropsBelow20PercentOfMax)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool relevant = (stats.Agility + stats.ArmorPenetration + stats.TargetArmorReduction + stats.AttackPower + stats.PhysicalCrit +
                stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritDamageMultiplier +
                stats.BonusDamageMultiplier + stats.BonusWhiteDamageMultiplier +
                stats.BonusStaminaMultiplier + stats.BonusStrengthMultiplier + stats.CritRating + stats.ExpertiseRating +
                stats.HasteRating + stats.MasteryRating + stats.Health + stats.HitRating +
                stats.Strength + stats.WeaponDamage + 
                stats.PhysicalHit + stats.MoteOfAnger +
                stats.PhysicalHaste +
                stats.ThreatReductionMultiplier + stats.ArcaneDamage + stats.ShadowDamage +
                stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance + stats.BonusBleedDamageMultiplier + stats.Paragon +
                stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff + stats.HighestStat +
                stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.BonusPhysicalDamageMultiplier + stats.BonusDamageMultiplierRakeTick +
                stats.SnareRootDurReduc + stats.FearDurReduc + stats.StunDurReduc + stats.MovementSpeed +
                stats.FrostResistanceBuff + stats.ShadowResistanceBuff) > 0 || (stats.Stamina > 0 && stats.SpellPower == 0);

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit || effect.Trigger == Trigger.MeleeAttack
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.PhysicalAttack || effect.Trigger == Trigger.RakeTick
                    || effect.Trigger == Trigger.MangleCatHit || effect.Trigger == Trigger.MangleCatAttack || effect.Trigger == Trigger.MangleCatOrShredHit 
                    || effect.Trigger == Trigger.MangleCatOrShredOrInfectedWoundsHit || effect.Trigger == Trigger.DamageOrHealingDone
                    || effect.Trigger == Trigger.FinishingMove || effect.Trigger == Trigger.EnergyOrFocusDropsBelow20PercentOfMax)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

#if FALSE
        public Stats GetBuffsStats(Character character, CalculationOptionsCat calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            List<Buff> buffGroup = new List<Buff>();

            #region Maintenance Auto-Fixing
            // Removes the Sunder Armor if you are maintaining it yourself
            // Also removes Acid Spit and Expose Armor
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.CustomUseMangle)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Sunder Armor"));
                buffGroup.Add(Buff.GetBuffByName("Expose Armor"));
                buffGroup.Add(Buff.GetBuffByName("Faerie Fire"));
                buffGroup.Add(Buff.GetBuffByName("Corrosive Spit"));
                buffGroup.Add(Buff.GetBuffByName("Tear Armor"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }
            #endregion

            StatsCat statsBuffs = new StatsCat();
            statsBuffs.Accumulate(GetBuffsStats(character.ActiveBuffs, character.SetBonusCount));

            foreach (Buff b in removedBuffs) { character.ActiveBuffsAdd(b); }
            foreach (Buff b in addedBuffs) { character.ActiveBuffs.Remove(b); }

            return statsBuffs;
        }
        private static void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup)
            {
                if (character.ActiveBuffs.Remove(b)) { removedBuffs.Add(b); }
            }
        }
#endif
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Horn of Winter");
            character.ActiveBuffsAdd("Unleashed Rage");
            character.ActiveBuffsAdd("Leader of the Pack");
            character.ActiveBuffsAdd("Improved Icy Talons");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Mark of the Wild");
            character.ActiveBuffsAdd("Faerie Fire");
            character.ActiveBuffsAdd("Flask of the Winds");
            character.ActiveBuffsAdd("Agility Food");
            character.ActiveBuffsAdd("Heroism/Bloodlust");

            character.DruidTalents.GlyphOfSavageRoar = true;
            character.DruidTalents.GlyphOfShred = true;
            character.DruidTalents.GlyphOfRip = true;
            character.DruidTalents.GlyphOfTigersFury = true;
            character.DruidTalents.GlyphOfFeralCharge = true;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Mangle");
                _relevantGlyphs.Add("Glyph of Shred");
                _relevantGlyphs.Add("Glyph of Rip");
                _relevantGlyphs.Add("Glyph of Berserk");
                _relevantGlyphs.Add("Glyph of Savage Roar");
                _relevantGlyphs.Add("Glyph of Tiger's Fury");
                _relevantGlyphs.Add("Glyph of Ferocious Bite");
                _relevantGlyphs.Add("Glyph of Feral Charge");
            }
            return _relevantGlyphs;
        }
    }

    public class ComparisonCalculationCat : ComparisonCalculationBase
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
