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

    enum SpellTree
    {
        Affliction, 
        Destruction,
        Demonology
    }

    internal abstract class Spell
    {
        //base stats
        public string Name { get; set; }
        public MagicSchool MagicSchool { get; set; }
        public SpellTree SpellTree { get; set; }
        public float BaseMinDamage { get; set; }
        public float BaseMaxDamage { get; set; }
        public float BaseCastTime { get; set; }
        public float BasePeriodicDamage { get; set; }
        public float BaseDotDuration { get; set; }
        public float BaseManaCost { get; set; }
        public float DirectDamageCoefficient { get; set; }
        public float DotDamageCoefficient { get; set; }

        public Spell(string name, MagicSchool magicSchool, SpellTree spellTree, float minDamage, float maxDamage, float periodicDamage, float dotDuration, float castTime, float manaCost)
        {
            Name = name;
            MagicSchool = magicSchool;
            SpellTree = spellTree;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            BaseDotDuration = dotDuration;
            BaseCastTime = castTime;
            BaseManaCost = manaCost;
            DirectDamageCoefficient = BaseCastTime / 3.5f;
            DotDamageCoefficient = BaseDotDuration / 15;
            CritBonus = 1;
        }

        //derived stats
        public float CritRate { get; set; }
        public float CritBonus { get; set; }
        public float HitRate { get; set; }
        public float Damage { get; set; }
        public float Frequency { get; set; }
        public float CastRatio { get; set; }
        public float CastTime { get; set; }
        //public float ManaPerSecond { get; set; }
        public float ManaCost { get; set; }
        public float HealthPerSecond { get; set; }

        public float ChanceToHit(int targetLevel, float hitPercent)
        {
            return Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + 0.01f * hitPercent);
        }

        public void CalculateHitRate(CharacterCalculationsWarlock calculations)
        {
            HitRate = calculations.HitPercent;
            if (SpellTree == SpellTree.Affliction)
                HitRate += 2 * calculations.CalculationOptions.Suppression;
        }

        public void CalculateCastTime(CharacterCalculationsWarlock calculations)
        {
            CastTime = BaseCastTime / (1 + 0.01f * calculations.HastePercent);
            CastTime += calculations.CalculationOptions.Latency;
            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            
            //for DoTs, factor in the chance to miss (because you have to re-apply)
            if (BaseDotDuration != 0)
                CastTime /= (1 - ChanceToHit(calculations.CalculationOptions.TargetLevel, HitRate) / 100);

            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
        }

        public void CalculateFrequency(CharacterCalculationsWarlock calculations)
        {
            if (BaseDotDuration == 0)
                Frequency = CastTime;
            else
                Frequency = BaseDotDuration + CastTime + calculations.CalculationOptions.DotGap - (BaseCastTime + calculations.CalculationOptions.Latency);
        }

        public void CalculateManaCost(CharacterCalculationsWarlock calculations)
        {
            ManaCost = BaseManaCost;
            if (SpellTree == SpellTree.Destruction)
                ManaCost *= (1 - 0.01f * calculations.CalculationOptions.Cataclysm);
            if (BaseDotDuration != 0)
                ManaCost /= ChanceToHit(calculations.CalculationOptions.TargetLevel, HitRate);
            ManaCost = (float)Math.Round(ManaCost);
        }

        public void CalculateDerivedStats(CharacterCalculationsWarlock calculations)
        {
            CalculateHitRate(calculations);
            CalculateCastTime(calculations);
            CalculateFrequency(calculations);
            CalculateManaCost(calculations);
        }

        public void CalculateDamage(CharacterCalculationsWarlock calculations)
        {
            if (calculations.BasicStats.BonusSpellCritMultiplier > 0)
                CritBonus = 1.09f;
            CritBonus *= (0.5f + 0.5f * calculations.CalculationOptions.Ruin);

            float plusDamage = 0;
            float bonusMultiplier = 0;
            switch(MagicSchool)
            {
                case MagicSchool.Shadow:
                    plusDamage = calculations.ShadowDamage;
                    bonusMultiplier = calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusShadowSpellPowerMultiplier;
                    break;
                case MagicSchool.Fire:
                    plusDamage = calculations.FireDamage;
                    bonusMultiplier = calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusFireSpellPowerMultiplier;
                    break;
            }

            float averageDamage = (BaseMinDamage + BaseMaxDamage) / 2;
            float dotDamage = BasePeriodicDamage;
            if (averageDamage > 0 && BasePeriodicDamage > 0)
            {
                DirectDamageCoefficient *= averageDamage / (averageDamage + dotDamage);
                DotDamageCoefficient *= dotDamage / (averageDamage + dotDamage);
            }

            if (averageDamage != 0)
            {
                averageDamage += plusDamage * DirectDamageCoefficient;
                averageDamage *= (1 + 0.01f * calculations.CritPercent * CritBonus);
                if (BaseDotDuration == 0)
                    averageDamage *= ChanceToHit(calculations.CalculationOptions.TargetLevel, HitRate);
                if (this is ShadowBolt || this is Incinerate)
                    bonusMultiplier *= (1 + calculations.BasicStats.BonusWarlockNukeMultiplier);
            }

            if (dotDamage != 0)
                dotDamage += plusDamage * DotDamageCoefficient;

            Damage = (float)Math.Round((averageDamage + dotDamage) * bonusMultiplier);
        }
    }

    internal class ShadowBolt : Spell
    {
        public ShadowBolt(CharacterCalculationsWarlock calculations)
            : base("Shadow Bolt", MagicSchool.Shadow, SpellTree.Destruction, 541, 603, 0, 0, 3, 420)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
        }
    }

    internal class Incinerate : Spell
    {
        public Incinerate(CharacterCalculationsWarlock calculations)
            : base("Incinerate", MagicSchool.Fire, SpellTree.Destruction, 555, 642, 0, 0, 2.5f, 355)
        {
            BaseCastTime *= (1 - 0.02f * calculations.CalculationOptions.Emberstorm);
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
        }
    }

    internal class Immolate : Spell
    {
        public Immolate(CharacterCalculationsWarlock calculations)
            : base("Immolate", MagicSchool.Fire, SpellTree.Destruction, 327, 327, 615, 15, 2, 445)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            if (calculations.BasicStats.BonusWarlockDotExtension > 0)
                BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
            BaseMinDamage *= (1 + 0.05f * calculations.CalculationOptions.ImprovedImmolate);
            BaseMaxDamage *= (1 + 0.05f * calculations.CalculationOptions.ImprovedImmolate);
        }
    }

    internal class CurseOfAgony : Spell
    {
        public CurseOfAgony(CharacterCalculationsWarlock calculations)
            : base("Curse of Agony", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1356, 24, 1.5f, 265)
        {
            BasePeriodicDamage *= (1 + 0.05f * calculations.CalculationOptions.ImprovedCurseOfAgony);
            BasePeriodicDamage *= (1 + 0.01f * calculations.CalculationOptions.Contagion);
            DotDamageCoefficient = 1.2f;
        }
    }

    internal class CurseOfDoom : Spell
    {
        public CurseOfDoom(CharacterCalculationsWarlock calculations)
            : base("Curse of Doom", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 4200, 60, 1.5f, 380)
        {
            DotDamageCoefficient = 2f;
        }
    }

    internal class CurseOfRecklessness : Spell
    {
        public CurseOfRecklessness(CharacterCalculationsWarlock calculations)
            : base("Curse of Recklessness", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 120, 1.5f, 160)
        {
        }
    }

    internal class CurseOfShadow : Spell
    {
        public CurseOfShadow(CharacterCalculationsWarlock calculations)
            : base("Curse of Shadow", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 300, 1.5f, 260)
        {
        }
    }

    internal class CurseOfTheElements : Spell
    {
        public CurseOfTheElements(CharacterCalculationsWarlock calculations)
            : base("Curse of the Elements", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 300, 1.5f, 260)
        {
        }
    }

    internal class CurseOfWeakness : Spell
    {
        public CurseOfWeakness(CharacterCalculationsWarlock calculations)
            : base("Curse of Weakness", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 120, 1.5f, 265)
        {
        }
    }

    internal class CurseOfTongues : Spell
    {
        public CurseOfTongues(CharacterCalculationsWarlock calculations)
            : base("Curse of Tongues", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 30, 1.5f, 110)
        {
        }
    }

    internal class Corruption : Spell
    {
        public Corruption(CharacterCalculationsWarlock calculations)
            : base("Corruption", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 900, 18, 2, 370)
        {
            BaseCastTime -= 0.4f * calculations.CalculationOptions.ImprovedCorruption;
            if (calculations.BasicStats.BonusWarlockDotExtension > 0)
                BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
            BasePeriodicDamage *= (1 + 0.01f * calculations.CalculationOptions.Contagion);
            DotDamageCoefficient = 0.94f + 0.12f * calculations.CalculationOptions.EmpoweredCorruption;
        }
    }

    internal class SiphonLife : Spell
    {
        public SiphonLife(CharacterCalculationsWarlock calculations)
            : base("Siphon Life", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 630, 30, 1.5f, 410)
        {
            DotDamageCoefficient /= 2;
        }
    }

    internal class UnstableAffliction : Spell
    {
        public UnstableAffliction(CharacterCalculationsWarlock calculations)
            : base("Unstable Affliction", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1050, 18, 1.5f, 400)
        {
        }
    }

    internal class LifeTap : Spell
    {
        public LifeTap(CharacterCalculationsWarlock calculations)
            : base("Life Tap", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 0, 1.5f, -580)
        {
            BaseManaCost -= calculations.ShadowDamage * 0.8f;
            BaseManaCost *= (1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap);
        }
    }
    
    //internal class ShadowBolt : Spell
    //{
    //    public float ISBuptime { get; set; }
    //    public ShadowBolt(Character character, Stats stats)
    //    {
    //        Name = "Shadowbolt";
    //        BaseCost = 420;
    //        BaseCastTime = 3.0f;
    //        BaseMinDamage = 541;
    //        BaseMaxDamage = 603;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 0.8571f;
    //        BaseCritPercent = stats.SpellCritRating / 22.08f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
            
    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;
    //        _character = character;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;
            
    //        //destruction talents
    //        Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
    //        CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);
    //        CritPercent +=  tal.GetTalent("Devastation").PointsInvested;
    //        HealthReturnFactor = tal.GetTalent("SoulLeech").PointsInvested / 10f;
    //        SpellDamageCoefficient += tal.GetTalent("ShadowandFlame").PointsInvested * 0.04f;
    //        if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        CastTime /= (1 + (stats.SpellHasteRating / 1570f));
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        float minCrit = MinDamage * CritModifier;
    //        float maxCrit = MaxDamage * CritModifier;
    //        AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
    //        float missRate = ChanceToMiss  - (stats.SpellHitRating / 1262.5f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //        float impSB = character.Talents.GetTalent("ImprovedShadowBolt").PointsInvested * 0.04f;
    //        ISBuptime = (float)(1f - (Math.Pow(1f - (CritPercent / 100f), 4f)));
    //        AverageDamage *= (1 + impSB * ISBuptime);
    //    }

    //}

    //internal class CurseOfAgony : Spell
    //{
    //    public CurseOfAgony(Character character, Stats stats)
    //    {
    //        Name = "CurseOfAgony";
    //        BaseCost = 265;
    //        BaseCastTime = 0f;
    //        BaseMinDamage = 1356;
    //        BaseMaxDamage = 1356;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 1.2f;
    //        BaseCritPercent = 0f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
    //        BasePeriodTickInterval = 2f;
    //        BasePeriodicDamage = 113f;
    //        BasePeriodicDuration = 24f;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;

    //        //bost in base damage only
    //        if (tal.GetTalent("ImprovedCurseOfAgony").PointsInvested > 0)
    //        {
    //            MinDamage = MaxDamage = BaseMinDamage * (1f + (tal.GetTalent("ImprovedCurseOfAgony").PointsInvested) * 0.1f);
    //        }

    //        //Contagion
    //        MinDamage = MaxDamage *= (1f + tal.GetTalent("Contagion").PointsInvested * 0.1f);

    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        AverageDamage = MinDamage;

    //        float supressionbonus = (stats.SpellHitRating + character.Talents.GetTalent("Supression").PointsInvested * 2f); 
    //        float missRate = ChanceToMiss - (stats.SpellHitRating  / 1262.5f) - (supressionbonus / 100f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}

    //internal class CurseOfDoom : Spell
    //{
    //    public CurseOfDoom(Character character, Stats stats)
    //    {
    //        Name = "CurseOfDoom";
    //        BaseCost = 380;
    //        BaseCastTime = 0f;
    //        BaseMinDamage = 4200;
    //        BaseMaxDamage = 4200;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 2f;
    //        BaseCritPercent = 0f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
    //        BasePeriodTickInterval = 60f;
    //        BasePeriodicDamage = 4200f;
    //        BasePeriodicDuration = 60f;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;

    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        //shadow mastery removed here even though it should apply (but doesn't)
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) / (1f + character.Talents.GetTalent("ShadowMastery").PointsInvested * 0.02f) * (stats.BonusSpellPowerMultiplier);
            
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        AverageDamage = MinDamage;

    //        //extra hit rating from supression
    //        float supressionbonus = (stats.SpellHitRating + character.Talents.GetTalent("Supression").PointsInvested * 2f);
    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f) - (supressionbonus / 100f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}


    
    //internal class Incinerate : Spell
    //{
    //    private List<Spell> currentSpellsOnTarget;

    //    //this constructor assumes immolate is up
    //    public Incinerate(Character character, Stats stats)
    //    {
    //        Name = "Incinerate";
    //        BaseCost = 355;
    //        BaseCastTime = 2.5f;
    //        BaseMinDamage = 444f + 111f;
    //        BaseMaxDamage = 514f + 129f;
    //        MagicSchool = MagicSchool.Fire;
    //        BaseSpellDamageCoefficient = 0.7143f;
    //        BaseCritPercent = stats.SpellCritRating / 22.08f;
    //        SpellDamage = stats.SpellDamageRating;
    //        FireDamage = stats.SpellFireDamageRating;
    //        BaseRange = 30;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;

    //        //destruction talents
    //        Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
    //        CritPercent += tal.GetTalent("Devastation").PointsInvested;
    //        HealthReturnFactor = tal.GetTalent("SoulLeech").PointsInvested / 10f;
    //        SpellDamageCoefficient += tal.GetTalent("ShadowandFlame").PointsInvested * 0.04f;
    //        if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        CastTime /= (1 + (stats.SpellHasteRating / 1570f));
    //        DamageModifier *= (stats.BonusFireSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
    //        MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
    //        float minCrit = MinDamage * CritModifier;
    //        float maxCrit = MaxDamage * CritModifier;
    //        AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;

    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}

    //internal class Immolate : Spell
    //{
    //    float PeriodicSpellDamageCoefficient = 0.38f;

    //    public Immolate(Character character, Stats stats)
    //    {
    //        Name = "Immolate";
    //        BaseCost = 445;
    //        BaseCastTime = 2.0f;
    //        BaseMinDamage = 327f;
    //        BaseMaxDamage = 327f;
    //        MagicSchool = MagicSchool.Fire;
    //        BaseSpellDamageCoefficient = 0.19f;
    //        BaseCritPercent = stats.SpellCritRating / 22.08f;
    //        SpellDamage = stats.SpellDamageRating;
    //        FireDamage = stats.SpellFireDamageRating;
    //        BaseRange = 30;
    //        BasePeriodicDamage = 615f / 5f;
    //        BasePeriodicDuration = 15f;
    //        BasePeriodTickInterval = 3f;
            


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;

    //        //destruction talents
    //        Cost = Convert.ToInt32((100f - tal.GetTalent("Cataclysm").PointsInvested) / 100f * BaseCost);
    //        CastTime -= (tal.GetTalent("Bane").PointsInvested / 10f);
    //        CritPercent += tal.GetTalent("Devastation").PointsInvested;
    //        MinDamage = MaxDamage *= tal.GetTalent("ImprovedImmolate").PointsInvested * 0.05f + 1f;
    //        if (tal.GetTalent("Ruin").PointsInvested == 1) CritModifier = 2f;
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        CastTime /= (1 + (stats.SpellHasteRating / 1570f));
    //        DamageModifier *= (stats.BonusFireSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
    //        MaxDamage = (MaxDamage + (SpellDamageCoefficient * (SpellDamage + FireDamage))) * DamageModifier;
    //        float minCrit = MinDamage * CritModifier;
    //        float maxCrit = MaxDamage * CritModifier;
    //        AverageDamage = ((MinDamage + minCrit * (CritPercent / 100f)) + (MaxDamage + maxCrit * (CritPercent / 100f))) / 2f;
    //        //dot portion
    //        AverageDamage += PeriodicDamage * (PeriodicDuration / PeriodicTickInterval) + (SpellDamage + FireDamage) * PeriodicSpellDamageCoefficient;
    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}

    //internal class Corruption : Spell
    //{
    //    public Corruption(Character character, Stats stats)
    //    {
    //        Name = "Corruption";
    //        BaseCost = 370;
    //        BaseCastTime = 2f;
    //        BaseMinDamage = 900;
    //        BaseMaxDamage = 900;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 0.936f;
    //        BaseCritPercent = 0f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
    //        BasePeriodTickInterval = 3f;
    //        BasePeriodicDamage = 900f / 6f;
    //        BasePeriodicDuration = 18f;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;

    //        //Empowered corruption
    //        SpellDamageCoefficient += tal.GetTalent("EmpoweredCorruption").PointsInvested * 0.12f;
    //        //Contagion
    //        MinDamage = MaxDamage *= (1f + tal.GetTalent("Contagion").PointsInvested * 0.1f);
    //        //Improved Corruption
    //        CastTime -= tal.GetTalent("ImprovedCorruption").PointsInvested * 0.4f;
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        AverageDamage = MinDamage;
    //        float supressionbonus = (stats.SpellHitRating + character.Talents.GetTalent("Supression").PointsInvested * 2f);
    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f) - (supressionbonus / 100f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }
    //}


    //internal class UnstableAffliction : Spell
    //{
    //    public UnstableAffliction(Character character, Stats stats)
    //    {
    //        Name = "UnstableAffliction";
    //        BaseCost = 400;
    //        BaseCastTime = 1.5f;
    //        BaseMinDamage = 1050;
    //        BaseMaxDamage = 1050;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 1.80f;
    //        BaseCritPercent = 0f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
    //        BasePeriodTickInterval = 3f;
    //        BasePeriodicDamage = 1050f / 6f;
    //        BasePeriodicDuration = 18f;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        AverageDamage = MinDamage;
    //        float supressionbonus = (stats.SpellHitRating + character.Talents.GetTalent("Supression").PointsInvested * 2f);
    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f) - (supressionbonus / 100f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}

    //internal class SiphonLife : Spell
    //{
    //    public SiphonLife(Character character, Stats stats)
    //    {
    //        Name = "SiphonLife";
    //        BaseCost = 410;
    //        BaseCastTime = 0f;
    //        BaseMinDamage = 630;
    //        BaseMaxDamage = 630;
    //        MagicSchool = MagicSchool.Shadow;
    //        BaseSpellDamageCoefficient = 1f;
    //        BaseCritPercent = 0f;
    //        SpellDamage = stats.SpellDamageRating;
    //        ShadowDamage = stats.SpellShadowDamageRating;
    //        BaseRange = 30;
    //        BasePeriodTickInterval = 3f;
    //        BasePeriodicDamage = 630f / 10f;
    //        BasePeriodicDuration = 30f;


    //        CritPercent = BaseCritPercent;
    //        CastTime = BaseCastTime;
    //        SpellDamageCoefficient = BaseSpellDamageCoefficient;
    //        Range = BaseRange;
    //        Cost = BaseCost;
    //        PeriodicTickInterval = BasePeriodTickInterval;
    //        PeriodicDuration = BasePeriodicDuration;
    //        PeriodicDamage = BasePeriodicDamage;
    //        MinDamage = BaseMinDamage;
    //        MaxDamage = BaseMaxDamage;

    //        ParseTalents(character, stats);
    //        Calculate(character, stats);
    //    }

    //    private void ParseTalents(Character character, Stats stats)
    //    {
    //        TalentTree tal = character.Talents;
    //    }

    //    private void Calculate(Character character, Stats stats)
    //    {
    //        DamageModifier *= (stats.BonusShadowSpellPowerMultiplier) * (stats.BonusSpellPowerMultiplier);
    //        MinDamage = (MinDamage + (SpellDamageCoefficient * (SpellDamage + ShadowDamage))) * DamageModifier;
    //        AverageDamage = MinDamage;
    //        float supressionbonus = (stats.SpellHitRating + character.Talents.GetTalent("Supression").PointsInvested * 2f);
    //        float missRate = ChanceToMiss - (stats.SpellHitRating / 1262.5f) - (supressionbonus / 100f);
    //        float spellHitFactor = missRate < 0 ? 0.99f : 0.99f - missRate;
    //        AverageDamage *= spellHitFactor;
    //    }

    //}


}
