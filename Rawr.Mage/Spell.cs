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

        public bool AffectedByFlameCap;
        public bool ABCycle;
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
            if (CastTime < calculations.GlobalCooldown + calculations.Latency) CastTime = calculations.GlobalCooldown + calculations.Latency;
            Cost = BaseCost * CostModifier;

            Cost *= (1f - CritRate + CritRate * (1f - 0.1f * calculations.CalculationOptions.MasterOfElements));

            CostPerSecond = Cost / CastTime;

            if (AveragedClearcasting)
            {
                CostPerSecond *= (1 - 0.02f * calculations.CalculationOptions.ArcaneConcentration);
            }
            else if (ClearcastingActive)
            {
                CostPerSecond = 0;
            }

            RawSpellDamage += calculations.BasicStats.SpellDamageOnCritProc * ProcBuffUp(1 - (float)Math.Pow(1 - CritRate, HitProcs), 6, CastTime);

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

            /*float casttimeHash = calculations.ClearcastingChance * 100 + CastTime;
            float OO5SR = 0;
            if (!FSRCalc.TryGetCachedOO5SR(Name, casttimeHash, out OO5SR))
            {
                FSRCalc fsr = new FSRCalc();
                fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);
                OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance, Name, casttimeHash);
            }*/

            FSRCalc fsr = new FSRCalc();
            fsr.AddSpell(CastTime - calculations.Latency, calculations.Latency, Channeled);
            float OO5SR = fsr.CalculateOO5SR(calculations.ClearcastingChance);

            ManaRegenPerSecond = calculations.ManaRegen5SR + OO5SR * (calculations.ManaRegen - calculations.ManaRegen5SR) + calculations.BasicStats.ManaRestorePerHit * HitProcs / CastTime + calculations.BasicStats.ManaRestorePerCast * CastProcs / CastTime;

            if (calculations.Mp5OnCastFor20Sec > 0)
            {
                float totalMana = calculations.Mp5OnCastFor20Sec / 5f / CastTime * 0.5f * (20 - CastTime / HitProcs / 2f) * (20 - CastTime / HitProcs / 2f);
                ManaRegenPerSecond += totalMana / 20f;
            }
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
        }


        public override void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            base.Calculate(character, calculations);
            CastTime = (BaseCastTime - 0.1f * calculations.CalculationOptions.ImprovedFireball) / calculations.CastingSpeed + calculations.Latency;
            SpellDamageCoefficient += 0.03f * calculations.CalculationOptions.EmpoweredFireball;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
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
            CritRate += 0.02f * calculations.CalculationOptions.ArcaneImpact;
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
            CostModifier += 0.02f * calculations.CalculationOptions.EmpoweredArcaneMissiles;
            CritRate += 0.01f * calculations.CalculationOptions.ArcanePotency; // CC double dipping
            SpellDamageCoefficient += 0.15f * calculations.CalculationOptions.EmpoweredArcaneMissiles;
            SpellModifier *= (1 + calculations.BasicStats.BonusMageNukeMultiplier);
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
            CritBonus = 1.5f;
            CalculateDerivedStats(character, calculations);
        }
    }

    abstract class SpellCycle : Spell
    {
        public float HitProcs;
        public float CastProcs;
        public float AverageDamage;
        public float Cost;

        public string Sequence;
        private List<string> spellList = new List<string>();

        private FSRCalc fsr = new FSRCalc();

        protected void AddSpell(BaseSpell spell, CharacterCalculationsMage calculations)
        {
            fsr.AddSpell(spell.CastTime - calculations.Latency, calculations.Latency, spell.Channeled);
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            AverageDamage += spell.DamagePerSecond * spell.CastTime;
            Cost += spell.CostPerSecond * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell.Name);
        }

        protected void AddPause(float duration)
        {
            fsr.AddPause(duration);
            spellList.Add("Pause");
        }

        public void Calculate(Character character, CharacterCalculationsMage calculations)
        {
            Sequence = string.Join("-", spellList.ToArray());

            float CastTime = fsr.Duration;

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
            AddPause(8 - AM.CastTime + calculations.Latency);
            AddSpell(AM, calculations);

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

}
