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
        #endregion

        #region Offensive Stats
        private float _crit;
        public float Crit
        {
            get { return _crit; }
            set { _crit = value; }
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

        private float _windfuryThreat;
        public float WindfuryThreat
        {
            get { return _windfuryThreat; }
            set { _windfuryThreat = value; }
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
            int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
            float levelDifference = 0.2f * (TargetLevel - 80f);
            float targetCritReduction = 5f + levelDifference;
            float currentCritReduction = ((float)Math.Floor(
                (BasicStats.DefenseRating * WarriorConversions.DefenseRatingToDefense + BasicStats.Defense)) *
                 WarriorConversions.DefenseToCritReduction) +
                (BasicStats.Resilience * WarriorConversions.ResilienceRatingToCritReduction);
            int defToCap = 0, resToCap = 0;
            if (currentCritReduction < targetCritReduction)
            {
                while ((((float)Math.Floor(
                ((BasicStats.DefenseRating + defToCap) * WarriorConversions.DefenseRatingToDefense + BasicStats.Defense)) *
                 WarriorConversions.DefenseToCritReduction) +
                (BasicStats.Resilience * WarriorConversions.ResilienceRatingToCritReduction)) < targetCritReduction)
                    defToCap++;
                while ((((float)Math.Floor(
                (BasicStats.DefenseRating * WarriorConversions.DefenseRatingToDefense + BasicStats.Defense)) *
                 WarriorConversions.DefenseToCritReduction) +
                ((BasicStats.Resilience + resToCap) * WarriorConversions.ResilienceRatingToCritReduction)) < targetCritReduction)
                    resToCap++;
            }
            else if (currentCritReduction > targetCritReduction)
            {
                while ((((float)Math.Floor(
                ((BasicStats.DefenseRating + defToCap) * WarriorConversions.DefenseRatingToDefense + BasicStats.Defense)) *
                 WarriorConversions.DefenseToCritReduction) +
                (BasicStats.Resilience * WarriorConversions.ResilienceRatingToCritReduction)) > targetCritReduction)

                    defToCap--;
                while ((((float)Math.Floor(
                (BasicStats.DefenseRating * WarriorConversions.DefenseRatingToDefense + BasicStats.Defense)) *
                 WarriorConversions.DefenseToCritReduction) +
                ((BasicStats.Resilience + resToCap) * WarriorConversions.ResilienceRatingToCritReduction)) > targetCritReduction)

                    resToCap--;
                defToCap++;
                resToCap++;
            }

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Defense", Defense.ToString() +
                string.Format("*Defense Rating {0}", BasicStats.DefenseRating));
            dictValues.Add("Dodge", Dodge.ToString() +
                string.Format("%*Dodge Rating {0}", BasicStats.DodgeRating));
            dictValues.Add("Parry", Parry.ToString() +
                string.Format("%*Parry Rating {0}", BasicStats.ParryRating));
            if (BlockOverCap > 0f)
                dictValues.Add("Block", (Block + BlockOverCap).ToString()
                + string.Format("%*Block Rating {0}. Over the crush cap by {1}% block", BasicStats.BlockRating, BlockOverCap));
            else
                dictValues.Add("Block", Block.ToString()
                + string.Format("%*Block Rating {0}", BasicStats.BlockRating));
            dictValues.Add("Miss", Miss.ToString() + "%");
            dictValues.Add("Resilience", BasicStats.Resilience.ToString() +
                string.Format(@"*Reduces periodic damage and chance to be critically hit by {0}%.
Reduces the effect of mana-drains and the damage of critical strikes by {1}%.",
                BasicStats.Resilience * WarriorConversions.ResilienceRatingToCritReduction,
                BasicStats.Resilience * WarriorConversions.ResilienceRatingToCritReduction * 2));
            dictValues.Add("Block Value", BlockValue.ToString());

            #region Offensive Stats
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues["Hit"] = (BasicStats.HitRating * WarriorConversions.HitRatingToHit +
                BasicStats.PhysicalHit).ToString() +
                string.Format("%*Hit Rating {0}", BasicStats.HitRating);
            dictValues["Expertise"] = (Math.Round(BasicStats.ExpertiseRating * WarriorConversions.ExpertiseRatingToExpertise +
                BasicStats.Expertise)).ToString() +
                string.Format(@"*Expertise Rating {0}
Reduces chance to be dodged or parried by {1}%.", BasicStats.ExpertiseRating,
                Math.Round((BasicStats.ExpertiseRating * WarriorConversions.ExpertiseRatingToExpertise +
                BasicStats.Expertise) * WarriorConversions.ExpertiseToDodgeParryReduction));
            dictValues["Haste"] = (BasicStats.HasteRating * WarriorConversions.HasteRatingToHaste).ToString() +
                string.Format("%*Haste Rating {0}", BasicStats.HasteRating);
            dictValues["Armor Penetration"] = BasicStats.ArmorPenetration.ToString();
            dictValues["Crit"] = Crit.ToString() +
                string.Format("%*Crit Rating {0}", BasicStats.CritRating);
            dictValues["Weapon Damage"] = BasicStats.WeaponDamage.ToString();
            dictValues.Add("Missed Attacks", AvoidedAttacks.ToString() +
                string.Format(@"%*Out of 100 attacks:
Attacks Missed: {0}%
Attacks Dodged: {1}%
Attacks Parried: {2}%", MissedAttacks, DodgedAttacks, ParriedAttacks));
            dictValues.Add("Limited Threat", (LimitedThreat / ThreatScale).ToString() +
                string.Format(@"*White TPS: {0}
Shield Slam TPS: {1}
Revenge TPS: {2}
Devastate TPS: {3}
Windfury TPS: {4}", WhiteThreat, ShieldSlamThreat, RevengeThreat, DevastateThreat, WindfuryThreat));
            dictValues.Add("Unlimited Threat", (UnlimitedThreat / ThreatScale).ToString() +
                string.Format(@"*Heroic Strike TPS: {0}
Shield Slam TPS: {1}
Revenge TPS: {2}
Devastate TPS: {3}
Windfury TPS: {4}", HeroicStrikeThreat, ShieldSlamThreat, RevengeThreat, DevastateThreat, WindfuryThreat));
            #endregion

            dictValues["Nature Resist"] = (BasicStats.NatureResistance + BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance + BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance + BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance + BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
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
            if (defToCap == 0 && resToCap == 0)
            {
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
            }
            else if (defToCap + resToCap > 0)
            {
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + string.Format("%*CRITTABLE! Short by {0} defense rating ({1} defense) or {2} resilience to be uncrittable by bosses.",
                    defToCap, defToCap * WarriorConversions.DefenseRatingToDefense, resToCap));
            }
            else
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritReduction).ToString()
                    + string.Format("%*Uncrittable by bosses. {0} defense rating ({1} defense) or {2} resilience over the crit cap.",
                    -defToCap, -defToCap * WarriorConversions.DefenseRatingToDefense, - resToCap));

            dictValues.Add("Chance Crushed", CrushChance.ToString() + "%");
            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Mitigation Points", MitigationPoints.ToString());
            dictValues.Add("Survival Points", SurvivalPoints.ToString());
            dictValues.Add("Threat Points", ThreatPoints.ToString());

            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();

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
                case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 80f))) - CritReduction);
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