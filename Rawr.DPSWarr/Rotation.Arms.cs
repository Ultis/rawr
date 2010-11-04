/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class ArmsRotation : Rotation {
        public ArmsRotation(Character character, Stats stats, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            //TalentsCata = Char == null || Char.WarriorTalentsCata == null ? new WarriorTalentsCata() : Char.WarriorTalentsCata;
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            BossOpts = (bo == null ? new BossOptions() : bo);
            WhiteAtks = wa;

            _cachedLatentGCD = 1.5f + CalcOpts.Latency + CalcOpts.AllowedReact;
            _cachedNumGCDs = FightDuration / LatentGCD;
            _cachedNumGCDsU20 = FightDuration / LatentGCD * (float)BossOpts.Under20Perc;
            // Initialize();
        }
        #region Variables
        // Ability Declarations
        public Skills.FakeWhite FW;
        #endregion
        #region Initialization
        public override void Initialize(CharacterCalculationsDPSWarr calcs) {
            base.Initialize(calcs);
            calcs.FW = FW;
        }
        #endregion
        #region Rage Calcs
        /*protected override float RageNeededOverDur {
            get {
                float SweepingRage = SW.GetRageUseOverDur(_SW_GCDs);
                float BladestormRage = BLS.GetRageUseOverDur(_BLS_GCDs);
                float MSRage = MS.GetRageUseOverDur(_MS_GCDs);
                float RendRage = RD.GetRageUseOverDur(_RD_GCDs);
                float OPRage = OP.GetRageUseOverDur(_OP_GCDs);
                float TBRage = TB.GetRageUseOverDur(_TB_GCDs);
                float SDRage = SD.GetRageUseOverDur(_SD_GCDs);
                return base.RageNeededOverDur + SweepingRage + BladestormRage + MSRage + 
                    RendRage + OPRage + TBRage + SDRage;
            }
        }*/
        #endregion
        protected override void calcDeepWounds() { base.calcDeepWounds(); }

        public float SettleAll(float totalPercTimeLost, float percTimeUnder20, float availRage, out float PercFailRage)
        {
            float percTimeOver20 = (1f - percTimeUnder20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndOver20 = percTimeOver20 * percTimeInDPS;
            availRage -= DoMaintenanceActivates(totalPercTimeLost) * percTimeOver20;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Slam sucks up all the cooldowns (except under <20% with that active, where Exec sucks all of them)
             * Heroic Strike and Cleave won't be used at all
             * Sudden Death Free Rage is minimum cost, no extra rage available
             * Execute Free Rage is minimum cost, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Sudden Death will get extra rage leftovers if there are any
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDs = GCDsAvailable, preloopGCDsUsed = GCDsUsed, preloopAvailRage = availRage;

            float origNumGCDs = (FightDuration / LatentGCD) * percTimeOver20,
                  origavailGCDs = preloopAvailGCDs * percTimeOver20,
                  origGCDsused = preloopGCDsUsed * percTimeOver20;
            float oldBLSGCDs = 0f, oldMSGCDs = 0f, oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f, oldEXGCDs = 0f, oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f, oldSoOActs = 0f;

            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();

            Execute EX_ability = EX.ability as Execute;
            
            SL.numActivates = origavailGCDs;
            WhiteAtks.Slam_ActsOverDur = SL.numActivates;
            EX_ability.FreeRage = EX_ability.RageCost;
            float origAvailRage = preloopAvailRage * percTimeOver20;
            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];
            availRage += WhiteAtks.whiteRageGenOverDurNoHS * percTimeInDPS * percTimeOver20;
            availRage -= SL.Rage;
            float repassAvailRage = 0f;
            PercFailRage = 1f;

            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (
                     Math.Abs(BLS.numActivates - oldBLSGCDs) > 0.1f ||
                     Math.Abs(MS.numActivates - oldMSGCDs) > 0.1f ||
                     Math.Abs(RD.numActivates - oldRDGCDs) > 0.1f ||
                     Math.Abs(OP.numActivates - oldOPGCDs) > 0.1f ||
                     Math.Abs(TB.numActivates - oldTBGCDs) > 0.1f ||
                     Math.Abs(CS.numActivates - oldCSGCDs) > 0.1f ||
                     Math.Abs(HS.numActivates - oldHSGCDs) > 0.1f ||
                     Math.Abs(CL.numActivates - oldCLGCDs) > 0.1f ||
                     Math.Abs(TH.numActivates - oldTHGCDs) > 0.1f ||
                     Math.Abs(VR.numActivates - oldVRGCDs) > 0.1f ||
                     Math.Abs(SL.numActivates - oldSLGCDs) > 0.1f ||
                     Math.Abs(SoO.numActivates - oldSoOActs) > 0.1f ||
                     (percTimeUnder20 > 0 && Math.Abs(EX.numActivates - oldEXGCDs) > 0.1f)))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                WhiteAtks.Slam_ActsOverDur = SL.numActivates;
                oldBLSGCDs = BLS.numActivates; oldMSGCDs = MS.numActivates; oldRDGCDs = RD.numActivates; oldOPGCDs = OP.numActivates; oldTBGCDs = TB.numActivates;
                oldEXGCDs = EX.numActivates; oldSLGCDs = SL.numActivates;
                oldCSGCDs = CS.numActivates; oldVRGCDs = VR.numActivates; oldHSGCDs = HS.numActivates; oldCLGCDs = CL.numActivates;
                oldSoOActs = SoO.numActivates;
                BLS.numActivates = MS.numActivates = RD.numActivates = OP.numActivates = TB.numActivates = CS.numActivates = EX.numActivates =
                    SL.numActivates = TH.numActivates = VR.numActivates = HS.numActivates = CL.numActivates = 0;
                availRage = origAvailRage;
                availRage += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * percTimeOver20;

                float acts;
                float RDspace, BLSspace, MSspace, TFBspace, OPspace, CSspace, SLspace, HSspace, CLspace, THspace, VRspace;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRage < 0f || PercFailRage != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRage *= 1f + repassAvailRage / (availRage - repassAvailRage); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                } else { PercFailRage = 1f; }
                
                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDur) * percTimeInDPSAndOver20 * PercFailRage);
                    CS.numActivates = acts;
                    availRage -= CS.Rage;
                }
                CSspace = CS.numActivates / NumGCDs * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, RD.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    RD.numActivates = acts;
                    availRage -= RD.Rage;
                }
                RDspace = RD.numActivates / NumGCDs * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, TH.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    TH.numActivates = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderApps = TH.numActivates * Talents.BloodAndThunder * 0.50f;
                    availRage -= TH.Rage;
                }
                THspace = TH.numActivates / NumGCDs * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, BLS.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    BLS.numActivates = acts * (1f - RDspace);
                    availRage -= BLS.Rage;
                }
                BLSspace = BLS.numActivates / NumGCDs * BLS.ability.UseTime / LatentGCD;

                // Mortal Strike
                if (MS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, MS.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    MS.numActivates = acts * (1f - BLSspace);
                    availRage -= MS.Rage;
                }
                MSspace = MS.numActivates / NumGCDs * MS.ability.UseTime / LatentGCD;

                // Taste for Blood
                float OPGCDReduc = (OP.ability.Cd < LatentGCD ? (OP.ability.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                if (TB.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, TB.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    TB.numActivates = acts * (1f - BLSspace);
                    availRage -= TB.Rage;
                }
                TFBspace = TB.numActivates / NumGCDs * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivates) * percTimeInDPSAndOver20 * PercFailRage);
                    OP.numActivates = acts * (1f - TFBspace - RDspace - BLSspace - MSspace);
                    availRage -= OP.Rage;
                }
                OPspace = OP.numActivates / NumGCDs * OP.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated) {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.ability.Validated && VR.ability.DamageOnUse > SL.ability.DamageOnUse)
                        || (HS.ability.Validated && PercFailRage == 1f && VR.ability.DamageOnUse > SL.ability.DamageOnUse))
                    {
                        acts = Math.Min(GCDsAvailable, VR.ability.Activates * percTimeInDPSAndOver20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.numActivates = acts * (1f - BLSspace);
                        //availRage -= VR.Rage; // it's free
                    }
                }
                VRspace = VR.numActivates / NumGCDs * VR.ability.UseTime / LatentGCD;
               
                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Heroic Strikes/Cleaves
                if (PercFailRage == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndOver20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRage / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivates = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivates = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRage -= HS.Rage + CL.Rage;
                } else if (PercFailRage == 1f && (HS.ability.Validated && !CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndOver20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRage / HS.ability.RageCost;
                    CL.numActivates = 0f;
                    HS.numActivates = Math.Min(hsActs, acts);
                    availRage -= HS.Rage;
                } else if (PercFailRage == 1f && (!HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, CL.ability.Activates * percTimeInDPSAndOver20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivates = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivates = 0f;
                    availRage -= CL.Rage;
                } else { CL.numActivates = HS.numActivates = 0f; }

                // Slam
                if (SL.ability.Validated && PercFailRage != 1) {
                    acts = Math.Min(GCDsAvailable, GCDsAvailable/*SL.Activates*/ * percTimeInDPS);
                    if (SL.ability.GetRageUseOverDur(acts) > availRage) acts = Math.Max(0f, availRage) / SL.ability.RageCost;
                    SL.numActivates = acts;
                    availRage -= SL.Rage;
                } else if (SL.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, GCDsAvailable/*SL.Activates*/ * percTimeInDPS);
                    SL.numActivates = acts;
                    availRage -= SL.Rage;
                } else { SL.numActivates = 0f; }

                HSspace = HS.numActivates / NumGCDs * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivates / NumGCDs * CL.ability.UseTime / LatentGCD;
                SLspace = SL.numActivates / NumGCDs * SL.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivates);
                WhiteAtks.Slam_ActsOverDur = SL.numActivates;

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivates = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDur) * percTimeOver20;
                    availRage -= SoO.Rage; // Not sure if it should affect Rage
                }

                float TotalSpace = (RDspace + THspace + BLSspace + MSspace + OPspace + TFBspace + CSspace + SLspace + HSspace + CLspace + VRspace);
                repassAvailRage = availRage; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeeded = 0f, rageGenOther = 0f;
            foreach (AbilWrapper aw in GetAbilityList())
            {
                if (aw.ability is Rend) {
                    DPS_TTL += aw.ability.GetDPS(aw.numActivates + TH.numActivates);
                } else {
                    DPS_TTL += aw.DPS;
                }
                _HPS_TTL += aw.HPS;
                if (aw.Rage > 0) { rageNeeded += aw.Rage; }
                else { rageGenOther -= aw.Rage; }
            }

            DPS_TTL += (WhiteAtks.MhDPS + (CombatFactors.useOH ? WhiteAtks.OhDPS : 0f)) * percTimeInDPSAndOver20;
            // InvalidateCache();
            return DPS_TTL;
        }

        public float SettleAll_U20(float totalPercTimeLost, float percTimeUnder20, float availRage, out float PercFailRageUnder20)
        {
            float percTimeOver20 = (1f - percTimeUnder20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndUnder20 = percTimeUnder20 * percTimeInDPS;
            availRage -= DoMaintenanceActivates(totalPercTimeLost) * percTimeUnder20;
            //availGCDs = NumGCDs - GCDsused;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Slam sucks up all the cooldowns (except under <20% with that active, where Exec sucks all of them)
             * Heroic Strike and Cleave won't be used at all
             * Sudden Death Free Rage is minimum cost, no extra rage available
             * Execute Free Rage is minimum cost, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Sudden Death will get extra rage leftovers if there are any
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDs = GCDsAvailable, preloopGCDsUsed = GCDsUsed, preloopAvailRage = availRage;

            float origNumGCDs = (FightDuration / LatentGCD) * (/*1f -*/ percTimeUnder20),
                  origavailGCDs = preloopAvailGCDs * (/*1f -*/ percTimeUnder20),
                  origGCDsused = preloopGCDsUsed * (/*1f -*/ percTimeUnder20);
            float oldBLSGCDs = 0f, //oldMSGCDs = 0f,
                  oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f,
                  oldEXGCDs = 0f, //oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f, oldSoOActs = 0f, oldCLGCDs = 0f;

            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();

            Execute EX_ability = EX.ability as Execute;

            EX.numActivatesU20 = origavailGCDs;
            WhiteAtks.Slam_ActsOverDur = 0f;// SL.numActivates;
            EX_ability.FreeRage = EX_ability.RageCost;
            float newHSActs = HS.numActivatesU20 = 0f;
            float newCLActs = CL.numActivatesU20 = 0f;
            float origAvailRage = preloopAvailRage * (/*1f -*/ percTimeUnder20);
            availRage += WhiteAtks.whiteRageGenOverDurNoHS * percTimeInDPS * (/*1f -*/ percTimeUnder20);
            availRage -= EX.RageU20;
            float repassAvailRage = 0f;
            PercFailRageUnder20 = 1f;

            int Iterator = 0;
            #region <20%
            while (
                    Iterator < 50 && (
                     Math.Abs(BLS.numActivates - oldBLSGCDs) > 0.1f ||
                     //Math.Abs(MS.numActivates - oldMSGCDs) > 0.1f ||
                     Math.Abs(RD.numActivates - oldRDGCDs) > 0.1f ||
                     Math.Abs(OP.numActivates - oldOPGCDs) > 0.1f ||
                     Math.Abs(TB.numActivates - oldTBGCDs) > 0.1f ||
                     Math.Abs(CS.numActivates - oldCSGCDs) > 0.1f ||
                     Math.Abs(HS.numActivates - oldHSGCDs) > 0.1f ||
                     Math.Abs(CL.numActivates - oldCLGCDs) > 0.1f ||
                     Math.Abs(TH.numActivates - oldTHGCDs) > 0.1f ||
                     Math.Abs(VR.numActivates - oldVRGCDs) > 0.1f ||
                     //Math.Abs(SL.numActivates - oldSLGCDs) > 0.1f ||
                     Math.Abs(SoO.numActivates - oldSoOActs) > 0.1f ||
                     (percTimeUnder20 > 0 && Math.Abs(EX.numActivates - oldEXGCDs) > 0.1f)))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                oldBLSGCDs = BLS.numActivatesU20; /*oldMSGCDs = MS.numActivates;*/ oldRDGCDs = RD.numActivatesU20; oldOPGCDs = OP.numActivatesU20; oldTBGCDs = TB.numActivatesU20;
                oldEXGCDs = EX.numActivatesU20; /*oldSLGCDs = SL.numActivatesU20;*/
                oldCSGCDs = CS.numActivatesU20;
                BLS.numActivatesU20 = /*MS.numActivatesU20 =*/ RD.numActivatesU20 = OP.numActivatesU20 = TB.numActivatesU20 = CS.numActivatesU20 =
                    EX.numActivatesU20 = SL.numActivatesU20 = HS.numActivatesU20 = CL.numActivatesU20 = TH.numActivatesU20 = VR.numActivatesU20 = 0;
                availRage = origAvailRage;
                availRage += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * (/*1f -*/ percTimeUnder20);

                float acts;
                float CSspace, RDspace, BLSspace, /*MSspace,*/ TFBspace, OPspace, EXspace/*, SLspace*/, HSspace, CLspace, THspace, VRspace;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRage < 0f || PercFailRageUnder20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageUnder20 *= 1f + repassAvailRage / (availRage - repassAvailRage); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                } else { PercFailRageUnder20 = 1f; }

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDur) * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    CS.numActivatesU20 = acts;
                    availRage -= CS.RageU20;
                }
                CSspace = CS.numActivatesU20 / NumGCDs * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, RD.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    RD.numActivatesU20 = acts;
                    availRage -= RD.RageU20;
                }
                RDspace = RD.numActivatesU20 / NumGCDs * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, TH.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    TH.numActivatesU20 = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderAppsU20 = TH.numActivatesU20 * Talents.BloodAndThunder * 0.50f;
                    availRage -= TH.RageU20;
                }
                THspace = TH.numActivatesU20 / NumGCDs * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, BLS.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    BLS.numActivatesU20 = acts * (1f - RDspace);
                    availRage -= BLS.RageU20;
                }
                BLSspace = BLS.numActivatesU20 / NumGCDs * BLS.ability.UseTime / LatentGCD;

                /*// Mortal Strike // MS doesn't get used in Exec phase
                if (MS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, MS.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    MS.numActivatesU20 = acts * (1f - BLSspace);
                    availRage -= MS.RageU20;
                }
                MSspace = MS.numActivatesU20 / NumGCDs * MS.ability.UseTime / LatentGCD;*/

                // Taste for Blood
                float OPGCDReduc = (OP.ability.Cd < LatentGCD ? (OP.ability.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                if (TB.ability.Validated) {
                    if (EX.ability.Validated && EX.ability.DamageOnUseOverride < TB.ability.DamageOnUseOverride) {
                        acts = Math.Min(GCDsAvailableU20, TB.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                        TB.numActivatesU20 = acts * (1f - BLSspace);
                        availRage -= TB.RageU20;
                    }
                }
                TFBspace = TB.numActivatesU20 / NumGCDs * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated) {
                    if (EX.ability.Validated && EX.ability.DamageOnUseOverride < OP.ability.DamageOnUseOverride) {
                        acts = Math.Min(GCDsAvailableU20, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                        OP.numActivatesU20 = acts * (1f - TFBspace - RDspace - BLSspace /*- MSspace*/);
                        availRage -= OP.RageU20;
                    }
                }
                OPspace = OP.numActivatesU20 / NumGCDs * OP.ability.UseTime / LatentGCD;

                // Skip Victory Rush, it's useless here

                // Heroic Strikes/Cleaves
                // I dont think we should HS/CL during this phase because Exec should do more damage for less/same Rage
                /*if (PercFailRageUnder20 == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRage / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivates = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivates = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRage -= HS.Rage + CL.Rage;
                } else if (PercFailRageUnder20 == 1f && (HS.ability.Validated && !CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRage / HS.ability.RageCost;
                    CL.numActivates = 0f;
                    HS.numActivates = Math.Min(hsActs, acts);
                    availRage -= HS.Rage;
                } else if (PercFailRageUnder20 == 1f && (!HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, CL.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivates = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivates = 0f;
                    availRage -= CL.Rage;
                } else { CL.numActivates = HS.numActivates = 0f; } */

                // Execute for remainder of GCDs
                if (EX.ability.Validated /*&& PercFailRageUnder20 == 1f*/) {
                    acts = Math.Min(GCDsAvailableU20, GCDsAvailableU20 * percTimeInDPS);
                    if (EX.ability.GetRageUseOverDur(acts) > availRage) acts = Math.Max(0f, availRage) / EX.ability.RageCost;
                    EX.numActivatesU20 = acts;
                    availRage -= EX.RageU20;
                } else { EX.numActivatesU20 = 0f; }
                EXspace = EX.numActivatesU20 / NumGCDs * EX.ability.UseTime / LatentGCD;

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivatesU20 = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDur) * percTimeUnder20;
                    availRage -= SoO.RageU20; // Not sure if it should affect Rage
                }

                float TotalSpace = (CSspace + RDspace + THspace + BLSspace /*+ MSspace*/ + OPspace + TFBspace /*+ SLspace*/ + EXspace /*+ HSspace + CLspace*/);
                (EX.ability as Execute).FreeRage = repassAvailRage = availRage; // check for not enough rage to maintain rotation and set Execute's FreeRage to this value
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeeded = 0f, rageGenOther = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                if (aw.ability is Rend) {
                    DPS_TTL += aw.ability.GetDPS(aw.numActivatesU20 + TH.numActivatesU20);
                } else {
                    DPS_TTL += aw.DPSU20;
                }
                _HPS_TTL += aw.HPSU20;
                if (aw.RageU20 > 0) rageNeeded += aw.RageU20;
                else rageGenOther -= aw.RageU20;
            }

            DPS_TTL += (WhiteAtks.MhDPS + (CombatFactors.useOH ? WhiteAtks.OhDPS : 0f)) * percTimeInDPSAndUnder20;
            // InvalidateCache();
            return DPS_TTL;
        }

        public void MakeRotationandDoDPS(bool setCalcs, float percTimeUnder20) {
            if (Char.MainHand == null) { return; }
            _HPS_TTL = 0f;
            if (_needDisplayCalcs) GCDUsage += NumGCDs.ToString("000.00") + " : Total GCDs\n\n";
            
            float TotalPercTimeLost = CalculateTimeLost(GetWrapper<MortalStrike>().ability);
            
            if (_needDisplayCalcs) { GCDUsage += (TotalPercTimeLost != 0f ? "\n" : ""); }

            // ==== Rage Generation Priorities ========
            float availRage = 0f;
            float PercFailRageUnder20 = 1f, PercFailRageOver20 = 1f;
            availRage += RageGenOverDur_Other + RageGainedWhileMoving;

            // ==== Standard Priorities ===============
            _DPS_TTL = SettleAll(TotalPercTimeLost, percTimeUnder20, availRage, out PercFailRageOver20);
            if (percTimeUnder20 != 0f) { _DPS_TTL_U20 = SettleAll_U20(TotalPercTimeLost, percTimeUnder20, availRage, out PercFailRageUnder20); }

            calcDeepWounds();
            _DPS_TTL += DW.TickSize;
            //_DPS_TTL += GetWrapper<Skills.SpellDamageEffect>().DPS;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs) {
                if (PercFailRageOver20 != 1.0f || PercFailRageUnder20 != 1.0f) {
                    GCDUsage += (PercFailRageOver20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Starvation before Exec Spam.\n", (1f - PercFailRageOver20)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Starvation during Exec Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                List<AbilWrapper> dmgAbils = GetDamagingAbilities();
                foreach (AbilWrapper aw in dmgAbils) {
                    if (aw.allNumActivates > 0) {
                        if (aw.ability.GCDTime < 1.5f) { // Overpower (And TfB procs) use less than a GCD to recouperate.
                            GCDUsage += string.Format("{0:000.000}@{1:0.000}={2:000.000} : {3}\n",
                                aw.allNumActivates,
                                aw.ability.GCDTime,
                                (aw.allNumActivates * aw.ability.GCDTime / (CalcOpts.FullLatency + 1.5f)),
                                aw.ability.Name
                            );
                        } else {
                            GCDUsage += string.Format("{0:000.000} : {1}{2}\n",
                                aw.allNumActivates,
                                aw.ability.Name,
                                aw.ability.UsesGCD ? "" : " (Doesn't use GCDs)");
                        }
                    }
                }
                GCDUsage += "\n" + (GCDsAvailable + (percTimeUnder20 != 0f ? GCDsAvailableU20 : 0f)).ToString("000.00") + " : Avail GCDs";
            }

            // Return result
            if (setCalcs) {
                this.calcs.TotalDPS = _DPS_TTL + _DPS_TTL_U20;
                this.calcs.WhiteDPS = WhiteAtks.MhDPS + WhiteAtks.OhDPS;
                this.calcs.WhiteDPSMH = WhiteAtks.MhDPS;
                this.calcs.WhiteDmg = this.WhiteAtks.MhDamageOnUse;

                this.calcs.WhiteRage = WhiteAtks.MHRageGenOverDur;
                this.calcs.OtherRage = this.RageGenOverDur_Other;
                this.calcs.NeedyRage = this.RageNeededOverDur;
                this.calcs.FreeRage = calcs.WhiteRage + calcs.OtherRage - calcs.NeedyRage;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
                float PercTimeUnder20 = 0f;
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_]) {
                    PercTimeUnder20 = (float)BossOpts.Under20Perc;
                }
                MakeRotationandDoDPS(setCalcs, PercTimeUnder20);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Arms Rotation Details",
                    ex.Message, "MakeRotationandDoDPS()", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
        }
    }
}
