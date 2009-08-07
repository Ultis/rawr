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
		private Stats _petStats;
		private float _baseAttackSpeed;
		private double _PetBaseDPS;
		private double _PetSpecialDPS;
		private double _PetKillCommandDPS;
        private double _autoshotDPS;
        private double _wildQuiverDPS;
        private double _customDPS;
	
        
        #region HasteStats
        public double hasteFromTalentsStatic { get; set; }
        public double hasteFromProcs { get; set; }
        public double hasteFromTalentsProc { get; set; }
        public double hasteEffectsTotal { get; set; }
        public double hasteFromBase { get; set; }
   		public double hasteFromRating { get; set; }
   		public double hasteFromRangedBuffs { get; set; }
        #endregion   		
   		
        #region RAP Stats
        public double apTotal { get; set; }
		public double apFromBase { get; set; }
		public double apFromAgil { get; set; }
		public double apFromCarefulAim { get; set; }
		public double apFromHunterVsWild { get; set; }
		public double apFromGear { get; set; }
		public double apFromBloodFury { get; set; }
		public double apFromAspectOfTheHawk { get; set; }
		public double apFromAspectMastery { get; set; }
		public double apFromFuriousHowl { get; set; }
        public double apFromProc { get; set; }
		public double apFromCallOfTheWild { get; set; }
		public double apFromTrueshotAura { get; set; }
		public double apFromHuntersMark {get; set;}
        public double apFromExposeWeakness { get; set; }
        #endregion
        
        #region Hit Stats
        public double hitOverall {get; set;}
        public double hitBase {get; set;}
        public double hitRating {get; set;}
        public double hitFromTalents {get; set;}
        public double hitFromBuffs {get; set;}
        public double hitLevelAdjustment {get; set;}       
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
        public double critfromDepression  {get; set;}
        #endregion
        
        public double damageReductionFromArmor  {get; set;}

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
        public ShotData readiness = new ShotData(Shots.Readiness, false, false);
        public ShotData beastialWrath = new ShotData(Shots.BeastialWrath, false, false);
        public ShotData bloodFury = new ShotData(Shots.BloodFury, false, false);
        public ShotData berserk = new ShotData(Shots.Berserk, false, false);

        public ShotPriority priorityRotation = new ShotPriority();


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

		public Stats PetStats
		{
			get { return _petStats;}
			set { _petStats = value; }
		}
		public List<string> ActiveBuffs { get; set; }

		public double PetBaseDPS
		{
			get { return _PetBaseDPS; }
			set { _PetBaseDPS = value; }
		}

		public double PetSpecialDPS
		{
			get { return _PetSpecialDPS; }
			set { _PetSpecialDPS = value; }
		}

		public double PetKillCommandDPS
		{
			get { return _PetKillCommandDPS; }
			set { _PetKillCommandDPS = value; }
		}

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
			dictValues.Add("Haste", hasteEffectsTotal.ToString("F2")+ " %*includes: \n"+
			               hasteFromBase.ToString("F2") + "% from quiver\n" +
			               hasteFromTalentsStatic.ToString("F2") +" % from talents\n"+ 
			               hasteFromProcs.ToString("F2") +" % from procs\n"+ 
			               hasteFromTalentsProc.ToString("F2") +" % from talented procs\n"+ 
			               hasteFromRating.ToString("F2") +" % from "+BasicStats.HasteRating.ToString("F0")+ " haste rating\n"+ 
			               hasteFromRangedBuffs.ToString("F2") +" % from buffs");
			dictValues.Add("Mana Per Second", manaRegenTotal.ToString("F2")+"*includes:\n" +
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
      						hitBase.ToString("P2") + " base hit chance \n" +
        					hitRating.ToString("P2") + " from rating \n" +
        					hitFromTalents.ToString("P2") + " from talents \n" +
        					hitFromBuffs.ToString("P2") + " from buffs \n" +
        					hitLevelAdjustment.ToString("P2") + " level penalty");
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
			               critfromDepression.ToString("P2") + " from depression");
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
                            apFromProc.ToString("F0") + " from proc effects \n" +
                            apFromCallOfTheWild.ToString("P2") + " from Call of the Wild \n" +
                            apFromTrueshotAura.ToString("P2") + " from Trueshot Aura \n" +
                            apFromHuntersMark.ToString("F0") + " from Hunter's Mark \n" +
                            apFromExposeWeakness.ToString("F0") + " from Expose Weakness");
            dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
			
            // Pet Stats						
			dictValues.Add("Pet Attack Power", PetStats.AttackPower.ToString("F0"));
			dictValues.Add("Pet Hit Percentage", PetStats.PhysicalHit.ToString("P2"));
			dictValues.Add("Pet Crit Percentage", PetStats.PhysicalCrit.ToString("P2"));
			dictValues.Add("Pet Base DPS", PetBaseDPS.ToString("F2"));
			dictValues.Add("Pet Special DPS", PetSpecialDPS.ToString("F2"));

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
            dictValues.Add("Proc DPS", "?");
            dictValues.Add("Kill Shot low HP gain", "?");
            dictValues.Add("Aspect Loss", "?");

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