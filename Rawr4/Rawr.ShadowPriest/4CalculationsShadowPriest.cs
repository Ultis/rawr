using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ShadowPriest.Spells;
using System.Windows.Media;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase
    {    
        #region started
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    //Meta
                    int chaotic = 0;

                    // [0] uncommon
                    // [1] perfect uncommon
                    // [2] rare
                    // [3] epic
                    // [4] jewelcrafting

                    //Red
                    //Blue
                    //Yellow
                    //Purple
                    //Green
                    //Orange
              
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    //AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon", false, runed[0], royal[0], reckless[0], quick[0], dazzling[0], rigid[0], veiled[0], lambent[0], chaotic);
                }
             return _defaultGemmingTemplates;
            }
        }

        //0
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
        //1
        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    //_subPointNameColors.Add("Burst DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 0, 0, 255));
                }
                return _subPointNameColors;
            }
        }
        //2
        private string[] _customChartNames = {};
        public override string[] CustomChartNames
        {
            get
            {
                return _customChartNames;
            }
        }
         //3
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
					"Basic Stats:Hit+Spirit",
					"Basic Stats:Spell Power",
					"Basic Stats:Crit",
					"Basic Stats:Haste",
                    "Basic Stats:Mastery",
                    "Simulation:Rotation",
                    "Simulation:Castlist",
                    "Simulation:DPS",
//                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
                    "Shadow:Imp. Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Shadowfiend",
                    "Shadow:Mind Spike",
                    "Shadow:Mind Sear",
                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
                     
                };
                return _characterDisplayCalculationLabels;
            }
        }
        //4
        public ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }
        //5
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;

            Stats statsRace = BaseStats.GetBaseStats(character);
            //Stats statsItems = GetItemStats(character, additionalItem);
            //Stats statsBuffs = GetBuffsStats(character, calcOpts);
            //Stats statsTalents = GetTalentStats(character.ShamanTalents);

            Stats statsTotal = statsRace; //+ statsItems + statsBuffs + statsTalents;

            return statsTotal;
        }
        //6
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsShadowPriest calcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;
            if (calcOpts == null) calcOpts = new CalculationOptionsShadowPriest();
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();
            Stats stats = GetCharacterStats(character, additionalItem);

            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            calculatedStats.BasicStats = stats;
            calculatedStats.LocalCharacter = character;

            return calculatedStats;
        }
        //7
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }  
        //8
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]{
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Dagger,
                        ItemType.Wand,
                        ItemType.OneHandMace,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }
        //9
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        //10
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }
        #endregion
        //11
        public override CharacterClass TargetClass { get { return CharacterClass.Priest; } }
        //12-CataPass Needed
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Health = stats.Health,
                Resilience = stats.Resilience,
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellHaste = stats.SpellHaste,
                HasteRating = stats.HasteRating,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                PriestInnerFire = stats.PriestInnerFire,
                MovementSpeed = stats.MovementSpeed,
                SWPDurationIncrease = stats.SWPDurationIncrease,
                BonusMindBlastMultiplier = stats.BonusMindBlastMultiplier,
                MindBlastCostReduction = stats.MindBlastCostReduction,
                ShadowWordDeathCritIncrease = stats.ShadowWordDeathCritIncrease,
                WeakenedSoulDurationDecrease = stats.WeakenedSoulDurationDecrease,
                DevouringPlagueBonusDamage = stats.DevouringPlagueBonusDamage,
                MindBlastHasteProc = stats.MindBlastHasteProc,
                PriestDPS_T9_2pc = stats.PriestDPS_T9_2pc,
                PriestDPS_T9_4pc = stats.PriestDPS_T9_4pc,
                PriestDPS_T10_2pc = stats.PriestDPS_T10_2pc,
                PriestDPS_T10_4pc = stats.PriestDPS_T10_4pc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                ManaRestore = stats.ManaRestore,
                SpellsManaReduction = stats.SpellsManaReduction,
                HighestStat = stats.HighestStat,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                ShadowDamage = stats.ShadowDamage,
                ValkyrDamage = stats.ValkyrDamage,

                /*ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                TimbalsProc = stats.TimbalsProc,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ExtractOfNecromanticPowerProc = stats.ExtractOfNecromanticPowerProc,
                */

                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Agility = stats.Agility,
                ArcaneResistance = stats.ArcaneResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistance = stats.FireResistance,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistance = stats.FrostResistance,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistance = stats.NatureResistance,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistance = stats.ShadowResistance,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
            };

            return s;
        }
        //13-CataPass Needed
        public override bool HasRelevantStats(Stats stats)
        {
            #region Yes
            bool Yes = (
                stats.Intellect + stats.Mana + stats.Spirit + stats.Mp5 + stats.SpellPower
                + stats.SpellShadowDamageRating + stats.CritRating
                + stats.SpellCrit + stats.HitRating + stats.SpellHit + stats.SpellCritOnTarget
                + stats.SpellHaste + stats.HasteRating

                + stats.BonusSpiritMultiplier
                + stats.BonusIntellectMultiplier + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier + stats.BonusDamageMultiplier
                + stats.BonusShadowDamageMultiplier + stats.BonusHolyDamageMultiplier
                + stats.BonusDiseaseDamageMultiplier
                + stats.PriestInnerFire + stats.MovementSpeed

                + stats.SWPDurationIncrease + stats.BonusMindBlastMultiplier
                + stats.MindBlastCostReduction + stats.ShadowWordDeathCritIncrease
                + stats.WeakenedSoulDurationDecrease
                + stats.DevouringPlagueBonusDamage + stats.MindBlastHasteProc
                + stats.PriestDPS_T9_2pc + stats.PriestDPS_T9_4pc
                + stats.PriestDPS_T10_2pc + stats.PriestDPS_T10_4pc
                + stats.ManaRestoreFromBaseManaPPM + stats.BonusSpellCritMultiplier
                + stats.ManaRestore + stats.SpellsManaReduction + stats.HighestStat
                + stats.ArcaneDamage + stats.FireDamage + stats.FrostDamage
                + stats.HolyDamage + stats.NatureDamage + stats.ShadowDamage
                + stats.ValkyrDamage

                /*+ stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse5Min
                + stats.SpellPowerFor10SecOnCast_15_45 + stats.SpellPowerFor10SecOnHit_10_45
                + stats.SpellHasteFor10SecOnCast_10_45 + stats.TimbalsProc
                + stats.PendulumOfTelluricCurrentsProc + stats.ExtractOfNecromanticPowerProc*/
            ) > 0;
            #endregion
            #region Maybe
            bool Maybe = (
                stats.Stamina + stats.Health + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility
                + stats.ArcaneResistance + stats.ArcaneResistanceBuff
                + stats.FireResistance + stats.FireResistanceBuff
                + stats.FrostResistance + stats.FrostResistanceBuff
                + stats.NatureResistance + stats.NatureResistanceBuff
                + stats.ShadowResistance + stats.ShadowResistanceBuff
            ) > 0;
            #endregion
            #region No
            bool No = (
                stats.Strength + stats.AttackPower
                + stats.ArmorPenetration + stats.ArmorPenetrationRating
                + stats.TargetArmorReduction
                + stats.ExpertiseRating
                + stats.Dodge + stats.DodgeRating
                + stats.Parry + stats.ParryRating
                + stats.Defense + stats.DefenseRating
            ) > 0;
            #endregion
            return Yes || (Maybe && !No);
        }

    }

    public static class Constants
    {
        // Source: http://bobturkey.wordpress.com/2010/09/28/priest-base-mana-pool-and-mana-regen-coefficient-at-85/
        public static float BaseMana = 20590;
    }
}
