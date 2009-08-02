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

        // Rage Generators
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

            // Rage Generators
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
            // Rage Generators
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
            SL = new Skills.Slam(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);// actually arms but BS needs it
            BS = new Skills.BloodSurge(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SL,WW,BT);
            // Arms
            BLS= new Skills.Bladestorm(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,WW);
            MS = new Skills.MortalStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            TB = new Skills.TasteForBlood(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
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
            //float SW_Acts = _SW_GCDs    !=0?_SW_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_  ] ? SW.Activates  : 0f;
            float BLS_Acts= _BLS_GCDs     !=0?_BLS_GCDs     :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bladestorm_       ] ? BLS.Activates : 0f;

            float MS_Acts = _MS_GCDs      !=0?_MS_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_     ] ? MS.Activates  : 0f;
            float OP_Acts = _OP_GCDs      !=0?_OP_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_        ] ? OP.Activates  : 0f;
            float TB_Acts = _TB_GCDs      !=0?_TB_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_        ] ? OP.Activates  : 0f;
            float SD_Acts = _SD_GCDs      !=0?_SD_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SuddenDeath_      ] ? SD.Activates  : 0f;
            float SL_Acts = _SL_GCDs      !=0?_SL_GCDs      :0f;//CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Slam_             ] ?

            float Dable = (BLS_Acts * 6f * 2f) + TH_Acts + (ST_Acts/2f) + MS_Acts + SD_Acts + SL_Acts;
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
                    float MS_Acts = MS.Activates;
                    float OP_Acts = OP.Activates;
                    float TB_Acts = TB.Activates;
                    float SD_Acts = SD.Activates;
                    float HS_Acts = Which.Activates;
                    float LatentGCD = 1.5f + CalcOpts.GetLatency();
                    float SL_Acts = Math.Max(0f, CalcOpts.Duration / LatentGCD - MS_Acts - OP_Acts - SD_Acts);

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
            if (StatS.DreadnaughtBonusRageProc != 0f) {
                RageGenOther += 0.5f * (Talents.DeepWounds > 0f ? 1f : 0f);
                RageGenOther += 0.5f * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f);
            }
            availRage += RageGenOther;

            /*Bloodrage         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Blood_GCDs,  BR,false);
            /*Berserker Rage    */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _ZRage_GCDs,  BZ,false);

            // ==== Trinket Priorites =================
            /*Trinket 1         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink1_GCDs, Trinket1);
            /*Trinket 2         */AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref _Trink2_GCDs, Trinket2);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Battle_GCDs, BTS);
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
            
            if (ok) {
                WhiteAtks.Slam_Freq = _SL_GCDs;
                //availRage += FightDuration / (CombatFactors.MHSpeed + (1.5f - 0.5f * Talents.ImprovedSlam) / WhiteAtks.Slam_Freq) * WhiteAtks.GetSwingRage(Char.MainHand.Item, true) / FightDuration;
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
        public float _Battle_GCDs = 0f;
        public float _Demo_GCDs   = 0f;
        public float _Sunder_GCDs = 0f;
        public float _Ham_GCDs    = 0f;
        public float _Shatt_GCDs  = 0f; public float _Shatt_DPS    = 0f;
        public float _Death_GCDs  = 0f;
        public float _Reck_GCDs   = 0f;
        //
        public float _OVD_PerHit  = 0f; public float _OVD_DPS      = 0f;
        public float _WhiteDPSMH  = 0f; public float _WhiteDPSOH   = 0f;
        public float _WhiteDPS    = 0f; public float _WhitePerHit  = 0f;
        public float RageGenWhite = 0f; public float RageGenOther  = 0f; public float RageNeeded = 0f;
        public string GCDUsage = "";
        #endregion
        /// <summary>Adds an Ability alteration schtuff. Includes DPS.</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, 
                              ref float _Abil_GCDs, ref float DPS_TTL,  ref float _Abil_DPS, Skills.Ability abil) {
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
        /// <summary>Adds an Ability alteration schtuff. Includes DPS. Adds a GCD Multiplier.</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, 
                              ref float _Abil_GCDs, ref float DPS_TTL,  ref float _Abil_DPS, Skills.Ability abil,
                              float GCDMulti) {
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
        /// <summary>Adds an Ability alteration schtuff. Does not include DPS.</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, 
                              ref float _Abil_GCDs, Skills.Ability abil) {
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
        /// <summary>Adds an Ability alteration schtuff. Not DPS but add rage instead of remove it</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, 
                              ref float _Abil_GCDs, Skills.Ability abil,bool flag) {
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

            // ==== Rage Generation Priorities ========
            RageGenOther = RageGenAngerPerSec + RageGenWrathPerSec;
            if (StatS.DreadnaughtBonusRageProc != 0f) {
                RageGenOther += 0.5f * (Talents.DeepWounds > 0f ? 1f : 0f);
                RageGenOther += 0.5f * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f);
            }
            availRage += RageGenOther;

            /*Bloodrage         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Blood_GCDs,  BR,false);
            /*Berserker Rage    */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _ZRage_GCDs,  BZ,false);

            // ==== Trinket Priorites =================
            /*Trinket 1         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Trink1_GCDs, Trinket1);
            /*Trinket 2         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Trink2_GCDs, Trinket2);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Battle_GCDs, BTS);
            /*Demoralizing Shout*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Demo_GCDs,   DS);
            /*Sunder Armor      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Sunder_GCDs, SN);
            /*Thunder Clap      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Thunder_GCDs,ref DPS_TTL,ref _TH_DPS,TH);
            /*Hamstring         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Ham_GCDs,    HMS);
            /*Shattering Throw  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Shatt_GCDs,  ref DPS_TTL,ref _Shatt_DPS,ST);
            /*Sweeping Strikes  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _SW_GCDs,     SW);
            /*Death Wish        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _Death_GCDs,  Death);

            // ==== Standard Priorities ===============
            /*Bladestorm        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _BLS_GCDs,    ref DPS_TTL,ref _BLS_DPS,BLS,4f);
            /*Mortal Strike     */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _MS_GCDs,     ref DPS_TTL,ref _MS_DPS ,MS);
            /*Rend              */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _RD_GCDs,     ref DPS_TTL,ref _RD_DPS ,RD);
            /*Overpower         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _OP_GCDs,     ref DPS_TTL,ref _OP_DPS ,OP,2f);
            /*Taste for Blood   */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _TB_GCDs,     ref DPS_TTL,ref _TB_DPS ,TB);
            /*Sudden Death      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,ref _SD_GCDs,     ref DPS_TTL,ref _SD_DPS ,SD);
            /*Slam for remainder of GCDs*/
            float SL_GCDs = SL.Validated ? availGCDs : 0f;
            _SL_GCDs = SL_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, SL_GCDs);
            GCDUsage += (SL_GCDs > 0 ? SL_GCDs.ToString("000") + " : " + SL.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            rageadd = SL.GetRageUsePerSecond(SL_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            /*Sword Spec, Doesn't eat GCDs*/
            float SS_Acts = SS.GetActivates(NumGCDs, _Thunder_GCDs + _Ham_GCDs
                                                   + _Shatt_GCDs
                                                   + _BLS_GCDs * 6 + _MS_GCDs + _OP_GCDs + _TB_GCDs + _SD_GCDs + _SL_GCDs);
            _SS_Acts = SS_Acts;
            _SS_DPS = SS.GetDPS(SS_Acts);
            DPS_TTL += _SL_DPS;
            // TODO: Add Rage since it's a white hit

            // Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps
            Skills.OnAttack Which = null;
            bool ok = false;
            if (CalcOpts.MultipleTargets) {
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_      ]) { ok = true; Which = CL; }
            }else{
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_]) { ok = true; Which = HS; }
            }
            
            if (ok) {
                WhiteAtks.Slam_Freq = _SL_GCDs;
                float slamspeedadd = WhiteAtks.Slam_Freq == 0 ? 0 : ((1.5f - 0.5f * Talents.ImprovedSlam) / (WhiteAtks.Slam_Freq));
                availRage += FightDuration / (WhiteAtks.MhEffectiveSpeed) * WhiteAtks.MHSwingRage / FightDuration;
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

            GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs";

            // Return result
            return DPS_TTL;
        }
    }
}
