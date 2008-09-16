using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public enum Cooldown
    {
        ArcanePower,
        IcyVeins,
        MoltenFury,
        Heroism,
        DestructionPotion,
        FlameCap,
        Trinket1,
        Trinket2,
        DrumsOfBattle,
        Combustion
    }

    public sealed class CastingState
    {
        private CharacterCalculationsMage calculations;

        public CalculationOptionsMage CalculationOptions
        {
            get
            {
                return calculations.CalculationOptions;
            }
        }

        public MageTalents MageTalents
        {
            get
            {
                return calculations.Character.MageTalents;
            }
        }

        public Stats BaseStats
        {
            get
            {
                return calculations.BaseStats;
            }
        }

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

        public float ArcaneCritBonus { get; set; }
        public float FireCritBonus { get; set; }
        public float FrostCritBonus { get; set; }
        public float NatureCritBonus { get; set; }
        public float ShadowCritBonus { get; set; }
        public float FrostFireCritBonus { get; set; }

        public float ArcaneCritRate { get; set; }
        public float FireCritRate { get; set; }
        public float FrostCritRate { get; set; }
        public float NatureCritRate { get; set; }
        public float ShadowCritRate { get; set; }
        public float FrostFireCritRate { get; set; }

        public float ArcaneHitRate { get; set; }
        public float FireHitRate { get; set; }
        public float FrostHitRate { get; set; }
        public float NatureHitRate { get; set; }
        public float ShadowHitRate { get; set; }
        public float FrostFireHitRate { get; set; }

        public float ArcaneThreatMultiplier { get; set; }
        public float FireThreatMultiplier { get; set; }
        public float FrostThreatMultiplier { get; set; }
        public float NatureThreatMultiplier { get; set; }
        public float ShadowThreatMultiplier { get; set; }
        public float FrostFireThreatMultiplier { get; set; }

        public float ResilienceCritDamageReduction { get; set; }
        public float ResilienceCritRateReduction { get; set; }
        public float Latency { get; set; }
        public float ClearcastingChance { get; set; }

        public int IncrementalSetIndex { get; set; }

        public int GetHex()
        {
            unchecked
            {
                int hex = 0;
                hex = (hex << 1) + (ArcanePower ? 1 : 0);
                hex = (hex << 1) + (Combustion ? 1 : 0);
                hex = (hex << 1) + (DestructionPotion ? 1 : 0);
                hex = (hex << 1) + (DrumsOfBattle ? 1 : 0);
                hex = (hex << 1) + (FlameCap ? 1 : 0);
                hex = (hex << 1) + (Heroism ? 1 : 0);
                hex = (hex << 1) + (IcyVeins ? 1 : 0);
                hex = (hex << 1) + (MoltenFury ? 1 : 0);
                hex = (hex << 1) + (Trinket1 ? 1 : 0);
                hex = (hex << 1) + (Trinket2 ? 1 : 0);
                return hex;
            }
        }

        public bool GetCooldown(Cooldown cooldown)
        {
            switch (cooldown)
            {
                case Cooldown.ArcanePower:
                    return ArcanePower;
                case Cooldown.Combustion:
                    return Combustion;
                case Cooldown.DestructionPotion:
                    return DestructionPotion;
                case Cooldown.DrumsOfBattle:
                    return DrumsOfBattle;
                case Cooldown.FlameCap:
                    return FlameCap;
                case Cooldown.Heroism:
                    return Heroism;
                case Cooldown.IcyVeins:
                    return IcyVeins;
                case Cooldown.MoltenFury:
                    return MoltenFury;
                case Cooldown.Trinket1:
                    return Trinket1;
                case Cooldown.Trinket2:
                    return Trinket2;
                default:
                    return false;
            }
        }

        public bool ArcanePower { get; set; }
        public bool IcyVeins { get; set; }
        public bool MoltenFury { get; set; }
        public bool Heroism { get; set; }
        public bool DestructionPotion { get; set; }
        public bool FlameCap { get; set; }
        public bool Trinket1 { get; set; }
        public bool Trinket2 { get; set; }
        public bool ManaGemActivation { get; set; }
        public bool DrumsOfBattle { get; set; }
        public bool Combustion { get; set; }
        public float CombustionDuration { get; set; }

        public float SpellDamageRating { get; set; }
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
                    if (DestructionPotion) buffList.Add("Destruction Potion");

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

        public CastingState(CharacterCalculationsMage calculations, Stats characterStats, CalculationOptionsMage calculationOptions, string armor, Character character, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool destructionPotion, bool flameCap, bool trinket1, bool trinket2, bool combustion, bool drums, int incrementalSetIndex)
        {
            this.calculations = calculations;
            IncrementalSetIndex = incrementalSetIndex;

            float levelScalingFactor;
            levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (calculationOptions.PlayerLevel - 70) / 10f));

            SpellDamageRating = characterStats.SpellDamageRating;
            SpellHasteRating = characterStats.SpellHasteRating;

            if (destructionPotion) SpellDamageRating += 120;

            if (trinket1)
            {
                Stats t = character.Trinket1.Stats;
                SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec + t.SpellDamageFor15SecOnUse2Min;
                SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
                if (t.SpellDamageFor15SecOnManaGem > 0.0) ManaGemActivation = true;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Stats;
                SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec + t.SpellDamageFor15SecOnUse2Min;
                SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
                if (t.SpellDamageFor15SecOnManaGem > 0.0) ManaGemActivation = true;
            }
            if (drums)
            {
                SpellHasteRating += 80;
            }

            CastingSpeed = 1 + SpellHasteRating / 995f * levelScalingFactor;
            ArcaneDamage = characterStats.SpellArcaneDamageRating + SpellDamageRating;
            FireDamage = characterStats.SpellFireDamageRating + SpellDamageRating + (flameCap ? 80.0f : 0.0f);
            FrostDamage = characterStats.SpellFrostDamageRating + SpellDamageRating;
            NatureDamage = /* characterStats.SpellNatureDamageRating + */ SpellDamageRating;
            ShadowDamage = characterStats.SpellShadowDamageRating + SpellDamageRating;
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
                    baseRegen = 0.009079f;
                    break;
                case 72:
                    spellCritPerInt = 0.0108f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008838f;
                    break;
                case 73:
                    spellCritPerInt = 0.0101f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008603f;
                    break;
                case 74:
                    spellCritPerInt = 0.0093f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008375f;
                    break;
                case 75:
                    spellCritPerInt = 0.0087f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.008152f;
                    break;
                case 76:
                    spellCritPerInt = 0.0081f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007936f;
                    break;
                case 77:
                    spellCritPerInt = 0.0075f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007725f;
                    break;
                case 78:
                    spellCritPerInt = 0.007f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.00752f;
                    break;
                case 79:
                    spellCritPerInt = 0.0065f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.00732f;
                    break;
                case 80:
                    spellCritPerInt = 0.006f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.007125f;
                    break;
            }
            SpellCrit = 0.01f * (characterStats.Intellect * spellCritPerInt + spellCritBase) + 0.01f * character.MageTalents.ArcaneInstability + 0.015f * character.MageTalents.ArcanePotency + characterStats.SpellCritRating / 1400f * levelScalingFactor + characterStats.SpellCrit;
            if (destructionPotion) SpellCrit += 0.02f;
            SpellHit = characterStats.SpellHitRating * levelScalingFactor / 800f;

            int targetLevel = calculationOptions.TargetLevel;
            int playerLevel = calculationOptions.PlayerLevel;
            float maxHitRate = 1.00f;
            ArcaneHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit + 0.01f * character.MageTalents.ArcaneFocus);
            FireHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit + 0.01f * character.MageTalents.ElementalPrecision);
            FrostHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit + 0.01f * character.MageTalents.ElementalPrecision);
            NatureHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            ShadowHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit);
            FrostFireHitRate = Math.Min(maxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + SpellHit + 0.01f * character.MageTalents.ElementalPrecision);

            SpiritRegen = 0.001f + characterStats.Spirit * baseRegen * (float)Math.Sqrt(characterStats.Intellect);
            ManaRegen = SpiritRegen + characterStats.Mp5 / 5f + SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration;
            ManaRegen5SR = SpiritRegen * characterStats.SpellCombatManaRegeneration + characterStats.Mp5 / 5f + SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration;
            ManaRegenDrinking = ManaRegen + 240f;
            HealthRegen = 0.0312f * characterStats.Spirit + characterStats.Hp5 / 5f;
            HealthRegenCombat = characterStats.Hp5 / 5f;
            HealthRegenEating = ManaRegen + 250f;
            MeleeMitigation = (1 - 1 / (1 + 0.1f * characterStats.Armor / (8.5f * (70 + 4.5f * (70 - 59)) + 40)));
            Defense = 350 + characterStats.DefenseRating / 2.37f;
            int molten = (armor == "Molten Armor") ? 1 : 0;
            PhysicalCritReduction = (0.04f * (Defense - 5 * 70) / 100 + characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            SpellCritReduction = (characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            CritDamageReduction = (characterStats.Resilience / 2500f * 2f * levelScalingFactor);
            Dodge = ((0.0443f * characterStats.Agility + 3.28f + 0.04f * (Defense - 5 * 70)) / 100f + characterStats.DodgeRating / 1200 * levelScalingFactor);

            // spell calculations

            ArcanePower = arcanePower;
            MoltenFury = moltenFury;
            IcyVeins = icyVeins;
            Heroism = heroism;
            DestructionPotion = destructionPotion;
            FlameCap = flameCap;
            Trinket1 = trinket1;
            Trinket2 = trinket2;
            Combustion = combustion;
            DrumsOfBattle = drums;

            if (icyVeins)
            {
                CastingSpeed *= 1.2f;
            }
            if (heroism)
            {
                CastingSpeed *= 1.3f;
            }
            CastingSpeed *= (1f + 0.02f * character.MageTalents.NetherwindPresence);

            Latency = calculationOptions.Latency;
            ClearcastingChance = 0.02f * character.MageTalents.ArcaneConcentration;

            GlobalCooldownLimit = 1f;
            GlobalCooldown = Math.Max(GlobalCooldownLimit, 1.5f / CastingSpeed);

            ArcaneSpellModifier = (1 + 0.01f * character.MageTalents.ArcaneInstability) * (1 + 0.01f * character.MageTalents.PlayingWithFire) * (1 + characterStats.BonusSpellPowerMultiplier) * (1 + 0.02f * character.MageTalents.TormentTheWeak * calculationOptions.SlowedTime);
            if (arcanePower)
            {
                ArcaneSpellModifier *= 1.3f;
            }
            if (moltenFury)
            {
                ArcaneSpellModifier *= (1 + 0.1f * character.MageTalents.MoltenFury);
            }
            FireSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.FirePower);
            FrostSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.PiercingIce) * (1 + 0.01f * character.MageTalents.ArcticWinds);
            NatureSpellModifier = ArcaneSpellModifier;
            ShadowSpellModifier = ArcaneSpellModifier;
            FrostFireSpellModifier = ArcaneSpellModifier * (1 + 0.02f * character.MageTalents.FirePower) * (1 + 0.02f * character.MageTalents.PiercingIce) * (1 + 0.01f * character.MageTalents.ArcticWinds) * Math.Max(1 + characterStats.BonusFireSpellPowerMultiplier, 1 + characterStats.BonusFrostSpellPowerMultiplier);
            ArcaneSpellModifier *= (1 + characterStats.BonusArcaneSpellPowerMultiplier);
            FireSpellModifier *= (1 + characterStats.BonusFireSpellPowerMultiplier);
            FrostSpellModifier *= (1 + characterStats.BonusFrostSpellPowerMultiplier);
            NatureSpellModifier *= (1 + characterStats.BonusNatureSpellPowerMultiplier);
            ShadowSpellModifier *= (1 + characterStats.BonusShadowSpellPowerMultiplier);

            ResilienceCritDamageReduction = 1;
            ResilienceCritRateReduction = 0;

            ArcaneCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower)) * ResilienceCritDamageReduction;
            FireCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower)) * (1 + 0.08f * character.MageTalents.Ignite) * ResilienceCritDamageReduction;
            FrostCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.2f * character.MageTalents.IceShards + 0.25f * character.MageTalents.SpellPower)) * ResilienceCritDamageReduction;
            FrostFireCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.2f * character.MageTalents.IceShards + 0.25f * character.MageTalents.SpellPower)) * (1 + 0.08f * character.MageTalents.Ignite) * ResilienceCritDamageReduction;
            NatureCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower)) * ResilienceCritDamageReduction;
            ShadowCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * character.MageTalents.SpellPower)) * ResilienceCritDamageReduction;

            ArcaneCritRate = SpellCrit;
            FireCritRate = SpellCrit + 0.02f * character.MageTalents.CriticalMass + 0.01f * character.MageTalents.Pyromaniac;
            if (combustion)
            {
                CombustionDuration = ComputeCombustion(FireCritRate);
                FireCritRate = 3 / CombustionDuration;
            }
            FrostFireCritRate = FireCritRate + characterStats.SpellFrostCritRating / 22.08f / 100f;
            FrostCritRate = SpellCrit + characterStats.SpellFrostCritRating / 22.08f / 100f;
            NatureCritRate = SpellCrit;
            ShadowCritRate = SpellCrit;

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            ArcaneThreatMultiplier = threatFactor * (1 - character.MageTalents.ArcaneSubtlety * 0.2f);
            FireThreatMultiplier = threatFactor * (1 - character.MageTalents.BurningSoul * 0.05f);
            FrostThreatMultiplier = threatFactor * (1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            FrostFireThreatMultiplier = threatFactor * (1 - character.MageTalents.BurningSoul * 0.05f) * (1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            NatureThreatMultiplier = threatFactor;
            ShadowThreatMultiplier = threatFactor;
        }

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
                case SpellId.LightningBolt:
                    s = new LightningBolt(this);
                    break;
                case SpellId.ArcaneMissiles:
                    s = new ArcaneMissiles(this, false);
                    break;
                case SpellId.ArcaneMissilesMB:
                    s = new ArcaneMissiles(this, true);
                    break;
                case SpellId.ArcaneMissilesCC:
                    s = new ArcaneMissilesCC(this);
                    break;
                case SpellId.ArcaneMissilesNoProc:
                    s = new ArcaneMissiles(this, false, true, false, false);
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
                case SpellId.FrostboltNoCC:
                    s = new Frostbolt(this, true, false, false);
                    break;
                case SpellId.Fireball:
                    s = new Fireball(this, false);
                    break;
                case SpellId.FrostfireBolt:
                    s = new FrostfireBolt(this, false);
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
                case SpellId.ArcaneBarrage:
                    s = new ArcaneBarrage(this);
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
                case SpellId.ABAM:
                    s = new ABAM(this);
                    break;
                case SpellId.ABMBAM:
                    s = new ABMBAM(this);
                    break;
                case SpellId.ABABar:
                    s = new ABABar(this);
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
                case SpellId.FireballScorch:
                    s = new FireballScorch(this);
                    break;
                case SpellId.FireballFireBlast:
                    s = new FireballFireBlast(this);
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
                    s = new Fireball(this, true);
                    break;
                case SpellId.FrostboltPOM:
                    s = new Frostbolt(this, false, false, true);
                    break;
                case SpellId.PyroblastPOM:
                    s = new Pyroblast(this, true);
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
