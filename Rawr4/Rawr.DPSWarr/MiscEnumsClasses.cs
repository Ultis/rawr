/**********
 * Owner: Shared
 **********/
using System;

namespace Rawr.DPSWarr
{
    public enum SpecialEffectDataType { AverageStats, Uptime };
    public enum AttackTableSelector { Missed = 0, Dodged, Parried, Blocked, Critical, Glance, Hit }
    public enum StatType { Unbuffed, Buffed, Average, Maximum };
    public enum SwingResult { Attempt = 0, Land, Critical, Parry, Dodge };
    public enum Hand { MH = 0, OH, Both };
    public enum AttackType { Yellow = 0, White, Both };
    public enum Maintenance
    {
        _RageGen__ = 0,
        StartWithCharge,
        BerserkerRage,
        DeadlyCalm,
        _Maintenance__,
        ShoutChoice,
        BattleShout,
        CommandingShout,
        DemoralizingShout,
        SunderArmor,
        ThunderClap,
        Hamstring,
        _Periodics__,
        ShatteringThrow,
        SweepingStrikes,
        DeathWish,
        Recklessness,
        EnragedRegeneration,
        _DamageDealers__,
        Fury_,
        Whirlwind,
        Bloodthirst,
        Bloodsurge,
        RagingBlow,
        Arms_,
        Bladestorm,
        MortalStrike,
        Rend,
        Overpower,
        TasteForBlood,
        ColossusSmash,
        VictoryRush,
        Slam,
        ExecuteSpam,
        ExecuteSpamStage2,
        _RageDumps__,
        Cleave,
        HeroicStrike,
        InnerRage,
    };

