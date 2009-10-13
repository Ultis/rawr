using System;

namespace Rawr.Elemental.Spells
{
    public abstract class Spell
    {
        protected float baseMinDamage = 0f;
        protected float baseMaxDamage = 0f;
        protected float baseCastTime = 0f;
        protected float castTime = 0f;
        protected float periodicTick = 0f;
        protected float periodicTicks = 0f;
        protected float periodicTickTime = 3f;
        protected float manaCost = 0f;
        protected float gcd = 1.5f;
        protected float crit = 0f;
        protected float critModifier = 1f;
        protected float cooldown = 0f;
        protected float missChance = .17f;

        protected float totalCoef = 1f;
        protected float baseCoef = 1f;
        protected float spCoef = 0f;
        protected float dotBaseCoef = 1f;
        protected float dotSpCoef = 0f;
        protected float dotCanCrit = 0f;
        protected float spellPower = 0f;

        /// <summary>
        /// This Constructor calls SetBaseValues.
        /// </summary>
        public Spell()
        {
            SetBaseValues();
        }

        protected virtual void SetBaseValues()
        {
            baseMinDamage = 0f;
            baseMaxDamage = 0f;
            baseCastTime = 0f;
            castTime = 0f;
            periodicTick = 0f;
            periodicTicks = 0f;
            periodicTickTime = 3f;
            manaCost = 0f;
            gcd = 1.5f;
            crit = 0f;
            critModifier = 1f;
            cooldown = 0f;
            missChance = .17f;

            totalCoef = 1f;
            baseCoef = 1f;
            spCoef = 0f;
            dotBaseCoef = 1f;
            dotSpCoef = 0f;
            dotCanCrit = 0f;
            spellPower = 0f;
        }

        public void Update(Stats stats, ShamanTalents talents)
        {
            SetBaseValues();
            Initialize(stats, talents);
        }

        protected string shortName = "Spell";

        protected static void add(Spell sp1, Spell sp2, Spell nS)
        {
            nS.baseMinDamage = (sp1.baseMinDamage + sp2.baseMaxDamage);
            nS.baseMaxDamage = (sp1.baseMaxDamage + sp2.baseMaxDamage);
            nS.castTime = (sp1.castTime + sp2.castTime);
            nS.periodicTick = (sp1.periodicTick + sp2.periodicTick);
            nS.periodicTicks = (sp1.periodicTicks + sp2.periodicTicks);
            nS.periodicTickTime = (sp1.periodicTickTime + sp2.periodicTickTime);
            nS.manaCost = (sp1.manaCost + sp2.manaCost);
            nS.gcd = (sp1.gcd + sp2.gcd);
            nS.crit = (sp1.crit + sp2.crit);
            nS.critModifier = (sp1.critModifier + sp2.critModifier);
            nS.cooldown = (sp1.cooldown + sp2.cooldown);
            nS.missChance = (sp1.missChance + sp2.missChance);
            nS.totalCoef = (sp1.totalCoef + sp2.totalCoef);
            nS.baseCoef = (sp1.baseCoef + sp2.baseCoef);
            nS.spCoef = (sp1.spCoef + sp2.spCoef);
            nS.dotBaseCoef = (sp1.dotBaseCoef + sp2.dotBaseCoef);
            nS.dotSpCoef = (sp1.dotSpCoef + sp2.dotSpCoef);
            nS.spellPower = (sp1.spellPower + sp2.spellPower);
        }

        protected static void multiply(Spell sp1, float c, Spell nS)
        {
            nS.baseMinDamage = sp1.baseMinDamage * c;
            nS.baseMaxDamage = sp1.baseMaxDamage * c;
            nS.castTime = sp1.castTime * c;
            nS.periodicTick = sp1.periodicTick * c;
            nS.periodicTicks = sp1.periodicTicks * c;
            nS.periodicTickTime = sp1.periodicTickTime * c;
            nS.manaCost = sp1.manaCost * c;
            nS.gcd = sp1.gcd * c;
            nS.crit = sp1.crit * c;
            nS.critModifier = sp1.critModifier * c;
            nS.cooldown = sp1.cooldown * c;
            nS.missChance = sp1.missChance * c;
            nS.totalCoef = sp1.totalCoef * c;
            nS.baseCoef = sp1.baseCoef * c;
            nS.spCoef = sp1.spCoef * c;
            nS.dotBaseCoef = sp1.dotBaseCoef * c;
            nS.dotSpCoef = sp1.dotSpCoef * c;
            nS.spellPower = sp1.spellPower * c;
        }

