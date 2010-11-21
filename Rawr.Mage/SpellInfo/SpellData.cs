using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Rawr.Mage
{
    public enum SpellId
    {
        None,
        [Description("Arcane Barrage (0)")]
        ArcaneBarrage,
        [Description("Arcane Barrage (1)")]
        ArcaneBarrage1,
        [Description("Arcane Barrage (2)")]
        ArcaneBarrage2,
        [Description("Arcane Barrage (3)")]
        ArcaneBarrage3,
        [Description("Arcane Barrage (4)")]
        ArcaneBarrage4,
        [Description("Arcane Missiles (0)")]
        ArcaneMissiles,
        [Description("Arcane Missiles (1)")]
        ArcaneMissiles1,
        [Description("Arcane Missiles (2)")]
        ArcaneMissiles2,
        [Description("Arcane Missiles (3)")]
        ArcaneMissiles3,
        [Description("Arcane Missiles (4)")]
        ArcaneMissiles4,
        [Description("MBAM (0)")]
        ArcaneMissilesMB,
        [Description("MBAM (1)")]
        ArcaneMissilesMB1,
        [Description("MBAM (2)")]
        ArcaneMissilesMB2,
        [Description("MBAM (3)")]
        ArcaneMissilesMB3,
        [Description("MBAM (4)")]
        ArcaneMissilesMB4,
        ArcaneMissilesNoProc,
        [Description("Frostbolt")]
        FrostboltFOF,
        Frostbolt,
        [Description("POM+Frostbolt")]
        FrostboltPOM,
        FrostboltNoCC,
        [Description("Deep Freeze")]
        DeepFreeze,
        [Description("Fireball")]
        Fireball,
        [Description("POM+Fireball")]
        FireballPOM,
        FireballBF,
        FrostfireBoltBF,
        FrostfireBoltBFFOF,
        [Description("Frostfire Bolt")]
        FrostfireBoltFOF,
        FrostfireBolt,
        FrostfireBoltFC,
        [Description("Pyroblast")]
        Pyroblast,
        [Description("POM+Pyroblast")]
        PyroblastPOM,
        PyroblastPOMSpammed,
        PyroblastPOMDotUptime,
        [Description("Fire Blast")]
        FireBlast,
        [Description("Flame Orb")]
        FlameOrb,
        [Description("Scorch")]
        Scorch,
        ScorchNoCC,
        [Description("Living Bomb")]
        LivingBomb,
        ArcaneBlast3NoCC,
        ArcaneBlastRaw,
        [Description("Arcane Blast (0)")]
        ArcaneBlast0,
        ArcaneBlast0NoCC,
        ArcaneBlast0POM,
        [Description("Arcane Blast (1)")]
        ArcaneBlast1,
        ArcaneBlast1NoCC,
        [Description("Arcane Blast (2)")]
        ArcaneBlast2,
        [Description("Arcane Blast (3)")]
        ArcaneBlast3,
        [Description("Arcane Blast (4)")]
        ArcaneBlast4,
        ArcaneBlast2NoCC,
        ArcaneBlast0Hit,
        ArcaneBlast1Hit,
        ArcaneBlast2Hit,
        ArcaneBlast3Hit,
        ArcaneBlast0Miss,
        ArcaneBlast1Miss,
        ArcaneBlast2Miss,
        ArcaneBlast3Miss,
        Slow,
        IceLance,
        [Description("Arcane Explosion")]
        ArcaneExplosion,
        FlamestrikeSpammed,
        [Description("Flamestrike")]
        FlamestrikeSingle,
        [Description("Blizzard")]
        Blizzard,
        [Description("Blast Wave")]
        BlastWave,
        [Description("Dragon's Breath")]
        DragonsBreath,
        [Description("Cone of Cold")]
        ConeOfCold,
        MageWard,
        Waterbolt,
        MirrorImage,
    }

    public class WaterboltTemplate : SpellTemplate
    {
        Stats waterElementalBuffs;
        private static readonly string[] validBuffs = new string[] { "Ferocious Inspiration", "Sanctified Retribution", "Improved Moonkin Form", "Swift Retribution", "Elemental Oath", "Moonkin Form", "Wrath of Air Totem", "Demonic Pact", "Flametongue Totem", "Enhancing Totems (Spell Power)", "Totem of Wrath (Spell Power)", "Heart of the Crusader", "Master Poisoner", "Totem of Wrath", "Winter's Chill", "Improved Scorch", "Improved Shadow Bolt", "Curse of the Elements", "Earth and Moon", "Ebon Plaguebringer", "Improved Faerie Fire", "Misery" };
        float baseDamage, baseHaste, dpspBase, multiplier;

        public void Initialize(Solver solver)
        {
            Name = "Waterbolt";
            waterElementalBuffs = new Stats();
            foreach (Buff buff in solver.ActiveBuffs)
            {
                if (Array.IndexOf(validBuffs, buff.Name) >= 0)
                {
                    waterElementalBuffs.Accumulate(buff.Stats);
                }
            }
            baseDamage = 292.0f + (solver.CalculationOptions.PlayerLevel - 50) * 11.5f;
            Character character = solver.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = solver.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + waterElementalBuffs.SpellCrit + waterElementalBuffs.SpellCritOnTarget;
            float hitRate = solver.BaseState.FrostHitRate;
            multiplier = hitRate;
            baseHaste = (1f + waterElementalBuffs.SpellHaste);
            multiplier *= (1 + waterElementalBuffs.BonusDamageMultiplier) * (1 + waterElementalBuffs.BonusFrostDamageMultiplier);
            RealResistance = calculationOptions.FrostResist;
            PartialResistFactor = (RealResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, RealResistance, 0));
            multiplier *= PartialResistFactor * (1 + (1.5f * 1.33f - 1) * spellCrit);
            dpspBase = ((1f / 3f) * 5f / 6f) * multiplier;
            Dirty = false;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);

            float haste = castingState.Heroism ? baseHaste * 1.3f : baseHaste;

            spell.CastTime = 2.5f / haste;
            spell.AverageCost = 0.0f;
            spell.AverageDamage = (baseDamage + (castingState.FrostSpellPower / 3f + waterElementalBuffs.SpellPower + waterElementalBuffs.BonusSpellPowerDemonicPactMultiplier * castingState.CalculationOptions.WarlockSpellPower) * 5f / 6f) * multiplier;
            spell.AverageThreat = 0.0f;
            spell.DamagePerSpellPower = dpspBase;

            return spell;
        }
    }

    public class MirrorImageTemplate : SpellTemplate
    {
        float baseDamageBlast, baseDamageBolt, boltMultiplier, blastMultiplier, castTime, multiplier, dpsp;

        public void Initialize(Solver solver)
        {
            Name = "Mirror Image";
            // these buffs are independent of casting state, so things that depend on them can be calculated only once and then reused
            baseDamageBlast = 97.5f;
            baseDamageBolt = 166.0f;
            Character character = solver.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = solver.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + solver.TargetDebuffs.SpellCritOnTarget;
            // hit rate could actually change between casting states theoretically, but it is negligible and would slow things down unnecessarily
            float blastHitRate = solver.BaseState.FireHitRate;
            float boltHitRate = solver.BaseState.FrostHitRate;
            float haste = (1f + solver.TargetDebuffs.SpellHaste);
            boltMultiplier = boltHitRate * (1 + solver.TargetDebuffs.BonusDamageMultiplier) * (1 + solver.TargetDebuffs.BonusFrostDamageMultiplier) * ((calculationOptions.FrostResist == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, calculationOptions.FrostResist, 0)));
            blastMultiplier = blastHitRate * (1 + solver.TargetDebuffs.BonusDamageMultiplier) * (1 + solver.TargetDebuffs.BonusFireDamageMultiplier) * ((calculationOptions.FireResist == -1) ? 0 : (1 - StatConversion.GetAverageResistance(playerLevel, targetLevel, calculationOptions.FireResist, 0)));
            castTime = (2 * 3.0f + 1.5f) / haste;
            multiplier = (solver.MageTalents.GlyphOfMirrorImage ? 4 : 3) * (1 + (1.5f * 1.33f - 1) * spellCrit);
            dpsp = multiplier * (2 * (1f / 3f * 0.3f) * boltMultiplier + (1f / 3f * 0.15f) * blastMultiplier);
            Dirty = false;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);

            spell.CastTime = castTime;
            spell.AverageCost = 0.0f;
            spell.AverageDamage = multiplier * (2 * (baseDamageBolt + castingState.FrostSpellPower / 3f * 0.3f) * boltMultiplier + (baseDamageBlast + castingState.FireSpellPower / 3f * 0.15f) * blastMultiplier);
            spell.AverageThreat = 0.0f;
            spell.DamagePerSpellPower = dpsp;

            return spell;
        }
    }

    public class WandTemplate : SpellTemplate
    {
        private float speed;

        public WandTemplate()
        {
        }

        public WandTemplate(Solver solver, MagicSchool school, int minDamage, int maxDamage, float speed)
        {
            Initialize(solver, school, minDamage, maxDamage, speed);
        }

        public void Initialize(Solver solver, MagicSchool school, int minDamage, int maxDamage, float speed)
        {
            Name = "Wand";
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            InitializeEffectDamage(solver, school, minDamage, maxDamage);
            Range = 30;
            this.speed = speed;
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = (1 + solver.BaseStats.BonusDamageMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusArcaneDamageMultiplier);
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusFireDamageMultiplier);
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusFrostDamageMultiplier);
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusNatureDamageMultiplier);
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier *= (1 + solver.BaseStats.BonusShadowDamageMultiplier);
                    break;
            }
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CastTime = speed;
            spell.CritRate = castingState.CritRate;

            if (spell.CritRate < 0.0f) spell.CritRate = 0.0f;
            if (spell.CritRate > 1.0f) spell.CritRate = 1.0f;

            spell.SpellModifier = BaseSpellModifier;

            spell.HitProcs = HitRate;
            spell.CritProcs = spell.HitProcs * spell.CritRate;
            spell.TargetProcs = spell.HitProcs;

            float damagePerSpellPower;
            float igniteDamage;
            float igniteDamagePerSpellPower;
            spell.AverageDamage = spell.CalculateAverageDamage(castingState.Solver, 0, false, false, out damagePerSpellPower, out igniteDamage, out igniteDamagePerSpellPower);
            spell.AverageThreat = spell.AverageDamage * ThreatMultiplier;
            spell.IgniteDamage = 0;
            spell.IgniteDamagePerSpellPower = 0;
            spell.AverageCost = 0;
            spell.OO5SR = 1;
            return spell;
        }
    }

    // spell id: 82739, scaling id: 572
    public class FlameOrbTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Flame Orb";
            InitializeCastTime(false, true, 0, 60);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.06f, 15 * 0.277999997138977f, 0.25f, 0, 15 * 0.13400000333786f, 0, 15, 15, 0);
            Dirty = false;
        }
    }

    // spell id: 2948, scaling id: 27
    public class ScorchTemplate : SpellTemplate
    {
        public virtual Spell GetSpell(CastingState castingState, bool clearcastingActive)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.Solver, false, true, false, clearcastingActive);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Scorch";
            InitializeCastTime(false, false, 1.5f, 0);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.08f, 0.781000018119812f, 0.170000001788139f, 0, 0.512000024318695f, 0, 1, 1, 0);
            BaseCostAmplifier *= (1 - 0.5f * solver.MageTalents.ImprovedScorch);
            Dirty = false;
        }
    }

    // spell id: 2120, scaling id: 19
    public class FlamestrikeTemplate : SpellTemplate
    {
        public virtual Spell GetSpell(CastingState castingState, bool spammedDot)
        {
            AoeSpell spell = new AoeSpell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, false, spammedDot);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Flamestrike";
            InitializeCastTime(false, false, 2, 0);
            InitializeScaledDamage(solver, true, 40, MagicSchool.Fire, 0.3f, 0.662000000476837f, 0.202000007033348f, 0.103000000119209f, 0.145999997854233f, 0.0610000006854534f, 1, 1, 8f);
            DotTickInterval = 2;
            Dirty = false;
        }
    }

    public class ConjureManaGemTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Conjure Mana Gem";
            InitializeCastTime(false, false, 3, 0);
            InitializeScaledDamage(solver, false, 0, MagicSchool.Arcane, 0.75f, 0, 0, 0, 0, 0, 0, 1, 0);
            Dirty = false;
        }
    }

    public class MageWardTemplate : SpellTemplate
    {
        private const float spellPowerCoefficient = 0.807f;

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            // 70% absorbed, 30% negated
            // number of negates until absorb is distributed negative binomial
            // mean number of negated is then (1-p)/p = 0.3 / 0.7 times the absorb value
            // however on average it can't be more than (1-p) * incoming damage
            float q = 0f;
            float absorb = castingState.CalculationOptions.GetSpellValue(2.32399988174438f) + spellPowerCoefficient * castingState.ArcaneSpellPower;
            spell.Absorb = absorb;
            // in 3.3.3 warding doesn't count as absorb for IA, assume that we'll get to normal absorb at least once in 30 sec (i.e. we're not lucky enough to continue proccing warding for the whole 30 sec)
            float dps = (float)(castingState.Solver.IncomingDamageDpsFire + castingState.Solver.IncomingDamageDpsArcane + castingState.Solver.IncomingDamageDpsFrost);
            spell.TotalAbsorb = Math.Min(absorb, 30f * dps);
            //spell.TotalAbsorb = Math.Min((1 + q / (1 - q)) * absorb, 30f * dps);
            spell.AverageCost -= Math.Min(q / (1 - q) * absorb, q * 30f * dps);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Mage Ward";
            InitializeCastTime(false, true, 0, 30);
            InitializeScaledDamage(solver, false, 0, MagicSchool.Arcane, 0.16f, 0, 0, 0, 0, 0, 0, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 122, scaling id: 20
    public class FrostNovaTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Frost Nova";
            InitializeCastTime(false, true, 0, 25);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Frost, 0.07f, 0.42399999499321f, 0.150000005960464f, 0, 0.193000003695488f, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 116, scaling id: 21
    public class FrostboltTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool manualClearcasting, bool clearcastingActive, bool pom, bool averageFingersOfFrost)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState, false, pom, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.Solver, false, true, false, clearcastingActive);
            return spell;
        }

        float fingersOfFrostCritRate;

        public Spell GetSpell(CastingState castingState, bool averageFingersOfFrost)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            return GetSpell(castingState, false);
        }

        public void Initialize(Solver solver)
        {
            Name = "Frostbolt";
            InitializeCastTime(false, false, 2, 0);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.13f, 0.804000020027161f, 0.241999998688698f, 0, 0.85699999332428f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfFrostbolt)
            {
                BaseCritRate += 0.05f;
            }
            BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            float fof = (solver.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * solver.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (solver.MageTalents.Shatter == 3 ? 0.5f : 0.17f * solver.MageTalents.Shatter);
            NukeProcs = 1;
            Dirty = false;
        }
    }

    // spell id: 71757, scaling id: 408
    public class DeepFreezeTemplate : SpellTemplate
    {
        //float fingersOfFrostCritRate;

        // 30 sec cooldown!!!
        public void Initialize(Solver solver)
        {
            Name = "Deep Freeze";
            InitializeCastTime(false, true, 0, 30);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.09f, 1.74000000953674f, 0.224999994039536f, 0, 2.57200002670288f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfDeepFreeze)
            {
                BaseSpellModifier *= 1.2f;
            }
            // deep freeze can only be cast in frozen state
            //float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            //fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
            Dirty = false;
        }

        /*public Spell GetSpell(CastingState castingState, bool averageFingersOfFrost)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (averageFingersOfFrost && castingState.CalculationOptions.TargetLevel > castingState.CalculationOptions.PlayerLevel + 2)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }*/

        /*public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }*/
    }

    // spell id: 2136, scaling id: 17
    public class FireBlastTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Fire Blast";
            InitializeCastTime(false, true, 0, 8);
            InitializeScaledDamage(solver, false, 30 + 5 * solver.MageTalents.ImprovedFireBlast, MagicSchool.Fire, 0.21f, 1.11300003528595f, 0.170000001788139f, 0, 0.428999990224838f, 0, 1, 1, 0);
            BaseCritRate += 0.04f * solver.MageTalents.ImprovedFireBlast;
            Dirty = false;
        }
    }

    // spell id: 133, scaling id: 18
    public class FireballTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool pom, bool brainFreeze)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Fireball";
            InitializeCastTime(false, false, 2.5f, 0);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.16f, 1.09099996089935f, 0.241999998688698f, 0, 1.12399995326996f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfFireball)
            {
                BaseCritRate += 0.05f;
            }
            BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            //BaseSpellModifier *= (1 + solver.BaseStats.BonusMageNukeMultiplier);
            NukeProcs = 1;
            Dirty = false;
        }
    }

    // spell id: 44614, scaling id: 22
    public class FrostfireBoltTemplate : SpellTemplate
    {
        private float fingersOfFrostCritRate;

        public Spell GetSpell(CastingState castingState, bool pom, bool averageFingersOfFrost, bool brainFreeze)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Frostfire Bolt";
            InitializeCastTime(false, false, 2.5f, 0);
            InitializeScaledDamage(solver, false, 40, MagicSchool.FrostFire, 0.16f, 0.949000000953674f, 0.241999998688698f, 0 /*0.00712000019848347*/, 0.976999998092651f, 0 /*0.00732999993488193*/, 1, 1, 0);
            if (solver.MageTalents.GlyphOfFrostfire)
            {
                BaseDirectDamageModifier *= 1.15f;
                BasePeriodicDamage = 3 * 0.03f * (BaseMinDamage + BaseMaxDamage) / 2f;
                DotDamageCoefficient = 3 * 0.03f * SpellDamageCoefficient;
                DotDuration = 12;
                DotTickInterval = 3;
            }
            BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            float fof = (solver.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * solver.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (solver.MageTalents.Shatter == 3 ? 0.5f : 0.17f * solver.MageTalents.Shatter);
            NukeProcs = 1;
            Dirty = false;
        }
    }

    // spell id: 11366, scaling id: 26
    public class PyroblastTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool pom, bool spammedDot)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, spammedDot);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, bool pom)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, false, false, false, false, true);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Pyroblast";
            InitializeCastTime(false, false, 3.5f, 0);
            InitializeScaledDamage(solver, false, 40, MagicSchool.Fire, 0.17f, 1.57500004768372f, 0.238000005483627f, 0.234999999403954f, 1.25f, 0.0869999974966049f, 1, 1, 12);
            DotDuration = 12;
            DotTickInterval = 3;
            if (solver.MageTalents.GlyphOfPyroblast)
            {
                BaseCritRate += 0.05f;
            }
            Dirty = false;
        }
    }

    // spell id: 44457/44461, scaling id: 24/25
    public class LivingBombTemplate : SpellTemplate
    {
        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            /*if (castingState.MageTalents.GlyphOfLivingBomb)
            {
                spell.DotDamageModifier = (1 + Math.Max(0.0f, Math.Min(1.0f, castingState.FireCritRate)) * (castingState.FireCritBonus - 1));
            }*/
            spell.CalculateDerivedStats(castingState, false, false, false);
            /*if (castingState.MageTalents.GlyphOfLivingBomb)
            {
                spell.IgniteProcs *= 5; // 4 ticks can proc ignite in addition to the explosion
                // add ignite contribution from dot
                if (castingState.Solver.NeedsDisplayCalculations)
                {
                    float igniteFactor = spell.SpellModifier * spell.HitRate * spell.PartialResistFactor * Math.Max(0.0f, Math.Min(1.0f, castingState.FireCritRate)) * castingState.FireCritBonus * castingState.Solver.IgniteFactor / (1 + castingState.Solver.IgniteFactor);
                    spell.IgniteDamage += spell.BasePeriodicDamage * igniteFactor;
                    spell.IgniteDamagePerSpellPower += spell.DotDamageCoefficient * igniteFactor;
                }
            }*/
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Living Bomb";
            InitializeCastTime(false, true, 0f, 0f);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Fire, 0.22f, 0.430000007152557f, 0, 4 * 0.430000007152557f, 0.232999995350838f, 4 * 0.232999995350838f, 1, 1, 0);
            DotDuration = 12;
            DotTickInterval = 3;
            if (solver.MageTalents.GlyphOfLivingBomb)
            {
                BaseSpellModifier *= 1.03f;
            }
            Dirty = false;
        }
    }

    public class SlowTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Slow";
            InitializeCastTime(false, true, 0f, 0f);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Arcane, 0.12f, 0, 0, 0, 0, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 120, scaling id: 15
    public class ConeOfColdTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Cone of Cold";
            InitializeCastTime(false, true, 0, 10);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Frost, 0.25f, 0.839999973773956f, 0.0900000035762787f, 0, 0.214000001549721f, 0, 1, 1, 0);
            Cooldown *= (1 - 0.07f * solver.MageTalents.IceFloes + (solver.MageTalents.IceFloes == 3 ? 0.01f : 0.00f));
            if (solver.MageTalents.GlyphOfConeOfCold)
            {
                BaseSpellModifier *= 1.25f;
            }
            Dirty = false;
        }
    }

    // spell id: 30455, scaling id: 23
    public class IceLanceTemplate : SpellTemplate
    {
        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (castingState.Frozen)
            {
                spell.SpellModifier *= 2;
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void Initialize(Solver solver)
        {
            Name = "Ice Lance";
            InitializeCastTime(false, true, 0, 0);
            InitializeScaledDamage(solver, false, 35, MagicSchool.Frost, 0.06f, 0.432000011205673f, 0.241999998688698f, 0, 0.377999991178513f, 0, 1, 1, 0);
            if (solver.MageTalents.GlyphOfIceLance)
            {
                BaseSpellModifier *= 1.05f;
            }
            Dirty = false;
        }
    }

    // spell id: 44425, scaling id: 8
    public class ArcaneBarrageTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, float arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        private float arcaneBlastDamageMultiplier;
        private float tormentTheWeak;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Barrage";
            if (solver.CalculationOptions.Beta)
            {
                InitializeCastTime(false, true, 0, 4);
                InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.12f, 1.25f, 0.200000002980232f, 0, 0.802999973297119f, 0, 1, 1, 0);
            }
            else
            {
                InitializeCastTime(false, true, 0, 5);
                InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.12f, 1.19099998474121f, 0.200000002980232f, 0, 0.764999985694885f, 0, 1, 1, 0);
            }
            tormentTheWeak = 0.02f * solver.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = 0f;
            if (solver.MageTalents.GlyphOfArcaneBarrage)
            {
                BaseDirectDamageModifier *= 1.04f;
            }
            Dirty = false;
        }
    }

    // spell id: 30451, scaling id: 9
    public class ArcaneBlastTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, int debuff, bool manualClearcasting, bool clearcastingActive, bool pom)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (castingState.CalculationOptions.Beta)
            {
                spell.BaseCastTime -= debuff * 0.1f;
            }
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += arcaneBlastManaMultiplier * debuff;
            spell.CalculateDerivedStats(castingState, false, pom, false, true, false, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.Solver, false, true, false, clearcastingActive);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff, bool forceHit)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (castingState.CalculationOptions.Beta)
            {
                spell.BaseCastTime -= debuff * 0.1f;
            }
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += arcaneBlastManaMultiplier * debuff;
            spell.CalculateDerivedStats(castingState, false, false, false, true, forceHit, !forceHit);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            if (castingState.CalculationOptions.Beta)
            {
                spell.BaseCastTime -= debuff * 0.1f;
            }
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += arcaneBlastManaMultiplier * debuff;
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3;
            if (solver.CalculationOptions.Beta)
            {
                cycle.CastTime += weight * rawSpell.CastTime - (weight1 * 0.1f + weight2 * 0.2f + weight3 * 0.3f) * rawSpell.CastTime / rawSpell.BaseCastTime;
            }
            else
            {
                cycle.CastTime += weight * rawSpell.CastTime;
            }
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.DamageProcs += weight * rawSpell.HitProcs;

            double roundCost = Math.Round(rawSpell.BaseCost * rawSpell.CostAmplifier);
            cycle.costPerSecond += (1 - solver.ClearcastingChance) * (weight0 * (float)Math.Floor(roundCost * rawSpell.CostModifier) + weight1 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + arcaneBlastManaMultiplier)) + weight2 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 2 * arcaneBlastManaMultiplier)) + weight3 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 3 * arcaneBlastManaMultiplier)));
            cycle.costPerSecond -= weight * rawSpell.CritRate * rawSpell.BaseCost * 0.15f * mageTalents.MasterOfElements;
            cycle.costPerSecond -= weight * BaseUntalentedCastTime / 60f * solver.BaseStats.ManaRestoreFromBaseManaPPM * solver.CalculationOptions.BaseMana;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            if (solver.CalculationOptions.Beta)
            {
                cycle.CastTime += weight * rawSpell.CastTime - (weight1 * 0.1f + weight2 * 0.2f + weight3 * 0.3f + weight4 * 0.4f) * rawSpell.CastTime / rawSpell.BaseCastTime;
            }
            else
            {
                cycle.CastTime += weight * rawSpell.CastTime;
            }
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.DamageProcs += weight * rawSpell.HitProcs;

            double roundCost = Math.Round(rawSpell.BaseCost * rawSpell.CostAmplifier);
            cycle.costPerSecond += (1 - solver.ClearcastingChance) * (weight0 * (float)Math.Floor(roundCost * rawSpell.CostModifier) + weight1 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + arcaneBlastManaMultiplier)) + weight2 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 2 * arcaneBlastManaMultiplier)) + weight3 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 3 * arcaneBlastManaMultiplier)) + weight4 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 4 * arcaneBlastManaMultiplier)));
            cycle.costPerSecond -= weight * rawSpell.CritRate * rawSpell.BaseCost * 0.15f * mageTalents.MasterOfElements;
            cycle.costPerSecond -= weight * BaseUntalentedCastTime / 60f * solver.BaseStats.ManaRestoreFromBaseManaPPM * solver.CalculationOptions.BaseMana;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        private float arcaneBlastDamageMultiplier;
        private float tormentTheWeak;
        private float arcaneBlastManaMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Blast";
            if (solver.CalculationOptions.Beta)
            {
                InitializeCastTime(false, false, 2.35f, 0);
            }
            else
            {
                InitializeCastTime(false, false, 2.5f, 0);
            }
            InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0.08f, 2.03500008583069f, 0.150000005960464f, 0, 1.057000041008f, 0, 1, 1, 0);
            Stats baseStats = solver.BaseStats;
            MageTalents mageTalents = solver.MageTalents;
            //BaseCostModifier += baseStats.ArcaneBlastBonus;
            BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            tormentTheWeak = 0.02f * solver.MageTalents.TormentTheWeak;
            if (solver.CalculationOptions.Beta)
            {
                arcaneBlastDamageMultiplier = mageTalents.GlyphOfArcaneBlast ? 0.13f : 0.1f;
                arcaneBlastManaMultiplier = 1.5f;
            }
            else
            {
                arcaneBlastDamageMultiplier = mageTalents.GlyphOfArcaneBlast ? 0.23f : 0.2f;
                arcaneBlastManaMultiplier = 1.75f;
            }
            NukeProcs = 1;
            Dirty = false;
        }
    }

    // spell id: 7268, scaling id: 12
    public class ArcaneMissilesTemplate : SpellTemplate
    {
        public Spell GetSpell(CastingState castingState, bool barrage, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing, int arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, clearcastingAveraged, clearcastingActive);
            //spell.BaseCastTime = ticks;
            if (barrage)
            {
                spell.BaseCastTime *= 0.5f;
                spell.CostModifier = Math.Max(spell.CostModifier - 1, 0);
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            //spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.Solver, false, true, clearcastingAveraged, clearcastingActive);
            return spell;
        }

        /*public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff)
        {
            return GetSpell(castingState, barrage, arcaneBlastDebuff, 5);
        }*/

        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Solver);
            spell.Calculate(castingState);
            //spell.BaseCastTime = ticks;
            if (barrage)
            {
                spell.BaseCastTime *= 0.5f;
                spell.CostModifier = Math.Max(spell.CostModifier - 1, 0);
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            //spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void AddToCycle(Solver solver, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = solver.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.CastProcs2 += weight * rawSpell.CastProcs2;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.costPerSecond += weight * rawSpell.AverageCost;
            cycle.DamageProcs += weight * rawSpell.HitProcs;
            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.DamagePerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.AverageDamage;
            cycle.threatPerSecond += multiplier * rawSpell.AverageThreat;
        }

        float tormentTheWeak;
        float arcaneBlastDamageMultiplier;

        public void Initialize(Solver solver)
        {
            Name = "Arcane Missiles";
            float castTime = 0.75f;
            if (solver.MageTalents.MissileBarrage == 1)
            {
                castTime = 0.6f;
            }
            else if (solver.MageTalents.MissileBarrage == 2)
            {
                castTime = 0.5f;
            }
#if RAWR4
            int missiles = 3 + solver.MageTalents.ImprovedArcaneMissiles;
#else
            int missiles = 5;
#endif
            InitializeCastTime(true, false, castTime * missiles, 0);
            if (solver.CalculationOptions.Beta)
            {
                InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0, 0.381999999284744f, 0, 0, 0.24600000679493f, 0, missiles, missiles + 1, 0);
            }
            else
            {
                InitializeScaledDamage(solver, false, 40, MagicSchool.Arcane, 0, 0.363999992609024f, 0, 0, 0.233999997377396f, 0, missiles, missiles + 1, 0);
            }
            BaseMinDamage *= missiles;
            BaseMaxDamage *= missiles;
            SpellDamageCoefficient *= missiles;
            CastProcs2 = 1;
            if (solver.MageTalents.GlyphOfArcaneMissiles)
            {
                BaseCritRate += 0.05f;
            }
            tormentTheWeak = 0.02f * solver.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = 0f;
            //BaseSpellModifier *= (1 + solver.BaseStats.BonusMageNukeMultiplier);
            BaseCritRate += 0.05f * solver.BaseStats.Mage4T9;
            // Arcane Potency bug/feature
            float arcanePotency;
            switch (solver.MageTalents.ArcanePotency)
            {
                case 0:
                default:
                    arcanePotency = 0;
                    break;
                case 1:
                    arcanePotency = 0.07f;
                    break;
                case 2:
                    arcanePotency = 0.15f;
                    break;
            }
            // not tested this, but assuming each wave has 1/5th chance even if less waves present
            // this is assuming two spells before AM are not AM, not completely accurate, but close enough
            float potencyChance = 11f / 125f * (10f - 3f * solver.ClearcastingChance) * solver.ClearcastingChance;
            BaseCritRate = BaseCritRate - solver.ArcanePotencyCrit + arcanePotency * potencyChance;
            Dirty = false;
        }
    }

    // spell id: 1449, scaling id: 10
    public class ArcaneExplosionTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Arcane Explosion";
            InitializeCastTime(false, true, 0, 0);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Arcane, 0.15f, 0.282999992370605f, 0.0799999982118607f, 0, 0.14300000667572f, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 11113, scaling id: 13
    public class BlastWaveTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Blast Wave";
            InitializeCastTime(false, true, 0, 15);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Fire, 0.07f, 0.989000022411346f, 0.164000004529953f, 0, 0.14300000667572f, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 31661, scaling id: 16
    public class DragonsBreathTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Dragon's Breath";
            InitializeCastTime(false, true, 0, 20);
            InitializeScaledDamage(solver, true, 0, MagicSchool.Fire, 0.07f, 1.37800002098083f, 0.150000005960464f, 0, 0.193000003695488f, 0, 1, 1, 0);
            Dirty = false;
        }
    }

    // spell id: 42208, scaling id: 14
    public class BlizzardTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Blizzard";
            InitializeCastTime(true, false, 8, 0);
            InitializeScaledDamage(solver, true, 30, MagicSchool.Frost, 0.74f, 8 * 0.319000005722046f, 0, 0, 0.0949999988079071f, 0, 8, 1, 0);
