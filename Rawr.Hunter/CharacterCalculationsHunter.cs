using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Rawr.Hunter.Skills;

namespace Rawr.Hunter
{
    public class CharacterCalculationsHunter : CharacterCalculationsBase
    {
//        public StatsHunter UnbuffedStats { get; set; }
//        public StatsHunter BuffStats { get; set; }
//        public StatsHunter MaximumStats { get; set; }

        public Character character = null;
        public CalculationOptionsHunter CalcOpts = null;

        /// <summary>
        /// The hunter with max or averaged SE stats.
        /// </summary>
        public HunterBase Hunter { get; set; }
        /// <summary>
        /// The Hunter with armory (no SE stats).
        /// </summary>
        public HunterBase HunterUnBuffed { get; set; }
        public PetBase Pet { get; set; }

        public PetCalculations PetCalc { get; set; }

/*      This is from the original Shot Code
 * 
 *      public ShotData aimedShot = new ShotData(Shots.AimedShot, false, true, true);
        public ShotData arcaneShot = new ShotData(Shots.ArcaneShot, false, true, true);
        public ShotData multiShot = new ShotData(Shots.MultiShot, false, true, true);
        public ShotData serpentSting = new ShotData(Shots.SerpentSting, false, true, true);
        public ShotData cobraShot = new ShotData(Shots.CobraShot, false, true, true);
        public ShotData steadyShot = new ShotData(Shots.SteadyShot, false, true, true);
        public ShotData killShot = new ShotData(Shots.KillShot, false, true, true);
        public ShotData explosiveShot = new ShotData(Shots.ExplosiveShot, false, true, true);
        public ShotData blackArrow = new ShotData(Shots.BlackArrow, false, false, true);
        public ShotData immolationTrap = new ShotData(Shots.ImmolationTrap, false, false, true);
        public ShotData explosiveTrap = new ShotData(Shots.ExplosiveTrap, false, false, true);
        public ShotData freezingTrap = new ShotData(Shots.FreezingTrap, true, false, true);
        public ShotData frostTrap = new ShotData(Shots.IceTrap, true, false, true);
        public ShotData chimeraShot = new ShotData(Shots.ChimeraShot, false, true, true);
        public ShotData rapidFire = new ShotData(Shots.RapidFire, true, false, false);
        public ShotData readiness = new ShotData(Shots.Readiness, true, false, true);
        public ShotData bestialWrath = new ShotData(Shots.BestialWrath, true, false, false);

//        public ShotPriority priorityRotation = null;
        */

        private float _overallPoints = 0f;
        private float[] _subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }
        public float HunterDpsPoints 
        {
            get { return _subPoints[(int)subpointINDEX.HUNTERDPS]; }
            set { _subPoints[(int)subpointINDEX.HUNTERDPS] = value; }
        }
        public float HunterSurvPoints
        {
            get { return _subPoints[(int)subpointINDEX.HUNTERSURVIVAL]; }
            set { _subPoints[(int)subpointINDEX.HUNTERSURVIVAL] = value; }
        }
        public float PetDpsPoints
        {
            get { return _subPoints[(int)subpointINDEX.PETDPS]; }
            set { _subPoints[(int)subpointINDEX.PETDPS] = value; }
        }
        public float PetSurvPoints
        {
            get { return _subPoints[(int)subpointINDEX.PETSURVIVAL]; }
            set { _subPoints[(int)subpointINDEX.PETSURVIVAL] = value; }
        }
        public override float OverallPoints
        {
            get 
            {
                _overallPoints = 0;
                _overallPoints += HunterDpsPoints;
                _overallPoints += HunterSurvPoints;
                _overallPoints += PetDpsPoints;
                _overallPoints += PetSurvPoints;
                return _overallPoints; 
            }
            set { _overallPoints = value; }
        }

