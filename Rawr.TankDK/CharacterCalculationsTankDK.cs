using System;
using System.Collections.Generic;

namespace Rawr.TankDK {
    // Reminder: This is the character totals based on all gear and talents.  Apply the weights here.
    public enum CalculationType
    {
        SMT = 0,
        Burst = 1,
    }

    class CharacterCalculationsTankDK : CharacterCalculationsBase 
    {
        public CalculationType cType;
        public override float OverallPoints 
        { 
            get 
            {
                if (cType == CalculationType.Burst)
                {
                    return (BurstTime + ReactionTime);
                }
                else
                {
                    return ((Survival * SurvivalWeight) + Mitigation + (Threat * ThreatWeight)); 
                }
            } 
            set 
            { }
        }

        public Stats BasicStats { get; set; }
        public int TargetLevel { get; set; }

        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }

        public float Survival { get; set; }
        public float Mitigation { get; set; }
        public float Threat { get; set; }

        public float SurvivalWeight { get; set; }
        public float ThreatWeight { get; set; }

        public float ArmorDamageReduction { get; set; }
        public float Armor { get; set; }

        public float Crit { get; set; }
        public float Defense { get; set; }
        public float Resilience { get; set; }
        public float DefenseRating { get; set; }
        public float DefenseRatingNeeded { get; set; }

        public float TargetMiss { get; set; }
        public float TargetDodge { get; set; }
        public float TargetParry { get; set; }

        public float BurstTime { get; set; }
        public float ReactionTime { get; set; }


        public float Expertise { get; set; }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints 
        {
            get 
            {
                if (cType == CalculationType.Burst)
                {
                    return new float[] {BurstTime, ReactionTime, 0f};
                }
                else
                {
                    return new float[] { Survival * SurvivalWeight, Mitigation, Threat * ThreatWeight };
                }
            }
            set 
            { 
                _subPoints = value; 
            }
        }