#if !RAWR4
            if (solver.MageTalents.ImprovedBlizzard > 0)
            {
                float fof = (solver.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * solver.MageTalents.FingersOfFrost);
                fof = Math.Max(fof, 0.05f * solver.MageTalents.Frostbite * solver.CalculationOptions.FrostbiteUtilization);
                BaseCritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (solver.MageTalents.Shatter == 3 ? 0.5f : 0.17f * solver.MageTalents.Shatter);
                //CritRate += (1.0f - (float)Math.Pow(1 - 0.05 * castingState.MageTalents.Frostbite, 5.0 / 2.0)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            BaseCritRate += 0.02f * solver.MageTalents.WorldInFlames;
#endif
            Dirty = false;
        }
    }

    public class ArcaneDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Arcane Damage";
            InitializeEffectDamage(solver, MagicSchool.Arcane, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusArcaneDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class FireDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Fire Damage";
            InitializeEffectDamage(solver, MagicSchool.Fire, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusFireDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class FrostDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Frost Damage";
            InitializeEffectDamage(solver, MagicSchool.Frost, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusFrostDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class ShadowDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Shadow Damage";
            InitializeEffectDamage(solver, MagicSchool.Shadow, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusShadowDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class NatureDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Nature Damage";
            InitializeEffectDamage(solver, MagicSchool.Nature, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusNatureDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class HolyDamageTemplate : SpellTemplate
    {
        public void Initialize(Solver solver)
        {
            Name = "Holy Damage";
            InitializeEffectDamage(solver, MagicSchool.Holy, 1, 1);
            CritBonus = 1.5f * 1.33f * (1 + solver.BaseStats.BonusSpellCritMultiplier);
            BaseSpellModifier = solver.BaseSpellModifier * (1 + solver.BaseStats.BonusHolyDamageMultiplier);
            BaseCritRate = solver.BaseCritRate;
            Dirty = false;
        }
    }

    public class ValkyrDamageTemplate : SpellTemplate
    {
        public float Multiplier;

        public void Initialize(Solver solver)
        {
            Name = "Valkyr Damage";
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + solver.TargetDebuffs.SpellCritOnTarget;
            // Valkyr always hit
            RealResistance = solver.CalculationOptions.HolyResist;
            PartialResistFactor = (RealResistance == -1) ? 0 : (1 - StatConversion.GetAverageResistance(solver.CalculationOptions.PlayerLevel, solver.CalculationOptions.TargetLevel, RealResistance, 0));
            Multiplier = PartialResistFactor * (1 + solver.TargetDebuffs.BonusDamageMultiplier) * (1 + solver.TargetDebuffs.BonusHolyDamageMultiplier) * (1 + (1.5f * 1.33f - 1) * spellCrit);
            Dirty = false;
        }
    }
}
