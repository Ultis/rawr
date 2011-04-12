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
    public enum ePriestSpec { Spec_Unknown, Spec_Disc, Spec_Holy, Spec_Shadow, Spec_ERROR };
    public static class PriestSpec
    {
        public static ePriestSpec GetPriestSpec(PriestTalents pt)
        {
            int spec = pt.HighestTree;
            if (spec == 0)
                return ePriestSpec.Spec_Disc;
            else if (spec == 1)
                return ePriestSpec.Spec_Holy;
            else if (spec == 2)
                return ePriestSpec.Spec_Shadow;
            return ePriestSpec.Spec_ERROR;
        }
    }
    public static class PriestInformation
    {
        public static float GetInnerFireSpellPowerBonus(Character character)
        {
            float InnerFireSpellPowerBonus = 532;
            return InnerFireSpellPowerBonus;
        }

        public static float GetInnerFireArmorBonus(Character character)
        {
            float InnerFireArmorBonus = 0.6f * (character.PriestTalents.GlyphofInnerFire ? 1.5f : 1f);

            return InnerFireArmorBonus;
        }
       
        public static float GetImprovedPowerWordShield(int points)
        {
            return points * 0.1f;
        }

        public static float GetTwinDisciplines(int points)
        {
            return points * 0.02f;
        }

        public static float GetMentalAgility(int points)
        {
            if (points == 1)
                return 0.97f;
            if (points == 2)
                return 0.94f;
            if (points == 3)
                return 0.90f;
            return 1f;
        }

        public static float GetBorrowedTime(int points)
        {
            if (points == 1)
                return 0.07f;
            if (points == 2)
                return 0.15f;
            return 0.0f;
        }

        public static float GetDivineAegis(int points)
        {
            return points * 0.1f;
        }

        /*                          */
        public static float GetEmpoweredHealing(int points)
        {
            return points * 0.05f;
        }

        public static float GetDivineFury(int points)
        {
            if (points == 1)
                return 0.15f;
            if (points == 2)
                return 0.35f;
            if (points == 3)
                return 0.5f;
            return 0f;
        }

    }

    /*
    public class PriestInformation
    {
        #region Constants
        public int Level { get; protected set; }
        public float BaseMana { get; protected set; }
        public float BaseHitPoints { get; protected set; }
        public float BaseCrit { get; protected set; }
        #endregion
        #region Primary Stats
        public float Strength { get; protected set; }
        public float Agility { get; protected set; }
        public float Intellect { get; protected set; }
        public float Stamina { get; protected set; }
        #endregion
        #region Secondary Stats
        public float Spirit { get; protected set; }
        public float MP5 { get; protected set; }
        public float SpellPower { get; protected set; }
        public float Haste { get; protected set; }
        public float HasteRating { get; protected set; }
        public float CritChance { get; protected set; }
        public float CritRating { get; protected set; }
        public float Mastery { get; protected set; }
        public float MasteryRating { get; protected set; }
        public float Hit { get; protected set; }
        public float HitRating { get; protected set; }
        public float Resilience { get; protected set; }
        public float ResilienceRating { get; protected set; }
        #endregion
        #region Tertiary stats
        public float Armor { get; protected set; }
        public float ArcaneResistance { get; protected set; }
        public float FrostResistance { get; protected set; }
        public float FireResistance { get; protected set; }
        public float NatureResistance { get; protected set; }
        public float ShadowResistance { get; protected set; }
        #endregion

        #region Talent Spec / Masteries
        public bool Discipline { get; protected set; }
        public bool Holy { get; protected set; }
        public bool Shadow { get; protected set; }
        public float Meditation { get; protected set; }
        public float Enlightenment { get; protected set; }
        public float SpiritualHealing { get; protected set; }
        public float ShadowPower { get; protected set; }
        #endregion
        #region Discipline Talent Tiers
            #region Tier 0
            public float ImprovedPowerWordShield_AbsorbIncrease { get; protected set; }
            private static readonly float[] IMPROVEDPOWERWORDSHIELD_ABSORBINCREASE = new float[] { 0.00f, 0.05f, 0.10f };
            public float TwinDisciplines_HolyShadowIncrease { get; protected set; }
            private static readonly float[] TWINDISCIPLINES_HOLYSHADOWINCREASE = new float[] { 0.00f, 0.02f, 0.04f, 0.06f };
            public float MentalAgility_InstantReduction { get; protected set; }
            private static readonly float[] MENTALAGILITY_INSTANTREDUCTION = new float[] { 0.00f, 0.04f, 0.07f, 0.10f };
            #endregion
            #region Tier 1
            public bool Evangelism { get; protected set; }
            public float Evangelism_MaxStack { get; protected set; }
            private static readonly float EVANGELISM_MAXSTACK = 5f;
            public float Evangelism_SmiteProcChance { get; protected set; }
            private static readonly float EVANGELISM_SMITEPROCCHANCE = 1f;
            public float Evangelism_MindFlayProcChance { get; protected set; }
            private static readonly float EVANGELISM_MINDFLAYPROCCHANCE = 0.4f;
            public float Evangelism_HolyDamageIncreasePerStack { get; protected set; }
            private static readonly float[] EVANGELISM_HOLYDAMAGEINCREASEPERSTACK = new float[] { 0.00f, 0.02f, 0.04f };
            public float Evangelism_HolyCostDecreasePerStack { get; protected set; }
            private static readonly float[] EVANGELISM_HOLYCOSTDECREASEPERSTACK = new float[] { 0.00f, 0.03f, 0.06f };
            public float Evangelism_ShadowPeriodicDamageIncreasePerStack { get; protected set; }
            private static readonly float[] EVANGELISM_SHADOWPERIODICDAMAGEINCREASEPERSTACK = new float[] { 0.00f, 0.01f, 0.02f };

            public bool Archangel { get; protected set; }
            public float Archangel_HolyManaRestorePerStack { get; protected set; }
            private static readonly float ARCHANGEL_HOLYMANARESTOREPERSTACK = 0.03f;
            public float Archangel_HolyHealIncreasePerStack { get; protected set; }
            private static readonly float ARCHANGEL_HOLYHEALINCREASEPERSTACK = 0.03f;
            public float Archangel_HolyCooldown { get; protected set; }
            private static readonly float ARCHANGEL_HOLYCOOLDOWN = 30f;
            public float Archangel_HolyDuration { get; protected set; }
            private static readonly float ARCHANGEL_HOLYDURATION = 18f;
            public float Archangel_ShadowManaRestorePerStack { get; protected set; }
            private static readonly float ARCHANGEL_SHADOWMANARESTOREPERSTACK = 0.05f;
            public float Archangel_ShadowDamageIncreasePerStack { get; protected set; }
            private static readonly float ARCHANGEL_SHADOWDAMAGEINCREASEPERSTACK = 0.04f;
            public float Archangel_ShadowCooldown { get; protected set; }
            private static readonly float ARCHANGEL_SHADOWCOOLDOWN = 90f;
            public float Archangel_ShadowDuration { get; protected set; }
            private static readonly float ARCHANGEL_SHADOWDURATION = 18f;
            public float InnerSanctum_InnerFireSpellDamageTakenReduction { get; protected set; }
            private static readonly float[] INNERSANCTUM_INNERFIRESPELLDAMAGETAKENREDUCTION = new float[] { 0.00f, 0.02f, 0.04f, 0.06f };
            public float InnerSanctum_InnerWillMovementBonus { get; protected set; }
            private static readonly float[] INNERSANCTUM_INNERWILLMOVEMENTBONUS = new float[] { 0.00f, 0.02f, 0.04f, 0.06f };
            public float SoulWarding_PowerWordShieldCooldownReduction { get; protected set; }
            private static readonly float[] SOULWARDING_POWERWORDSHIELDCOOLDOWNREDUCTION = new float[] { 0f, 1f, 2f };
            #endregion
            #region Tier 2
            public float RenewedHope_CritChanceIncrease { get; protected set; }
            private static readonly float[] RENEWEDHOPE_CRITCHANCEINCREASE = new float[] { 0.00f, 0.05f, 0.10f };
            public bool PowerInfusion { get; protected set; }
            public float PowerInfusion_SpellCastingIncrease { get; protected set; }
            private static readonly float POWERINFUSION_SPELLCASTINGINCREASE = 0.2f;
            public float PowerInfusion_SpellCostReduction { get; protected set; }
            private static readonly float POWERINFUSION_SPELLCOSTREDUCTION = 0.2f;
            public float Atonement_HealProcRatio { get; protected set; }
            private static readonly float[] ATONEMENT_HEALPROCRATIO = new float[] { 0.00f, 0.40f, 0.80f };
            public bool InnerFocus { get; protected set; }
            public float InnerFocus_CostReduction { get; protected set; }
            private static readonly float INNERFOCUS_COSTREDUCTION = 1f;               
            public float InnerFocus_CritIncrease { get; protected set; }
            private static readonly float INNERFOCUS_CRITINCREASE = 0.25f;
            public float InnerFocus_Cooldown { get; protected set; }
            private static readonly float INNERFOCUS_COOLDOWN = 45f;         
            #endregion
            #region Tier 3
            public float Rapture_ManaGain { get; protected set; }
            private static readonly float[] RAPTURE_MANAGAIN = new float[] { 0.0f, 1.5f, 2.0f, 2.5f };
            public float BorrowedTime_HasteGain { get; protected set; }
            private static readonly float[] BORROWEDTIME_HASTEGAIN = new float[] { 0.00f, 0.07f, 0.14f };
            public float ReflectiveShield_ReflectDamage { get; protected set; }
            private static readonly float[] REFLECTIVESHIELD_REFLECTDAMAGE = new float[] { 0.00f, 0.22f, 0.45f };
            #endregion
            #region Tier 4
            public float StrengthofSoul_WeakenedSoulReduction { get; protected set; }
            private static readonly float[] STRENGTHOFSOUL_WEAKENEDSOULREDUCTION = new float[] { 0f, 2f, 4f };
            public float DivineAegis_Absorb { get; protected set; }
            private static readonly float[] DIVINEAEGIS_ABSORB = new float[] { 0.00f, 0.10f, 0.20f, 0.30f };
            public bool PainSuppression { get; protected set; }
            public float PainSuppression_ThreatReduction { get; protected set; }
            private static readonly float PAINSUPPRESSION_THREATREDUCTION = 0.05f;
            public float PainSuppression_DamageReduction { get; protected set; }
            private static readonly float PAINSUPPRESION_DAMAGEREDUCTION = 0.4f;
            public float PainSuppression_DispelResistance { get; protected set; }
            private static readonly float PAINSUPPRESSION_DISPELRESISTANCE = 0.65f;
            public float TrainofThought_InnerFocusCooldownReduction { get; protected set; }
            private static readonly float TRAINOFTHOUGHT_INNERFOCUSCOOLDOWNREDUCITON = 5f;
            public float TrainofThought_PenanceCooldownReduction { get; protected set; }
            private static readonly float TRAINOFTHOUGHT_PENANCECOOLDOWNREDUCTION = 0.5f;
            public float TrainofThought_ProcChance { get; protected set; }
            private static readonly float[] TRAINOFTHOUGHT_PROCCHANCE = { 0.0f, 0.5f, 1.0f };
            #endregion
            #region Tier 5
            public float FocusedWill_MaxStacks { get; protected set; }
            private static readonly float FOCUSEDWILL_MAXSTACKS = 2f;    
            public float FocusedWill_ProcThreshold { get; protected set; }
            private static readonly float FOCUSEDWILL_PROCTHRESHOLD = 0.1f;
            public float FocusedWill_DamageReduction { get; protected set; }
            private static readonly float[] FOCUSEDWILL_DAMAGEREDUCTION = new float[] { 0.00f, 0.04f, 0.06f };
            public float Grace_MaxStacks { get; protected set; }
            private static readonly float GRACE_MAXSTACKS = 3f;
            public float Grace_Duration { get; protected set; }
            private static readonly float GRACE_DURATION = 15f;
            public float Grace_HealBonus { get; protected set; }
            private static readonly float[] GRACE_HEALBONUS = new float[] { 0.00f, 0.02f, 0.04f };
            #endregion
            #region Tier 6
                public bool PowerWordBarrier { get; protected set; }
            #endregion
        #endregion
        #region Holy Talent Tiers
            #region Tier 0
            public float ImprovedRenew_HealAmount { get; protected set; }
            private static readonly float[] IMPROVEDRENEW_HEALAMOUNT = new float[] { 0.00f, 0.05f, 0.10f };
            public float EmpoweredHealing_HealAmount { get; protected set; }
            private static readonly float[] EMPOWEREDHEALING_HEALAMOUNT = new float[] { 0.00f, 0.05f, 0.10f, 0.15f };
            public float DivineFury_CastReduction { get; protected set; }
            private static readonly float[] DIVINEFURY_CASTREDUCTION = new float[] { 0.00f, 0.15f, 0.35f, 0.50f };
            #endregion
            #region Tier 1
            public bool DesperatePrayer { get; protected set; }
            public float SurgeofLight_ProcChance { get; protected set; }
            private static readonly float[] SURGEOFLIGHT_PROCCHANCE = new float[] { 0.00f, 0.03f, 0.06f };
            public float Inspiration_Duration { get; protected set; }
            private static readonly float INSPIRATION_DURATION = 15f;
            public float Inspiration_DamageReduction { get; protected set; }
            private static readonly float[] INSPIRATION_DAMAGEREDUCTION = new float[] { 0.00f, 0.05f, 0.10f };
            #endregion
            #region Tier 2
            public float DivineTouch_InstantAmount { get; protected set; }
            private static readonly float[] DIVINETOUCH_INSTANTAMOUNT = new float[] { 0.00f, 0.05f, 0.10f };
            public float HolyConcentration_RegenIncrease { get; protected set; }
            private static readonly float[] HOLYCONCENTRATION_REGENINCREASE = new float[] { 0.00f, 0.10f, 0.20f };
            public bool Lightwell { get; protected set; }
            #endregion
            #region Tier 3
            public float Serendipity_MaxStacks { get; protected set; }
            private static readonly float SERENDIPITY_MAXSTACKS = 2f;
            public float Serendipity_Duration { get; protected set; }
            private static readonly float SERENDIPITY_DURATION = 20f;
            public float Serendipity_CastReduction { get; protected set; }
            private static readonly float[] SERENDIPITY_CASTREDUCTION = new float[] { 0.00f, 0.10f, 0.20f };
            public float Serendipity_ManaCostReduction { get; protected set; }
            private static readonly float[] SERENDIPITY_MANACOSTREDUCTION = new float[] { 0.00f, 0.05f, 0.10f };
            public bool SpiritofRedemption { get; protected set; }
            public float SpiritofRedemption_Duration { get; protected set; }
            private static readonly float SPIRITOFREDEMPTION_DURATION = 15f;
            public float TomeofLight_HolyWordCooldownReduction { get; protected set; }
            private static readonly float[] TOMEOFLIGHT_HOLYWORDCOOLDOWNREDUCTION = new float[] { 0.00f, 0.15f, 0.30f };

            #endregion
            #region Tier 4
            public float BodyandSoul_MovementIncreaseAmount { get; protected set; }
            private static readonly float[] BODYANDSOUL_MOVEMENTINCREASEAMOUNT = new float[] { 0.00f, 0.30f, 0.60f };
            public float BodyandSoul_MovementIncreaseDuration { get; protected set; }
            private static readonly float BODYANDSOUL_MOVEMENTINCREASEDURATION = 4f;
            public float BodyandSoul_CurePoisonChance { get; protected set; }
            private static readonly float[] BODYANDSOUL_CUREPOISONCHANCE = new float[] { 0.00f, 0.50f, 1.00f };
            public bool Chakra { get; protected set; }
            public float Chakra_Duration { get; protected set; }
            private static readonly float CHAKRA_DURATION = 30f;
            public float Chakra_Cooldown { get; protected set; }
            private static readonly float CHAKRA_COOLDOWN = 1f* 60f;
            public float Chakra_HealCritChanceIncrease { get; protected set; }
            private static float CHAKRA_HEALCRITCHANCEINCREASE = 0.25f;
            public float Chakra_RenewHealIncrease { get; protected set; }
            private static float CHAKRA_RENEWHEALINCREASE = 0.1f;
            public float Chakra_RenewGlobalCooldownReduction { get; protected set; }
            private static float CHAKRA_RENEWGLOBALCOOLDOWNREDUCTION = 0.5f;
            public float Chakra_PrayerofHealingAOEHealIncrease { get; protected set; }
            private static float CHAKRA_PRAYEROFHEALINGAOEHEALINCREASE = 0.1f; 
            public float Chakra_PrayerofHealingCoHCooldownReduction { get; protected set; }
            private static float CHAKRA_PRAYEROFHEALINGCOHCOOLDOWNREDUCTION = 2f;
            public float Chakra_SmiteDamageIncrease { get; protected set; }
            private static float CHAKRA_SMITEDAMAGEINCREASE = 0.15f;
            public bool Revelations { get; protected set; }
            public float Revelations_HolyWordSerenityCritIncrease { get; protected set; }
            private static float REVELATIONS_HOLYWORDSERENITYCRITINCREASE = 0.25f;
            public float Revelations_HolyWordSerenityDuration { get; protected set; }
            private static float REVELATIONS_HOLYWRODSERENITYDURATION = 6f;
            public float BlessedResilience_ProcThreshold { get; protected set; }
            private static float BLESSEDRESILIENCE_PROCTHRESHOLD = 0.1f;
            public float BlessedResilience_DamageReduction { get; protected set; }
            private static float[] BLESSEDRESILIENCE_DAMAGEREDUCITON = new float[] { 0.00f, 0.05f, 0.10f };
            #endregion
            #region Tier 5
            public bool TestofFaith { get; protected set; }
            public float TestofFaith_ProcThreshold { get; protected set; }
            private static readonly float TESTOFFAITH_PROCTHRESHOLD = 0.5f;
            public float TestofFaith_HealIncrease { get; protected set; }
            private static readonly float[] TESTOFFAITH_HEALINCREASE = new float[] { 0.00f, 0.04f, 0.08f, 0.12f };
            public bool StateofMind { get; protected set; }
            public float StateofMind_ChakraDurationIncrease { get; protected set; }
            private static readonly float[] STATEOFMIND_CHAKRADURATIONINCREASE = new float[] { 0, 2f, 4f };
            public bool CircleofHealing { get; protected set; }
            #endregion
            #region Tier 6
            public bool GuardianSpirit { get; protected set; }
            public float GuardianSpirit_HealIncrease { get; protected set; }
            private static readonly float GUARDIANSPIRIT_HEALINCREASE = 0.40f;
            public float GuardianSpirit_Duration { get; protected set; }
            private static readonly float GUARDIANSPIRIT_DURATION = 10f;
            public float GuardianSpirit_Cooldown { get; protected set; }
            private static readonly float GUARDIANSPIRIT_COOLDOWN = 3f * 60f;
            #endregion
        #endregion
        #region Shadow Talent Tiers
            #region Tier 0
            public float Darkness_Haste { get; protected set; }
            private static readonly float[] DARKNESS_HASTE = new float[] { 0.00f, 0.01f, 0.02f, 0.03f };
            public float ImprovedShadowWordPain_Increase { get; protected set; }
            private static readonly float[] IMPROVEDSHADOWWORDPAIN_INCREASE = new float[] { 0.00f, 0.03f, 0.06f };
            public float VeiledShadows_FadeReduction { get; protected set; }
            private static readonly float[] VEILEDSHADOWS_FADEREDUCTION = new float[] { 0f, 3f, 6f };
            public float VeiledShadows_ShadowfiendReduction { get; protected set; }
            private static readonly float[] VEILEDSHADOWS_SHADOWFIENDREDUCTION = new float[] { 0f, 30f, 60f };
            #endregion
            #region Tier 1
            public float ImprovedPsychicScream_CooldownReduction { get; protected set; }
            private static readonly float[] IMPROVEDPSYCHICSCREAM_COOLDOWNREDUCTION = new float[] { 0f, 2f, 4f };
            public float ImprovedMindBlast_CooldownReduction { get; protected set; }
            private static readonly float[] IMPROVEDMINDBLAST_COOLDOWNREDUCTION = new float[] { 0.0f, 0.5f, 1.0f, 1.5f };
            public float ImprovedMindBlast_HealReductionChance { get; protected set; }
            private static readonly float[] IMPROVEDMINDBLAST_HEALREDUCTIONCHANCE = new float[] { 0.0f, 0.33f, 0.66f, 1.0f };
            public float ImprovedDevouringPlague_InstantDamage { get; protected set; }
            private static readonly float[] IMPROVEDDEVOURINGPLAGUE_INSTANT_DAMAGE = new float[] { 0.00f, 0.15f, 0.30f };
            public float TwistedFaith_SpellDamageBonus { get; protected set; }
            private static readonly float[] TWISTEDFAITH_SPELLDAMAGEBONUS = new float[] { 0.00f, 0.01f, 0.02f };
            public float TwistedFaith_SpellHitFromSpirit { get; protected set; }
            private static readonly float[] TWISTEDFAITH_SPELLHITFROMSPIRIT = new float[] { 0.0f, 0.5f, 1.0f };
            #endregion
            #region Tier 2
                    public float Shadowform0, Phantasm0, HarnessedShadows0;
            #endregion
            #region Tier 3
                    public float Silence0, VampiricEmbrace0, Masochism0, MindMelt0;
            #endregion
            #region Tier 4
                    public float PainandSuffering0, VampiricTouch0, Paralysis0;
            #endregion
            #region Tier 5
                    public float PsychicHorror0, SinandPunishment0, ShadowyApparition0;
            #endregion
            #region Tier 6
                    public float Dispersion0;
            #endregion
        #endregion
        #region Glyphs
            public float Glyph_DispelMagic_HealPercentage { get; protected set; }
            //private static readonly float GLYPH_DISPELMAGIC_HEALPERCENTAGE = 0.30f;
            private static readonly float GLYPH_GUARDIANSPIRIT_COOLDOWN = 30f;
            //private static readonly float GLYPH_SPIRITOFREDEMPTION_DURATION = 6f;
        #endregion
        #region Buffs
        #endregion
        #region Debuffs
        #endregion

        public void SetTalents(PriestTalents Talents)
        {
            #region Discipline calculated values
            #region Tier 0
            ImprovedPowerWordShield_AbsorbIncrease = IMPROVEDPOWERWORDSHIELD_ABSORBINCREASE[Talents.ImprovedPowerWordShield];
            TwinDisciplines_HolyShadowIncrease = TWINDISCIPLINES_HOLYSHADOWINCREASE[Talents.TwinDisciplines];
            MentalAgility_InstantReduction = MENTALAGILITY_INSTANTREDUCTION[Talents.MentalAgility];
            #endregion
            #region Tier 1
            Evangelism = (Talents.Evangelism > 0);
            Evangelism_MaxStack = EVANGELISM_MAXSTACK;
            Evangelism_SmiteProcChance = EVANGELISM_SMITEPROCCHANCE;
            Evangelism_MindFlayProcChance = EVANGELISM_MINDFLAYPROCCHANCE;
            Evangelism_HolyDamageIncreasePerStack = EVANGELISM_HOLYDAMAGEINCREASEPERSTACK[Talents.Evangelism];
            Evangelism_HolyCostDecreasePerStack = EVANGELISM_HOLYCOSTDECREASEPERSTACK[Talents.Evangelism];
            Evangelism_ShadowPeriodicDamageIncreasePerStack = EVANGELISM_SHADOWPERIODICDAMAGEINCREASEPERSTACK[Talents.Evangelism];
            Archangel = (Talents.Archangel > 0);
            Archangel_HolyManaRestorePerStack = ARCHANGEL_HOLYMANARESTOREPERSTACK;
            Archangel_HolyHealIncreasePerStack = ARCHANGEL_HOLYHEALINCREASEPERSTACK;
            Archangel_HolyCooldown = ARCHANGEL_HOLYCOOLDOWN;
            Archangel_HolyDuration = ARCHANGEL_HOLYDURATION;
            Archangel_ShadowManaRestorePerStack = ARCHANGEL_SHADOWMANARESTOREPERSTACK;
            Archangel_ShadowDamageIncreasePerStack = ARCHANGEL_SHADOWDAMAGEINCREASEPERSTACK;
            Archangel_ShadowCooldown = ARCHANGEL_SHADOWCOOLDOWN;
            Archangel_ShadowDuration = ARCHANGEL_SHADOWDURATION;
            InnerSanctum_InnerFireSpellDamageTakenReduction = INNERSANCTUM_INNERFIRESPELLDAMAGETAKENREDUCTION[Talents.InnerSanctum];
            InnerSanctum_InnerWillMovementBonus = INNERSANCTUM_INNERWILLMOVEMENTBONUS[Talents.InnerSanctum];
            SoulWarding_PowerWordShieldCooldownReduction = SOULWARDING_POWERWORDSHIELDCOOLDOWNREDUCTION[Talents.SoulWarding];
            #endregion
            #region Tier 2
            RenewedHope_CritChanceIncrease = RENEWEDHOPE_CRITCHANCEINCREASE[Talents.RenewedHope];
            PowerInfusion = (Talents.PowerInfusion > 0);
            PowerInfusion_SpellCastingIncrease = POWERINFUSION_SPELLCASTINGINCREASE;
            PowerInfusion_SpellCostReduction = POWERINFUSION_SPELLCOSTREDUCTION;
            Atonement_HealProcRatio = ATONEMENT_HEALPROCRATIO[Talents.Atonement];
            InnerFocus = (Talents.InnerFocus > 0);
            InnerFocus_CostReduction = INNERFOCUS_COSTREDUCTION;
            InnerFocus_CritIncrease = INNERFOCUS_CRITINCREASE;
            InnerFocus_Cooldown = INNERFOCUS_COOLDOWN;
            #endregion
            #region Tier 3
            Rapture_ManaGain = RAPTURE_MANAGAIN[Talents.Rapture];
            BorrowedTime_HasteGain = BORROWEDTIME_HASTEGAIN[Talents.BorrowedTime];
            ReflectiveShield_ReflectDamage = REFLECTIVESHIELD_REFLECTDAMAGE[Talents.ReflectiveShield];
            #endregion
            #region Tier 4
            StrengthofSoul_WeakenedSoulReduction = STRENGTHOFSOUL_WEAKENEDSOULREDUCTION[Talents.StrengthOfSoul];
            DivineAegis_Absorb = DIVINEAEGIS_ABSORB[Talents.DivineAegis];
            PainSuppression = (Talents.PainSuppression > 0f);
            PainSuppression_ThreatReduction = PAINSUPPRESSION_THREATREDUCTION;
            PainSuppression_DamageReduction = PAINSUPPRESION_DAMAGEREDUCTION;
            PainSuppression_DispelResistance = PAINSUPPRESSION_DISPELRESISTANCE;
            TrainofThought_InnerFocusCooldownReduction = TRAINOFTHOUGHT_INNERFOCUSCOOLDOWNREDUCITON;
            TrainofThought_PenanceCooldownReduction = TRAINOFTHOUGHT_PENANCECOOLDOWNREDUCTION;
            TrainofThought_ProcChance = TRAINOFTHOUGHT_PROCCHANCE[Talents.TrainOfThought];
            #endregion
            #region Tier 5
            FocusedWill_MaxStacks = FOCUSEDWILL_MAXSTACKS;
            FocusedWill_ProcThreshold = FOCUSEDWILL_PROCTHRESHOLD;
            FocusedWill_DamageReduction = FOCUSEDWILL_DAMAGEREDUCTION[Talents.FocusedWill];
            Grace_MaxStacks = GRACE_MAXSTACKS;
            Grace_Duration = GRACE_DURATION;
            Grace_HealBonus = GRACE_HEALBONUS[Talents.Grace];
            #endregion
            #region Tier 6
            PowerWordBarrier = (Talents.PowerWordBarrier > 0);
            #endregion
            #endregion
            #region Holy calculated values
            #region Tier 0
            ImprovedRenew_HealAmount = IMPROVEDRENEW_HEALAMOUNT[Talents.ImprovedRenew];
            EmpoweredHealing_HealAmount = EMPOWEREDHEALING_HEALAMOUNT[Talents.EmpoweredHealing];
            DivineFury_CastReduction = DIVINEFURY_CASTREDUCTION[Talents.DivineFury];
            #endregion
            #region Tier 1
            DesperatePrayer = (Talents.DesperatePrayer > 0);
            SurgeofLight_ProcChance = SURGEOFLIGHT_PROCCHANCE[Talents.SurgeOfLight];
            Inspiration_Duration = INSPIRATION_DURATION;
            Inspiration_DamageReduction = INSPIRATION_DAMAGEREDUCTION[Talents.Inspiration];
            #endregion
            #region Tier 2
            DivineTouch_InstantAmount = DIVINETOUCH_INSTANTAMOUNT[Talents.DivineTouch];
            HolyConcentration_RegenIncrease = HOLYCONCENTRATION_REGENINCREASE[Talents.HolyConcentration];
            Lightwell = (Talents.Lightwell > 0);
            #endregion
            #region Tier 3
            Serendipity_MaxStacks = SERENDIPITY_MAXSTACKS;
            Serendipity_Duration = SERENDIPITY_DURATION;
            Serendipity_CastReduction = SERENDIPITY_CASTREDUCTION[Talents.Serendipity];
            Serendipity_ManaCostReduction = SERENDIPITY_MANACOSTREDUCTION[Talents.Serendipity];
            SpiritofRedemption = (Talents.SpiritOfRedemption > 0);
            SpiritofRedemption_Duration = SPIRITOFREDEMPTION_DURATION;
            TomeofLight_HolyWordCooldownReduction = TOMEOFLIGHT_HOLYWORDCOOLDOWNREDUCTION[Talents.Atonement];
            #endregion
            #region Tier 4
            BodyandSoul_MovementIncreaseAmount = BODYANDSOUL_MOVEMENTINCREASEAMOUNT[Talents.BodyAndSoul];
            BodyandSoul_MovementIncreaseDuration = BODYANDSOUL_MOVEMENTINCREASEDURATION;
            BodyandSoul_CurePoisonChance = BODYANDSOUL_CUREPOISONCHANCE[Talents.BodyAndSoul];
            Chakra = (Talents.Chakra > 0);
            Chakra_Duration = CHAKRA_DURATION;
            Chakra_Cooldown = CHAKRA_COOLDOWN;
            Chakra_HealCritChanceIncrease = CHAKRA_HEALCRITCHANCEINCREASE;
            Chakra_RenewHealIncrease = CHAKRA_RENEWHEALINCREASE;
            Chakra_RenewGlobalCooldownReduction = CHAKRA_RENEWGLOBALCOOLDOWNREDUCTION;
            Chakra_PrayerofHealingAOEHealIncrease = CHAKRA_PRAYEROFHEALINGAOEHEALINCREASE;
            Chakra_PrayerofHealingCoHCooldownReduction = CHAKRA_PRAYEROFHEALINGCOHCOOLDOWNREDUCTION;
            Chakra_SmiteDamageIncrease = CHAKRA_SMITEDAMAGEINCREASE;
            Revelations = (Talents.Revelations > 0);
            Revelations_HolyWordSerenityCritIncrease = REVELATIONS_HOLYWORDSERENITYCRITINCREASE;
            Revelations_HolyWordSerenityDuration = REVELATIONS_HOLYWRODSERENITYDURATION;
            BlessedResilience_ProcThreshold = BLESSEDRESILIENCE_PROCTHRESHOLD;
            BlessedResilience_DamageReduction = BLESSEDRESILIENCE_DAMAGEREDUCITON[Talents.BlessedResilience];
            #endregion
            #region Tier 5
            TestofFaith = (Talents.TestOfFaith > 0);
            TestofFaith_ProcThreshold = TESTOFFAITH_PROCTHRESHOLD;
            TestofFaith_HealIncrease = TESTOFFAITH_HEALINCREASE[Talents.TestOfFaith];
            StateofMind = (Talents.StateOfMind > 0);
            StateofMind_ChakraDurationIncrease = STATEOFMIND_CHAKRADURATIONINCREASE[Talents.StateOfMind];
            CircleofHealing = (Talents.CircleOfHealing > 0);
            #endregion
            #region Tier 6
            GuardianSpirit = (Talents.GuardianSpirit > 0);
            GuardianSpirit_HealIncrease = GUARDIANSPIRIT_HEALINCREASE;
            GuardianSpirit_Duration = GUARDIANSPIRIT_DURATION;
            GuardianSpirit_Cooldown = GUARDIANSPIRIT_COOLDOWN;
            #endregion
            #endregion
            #region Shadow calculated values
            #region Tier 0
            Darkness_Haste = DARKNESS_HASTE[Talents.Darkness];
            ImprovedShadowWordPain_Increase = IMPROVEDSHADOWWORDPAIN_INCREASE[Talents.Darkness];
            VeiledShadows_FadeReduction = VEILEDSHADOWS_FADEREDUCTION[Talents.VeiledShadows];
            VeiledShadows_ShadowfiendReduction = VEILEDSHADOWS_SHADOWFIENDREDUCTION[Talents.VeiledShadows];
            #endregion
            #region Tier 1
            ImprovedPsychicScream_CooldownReduction = IMPROVEDPSYCHICSCREAM_COOLDOWNREDUCTION[Talents.ImprovedPsychicScream];
            ImprovedMindBlast_CooldownReduction = IMPROVEDMINDBLAST_COOLDOWNREDUCTION[Talents.ImprovedMindBlast];
            ImprovedMindBlast_HealReductionChance = IMPROVEDMINDBLAST_HEALREDUCTIONCHANCE[Talents.ImprovedMindBlast];
            ImprovedDevouringPlague_InstantDamage = IMPROVEDDEVOURINGPLAGUE_INSTANT_DAMAGE[Talents.ImprovedDevouringPlague];
            TwistedFaith_SpellDamageBonus = TWISTEDFAITH_SPELLDAMAGEBONUS[Talents.TwistedFaith];
            TwistedFaith_SpellHitFromSpirit = TWISTEDFAITH_SPELLHITFROMSPIRIT[Talents.TwistedFaith];
            #endregion


            #endregion
            #region Glyphs calculated values
            GuardianSpirit_Cooldown -= Talents.GlyphofGuardianSpirit ? GLYPH_GUARDIANSPIRIT_COOLDOWN : 0f;
            #endregion
        }
    
    }*/

}