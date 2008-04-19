using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class CharacterCalculationsProtWarr : CharacterCalculationsBase
    {
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
        private float _threatScale;
        public float ThreatScale
        {
            get { return _threatScale; }
            set { _threatScale = value; }
        }

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

        private float _mitigation;
        public float Mitigation
        {
            get { return _mitigation; }
            set { _mitigation = value; }
        }

        private float _cappedMitigation;
        public float CappedMitigation
        {
            get { return _cappedMitigation; }
            set { _cappedMitigation = value; }
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

        private float _cappedCritReduction;
        public float CappedCritReduction
        {
            get { return _cappedCritReduction; }
            set { _cappedCritReduction = value; }
        }

        private float _crushChance;
        public float CrushChance
        {
            get { return _crushChance; }
            set { _crushChance = value; }
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

        public float NatureSurvivalPoints { get; set; }
        public float FrostSurvivalPoints { get; set; }
        public float FireSurvivalPoints { get; set; }
        public float ShadowSurvivalPoints { get; set; }
        public float ArcaneSurvivalPoints { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
            float levelDifference = 0.2f * (TargetLevel - 70);

            dictValues["Nature Resist"] = (BasicStats.NatureResistance + BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance + BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance + BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance + BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Dodge Rating", BasicStats.DodgeRating.ToString());
            dictValues.Add("Parry Rating", BasicStats.ParryRating.ToString());
            dictValues.Add("Block Rating", BasicStats.BlockRating.ToString());
            dictValues.Add("Block Value", BlockValue.ToString());
            dictValues.Add("Defense Rating", BasicStats.DefenseRating.ToString());
            dictValues.Add("Resilience", BasicStats.Resilience.ToString());
            dictValues.Add("Dodge", Dodge.ToString() + "%");
            dictValues.Add("Parry", Parry.ToString() + "%");
            if (BlockOverCap > 0f)
                dictValues.Add("Block", Block.ToString()
                + string.Format("%*Over the crush cap by {0}% block. (Total is {1}% block)", BlockOverCap, Block + BlockOverCap));
            else
                dictValues.Add("Block", Block.ToString()
                + string.Format("%*(Total is {0}% block)", Block + BlockOverCap));
            dictValues.Add("Miss", Miss.ToString() + "%");
            if (BasicStats.Armor == armorCap)
                dictValues.Add("Mitigation", Mitigation.ToString()
                    + string.Format("%*Exactly at the armor cap against level {0} mobs.", TargetLevel));
            else if (BasicStats.Armor > armorCap)
                dictValues.Add("Mitigation", Mitigation.ToString()
                    + string.Format("%*Over the armor cap by {0} armor.", BasicStats.Armor - armorCap));
            else
                dictValues.Add("Mitigation", Mitigation.ToString()
                    + string.Format("%*Short of the armor cap by {0} armor.", armorCap - BasicStats.Armor));
            dictValues.Add("Avoidance", DodgePlusMissPlusParry.ToString() + "%");
            dictValues.Add("Avoidance + Block", DodgePlusMissPlusParryPlusBlock.ToString() + "%");
            dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
            dictValues.Add("Damage Taken", DamageTaken.ToString() + "%");
            if (CritReduction == (5f + levelDifference))
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
            else if (CritReduction < (5f + levelDifference))
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
                    Math.Ceiling(((5f + levelDifference) - CritReduction) * 60f), Math.Ceiling(((5f + levelDifference) - CritReduction) * 39.423f)));
            else
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + string.Format("%*Uncrittable by bosses. {0} defense rating or {1} resilience over the crit cap.",
                    Math.Floor(((5f + levelDifference) - CritReduction) * -60f), Math.Floor(((5f + levelDifference) - CritReduction) * -39.423f)));
            dictValues.Add("Chance Crushed", CrushChance.ToString() + "%");
            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Mitigation Points", MitigationPoints.ToString());
            dictValues.Add("Survival Points", SurvivalPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();

            float critRating = BasicStats.CritRating;
            if (Calculations.CachedCharacter.ActiveBuffs.Contains("Improved Judgement of the Crusade"))
                critRating -= 66.24f;
            //critRating -= 131.4768f; //Base 5%

            dictValues["Strength"] = BasicStats.Strength.ToString();
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues["Hit Rating"] = BasicStats.HitRating.ToString();
            dictValues["Expertise Rating"] = BasicStats.ExpertiseRating.ToString();
            dictValues["Haste Rating"] = BasicStats.HasteRating.ToString();
            dictValues["Armor Penetration"] = BasicStats.ArmorPenetration.ToString();
            dictValues["Crit Rating"] = critRating.ToString();
            dictValues["Weapon Damage"] = BasicStats.WeaponDamage.ToString();


            dictValues["Limited Threat"] = (ThreatPoints / ThreatScale).ToString();
            dictValues["Unlimited Threat"] = (UnlimitedThreat / ThreatScale).ToString();
            dictValues["Missed Attacks"] = AvoidedAttacks.ToString();

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            /*
             * 		"Health",
                    "Hit Rating",
                    "Expertise Rating",
                    "Haste Rating",
                    "Missed Attacks",
                    "Unlimited Threat",
                    "Limited Threat",
                    "Mitigation % from Armor",
                    "Avoidance %",
                    "% Chance to be Crit",
                    "% to be Crushed",
                    "Nature Survival",
                    "Fire Survival",
                    "Frost Survival",
                    "Shadow Survival",
                    "Arcane Survival",
             */
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Mitigation % from Armor": return Mitigation;
                case "Avoidance %": return DodgePlusMissPlusParry;
                case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 70))) - CritReduction);
                case "% to be Crushed": return CrushChance;
                case "Nature Survival": return NatureSurvivalPoints;
                case "Fire Survival": return FireSurvivalPoints;
                case "Frost Survival": return FrostSurvivalPoints;
                case "Shadow Survival": return ShadowSurvivalPoints;
                case "Arcane Survival": return ArcaneSurvivalPoints;
            }
            return 0f;
        }
    }
}