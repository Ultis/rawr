using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class Rotation {
        // Constructors
        public Rotation(Character character, WarriorTalents talents, Stats stats, CombatFactors combatFactors, Skills.WhiteAttacks whiteStats) {
            Char = character;
            Talents = talents;
            StatS = stats;
            CombatFactors = combatFactors;
            WhiteAtks = whiteStats;
            CalcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
        }
        #region Variables
        private Character CHARACTER;
        private WarriorTalents TALENTS;
        private Stats STATS;
        private CombatFactors COMBATFACTORS;
        private Skills.WhiteAttacks WHITEATTACKS;
        private CalculationOptionsDPSWarr CALCOPTS;

        public Skills.Slam SL;
        public Skills.Rend RD;
        public Skills.MortalStrike MS;
        public Skills.OverPower OP;
        public Skills.Swordspec SS;
        public Skills.SweepingStrikes SW;
        public Skills.Bladestorm BLS;
        public Skills.Suddendeath SD;
        public Skills.BloodThirst BT;
        public Skills.WhirlWind WW;
        public Skills.HeroicStrike HS;
        public Skills.Cleave CL;
        public Skills.BloodSurge BS;
        public Skills.DeepWounds DW;
        public Skills.ThunderClap TH;
        
        public const float ROTATION_LENGTH_FURY = 8.0f;
        public const float ROTATION_LENGTH_ARMS_GLYPH = 42.0f;
        public const float ROTATION_LENGTH_ARMS_NOGLYPH = 30.0f;
        #endregion
        #region Get/Set
        public Character Char { get { return CHARACTER; } set { CHARACTER = value; } }
        public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
        public Stats StatS { get { return STATS; } set { STATS = value; } }
        public CombatFactors CombatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
        public Skills.WhiteAttacks WhiteAtks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
        public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
        #endregion
        // Functions
        public void Initialize(CharacterCalculationsDPSWarr calcs) {
            initAbilities();
            doIterations();

            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; }

            WHITEATTACKS.Ovd_Freq = Which.GetActivates() * COMBATFACTORS.MainHandSpeed / Which.GetRotation();
            calcs.WhiteDPSMH = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.CalcMhWhiteDPS());
            calcs.WhiteDPSOH = (CHARACTER.OffHand == null ? 0f : WHITEATTACKS.CalcOhWhiteDPS());
            calcs.WhiteDPS = calcs.WhiteDPSMH + calcs.WhiteDPSOH;
            calcs.WhiteDmg = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhAvgSwingDmg());
            WHITEATTACKS.Ovd_Freq = 0;

            calcDeepWounds();

            calcs.SL = SL;
            calcs.RD = RD;
            calcs.MS = MS;
            calcs.OP = OP;
            calcs.SS = SS;
            calcs.SW = SW;
            calcs.BLS = BLS;
            calcs.SD = SD;
            calcs.BT = BT;
            calcs.WW = WW;
            calcs.HS = HS;
            calcs.CL = CL;
            calcs.BS = BS;
            calcs.DW = DW;
            calcs.TH = TH;
        }
        public void Initialize() {initAbilities();doIterations();calcDeepWounds();}

        private void initAbilities() {
            SL = new Skills.Slam(           CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(           CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            MS = new Skills.MortalStrike(   CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SW = new Skills.SweepingStrikes(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BLS= new Skills.Bladestorm(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SD = new Skills.Suddendeath(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BT = new Skills.BloodThirst(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            WW = new Skills.WhirlWind(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(   CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CL = new Skills.Cleave(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BS = new Skills.BloodSurge(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            DW = new Skills.DeepWounds(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            TH = new Skills.ThunderClap(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            
            SD.FreeRage = freeRage();
            
            BS.Slam = SL;
            BS.Whirlwind = WW;
            BS.Bloodthirst = BT;
            
            BLS.Whirlwind = WW;

            SS.Slam = SL;
            SS.Overpower = OP;
            SS.MortalStrike = MS;
            SS.SuddenDeath = SD;
        }
        private void doIterations() {
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };

            // Fury Iteration
            Which.OverridesPerSec = GetOvdActivates(Which);
            float oldActivates = 0.0f, newHSActivates = Which.GetActivates();
            while (CALCOPTS.FuryStance && Math.Abs(newHSActivates - oldActivates) > 0.01f) {
                oldActivates = Which.GetActivates();
                BS.hsActivates = oldActivates;
                _bloodsurgeRPS = Which.bloodsurgeRPS = Which.GetRageUsePerSecond();
                Which.OverridesPerSec = GetOvdActivates(Which);
                newHSActivates = Which.GetActivates();
            }
            
            // Arms Iteration
            float oldSDActivates = 0.0f, newSDActivates = SD.GetActivates();
            while (!CALCOPTS.FuryStance && Math.Abs(newSDActivates - oldSDActivates) > 0.01f) {
                oldSDActivates = SD.GetActivates();
                SD.LandedAtksPerSec = GetLandedAtksPerSec();
                newSDActivates = SD.GetActivates();
            }
        }

        private void calcDeepWounds() {
            DW = new Skills.DeepWounds(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
//            float MHAbilityActivates = SL.GetActivates() + MS.GetActivates() +
//                OP.GetActivates() + /*SW.GetActivates() + SS.GetActivates()*/ +
//                BLS.GetActivates() * 6f + BS.GetActivates() + SD.GetActivates() +
//                BT.GetActivates() + WW.GetActivates();
            float MHAbilityActivates = _SL_GCDs + _MS_GCDs +
                _OP_GCDs + /*SW.GetActivates() + SS.GetActivates()*/ +
                _BLS_GCDs *6 + BS.GetActivates() + _SD_GCDs +
                BT.GetActivates() + WW.GetActivates();
            float OHAbilityActivates = 0f;
            if (CHARACTER.OffHand != null) { OHAbilityActivates = WW.GetActivates() + BLS.GetActivates() * 6f; }
            DW.SetAllAbilityActivates(MHAbilityActivates, OHAbilityActivates);
            _DW_PerHit = DW.GetTickSize();
            _DW_DPS = DW.GetDPS();
        }

        private float GetOvdActivates(Skills.OnAttack Which) { return freeRage() / Which.FullRageCost; }
        private float GetLandedAtksPerSecNoSS() {
            float MS_Acts = MS.GetActivates();
            float OP_Acts = OP.GetActivates();
            float SD_Acts = SD.GetActivates();
            float SL_Acts = MS.GetRotation() / 1.5f - MS.GetActivates() - OP.GetActivates() - SD.GetActivates();

            float Dable = MS_Acts + SD_Acts + SL_Acts;
            float nonDable = OP_Acts;

            float white = (COMBATFACTORS.ProbMhWhiteHit + COMBATFACTORS.MhCrit + COMBATFACTORS.GlanceChance)
                * (COMBATFACTORS.MainHand.Speed / COMBATFACTORS.TotalHaste);

            float ProbYellowHit = (1f - COMBATFACTORS.WhiteMissChance - COMBATFACTORS.MhDodgeChance);
            float ProbYellowHitOP = (1f - COMBATFACTORS.WhiteMissChance);

            float result = white + (Dable * ProbYellowHit) + (nonDable * ProbYellowHitOP);

            return result;
        }
        public float GetLandedAtksPerSec() {return GetLandedAtksPerSecNoSS();}// TODO: add swordspec to this
        public float GetCritHsSlamPerSec() {
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };
            float MS_Acts = MS.GetActivates();
            float OP_Acts = OP.GetActivates();
            float SD_Acts = SD.GetActivates();
            float HS_Acts = Which.GetActivates();
            float SL_Acts = MS.GetRotation() / 1.5f - MS_Acts - OP_Acts - SD_Acts;

            float result = COMBATFACTORS.MhYellowCrit * (SL_Acts / Which.GetRotation() + HS_Acts / Which.GetRotation());

            return result;
        }

        #region Rage Calcs
        public virtual float AngerManagementRagePerSec() { return Talents.AngerManagement / 3.0f; }
        public virtual float ImprovedBerserkerRagePerSec() { return Talents.ImprovedBerserkerRage * 10 / (30 * (1 - 1.0f / 9.0f * Talents.IntensifyRage)); }
        public virtual float OtherRageGenPerSec() {
            float rage = AngerManagementRagePerSec() + ImprovedBerserkerRagePerSec() + BloodRageGain();

            // 4pcT7
            if (StatS.DreadnaughtBonusRageProc != 0f) {
                rage += 5.0f * 0.1f * ((Talents.DeepWounds > 0f ? 1f : 0f) + (!CalcOpts.FuryStance ? 1f / 3f : 0f));
            }

            return rage;
        }
        public virtual float neededRagePerSecond() {
            float BTRage = BT.GetRageUsePerSecond();
            float WWRage = WW.GetRageUsePerSecond();
            float MSRage = MS.GetRageUsePerSecond();
            float OPRage = OP.GetRageUsePerSecond();
            float SDRage = SD.GetRageUsePerSecond();
            float SlamRage = SL.GetRageUsePerSecond();
            float BloodSurgeRage = _bloodsurgeRPS;// BS.GetRageUsePerSecond();
            float BladestormRage = BLS.GetRageUsePerSecond();
            float SweepingRage = SW.GetRageUsePerSecond();
            float RendRage = RD.GetRageUsePerSecond();
            // Total
            float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage +
                BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
            return rage;
        }
        public virtual float neededRage() {
            float BTRage = BT.GetRageUsePerSecond();
            float WWRage = WW.GetRageUsePerSecond();
            float MSRage = MS.GetRageUsePerSecond();
            float OPRage = OP.GetRageUsePerSecond();
            float SDRage = SD.GetRageUsePerSecond();
            float SlamRage = SL.GetRageUsePerSecond();
            float BloodSurgeRage = BS.GetRageUsePerSecond();
            float BladestormRage = BLS.GetRageUsePerSecond();
            float SweepingRage = SW.GetRageUsePerSecond();
            float RendRage = RD.GetRageUsePerSecond();
            // Total
            float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage
                + BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
            return rage;
        }
        public virtual float freeRage() {
            if (Char.MainHand == null) { return 0f; }
            float white = WHITEATTACKS.whiteRageGenPerSec();
            float other = OtherRageGenPerSec();
            float death = SD.GetRageUsePerSecond();
            float needy = neededRagePerSecond();
            return white + other + death - needy;
        }
        public virtual float BloodRageGain() { return (20f * (1f + 0.25f * Talents.ImprovedBloodrage)) / (60f * (1f - 1.0f / 9.0f * Talents.IntensifyRage)); }
        public virtual float AngerManagementGain() { return Talents.AngerManagement / 3.0f; }
        public virtual float ImprovedBerserkerRage() { return Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 0.11f * Talents.IntensifyRage)); }
        public virtual float UnbridledWrathGain() { return Talents.UnbridledWrath * 3.0f / 60.0f; }
        public virtual float OtherRage() {
            if (Char.MainHand == null) { return 0f; }
            float rage = (14.0f / 8.0f * (1 + CombatFactors.MhCrit - (1.0f - CombatFactors.ProbMhWhiteHit)));
            if (CombatFactors.OffHand != null && CombatFactors.OffHand.DPS > 0 &&
                (CombatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                rage += 7.0f / 8.0f * (1 + CombatFactors.OhCrit - (1.0f - CombatFactors.ProbOhWhiteHit));
            rage *= CombatFactors.TotalHaste;
            rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
            rage *= 1 + Talents.EndlessRage * 0.25f;

            return rage;
        }
        #endregion

        #region FuryRotVariables
        float _bloodsurgeRPS;

        #endregion

        public float Latency()
        {
            return 0f;
        }

        #region ArmsRotVariables
        public float _MS_DPS      = 0f; public float _MS_GCDs      = 0f; public float _MS_GCDsD      = 0f;
        public float _RD_DPS      = 0f; public float _RD_GCDs      = 0f; public float _RD_GCDsD      = 0f;
        public float _OP_DPS      = 0f; public float _OP_GCDs      = 0f; public float _OP_GCDsD      = 0f;
        public float _SD_DPS      = 0f; public float _SD_GCDs      = 0f; public float _SD_GCDsD      = 0f;
        public float _SL_DPS      = 0f; public float _SL_GCDs      = 0f; public float _SL_GCDsD      = 0f;
        public float _BLS_DPS     = 0f; public float _BLS_GCDs     = 0f; public float _BLS_GCDsD     = 0f;
        public float _DW_PerHit   = 0f; public float _DW_DPS       = 0f; public float _OVD_PerHit    = 0f; public float _OVD_DPS     = 0f;
        public float _Blood_GCDs  = 0f; public float _Blood_GCDsD  = 0f; public float _ZRage_GCDs    = 0f; public float _ZRage_GCDsD = 0f;
        public float _Shout_GCDs  = 0f; public float _Shout_GCDsD  = 0f; public float _Demo_GCDs     = 0f; public float _Demo_GCDsD  = 0f;
        public float _Sunder_GCDs = 0f; public float _Sunder_GCDsD = 0f; public float _Shatt_GCDs    = 0f; public float _Shatt_GCDsD = 0f;
        public float _TH_DPS      = 0f; public float _Thunder_GCDs = 0f; public float _Thunder_GCDsD = 0f;
        public float _Death_GCDs  = 0f; public float _Death_GCDsD  = 0f; public float _Reck_GCDs     = 0f; public float _Reck_GCDsD  = 0f;
        public float _WhiteDPSMH  = 0f; public float _WhiteDPSOH   = 0f; public float _WhiteDPS      = 0f; public float _WhitePerHit = 0f;
        public string GCDUsage = "";
        #endregion
        public float MakeRotationandDoDPS_Arms() {
            // Starting Numbers
            float DPS_TTL = 0f;
            float rotation = BLS.GetRotation();
            float duration = CalcOpts.Duration;
            float latencyMOD = 1f + CalcOpts.GetLatency();
            float NumGCDs = rotation / (1.5f * latencyMOD);
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;

            if (Char.MainHand == null) { return 0f; }

            // White DPS
            //DPS_TTL += _whiteStats.CalcMhWhiteDPS(); // White DPS is being handled in CharacterCalculationsBase() now
            availRage += WHITEATTACKS.whiteRageGenPerSec();

            // Passive DPS (occurs regardless)
            /*Ability DW = new DeepWounds(_character, _stats, _combatFactors, _whiteStats);
            _DW_PerHit = DW.GetDamageOnUse();
            _DW_DPS = DW.GetDPS();
            DPS_TTL += _DW_DPS;
            // DW is being handled in GetCharacterCalcs right now*/

            // Rage Generators
            Skills.Bloodrage Blood = new Skills.Bloodrage(Char, StatS, CombatFactors, WhiteAtks);
            float Blood_GCDs = (float)Math.Min(availGCDs, Blood.GetActivates());
            _Blood_GCDs = Blood_GCDs; _Blood_GCDsD = _Blood_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, Blood_GCDs);
            GCDUsage += Blood.Name + ": " + Blood_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            availRage += Blood.GetRageUsePerSecond(Blood_GCDs) + Blood.GetAverageStats().BonusRageGen; // used per sec reverses the rage cost in this instance

            Skills.BerserkerRage ZRage = new Skills.BerserkerRage(Char, StatS, CombatFactors, WhiteAtks);
            if (ZRage.RageCost > 0) {
                float ZRage_GCDs = (float)Math.Min(availGCDs, ZRage.GetActivates());
                _ZRage_GCDs = ZRage_GCDs; _ZRage_GCDsD = _ZRage_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, ZRage_GCDs);
                GCDUsage += ZRage.Name + ": " + ZRage_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage += ZRage.GetRageUsePerSecond(ZRage_GCDs); // used per sec reverses the rage cost in this instance
            }

            // Periodic GCD eaters (DPS for these handled elsewhere)
            if (CalcOpts.Mntn_Battle) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.BattleShout Shout = new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks);
                float Shout_GCDs = (float)Math.Min(availGCDs, Shout.GetActivates());
                _Shout_GCDs = Shout_GCDs; _Shout_GCDsD = _Shout_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Shout_GCDs);
                GCDUsage += Shout.Name + ": " + Shout_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Shout.GetRageUsePerSecond(Shout_GCDs);
            }
            if (CalcOpts.Mntn_Demo) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.DemoralizingShout Demo = new Skills.DemoralizingShout(Char, StatS, CombatFactors, WhiteAtks);
                float Demo_GCDs = (float)Math.Min(availGCDs, Demo.GetActivates());
                _Demo_GCDs = Demo_GCDs; _Demo_GCDsD = _Demo_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Demo_GCDs);
                GCDUsage += Demo.Name + ": " + Demo_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Demo.GetRageUsePerSecond(Demo_GCDs);
            }
            if (CalcOpts.Mntn_Sunder) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.SunderArmor Sunder = new Skills.SunderArmor(Char, StatS, CombatFactors, WhiteAtks);
                float Sunder_GCDs = (float)Math.Min(availGCDs, 4f + Sunder.GetActivates()); // 4 to stack up the initial, 5th+ are just maintenance
                _Sunder_GCDs = Sunder_GCDs; _Sunder_GCDsD = _Sunder_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Sunder_GCDs);
                GCDUsage += Sunder.Name + ": " + Sunder_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Sunder.GetRageUsePerSecond(Sunder_GCDs);
            }
            if (CalcOpts.Mntn_Thunder) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.ThunderClap Thunder = new Skills.ThunderClap(Char, StatS, CombatFactors, WhiteAtks);
                float Thunder_GCDs = (float)Math.Min(availGCDs, Thunder.GetActivates());
                _Thunder_GCDs = Thunder_GCDs; _Thunder_GCDsD = _Thunder_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Thunder_GCDs);
                GCDUsage += Thunder.Name + ": " + Thunder_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                _TH_DPS = Thunder.GetDPS(Thunder_GCDs);
                DPS_TTL += _TH_DPS;
                availRage -= Thunder.GetRageUsePerSecond(Thunder_GCDs);
            }

            if (!CalcOpts.FuryStance) {
                Skills.ShatteringThrow Shatt = new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks);
                float Shatt_GCDs = (float)Math.Min(availGCDs, Shatt.GetActivates());
                _Shatt_GCDs = Shatt_GCDs; _Shatt_GCDsD = _Shatt_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Shatt_GCDs);
                GCDUsage += Shatt.Name + ": " + Shatt_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Shatt.GetRageUsePerSecond(Shatt_GCDs);
            } else {
                Skills.DeathWish Death = new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks);
                float Death_GCDs = (float)Math.Min(availGCDs, Death.GetActivates());
                _Death_GCDs = Death_GCDs; _Death_GCDsD = _Death_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Death_GCDs);
                GCDUsage += Death.Name + ": " + Death_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Death.GetRageUsePerSecond(Death_GCDs);

                Skills.Recklessness Reck = new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks);
                float Reck_GCDs = (float)Math.Min(availGCDs, Reck.GetActivates());
                _Reck_GCDs = Reck_GCDs; _Reck_GCDsD = _Reck_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Reck_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                availRage -= Reck.GetRageUsePerSecond(Reck_GCDs);
            }

            // Periodic DPS (run only once every few rotations)
            float BLS_GCDs = (float)Math.Min(availGCDs, BLS.GetActivates());
            _BLS_GCDs = BLS_GCDs; _BLS_GCDsD = _BLS_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, BLS_GCDs * 4f); // the *4 is because it is channeled over 6 secs (4 GCD's consumed from 1 activate)
            GCDUsage += BLS.Name + ": " + BLS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BLS_DPS = BLS.GetDPS(BLS_GCDs);
            DPS_TTL += _BLS_DPS;
            availRage -= BLS.GetRageUsePerSecond(BLS_GCDs);

            // Priority 1 : Mortal Strike on every CD
            float MS_GCDs = (float)Math.Min(availGCDs, MS.GetActivates());
            _MS_GCDs = MS_GCDs; _MS_GCDsD = _MS_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, MS_GCDs);
            GCDUsage += MS.Name + ": " + MS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _MS_DPS = MS.GetDPS(MS_GCDs);
            DPS_TTL += _MS_DPS;
            availRage -= MS.GetRageUsePerSecond(MS_GCDs);

            // Priority 2 : Rend on every tick off
            float RND_GCDs = (float)Math.Min(availGCDs, RD.GetActivates());
            _RD_GCDs = RND_GCDs; _RD_GCDsD = _RD_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, RND_GCDs);
            GCDUsage += RD.Name + ": " + RND_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _RD_DPS = RD.GetDPS(RND_GCDs);
            DPS_TTL += _RD_DPS;
            availRage -= RD.GetRageUsePerSecond(RND_GCDs);

            // Priority 3 : Taste for Blood Proc (Do Overpower) if available
            float OP_GCDs = (float)Math.Min(availGCDs, OP.GetActivates());
            _OP_GCDs = OP_GCDs; _OP_GCDsD = _OP_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, OP_GCDs);
            GCDUsage += OP.Name + ": " + OP_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _OP_DPS = OP.GetDPS(OP_GCDs);
            DPS_TTL += _OP_DPS;
            availRage -= OP.GetRageUsePerSecond(OP_GCDs);

            // Priority 4 : Sudden Death Proc (Do Execute) if available
            float SD_GCDs = (float)Math.Min(availGCDs, SD.GetActivates(false));
            _SD_GCDs = SD_GCDs; _SD_GCDsD = _SD_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, SD_GCDs);
            GCDUsage += SD.Name + ": " + SD_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _SD_DPS = SD.GetDPS(SD_GCDs);
            DPS_TTL += _SD_DPS;
            availRage -= SD.GetRageUsePerSecond(SD_GCDs);

            // Priority 5 : Slam for remainder of GCDs
            float SL_GCDs = SL.GetValided() ? availGCDs * latencyMOD : 0f;
            _SL_GCDs = SL_GCDs; _SL_GCDsD = _SL_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, SL_GCDs);
            GCDUsage += SL.Name + ": " + SL_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            availRage -= SL.GetRageUsePerSecond(SL_GCDs);

            // Priority 6 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; }
            WhiteAtks.Ovd_Freq = Which.GetActivates() * CombatFactors.MainHandSpeed / Which.GetRotation();
            _WhiteDPSMH = WhiteAtks.CalcMhWhiteDPS();
            _WhiteDPS = _WhiteDPSMH;
            _WhitePerHit = WhiteAtks.MhAvgSwingDmg();
            _OVD_DPS = Which.GetDPS(/*WhiteAtks.Ovd_Freq*/);
            WhiteAtks.Ovd_Freq = 0;
            _OVD_PerHit = Which.GetDamageOnUse();

            GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            return DPS_TTL;
        }
    }
}
