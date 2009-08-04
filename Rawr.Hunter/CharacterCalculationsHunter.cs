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
		private float _RAP;
		private float _baseAttackSpeed;
        private float _steadySpeed;
		private double _PetBaseDPS;
		private double _PetSpecialDPS;
		private double _PetKillCommandDPS;
        private double _autoshotDPS;
        private double _explosiveShotDPS;
        private double _chimeraShotDPS;
        private double _killshotDPS;
        private double _silencingshotDPS;
        private double _blackDPS;

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
        public double RAPtotal { get; set; }
		public double apFromBase { get; set; }
		public double apFromAgil { get; set; }
		public double apFromCarefulAim { get; set; }
		public double apFromHunterVsWild { get; set; }
		public double apFromGear { get; set; }
		public double apFromBloodFury { get; set; }
		public double apFromAspectOfTheHawk { get; set; }
		public double apFromAspectMastery { get; set; }
		public double apFromFuriousHowl { get; set; }
		public double apFromCallOfTheWild { get; set; }
		public double apFromTrueshotAura { get; set; }
		public double apFromHuntersMark {get; set;}
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
        public double critBase  {get; set;}
        public double critFromAgi  {get; set;}
        public double critFromRating  {get; set;}
        public double critFromTalents  {get; set;}
        public double critFromBuffs  {get; set;}
        public double critfromDepression  {get; set;}
        #endregion
        
        public double damageReductionFromArmor  {get; set;}

        public double SilencingDPS
        {
            get { return _silencingshotDPS; }
            set { _silencingshotDPS = value; }
        }
        
        #region Steady Shot
        public double steadyCrit {get; set;}
        public double steadyHit {get; set;}
        public double steadyDamageNormal {get; set;}
        public double steadyDamageCrit {get; set;}
        public double steadyDamageTotal {get; set;}
        public double steadyDamagePerMana {get; set;}
        public double steadyDPS {get; set;}
        #endregion
        
        #region Arcane Shot
        public double arcaneCrit  {get; set;}
        public double arcaneHit  {get; set;}
        public double arcaneDamageNormal  {get; set;}
        public double arcaneDamageCrit  {get; set;}
        public double arcaneDamageTotal  {get; set;}
        public double arcaneDamagePerMana  {get; set;}
        public double arcaneDPS  {get; set;} 
        #endregion
        
        #region Multi Shot
        public double multiDPCD {get; set;}
        
        #endregion
        
        #region Aimed Shot
        public double aimedDPCD {get; set;}
        
        #endregion
        
        public double SerpentDPS {get; set; }
        
        #region mana regen
        public double manaRegenGearBuffs {get; set;}
        public double manaRegenBase {get; set;}
        public double manaRegenReplenishment {get; set;}
        public double manaRegenTotal { get; set; }
        
        
        #endregion

        public double BlackDPS
        {
            get { return _blackDPS; }
            set { _blackDPS = value; }
        }
        
		public float BaseAttackSpeed
		{
			get { return _baseAttackSpeed; }
			set { _baseAttackSpeed = value; }
		}

        public float SteadySpeed
        {
            get { return _steadySpeed; }
            set { _steadySpeed = value; }
        }

        public double AutoshotDPS
        {
            get { return _autoshotDPS; }
            set { _autoshotDPS = value; }
        }

        public double ExplosiveShotDPS
        {
            get { return _explosiveShotDPS; }
            set { _explosiveShotDPS = value; }
        }

        public double ChimeraShotDPS
        {
            get { return _chimeraShotDPS; }
            set { _chimeraShotDPS = value; }
        }

        private double _aimedDPS;
        public double AimedShotDPS
        {
            get { return _aimedDPS; }
            set { _aimedDPS = value; }
        }

        public double KillDPS
        {
            get { return _killshotDPS; }
            set { _killshotDPS = value; }
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

		public float RAP
		{
			get { return _RAP; }
			set { _RAP = value; }
		}

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
			
			dictValues.Add("Agility", BasicStats.Agility.ToString("F0"));
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString("F0"));
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString("F0"));
			dictValues.Add("Intellect", BasicStats.Intellect.ToString("F0"));
			dictValues.Add("Stamina", BasicStats.Stamina.ToString("F0"));
			dictValues.Add("Armor", BasicStats.Armor.ToString("F0"));
			dictValues.Add("Haste", hasteEffectsTotal.ToString("F2")+ " %*includes: \n"+
			               hasteFromBase.ToString("F0") + "% from quiver\n" +
			               hasteFromTalentsStatic.ToString("F0") +" % from talents\n"+ 
			               hasteFromProcs.ToString("F0") +" % from procs\n"+ 
			               hasteFromTalentsProc.ToString("F0") +" % from talented procs\n"+ 
			               hasteFromRating.ToString("F0") +" % from "+BasicStats.HasteRating.ToString("F0")+ " haste rating\n"+ 
			               hasteFromRangedBuffs.ToString("F0") +" % from buffs");
			dictValues.Add("Armor Penetration", BasicStats.ArmorPenetrationRating.ToString() + "* Enemy's Damage Reduction from armor: " + damageReductionFromArmor.ToString("P2"));
			dictValues.Add("Mana Per Second", manaRegenTotal.ToString("F0")+"*includes:\n" +
			              	manaRegenBase.ToString("F0")+" from base spirit regen\n" +
			              	manaRegenGearBuffs.ToString("F0")+" from mp5 gear & buffs\n" +
			              	manaRegenReplenishment.ToString("F0")+" from replenishment\n");
			dictValues.Add("Mana", BasicStats.Mana.ToString("F0"));
			dictValues.Add("Health", BasicStats.Health.ToString("F0"));
			dictValues.Add("Hit Percentage", hitOverall.ToString("P2") + "*includes: \n" +
      						hitBase.ToString("P2") + " base hit chance \n" +
        					hitRating.ToString("P2") + " from rating \n" +
        					hitFromTalents.ToString("P2") + " from talents \n" +
        					hitFromBuffs.ToString("P2") + " from buffs \n" +
        					hitLevelAdjustment.ToString("P2") + " level penalty");
			
			
			
			dictValues.Add("Crit Percentage", critRateOverall.ToString("P2") + "*includes: \n" +
			               critBase.ToString("P2") + "base crit \n" +
			               critFromAgi.ToString("P2") + "from Agility \n" +
			               critFromRating.ToString("P2") + "from Rating \n" +
			               critFromTalents.ToString("P2") + " from Talents \n" +
			               critFromBuffs.ToString("P2") + " from Buffs \n" +
			               critfromDepression.ToString("P2") + " from Depression");
			
			
			
			dictValues.Add("Pet Attack Power", PetStats.AttackPower.ToString("F0"));
			dictValues.Add("Pet Hit Percentage", PetStats.PhysicalHit.ToString("P2"));
			dictValues.Add("Pet Crit Percentage", PetStats.PhysicalCrit.ToString("P2"));
			dictValues.Add("Pet Base DPS", PetBaseDPS.ToString("F2"));
			dictValues.Add("Pet Special DPS", PetSpecialDPS.ToString("F2"));
			dictValues.Add("Ranged AP", RAPtotal.ToString("F0") + "*includes: \n"+
			               	apFromBase.ToString("F0")+" from base \n"+
							apFromAgil.ToString("F0")+" from Agility \n"+
							apFromCarefulAim.ToString("F0")+" from CarefulAim \n"+
							apFromHunterVsWild.ToString("F0")+" from HunterVsWild \n"+
							apFromGear.ToString("F0")+" from +ap gear & buffs \n"+
							apFromBloodFury.ToString("F0")+" from racials \n"+
							apFromAspectOfTheHawk.ToString("F0")+" from Aspect of the Hawk \n"+
							apFromAspectMastery.ToString("F0")+" from Aspect Mastery \n"+
							apFromFuriousHowl.ToString("F0")+" from Furious Howl \n"+
							apFromCallOfTheWild.ToString("F0")+"% from CallOfTheWild \n"+
							apFromTrueshotAura.ToString("F0")+"% from Trueshot Aura \n"+
							apFromHuntersMark.ToString("F0") + " from Hunter's Mark");
			dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
            dictValues.Add("Steady Speed", SteadySpeed.ToString("F2"));
			dictValues.Add("Hunter Total DPS", HunterDpsPoints.ToString("F2"));
			dictValues.Add("Pet DPS", PetDpsPoints.ToString("F2"));
			dictValues.Add("Overall DPS", OverallPoints.ToString("F2"));

            dictValues.Add("Autoshot DPS", AutoshotDPS.ToString("F2"));
            
            dictValues.Add("Auto Shot", AutoshotDPS.ToString("F2"));
			dictValues.Add("Steady Shot", steadyDPS.ToString("F2"));
			dictValues.Add("Serpent Sting", SerpentDPS.ToString("F2"));
            dictValues.Add("Silencing Shot", SilencingDPS.ToString("F2"));
			dictValues.Add("Arcane Shot", arcaneDPS.ToString("F2"));
			dictValues.Add("Explosive Shot", ExplosiveShotDPS.ToString("F2"));
			dictValues.Add("Chimera Shot", ChimeraShotDPS.ToString("F2"));
			dictValues.Add("Aimed Shot", aimedDPCD.ToString("F2"));
			dictValues.Add("Multi Shot", multiDPCD.ToString("F2"));
            dictValues.Add("Black Arrow", AutoshotDPS.ToString("F2"));
            dictValues.Add("Kill Shot", KillDPS.ToString("F2"));

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