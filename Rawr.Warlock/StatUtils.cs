using System;
using System.Collections.Generic;

namespace Rawr.Warlock
{
    public delegate float StatExtractor(Stats stats);

    public delegate WeightedStat[] UptimeCombiner(
        SpecialEffect[] effects, 
        float[] triggerInterval, 
        float[] triggerChance, 
        float[] offset, 
        float[] scale,
        float attackSpeed, 
        float fightDuration, 
        float[] value);

    // Please note that levels below 85 are deliberately supported.
    public class StatUtils
    {
        public static float GetBuffEffect(List<Buff> activeBuffs, float candidateBuff, string group, StatExtractor extractor)
        {
            float active = GetActiveBuff(activeBuffs, group, extractor);
            return Math.Max(0f, candidateBuff - active);
        }
        public static float GetActiveBuff(List<Buff> activeBuffs, string group, StatExtractor extractor)
        {
            float active = 0f;
            foreach (Buff buff in activeBuffs)
            {
                if (buff.ConflictingBuffs.Contains(group))
                {
                    active += extractor(buff.Stats);
                    foreach (Buff improvement in buff.Improvements)
                    {
                        if (activeBuffs.Contains(improvement))
                        {
                            active += extractor(improvement.Stats);
                        }
                    }
                }
            }
            return active;
        }
        public static float CalcStamina(Stats stats)
        {
            return (float)Math.Round(stats.Stamina * (1f + stats.BonusStaminaMultiplier) - 0.00001f);
        }
        public static float CalcIntellect(Stats stats, float baseIntellect, int playerLevel)
        {
            return (float)Math.Round((  Math.Floor(0.00001 + baseIntellect * (1 + (playerLevel > 50 ? .05f : 0f)))
                                      + Math.Floor(0.00001 + (stats.Intellect - baseIntellect) * (1 + (playerLevel > 50 ? .05f : 0f))))
                                     * (1 + stats.BonusIntellectMultiplier) - 0.00001);
        }
        public static float CalcHealth(Stats stats)
        {
            return (float)Math.Round((stats.Health + StatConversion.GetHealthFromStamina(CalcStamina(stats))) * (1 + stats.BonusHealthMultiplier) - 0.00001f);
        }
        public static float CalcMana(Stats stats, float baseIntellect, int playerLevel)
        {
            return (float)Math.Round((stats.Mana + StatConversion.GetManaFromIntellect(CalcIntellect(stats, baseIntellect, playerLevel))) * (1 + stats.BonusManaMultiplier) - 0.00001f);
        }
        public static float CalcUsableMana(Stats stats, float fightLen, float baseIntellect, int playerLevel)
        {
            float mps = stats.Mp5 / 5f + stats.Mana * stats.ManaRestoreFromMaxManaPerSecond;
            return CalcMana(stats, baseIntellect, playerLevel) + stats.ManaRestore + mps * fightLen;
        }
        public static float CalcSpellCrit(Stats stats, float baseIntellect, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return stats.SpellCrit
                    + StatConversion.GetSpellCritFromIntellect(CalcIntellect(stats, baseIntellect, playerLevel))
                    + StatConversion.GetSpellCritFromRating(stats.CritRating)
                    + stats.BonusCritChance
                    + stats.SpellCritOnTarget;
            }
            else
            {
                return stats.SpellCrit
                    + GetSpellCritFromIntellect(CalcIntellect(stats, baseIntellect, playerLevel), playerLevel)
                    + GetSpellCritFromRating(stats.CritRating, playerLevel)
                    + stats.BonusCritChance
                    + stats.SpellCritOnTarget;
            }
        }
        public static float CalcSpellHit(Stats stats, int playerLevel)
        {
            if (playerLevel == 85) {
                return stats.SpellHit
                    + StatConversion.GetSpellHitFromRating(stats.HitRating);
            } else {
                return stats.SpellHit
                    + GetSpellHitFromRating(stats.HitRating, playerLevel);
            }
        }
        public static float CalcSpellHaste(Stats stats, int playerLevel)
        {
            if (playerLevel == 85) {
                return (1f + stats.SpellHaste)
                     * (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating));
            } else {
                return (1f + stats.SpellHaste)
                     * (1f + GetSpellHasteFromRating(stats.HasteRating, playerLevel));
            }
        }
        public static float CalcMastery(Stats stats, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetMasteryFromRating(stats.MasteryRating) + 8f;
            }
            else
            {
                return GetMasteryFromRating(stats.MasteryRating, playerLevel) + 8f;
            }
        }
        public static float GetSpellCritFromIntellect(float intellect, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetSpellCritFromIntellect(intellect);
            }
            else
            {
                float[] scaling = { 0.000060183800088f, 0.000045833898184f, 0.000034903401684f, 0.000026568999601f, 0.000020234600015f };
                return intellect * scaling[playerLevel - 80];
            }
        }
        public static float GetSpellCritFromRating(float rating, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetSpellCritFromRating(rating);
            }
            else
            {
                float[] scaling = { 45.905986785888672f, 60.278423309326172f, 79.155647277832031f, 103.985641479492188f, 136.538131713867188f };
                return rating / scaling[playerLevel - 80] * 0.01f;
            }
        }
        public static float GetSpellHitFromRating(float rating, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetSpellHitFromRating(rating);
            }
            else
            {
                float[] scaling = { 26.231992721557617f, 34.444812774658203f, 45.231800079345703f, 59.420368194580078f, 78.021789550781250f };
                return rating / scaling[playerLevel - 80] * 0.01f;
            }
        }
        public static float GetSpellHasteFromRating(float rating, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetSpellHasteFromRating(rating);
            }
            else
            {
                float[] scaling = { 32.789989471435547f, 43.056015014648438f, 56.539749145507812f, 74.275451660156250f, 97.527236938476562f };
                return rating / scaling[playerLevel - 80] * 0.01f;
            }
        }
        public static float GetMasteryFromRating(float points, int playerLevel)
        {
            if (playerLevel == 85)
            {
                return StatConversion.GetMasteryFromRating(points);
            }
            else
            {
                float[] scaling = { 45.905986785888672f, 60.278423309326172f, 79.155647277832031f, 103.985641479492188f, 136.538131713867188f };
                return points / scaling[playerLevel - 80];
            }
        }
        public static float CalcSpellPower(Stats stats, float baseIntellect, int playerLevel)
        {
            return (float)Math.Round((stats.SpellPower) * (1f + stats.BonusSpellPowerMultiplier)) + CalcIntellect(stats, baseIntellect, playerLevel) - 10f;
        }
        // mainly used by pets
        public static float CalcStrength(Stats stats)
        {
            return (float)Math.Round(stats.Strength * (1 + stats.BonusStrengthMultiplier));
        }
        public static float CalcAgility(Stats stats)
        {
            return (float)Math.Round(stats.Agility * (1 + stats.BonusAgilityMultiplier));
        }
        public static float CalcAttackPower(Stats stats, float apPerStrength, float apPerAgility)
        {
            return (float)Math.Round((1 + stats.BonusAttackPowerMultiplier) * (stats.AttackPower + CalcStrength(stats) * apPerStrength + CalcAgility(stats) * apPerAgility));
        }
        public static float CalcPhysicalCrit(Stats stats, float critPerAgility, int levelDelta)
        {
            return stats.PhysicalCrit + CalcAgility(stats) * critPerAgility + StatConversion.NPC_LEVEL_CRIT_MOD[levelDelta > 3 ? 3 : levelDelta];
        }
    }

    public class SpellModifiers
    {
        public float AdditiveMultiplier { get; private set; }
        public float AdditiveDirectMultiplier { get; private set; }
        public float AdditiveTickMultiplier { get; private set; }
        public float MultiplicativeMultiplier { get; private set; }
        public float MultiplicativeDirectMultiplier { get; private set; }
        public float MultiplicativeTickMultiplier { get; private set; }
        public float CritChance { get; private set; }
        public float CritOverallMultiplier { get; private set; }
        public float CritBonusMultiplier { get; private set; }

        public SpellModifiers() { }
        public void Accumulate(SpellModifiers other)
        {
            AddAdditiveMultiplier(other.AdditiveMultiplier);
            AddAdditiveDirectMultiplier(other.AdditiveDirectMultiplier);
            AddAdditiveTickMultiplier(other.AdditiveTickMultiplier);
            AddMultiplicativeMultiplier(other.MultiplicativeMultiplier);
            AddMultiplicativeDirectMultiplier(other.MultiplicativeDirectMultiplier);
            AddMultiplicativeTickMultiplier(other.MultiplicativeTickMultiplier);
            AddCritChance(other.CritChance);
            AddCritOverallMultiplier(other.CritOverallMultiplier);
            AddCritBonusMultiplier(other.CritBonusMultiplier);
        }
        public float GetFinalDirectMultiplier()
        {
            return (1 + AdditiveMultiplier + AdditiveDirectMultiplier) * (1 + MultiplicativeMultiplier) * (1 + MultiplicativeDirectMultiplier);
        }
        public float GetFinalTickMultiplier()
        {
            return (1 + AdditiveMultiplier + AdditiveTickMultiplier) * (1 + MultiplicativeMultiplier) * (1 + MultiplicativeTickMultiplier);
        }
        public float GetFinalCritMultiplier()
        {
            float bonus = 1.5f * (1 + CritOverallMultiplier) - 1;
            return 1f + bonus * (1 + CritBonusMultiplier);
        }
        public void AddAdditiveMultiplier(float val)
        {
            AdditiveMultiplier += val;
        }
        public void AddAdditiveDirectMultiplier(float val)
        {
            AdditiveDirectMultiplier += val;
        }
        public void AddAdditiveTickMultiplier(float val)
        {
            AdditiveTickMultiplier += val;
        }
        public void AddMultiplicativeMultiplier(float val)
        {
            MultiplicativeMultiplier = (1 + MultiplicativeMultiplier) * (1 + val) - 1;
        }
        public void AddMultiplicativeDirectMultiplier(float val)
        {
            MultiplicativeDirectMultiplier = (1 + MultiplicativeDirectMultiplier) * (1 + val) - 1;
        }
        public void AddMultiplicativeTickMultiplier(float val)
        {
            MultiplicativeTickMultiplier = (1 + MultiplicativeTickMultiplier) * (1 + val) - 1;
        }
        public void AddCritChance(float val)
        {
            CritChance = Math.Min(1, CritChance + val);
        }
        public void AddCritOverallMultiplier(float val)
        {
            CritOverallMultiplier = (1 + CritOverallMultiplier) * (1 + val) - 1;
        }
        public void AddCritBonusMultiplier(float val)
        {
            CritBonusMultiplier = (1 + CritBonusMultiplier) * (1 + val) - 1;
        }
    }
}
