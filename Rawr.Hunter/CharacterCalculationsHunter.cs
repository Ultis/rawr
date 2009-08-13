using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Hunter
{
    public class CharacterCalculationsHunter : CharacterCalculationsBase
    {

		private float _overallPoints = 0f;
		private float[] _subPoints = new float[] { 0f,0f };
		private Stats _basicStats;
		private float _baseAttackSpeed;
        private double _autoshotDPS;
        private double _wildQuiverDPS;
        private double _customDPS;

        public PetCalculations pet { get; set; }

        #region Pet Stats
        public double petKillCommandDPS { get; set; }
        public double petKillCommandMPS { get; set; }
        public double petWhiteDPS { get; set; }
        public double petSpecialDPS { get; set; }
        public double petArmorDebuffs { get; set; }
        public double petHit { get; set; }
        public double petSpellHit { get; set; }
        public double petTargetDodge { get; set; }
        public double ferociousInspirationDamageAdjust { get; set; }
        public double petCritMelee { get; set; }
        public double petCritSpecials { get; set; }
        #endregion

        #region Debuffs
        public double targetDebuffsCrit { get; set; }
        public double targetDebuffsArmor { get; set; }
        public double targetDebuffsNature { get; set; }
        public double targetDebuffsPetDamage { get; set; }
        #endregion

        #region Haste Stats
        public double hasteFromHeroism { get; set; }
        public double hasteFromTalentsStatic { get; set; }
        public double hasteFromRapidFire { get; set; }
        public double hasteFromProcs { get; set; }
        public double hasteFromBase { get; set; }
   		public double hasteFromRating { get; set; }
   		public double hasteFromRangedBuffs { get; set; }
        public double hasteFromRacial { get; set; }
        public double hasteStaticTotal { get; set; }
        public double hasteDynamicTotal { get; set; }
        public double hasteEffectsTotal { get; set; }
        #endregion   		
   		
        #region RAP Stats
        public double apTotal { get; set; }
        public double apSelfBuffed { get; set; }
		public double apFromBase { get; set; }
		public double apFromAgil { get; set; }
		public double apFromCarefulAim { get; set; }
		public double apFromHunterVsWild { get; set; }
		public double apFromGear { get; set; }
		public double apFromBloodFury { get; set; }
		public double apFromAspectOfTheHawk { get; set; }
		public double apFromAspectMastery { get; set; }
		public double apFromFuriousHowl { get; set; }
        public double apFromDebuffs { get; set; }
        public double apFromProc { get; set; }
		public double apFromCallOfTheWild { get; set; }
		public double apFromTrueshotAura { get; set; }
		public double apFromHuntersMark {get; set;}
        public double apFromExposeWeakness { get; set; }
        #endregion
        
        #region Hit Stats
        public double hitOverall {get; set;}
        public double hitFromBase { get; set; }
        public double hitFromRating { get; set; }
        public double hitFromTalents {get; set;}
        public double hitFromBuffs {get; set;}
        public double hitFromTargetDebuffs { get; set; }
        public double hitFromLevelAdjustment { get; set; }       
        #endregion        
   		
        #region Crit Stats
        public double critRateOverall {get; set;}
        public double critBase {get; set;}
        public double critFromAgi {get; set;}
        public double critFromRating {get; set;}
        public double critFromProcRating { get; set; }
        public double critFromLethalShots {get; set;}
        public double critFromKillerInstincts {get; set;}
        public double critFromMasterMarksman {get; set;}
        public double critFromMasterTactician { get; set; }
        public double critFromBuffs {get; set;}
        public double critFromDepression  {get; set;}
        #endregion

        #region Shots Per Second
        public double shotsPerSecondCritting { get; set; }
        #endregion

        public double damageReductionFromArmor { get; set; }
        public double baseMana { get; set; }

        // new shots data
        public ShotData aimedShot = new ShotData(Shots.AimedShot, true, true);
        public ShotData arcaneShot = new ShotData(Shots.ArcaneShot, true, true);
        public ShotData multiShot = new ShotData(Shots.MultiShot, true, true);
        public ShotData serpentSting = new ShotData(Shots.SerpentSting, false, true);
        public ShotData scorpidSting = new ShotData(Shots.ScorpidSting, false, true);
        public ShotData viperSting = new ShotData(Shots.ViperSting, false, true);
        public ShotData silencingShot = new ShotData(Shots.SilencingShot, true, false);
        public ShotData steadyShot = new ShotData(Shots.SteadyShot, true, true);
        public ShotData killShot = new ShotData(Shots.KillShot, true, true);       
        public ShotData explosiveShot = new ShotData(Shots.ExplosiveShot, true, true);        
        public ShotData blackArrow = new ShotData(Shots.BlackArrow, false, true);
        public ShotData immolationTrap = new ShotData(Shots.ImmolationTrap, false, true);
        public ShotData chimeraShot = new ShotData(Shots.ChimeraShot, true, true);
        public ShotData rapidFire = new ShotData(Shots.RapidFire, false, false);
        public ShotData readiness = new ShotData(Shots.Readiness, false, true);
        public ShotData beastialWrath = new ShotData(Shots.BeastialWrath, false, false);
        public ShotData bloodFury = new ShotData(Shots.BloodFury, false, false);
        public ShotData berserk = new ShotData(Shots.Berserk, false, false);

        public ShotPriority priorityRotation = null;


        #region mana regen
        public double manaRegenGearBuffs {get; set;}
        public double manaRegenViper { get; set; }
        public double manaRegenRoarOfRecovery { get; set; }
        public double manaRegenRapidRecuperation { get; set; }
        public double manaRegenChimeraViperProc { get; set; }
        public double manaRegenInvigoration { get; set; }
        public double manaRegenHuntingParty { get; set; }
        public double manaRegenTargetDebuffs { get; set; }
        public double manaRegenTotal { get; set; }                
        #endregion

        #region Kill shot used sub-20%
        public double killShotSub20NewSteadyFreq { get; set; }
        public double killShotSub20NewDPS { get; set; }
        public double killShotSub20NewSteadyDPS { get; set; }
        public double killShotSub20Gain { get; set; }
        public double killShotSub20TimeSpent { get; set; }
        public double killShotSub20FinalGain { get; set; }
        #endregion

        #region Aspect uptime/penalties/bonuses
        public double aspectUptimeHawk { get; set; }
        public double aspectUptimeViper { get; set; }
        public double aspectUptimeBeast { get; set; }
        public double aspectBeastLostDPS { get; set; }
        public double aspectViperPenalty { get; set; }
        public double aspectBonusAPBeast { get; set; }
        #endregion

        #region Final Hunter DPS stats
        public double OnProcDPS { get; set; }
        #endregion


        public float BaseAttackSpeed
		{
			get { return _baseAttackSpeed; }
			set { _baseAttackSpeed = value; }
		}

        public double AutoshotDPS
        {
            get { return _autoshotDPS; }
            set { _autoshotDPS = value; }
        }

        public double WildQuiverDPS
        {
            get { return _wildQuiverDPS; }
            set { _wildQuiverDPS = value; }
        }

        public double CustomDPS
        {
            get { return _customDPS; }
            set { _customDPS = value; }
        }

		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float HunterDpsPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float PetDpsPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }

		}

		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}
		public List<string> ActiveBuffs { get; set; }



		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();

            // Basic Stats
			dictValues.Add("Agility", BasicStats.Agility.ToString("F0"));
            dictValues.Add("Stamina", BasicStats.Stamina.ToString("F0"));
            dictValues.Add("Intellect", BasicStats.Intellect.ToString("F0"));
            dictValues.Add("Armor", BasicStats.Armor.ToString("F0"));
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString("F0"));
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString("F0"));
            dictValues.Add("Armor Penetration", BasicStats.ArmorPenetrationRating.ToString() +
                            "* Enemy's Damage Reduction from armor: " + damageReductionFromArmor.ToString("P2"));
            dictValues.Add("Static Haste", hasteStaticTotal.ToString("P2")+ "*includes: \n"+
			               hasteFromBase.ToString("P2") + " from quiver\n" +
                           hasteFromRating.ToString("P2") + " from " + BasicStats.HasteRating.ToString("F0") + " gear haste rating\n" +
                           hasteFromTalentsStatic.ToString("P2") + " from Serpent's Swiftness\n" + 
			               hasteFromRangedBuffs.ToString("P2") +" from buffs");
            dictValues.Add("Dynamic Haste", hasteDynamicTotal.ToString("P2") + "*includes: \n" +
                           hasteFromRapidFire.ToString("P2") + " from rapid fire\n" +
                           hasteFromRacial.ToString("P2") + " from troll Berserk\n" +
                           hasteFromHeroism.ToString("P2") + " from Heroism\n" +
                           hasteFromProcs.ToString("P2") + " from procs");
			dictValues.Add("Mana Regen Per Second", manaRegenTotal.ToString("F2")+"*includes:\n" +
                           manaRegenGearBuffs.ToString("F2") + " from Gear and Buffs\n"+
                           manaRegenViper.ToString("F2") + " from Aspect of the Viper\n"+
                           manaRegenRoarOfRecovery.ToString("F2") + " from Roar of Recovery\n"+
                           manaRegenRapidRecuperation.ToString("F2") + " from Rapid Recuperation\n"+
                           manaRegenChimeraViperProc.ToString("F2") + " from Chimera Viper String Proc\n"+
                           manaRegenInvigoration.ToString("F2") + " from Invigoration\n"+
                           manaRegenHuntingParty.ToString("F2") + " from Hunting Party\n"+
                           manaRegenTargetDebuffs.ToString("F2") + " from Target Debuffs");

            // Basic Calculated Stats
            dictValues.Add("Health", BasicStats.Health.ToString("F0"));
            dictValues.Add("Mana", BasicStats.Mana.ToString("F0"));
			dictValues.Add("Hit Percentage", hitOverall.ToString("P2") + "*includes: \n" +
                            hitFromBase.ToString("P2") + " base hit chance \n" +
                            hitFromRating.ToString("P2") + " from rating \n" +
                            hitFromTalents.ToString("P2") + " from talents \n" +
                            hitFromTargetDebuffs.ToString("P2") + " from target debuffs \n" +
                            hitFromBuffs.ToString("P2") + " from buffs \n" +
                            hitFromLevelAdjustment.ToString("P2") + " level penalty");
			dictValues.Add("Crit Percentage", critRateOverall.ToString("P2") + "*includes: \n" +
			               critBase.ToString("P2") + " base crit \n" +
			               critFromAgi.ToString("P2") + " from agility \n" +
			               critFromRating.ToString("P2") + " from rating \n" +
                           critFromProcRating.ToString("P2") + " from proc rating \n" +
                           critFromLethalShots.ToString("P2") + " from Lethal Shots\n" +
                           critFromKillerInstincts.ToString("P2") + " from Killer Instincts\n" +
                           critFromMasterMarksman.ToString("P2") + " from Master Marksman\n" +
                           critFromMasterTactician.ToString("P2") + " from Master Tactician\n" +
			               critFromBuffs.ToString("P2") + " from buffs \n" +
			               critFromDepression.ToString("P2") + " from depression");
            dictValues.Add("Ranged AP", apTotal.ToString("F0") + "*includes: \n" +
                            apFromBase.ToString("F0") + " from base \n" +
                            apFromAgil.ToString("F0") + " from agility \n" +
                            apFromCarefulAim.ToString("F0") + " from Careful Aim \n" +
                            apFromHunterVsWild.ToString("F0") + " from Hunter vs Wild \n" +
                            apFromGear.ToString("F0") + " from +AP gear & buffs \n" +
                            apFromBloodFury.ToString("F0") + " from racials \n" +
                            apFromAspectOfTheHawk.ToString("F0") + " from Aspect of the Hawk \n" +
                            apFromAspectMastery.ToString("F0") + " from Aspect Mastery \n" +
                            apFromFuriousHowl.ToString("F0") + " from Furious Howl \n" +
                            apFromDebuffs.ToString("F0") + " from target debuffs \n" +
                            apFromProc.ToString("F0") + " from proc effects \n" +
                            apFromCallOfTheWild.ToString("P2") + " from Call of the Wild \n" +
                            apFromTrueshotAura.ToString("P2") + " from Trueshot Aura \n" +
                            apFromHuntersMark.ToString("F0") + " from Hunter's Mark \n" +
                            apFromExposeWeakness.ToString("F0") + " from Expose Weakness");
            dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
			
            // Pet Stats						
			dictValues.Add("Pet Attack Power", pet.petStats.AttackPower.ToString("F0"));
            dictValues.Add("Pet Hit Percentage", pet.petStats.PhysicalHit.ToString("P2"));
            dictValues.Add("Pet Melee Crit Percentage", petCritMelee.ToString("P2"));
            dictValues.Add("Pet Specials Crit Percentage", petCritSpecials.ToString("P2"));
            dictValues.Add("Pet White DPS", petWhiteDPS.ToString("F2"));
            dictValues.Add("Pet Kill Command DPS", petKillCommandDPS.ToString("F2"));
            dictValues.Add("Pet Specials DPS", petSpecialDPS.ToString("F2"));

            // Shot Stats
            dictValues.Add("Aimed Shot", aimedShot.formatTooltip());
            dictValues.Add("Arcane Shot", arcaneShot.formatTooltip());
            dictValues.Add("Multi Shot", multiShot.formatTooltip());
            dictValues.Add("Serpent Sting", serpentSting.formatTooltip());
            dictValues.Add("Scorpid Sting", scorpidSting.formatTooltip());
            dictValues.Add("Viper Sting", viperSting.formatTooltip());
            dictValues.Add("Silencing Shot", silencingShot.formatTooltip());
            dictValues.Add("Steady Shot", steadyShot.formatTooltip());
            dictValues.Add("Kill Shot", killShot.formatTooltip());
            dictValues.Add("Explosive Shot", explosiveShot.formatTooltip());
            dictValues.Add("Black Arrow", blackArrow.formatTooltip());
            dictValues.Add("Immolation Trap", immolationTrap.formatTooltip());
            dictValues.Add("Chimera Shot", chimeraShot.formatTooltip());
            dictValues.Add("Rapid Fire", rapidFire.formatTooltip());
            dictValues.Add("Readiness", readiness.formatTooltip());
            dictValues.Add("Beastial Wrath", beastialWrath.formatTooltip());
            dictValues.Add("Blood Fury", bloodFury.formatTooltip());
            dictValues.Add("Berserk", berserk.formatTooltip());

            // Hunter DPS
            dictValues.Add("Autoshot DPS", AutoshotDPS.ToString("F2"));
            dictValues.Add("Priority Rotation DPS", CustomDPS.ToString("F2"));
            dictValues.Add("Wild Quiver DPS", WildQuiverDPS.ToString("F2"));
            dictValues.Add("Proc DPS", OnProcDPS.ToString("F2"));
            dictValues.Add("Kill Shot low HP gain", killShotSub20FinalGain.ToString("F2")+"*"+
                            "Kill Shot freq: "+killShot.freq.ToString("F2")+" -> "+killShot.start_freq.ToString("F2")+"\n"+
                            "Steady Shot freq: "+steadyShot.freq.ToString("F2")+" -> "+killShotSub20NewSteadyFreq.ToString("F2")+"\n"+
                            "Kill Shot DPS: "+killShot.dps.ToString("F2")+" -> "+killShotSub20NewDPS.ToString("F2")+"\n"+
                            "Steady Shot DPS: "+steadyShot.dps.ToString("F2")+" -> "+killShotSub20NewSteadyDPS.ToString("F2")+"\n"+
                            "DPS Gain when switched: " + killShotSub20Gain.ToString("F2")+"\n"+
                            "Time spent sub-20%: " + killShotSub20TimeSpent.ToString("P2"));
            dictValues.Add("Aspect Loss", aspectBeastLostDPS.ToString("F2") + "*" +
                            "Hawk Uptime: " + aspectUptimeHawk.ToString("P2") + "\n" + 
                            "Viper Uptime: " + aspectUptimeViper.ToString("P2") + "\n" + 
                            "Beast Uptime: " + aspectUptimeBeast.ToString("P2") + "\n" +
                            "Viper Damage Penalty: " + aspectViperPenalty.ToString("P2"));

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
				case "Health": return BasicStats.Health;
                case "Crit Rating": return BasicStats.CritRating;
				case "Hit Rating": return BasicStats.HitRating;
                case "Haste Rating": return BasicStats.HasteRating;
				case "Mana": return BasicStats.Mana;
			}
			return 0;
		}
    }
}