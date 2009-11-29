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
        FireWard,
        FrostWard,
        Waterbolt,
        MirrorImage,
    }

    public class WaterboltTemplate : SpellTemplate
    {
        Stats waterElementalBuffs;
        private static readonly string[] validBuffs = new string[] { "Ferocious Inspiration", "Sanctified Retribution", "Improved Moonkin Form", "Swift Retribution", "Elemental Oath", "Moonkin Form", "Wrath of Air Totem", "Demonic Pact", "Flametongue Totem", "Enhancing Totems (Spell Power)", "Totem of Wrath (Spell Power)", "Heart of the Crusader", "Master Poisoner", "Totem of Wrath", "Winter's Chill", "Improved Scorch", "Improved Shadow Bolt", "Curse of the Elements", "Earth and Moon", "Ebon Plaguebringer", "Improved Faerie Fire", "Misery" };
        float baseDamage, baseHaste, dpspBase, multiplier;

        public WaterboltTemplate(CharacterCalculationsMage calculations)
        {
            Name = "Waterbolt";
            waterElementalBuffs = new Stats();
            foreach (Buff buff in calculations.ActiveBuffs)
            {
                if (Array.IndexOf(validBuffs, buff.Name) >= 0)
                {
                    waterElementalBuffs.Accumulate(buff.Stats);
                }
            }
            baseDamage = 292.0f + (calculations.CalculationOptions.PlayerLevel - 50) * 11.5f;
            Character character = calculations.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = calculations.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + waterElementalBuffs.SpellCrit;
            float hitRate = calculations.BaseState.FrostHitRate;
            multiplier = hitRate;
            baseHaste = (1f + waterElementalBuffs.SpellHaste);
            multiplier *= (1 + waterElementalBuffs.BonusDamageMultiplier) * (1 + waterElementalBuffs.BonusFrostDamageMultiplier);
            float realResistance = calculationOptions.FrostResist;
            float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > playerLevel) ? ((targetLevel - playerLevel) * 0.02f) : 0f));
            multiplier *= partialResistFactor * (1 + 0.5f * spellCrit);
            dpspBase = ((1f / 3f) * 5f / 6f) * multiplier / 2.5f;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);

            float haste = castingState.Heroism ? baseHaste * 1.3f : baseHaste;

            spell.CastTime = 2.5f / haste;
            spell.CostPerSecond = 0.0f;
            spell.DamagePerSecond = (baseDamage + (castingState.FrostSpellPower / 3f + waterElementalBuffs.SpellPower + waterElementalBuffs.BonusSpellPowerDemonicPactMultiplier * castingState.CalculationOptions.WarlockSpellPower) * 5f / 6f) * multiplier / 2.5f * haste;
            spell.ThreatPerSecond = 0.0f;
            spell.DpsPerSpellPower = dpspBase * haste;

            return spell;
        }
    }

    public class MirrorImageTemplate : SpellTemplate
    {
        Stats mirrorImageBuffs;
        private static readonly string[] validBuffs = new string[] { "Heart of the Crusader", "Master Poisoner", "Totem of Wrath", "Winter's Chill", "Improved Scorch", "Improved Shadow Bolt", "Curse of the Elements", "Earth and Moon", "Ebon Plaguebringer", "Improved Faerie Fire", "Misery" };
        float baseDamageBlast, baseDamageBolt, boltMultiplier, blastMultiplier, castTime, multiplier, dpsp;

        public MirrorImageTemplate(CharacterCalculationsMage calculations)
        {
            Name = "Mirror Image";
            // these buffs are independent of casting state, so things that depend on them can be calculated only once and then reused
            mirrorImageBuffs = new Stats();
            foreach (Buff buff in calculations.ActiveBuffs)
            {
                if (Array.IndexOf(validBuffs, buff.Name) >= 0)
                {
                    mirrorImageBuffs.Accumulate(buff.Stats);
                }
            }
            baseDamageBlast = 97.5f;
            baseDamageBolt = 166.0f;
            Character character = calculations.CalculationOptions.Character;
            CalculationOptionsMage calculationOptions = calculations.CalculationOptions;
            int playerLevel = calculationOptions.PlayerLevel;
            int targetLevel = calculationOptions.TargetLevel;
            // TODO recheck all buffs that apply
            float spellCrit = 0.05f + mirrorImageBuffs.SpellCrit;
            // hit rate could actually change between casting states theoretically, but it is negligible and would slow things down unnecessarily
            float blastHitRate = calculations.BaseState.FireHitRate;
            float boltHitRate = calculations.BaseState.FrostHitRate;
            float haste = (1f + mirrorImageBuffs.SpellHaste);
            boltMultiplier = boltHitRate * (1 + mirrorImageBuffs.BonusDamageMultiplier) * (1 + mirrorImageBuffs.BonusFrostDamageMultiplier) * ((calculationOptions.FrostResist == 1) ? 0 : (1 - calculationOptions.FrostResist - ((targetLevel > playerLevel) ? ((targetLevel - playerLevel) * 0.02f) : 0f)));
            blastMultiplier = blastHitRate * (1 + mirrorImageBuffs.BonusDamageMultiplier) * (1 + mirrorImageBuffs.BonusFireDamageMultiplier) * ((calculationOptions.FireResist == 1) ? 0 : (1 - calculationOptions.FireResist - ((targetLevel > playerLevel) ? ((targetLevel - playerLevel) * 0.02f) : 0f)));
            castTime = (2 * 3.0f + 1.5f) / haste;
            multiplier = (calculations.MageTalents.GlyphOfMirrorImage ? 4 : 3) * (1 + 0.5f * spellCrit) / (2 * 3.0f + 1.5f) * haste;
            dpsp = multiplier * (2 * (1f / 3f * 0.3f) * boltMultiplier + (1f / 3f * 0.15f) * blastMultiplier);
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);

            spell.CastTime = castTime;
            spell.CostPerSecond = 0.0f;
            spell.DamagePerSecond = multiplier * (2 * (baseDamageBolt + castingState.FrostSpellPower / 3f * 0.3f) * boltMultiplier + (baseDamageBlast + castingState.FireSpellPower / 3f * 0.15f) * blastMultiplier);
            spell.ThreatPerSecond = 0.0f;
            spell.DpsPerSpellPower = dpsp;

            return spell;
        }
    }

    public class WandTemplate : SpellTemplate
    {
        private float speed;

        public WandTemplate(CharacterCalculationsMage calculations, MagicSchool school, int minDamage, int maxDamage, float speed)
            : base("Wand", false, false, false, 0, 30, 0, 0, school, minDamage, maxDamage, 0, 1, 0, 0, 0, 0)
        {
            // Tested: affected by Arcane Instability, affected by Chaotic meta, not affected by Arcane Power
            Calculate(calculations);
            this.speed = speed;
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = (1 + 0.01f * calculations.MageTalents.ArcaneInstability) * (1 + 0.01f * calculations.MageTalents.PlayingWithFire) * (1 + calculations.BaseStats.BonusDamageMultiplier);
            switch (school)
            {
                case MagicSchool.Arcane:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusArcaneDamageMultiplier);
                    break;
                case MagicSchool.Fire:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusFireDamageMultiplier);
                    break;
                case MagicSchool.Frost:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusFrostDamageMultiplier);
                    break;
                case MagicSchool.Nature:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusNatureDamageMultiplier);
                    break;
                case MagicSchool.Shadow:
                    BaseSpellModifier *= (1 + calculations.BaseStats.BonusShadowDamageMultiplier);
                    break;
            }
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
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
            spell.AverageDamage = spell.CalculateAverageDamage(castingState.Calculations, 0, false, false, out damagePerSpellPower, out igniteDamage, out igniteDamagePerSpellPower);

            spell.DamagePerSecond = spell.AverageDamage / speed;
            spell.ThreatPerSecond = spell.DamagePerSecond * ThreatMultiplier;
            spell.IgniteDamagePerSecond = 0;
            spell.IgniteDpsPerSpellPower = 0;
            spell.CostPerSecond = 0;
            spell.OO5SR = 1;
            return spell;
        }
    }

    public class FireBlastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireBlastTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.21 * BaseMana[71]), MinDamage = 664, MaxDamage = 786, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.21 * BaseMana[71]), MinDamage = 667, MaxDamage = 790, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.21 * BaseMana[72]), MinDamage = 671, MaxDamage = 794, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.21 * BaseMana[73]), MinDamage = 675, MaxDamage = 798, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.21 * BaseMana[74]), MinDamage = 760, MaxDamage = 900, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.21 * BaseMana[75]), MinDamage = 764, MaxDamage = 904, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.21 * BaseMana[76]), MinDamage = 768, MaxDamage = 908, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.21 * BaseMana[77]), MinDamage = 772, MaxDamage = 912, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.21 * BaseMana[78]), MinDamage = 776, MaxDamage = 916, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.21 * BaseMana[79]), MinDamage = 780, MaxDamage = 920, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.21 * BaseMana[80]), MinDamage = 925, MaxDamage = 1095, SpellDamageCoefficient = 1.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public FireBlastTemplate(CharacterCalculationsMage calculations)
            : base("Fire Blast", false, true, false, 20, 0, 8, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            Cooldown -= 1.0f * calculations.MageTalents.ImprovedFireBlast;
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
        }
    }

    public class ScorchTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ScorchTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.08 * BaseMana[70]), MinDamage = 305, MaxDamage = 361, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.08 * BaseMana[71]), MinDamage = 307, MaxDamage = 364, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.08 * BaseMana[72]), MinDamage = 310, MaxDamage = 366, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.08 * BaseMana[73]), MinDamage = 321, MaxDamage = 379, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.08 * BaseMana[74]), MinDamage = 323, MaxDamage = 382, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.08 * BaseMana[75]), MinDamage = 326, MaxDamage = 385, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.08 * BaseMana[76]), MinDamage = 328, MaxDamage = 387, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.08 * BaseMana[77]), MinDamage = 331, MaxDamage = 390, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.08 * BaseMana[78]), MinDamage = 376, MaxDamage = 444, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.08 * BaseMana[79]), MinDamage = 379, MaxDamage = 448, SpellDamageCoefficient = 1.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.08 * BaseMana[80]), MinDamage = 382, MaxDamage = 451, SpellDamageCoefficient = 1.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public virtual Spell GetSpell(CastingState castingState, bool clearcastingActive)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.Calculations, false, true, false, clearcastingActive);
            return spell;
        }

        public ScorchTemplate(CharacterCalculationsMage calculations)
            : base("Scorch", false, false, false, 30, 1.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            if (calculations.CalculationOptions.Mode33 && calculations.MageTalents.GlyphOfImprovedScorch)
            {
                BaseSpellModifier *= 1.2f;
            }
        }
    }

    public class FlamestrikeTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FlamestrikeTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.53 * BaseMana[70]), MinDamage = 480, MaxDamage = 585, PeriodicDamage = 424, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[1] = new SpellData() { Cost = (int)(0.53 * BaseMana[71]), MinDamage = 480, MaxDamage = 585, PeriodicDamage = 424, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[2] = new SpellData() { Cost = (int)(0.53 * BaseMana[72]), MinDamage = 688, MaxDamage = 842, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[3] = new SpellData() { Cost = (int)(0.53 * BaseMana[73]), MinDamage = 690, MaxDamage = 845, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[4] = new SpellData() { Cost = (int)(0.53 * BaseMana[74]), MinDamage = 693, MaxDamage = 848, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[5] = new SpellData() { Cost = (int)(0.53 * BaseMana[75]), MinDamage = 696, MaxDamage = 851, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[6] = new SpellData() { Cost = (int)(0.53 * BaseMana[76]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[7] = new SpellData() { Cost = (int)(0.53 * BaseMana[77]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[8] = new SpellData() { Cost = (int)(0.53 * BaseMana[78]), MinDamage = 699, MaxDamage = 854, PeriodicDamage = 620, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[9] = new SpellData() { Cost = (int)(0.53 * BaseMana[79]), MinDamage = 873, MaxDamage = 1067, PeriodicDamage = 780, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
            SpellData[10] = new SpellData() { Cost = (int)(0.53 * BaseMana[80]), MinDamage = 876, MaxDamage = 1071, PeriodicDamage = 780, SpellDamageCoefficient = 0.2427f, DotDamageCoefficient = 4 * 0.122f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public virtual Spell GetSpell(CastingState castingState, bool spammedDot)
        {
            AoeSpell spell = new AoeSpell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, false, spammedDot);
            return spell;
        }

        public FlamestrikeTemplate(CharacterCalculationsMage calculations)
            : base("Flamestrike", false, false, true, 30, 3, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions), 1, 1, 8f)
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            DotTickInterval = 2;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class ConjureManaGemTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConjureManaGemTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.75 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[1] = new SpellData() { Cost = (int)(0.75 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[2] = new SpellData() { Cost = (int)(0.75 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[3] = new SpellData() { Cost = (int)(0.75 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[4] = new SpellData() { Cost = (int)(0.75 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[5] = new SpellData() { Cost = (int)(0.75 * BaseMana[75]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[6] = new SpellData() { Cost = (int)(0.75 * BaseMana[76]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[7] = new SpellData() { Cost = (int)(0.75 * BaseMana[77]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[8] = new SpellData() { Cost = (int)(0.75 * BaseMana[78]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[9] = new SpellData() { Cost = (int)(0.75 * BaseMana[79]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[10] = new SpellData() { Cost = (int)(0.75 * BaseMana[80]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ConjureManaGemTemplate(CharacterCalculationsMage calculations)
            : base("Conjure Mana Gem", false, false, false, 0, 3, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions), 0, 1)
        {
            Calculate(calculations);
        }
    }

    public class FireWardTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireWardTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.16 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[1] = new SpellData() { Cost = (int)(0.16 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[2] = new SpellData() { Cost = (int)(0.16 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[3] = new SpellData() { Cost = (int)(0.16 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[4] = new SpellData() { Cost = (int)(0.16 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[5] = new SpellData() { Cost = (int)(0.16 * BaseMana[75]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[6] = new SpellData() { Cost = (int)(0.16 * BaseMana[76]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[7] = new SpellData() { Cost = (int)(0.16 * BaseMana[77]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[8] = new SpellData() { Cost = (int)(0.16 * BaseMana[78]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[9] = new SpellData() { Cost = (int)(0.16 * BaseMana[79]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[10] = new SpellData() { Cost = (int)(0.16 * BaseMana[80]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        private const float spellPowerCoefficient = 1.5f / 3.5f * 0.855f / 0.455f;

        public override Spell GetSpell(CastingState castingState)
        {
            AbsorbSpell spell = new AbsorbSpell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            // 70% absorbed, 30% negated
            // number of negates until absorb is distributed negative binomial
            // mean number of negated is then (1-p)/p = 0.3 / 0.7 times the absorb value
            // however on average it can't be more than (1-p) * incoming damage
            float q = 0.15f * castingState.MageTalents.FrostWarding;
            float absorb = 1950f + spellPowerCoefficient * castingState.FireSpellPower;
            spell.Absorb = absorb;
            spell.CostPerSecond -= Math.Min(q / (1 - q) * absorb, q * 30f * (float)castingState.Calculations.IncomingDamageDpsFire) / spell.CastTime;
            return spell;
        }

        public FireWardTemplate(CharacterCalculationsMage calculations)
            : base("Fire Ward", false, true, false, 0, 0, 30, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions), 0, 1)
        {
            Calculate(calculations);
        }
    }

    public class FrostWardTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostWardTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.14 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[1] = new SpellData() { Cost = (int)(0.14 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[2] = new SpellData() { Cost = (int)(0.14 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[3] = new SpellData() { Cost = (int)(0.14 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[4] = new SpellData() { Cost = (int)(0.14 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[5] = new SpellData() { Cost = (int)(0.14 * BaseMana[75]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[6] = new SpellData() { Cost = (int)(0.14 * BaseMana[76]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[7] = new SpellData() { Cost = (int)(0.14 * BaseMana[77]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[8] = new SpellData() { Cost = (int)(0.14 * BaseMana[78]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[9] = new SpellData() { Cost = (int)(0.14 * BaseMana[79]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
            SpellData[10] = new SpellData() { Cost = (int)(0.14 * BaseMana[80]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0, DotDamageCoefficient = 0 };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        private const float spellPowerCoefficient = 1.5f / 3.5f * 0.855f / 0.455f;

        public override Spell GetSpell(CastingState castingState)
        {
            AbsorbSpell spell = new AbsorbSpell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            float q = 0.15f * castingState.MageTalents.FrostWarding;
            float absorb = 1950f + spellPowerCoefficient * castingState.FrostSpellPower;
            spell.Absorb = absorb;
            spell.CostPerSecond -= Math.Min(q / (1 - q) * absorb, q * 30f * (float)castingState.Calculations.IncomingDamageDpsFrost) / spell.CastTime;
            return spell;
        }

        public FrostWardTemplate(CharacterCalculationsMage calculations)
            : base("Frost Ward", false, true, false, 0, 0, 30, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions), 0, 1)
        {
            Calculate(calculations);
        }
    }

    public class FrostNovaTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostNovaTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.07 * BaseMana[70]), MinDamage = 100, MaxDamage = 113, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f }; // TODO need level 70 WotLK data
            SpellData[1] = new SpellData() { Cost = (int)(0.07 * BaseMana[71]), MinDamage = 232, MaxDamage = 262, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[2] = new SpellData() { Cost = (int)(0.07 * BaseMana[72]), MinDamage = 232, MaxDamage = 263, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[3] = new SpellData() { Cost = (int)(0.07 * BaseMana[73]), MinDamage = 233, MaxDamage = 263, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[4] = new SpellData() { Cost = (int)(0.07 * BaseMana[74]), MinDamage = 233, MaxDamage = 264, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[5] = new SpellData() { Cost = (int)(0.07 * BaseMana[75]), MinDamage = 365, MaxDamage = 415, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[6] = new SpellData() { Cost = (int)(0.07 * BaseMana[76]), MinDamage = 365, MaxDamage = 416, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[7] = new SpellData() { Cost = (int)(0.07 * BaseMana[77]), MinDamage = 366, MaxDamage = 417, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[8] = new SpellData() { Cost = (int)(0.07 * BaseMana[78]), MinDamage = 367, MaxDamage = 418, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[9] = new SpellData() { Cost = (int)(0.07 * BaseMana[79]), MinDamage = 368, MaxDamage = 419, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
            SpellData[10] = new SpellData() { Cost = (int)(0.07 * BaseMana[80]), MinDamage = 368, MaxDamage = 419, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f * 0.13f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public FrostNovaTemplate(CharacterCalculationsMage calculations)
            : base("Frost Nova", false, true, true, 0, 0, 25, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
        }
    }

    public class FrostboltTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostboltTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.11 * BaseMana[70]), MinDamage = 630, MaxDamage = 680, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.11 * BaseMana[71]), MinDamage = 633, MaxDamage = 684, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.11 * BaseMana[72]), MinDamage = 637, MaxDamage = 688, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.11 * BaseMana[73]), MinDamage = 641, MaxDamage = 692, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.11 * BaseMana[74]), MinDamage = 645, MaxDamage = 696, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.11 * BaseMana[75]), MinDamage = 702, MaxDamage = 758, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.11 * BaseMana[76]), MinDamage = 706, MaxDamage = 763, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.11 * BaseMana[77]), MinDamage = 710, MaxDamage = 767, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.11 * BaseMana[78]), MinDamage = 714, MaxDamage = 771, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.11 * BaseMana[79]), MinDamage = 799, MaxDamage = 861, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.11 * BaseMana[80]), MinDamage = 803, MaxDamage = 866, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, bool manualClearcasting, bool clearcastingActive, bool pom, bool averageFingersOfFrost)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState, false, pom, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.Calculations, false, true, false, clearcastingActive);
            return spell;
        }

        float fingersOfFrostCritRate;
        float tormentTheWeak;

        public Spell GetSpell(CastingState castingState, bool averageFingersOfFrost, bool frozenCore)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            if (frozenCore && castingState.MageTalents.FrozenCore > 0)
            {
                spell.BaseCastTime -= 0.1f + 0.3f * castingState.MageTalents.FrozenCore;
            }
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            return GetSpell(castingState, false, false);
        }

        public FrostboltTemplate(CharacterCalculationsMage calculations)
            : base("Frostbolt", false, false, false, 30, 3, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFrostbolt)
            {
                BaseDirectDamageModifier *= 1.05f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.WintersChill;
            BaseCastTime -= 0.1f * calculations.MageTalents.ImprovedFrostbolt;
            BaseCastTime -= 0.1f * calculations.MageTalents.EmpoweredFrostbolt;
            BaseCritRate += 0.05f * calculations.BaseStats.Mage4T9;
            BaseInterruptProtection += calculations.BaseStats.AldorRegaliaInterruptProtection;
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFrostbolt;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier) * (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
            float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            NukeProcs = 1;
        }
    }

    public class DeepFreezeTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static DeepFreezeTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.09 * BaseMana[70]), MinDamage = 1919, MaxDamage = 2191, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.09 * BaseMana[71]), MinDamage = 1964, MaxDamage = 2236, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.09 * BaseMana[72]), MinDamage = 2009, MaxDamage = 2281, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.09 * BaseMana[73]), MinDamage = 2054, MaxDamage = 2326, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.09 * BaseMana[74]), MinDamage = 2099, MaxDamage = 2371, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.09 * BaseMana[75]), MinDamage = 2144, MaxDamage = 2416, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.09 * BaseMana[76]), MinDamage = 2189, MaxDamage = 2461, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.09 * BaseMana[77]), MinDamage = 2234, MaxDamage = 2506, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.09 * BaseMana[78]), MinDamage = 2279, MaxDamage = 2551, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.09 * BaseMana[79]), MinDamage = 2324, MaxDamage = 2596, SpellDamageCoefficient = 7.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.09 * BaseMana[80]), MinDamage = 2369, MaxDamage = 2641, SpellDamageCoefficient = 7.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        //float fingersOfFrostCritRate;

        // 30 sec cooldown!!!
        public DeepFreezeTemplate(CharacterCalculationsMage calculations)
            : base("Deep Freeze", false, true, false, 30, 0, 30, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfDeepFreeze)
            {
                Range += 10f;
            }
            // deep freeze can only be cast in frozen state
            //float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            //fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
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

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }
    }

    public class FireballTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FireballTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.19 * BaseMana[70]), MinDamage = 717, MaxDamage = 913, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.19 * BaseMana[71]), MinDamage = 721, MaxDamage = 918, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.19 * BaseMana[72]), MinDamage = 725, MaxDamage = 922, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.19 * BaseMana[73]), MinDamage = 729, MaxDamage = 926, PeriodicDamage = 92, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.19 * BaseMana[74]), MinDamage = 783, MaxDamage = 997, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.19 * BaseMana[75]), MinDamage = 787, MaxDamage = 1002, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.19 * BaseMana[76]), MinDamage = 792, MaxDamage = 1007, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.19 * BaseMana[77]), MinDamage = 796, MaxDamage = 1011, PeriodicDamage = 100, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.19 * BaseMana[78]), MinDamage = 888, MaxDamage = 1132, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.19 * BaseMana[79]), MinDamage = 893, MaxDamage = 1138, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.19 * BaseMana[80]), MinDamage = 898, MaxDamage = 1143, PeriodicDamage = 116, SpellDamageCoefficient = 3.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, bool pom, bool brainFreeze)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (brainFreeze)
            {
                spell.CostAmplifier = 0;
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, pom || brainFreeze, true);
            return spell;
        }

        float tormentTheWeak;

        public FireballTemplate(CharacterCalculationsMage calculations)
            : base("Fireball", false, false, false, 35, 3.5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFireball)
            {
                BasePeriodicDamage = 0.0f;
                BaseCritRate += 0.05f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch + 0.05f * calculations.BaseStats.Mage4T9;
            DotDuration = 8;
            DotTickInterval = 2;
            BaseInterruptProtection += calculations.BaseStats.AldorRegaliaInterruptProtection;
            BaseCastTime -= 0.1f * calculations.MageTalents.ImprovedFireball;
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFire;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            NukeProcs = 1;
        }
    }

    public class FrostfireBoltTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static FrostfireBoltTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.14 * BaseMana[70]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[1] = new SpellData() { Cost = (int)(0.14 * BaseMana[71]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[2] = new SpellData() { Cost = (int)(0.14 * BaseMana[72]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[3] = new SpellData() { Cost = (int)(0.14 * BaseMana[73]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[4] = new SpellData() { Cost = (int)(0.14 * BaseMana[74]), MinDamage = 0, MaxDamage = 0, PeriodicDamage = 0, SpellDamageCoefficient = 0f };
            SpellData[5] = new SpellData() { Cost = (int)(0.14 * BaseMana[75]), MinDamage = 629, MaxDamage = 731, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.14 * BaseMana[76]), MinDamage = 632, MaxDamage = 735, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.14 * BaseMana[77]), MinDamage = 636, MaxDamage = 739, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.14 * BaseMana[78]), MinDamage = 640, MaxDamage = 743, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.14 * BaseMana[79]), MinDamage = 644, MaxDamage = 747, PeriodicDamage = 60, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.14 * BaseMana[80]), MinDamage = 722, MaxDamage = 838, PeriodicDamage = 90, SpellDamageCoefficient = 3.0f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        private float tormentFactor;
        private float fingersOfFrostCritRate;

        public Spell GetSpell(CastingState castingState, bool pom, bool averageFingersOfFrost, bool frozenCore)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentFactor * castingState.SnaredTime);
            if (averageFingersOfFrost)
            {
                spell.CritRate += fingersOfFrostCritRate;
            }
            if (frozenCore && castingState.MageTalents.FrozenCore > 0)
            {
                spell.BaseCastTime -= 0.1f + 0.3f * castingState.MageTalents.FrozenCore;
            }
            spell.CalculateDerivedStats(castingState, false, pom, true);
            return spell;
        }

        public FrostfireBoltTemplate(CharacterCalculationsMage calculations)
            : base("Frostfire Bolt", false, false, false, 40, 3.0f, 0, MagicSchool.FrostFire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfFrostfire)
            {
                BaseCritRate += 0.02f;
                BaseDirectDamageModifier *= 1.02f;
            }
            BaseCritRate += 0.01f * calculations.MageTalents.ImprovedScorch + 0.05f * calculations.BaseStats.Mage4T9;
            tormentFactor = 0.04f * calculations.MageTalents.TormentTheWeak;
            BaseSpellModifier *= (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
            SpellDamageCoefficient += 0.05f * calculations.MageTalents.EmpoweredFire;
            DotDuration = 9;
            DotTickInterval = 3;
            float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
            fingersOfFrostCritRate = (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
            NukeProcs = 1;
        }
    }

    public class PyroblastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static PyroblastTemplate()
        {
            // spell data for Pyroblast is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 939, MaxDamage = 1191, PeriodicDamage = 356, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 1014, MaxDamage = 1286, PeriodicDamage = 384, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 1190, MaxDamage = 1510, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 1210, MaxDamage = 1531, PeriodicDamage = 452, SpellDamageCoefficient = 1.15f, DotDamageCoefficient = 0.2f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, bool pom, bool spammedDot)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom, spammedDot);
            return spell;
        }

        public DotSpell GetSpell(CastingState castingState, bool pom)
        {
            DotSpell spell = new DotSpell(this);
            spell.Calculate(castingState);
            spell.CalculateDerivedStats(castingState, false, pom);
            return spell;
        }

        public PyroblastTemplate(CharacterCalculationsMage calculations)
            : base("Pyroblast", false, false, false, 35, 5f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            DotDuration = 12;
            DotTickInterval = 3;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class LivingBombTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static LivingBombTemplate()
        {
            // spell data for Living Bomb is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 306, MaxDamage = 306, PeriodicDamage = 1024, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 690, MaxDamage = 690, PeriodicDamage = 1380, SpellDamageCoefficient = 0.4f, DotDamageCoefficient = 0.8f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (castingState.MageTalents.GlyphOfLivingBomb)
            {
                spell.DotDamageModifier = (1 + Math.Max(0.0f, Math.Min(1.0f, castingState.FireCritRate)) * (castingState.FireCritBonus - 1));
            }
            spell.CalculateDerivedStats(castingState, false, false, false);
            if (castingState.MageTalents.GlyphOfLivingBomb)
            {
                spell.IgniteProcs *= 5; // 4 ticks can proc ignite in addition to the explosion
                // add ignite contribution from dot
                if (castingState.Calculations.NeedsDisplayCalculations)
                {
                    float igniteFactor = spell.SpellModifier * spell.HitRate * spell.PartialResistFactor * Math.Max(0.0f, Math.Min(1.0f, castingState.FireCritRate)) * castingState.FireCritBonus * (0.08f * castingState.MageTalents.Ignite) / (1 + 0.08f * castingState.MageTalents.Ignite);
                    spell.IgniteDamagePerSecond += spell.BasePeriodicDamage * igniteFactor;
                    spell.IgniteDpsPerSpellPower += spell.DotDamageCoefficient * igniteFactor;
                }
            }
            return spell;
        }

        public LivingBombTemplate(CharacterCalculationsMage calculations)
            : base("Living Bomb", false, true, false, 35, 0f, 0, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            DotDuration = 12;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
            BaseAdditiveSpellModifier -= 0.02f * calculations.MageTalents.FirePower; // Living Bomb dot does not benefit from Fire Power
            BaseDirectDamageModifier *= (1 + 0.02f * calculations.MageTalents.FirePower);
        }
    }

    public class SlowTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static SlowTemplate()
        {
            // spell data for Living Bomb is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.12 * BaseMana[70]) };
            SpellData[1] = new SpellData() { Cost = (int)(0.12 * BaseMana[71]) };
            SpellData[2] = new SpellData() { Cost = (int)(0.12 * BaseMana[72]) };
            SpellData[3] = new SpellData() { Cost = (int)(0.12 * BaseMana[73]) };
            SpellData[4] = new SpellData() { Cost = (int)(0.12 * BaseMana[74]) };
            SpellData[5] = new SpellData() { Cost = (int)(0.12 * BaseMana[75]) };
            SpellData[6] = new SpellData() { Cost = (int)(0.12 * BaseMana[76]) };
            SpellData[7] = new SpellData() { Cost = (int)(0.12 * BaseMana[77]) };
            SpellData[8] = new SpellData() { Cost = (int)(0.12 * BaseMana[78]) };
            SpellData[9] = new SpellData() { Cost = (int)(0.12 * BaseMana[79]) };
            SpellData[10] = new SpellData() { Cost = (int)(0.12 * BaseMana[80]) };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public SlowTemplate(CharacterCalculationsMage calculations)
            : base("Slow", false, true, false, 30, 0f, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
        }
    }

    //922-983
    //
    //709 + k*992<=922
    //776 + k*992>=983
    //
    //0.20866935483870967741935483870968 <= k <= 0.21471774193548387096774193548387
    public class ConeOfColdTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ConeOfColdTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.25 * BaseMana[70]), MinDamage = 418, MaxDamage = 457, SpellDamageCoefficient = 0.2142f };
            SpellData[1] = new SpellData() { Cost = (int)(0.25 * BaseMana[71]), MinDamage = 418, MaxDamage = 457, SpellDamageCoefficient = 0.2142f };
            SpellData[2] = new SpellData() { Cost = (int)(0.25 * BaseMana[72]), MinDamage = 559, MaxDamage = 611, SpellDamageCoefficient = 0.2142f };
            SpellData[3] = new SpellData() { Cost = (int)(0.25 * BaseMana[73]), MinDamage = 561, MaxDamage = 614, SpellDamageCoefficient = 0.2142f };
            SpellData[4] = new SpellData() { Cost = (int)(0.25 * BaseMana[74]), MinDamage = 563, MaxDamage = 616, SpellDamageCoefficient = 0.2142f };
            SpellData[5] = new SpellData() { Cost = (int)(0.25 * BaseMana[75]), MinDamage = 565, MaxDamage = 618, SpellDamageCoefficient = 0.2142f };
            SpellData[6] = new SpellData() { Cost = (int)(0.25 * BaseMana[76]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[7] = new SpellData() { Cost = (int)(0.25 * BaseMana[77]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[8] = new SpellData() { Cost = (int)(0.25 * BaseMana[78]), MinDamage = 568, MaxDamage = 621, SpellDamageCoefficient = 0.2142f };
            SpellData[9] = new SpellData() { Cost = (int)(0.25 * BaseMana[79]), MinDamage = 707, MaxDamage = 773, SpellDamageCoefficient = 0.2142f };
            SpellData[10] = new SpellData() { Cost = (int)(0.25 * BaseMana[80]), MinDamage = 709, MaxDamage = 776, SpellDamageCoefficient = 0.2142f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ConeOfColdTemplate(CharacterCalculationsMage calculations)
            : base("Cone of Cold", false, true, true, 0, 0, 10, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            Cooldown *= (1 - 0.07f * calculations.MageTalents.IceFloes + (calculations.MageTalents.IceFloes == 3 ? 0.01f : 0.00f));
            AoeDamageCap = 37500;
            int ImprovedConeOfCold = calculations.MageTalents.ImprovedConeOfCold;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            BaseSpellModifier *= (1 + ((ImprovedConeOfCold > 0) ? (0.05f + 0.1f * ImprovedConeOfCold) : 0));
            BaseCritRate += 0.02f * calculations.MageTalents.Incineration;
        }
    }

    public class IceLanceTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static IceLanceTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.06 * BaseMana[70]), MinDamage = 161, MaxDamage = 187, SpellDamageCoefficient = 0.1429f };
            SpellData[1] = new SpellData() { Cost = (int)(0.06 * BaseMana[71]), MinDamage = 161, MaxDamage = 187, SpellDamageCoefficient = 0.1429f };
            SpellData[2] = new SpellData() { Cost = (int)(0.06 * BaseMana[72]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[3] = new SpellData() { Cost = (int)(0.06 * BaseMana[73]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[4] = new SpellData() { Cost = (int)(0.06 * BaseMana[74]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[5] = new SpellData() { Cost = (int)(0.06 * BaseMana[75]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[6] = new SpellData() { Cost = (int)(0.06 * BaseMana[76]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[7] = new SpellData() { Cost = (int)(0.06 * BaseMana[77]), MinDamage = 182, MaxDamage = 210, SpellDamageCoefficient = 0.1429f };
            SpellData[8] = new SpellData() { Cost = (int)(0.06 * BaseMana[78]), MinDamage = 221, MaxDamage = 255, SpellDamageCoefficient = 0.1429f };
            SpellData[9] = new SpellData() { Cost = (int)(0.06 * BaseMana[79]), MinDamage = 221, MaxDamage = 255, SpellDamageCoefficient = 0.1429f };
            SpellData[10] = new SpellData() { Cost = (int)(0.06 * BaseMana[80]), MinDamage = 223, MaxDamage = 258, SpellDamageCoefficient = 0.1429f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (castingState.Frozen)
            {
                if (castingState.MageTalents.GlyphOfIceLance && castingState.CalculationOptions.TargetLevel > castingState.CalculationOptions.PlayerLevel)
                {
                    spell.SpellModifier *= 4;
                }
                else
                {
                    spell.SpellModifier *= 3;
                }
            }
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public IceLanceTemplate(CharacterCalculationsMage calculations)
            : base("Ice Lance", false, true, false, 30, 0, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            BaseSpellModifier *= (1 + 0.01f * calculations.MageTalents.ChilledToTheBone);
        }
    }

    public class ArcaneBarrageTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBarrageTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.18 * BaseMana[70]), MinDamage = 709, MaxDamage = 865, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.18 * BaseMana[71]), MinDamage = 724, MaxDamage = 881, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.18 * BaseMana[72]), MinDamage = 740, MaxDamage = 896, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.18 * BaseMana[73]), MinDamage = 755, MaxDamage = 912, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.18 * BaseMana[74]), MinDamage = 771, MaxDamage = 927, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.18 * BaseMana[75]), MinDamage = 786, MaxDamage = 943, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.18 * BaseMana[76]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.18 * BaseMana[77]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.18 * BaseMana[78]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 3.0f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.18 * BaseMana[79]), MinDamage = 802, MaxDamage = 958, SpellDamageCoefficient = 0.95f * 3.0f / 3.5f }; // downranking penalty
            SpellData[10] = new SpellData() { Cost = (int)(0.18 * BaseMana[80]), MinDamage = 936, MaxDamage = 1144, SpellDamageCoefficient = 2.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, float arcaneBlastDebuff)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        private float arcaneBlastDamageMultiplier;
        private float tormentTheWeak;

        public ArcaneBarrageTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Barrage", false, true, false, 30, 0, 3, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = calculations.MageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            if (calculations.MageTalents.GlyphOfArcaneBarrage)
            {
                BaseCostAmplifier *= 0.8f; // TODO is it additive or multiplicative?
            }
        }
    }

    public class ArcaneBlastTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneBlastTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.07 * BaseMana[70]), MinDamage = 668, MaxDamage = 772, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.07 * BaseMana[71]), MinDamage = 690, MaxDamage = 800, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.07 * BaseMana[72]), MinDamage = 695, MaxDamage = 806, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.07 * BaseMana[73]), MinDamage = 700, MaxDamage = 811, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.07 * BaseMana[74]), MinDamage = 705, MaxDamage = 816, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.07 * BaseMana[75]), MinDamage = 711, MaxDamage = 822, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.07 * BaseMana[76]), MinDamage = 805, MaxDamage = 935, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.07 * BaseMana[77]), MinDamage = 811, MaxDamage = 942, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.07 * BaseMana[78]), MinDamage = 817, MaxDamage = 948, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.07 * BaseMana[79]), MinDamage = 823, MaxDamage = 954, SpellDamageCoefficient = 2.5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.07 * BaseMana[80]), MinDamage = 1185, MaxDamage = 1377, SpellDamageCoefficient = 2.5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, int debuff, bool manualClearcasting, bool clearcastingActive, bool pom)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            if (manualClearcasting) spell.CalculateManualClearcasting(true, false, clearcastingActive);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += 1.75f * debuff;
            spell.CalculateDerivedStats(castingState, false, pom, false, true, false, false);
            if (manualClearcasting) spell.CalculateManualClearcastingCost(castingState.Calculations, false, true, false, clearcastingActive);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff, bool forceHit)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += 1.75f * debuff;
            spell.CalculateDerivedStats(castingState, false, false, false, true, forceHit, !forceHit);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, int debuff)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * debuff;
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CostModifier += 1.75f * debuff;
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public override Spell GetSpell(CastingState castingState)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.CalculateDerivedStats(castingState, false, false, false, true, false, false);
            return spell;
        }

        public void AddToCycle(CharacterCalculationsMage calculations, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3)
        {
            MageTalents mageTalents = calculations.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.DamageProcs += weight * rawSpell.HitProcs;

            double roundCost = Math.Round(rawSpell.BaseCost * rawSpell.CostAmplifier);
            cycle.costPerSecond += (1 - 0.02f * mageTalents.ArcaneConcentration) * (weight0 * (float)Math.Floor(roundCost * rawSpell.CostModifier) + weight1 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 1.75f)) + weight2 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 3.50f)) + weight3 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 5.25f)));
            cycle.costPerSecond -= weight * rawSpell.CritRate * rawSpell.BaseCost * 0.1f * mageTalents.MasterOfElements;
            cycle.costPerSecond -= weight * BaseUntalentedCastTime / 60f * calculations.BaseStats.ManaRestoreFromBaseManaPPM * 3268;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.CastTime * rawSpell.DpsPerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.CastTime * rawSpell.DamagePerSecond;
            cycle.threatPerSecond += multiplier * rawSpell.CastTime * rawSpell.ThreatPerSecond;
        }

        public void AddToCycle(CharacterCalculationsMage calculations, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = calculations.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.DamageProcs += weight * rawSpell.HitProcs;

            double roundCost = Math.Round(rawSpell.BaseCost * rawSpell.CostAmplifier);
            cycle.costPerSecond += (1 - 0.02f * mageTalents.ArcaneConcentration) * (weight0 * (float)Math.Floor(roundCost * rawSpell.CostModifier) + weight1 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 1.75f)) + weight2 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 3.50f)) + weight3 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 5.25f)) + weight4 * (float)Math.Floor(roundCost * (rawSpell.CostModifier + 7.00f)));
            cycle.costPerSecond -= weight * rawSpell.CritRate * rawSpell.BaseCost * 0.1f * mageTalents.MasterOfElements;
            cycle.costPerSecond -= weight * BaseUntalentedCastTime / 60f * calculations.BaseStats.ManaRestoreFromBaseManaPPM * 3268;

            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.CastTime * rawSpell.DpsPerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.CastTime * rawSpell.DamagePerSecond;
            cycle.threatPerSecond += multiplier * rawSpell.CastTime * rawSpell.ThreatPerSecond;
        }

        private float arcaneBlastDamageMultiplier;
        private float tormentTheWeak;

        public ArcaneBlastTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Blast", false, false, false, 30, 2.5f, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Stats baseStats = calculations.BaseStats;
            MageTalents mageTalents = calculations.MageTalents;
            CalculationOptionsMage calculationOptions = calculations.CalculationOptions;
            Calculate(calculations);
            BaseInterruptProtection += 0.2f * mageTalents.ArcaneStability;
            BaseCostModifier += baseStats.ArcaneBlastBonus;
            BaseCritRate += 0.05f * calculations.BaseStats.Mage4T9;
            arcaneBlastDamageMultiplier = mageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            BaseAdditiveSpellModifier += baseStats.ArcaneBlastBonus + 0.02f * mageTalents.SpellImpact;
            tormentTheWeak = 0.04f * mageTalents.TormentTheWeak;
            SpellDamageCoefficient += 0.03f * mageTalents.ArcaneEmpowerment;
            BaseCritRate += 0.02f * mageTalents.Incineration;
            NukeProcs = 1;
        }
    }

    // 582 arcane, 500-501, 1.03 amp
    // 982 arcane, 655-656 , 1.03 amp
    // 1274 arcane, 768-769, 1.03 amp
    // 1269 arcane, 805-806, 1.03 * 1.05 amp
    // 1275 arcane, 807-808 , 1.03 * 1.05 amp
    //
    // 267.55514100785945446139620896945 <= base 10 <= 267.7420527045769764216366158113
    // 287.7142857142857142857142857144 <= base 11 <= 288.7142857142857142857142857144
    //
    // rank 11: 71 (with k = kbase + 0.45/5
    //792/(1.03*1.03*1.05) <= x + k*1182 <= 793/(1.03*1.03*1.05)
    //665/(1.03*1.03*1.05) <= x + k*864 <= 666/(1.03*1.03*1.05)
    //476/(1.03*1.03*1.05) <= x + k*389 <= 477/(1.03*1.03*1.05)
    //319/(1.03*1.03) <= x + k*35 <= 320/(1.03*1.03)
    //289 <= x <= 290
    //297/1.03 <= x <= 298/1.03
    //306/(1.03*1.03) <= x <= 307/(1.03*1.03)

    //710.98662860374614545601443518307 <= x + k*1182 <= 711.88433899339734008411546351032
    //596.97740911804442768718383762214 <= x + k*864 <= 597.8751195076956223152848659494
    //427.31014547396864297608948377164 <= x + k*389 <= 428.20785586361983760419051209889
    //300.68809501366764068243943821284 <= x + k*35 <= 301.63069092280139504194551795645

    //409.3559376809447504140689172266 <= k*1147 <= 411.1962439797296994016760252975
    //295.3467181952430326452383196657 <= k*829 <= 297.1870244940279816328454277366
    //125.6794545511672479341439658152 <= k*354 <= 127.519760849952196921751073886

    //0.3568927093992543595589092565184 <= k <= 0.35849716127265013025429470383391
    //0.3562686588603655399821933892228 <= k <= 0.35848856995660793924348061246876
    //0.3550267077716588924693332367661 <= k <= 0.36022531313545818339477704487571

    //0.3568927093992543595589092565184 <= k <= 0.35848856995660793924348061246876
    //0.266892709399254359558909256518 <= kraw <= 0.268488569956607939243480612468
    //0.934124482897390258456182397813 <= kbase <= 0.939709994848127787352182143638

    //4.670622414486951292280911989065 <= kbase <= 4.69854997424063893676091071819

    //k := 0.35692857142857142857142857142857

    //289.0970571751747168845858637551 <= x <= 289.9947675648259115126868920824
    //288.59112340375871340146955190831 <= x <= 289.48883379340990802957058023561
    //288.46493118825435726180376948611 <= x <= 289.36264157790555188990479781331
    //288.19559501366764068243943821282 <= x <= 289.13819092280139504194551795642

    //289.0970571751747168845858637551 <= x <= 289.13819092280139504194551795642

    // x := 289.1

    // level 72
    // 290 <= x <= 291
    // 299/1.03 <= x <= 300/1.03

    // 290.31954001319634272787256103309 <= x <= 291

    // 308/(1.03*1.03) <= x <= 309/(1.03*1.03)
    // 321/(1.03*1.03) <= x + k*36 <= 322/(1.03*1.03) (very heavily slanted towards 321)
    // 476/(1.03*1.03) <= x + k*468 <= 477/(1.03*1.03)
    // 769/(1.03*1.03*1.05) <= x + k*1182 <= 770/(1.03*1.03*1.05)

    // 0.32148018977597637226254438055556 <= k <= 0.36656507577423780647458656696111
    // 0.33691378792236554513866230333376 <= k <= 0.34038185607607796315497324074957
    // 0.3378504988509041192975387340577 <= k <= 0.33918566837413157437387415478046

    // 0.3378504988509041192975387340577 <= k <= 0.33918566837413157437387415478046

    // k := 0.33814285714285714285714285714286

    // 290.40014397479229225859445484289 <= x <= 291.34273988392604661810053458649
    // 290.42479560480993226775110081774 <= x <= 291.36739151394368662725718056134
    // 290.65443249891152615254792651436 <= x <= 291.55214288856272078064895484166

    // 290.65443249891152615254792651436 <= x <= 291

    // x := 290.8

    // level 73
    // 291 <= x <= 292
    // 300/1.03 <= x <= 301/1.03
    // 309/(1.03*1.03) <= x <= 310/(1.03*1.03)

    // 291.2621359223300970873786407767 <= x <= 292

    // k := 0.85*(5/3.5+0.45)/5 = 0.31935714285714285714285714285714

    // 612/(1.03*1.03*1.05) <= 257.67142857142857142857142857143 + x <= 613/(1.03*1.03*1.05)

    // => 291.72732989510254096925790770642 <= x <= 292

    // 702/(1.03*1.03*1.05) <= 338.83792857142857142857142857128 + x <= 703/(1.03*1.03*1.05)

    // x := 291.9
    public class ArcaneMissilesTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneMissilesTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.31 * BaseMana[70]), MinDamage = 287.9f * 5, MaxDamage = 287.9f * 5, SpellDamageCoefficient = 5f / 3.5f }; // there's some indication that coefficient might be slightly different
            SpellData[1] = new SpellData() { Cost = (int)(0.31 * BaseMana[71]), MinDamage = 289.1f * 5, MaxDamage = 289.1f * 5, SpellDamageCoefficient = 4.67125f / 3.5f }; // some huge downraking style penalty for some reason (seems to be 0.95 * (5/3.5 + 0.45)), for now don't place the coeff on 0.45, just use 4.67125 instead of 4.75
            SpellData[2] = new SpellData() { Cost = (int)(0.31 * BaseMana[72]), MinDamage = 290.8f * 5, MaxDamage = 290.8f * 5, SpellDamageCoefficient = 4.3425f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.9 * (5/3.5 + 0.45), confirmed)
            SpellData[3] = new SpellData() { Cost = (int)(0.31 * BaseMana[73]), MinDamage = 291.9f * 5, MaxDamage = 291.9f * 5, SpellDamageCoefficient = 4.01375f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.85 * (5/3.5 + 0.45), confirmed)
            SpellData[4] = new SpellData() { Cost = (int)(0.31 * BaseMana[74]), MinDamage = 293.0f * 5, MaxDamage = 293.0f * 5, SpellDamageCoefficient = 3.685f / 3.5f }; // some huge downraking style penalty for some reason (hypothesis 0.8 * (5/3.5 + 0.45), confirmed)
            SpellData[5] = new SpellData() { Cost = (int)(0.31 * BaseMana[75]), MinDamage = 320.0f * 5, MaxDamage = 320.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.31 * BaseMana[76]), MinDamage = 321.7f * 5, MaxDamage = 321.7f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.31 * BaseMana[77]), MinDamage = 323.0f * 5, MaxDamage = 323.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.31 * BaseMana[78]), MinDamage = 324.8f * 5, MaxDamage = 324.8f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.31 * BaseMana[79]), MinDamage = 360.0f * 5, MaxDamage = 360.0f * 5, SpellDamageCoefficient = 5f / 3.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.31 * BaseMana[80]), MinDamage = 361.9f * 5, MaxDamage = 361.9f * 5, SpellDamageCoefficient = 5f / 3.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public Spell GetSpell(CastingState castingState, bool barrage, bool clearcastingAveraged, bool clearcastingActive, bool clearcastingProccing, int arcaneBlastDebuff, float ticks)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.CalculateManualClearcasting(true, clearcastingAveraged, clearcastingActive);
            spell.BaseCastTime = ticks;
            if (barrage)
            {
                spell.BaseCastTime *= 0.5f;
                spell.CostModifier = Math.Max(spell.CostModifier - 1, 0);
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            spell.CalculateManualClearcastingCost(castingState.Calculations, false, true, clearcastingAveraged, clearcastingActive);
            return spell;
        }

        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff)
        {
            return GetSpell(castingState, barrage, arcaneBlastDebuff, 5);
        }

        public Spell GetSpell(CastingState castingState, bool barrage, int arcaneBlastDebuff, int ticks)
        {
            Spell spell = Spell.New(this, castingState.Calculations);
            spell.Calculate(castingState);
            spell.BaseCastTime = ticks;
            if (barrage)
            {
                spell.BaseCastTime *= 0.5f;
                spell.CostModifier = Math.Max(spell.CostModifier - 1, 0);
            }
            spell.SpellModifier *= (1 + tormentTheWeak * castingState.SnaredTime);
            spell.AdditiveSpellModifier += arcaneBlastDamageMultiplier * arcaneBlastDebuff;
            spell.SpellModifier *= ticks / 5.0f;
            spell.CalculateDerivedStats(castingState);
            return spell;
        }

        public void AddToCycle(CharacterCalculationsMage calculations, Cycle cycle, Spell rawSpell, float weight0, float weight1, float weight2, float weight3, float weight4)
        {
            MageTalents mageTalents = calculations.MageTalents;
            float weight = weight0 + weight1 + weight2 + weight3 + weight4;
            cycle.CastTime += weight * rawSpell.CastTime;
            cycle.CastProcs += weight * rawSpell.CastProcs;
            cycle.NukeProcs += weight * rawSpell.NukeProcs;
            cycle.Ticks += weight * rawSpell.Ticks;
            cycle.HitProcs += weight * rawSpell.HitProcs;
            cycle.CritProcs += weight * rawSpell.CritProcs;
            cycle.TargetProcs += weight * rawSpell.TargetProcs;
            cycle.costPerSecond += weight * rawSpell.CastTime * rawSpell.CostPerSecond;
            cycle.DamageProcs += weight * rawSpell.HitProcs;
            float multiplier = (weight * rawSpell.AdditiveSpellModifier + arcaneBlastDamageMultiplier * (weight1 + 2 * weight2 + 3 * weight3 + 4 * weight4)) / rawSpell.AdditiveSpellModifier;
            cycle.DpsPerSpellPower += multiplier * rawSpell.CastTime * rawSpell.DpsPerSpellPower;
            cycle.damagePerSecond += multiplier * rawSpell.CastTime * rawSpell.DamagePerSecond;
            cycle.threatPerSecond += multiplier * rawSpell.CastTime * rawSpell.ThreatPerSecond;
        }

        float tormentTheWeak;
        float arcaneBlastDamageMultiplier;        

        public ArcaneMissilesTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Missiles", true, false, false, 30, 5, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions), 5, 6)
        {
            base.Calculate(calculations);
            if (calculations.MageTalents.GlyphOfArcaneMissiles)
            {
                CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculations.MageTalents.SpellPower + 0.1f * calculations.MageTalents.Burnout + calculations.BaseStats.CritBonusDamage + 0.25f));
            }
            SpellDamageCoefficient += 0.15f * calculations.MageTalents.ArcaneEmpowerment;
            tormentTheWeak = 0.04f * calculations.MageTalents.TormentTheWeak;
            arcaneBlastDamageMultiplier = calculations.MageTalents.GlyphOfArcaneBlast ? 0.18f : 0.15f;
            BaseSpellModifier *= (1 + calculations.BaseStats.BonusMageNukeMultiplier);
            BaseInterruptProtection += 0.2f * calculations.MageTalents.ArcaneStability;
            BaseCritRate += 0.05f * calculations.BaseStats.Mage4T9;
            // Arcane Potency bug
            BaseCritRate -= 0.8f * 0.15f * 0.02f * calculations.MageTalents.ArcaneConcentration * calculations.MageTalents.ArcanePotency;
        }
    }

    public class ArcaneExplosionTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static ArcaneExplosionTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.22 * BaseMana[70]), MinDamage = 377, MaxDamage = 407, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[1] = new SpellData() { Cost = (int)(0.22 * BaseMana[71]), MinDamage = 378, MaxDamage = 409, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[2] = new SpellData() { Cost = (int)(0.22 * BaseMana[72]), MinDamage = 380, MaxDamage = 411, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[3] = new SpellData() { Cost = (int)(0.22 * BaseMana[73]), MinDamage = 381, MaxDamage = 412, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[4] = new SpellData() { Cost = (int)(0.22 * BaseMana[74]), MinDamage = 383, MaxDamage = 414, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[5] = new SpellData() { Cost = (int)(0.22 * BaseMana[75]), MinDamage = 385, MaxDamage = 415, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[6] = new SpellData() { Cost = (int)(0.22 * BaseMana[76]), MinDamage = 481, MaxDamage = 519, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[7] = new SpellData() { Cost = (int)(0.22 * BaseMana[77]), MinDamage = 483, MaxDamage = 521, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[8] = new SpellData() { Cost = (int)(0.22 * BaseMana[78]), MinDamage = 485, MaxDamage = 523, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[9] = new SpellData() { Cost = (int)(0.22 * BaseMana[79]), MinDamage = 487, MaxDamage = 525, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
            SpellData[10] = new SpellData() { Cost = (int)(0.22 * BaseMana[80]), MinDamage = 538, MaxDamage = 582, SpellDamageCoefficient = 1.5f / 3.5f * 0.5f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public ArcaneExplosionTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Explosion", false, true, true, 0, 0, 0, MagicSchool.Arcane, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            if (calculations.MageTalents.GlyphOfArcaneExplosion) BaseCostAmplifier *= 0.9f;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            AoeDamageCap = 37500;
        }
    }

    public class BlastWaveTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static BlastWaveTemplate()
        {
            // spell data for Blast Wave is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.28 * BaseMana[70]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[1] = new SpellData() { Cost = (int)(0.28 * BaseMana[71]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[2] = new SpellData() { Cost = (int)(0.28 * BaseMana[72]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[3] = new SpellData() { Cost = (int)(0.28 * BaseMana[73]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[4] = new SpellData() { Cost = (int)(0.28 * BaseMana[74]), MinDamage = 616, MaxDamage = 724, SpellDamageCoefficient = 0.1929f };
            SpellData[5] = new SpellData() { Cost = (int)(0.28 * BaseMana[75]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[6] = new SpellData() { Cost = (int)(0.28 * BaseMana[76]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[7] = new SpellData() { Cost = (int)(0.28 * BaseMana[77]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[8] = new SpellData() { Cost = (int)(0.28 * BaseMana[78]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[9] = new SpellData() { Cost = (int)(0.28 * BaseMana[79]), MinDamage = 882, MaxDamage = 1038, SpellDamageCoefficient = 0.1929f };
            SpellData[10] = new SpellData() { Cost = (int)(0.28 * BaseMana[80]), MinDamage = 1047, MaxDamage = 1233, SpellDamageCoefficient = 0.1929f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public BlastWaveTemplate(CharacterCalculationsMage calculations)
            : base("Blast Wave", false, true, true, 0, 0, 30, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            BaseAdditiveSpellModifier += 0.02f * calculations.MageTalents.SpellImpact;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class DragonsBreathTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static DragonsBreathTemplate()
        {
            // spell data for Dragon's Breath is not level adjusted except for level 70 and 80, adjust if the needed data is found
            SpellData[0] = new SpellData() { Cost = (int)(0.31 * BaseMana[70]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[1] = new SpellData() { Cost = (int)(0.31 * BaseMana[71]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[2] = new SpellData() { Cost = (int)(0.31 * BaseMana[72]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[3] = new SpellData() { Cost = (int)(0.31 * BaseMana[73]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[4] = new SpellData() { Cost = (int)(0.31 * BaseMana[74]), MinDamage = 680, MaxDamage = 790, SpellDamageCoefficient = 0.1929f };
            SpellData[5] = new SpellData() { Cost = (int)(0.31 * BaseMana[75]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[6] = new SpellData() { Cost = (int)(0.31 * BaseMana[76]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[7] = new SpellData() { Cost = (int)(0.31 * BaseMana[77]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[8] = new SpellData() { Cost = (int)(0.31 * BaseMana[78]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[9] = new SpellData() { Cost = (int)(0.31 * BaseMana[79]), MinDamage = 935, MaxDamage = 1085, SpellDamageCoefficient = 0.1929f };
            SpellData[10] = new SpellData() { Cost = (int)(0.31 * BaseMana[80]), MinDamage = 1101, MaxDamage = 1279, SpellDamageCoefficient = 0.1929f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public DragonsBreathTemplate(CharacterCalculationsMage calculations)
            : base("Dragon's Breath", false, true, true, 0, 0, 20, MagicSchool.Fire, GetMaxRankSpellData(calculations.CalculationOptions))
        {
            Calculate(calculations);
            AoeDamageCap = 37500;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    public class BlizzardTemplate : SpellTemplate
    {
        public static SpellData[] SpellData = new SpellData[11];
        static BlizzardTemplate()
        {
            SpellData[0] = new SpellData() { Cost = (int)(0.74 * BaseMana[70]), MinDamage = 2184, MaxDamage = 2184, SpellDamageCoefficient = 1.1429f }; // TODO verify level 70 WotLK data
            SpellData[1] = new SpellData() { Cost = (int)(0.74 * BaseMana[71]), MinDamage = 2192, MaxDamage = 2192, SpellDamageCoefficient = 1.1429f };
            SpellData[2] = new SpellData() { Cost = (int)(0.74 * BaseMana[72]), MinDamage = 2192, MaxDamage = 2192, SpellDamageCoefficient = 1.1429f };
            SpellData[3] = new SpellData() { Cost = (int)(0.74 * BaseMana[73]), MinDamage = 2200, MaxDamage = 2200, SpellDamageCoefficient = 1.1429f };
            SpellData[4] = new SpellData() { Cost = (int)(0.74 * BaseMana[74]), MinDamage = 2800, MaxDamage = 2800, SpellDamageCoefficient = 1.1429f };
            SpellData[5] = new SpellData() { Cost = (int)(0.74 * BaseMana[75]), MinDamage = 2800, MaxDamage = 2800, SpellDamageCoefficient = 1.1429f };
            SpellData[6] = new SpellData() { Cost = (int)(0.74 * BaseMana[76]), MinDamage = 2808, MaxDamage = 2808, SpellDamageCoefficient = 1.1429f };
            SpellData[7] = new SpellData() { Cost = (int)(0.74 * BaseMana[77]), MinDamage = 2808, MaxDamage = 2808, SpellDamageCoefficient = 1.1429f };
            SpellData[8] = new SpellData() { Cost = (int)(0.74 * BaseMana[78]), MinDamage = 2816, MaxDamage = 2816, SpellDamageCoefficient = 1.1429f };
            SpellData[9] = new SpellData() { Cost = (int)(0.74 * BaseMana[79]), MinDamage = 2816, MaxDamage = 2816, SpellDamageCoefficient = 1.1429f };
            SpellData[10] = new SpellData() { Cost = (int)(0.74 * BaseMana[80]), MinDamage = 3408, MaxDamage = 3408, SpellDamageCoefficient = 1.1429f };
        }
        private static SpellData GetMaxRankSpellData(CalculationOptionsMage options)
        {
            return SpellData[options.PlayerLevel - 70];
        }

        public BlizzardTemplate(CharacterCalculationsMage calculations)
            : base("Blizzard", true, false, true, 30, 8, 0, MagicSchool.Frost, GetMaxRankSpellData(calculations.CalculationOptions), 4, 1)
        {
            Calculate(calculations);
            if (calculations.MageTalents.ImprovedBlizzard > 0)
            {
                float fof = (calculations.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * calculations.MageTalents.FingersOfFrost);
                fof = Math.Max(fof, 0.05f * calculations.MageTalents.Frostbite * calculations.CalculationOptions.FrostbiteUtilization);
                BaseCritRate += (1.0f - (1.0f - fof) * (1.0f - fof)) * (calculations.MageTalents.Shatter == 3 ? 0.5f : 0.17f * calculations.MageTalents.Shatter);
                //CritRate += (1.0f - (float)Math.Pow(1 - 0.05 * castingState.MageTalents.Frostbite, 5.0 / 2.0)) * (castingState.MageTalents.Shatter == 3 ? 0.5f : 0.17f * castingState.MageTalents.Shatter);
            }
            AoeDamageCap = 200000;
            BaseCritRate += 0.02f * calculations.MageTalents.WorldInFlames;
        }
    }

    // lightning capacitor
    public class LightningBoltTemplate : SpellTemplate
    {
        public LightningBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Nature, 694, 806, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // lightning capacitor
    public class ThunderBoltTemplate : SpellTemplate
    {
        public ThunderBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightning Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Nature, 1181, 1371, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Shattered Sun Pendant of Acumen
    public class ArcaneBoltTemplate : SpellTemplate
    {
        public ArcaneBoltTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Arcane, 333, 367, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Pendulum of Telluric Currents
    public class PendulumOfTelluricCurrentsTemplate : SpellTemplate
    {
        public PendulumOfTelluricCurrentsTemplate(CharacterCalculationsMage calculations)
            : base("Pendulum of Telluric Currents", false, true, false, 0, 0, 0, 0, MagicSchool.Shadow, 1168, 1752, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    // Lightweave Embroidery
    public class LightweaveBoltTemplate : SpellTemplate
    {
        public LightweaveBoltTemplate(CharacterCalculationsMage calculations)
            : base("Lightweave Bolt", false, true, false, 0, 0, 0, 0, MagicSchool.Holy, 1000, 1200, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
        }
    }

    public class ArcaneDamageTemplate : SpellTemplate
    {
        public ArcaneDamageTemplate(CharacterCalculationsMage calculations)
            : base("Arcane Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Arcane, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusArcaneDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }

    public class FireDamageTemplate : SpellTemplate
    {
        public FireDamageTemplate(CharacterCalculationsMage calculations)
            : base("Fire Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Fire, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusFireDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }

    public class FrostDamageTemplate : SpellTemplate
    {
        public FrostDamageTemplate(CharacterCalculationsMage calculations)
            : base("Frost Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Frost, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusFrostDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }

    public class ShadowDamageTemplate : SpellTemplate
    {
        public ShadowDamageTemplate(CharacterCalculationsMage calculations)
            : base("Shadow Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Shadow, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusShadowDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }

    public class NatureDamageTemplate : SpellTemplate
    {
        public NatureDamageTemplate(CharacterCalculationsMage calculations)
            : base("Nature Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Nature, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusNatureDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }

    public class HolyDamageTemplate : SpellTemplate
    {
        public HolyDamageTemplate(CharacterCalculationsMage calculations)
            : base("Holy Damage", false, true, false, 0, 0, 0, 0, MagicSchool.Holy, 1, 1, 0, 0, 0, 0, 0, 0)
        {
            Calculate(calculations);
            CritBonus = (1 + (1.5f * (1 + calculations.BaseStats.BonusSpellCritMultiplier) - 1));
            BaseSpellModifier = calculations.BaseSpellModifier * (1 + calculations.BaseStats.BonusHolyDamageMultiplier);
            BaseCritRate = calculations.BaseCritRate;
        }
    }
}
