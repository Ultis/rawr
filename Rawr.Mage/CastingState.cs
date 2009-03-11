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
        private CharacterCalculationsMage calculations;

        public CalculationOptionsMage CalculationOptions { get; private set; }
        public MageTalents MageTalents { get; private set; }
        public Stats BaseStats { get; private set; }

        public float SpellCrit { get; set; }
        public float SpellHit { get; set; }
        public float CastingSpeed { get; set; }
        public float GlobalCooldown { get; set; }
        public float GlobalCooldownLimit { get; set; }

        public float ArcaneDamage { get; set; }
        public float FireDamage { get; set; }
        public float FrostDamage { get; set; }
        public float NatureDamage { get; set; }
        public float ShadowDamage { get; set; }
        public float FrostFireDamage { get; set; }
        public float HolyDamage { get; set; }

        public float SpiritRegen { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }
        public float ManaRegenDrinking { get; set; }
        public float HealthRegen { get; set; }
        public float HealthRegenCombat { get; set; }
        public float HealthRegenEating { get; set; }
        public float MeleeMitigation { get; set; }
        public float Defense { get; set; }
        public float PhysicalCritReduction { get; set; }
        public float SpellCritReduction { get; set; }
        public float CritDamageReduction { get; set; }
        public float Dodge { get; set; }

        public float ArcaneSpellModifier { get; set; }
        public float FireSpellModifier { get; set; }
        public float FrostSpellModifier { get; set; }
        public float NatureSpellModifier { get; set; }
        public float ShadowSpellModifier { get; set; }
        public float FrostFireSpellModifier { get; set; }
        public float HolySpellModifier { get; set; }

        public float ArcaneCritBonus { get; set; }
        public float FireCritBonus { get; set; }
        public float FrostCritBonus { get; set; }
        public float NatureCritBonus { get; set; }
        public float ShadowCritBonus { get; set; }
        public float FrostFireCritBonus { get; set; }
        public float HolyCritBonus { get; set; }

        public float ArcaneCritRate { get; set; }
        public float FireCritRate { get; set; }
        public float FrostCritRate { get; set; }
        public float NatureCritRate { get; set; }
        public float ShadowCritRate { get; set; }
        public float FrostFireCritRate { get; set; }
        public float HolyCritRate { get; set; }

        public float ArcaneHitRate { get; set; }
        public float FireHitRate { get; set; }
        public float FrostHitRate { get; set; }
        public float NatureHitRate { get; set; }
        public float ShadowHitRate { get; set; }
        public float FrostFireHitRate { get; set; }
        public float HolyHitRate { get; set; }

        public float ArcaneThreatMultiplier { get; set; }
        public float FireThreatMultiplier { get; set; }
        public float FrostThreatMultiplier { get; set; }
        public float NatureThreatMultiplier { get; set; }
        public float ShadowThreatMultiplier { get; set; }
        public float FrostFireThreatMultiplier { get; set; }
        public float HolyThreatMultiplier { get; set; }

        public float ResilienceCritDamageReduction { get; set; }
        public float ResilienceCritRateReduction { get; set; }
        public float Latency { get; set; }
        public float ClearcastingChance { get; set; }

        public float SnaredTime { get; set; }

        public bool GetCooldown(Cooldown cooldown)
        {
            return (cooldown & Cooldown) == cooldown;
        }

        public bool Evocation { get; set; }
        public bool ArcanePower { get; set; }
        public bool IcyVeins { get; set; }
        public bool MoltenFury { get; set; }
        public bool Heroism { get; set; }
        public bool PotionOfWildMagic { get; set; }
        public bool PotionOfSpeed { get; set; }
        public bool FlameCap { get; set; }
        public bool Trinket1 { get; set; }
        public bool Trinket2 { get; set; }
        public bool ManaGemEffect { get; set; }
        public bool ManaGemActivation { get; set; }
        public bool DrumsOfBattle { get; set; }
        public bool Combustion { get; set; }
        public bool WaterElemental { get; set; }
        public bool PowerInfusion { get; set; }
        public bool Frozen { get; set; }
        public string MageArmor { get; set; }

        public Cooldown Cooldown { get; set; }

        public float CombustionDuration { get; set; }
        public float SpellDamageRating { get; set; }
        public float SpellCritRating { get; set; }
        public float SpellHasteRating { get; set; }
        public float Mp5OnCastFor20Sec { get; set; }

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
                    if (Trinket1) buffList.Add(calculations.Trinket1Name);
                    if (Trinket2) buffList.Add(calculations.Trinket2Name);
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
                        maintainSnareState.Spells = new Spell[SpellIdCount];
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
                        frozenState = new CastingState(calculations, BaseStats, CalculationOptions, MageArmor, calculations.Character, ArcanePower, MoltenFury, IcyVeins, Heroism, PotionOfWildMagic, PotionOfSpeed, FlameCap, Trinket1, Trinket2, Combustion, DrumsOfBattle, WaterElemental, ManaGemEffect, PowerInfusion, true);
                    }
                }
                return frozenState;
            }
        }

        public CastingState(CharacterCalculationsMage calculations, Stats characterStats, CalculationOptionsMage calculationOptions, string armor, Character character, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool potionOfWildMagic, bool potionOfSpeed, bool flameCap, bool trinket1, bool trinket2, bool combustion, bool drums, bool waterElemental, bool manaGemEffect, bool powerInfusion, bool frozen)
        {
            MageTalents = calculations.Character.MageTalents;
            BaseStats = calculations.BaseStats; // == characterStats
            CalculationOptions = calculations.CalculationOptions;
            this.calculations = calculations;

            float levelScalingFactor;
            levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (calculationOptions.PlayerLevel - 70) / 10f));

            SnaredTime = CalculationOptions.SnaredTime;
            if (CalculationOptions.MaintainSnare) SnaredTime = 1.0f;

            SpellDamageRating = characterStats.SpellPower;
            SpellCritRating = characterStats.CritRating;
            SpellHasteRating = characterStats.HasteRating;

            if (potionOfWildMagic)
            {
                SpellDamageRating += 200;
                SpellCritRating += 200;
            }
            if (potionOfSpeed)
            {
                SpellHasteRating += 500;
            }

            if (trinket1)
            {
                Stats t = character.Trinket1.Item.Stats;
                SpellDamageRating += t.SpellPowerFor20SecOnUse2Min + t.SpellPowerFor20SecOnUse5Min + t.SpellPowerFor15SecOnUse90Sec + t.SpellPowerFor15SecOnUse2Min;
                SpellHasteRating += t.HasteRatingFor20SecOnUse2Min + t.HasteRatingFor20SecOnUse5Min;
                Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Item.Stats;
                SpellDamageRating += t.SpellPowerFor20SecOnUse2Min + t.SpellPowerFor20SecOnUse5Min + t.SpellPowerFor15SecOnUse90Sec + t.SpellPowerFor15SecOnUse2Min;
                SpellHasteRating += t.HasteRatingFor20SecOnUse2Min + t.HasteRatingFor20SecOnUse5Min;
                Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }
            if (manaGemEffect)
            {
                SpellDamageRating += BaseStats.SpellPowerFor15SecOnManaGem;
                ManaGemActivation = true;
            }
            if (drums)
            {
                SpellHasteRating += 80;
            }

            CastingSpeed = 1 + SpellHasteRating / 995f * levelScalingFactor;
            ArcaneDamage = characterStats.SpellArcaneDamageRating + SpellDamageRating;
            FireDamage = characterStats.SpellFireDamageRating + SpellDamageRating + (flameCap ? 80.0f : 0.0f);
            FrostDamage = characterStats.SpellFrostDamageRating + SpellDamageRating;
            NatureDamage = characterStats.SpellNatureDamageRating + SpellDamageRating;
            ShadowDamage = characterStats.SpellShadowDamageRating + SpellDamageRating;
            HolyDamage = /* characterStats.SpellHolyDamageRating + */ SpellDamageRating;
            FrostFireDamage = Math.Max(FireDamage, FrostDamage);

            float spellCritPerInt = 0f;
            float spellCritBase = 0f;
            float baseRegen = 0f;
            switch (calculationOptions.PlayerLevel)
            {
                case 70:
                    spellCritPerInt = 0.0125f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.009327f;
                    break;
                case 71:
                    spellCritPerInt = 0.0116f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008859f;
                    break;
                case 72:
                    spellCritPerInt = 0.0108f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008415f;
                    break;
                case 73:
                    spellCritPerInt = 0.0101f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007993f;
                    break;
                case 74:
                    spellCritPerInt = 0.0093f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007592f;
                    break;
                case 75:
                    spellCritPerInt = 0.0087f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007211f;
                    break;
                case 76:
                    spellCritPerInt = 0.0081f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.006849f;
                    break;
                case 77:
                    spellCritPerInt = 0.0075f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.006506f;
                    break;
                case 78:
                    spellCritPerInt = 0.007f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.006179f;
                    break;
                case 79:
                    spellCritPerInt = 0.0065f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.005869f;
                    break;
                case 80:
                    spellCritPerInt = 0.006f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.005575f;
                    break;
            }
            SpellCrit = 0.01f * (characterStats.Intellect * spellCritPerInt + spellCritBase) + 0.01f * character.MageTalents.ArcaneInstability + 0.15f * 0.02f * character.MageTalents.ArcaneConcentration * character.MageTalents.ArcanePotency + SpellCritRating / 1400f * levelScalingFactor + characterStats.SpellCrit + character.MageTalents.FocusMagic * 0.03f * (1 - (float)Math.Pow(1 - calculationOptions.FocusMagicTargetCritRate, 10.0)) + 0.01f * character.MageTalents.Pyromaniac;
            if (frozen) SpellCrit += (MageTalents.Shatter == 3 ? 0.5f : 0.17f * MageTalents.Shatter);
            SpellHit = characterStats.HitRating * levelScalingFactor / 800f + characterStats.SpellHit + 0.01f * character.MageTalents.ElementalPrecision;

            int targetLevel = calculationOptions.TargetLevel;
            int playerLevel = calculationOptions.PlayerLevel;
            // if (targetLevel >= playerLevel + 3) SpellCrit -= 0.021f; boss crit depression, disproved
            float maxHitRate = 1.00f;
            ArcaneHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit + 0.01f * character.MageTalents.ArcaneFocus);
            FireHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            FrostHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            NatureHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            ShadowHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            FrostFireHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            HolyHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);

            SpiritRegen = 0.001f + characterStats.Spirit * baseRegen * (float)Math.Sqrt(characterStats.Intellect);
            ManaRegen = SpiritRegen + characterStats.Mp5 / 5f + SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
            ManaRegen5SR = SpiritRegen * characterStats.SpellCombatManaRegeneration + characterStats.Mp5 / 5f + SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
            HealthRegen = 0.0312f * characterStats.Spirit + characterStats.Hp5 / 5f;
            HealthRegenCombat = characterStats.Hp5 / 5f;
            if (playerLevel < 75)
            {
                ManaRegenDrinking = ManaRegen + 240f;
                HealthRegenEating = HealthRegen + 250f;
            }
            else if (playerLevel < 80)
            {
                ManaRegenDrinking = ManaRegen + 306f;
                HealthRegenEating = HealthRegen + 440f;
            }
            else
            {
                ManaRegenDrinking = ManaRegen + 432f;
                HealthRegenEating = HealthRegen + 500f;
            }
            MeleeMitigation = (1 - 1 / (1 + 0.1f * characterStats.Armor / (8.5f * (calculationOptions.TargetLevel + 4.5f * (calculationOptions.TargetLevel - 59)) + 40)));
            Defense = 5 * calculationOptions.PlayerLevel + characterStats.DefenseRating / 4.918498039f; // this is for level 80 only
            int molten = (armor == "Molten Armor") ? 1 : 0;
            PhysicalCritReduction = (0.04f * (Defense - 5 * calculationOptions.PlayerLevel) / 100 + characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            SpellCritReduction = (characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            CritDamageReduction = (characterStats.Resilience / 2500f * 2.2f * levelScalingFactor);
            Dodge = 0.043545f + 0.01f / (0.006650f + 0.953f / ((0.04f * (Defense - 5 * calculationOptions.PlayerLevel)) / 100f + characterStats.DodgeRating / 1200 * levelScalingFactor + (characterStats.Agility - 46f) * 0.0195f));

            // spell calculations

            ArcanePower = arcanePower;
            MoltenFury = moltenFury;
            IcyVeins = icyVeins;
            Heroism = heroism;
            PotionOfWildMagic = potionOfWildMagic;
            PotionOfSpeed = potionOfSpeed;
            FlameCap = flameCap;
            Trinket1 = trinket1;
            Trinket2 = trinket2;
            Combustion = combustion;
            DrumsOfBattle = drums;
            WaterElemental = waterElemental;
            ManaGemEffect = manaGemEffect;
            PowerInfusion = powerInfusion;
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
            CastingSpeed *= (1f + characterStats.SpellHaste);
            CastingSpeed *= (1f + 0.02f * character.MageTalents.NetherwindPresence);

            Latency = calculationOptions.Latency;
            ClearcastingChance = 0.02f * character.MageTalents.ArcaneConcentration;

            GlobalCooldownLimit = 1f;
            GlobalCooldown = Math.Max(GlobalCooldownLimit, 1.5f / CastingSpeed);

            ArcaneSpellModifier = (1 + 0.01f * character.MageTalents.ArcaneInstability) * (1 + 0.01f * character.MageTalents.PlayingWithFire) * (1 + characterStats.BonusSpellPowerMultiplier) * (1 + characterStats.BonusDamageMultiplier);
            if (arcanePower)
            {
                ArcaneSpellModifier *= 1.2f;
            }
            if (moltenFury)
            {
                ArcaneSpellModifier *= (1 + 0.06f * character.MageTalents.MoltenFury);
            }
            FireSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.FirePower);
            FrostSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.PiercingIce) * (1 + 0.01f * character.MageTalents.ArcticWinds);
            NatureSpellModifier = ArcaneSpellModifier;
            ShadowSpellModifier = ArcaneSpellModifier;
            HolySpellModifier = ArcaneSpellModifier;
            FrostFireSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.FirePower) * (1 + 0.02f * character.MageTalents.PiercingIce) * (1 + 0.01f * character.MageTalents.ArcticWinds) * Math.Max(1 + characterStats.BonusFireDamageMultiplier, 1 + characterStats.BonusFrostDamageMultiplier);
            ArcaneSpellModifier *= (1 + characterStats.BonusArcaneDamageMultiplier);
            FireSpellModifier *= (1 + characterStats.BonusFireDamageMultiplier);
            FrostSpellModifier *= (1 + characterStats.BonusFrostDamageMultiplier);
            NatureSpellModifier *= (1 + characterStats.BonusNatureDamageMultiplier);
            ShadowSpellModifier *= (1 + characterStats.BonusShadowDamageMultiplier);
            HolySpellModifier *= (1 + characterStats.BonusHolyDamageMultiplier);

            ResilienceCritDamageReduction = 1;
            ResilienceCritRateReduction = 0;

            ArcaneCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower + 0.1f * character.MageTalents.Burnout + characterStats.CritBonusDamage)) * ResilienceCritDamageReduction;
            FireCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower + 0.1f * character.MageTalents.Burnout + characterStats.CritBonusDamage)) * (1 + 0.08f * character.MageTalents.Ignite) * ResilienceCritDamageReduction;
            FrostCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + character.MageTalents.IceShards / 3.0f + 0.25f * character.MageTalents.SpellPower + 0.1f * character.MageTalents.Burnout + characterStats.CritBonusDamage)) * ResilienceCritDamageReduction;
            FrostFireCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + character.MageTalents.IceShards / 3.0f + 0.25f * character.MageTalents.SpellPower + 0.1f * character.MageTalents.Burnout + characterStats.CritBonusDamage)) * (1 + 0.08f * character.MageTalents.Ignite) * ResilienceCritDamageReduction;
            NatureCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower + characterStats.CritBonusDamage)) * ResilienceCritDamageReduction; // unknown if affected by burnout
            ShadowCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower + characterStats.CritBonusDamage)) * ResilienceCritDamageReduction;
            HolyCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower + characterStats.CritBonusDamage)) * ResilienceCritDamageReduction;

            ArcaneCritRate = SpellCrit;
            FireCritRate = SpellCrit + 0.02f * character.MageTalents.CriticalMass;
            if (combustion)
            {
                CombustionDuration = ComputeCombustion(FireCritRate);
                FireCritRate = 3 / CombustionDuration;
            }
            FrostFireCritRate = FireCritRate;
            FrostCritRate = SpellCrit;
            NatureCritRate = SpellCrit;
            ShadowCritRate = SpellCrit;
            HolyCritRate = SpellCrit;

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            ArcaneThreatMultiplier = threatFactor * (1 - character.MageTalents.ArcaneSubtlety * 0.2f);
            FireThreatMultiplier = threatFactor * (1 - character.MageTalents.BurningSoul * 0.05f);
            FrostThreatMultiplier = threatFactor * (1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            FrostFireThreatMultiplier = threatFactor * (1 - character.MageTalents.BurningSoul * 0.05f) * (1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            NatureThreatMultiplier = threatFactor;
            ShadowThreatMultiplier = threatFactor;
            HolyThreatMultiplier = threatFactor;

            if (BaseStats.LightningCapacitorProc > 0)
            {
                LightningBolt = (LightningBolt)GetSpell(SpellId.LightningBolt);
            }
            if (BaseStats.ThunderCapacitorProc > 0)
            {
                ThunderBolt = (ThunderBolt)GetSpell(SpellId.ThunderBolt);
            }
            if (BaseStats.LightweaveEmbroideryProc > 0)
            {
                LightweaveBolt = (LightweaveBolt)GetSpell(SpellId.LightweaveBolt);
            }
            if (BaseStats.ShatteredSunAcumenProc > 0 && !CalculationOptions.Aldor)
            {
                ArcaneBolt = (ArcaneBolt)GetSpell(SpellId.ArcaneBolt);
            }
            if (BaseStats.PendulumOfTelluricCurrentsProc > 0)
            {
                PendulumOfTelluricCurrents = (PendulumOfTelluricCurrents)GetSpell(SpellId.PendulumOfTelluricCurrents);
            }
        }

        public ArcaneBolt ArcaneBolt { get; set; }
        public LightningBolt LightningBolt { get; set; }
        public ThunderBolt ThunderBolt { get; set; }
        public LightweaveBolt LightweaveBolt { get; set; }
        public PendulumOfTelluricCurrents PendulumOfTelluricCurrents { get; set; }

        private static int SpellIdCount;

        static CastingState()
        {
            SpellIdCount = Enum.GetValues(typeof(SpellId)).Length;
        }

        private Spell[] Spells = new Spell[SpellIdCount];

        //private Dictionary<int, Spell> Spells = new Dictionary<int, Spell>(); // profiling doesn't show any noticeable benefit using plain array, so use dictionary to save on size

        public void SetSpell(SpellId spellId, Spell spell)
        {
            Spells[(int)spellId] = spell;
        }

        public Spell GetSpell(SpellId spellId)
        {
            Spell s = Spells[(int)spellId];
            if (s != null) return s;
            //Spell s = null;
            //if (Spells.TryGetValue((int)spellId, out s)) return s;

            switch (spellId)
            {
                case SpellId.ArcaneBolt:
                    s = new ArcaneBolt(this);
                    break;
                case SpellId.ThunderBolt:
                    s = new ThunderBolt(this);
                    break;
                case SpellId.LightweaveBolt:
                    s = new LightweaveBolt(this);
                    break;
                case SpellId.PendulumOfTelluricCurrents:
                    s = new PendulumOfTelluricCurrents(this);
                    break;
                case SpellId.LightningBolt:
                    s = new LightningBolt(this);
                    break;
                case SpellId.ArcaneMissiles:
                    s = new ArcaneMissiles(this, false, 0);
                    break;
                case SpellId.ArcaneMissiles1:
                    s = new ArcaneMissiles(this, false, 1);
                    break;
                case SpellId.ArcaneMissiles2:
                    s = new ArcaneMissiles(this, false, 2);
                    break;
                case SpellId.ArcaneMissiles3:
                    s = new ArcaneMissiles(this, false, 3);
                    break;
                case SpellId.ArcaneMissiles0Clipped:
                    s = new ArcaneMissiles(this, false, 0, 4);
                    break;
                case SpellId.ArcaneMissiles1Clipped:
                    s = new ArcaneMissiles(this, false, 1, 4);
                    break;
                case SpellId.ArcaneMissiles2Clipped:
                    s = new ArcaneMissiles(this, false, 2, 4);
                    break;
                case SpellId.ArcaneMissiles3Clipped:
                    s = new ArcaneMissiles(this, false, 3, 4);
                    break;
                case SpellId.ArcaneMissilesMB:
                    s = new ArcaneMissiles(this, true, 0);
                    break;
                case SpellId.ArcaneMissilesMB1:
                    s = new ArcaneMissiles(this, true, 1);
                    break;
                case SpellId.ArcaneMissilesMB2:
                    s = new ArcaneMissiles(this, true, 2);
                    break;
                case SpellId.ArcaneMissilesMB3:
                    s = new ArcaneMissiles(this, true, 3);
                    break;
                case SpellId.ArcaneMissilesMB0Clipped:
                    s = new ArcaneMissiles(this, true, 0, 4);
                    break;
                case SpellId.ArcaneMissilesMB1Clipped:
                    s = new ArcaneMissiles(this, true, 1, 4);
                    break;
                case SpellId.ArcaneMissilesMB2Clipped:
                    s = new ArcaneMissiles(this, true, 2, 4);
                    break;
                case SpellId.ArcaneMissilesMB3Clipped:
                    s = new ArcaneMissiles(this, true, 3, 4);
                    break;
                case SpellId.ArcaneMissilesCC:
                    s = new ArcaneMissilesCC(this);
                    break;
                case SpellId.ArcaneMissilesNoProc:
                    s = new ArcaneMissiles(this, false, true, false, false, 0, 5);
                    break;
                /*case SpellId.ArcaneMissilesFTF:
                    s = new ArcaneMissiles(this);
                    break;
                case SpellId.ArcaneMissilesFTT:
                    s = new ArcaneMissiles(this);
                    break;*/
                case SpellId.Frostbolt:
                    s = new Frostbolt(this);
                    break;
                case SpellId.FrostboltFOF:
                    s = new Frostbolt(this, false, false, false, true);
                    break;
                case SpellId.FrostboltNoCC:
                    s = new Frostbolt(this, true, false, false, false);
                    break;
                case SpellId.Fireball:
                    s = new Fireball(this, false, false);
                    break;
                case SpellId.FireballBF:
                    s = new Fireball(this, false, true);
                    break;
                case SpellId.FBPyro:
                    s = new FBPyro(this);
                    break;
                case SpellId.FBLBPyro:
                    s = new FBLBPyro(this);
                    break;
                case SpellId.FFBLBPyro:
                    s = new FFBLBPyro(this);
                    break;
                case SpellId.FBScPyro:
                    s = new FBScPyro(this);
                    break;
                case SpellId.FFBPyro:
                    s = new FFBPyro(this);
                    break;
                case SpellId.FFBScPyro:
                    s = new FFBScPyro(this);
                    break;
                case SpellId.FFBScLBPyro:
                    s = new FFBScLBPyro(this);
                    break;
                case SpellId.FrostfireBolt:
                    s = new FrostfireBolt(this, false, false);
                    break;
                case SpellId.FrostfireBoltFOF:
                    s = new FrostfireBolt(this, false, true);
                    break;
                case SpellId.Pyroblast:
                    s = new Pyroblast(this, false);
                    break;
                case SpellId.FireBlast:
                    s = new FireBlast(this);
                    break;
                case SpellId.Scorch:
                    s = new Scorch(this);
                    break;
                case SpellId.ScorchNoCC:
                    s = new Scorch(this, false);
                    break;
                case SpellId.ABABarSc:
                    s = new ABABarSc(this);
                    break;
                case SpellId.ABABarCSc:
                    s = new ABABarCSc(this);
                    break;
                case SpellId.ABAMABarSc:
                    s = new ABAMABarSc(this);
                    break;
                case SpellId.AB3ABarCSc:
                    s = new AB3ABarCSc(this);
                    break;
                case SpellId.AB3AMABarSc:
                    s = new AB3AMABarSc(this);
                    break;
                case SpellId.AB3MBAMABarSc:
                    s = new AB3MBAMABarSc(this);
                    break;
                case SpellId.ArcaneBarrage:
                    s = new ArcaneBarrage(this, 0);
                    break;
                case SpellId.ArcaneBarrage1:
                    s = new ArcaneBarrage(this, 1);
                    break;
                case SpellId.ArcaneBarrage2:
                    s = new ArcaneBarrage(this, 2);
                    break;
                case SpellId.ArcaneBarrage3:
                    s = new ArcaneBarrage(this, 3);
                    break;
                case SpellId.ArcaneBarrage1Combo:
                    s = new ArcaneBarrage(this, 0/* 1 * CalculationOptions.ComboReliability*/);
                    break;
                case SpellId.ArcaneBarrage2Combo:
                    s = new ArcaneBarrage(this, 0/* 2 * CalculationOptions.ComboReliability*/);
                    break;
                case SpellId.ArcaneBarrage3Combo:
                    s = new ArcaneBarrage(this, 0/* 3 * CalculationOptions.ComboReliability*/);
                    break;
                case SpellId.ArcaneBlast33:
                    s = new ArcaneBlast(this, 3, 3);
                    break;
                case SpellId.ArcaneBlast33NoCC:
                    s = new ArcaneBlast(this, 3, 3, true, false, false);
                    break;
                case SpellId.ArcaneBlast00:
                    s = new ArcaneBlast(this, 0, 0);
                    break;
                case SpellId.ArcaneBlast00NoCC:
                    s = new ArcaneBlast(this, 0, 0, true, false, false);
                    break;
                case SpellId.ArcaneBlast10:
                    s = new ArcaneBlast(this, 1, 0);
                    break;
                case SpellId.ArcaneBlast01:
                    s = new ArcaneBlast(this, 0, 1);
                    break;
                case SpellId.ArcaneBlast11:
                    s = new ArcaneBlast(this, 1, 1);
                    break;
                case SpellId.ArcaneBlast11NoCC:
                    s = new ArcaneBlast(this, 1, 1, true, false, false);
                    break;
                case SpellId.ArcaneBlast22:
                    s = new ArcaneBlast(this, 2, 2);
                    break;
                case SpellId.ArcaneBlast22NoCC:
                    s = new ArcaneBlast(this, 2, 2, true, false, false);
                    break;
                case SpellId.ArcaneBlast12:
                    s = new ArcaneBlast(this, 1, 2);
                    break;
                case SpellId.ArcaneBlast23:
                    s = new ArcaneBlast(this, 2, 3);
                    break;
                case SpellId.ArcaneBlast30:
                    s = new ArcaneBlast(this, 3, 0);
                    break;
                case SpellId.ArcaneBlast0Hit:
                    s = new ArcaneBlast(this, 0, 0, true);
                    break;
                case SpellId.ArcaneBlast1Hit:
                    s = new ArcaneBlast(this, 1, 1, true);
                    break;
                case SpellId.ArcaneBlast2Hit:
                    s = new ArcaneBlast(this, 2, 2, true);
                    break;
                case SpellId.ArcaneBlast3Hit:
                    s = new ArcaneBlast(this, 3, 3, true);
                    break;
                case SpellId.ArcaneBlast0Miss:
                    s = new ArcaneBlast(this, 0, 0, false);
                    break;
                case SpellId.ArcaneBlast1Miss:
                    s = new ArcaneBlast(this, 1, 1, false);
                    break;
                case SpellId.ArcaneBlast2Miss:
                    s = new ArcaneBlast(this, 2, 2, false);
                    break;
                case SpellId.ArcaneBlast3Miss:
                    s = new ArcaneBlast(this, 3, 3, false);
                    break;
                case SpellId.ArcaneBlastSpam:
                    s = new AB(this);
                    break;
                case SpellId.ABarAM:
                    s = new ABarAM(this);
                    break;
                case SpellId.ABP:
                    s = new ABP(this);
                    break;
                case SpellId.ABAM:
                    s = new ABAM(this);
                    break;
                case SpellId.ABSpamMBAM:
                    s = new ABSpamMBAM(this);
                    break;
                case SpellId.ABSpam3C:
                    s = new ABSpam3C(this);
                    break;
                case SpellId.ABSpam03C:
                    s = new ABSpam03C(this);
                    break;
                case SpellId.AB2ABar3C:
                    s = new AB2ABar3C(this);
                    break;
                case SpellId.ABABar2C:
                    s = new ABABar2C(this);
                    break;
                case SpellId.ABABar2MBAM:
                    s = new ABABar2MBAM(this);
                    break;
                case SpellId.ABABar1MBAM:
                    s = new ABABar1MBAM(this);
                    break;
                case SpellId.ABABar3C:
                    s = new ABABar3C(this);
                    break;
                case SpellId.AB3ABar3MBAM:
                    s = new AB3ABar3MBAM(this);
                    break;
                case SpellId.AB3AM:
                    s = new AB3AM(this);
                    break;
                case SpellId.AB3AM2MBAM:
                    s = new AB3AM2MBAM(this);
                    break;
                case SpellId.AB2ABar2MBAM:
                    s = new AB2ABar2MBAM(this);
                    break;
                case SpellId.ABABar0MBAM:
                    s = new ABABar0MBAM(this);
                    break;
                case SpellId.ABABar:
                    s = new ABABar(this);
                    break;
                case SpellId.ABSpam3MBAM:
                    s = new ABSpam3MBAM(this);
                    break;
                case SpellId.ABAMABar:
                    s = new ABAMABar(this);
                    break;
                case SpellId.AB2AMABar:
                    s = new AB2AMABar(this);
                    break;
                case SpellId.AB3AMABar:
                    s = new AB3AMABar(this);
                    break;
                case SpellId.AB32AMABar:
                    s = new AB32AMABar(this);
                    break;
                case SpellId.AB3ABar3C:
                    s = new AB3ABar3C(this);
                    break;
                case SpellId.ABABar0C:
                    s = new ABABar0C(this);
                    break;
                case SpellId.ABABar1C:
                    s = new ABABar1C(this);
                    break;
                case SpellId.ABABarY:
                    s = new ABABarY(this);
                    break;
                case SpellId.AB2ABar:
                    s = new AB2ABar(this);
                    break;
                case SpellId.AB2ABar2C:
                    s = new AB2ABar2C(this);
                    break;
                case SpellId.AB2ABarMBAM:
                    s = new AB2ABarMBAM(this);
                    break;
                case SpellId.AB3ABar:
                    s = new AB3ABar(this);
                    break;
                case SpellId.AB3ABarX:
                    s = new AB3ABarX(this);
                    break;
                case SpellId.AB3ABarY:
                    s = new AB3ABarY(this);
                    break;
                case SpellId.FBABar:
                    s = new FBABar(this);
                    break;
                case SpellId.FrBABar:
                    s = new FrBABar(this);
                    break;
                case SpellId.FFBABar:
                    s = new FFBABar(this);
                    break;
                case SpellId.ABAMP:
                    s = new ABAMP(this);
                    break;
                case SpellId.AB3AMSc:
                    s = new AB3AMSc(this);
                    break;
                case SpellId.ABAM3Sc:
                    s = new ABAM3Sc(this);
                    break;
                case SpellId.ABAM3Sc2:
                    s = new ABAM3Sc2(this);
                    break;
                case SpellId.ABAM3FrB:
                    s = new ABAM3FrB(this);
                    break;
                case SpellId.ABAM3FrB2:
                    s = new ABAM3FrB2(this);
                    break;
                case SpellId.ABFrB:
                    s = new ABFrB(this);
                    break;
                case SpellId.AB3FrB:
                    s = new AB3FrB(this);
                    break;
                case SpellId.ABFrB3FrB:
                    s = new ABFrB3FrB(this);
                    break;
                case SpellId.ABFrB3FrB2:
                    s = new ABFrB3FrB2(this);
                    break;
                case SpellId.ABFrB3FrBSc:
                    s = new ABFrB3FrBSc(this);
                    break;
                case SpellId.ABFB3FBSc:
                    s = new ABFB3FBSc(this);
                    break;
                case SpellId.AB3Sc:
                    s = new AB3Sc(this);
                    break;
                case SpellId.FBSc:
                    s = new FBSc(this);
                    break;
                case SpellId.FBFBlast:
                    s = new FBFBlast(this);
                    break;
                case SpellId.ABAM3ScCCAM:
                    s = new ABAM3ScCCAM(this);
                    break;
                case SpellId.ABAM3Sc2CCAM:
                    s = new ABAM3Sc2CCAM(this);
                    break;
                case SpellId.ABAM3FrBCCAM:
                    s = new ABAM3FrBCCAM(this);
                    break;
                case SpellId.ABAM3FrBCCAMFail:
                    s = new ABAM3FrBCCAMFail(this);
                    break;
                case SpellId.ABAM3FrBScCCAM:
                    s = new ABAM3FrBScCCAM(this);
                    break;
                case SpellId.ABAMCCAM:
                    s = new ABAMCCAM(this);
                    break;
                case SpellId.ABAM3CCAM:
                    s = new ABAM3CCAM(this);
                    break;
                case SpellId.IceLance:
                    s = new IceLance(this);
                    break;
                case SpellId.FrBFBIL:
                    s = new FrBFBIL(this);
                    break;
                case SpellId.ArcaneExplosion:
                    s = new ArcaneExplosion(this);
                    break;
                case SpellId.FlamestrikeSpammed:
                    s = new Flamestrike(this, true);
                    break;
                case SpellId.FlamestrikeSingle:
                    s = new Flamestrike(this, false);
                    break;
                case SpellId.Blizzard:
                    s = new Blizzard(this);
                    break;
                case SpellId.BlastWave:
                    s = new BlastWave(this);
                    break;
                case SpellId.DragonsBreath:
                    s = new DragonsBreath(this);
                    break;
                case SpellId.ConeOfCold:
                    s = new ConeOfCold(this);
                    break;
                case SpellId.ArcaneBlast0POM:
                    s = new ArcaneBlast(this, 0, 0, false, false, true);
                    break;
                case SpellId.FireballPOM:
                    s = new Fireball(this, true, false);
                    break;
                case SpellId.FrBFB:
                    s = new FrBFB(this);
                    break;
                case SpellId.FBScLBPyro:
                    s = new FBScLBPyro(this);
                    break;
                case SpellId.FB2ABar:
                    s = new FB2ABar(this);
                    break;
                case SpellId.FrB2ABar:
                    s = new FrB2ABar(this);
                    break;
                case SpellId.ScLBPyro:
                    s = new ScLBPyro(this);
                    break;
                case SpellId.Slow:
                    s = new Slow(this);
                    break;
                case SpellId.ABABarSlow:
                    s = new ABABarSlow(this);
                    break;
                case SpellId.FBABarSlow:
                    s = new FBABarSlow(this);
                    break;
                case SpellId.FrBABarSlow:
                    s = new FrBABarSlow(this);
                    break;
                case SpellId.FrostboltPOM:
                    s = new Frostbolt(this, false, false, true, false);
                    break;
                case SpellId.PyroblastPOM:
                    s = new Pyroblast(this, true);
                    break;
                case SpellId.LivingBomb:
                    s = new LivingBomb(this);
                    break;
                case SpellId.CustomSpellMix:
                    s = new SpellCustomMix(this);
                    break;
            }
            if (s != null)
            {
                s.SpellId = spellId;
                Spells[(int)spellId] = s;
            }

            return s;
        }
    }
}