        /*
        private float _whiteDPS;
        public float WhiteDPS {
            get { return _whiteDPS; }
            set { _whiteDPS = value; }
        }
        private float _NecrosisDPS;
        public float NecrosisDPS {
            get { return _NecrosisDPS; }
            set { _NecrosisDPS = value; }
        }
        private float _BCBDPS;
        public float BCBDPS {
            get { return _BCBDPS; }
            set { _BCBDPS = value; }
        }
        private float _DeathCoilDPS;
        public float DeathCoilDPS {
            get { return _DeathCoilDPS; }
            set { _DeathCoilDPS = value; }
        }
        private float _IcyTouchDPS;
        public float IcyTouchDPS {
            get { return _IcyTouchDPS; }
            set { _IcyTouchDPS = value; }
        }
        private float _PlagueStrikeDPS;
        public float PlagueStrikeDPS {
            get { return _PlagueStrikeDPS; }
            set { _PlagueStrikeDPS = value; }
        }
        private float _FrostFeverDPS;
        public float FrostFeverDPS {
            get { return _FrostFeverDPS; }
            set { _FrostFeverDPS = value; }
        }
        private float _BloodPlagueDPS;
        public float BloodPlagueDPS {
            get { return _BloodPlagueDPS; }
            set { _BloodPlagueDPS = value; }
        }
        private float _ScourgeStrikeDPS;
        public float ScourgeStrikeDPS {
            get { return _ScourgeStrikeDPS; }
            set { _ScourgeStrikeDPS = value; }
        }
        private float _UnholyBlightDPS;
        public float UnholyBlightDPS {
            get { return _UnholyBlightDPS; }
            set { _UnholyBlightDPS = value; }
        }
        private float _RuneStrikeDPS;
        public float RuneStrikeDPS {
            get { return _RuneStrikeDPS; }
            set { _RuneStrikeDPS = value; }
        }
        private float _BloodwormsDPS;
        public float BloodwormsDPS {
            get { return _BloodwormsDPS; }
            set { _BloodwormsDPS = value; }
        }
        private float _OtherDPS;
        public float OtherDPS {
            get { return _OtherDPS; }
            set { _OtherDPS = value; }
        }
        private float _FrostStrikeDPS;
        public float FrostStrikeDPS {
            get { return _FrostStrikeDPS; }
            set { _FrostStrikeDPS = value; }
        }
        private float _HowlingBlastDPS;
        public float HowlingBlastDPS {
            get { return _HowlingBlastDPS; }
            set { _HowlingBlastDPS = value; }
        }
        private float _DeathNDecayDPS;
        public float DeathNDecayDPS {
            get { return _DeathNDecayDPS; }
            set { _DeathNDecayDPS = value; }
        }
        private float _ObliterateDPS;
        public float ObliterateDPS {
            get { return _ObliterateDPS; }
            set { _ObliterateDPS = value; }
        }
        private float _DeathStrikeDPS;
        public float DeathStrikeDPS {
            get { return _DeathStrikeDPS; }
            set { _DeathStrikeDPS = value; }
        }
        private float _BloodStrikeDPS;
        public float BloodStrikeDPS {
            get { return _BloodStrikeDPS; }
            set { _BloodStrikeDPS = value; }
        }
        private float _HeartStrikeDPS;
        public float HeartStrikeDPS {
            get { return _HeartStrikeDPS; }
            set { _HeartStrikeDPS = value; }
        }
        private float _DRWDPS;
        public float DRWDPS {
            get { return _DRWDPS; }
            set { _DRWDPS = value; }
        }
        private float _WanderingPlagueDPS;
        public float WanderingPlagueDPS {
            get { return _WanderingPlagueDPS; }
            set { _WanderingPlagueDPS = value; }
        }
        private float _MHweaponDamage;
        public float MHWeaponDamage {
            get { return _MHweaponDamage; }
            set { _MHweaponDamage = value; }
        }
        private float _OHWeaponDamage;
        public float OHWeaponDamage {
            get { return _OHWeaponDamage; }
            set { _OHWeaponDamage = value; }
        }
        private float _MHattackSpeed;
        public float MHAttackSpeed {
            get { return _MHattackSpeed; }
            set { _MHattackSpeed = value; }
        }
        private float _OHattackSpeed;
        public float OHAttackSpeed {
            get { return _OHattackSpeed; }
            set { _OHattackSpeed = value; }
        }
        private float _critChance;
        public float CritChance {
            get { return _critChance; }
            set { _critChance = value; }
        }
        private float _SpellCritChance;
        public float SpellCritChance {
            get { return _SpellCritChance; }
            set { _SpellCritChance = value; }
        }
        private float _avoidedAttacks;
        public float AvoidedAttacks {
            get { return _avoidedAttacks; }
            set { _avoidedAttacks = value; }
        }
        private float _dodgedMHAttacks;
        public float DodgedMHAttacks {
            get { return _dodgedMHAttacks; }
            set { _dodgedMHAttacks = value; }
        }
        private float _dodgedOHAttacks;
        public float DodgedOHAttacks {
            get { return _dodgedOHAttacks; }
            set { _dodgedOHAttacks = value; }
        }
        private float _dodgedAttacks;
        public float DodgedAttacks {
            get { return _dodgedAttacks; }
            set { _dodgedAttacks = value; }
        }
        private float _missedAttacks;
        public float MissedAttacks {
            get { return _missedAttacks; }
            set { _missedAttacks = value; }
        }
        private float _enemyMitigation;
        public float EnemyMitigation {
            get { return _enemyMitigation; }
            set { _enemyMitigation = value; }
        }
        private float _effectiveArmor;
        public float EffectiveArmor {
            get { return _effectiveArmor; }
            set { _effectiveArmor = value; }
        }
        private float _MHExpertise;
        public float MHExpertise {
            get { return _MHExpertise; }
            set { _MHExpertise = value; }
        }
        private float _OHExpertise;
        public float OHExpertise {
            get { return _OHExpertise; }
            set { _OHExpertise = value; }
        }
        */

        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Chance to be Crit": return Crit; // Def cap chance to be critted by boss.  For optimization this needs to be  <= 0
                case "Avoidance %": return (Miss + Parry + Dodge); // Another duplicate math location?
                case "+Hit": return (1f - TargetMiss) * 100.0f; // +Hit related
                case "Target Parry %": return TargetParry * 100.0f; // Expertise related.
                case "Target Dodge %": return TargetDodge * 100.0f; // Expertise related.
                case "Damage Reduction %": return ArmorDamageReduction * 100.0f; // % Damage reduction by Armor
                case "Armor": return Armor; // Raw Armor
                case "Health": return BasicStats.Health;
                case "Burst Time": return BurstTime;
                case "Reaction Time": return ReactionTime;
                default: return 0.0f;
            }
        }
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["Miss"] = Miss.ToString("F2") + "%";
            dict["Dodge"] = Dodge.ToString("F2") + "%";
            dict["Parry"] = Parry.ToString("F2") + "%";
            dict["Armor Damage Reduction"] = (ArmorDamageReduction * 100.0f).ToString("F2") + "%";

            dict["Total Avoidance"] = (Miss + Parry + Dodge).ToString("F2") + "%"; // Another duplicate math location.
            dict["Burst Time"] = BurstTime.ToString("F2") + " sec";
            dict["Reaction Time"] = ReactionTime.ToString("F2") + " sec";

            dict["Health"] = BasicStats.Health.ToString("F0");
            dict["Armor"] = BasicStats.Armor.ToString("F0");
            dict["Strength"] = BasicStats.Strength.ToString("F0");
            dict["Agility"] = BasicStats.Agility.ToString("F0");
            dict["Stamina"] = BasicStats.Stamina.ToString("F0");
            dict["Hit Rating"] = BasicStats.HitRating.ToString("F0");
            dict["Haste Rating"] = BasicStats.HasteRating.ToString("F0");
            dict["Crit Rating"] = BasicStats.CritRating.ToString("F0");
            dict["Physical Crit"] = (BasicStats.PhysicalCrit * 100f).ToString("F2");
            dict["Expertise"] = Expertise.ToString("F0");
            dict["Attack Power"] = BasicStats.AttackPower.ToString("F0");
            dict["Armor Penetration"] = (BasicStats.ArmorPenetration * 100f).ToString("F2") + "%";
            dict["Armor Penetration Rating"] = BasicStats.ArmorPenetrationRating.ToString("F0");

            dict["Overall Points"] = OverallPoints.ToString("F1"); 
            dict["Mitigation Points"] = String.Format("{0:0.0}", Mitigation); // Unmodified Mitigation.
            dict["Survival Points"] = String.Format("{0:0.0}", Survival); // Unmodified Survival
            dict["Threat Points"] = String.Format("{0:0.0}", Threat); // Unmodified Threat

            dict["Crit"] = Crit.ToString("F2");
            dict["Defense"] = Defense.ToString("F0");
            dict["Resilience"] = Resilience.ToString("F0");
            dict["Defense Rating"] = DefenseRating.ToString("F0");
            dict["Defense Rating needed"] = DefenseRatingNeeded.ToString("F0");

            dict["Target Miss"] = (TargetMiss * 100.0f).ToString("F1") + "%";
            dict["Target Dodge"] = (TargetDodge * 100.0f).ToString("F1") + "%";
            dict["Target Parry"] = (TargetParry * 100.0f).ToString("F1") + "%";

            dict["Threat"] = Threat.ToString("F1"); // Unmodified Threat.
            dict["Overall"] = OverallPoints.ToString("F1");  
            dict["Modified Survival"] = (Survival * SurvivalWeight).ToString("F1"); // another place of duplicate math.
            dict["Modified Mitigation"] = (Mitigation).ToString("F1");
            dict["Modified Threat"] = (Threat * ThreatWeight).ToString("F1"); // another place of duplicate math.

            return dict;
        }
    }
}
