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

        public CalculationOptionsMage CalculationOptions { get; private set; }
        public MageTalents MageTalents { get; private set; }
        public Stats BaseStats { get; private set; }

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
        public float GlobalCooldown { get; set; }

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

        public float ArcaneSpellModifier { get { return StateSpellModifier * Calculations.BaseArcaneSpellModifier; } }
        public float FireSpellModifier { get { return StateSpellModifier * Calculations.BaseFireSpellModifier; } }
        public float FrostSpellModifier { get { return StateSpellModifier * Calculations.BaseFrostSpellModifier; } }
        public float NatureSpellModifier { get { return StateSpellModifier * Calculations.BaseNatureSpellModifier; } }
        public float ShadowSpellModifier { get { return StateSpellModifier * Calculations.BaseShadowSpellModifier; } }
        public float FrostFireSpellModifier { get { return StateSpellModifier * Calculations.BaseFrostFireSpellModifier; } }
        public float HolySpellModifier { get { return StateSpellModifier * Calculations.BaseHolySpellModifier; } }

        public float ArcaneCritBonus { get { return Calculations.BaseArcaneCritBonus; } }
        public float FireCritBonus { get { return Calculations.BaseFireCritBonus; } }
        public float FrostCritBonus { get { return Calculations.BaseFrostCritBonus; } }
        public float NatureCritBonus { get { return Calculations.BaseNatureCritBonus; } }
        public float ShadowCritBonus { get { return Calculations.BaseShadowCritBonus; } }
        public float FrostFireCritBonus { get { return Calculations.BaseFrostFireCritBonus; } }
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

        public float ResilienceCritDamageReduction { get; set; }
        public float ResilienceCritRateReduction { get; set; }

        public float Latency
        {
            get
            {
                return CalculationOptions.Latency;
            }
        }

        public float SnaredTime { get; set; }

        public bool GetCooldown(Cooldown cooldown)
        {
            return (cooldown & Cooldown) == cooldown;
        }

        public bool Evocation { get { return GetCooldown(Cooldown.Evocation); } }
        public bool ArcanePower { get { return GetCooldown(Cooldown.ArcanePower); } }
        public bool IcyVeins { get { return GetCooldown(Cooldown.IcyVeins); } }
        public bool MoltenFury { get { return GetCooldown(Cooldown.MoltenFury); } }
        public bool Heroism { get { return GetCooldown(Cooldown.Heroism); } }
        public bool PotionOfWildMagic { get { return GetCooldown(Cooldown.PotionOfWildMagic); } }
        public bool PotionOfSpeed { get { return GetCooldown(Cooldown.PotionOfSpeed); } }
        public bool FlameCap { get { return GetCooldown(Cooldown.FlameCap); } }
        public bool Trinket1 { get { return GetCooldown(Cooldown.Trinket1); } }
        public bool Trinket2 { get { return GetCooldown(Cooldown.Trinket2); } }
        public bool ManaGemEffect { get { return GetCooldown(Cooldown.ManaGemEffect); } }
        public bool DrumsOfBattle { get { return GetCooldown(Cooldown.DrumsOfBattle); } }
        public bool Combustion { get { return GetCooldown(Cooldown.Combustion); } }
        public bool WaterElemental { get { return GetCooldown(Cooldown.WaterElemental); } }
        public bool PowerInfusion { get { return GetCooldown(Cooldown.PowerInfusion); } }
        public bool Frozen { get; set; }
        public string MageArmor { get; set; }

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
                        maintainSnareState.Spells = new Dictionary<int, Spell>();
                        maintainSnareState.Cycles = new Dictionary<int, Cycle>();
                        maintainSnareState.Spells[(int)SpellId.Wand] = Spells[(int)SpellId.Wand];
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
                        frozenState = new CastingState(Calculations, MageArmor, ArcanePower, MoltenFury, IcyVeins, Heroism, PotionOfWildMagic, PotionOfSpeed, FlameCap, Trinket1, Trinket2, Combustion, DrumsOfBattle, WaterElemental, ManaGemEffect, PowerInfusion, true);
                    }
                }
                return frozenState;
            }
        }

        public CastingState(CharacterCalculationsMage calculations, string armor, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool potionOfWildMagic, bool potionOfSpeed, bool flameCap, bool trinket1, bool trinket2, bool combustion, bool drums, bool waterElemental, bool manaGemEffect, bool powerInfusion, bool frozen)
        {
            MageTalents = calculations.MageTalents;
            BaseStats = calculations.BaseStats; // == characterStats
            CalculationOptions = calculations.CalculationOptions;
            Character character = calculations.Character;
            this.Calculations = calculations;

            float levelScalingFactor = CalculationOptions.LevelScalingFactor;

            SnaredTime = CalculationOptions.SnaredTime;
            if (CalculationOptions.MaintainSnare) SnaredTime = 1.0f;

            float stateCritRating = 0.0f;
            SpellHasteRating = BaseStats.HasteRating;

            if (potionOfWildMagic)
            {
                StateSpellPower += 200;
                stateCritRating += 200;
            }
            if (potionOfSpeed)
            {
                SpellHasteRating += 500;
            }

            if (trinket1)
            {
                Stats t = character.Trinket1.Item.Stats;
                StateSpellPower += t.SpellPowerFor20SecOnUse2Min + t.SpellPowerFor20SecOnUse5Min + t.SpellPowerFor15SecOnUse90Sec + t.SpellPowerFor15SecOnUse2Min;
                SpellHasteRating += t.HasteRatingFor20SecOnUse2Min + t.HasteRatingFor20SecOnUse5Min;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Item.Stats;
                StateSpellPower += t.SpellPowerFor20SecOnUse2Min + t.SpellPowerFor20SecOnUse5Min + t.SpellPowerFor15SecOnUse90Sec + t.SpellPowerFor15SecOnUse2Min;
                SpellHasteRating += t.HasteRatingFor20SecOnUse2Min + t.HasteRatingFor20SecOnUse5Min;
            }
            if (manaGemEffect)
            {
                StateSpellPower += BaseStats.SpellPowerFor15SecOnManaGem;
            }
            if (drums)
            {
                SpellHasteRating += 80;
            }

            CastingSpeed = 1 + SpellHasteRating / 995f * levelScalingFactor;

            StateCritRate = stateCritRating / 1400f * levelScalingFactor;
            if (frozen) StateCritRate += (MageTalents.Shatter == 3 ? 0.5f : 0.17f * MageTalents.Shatter);

            if (combustion)
            {
                CombustionDuration = ComputeCombustion(calculations.BaseFireCritRate + StateCritRate);
            }

            // spell calculations

            Frozen = frozen;
            MageArmor = armor;

            Cooldown c = Cooldown.None;
            if (arcanePower) c |= Cooldown.ArcanePower;
            if (moltenFury) c |= Cooldown.MoltenFury;
            if (icyVeins) c |= Cooldown.IcyVeins;
            if (heroism) c |= Cooldown.Heroism;
            if (potionOfWildMagic) c |= Cooldown.PotionOfWildMagic;
            if (potionOfSpeed) c |= Cooldown.PotionOfSpeed;
            if (flameCap) c |= Cooldown.FlameCap;
            if (trinket1) c |= Cooldown.Trinket1;
            if (trinket2) c |= Cooldown.Trinket2;
            if (combustion) c |= Cooldown.Combustion;
            if (drums) c |= Cooldown.DrumsOfBattle;
            if (waterElemental) c |= Cooldown.WaterElemental;
            if (manaGemEffect) c |= Cooldown.ManaGemEffect;
            if (powerInfusion) c |= Cooldown.PowerInfusion;
            Cooldown = c;

            if (icyVeins)
            {
                CastingSpeed *= 1.2f;
            }
            if (heroism)
            {
                CastingSpeed *= 1.3f;
            }
            else if (powerInfusion)
            {
                CastingSpeed *= 1.2f;
            }
            CastingSpeed *= (1f + BaseStats.SpellHaste);
            CastingSpeed *= (1f + 0.02f * character.MageTalents.NetherwindPresence);

            GlobalCooldown = Math.Max(Spell.GlobalCooldownLimit, 1.5f / CastingSpeed);

            StateSpellModifier = 1.0f;
            if (arcanePower)
            {
                StateSpellModifier *= 1.2f;
            }
            if (moltenFury)
            {
                StateSpellModifier *= (1 + 0.06f * MageTalents.MoltenFury);
            }

            ResilienceCritDamageReduction = 1;
            ResilienceCritRateReduction = 0;

            if (BaseStats.LightningCapacitorProc > 0)
            {
                LightningBolt = calculations.LightningBoltTemplate.GetSpell(this);
            }
            if (BaseStats.ThunderCapacitorProc > 0)
            {
                ThunderBolt = calculations.ThunderBoltTemplate.GetSpell(this);
            }
            if (BaseStats.LightweaveEmbroideryProc > 0)
            {
                LightweaveBolt = calculations.LightweaveBoltTemplate.GetSpell(this);
            }
            if (BaseStats.ShatteredSunAcumenProc > 0 && !CalculationOptions.Aldor)
            {
                ArcaneBolt = calculations.ArcaneBoltTemplate.GetSpell(this);
            }
            if (BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                PendulumOfTelluricCurrents = calculations.PendulumOfTelluricCurrentsTemplate.GetSpell(this);
            }
        }

        public Spell ArcaneBolt { get; set; }
        public Spell LightningBolt { get; set; }
        public Spell ThunderBolt { get; set; }
        public Spell LightweaveBolt { get; set; }
        public Spell PendulumOfTelluricCurrents { get; set; }

        //private static int CycleIdCount;
        //private static int SpellIdCount;

        //static CastingState()
        //{
        //    CycleIdCount = Enum.GetValues(typeof(CycleId)).Length;
        //    SpellIdCount = Enum.GetValues(typeof(SpellId)).Length;
        //}

        //private Cycle[] Cycles = new Cycle[CycleIdCount];
        //private Spell[] Spells = new Spell[SpellIdCount];

        private Dictionary<int, Spell> Spells = new Dictionary<int, Spell>();
        private Dictionary<int, Cycle> Cycles = new Dictionary<int, Cycle>();

        public void SetSpell(SpellId spellId, Spell spell)
        {
            Spells[(int)spellId] = spell;
        }

        public Cycle GetCycle(CycleId cycleId)
        {
            //Cycle c = Cycles[(int)cycleId];
            //if (c != null) return c;
            Cycle c = null;
            if (Cycles.TryGetValue((int)cycleId, out c)) return c;

            switch (cycleId)
            {
                case CycleId.FrostboltFOF:
                    c = GetSpell(SpellId.FrostboltFOF);
                    break;
                case CycleId.Fireball:
                    c = GetSpell(SpellId.Fireball);
                    break;
                case CycleId.FBPyro:
                    c = new FBPyro(this);
                    break;
                case CycleId.FBLBPyro:
                    c = new FBLBPyro(this);
                    break;
                case CycleId.FFBLBPyro:
                    c = new FFBLBPyro(this);
                    break;
                case CycleId.FBScPyro:
                    c = new FBScPyro(this);
                    break;
                case CycleId.FFBPyro:
                    c = new FFBPyro(this);
                    break;
                case CycleId.FFBScPyro:
                    c = new FFBScPyro(this);
                    break;
                case CycleId.FFBScLBPyro:
                    c = new FFBScLBPyro(this);
                    break;
                case CycleId.FrostfireBoltFOF:
                    c = GetSpell(SpellId.FrostfireBoltFOF);
                    break;
                case CycleId.ABABarSc:
                    c = new ABABarSc(this);
                    break;
                case CycleId.ABABarCSc:
                    c = new ABABarCSc(this);
                    break;
                case CycleId.ABAMABarSc:
                    c = new ABAMABarSc(this);
                    break;
                case CycleId.AB3ABarCSc:
                    c = new AB3ABarCSc(this);
                    break;
                case CycleId.AB3AMABarSc:
                    c = new AB3AMABarSc(this);
                    break;
                case CycleId.AB3MBAMABarSc:
                    c = new AB3MBAMABarSc(this);
                    break;
                case CycleId.ArcaneBlastSpam:
                    c = GetSpell(SpellId.ArcaneBlast3);
                    break;
                case CycleId.ABarAM:
                    c = new ABarAM(this);
                    break;
                case CycleId.ABP:
                    c = new ABP(this);
                    break;
                case CycleId.ABAM:
                    c = new ABAM(this);
                    break;
                case CycleId.ABSpamMBAM:
                    c = new ABSpamMBAM(this);
                    break;
                case CycleId.ABSpam3C:
                    c = new ABSpam3C(this);
                    break;
                case CycleId.ABSpam03C:
                    c = new ABSpam03C(this);
                    break;
                case CycleId.AB2ABar3C:
                    c = new AB2ABar3C(this);
                    break;
                case CycleId.ABABar2C:
                    c = new ABABar2C(this);
                    break;
                case CycleId.ABABar2MBAM:
                    c = new ABABar2MBAM(this);
                    break;
                case CycleId.ABABar1MBAM:
                    c = new ABABar1MBAM(this);
                    break;
                case CycleId.ABABar3C:
                    c = new ABABar3C(this);
                    break;
                case CycleId.AB3ABar3MBAM:
                    c = new AB3ABar3MBAM(this);
                    break;
                case CycleId.AB3AM:
                    c = new AB3AM(this);
                    break;
                case CycleId.AB3AM2MBAM:
                    c = new AB3AM2MBAM(this);
                    break;
                case CycleId.AB2ABar2MBAM:
                    c = new AB2ABar2MBAM(this);
                    break;
                case CycleId.ABABar0MBAM:
                    c = new ABABar0MBAM(this);
                    break;
                case CycleId.ABABar:
                    c = new ABABar(this);
                    break;
                case CycleId.ABSpam3MBAM:
                    c = new ABSpam3MBAM(this);
                    break;
                case CycleId.ABAMABar:
                    c = new ABAMABar(this);
                    break;
                case CycleId.AB2AMABar:
                    c = new AB2AMABar(this);
                    break;
                case CycleId.AB3AMABar:
                    c = new AB3AMABar(this);
                    break;
                case CycleId.AB3AMABar2C:
                    c = new AB3AMABar2C(this);
                    break;
                case CycleId.AB32AMABar:
                    c = new AB32AMABar(this);
                    break;
                case CycleId.AB3ABar3C:
                    c = new AB3ABar3C(this);
                    break;
                case CycleId.ABABar0C:
                    c = new ABABar0C(this);
                    break;
                case CycleId.ABABar1C:
                    c = new ABABar1C(this);
                    break;
                case CycleId.ABABarY:
                    c = new ABABarY(this);
                    break;
                case CycleId.AB2ABar:
                    c = new AB2ABar(this);
                    break;
                case CycleId.AB2ABar2C:
                    c = new AB2ABar2C(this);
                    break;
                case CycleId.AB2ABarMBAM:
                    c = new AB2ABarMBAM(this);
                    break;
                case CycleId.AB3ABar:
                    c = new AB3ABar(this);
                    break;
                case CycleId.AB3ABarX:
                    c = new AB3ABarX(this);
                    break;
                case CycleId.AB3ABarY:
                    c = new AB3ABarY(this);
                    break;
                case CycleId.FBABar:
                    c = new FBABar(this);
                    break;
                case CycleId.FrBABar:
                    c = new FrBABar(this);
                    break;
                case CycleId.FFBABar:
                    c = new FFBABar(this);
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
                    c = new FrBFBIL(this);
                    break;
                case CycleId.FrBFB:
                    c = new FrBFB(this);
                    break;
                case CycleId.FBScLBPyro:
                    c = new FBScLBPyro(this);
                    break;
                case CycleId.FB2ABar:
                    c = new FB2ABar(this);
                    break;
                case CycleId.FrB2ABar:
                    c = new FrB2ABar(this);
                    break;
                case CycleId.ScLBPyro:
                    c = new ScLBPyro(this);
                    break;
                case CycleId.ABABarSlow:
                    c = new ABABarSlow(this);
                    break;
                case CycleId.FBABarSlow:
                    c = new FBABarSlow(this);
                    break;
                case CycleId.FrBABarSlow:
                    c = new FrBABarSlow(this);
                    break;
                case CycleId.CustomSpellMix:
                    c = new SpellCustomMix(this);
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
                Cycles[(int)cycleId] = c;
            }

            return c;
        }

        public Spell GetSpell(SpellId spellId)
        {
            //Spell s = Spells[(int)spellId];
            //if (s != null) return s;
            Spell s = null;
            if (Spells.TryGetValue((int)spellId, out s)) return s;

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
                    s = Calculations.PyroblastTemplate.GetSpell(this, false);
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
                case SpellId.ArcaneBlast3:
                    s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3);
                    break;
                case SpellId.ArcaneBlast3NoCC:
                    s = Calculations.ArcaneBlastTemplate.GetSpell(this, 3, true, false, false);
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
                    s = Calculations.PyroblastTemplate.GetSpell(this, true);
                    break;
                case SpellId.LivingBomb:
                    s = Calculations.LivingBombTemplate.GetSpell(this);
                    break;
            }
            if (s != null)
            {
                Spells[(int)spellId] = s;
            }

            return s;
        }
    }
}
