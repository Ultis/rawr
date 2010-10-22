using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.HealPriest
{
    public class PriestInformation
    {
        // Constants
        public int level, baseMana;
        // Primary Stats
        public float strength, agility, intellect, stamina;
        // Secondary stats
        public float spirit, mp5, spellPower, haste, critical, mastery, resilience;
        // Tertiary stats
        public float armor, arcaneResistance, frostResistance, fireResistance, natureResistance, shadowResistance;
        // Talents
        public bool Discipline { get; protected set; }
        public bool Holy { get; protected set; }
        public bool Shadow { get; protected set; }
        public float Meditation { get; protected set; }
        public float Enlightenment { get; protected set; }
        public float SpiritualHealing { get; protected set; }
        public float ShadowPower { get; protected set; }

            // Disc, tiers
                // Tier0
                public float ImprovedPowerWordShield_AbsorbIncrease { get; protected set; }
                public float TwinDisciplines_HolyShadowIncrease { get; protected set; }
                public float MentalAgility_InstantReduction { get; protected set; }
                // Tier1
                public bool Evangelism { get; protected set; }
                public float Evangelism_MaxStack { get; protected set; }
                public float Evangelism_SmiteProcChance { get; protected set; }
                public float Evangelism_MindFlayProcChance { get; protected set; }
                public float Evangelism_HolyDamageIncreasePerStack { get; protected set; }
                public float Evangelism_HolyCostDecreasePerStack { get; protected set; }
                public float Evangelism_ShadowPeriodicDamageIncreasePerStack { get; protected set; }
                public bool Archangel { get; protected set; }
                public float Archangel_HolyManaRestorePerStack { get; protected set; }
                public float Archangel_HolyHealIncreasePerStack { get; protected set; }
                public float Archangel_HolyCooldown { get; protected set; }
                public float Archangel_HolyDuration { get; protected set; }
                public float Archangel_ShadowManaRestorePerStack { get; protected set; }
                public float Archangel_ShadowDamageIncreasePerStack { get; protected set; }
                public float Archangel_ShadowCooldown { get; protected set; }
                public float Archangel_ShadowDuration { get; protected set; }
                public float InnerSanctum_InnerFireSpellDamageTakenReduction { get; protected set; }
                public float InnerSanctum_InnerWillMovmentBonus { get; protected set; }
                public float SoulWarding_PowerWordShieldCooldownReduction { get; protected set; }
                // Tier2
                public float RenewedHope_CritChanceIncrease { get; protected set; }
                public float PowerInfusion_SpellCastingIncrease { get; protected set; }
                public float PowerInfusion_SpellCostReduction { get; protected set; }
                public float Atonement_HealProcRatio { get; protected set; }
                public bool InnerFocus { get; protected set; }
                public float InnerFocus_CostReduction { get; protected set; }
                public float InnerFocus_CritIncrease { get; protected set; }
                public float InnerFocus_Cooldown { get; protected set; }
                // Tier3
                public float Rapture_ManaGain { get; protected set; }
                public float BorrowedTime_HasteGain { get; protected set; }
                public float ReflectiveShield_ReflectDamage { get; protected set; }
                // Tier4
                public float StrengthofSoul_WeakenedSoulReduction { get; protected set; }
                public float DivineAegis_Absorb { get; protected set; }
                public bool PainSuppression { get; protected set; }
                public float PainSuppression_ThreatReduction { get; protected set; }
                public float PainSuppression_DamageReduction { get; protected set; }
                public float PainSuppression_DispelResistance { get; protected set; }
                public float TrainofThought_InnerFocusCooldownReduction { get; protected set; }
                public float TrainofThought_PenanceCooldownReduction { get; protected set; }
                public float TrainofThought_ProcChance { get; protected set; }
                // Tier5
                public float FocusedWill_MaxStacks { get; protected set; }
                public float FocusedWill_ProcThreshold { get; protected set; }
                public float FocusedWill_DamageReduction { get; protected set; }
                public float Grace_MaxStacks { get; protected set; }
                public float Grace_Duration { get; protected set; }
                public float Grace_HealBonus { get; protected set; }
                // Tier6
                public bool PowerWordBarrier { get; protected set; }

            // Holy, tiers
                // Tier0
                public float ImprovedRenew_HealAmount { get; protected set; }
                public float EmpoweredHealing_HealAmount { get; protected set; }
                public float DivineFury_CastReduction { get; protected set; }
                // Tier1
                public bool DesperatePrayer { get; protected set; }
                public float SurgeofLight_ProcChance { get; protected set; }
                public float Inspiration_Duration { get; protected set; }
                public float Inspiration_DamageReduction { get; protected set; }
                // Tier2
                public float DivineTouch_InstantAmount { get; protected set; }
                public float HolyConcentration_RegenIncrease { get; protected set; }
                public bool Lightwell { get; protected set; }
                // Tier3               
                public float Serendipity_MaxStacks { get; protected set; }
                public float Serendipity_Duration { get; protected set; }
                public float Serendipity_CastReduction { get; protected set; }
                public float Serendipity_ManaCostReduction { get; protected set; }
                public bool SpiritofRedemption { get; protected set; }
                public float SpiritofRedemption_Duration { get; protected set; }
                public float TomeofLight_HolyWordCooldownReduction { get; protected set; }
                // Tier4
                public float BodyandSoul_MovementIncreaseAmount { get; protected set; }
                public float BodyandSoul_MovementIncreaseDuration { get; protected set; }
                public float BodyandSoul_CurePoisonChance { get; protected set; }
                public bool Chakra { get; protected set; }
                public float Chakra_Duration { get; protected set; }
                public float Chakra_Cooldown { get; protected set; }
                public float Chakra_HealCritChanceIncrease { get; protected set; }
                public float Chakra_RenewHealIncrease { get; protected set; }
                public float Chakra_RenewGlobalCooldownReduction { get; protected set; }
                public float Chakra_PrayerofHealingAOEHealIncrease { get; protected set; }
                public float Chakra_PrayerofHealingCoHCooldownReduction { get; protected set; }
                public float Chakra_SmiteDamageIncrease { get; protected set; }
                public bool Revelations { get; protected set; }
                public float Revelations_HolyWordSerenityCritIncrease { get; protected set; }
                public float Revelations_HolyWordSerenityDuration { get; protected set; }
                public float BlessedResilience_ProcThreshold { get; protected set; }
                public float BlessedResilience_DamageReduction { get; protected set; }
                // Tier5
                public bool TestofFaith { get; protected set; }
                public float TestofFaith_ProcThreshold { get; protected set; }
                public float TestofFaith_HealIncrease { get; protected set; }
                public bool StateofMind { get; protected set; }
                public float StateofMind_ChakraDurationIncrease { get; protected set; }
                public bool CircleofHealing { get; protected set; }
                // Tier6
                public bool GuardianSpirit { get; protected set; }
                public float GuardianSpirit_HealIncrease { get; protected set; }
                public float GuardianSpirit_Duration { get; protected set; }
                public float GuardianSpirit_Cooldown { get; protected set; }

        // Shadow, tiers
        public float Darkness0, ImpShadowWordPain0, VeiledShadows0;
        public float ImprovedPsychicScream0, ImprovedMindBlast0, ImprovedDevouringPlague0, TwistedFaith0;
        public float Shadowform0, Phantasm0, HarnessedShadows0;
        public float Silence0, VampiricEmbrace0, Masochism0, MindMelt0;
        public float PainandSuffering0, VampiricTouch0, Paralysis0;
        public float PsychicHorror0, SinandPunishment0, ShadowyApparition0;
        public float Dispersion0;
        // Glyphs
        public float Glyph_DispelMagic_HealPercentage { get; protected set; }
        // Buffs
        // Debuffs

        public void SetTalents(PriestTalents Talents)
        {
            // Disc
            // Tier0
            ImprovedPowerWordShield_AbsorbIncrease = new float[] { 0.00f, 0.05f, 0.10f }[Talents.ImprovedPowerWordShield];
            TwinDisciplines_HolyShadowIncrease = new float[] { 0.00f, 0.02f, 0.04f, 0.06f }[Talents.TwinDisciplines];
            MentalAgility_InstantReduction = new float[] { 0.00f, 0.04f, 0.07f, 0.10f }[Talents.MentalAgility];
            // Tier1
            Evangelism = (Talents.Evangelism > 0);
            Evangelism_MaxStack = 5f;
            Evangelism_SmiteProcChance = 1f;
            Evangelism_MindFlayProcChance = 0.4f;
//		public static readonly float[] WHITE_MISS_CHANCE_CAP                = new float[] { 0.0500f, 0.0520f, 0.0540f, 0.0800f };

            Evangelism_HolyDamageIncreasePerStack = new float[] { 0.00f, 0.02f, 0.04f }[Talents.Evangelism];
            Evangelism_HolyCostDecreasePerStack = new float[] { 0.00f, 0.03f, 0.06f }[Talents.Evangelism];
            Evangelism_ShadowPeriodicDamageIncreasePerStack = new float[] { 0.00f, 0.01f, 0.02f}[Talents.Evangelism];
            Archangel = (Talents.Archangel > 0);
            Archangel_HolyManaRestorePerStack = 0.03f;
            Archangel_HolyHealIncreasePerStack = 0.03f;
            Archangel_HolyCooldown = 30f;
            Archangel_HolyDuration = 18f;
            Archangel_ShadowManaRestorePerStack = 0.05f;
            Archangel_ShadowDamageIncreasePerStack = 0.04f;
            Archangel_ShadowCooldown = 90f;
            Archangel_ShadowDuration = 18f;
            InnerSanctum_InnerFireSpellDamageTakenReduction = new float[] { 0.00f, 0.02f, 0.04f, 0.06f }[Talents.InnerSanctum];
            InnerSanctum_InnerWillMovmentBonus = new float[] { 0.00f, 0.02f, 0.04f, 0.06f }[Talents.InnerSanctum];
            SoulWarding_PowerWordShieldCooldownReduction = new float[] { 0f, 1f, 2f }[Talents.SoulWarding];
            // Tier2
            RenewedHope_CritChanceIncrease = new float[] { 0.00f, 0.05f, 0.10f }[Talents.RenewedHope];
            PowerInfusion_SpellCastingIncrease = new float[] { 0.00f, 0.20f }[Talents.PowerInfusion];
            PowerInfusion_SpellCostReduction = new float[] { 0.00f, 0.20f }[Talents.PowerInfusion];
            Atonement_HealProcRatio = new float[] { 0.00f, 0.40f, 0.80f }[Talents.Atonement];
            InnerFocus = (Talents.InnerFocus > 0);
            InnerFocus_CostReduction = new float[] { 0f, 1f }[Talents.InnerFocus];
            InnerFocus_CritIncrease = new float[] { 0.00f, 0.25f }[Talents.InnerFocus];
            InnerFocus_Cooldown = 45f;
            // Tier3
            Rapture_ManaGain = new float[] { 0.0f, 1.5f, 2.0f, 2.5f }[Talents.Rapture];
            BorrowedTime_HasteGain = new float[] { 0.00f, 0.07f, 0.14f }[Talents.BorrowedTime];
            ReflectiveShield_ReflectDamage = new float[] { 0.00f, 0.22f, 0.45f }[Talents.ReflectiveShield];
            // Tier4
            StrengthofSoul_WeakenedSoulReduction = new float[] { 0f, 2f, 4f }[Talents.StrengthOfSoul];
            DivineAegis_Absorb = new float[] { 0.00f, 0.10f, 0.20f, 0.30f }[Talents.DivineAegis];
            PainSuppression = (Talents.PainSuppression > 0f);
            PainSuppression_ThreatReduction = 0.05f;
            PainSuppression_DamageReduction = 0.4f;
            PainSuppression_DispelResistance = 0.65f;
            TrainofThought_InnerFocusCooldownReduction = 5f;
            TrainofThought_PenanceCooldownReduction = 0.5f;
            TrainofThought_ProcChance = new float[] { 0.0f, 0.5f, 1.0f }[Talents.Trinity];
            // Tier5
            FocusedWill_MaxStacks = 2f;
            FocusedWill_ProcThreshold = 0.1f;
            FocusedWill_DamageReduction = new float[] { 0.00f, 0.04f, 0.06f }[Talents.FocusedWill];
            Grace_MaxStacks = 3f;
            Grace_Duration = 15f;
            Grace_HealBonus = new float[] { 0.00f, 0.02f, 0.04f }[Talents.Grace];
            // Tier6
            PowerWordBarrier = (Talents.PowerWordBarrier > 0);

            // Holy
            // Tier0
            ImprovedRenew_HealAmount = new float[] { 0.00f, 0.05f, 0.10f }[Talents.ImprovedRenew];
            EmpoweredHealing_HealAmount = new float[] { 0.00f, 0.05f, 0.10f, 0.15f }[Talents.EmpoweredHealing];
            DivineFury_CastReduction = new float[] { 0.00f, 0.15f, 0.35f, 0.50f }[Talents.DivineFury];
            // Tier1
            DesperatePrayer = (Talents.DesperatePrayer > 0);
            SurgeofLight_ProcChance = new float[] { 0.00f, 0.03f, 0.06f }[Talents.SurgeOfLight];
            Inspiration_Duration = 15f;
            Inspiration_DamageReduction = new float[] { 0.00f, 0.05f, 0.10f }[Talents.Inspiration];
            // Tier2
            DivineTouch_InstantAmount = new float[] { 0.00f, 0.05f, 0.10f }[Talents.DivineTouch];
            HolyConcentration_RegenIncrease = new float[] { 0.00f, 0.10f, 0.20f }[Talents.HolyConcentration];
            Lightwell = (Talents.Lightwell > 0);
            // Tier3
            Serendipity_MaxStacks = 2f;
            Serendipity_Duration = 20f;
            Serendipity_CastReduction = new float[] { 0.00f, 0.10f, 0.20f }[Talents.Serendipity];
            Serendipity_ManaCostReduction = new float[] { 0.00f, 0.05f, 0.10f }[Talents.Serendipity];
            SpiritofRedemption = (Talents.SpiritOfRedemption > 0);
            SpiritofRedemption_Duration = 15f;
            TomeofLight_HolyWordCooldownReduction = new float[] { 0.00f, 0.15f, 0.30f }[Talents.Atonement];
            // Tier4
            BodyandSoul_MovementIncreaseAmount = new float[] { 0.00f, 0.30f, 0.60f }[Talents.BodyAndSoul];
            BodyandSoul_MovementIncreaseDuration = 4f;
            BodyandSoul_CurePoisonChance = new float[] { 0.00f, 0.50f, 1.00f }[Talents.BodyAndSoul];
            Chakra = (Talents.Chakra > 0);
            Chakra_Duration = 30f;
            Chakra_Cooldown = 1f * 60f;
            Chakra_HealCritChanceIncrease = 0.25f;
            Chakra_RenewHealIncrease = 0.1f;
            Chakra_RenewGlobalCooldownReduction = 0.5f;
            Chakra_PrayerofHealingAOEHealIncrease = 0.1f;
            Chakra_PrayerofHealingCoHCooldownReduction = 2f;
            Chakra_SmiteDamageIncrease = 0.15f;
            Revelations = (Talents.Revelations > 0);
            Revelations_HolyWordSerenityCritIncrease = 0.25f;
            Revelations_HolyWordSerenityDuration = 6f;
            BlessedResilience_ProcThreshold = 0.1f;
            BlessedResilience_DamageReduction = new float[] { 0.00f, 0.05f, 0.10f }[Talents.BlessedResilience];
            // Tier5
            TestofFaith = (Talents.TestOfFaith > 0);
            TestofFaith_ProcThreshold = 0.1f;
            TestofFaith_HealIncrease = new float[] { 0.00f, 0.04f, 0.08f, 0.12f }[Talents.TestOfFaith];
            StateofMind = (Talents.StateOfMind > 0);
            StateofMind_ChakraDurationIncrease = new float[] { 0, 2f, 4f }[Talents.StateOfMind];
            CircleofHealing = (Talents.CircleOfHealing > 0);
            // Tier6
            GuardianSpirit = (Talents.GuardianSpirit > 0);
            GuardianSpirit_HealIncrease = 0.40f;
            GuardianSpirit_Duration = 10f;
            GuardianSpirit_Cooldown = 3f * 60f;
            

            // Glyphs
            GuardianSpirit_Cooldown -= Talents.GlyphofGuardianSpirit ? 30f : 0f;
            SpiritofRedemption_Duration += Talents.GlyphofSpiritofRedemption ? 6f : 0f;
        }

    }
}