        #region Skills
        public Skills.WhiteAttacks Whites { get; set; }
        public AbilWrapper Explosive { get; set; }
        public AbilWrapper Steady { get; set; }
        public AbilWrapper Cobra { get; set; }
        public AbilWrapper Aimed { get; set; }
        public AbilWrapper Multi { get; set; }
        public AbilWrapper Arcane { get; set; }
        public AbilWrapper Kill { get; set; }
        public Skills.BlackArrow BlackArrowD { get; set; }
        //        public Skills.BlackArrowBuff BlackArrowB { get; set; }
        public Skills.PiercingShots Piercing { get; set; }
        public AbilWrapper Serpent { get; set; }
        public AbilWrapper Chimera { get; set; }
        public Skills.ImmolationTrap Immolation { get; set; }
        public Skills.ExplosiveTrap ExplosiveT { get; set; }
        public Skills.FreezingTrap Freezing { get; set; }
        public Skills.FrostTrap Frost { get; set; }
        public Skills.Readiness Ready { get; set; }
        public Skills.BestialWrath Bestial { get; set; }
        public Skills.RapidFire Rapid { get; set; }
        #endregion

//        private Stats _basicStats;
        private float _baseAttackSpeed;
        private float _autoshotDPS;
        private float _BonusAttackProcsDPS;
        private float _wildQuiverDPS;
        public float SpecProcDPS;
        private float _customDPS;
        public float _masteryBase = 8f;
        public BossOptions BossOpts = null;

        public float critRateOverall { get; set; }
        public float critFromAgi { get; set; }

        private double _piercingShotsDPS;
        private double _piercingShotsDPSSteadyShot;
        private double _piercingShotsDPSAimedShot;
        private double _piercingShotsDPSChimeraShot;

        public float BaseHealth { get; set; }
        public float Agility { get; set; }


        #region Pet Stats
        public float petBaseHealth { get; set; }
        public float petHealthfromStamina { get; set; }
        public float petBonusHealth { get; set; }
        public float petKillCommandDPS { get; set; }
        public float petKillCommandMPS { get; set; }
        public float petWhiteDPS { get; set; }
        public float petSpecialDPS { get; set; }
        public float petArmorDebuffs { get; set; }
        public float petTargetArmorReduction { get; set; }
        public float petTargetDodge { get; set; }
        public float ferociousInspirationDamageAdjust { get; set; }
        #endregion

        #region Pet Hit
        public float petHitTotal { get; set; }
        public float petHitSpellTotal { get; set; }
        #endregion

        #region Pet Crit
        public float petCritFromBase { get; set; }
        public float petCritFromAgility { get; set; }
        public float petCritFromSpidersBite { get; set; }
        public float petCritFromFerocity { get; set; }
        public float petCritFromGear { get; set; }
        public float petCritFromBuffs { get; set; }
        public float petCritFromTargetDebuffs { get; set; }
        public float petCritFromDepression { get; set; }
        public float petCritFromCobraStrikes { get; set; }
        public float petCritTotalMelee { get; set; }
        public float petCritTotalSpecials { get; set; }
        #endregion

        #region Pet AP
        public float petAPFromStrength { get; set; }
        public float petAPFromHunterVsWild { get; set; }
        public float petAPFromTier9 { get; set; }
        public float petAPFromBuffs { get; set; }
        public float petAPFromHunterRAP { get; set; }
        public float petAPFromRabidProc { get; set; }
        public float petAPFromSerenityDust { get; set; }
        public float petAPFromOutsideBuffs { get; set; }
        public float petAPFromAnimalHandler { get; set; }
        public float petAPFromAspectOfTheBeast { get; set; }
        #endregion

        #region Debuffs
        public float targetDebuffsCrit { get; set; }
        public float targetDebuffsArmor { get; set; }
        public float targetDebuffsNature { get; set; }
        public float targetDebuffsPetDamage { get; set; }
        #endregion

        #region Haste Stats
        public float hasteFromTalentsStatic { get; set; }
        public float hasteFromRapidFire { get; set; }
        public float hasteFromProcs { get; set; }
        public float hasteFromBase { get; set; }
        public float hasteFromRating { get; set; }
        public float hasteFromRangedBuffs { get; set; }
        public float hasteFromRacial { get; set; }
        public float hasteStaticTotal { get; set; }
        public float hasteDynamicTotal { get; set; }
        public float hasteEffectsTotal { get; set; }
        #endregion   		
        
        #region RAP Stats
        public float apTotal { get; set; }
        public float apSelfBuffed { get; set; }
        public float apFromBase { get; set; }
        public float apFromAGI { get; set; }
        public float apFromGear { get; set; }
        #endregion
        
        #region Hit Stats
        public float hitOverall {get; set;}
        public float hitFromBase { get; set; }
        public float hitFromRating { get; set; }
        public float hitFromTalents {get; set;}
        public float hitFromBuffs {get; set;}
        public float hitFromTargetDebuffs { get; set; }
        public float hitFromLevelAdjustment { get; set; }
        #endregion        
        
