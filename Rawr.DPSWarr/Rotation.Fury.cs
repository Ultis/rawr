/**********
 * Owner: Ebs
 **********/
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class FuryRotation : Rotation
    {
        public FuryRotation(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = new CombatFactors(Char, StatS);
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            //WhiteAtks = new Skills.WhiteAttacks(Char, StatS);
            // Initialize();
        }

        #region FuryRotVariables
        public Skills.BloodThirst BT;
        public Skills.BloodSurge BS;

        public const float ROTATION_LENGTH_FURY = 8.0f;
        
        float _bloodsurgeRPS;
        public float _BS_DPS = 0f, _BS_HPS = 0f, _BS_GCDs = 0f;
        public float _BT_DPS = 0f, _BT_HPS = 0f, _BT_GCDs = 0f;
        #endregion

        public override void Initialize(CharacterCalculationsDPSWarr calcs)
        {
            base.Initialize(calcs);
            calcs.BT = BT;
            calcs.BS = BS;
        }

        protected override void initAbilities()
        {
            base.initAbilities();
            BT = new Skills.BloodThirst(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BS = new Skills.BloodSurge(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS, SL, WW, BT);
        }

        protected void new_doIterations()
        {
            base.doIterations();

            HS.OverridesOverDur = 0f;
            WhiteAtks.HSOverridesOverDur = 0f;
            WhiteAtks.CLOverridesOverDur = 0f;

            float bsBaseRage = BS.RageUseOverDur;
            float hsRageUsed = (FreeRageOverDur - bsBaseRage) / (1f + HS.FullRageCost * (Talents.Bloodsurge * 0.20f / 3f));
            
        }
        protected override void doIterations()
        {
            base.doIterations();
            // Fury Iteration
            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            float hsPercOvd, clPercOvd; // what percentage of overrides are cleave and hs
            hsPercOvd = (hsok ? 1f : 0f);
            if (CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_])
                hsPercOvd -= CalcOpts.MultipleTargetsPerc / 100f;
            clPercOvd = (clok ? 1f - hsPercOvd : 0f);

            float hsRageUsed = FreeRageOverDur * hsPercOvd;
            float clRageUsed = FreeRageOverDur * clPercOvd;

            WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
            WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;

            float oldHSActivates = 0f, newHSActivates = HS.Activates;
            float oldCLActivates = 0f, newCLActivates = CL.Activates;
            BS.maintainActs = MaintainCDs;
            int loopIterator;
            for (loopIterator = 0;
                 CalcOpts.FuryStance
                    && loopIterator < 50
                    && (Math.Abs(newHSActivates - oldHSActivates) > 1f
                        || Math.Abs(newCLActivates - oldCLActivates) > 1f);
                  loopIterator++)
            {
                oldHSActivates = HS.Activates;
                oldCLActivates = CL.Activates;
                //
                BS.hsActivates = oldHSActivates; // bloodsurge only cares about HSes, not Cleaves
                _bloodsurgeRPS = (BS.RageUseOverDur);
                hsRageUsed = FreeRageOverDur * hsPercOvd;
                clRageUsed = FreeRageOverDur * clPercOvd;
                WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
                WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;
                //
                newHSActivates = HS.Activates;
                newCLActivates = CL.Activates;
            }

            BS.hsActivates = newHSActivates;
            BS.hsActivates += newCLActivates;
            if (CalcOpts.FuryStance)
            {
                _HS_DPS = HS.DPS;
                _CL_DPS += CL.DPS;
                _HS_PerHit = HS.DamageOnUse * hsPercOvd;
                _CL_PerHit = CL.DamageOnUse * clPercOvd;
            }
        }

        #region LandedAtks
        public override float GetLandedYellowsOverDurMH()
        {
            float ret = base.GetLandedYellowsOverDurMH();
            ret += _BT_GCDs * BT.MHAtkTable.AnyLand * BT.AvgTargets
                 + _BS_GCDs * BS.MHAtkTable.AnyLand * BS.AvgTargets;
            return ret;
                
        }
        public override float GetCriticalYellowsOverDurMH()
        {
            float ret = base.GetCriticalYellowsOverDurMH();
            return ret + _BT_GCDs * BT.MHAtkTable.Crit * BT.AvgTargets
                       + _BS_GCDs * BS.MHAtkTable.Crit * BS.AvgTargets;
        }
        public override float GetParriedYellowsOverDur()
        {
            return base.GetParriedYellowsOverDur()
                + _BT_GCDs * BT.MHAtkTable.Parry * BT.AvgTargets
                + _BS_GCDs * BS.MHAtkTable.Parry * BS.AvgTargets;
        }
        public override float GetCriticalYellowsOverDurOH()
        {
            return base.GetCriticalYellowsOverDurOH();
        }
        public override float GetDodgedYellowsOverDur()
        {
            return base.GetDodgedYellowsOverDur()
                + _BT_GCDs * BT.MHAtkTable.Dodge * BT.AvgTargets
                + _BS_GCDs * BS.MHAtkTable.Dodge * BS.AvgTargets;
        }
        public override float GetLandedYellowsOverDurOH()
        {
            return base.GetLandedYellowsOverDurOH();
        }
        #endregion
        protected override float RageNeededOverDur
        {
            get
            {
                float BTRage         = BT.GetRageUseOverDur(_BT_GCDs);
                float BloodSurgeRage = _bloodsurgeRPS;// BS.RageUsePerSecond;

                return base.RageNeededOverDur + BTRage + BloodSurgeRage;
            }
        }

        public void new_MakeRotationandDoDPS(bool setCalcs)
        {
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = CalcOpts.Duration / LatentGCD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";

            // Maintenance abilities

            // First, apply initial debuffs
            float bloodsurge_percUsed = 1f; // Since we can only bloodsurge once every 8secs, 
                                            // this keeps track of how many times we can actually slam vs refresh an ability
            
            if (SN.Validated) // Sunder
            {
                NumGCDs -= 5f / SN.MHAtkTable.AnyLand; // initial application
                bloodsurge_percUsed -= 1f / (int)(SN.Duration / 8f); // keep it up
            }
            if (TH.Validated) // Thunderclap
            {
                NumGCDs -= 1f / TH.MHAtkTable.AnyLand; // initial application -- TODO: Remove support for tclap in general?
                bloodsurge_percUsed -= 1f / (int)(TH.Duration / 8f); // keep it up
            }
            if (DS.Validated) // Demo Shout
            {
                NumGCDs -= 1f / DS.MHAtkTable.AnyLand; // initial application
                bloodsurge_percUsed -= 1f / (int)(DS.Duration / 8f); // keep it up
            }
            // Assuming these are already applied at the start of the fight
            if (BTS.Validated)
            {
                bloodsurge_percUsed -= 1f / (int)(BTS.Duration / 8f);
            }
            if (CS.Validated)
            {
                bloodsurge_percUsed -= 1f / (int)(CS.Duration / 8f);
            }
            bloodsurge_percUsed = Math.Max(bloodsurge_percUsed, 0f);

        }

        public override void MakeRotationandDoDPS(bool setCalcs)
        {
            new_MakeRotationandDoDPS(setCalcs);
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = FightDuration / LatentGCD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;
            //float timelostwhilestunned = 0f;
            float percTimeInStun = 0f;

            if (Char.MainHand == null) { return; }

            //doIterations();

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;
            
            /*Bloodrage         */
            AddAnItem(ref availRage, percTimeInStun, ref _Blood_GCDs, ref HPS_TTL, ref _Blood_HPS, BR);
            /*Berserker Rage    */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _ZRage_GCDs, ref HPS_TTL, ref _ZRage_HPS, BZ, false);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Battle_GCDs, ref HPS_TTL, ref _Battle_HPS, BTS);
            /*Commanding Shout  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Comm_GCDs, ref HPS_TTL, ref _Comm_HPS, CS);
            /*Demoralizing Shout*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Demo_GCDs, ref HPS_TTL, ref _Demo_HPS, DS);
            /*Sunder Armor      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Sunder_GCDs, ref HPS_TTL, ref _Sunder_HPS, SN);
            /*Thunder Clap      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Thunder_GCDs, ref DPS_TTL, ref HPS_TTL, ref _TH_DPS, ref _TH_HPS, TH);
            /*Hamstring         */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Ham_GCDs, ref HPS_TTL, ref _Ham_HPS, HMS);
            /*Shattering Throw  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Shatt_GCDs, ref DPS_TTL, ref HPS_TTL, ref _Shatt_DPS, ref _Shatt_HPS, ST);
            /*Enraged Regeneratn*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _ER_GCDs, ref HPS_TTL, ref _ER_HPS, ER);
            /*Sweeping Strikes  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _SW_GCDs, ref HPS_TTL, ref _SW_HPS, SW);
            /*Death Wish        */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Death_GCDs, ref HPS_TTL, ref _Death_HPS, Death);

            /*float Reck_GCDs = (float)Math.Min(availGCDs, RK.Activates);
            _Reck_GCDs = Reck_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Reck_GCDs);
            GCDUsage += RK.Name + ": " + Reck_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            rageadd = RK.GetRageUsePerSecond(Reck_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;*/

            //doIterations();

            // Priority 1 : Whirlwind on every CD
            float WW_GCDs = (float)Math.Min(availGCDs, WW.Activates);
            _WW_GCDs = WW_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, WW_GCDs);
            GCDUsage += WW.Name + ": " + WW_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _WW_DPS = WW.GetDPS(WW_GCDs);
            _WW_HPS = WW.GetHPS(WW_GCDs);
            DPS_TTL += _WW_DPS;
            HPS_TTL += _WW_HPS;
            rageadd = WW.GetRageUseOverDur(WW_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 2 : Bloodthirst on every CD
            float BT_GCDs = (float)Math.Min(availGCDs, BT.Activates);
            _BT_GCDs = BT_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BT_GCDs);
            GCDUsage += BT.Name + ": " + BT_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BT_DPS = BT.GetDPS(BT_GCDs);
            _BT_HPS = BT.GetHPS(BT_GCDs);
            DPS_TTL += _BT_DPS;
            HPS_TTL += _BT_HPS;
            rageadd = BT.GetRageUseOverDur(BT_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            //doIterations();
            // Priority 3 : Bloodsurge Blood Proc (Do an Instant Slam) if available
            float BS_GCDs = (float)Math.Min(availGCDs, BS.Activates);
            _BS_GCDs = BS_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BS_GCDs);
            GCDUsage += BS.Name + ": " + BS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BS_DPS = BS.GetDPS(BS_GCDs);
            _BS_HPS = BS.GetHPS(BS_GCDs);
            DPS_TTL += _BS_DPS;
            HPS_TTL += _BS_HPS;
            rageadd = BS.GetRageUseOverDur(BS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            //Sword Spec, Doesn't eat GCDs
            /*float SS_Acts = SS.GetActivates(GetLandedYellowsOverDur());
            _SS_Acts = SS_Acts;
            _SS_DPS = SS.GetDPS(SS_Acts);
            DPS_TTL += _SS_DPS;*/
            // TODO: Add Rage since it's a white hit

            //doIterations();
            // Priority 4 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps

            availRage += WhiteAtks.MHRageGenOverDur + WhiteAtks.OHRageGenOverDur;

            bool HSok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool CLok = CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            WhiteAtks.Slam_Freq = 0f;// _SL_GCDs;
            if ((HSok || CLok) && availRage > 0f)
            {
                float numHSOverDur = availRage / HS.FullRageCost;
                HS.OverridesOverDur = numHSOverDur;
                WhiteAtks.HSOverridesOverDur = numHSOverDur;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPSOH = WhiteAtks.OhDPS;
                _WhiteDPS = _WhiteDPSMH + _WhiteDPSOH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_DPS = HS.DPS;
                _HS_PerHit = HS.DamageOnUse;
                _CL_DPS = CL.DPS;
                _CL_PerHit = CL.DamageOnUse;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _HS_DPS;
                //DPS_TTL += _CL_DPS;
            }
            else
            {
                RageGenWhite = WHITEATTACKS.whiteRageGenOverDur;
                //availRage += RageGenWhite;
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_DPS = _HS_PerHit = _HS_Acts = 0;
                _CL_DPS = _CL_PerHit = _CL_Acts = 0;
                DPS_TTL += _WhiteDPS;
            }
            calcDeepWounds();
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
            DPS_TTL += _DW_DPS;

            GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            _HPS_TTL = HPS_TTL;

            if (setCalcs)
            {
                this.calcs.TotalDPS = DPS_TTL;
                this.calcs.WhiteDPS = this._WhiteDPS;

                this.calcs.WhiteRage = this.RageGenWhite;
                this.calcs.OtherRage = this.RageGenOther;
                this.calcs.NeedyRage = this.RageNeeded;
                this.calcs.FreeRage = this.RageGenWhite + this.RageGenOther - this.RageNeeded;
            }
        }
    }
}