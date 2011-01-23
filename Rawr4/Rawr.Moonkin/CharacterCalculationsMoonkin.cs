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
        public float Mastery { get; set; }
        public float ManaRegen { get; set; }
        public float Latency { get; set; }
        public int TargetLevel { get; set; }
        public int PlayerLevel { get; set; }
        public float FightLength { get; set; }
        public bool PtrMode { get; set; }
        public float EclipseBase = 0.25f;
        private StatsMoonkin baseStats;
        public StatsMoonkin BasicStats { get { return baseStats; } set { baseStats = value; } }
        public RotationData SelectedRotation { get; set; }
        public RotationData BurstRotation { get; set; }
        public Dictionary<string, RotationData> Rotations = new Dictionary<string, RotationData>();

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            //
            if (baseStats == null) baseStats = new StatsMoonkin();
            if (SelectedRotation == null) SelectedRotation = new RotationData();
            if (BurstRotation == null) BurstRotation = new RotationData();
            //
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
                StatConversion.GetRatingFromHit(Math.Max(0, StatConversion.GetSpellMiss(PlayerLevel - TargetLevel, false) - SpellHit))));
            retVal.Add("Treant Hit", String.Format("{0:F}%*{1} Hit Rating, {2} Rating To Cap",
                100 * BasicStats.PhysicalHit,
                StatConversion.GetRatingFromHit(BasicStats.PhysicalHit),
                StatConversion.GetRatingFromHit(Math.Max(0, StatConversion.YELLOW_MISS_CHANCE_CAP[PlayerLevel - TargetLevel] - BasicStats.PhysicalHit))));
            retVal.Add("Spell Crit", String.Format("{0:F}%*{1} Crit Rating, {2:F}% Crit From Gear, {3:F}% Crit From Intellect",
                100 * SpellCrit,
                baseStats.CritRating,
                100 * StatConversion.GetSpellCritFromRating(baseStats.CritRating),
                100 * StatConversion.GetSpellCritFromIntellect(baseStats.Intellect)));
            retVal.Add("Spell Haste", String.Format("{0:F}%*{1} Haste Rating, {2:F}% Haste From Gear",
                100 * SpellHaste,
                baseStats.HasteRating,
                100 * StatConversion.GetSpellHasteFromRating(baseStats.HasteRating)));
            retVal.Add("Mastery", String.Format("{0:F}*{1:F} Eclipse %, {2} Rating",
                Mastery,
                Mastery * 1.5f,
                baseStats.MasteryRating));
            retVal.Add("Mana Regen", String.Format("{0:F0}", ManaRegen * 5.0f));
            retVal.Add("Total Score", String.Format("{0:F2}", OverallPoints));
            retVal.Add("Selected Rotation", String.Format("*{0}", SelectedRotation.Name));
            retVal.Add("Selected DPS", String.Format("{0:F2}", SelectedRotation.SustainedDPS));
            retVal.Add("Selected Time To OOM", String.Format(SelectedRotation.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", SelectedRotation.TimeToOOM.Minutes, SelectedRotation.TimeToOOM.Seconds));
            retVal.Add("Selected Cycle Length", String.Format("{0:F1} s", SelectedRotation.Duration));

            StringBuilder sb = new StringBuilder("*");
            float rotationDamage = SelectedRotation.SustainedDPS * SelectedRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * SelectedRotation.StarfireAvgHit * SelectedRotation.StarfireCount / rotationDamage,
                SelectedRotation.StarfireAvgHit * SelectedRotation.StarfireCount,
                SelectedRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (SelectedRotation.MoonfireAvgHit) * SelectedRotation.MoonfireCasts / rotationDamage,
                (SelectedRotation.MoonfireAvgHit) * SelectedRotation.MoonfireCasts,
                SelectedRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Insect Swarm", 100 * SelectedRotation.InsectSwarmAvgHit * SelectedRotation.InsectSwarmCasts / rotationDamage,
                SelectedRotation.InsectSwarmAvgHit * (SelectedRotation.InsectSwarmCasts),
                SelectedRotation.InsectSwarmCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * SelectedRotation.WrathAvgHit * SelectedRotation.WrathCount / rotationDamage,
                SelectedRotation.WrathAvgHit * SelectedRotation.WrathCount,
                SelectedRotation.WrathCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starsurge", 100 * SelectedRotation.StarSurgeAvgHit * SelectedRotation.StarSurgeCount / rotationDamage,
                SelectedRotation.StarSurgeAvgHit * SelectedRotation.StarSurgeCount,
                SelectedRotation.StarSurgeCount));

            retVal.Add("Selected Spell Breakdown", sb.ToString());

            retVal.Add("Burst Rotation", String.Format("*{0}", BurstRotation.Name));
            retVal.Add("Burst DPS", String.Format("{0:F2}", BurstRotation.BurstDPS));
            retVal.Add("Burst Time To OOM", String.Format(BurstRotation.TimeToOOM > new TimeSpan(0, 0, 0) ? "{0} m {1} s" : "Not during fight", BurstRotation.TimeToOOM.Minutes, BurstRotation.TimeToOOM.Seconds));
            retVal.Add("Burst Cycle Length", String.Format("{0:F1} s", BurstRotation.Duration));

            sb = new StringBuilder("*");
            rotationDamage = BurstRotation.BurstDPS * BurstRotation.Duration;
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starfire", 100 * BurstRotation.StarfireAvgHit * BurstRotation.StarfireCount / rotationDamage,
                BurstRotation.StarfireAvgHit * BurstRotation.StarfireCount,
                BurstRotation.StarfireCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Moonfire", 100 * (BurstRotation.MoonfireAvgHit) * BurstRotation.MoonfireCasts / rotationDamage,
                (BurstRotation.MoonfireAvgHit) * BurstRotation.MoonfireCasts,
                BurstRotation.MoonfireCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Insect Swarm", 100 * BurstRotation.InsectSwarmAvgHit * BurstRotation.InsectSwarmCasts / rotationDamage,
                BurstRotation.InsectSwarmAvgHit * (BurstRotation.InsectSwarmCasts),
                BurstRotation.InsectSwarmCasts));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Wrath", 100 * BurstRotation.WrathAvgHit * BurstRotation.WrathCount / rotationDamage,
                BurstRotation.WrathAvgHit * BurstRotation.WrathCount,
                BurstRotation.WrathCount));
            sb.AppendLine(String.Format("{0}: {1:F2}%, {2:F2} damage, {3:F0} count", "Starsurge", 100 * BurstRotation.StarSurgeAvgHit * BurstRotation.StarSurgeCount / rotationDamage,
                BurstRotation.StarSurgeAvgHit * BurstRotation.StarSurgeCount,
                BurstRotation.StarSurgeCount));

            retVal.Add("Burst Spell Breakdown", sb.ToString());

            retVal.Add("Nature's Grace Uptime", String.Format("{0:F2}%", SelectedRotation.NaturesGraceUptime * 100));
            retVal.Add("Solar Eclipse Uptime", String.Format("{0:F2}%", SelectedRotation.SolarUptime * 100));
            retVal.Add("Lunar Eclipse Uptime", String.Format("{0:F2}%", SelectedRotation.LunarUptime * 100));

            retVal.Add("Starfire", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F0} avg energy",
                SelectedRotation.StarfireAvgHit / (SelectedRotation.StarfireAvgCast > 0 ? SelectedRotation.StarfireAvgCast : 1f),
                SelectedRotation.StarfireAvgCast,
                SelectedRotation.StarfireAvgHit,
                SelectedRotation.StarfireAvgEnergy));
            retVal.Add("Wrath", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F0} avg energy",
                SelectedRotation.WrathAvgHit / (SelectedRotation.WrathAvgCast > 0 ? SelectedRotation.WrathAvgCast : 1f),
                SelectedRotation.WrathAvgCast,
                SelectedRotation.WrathAvgHit,
                SelectedRotation.WrathAvgEnergy));
            retVal.Add("Starsurge", String.Format("{0:F2} dps*{1:F2} s avg\n {2:F2} avg hit\n{3:F0} avg energy",
                SelectedRotation.StarSurgeAvgHit / (SelectedRotation.StarSurgeAvgCast > 0 ? SelectedRotation.StarSurgeAvgCast : 1f),
                SelectedRotation.StarSurgeAvgCast,
                SelectedRotation.StarSurgeAvgHit,
                SelectedRotation.StarSurgeAvgEnergy));
            retVal.Add("Moonfire", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit",
                SelectedRotation.MoonfireAvgHit / (SelectedRotation.MoonfireDuration > 0 ? SelectedRotation.MoonfireDuration : 1f),
                SelectedRotation.AverageInstantCast,
                SelectedRotation.MoonfireAvgHit));
            retVal.Add("Insect Swarm", String.Format("{0:F2} dps*{1:F2} s avg\n{2:F2} avg hit",
                SelectedRotation.InsectSwarmAvgHit / (SelectedRotation.InsectSwarmDuration > 0 ? SelectedRotation.InsectSwarmDuration : 1f),
                SelectedRotation.AverageInstantCast,
                SelectedRotation.InsectSwarmAvgHit));
            retVal.Add("Starfall", String.Format("{0:F2} dps*{1:F2} avg per cast\n{2:F2} avg per star",
                SelectedRotation.StarfallDamage / 10.0f,
                SelectedRotation.StarfallDamage,
                SelectedRotation.StarfallDamage / (SelectedRotation.StarfallStars > 0 ? SelectedRotation.StarfallStars : 1f)));
            retVal.Add("Treants", String.Format("{0:F2} dps*{1:F2} avg per cast\n{2:F2} avg per tree",
                SelectedRotation.TreantDamage / 30.0f, SelectedRotation.TreantDamage, SelectedRotation.TreantDamage / 3.0f));
            retVal.Add("Wild Mushroom", String.Format("{0:F2} dps*{1:F2} avg per cast\n{2:F2} avg per mushroom",
                SelectedRotation.MushroomDamage / 10.0f,
                SelectedRotation.MushroomDamage,
                SelectedRotation.MushroomDamage / 3f));

            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Hit Rating": return baseStats.HitRating;
                case "Haste Rating": return baseStats.HasteRating;
                case "Crit Rating": return baseStats.CritRating;
                case "Mastery Rating": return baseStats.MasteryRating;
            }
            return 0;
        }
    }
}
