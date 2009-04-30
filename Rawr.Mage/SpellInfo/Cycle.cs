using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public enum CycleId
    {
        None,
        ArcaneMissiles,
        Scorch,
        FrostboltFOF,
        Fireball,
        FrostfireBoltFOF,
        ArcaneBlastSpam,
        ABABarSc,
        ABABarCSc,
        ABAMABarSc,
        AB3AMABarSc,
        AB3ABarCSc,
        AB3MBAMABarSc,
        ABarAM,
        ABP,
        ABAM,
        ABABar,
        ABABar3C,
        ABABar2C,
        ABABar2MBAM,
        ABABar1MBAM,
        ABABar0MBAM,
        ABSpamMBAM,
        ABSpam03C,
        ABSpam3C,
        ABSpam3MBAM,
        ABAMABar,
        AB2AMABar,
        AB32AMABar,
        AB3ABar3MBAM,
        AB3AM,
        AB3AM2MBAM,
        ABABar0C,
        ABABar1C,
        ABABarY,
        AB2ABar,
        AB2ABarMBAM,
        AB2ABar2C,
        AB2ABar2MBAM,
        AB2ABar3C,
        AB3AMABar,
        AB3AMABar2C,
        AB3ABar,
        AB3ABar3C,
        AB3ABarX,
        AB3ABarY,
        FBABar,
        FrBABar,
        FFBABar,
        //ABAMP,
        //AB3AMSc,
        //ABAM3Sc,
        //ABAM3Sc2,
        //ABAM3FrB,
        //ABAM3FrB2,
        //ABFrB,
        //AB3FrB,
        //ABFrB3FrB,
        //ABFrB3FrB2,
        //ABFrB3FrBSc,
        //ABFB3FBSc,
        //AB3Sc,
        FB2ABar,
        FrB2ABar,
        ScLBPyro,
        FrBFB,
        FrBFBIL,
        FBSc,
        FBFBlast,
        FBPyro,
        FBLBPyro,
        FFBLBPyro,
        FFBPyro,
        FBScPyro,
        FFBScPyro,
        FBScLBPyro,
        FFBScLBPyro,
        ABABarSlow,
        FBABarSlow,
        FrBABarSlow,
        /*ABAM3ScCCAM,
        ABAM3Sc2CCAM,
        ABAM3FrBCCAM,
        ABAM3FrBCCAMFail,
        ABAM3FrBScCCAM,
        ABAMCCAM,
        ABAM3CCAM,*/
        CustomSpellMix,
        ArcaneExplosion,
        FlamestrikeSpammed,
        FlamestrikeSingle,
        Blizzard,
        BlastWave,
        DragonsBreath,
        ConeOfCold
    }

    public abstract class Cycle
    {
        public string Name;
        public CycleId CycleId;

        public CastingState CastingState;

        protected Cycle(CastingState castingState)
        {
            CastingState = castingState;
        }

        private bool calculated;

        internal float damagePerSecond;
        internal float effectDamagePerSecond;
        public float DamagePerSecond
        {
            get
            {
                Calculate();
                return damagePerSecond + effectDamagePerSecond;
            }
        }

        internal float threatPerSecond;
        internal float effectThreatPerSecond;
        public float ThreatPerSecond
        {
            get
            {
                Calculate();
                return threatPerSecond + effectThreatPerSecond;
            }
        }

        internal float costPerSecond;
        public float CostPerSecond
        {
            get
            {
                Calculate();
                return costPerSecond;
            }
        }

        private float manaRegenPerSecond;
        public float ManaRegenPerSecond
        {
            get
            {
                Calculate();
                return manaRegenPerSecond;
            }
        }

        public float ManaPerSecond
        {
            get
            {
                return CostPerSecond - ManaRegenPerSecond;
            }
        }

        public float DpsPerSpellPower;

        public bool AffectedByFlameCap;
        public bool ProvidesSnare;
        public bool ProvidesScorch;

        public bool AreaEffect;
        public Spell AoeSpell;

        protected string sequence = null;
        public virtual string Sequence
        {
            get
            {
                return sequence;
            }
        }

        public float HitProcs;
        public float Ticks;
        public float CastProcs;
        public float NukeProcs;
        public float CritProcs;
        public float CastTime;
        public float TargetProcs;
        public float OO5SR = 0;

        public abstract void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration);
        public abstract void AddManaUsageContribution(Dictionary<string, float> dict, float duration);

        private void Calculate()
        {
            if (!calculated)
            {
                CalculateManaRegen();
                CalculateEffectDamage();
                calculated = true;
            }
        }

        private Spell waterbolt;

        private void CalculateEffectDamage()
        {
            Stats baseStats = CastingState.BaseStats;
            float spellPower = 0;
            if (Ticks > 0)
            {
                foreach (SpecialEffect effect in CastingState.Calculations.SpellPowerEffects)
                {
                    switch (effect.Trigger)
                    {
                        case Trigger.DamageSpellCrit:
                        case Trigger.SpellCrit:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.SpellMiss:
                            spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / Ticks, 1 - HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                        case Trigger.DamageSpellCast:
                        case Trigger.SpellCast:
                            if (CastProcs > 0)
                            {
                                spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / CastProcs, 1, 3, CastingState.CalculationOptions.FightDuration);
                            }
                            break;
                        case Trigger.MageNukeCast:
                            if (NukeProcs > 0) spellPower += effect.Stats.SpellPower * effect.GetAverageUptime(CastTime / NukeProcs, 1, 3, CastingState.CalculationOptions.FightDuration);
                            break;
                    }
                }
                if (baseStats.ShatteredSunAcumenProc > 0 && CastingState.CalculationOptions.Aldor) spellPower += 120 * 10f / (45f + CastTime / HitProcs / 0.1f);
            }
            effectDamagePerSecond += spellPower * DpsPerSpellPower;
            //effectThreatPerSecond += spellPower * TpsPerSpellPower; // do we really need more threat calculations???
            if (CastingState.WaterElemental)
            {
                waterbolt = CastingState.Calculations.WaterboltTemplate.GetSpell(CastingState, CastingState.FrostSpellPower + spellPower);
                effectDamagePerSecond += waterbolt.DamagePerSecond;
            }
            if (baseStats.LightningCapacitorProc > 0)
            {
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.LightningBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 3f * CastTime / (CritRate * TargetProcs));
            }
            if (baseStats.ThunderCapacitorProc > 0)
            {
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 4f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.ThunderBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.NatureThreatMultiplier;
                //continuous model
                //DamagePerSecond += LightningBolt.AverageDamage / (2.5f + 4f * CastTime / (CritRate * TargetProcs));
            }
            if (baseStats.ShatteredSunAcumenProc > 0 && !CastingState.CalculationOptions.Aldor)
            {
                float boltDps = CastingState.ArcaneBoltAverageDamage / (45f + CastTime / HitProcs / 0.1f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ArcaneThreatMultiplier;
            }
            if (baseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                effectDamagePerSecond += boltDps;
                effectThreatPerSecond += boltDps * CastingState.ShadowThreatMultiplier;
            }
        }

        private void CalculateManaRegen()
        {
            Stats baseStats = CastingState.BaseStats;
            manaRegenPerSecond = CastingState.ManaRegen5SR + OO5SR * (CastingState.ManaRegen - CastingState.ManaRegen5SR) + CastingState.BaseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs;
            float fight = CastingState.CalculationOptions.FightDuration;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(0, 1, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / CastProcs, 1, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                }
            }
            foreach (SpecialEffect effect in CastingState.Calculations.Mp5Effects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(0f, 1f, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            manaRegenPerSecond += effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                }
            }
            threatPerSecond += (baseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs) * 0.5f * (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);
        }

        public virtual void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            dict["Intellect/Spirit"] += duration * (CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration + OO5SR * (CastingState.SpiritRegen - CastingState.SpiritRegen * CastingState.BaseStats.SpellCombatManaRegeneration));
            dict["MP5"] += duration * CastingState.BaseStats.Mp5 / 5f;
            dict["Innervate"] += duration * ((CastingState.SpiritRegen * 4 * 20 * CastingState.CalculationOptions.Innervate / CastingState.CalculationOptions.FightDuration) * (OO5SR) + (CastingState.SpiritRegen * (5 - CastingState.BaseStats.SpellCombatManaRegeneration) * 20 * CastingState.CalculationOptions.Innervate / CastingState.CalculationOptions.FightDuration) * (1 - OO5SR));
            dict["Mana Tide"] += duration * CastingState.CalculationOptions.ManaTide * 0.24f * CastingState.BaseStats.Mana / CastingState.CalculationOptions.FightDuration;
            dict["Replenishment"] += duration * CastingState.BaseStats.ManaRestoreFromMaxManaPerSecond * CastingState.BaseStats.Mana;
            dict["Judgement of Wisdom"] += duration * CastingState.BaseStats.ManaRestoreFromBaseManaPerHit * 3268 / CastTime * HitProcs;
            float fight = CastingState.CalculationOptions.FightDuration;
            foreach (SpecialEffect effect in CastingState.Calculations.ManaRestoreEffects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(0, 1, 3, fight);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / CastProcs, 1, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, CritProcs / Ticks, 3, fight);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.ManaRestore * effect.GetAverageProcsPerSecond(CastTime / Ticks, HitProcs / Ticks, 3, fight);
                        }
                        break;
                }
            }
            foreach (SpecialEffect effect in CastingState.Calculations.Mp5Effects)
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(0f, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        if (CastProcs > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / CastProcs, 1f, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, CritProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        if (Ticks > 0)
                        {
                            dict["Other"] += duration * effect.Stats.Mp5 / 5f * effect.GetAverageUptime(CastTime / Ticks, HitProcs / Ticks, 3, CastingState.CalculationOptions.FightDuration);
                        }
                        break;
                }
            }
        }

        public void AddEffectContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            SpellContribution contrib;
            if (waterbolt != null)
            {
                if (!dict.TryGetValue(waterbolt.Name, out contrib))
                {
                    contrib = new SpellContribution() { Name = waterbolt.Name };
                    dict[waterbolt.Name] = contrib;
                }
                contrib.Hits += duration / waterbolt.CastTime;
                contrib.Damage += waterbolt.DamagePerSecond * duration;
            }
            if (CastingState.BaseStats.LightningCapacitorProc > 0)
            {
                if (!dict.TryGetValue("Lightning Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Lightning Bolt" };
                    dict["Lightning Bolt"] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 3f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.LightningBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Hits += duration / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.ThunderCapacitorProc > 0)
            {
                if (!dict.TryGetValue("Thunder Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Thunder Bolt" };
                    dict["Thunder Bolt"] = contrib;
                }
                //discrete model
                int hitsInsideCooldown = (int)(2.5f / (CastTime / Ticks));
                float avgCritsPerHit = CritProcs / Ticks * TargetProcs / HitProcs;
                float avgHitsToDischarge = 4f / avgCritsPerHit;
                if (avgHitsToDischarge < 1) avgHitsToDischarge = 1;
                float boltDps = CastingState.ThunderBoltAverageDamage / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Hits += duration / ((CastTime / Ticks) * (hitsInsideCooldown + avgHitsToDischarge));
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.ShatteredSunAcumenProc > 0 && !CastingState.CalculationOptions.Aldor)
            {
                if (!dict.TryGetValue("Arcane Bolt", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Arcane Bolt" };
                    dict["Arcane Bolt"] = contrib;
                }
                float boltDps = CastingState.ArcaneBoltAverageDamage / (45f + CastTime / HitProcs / 0.1f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.1f);
                contrib.Damage += boltDps * duration;
            }
            if (CastingState.BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                if (!dict.TryGetValue("Pendulum of Telluric Currents", out contrib))
                {
                    contrib = new SpellContribution() { Name = "Pendulum of Telluric Currents" };
                    dict["Pendulum of Telluric Currents"] = contrib;
                }
                float boltDps = CastingState.PendulumOfTelluricCurrentsAverageDamage / (45f + CastTime / HitProcs / 0.15f);
                contrib.Hits += duration / (45f + CastTime / HitProcs / 0.15f);
                contrib.Damage += boltDps * duration;
            }
        }
    }

    public class SpellCustomMix : DynamicCycle
    {
        public SpellCustomMix(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            sequence = "Custom Mix";
            Name = "Custom Mix";
            if (castingState.CalculationOptions.CustomSpellMix == null) return;
            for (int i = 0; i < castingState.CalculationOptions.CustomSpellMix.Count; i++)
            {
                SpellWeight spellWeight = castingState.CalculationOptions.CustomSpellMix[i];
                AddSpell(needsDisplayCalculations, castingState.GetSpell(spellWeight.Spell), spellWeight.Weight);
            }
            Calculate();
        }
    }

    public class StaticCycle : Cycle
    {
        public override string Sequence
        {
            get
            {
                if (sequence == null) sequence = string.Join("-", spellList.ConvertAll<string>(spell => (spell != null) ? spell.Name : "Pause").ToArray());
                return sequence;
            }
        }

        public bool recalc5SR;

        private List<Spell> spellList;
        private FSRCalc fsr;

        public StaticCycle()
            : base(null)
        {
            spellList = new List<Spell>();
        }

        public StaticCycle(int capacity)
            : base(null)
        {
            spellList = new List<Spell>(capacity);
        }

        public StaticCycle(int capacity, bool recalcFiveSecondRule)
            : base(null)
        {
            spellList = new List<Spell>(capacity);
            recalc5SR = recalcFiveSecondRule;
            if (recalc5SR)
            {
                fsr = new FSRCalc(capacity);
            }
        }

        public void AddSpell(Spell spell, CastingState castingState)
        {
            if (recalc5SR)
            {
                fsr.AddSpell(spell.CastTime - castingState.Latency, castingState.Latency, spell.Channeled);
            }
            Ticks += spell.Ticks;
            CastTime += spell.CastTime;
            NukeProcs += spell.NukeProcs;
            HitProcs += spell.HitProcs;
            CastProcs += spell.CastProcs;
            CritProcs += spell.CritProcs;
            TargetProcs += spell.TargetProcs;
            damagePerSecond += spell.DamagePerSecond * spell.CastTime;
            threatPerSecond += spell.ThreatPerSecond * spell.CastTime;
            costPerSecond += spell.CostPerSecond * spell.CastTime;
            DpsPerSpellPower += spell.DpsPerSpellPower * spell.CastTime;
            AffectedByFlameCap = AffectedByFlameCap || spell.AffectedByFlameCap;
            spellList.Add(spell);
        }

        public void AddPause(float duration)
        {
            if (recalc5SR)
            {
                fsr.AddPause(duration);
            }
            CastTime += duration;
            spellList.Add(null);
        }

        public void Calculate(CastingState castingState)
        {
            //CastTime = fsr.Duration;

            costPerSecond /= CastTime;
            damagePerSecond /= CastTime;
            threatPerSecond /= CastTime;
            DpsPerSpellPower /= CastTime;
            this.CastingState = castingState;

            if (recalc5SR)
            {
                OO5SR = fsr.CalculateOO5SR(0.02f * castingState.MageTalents.ArcaneConcentration);
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            foreach (Spell spell in spellList)
            {
                if (spell != null)
                {
                    spell.AddSpellContribution(dict, spell.CastTime * duration / CastTime);
                }
            }
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            foreach (Spell spell in spellList)
            {
                if (spell != null)
                {
                    spell.AddManaUsageContribution(dict, spell.CastTime * duration / CastTime);
                }
            }
        }
    }

    public class DynamicCycle : Cycle
    {
        private List<Cycle> Cycle;
        private List<float> Weight;

        protected DynamicCycle(bool needsDisplayCalculations, CastingState castingState)
            : base(castingState)
        {
            if (needsDisplayCalculations)
            {
                Cycle = new List<Cycle>();
                Weight = new List<float>();
            }
        }

        protected void AddCycle(bool needsDisplayCalculations, Cycle cycle, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(cycle);
                Weight.Add(weight);
            }
            CastTime += weight * cycle.CastTime;
            CastProcs += weight * cycle.CastProcs;
            NukeProcs += weight * cycle.NukeProcs;
            Ticks += weight * cycle.Ticks;
            HitProcs += weight * cycle.HitProcs;
            CritProcs += weight * cycle.CritProcs;
            TargetProcs += weight * cycle.TargetProcs;
            costPerSecond += weight * cycle.CastTime * cycle.costPerSecond;
            damagePerSecond += weight * cycle.CastTime * cycle.damagePerSecond;
            threatPerSecond += weight * cycle.CastTime * cycle.threatPerSecond;
            DpsPerSpellPower += weight * cycle.CastTime * cycle.DpsPerSpellPower;
        }

        protected void AddSpell(bool needsDisplayCalculations, Spell spell, float weight)
        {
            if (needsDisplayCalculations)
            {
                Cycle.Add(spell);
                Weight.Add(weight);
            }
            CastTime += weight * spell.CastTime;
            CastProcs += weight * spell.CastProcs;
            NukeProcs += weight * spell.NukeProcs;
            Ticks += weight * spell.Ticks;
            HitProcs += weight * spell.HitProcs;
            CritProcs += weight * spell.CritProcs;
            TargetProcs += weight * spell.TargetProcs;
            costPerSecond += weight * spell.CastTime * spell.CostPerSecond;
            damagePerSecond += weight * spell.CastTime * spell.DamagePerSecond;
            threatPerSecond += weight * spell.CastTime * spell.ThreatPerSecond;
            DpsPerSpellPower += weight * spell.CastTime * spell.DpsPerSpellPower;
        }

        protected void AddPause(float duration, float weight)
        {
            CastTime += weight * duration;
        }

        protected void Calculate()
        {
            costPerSecond /= CastTime;
            damagePerSecond /= CastTime;
            threatPerSecond /= CastTime;
            DpsPerSpellPower /= CastTime;
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            for (int i = 0; i < Cycle.Count; i++)
            {
                if (Cycle[i] != null) Cycle[i].AddSpellContribution(dict, Weight[i] * Cycle[i].CastTime / CastTime * duration);
            }
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            for (int i = 0; i < Cycle.Count; i++)
            {
                if (Cycle[i] != null) Cycle[i].AddManaUsageContribution(dict, Weight[i] * Cycle[i].CastTime / CastTime * duration);
            }
        }
    }

    public class CycleState
    {
        public List<CycleStateTransition> Transitions { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    public class CycleStateTransition
    {
        public CycleState TargetState { get; set; }
        public Cycle Cycle { get; set; }
        public Spell Spell { get; set; }
        public float Pause { get; set; }
        public virtual float TransitionProbability { get; set; }
    }

    public class GenericCycle : DynamicCycle
    {
        public List<CycleState> StateList;
        public double[] StateWeight;
        Dictionary<Spell, double> SpellWeight = new Dictionary<Spell, double>();
        Dictionary<Cycle, double> CycleWeight = new Dictionary<Cycle, double>();
        public string SpellDistribution;

        public GenericCycle(string name, CastingState castingState)
            : base(false, castingState)
        {
            Name = name;
        }

        public unsafe void SetStateDescription(List<CycleState> stateDescription)
        {
            StateList = stateDescription;
            for (int i = 0; i < StateList.Count; i++)
            {
                StateList[i].Index = i;
            }

            int size = stateDescription.Count;

            ArraySet arraySet = ArrayPool.RequestArraySet(size, size);
            LU M = new LU(size, arraySet);

            StateWeight = new double[size];

            fixed (double* U = arraySet.LU_U, x = StateWeight)
            fixed (double* sL = arraySet.LUsparseL, column = arraySet.LUcolumn, column2 = arraySet.LUcolumn2)
            fixed (int* P = arraySet.LU_P, Q = arraySet.LU_Q, LJ = arraySet.LU_LJ, sLI = arraySet.LUsparseLI, sLstart = arraySet.LUsparseLstart)
            {
                M.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);

                for (int replace = size - 1; replace >= size - 1; replace--)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            U[i * size + j] = 0;
                        }
                    }

                    //U[i * rows + j]

                    foreach (CycleState state in StateList)
                    {
                        foreach (CycleStateTransition transition in state.Transitions)
                        {
                            U[transition.TargetState.Index * size + state.Index] = transition.TransitionProbability;
                        }
                    }

                    // the above system is singular, "guess" which one is dependent and replace with sum=1
                    // since not all states are used always we'll get a singular system anyway sometimes, but in those cases the FSolve should still work ok on the nonsingular part
                    for (int i = 0; i < size; i++) x[i] = 0;

                    if (replace < size)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            U[replace * size + i] = 1;
                        }

                        x[replace] = 1;
                    }

                    M.Decompose();
                    if (!M.Singular) break;
                }
                M.FSolve(x);

                M.EndUnsafe();
            }

            SpellWeight = new Dictionary<Spell, double>();
            CycleWeight = new Dictionary<Cycle, double>();

            foreach (CycleState state in StateList)
            {
                foreach (CycleStateTransition transition in state.Transitions)
                {
                    if (transition.Spell != null)
                    {
                        double weight;
                        SpellWeight.TryGetValue(transition.Spell, out weight);
                        SpellWeight[transition.Spell] = weight + StateWeight[state.Index] * transition.TransitionProbability;
                    }
                    if (transition.Cycle != null)
                    {
                        double weight;
                        CycleWeight.TryGetValue(transition.Cycle, out weight);
                        CycleWeight[transition.Cycle] = weight + StateWeight[state.Index] * transition.TransitionProbability;
                    }
                    if (transition.Pause > 0)
                    {
                        AddPause(transition.Pause, (float)(StateWeight[state.Index] * transition.TransitionProbability));
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Spell, double> kvp in SpellWeight)
            {
                AddSpell(false, kvp.Key, (float)kvp.Value);
                if (kvp.Value > 0) sb.AppendFormat("{0}:\t{1:F}%\r\n", kvp.Key.Name, 100.0 * kvp.Value);
            }
            foreach (KeyValuePair<Cycle, double> kvp in CycleWeight)
            {
                AddCycle(false, kvp.Key, (float)kvp.Value);
                if (kvp.Value > 0) sb.AppendFormat("{0}:\t{1:F}%\r\n", kvp.Key.Name, 100.0 * kvp.Value);
            }

            Calculate();
           
            SpellDistribution = sb.ToString();
            ArrayPool.ReleaseArraySet(arraySet);
        }
    }

    public class CycleControlledStateTransition : CycleStateTransition
    {
        private float rawProbability;
        private int controlIndex;
        private int controlValue;
        private int[] controlStates;

        public void SetControls(int controlIndex, int[] controlStates, int controlValue)
        {
            this.controlIndex = controlIndex;
            this.controlStates = controlStates;
            this.controlValue = controlValue;
        }

        public override float TransitionProbability
        {
            get
            {
                return (controlStates[controlIndex] == controlValue) ? rawProbability : 0.0f;
            }
            set
            {
                rawProbability = value;
            }
        }
    }

    public abstract class GenerativeCycle : GenericCycle
    {
        public int[] ControlOptions;
        public int[] ControlValue;
        public int[] ControlIndex;

        public GenerativeCycle(string name, CastingState castingState)
            : base(name, castingState)
        {
        }

        public void GenerateStateDescription()
        {
            List<CycleState> remainingStates = new List<CycleState>();
            List<CycleState> processedStates = new List<CycleState>();
            remainingStates.Add(GetInitialState());

            while (remainingStates.Count > 0)
            {
                CycleState state = remainingStates[remainingStates.Count - 1];
                remainingStates.RemoveAt(remainingStates.Count - 1);

                List<CycleControlledStateTransition> transitions = GetStateTransitions(state);
                state.Transitions = transitions.ConvertAll(transition => (CycleStateTransition)transition);
                foreach (CycleControlledStateTransition transition in transitions)
                {
                    if (transition.TargetState != state && !processedStates.Contains(transition.TargetState) && !remainingStates.Contains(transition.TargetState))
                    {
                        remainingStates.Add(transition.TargetState);
                    }
                }

                processedStates.Add(state);
            }

            StateList = processedStates;
            for (int i = 0; i < StateList.Count; i++)
            {
                StateList[i].Index = i;
            }

            ControlIndex = new int[StateList.Count];
            List<CycleState> controlledStates = new List<CycleState>();
            foreach (CycleState state in StateList)
            {
                int controlIndex = -1;
                foreach (CycleState controlledState in controlledStates)
                {
                    if (!CanStatesBeDistinguished(state, controlledState))
                    {
                        controlIndex = ControlIndex[controlledState.Index];
                        break;
                    }
                }
                if (controlIndex == -1)
                {
                    controlIndex = controlledStates.Count;
                    controlledStates.Add(state);
                }
                ControlIndex[state.Index] = controlIndex;
            }

            ControlOptions = new int[controlledStates.Count];
            ControlValue = new int[controlledStates.Count];

            Dictionary<string, int>[] spellMap = new Dictionary<string, int>[controlledStates.Count];

            foreach (CycleState state in StateList)
            {
                int controlIndex = ControlIndex[state.Index];
                if (spellMap[controlIndex] == null)
                {
                    spellMap[controlIndex] = new Dictionary<string, int>();
                }
                foreach (CycleControlledStateTransition transition in state.Transitions)
                {
                    string n;
                    if (transition.Spell != null)
                    {
                        n = transition.Spell.Name;
                    }
                    else
                    {
                        n = "Pause";
                    }
                    int controlValue;
                    if (!spellMap[controlIndex].TryGetValue(n, out controlValue))
                    {
                        controlValue = spellMap[controlIndex].Keys.Count;
                        spellMap[controlIndex][n] = controlValue;
                    }
                    transition.SetControls(controlIndex, ControlValue, controlValue);
                }
            }
        }

        protected abstract CycleState GetInitialState();
        // the transition probabilities should be set as given the spell/pause is executed 100%
        // the transitions should all be spell transitions and at most one can be a state changing pause
        protected abstract List<CycleControlledStateTransition> GetStateTransitions(CycleState state);
        // the states must form equivalence classes
        protected abstract bool CanStatesBeDistinguished(CycleState state1, CycleState state2);
    }
}
