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
        //Drizz: Added
        private double _piercingShotsDPS;
        private double _piercingShotsDPSSteadyShot;
        private double _piercingShotsDPSAimedShot;
        private double _piercingShotsDPSChimeraShot;

        public PetCalculations pet { get; set; }

        #region Pet Stats
        public double petKillCommandDPS { get; set; }
        public double petKillCommandMPS { get; set; }
        public double petWhiteDPS { get; set; }
        public double petSpecialDPS { get; set; }
        public double petArmorDebuffs { get; set; }
        public double petTargetArmorReduction { get; set; }
        public double petTargetDodge { get; set; }
        public double ferociousInspirationDamageAdjust { get; set; }
        #endregion

        #region Pet Hit
        public double petHitFromBase { get; set; }
        public double petHitFromTargetDebuffs { get; set; }
        public double petHitFromFocusedAim { get; set; }
        public double petHitFromRacial { get; set; }
        public double petHitFromHunter { get; set; }
        public double petHitFromLevelAdjust { get; set; }
        public double petHitTotal { get; set; }
        public double petHitSpellTotal { get; set; }
        #endregion

        #region Pet Crit
        public double petCritFromBase { get; set; }
        public double petCritFromAgility { get; set; }
        public double petCritFromSpidersBite { get; set; }
        public double petCritFromFerocity { get; set; }
        public double petCritFromGear { get; set; }
        public double petCritFromBuffs { get; set; }
        public double petCritFromTargetDebuffs { get; set; }
        public double petCritFromDepression { get; set; }
        public double petCritFromCobraStrikes { get; set; }
        public double petCritTotalMelee { get; set; }
        public double petCritTotalSpecials { get; set; }
        #endregion

        #region Pet AP
        public double petAPFromStrength { get; set; }
        public double petAPFromHunterVsWild { get; set; }
        public double petAPFromTier9 { get; set; }
        public double petAPFromBuffs { get; set; }
        public double petAPFromFuriousHowl { get; set; }
        public double petAPFromHunterRAP { get; set; }
        public double petAPFromCallOfTheWild { get; set; }
        public double petAPFromRabidProc { get; set; }
        public double petAPFromSerenityDust { get; set; }
        public double petAPFromTrueShotAura { get; set; }
        public double petAPFromOutsideBuffs { get; set; }
        public double petAPFromAnimalHandler { get; set; }
        public double petAPFromAspectOfTheBeast { get; set; }
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
        public double apFromBuffs { get; set; }
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
        public double critFromRacial { get; set; }
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

        // stuff for rotation test
        //public double autoShotSpeed { get; set; }
        public double autoShotStaticSpeed { get; set; }
        public double quickShotsEffect { get; set; }

        // new shots data
        public ShotData aimedShot = new ShotData(Shots.AimedShot, true, true);
        public ShotData arcaneShot = new ShotData(Shots.ArcaneShot, true, true);
        public ShotData multiShot = new ShotData(Shots.MultiShot, true, true);
        // Drizz: Serpentsstings now Crits
        //public ShotData serpentSting = new ShotData(Shots.SerpentSting, false, true);
        public ShotData serpentSting = new ShotData(Shots.SerpentSting, true, true);
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


        #region Mana Regen
        public double manaRegenGearBuffs {get; set;}
        public double manaRegenConstantViper { get; set; }
        public double manaRegenRoarOfRecovery { get; set; }
        public double manaRegenRapidRecuperation { get; set; }
        public double manaRegenChimeraViperProc { get; set; }
        public double manaRegenInvigoration { get; set; }
        public double manaRegenHuntingParty { get; set; }
        public double manaRegenTargetDebuffs { get; set; }
        public double manaRegenTotal { get; set; }

        public double manaRegenViper { get; set; }
        public double manaRegenPotion { get; set; }
        #endregion

        #region Mana Usage
        public double manaUsageRotation { get; set; }
        public double manaUsageKillCommand { get; set; }
        public double manaUsageTotal { get; set; }

        public double manaChangeDuringViper { get; set; }
        public double manaChangeDuringNormal { get; set; }
        public double manaTimeToFull { get; set; }
        public double manaTimeToOOM { get; set; }
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

        //Drizz: Added 27-10-2009
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
                           critFromRacial.ToString("P2") + " from racial \n" +
                           critFromProcRating.ToString("P2") + " from proc rating \n" +
                           critFromLethalShots.ToString("P2") + " from Lethal Shots\n" +
                           critFromKillerInstincts.ToString("P2") + " from Killer Instincts\n" +
                           critFromMasterMarksman.ToString("P2") + " from Master Marksman\n" +
                           critFromMasterTactician.ToString("P2") + " from Master Tactician\n" +
			               critFromBuffs.ToString("P2") + " from target debuffs \n" +
			               critFromDepression.ToString("P2") + " from depression");
            dictValues.Add("Ranged AP", apTotal.ToString("F0") + "*includes:\n" +
                            apFromBase.ToString("F0") + " from base\n" +
                            apFromAgil.ToString("F0") + " from agility\n" +
                            apFromCarefulAim.ToString("F0") + " from Careful Aim\n" +
                            apFromHunterVsWild.ToString("F0") + " from Hunter vs Wild\n" +
                            apFromGear.ToString("F0") + " from +AP gear & buffs\n" +
                            apFromBloodFury.ToString("F0") + " from Orc racial\n" +
                            apFromAspectOfTheHawk.ToString("F0") + " from Aspect of the Hawk\n" +
                            apFromAspectMastery.ToString("F0") + " from Aspect Mastery\n" +
                            apFromFuriousHowl.ToString("F0") + " from Furious Howl\n" +
                            apFromProc.ToString("F0") + " from proc effects\n" +
                            apFromCallOfTheWild.ToString("P2") + " from Call of the Wild\n" +
                            apFromTrueshotAura.ToString("P2") + " from Trueshot Aura\n" +
                            apFromBuffs.ToString("P2") + " from buffs\n" +
                            apFromExposeWeakness.ToString("F0") + " from Expose Weakness\n" +
                            apFromDebuffs.ToString("F0") + " from target debuffs\n" +
                            "----------------------\n" +
                            apFromHuntersMark.ToString("F0") + " from Hunter's Mark\n");

            dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
			
            // Pet Stats						
            dictValues.Add("Pet Attack Power", pet.petStats.AttackPower.ToString("F0") + "*includes:\n" +
                            petAPFromStrength.ToString("F2") + " from strength\n" +
                            petAPFromHunterVsWild.ToString("F2") + " from Hunter vs Wild\n" +
                            petAPFromTier9.ToString("F2") + " from Tier 9 4-piece bonus\n" +
                            petAPFromBuffs.ToString("F2") + " from buffs\n" +
                            petAPFromFuriousHowl.ToString("F2") + " from Furious Howl\n" +
                            petAPFromHunterRAP.ToString("F2") + " from Hunter RAP\n" +
                            petAPFromCallOfTheWild.ToString("P2") + " from Call of the Wild\n" +
                            petAPFromRabidProc.ToString("P2") + " from Rabid\n" +
                            petAPFromSerenityDust.ToString("P2") + " from Serenity Dust\n" +
                            petAPFromTrueShotAura.ToString("P2") + " from True Shot Aura\n" +
                            petAPFromOutsideBuffs.ToString("P2") + " from outside buffs\n" +
                            petAPFromAnimalHandler.ToString("P2") + " from Animal Handler\n" +
                            petAPFromAspectOfTheBeast.ToString("P2") + " from Aspect of the Beast");
            dictValues.Add("Pet Hit Percentage", petHitTotal.ToString("P2") + "*includes:\n" +
                            petHitFromBase.ToString("P2") + " from base\n"+
                            petHitFromTargetDebuffs.ToString("P2") + " from target debuffs\n" +
                            petHitFromFocusedAim.ToString("P2") + " from Focused Aim\n" +
                            petHitFromRacial.ToString("P2") + " from Racial\n" +
                            petHitFromHunter.ToString("P2") + " from Hunter\n" +
                            petHitFromLevelAdjust.ToString("P2") + " from level penalty");
            dictValues.Add("Pet Melee Crit Percentage", petCritTotalMelee.ToString("P2") + "*includes:\n" +
                            petCritFromBase.ToString("P2") + " from base\n" +
                            petCritFromAgility.ToString("P2") + " from agility\n" +
                            petCritFromSpidersBite.ToString("P2") + " from Spider's Bite\n" +
                            petCritFromFerocity.ToString("P2") + " from Ferocity\n" +
                            petCritFromGear.ToString("P2") + " from gear\n" +
                            petCritFromBuffs.ToString("P2") + " from buffs\n" +
                            petCritFromTargetDebuffs.ToString("P2") + " from target debuffs\n" +
                            petCritFromDepression.ToString("P2") + " from depression");
            dictValues.Add("Pet Specials Crit Percentage", petCritTotalSpecials.ToString("P2") + "*includes:\n" +
                            petCritTotalMelee.ToString("P2") + " from melee crit\n" +
                            petCritFromCobraStrikes.ToString("P2") + " from Cobra Strikes");
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

            // Mana
            dictValues.Add("Mana Usage Per Second", manaUsageTotal.ToString("F2") + "*includes:\n" +
                           manaUsageRotation.ToString("F2") + " from shot rotation\n" +
                           manaUsageKillCommand.ToString("F2") + " from Kill Command");
            dictValues.Add("Mana Regen Per Second", manaRegenTotal.ToString("F2") + "*includes:\n" +
                           manaRegenGearBuffs.ToString("F2") + " from Gear and Buffs\n" +
                           manaRegenConstantViper.ToString("F2") + " from Aspect of the Viper\n" +
                           manaRegenRoarOfRecovery.ToString("F2") + " from Roar of Recovery\n" +
                           manaRegenRapidRecuperation.ToString("F2") + " from Rapid Recuperation\n" +
                           manaRegenChimeraViperProc.ToString("F2") + " from Chimera Viper String Proc\n" +
                           manaRegenInvigoration.ToString("F2") + " from Invigoration\n" +
                           manaRegenHuntingParty.ToString("F2") + " from Hunting Party\n" +
                           manaRegenTargetDebuffs.ToString("F2") + " from Target Debuffs");
            dictValues.Add("Potion Regen Per Second", manaRegenPotion.ToString("F2"));
            dictValues.Add("Viper Regen Per Second", manaRegenViper.ToString("F2"));
            dictValues.Add("Normal Change", manaChangeDuringNormal.ToString("F2"));
            dictValues.Add("Change during Viper", manaChangeDuringViper.ToString("F2"));
            dictValues.Add("Time to OOM", manaTimeToOOM.ToString("F2"));
            dictValues.Add("Time to Full", manaTimeToFull.ToString("F2"));
            dictValues.Add("Viper Damage Penalty", aspectViperPenalty.ToString("P2"));
            dictValues.Add("Viper Uptime", aspectUptimeViper.ToString("P2"));

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
                            "Beast Uptime: " + aspectUptimeBeast.ToString("P2"));
            dictValues.Add("Piercing Shots DPS", PiercingShotsDPS.ToString("F2") + "*" +
                            "Steady Shot: " + PiercingShotsDPSSteadyShot.ToString("F2") + "\n" +
                            "Aimed Shot: " + PiercingShotsDPSAimedShot.ToString("F2") + "\n" +
                            "Chimera Shot: " + PiercingShotsDPSChimeraShot.ToString("F2") + "\n");


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