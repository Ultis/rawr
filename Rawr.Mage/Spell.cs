using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    enum MagicSchool
    {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane
    }

    abstract class Spell
    {
        public string Name;
        public float DamagePerSecond;
        public float CostPerSecond;
        public float ManaRegenPerSecond;

        public bool AffectedByFlameCap;
        public bool ABCycle;

        public bool AreaEffect;

        public string Sequence;
        public bool Channeled;
        public float HitProcs;
        public float CastProcs;
        public float CastTime;
    }

    abstract class BaseSpell : Spell
    {
        public bool Instant;
        public bool Binary;
        public int BaseCost;
        public int BaseRange;
        public float BaseCastTime;
        public float BaseCooldown;
        public MagicSchool MagicSchool;
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float BasePeriodicDamage;
        public float SpellDamageCoefficient;
        public float DotDamageCoefficient;
        public float DotDuration;
        public bool SpammedDot;
        public float TargetProcs;
        public float AoeDamageCap;

        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float spellDamageCoefficient) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, spellDamageCoefficient, 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, 1, 1, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs) : this(name, channeled, binary, instant, areaEffect, cost, range, castTime, cooldown, magicSchool, minDamage, maxDamage, periodicDamage, hitProcs, castProcs, instant ? (1.5f / 3.5f) : (castTime / 3.5f), 0, 0, false) { }
        public BaseSpell(string name, bool channeled, bool binary, bool instant, bool areaEffect, int cost, int range, float castTime, float cooldown, MagicSchool magicSchool, float minDamage, float maxDamage, float periodicDamage, float hitProcs, float castProcs, float spellDamageCoefficient, float dotDamageCoefficient, float dotDuration, bool spammedDot)
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
            DotDamageCoefficient = dotDamageCoefficient;
            DotDuration = dotDuration;
            SpammedDot = spammedDot;
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
        public bool ManualClearcasting = false;
        public bool ClearcastingAveraged;
        public bool ClearcastingActive;
        public bool ClearcastingProccing;
        public float InterruptProtection;

        public float Cooldown;
        public float Cost;

        public virtual void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            if (AreaEffect) TargetProcs *= calculations.CalculationOptions.AoeTargets;
            Cooldown = BaseCooldown;

            CostModifier = 1;
            if (MagicSchool == MagicSchool.Fire) CostModifier -= 0.01f * calculations.CalculationOptions.Pyromaniac;
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) CostModifier -= 0.01f * calculations.CalculationOptions.ElementalPrecision;
            if (MagicSchool == MagicSchool.Frost) CostModifier -= 0.05f * calculations.CalculationOptions.FrostChanneling;
            if (calculations.ArcanePower) CostModifier += 0.3f;
            if (MagicSchool == MagicSchool.Fire) AffectedByFlameCap = true;
            if (MagicSchool == MagicSchool.Fire) InterruptProtection += 0.35f * calculations.CalculationOptions.BurningSoul;

            CastTime = BaseCastTime / calculations.CastingSpeed + calculations.Latency;

            switch (MagicSchool)
            {
                case MagicSchool.Arcane:
                    SpellModifier = calculations.ArcaneSpellModifier;
                    CritRate = calculations.ArcaneCritRate;
                    CritBonus = calculations.ArcaneCritBonus;
                    RawSpellDamage = calculations.ArcaneDamage;
                    HitRate = calculations.ArcaneHitRate;
                    RealResistance = calculations.CalculationOptions.ArcaneResist;
                    break;
                case MagicSchool.Fire:
                    SpellModifier = calculations.FireSpellModifier;
                    CritRate = calculations.FireCritRate;
                    CritBonus = calculations.FireCritBonus;
                    RawSpellDamage = calculations.FireDamage;
                    HitRate = calculations.FireHitRate;
                    RealResistance = calculations.CalculationOptions.FireResist;
                    break;
                case MagicSchool.Frost:
                    SpellModifier = calculations.FrostSpellModifier;
                    CritRate = calculations.FrostCritRate;
                    CritBonus = calculations.FrostCritBonus;
                    RawSpellDamage = calculations.FrostDamage;
                    HitRate = calculations.FrostHitRate;
                    RealResistance = calculations.CalculationOptions.FrostResist;
                    break;
                case MagicSchool.Nature:
                    SpellModifier = calculations.NatureSpellModifier;
                    CritRate = calculations.NatureCritRate;
                    CritBonus = calculations.NatureCritBonus;
                    RawSpellDamage = calculations.NatureDamage;
                    HitRate = calculations.NatureHitRate;
                    RealResistance = calculations.CalculationOptions.NatureResist;
                    break;
                case MagicSchool.Shadow:
                    SpellModifier = calculations.ShadowSpellModifier;
                    CritRate = calculations.ShadowCritRate;
                    CritBonus = calculations.ShadowCritBonus;
                    RawSpellDamage = calculations.ShadowDamage;
                    HitRate = calculations.ShadowHitRate;
                    RealResistance = calculations.CalculationOptions.ShadowResist;
                    break;
            }

            // do not count debuffs for aoe effects, can't assume it will be up on all
            // do not include molten fury (molten fury relates to boss), instead amplify all by average
            if (AreaEffect)
            {
                SpellModifier = (1 + 0.01f * calculations.CalculationOptions.ArcaneInstability) * (1 + 0.01f * calculations.CalculationOptions.PlayingWithFire);
                if (calculations.ArcanePower)
                {
                    SpellModifier *= 1.3f;
                }
                if (calculations.CalculationOptions.MoltenFury > 0)
                {
                    SpellModifier *= (1 + 0.1f * calculations.CalculationOptions.MoltenFury * calculations.CalculationOptions.MoltenFuryPercentage);
                }
                if (MagicSchool == MagicSchool.Fire) SpellModifier *= (1 + 0.02f * calculations.CalculationOptions.FirePower);
                if (MagicSchool == MagicSchool.Frost) SpellModifier *= (1 + 0.02f * calculations.CalculationOptions.PiercingIce);
            }

            if (!ManualClearcasting && !ClearcastingAveraged)
            {
                CritRate -= 0.01f * calculations.CalculationOptions.ArcanePotency; // replace averaged arcane potency with actual % chance
                if (ClearcastingActive) CritRate += 0.1f * calculations.CalculationOptions.ArcanePotency;
            }

            int targetLevel = calculations.CalculationOptions.TargetLevel;
            PartialResistFactor = (RealResistance == 1) ? 0 : (1 - Math.Max(0f, RealResistance - calculations.BasicStats.SpellPenetration / 350f * 0.75f) - ((targetLevel > 70 && !Binary) ? ((targetLevel - 70) * 0.02f) : 0f));
        }

        private float ProcBuffUp(float procChance, float buffDuration, float triggerInterval)
        {
            if (triggerInterval <= 0)
                return 0;
            else
                return 1 - (float)Math.Pow(1 - procChance, buffDuration / triggerInterval);
        }

        protected void CalculateDerivedStats(Character character, CharacterCalculationsMage calculations)
        {
            if (Instant) InterruptProtection = 1;
            if (calculations.IcyVeins) InterruptProtection = 1;
            float InterruptFactor = calculations.CalculationOptions.InterruptFrequency * Math.Max(0, 1 - InterruptProtection);
            CastTime = CastTime * (1 + InterruptFactor) - (0.5f + calculations.Latency) * InterruptFactor;
            if (CastTime < calculations.GlobalCooldown + calculations.Latency) CastTime = calculations.GlobalCooldown + calculations.Latency;
            Cost = BaseCost * CostModifier;

            CritRate = Math.Min(1, CritRate);
            if (MagicSchool == MagicSchool.Fire || MagicSchool == MagicSchool.Frost) Cost *= (1f - CritRate * 0.1f * calculations.CalculationOptions.MasterOfElements);

            CostPerSecond = Cost / CastTime;

            if (!ManualClearcasting || ClearcastingAveraged)
            {
                CostPerSecond *= (1 - 0.02f * calculations.CalculationOptions.ArcaneConcentration);
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            if (calculations.BasicStats.SpellDamageFor6SecOnCrit > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor6SecOnCrit * ProcBuffUp(1 - (float)Math.Pow(1 - CritRate, HitProcs), 6, CastTime);
            if (calculations.BasicStats.SpellDamageFor10SecOnHit_10_45 > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnHit_10_45 * 10f / (45f + CastTime / HitProcs / 0.1f);
            if (calculations.BasicStats.SpellDamageFor10SecOnResist > 0) RawSpellDamage += calculations.BasicStats.SpellDamageFor10SecOnResist * ProcBuffUp(1 - (float)Math.Pow(HitRate, HitProcs), 10, CastTime);

            SpellDamage = RawSpellDamage * SpellDamageCoefficient;
            float baseAverage = (BaseMinDamage + BaseMaxDamage) / 2f + SpellDamage;
            float critMultiplier = 1 + (CritBonus - 1) * Math.Max(0, CritRate - calculations.ResilienceCritRateReduction);
            float resistMultiplier = HitRate * PartialResistFactor;
            int targets = 1;
            if (AreaEffect) targets = calculations.CalculationOptions.AoeTargets;
            AverageDamage = baseAverage * SpellModifier * targets * HitRate;
            if (AreaEffect && AverageDamage > AoeDamageCap) AverageDamage = AoeDamageCap;
            AverageDamage = AverageDamage * critMultiplier * PartialResistFactor;
            if (SpammedDot)
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier * CastTime / DotDuration;
            }
            else
            {
                AverageDamage += targets * (BasePeriodicDamage + DotDamageCoefficient * RawSpellDamage) * SpellModifier * resistMultiplier;
            }
            DamagePerSecond = AverageDamage / CastTime;

            if (Name != "Lightning Bolt" && calculations.BasicStats.LightningCapacitorProc > 0)
            {
                BaseSpell LightningBolt = (BaseSpell)calculations.GetSpell("Lightning Bolt");
                DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }

            /*float casttimeHash = calculations.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance, Name, casttimeHash);
            }*/

            float OO5SR;

            if (Cost > 0)
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance);
            }
            else
            {
                OO5SR = 1;
            }

            ManaRegenPerSecond = calculations.ManaRegen5SR + OO5SR * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;

            if (calculations.Mp5OnCastFor20Sec > 0 && CastProcs > 0)
            {
                float totalMana = calculations.Mp5OnCastFor20Sec / 5f / CastTime * 0.5f * (20 - CastTime / CastProcs / 2f) * (20 - CastTime / CastProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
            }
        }
    }

    class Wand : BaseSpell
    {
        public Wand(Character character, CharacterCalculationsMage calculations, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0, false)
        {
            Calculate(character, calculations);
            CastTime = speed;
            CritRate = calculations.SpellCrit;
            CritBonus = 1.5f;
            SpellModifier = 1 + calculations.BasicStats.BonusSpellPowerMultiplier;
            switch (school)
            {
                case MagicSchool.Arcane:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Fire:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Frost:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Nature:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
                case MagicSchool.Shadow:
                    SpellModifier *= (1 + calculations.BasicStats.BonusArcaneSpellPowerMultiplier);
                    break;
            }
            if (calculations.CalculationOptions.WandSpecialization > 0)
            {
                SpellModifier *= 1 + 0.01f + 0.12f * calculations.CalculationOptions.WandSpecialization;
            }
            CalculateDerivedStats(character, calculations);
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
            CritRate += 0.02f * calculations.CalculationOptions.Incinerate;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Scorch : BaseSpell
    {

        public Scorch(Character character, CharacterCalculationsMage calculations, bool clearcastingActive)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            Calculate(character, calculations);
        }

        public Scorch(Character character, CharacterCalculationsMage calculations)
            : base("Scorch", false, false, false, false, 180, 30, 1.5f, 0, MagicSchool.Fire, 305, 361, 0)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * calculations.CalculationOptions.Incinerate;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Flamestrike : BaseSpell
    {
        public Flamestrike(Character character, CharacterCalculationsMage calculations, bool spammedDot)
            : base("Flamestrike", false, false, false, true, 1175, 30, 3, 0, MagicSchool.Fire, 480, 585, 424, 1, 1, 0.2363f, 0.12f, 8f, spammedDot)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 7830;
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
            CastTime = (BaseCastTime - 0.1f * calculations.CalculationOptions.ImprovedFrostbolt) / calculations.CastingSpeed + calculations.Latency;
            CritRate += 0.01f * calculations.CalculationOptions.EmpoweredFrostbolt;
            SpellDamageCoefficient += 0.02f * calculations.CalculationOptions.EmpoweredFrostbolt;
            int targetLevel = calculations.CalculationOptions.TargetLevel;
            HitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculations.SpellHit + 0.02f * calculations.CalculationOptions.ElementalPrecision); // bugged Elemental Precision
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(character, calculations);
        }
    }

    class Fireball : BaseSpell
    {
        public Fireball(Character character, CharacterCalculationsMage calculations)
            : base("Fireball", false, false, false, false, 425, 35, 3.5f, 0, MagicSchool.Fire, 649, 821, 84)
        {
            Calculate(character, calculations);
            SpammedDot = true;
            DotDuration = 8;
            CastTime = (BaseCastTime - 0.1f * calculations.CalculationOptions.ImprovedFireball) / calculations.CastingSpeed + calculations.Latency;
            SpellDamageCoefficient += 0.03f * calculations.CalculationOptions.EmpoweredFireball;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            CalculateDerivedStats(character, calculations);
        }
    }

    class ConeOfCold : BaseSpell
    {
        public ConeOfCold(Character character, CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, true, 645, 0, 0, 10, MagicSchool.Frost, 418, 457, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 6500;
            int ImprovedConeOfCold = int.Parse(character.CalculationOptions["ImprovedConeOfCold"]);
            SpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0));
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneBlast : BaseSpell
    {
        public ArcaneBlast(Character character, CharacterCalculationsMage calculations, int timeDebuff, int costDebuff, bool clearcastingActive)
            : base("Arcane Blast", false, false, false, false, 195, 30, 2.5f, 0, MagicSchool.Arcane, 668, 772, 0)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = false;
            this.timeDebuff = timeDebuff;
            this.costDebuff = costDebuff;
            Calculate(character, calculations);
        }

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
            CritRate += 0.02f * calculations.CalculationOptions.ArcaneImpact;
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissiles : BaseSpell
    {
        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing)
            : base("Arcane Missiles", true, false, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0, 5, 1)
        {
            ManualClearcasting = true;
            ClearcastingActive = clearcastingActive;
            ClearcastingAveraged = clearcastingAveraged;
            ClearcastingProccing = clearcastingProccing;
            Calculate(character, calculations);
        }

        public ArcaneMissiles(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, false, 740, 30, 5, 0, MagicSchool.Arcane, 264 * 5, 265 * 5, 0, 5, 1)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CostModifier += 0.02f * calculations.CalculationOptions.EmpoweredArcaneMissiles;

            // CC double dipping
            if (!ManualClearcasting) CritRate += 0.01f * calculations.CalculationOptions.ArcanePotency;
            else if (ClearcastingProccing) CritRate += 0.1f * calculations.CalculationOptions.ArcanePotency;

            SpellDamageCoefficient += 0.15f * calculations.CalculationOptions.EmpoweredArcaneMissiles;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
            InterruptProtection += 0.2f * calculations.CalculationOptions.ImprovedArcaneMissiles;
            CalculateDerivedStats(character, calculations);
        }
    }

    class ArcaneMissilesCC : Spell
    {
        public ArcaneMissilesCC(Character character, CharacterCalculationsMage calculations)
        {
            Name= "Arcane Missiles CC";

            //AM?1-AM11-AM11-...=0.9*0.1*...
            //...
            //AM?1-AM10=0.9

            //TIME = T * [1 + 1/0.9]
            //DAMAGE = AM?1 + AM10 + 0.1/0.9*AM11

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            BaseSpell AMc1 = new ArcaneMissiles(character, calculations, true, false, true);
            BaseSpell AM10 = new ArcaneMissiles(character, calculations, false, true, false);
            BaseSpell AM11 = new ArcaneMissiles(character, calculations, false, true, true);

            CastProcs = AMc1.CastProcs * (1 + 1 / (1 - CC));
            CastTime = AMc1.CastTime * (1 + 1 / (1 - CC));
            HitProcs = AMc1.HitProcs * (1 + 1 / (1 - CC));
            Channeled = true;
            CostPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            DamagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            ManaRegenPerSecond = 0;
        }
    }

    class ArcaneExplosion : BaseSpell
    {
        public ArcaneExplosion(Character character, CharacterCalculationsMage calculations)
            : base("Arcane Explosion", false, false, true, true, 545, 0, 0, 0, MagicSchool.Arcane, 377, 407, 0, 1.5f / 3.5f * 0.5f)
        {
            base.Calculate(character, calculations);
            CritRate += 0.02f * int.Parse(character.CalculationOptions["ArcaneImpact"]);
            AoeDamageCap = 10180;
            CalculateDerivedStats(character, calculations);
        }
    }

    class BlastWave : BaseSpell
    {
        public BlastWave(Character character, CharacterCalculationsMage calculations)
            : base("Blast Wave", false, false, true, true, 645, 0, 0, 30, MagicSchool.Fire, 616, 724, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 9440;
            CalculateDerivedStats(character, calculations);
        }
    }

    class DragonsBreath : BaseSpell
    {
        public DragonsBreath(Character character, CharacterCalculationsMage calculations)
            : base("Dragon's Breath", false, false, true, true, 700, 0, 0, 20, MagicSchool.Fire, 680, 790, 0, 0.1357f)
        {
            base.Calculate(character, calculations);
            AoeDamageCap = 10100;
            CalculateDerivedStats(character, calculations);
        }
    }

    class Blizzard : BaseSpell
    {
        public Blizzard(Character character, CharacterCalculationsMage calculations)
            : base("Blizzard", true, false, false, true, 1645, 0, 8, 0, MagicSchool.Frost, 1476, 1476, 0, 1.1429f)
        {
            base.Calculate(character, calculations);
            CritBonus = 1;
            CritRate = 0;
            HitRate = 1;
            AoeDamageCap = 28950;
            CalculateDerivedStats(character, calculations);
        }
    }

    class LightningBolt : BaseSpell
    {
        public LightningBolt(Character character, CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, false, true, false, 0, 30, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0, 0, 0, false)
        {
            Calculate(character, calculations);
        }

        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CritBonus = 1.5f;
            CalculateDerivedStats(character, calculations);
        }
    }

    class SpellCycle : Spell
    {
        public float AverageDamage;
        public float Cost;

        private List<string> spellList = new List<string>();

        private FSRCalc fsr = new FSRCalc();

        public void AddSpell(Spell spell, CharacterCalculationsMage calculations)
        {
            fsr.AddSpell(spell.CastTime - calculations.Latency, calculations.Latency, spell.Channeled);
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell.Name);
        }

        public void AddPause(float duration)
        {
            fsr.AddPause(duration);
            spellList.Add("Pause");
        }

        public void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            Sequence = string.Join("-", spellList.ToArray());

            CastTime = fsr.Duration;

            CostPerSecond = Cost / CastTime;
            DamagePerSecond = AverageDamage / CastTime;

            float OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance);

            ManaRegenPerSecond = calculations.ManaRegen5SR + OO5SR * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;

            if (calculations.Mp5OnCastFor20Sec > 0)
            {
                float averageCastTime = fsr.Duration / CastProcs;
                float totalMana = calculations.Mp5OnCastFor20Sec / 5f / averageCastTime * 0.5f * (20 - averageCastTime / HitProcs / 2f) * (20 - averageCastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
            }
        }
    }

    class ABAM : SpellCycle
    {
        public ABAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM";
            ABCycle = true;

            BaseSpell AB = (BaseSpell)calculations.GetSpell("Arcane Blast 1,0");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");

            AddSpell(AB, calculations);
            AddSpell(AM, calculations);
            AddPause(8 - AM.CastTime - AB.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class AB3AMSc : SpellCycle
    {
        public AB3AMSc(Character character, CharacterCalculationsMage calculations)
        {
            Name = "AB3AMSc";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB01 = (BaseSpell)calculations.GetSpell("Arcane Blast 0,1");
            BaseSpell AB12 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,2");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            AddSpell(AB30, calculations);
            AddSpell(AB01, calculations);
            AddSpell(AB12, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3Sc : SpellCycle
    {
        public ABAM3Sc(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3Sc";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3Sc2 : SpellCycle
    {
        public ABAM3Sc2(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3Sc2";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap >= AB30.CastTime + calculations.Latency)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3FrB : SpellCycle
    {
        public ABAM3FrB(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3FrB";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");
            BaseSpell FrB = (BaseSpell)calculations.GetSpell("Frostbolt");

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABAM3FrB2 : SpellCycle
    {
        public ABAM3FrB2(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3FrB2";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell AM = (BaseSpell)calculations.GetSpell("Arcane Missiles");
            BaseSpell FrB = (BaseSpell)calculations.GetSpell("Frostbolt");

            AddSpell(AB30, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB11, calculations);
            AddSpell(AM, calculations);
            AddSpell(AB22, calculations);
            AddSpell(AM, calculations);
            float gap = 8 - AM.CastTime;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class AB3FrB : SpellCycle
    {
        public AB3FrB(Character character, CharacterCalculationsMage calculations)
        {
            Name = "AB3FrB";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB01 = (BaseSpell)calculations.GetSpell("Arcane Blast 0,1");
            BaseSpell AB12 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,2");
            BaseSpell FrB = (BaseSpell)calculations.GetSpell("Frostbolt");

            AddSpell(AB30, calculations);
            AddSpell(AB01, calculations);
            AddSpell(AB12, calculations);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFrB3FrB : SpellCycle
    {
        public ABFrB3FrB(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABFrB3FrB";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell FrB = (BaseSpell)calculations.GetSpell("Frostbolt");

            AddSpell(AB30, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFrB3FrB2 : SpellCycle
    {
        public ABFrB3FrB2(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABFrB3FrB2";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell FrB = (BaseSpell)calculations.GetSpell("Frostbolt");

            AddSpell(AB30, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FrB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, calculations);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class ABFB3FBSc : SpellCycle
    {
        public ABFB3FBSc(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABFB3FBSc";
            ABCycle = true;

            BaseSpell AB30 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell AB11 = (BaseSpell)calculations.GetSpell("Arcane Blast 1,1");
            BaseSpell AB22 = (BaseSpell)calculations.GetSpell("Arcane Blast 2,2");
            BaseSpell FB = (BaseSpell)calculations.GetSpell("Fireball");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            AddSpell(AB30, calculations);
            AddSpell(FB, calculations);
            AddSpell(AB11, calculations);
            AddSpell(FB, calculations);
            AddSpell(AB22, calculations);
            float gap = 8;
            while (gap >= AB30.CastTime + calculations.Latency)
            {
                AddSpell(FB, calculations);
                gap -= FB.CastTime;
            }
            while (gap >= AB30.CastTime + calculations.Latency)
            {
                AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + calculations.Latency);

            Calculate(character, calculations);
        }
    }

    class FireballScorch : SpellCycle
    {
        public FireballScorch(Character character, CharacterCalculationsMage calculations)
        {
            Name = "FireballScorch";

            BaseSpell FB = (BaseSpell)calculations.GetSpell("Fireball");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            if (calculations.CalculationOptions.ImprovedScorch == 0)
            {
                // in this case just Fireball, scorch debuff won't be applied
                AddSpell(FB, calculations);
                Calculate(character, calculations);
            }
            else
            {
                int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)calculations.CalculationOptions.ImprovedScorch);
                double timeOnScorch = 30;
                int fbCount = 0;

                while (timeOnScorch > FB.CastTime + (averageScorchesNeeded + 1) * Sc.CastTime) // one extra scorch gap to account for possible resist
                {
                    AddSpell(FB, calculations);
                    fbCount++;
                    timeOnScorch -= FB.CastTime;
                }
                for (int i = 0; i < averageScorchesNeeded; i++)
                {
                    AddSpell(Sc, calculations);
                }

                Calculate(character, calculations);

                Sequence = string.Format("{0}x Fireball : {1}x Scorch", fbCount, averageScorchesNeeded);
            }
        }
    }

    class ABAM3ScCCAM : Spell
    {
        public ABAM3ScCCAM(Character character, CharacterCalculationsMage calculations)
        {
            Name = "ABAM3ScCCAM";
            ABCycle = true;

            //AMCC-AB0                       0.1
            //AM?0-AB1-AMCC-AB0              0.9*0.1
            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

            //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
            //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
            //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
            //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

            Spell AMc0 = new ArcaneMissiles(character, calculations, true, false, false);
            Spell AMCC = new ArcaneMissilesCC(character, calculations);
            Spell AB0 = new ArcaneBlast(character, calculations, 0, 0, false);
            Spell AB1 = new ArcaneBlast(character, calculations, 1, 1, false);
            Spell AB2 = new ArcaneBlast(character, calculations, 2, 2, false);
            Spell Sc0 = new Scorch(character, calculations, false);

            BaseSpell AB3 = (BaseSpell)calculations.GetSpell("Arcane Blast 3,0");
            BaseSpell Sc = (BaseSpell)calculations.GetSpell("Scorch");

            float CC = 0.02f * calculations.CalculationOptions.ArcaneConcentration;

            //AMCC-AB0                       0.1
            SpellCycle chain1 = new SpellCycle();
            chain1.AddSpell(AMCC, calculations);
            chain1.AddSpell(AB0, calculations);
            chain1.Calculate(character, calculations);

            //AM?0-AB1-AMCC-AB0              0.9*0.1
            SpellCycle chain2 = new SpellCycle();
            chain2.AddSpell(AMc0, calculations);
            chain2.AddSpell(AB1, calculations);
            chain2.AddSpell(AMCC, calculations);
            chain2.AddSpell(AB0, calculations);
            chain2.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
            SpellCycle chain3 = new SpellCycle();
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB1, calculations);
            chain3.AddSpell(AMc0, calculations);
            chain3.AddSpell(AB2, calculations);
            chain3.AddSpell(AMCC, calculations);
            chain3.AddSpell(AB0, calculations);
            chain3.Calculate(character, calculations);

            //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
            SpellCycle chain4 = new SpellCycle();
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB1, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(AB2, calculations);
            chain4.AddSpell(AMc0, calculations);
            chain4.AddSpell(Sc0, calculations);
            float gap = 8 - AMc0.CastTime - Sc0.CastTime;
            while (gap - AB3.CastTime >= Sc.CastTime)
            {
                chain4.AddSpell(Sc, calculations);
                gap -= Sc.CastTime;
            }
            if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + calculations.Latency);
            chain4.AddSpell(AB3, calculations);
            chain4.Calculate(character, calculations);

            CastTime = CC * chain1.CastTime + CC * (1 - CC) * chain2.CastTime + CC * (1 - CC) * (1 - CC) * chain3.CastTime + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime;
            CostPerSecond = (CC * chain1.CastTime * chain1.CostPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.CostPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.CostPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.CostPerSecond) / CastTime;
            DamagePerSecond = (CC * chain1.CastTime * chain1.DamagePerSecond + CC * (1 - CC) * chain2.CastTime * chain2.DamagePerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.DamagePerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.DamagePerSecond) / CastTime;
            ManaRegenPerSecond = (CC * chain1.CastTime * chain1.ManaRegenPerSecond + CC * (1 - CC) * chain2.CastTime * chain2.ManaRegenPerSecond + CC * (1 - CC) * (1 - CC) * chain3.CastTime * chain3.ManaRegenPerSecond + (1 - CC) * (1 - CC) * (1 - CC) * chain4.CastTime * chain4.ManaRegenPerSecond) / CastTime;

            Sequence = chain4.Sequence;
        }
    }
}
