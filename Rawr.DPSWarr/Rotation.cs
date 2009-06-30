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
        public Skills.BerserkerRage BZ;
        public Skills.Bloodrage BR;
        public Skills.SunderArmor SN;
        public Skills.DemoralizingShout DS;
        public Skills.BattleShout BTS;
        public Skills.Hamstring HMS;
        
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
        #region Functions
        public void Initialize(CharacterCalculationsDPSWarr calcs) {
            initAbilities();
            doIterations();

            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; }

            if (CalcOpts.FuryStance) {
                WHITEATTACKS.Ovd_Freq = Which.Activates * COMBATFACTORS.MainHandSpeed / Which.RotationLength;
            }else{
                WHITEATTACKS.Ovd_Freq = Which.Activates * COMBATFACTORS.MainHandSpeed / Which.RotationLength;
            }
            calcs.WhiteDPSMH = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhWhiteDPS);
            calcs.WhiteDPSOH = (CHARACTER.OffHand == null ? 0f : WHITEATTACKS.OhWhiteDPS);
            calcs.WhiteDPS = calcs.WhiteDPSMH + calcs.WhiteDPSOH;
            calcs.WhiteDmg = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhAvgSwingDmg);
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
            calcs.BZ = BZ;
            calcs.BR = BR;
            calcs.SN = SN;
            calcs.DS = DS;
        }
        public void Initialize() {initAbilities();doIterations();calcDeepWounds();}

        private void initAbilities() {
            // Fury
            BT = new Skills.BloodThirst(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            WW = new Skills.WhirlWind(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BS = new Skills.BloodSurge(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Arms
            RD = new Skills.Rend(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            MS = new Skills.MortalStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SW = new Skills.SweepingStrikes(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BLS= new Skills.Bladestorm(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SD = new Skills.Suddendeath(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SL = new Skills.Slam(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Generic DPS
            DW = new Skills.DeepWounds(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CL = new Skills.Cleave(             CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Rage Gen
            BZ = new Skills.BerserkerRage(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BR = new Skills.Bloodrage(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Maintenance
            TH = new Skills.ThunderClap(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SN = new Skills.SunderArmor(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            DS = new Skills.DemoralizingShout(  CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BTS = new Skills.BattleShout(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HMS = new Skills.Hamstring(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);

            SD.FreeRage = freeRage;
            
            BS.Slam = SL;
            BS.Whirlwind = WW;
            BS.Bloodthirst = BT;
            
            BLS.WW = WW;

            SS.Slam = SL;
            SS.Overpower = OP;
            SS.MortalStrike = MS;
            SS.SuddenDeath = SD;
        }
        private void doIterations() {
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };

            // Arms Iteration
            float oldSDActivates = 0.0f, newSDActivates = SD.MaxActivates;
            while (!CalcOpts.FuryStance && Math.Abs(newSDActivates - oldSDActivates) > 0.01f) {
                oldSDActivates = SD.MaxActivates;
                SD.LandedAtksPerSec = GetLandedAtksPerSec();
                newSDActivates = SD.MaxActivates;
            }
            // Fury Iteration
            Which.OverridesPerSec = GetOvdActivates(Which);
            float oldActivates = 0.0f, newHSActivates = Which.Activates;
            BS.maintainActs = MaintainCDs;
            while (CalcOpts.FuryStance && Math.Abs(newHSActivates - oldActivates) > 0.01f) {
                oldActivates = Which.Activates;
                BS.hsActivates = oldActivates;
                _bloodsurgeRPS = Which.bloodsurgeRPS = Which.RageUsePerSecond;
                Which.OverridesPerSec = GetOvdActivates(Which);
                newHSActivates = Which.Activates;
            }
            BS.hsActivates = newHSActivates;
            _OVD_DPS = Which.DPS;
            _OVD_PerHit = Which.DamageOnUse;
        }

        private void calcDeepWounds() {
            //DW = new Skills.DeepWounds(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
//            float MHAbilityActivates = SL.Activates + MS.Activates +
//                OP.Activates + /*SW.Activates + SS.Activates*/ +
//                BLS.Activates * 6f + BS.Activates + SD.Activates +
//                BT.Activates + WW.Activates;
            float MHAbilityActivates = _SL_GCDs + _MS_GCDs +
                _OP_GCDs + /*SW.Activates + SS.Activates*/ +
                _BLS_GCDs *6 + BS.Activates + _SD_GCDs +
                BT.Activates + WW.Activates;
            float OHAbilityActivates = 0f;
            if (CHARACTER.OffHand != null) { OHAbilityActivates = WW.Activates + BLS.Activates * 6f; }
            DW.SetAllAbilityActivates(MHAbilityActivates, OHAbilityActivates);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
        }

        private float GetOvdActivates(Skills.OnAttack Which) {
            return (CalcOpts.FuryStance ? freeRage : RageGenWhite + RageGenOther - RageNeeded) / Which.FullRageCost;
        }
        private float GetLandedAtksPerSecNoSS() {
            float MS_Acts = /*_MS_GCDs!=0?_MS_GCDs:*/MS.MaxActivates;
            float OP_Acts = /*_OP_GCDs!=0?_OP_GCDs:*/OP.MaxActivates;
            float SD_Acts = /*_SD_GCDs!=0?_SD_GCDs:*/SD.MaxActivates;
            float SL_Acts = /*_SL_GCDs!=0?_SL_GCDs:*/(MS.RotationLength / 1.5f - MS_Acts - OP_Acts - SD_Acts);

            float Dable = MS_Acts + SD_Acts + SL_Acts;
            float nonDable = OP_Acts;

            float white = (COMBATFACTORS.ProbMhWhiteHit + COMBATFACTORS.MhCrit)
                * (COMBATFACTORS.MainHand.Speed / COMBATFACTORS.TotalHaste);

            float ProbYellowHit = (1f - COMBATFACTORS.YwMissChance - COMBATFACTORS.MhDodgeChance);
            float ProbYellowHitOP = (1f - COMBATFACTORS.YwMissChance);

            float result = white + (Dable * ProbYellowHit) + (nonDable * ProbYellowHitOP);

            return result;
        }
        public float GetLandedAtksPerSec() { return GetLandedAtksPerSecNoSS(); }// TODO: add swordspec to this
        public float CritHsSlamPerSec {
            get
            {
                if (CalcOpts.FuryStance)
                {
                    float critsPerRot = HS.Activates * (CombatFactors.MhYellowCrit + HS.BonusCritChance) + 
                                        SL.Activates * (CombatFactors.MhYellowCrit + SL.BonusCritChance);
                    return critsPerRot / HS.RotationLength;
                }
                else
                {
                    Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };
                    float MS_Acts = MS.Activates;
                    float OP_Acts = OP.Activates;
                    float SD_Acts = SD.Activates;
                    float HS_Acts = Which.Activates;
                    float SL_Acts = MS.RotationLength / 1.5f - MS_Acts - OP_Acts - SD_Acts;

                    float result = (SL.BonusCritChance + CombatFactors.MhYellowCrit) * (SL_Acts / Which.RotationLength) +
                                   (HS.BonusCritChance + CombatFactors.MhYellowCrit) * (HS_Acts / Which.RotationLength);

                    return result;
                }
            }
        }
        public float MaintainCDs
        {
            get
            {
                float ThunderActs = CalcOpts.Mntn_Thunder ? TH.Activates : 0f;
                float SunderActs = CalcOpts.Mntn_Sunder ? SN.Activates : 0f;
                float DemoActs = CalcOpts.Mntn_Demo ? DS.Activates : 0f;
                float HamstringActs = CalcOpts.Mntn_Hamstring ? HMS.Activates : 0f;
                float BattleActs = CalcOpts.Mntn_Battle ? BTS.Activates : 0f;
                return ThunderActs + SunderActs + DemoActs + HamstringActs + BattleActs;
            }
        }
        #endregion
        #region Rage Calcs
        public virtual float RageGenBloodPerSec {
            get {
                float result = 0f;
                if (CalcOpts.FuryStance){
                    result = (20f * (1f + 0.25f * Talents.ImprovedBloodrage)) / (60f * (1f - 1.0f / 9.0f * Talents.IntensifyRage));
                }else{
                    result = BR.RageUsePerSecond;
                }
                return result;
            }
        }
        public virtual float RageGenAngerPerSec { get { return Talents.AngerManagement / 3.0f; } }
        public virtual float RageGenWrathPerSec { get { return Talents.UnbridledWrath * 3.0f / 60.0f; } }
        public virtual float RageGenZerkrPerSec {
            get { 
                float result = 0f;
                if (CalcOpts.FuryStance) {
                    result = Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 1f / 9f * Talents.IntensifyRage));
                }else{
                    result = BZ.RageUsePerSecond;
                }
                return result;
            }
        }
        public virtual float RageGenOtherPerSec {
            get {
                if (Char.MainHand == null) { return 0f; }
                float rage  = RageGenAngerPerSec
                            + RageGenZerkrPerSec
                            + RageGenBloodPerSec
                            + RageGenWrathPerSec;

                // 4pcT7
                if (StatS.DreadnaughtBonusRageProc != 0f) {
                    rage += 5.0f * 0.1f * ((Talents.DeepWounds > 0f ? 1f : 0f) + (!CalcOpts.FuryStance ? 1f / 3f : 0f));
                }

                return rage;
            }
        }
        public virtual float RageNeededPerSec {
            get {
                if (Char.MainHand == null) { return 0f; }
                // Fury
                float BTRage = BT.RageUsePerSecond;
                float WWRage = WW.RageUsePerSecond;
                float BloodSurgeRage = _bloodsurgeRPS;// BS.RageUsePerSecond;
                // Arms
                float SweepingRage   = _SW_GCDs  != 0 ? SW.GetRageUsePerSecond(_SW_GCDs ) : SW.RageUsePerSecond;
                float BladestormRage = _BLS_GCDs != 0 ?BLS.GetRageUsePerSecond(_BLS_GCDs) :BLS.RageUsePerSecond;
                float MSRage         = _MS_GCDs  != 0 ? MS.GetRageUsePerSecond(_MS_GCDs ) : MS.RageUsePerSecond;
                float RendRage       = _RD_GCDs  != 0 ? RD.GetRageUsePerSecond(_RD_GCDs ) : RD.RageUsePerSecond;
                float OPRage         = _OP_GCDs  != 0 ? OP.GetRageUsePerSecond(_OP_GCDs ) : OP.RageUsePerSecond;
                float SDRage         = _SD_GCDs  != 0 ? SD.GetRageUsePerSecond(_SD_GCDs ) : SD.RageUsePerSecond;
                float SlamRage       = _SL_GCDs  != 0 ? SL.GetRageUsePerSecond(_SL_GCDs ) : SL.RageUsePerSecond;
                // Maintenance
                float ThunderRage = CalcOpts.Mntn_Thunder ? TH.RageUsePerSecond : 0f;
                float SunderRage = CalcOpts.Mntn_Sunder ? SN.RageUsePerSecond : 0f;
                float DemoRage = CalcOpts.Mntn_Demo ? DS.RageUsePerSecond : 0f;
                float HamstringRage = CalcOpts.Mntn_Hamstring ? HMS.RageUsePerSecond : 0f;
                float BattleRage = CalcOpts.Mntn_Battle ? BTS.RageUsePerSecond : 0f;
                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage +
                    BloodSurgeRage + SweepingRage + BladestormRage + RendRage +
                    ThunderRage + SunderRage + DemoRage + HamstringRage + BattleRage;
                return rage;
            }
        }
        public virtual float freeRage {
            get {
                if (Char.MainHand == null) { return 0f; }
                float white = WHITEATTACKS.whiteRageGenPerSec;
                float other = RageGenOtherPerSec;
                float needy = RageNeededPerSec;
                return white + other - needy;
            }
        }
        #endregion

        #region FuryRotVariables
        float _bloodsurgeRPS;

        #endregion

        #region ArmsRotVariables
        public float _MS_DPS      = 0f; public float _MS_GCDs      = 0f; public float _MS_GCDsD      = 0f;
        public float _RD_DPS      = 0f; public float _RD_GCDs      = 0f; public float _RD_GCDsD      = 0f;
        public float _OP_DPS      = 0f; public float _OP_GCDs      = 0f; public float _OP_GCDsD      = 0f;
        public float _SD_DPS      = 0f; public float _SD_GCDs      = 0f; public float _SD_GCDsD      = 0f;
        public float _SL_DPS      = 0f; public float _SL_GCDs      = 0f; public float _SL_GCDsD      = 0f;
        public float _BLS_DPS     = 0f; public float _BLS_GCDs     = 0f; public float _BLS_GCDsD     = 0f;
        public float _SW_DPS      = 0f; public float _SW_GCDs      = 0f; public float _SW_GCDsD      = 0f;
        public float _DW_PerHit   = 0f; public float _DW_DPS       = 0f; public float _OVD_PerHit    = 0f; public float _OVD_DPS     = 0f;
        public float _Blood_GCDs  = 0f; public float _Blood_GCDsD  = 0f; public float _ZRage_GCDs    = 0f; public float _ZRage_GCDsD = 0f;
        public float _Shout_GCDs  = 0f; public float _Shout_GCDsD  = 0f; public float _Demo_GCDs     = 0f; public float _Demo_GCDsD  = 0f;
        public float _Sunder_GCDs = 0f; public float _Sunder_GCDsD = 0f; public float _Shatt_GCDs    = 0f; public float _Shatt_GCDsD = 0f;
        public float _TH_DPS      = 0f; public float _Thunder_GCDs = 0f; public float _Thunder_GCDsD = 0f;
        public float _Death_GCDs  = 0f; public float _Death_GCDsD  = 0f; public float _Reck_GCDs     = 0f; public float _Reck_GCDsD  = 0f;
        public float _WhiteDPSMH  = 0f; public float _WhiteDPSOH   = 0f; public float _WhiteDPS      = 0f; public float _WhitePerHit = 0f;
        public string GCDUsage = "";
        public float RageGenWhite = 0f; public float RageGenOther = 0f; public float RageNeeded = 0f;
        #endregion
        public float MakeRotationandDoDPS_Arms() {
            // Starting Numbers
            float DPS_TTL = 0f;
            float rotation = BLS.RotationLength;
            float duration = CalcOpts.Duration;
            float latencyMOD = 1f - CalcOpts.GetLatency();
            float NumGCDs = rotation / 1.5f * latencyMOD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;

            if (Char.MainHand == null) { return 0f; }

            // Passive DPS (occurs regardless)
            /*Ability DW = new DeepWounds(_character, _stats, _combatFactors, _whiteStats);
            _DW_PerHit = DW.DamageOnUse;
            _DW_DPS = DW.DPS;
            DPS_TTL += _DW_DPS;
            // DW is being handled in GetCharacterCalcs right now*/
            
            // Rage Generators
            RageGenOther = RageGenAngerPerSec + RageGenWrathPerSec;
            if (StatS.DreadnaughtBonusRageProc != 0f) {
                RageGenOther += 5.0f * 0.1f * ((Talents.DeepWounds > 0f ? 1f : 0f) + (!CalcOpts.FuryStance ? 1f / 3f : 0f));
            }

            Skills.Bloodrage Blood = new Skills.Bloodrage(Char, StatS, CombatFactors, WhiteAtks);
            float Blood_GCDs = (float)Math.Min(availGCDs, Blood.MaxActivates);
            _Blood_GCDs = Blood_GCDs; _Blood_GCDsD = _Blood_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, Blood_GCDs);
            GCDUsage += Blood.Name + ": " + Blood_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            rageadd = Blood.GetRageUsePerSecond(Blood_GCDs) + Blood.AverageStats.BonusRageGen; // used per sec reverses the rage cost in this instance
            RageGenOther += rageadd;
            availRage += rageadd;

            Skills.BerserkerRage ZRage = new Skills.BerserkerRage(Char, StatS, CombatFactors, WhiteAtks);
            if (ZRage.RageCost > 0) {
                float ZRage_GCDs = (float)Math.Min(availGCDs, ZRage.MaxActivates);
                _ZRage_GCDs = ZRage_GCDs; _ZRage_GCDsD = _ZRage_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, ZRage_GCDs);
                GCDUsage += ZRage.Name + ": " + ZRage_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = ZRage.GetRageUsePerSecond(ZRage_GCDs); // used per sec reverses the rage cost in this instance
                RageGenOther += rageadd;
                availRage += rageadd;
            }

            // Periodic GCD eaters (DPS for these handled elsewhere)
            if (CalcOpts.Mntn_Battle) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.BattleShout Shout = new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks);
                float Shout_GCDs = (float)Math.Min(availGCDs, Shout.MaxActivates);
                _Shout_GCDs = Shout_GCDs; _Shout_GCDsD = _Shout_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Shout_GCDs);
                GCDUsage += Shout.Name + ": " + Shout_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Shout.GetRageUsePerSecond(Shout_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }
            if (CalcOpts.Mntn_Demo) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.DemoralizingShout Demo = new Skills.DemoralizingShout(Char, StatS, CombatFactors, WhiteAtks);
                float Demo_GCDs = (float)Math.Min(availGCDs, Demo.MaxActivates);
                _Demo_GCDs = Demo_GCDs; _Demo_GCDsD = _Demo_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Demo_GCDs);
                GCDUsage += Demo.Name + ": " + Demo_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Demo.GetRageUsePerSecond(Demo_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }
            if (CalcOpts.Mntn_Sunder) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.SunderArmor Sunder = new Skills.SunderArmor(Char, StatS, CombatFactors, WhiteAtks);
                float Sunder_GCDs = (float)Math.Min(availGCDs, 4f + Sunder.MaxActivates); // 4 to stack up the initial, 5th+ are just maintenance
                _Sunder_GCDs = Sunder_GCDs; _Sunder_GCDsD = _Sunder_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Sunder_GCDs);
                GCDUsage += Sunder.Name + ": " + Sunder_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Sunder.GetRageUsePerSecond(Sunder_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }
            if (CalcOpts.Mntn_Thunder) {
                // Enforcing a "Maintaining" argument so we know if we are the ones putting this up or not
                Skills.ThunderClap Thunder = new Skills.ThunderClap(Char, StatS, CombatFactors, WhiteAtks);
                float Thunder_GCDs = (float)Math.Min(availGCDs, Thunder.MaxActivates);
                _Thunder_GCDs = Thunder_GCDs; _Thunder_GCDsD = _Thunder_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Thunder_GCDs);
                GCDUsage += Thunder.Name + ": " + Thunder_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                _TH_DPS = Thunder.GetDPS(Thunder_GCDs);
                DPS_TTL += _TH_DPS;
                rageadd = Thunder.GetRageUsePerSecond(Thunder_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }

            if (!CalcOpts.FuryStance) {
                Skills.ShatteringThrow Shatt = new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks);
                float Shatt_GCDs = (float)Math.Min(availGCDs, Shatt.MaxActivates);
                _Shatt_GCDs = Shatt_GCDs; _Shatt_GCDsD = _Shatt_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Shatt_GCDs);
                GCDUsage += Shatt.Name + ": " + Shatt_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Shatt.GetRageUsePerSecond(Shatt_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
                //
                Skills.SweepingStrikes Sweep = new Skills.SweepingStrikes(Char, StatS, CombatFactors, WhiteAtks);
                float Sweep_GCDs = (float)Math.Min(availGCDs, Sweep.MaxActivates);
                _SW_GCDs = Sweep_GCDs; _SW_GCDsD = _SW_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Sweep_GCDs);
                if (Sweep_GCDs > 0) { GCDUsage += Sweep.Name + ": " + Sweep_GCDs.ToString() + "\n"; }
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Sweep.GetRageUsePerSecond(Sweep_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }else{
                Skills.DeathWish Death = new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks);
                float Death_GCDs = (float)Math.Min(availGCDs, Death.MaxActivates);
                _Death_GCDs = Death_GCDs; _Death_GCDsD = _Death_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Death_GCDs);
                GCDUsage += Death.Name + ": " + Death_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Death.GetRageUsePerSecond(Death_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
                //
                Skills.Recklessness Reck = new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks);
                float Reck_GCDs = (float)Math.Min(availGCDs, Reck.MaxActivates);
                _Reck_GCDs = Reck_GCDs; _Reck_GCDsD = _Reck_GCDs * duration / rotation;
                GCDsused += (float)Math.Min(NumGCDs, Reck_GCDs);
                GCDUsage += Reck.Name + ": " + Reck_GCDs.ToString() + "\n";
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                rageadd = Reck.GetRageUsePerSecond(Reck_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;
            }

            // Periodic DPS (run only once every few rotations)
            float BLS_GCDs = (float)Math.Min(availGCDs, BLS.MaxActivates);
            _BLS_GCDs = BLS_GCDs; _BLS_GCDsD = _BLS_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, BLS_GCDs * 4f); // the *4 is because it is channeled over 6 secs (4 GCD's consumed from 1 activate)
            GCDUsage += BLS.Name + ": " + BLS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BLS_DPS = BLS.GetDPS(BLS_GCDs);
            DPS_TTL += _BLS_DPS;
            rageadd = BLS.GetRageUsePerSecond(BLS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 1 : Mortal Strike on every CD
            float MS_GCDs = (float)Math.Min(availGCDs, MS.MaxActivates);
            _MS_GCDs = MS_GCDs; _MS_GCDsD = _MS_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, MS_GCDs);
            GCDUsage += MS.Name + ": " + MS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _MS_DPS = MS.GetDPS(MS_GCDs);
            DPS_TTL += _MS_DPS;
            rageadd = MS.GetRageUsePerSecond(MS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 2 : Rend on every tick off
            float RND_GCDs = (float)Math.Min(availGCDs, RD.MaxActivates);
            _RD_GCDs = RND_GCDs; _RD_GCDsD = _RD_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, RND_GCDs);
            GCDUsage += RD.Name + ": " + RND_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _RD_DPS = RD.GetDPS(RND_GCDs);
            DPS_TTL += _RD_DPS;
            rageadd = RD.GetRageUsePerSecond(RND_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 3 : Taste for Blood Proc (Do Overpower) if available
            float OP_GCDs = (float)Math.Min(availGCDs, OP.MaxActivates);
            _OP_GCDs = OP_GCDs; _OP_GCDsD = _OP_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, OP_GCDs);
            GCDUsage += OP.Name + ": " + OP_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _OP_DPS = OP.GetDPS(OP_GCDs);
            DPS_TTL += _OP_DPS;
            rageadd = OP.GetRageUsePerSecond(OP_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 4 : Sudden Death Proc (Do Execute) if available
            doIterations();
            float SD_GCDs = (float)Math.Min(availGCDs, SD.MaxActivates);
            _SD_GCDs = SD_GCDs; _SD_GCDsD = _SD_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, SD_GCDs);
            GCDUsage += SD.Name + ": " + SD_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _SD_DPS = SD.GetDPS(SD_GCDs);
            DPS_TTL += _SD_DPS;
            rageadd = SD.GetRageUsePerSecond(SD_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 5 : Slam for remainder of GCDs
            float SL_GCDs = SL.GetValided() ? availGCDs /* * latencyMOD*/ : 0f;
            _SL_GCDs = SL_GCDs; _SL_GCDsD = _SL_GCDs * duration / rotation;
            GCDsused += (float)Math.Min(NumGCDs, SL_GCDs);
            GCDUsage += SL.Name + ": " + SL_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            rageadd = SL.GetRageUsePerSecond(SL_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // White DPS
            //DPS_TTL += _whiteStats.MhWhiteDPS; // White DPS is being handled in CharacterCalculationsBase() now
            RageGenWhite = WHITEATTACKS.whiteRageGenPerSec;
            availRage += RageGenWhite;

            // Priority 6 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            doIterations();
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; }
            WhiteAtks.Ovd_Freq = Which.Activates * CombatFactors.MainHandSpeed / Which.RotationLength;
            _WhiteDPSMH = WhiteAtks.MhWhiteDPS;
            _WhiteDPS = _WhiteDPSMH;
            _WhitePerHit = WhiteAtks.MhAvgSwingDmg;
            _OVD_DPS = Which.DPS;//(/*WhiteAtks.Ovd_Freq*/);
            WhiteAtks.Ovd_Freq = 0;
            _OVD_PerHit = Which.DamageOnUse;

            GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            return DPS_TTL;
        }
    }
}