        public virtual float MinHit
        { get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef); } }

        public float MinCrit
        { get { return MinHit * (1 + critModifier); } }

        public virtual float MaxHit
        { get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef); } }

        public float MaxCrit
        { get { return MaxHit * (1 + critModifier); } }

        public float AvgHit
        { get { return (MinHit + MaxHit) / 2; } }

        public float AvgCrit
        { get { return (MinCrit + MaxCrit) / 2; } }

        public float AvgDamage
        { get { return (1f - CritChance) * AvgHit + CritChance * AvgCrit; } }

        public float MinDamage
        { get { return (1f - CritChance) * MinHit + CritChance * MinCrit; } }

        public float MaxDamage
        { get { return (1f - CritChance) * MaxHit + CritChance * MaxCrit; } }

        public float CastTime
        {
            get
            {
                if (castTime > gcd)
                    return gcd==0?castTime:Math.Max(castTime,1f);
                else if (gcd > 1 || gcd == 0)
                    return gcd;
                else
                    return 1;
            }
        }

        public float BaseCastTime
        {
            get
            {
                return Math.Max(baseCastTime, 1.5f);
            }
        }

        public float CastTimeWithoutGCD
        { get { return castTime; } }

        public float CritChance
        { get { return Math.Min(1f, crit); } }

        /// <summary>
        /// Crit chance for all kind of proc triggers. This exists seperately because of Lightning Overload.
        /// </summary>
        public virtual float CCCritChance
        { get { return CritChance; } }

        public float MissChance
        { get { return missChance; } }

        public float HitChance
        { get { return 1f - missChance; } }

        public float DamageFromSpellPower
        { get { return spellPower * spCoef * totalCoef; } }

        public float PeriodicTick
        { get { return (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + dotCanCrit * critModifier * CritChance); } }

        public float PeriodicTicks
        { get { return periodicTicks; } }

        public float PeriodicDamage()
        {
            return PeriodicDamage(Duration);
        }

        public float PeriodicDamage(float duration)
        {
            if (PeriodicTickTime <= 0 || duration <= 0)
                return 0;
            int effectiveTicks = (int)Math.Floor(Math.Min(duration, Duration) / PeriodicTickTime);
            return PeriodicTick * effectiveTicks;
        }

        public virtual float TotalDamage
        { get { return AvgDamage + PeriodicDamage(); } }

        public virtual float DirectDpS
        { get { return AvgDamage / CastTime; } }

        public float PeriodicDpS
        { get { return PeriodicTick / periodicTickTime; } }

        public float PeriodicTickTime
        { get { return periodicTickTime; } }

        public float DpM
        { get { return TotalDamage / manaCost; } }

        public float DpCT
        { get { return TotalDamage / CastTime; } }

        public float DpPR
        { get { return TotalDamage / PeriodicRefreshTime; } }

        public float DpCDR
        { get { return TotalDamage / CDRefreshTime; } }

        public float CTpDuration
        { get { return Duration > 0 ? CastTime / Duration : 1f; } }

        public float Duration
        { get { return periodicTicks * periodicTickTime; } }

        public float Cooldown
        { get { return cooldown; } }

        public float PeriodicRefreshTime
        { get { return (Duration > CDRefreshTime ? Duration : CDRefreshTime); } }

        public float CDRefreshTime
        { get { return cooldown > CastTime ? cooldown + castTime : CastTime; } }
        //{ get { return cooldown + CastTime; } }

        public float ManaCost
        { get { return manaCost; } }

        public virtual void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            float Speed = (1f + stats.SpellHaste) * (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating));
            gcd = (float)Math.Round(gcd / Speed, 4);
            castTime = (float)Math.Round(castTime / Speed, 4);
            critModifier += .2f * shamanTalents.ElementalFury;
            critModifier *= (float)Math.Round(1.5f * (1f + stats.BonusSpellCritMultiplier) - 1f, 6);
            //critModifier += 1f;
            spellPower += stats.SpellPower;
            crit += stats.SpellCrit;
            missChance -= stats.SpellHit;
            totalCoef *= 1 + stats.BonusDamageMultiplier; //ret + bm buff
            if (missChance < 0) missChance = 0;
            manaCost = (float)Math.Floor(manaCost);
        }

        public void ApplyEM(float modifier)
        {
            crit += modifier * .15f;
            if (crit > 1f)
                crit = 1f;
        }

        public virtual Spell Clone()
        {
            return (Spell)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return shortName;
        }
    }
}