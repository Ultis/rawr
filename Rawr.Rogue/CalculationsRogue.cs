using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Rawr.Rogue
{
    [Calculations.RawrModelInfoAttribute("Rogue", "Ability_Rogue_SliceDice", Character.CharacterClass.Rogue)]
    public class CalculationsRogue : CalculationsBase
    {
        public CalculationsRogue()
        {
            SetupDisplayCalculationLabels();
            SetupRelevantItemTypes();
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = new CalculationOptionsPanelRogue();
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel; }
        }

        private string[] _characterDisplayCalculationLabels;
        public override string[] CharacterDisplayCalculationLabels
        {
            get { return _characterDisplayCalculationLabels; }
        }

        private Dictionary<string, Color> _subPointNameColors = new Dictionary<string, Color> { { "DPS", Color.Red } };
        public override Dictionary<string, Color> SubPointNameColors
        {
            get { return _subPointNameColors; }
        }

        private string[] _customChartNames = new[] { "Combat Table" };
        public override string[] CustomChartNames
        {
            get { return _customChartNames; }
        }

        private List<Item.ItemType> _relevantItemTypes;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get { return _relevantItemTypes; }
        }

        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Rogue; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationsRogue();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRogue();
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            var s = new XmlSerializer(typeof (CalculationOptionsRogue));
            var sr = new StringReader(xml);
            var calcOpts = s.Deserialize(sr) as CalculationOptionsRogue;
            return calcOpts;
        }

        /// <summary>
        /// Calculate damage output
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        /// Much of this code is based on Aldriana's RogueCalc
        /// 
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            var calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            Stats stats = GetCharacterStats(character, additionalItem);
            var calculatedStats = new CharacterCalculationsRogue();
            calculatedStats.BasicStats = stats;

            var missChance = GetMissChance(character, stats);
            var mhExpertise = CalcExpertise(character, stats, character.MainHand);
            var ohExpertise = CalcExpertise(character, stats, character.OffHand);
            var mhDodgeChance = CalcDodgeChance(mhExpertise);
            var ohDodgeChance = CalcDodgeChance(ohExpertise);
            var mhCrit = CalcCrit(character, stats, character.MainHand);
            var ohCrit = CalcCrit(character, stats, character.OffHand);
            var probMHHit = 1f - missChance / 100f - mhDodgeChance / 100f;
            var probOHHit = 1f - missChance / 100f - ohDodgeChance / 100f;
            var totalArmor = calcOpts.TargetArmor - stats.ArmorPenetration;
            var damageReduction = 1f - (totalArmor / (totalArmor + 10557.5f));
            var totalHaste = CalcTotalHaste(character, stats);
            var bonusWhiteCritDmg = 1f + stats.BonusCritMultiplier;
            var probPoison = (.83f + .05f*character.RogueTalents.MasterPoisoner)*(.2f + .02f*character.RogueTalents.ImprovedPoisons);
            
            var cpg = ComboPointGenerator.Get(character);

            // cycle stuff
            
            #region SnD - Doesn't appear to affect calculations.  TODO: Remove or fix?? 
            float sndLength = 6f + 3f*calcOpts.DPSCycle['s'];
            sndLength += stats.BonusSnDDuration;
            sndLength *= 1f + 0.15f*character.RogueTalents.ImprovedSliceAndDice;
            #endregion

            var ruthlessnessCP = .2f*character.RogueTalents.Ruthlessness;
            var numCPG = calcOpts.DPSCycle.TotalComboPoints - 2f*ruthlessnessCP;
            var finisher = Finishers.Get(calcOpts);
            var energyRegen = CalcEnergyRegen(character);
            var cycleTime = (numCPG * cpg.EnergyCost + 25f + finisher.EnergyCost) / energyRegen;

            // MH
            var mhWhite = 0f;
            var ohWhite = 0f;
            var mhAttacks = 0f;
            var avgMHDmg = 0f;
            var ohHits = 0f;

            if (character.MainHand != null)
            {
                avgMHDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage*2)/2.0f;
                avgMHDmg += (stats.AttackPower/14.0f)*character.MainHand.Speed;

                mhAttacks = totalHaste/character.MainHand.Speed;

                mhWhite = avgMHDmg*mhAttacks*probMHHit;
                mhWhite = (1f - mhCrit/100f)*mhWhite + (mhCrit/100f)*(mhWhite*(2f*bonusWhiteCritDmg));
                mhWhite *= damageReduction;
            }

            // OH
            var ohAttacks = 0f;
            if (character.OffHand != null)
            {
                var avgOHDmg = (character.OffHand.MinDamage + character.OffHand.MaxDamage + stats.WeaponDamage*2)/2.0f;
                avgOHDmg += (stats.AttackPower/14.0f)*character.OffHand.Speed;
                avgOHDmg *= (0.25f + character.RogueTalents.DualWieldSpecialization*0.1f);

                ohAttacks = totalHaste/character.OffHand.Speed;
                ohHits = ohAttacks*probOHHit;

                energyRegen += (.2f*3f*character.RogueTalents.CombatPotency)*ohHits;

                ohWhite = avgOHDmg*ohHits;
                ohWhite = (1f - ohCrit/100f)*ohWhite + (ohCrit/100f)*(ohWhite*(2f*bonusWhiteCritDmg));
                ohWhite *= damageReduction;
            }



            #region CPG Damage

            var cpgDPS = 0f;
            if (character.MainHand != null)
            {
                float avgCPGDmg;
                var bonusCPGCrit = 0f;
                var bonusCPGDmgMult = 1f;
                var bonusCPGCritDmgMult = 2f;

                if (cpg.Name == "mutilate" && character.OffHand != null)
                {
                    bonusCPGCrit += 5f*character.RogueTalents.PuncturingWounds;
                    bonusCPGCritDmgMult *= (1f + .06f*character.RogueTalents.Lethality);
                    bonusCPGDmgMult *= (1f + 0.04f*character.RogueTalents.Opportunity);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage)/2f + 121.5f;
                    avgCPGDmg += stats.AttackPower/14f*1.7f;
                    avgCPGDmg += (character.OffHand.MinDamage + character.OffHand.MaxDamage)/2f + 121.5f;
                    avgCPGDmg += stats.AttackPower/14f*1.7f;
                    avgCPGDmg *= 1.5f;
                }
                else if (cpg.Name == "backstab")
                {
                    bonusCPGDmgMult *= (1f + .02f*character.RogueTalents.Aggression);
                    bonusCPGDmgMult *= (1f + .1f*character.RogueTalents.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + 0.04f*character.RogueTalents.Opportunity);
                    bonusCPGCrit += 10f*character.RogueTalents.PuncturingWounds;
                    bonusCPGCritDmgMult *= (1f + .06f*character.RogueTalents.Lethality);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage)/2f;
                    avgCPGDmg += stats.AttackPower/14f*1.7f;
                    avgCPGDmg *= 1.5f;
                    avgCPGDmg += 255f;
                }
                else if (cpg.Name == "hemo")
                {
                    bonusCPGDmgMult *= (1f + .1f*character.RogueTalents.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + stats.BonusCPGDamage);
                    bonusCPGCritDmgMult *= (1f + .06f*character.RogueTalents.Lethality);

                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage)/2f;
                    avgCPGDmg += stats.AttackPower/14f*2.4f;
                    avgCPGDmg *= 1.1f;
                }
                else
                {
                    // sinister strike
                    avgCPGDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage)/2f;
                    avgCPGDmg += stats.AttackPower/14f*2.4f;
                    avgCPGDmg += 98f; // TBC max rank

                    bonusCPGDmgMult *= (1f + .02f*character.RogueTalents.Aggression);
                    bonusCPGDmgMult *= (1f + .1f*character.RogueTalents.SurpriseAttacks);
                    bonusCPGDmgMult *= (1f + stats.BonusCPGDamage);
                    bonusCPGCritDmgMult *= (1f + .06f*character.RogueTalents.Lethality);
                }

                avgCPGDmg *= bonusCPGDmgMult;

                var cpgCrit = mhCrit + bonusCPGCrit;

                avgCPGDmg = (1f - cpgCrit/100f)*avgCPGDmg + (cpgCrit/100f)*(avgCPGDmg*bonusCPGCritDmgMult);

                cpgDPS = avgCPGDmg*numCPG/cycleTime;
                cpgDPS *= damageReduction;
            }

            #endregion

            #region Finisher Damage

            var finisherDPS = 0f;
            if (character.MainHand != null)
            {
                float finisherDmg;
            
                if (calcOpts.DPSCycle['r'] > 0)
                {
                    switch (calcOpts.DPSCycle['r'])
                    {
                        case 5:
                            finisherDmg = 4f*(stats.AttackPower*.01f + 81f);
                            break;
                        case 4:
                            finisherDmg = 5f*(stats.AttackPower*0.02f + 92f);
                            break;
                        case 3:
                            finisherDmg = 6f*(stats.AttackPower*0.03f + 103f);
                            break;
                        case 2:
                            finisherDmg = 7f*(stats.AttackPower*0.03f + 114f);
                            break;
                        default:
                            finisherDmg = 8f*(stats.AttackPower*0.03f + 125f);
                            break;
                    }

                    finisherDmg *= (1f + .1f*character.RogueTalents.SerratedBlades)*(1f + stats.BonusBleedDamageMultiplier);
                    finisherDmg *= (1f - missChance/100f);
                    if (character.RogueTalents.SurpriseAttacks < 1)
                        finisherDmg *= (1f - mhDodgeChance/100f);
                    finisherDPS = finisherDmg/cycleTime;
                }
                else if (calcOpts.DPSCycle['e'] > 0)
                {
                    var evisMod = stats.AttackPower*calcOpts.DPSCycle['e']*.03f;
                    var evisMin = 245f + (calcOpts.DPSCycle['e'] - 1f)*185f + evisMod;
                    var evisMax = 365f + (calcOpts.DPSCycle['e'] - 1f)*185f + evisMod;

                    finisherDmg = (evisMin + evisMax)/2f;
                    finisherDmg *= (1f + 0.05f*character.RogueTalents.ImprovedEviscerate);
                    finisherDmg *= (1f + 0.02f*character.RogueTalents.Aggression);
                    finisherDmg = finisherDmg*(1f - (mhCrit/100f)) + (finisherDmg*2f)*(mhCrit/100f);
                    finisherDmg *= (1f - (missChance/100f));
                    if (character.RogueTalents.SurpriseAttacks < 1)
                        finisherDmg *= (1f - (mhDodgeChance/100f));
                    finisherDmg *= damageReduction;
                    finisherDPS = finisherDmg/cycleTime;
                }
            }

            #endregion

            #region Sword Spec Damage

            var ssHits = 0f;

            // main hand
            if (character.MainHand != null && character.MainHand.Type == Item.ItemType.OneHandSword)
            {
                ssHits += mhAttacks*0.01f*character.RogueTalents.SwordSpecialization*probMHHit;

                // CPG
                ssHits += (numCPG/cycleTime)*0.01f*character.RogueTalents.SwordSpecialization*probMHHit;

                // finishers
                ssHits += 1f/cycleTime*0.01f*character.RogueTalents.SwordSpecialization*probMHHit;
            }

            // offhand
            if (character.OffHand != null && character.OffHand.Type == Item.ItemType.OneHandSword)
            {
                ssHits += ohAttacks*0.01f*character.RogueTalents.SwordSpecialization*probOHHit;
            }

            var ssDPS = (ssHits*avgMHDmg)*(1 - mhCrit/100f) + (ssHits*avgMHDmg*2f*bonusWhiteCritDmg)*(mhCrit/100f);
            ssDPS *= damageReduction;

            #endregion

            #region WF Damage

            var wfDPS = 0f;
            if (character.MainHand != null && stats.WindfuryAPBonus > 0)
            {
                var wfHits = mhAttacks*probMHHit*.2f*probMHHit;
                wfHits += ssHits*.2f*probMHHit;

                var avgWFDmg = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage*2)/2.0f;
                avgWFDmg += (stats.AttackPower + stats.WindfuryAPBonus)/14f*character.MainHand.Speed;
                avgWFDmg = avgWFDmg*(1f - mhCrit/100f) + avgMHDmg*2f*(mhCrit/100f);

                wfDPS = avgWFDmg*wfHits;
                wfDPS *= damageReduction;
            }

            #endregion
          
     
            #region Poison DPS

            var poisonDPS = 0f;
            

            if (character.MainHand != null && stats.WindfuryAPBonus == 0f)
            {
                // no WF, consider the main hand poison
                if (calcOpts.TempMainHandEnchant == "Deadly Poison" )
                {
                    poisonDPS += 180f*character.RogueTalents.VilePoisons*.04f/12f;
                }
                else if (calcOpts.TempMainHandEnchant == "Instant Poison")
                {
                    poisonDPS += ohHits*probPoison*170f*(1f + character.RogueTalents.VilePoisons*0.04f);
                }
            }
            if (character.OffHand != null)
            {
                if (calcOpts.TempOffHandEnchant == "Deadly Poison")
                {
                    poisonDPS += 180f*(1f + character.RogueTalents.VilePoisons*.04f)/12f;
                }
                else if (calcOpts.TempOffHandEnchant == "Instant Poison")
                {
                    poisonDPS += ohHits*probPoison*170f*(1f + character.RogueTalents.VilePoisons*0.04f);
                }
            }

            #endregion

            var whiteDPS = mhWhite + ohWhite;
            calculatedStats.WhiteDPS = whiteDPS + ssDPS;
            calculatedStats.CPGDPS = cpgDPS;
            calculatedStats.FinisherDPS = finisherDPS;
            calculatedStats.WindfuryDPS = wfDPS;
            calculatedStats.SwordSpecDPS = ssDPS;
            calculatedStats.PoisonDPS = poisonDPS;
            calculatedStats.DPSPoints = whiteDPS + cpgDPS + finisherDPS + wfDPS + ssDPS + poisonDPS;
            calculatedStats.OverallPoints = calculatedStats.DPSPoints;

            calculatedStats.Misc["missChance"] = missChance;  //for testing until it can be refactored

            return calculatedStats;
        }

        public static float CalcEnergyRegen(Character character)
        {
            var energyRegen = 10f;
            if (character.RogueTalents.AdrenalineRush > 0)
            {
                energyRegen += .5f;
            }
            return energyRegen;
        }

        private static float CalcTotalHaste(Character character, Stats stats)
        {
            var sndHaste = .3f;
            sndHaste *= (1f + stats.BonusSnDHaste);

            var totalHaste = 1f;
            totalHaste *= (1f + sndHaste)*(1f + (stats.HasteRating*RogueConversions.HasteRatingToHaste)/100);
            totalHaste *= (1f + .2f*15f/120f*character.RogueTalents.BladeFlurry);
            return totalHaste;
        }

        public static float CalcCrit(Character character, Stats stats, Item weapon)
        {
            var crit = stats.PhysicalCrit + stats.CritRating*RogueConversions.CritRatingToCrit;
            if (weapon != null && ( weapon.Type == Item.ItemType.Dagger || weapon.Type == Item.ItemType.FistWeapon))
                crit += character.RogueTalents.CloseQuartersCombat;
            return crit;
        }

        public static float CalcDodgeChance(float mhExpertise)
        {
            var mhDodgeChance = 6.5f - .25f*mhExpertise;

            if (mhDodgeChance < 0f)
                mhDodgeChance = 0f;
            return mhDodgeChance;
        }

        public static float CalcExpertise(Character character, Stats stats, Item weapon)
        {
            var baseExpertise = character.RogueTalents.WeaponExpertise*5f + stats.Expertise + stats.ExpertiseRating*RogueConversions.ExpertiseRatingToExpertise;

            if (character.Race == Character.CharacterRace.Human)
            {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandSword || weapon.Type == Item.ItemType.OneHandMace))
                    baseExpertise += 5f;
            }

            return baseExpertise;
        }

        public static float GetMissChance(Character character, Stats stats)
        {
            var missChance = 28f;
            missChance -= character.RogueTalents.Precision + stats.PhysicalHit + stats.HitRating*RogueConversions.HitRatingToHit;
            return missChance < 0f ? 0f : missChance;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character.Race);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            // buffs from DPSWarr
            //Add Expose Weakness since it's not listed as an AP buff
            if (statsBuffs.ExposeWeakness > 0) statsBuffs.AttackPower += 200f;

            //Mongoose
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 2673)
            {
                statsBuffs.Agility += 120f*((40f*(1f/(60f/character.MainHand.Speed))/6f));
                statsBuffs.HasteRating += (15.76f*2f)*((40f*(1f/(60f/character.MainHand.Speed))/6f));
            }
            if (character.OffHand != null && character.OffHandEnchant != null && character.OffHandEnchant.Id == 2673)
            {
                statsBuffs.Agility += 120f*((40f*(1f/(60f/character.OffHand.Speed))/6f));
                statsBuffs.HasteRating += (15.76f*2f)*((40f*(1f/(60f/character.OffHand.Speed))/6f));
            }

            //Executioner
            if (character.MainHand != null && character.MainHandEnchant != null && character.MainHandEnchant.Id == 3225)
            {
                statsBuffs.ArmorPenetration += 840f*((40f*(1f/(60f/character.MainHand.Speed))/6f));
            }

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

            //TalentTree tree = character.AllTalents;

            var agiBase = (float) Math.Floor(statsRace.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var agiBonus = (float) Math.Floor(statsGearEnchantsBuffs.Agility*(1 + statsRace.BonusAgilityMultiplier));
            var strBase = (float) Math.Floor(statsRace.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var strBonus = (float) Math.Floor(statsGearEnchantsBuffs.Strength*(1 + statsRace.BonusStrengthMultiplier));
            var staBase = (float) Math.Floor(statsRace.Stamina*(1 + statsRace.BonusStaminaMultiplier));
            var staBonus = (float) Math.Floor(statsGearEnchantsBuffs.Stamina*(1 + statsRace.BonusStaminaMultiplier));

            var statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier)*(1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)*(1 + character.RogueTalents.Deadliness*0.02f)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier)*(1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)*(1 + character.RogueTalents.Vitality*0.01f)*(1 + character.RogueTalents.SinisterCalling*0.03f)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier)*(1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier)*(1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)*(1 + character.RogueTalents.Vitality*0.02f)) - 1;

            statsTotal.Agility = (float) Math.Floor(agiBase*(1f + statsTotal.BonusAgilityMultiplier)) + (float) Math.Floor(agiBonus*(1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float) Math.Floor(strBase*(1f + statsTotal.BonusStrengthMultiplier)) + (float) Math.Floor(strBonus*(1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float) Math.Floor(staBase*(1f + statsTotal.BonusStaminaMultiplier)) + (float) Math.Floor(staBonus*(1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float) Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina - staBase)*10f))));

            statsTotal.AttackPower = (statsTotal.Agility + statsTotal.Strength + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            //statsTotal.AttackPower = statsRace.AttackPower + ((statsTotal.Agility - agiBase) + (statsTotal.Strength - strBase) + statsGearEnchantsBuffs.AttackPower) * (1f + statsTotal.BonusAttackPowerMultiplier);
            //statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + ((statsTotal.Strength - strBase) * 1) + ((statsTotal.Agility - agiBase) * 1)) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.PhysicalHit = character.RogueTalents.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.Expertise += character.RogueTalents.WeaponExpertise*5.0f;
            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;

            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;

            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration;
            switch (character.RogueTalents.SerratedBlades)
            {
                case 3:
                    statsTotal.ArmorPenetration += 560;
                    break;
                case 2:
                    statsTotal.ArmorPenetration += 373;
                    break;
                case 1:
                    statsTotal.ArmorPenetration += 186;
                    break;
            }

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.PhysicalCrit = statsRace.PhysicalCrit;
            statsTotal.PhysicalCrit += (statsTotal.Agility - (statsRace.Agility*(1f + statsTotal.BonusAgilityMultiplier)))*RogueConversions.AgilityToCrit;
            statsTotal.PhysicalCrit += character.RogueTalents.Malice*1f;
            //statsTotal.CritRating += statsBuffs.LotPCritRating;

            statsTotal.Dodge += statsTotal.Agility*RogueConversions.AgilityToDodge;
            statsTotal.Dodge += character.RogueTalents.LightningReflexes;

            statsTotal.Parry += character.RogueTalents.Deflection;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;

            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;

            // T4 bonuses
            statsTotal.BonusSnDDuration = statsGearEnchantsBuffs.BonusSnDDuration;
            statsTotal.CPOnFinisher = statsGearEnchantsBuffs.CPOnFinisher;

            // T5 bonuses
            statsTotal.BonusEvisEnvenomDamage = statsGearEnchantsBuffs.BonusEvisEnvenomDamage;
            statsTotal.BonusFreeFinisher = statsGearEnchantsBuffs.BonusFreeFinisher;

            // T6 bonuses
            statsTotal.BonusCPGDamage = statsGearEnchantsBuffs.BonusCPGDamage;
            statsTotal.BonusSnDHaste = statsGearEnchantsBuffs.BonusSnDHaste;

            return statsTotal;
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
            return new Stats
                       {
                           Stamina = stats.Stamina,
                           Agility = stats.Agility,
                           Strength = stats.Strength,
                           AttackPower = stats.AttackPower,
                           BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                           BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                           BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                           BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                           Health = stats.Health,
                           CritRating = stats.CritRating,
                           HitRating = stats.HitRating,
                           HasteRating = stats.HasteRating,
                           ExpertiseRating = stats.ExpertiseRating,
                           ArmorPenetration = stats.ArmorPenetration,
                           WeaponDamage = stats.WeaponDamage,
                           BonusCritMultiplier = stats.BonusCritMultiplier,
                           WindfuryAPBonus = stats.WindfuryAPBonus,
                           MongooseProc = stats.MongooseProc,
                           MongooseProcAverage = stats.MongooseProcAverage,
                           MongooseProcConstant = stats.MongooseProcConstant,
                           ExecutionerProc = stats.ExecutionerProc,
                           BonusSnDDuration = stats.BonusSnDDuration,
                           CPOnFinisher = stats.CPOnFinisher,
                           BonusEvisEnvenomDamage = stats.BonusEvisEnvenomDamage,
                           BonusFreeFinisher = stats.BonusFreeFinisher,
                           BonusCPGDamage = stats.BonusCPGDamage,
                           BonusSnDHaste = stats.BonusSnDHaste,
                           BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier
                       };
        }

        /////// ** Don't think these are needed anymore, but not posative. 
        //public static void LoadTalentSpec(Character c, string talentSpec) {
        //    string talentCode = "";

        //    switch (talentSpec) {
        //        case "Combat Swords (20/41/0)":
        //            // http://www.worldofwarcraft.com/info/classes/rogue/talents.html?tal=0053201054000000000000233050020050150023211510000000000000000000000
        //            talentCode = "0053201054000000000000233050020050150023211510000000000000000000000";
        //            break;
        //        case "Combat Fist/Sword (16/45/0)":
        //            talentCode = "0053201050000000000000233050020050155023210510000000000000000000000";
        //            break;
        //        case "Combat Daggers (15/41/5)":
        //            talentCode = "0052031040000000000000233050020550100023211510500000000000000000000";
        //            break;
        //        case "Trispec Hemo Swords (11/28/22)":
        //            talentCode = "0053201000000000000000053050020050150020000000502530002300110000000";
        //            break;
        //        case "Trispec Hemo Deadliness (11/21/29)":
        //            talentCode = "0053201000000000000000053050020050100000000000502530030301210400000";
        //            break;
        //        case "Mutilate (41/20/0)":
        //            talentCode = "0053231055021025010510053050020050000000000000000000000000000000000";
        //            break;
        //        default:
        //            break;
        //    }

        //    LoadTalentCode(c, talentCode);
        //}

        //public static void LoadTalentCode(Character c, string talentCode) {
        //    if (talentCode == null || talentCode.Length != 67) return;

        //    c.RogueTalents.ImprovedEviscerate = int.Parse(talentCode.Substring(0, 1));
        //    c.RogueTalents.RemorselessAttacks = int.Parse(talentCode.Substring(1, 1));
        //    c.RogueTalents.Malice = int.Parse(talentCode.Substring(2, 1));
        //    c.RogueTalents.Ruthlessness = int.Parse(talentCode.Substring(3, 1));
        //    c.RogueTalents.Murder = int.Parse(talentCode.Substring(4, 1));
        //    c.RogueTalents.PuncturingWounds = int.Parse(talentCode.Substring(5, 1));
        //    c.RogueTalents.RelentlessStrikes = int.Parse(talentCode.Substring(6, 1));
        //    c.RogueTalents.ImprovedExposeArmor = int.Parse(talentCode.Substring(7, 1));
        //    c.RogueTalents.Lethality = int.Parse(talentCode.Substring(8, 1));
        //    c.RogueTalents.VilePoisons = int.Parse(talentCode.Substring(9, 1));
        //    c.RogueTalents.ImprovedPoisons = int.Parse(talentCode.Substring(10, 1));
        //    c.RogueTalents.FleetFooted = int.Parse(talentCode.Substring(11, 1));
        //    c.RogueTalents.ColdBlood = int.Parse(talentCode.Substring(12, 1));
        //    c.RogueTalents.ImprovedKidneyShot = int.Parse(talentCode.Substring(13, 1));
        //    c.RogueTalents.QuickRecovery = int.Parse(talentCode.Substring(14, 1));
        //    c.RogueTalents.SealFate = int.Parse(talentCode.Substring(15, 1));
        //    c.RogueTalents.MasterPoisoner = int.Parse(talentCode.Substring(16, 1));
        //    c.RogueTalents.Vigor = int.Parse(talentCode.Substring(17, 1));
        //    c.RogueTalents.DeadenedNerves = int.Parse(talentCode.Substring(18, 1));
        //    c.RogueTalents.FindWeakness = int.Parse(talentCode.Substring(19, 1));
        //    c.RogueTalents.Mutilate = int.Parse(talentCode.Substring(20, 1));

        //    c.RogueTalents.ImprovedGouge = int.Parse(talentCode.Substring(21, 1));
        //    c.RogueTalents.ImprovedSinisterStrike = int.Parse(talentCode.Substring(22, 1));
        //    c.RogueTalents.LightningReflexes = int.Parse(talentCode.Substring(23, 1));
        //    c.RogueTalents.ImprovedSliceandDice = int.Parse(talentCode.Substring(24, 1));
        //    c.RogueTalents.Deflection = int.Parse(talentCode.Substring(25, 1));
        //    c.RogueTalents.Precision = int.Parse(talentCode.Substring(26, 1));
        //    c.RogueTalents.Endurance = int.Parse(talentCode.Substring(27, 1));
        //    c.RogueTalents.Riposte = int.Parse(talentCode.Substring(28, 1));
        //    c.RogueTalents.ImprovedSprint = int.Parse(talentCode.Substring(29, 1));
        //    c.RogueTalents.ImprovedKick = int.Parse(talentCode.Substring(30, 1));
        //    c.RogueTalents.DaggerSpecialization = int.Parse(talentCode.Substring(31, 1));
        //    c.RogueTalents.DualWieldSpecialization = int.Parse(talentCode.Substring(32, 1));
        //    c.RogueTalents.MaceSpecialization = int.Parse(talentCode.Substring(33, 1));
        //    c.RogueTalents.BladeFlurry = int.Parse(talentCode.Substring(34, 1));
        //    c.RogueTalents.SwordSpecialization = int.Parse(talentCode.Substring(35, 1));
        //    c.RogueTalents.FistSpecialization = int.Parse(talentCode.Substring(36, 1));
        //    c.RogueTalents.BladeTwisting = int.Parse(talentCode.Substring(37, 1));
        //    c.RogueTalents.WeaponExpertise = int.Parse(talentCode.Substring(38, 1));
        //    c.RogueTalents.Aggression = int.Parse(talentCode.Substring(39, 1));
        //    c.RogueTalents.Vitality = int.Parse(talentCode.Substring(40, 1));
        //    c.RogueTalents.AdrenalineRush = int.Parse(talentCode.Substring(41, 1));
        //    c.RogueTalents.NervesOfSteel = int.Parse(talentCode.Substring(42, 1));
        //    c.RogueTalents.CombatPotency = int.Parse(talentCode.Substring(43, 1));
        //    c.RogueTalents.SurpriseAttacks = int.Parse(talentCode.Substring(44, 1));

        //    c.RogueTalents.MasterOfDeception = int.Parse(talentCode.Substring(45, 1));
        //    c.RogueTalents.Opportunity = int.Parse(talentCode.Substring(46, 1));
        //    c.RogueTalents.SleightOfHand = int.Parse(talentCode.Substring(47, 1));
        //    c.RogueTalents.DirtyTricks = int.Parse(talentCode.Substring(48, 1));
        //    c.RogueTalents.Camouflage = int.Parse(talentCode.Substring(49, 1));
        //    c.RogueTalents.Initiative = int.Parse(talentCode.Substring(50, 1));
        //    c.RogueTalents.GhostlyStrike = int.Parse(talentCode.Substring(51, 1));
        //    c.RogueTalents.ImprovedAmbush = int.Parse(talentCode.Substring(52, 1));
        //    c.RogueTalents.Setup = int.Parse(talentCode.Substring(53, 1));
        //    c.RogueTalents.Elusiveness = int.Parse(talentCode.Substring(54, 1));
        //    c.RogueTalents.SerratedBlades = int.Parse(talentCode.Substring(55, 1));
        //    c.RogueTalents.HeightenedSenses = int.Parse(talentCode.Substring(56, 1));
        //    c.RogueTalents.Preparation = int.Parse(talentCode.Substring(57, 1));
        //    c.RogueTalents.DirtyDeeds = int.Parse(talentCode.Substring(58, 1));
        //    c.RogueTalents.Hemorrhage = int.Parse(talentCode.Substring(59, 1));
        //    c.RogueTalents.MasterOfSubtlety = int.Parse(talentCode.Substring(60, 1));
        //    c.RogueTalents.Deadliness = int.Parse(talentCode.Substring(61, 1));
        //    c.RogueTalents.EnvelopingShadows = int.Parse(talentCode.Substring(62, 1));
        //    c.RogueTalents.Premeditation = int.Parse(talentCode.Substring(63, 1));
        //    c.RogueTalents.CheatDeath = int.Parse(talentCode.Substring(64, 1));
        //    c.RogueTalents.SinisterCalling = int.Parse(talentCode.Substring(65, 1));
        //    c.RogueTalents.Shadowstep = int.Parse(talentCode.Substring(66, 1));
        //}

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Agility + stats.Strength + stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.AttackPower + stats.BonusAttackPowerMultiplier + stats.CritRating + stats.HitRating + stats.HasteRating + stats.ExpertiseRating + stats.ArmorPenetration + stats.WeaponDamage + stats.BonusCritMultiplier + stats.WindfuryAPBonus + stats.MongooseProc + stats.MongooseProcAverage + stats.MongooseProcConstant + stats.ExecutionerProc + stats.BonusSnDDuration + stats.CPOnFinisher + stats.BonusEvisEnvenomDamage + stats.BonusFreeFinisher + stats.BonusCPGDamage + stats.BonusSnDHaste + stats.BonusBleedDamageMultiplier) != 0;
        }

        #region Rogue Racial Stats

        private static readonly float[,] BaseRogueRaceStats = new[,]
                                                                  {
                                                                      //	Agility,	Strength,	Stamina
                                                                      /*Empty*/     {0f, 0f, 0f,},
                                                                      /*Human*/		{158f, 95f, 89f,},
                                                                      /*Orc*/		{155f, 98f, 91f,},
                                                                      /*Dwarf*/		{154f, 97f, 92f,},
                                                                      /*Night Elf*/	{163f, 92f, 88f,},
                                                                      /*Undead*/	{156f, 94f, 90f,},
                                                                      /*Tauren*/	{0f, 0f, 0f,},
                                                                      /*Gnome*/		{161f, 90f, 88f,},
                                                                      /*Troll*/		{160f, 96f, 90f,},
                                                                      /*BloodElf*/	{160f, 92f, 87f,},
                                                                      /*Draenei*/	{0f, 0f, 0f,}
                                                                  };

        private Stats GetRaceStats(Character.CharacterRace race)
        {
            if (race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei)
                return new Stats();

            var statsRace = new Stats
                                {
                                    Health = 3524f,
                                    Agility = BaseRogueRaceStats[(int) race, 0],
                                    Strength = BaseRogueRaceStats[(int) race, 1],
                                    Stamina = BaseRogueRaceStats[(int) race, 2],
                                    AttackPower = 120f,
                                    PhysicalCrit = 3.73f,
                                    DodgeRating = (float) (-0.59*18.92f),
                                };

            statsRace.Health += statsRace.Stamina*10f;

            if (race == Character.CharacterRace.NightElf)
                statsRace.DodgeRating += 18.92f;

            return statsRace;
        }

        #endregion

        private void SetupDisplayCalculationLabels()
        {
            _characterDisplayCalculationLabels = new[]
                                                     {
                                                         "Base Stats:Health",
                                                         "Base Stats:Stamina",
                                                         "Base Stats:Strength",
                                                         "Base Stats:Agility",
                                                         "Base Stats:Attack Power",
                                                         "Base Stats:Hit",
                                                         "Base Stats:Expertise",
                                                         "Base Stats:Haste",
                                                         "Base Stats:Armor Penetration",
                                                         "Base Stats:Crit",
                                                         "Base Stats:Weapon Damage",
                                                         "DPS Breakdown:White DPS",
                                                         "DPS Breakdown:CPG DPS",
                                                         "DPS Breakdown:Finisher DPS",
                                                         "DPS Breakdown:Sword Spec DPS",
                                                         "DPS Breakdown:Windfury DPS",
                                                         "DPS Breakdown:Poison DPS",
                                                         "DPS Breakdown:Total DPS"
                                                     };
        }

        private void SetupRelevantItemTypes()
        {

            _relevantItemTypes = new List<Item.ItemType>(new[]
                                                             {
                                                                 Item.ItemType.None,
                                                                 Item.ItemType.Leather,
                                                                 Item.ItemType.Bow,
                                                                 Item.ItemType.Crossbow,
                                                                 Item.ItemType.Gun,
                                                                 Item.ItemType.Thrown,
                                                                 Item.ItemType.Dagger,
                                                                 Item.ItemType.FistWeapon,
                                                                 Item.ItemType.OneHandMace,
                                                                 Item.ItemType.OneHandSword
                                                             });
        }
    }

    public class RogueConversions
    {
        public static readonly float AgilityToAP = 1.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f/40.0f;
        public static readonly float AgilityToDodge = 1.0f/20.0f;
        public static readonly float CritRatingToCrit = 1.0f/22.0769f; //14*82/52
        public static readonly float CritToCritRating = 22.0769f; //14*82/52
        public static readonly float ExpertiseRatingToExpertise = 1.0f/3.9423f;
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float HasteRatingToHaste = 1.0f/15.77f;
        public static readonly float HitRatingToHit = 1.0f/15.7692f;
        public static readonly float ParryRatingToParry = 1.0f/23.6538461538462f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float StrengthToAP = 1.0f;
    }
}