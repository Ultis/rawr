using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    enum MagicSchool
    {
        Arcane,
        Fire,
        Frost,
        Nature
    }

    abstract class Spell
    {
        public string Name;
        public float DamagePerSecond;
        public float CostPerSecond;
        public float ManaRegenPerSecond;

        public abstract void Calculate(Character character, CharacterCalculationsMage calculations);
    }

    abstract class BaseSpell : Spell
    {
        public bool Channeled;
        public bool Instant;
        public bool Binary;
        public bool AreaEffect;
        public int BaseCost;
        public int BaseRange;
        public float BaseCastTime;
        public float BaseCooldown;
        public MagicSchool MagicSchool;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BasePeriodicDamage;
        public float SpellDamageCoefficient;
        public float HitProcs;
        public float TargetProcs;
        public float CastProcs;

        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, spellDamageCoefficient) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, instant ? (1.5f / 3.5f) : (castTime / 3.5f)) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, hitProcs, castProcs, instant ? (1.5f / 3.5f) : (castTime / 3.5f)) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs, float spellDamageCoefficient)
        {
            Name = name;
            Channeled = channeled;
            Binary = binary;
            Instant = instant;
            AreaEffect = areaEffect;
            BaseCost = cost;
            BaseRange = range;
            BaseCastTime = castTime;
            BaseCooldown = cooldown;
            MagicSchool = magicSchool;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            SpellDamageCoefficient = spellDamageCoefficient;
            HitProcs = hitProcs;
            CastProcs = castProcs;
            TargetProcs = hitProcs;
        }

        public float CostModifier;
        public float SpellModifier;
        public float RealResistance;
        public float CritRate;
        public float CritBonus;
        public float HitRate;
        public float PartialResistFactor;
        public float RawSpellDamage;
        public float SpellDamage;
        public float AverageDamage;
        public bool AveragedClearcasting = true;
        public bool ClearcastingActive;
        public bool ClearcastingProccing;

        public float CastTime;
        public float Cooldown;
        public float Cost;

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            if (AreaEffect) TargetProcs *= int.Parse(character.CalculationOptions["AoeTargets"]);
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01f * int.Parse(character.CalculationOptions["Pyromaniac"]);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01f * int.Parse(character.CalculationOptions["ElementalPrecision"]);
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05f * int.Parse(character.CalculationOptions["FrostChanneling"]);
            if (calculations.ArcanePower) CostModifier += 0.3f;

            CastTime = BaseCastTime / calculations.CastingSpeed + calculations.Latency;

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    SpellModifier = calculations.ArcaneSpellModifier;
                    CritRate = calculations.ArcaneCritRate;
                    CritBonus = calculations.ArcaneCritBonus;
                    RawSpellDamage = calculations.ArcaneDamage;
                    HitRate = calculations.ArcaneHitRate;
                    RealResistance = float.Parse(character.CalculationOptions["ArcaneResist"]);
                    break;
                case MagicSchool.Fire:
                    SpellModifier = calculations.FireSpellModifier;
                    CritRate = calculations.FireCritRate;
                    CritBonus = calculations.FireCritBonus;
                    RawSpellDamage = calculations.FireDamage;
                    HitRate = calculations.FireHitRate;
                    RealResistance = float.Parse(character.CalculationOptions["FireResist"]);
                    break;
                case MagicSchool.Frost:
                    SpellModifier = calculations.FrostSpellModifier;
                    CritRate = calculations.FrostCritRate;
                    CritBonus = calculations.FrostCritBonus;
                    RawSpellDamage = calculations.FrostDamage;
                    HitRate = calculations.FrostHitRate;
                    RealResistance = float.Parse(character.CalculationOptions["FrostResist"]);
                    break;
                case MagicSchool.Nature:
                    SpellModifier = calculations.NatureSpellModifier;
                    CritRate = calculations.NatureCritRate;
                    CritBonus = calculations.NatureCritBonus;
                    RawSpellDamage = calculations.NatureDamage;
                    HitRate = calculations.NatureHitRate;
                    RealResistance = float.Parse(character.CalculationOptions["NatureResist"]);
                    break;
            }

            int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - calculations.BasicStats.SpellPenetration / 350f * 0.75f) + ((targetLevel > 70 && !Binary) ? ((targetLevel - 70) * 0.02f) : 0f));
        }

        protected void CalculateDerivedStats(Character character, CharacterCalculationsMage calculations)
        {
            if (CastTime < calculations.GlobalCooldown + calculations.Latency) CastTime = calculations.GlobalCooldown + calculations.Latency;
            Cost = BaseCost * CostModifier;

            Cost *= (1f - CritRate + CritRate * (1f - 0.1f * int.Parse(character.CalculationOptions["MasterOfElements"])));

            CostPerSecond = Cost / CastTime;

            if (AveragedClearcasting)
            {
                CostPerSecond *= (1 - 0.02f * int.Parse(character.CalculationOptions["ArcaneConcentration"]));
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            SpellDamage = RawSpellDamage * SpellDamageCoefficient;
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - calculations.ResilienceCritRateReduction);
            float resistMultiplier = HitRate * PartialResistFactor;
            // TODO dot scaling by spell damage
            AverageDamage = (baseAverage * critMultiplier + BasePeriodicDamage) * SpellModifier * resistMultiplier;
            DamagePerSecond = AverageDamage / CastTime;

            if (Name != "Lightning Bolt" && calculations.BasicStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = (BaseSpell)calculations.GetSpell("Lightning Bolt");
                DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }

            FSRCalc fsr = new FSRCalc();
            fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);

            ManaRegenPerSecond = calculations.ManaRegen5SR + fsr.CalculateOO5SR(calculations.ClearcastingChance) * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;
        }
    }

    class FireBlast : BaseSpell
    {
        public FireBlast(Character character, CharacterCalculationsMage calculations)
            : base("Fire Blast", false, false, true, false, 465, 20, 0, 8, MagicSchool.Fire, 664, 786, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            Cooldown -= 0.5f * int.Parse(character.CalculationOptions["ImprovedFireBlast"]);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["Incineration"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Scorch : BaseSpell
    {
        public Scorch(Character character, CharacterCalculationsMage calculations)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["Incineration"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Flamestrike : BaseSpell
    {
        public Flamestrike(Character character, CharacterCalculationsMage calculations)
            : base("Flamestrike", false, false, false, true, 1175, 30, 3, 0, MagicSchool.Fire, 480, 585, 340, 0.83f)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritRate += 0.05f * int.Parse(character.CalculationOptions["ImprovedFlamestrike"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class FrostNova : BaseSpell
    {
        public FrostNova(Character character, CharacterCalculationsMage calculations)
            : base("Frost Nova", false, true, true, true, 185, 0, 0, 25, MagicSchool.Frost, 100, 113, 0, 1.5f / 3.5f * 0.5f * 0.13f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            Cooldown -= 2 * int.Parse(character.CalculationOptions["ImprovedFrostNova"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Frostbolt : BaseSpell
    {
        public Frostbolt(Character character, CharacterCalculationsMage calculations)
            : base("Frostbolt", false, true, false, false, 330, 30, 3, 0, MagicSchool.Frost, 600, 647, 0, 0.95f * 3f / 3.5f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CastTime = (BaseCastTime - 0.1f * int.Parse(character.CalculationOptions["ImprovedFrostbolt"])) / calculations.CastingSpeed + calculations.Latency;
            CritRate += 0.01f * int.Parse(character.CalculationOptions["EmpoweredFrostbolt"]);
            SpellDamageCoefficient += 0.02f * int.Parse(character.CalculationOptions["EmpoweredFrostbolt"]);
            int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.02f * int.Parse(character.CalculationOptions["ElementalPrecision"])); // bugged Elemental Precision
            CalculateDerivedStats(character, calculations);
        }
    }

    class Fireball : BaseSpell
    {
        public Fireball(Character character, CharacterCalculationsMage calculations)
            : base("Fireball", false, false, false, false, 425, 35, 3.5f, 0, MagicSchool.Fire, 649, 821, 84)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CastTime = (BaseCastTime - 0.1f * int.Parse(character.CalculationOptions["ImprovedFireball"])) / calculations.CastingSpeed + calculations.Latency;
            SpellDamageCoefficient += 0.03f * int.Parse(character.CalculationOptions["EmpoweredFireball"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ConeOfCold : BaseSpell
    {
        public ConeOfCold(Character character, CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, true, 645, 0, 0, 10, MagicSchool.Frost, 418, 457, 0, 1.5f / 3.5f * 0.5f * 0.95f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            int ImprovedConeOfCold = int.Parse(character.CalculationOptions["ImprovedConeOfCold"]);
            SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0));
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneBlast : BaseSpell
    {
        public ArcaneBlast(Character character, CharacterCalculationsMage calculations, int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(character, calculations);
        }

        private int timeDebuff;
        private int costDebuff;

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CastTime = (BaseCastTime - timeDebuff / 3f) / calculations.CastingSpeed + calculations.Latency;
            CostModifier += 0.75f * costDebuff + calculations.BasicStats.ArcaneBlastBonus;
            SpellModifier *= (1 + calculations.BasicStats.ArcaneBlastBonus);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["ArcaneImpact"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissiles : BaseSpell
    {
        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0, 5, 1)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CostModifier += 0.02f * int.Parse(character.CalculationOptions["EmpoweredArcaneMissiles"]);
            CritRate += 0.01f * int.Parse(character.CalculationOptions["ArcanePotency"]); // CC double dipping
            SpellDamageCoefficient += 0.15f * int.Parse(character.CalculationOptions["EmpoweredArcaneMissiles"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneExplosion : BaseSpell
    {
        public ArcaneExplosion(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Explosion", false, false, true, true, 545, 0, 0, 0, MagicSchool.Arcane, 377, 407, 0, 1.5f / 3.5f * 0.5f)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["ArcaneImpact"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class LightningBolt : BaseSpell
    {
        public LightningBolt(Character character, CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, false, true, false, 0, 30, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CalculateDerivedStats(character, calculations);
        }
    }

    class SpellCycle : Spell
    {
        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            throw new NotImplementedException();
        }
    }
}
