using System;
using System.Collections.Generic;
using Rawr.DK;

namespace Rawr.TankDK {
    // Reminder: This is the character totals based on all gear and talents.  Apply the weights here.
    public enum CalculationType
    {
        SMT = 0,
        Burst = 1,
    }

    public enum SMTSubPoints
    {
        Survival,
        Mitigation,
        Threat
    }

    public enum BurstSubPoints
    {
        Burst,
        Reaction,
        Threat
    }

    public  class CharacterCalculationsTankDK : CharacterCalculationsBase 
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
                    return ((Survival * SurvivalWeight) + (Mitigation * MitigationWeight) + (Threat * ThreatWeight)); 
                }
            } 
            set 
            { }
        }

        public StatsDK BasicStats { get; set; }
        public StatsDK SEStats { get; set; }
        public int TargetLevel { get; set; }

        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }

        public float PhysicalSurvival { get; set; }
        public float BleedSurvival { get; set; }
        public float MagicSurvival { get; set; }
        public float Survival { 
            get
            {
                // Moving this to out here.
                return PhysicalSurvival + BleedSurvival + MagicSurvival;
            }
        }

        public float CritMitigation { get; set; }
        public float AvoidanceMitigation { get; set; }
        public float ArmorMitigation { get; set; }
        public float DamageTakenMitigation { get; set; }
        public float HealsMitigation { get; set; }
        public float ImpedenceMitigation { get; set; }

        public float Mitigation { get; set; }
        public float Threat { get; set; }

        public float SurvivalWeight { get; set; }
        public float MitigationWeight { get; set; }
        public float ThreatWeight { get; set; }

        public float MagicDamageReduction { get; set; }
        public float ArmorDamageReduction { get; set; }
        public float Armor { get; set; }

        /// <summary>
        /// Chance to be crit.
        /// </summary>
        public float Crit { get; set; }
        public float Resilience { get; set; }

        public float TargetMiss { get; set; }
        public float TargetDodge { get; set; }
        public float TargetParry { get; set; }

        public float BurstTime { get; set; }
        public float ReactionTime { get; set; }

        public float Expertise { get; set; }

        #region Subpoints
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
                    return new float[] { Survival * SurvivalWeight, Mitigation * MitigationWeight, Threat * ThreatWeight };
                }
            }
            set 
            { 
                _subPoints = value; 
            }
        }
        #endregion

        #region Threat & DPS info
        public float DPS { get; set; }
        public int TotalThreat { get; set; }

        #region Costs
        public float RotationTime { get; set; }
        public int Blood { get; set; }
        public int Unholy { get; set; }
        public int Frost { get; set; }
        public int Death { get; set; }
        public int RP { get; set; }
        public int FreeRERunes { get; set; }
        #endregion

        #endregion

        #region Combat Data
        public float DTPS { get; set; }
        public float HPS { get; set; }
        public float TotalBShield { get; set; }
        public float TotalDShealed { get; set; }
        public float DSHeal { get; set; }
        public float DSOverHeal { get; set; }        
        public float DSCount { get; set; }
        public float BShield { get; set; }        
        #endregion

        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Chance to be Crit": return Crit; // Def cap chance to be critted by boss.  For optimization this needs to be  <= 0
                case "Avoidance %": return (Miss + Parry + Dodge); // Another duplicate math location?
                case "% Chance to Hit": return (1f - TargetMiss) * 100.0f; // +Hit related
                case "Target Parry %": return TargetParry * 100.0f; // Expertise related.
                case "Target Dodge %": return TargetDodge * 100.0f; // Expertise related.
                case "Damage Reduction %": return ArmorDamageReduction * 100.0f; // % Damage reduction by Armor
                case "Armor": return Armor; // Raw Armor
                case "Health": return BasicStats.Health;
                case "Hit Rating": return BasicStats.HitRating; // Raw Hit Rating
                case "Burst Time": return BurstTime;
                case "Reaction Time": return ReactionTime;
                case "Resilience": return Resilience;
                case "Spell Penetration": return BasicStats.SpellPenetration;
                case "DPS": return DPS;

                default: return 0.0f;
            }
        }
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["Miss"] = Miss.ToString("P2") + "*" + SEStats.Miss.ToString("P2");
            dict["Dodge"] = Dodge.ToString("P2") + "*" + SEStats.Dodge.ToString("P2");
            dict["Parry"] = Parry.ToString("P2") + "*" + SEStats.Parry.ToString("P2");
            dict["Armor Damage Reduction"] = ArmorDamageReduction.ToString("P2");
            dict["Magic Damage Reduction"] = MagicDamageReduction.ToString("P2")
                + string.Format("*Arcane: {0:0}\n", BasicStats.ArcaneResistance)
                + string.Format("Fire: {0:0}\n", BasicStats.FireResistance)
                + string.Format("Frost: {0:0}\n", BasicStats.FrostResistance)
                + string.Format("Nature: {0:0}\n", BasicStats.NatureResistance)
                + string.Format("Shadow: {0:0}", BasicStats.ShadowResistance);

            dict["Total Avoidance"] = (Miss + Parry + Dodge).ToString("P2"); // Another duplicate math location.
            dict["Burst Time"] = String.Format("{0:0.0} sec", BurstTime);
            dict["Reaction Time"] = String.Format("{0:0.0} sec", ReactionTime);

            dict["Health"] = BasicStats.Health.ToString("F0") + "*" + SEStats.Health.ToString("F0");
            dict["Armor"] = BasicStats.Armor.ToString("F0") + "*" + SEStats.Armor.ToString("F0");
            dict["Strength"] = BasicStats.Strength.ToString("F0") + "*" + SEStats.Strength.ToString("F0");
            dict["Agility"] = BasicStats.Agility.ToString("F0") + "*" + SEStats.Agility.ToString("F0");
            dict["Stamina"] = BasicStats.Stamina.ToString("F0") + "*" + SEStats.Stamina.ToString("F0");
            dict["Hit Rating"] = BasicStats.HitRating.ToString("F0") + "*" + SEStats.HitRating.ToString("F0");
            dict["Haste Rating"] = BasicStats.HasteRating.ToString("F0") + "*" + SEStats.HasteRating.ToString("F0");
            dict["Crit Rating"] = BasicStats.CritRating.ToString("F0") + "*" + SEStats.CritRating.ToString("F0");
            dict["Physical Crit"] = BasicStats.PhysicalCrit.ToString("P2") + "*" + SEStats.PhysicalCrit.ToString("F0");
            dict["Expertise"] = Expertise.ToString("F0") + "*" + SEStats.Expertise.ToString("F0");
            dict["Attack Power"] = BasicStats.AttackPower.ToString("F0") + "*" + SEStats.AttackPower.ToString("F0");
            dict["Mastery"] = BasicStats.Mastery.ToString("F2") + "*" + BasicStats.MasteryRating.ToString("F0") + " Rating " + SEStats.MasteryRating.ToString("F0");

            dict["DPS"] = DPS.ToString("F0") + "* At Max Vengeance";
            dict["Rotation Time"] = String.Format("{0:0.00} sec", RotationTime);
            dict["Total Threat"] = TotalThreat.ToString("F0");

            #region Ability Costs
            dict["Blood"] = Blood.ToString("F0");
            dict["Frost"] = Frost.ToString("F0");
            dict["Unholy"] = Unholy.ToString("F0");
            dict["Death"] = Death.ToString("F0");
            dict["Runic Power"] = RP.ToString("F0");
            dict["RE Runes"] = FreeRERunes.ToString("F0");
            #endregion

            dict["Overall Points"] = OverallPoints.ToString("F1");
            dict["Mitigation Points"] = String.Format("{0:0.0}", (Mitigation * MitigationWeight)) 
                + string.Format("*CritMitigation:{0:0.0}\n", (CritMitigation * MitigationWeight))
                + string.Format("Avoidance:{0:0.0}\n", (AvoidanceMitigation * MitigationWeight))
                + string.Format("Armor:{0:0.0}\n", (ArmorMitigation * MitigationWeight))
                + string.Format("DamageTaken:{0:0.0}\n", (DamageTakenMitigation * MitigationWeight))
                + string.Format("Impedence:{0:0.0}\n", (ImpedenceMitigation * MitigationWeight))
                + string.Format("Heals:{0:0.0}", (HealsMitigation * MitigationWeight)); // Modified Mitigation.
            dict["Survival Points"] = String.Format("{0:0.0}", (Survival * SurvivalWeight)) 
                + string.Format("*Physical:{0:0.0}\n", (PhysicalSurvival * SurvivalWeight)) 
                + string.Format("Bleed:{0:0.0}\n", (BleedSurvival * SurvivalWeight)) 
                + string.Format("Magic:{0:0.0}", (MagicSurvival * SurvivalWeight)); // Modified Survival
            dict["Threat Points"] = String.Format("{0:0.0}", (Threat * ThreatWeight)); // Modified Threat

            dict["Resilience"] = Resilience.ToString("F0");

            dict["Target Miss"] = (TargetMiss).ToString("P1");
            dict["Target Dodge"] = (TargetDodge).ToString("P1");
            dict["Target Parry"] = (TargetParry).ToString("P1");

            dict["DTPS"] = DTPS.ToString("F2");
            dict["HPS"] = HPS.ToString("F2");
            dict["DPS Avoided"] = AvoidanceMitigation.ToString("F0");
            dict["DPS Reduced By Armor"] = ArmorMitigation.ToString("F0");
            dict["Death Strike"] = TotalDShealed.ToString("F0") + "*" + DSCount.ToString("F0") + " Death Strikes Healing for " + DSHeal.ToString("F0") + " avg " + DSOverHeal.ToString("F0") + " avg Overheal";
            dict["Blood Shield"] = TotalBShield.ToString("F0") + "*" + BShield.ToString("F0") + " average shield size";

            return dict;
        }
    }
}
