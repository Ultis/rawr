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
            BonusMultiplier = 1;
        }

        //derived stats
        public float CritRate { get; set; }
        public float CritBonus { get; set; }
        public float ChanceToHit { get; set; }
        public float Damage { get; set; }
        public float Frequency { get; set; }
        public float CastRatio { get; set; }
        public float CastTime { get; set; }
        public float ManaPerSecond { get; set; }
        public float ManaCost { get; set; }
        public float BonusMultiplier { get; set; }
        //public float HealthPerSecond { get; set; }

        public void CalculateDerivedStats(CharacterCalculationsWarlock calculations)
        {
            //hit rate
            ChanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, calculations.HitPercent);
            if (SpellTree == SpellTree.Affliction)
                ChanceToHit = Math.Min(0.99f, ChanceToHit + 2 * calculations.CalculationOptions.Suppression);

            //cast time
            CastTime = BaseCastTime / (1 + 0.01f * calculations.HastePercent);
            CastTime += calculations.CalculationOptions.Latency;
            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            //for DoTs, factor in the chance to miss (because you have to re-apply)
            if (BaseDotDuration != 0)
                CastTime /= ChanceToHit;
            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;

            //frequency
            if (BaseDotDuration == 0)
                Frequency = CastTime;
            else
                Frequency = BaseDotDuration + CastTime + calculations.CalculationOptions.DotGap - (BaseCastTime + calculations.CalculationOptions.Latency);

            //mana cost
            ManaCost = BaseManaCost;
            if (SpellTree == SpellTree.Destruction)
                ManaCost *= (1 - 0.01f * calculations.CalculationOptions.Cataclysm);
            if (BaseDotDuration != 0)
                ManaCost /= ChanceToHit;
            ManaCost = (float)Math.Round(ManaCost);
            ManaPerSecond = ManaCost / Frequency;

            //cast ratio
            CastRatio = CastTime / Frequency;
        }

        public void CalculateDamage(CharacterCalculationsWarlock calculations)
        {
            if (calculations.BasicStats.BonusSpellCritMultiplier > 0)
                CritBonus = 1.09f;
            CritBonus *= 0.5f + 0.5f * calculations.CalculationOptions.Ruin;

            float plusDamage = 0;
            switch(MagicSchool)
            {
                case MagicSchool.Shadow:
                    plusDamage = calculations.ShadowDamage;
                    BonusMultiplier *= calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusShadowSpellPowerMultiplier * (1 + 0.2f * calculations.IsbUptime);
                    break;
                case MagicSchool.Fire:
                    plusDamage = calculations.FireDamage;
                    BonusMultiplier *= calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusFireSpellPowerMultiplier;
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
                averageDamage *= 1 + 0.01f * calculations.CritPercent * CritBonus;
                if (BaseDotDuration == 0)
                    averageDamage *= ChanceToHit;
                else if (this is Immolate)
                    averageDamage *= 1 + 0.05f * calculations.CalculationOptions.ImprovedImmolate;
            }

            if (dotDamage != 0)
                dotDamage += plusDamage * DotDamageCoefficient;

            Damage = (float)Math.Round((averageDamage + dotDamage) * BonusMultiplier);
        }
    }

    internal class ShadowBolt : Spell
    {
        public ShadowBolt(CharacterCalculationsWarlock calculations)
            : base("Shadow Bolt", MagicSchool.Shadow, SpellTree.Destruction, 541, 603, 0, 0, 3, 420)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
            BonusMultiplier *= 1 + calculations.BasicStats.BonusWarlockNukeMultiplier;
        }
    }

    internal class Incinerate : Spell
    {
        public Incinerate(CharacterCalculationsWarlock calculations)
            : base("Incinerate", MagicSchool.Fire, SpellTree.Destruction, 555, 642, 0, 0, 2.5f, 355)
        {
            BaseCastTime *= 1 - 0.02f * calculations.CalculationOptions.Emberstorm;
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
            BonusMultiplier *= 1 + calculations.BasicStats.BonusWarlockNukeMultiplier;
        }
    }

    internal class Immolate : Spell
    {
        public Immolate(CharacterCalculationsWarlock calculations)
            : base("Immolate", MagicSchool.Fire, SpellTree.Destruction, 327, 327, 615, 15, 2, 445)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
        }
    }

    internal class CurseOfAgony : Spell
    {
        public CurseOfAgony(CharacterCalculationsWarlock calculations)
            : base("Curse of Agony", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1356, 24, 1.5f, 265)
        {
            BonusMultiplier *= 1 + 0.05f * calculations.CalculationOptions.ImprovedCurseOfAgony;
            BonusMultiplier *= 1 + 0.01f * calculations.CalculationOptions.Contagion;
            DotDamageCoefficient = 1.2f;
        }
    }

    internal class CurseOfDoom : Spell
    {
        public CurseOfDoom(CharacterCalculationsWarlock calculations)
            : base("Curse of Doom", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 4200, 60, 1.5f, 380)
        {
            BonusMultiplier /= 1 + 0.02f * calculations.CalculationOptions.ShadowMastery;
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
            if (BaseCastTime < 1.5f)
                BaseCastTime = 1.5f;
            BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
            BonusMultiplier *= 1 + 0.01f * calculations.CalculationOptions.Contagion;
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
}
