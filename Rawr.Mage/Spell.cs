using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    enum MagicSchool
    {
        Arcane,
        Fire,
        Frost
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

        public BaseSpell(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, instant ? (1.5f / 3.5f) : (castTime / 3.5f)) {}
        public BaseSpell(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient)
        {
            Name = name;
            Channeled = channeled;
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
        }

        public float CostModifier;
        public float SpellModifier;
        public float CritRate;
        public float CritBonus;
        public float RawSpellDamage;
        public float SpellDamage;
        public float AverageDamage;
        public bool AveragedClearcasting = true;
        public bool ClearcastingActive;
        public bool ClearcastingProccing;

        public float CastTime;
        public float Cooldown;
        public int Cost;

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01f * int.Parse(character.CalculationOptions["Pyromaniac"]);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01f * int.Parse(character.CalculationOptions["ElementalPrecision"]);
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05f * int.Parse(character.CalculationOptions["FrostChanneling"]);

            CastTime = BaseCastTime / calculations.CastingSpeed + float.Parse(character.CalculationOptions["Latency"]);

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    SpellModifier = calculations.ArcaneSpellModifier;
                    CritRate = calculations.ArcaneCritRate;
                    CritBonus = calculations.ArcaneCritBonus;
                    RawSpellDamage = calculations.ArcaneDamage;
                    break;
                case MagicSchool.Fire:
                    SpellModifier = calculations.FireSpellModifier;
                    CritRate = calculations.FireCritRate;
                    CritBonus = calculations.FireCritBonus;
                    RawSpellDamage = calculations.FireDamage;
                    break;
                case MagicSchool.Frost:
                    SpellModifier = calculations.FrostSpellModifier;
                    CritRate = calculations.FrostCritRate;
                    CritBonus = calculations.FrostCritBonus;
                    RawSpellDamage = calculations.FrostDamage;
                    break;
            }
        }

        protected void CalculateDerivedStats(Character character, CharacterCalculationsMage calculations)
        {
            Cost = (int)(BaseCost * CostModifier);

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
            AverageDamage = (((BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage) * (1 + (CritBonus - 1) * Math.Max(0, CritRate - calculations.ResilienceCritRateReduction)) + BasePeriodicDamage) * SpellModifier;
            DamagePerSecond = AverageDamage / CastTime;
        }
    }

    class FireBlast : BaseSpell
    {
        public FireBlast(Character character, CharacterCalculationsMage calculations)
            : base("Fire Blast", false, true, false, 465, 20, 0, 8, MagicSchool.Fire, 664, 786, 0)
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
            : base("Scorch", false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
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
            : base("Flamestrike", false, false, true, 1175, 30, 3, 0, MagicSchool.Fire, 480, 585, 340, 0.83f)
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
            : base("Frost Nova", false, true, true, 185, 0, 0, 25, MagicSchool.Frost, 100, 113, 0, 1.5f / 3.5f * 0.5f * 0.13f)
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
            : base("Frostbolt", false, false, false, 330, 30, 3, 0, MagicSchool.Frost, 600, 647, 0, 0.95f * 3f / 3.5f)
        {
            Calculate(character, calculations);
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CastTime = (BaseCastTime - 0.1f * int.Parse(character.CalculationOptions["ImprovedFrostbolt"])) / calculations.CastingSpeed + float.Parse(character.CalculationOptions["Latency"]);
            CritRate += 0.01f * int.Parse(character.CalculationOptions["EmpoweredFrostbolt"]);
            SpellDamageCoefficient += 0.02f * int.Parse(character.CalculationOptions["EmpoweredFrostbolt"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ConeOfCold : BaseSpell
    {
        public ConeOfCold(Character character, CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, 645, 0, 0, 10, MagicSchool.Frost, 418, 457, 0, 1.5f / 3.5f * 0.5f * 0.95f)
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
        public ArcaneBlast(Character character, CharacterCalculationsMage calculations, int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
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
            CostModifier += 0.75f * costDebuff + calculations.BasicStats.ArcaneBlastBonus;
            SpellModifier *= (1 + calculations.BasicStats.ArcaneBlastBonus);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["ArcaneImpact"]);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissiles : BaseSpell
    {
        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0)
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
            : base("Arcane Explosion", false, true, true, 545, 0, 0, 0, MagicSchool.Arcane, 377, 407, 0, 1.5f / 3.5f * 0.5f)
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

    class SpellCycle : Spell
    {
        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            throw new NotImplementedException();
        }
    }
}
