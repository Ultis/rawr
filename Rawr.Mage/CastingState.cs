using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    [Flags]
    public enum StandardEffect
    {
        None = 0,
        WaterElemental = 0x1,
        PowerInfusion = 0x2,
        PotionOfSpeed = 0x4,
        ArcanePower = 0x8,
        Combustion = 0x10,
        PotionOfWildMagic = 0x20,
        Berserking = 0x40,
        FlameCap = 0x80,
        Heroism = 0x100,
        IcyVeins = 0x200,
        MoltenFury = 0x400,
        Evocation = 0x800,
        ManaGemEffect = 0x1000,
        MirrorImage = 0x2000, // make sure to update shifting of item based effects if this changes (Solver.standardEffectCount)
        NonItemBasedMask = PowerInfusion | PotionOfSpeed | PotionOfSpeed | ArcanePower | Combustion | PotionOfWildMagic | Berserking | FlameCap | Heroism | IcyVeins | MoltenFury | WaterElemental | MirrorImage
    }

    public class CastingState
    {
        public CastingState ReferenceCastingState { get; set; } // state from where we take base spell calculations for adjusting

        public CharacterCalculationsMage Calculations { get; private set; }

        public CalculationOptionsMage CalculationOptions { get; set; }
        public MageTalents MageTalents { get; set; }
        public Stats BaseStats { get; set; }

        public float SpellHit { get { return Calculations.BaseSpellHit; } }
        public float ArcaneHitRate { get { return Calculations.BaseArcaneHitRate; } }
        public float FireHitRate { get { return Calculations.BaseFireHitRate; } }
        public float FrostHitRate { get { return Calculations.BaseFrostHitRate; } }
        public float NatureHitRate { get { return Calculations.BaseNatureHitRate; } }
        public float ShadowHitRate { get { return Calculations.BaseShadowHitRate; } }
        public float FrostFireHitRate { get { return Calculations.BaseFrostFireHitRate; } }
        public float HolyHitRate { get { return Calculations.BaseHolyHitRate; } }

        public float ArcaneThreatMultiplier { get { return Calculations.ArcaneThreatMultiplier; } }
        public float FireThreatMultiplier { get { return Calculations.FireThreatMultiplier; } }
        public float FrostThreatMultiplier { get { return Calculations.FrostThreatMultiplier; } }
        public float NatureThreatMultiplier { get { return Calculations.NatureThreatMultiplier; } }
        public float ShadowThreatMultiplier { get { return Calculations.ShadowThreatMultiplier; } }
        public float FrostFireThreatMultiplier { get { return Calculations.FrostFireThreatMultiplier; } }
        public float HolyThreatMultiplier { get { return Calculations.HolyThreatMultiplier; } }

        public float CastingSpeed { get; set; }

        public float StateSpellPower { get; set; }

        public float ArcaneSpellPower { get { return Calculations.BaseArcaneSpellPower + StateSpellPower; } }
        public float FireSpellPower { get { return Calculations.BaseFireSpellPower + StateSpellPower + (FlameCap ? 80.0f : 0.0f); } }
        public float FrostSpellPower { get { return Calculations.BaseFrostSpellPower + StateSpellPower; } }
        public float NatureSpellPower { get { return Calculations.BaseNatureSpellPower + StateSpellPower; } }
        public float ShadowSpellPower { get { return Calculations.BaseShadowSpellPower + StateSpellPower; } }
        public float FrostFireSpellPower { get { return Math.Max(FrostSpellPower, FireSpellPower); } }
        public float HolySpellPower { get { return Calculations.BaseHolySpellPower + StateSpellPower; } }

        public float SpiritRegen { get { return Calculations.SpiritRegen; } }
        public float ManaRegen { get { return Calculations.ManaRegen; } }
        public float ManaRegen5SR { get { return Calculations.ManaRegen5SR; } }
        public float ManaRegenDrinking { get { return Calculations.ManaRegenDrinking; } }
        public float HealthRegen { get { return Calculations.HealthRegen; } }
        public float HealthRegenCombat { get { return Calculations.HealthRegenCombat; } }
        public float HealthRegenEating { get { return Calculations.HealthRegenEating; } }
        public float MeleeMitigation { get { return Calculations.MeleeMitigation; } }
        public float Defense { get { return Calculations.Defense; } }
        public float PhysicalCritReduction { get { return Calculations.PhysicalCritReduction; } }
        public float SpellCritReduction { get { return Calculations.SpellCritReduction; } }
        public float CritDamageReduction { get { return Calculations.CritDamageReduction; } }
        public float Dodge { get { return Calculations.Dodge; } }

        public float StateSpellModifier { get; set; }
        public float StateAdditiveSpellModifier { get; set; }

        public float ArcaneCritBonus { get { return Calculations.BaseArcaneCritBonus; } }
        public float FireCritBonus { get { return Combustion ? Calculations.CombustionFireCritBonus : Calculations.BaseFireCritBonus; } }
        public float FrostCritBonus { get { return Calculations.BaseFrostCritBonus; } }
        public float NatureCritBonus { get { return Calculations.BaseNatureCritBonus; } }
        public float ShadowCritBonus { get { return Calculations.BaseShadowCritBonus; } }
        public float FrostFireCritBonus { get { return Combustion ? Calculations.CombustionFrostFireCritBonus : Calculations.BaseFrostFireCritBonus; } }
        public float HolyCritBonus { get { return Calculations.BaseHolyCritBonus; } }

        public float StateCritRate { get; set; }

        public float CritRate { get { return StateCritRate + Calculations.BaseCritRate; } }
        public float ArcaneCritRate { get { return StateCritRate + Calculations.BaseArcaneCritRate; } }
        public float FireCritRate { get { return Combustion ? 3 / CombustionDuration : StateCritRate + Calculations.BaseFireCritRate; } }
        public float FrostCritRate { get { return StateCritRate + Calculations.BaseFrostCritRate; } }
        public float NatureCritRate { get { return StateCritRate + Calculations.BaseNatureCritRate; } }
        public float ShadowCritRate { get { return StateCritRate + Calculations.BaseShadowCritRate; } }
        public float FrostFireCritRate { get { return Combustion ? 3 / CombustionDuration : StateCritRate + Calculations.BaseFrostFireCritRate; } }
        public float HolyCritRate { get { return StateCritRate + Calculations.BaseHolyCritRate; } }

        //public float ResilienceCritDamageReduction { get; set; }
        //public float ResilienceCritRateReduction { get; set; }

        public float SnaredTime { get; set; }

        public bool EffectsActive(int effects)
        {
            return (effects & Effects) == effects;
        }

        public bool Evocation { get; private set; }
        public bool ArcanePower { get; private set; }
        public bool IcyVeins { get; private set; }
        public bool MoltenFury { get; private set; }
        public bool Heroism { get; private set; }
        public bool PotionOfWildMagic { get; private set; }
        public bool PotionOfSpeed { get; private set; }
        public bool FlameCap { get; private set; }
        public bool ManaGemEffect { get; private set; }
        public bool Berserking { get; private set; }
        public bool Combustion { get; private set; }
        public bool WaterElemental { get; private set; }
        public bool MirrorImage { get; private set; }
        public bool PowerInfusion { get; private set; }
        public bool Frozen { get; set; }
        public bool UseFireWard { get; set; }
        public bool UseFrostWard { get; set; }

        private int effects;
        public int Effects
        {
            get
            {
                return effects;
            }
            set
            {
                effects = value;
                Evocation = (value & (int)StandardEffect.Evocation) != 0;
                ArcanePower = (value & (int)StandardEffect.ArcanePower) != 0;
                IcyVeins = (value & (int)StandardEffect.IcyVeins) != 0;
                MoltenFury = (value & (int)StandardEffect.MoltenFury) != 0;
                Heroism = (value & (int)StandardEffect.Heroism) != 0;
                PotionOfWildMagic = (value & (int)StandardEffect.PotionOfWildMagic) != 0;
                PotionOfSpeed = (value & (int)StandardEffect.PotionOfSpeed) != 0;
                FlameCap = (value & (int)StandardEffect.FlameCap) != 0;
                ManaGemEffect = (value & (int)StandardEffect.ManaGemEffect) != 0;
                Berserking = (value & (int)StandardEffect.Berserking) != 0;
                Combustion = (value & (int)StandardEffect.Combustion) != 0;
                WaterElemental = (value & (int)StandardEffect.WaterElemental) != 0;
                MirrorImage = (value & (int)StandardEffect.MirrorImage) != 0;
                PowerInfusion = (value & (int)StandardEffect.PowerInfusion) != 0;
            }
        }

        public float CombustionDuration { get; set; }
        public float SpellHasteRating { get; set; }
        public float ProcHasteRating { get; set; }

        private string buffLabel;
        public string BuffLabel
        {
            get
            {
                if (buffLabel == null)
                {
                    buffLabel = Calculations.EffectsDescription(Effects);
                }
                return buffLabel;
            }
        }

        public override string ToString()
        {
            return BuffLabel;
        }

        private static float ComputeCombustion(float critRate)
        {
            float c0 = 1, c1 = 0, c2 = 0, c3 = 0;
            float duration = 0;

            for (int cast = 1; cast <= 13; cast++)
            {
                c3 = critRate * c2;
                c2 = c2 * (1 - critRate) + c1 * critRate;
                c1 = c1 * (1 - critRate) + c0 * critRate;
                c0 = c0 * (1 - critRate);
                critRate = Math.Min(critRate + 0.1f, 1f);
                duration += c3 * cast;
            }
            return duration;
        }

        private CastingState maintainSnareState;
        public CastingState MaintainSnareState
        {
            get
            {
                if (maintainSnareState == null)
                {
                    if (SnaredTime == 1.0f)
                    {
                        maintainSnareState = this;
                    }
                    else
                    {
                        maintainSnareState = (CastingState)MemberwiseClone();
                        //maintainSnareState.Spells = new Spell[SpellIdCount];
                        //maintainSnareState.Cycles = new Cycle[CycleIdCount];
                        //maintainSnareState.Spells = new Dictionary<int, Spell>();
                        //maintainSnareState.Cycles = new Dictionary<int, Cycle>();
                        maintainSnareState.Spells = new Spell[8];
                        maintainSnareState.Cycles = new Cycle[8];
                        maintainSnareState.SnaredTime = 1.0f;
                    }
                }
                return maintainSnareState;
            }
        }

        private CastingState frozenState;
        public CastingState FrozenState
        {
            get
            {
                if (frozenState == null)
                {
                    if (Frozen)
                    {
                        frozenState = this;
                    }
                    else
                    {
                        frozenState = CastingState.New(Calculations, Effects, true, ProcHasteRating);
                    }
                }
                return frozenState;
            }
        }

        private CastingState tier10TwoPieceState;
        public CastingState Tier10TwoPieceState
        {
            get
            {
                if (tier10TwoPieceState == null)
                {
                    tier10TwoPieceState = CastingState.New(Calculations, Effects, Frozen, ProcHasteRating);
                    tier10TwoPieceState.CastingSpeed *= 1.12f;
                    tier10TwoPieceState.ReferenceCastingState = this;
                }
                return tier10TwoPieceState;
            }
        }

        private CastingState[] HasteProcs { get; set; }

        public CastingState()
        {
        }

        public CastingState(CharacterCalculationsMage calculations, int effects, bool frozen, float procHasteRating)
        {
            Initialize(calculations, effects, frozen, procHasteRating);
        }

        public static CastingState New(CharacterCalculationsMage calculations, int effects, bool frozen, float procHasteRating)
        {
            CastingState state;
            if (calculations.NeedsDisplayCalculations || calculations.ArraySet == null)
            {
                state = new CastingState();
            }
            else
            {
                state = calculations.ArraySet.NewCastingState();
            }
            state.Initialize(calculations, effects, frozen, procHasteRating);
            return state;
        }

        public static CastingState NewRaw(CharacterCalculationsMage calculations, int effects)
        {
            CastingState state = calculations.NeedsDisplayCalculations ? new CastingState() : calculations.ArraySet.NewCastingState();
            state.Calculations = calculations;
            state.ReferenceCastingState = null;
            state.UseFireWard = false;
            state.UseFrostWard = false;
            state.Effects = effects;
            state.buffLabel = null;
            state.SpellsCount = 0;
            state.CyclesCount = 0;
            state.ProcHasteRating = 0;
            return state;
        }

        public void Initialize(CharacterCalculationsMage calculations, int effects, bool frozen, float procHasteRating)
        {
            frozenState = null;
            UseFireWard = false;
            UseFrostWard = false;
            maintainSnareState = null;
            tier10TwoPieceState = null;
            ReferenceCastingState = null;            

            StateSpellPower = 0;
            StateAdditiveSpellModifier = 0;
            buffLabel = null;

            //MageTalents = calculations.MageTalents;
            //BaseStats = calculations.BaseStats; // == characterStats
            //CalculationOptions = calculations.CalculationOptions;
            Character character = calculations.Character;
            Calculations = calculations;
            CalculationOptions = calculations.CalculationOptions;
            MageTalents = calculations.MageTalents;
            BaseStats = calculations.BaseStats;

            HasteProcs = null;

            float levelScalingFactor = CalculationOptions.LevelScalingFactor;

            SnaredTime = CalculationOptions.SnaredTime;
            if (CalculationOptions.MaintainSnare) SnaredTime = 1.0f;

            float stateCritRating = 0.0f;
            ProcHasteRating = procHasteRating;
            SpellHasteRating = BaseStats.HasteRating + procHasteRating;

            Effects = effects;

            if (PotionOfWildMagic)
            {
                StateSpellPower += 200;
                stateCritRating += 200;
            }
            if (PotionOfSpeed)
            {
                SpellHasteRating += 500;
            }

            List<EffectCooldown> cooldownList = calculations.CooldownList;
            for (int i = 0; i < cooldownList.Count; i++)
            {
                EffectCooldown effect = cooldownList[i];
                if (effect.SpecialEffect != null && (effects & effect.Mask) == effect.Mask)
                {
                    StateSpellPower += effect.SpecialEffect.Stats.SpellPower;
                    SpellHasteRating += effect.SpecialEffect.Stats.HasteRating;                    
                }
            }

            CastingSpeed = (1 + SpellHasteRating / 1000f * levelScalingFactor) * (1f + BaseStats.SpellHaste) * (1f + 0.02f * MageTalents.NetherwindPresence) * CalculationOptions.EffectHasteMultiplier;

            StateCritRate = stateCritRating / 1400f * levelScalingFactor;
            if (frozen) StateCritRate += (MageTalents.Shatter == 3 ? 0.5f : 0.17f * MageTalents.Shatter);

            if (Combustion)
            {
                CombustionDuration = ComputeCombustion(calculations.BaseFireCritRate + StateCritRate);
            }
            else
            {
                CombustionDuration = 0;
            }

            // spell calculations

            Frozen = frozen;

            if (IcyVeins)
            {
                CastingSpeed *= 1.2f;
            }
            if (Berserking)
            {
                CastingSpeed *= 1.2f;
            }
            if (Heroism)
            {
                CastingSpeed *= 1.3f;
            }
            else if (PowerInfusion)
            {
                CastingSpeed *= 1.2f;
            }

            StateSpellModifier = 1.0f;
            if (ArcanePower)
            {
                StateAdditiveSpellModifier += 0.2f;
            }
            if (MoltenFury)
            {
                StateSpellModifier *= (1 + 0.06f * MageTalents.MoltenFury);
            }
            if (MirrorImage && BaseStats.Mage4T10 > 0)
            {
                StateSpellModifier *= 1.18f;
            }

            SpellsCount = 0;
            CyclesCount = 0;

            //ResilienceCritDamageReduction = 1;
            //ResilienceCritRateReduction = 0;
        }

        public float ArcaneBoltAverageDamage { get { return Calculations.ArcaneBoltTemplate.GetEffectAverageDamage(this); } }
        public float LightningBoltAverageDamage { get { return Calculations.LightningBoltTemplate.GetEffectAverageDamage(this); } }
        public float ThunderBoltAverageDamage { get { return Calculations.ThunderBoltTemplate.GetEffectAverageDamage(this); } }
        public float PendulumOfTelluricCurrentsAverageDamage { get { return Calculations.PendulumOfTelluricCurrentsTemplate.GetEffectAverageDamage(this); } }

        public float ArcaneAverageDamage { get { return Calculations.ArcaneDamageTemplate.GetEffectAverageDamage(this); } }
        public float FireAverageDamage { get { return Calculations.FireDamageTemplate.GetEffectAverageDamage(this); } }
        public float FrostAverageDamage { get { return Calculations.FrostDamageTemplate.GetEffectAverageDamage(this); } }
        public float ShadowAverageDamage { get { return Calculations.ShadowDamageTemplate.GetEffectAverageDamage(this); } }
        public float NatureAverageDamage { get { return Calculations.NatureDamageTemplate.GetEffectAverageDamage(this); } }
        public float HolyAverageDamage { get { return Calculations.HolyDamageTemplate.GetEffectAverageDamage(this); } }
        public float ValkyrAverageDamage { get { return Calculations.ValkyrDamageTemplate.Multiplier; } }

        //private static int CycleIdCount;
        //private static int SpellIdCount;

        //static CastingState()
        //{
        //    CycleIdCount = Enum.GetValues(typeof(CycleId)).Length;
        //    SpellIdCount = Enum.GetValues(typeof(SpellId)).Length;
        //}

        //private Cycle[] Cycles = new Cycle[CycleIdCount];
        //private Spell[] Spells = new Spell[SpellIdCount];

        //private Dictionary<int, Spell> Spells = new Dictionary<int, Spell>(7);
        //private Dictionary<int, Cycle> Cycles = new Dictionary<int, Cycle>(7);

        // typical sizes are below 10, so it is more efficient to just have a list
        // and look through the entries already stored for a match
        private Spell[] Spells = new Spell[8];
        private int SpellsCount;
        private Cycle[] Cycles = new Cycle[8];
        private int CyclesCount;

        public Cycle GetCycle(CycleId cycleId)
        {
            //Cycle c = Cycles[(int)cycleId];
            //if (c != null) return c;
            Cycle c = null;
            //if (Cycles.TryGetValue((int)cycleId, out c)) return c;
            for (int i = 0; i < CyclesCount; i++)
            {
                Cycle cycle = Cycles[i];
                if (cycle.CycleId == cycleId) return cycle;
            }

            if (CalculationOptions.AdvancedHasteProcs)
            {
                c = GetAveragedHasteCycle(cycleId);
            }

            if (c == null)
            {
                c = GetNewCycle(cycleId);
            }
            if (c != null)
            {
                if (UseFireWard)
                {
                    c = FireWardCycle.GetCycle(Calculations.NeedsDisplayCalculations, this, c);
                }
                else if (UseFrostWard)
                {
                    c = FrostWardCycle.GetCycle(Calculations.NeedsDisplayCalculations, this, c);
                }

                c.CycleId = cycleId;
                //Cycles[(int)cycleId] = c;
                //Cycles.Add(c);
                if (CyclesCount >= Cycles.Length)
                {
                    int length = 2 * Cycles.Length;
                    Cycle[] destinationArray = new Cycle[length];
                    Array.Copy(Cycles, 0, destinationArray, 0, CyclesCount);
                    Cycles = destinationArray;
                }
                Cycles[CyclesCount++] = c;
            }

            return c;
        }

        private Cycle GetAveragedHasteCycle(CycleId cycleId)
        {
            float baseProcHaste = 0;
            // construct possible proc combinations
            Cycle baseCycle = GetNewCycle(cycleId);
            // on use stacking items
            foreach (EffectCooldown effectCooldown in Calculations.StackingHasteEffectCooldowns)
            {
                if (EffectsActive(effectCooldown.Mask))
                {
                    foreach (SpecialEffect effect in effectCooldown.SpecialEffect.Stats.SpecialEffects())
                    {
                        if (effect.Stats.HasteRating > 0)
                        {
                            float procs = 0.0f;
                            switch (effect.Trigger)
                            {
                                case Trigger.DamageSpellCast:
                                case Trigger.SpellCast:
                                    procs = baseCycle.CastProcs;
                                    break;
                                case Trigger.DamageSpellCrit:
                                case Trigger.SpellCrit:
                                    procs = baseCycle.CritProcs;
                                    break;
                                case Trigger.DamageSpellHit:
                                case Trigger.SpellHit:
                                    procs = baseCycle.HitProcs;
                                    break;
                            }
                            if (procs == 0.0f) continue;
                            // until they put in some good trinkets with such effects just do a quick dirty calculation
                            if (procs > baseCycle.Ticks)
                            {
                                // some 100% on cast procs, happens because AM has 6 cast procs and only 5 ticks
                                baseProcHaste += effect.GetAverageStackSize(baseCycle.CastTime / procs, 1.0f, 3.0f, effectCooldown.SpecialEffect.Duration) * effect.Stats.HasteRating;
                            }
                            else
                            {
                                baseProcHaste += effect.GetAverageStackSize(baseCycle.CastTime / baseCycle.Ticks, procs / baseCycle.Ticks, 3.0f, effectCooldown.SpecialEffect.Duration) * effect.Stats.HasteRating;
                            }
                        }
                    }
                }
            }
            int N = Calculations.HasteRatingEffects.Length;
            if (baseProcHaste == 0 && N == 0) return baseCycle;
            float[] triggerInterval = new float[N];
            float[] triggerChance = new float[N];
            float[] offset = new float[N];
            for (int i = 0; i < N; i++)
            {
                SpecialEffect effect = Calculations.HasteRatingEffects[i];
                float procs = 0.0f;
                switch (effect.Trigger)
                {
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        procs = baseCycle.CastProcs;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        procs = baseCycle.CritProcs;
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        procs = baseCycle.HitProcs;
                        break;
                }
                triggerInterval[i] = baseCycle.CastTime / baseCycle.Ticks;
                triggerChance[i] = procs / baseCycle.Ticks;
            }
            WeightedStat[] result = SpecialEffect.GetAverageCombinedUptimeCombinations(Calculations.HasteRatingEffects, triggerInterval, triggerChance, offset, 3.0f, CalculationOptions.FightDuration, AdditiveStat.HasteRating);
            if (HasteProcs == null)
            {
                HasteProcs = new CastingState[result.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    CastingState subState = CastingState.New(Calculations, Effects, Frozen, baseProcHaste + result[i].Value);
                    subState.ReferenceCastingState = this;
                    HasteProcs[i] = subState;
                }
            }
            DynamicCycle c = DynamicCycle.New(Calculations.NeedsDisplayCalculations, this);
            c.Name = baseCycle.Name;
            for (int i = 0; i < result.Length; i++)
            {
                Cycle subCycle = null;
                if (HasteProcs[i].ProcHasteRating == 0)
                {
                    subCycle = baseCycle;
                }
                else
                {
                    subCycle = HasteProcs[i].GetNewCycle(cycleId);
                }
                c.AddCycle(Calculations.NeedsDisplayCalculations, subCycle, result[i].Chance / subCycle.CastTime);
            }
            // if base proc is not zero then the substates are different for each cycle
            if (baseProcHaste > 0)
            {
                HasteProcs = null;
            }
            return c;
        }

        private Cycle GetNewCycle(CycleId cycleId)
        {
            Cycle c = null;
            switch (cycleId)
            {
                case CycleId.FrostboltFOF:
                    c = GetSpell(SpellId.FrostboltFOF);
                    break;
                case CycleId.Fireball:
                    c = GetSpell(SpellId.Fireball);
                    break;
                case CycleId.FBPyro:
                    c = FBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBLBPyro:
                    c = FBLBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBLBPyro:
                    c = FFBLBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBScPyro:
                    c = FBScPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBPyro:
                    c = FFBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBScPyro:
                    c = FFBScPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBScLBPyro:
                    c = FFBScLBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrostfireBoltFOF:
                    c = GetSpell(SpellId.FrostfireBoltFOF);
                    break;
                case CycleId.ABABarSc:
                    c = new ABABarSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABarCSc:
                    c = new ABABarCSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABAMABarSc:
                    c = new ABAMABarSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABarCSc:
                    c = new AB3ABarCSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AMABarSc:
                    c = new AB3AMABarSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3MBAMABarSc:
                    c = new AB3MBAMABarSc(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ArcaneBlastSpam:
                    c = GetSpell(SpellId.ArcaneBlast4);
                    break;
                case CycleId.ABSpam04MBAM:
                    c = ABSpam04MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam024MBAM:
                    c = ABSpam024MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam034MBAM:
                    c = ABSpam034MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam0234MBAM:
                    c = ABSpam0234MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam4MBAM:
                    c = ABSpam4MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam24MBAM:
                    c = ABSpam24MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam234MBAM:
                    c = ABSpam234MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB4AM234MBAM:
                    c = AB4AM234MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM23MBAM:
                    c = AB3AM23MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB4AM0234MBAM:
                    c = AB4AM0234MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM023MBAM:
                    c = AB3AM023MBAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABarAM:
                    c = new ABarAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.ABP:
                    c = new ABP(this);
                    break;*/
                case CycleId.ABAM:
                    c = ABAM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpamMBAM:
                    c = new ABSpamMBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam3C:
                    c = new ABSpam3C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam03C:
                    c = new ABSpam03C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar3C:
                    c = new AB2ABar3C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar2C:
                    c = new ABABar2C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar2MBAM:
                    c = new ABABar2MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar1MBAM:
                    c = new ABABar1MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar3C:
                    c = new ABABar3C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABar3MBAM:
                    c = new AB3ABar3MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM:
                    c = AB3AM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2AM:
                    c = AB2AM.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM2MBAM:
                    c = new AB3AM2MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar2MBAM:
                    c = new AB2ABar2MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar0MBAM:
                    c = new ABABar0MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.ABABar:
                    c = new ABABar(this);
                    break;*/
                case CycleId.ABSpam3MBAM:
                    c = new ABSpam3MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam03MBAM:
                    c = new ABSpam03MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.ABAMABar:
                    c = new ABAMABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2AMABar:
                    c = new AB2AMABar(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                case CycleId.AB3AMABar:
                    c = AB3AMABar.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AMABar2C:
                    c = new AB3AMABar2C(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.AB32AMABar:
                    c = new AB32AMABar(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                case CycleId.AB3ABar3C:
                    c = new AB3ABar3C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar0C:
                    c = new ABABar0C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar1C:
                    c = new ABABar1C(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.ABABarY:
                    c = new ABABarY(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar:
                    c = new AB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                case CycleId.AB2ABar2C:
                    c = new AB2ABar2C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABarMBAM:
                    c = new AB2ABarMBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.AB3ABar:
                    c = new AB3ABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABarX:
                    c = new AB3ABarX(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABarY:
                    c = new AB3ABarY(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBABar:
                    c = new FBABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBABar:
                    c = new FrBABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBABar:
                    c = new FFBABar(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                /*case CycleId.ABAMP:
                    c = new ABAMP(this);
                    break;
                case CycleId.AB3AMSc:
                    c = new AB3AMSc(this);
                    break;
                case CycleId.ABAM3Sc:
                    c = new ABAM3Sc(this);
                    break;
                case CycleId.ABAM3Sc2:
                    c = new ABAM3Sc2(this);
                    break;
                case CycleId.ABAM3FrB:
                    c = new ABAM3FrB(this);
                    break;
                case CycleId.ABAM3FrB2:
                    c = new ABAM3FrB2(this);
                    break;
                case CycleId.ABFrB:
                    c = new ABFrB(this);
                    break;
                case CycleId.AB3FrB:
                    c = new AB3FrB(this);
                    break;
                case CycleId.ABFrB3FrB:
                    c = new ABFrB3FrB(this);
                    break;
                case CycleId.ABFrB3FrB2:
                    c = new ABFrB3FrB2(this);
                    break;
                case CycleId.ABFrB3FrBSc:
                    c = new ABFrB3FrBSc(this);
                    break;
                case CycleId.ABFB3FBSc:
                    c = new ABFB3FBSc(this);
                    break;
                case CycleId.AB3Sc:
                    c = new AB3Sc(this);
                    break;*/
                /*case CycleId.FBSc:
                    c = new FBSc(this);
                    break;
                case CycleId.FBFBlast:
                    c = new FBFBlast(this);
                    break;*/
                case CycleId.FrBFBIL:
                    c = FrBFBIL.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBDFFBIL:
                    c = FrBDFFBIL.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBILFB:
                    c = FrBILFB.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBIL:
                    c = FrBIL.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBFB:
                    c = FrBFB.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBScLBPyro:
                    c = FBScLBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.FB2ABar:
                    c = new FB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrB2ABar:
                    c = new FrB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                case CycleId.ScLBPyro:
                    c = ScLBPyro.GetCycle(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABarSlow:
                    c = new ABABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;
                /*case CycleId.FBABarSlow:
                    c = new FBABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBABarSlow:
                    c = new FrBABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;*/
                case CycleId.CustomSpellMix:
                    c = new SpellCustomMix(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ArcaneMissiles:
                    c = GetSpell(SpellId.ArcaneMissiles);
                    break;
                case CycleId.Scorch:
                    c = GetSpell(SpellId.Scorch);
                    break;
                case CycleId.ArcaneExplosion:
                    c = GetSpell(SpellId.ArcaneExplosion);
                    break;
                case CycleId.FlamestrikeSpammed:
                    c = GetSpell(SpellId.FlamestrikeSpammed);
                    break;
                case CycleId.FlamestrikeSingle:
                    c = GetSpell(SpellId.FlamestrikeSingle);
                    break;
                case CycleId.Blizzard:
                    c = GetSpell(SpellId.Blizzard);
                    break;
                case CycleId.BlastWave:
                    c = GetSpell(SpellId.BlastWave);
                    break;
                case CycleId.DragonsBreath:
                    c = GetSpell(SpellId.DragonsBreath);
                    break;
                case CycleId.ConeOfCold:
                    c = GetSpell(SpellId.ConeOfCold);
                    break;
            }
            return c;
        }

        public virtual Spell GetSpell(SpellId spellId)
        {
            //Spell s = Spells[(int)spellId];
            //if (s != null) return s;
            Spell s = null;
            //if (Spells.TryGetValue((int)spellId, out s)) return s;
            for (int i = 0; i < SpellsCount; i++)
            {
                Spell spell = Spells[i];
                if (spell.SpellId == spellId) return spell;
            }
            switch (spellId)
            {
                case SpellId.Waterbolt:
                    s = Calculations.WaterboltTemplate.GetSpell(this);
                    break;
                case SpellId.MirrorImage:
                    s = Calculations.MirrorImageTemplate.GetSpell(this);
                    break;
            }
            if (s == null && ReferenceCastingState != null)
            {
                // get base spell and recalculate cast time
                Spell reference = ReferenceCastingState.GetSpell(spellId);
                if (reference.GetType() == typeof(Spell))
                {
                    // we only do this for base spells for now, not aoe/dot/absorb variants
                    // end solution should be merging all aoe/dot/absorb into single class
                    s = Spell.NewFromReference(reference, this);
                }
            }
            if (s == null)
            {
                switch (spellId)
                {
                    case SpellId.FrostboltFOF:
                        s = Calculations.FrostboltTemplate.GetSpell(this, false, false, false, true);
                        break;
                    case SpellId.FrostfireBoltFOF:
                        s = Calculations.FrostfireBoltTemplate.GetSpell(this, false, true, false);
                        break;
                    case SpellId.ArcaneMissiles:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, 0);
                        break;
                    case SpellId.ArcaneMissiles1:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, 1);
                        break;
                    case SpellId.ArcaneMissiles2:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, 2);
                        break;
                    case SpellId.ArcaneMissiles3:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, 3);
                        break;
                    case SpellId.ArcaneMissiles4:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, 4);
                        break;
                    case SpellId.ArcaneMissilesMB:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, true, 0);
                        break;
                    case SpellId.ArcaneMissilesMB1:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, true, 1);
                        break;
                    case SpellId.ArcaneMissilesMB2:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, true, 2);
                        break;
                    case SpellId.ArcaneMissilesMB3:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, true, 3);
                        break;
                    case SpellId.ArcaneMissilesMB4:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, true, 4);
                        break;
                    case SpellId.ArcaneMissilesNoProc:
                        s = Calculations.ArcaneMissilesTemplate.GetSpell(this, false, true, false, false, 0, 5);
                        break;
                    case SpellId.Frostbolt:
                        s = Calculations.FrostboltTemplate.GetSpell(this);
                        break;
                    case SpellId.FrostboltNoCC:
                        s = Calculations.FrostboltTemplate.GetSpell(this, true, false, false, false);
                        break;
                    case SpellId.DeepFreeze:
                        s = Calculations.DeepFreezeTemplate.GetSpell(this);
                        break;
                    case SpellId.Fireball:
                        s = Calculations.FireballTemplate.GetSpell(this, false, false);
                        break;
                    case SpellId.FireballBF:
                        s = Calculations.FireballTemplate.GetSpell(this, false, true);
                        break;
                    case SpellId.FrostfireBolt:
                        s = Calculations.FrostfireBoltTemplate.GetSpell(this, false, false, false);
                        break;
                    case SpellId.FrostfireBoltFC:
                        s = Calculations.FrostfireBoltTemplate.GetSpell(this, false, false, true);
                        break;
                    case SpellId.Pyroblast:
                        s = Calculations.PyroblastTemplate.GetSpell(this, false, false);
                        break;
                    case SpellId.FireBlast:
                        s = Calculations.FireBlastTemplate.GetSpell(this);
                        break;
                    case SpellId.Scorch:
                        s = Calculations.ScorchTemplate.GetSpell(this);
                        break;
                    case SpellId.ScorchNoCC:
                        s = Calculations.ScorchTemplate.GetSpell(this, false);
                        break;
                    case SpellId.ArcaneBarrage:
                        s = Calculations.ArcaneBarrageTemplate.GetSpell(this, 0);
                        break;
                    case SpellId.ArcaneBarrage1:
                        s = Calculations.ArcaneBarrageTemplate.GetSpell(this, 1);
                        break;
                    case SpellId.ArcaneBarrage2:
                        s = Calculations.ArcaneBarrageTemplate.GetSpell(this, 2);
                        break;
                    case SpellId.ArcaneBarrage3:
                        s = Calculations.ArcaneBarrageTemplate.GetSpell(this, 3);
                        break;
                    case SpellId.ArcaneBarrage4:
                        s = Calculations.ArcaneBarrageTemplate.GetSpell(this, 4);
                        break;
                    case SpellId.ArcaneBlast3:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3);
                        break;
                    case SpellId.ArcaneBlast4:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 4);
                        break;
                    case SpellId.ArcaneBlast3NoCC:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3, true, false, false);
                        break;
                    case SpellId.ArcaneBlastRaw:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this);
                        break;
                    case SpellId.ArcaneBlast0:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 0);
                        break;
                    case SpellId.ArcaneBlast0NoCC:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 0, true, false, false);
                        break;
                    case SpellId.ArcaneBlast1:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 1);
                        break;
                    case SpellId.ArcaneBlast1NoCC:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 1, true, false, false);
                        break;
                    case SpellId.ArcaneBlast2:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 2);
                        break;
                    case SpellId.ArcaneBlast2NoCC:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 2, true, false, false);
                        break;
                    case SpellId.ArcaneBlast0Hit:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 0, true);
                        break;
                    case SpellId.ArcaneBlast1Hit:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 1, true);
                        break;
                    case SpellId.ArcaneBlast2Hit:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 2, true);
                        break;
                    case SpellId.ArcaneBlast3Hit:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3, true);
                        break;
                    case SpellId.ArcaneBlast0Miss:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 0, false);
                        break;
                    case SpellId.ArcaneBlast1Miss:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 1, false);
                        break;
                    case SpellId.ArcaneBlast2Miss:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 2, false);
                        break;
                    case SpellId.ArcaneBlast3Miss:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3, false);
                        break;
                    case SpellId.IceLance:
                        s = Calculations.IceLanceTemplate.GetSpell(this);
                        break;
                    case SpellId.ArcaneExplosion:
                        s = Calculations.ArcaneExplosionTemplate.GetSpell(this);
                        break;
                    case SpellId.FlamestrikeSpammed:
                        s = Calculations.FlamestrikeTemplate.GetSpell(this, true);
                        break;
                    case SpellId.FlamestrikeSingle:
                        s = Calculations.FlamestrikeTemplate.GetSpell(this, false);
                        break;
                    case SpellId.Blizzard:
                        s = Calculations.BlizzardTemplate.GetSpell(this);
                        break;
                    case SpellId.BlastWave:
                        s = Calculations.BlastWaveTemplate.GetSpell(this);
                        break;
                    case SpellId.DragonsBreath:
                        s = Calculations.DragonsBreathTemplate.GetSpell(this);
                        break;
                    case SpellId.ConeOfCold:
                        s = Calculations.ConeOfColdTemplate.GetSpell(this);
                        break;
                    case SpellId.ArcaneBlast0POM:
                        s = Calculations.ArcaneBlastTemplate.GetSpell(this, 0, false, false, true);
                        break;
                    case SpellId.FireballPOM:
                        s = Calculations.FireballTemplate.GetSpell(this, true, false);
                        break;
                    case SpellId.Slow:
                        s = Calculations.SlowTemplate.GetSpell(this);
                        break;
                    case SpellId.FrostboltPOM:
                        s = Calculations.FrostboltTemplate.GetSpell(this, false, false, true, false);
                        break;
                    case SpellId.PyroblastPOM:
                        s = Calculations.PyroblastTemplate.GetSpell(this, true, false);
                        break;
                    case SpellId.PyroblastPOMSpammed:
                        s = Calculations.PyroblastTemplate.GetSpell(this, true, true);
                        break;
                    case SpellId.PyroblastPOMDotUptime:
                        s = Calculations.PyroblastTemplate.GetSpell(this, true);
                        break;
                    case SpellId.LivingBomb:
                        s = Calculations.LivingBombTemplate.GetSpell(this);
                        break;
                    case SpellId.FireWard:
                        s = Calculations.FireWardTemplate.GetSpell(this);
                        break;
                    case SpellId.FrostWard:
                        s = Calculations.FrostWardTemplate.GetSpell(this);
                        break;
                }
            }

            if (s != null)
            {
                s.SpellId = spellId;
                //Spells[(int)spellId] = s;
                //Spells.Add(s);
                if (SpellsCount >= Spells.Length)
                {
                    int length = 2 * Spells.Length;
                    Spell[] destinationArray = new Spell[length];
                    Array.Copy(Spells, 0, destinationArray, 0, SpellsCount);
                    Spells = destinationArray;
                }
                Spells[SpellsCount++] = s;
            }

            return s;
        }
    }
}
