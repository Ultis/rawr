using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    [Flags]
    public enum Cooldown
    {
        Evocation = 0x4000, // should always be the highest value
        PowerInfusion = 0x2000,
        PotionOfSpeed = 0x1000,
        ArcanePower = 0x800,
        Combustion = 0x400,
        PotionOfWildMagic = 0x200,
        DrumsOfBattle = 0x100,
        FlameCap = 0x80,
        Heroism = 0x40,
        IcyVeins = 0x20,
        MoltenFury = 0x10,
        WaterElemental = 0x8,
        ManaGemEffect = 0x4,
        Trinket1 = 0x2,
        Trinket2 = 0x1,
        None = 0x0,
        NonItemBasedMask = 0x3FF8,
        ItemBasedMask = 0x7,
        Mask = ItemBasedMask | NonItemBasedMask,
        FullMask = Mask | Evocation,
    }

    public sealed class CastingState
    {
        public CharacterCalculationsMage Calculations { get; private set; }

        public CalculationOptionsMage CalculationOptions { get { return Calculations.CalculationOptions; } }
        public MageTalents MageTalents { get { return Calculations.MageTalents; } }
        public Stats BaseStats { get { return Calculations.BaseStats; } }

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

        public bool GetCooldown(Cooldown cooldown)
        {
            return (cooldown & Cooldown) == cooldown;
        }

        public bool Evocation { get { return (Cooldown & Cooldown.Evocation) != 0; } }
        public bool ArcanePower { get { return (Cooldown & Cooldown.ArcanePower) != 0; } }
        public bool IcyVeins { get { return (Cooldown & Cooldown.IcyVeins) != 0; } }
        public bool MoltenFury { get { return (Cooldown & Cooldown.MoltenFury) != 0; } }
        public bool Heroism { get { return (Cooldown & Cooldown.Heroism) != 0; } }
        public bool PotionOfWildMagic { get { return (Cooldown & Cooldown.PotionOfWildMagic) != 0; } }
        public bool PotionOfSpeed { get { return (Cooldown & Cooldown.PotionOfSpeed) != 0; } }
        public bool FlameCap { get { return (Cooldown & Cooldown.FlameCap) != 0; } }
        public bool Trinket1 { get { return (Cooldown & Cooldown.Trinket1) != 0; } }
        public bool Trinket2 { get { return (Cooldown & Cooldown.Trinket2) != 0; } }
        public bool ManaGemEffect { get { return (Cooldown & Cooldown.ManaGemEffect) != 0; } }
        public bool DrumsOfBattle { get { return (Cooldown & Cooldown.DrumsOfBattle) != 0; } }
        public bool Combustion { get { return (Cooldown & Cooldown.Combustion) != 0; } }
        public bool WaterElemental { get { return (Cooldown & Cooldown.WaterElemental) != 0; } }
        public bool PowerInfusion { get { return (Cooldown & Cooldown.PowerInfusion) != 0; } }
        public bool Frozen { get; set; }

        public Cooldown Cooldown { get; set; }

        public float CombustionDuration { get; set; }
        public float SpellHasteRating { get; set; }

        private string buffLabel;
        public string BuffLabel
        {
            get
            {
                if (buffLabel == null)
                {
                    List<String> buffList = new List<string>();
                    if (MoltenFury) buffList.Add("Molten Fury");
                    if (Heroism) buffList.Add("Heroism");
                    if (IcyVeins) buffList.Add("Icy Veins");
                    if (ArcanePower) buffList.Add("Arcane Power");
                    if (Combustion) buffList.Add("Combustion");
                    if (DrumsOfBattle) buffList.Add("Drums of Battle");
                    if (FlameCap) buffList.Add("Flame Cap");
                    if (Trinket1) buffList.Add(Calculations.Trinket1Name);
                    if (Trinket2) buffList.Add(Calculations.Trinket2Name);
                    if (PotionOfWildMagic) buffList.Add("Potion of Wild Magic");
                    if (PotionOfSpeed) buffList.Add("Potion of Speed");
                    if (WaterElemental) buffList.Add("Water Elemental");
                    if (ManaGemEffect) buffList.Add("Mana Gem Effect");
                    if (PowerInfusion) buffList.Add("Power Infusion");

                    buffLabel = string.Join("+", buffList.ToArray());
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

        public CastingState Clone()
        {
            return (CastingState)MemberwiseClone();
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
                        maintainSnareState.Spells = new List<Spell>();
                        maintainSnareState.Cycles = new List<Cycle>();
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
                        frozenState = new CastingState(Calculations, Cooldown, true);
                    }
                }
                return frozenState;
            }
        }

        public CastingState(CharacterCalculationsMage calculations, Cooldown cooldown, bool frozen)
        {
            //MageTalents = calculations.MageTalents;
            //BaseStats = calculations.BaseStats; // == characterStats
            //CalculationOptions = calculations.CalculationOptions;
            Character character = calculations.Character;
            this.Calculations = calculations;

            float levelScalingFactor = CalculationOptions.LevelScalingFactor;

            SnaredTime = CalculationOptions.SnaredTime;
            if (CalculationOptions.MaintainSnare) SnaredTime = 1.0f;

            float stateCritRating = 0.0f;
            SpellHasteRating = BaseStats.HasteRating;

            Cooldown = cooldown;

            if (PotionOfWildMagic)
            {
                StateSpellPower += 200;
                stateCritRating += 200;
            }
            if (PotionOfSpeed)
            {
                SpellHasteRating += 500;
            }

            if (Trinket1)
            {
                StateSpellPower += calculations.Trinket1SpellPower;
                SpellHasteRating += calculations.Trinket1HasteRating;
            }
            if (Trinket2)
            {
                StateSpellPower += calculations.Trinket2SpellPower;
                SpellHasteRating += calculations.Trinket2HasteRating;
            }
            if (ManaGemEffect)
            {
                StateSpellPower += calculations.ManaGemEffectSpellPower;
            }
            if (DrumsOfBattle)
            {
                SpellHasteRating += 80;
            }

            CastingSpeed = (1 + SpellHasteRating / 995f * levelScalingFactor) * (1f + BaseStats.SpellHaste) * (1f + 0.02f * MageTalents.NetherwindPresence) * CalculationOptions.EffectHasteMultiplier;

            StateCritRate = stateCritRating / 1400f * levelScalingFactor;
            if (frozen) StateCritRate += (MageTalents.Shatter == 3 ? 0.5f : 0.17f * MageTalents.Shatter);

            if (Combustion)
            {
                CombustionDuration = ComputeCombustion(calculations.BaseFireCritRate + StateCritRate);
            }

            // spell calculations

            Frozen = frozen;

            if (IcyVeins)
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
        private List<Spell> Spells = new List<Spell>();
        private List<Cycle> Cycles = new List<Cycle>();

        public Cycle GetCycle(CycleId cycleId)
        {
            //Cycle c = Cycles[(int)cycleId];
            //if (c != null) return c;
            Cycle c = null;
            //if (Cycles.TryGetValue((int)cycleId, out c)) return c;
            foreach (Cycle cycle in Cycles)
            {
                if (cycle.CycleId == cycleId) return cycle;
            }

            switch (cycleId)
            {
                case CycleId.FrostboltFOF:
                    c = GetSpell(SpellId.FrostboltFOF);
                    break;
                case CycleId.Fireball:
                    c = GetSpell(SpellId.Fireball);
                    break;
                case CycleId.FBPyro:
                    c = new FBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBLBPyro:
                    c = new FBLBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBLBPyro:
                    c = new FFBLBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBScPyro:
                    c = new FBScPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBPyro:
                    c = new FFBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBScPyro:
                    c = new FFBScPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FFBScLBPyro:
                    c = new FFBScLBPyro(Calculations.NeedsDisplayCalculations, this);
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
                    c = new ABSpam04MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam024MBAM:
                    c = new ABSpam024MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam1234MBAM:
                    c = new ABSpam1234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2AM12MBAM:
                    c = new AB2AM12MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM123MBAM:
                    c = new AB3AM123MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB4AM1234MBAM:
                    c = new AB4AM1234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB4ABar1234MBAM:
                    c = new AB4ABar1234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABar123MBAM:
                    c = new AB3ABar123MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar12MBAM:
                    c = new AB2ABar12MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam0234MBAM:
                    c = new ABSpam0234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam4MBAM:
                    c = new ABSpam4MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam24MBAM:
                    c = new ABSpam24MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam234MBAM:
                    c = new ABSpam234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB4AM234MBAM:
                    c = new AB4AM234MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AM23MBAM:
                    c = new AB3AM23MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABarAM:
                    c = new ABarAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABP:
                    c = new ABP(this);
                    break;
                case CycleId.ABAM:
                    c = new ABAM(Calculations.NeedsDisplayCalculations, this);
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
                    c = new AB3AM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2AM:
                    c = new AB2AM(Calculations.NeedsDisplayCalculations, this);
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
                case CycleId.ABABar:
                    c = new ABABar(this);
                    break;
                case CycleId.ABSpam3MBAM:
                    c = new ABSpam3MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABSpam03MBAM:
                    c = new ABSpam03MBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABAMABar:
                    c = new ABAMABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2AMABar:
                    c = new AB2AMABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AMABar:
                    c = new AB3AMABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3AMABar2C:
                    c = new AB3AMABar2C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB32AMABar:
                    c = new AB32AMABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABar3C:
                    c = new AB3ABar3C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar0C:
                    c = new ABABar0C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABar1C:
                    c = new ABABar1C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABarY:
                    c = new ABABarY(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar:
                    c = new AB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABar2C:
                    c = new AB2ABar2C(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB2ABarMBAM:
                    c = new AB2ABarMBAM(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.AB3ABar:
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
                    break;
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
                case CycleId.FBSc:
                    c = new FBSc(this);
                    break;
                case CycleId.FBFBlast:
                    c = new FBFBlast(this);
                    break;
                case CycleId.FrBFBIL:
                    c = new FrBFBIL(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBILFB:
                    c = new FrBILFB(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBIL:
                    c = new FrBIL(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBFB:
                    c = new FrBFB(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBScLBPyro:
                    c = new FBScLBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FB2ABar:
                    c = new FB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrB2ABar:
                    c = new FrB2ABar(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ScLBPyro:
                    c = new ScLBPyro(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.ABABarSlow:
                    c = new ABABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FBABarSlow:
                    c = new FBABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;
                case CycleId.FrBABarSlow:
                    c = new FrBABarSlow(Calculations.NeedsDisplayCalculations, this);
                    break;
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
            if (c != null)
            {
                c.CycleId = cycleId;
                //Cycles[(int)cycleId] = c;
                Cycles.Add(c);
            }

            return c;
        }

        public Spell GetSpell(SpellId spellId)
        {
            //Spell s = Spells[(int)spellId];
            //if (s != null) return s;
            Spell s = null;
            //if (Spells.TryGetValue((int)spellId, out s)) return s;
            foreach (Spell spell in Spells)
            {
                if (spell.SpellId == spellId) return spell;
            }

            switch (spellId)
            {
                case SpellId.FrostboltFOF:
                    s = Calculations.FrostboltTemplate.GetSpell(this, false, false, false, true);
                    break;
                case SpellId.FrostfireBoltFOF:
                    s = Calculations.FrostfireBoltTemplate.GetSpell(this, false, true);
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
                case SpellId.Fireball:
                    s = Calculations.FireballTemplate.GetSpell(this, false, false);
                    break;
                case SpellId.FireballBF:
                    s = Calculations.FireballTemplate.GetSpell(this, false, true);
                    break;
                case SpellId.FrostfireBolt:
                    s = Calculations.FrostfireBoltTemplate.GetSpell(this, false, false);
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
            if (s != null)
            {
                s.SpellId = spellId;
                //Spells[(int)spellId] = s;
                Spells.Add(s);
            }

            return s;
        }
    }
}
