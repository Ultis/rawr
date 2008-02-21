using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    enum MagicSchool
    {
        Arcane,
        Fire,
        Frost,
        Shadow,
        Nature
    }

    abstract class Spell
    {
        public string Name;
        public int Rank;
        public float BaseCastTime;
        public float BasePeriodicDamage;
        public float BasePeriodicDuration;
        public float BasePeriodTickInterval;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BaseCritPercent;
        public bool Channeled;
        public bool Instant;
        public bool AreaEffect;
        public int BaseCost;
        public int BaseRange;
        public MagicSchool MagicSchool;
        public float BaseSpellDamageCoefficient;
        public float BaseCritModifier;

        public float MinDamage;
        public float MaxDamage;
        public float AverageDamage;
        public float PeriodicDamage;
        public float PeriodicDuration;
        public float PeriodicTickInterval;
        public int Cost;
        public int Range;
        public float SpellDamageCoefficient;
        public float CastTime;
        public float CritPercent;
        public float CritModifier = 1.5f;
        public float ThreatModifier;
        public float HealthReturnFactor = 0f;
        public float DamageModifier = 1f;
        public float SpellDamage;
        public float ShadowDamage;
        public float FireDamage;

    }


    
    internal class ShadowBolt : Spell
    {
        public ShadowBolt(Character character, Stats stats)
        {

            BaseCost = 420;
            BaseCastTime = 3.0f;
            BaseMinDamage = 541;
            BaseMaxDamage = 603;
            MagicSchool = MagicSchool.Shadow;
            BaseSpellDamageCoefficient = 0.8571f;
            BaseCritPercent = stats.SpellCritRating / 22.08f;
            SpellDamage = stats.SpellDamageRating;
            ShadowDamage = stats.SpellShadowDamageRating;
            BaseRange = 30;
            
            
            CritPercent = BaseCritPercent;
            CastTime = BaseCastTime;
            SpellDamageCoefficient = BaseSpellDamageCoefficient;
            Range = BaseRange;
            Cost = BaseCost;
            PeriodicTickInterval = BasePeriodTickInterval;
            PeriodicDuration = BasePeriodicDuration;
            PeriodicDamage = BasePeriodicDamage;
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            ParseTalents(character, stats);
            Calculate(character, stats);
        }

        private void ParseTalents(Character character, Stats stats)
        {
            TalentTree tal = character.Talents;
            
            //destruction talents
            Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
            CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);
            CritPercent +=  tal.GetTalent("Devastation").PointsInvested;
            HealthReturnFactor = tal.GetTalent("Soul Leech").PointsInvested / 10f;
            SpellDamageCoefficient += tal.GetTalent("Shadow and Flame").PointsInvested * 0.04f;
            if (tal.GetTalent("Demonic Sacrifice").PointsInvested == 1 && character.CalculationOptions["SacraficedPet"] == "Succubus")
                DamageModifier += 0.15f;
            if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
        }

        private void Calculate(Character character, Stats stats)
        {
            if (character.CalculationOptions["Misery"] =="T")
                DamageModifier *= 1.05f;
            if (character.CalculationOptions["ShadowWeaving"] == "T")
                DamageModifier *= 1.1f;
            DamageModifier *= float.Parse(character.CalculationOptions["CurseOfShadows"]);

            MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
            float minCrit = MinDamage * CritModifier;
            float maxCrit = MaxDamage * CritModifier;
            AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
        }

    }
}