    public struct DPSWarrCharacter
    {
        public Character Char;
        public Rotation Rot;
        public CombatFactors CombatFactors;
        public CalculationOptionsDPSWarr CalcOpts;
        public BossOptions BossOpts;
        public WarriorTalents Talents;
        public Skills.WhiteAttacks Whiteattacks;
        public Stats StatS;
        // Equality overrides
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is DPSWarrCharacter))
                return false;

            return Equals((DPSWarrCharacter)obj);
        }
        public bool Equals(DPSWarrCharacter other)
        {
            if (Char != other.Char)
                return false;
            if (Rot != other.Rot)
                return false;
            if (CombatFactors != other.CombatFactors)
                return false;
            if (CalcOpts != other.CalcOpts)
                return false;
            if (BossOpts != other.BossOpts)
                return false;
            if (Talents != other.Talents)
                return false;

            return true;
        }
        public static bool operator ==(DPSWarrCharacter dpswc1, DPSWarrCharacter dpswc2)
        {
            return dpswc1.Equals(dpswc2);
        }
        public static bool operator !=(DPSWarrCharacter dpswc1, DPSWarrCharacter dpswc2)
        {
            return !dpswc1.Equals(dpswc2);
        }
    }

    public class AbilityWrapper
    {
        public AbilityWrapper(Skills.Ability ability)
        {
            Ability = ability;
            _isDamaging = Ability.DamageOverride + Ability.DamageOnUseOverride > 0f;
        }
        public Skills.Ability Ability { get; set; }
        protected bool _isDamaging = false;
        public bool IsDamaging { get { return _isDamaging; } protected set { _isDamaging = value; } }
        // Over 20%
        private float _numActivatesO20 = 0f;
        public float NumActivatesO20
        {
            get { return _numActivatesO20; }
            set { if (_numActivatesO20 != value) { _numActivatesO20 = value; gcdUsageO20 = -1f; } }
        }
        public float RageO20 { get { return Ability.RageCost == -1 ? 0f : Ability.GetRageUseOverDur(NumActivatesO20); } }
        public float DPS_O20 { get { return Ability.GetDPS(NumActivatesO20, Ability.TimeOver20Perc); } }
        public float HPS_O20 { get { return Ability.GetHPS(NumActivatesO20, Ability.TimeOver20Perc); } }
        private float gcdUsageO20 = -1f;
        public float GCDUsageO20
        {
            get
            {
                if (gcdUsageO20 == -1f && Ability.UsesGCD)
                {
                    gcdUsageO20 = NumActivatesO20 * (Ability.UseTime / LatentGCD);
                }
                return gcdUsageO20;
            }
        }
        // Under 20%
        private float _numActivatesU20 = 0f;
        public float NumActivatesU20
        {
            get { return _numActivatesU20; }
            set { if (_numActivatesU20 != value) { _numActivatesU20 = value; gcdUsageU20 = -1f; } }
        }
        public float RageU20 { get { return Ability.RageCost == -1 ? 0f : Ability.GetRageUseOverDur(NumActivatesU20); } }
        public float DPS_U20 { get { return Ability.GetDPS(NumActivatesU20, Ability.TimeUndr20Perc); } }
        public float HPS_U20 { get { return Ability.GetHPS(NumActivatesU20, Ability.TimeUndr20Perc); } }
        private float gcdUsageU20 = -1f;
        public float GCDUsageU20
        {
            get
            {
                if (gcdUsageU20 == -1f && Ability.UsesGCD)
                {
                    gcdUsageU20 = NumActivatesU20 * (Ability.UseTime / LatentGCD);
                }
                return gcdUsageU20;
            }
        }
        private static float _cachedLatentGCD = 1.5f;
        public static float LatentGCD { get { return _cachedLatentGCD; } set { _cachedLatentGCD = value; } }
        // Total
        public float AllNumActivates { get { return NumActivatesO20 + NumActivatesU20; } }
        public float AllRage { get { return RageO20 + RageU20; } }
        public float AllDPS
        {
            get
            {
                float dpsO20 = DPS_O20;
                float dpsU20 = DPS_U20;
                if (dpsO20 > 0 && dpsU20 > 0) { return dpsO20 * Ability.TimeOver20Perc + dpsU20 * Ability.TimeUndr20Perc; }
                if (dpsU20 > 0) { return dpsU20; }
                if (dpsO20 > 0) { return dpsO20; }
                return 0f;
                //return DPSO20 + DPSU20 > 0 ? (DPSO20 + DPSU20) / 2f : DPSU20 > 0 ? DPSU20 : DPSO20;
            }
        }
        public float AllHPS
        {
            get
            {
                float hpsO20 = HPS_O20;
                float hpsU20 = HPS_U20;
                if (hpsO20 > 0 && hpsU20 > 0) { return hpsO20 * Ability.TimeOver20Perc + hpsU20 * Ability.TimeUndr20Perc; }
                if (hpsU20 > 0) { return hpsU20; }
                if (hpsO20 > 0) { return hpsO20; }
                return 0f;
                //return HPSO20 + HPSU20 > 0 ? (HPSO20 + HPSU20) / 2f : HPSO20;
            }
        }

        public override string ToString()
        {
            if (Ability == null) return "NULLed";
            return string.Format("{0} : Rage {1:0.##} : DPS {2:0.##} : HPS {3:0.##}",
                Ability.Name, AllRage == 0 ? "<None>" : string.Format("{0:0.##}", AllRage), AllDPS, AllHPS);
        }
    }

    public static class HelperFunctions
    {
        public static bool ValidatePlateSpec(DPSWarrCharacter dpswarchar)
        {
            // Null Check
            if (dpswarchar.Char == null) { return false; }
            // Item Type Fails
            if (dpswarchar.Char.Head == null || dpswarchar.Char.Head.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Shoulders == null || dpswarchar.Char.Shoulders.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Chest == null || dpswarchar.Char.Chest.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Wrist == null || dpswarchar.Char.Wrist.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Hands == null || dpswarchar.Char.Hands.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Waist == null || dpswarchar.Char.Waist.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Legs == null || dpswarchar.Char.Legs.Type != ItemType.Plate) { return false; }
            if (dpswarchar.Char.Feet == null || dpswarchar.Char.Feet.Type != ItemType.Plate) { return false; }
            // If it hasn't failed by now, it must be good
            return true;
        }
        public static bool ValidateSMTBonus(DPSWarrCharacter dpswarchar)
        {
            // Null Check
            if (dpswarchar.Char == null) { return false; }
            if (dpswarchar.Char.MainHand == null || dpswarchar.Char.OffHand == null) { return false; }
            // Item Type Fails
            if (dpswarchar.Char.MainHand.Type != ItemType.OneHandAxe
                && dpswarchar.Char.MainHand.Type != ItemType.OneHandSword
                && dpswarchar.Char.MainHand.Type != ItemType.OneHandMace) { return false; }
            if (dpswarchar.Char.OffHand.Type != ItemType.OneHandAxe
                && dpswarchar.Char.OffHand.Type != ItemType.OneHandSword
                && dpswarchar.Char.OffHand.Type != ItemType.OneHandMace) { return false; }
            // If it hasn't failed by now, it must be good
            return true;
        }
    }

    public static class TalentsAsSpecialEffects
    {
        // We need these to be static so they aren't re-created 50 bajillion times

        #region Static Lists, ones that don't need special handling
        #region Drums of War
        public static float[] DrumsOfWarRageCosts = new float[] { 10, 5, -1 };
        #endregion
        #region Bloodsurge
        public static SpecialEffect[] Bloodsurge = {
            null,
            new SpecialEffect(Trigger.MortalStrikeHit, null, 10, 3, 0.10f * 1f),
            new SpecialEffect(Trigger.MortalStrikeHit, null, 10, 3, 0.10f * 2f),
            new SpecialEffect(Trigger.MortalStrikeHit, null, 10, 3, 0.10f * 3f),
        };
        #endregion
        #region Incite
        public static SpecialEffect[] Incite = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 1f), // actual trigger is HS Crit but no need to make one
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 2f),
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 3f),
        };
        #endregion
        #region Battle Shout (Booming Voice & Glyph of Battle)
        /// <summary>2d Array,  Glyph of Battle 0-1, Booming Voice 0-2, Cata no longer has Comm Presence</summary>
        public static SpecialEffect[/*Glyph:0-1*/][/*boomVoice:0-2*/] BattleShout = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 549f, Agility = 549f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
        };
        #endregion
        #region Commanding Shout (Booming Voice & Glyph of Command)
        /// <summary>2d Array, Glyph of Command 0-1, Booming Voice 0-2, Cata no longer has Comm Presence</summary>
        public static SpecialEffect[/*Glyph:0-1*/][/*boomVoice:0-2*/] CommandingShout = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 584f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
        };
        #endregion
        #region Demoralizing Shout
        public static SpecialEffect[/*Glyph of Demoralizing Shout*/] DemoralizingShout = {
            new SpecialEffect(Trigger.Use, new Stats() { BossPhysicalDamageDealtMultiplier = -0.10f, }, 30f + (false ? 15f : 0f), 1.5f),
            new SpecialEffect(Trigger.Use, new Stats() { BossPhysicalDamageDealtMultiplier = -0.10f, }, 30f + (true  ? 15f : 0f), 1.5f),
        };
        #endregion
        #region Recklessness, Shattering Throw, ThunderClap, Sunder Armor, Sweeping Strikes
        //public static Dictionary<float, SpecialEffect> _SE_Recklessness = new Dictionary<float, SpecialEffect>();
        //public static Dictionary<float, SpecialEffect> _SE_ShatteringThrow = new Dictionary<float, SpecialEffect>();
        //public static Dictionary<float, SpecialEffect> _SE_ThunderClap = new Dictionary<float, SpecialEffect>();
        //public static Dictionary<float, SpecialEffect> _SE_SunderArmor = new Dictionary<float, SpecialEffect>();
        //public static Dictionary<float, SpecialEffect> _SE_SweepingStrikes = new Dictionary<float, SpecialEffect>();
        #endregion
        #region Berserker Rage
        public static SpecialEffect BerserkerRage = new SpecialEffect(Trigger.Use, null, 10f, 30f);
        #endregion
        #region Battle Trance
        public static SpecialEffect[] BattleTrance = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 1f),
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 2f),
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 3f),
        };
        #endregion
        #region Wrecking Crew
        public static SpecialEffect[] WreckingCrew = {
            null,
            new SpecialEffect(Trigger.MortalStrikeCrit, new Stats() { BonusDamageMultiplier = 1 * (0.10f/3f), }, 12, 0, 1f * (1f/3f)),
            new SpecialEffect(Trigger.MortalStrikeCrit, new Stats() { BonusDamageMultiplier = 2 * (0.10f/3f), }, 12, 0, 2f * (1f/3f)),
            new SpecialEffect(Trigger.MortalStrikeCrit, new Stats() { BonusDamageMultiplier = 3 * (0.10f/3f), }, 12, 0, 3f * (1f/3f)),
        };
        #endregion
        #region Lambs to the Slaughter
        public static SpecialEffect[] LambsToTheSlaughter = {
            null,
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 1 * 0.10f, }, 3.0f, 0),
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 2 * 0.10f, }, 3.0f, 0),
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 3 * 0.10f, }, 3.0f, 0),
        };
        public static SpecialEffect[] LambsToTheSlaughterPTR = {
            null,
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 0.10f, }, 3.0f, 0, 1f, 1),
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 0.10f, }, 3.0f, 0, 1f, 2),
            new SpecialEffect(Trigger.MortalStrikeHit, new Stats() { BonusExecOPMSDamageMultiplier = 0.10f, }, 3.0f, 0, 1f, 3),
        };
        #endregion
        #region Blood Frenzy
        public static SpecialEffect[] BloodFrenzy = { // This is just the Bonus Rage of the talent, the rest is modelled as static on another part
            null,
            new SpecialEffect(Trigger.WhiteAttack, new Stats() { BonusRageGen = 20f, }, 0, 0, 1 * 0.05f),
            new SpecialEffect(Trigger.WhiteAttack, new Stats() { BonusRageGen = 20f, }, 0, 0, 2 * 0.05f),
        };
        #endregion
        #region Blood Craze
        public static SpecialEffect[] BloodCraze = {
            null,
            new SpecialEffect(Trigger.DamageTaken, new Stats() { HealthRestoreFromMaxHealth = 0.01f * 1f, }, 0f, 0f, 0.10f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { HealthRestoreFromMaxHealth = 0.01f * 2f, }, 0f, 0f, 0.10f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { HealthRestoreFromMaxHealth = 0.02f * 3f, }, 0f, 0f, 0.10f),
        };
        #endregion
        #region Executioner
        public static SpecialEffect[] Executioner = {
            null,
            new SpecialEffect(Trigger.ExecuteHit, new Stats() { PhysicalHaste = 0.05f, }, 9, 0, 0.50f * 1f, 5),
            new SpecialEffect(Trigger.ExecuteHit, new Stats() { PhysicalHaste = 0.05f, }, 9, 0, 0.50f * 2f, 5),
        };
        #endregion

        public static SpecialEffect ColossusSmash = new SpecialEffect(Trigger.ColossusSmashHit, new Stats() { ArmorPenetration = 1.0f }, 6f, 0f);

        public static SpecialEffect[] MeatCleaver = {
            null, //0 Talents
            new SpecialEffect(Trigger.WWorCleaveHit, new Stats() { BonusCleaveWWDamageMultiplier = 1f * 0.05f, }, 10f, 0f, 1f, 3),
            new SpecialEffect(Trigger.WWorCleaveHit, new Stats() { BonusCleaveWWDamageMultiplier = 2f * 0.05f, }, 10f, 0f, 1f, 3)
        };
        #endregion

        #region Functions that take in something to affect the SpecialEffect needing to be returned
        public static SpecialEffect GetDeathWishWithMastery(float masteryVal, DPSWarrCharacter dpswarrchar) {
            SpecialEffect retVal = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusDamageMultiplier = 0.20f * (1f + masteryVal), DamageTakenMultiplier = (dpswarrchar.Talents.GlyphOfDeathWish ? 0f : 0.05f), },
                    30f, 3f * 60f * (1f - 0.10f * dpswarrchar.Talents.IntensifyRage));
            return retVal;
        }
        public static SpecialEffect GetEnragedRegenerationWithMastery(float masteryVal, DPSWarrCharacter dpswarrchar) {
            SpecialEffect retVal = new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { BonusDamageMultiplier = 0.10f / 3f * dpswarrchar.Talents.Enrage * (1f + masteryVal), },
                    9f, 0f, 0.03f * 3f);
            return retVal;
        }
        #endregion
    }
}