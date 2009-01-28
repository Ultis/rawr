using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class CharacterCalculationsProtWarr : CharacterCalculationsBase
    {
        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float MitigationPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float ThreatPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }
        #endregion

        #region Basic Stats and Scaling
        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        private int _targetLevel;
        public int TargetLevel
        {
            get { return _targetLevel; }
            set { _targetLevel = value; }
        }

        private float _threatScale;
        public float ThreatScale
        {
            get { return _threatScale; }
            set { _threatScale = value; }
        }
        #endregion

        #region Defensive Stats
        private float _defense;
        public float Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        private float _dodge;
        public float Dodge
        {
            get { return _dodge; }
            set { _dodge = value; }
        }

        private float _parry;
        public float Parry
        {
            get { return _parry; }
            set { _parry = value; }
        }

        private float _block;
        public float Block
        {
            get { return _block; }
            set { _block = value; }
        }

        private float _blockOverCap;
        public float BlockOverCap
        {
            get { return _blockOverCap; }
            set { _blockOverCap = value; }
        }

        private float _blockValue;
        public float BlockValue
        {
            get { return _blockValue; }
            set { _blockValue = value; }
        }

        private float _miss;
        public float Miss
        {
            get { return _miss; }
            set { _miss = value; }
        }

        private float _armorReduction;
        public float ArmorReduction
        {
            get { return _armorReduction; }
            set { _armorReduction = value; }
        }

        private float _guaranteedReduction;
        public float GuaranteedReduction
        {
            get { return _guaranteedReduction; }
            set { _guaranteedReduction = value; }
        }

        private float _dodgePlusMissPlusParry;
        public float DodgePlusMissPlusParry
        {
            get { return _dodgePlusMissPlusParry; }
            set { _dodgePlusMissPlusParry = value; }
        }

        private float _dodgePlusMissPlusParryPlusBlock;
        public float DodgePlusMissPlusParryPlusBlock
        {
            get { return _dodgePlusMissPlusParryPlusBlock; }
            set { _dodgePlusMissPlusParryPlusBlock = value; }
        }

        private float _totalMitigation;
        public float TotalMitigation
        {
            get { return _totalMitigation; }
            set { _totalMitigation = value; }
        }

        private float _damageTaken;
        public float DamageTaken
        {
            get { return _damageTaken; }
            set { _damageTaken = value; }
        }

        private float _critReduction;
        public float CritReduction
        {
            get { return _critReduction; }
            set { _critReduction = value; }
        }

        private float _critVulnerability;
        public float CritVulnerability
        {
            get { return _critVulnerability; }
            set { _critVulnerability = value; }
        }

        private float _missedAttacks;
        public float MissedAttacks
        {
            get { return _missedAttacks; }
            set { _missedAttacks = value; }
        }

        private float _avoidedAttacks;
        public float AvoidedAttacks
        {
            get { return _avoidedAttacks; }
            set { _avoidedAttacks = value; }
        }

        private float _dodgedAttacks;
        public float DodgedAttacks
        {
            get { return _dodgedAttacks; }
            set { _dodgedAttacks = value; }
        }

        private float _parriedAttacks;
        public float ParriedAttacks
        {
            get { return _parriedAttacks; }
            set { _parriedAttacks = value; }
        }

        private float _blockedAttacks;
        public float BlockedAttacks
        {
            get { return _blockedAttacks; }
            set { _blockedAttacks = value; }
        }
        #endregion

        #region Offensive Stats
        private float _hit;
        public float Hit
        {
            get { return _hit; }
            set { _hit = value; }
        }

        private float _crit;
        public float Crit
        {
            get { return _crit; }
            set { _crit = value; }
        }

        private float _expertise;
        public float Expertise
        {
            get { return _expertise; }
            set { _expertise = value; }
        }

        private float _haste;
        public float Haste
        {
            get { return _haste; }
            set { _haste = value; }
        }

        private float _armorPenetration;
        public float ArmorPenetration
        {
            get { return _armorPenetration; }
            set { _armorPenetration = value; }
        }

        private float _limitedThreat;
        public float LimitedThreat
        {
            get { return _limitedThreat; }
            set { _limitedThreat = value; }
        }

        private float _unlimitedThreat;
        public float UnlimitedThreat
        {
            get { return _unlimitedThreat; }
            set { _unlimitedThreat = value; }
        }

        private float _whiteThreat;
        public float WhiteThreat
        {
            get { return _whiteThreat; }
            set { _whiteThreat = value; }
        }

        private float _shieldSlamThreat;
        public float ShieldSlamThreat
        {
            get { return _shieldSlamThreat; }
            set { _shieldSlamThreat = value; }
        }

        private float _revengeThreat;
        public float RevengeThreat
        {
            get { return _revengeThreat; }
            set { _revengeThreat = value; }
        }

        private float _devastateThreat;
        public float DevastateThreat
        {
            get { return _devastateThreat; }
            set { _devastateThreat = value; }
        }

        private float _heroicStrikeThreat;
        public float HeroicStrikeThreat
        {
            get { return _heroicStrikeThreat; }
            set { _heroicStrikeThreat = value; }
        }

        #endregion

        #region Resist and Buffs
        public float NatureSurvivalPoints { get; set; }
        public float FrostSurvivalPoints { get; set; }
        public float FireSurvivalPoints { get; set; }
        public float ShadowSurvivalPoints { get; set; }
		public float ArcaneSurvivalPoints { get; set; }
		public List<Buff> ActiveBuffs { get; set; }
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Armor", string.Format("{0}*Reduces physical damage taken by {1:0.00%}", BasicStats.Armor, ArmorReduction));
            dictValues.Add("Defense", Defense.ToString() + string.Format("*Defense Rating {0}", BasicStats.DefenseRating));
            dictValues.Add("Dodge", string.Format("{0:0.00%}*Dodge Rating {1}", Dodge, BasicStats.DodgeRating));
            dictValues.Add("Parry", string.Format("{0:0.00%}*Parry Rating {1}", Parry, BasicStats.ParryRating));
            dictValues.Add("Block", string.Format("{0:0.00%}*Block Rating {1}", Block, BasicStats.BlockRating));
            dictValues.Add("Miss", string.Format("{0:0.00%}", Miss));
            dictValues.Add("Block Value", string.Format("{0}", BlockValue));
            dictValues.Add("Guaranteed Reduction", string.Format("{0:0.00%}", GuaranteedReduction));
            dictValues.Add("Avoidance", string.Format("{0:0.00%}", DodgePlusMissPlusParry));
            dictValues.Add("Avoidance + Block", string.Format("{0:0.00%}", DodgePlusMissPlusParryPlusBlock));
            dictValues.Add("Total Mitigation", string.Format("{0:0.00%}", TotalMitigation));
            dictValues.Add("Damage Taken", string.Format("{0:0.00%}", DamageTaken));
            dictValues.Add("Resilience",
                string.Format(@"{0}*Reduces periodic damage and chance to be critically hit by {1}%." + Environment.NewLine +
                                "Reduces the effect of mana-drains and the damage of critical strikes by {2}%.",
                                BasicStats.Resilience,
                                BasicStats.Resilience * ProtWarr.ResilienceRatingToCritReduction,
                                BasicStats.Resilience * ProtWarr.ResilienceRatingToCritReduction * 2));
            if (CritVulnerability > 0.0001f)
            {
                double defenseNeeded = Math.Ceiling((CritVulnerability / (ProtWarr.DefenseToCritReduction / 100.0f)) / ProtWarr.DefenseRatingToDefense);
                double resilienceNeeded = Math.Ceiling(CritVulnerability / (ProtWarr.ResilienceRatingToCritReduction / 100.0f));
                dictValues.Add("Chance to be Crit",
                    string.Format("{0:0.00%}*CRITTABLE! Short by {1:0} defense or {2:0} resilience to be uncrittable.",
                                    CritVulnerability, defenseNeeded, resilienceNeeded));
            }
            else
                dictValues.Add("Chance to be Crit", string.Format("{0:0.00%}*Chance to crit reduced by {1:0.00%}", CritVulnerability, CritReduction));


            dictValues["Nature Resist"] = (BasicStats.NatureResistance + BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance + BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance + BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance + BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();

            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues.Add("Hit", string.Format("{0:0.00%}*Hit Rating {1}", Hit, BasicStats.HitRating));
            dictValues.Add("Expertise", 
                string.Format("{0}*Expertise Rating {1}" + Environment.NewLine + "Reduces chance to be dodged or parried by {2:0.00%}.", 
                                Math.Round(BasicStats.ExpertiseRating * ProtWarr.ExpertiseRatingToExpertise + BasicStats.Expertise),
                                BasicStats.ExpertiseRating, Expertise));
            dictValues.Add("Haste", string.Format("{0:0.00%}*Haste Rating {1}", Haste, BasicStats.HasteRating));
            dictValues.Add("Armor Penetration", string.Format("{0:0.00%}*Armor Penetration Rating {1}", ArmorPenetration, BasicStats.ArmorPenetration));
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1}", Crit, BasicStats.CritRating));
            dictValues.Add("Weapon Damage", string.Format("{0}", BasicStats.WeaponDamage));
            dictValues.Add("Missed Attacks",
                string.Format("{0:0.00%}*Out of 100 attacks:" + Environment.NewLine + "Attacks Missed: {1:0.00%}" + Environment.NewLine +
                                "Attacks Dodged: {2:0.00%}" + Environment.NewLine + "Attacks Parried: {3:0.00%}",
                                AvoidedAttacks, MissedAttacks, DodgedAttacks, ParriedAttacks));

            dictValues.Add("Limited Threat",
                string.Format("{0:0.0}*White TPS: {1:0.00}" + Environment.NewLine + "Shield Slam Threat: {2:0.0}" + Environment.NewLine +
                                "Revenge Threat: {3:0.0}" + Environment.NewLine + "Devastate Threat: {4:0.0}",
                                LimitedThreat, WhiteThreat, ShieldSlamThreat, RevengeThreat, DevastateThreat));
            dictValues.Add("Unlimited Threat",
                string.Format("{0:0.0}*Heroic Strike TPS: {1:0.00}" + Environment.NewLine + "Shield Slam Threat: {2:0.0}" + Environment.NewLine +
                                "Revenge Threat: {3:0.0}" + Environment.NewLine + "Devastate Threat: {4:0.0}",
                                UnlimitedThreat, HeroicStrikeThreat, ShieldSlamThreat, RevengeThreat, DevastateThreat));

            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Mitigation Points", MitigationPoints.ToString());
            dictValues.Add("Survival Points", SurvivalPoints.ToString());
            dictValues.Add("Threat Points", ThreatPoints.ToString());

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Guaranteed Reduction %": return GuaranteedReduction;
                case "Avoidance %": return DodgePlusMissPlusParry;
                case "% Chance to be Crit": return (1.0f - CritVulnerability);
                case "Nature Survival": return NatureSurvivalPoints;
                case "Fire Survival": return FireSurvivalPoints;
                case "Frost Survival": return FrostSurvivalPoints;
                case "Shadow Survival": return ShadowSurvivalPoints;
                case "Arcane Survival": return ArcaneSurvivalPoints;
            }
            return 0.0f;
        }
    }
}