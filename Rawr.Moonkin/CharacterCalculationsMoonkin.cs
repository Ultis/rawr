using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    public class CharacterCalculationsMoonkin : CharacterCalculationsBase
    {
        private float overallPoints = 0f;
        public override float OverallPoints { get { return overallPoints; } set { overallPoints = value; } }

        private float[] subPoints = new float[] { 0f, 0f };

        public override float[] SubPoints { get { return subPoints; } set { subPoints = value; } }

        public float SpellHit { get; set; }
        public float SpellCrit { get; set; }
        public float SpellHaste { get; set; }
        public float SpellPower { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }
        public float Latency { get; set; }
        public int TargetLevel { get; set; }
        public float FightLength { get; set; }
        public bool Scryer { get; set; }
        public SpellRotation SelectedRotation { get; set; }
        public SpellRotation BurstDPSRotation { get; set; }
        public string RotationName { get { return SelectedRotation.Name; } }
        public string DpsRotationName { get { return BurstDPSRotation.Name; } }
        public Dictionary<string, RotationData> Rotations { get; set; }
        private Stats baseStats;
        public Stats BasicStats { get { return baseStats; } set { baseStats = value; } }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            retVal.Add("Health", baseStats.Health.ToString());
            retVal.Add("Mana", baseStats.Mana.ToString());
            retVal.Add("Armor", baseStats.Armor.ToString());
            retVal.Add("Agility", baseStats.Agility.ToString());
            retVal.Add("Stamina", baseStats.Stamina.ToString());
            retVal.Add("Intellect", baseStats.Intellect.ToString());
            retVal.Add("Spirit", baseStats.Spirit.ToString());
            retVal.Add("Spell Power", SpellPower.ToString());
            retVal.Add("Spell Hit", String.Format("{0:F}%*{1} Hit Rating, {2:F}% Hit From Gear, {3} Rating To Cap",
                100 * SpellHit,
                baseStats.HitRating,
                100 * StatConversion.GetSpellHitFromRating(baseStats.HitRating),
                StatConversion.GetRatingFromHit(Math.Max(0, 0.17f - SpellHit))));
            retVal.Add("Spell Crit", String.Format("{0:F}%*{1} Crit Rating, {2:F}% Crit From Gear, {3:F}% Crit From Intellect",
                100 * SpellCrit,
                baseStats.CritRating,
                100 * StatConversion.GetSpellCritFromRating(baseStats.CritRating),
                100 * StatConversion.GetSpellCritFromIntellect(baseStats.Intellect)));
            retVal.Add("Spell Haste", String.Format("{0:F}%*{1} Haste Rating, {2:F}% Haste From Gear",
                100 * SpellHaste,
                baseStats.HasteRating,
                100 * StatConversion.GetSpellHasteFromRating(baseStats.HasteRating)));
            retVal.Add("MP5 Not Casting", String.Format("{0:F0}", ManaRegen * 5.0f));
            retVal.Add("MP5 While Casting", String.Format("{0:F0}", ManaRegen5SR * 5.0f));
            retVal.Add("Total Score", String.Format("{0:F2}", SubPoints[0]+SubPoints[1]));
            retVal.Add("Selected Rotation", SelectedRotation.Name);
            retVal.Add("Selected DPS", String.Format("{0:F2}", SelectedRotation.RotationData.DPS));
            retVal.Add("Selected Time To OOM", String.Format(SelectedRotation.RotationData.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", SelectedRotation.RotationData.TimeToOOM.Minutes, SelectedRotation.RotationData.TimeToOOM.Seconds));
            retVal.Add("Selected Cycle Length", String.Format("{0:F1} s", SelectedRotation.Duration));

            StringBuilder sb = new StringBuilder("*");
            float rotationDamage = SelectedRotation.RotationData.DPS * SelectedRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * SelectedRotation.Solver.Starfire.DamagePerHit * SelectedRotation.StarfireCount / rotationDamage,
                SelectedRotation.Solver.Starfire.DamagePerHit * SelectedRotation.StarfireCount, 
                SelectedRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (SelectedRotation.Solver.Moonfire.DamagePerHit + SelectedRotation.Solver.Moonfire.DotEffect.DamagePerHit) * SelectedRotation.MoonfireCasts / rotationDamage,
                (SelectedRotation.Solver.Moonfire.DamagePerHit + SelectedRotation.Solver.Moonfire.DotEffect.DamagePerHit) * SelectedRotation.MoonfireCasts, 
                SelectedRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Insect Swarm", 100 * SelectedRotation.Solver.InsectSwarm.DotEffect.DamagePerHit * (SelectedRotation.InsectSwarmTicks / SelectedRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks) / rotationDamage,
                SelectedRotation.Solver.InsectSwarm.DotEffect.DamagePerHit * (SelectedRotation.InsectSwarmTicks / SelectedRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks), 
                SelectedRotation.InsectSwarmTicks / SelectedRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * SelectedRotation.Solver.Wrath.DamagePerHit * SelectedRotation.WrathCount / rotationDamage,
                SelectedRotation.Solver.Wrath.DamagePerHit * SelectedRotation.WrathCount, 
                SelectedRotation.WrathCount));

            retVal.Add("Selected Spell Breakdown", sb.ToString());
            retVal.Add("Burst Rotation", BurstDPSRotation.Name);
            retVal.Add("Burst DPS", String.Format("{0:F2}", BurstDPSRotation.RotationData.DPS));
            retVal.Add("Burst Time To OOM", String.Format(BurstDPSRotation.RotationData.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", BurstDPSRotation.RotationData.TimeToOOM.Minutes, BurstDPSRotation.RotationData.TimeToOOM.Seconds));
            retVal.Add("Burst Cycle Length", String.Format("{0:F1} s", BurstDPSRotation.Duration));

            sb = new StringBuilder("*");
            rotationDamage = BurstDPSRotation.RotationData.DPS * BurstDPSRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * BurstDPSRotation.Solver.Starfire.DamagePerHit * BurstDPSRotation.StarfireCount / rotationDamage,
                BurstDPSRotation.Solver.Starfire.DamagePerHit * BurstDPSRotation.StarfireCount,
                BurstDPSRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (BurstDPSRotation.Solver.Moonfire.DamagePerHit + BurstDPSRotation.Solver.Moonfire.DotEffect.DamagePerHit) * SelectedRotation.MoonfireCasts / rotationDamage,
                (BurstDPSRotation.Solver.Moonfire.DamagePerHit + BurstDPSRotation.Solver.Moonfire.DotEffect.DamagePerHit) * BurstDPSRotation.MoonfireCasts,
                BurstDPSRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Insect Swarm", 100 * BurstDPSRotation.Solver.InsectSwarm.DotEffect.DamagePerHit * (BurstDPSRotation.InsectSwarmTicks / BurstDPSRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks) / rotationDamage,
                BurstDPSRotation.Solver.InsectSwarm.DotEffect.DamagePerHit * (BurstDPSRotation.InsectSwarmTicks / BurstDPSRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks),
                BurstDPSRotation.InsectSwarmTicks / SelectedRotation.Solver.InsectSwarm.DotEffect.NumberOfTicks));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * BurstDPSRotation.Solver.Wrath.DamagePerHit * BurstDPSRotation.WrathCount / rotationDamage,
                BurstDPSRotation.Solver.Wrath.DamagePerHit * BurstDPSRotation.WrathCount, 
                BurstDPSRotation.WrathCount));

            retVal.Add("Burst Spell Breakdown", sb.ToString());
            retVal.Add("Starfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} s w/NG\n{3:F2} avg hit\n{4:F0} avg mana",
                SelectedRotation.Solver.Starfire.DamagePerHit / SelectedRotation.Solver.Starfire.CastTime,
                SelectedRotation.Solver.Starfire.CastTime,
                SelectedRotation.Solver.Starfire.NGCastTime,
                SelectedRotation.Solver.Starfire.DamagePerHit,
                SelectedRotation.Solver.Starfire.ManaCost));
            retVal.Add("Wrath", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} s w/NG\n{3:F2} avg hit\n{4:F0} avg mana",
                SelectedRotation.Solver.Wrath.DamagePerHit / SelectedRotation.Solver.Wrath.CastTime,
                SelectedRotation.Solver.Wrath.CastTime,
                SelectedRotation.Solver.Wrath.NGCastTime,
                SelectedRotation.Solver.Wrath.DamagePerHit,
                SelectedRotation.Solver.Wrath.ManaCost));
            retVal.Add("Moonfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit\n{3:F0} avg mana",
                (SelectedRotation.Solver.Moonfire.DamagePerHit + SelectedRotation.Solver.Moonfire.DotEffect.DamagePerHit) / SelectedRotation.Solver.Moonfire.DotEffect.Duration,
                SelectedRotation.Solver.Moonfire.CastTime,
                (SelectedRotation.Solver.Moonfire.DamagePerHit + SelectedRotation.Solver.Moonfire.DotEffect.DamagePerHit),
                SelectedRotation.Solver.Moonfire.ManaCost));
            retVal.Add("Insect Swarm", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit\n{3:F0} avg mana",
                SelectedRotation.Solver.InsectSwarm.DotEffect.DamagePerHit / SelectedRotation.Solver.InsectSwarm.DotEffect.Duration,
                SelectedRotation.Solver.InsectSwarm.CastTime,
                SelectedRotation.Solver.InsectSwarm.DotEffect.DamagePerHit,
                SelectedRotation.Solver.InsectSwarm.ManaCost));

            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Hit Rating": return baseStats.HitRating;
                case "Haste Rating": return baseStats.HasteRating;
                case "Crit Rating": return baseStats.CritRating;
            }
            return 0;
        }
    }
}
