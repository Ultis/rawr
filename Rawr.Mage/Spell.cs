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
        public decimal DamagePerSecond;
        public decimal CostPerSecond;
        public decimal ManaRegenPerSecond;

        public abstract void Calculate(Character character);
    }

    class BaseSpell : Spell
    {
        public string Name;
        public bool Channeled;
        public bool Instant;
        public bool AreaEffect;
        public int BaseCost;
        public int BaseRange;
        public decimal BaseCastTime;
        public decimal BaseCooldown;
        public MagicSchool MagicSchool;
        public decimal BaseMinDamage;
        public decimal BaseMaxDamage;
        public decimal BasePeriodicDamage;

        public BaseSpell(string name, bool channeled, bool instant, bool areaEffect, int cost, int range, decimal castTime, decimal cooldown, MagicSchool magicSchool, decimal minDamage, decimal maxDamage, decimal periodicDamage)
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
        }

        protected decimal CostModifier;
        protected bool AveragedClearcasting = true;
        protected bool ClearcastingActive;
        protected bool ClearcastingProccing;

        public decimal CastTime;
        public decimal Cooldown;
        public int Cost;

        public override void Calculate(Character character)
        {
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01m * int.Parse(character.CalculationOptions["Pyromaniac"]);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01m * int.Parse(character.CalculationOptions["ElementalPrecision"]);
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05m * int.Parse(character.CalculationOptions["FrostChanneling"]);
        }

        protected void CalculateDerivedStats(Character character)
        {
            Cost = (int)(BaseCost * CostModifier);

            CostPerSecond = Cost / CastTime;
            if (AveragedClearcasting)
            {
                CostPerSecond *= (1 - 0.02m * int.Parse(character.CalculationOptions["ArcaneConcentration"]));
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }
        }
    }

    class FireBlast : BaseSpell
    {
        public FireBlast() : base("Fire Blast", false, true, false, 465, 20, 0, 8, MagicSchool.Fire, 664, 786, 0) { }

        public override void Calculate(Character character)
        {
            base.Calculate(character);
            Cooldown -= 0.5m * int.Parse(character.CalculationOptions["ImprovedFireBlast"]);
            CalculateDerivedStats(character);
        }
    }

    class FrostNova : BaseSpell
    {
        public FrostNova() : base("Frost Nova", false, true, true, 185, 0, 0, 25, MagicSchool.Frost, 100, 113, 0) { }

        public override void Calculate(Character character)
        {
            base.Calculate(character);
            Cooldown -= 2 * int.Parse(character.CalculationOptions["ImprovedFrostNova"]);
            CalculateDerivedStats(character);
        }
    }

    class ArcaneBlast : BaseSpell
    {
        public ArcaneBlast(int timeDebuff, int costDebuff) : base("Arcane Blast", false, false, false, 195, 30, 2.5m, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
        }

        private int timeDebuff;
        private int costDebuff;

        public override void Calculate(Character character)
        {
            base.Calculate(character);
            //CostModifier += 0.75 * costDebuff + 0.2 * 2T5;
            CalculateDerivedStats(character);
        }
    }

    class ArcaneMissiles : BaseSpell
    {
        public ArcaneMissiles() : base("Arcane Missiles", true, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0) { }

        public override void Calculate(Character character)
        {
            base.Calculate(character);
            CostModifier += 0.02m * int.Parse(character.CalculationOptions["EmpoweredArcaneMissiles"]);
            CalculateDerivedStats(character);
        }
    }

    class SpellCycle : Spell
    {
        public override void Calculate(Character character)
        {
            throw new NotImplementedException();
        }
    }
}
