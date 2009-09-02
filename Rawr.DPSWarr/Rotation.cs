using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class Rotation {
        // Constructors
        public Rotation(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = new CombatFactors(Char, StatS);
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            //WhiteAtks = new Skills.WhiteAttacks(Char, StatS);
            // Initialize();
        }
        #region Variables
        private Character CHARACTER;
        private WarriorTalents TALENTS;
        private Stats STATS;
        private CombatFactors COMBATFACTORS;
        private Skills.WhiteAttacks WHITEATTACKS;
        private CalculationOptionsDPSWarr CALCOPTS;

        // Anti-DeBuff
        public Skills.HeroicFury HF;
        public Skills.EveryManForHimself EM;
        // Rage Generators
        public Skills.SecondWind SndW;
        public Skills.BerserkerRage BZ;
        public Skills.Bloodrage BR;
        // Maintenance
        public Skills.BattleShout BTS;
        public Skills.CommandingShout CS;
        public Skills.DemoralizingShout DS;
        public Skills.SunderArmor SN;
        public Skills.ThunderClap TH;
        public Skills.Hamstring HMS;
        // Periodics
        public Skills.ShatteringThrow ST;
        public Skills.SweepingStrikes SW;
        public Skills.DeathWish Death;
        public Skills.Recklessness RK;
        public Skills.Trinket1 Trinket1;
        public Skills.Trinket2 Trinket2;
        // Fury
        public Skills.WhirlWind WW;
        public Skills.BloodThirst BT;
        public Skills.BloodSurge BS;
        // Arms
        public Skills.Bladestorm BLS;
        public Skills.MortalStrike MS;
        public Skills.Rend RD;
        public Skills.OverPower OP;
        public Skills.TasteForBlood TB;
        public Skills.Suddendeath SD;
        public Skills.Slam SL;
        public Skills.Swordspec SS;
        // Generic
        public Skills.DeepWounds DW;
        public Skills.Cleave CL;
        public Skills.HeroicStrike HS;
        
        public const float ROTATION_LENGTH_FURY = 8.0f;
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

            WHITEATTACKS.Ovd_Freq = Which.Activates * WHITEATTACKS.MhEffectiveSpeed / CalcOpts.Duration;

            calcs.WhiteDPSMH = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhDPS); // MhWhiteDPS
            calcs.WhiteDPSOH = (CHARACTER.OffHand  == null ? 0f : WHITEATTACKS.OhDPS);
            calcs.WhiteDPS = calcs.WhiteDPSMH + calcs.WhiteDPSOH;
            calcs.WhiteDmg = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhDamageOnUse); //MhAvgSwingDmg

            WHITEATTACKS.Ovd_Freq = 0;

            // Whites
            calcs.Whites = WhiteAtks;
            // Anti-Debuff
            calcs.HF = HF;
            calcs.EM = EM;
            // Rage Generators
            calcs.SndW = SndW;
            calcs.BZ = BZ;
            calcs.BR = BR;
            // Maintenance
            calcs.BTS = BTS;
            calcs.CS = CS;
            calcs.DS = DS;
            calcs.SN = SN;
            calcs.TH = TH;
            calcs.HMS = HMS;
            // Periodics
            calcs.ST = ST;
            calcs.SW = SW;
            calcs.Death = Death;
            calcs.RK = RK;
            calcs.Trinket1 = Trinket1;
            calcs.Trinket2 = Trinket2;
            // Fury
            calcs.WW = WW;
            calcs.BT = BT;
            calcs.BS = BS;
            // Arms
            calcs.BLS = BLS;
            calcs.MS = MS;
            calcs.RD = RD;
            calcs.OP = OP;
            calcs.TB = TB;
            calcs.SD = SD;
            calcs.SL = SL;
            calcs.SS = SS;
            // Generic
            calcs.CL = CL;
            calcs.DW = DW;
            calcs.HS = HS;
        }
        public void Initialize() { initAbilities(); doIterations(); }

        private void initAbilities() {
            // Whites
            WhiteAtks = new Skills.WhiteAttacks(CHARACTER, STATS, COMBATFACTORS);
            // Anti-Debuff
            HF  = new Skills.HeroicFury(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            EM  = new Skills.EveryManForHimself(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Rage Generators
            SndW= new Skills.SecondWind(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BZ  = new Skills.BerserkerRage(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BR  = new Skills.Bloodrage(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Maintenance
            BTS = new Skills.BattleShout(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CS  = new Skills.CommandingShout(   CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            DS  = new Skills.DemoralizingShout( CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SN  = new Skills.SunderArmor(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            TH  = new Skills.ThunderClap(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HMS = new Skills.Hamstring(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Periodics
            ST = new Skills.ShatteringThrow(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SW = new Skills.SweepingStrikes(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            Death = new Skills.DeathWish(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RK = new Skills.Recklessness(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            Trinket1 = new Skills.Trinket1(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            Trinket2 = new Skills.Trinket2(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Fury
            WW = new Skills.WhirlWind(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BT = new Skills.BloodThirst(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SL = new Skills.Slam(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); // actually arms but BS needs it
            BS = new Skills.BloodSurge(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SL,WW,BT);
            // Arms
            BLS= new Skills.Bladestorm(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,WW);
            MS = new Skills.MortalStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SS);
            TB = new Skills.TasteForBlood(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SD = new Skills.Suddendeath(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SS);
            // Generic DPS
            DW = new Skills.DeepWounds(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CL = new Skills.Cleave(             CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);

            SD.FreeRage = freeRage;
        }
        private void doIterations() {
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };

            // Fury Iteration
            Which.OverridesPerSec = GetOvdActivates(Which);
            float oldActivates = 0.0f, newHSActivates = Which.Activates;
            BS.maintainActs = MaintainCDs;
            while (CalcOpts.FuryStance && Math.Abs(newHSActivates - oldActivates) > 0.01f) {
                oldActivates = Which.Activates;
                WhiteAtks.Ovd_Freq = (WhiteAtks.MhEffectiveSpeed * Which.OverridesPerSec);
                BS.hsActivates = oldActivates;
                _bloodsurgeRPS = Which.bloodsurgeRPS = Which.RageUsePerSecond;
                Which.OverridesPerSec = GetOvdActivates(Which);
                newHSActivates = Which.Activates;
            }
            BS.hsActivates = newHSActivates;
            if (CalcOpts.FuryStance) {
                _OVD_DPS = Which.DPS;
                _OVD_PerHit = Which.DamageOnUse;
            }
        }

        public float ContainCritValue(Skills.Ability abil, bool IsMH) {
            float BaseCrit = IsMH ? CombatFactors._c_mhycrit : CombatFactors._c_ohycrit;
            return (float)Math.Min(1f, Math.Max(0f, BaseCrit + abil.BonusCritChance));
        }

        private void calcDeepWounds() {
            Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };
            float numWhichActivates = Which.OverridesPerSec * CalcOpts.Duration;
            //if (CalcOpts.MultipleTargets) numWhichActivates *= 1f + (Which.Targets - 1f) * CalcOpts.MultipleTargetsPerc;

            // Main Hand
            float MHAbilityActivates =
                // Fury
                (_WW_GCDs * ContainCritValue(WW, true)) +
                (_BT_GCDs * ContainCritValue(BT, true)) +
                (_BS_GCDs * ContainCritValue(BS, true)) +
                // Arms
                (_BLS_GCDs* ContainCritValue(BLS,true)) * 6f +
                (_MS_GCDs * ContainCritValue(MS, true)) +
                (_OP_GCDs * ContainCritValue(OP, true)) +
                (_TB_GCDs * ContainCritValue(TB, true)) +
                (_SD_GCDs * ContainCritValue(SD, true)) +
                (_SL_GCDs * ContainCritValue(SL, true)) +
                (_SS_Acts * ContainCritValue(SS, true)) +
                // Both
                (numWhichActivates * ContainCritValue(Which, true));
            float mhActivates =
                /*Yellow*/MHAbilityActivates +
                /*White */CalcOpts.Duration / WHITEATTACKS.MhEffectiveSpeed * CombatFactors._c_mhwcrit;

            // Off Hand
            float OHAbilityActivates = (CHARACTER.OffHand != null && Char.OffHand.Speed == 0f) ?
                (_WW_GCDs * ContainCritValue(WW, false)) +
                (_BLS_GCDs* ContainCritValue(BLS,false)) * 6f
                : 0f;
            float ohActivates = (CHARACTER.OffHand != null && Char.OffHand.Speed == 0f) ?
                /*Yellow*/OHAbilityActivates +
                /*White */CalcOpts.Duration / WHITEATTACKS.OhEffectiveSpeed * CombatFactors._c_ohwcrit
                : 0f;

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
        }

        private float GetOvdActivates(Skills.OnAttack Which) {
            return (CalcOpts.FuryStance ? freeRage : RageGenWhite + RageGenOther - RageNeeded) / Which.FullRageCost;
        }
        private float GetLandedAtksPerSecNoSS() {
            float LatentGCD = 1.5f + CalcOpts.GetLatency();

            //float SN_Acts = _Sunder_GCDs!=0?_Sunder_GCDs  :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SunderArmor_      ] ? SN.Activates  : 0f;
            float TH_Acts = _Thunder_GCDs !=0?_Thunder_GCDs :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ThunderClap_      ] ? TH.Activates  : 0f;
            float ST_Acts = _Shatt_GCDs   !=0?_Shatt_GCDs   :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_  ] ? ST.Activates  : 0f;
            float BLS_Acts= _BLS_GCDs     !=0?_BLS_GCDs     :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bladestorm_       ] ? BLS.Activates : 0f;

            float MS_Acts = _MS_GCDs      !=0?_MS_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_     ] ? MS.Activates  : 0f;
            float OP_Acts = _OP_GCDs      !=0?_OP_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_        ] ? OP.Activates  : 0f;
            float TB_Acts = _TB_GCDs      !=0?_TB_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_        ] ? OP.Activates  : 0f;
            float SD_Acts = _SD_GCDs      !=0?_SD_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SuddenDeath_      ] ? SD.Activates  : 0f;
            float SL_Acts = _SL_GCDs      !=0?_SL_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Slam_             ] ?

            float Dable = (BLS_Acts*6f) + TH_Acts + ST_Acts + MS_Acts + SD_Acts + SL_Acts;
            float nonDable = OP_Acts + TB_Acts;

            Dable    /= LatentGCD;
            nonDable /= LatentGCD;

            float white = (COMBATFACTORS.ProbMhWhiteLand * WhiteAtks.MhEffectiveSpeed)
                        + (COMBATFACTORS.ProbOhWhiteLand * WhiteAtks.OhEffectiveSpeed);

            float ProbYellowHitOP = (1f - COMBATFACTORS._c_ymiss);

            float result = white
                + (Dable    * COMBATFACTORS.ProbMhYellowLand)
                + (nonDable * ProbYellowHitOP);

            return result / 60f;
        }
        public float GetLandedAtksPerSec() {
            float landednoss = GetLandedAtksPerSecNoSS();
            float ssActs = (landednoss * Talents.SwordSpecialization / 100f);

            ssActs *= CombatFactors.ProbMhWhiteLand;

            return landednoss + (float)Math.Max(0f, Math.Min(ssActs, 1f / SS.Cd));
        }
        public float CritHsSlamPerSec {
            get {
                if (CalcOpts.FuryStance) {
                    float critsPerRot = HS.Activates * (CombatFactors._c_mhycrit + HS.BonusCritChance) +
                                        SL.Activates * (CombatFactors._c_mhycrit + SL.BonusCritChance);
                    return critsPerRot / CalcOpts.Duration;
                }else{
                    Skills.OnAttack Which; if (CalcOpts.MultipleTargets) { Which = CL; } else { Which = HS; };

                    float UsedGCDs = BZ.Activates
                                   + Trinket1.Activates
                                   + Trinket2.Activates
                                   + BTS.Activates
                                   + DS.Activates
                                   + SN.Activates
                                   + TH.Activates
                                   + HMS.Activates
                                   + ST.Activates
                                   + SW.Activates
                                   + Death.Activates
                                   + BLS.Activates * 4f
                                   + MS.Activates
                                   + RD.Activates
                                   + OP.Activates
                                   + TB.Activates
                                   + SD.Activates;
                    float HS_Acts = (CalcOpts.MultipleTargets ? 0f : Which.Activates);
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    float SL_Acts = Math.Max(0f, CalcOpts.Duration / LatentGCD - UsedGCDs);

                    float result = (SL.BonusCritChance + CombatFactors._c_mhycrit) * (SL_Acts / CalcOpts.Duration) +
                                   (HS.BonusCritChance + CombatFactors._c_mhycrit) * (HS_Acts / CalcOpts.Duration);

                    return result;
                }
            }
        }
        public float MaintainCDs {
            get {
                float ThunderActs   = _Thunder_GCDs != 0 ? _Thunder_GCDs : 0f;
                float SunderActs    = _Sunder_GCDs  != 0 ? _Sunder_GCDs  : 0f;
                float DemoActs      = _Demo_GCDs    != 0 ? _Demo_GCDs    : 0f;
                float HamstringActs = _Ham_GCDs     != 0 ? _Ham_GCDs     : 0f;
                float BattleActs    = _Battle_GCDs  != 0 ? _Battle_GCDs  : 0f;
                return ThunderActs + SunderActs + DemoActs + HamstringActs + BattleActs;
            }
        }
        #endregion
        #region Rage Calcs
        public virtual float RageGenBloodPerSec {
            get {
                float result = 0f;
                if (CalcOpts.FuryStance) {
                    result = (20f * (1f + 0.25f * Talents.ImprovedBloodrage)) / (60f * (1f - 1.0f / 9.0f * Talents.IntensifyRage));
                }else{
                    result = _Blood_GCDs != 0 ? BR.GetRageUsePerSecond(_Blood_GCDs) : 0f;
                }
                return result;
            }
        }
        public virtual float RageGenAngerPerSec { get { return Talents.AngerManagement / 3.0f; } }
        public virtual float RageGenWrathPerSec { get { return Talents.UnbridledWrath * 3.0f / 60.0f; } }
        //public virtual float RageGen2dWindPerSec { get { return Talents.UnbridledWrath * 3.0f / 60.0f; } }
        public virtual float RageGenZerkrPerSec {
            get {
                float result = 0f;
                if (CalcOpts.FuryStance) {
                    result = Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 1f / 9f * Talents.IntensifyRage));
                }else{
                    result = _ZRage_GCDs != 0 ? BZ.GetRageUsePerSecond(_ZRage_GCDs) : 0f;
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
                if (StatS.BonusWarrior_T7_4P_RageProc != 0f) {
                    rage += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * ((Talents.DeepWounds > 0f ? 1f : 0f) + (!CalcOpts.FuryStance ? 1f / 3f : 0f));
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
                float SweepingRage   = _SW_GCDs     != 0 ? SW.GetRageUsePerSecond( _SW_GCDs    ) : 0f;
                float BladestormRage = _BLS_GCDs    != 0 ?BLS.GetRageUsePerSecond( _BLS_GCDs   ) : 0f;
                float MSRage         = _MS_GCDs     != 0 ? MS.GetRageUsePerSecond( _MS_GCDs    ) : 0f;
                float RendRage       = _RD_GCDs     != 0 ? RD.GetRageUsePerSecond( _RD_GCDs    ) : 0f;
                float OPRage         = _OP_GCDs     != 0 ? OP.GetRageUsePerSecond( _OP_GCDs    ) : 0f;
                float TBRage         = _TB_GCDs     != 0 ? TB.GetRageUsePerSecond( _TB_GCDs    ) : 0f;
                float SDRage         = _SD_GCDs     != 0 ? SD.GetRageUsePerSecond( _SD_GCDs    ) : 0f;
                float SlamRage       = _SL_GCDs     != 0 ? SL.GetRageUsePerSecond( _SL_GCDs    ) : 0f;
                // Maintenance
                float ThunderRage    = _TH_DPS      != 0 ? TH.GetRageUsePerSecond( _TH_DPS     ) : 0f;
                float SunderRage     = _Sunder_GCDs != 0 ? SN.GetRageUsePerSecond( _Sunder_GCDs) : 0f;
                float DemoRage       = _Demo_GCDs   != 0 ? DS.GetRageUsePerSecond( _Demo_GCDs  ) : 0f;
                float HamstringRage  = _Ham_GCDs    != 0 ? HMS.GetRageUsePerSecond(_Ham_GCDs   ) : 0f;
                float BattleRage     = _Battle_GCDs != 0 ? BTS.GetRageUsePerSecond(_Battle_GCDs) : 0f;
                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + TBRage + SDRage + SlamRage +
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
        public float _BS_DPS = 0f; public float _BS_GCDs = 0f;
        public float _BT_DPS = 0f; public float _BT_GCDs = 0f;
        public float _WW_DPS = 0f; public float _WW_GCDs = 0f;
        #endregion
        public float MakeRotationandDoDPS_Fury() {
            // Starting Numbers
            float DPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = FightDuration / LatentGCD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;

            if (Char.MainHand == null) { return 0f; }

            doIterations();

            // ==== Rage Generation Priorities ========
            RageGenOther = RageGenAngerPerSec + RageGenWrathPerSec;
            if (StatS.BonusWarrior_T7_4P_RageProc != 0f) {
                RageGenOther += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (Talents.DeepWounds > 0f ? 1f : 0f);
                RageGenOther += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f);
            }
            availRage += RageGenOther;

            /*Bloodrage         */AddAnItem(                                       ref availRage,ref _Blood_GCDs,  BR);
            /*Berserker Rage    */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _ZRage_GCDs,  BZ,false);

            // ==== Trinket Priorites =================
            // /*Trinket 1         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink1_GCDs, Trinket1);
            // /*Trinket 2         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink2_GCDs, Trinket2);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Battle_GCDs, BTS);
            /*Commanding Shout  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Comm_GCDs,   CS);
            /*Demoralizing Shout*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Demo_GCDs,   DS);
            /*Sunder Armor      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Sunder_GCDs, SN);
            /*Thunder Clap      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Thunder_GCDs,ref DPS_TTL,ref _TH_DPS,TH);
            /*Hamstring         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Ham_GCDs,    HMS);
            /*Shattering Throw  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Shatt_GCDs,  ref DPS_TTL,ref _Shatt_DPS,ST);
            /*Sweeping Strikes  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _SW_GCDs,     SW);
            /*Death Wish        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Death_GCDs,  Death);

            /*float Reck_GCDs = (float)Math.Min(availGCDs, RK.Activates);
            _Reck_GCDs = Reck_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Reck_GCDs);
            GCDUsage += RK.Name + ": " + Reck_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            rageadd = RK.GetRageUsePerSecond(Reck_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;*/

            doIterations();

            // Priority 1 : Whirlwind on every CD
            float WW_GCDs = (float)Math.Min(availGCDs, WW.Activates);
            _WW_GCDs = WW_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, WW_GCDs);
            GCDUsage += WW.Name + ": " + WW_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _WW_DPS = WW.GetDPS(WW_GCDs);
            DPS_TTL += _WW_DPS;
            rageadd = WW.GetRageUsePerSecond(WW_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 2 : Bloodthirst on every CD
            float BT_GCDs = (float)Math.Min(availGCDs, BT.Activates);
            _BT_GCDs = BT_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BT_GCDs);
            GCDUsage += BT.Name + ": " + BT_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BT_DPS = BT.GetDPS(BT_GCDs);
            DPS_TTL += _BT_DPS;
            rageadd = BT.GetRageUsePerSecond(BT_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            doIterations();
            // Priority 3 : Bloodsurge Blood Proc (Do an Instant Slam) if available
            float BS_GCDs = (float)Math.Min(availGCDs, BS.Activates);
            _BS_GCDs = BS_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BS_GCDs);
            GCDUsage += BS.Name + ": " + BS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BS_DPS = BS.GetDPS(BS_GCDs);
            DPS_TTL += _BS_DPS;
            rageadd = BS.GetRageUsePerSecond(BS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            /*Sword Spec, Doesn't eat GCDs*/
            float SS_Acts = SS.GetActivates(NumGCDs, _Thunder_GCDs + _Ham_GCDs
                                                   + _Shatt_GCDs
                                                   + _BT_GCDs + _WW_GCDs * 2 + _BS_GCDs);
            _SS_Acts = SS_Acts;
            _SS_DPS = SS.GetDPS(SS_Acts);
            DPS_TTL += _SS_DPS;
            // TODO: Add Rage since it's a white hit
            
            doIterations();
            // Priority 4 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps

            availRage += WhiteAtks.MHRageGenPerSec + WhiteAtks.OHRageGenPerSec;

            Skills.OnAttack Which = null;
            bool ok = false;
            if (CalcOpts.MultipleTargets) {
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_      ]) { ok = true; Which = CL; }
            }else{
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_]) { ok = true; Which = HS; }
            }

            WhiteAtks.Slam_Freq = _SL_GCDs;
            if (ok) {
                float numHSPerSec = availRage / Which.FullRageCost;
                Which.OverridesPerSec = numHSPerSec;
                WhiteAtks.Ovd_Freq = numHSPerSec / WhiteAtks.MhEffectiveSpeed;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _OVD_DPS = Which.DPS;
                _OVD_PerHit = Which.DamageOnUse;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _OVD_DPS;
            }else{
                RageGenWhite = WHITEATTACKS.whiteRageGenPerSec;
                availRage += RageGenWhite;
                WhiteAtks.Ovd_Freq = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _OVD_DPS = 0f;
                _OVD_PerHit = 0f;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _OVD_DPS;
            }
            calcDeepWounds();

            GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            return DPS_TTL;
        }

        #region ArmsRotVariables
        public float _MS_DPS      = 0f; public float _MS_GCDs      = 0f;
        public float _RD_DPS      = 0f; public float _RD_GCDs      = 0f;
        public float _OP_DPS      = 0f; public float _OP_GCDs      = 0f;
        public float _TB_DPS      = 0f; public float _TB_GCDs      = 0f;
        public float _SD_DPS      = 0f; public float _SD_GCDs      = 0f;
        public float _SL_DPS      = 0f; public float _SL_GCDs      = 0f;
        public float _SS_DPS      = 0f; public float _SS_Acts      = 0f;
        public float _BLS_DPS     = 0f; public float _BLS_GCDs     = 0f;
        public float _SW_DPS      = 0f; public float _SW_GCDs      = 0f;
        public float _DW_PerHit   = 0f; public float _DW_DPS       = 0f; 
        //
        public float _Trink1_GCDs = 0f;
        public float _Trink2_GCDs = 0f;
        //
        public float _Thunder_GCDs= 0f; public float _TH_DPS       = 0f;
        public float _Blood_GCDs  = 0f;
        public float _ZRage_GCDs  = 0f;
        public float _Second_Acts = 0f;
        public float _Battle_GCDs = 0f;
        public float _Comm_GCDs   = 0f;
        public float _Demo_GCDs   = 0f;
        public float _Sunder_GCDs = 0f;
        public float _Ham_GCDs    = 0f;
        public float _Shatt_GCDs  = 0f; public float _Shatt_DPS    = 0f;
        public float _Death_GCDs  = 0f;
        public float _Reck_GCDs   = 0f;
        // GCD Losses
        public float _Stunned_GCDs = 0f;
        public float _HF_Acts      = 0f;
        public float _EM_Acts      = 0f;
        //
        public float _OVD_PerHit  = 0f; public float _OVD_DPS      = 0f;
        public float _WhiteDPSMH  = 0f; public float _WhiteDPSOH   = 0f;
        public float _WhiteDPS    = 0f; public float _WhitePerHit  = 0f;
        public float RageGenWhite = 0f; public float RageGenOther  = 0f; public float RageNeeded = 0f;
        public string GCDUsage = "";
        #endregion
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, Add DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, ref float _Abil_GCDs, ref float DPS_TTL,  ref float _Abil_DPS, Skills.Ability abil) {
            float acts = (float)Math.Min(availGCDs, abil.Activates);
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_DPS = abil.GetDPS(Abil_GCDs);
            DPS_TTL += _Abil_DPS;
            float rageadd = abil.GetRageUsePerSecond(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, Add DPS, Pull Rage, Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, ref float _Abil_GCDs, ref float DPS_TTL,  ref float _Abil_DPS, Skills.Ability abil, float GCDMulti) {
            float acts = (float)Math.Min(availGCDs, abil.Activates);
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs * GCDMulti);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + GCDMulti.ToString() + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_DPS = abil.GetDPS(Abil_GCDs);
            DPS_TTL += _Abil_DPS;
            float rageadd = abil.GetRageUsePerSecond(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, ref float _Abil_GCDs, Skills.Ability abil) {
            float acts = (float)Math.Min(availGCDs, abil.Activates);
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float rageadd = abil.GetRageUsePerSecond(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Add Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, ref float _Abil_GCDs, Skills.Ability abil,bool flag) {
            float acts = (float)Math.Min(availGCDs, abil.Activates);
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float rageadd = abil.GetRageUsePerSecond(Abil_GCDs);
            RageGenOther += rageadd;
            availRage += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: No GCDs, No DPS, Add Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float availRage, ref float _Abil_Acts, Skills.Ability abil) {
            float acts = abil.Activates;
            float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_Acts = Abil_Acts;
            GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + " (Doesn't use GCDs)\n" : "");
            float rageadd = abil.GetRageUsePerSecond(Abil_Acts);
            RageGenOther += rageadd;
            availRage += rageadd;
        }
        public float MakeRotationandDoDPS_Arms() {
            // Starting Numbers
            float DPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = (float)Math.Floor(FightDuration / LatentGCD);
            GCDUsage += NumGCDs.ToString("000") + " : Total GCDs\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;

            if (Char.MainHand == null) { return 0f; }

            // ==== Reasons GCDs would be lost ========
            float IronWillBonus = (float)Math.Round(20f/3f*Talents.IronWill) / 100f;
            // Being Stunned or Charmed
            if(CalcOpts.StunningTargets){
                // Assume you are Stunned for 3 GCDs (1.5+latency)*3 = ~1.6*3 = ~4.8 seconds per stun
                // Iron Will reduces the Duration of the stun by 7%,14%,20%
                // 100% perc means you are stunned the entire fight, the boss is stunning you every third GCD, basically only refreshing his stun
                //  50% perc means you are stunned half the fight, The boss is stunning you every sixth GCD
                float stunnedGCDs = (float)Math.Max(0f, FightDuration / (LatentGCD * 3f) * (CalcOpts.StunningTargetsPerc / 100f));
                float acts = (float)Math.Min(availGCDs, stunnedGCDs);
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _Stunned_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs * (3f * (1f - IronWillBonus)));
                GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x3-IronWillBonus : Stunned\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                if (Talents.HeroicFury > 0 && _Stunned_GCDs > 0f) {
                    float hfacts = HF.Activates;
                    _HF_Acts = (float)Math.Min(_Stunned_GCDs, hfacts);
                    GCDsused -= (float)Math.Min(NumGCDs, _HF_Acts * 2f);
                    GCDUsage += (_HF_Acts > 0 ? _HF_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : " + HF.Name + " (adds back to GCDs when stunned)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }
                if (CHARACTER.Race == CharacterRace.Human && (_Stunned_GCDs-_HF_Acts > 0)) {
                    float emacts = EM.Activates;
                    _EM_Acts = (float)Math.Min(_Stunned_GCDs - _HF_Acts, emacts);
                    GCDsused -= (float)Math.Min(NumGCDs, _EM_Acts * 2f);
                    GCDUsage += (_EM_Acts > 0 ? _EM_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : " + EM.Name + " (adds back to GCDs when stunned)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }
            }
            // Getting Incapacitated (E.g.- Igneous Pot)
            if(CalcOpts.StunningTargets){
                /*float acts = (float)Math.Min(availGCDs, abil.Activates);
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _Abil_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                float rageadd = abil.GetRageUsePerSecond(Abil_GCDs);
                availRage -= rageadd;
                RageNeeded += rageadd;*/
            }

            // ==== Rage Generation Priorities ========
            RageGenOther = RageGenAngerPerSec + RageGenWrathPerSec;
            if (StatS.BonusWarrior_T7_4P_RageProc != 0f) {
                RageGenOther += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (Talents.DeepWounds > 0f ? 1f : 0f);
                RageGenOther += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f);
            }
            availRage += RageGenOther;

            SndW.NumStunsOverDur = _Stunned_GCDs;
            /*Second Wind       */AddAnItem(ref availRage, ref _Second_Acts, SndW);
            /*Bloodrage         */AddAnItem(ref availRage, ref _Blood_GCDs , BR  );
            /*Berserker Rage    */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _ZRage_GCDs, BZ, false);

            // ==== Trinket Priorites =================
            // /*Trinket 1         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink1_GCDs, Trinket1);
            // /*Trinket 2         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink2_GCDs, Trinket2);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Battle_GCDs, BTS);
            /*Commanding Shout  */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Comm_GCDs, CS);
            /*Demoralizing Shout*/AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Demo_GCDs, DS);
            /*Sunder Armor      */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Sunder_GCDs, SN);
            /*Thunder Clap      */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Thunder_GCDs, ref DPS_TTL, ref _TH_DPS, TH);
            /*Hamstring         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Ham_GCDs, HMS);
            /*Shattering Throw  */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Shatt_GCDs, ref DPS_TTL, ref _Shatt_DPS, ST);
            /*Sweeping Strikes  */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _SW_GCDs, SW);
            /*Death Wish        */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Death_GCDs, Death);

            // ==== Standard Priorities ===============

            // These are solid and not dependant on other attacks
            /*Bladestorm        */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _BLS_GCDs,ref DPS_TTL, ref _BLS_DPS,BLS, 4f);
            /*Mortal Strike     */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _MS_GCDs, ref DPS_TTL, ref _MS_DPS, MS);
            /*Rend              */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _RD_GCDs, ref DPS_TTL, ref _RD_DPS, RD);
            /*Taste for Blood   */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _TB_GCDs, ref DPS_TTL, ref _TB_DPS, TB);

            // The following are dependant on other attacks as they are proccing abilities or are the fallback item
            // We need to loop these until the activates are relatively unchanged
            float origavailGCDs = availGCDs;
            float origGCDsused = GCDsused;
            float oldOPGCDs = 0f, oldSDGCDs = 0f, oldSLGCDs = 0f, oldSSActs = 0f;
            _OP_GCDs = 0f;
            _SD_GCDs = 0f;
            _SL_GCDs = origavailGCDs;
            _SS_Acts = 0f;
            int loopCounter = 0;
            while (
                    loopCounter < 500 &&
                    ((float)Math.Abs(_OP_GCDs - oldOPGCDs) > 0.1f ||
                     (float)Math.Abs(_SD_GCDs - oldSDGCDs) > 0.1f ||
                     (float)Math.Abs(_SL_GCDs - oldSLGCDs) > 0.1f ||
                     (Talents.SwordSpecialization > 0
                        && CombatFactors.MH.Type == ItemType.TwoHandSword
                        && (float)Math.Abs(_SS_Acts - oldSSActs) > 0.1f)
                    )
                  )
            {
                // Reset a couple of items so we can keep iterating
                availGCDs = origavailGCDs;
                GCDsused = origGCDsused;
                oldOPGCDs = _OP_GCDs; oldSDGCDs = _SD_GCDs; oldSLGCDs = _SL_GCDs; oldSSActs = _SS_Acts;
                WhiteAtks.Slam_Freq = _SL_GCDs;
                //Overpower
                float acts = (float)Math.Min(availGCDs, OP.GetActivates(
                    (_Thunder_GCDs + _Shatt_GCDs + _BLS_GCDs * 6f + _MS_GCDs + _SD_GCDs + _SL_GCDs) / NumGCDs, _SS_Acts
                    ));
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _OP_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Sudden Death
                acts = (float)Math.Min(availGCDs, SD.GetActivates(
                    (_Thunder_GCDs + _Shatt_GCDs + _BLS_GCDs * 6f + _MS_GCDs + _OP_GCDs + _TB_GCDs + _SD_GCDs + _SL_GCDs) / NumGCDs, _SS_Acts
                    ));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SD_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Slam for remainder of GCDs
                _SL_GCDs = SL.Validated ? availGCDs : 0f;
                GCDsused += (float)Math.Min(NumGCDs, _SL_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                if (Talents.SwordSpecialization > 0 && CombatFactors.MH.Type == ItemType.TwoHandSword) {
                    //Sword Spec, Doesn't eat GCDs
                    float SS_Acts = SS.GetActivates(NumGCDs, _Thunder_GCDs + _Ham_GCDs
                                                           + _Shatt_GCDs
                                                           + _BLS_GCDs * 6f + _MS_GCDs + _OP_GCDs + _TB_GCDs + _SD_GCDs + _SL_GCDs);
                    _SS_Acts = SS_Acts;
                    // TODO: Add Rage since it's a white hit
                }
                loopCounter++;
            }
            rageadd = SL.GetRageUsePerSecond(_SL_GCDs) + OP.GetRageUsePerSecond(_OP_GCDs) + SD.GetRageUsePerSecond(_SD_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
            GCDUsage += (_OP_GCDs > 0 ? _OP_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : " + OP.Name + "\n" : "");
            GCDUsage += (_SD_GCDs > 0 ? _SD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SD.Name + "\n" : "");
            GCDUsage += (_SL_GCDs > 0 ? _SL_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SL.Name + "\n" : "");
            _OP_DPS = OP.GetDPS(_OP_GCDs);
            _SD_DPS = SD.GetDPS(_SD_GCDs);
            _SL_DPS = SL.GetDPS(_SL_GCDs);
            if (Talents.SwordSpecialization > 0 && CombatFactors.MH.Type == ItemType.TwoHandSword) { _SS_DPS = SS.GetDPS(_SS_Acts); } else { _SS_DPS = 0f; }
            DPS_TTL += _OP_DPS + _SD_DPS + _SL_DPS + _SS_DPS;

            // Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps
            Skills.OnAttack Which = null;
            bool ok = false;
            if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_]) { ok = true; Which = HS; }
            if (CalcOpts.MultipleTargets) {
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_]) { ok = true; Which = CL; }
            }

            WhiteAtks.Slam_Freq = _SL_GCDs;
            float timelostwhilestunned = ((_Stunned_GCDs * 3f * (1f - IronWillBonus)) - _HF_Acts * 2f - _EM_Acts * 2f) * LatentGCD;
            if (ok) {
                availRage += (FightDuration - timelostwhilestunned) / WhiteAtks.MhEffectiveSpeed * WhiteAtks.MHSwingRage / (FightDuration);
                float numHSPerSec = availRage / Which.FullRageCost;
                Which.OverridesPerSec = numHSPerSec;
                WhiteAtks.Ovd_Freq = numHSPerSec / WhiteAtks.MhEffectiveSpeed;
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - (CalcOpts.StunningTargets ? (timelostwhilestunned / FightDuration) * CalcOpts.StunningTargetsPerc / 100f : 0f)); // MhWhiteDPS with loss of time in stun
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _OVD_DPS = Which.DPS;
                _OVD_PerHit = Which.DamageOnUse;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _OVD_DPS;
            }else{
                RageGenWhite = WHITEATTACKS.whiteRageGenPerSec;
                availRage += RageGenWhite;
                WhiteAtks.Ovd_Freq = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - (CalcOpts.StunningTargets ? (timelostwhilestunned / FightDuration) * CalcOpts.StunningTargetsPerc / 100f : 0f)); // MhWhiteDPS with loss of time in stun
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _OVD_DPS = 0f;
                _OVD_PerHit = 0f;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _OVD_DPS;
            }
            calcDeepWounds();

            GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs";

            // Return result
            return DPS_TTL;
        }
    }
}
