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
        protected float dotCanCrit = 0f;
        protected float spellPower = 0f;

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
        { get { return MinHit * (1+critModifier); } }

        public virtual float MaxHit
        { get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef); } }

        public float MaxCrit
        { get { return MaxHit * (1+critModifier); } }

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
            if (PeriodicTickTime == 0)
                return 0;
            int effectiveTicks = (int)Math.Floor(Math.Min(duration, Duration) / PeriodicTickTime);
            return PeriodicTick * effectiveTicks;
        }

        public virtual float TotalDamage
        { get { return AvgDamage + PeriodicDamage(); } }

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

    public class Shock : Spell
    {
    }

    public class Thunderstorm : Spell
    {
        public Thunderstorm()
        {
            #region Base Values
            baseMinDamage = 1450;
            baseMaxDamage = 1656;
            castTime = 0f;
            spCoef = 1.5f / 7f * .9f; // NOT CORRECT YET, assuming 1.5f/7f * 0.9f (aoe with additional effect)
            manaCost = 0f;
            cooldown = 45f;
            shortName = "TS";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            crit += .01f * shamanTalents.TidalMastery;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;

            if (shamanTalents.GlyphofThunder)
                cooldown -= 10f;

            base.Initialize(stats, shamanTalents);
        }

        public Thunderstorm(Stats stats, ShamanTalents shamanTalents) : this()
        {
            Initialize(stats, shamanTalents);
        }

        public static float getProcsPerSecond(bool glyphed, int fightDuration)
        {
            if (glyphed)
            {
                if (manaRestoreGlyphed == null)
                    manaRestoreGlyphed = new SpecialEffect(Trigger.Use, new Stats { }, 0f, 35f, 1f);
                return manaRestoreGlyphed.GetAverageProcsPerSecond(35f, 1f, 1f, fightDuration);
            }
            else
            {
                if (manaRestoreUnglyphed == null)
                    manaRestoreUnglyphed = new SpecialEffect(Trigger.Use, new Stats { }, 0f, 45f, 1f);
                return manaRestoreUnglyphed.GetAverageProcsPerSecond(45f, 1f, 1f, fightDuration);
            }
        }

        private static SpecialEffect manaRestoreGlyphed = null;
        private static SpecialEffect manaRestoreUnglyphed = null;

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
        public LightningBolt()
        {
            #region Base Values
            baseMinDamage = 715;
            baseMaxDamage = 815;
            castTime = 2.5f;
            spCoef = 2.5f / 3.5f;
            lspCoef = spCoef;
            loCoef = spCoef / 2f;
            manaCost = 0.1f * Constants.BaseMana;
            shortName = "LB";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            castTime -= .1f * shamanTalents.LightningMastery;
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef += .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            spCoef += .02f * shamanTalents.Shamanism;
            loCoef += .02f * shamanTalents.Shamanism;
            crit += .05f * shamanTalents.TidalMastery;
            manaCost *= 1 - stats.LightningBoltCostReduction / 100f; // T7 2 piece
            spellPower += stats.SpellNatureDamageRating; // Nature SP
            lightningSpellpower += stats.LightningSpellPower; // Totem (relic) is not affected by shamanism
            if (shamanTalents.GlyphofLightningBolt) totalCoef += .04f;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f; // T6 4 piece
            
            lightningOverload = shamanTalents.LightningOverload;

            base.Initialize(stats, shamanTalents);
            critModifier *= 1 + stats.LightningBoltCritDamageModifier; // T8 4 piece
        }

        public LightningBolt(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
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

        private int lightningOverload = 0;
        private float loCoef, lightningSpellpower = 0f, lspCoef;

        public override float CCCritChance
        {
            get { return Math.Min(1f, CritChance * (1f + LOChance())); }
        }

        public override float MinHit
        {
            get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public override float MaxHit
        {
            get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        private float LOChance()
        {
            return .11f * lightningOverload;
        }

        public override float TotalDamage
        {
            get { return base.TotalDamage + LOChance() * LightningOverloadDamage; }
        }

        public float LightningOverloadDamage
        {
            get { return totalCoef * ((baseMinDamage + baseMaxDamage) / 4f * baseCoef + spellPower * loCoef + lightningSpellpower * lspCoef) * (1 + CritChance * critModifier); }
        }
    }

    public class ChainLightning : Spell
    {
        private int additionalTargets;
        private float loCoef, lightningSpellpower = 0f, lspCoef;
        public ChainLightning()
        {
            #region Base Values
            baseMinDamage = 973;
            baseMaxDamage = 1111;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            lspCoef = spCoef;
            loCoef = spCoef / 2f;
            manaCost = 0.26f * Constants.BaseMana;
            cooldown = 6f;
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents, int additionalTargets)
        {
            // jumps
            if (additionalTargets < 0)
                additionalTargets = 0;
            if (additionalTargets > 3)
                additionalTargets = 3;
            shortName = "CL" + 1 + additionalTargets;
            if (!shamanTalents.GlyphofChainLightning && additionalTargets > 3)
                additionalTargets = 3;
            this.additionalTargets = additionalTargets;
            totalCoef *= new float[] { 1f, 1.7f, 2.19f, 2.533f, 2.7731f }[additionalTargets];

            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef += .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            castTime -= .1f * shamanTalents.LightningMastery;
            cooldown -= new float[] { 0, .75f, 1.5f, 2.5f }[shamanTalents.StormEarthAndFire];
            crit += .01f * shamanTalents.TidalMastery;
            spellPower += stats.SpellNatureDamageRating;
            lightningSpellpower += stats.LightningSpellPower;  
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;

            lightningOverload = shamanTalents.LightningOverload;

            base.Initialize(stats, shamanTalents);
        }

        public ChainLightning(Stats stats, ShamanTalents shamanTalents, int additionalTargets)
            : this()
        {
            Initialize(stats, shamanTalents, additionalTargets);
        }

        public int AdditionalTargets
        {
            get { return additionalTargets; }
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

        private int lightningOverload = 0;

        public override float CCCritChance
        {
            get { return Math.Min(1f, CritChance * (1f + LOChance())); }
        }

        public override float MinHit
        {
            get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public override float MaxHit
        {
            get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        private float LOChance()
        {
            return .11f * lightningOverload / 3f * (1 + AdditionalTargets);
        }

        public override float TotalDamage
        {
            get { return base.TotalDamage + LOChance() * LightningOverloadDamage; }
        }

        public float LightningOverloadDamage
        {
            get { return totalCoef * ((baseMinDamage + baseMaxDamage) / 4f * baseCoef + spellPower * loCoef + lightningSpellpower * lspCoef) * (1 + CritChance * critModifier); }
        }
    }

    public class LavaBurst : Spell
    {
        public LavaBurst()
        {
            #region Base Values
            baseMinDamage = 1192;
            baseMaxDamage = 1518;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = .1f * Constants.BaseMana;
            cooldown = 8f;
            shortName = "LvB";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents, float fs)
        {
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef += .01f * shamanTalents.Concussion;
            totalCoef += .02f * shamanTalents.CallOfFlame;
            totalCoef += stats.BonusLavaBurstDamageMultiplier; // t9 4 piece
            castTime -= .1f * shamanTalents.LightningMastery;
            spCoef += .04f * shamanTalents.Shamanism;
            critModifier += new float[] { 0f, 0.06f, 0.12f, 0.24f }[shamanTalents.LavaFlows];
            critModifier += stats.BonusLavaBurstCritDamage / 100f; // t7 4 piece
            baseMinDamage += stats.LavaBurstBonus; // Totem (relic)
            baseMaxDamage += stats.LavaBurstBonus; // Totem (relic) 
            spellPower += stats.SpellFireDamageRating;
            if (shamanTalents.GlyphofLava)
                spCoef += .1f;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            base.Initialize(stats, shamanTalents);

            crit = (1 - fs) * crit + fs;
        }

        public LavaBurst(Stats stats, ShamanTalents shamanTalents, float fs)
            : this()
        {
            Initialize(stats, shamanTalents, fs);
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

    public class FlameShock : Shock
    {
        public FlameShock()
        {
            #region Base Values
            baseMinDamage = 500;
            baseMaxDamage = 500;
            spCoef = 1.5f / 3.5f / 2f;
            dotSpCoef = .1f;
            periodicTick = 556f / 4f;
            periodicTicks = 4f;
            manaCost = .17f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FS";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            //for reference
            //dotTick = (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + dotCanCrit * critModifier * CritChance)

            totalCoef += .01f * shamanTalents.Concussion;
            totalCoef += .1f * shamanTalents.BoomingEchoes;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            dotBaseCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotSpCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotBaseCoef *= 1 + .01f * shamanTalents.Concussion;
            dotSpCoef *= 1 + .01f * shamanTalents.Concussion;
            dotBaseCoef *= 1 + stats.BonusFireDamageMultiplier;
            dotSpCoef *= 1 + stats.BonusFireDamageMultiplier;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            cooldown -= 1f * shamanTalents.BoomingEchoes;
            spellPower += stats.SpellFireDamageRating;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;
            periodicTicks += stats.BonusFlameShockDuration / 3f; // t9 2 piece

            if (shamanTalents.GlyphofFlameShock)
            {
                periodicTicks += 2;
            }
            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            dotCanCrit = stats.FlameShockDoTCanCrit; // T8 2 piece

            base.Initialize(stats, shamanTalents);
        }

        public FlameShock(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
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

    public class EarthShock : Shock
    {
        public EarthShock()
        {
            #region Base Values
            baseMinDamage = 849;
            baseMaxDamage = 895;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "ES";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(stats, shamanTalents);
        }

        public EarthShock(Stats stats, ShamanTalents shamanTalents)
        {
            Initialize(stats, shamanTalents);
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

    public class FrostShock : Shock
    {
        public FrostShock()
        {
            #region Base Values
            baseMinDamage = 802;
            baseMaxDamage = 848;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FrS";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            spCoef *= 1f + .1f * shamanTalents.BoomingEchoes;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            cooldown -= 1f * shamanTalents.BoomingEchoes;
            spellPower += stats.SpellFrostDamageRating;
            totalCoef *= 1 + stats.BonusFrostDamageMultiplier;
            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(stats, shamanTalents);
        }

        public FrostShock(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
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
        public ElementalMastery()
        {
            #region Base Values
            missChance = 0;
            cooldown = 180f;
            gcd = 0; // no global cooldown ;)
            shortName = "EM";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            if (shamanTalents.GlyphofElementalMastery)
                cooldown -= 30f;

            base.Initialize(stats, shamanTalents);
        }

        public ElementalMastery(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
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

    public class Wait : Spell
    {
        public Wait()
        {
            gcd = 0f;
            missChance = 0f;
            totalCoef = 0f;
            spCoef = 0f;
        }

        public new void Initialize(float duration)
        {
            castTime = duration;
        }

        public Wait(float duration)
            : this()
        {
            Initialize(duration);
        }

        public override string ToString()
        {
            return "W(" + Math.Round(castTime,2) + ")";
        }
    }

}