using System;

namespace Rawr.Elemental
{
    public abstract class Spell
    {
        protected float baseMinDamage = 0f;
        protected float baseMaxDamage = 0f;
        protected float castTime = 0f;
        protected float periodicTick = 0f;
        protected float periodicTicks = 0f;
        protected float periodicTickTime = 3f;
        protected float manaCost = 0f;
        protected float gcd = 1.5f;
        protected float crit = 0f;
        protected float critModifier = 1f;
        protected float cooldown = 0f;
        protected float missChance = 0.17f;

        protected float totalCoef = 1f;
        protected float baseCoef = 1f;
        protected float spCoef = 0f;
        protected float dotBaseCoef = 1f;
        protected float dotSpCoef = 0f;
        protected float spellPower = 0f;

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

        public float MinHit
        { get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef); } }

        public float MinCrit
        { get { return MinHit * critModifier; } }

        public float MaxHit
        { get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef); } }

        public float MaxCrit
        { get { return MaxHit * critModifier; } }

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
                    return castTime;
                else if (gcd > 1 || gcd==0)
                    return gcd;
                else
                    return 1;
            }
        }

        public float CastTimeWithoutGCD
        { get { return castTime; } }

        public float CritChance
        { get { return Math.Min(1f, crit); } }

        public float MissChance
        { get { return missChance; } }

        public float HitChance
        { get { return 1f - missChance; } }

        public float DamageFromSpellPower
        { get { return spellPower * spCoef * totalCoef; } }

        public float PeriodicTick
        { get { return periodicTick * dotBaseCoef + spellPower * dotSpCoef; } }

        public float PeriodicTicks
        { get { return periodicTicks; } }

        public float TotalDamage
        { get { return AvgDamage + PeriodicTick * PeriodicTicks; } }

        public float DirectDpS
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

        public void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            float Speed = (1f + stats.SpellHaste) * (1f + stats.HasteRating * 0.000304971132f);
            gcd = (float)Math.Round(gcd / Speed, 4);
            castTime = (float)Math.Round(castTime / Speed, 4);
            critModifier += .20f * shamanTalents.ElementalFury;
            critModifier *= 1.5f * (1 + stats.BonusSpellCritMultiplier) - 1;
            critModifier += 1f;
            spellPower += stats.SpellPower;
            crit += stats.SpellCrit;
            missChance -= stats.SpellHit;
            if (missChance < 0) missChance = 0;
        }

        public void ApplyEM(float modifier)
        {
            crit += modifier * .2f;
            manaCost *= 1 - modifier * .2f;
        }

        public virtual Spell Clone()
        {
            return (Spell)this.MemberwiseClone();
        }
    }

    public class Thunderstorm : Spell
    {
        public Thunderstorm(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 1450;
            baseMaxDamage = 1656;
            castTime = 0f;
            spCoef = .193f; // NOT CORRECT YET, assuming 1.5f/7f * 0.9f (aoe with additional effect)
            manaCost = 0f;
            cooldown = 45f;
            #endregion

            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            crit += .05f * shamanTalents.TidalMastery;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f;

            base.Initialize(stats, shamanTalents);
        }

        public static Thunderstorm operator +(Thunderstorm A, Thunderstorm B)
        {
            Thunderstorm C = (Thunderstorm)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static Thunderstorm operator *(Thunderstorm A, float b)
        {
            Thunderstorm C = (Thunderstorm)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }

    public class LightningBolt : Spell
    {
        public LightningBolt(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 715;
            baseMaxDamage = 815;
            castTime = 2.5f;
            spCoef = .7143f;
            manaCost = 0.1f * Constants.BaseMana;
            #endregion

            castTime -= .1f * shamanTalents.LightningMastery;
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            spCoef *= 1f + .02f * shamanTalents.Shamanism;
            crit += .05f * shamanTalents.TidalMastery;
            manaCost *= 1 - stats.LightningBoltCostReduction / 100f;
            spellPower += stats.LightningSpellPower + stats.SpellNatureDamageRating;
            if (calcOpts.glyphOfLightningBolt) totalCoef *= 1.04f;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f;

            /* emulate lightning overload by increasing crit chance
            totalCoef *= 1f + .04f * shamanTalents.LightningOverload * .5f;
            crit *= 1 + .04f * shamanTalents.LightningOverload;*/

            base.Initialize(stats, shamanTalents);
        }

        public void increaseCritFromOverload(int ranks)
        {
            crit *= (1f + .04f * ranks);
        }

        public static LightningBolt operator +(LightningBolt A, LightningBolt B)
        {
            LightningBolt C = (LightningBolt)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static LightningBolt operator *(LightningBolt A, float b)
        {
            LightningBolt C = (LightningBolt)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }

    public class ChainLightning : Spell
    {
        public ChainLightning(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts, int additionalTargets)
        {
            #region Base Values
            baseMinDamage = 973;
            baseMaxDamage = 1111;
            castTime = 2f;
            spCoef = 2f/3.5f;
            manaCost = 0.26f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            // jumps
            if (additionalTargets < 0) additionalTargets = 0;
            if (additionalTargets > 4) additionalTargets = 4;
            totalCoef *= new float[] { 1f, 1.7f, 2.19f, 2.533f, 2.7731f }[additionalTargets];

            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            castTime -= .1f * shamanTalents.LightningMastery;
            cooldown -= new float[] { 0, .75f, 1.5f, 2.5f }[shamanTalents.StormEarthAndFire];
            /* emulate lightning overload by increasing crit chance
            totalCoef *= 1f + .04f * shamanTalents.LightningOverload * .5f;*/
            crit += .05f * shamanTalents.TidalMastery;
            spellPower += stats.LightningSpellPower + stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;

            base.Initialize(stats, shamanTalents);
        }

        public void increaseCritFromOverload(int ranks)
        {
            crit *= (1f + .04f * ranks);
        }

        public static ChainLightning operator +(ChainLightning A, ChainLightning B)
        {
            ChainLightning C = (ChainLightning)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ChainLightning operator *(ChainLightning A, float b)
        {
            ChainLightning C = (ChainLightning)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }

    public class LavaBurst : Spell
    {
        public LavaBurst(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts, float fs)
        {
            #region Base Values
            baseMinDamage = 1192;
            baseMaxDamage = 1518;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = 0.1f * Constants.BaseMana;
            cooldown = 8f;
            #endregion

            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            totalCoef *= 1f + .02f * shamanTalents.CallOfFlame;
            castTime -= .1f * shamanTalents.LightningMastery;
            spCoef += .04f * shamanTalents.Shamanism;
            // emulate lightning overload by increasing crit chance
            crit += .05f * shamanTalents.TidalMastery;
            critModifier += new float[] { 0f, 0.06f, 0.12f, 0.24f }[shamanTalents.LavaFlows];
            critModifier += stats.BonusLavaBurstCritDamage / 100f;
            //critModifier *= 1 + stats.BonusLavaBurstCritDamage / 100f;
            baseMinDamage += stats.LavaBurstBonus;
            baseMaxDamage += stats.LavaBurstBonus;
            spellPower += stats.SpellFireDamageRating;
            if (calcOpts.glyphOfLava) spCoef += .1f;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            base.Initialize(stats, shamanTalents);

            crit = (1-fs)*crit + fs;
        }

        public static LavaBurst operator +(LavaBurst A, LavaBurst B)
        {
            LavaBurst C = (LavaBurst)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static LavaBurst operator *(LavaBurst A, float b)
        {
            LavaBurst C = (LavaBurst)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }

    public class FlameShock : Spell
    {
        public FlameShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 500;
            baseMaxDamage = 500;
            spCoef = .2142f;
            dotSpCoef = .1f;
            periodicTick = 139f; // 556f / 4f;
            periodicTicks = 4f;
            manaCost = 0.17f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            dotBaseCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotSpCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellFireDamageRating;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            if (calcOpts.glyphOfFlameShock) periodicTicks += 2;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            base.Initialize(stats, shamanTalents);
        }

        public static FlameShock operator +(FlameShock A, FlameShock B)
        {
            FlameShock C = (FlameShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static FlameShock operator *(FlameShock A, float b)
        {
            FlameShock C = (FlameShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }

    public class EarthShock : Spell
    {
        public EarthShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 849;
            baseMaxDamage = 895;
            spCoef = .3858f;
            manaCost = 0.18f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            base.Initialize(stats, shamanTalents);
        }

        public static EarthShock operator +(EarthShock A, EarthShock B)
        {
            EarthShock C = (EarthShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static EarthShock operator *(EarthShock A, float b)
        {
            EarthShock C = (EarthShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }

    public class FrostShock : Spell
    {
        public FrostShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 802;
            baseMaxDamage = 848;
            spCoef = .3858f;
            manaCost = 0.18f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellFrostDamageRating;
            totalCoef *= 1 + stats.BonusFrostDamageMultiplier;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            base.Initialize(stats, shamanTalents);
        }

        public static FrostShock operator +(FrostShock A, FrostShock B)
        {
            FrostShock C = (FrostShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static FrostShock operator *(FrostShock A, float b)
        {
            FrostShock C = (FrostShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }

    public class ElementalMastery : Spell
    {
        public ElementalMastery(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            missChance = 0;
            cooldown = 180f - (calcOpts.glyphOfElementalMastery?30f:0f);
            gcd = 0; // no global cooldown ;)
            #endregion

            base.Initialize(stats, shamanTalents);
        }


        public static ElementalMastery operator +(ElementalMastery A, ElementalMastery B)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ElementalMastery operator *(ElementalMastery A, float b)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }

}