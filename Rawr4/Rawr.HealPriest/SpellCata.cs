using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.HealPriest
{
    public class ResultDamage
    {
    }

    public class ResultHeal
    {
    }

    public abstract class SpellCata
    {
        protected PriestInformation priestInformation;
        protected bool bDirty { get; set; }

        public bool IsInstant { get; protected set; }
        public bool IsDirectDamage { get; protected set; }
        public bool IsOverTimeDamage { get; protected set; }
        public bool IsDirectHeal { get; protected set; }
        public bool IsOverTimeHeal { get; protected set; }
        public bool IsAbsorb { get; protected set; }

        public int MaxTargets { get; protected set; }
        public int Targets { get; protected set; }

        public float BaseDirectValue { get; protected set; }
        public float BaseDirectVariation { get; protected set; }
        public float BaseDirectCoefficient { get; protected set; }

        public float BaseOverTimeValue { get; protected set; }
        public float BaseOverTimeCoefficient { get; protected set; }
        public float BaseOverTimeTickPeriod { get; protected set; }
        public float BaseOverTimeTicks { get; protected set; }

        public float BaseManaCost { get; protected set; }
        public float ManaCost { get; protected set; }

        public float BaseCastTime { get; protected set; }
        public float CastTime { get; protected set; }
        public readonly float BaseGlobalCooldown = 1.5f;
        public float GlobalCooldown { get; protected set; }
        public float CritChance { get; protected set; }

        public virtual void SetPriestInformation(PriestInformation PI) { priestInformation = PI; UpdateSpell();  }
        
        public virtual void UpdateSpell() {
            bDirty = false;
            ManaCost = BaseManaCost * priestInformation.baseMana;
            float hc = 1f / (1f + priestInformation.haste);
            IsInstant = BaseCastTime == 0f;
            if (IsInstant)
                CastTime = 0f;
            else
                CastTime = Math.Max(1f, BaseCastTime * hc);
            GlobalCooldown = Math.Max(1f, BaseGlobalCooldown * hc);
        }

        public override string ToString()
        {
            string retval = String.Format("Cast Time: %s s", (IsInstant) ? String.Format("Instant %s", GlobalCooldown.ToString("0.00")) : CastTime.ToString("0.00"));
            retval += String.Format("\nMana Cost: %s", ManaCost.ToString("0.00"));
            retval += String.Format("\nCrit Chance: %s", CritChance.ToString("0.00"));
            if (IsDirectDamage)
                retval += ToStringDirectDamage();
            if (IsOverTimeDamage)
                retval += ToStringOverTimeDamage();
            if (IsDirectHeal)
                retval += ToStringDirectHeal();
            if (IsOverTimeHeal)
                retval += ToStringOverTimeHeal();
            if (IsAbsorb)
                retval += ToStringAbsorb();
            return retval;
        }

        protected virtual string ToStringDirectDamage()
        {
            return String.Empty;
        }

        protected virtual string ToStringOverTimeDamage()
        {
            return String.Empty;
        }

        protected virtual string ToStringDirectHeal()
        {
            return String.Empty;
        }

        protected virtual string ToStringOverTimeHeal()
        {
            return String.Empty;
        }

        protected virtual string ToStringAbsorb()
        {
            return String.Empty;
        }
    }

    public abstract class HealSpell : SpellCata
    {
        protected float AbsorbRatio;
        protected float HoTMasteryRatio;

        public float DirectHealMinHit { get; protected set; }
        public float DirectHealMaxHit { get; protected set; }
        public float DirectHealMinCrit { get; protected set; }
        public float DirectHealMaxCrit { get; protected set; }
        public float DirectHealAvgHit { get; protected set; }
        public float DirectHealAvgCrit { get; protected set; }
        public float DirectHealAvg { get; protected set; }

        public float OverTimeHealHit { get; protected set; }
        public float OverTimeHealCrit { get; protected set; }
        public float OverTimeHealAvg { get; protected set; }
        public float OverTimeTickPeriod { get; protected set; }
        public float OverTimeTicks { get; protected set; }

        public float AbsorbMinHit { get; protected set; }
        public float AbsorbMaxHit { get; protected set; }
        public float AbsorbMinCrit { get; protected set; }
        public float AbsorbMaxCrit { get; protected set; }
        public float AbsorbAvgHit { get; protected set; }
        public float AbsorbAvgCrit { get; protected set; }
        public float AbsorbAvg { get; protected set; }

        public override void UpdateSpell()
        {
            base.UpdateSpell();
        }

        protected override string ToStringDirectHeal()
        {
            return base.ToStringDirectHeal();
        }

        protected override string ToStringOverTimeHeal()
        {
            return base.ToStringOverTimeHeal();
        }

        protected override string ToStringAbsorb()
        {
            string s = base.ToStringAbsorb();
            if (AbsorbMinHit == AbsorbMaxHit)
                s += String.Format("Absorb : %s", AbsorbAvgHit.ToString("0.00"));
            else
                s += String.Format("Absorb : %s - %s (Avg %s)", AbsorbMinHit.ToString("0.00"), AbsorbMaxHit.ToString("0.00"), AbsorbAvgHit.ToString("0.00"));
            if (AbsorbAvgHit != AbsorbAvgCrit)
            {
                if (AbsorbMinCrit == AbsorbMaxCrit)
                    s += String.Format("Crit Absorb : %s", AbsorbAvgCrit.ToString("0.00"));
                else
                    s += String.Format("Crit Absorb : %s - %s (Avg %s)", AbsorbMinCrit.ToString("0.00"), AbsorbMaxCrit.ToString("0.00"), AbsorbAvgCrit.ToString("0.00"));
            }

            return s;
        }
    }

    public abstract class DamageSpell : SpellCata
    {
        public float DirectDamageMinHit { get; protected set; }
        public float DirectDamageMaxHit { get; protected set; }
        public float DirectDamageMinCrit { get; protected set; }
        public float DirectDamageMaxCrit { get; protected set; }
        public float DirectDamageAvgHit { get; protected set; }
        public float DirectDamageAvgCrit { get; protected set; }
        public float DirectDamageAvg { get; protected set; }

        public float OverTimeDamageHit { get; protected set; }
        public float OverTimeDamageCrit { get; protected set; }
        public float OverTimeDamageAvg { get; protected set; }
        public float OverTimeTickPeriod { get; protected set; }
        public float OverTimeTicks { get; protected set; }

        public override void UpdateSpell()
        {
            base.UpdateSpell();
        }

        protected override string ToStringDirectDamage()
        {
            return base.ToStringDirectDamage();
        }

        protected override string ToStringOverTimeDamage()
        {
            return base.ToStringOverTimeDamage();
        }
    }

    /// actual spells!
    /// 

    public class spellHeal : HealSpell
    {

        public override void UpdateSpell()
        {
            base.UpdateSpell();

        }
    }
}