        #region Crit Stats
        public float critBase { get; set; }
        public float critFromRacial { get; set; }
        public float critFromRating { get; set; }
        public float critFromProcRating { get; set; }
        public float critFromLethalShots { get; set; }
        public float critFromKillerInstincts { get; set; }
        public float critFromMasterMarksman { get; set; }
        public float critFromMasterTactician { get; set; }
        public float critFromBuffs { get; set; }
        public float critFromDepression { get; set; }
        #endregion

        #region Mastery
        public float masteryoverall { get; set; }
        public float masteryfrombase { get; set; }
        public float masteryfromincrement { get; set; }
        #endregion

        #region Shots Per Second
        public float shotsPerSecondCritting { get; set; }
        #endregion

        public float damageReductionFromArmor { get; set; }

        #region Focus
        public float focus { get; set; }
        public float basefocus { get; set; }
        public float focusfromtalents { get; set; }
        #endregion

        #region Kill shot used sub-20%
        public float killShotSub20NewSteadyFreq { get; set; }
        public float killShotSub20NewDPS { get; set; }
        public float killShotSub20NewSteadyDPS { get; set; }
        public float killShotSub20Gain { get; set; }
        public float killShotSub20TimeSpent { get; set; }
        public float killShotSub20FinalGain { get; set; }
        #endregion

        #region Aspect uptime/penalties/bonuses
        public float aspectUptimeHawk { get; set; }
        public float aspectUptimeViper { get; set; }
        public float aspectUptimeBeast { get; set; }
        public float aspectBeastLostDPS { get; set; }
        public float aspectViperPenalty { get; set; }
        public float aspectBonusAPBeast { get; set; }
        public float NoManaDPSDownTimePerc { get; set; }
        #endregion

        public float BaseAttackSpeed
        {
            get { return Whites.RwEffectiveSpeed; }
        }

        public float BonusAttackProcsDPS
        {
            get { return _BonusAttackProcsDPS; }
            set { _BonusAttackProcsDPS = value; }
        }

        public float WildQuiverDPS
        {
            get { return _wildQuiverDPS; }
            set { _wildQuiverDPS = value; }
        }

        public double PiercingShotsDPS
        {
            get { return _piercingShotsDPS; }
            set { _piercingShotsDPS = value; }
        }

        public double PiercingShotsDPSSteadyShot
        {
            get { return _piercingShotsDPSSteadyShot; }
            set { _piercingShotsDPSSteadyShot = value; }
        }
        
        public double PiercingShotsDPSAimedShot
        {
            get { return _piercingShotsDPSAimedShot; }
            set { _piercingShotsDPSAimedShot = value; }
        }

        public double PiercingShotsDPSChimeraShot
        {
            get { return _piercingShotsDPSChimeraShot; }
            set { _piercingShotsDPSChimeraShot = value; }
        }

        public float CustomDPS
        {
            get { return _customDPS; }
            set { _customDPS = value; }
        }

        public List<string> ActiveBuffs { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            //string format = "";
            CalcOpts = character.CalculationOptions as CalculationOptionsHunter;

            //Basic Stats
            dictValues.Add("Health and Stamina", string.Format("{0:##,##0}*{1:##,##0} : Stamina",
                                Hunter.Health, Hunter.Stamina));
            dictValues.Add("Focus", string.Format("{0:000}", Hunter.Focus.ToString("F0")));
            dictValues.Add("Armor", Hunter.Armor.ToString("F0"));
            dictValues.Add("Agility", string.Format("{0}*{1} : After Special Effects", HunterUnBuffed.Agility, Hunter.Agility));
            dictValues.Add("Ranged Attack Power", string.Format("{0:00,000}*{1:00,000} : After Special Effects", //+
//                            "\r\n{1:00,000} : Base" +
//                            "\r\n{2:00,000} : Agility" +
//                            "\r\n{3:00,000} : Gear / Spec" +
//                            "\r\nProcs were averaged out and added",
                            HunterUnBuffed.RangedAttackPower, Hunter.RangedAttackPower));//, apFromBase, apFromAGI, apFromGear));
            dictValues.Add("Hit", string.Format("{0:00.00%} : {1}*" + 
                                "Ranged Cap: " + Hunter.HitNeededLabel,
                                HunterUnBuffed.HitRatingPercent,
                                HunterUnBuffed.HitRating,
                                HunterUnBuffed.HitRatingNeeded));
             
            dictValues.Add("Crit", string.Format("{0:00.00%} : {1}*Includes:" +
                                "\r\n{2:00.00%} : Agility" +
                                "\r\n{3:00.00%} : Rating" +
                                "\r\n{4:00.00%} : Buffs" + 
                                "\r\n{5:00.00%} : Target Modifier" +
                                "\r\n\r\nNote that individual Shots will handle their own crit caps",
                                HunterUnBuffed.ChancetoCrit,
                                HunterUnBuffed.CritRating,
                                HunterUnBuffed.CritfromAgility,
                                HunterUnBuffed.CritfromRating,
                                HunterUnBuffed.PhysicalCrit,
                                HunterUnBuffed.CritModifiedfromTarget));
            dictValues.Add("Haste", string.Format("{0:00.00%} : {1:0}",
//                                "*Includes:" +
//                                "\r\n{2:00.00%} : Base" +
//                                "\r\n{3:00.00%} : Rating" +
                                HunterUnBuffed.Haste,
                                HunterUnBuffed.HasteRating));
            dictValues.Add("Mastery", string.Format("{0:00.00%} : {1}* Includes:" +
                                "\r\n{2:00.0000} : Mastery From Rating" +
                                "\r\n{3:00.0000%} : Spec base %" +
                                "\r\n{4:00.0000%} : Incremental %" +
                                HunterUnBuffed.MasteryLabel,
                                HunterUnBuffed.MasteryRatePercent,
                                HunterUnBuffed.MasteryRating,
                                HunterUnBuffed.MasteryRateConversion,
                                HunterUnBuffed.BaseMastery,
                                HunterUnBuffed.IncrementalmasterywithConversion,
                                HunterUnBuffed.MasteryRatePercent));
            dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
            
            // Pet Stats
/*            dictValues.Add("Pet Health", string.Format("{0:000,000}*" +
                                        "{1:000,000} : Base" + 
                                        "\r\n{2:000,000} : Hunter" +
                                        "\r\n{3:000,000} : Bonus",
                                        pet.PetStats.Health, petBaseHealth, petHealthfromStamina, petBonusHealth));
            dictValues.Add("Pet Armor", pet.PetStats.Armor.ToString("F0"));
            dictValues.Add("Pet Focus", focus.ToString("F0"));
            dictValues.Add("Pet Attack Power", pet.PetStats.AttackPower.ToString("F0") +
                                                string.Format("*Full Pet Stats:\r\n"
//                                                              + "Strength: {0:0.0}\r\n"
//                                                              + "Agility: {1:0.0}\r\n"
                                                              + "Hit: {0:0.00%}\r\n"
                                                              + "PhysCrit: {1:0.00%}\r\n"
                                                              + "PhysHaste: {2:0.00%}\r\n",
//                                                            pet.PetStats.Strength,
//                                                            pet.PetStats.Agility,
                                                            pet.PetStats.PhysicalHit,
                                                            pet.PetStats.PhysicalCrit,
                                                            pet.PetStats.PhysicalHaste));
            dictValues.Add("Pet Hit %", petHitTotal.ToString("P2"));
            dictValues.Add("Pet Dodge %", petTargetDodge.ToString("P2"));
            dictValues.Add("Pet Melee Crit %", petCritTotalMelee.ToString("P2") + "*includes:\n" +
                            petCritFromBase.ToString("P2") + " from base\n" +
                            petCritFromAgility.ToString("P2") + " from agility\n" +
                            petCritFromSpidersBite.ToString("P2") + " from Spider's Bite\n" +
                            petCritFromFerocity.ToString("P2") + " from Ferocity\n" +
                            petCritFromGear.ToString("P2") + " from gear\n" +
                            petCritFromBuffs.ToString("P2") + " from buffs\n" +
                            petCritFromTargetDebuffs.ToString("P2") + " from target debuffs\n" +
                            petCritFromDepression.ToString("P2") + " from depression");
            dictValues.Add("Pet Specials Crit %", petCritTotalSpecials.ToString("P2") + "*includes:\n" +
                            petCritTotalMelee.ToString("P2") + " from melee crit\n" +
                            petCritFromCobraStrikes.ToString("P2") + " from Cobra Strikes");
            dictValues.Add("Pet White DPS", petWhiteDPS.ToString("F2"));
            dictValues.Add("Pet Kill Command DPS", petKillCommandDPS.ToString("F2"));
            dictValues.Add("Pet Specials DPS", petSpecialDPS.ToString("F2") /*+ 
                string.Format("Breakout:\r\n"
                            + "Furious Howl: Use {0} DPS {1:0.00}"
                            + "Bite: Use {2} DPS {3:0.00}",
                            pet.priorityRotation.getSkillFrequency(PetAttacks.FuriousHowl), 0f,
                            pet.priorityRotation.getSkillFrequency(PetAttacks.Bite), pet.priorityRotation.dps - petWhiteDPS));
*/
            // Shot Stats
            dictValues.Add("Aimed Shot", this.Aimed.GenTooltip(this.CustomDPS));
            dictValues.Add("Arcane Shot", this.Arcane.GenTooltip(this.CustomDPS));
//            dictValues.Add("Multi Shot", multiShot.GenTooltip());
//            dictValues.Add("Cobra Shot", cobraShot.GenTooltip());
            dictValues.Add("Steady Shot", this.Steady.GenTooltip(this.CustomDPS));
            dictValues.Add("Kill Shot", this.Kill.GenTooltip(this.CustomDPS));
//            dictValues.Add("Explosive Shot", explosiveShot.GenTooltip());
//            dictValues.Add("Black Arrow", blackArrow.GenTooltip());
            dictValues.Add("Chimera Shot", this.Chimera.GenTooltip(this.CustomDPS));
            
            //dictValues.Add("Rapid Fire", rapidFire.GenTooltip());
            //dictValues.Add("Readiness", readiness.GenTooltip());
            //dictValues.Add("Bestial Wrath", bestialWrath.GenTooltip());

            // Sting Stats
//            dictValues.Add("Serpent Sting", serpentSting.GenTooltip());

            // Trap Stats
            //dictValues.Add("Immolation Trap", immolationTrap.GenTooltip());
            //dictValues.Add("Explosive Trap", explosiveTrap.GenTooltip());
            //dictValues.Add("Freezing Trap", freezingTrap.GenTooltip());
            //dictValues.Add("Frost Trap", frostTrap.GenTooltip());

            // Hunter DPS
            dictValues.Add("Autoshot DPS", Whites.GenTooltip(CustomDPS));
            dictValues.Add("Priority Rotation DPS", CustomDPS.ToString("F2"));
            dictValues.Add("Wild Quiver DPS", WildQuiverDPS.ToString("F2"));
//            dictValues.Add("Kill Shot low HP gain", killShotSub20FinalGain.ToString("F2")+"*"+
//                            "Kill Shot freq: "+killShot.Freq.ToString("F2")+" -> "+killShot.start_freq.ToString("F2")+"\n"+
//                            "Steady Shot freq: "+steadyShot.Freq.ToString("F2")+" -> "+killShotSub20NewSteadyFreq.ToString("F2")+"\n"+
//                            "Kill Shot DPS: "+killShot.DPS.ToString("F2")+" -> "+killShotSub20NewDPS.ToString("F2")+"\n"+
//                            "Steady Shot DPS: "+steadyShot.DPS.ToString("F2")+" -> "+killShotSub20NewSteadyDPS.ToString("F2")+"\n"+
//                            "DPS Gain when switched: " + killShotSub20Gain.ToString("F2")+"\n"+
//                            "Time spent sub-20%: " + killShotSub20TimeSpent.ToString("P2"));
            dictValues.Add("Aspect Loss", aspectBeastLostDPS.ToString("F2") + "*" +
                            "Hawk Uptime: " + aspectUptimeHawk.ToString("P2") + "\n" + 
                            "Fox Uptime: " + aspectUptimeBeast.ToString("P2"));
            dictValues.Add("Piercing Shots DPS", PiercingShotsDPS.ToString("F2") + "*" +
                            "Steady Shot: " + PiercingShotsDPSSteadyShot.ToString("F2") + "\n" +
                            "Aimed Shot: " + PiercingShotsDPSAimedShot.ToString("F2") + "\n" +
                            "Chimera Shot: " + PiercingShotsDPSChimeraShot.ToString("F2") + "\n");
            dictValues.Add("Special DMG Procs DPS", SpecProcDPS.ToString("F2"));

            // Combined DPS
            dictValues.Add("Hunter DPS", HunterDpsPoints.ToString("F2"));
            dictValues.Add("Pet DPS", PetDpsPoints.ToString("F2"));
            dictValues.Add("Total DPS", OverallPoints.ToString("F2"));

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return HunterUnBuffed.Health;
                case "Agility": return HunterUnBuffed.Agility;
                case "Crit %": return HunterUnBuffed.PhysicalCrit * 100f;
                case "Haste %": return HunterUnBuffed.PhysicalHaste * 100f;
                case "Attack Power": return HunterUnBuffed.RangedAttackPower;
                //case "% Chance to Miss (Yellow)": return (StatConversion.WHITE_MISS_CHANCE_CAP[BossOpts.Level - character.Level] - BasicStats.PhysicalHit) * 100f;
            }
            return 0;
        }
    }
}