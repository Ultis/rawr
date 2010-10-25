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
            _cachedNumGCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD;
            _cachedNumGCDsU20 = CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD * (float)BossOpts.Under20Perc) : FightDuration / LatentGCD * (float)BossOpts.Under20Perc;
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
        protected override void calcDeepWounds() {
            base.calcDeepWounds();
        }

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

            float origNumGCDs = (CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD) * percTimeOver20,
                  origavailGCDs = preloopAvailGCDs * percTimeOver20,
                  origGCDsused = preloopGCDsUsed * percTimeOver20;
            float oldBLSGCDs = 0f, oldMSGCDs = 0f, oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f, oldEXGCDs = 0f, oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f;

            AbilWrapper SL = GetWrapper<Slam>();
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
                     (percTimeUnder20 > 0 && Math.Abs(EX.numActivates - oldEXGCDs) > 0.1f)))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                WhiteAtks.Slam_ActsOverDur = SL.numActivates;
                oldBLSGCDs = BLS.numActivates; oldMSGCDs = MS.numActivates; oldRDGCDs = RD.numActivates; oldOPGCDs = OP.numActivates; oldTBGCDs = TB.numActivates;
                oldEXGCDs = EX.numActivates; oldSLGCDs = SL.numActivates;
                oldCSGCDs = CS.numActivates; oldVRGCDs = VR.numActivates; oldHSGCDs = HS.numActivates; oldCLGCDs = CL.numActivates;
                BLS.numActivates = MS.numActivates = RD.numActivates = OP.numActivates = TB.numActivates = CS.numActivates = EX.numActivates =
                    SL.numActivates = TH.numActivates = VR.numActivates = HS.numActivates = CL.numActivates = 0;
                availRage = origAvailRage;
                availRage += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * percTimeOver20;

                float acts;
                float Abil_GCDs;
                float RDspace, BLSspace, MSspace, TFBspace, OPspace, CSspace, SLspace, HSspace, CLspace, THspace, VRspace;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRage < 0f || PercFailRage != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRage *= 1f + repassAvailRage / (availRage - repassAvailRage); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                } else { PercFailRage = 1f; }
                
                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, CS.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    CS.numActivates = Abil_GCDs;
                    availRage -= CS.Rage;
                }
                CSspace = CS.numActivates / NumGCDs * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, RD.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    RD.numActivates = Abil_GCDs;
                    availRage -= RD.Rage;
                }
                RDspace = RD.numActivates / NumGCDs * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, TH.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    TH.numActivates = Abil_GCDs;
                    (RD.ability as Rend).ThunderApps = TH.numActivates * Talents.BloodAndThunder * 0.50f;
                    availRage -= TH.Rage;
                }
                THspace = TH.numActivates / NumGCDs * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, BLS.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    BLS.numActivates = (1f - RDspace) * Abil_GCDs;
                    availRage -= BLS.Rage;
                }
                BLSspace = BLS.numActivates / NumGCDs * BLS.ability.UseTime / LatentGCD;

                // Mortal Strike
                if (MS.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, MS.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    MS.numActivates = (1f - BLSspace) * Abil_GCDs;
                    availRage -= MS.Rage;
                }
                MSspace = MS.numActivates / NumGCDs * MS.ability.UseTime / LatentGCD;

                // Taste for Blood
                float OPGCDReduc = (OP.ability.Cd < LatentGCD ? (OP.ability.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                if (TB.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, TB.ability.Activates * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    TB.numActivates = (1f - BLSspace) * Abil_GCDs;
                    availRage -= TB.Rage;
                }
                TFBspace = TB.numActivates / NumGCDs * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, (OP.ability as OverPower).GetActivates(DodgedYellowsOverDur, ParriedYellowsOverDur) * percTimeInDPSAndOver20 * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    OP.numActivates = Abil_GCDs * (1f - TFBspace - RDspace - BLSspace - MSspace);
                    availRage -= OP.Rage;
                }
                OPspace = OP.numActivates / NumGCDs * OP.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated) {
                    acts = Math.Min(GCDsAvailable, VR.ability.Activates * percTimeInDPSAndOver20); // Since VR is Free, we don't reduc for Rage Fails
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    VR.numActivates = (1f - BLSspace) * Abil_GCDs;
                    //availRage -= VR.Rage; // it's free
                }
                VRspace = VR.numActivates / NumGCDs * VR.ability.UseTime / LatentGCD;
               
                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                /*// 0 If is for no rage to Slam or HS/CL
                if (availRage <= 0) {
                    CL.numActivates = 0f;
                    HS.numActivates = 0f;
                    SL.numActivates = 0f;
                } else {*/
                    /*RageDumpMethod method = RageDumpMethod.NoRageDumping;
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    float AvailRagePerGCD = availRage / GCDsAvailable;
                    // We need to determine the best method for dumping rage
                    if (availRage <= 0f)
                    {
                        // There's no rage to dump, you suck dude!
                        method = RageDumpMethod.NoRageDumping;
                    }
                    else if (CL.ability.Validated
                         && (MultTargsPerc >= 1f)
                         && (AvailRagePerGCD >= CL.ability.RageCost))
                    {
                        // Wow! There's a constant Multiple Targets! And there's plenty of rage to go nuts on Cleaves!
                        method = RageDumpMethod.CleaveOnly;
                    }
                    else if (CL.ability.Validated && HS.ability.Validated
                         && (MultTargsPerc >= 1f)
                         && (AvailRagePerGCD <  CL.ability.RageCost)
                         && (AvailRagePerGCD >= HS.ability.RageCost))
                    {
                        // Wow! There's a constant Multiple Targets! But there's not enough rage to go nuts on Cleaves, so we're gonna throw a few Heroic Strikes in
                        method = RageDumpMethod.CleaveAndHeroicStrike;
                    }

                    // Individual, none affect the other
                    float CLAtTtl = CL.ability.RageCost * GCDsAvailable;
                    float HSAtTtl = HS.ability.RageCost * GCDsAvailable;
                    float SLAtTtl          = SL.ability.RageCost * GCDsAvailable;
                    // HS/CL together in Mix
                    float CLAtTtlMix       = CL.ability.RageCost * GCDsAvailable * (     MultTargsPerc);
                    float HSAtTtlMix       = HS.ability.RageCost * GCDsAvailable * (1f - MultTargsPerc);
                    // HS/CL and Slam all together
                    float CLAtTtlMixWithSL = CL.ability.RageCost * GCDsAvailable * (     MultTargsPerc);
                    float HSAtTtlMixWithSL = HS.ability.RageCost * GCDsAvailable * (1f - MultTargsPerc);
                    float SLAtTtlMixWithSL = SL.ability.RageCost * GCDsAvailable;*/

                    // First If contains total HS/CL conversion from Slams, because we have tons of rage and we are awesome
                    if (PercFailRage == 1f && (HS.ability.Validated || CL.ability.Validated)) {
                        acts = GCDsAvailable * percTimeInDPSAndOver20;
                        Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                        //float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                        //float clActs = (availRage * MultTargsPerc) / CL.ability.RageCost;
                        //float hsActs = (availRage * (1f - MultTargsPerc)) / HS.ability.RageCost;
                        CL.numActivates = Abil_GCDs * (BossOpts.MultiTargsTime / FightDuration);
                        HS.numActivates = Abil_GCDs - CL.numActivates;
                        SL.numActivates = 0f;
                        availRage -= HS.Rage + CL.Rage;

                    // Second If contains partial HS/CL conversion from Slams because there wasn't enough rage for total conversion
                    } else if (PercFailRage != 1f && (HS.ability.Validated || CL.ability.Validated) && SL.ability.Validated) {
                        acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndOver20);
                        Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                        CL.numActivates = (Abil_GCDs * (BossOpts.MultiTargsTime / FightDuration)) * (PercFailRage);
                        HS.numActivates = (Abil_GCDs - CL.numActivates) * (PercFailRage);
                        SL.numActivates = Abil_GCDs * (1f - PercFailRage);
                        availRage -= HS.Rage + CL.Rage + SL.Rage;
                    
                    // Third If contains no HS/CL conversion from Slams when there is enough rage to maintain this
                    } else if (PercFailRage == 1f && SL.ability.Validated){
                        acts = Math.Min(GCDsAvailable, GCDsAvailable/*SL.Activates*/ * percTimeInDPS);
                        Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                        CL.numActivates = 0f;
                        HS.numActivates = 0f;
                        SL.numActivates = Abil_GCDs;
                        availRage -= SL.Rage;

                    // Fourth If contains no HS/CL conversion from Slams but there is NOT enough rage to maintain this, so we dumb down Slams too
                    } else if (SL.ability.Validated){
                        acts = Math.Min(GCDsAvailable, GCDsAvailable/*SL.Activates*/ * percTimeInDPS);
                        Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                        if (SL.ability.GetRageUseOverDur(Abil_GCDs) > availRage) Abil_GCDs = Math.Max(0f, availRage) / SL.ability.RageCost;
                        CL.numActivates = 0f;
                        HS.numActivates = 0f;
                        SL.numActivates = Abil_GCDs;
                        availRage -= SL.Rage;

                    // Final If contains no HS/CL's and no Slams either because your toon sux
                    } else {
                        CL.numActivates = 0f;
                        HS.numActivates = 0f;
                        SL.numActivates = 0f;
                    }
                //}

                HSspace = HS.numActivates / NumGCDs * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivates / NumGCDs * CL.ability.UseTime / LatentGCD;
                SLspace = SL.numActivates / NumGCDs * SL.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivates);
                WhiteAtks.Slam_ActsOverDur = SL.numActivates;

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
                if (aw.ability.GetType() == typeof(Rend)) {
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

        enum RageDumpMethod
        {
            /// <summary>There was no extra rage to dump, so we don't Cleave, Heroic Strike or Slam</summary>
            NoRageDumping = 0,
            /// <summary>There isn't enough rage to warrant Heroic Striking</summary
            SlamOnly,
            HeroicStrikeAndSlam,
            CleaveAndHeroicStrikeAndSlam,
            HeroicStrikeOnly,
            CleaveOnly,
            CleaveAndHeroicStrike,
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

            float origNumGCDs = (CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD) * (/*1f -*/ percTimeUnder20),
                  origavailGCDs = preloopAvailGCDs * (/*1f -*/ percTimeUnder20),
                  origGCDsused = preloopGCDsUsed * (/*1f -*/ percTimeUnder20);
            float oldBLSGCDs = 0f, //oldMSGCDs = 0f,
                  oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f,
                  oldEXGCDs = 0f, //oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f;

            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
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
            // Run the loop for <20%
            //float MSBaseCd = 6f - Talents.ImprovedMortalStrike / 3f;
            /*float MS_WeightedValue = MS.ability.DamageOnUse + DW.TickSize * MS.ability.MHAtkTable.Crit,
                  SD_WeightedValue = SD.ability.DamageOnUse + DW.TickSize * SD.ability.MHAtkTable.Crit,
                  SL_WeightedValue = SL.ability.DamageOnUse + DW.TickSize * SL.ability.MHAtkTable.Crit;
            float OnePt5Plus1 = LatentGCD + (OP.ability.Cd + CalcOpts.AllowedReact),
                  Two1pt5 = LatentGCD * 2f,
                  Two1pt0 = (OP.ability.Cd + CalcOpts.AllowedReact) * 2f;*/
            //float TasteForBloodMOD = (Talents.TasteForBlood == 3 ? 1f / 6f : (Talents.TasteForBlood == 2 ? 0.144209288653733f : (Talents.TasteForBlood == 1 ? 0.104925207394343f : 0)));
            //float OtherMOD = (MSBaseCd + CalcOpts.Latency);
            //float SDMOD = 1f - 0.03f * Talents.SuddenDeath;
            //float avoid = (1f - CombatFactors._c_mhdodge - CombatFactors._c_ymiss);
            //float atleast1 = 0f, atleast2 = 0f, atleast3 = 0f, extLength1, extLength2, extLength3, averageTimeBetween, OnePt5Plus1_Occurs, Two1pt5_Occurs, Two1PtZero_Occurs;
            //float LeavingUntilNextMS_1, MSatExtra1, msNormally1, lengthFor1;
            //float LeavingUntilNextMS_2, MSatExtra2, msNormally2, lengthFor2;
            //float LeavingUntilNextMS_3, MSatExtra3, msNormally3, lengthFor3;
            //float timeInBetween = MSBaseCd - 1.5f;
            //float useExeifMSHasMoreThan, useSlamifMSHasMoreThan;
            //string canUse1, canUse2, canUse3;
            //float HPS;
            #region Abilities
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
                     //Math.Abs(SL.numActivates - oldSLGCDs) > 0.1f ||
                     (percTimeUnder20 > 0 && Math.Abs(EX.numActivates - oldEXGCDs) > 0.1f)))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                //WhiteAtks.Slam_Freq = SL.numActivates;
                oldBLSGCDs = BLS.numActivatesU20; /*oldMSGCDs = MS.numActivates;*/ oldRDGCDs = RD.numActivatesU20; oldOPGCDs = OP.numActivatesU20; oldTBGCDs = TB.numActivatesU20;
                oldEXGCDs = EX.numActivatesU20; /*oldSLGCDs = SL.numActivatesU20;*/
                oldCSGCDs = CS.numActivatesU20;
                BLS.numActivatesU20 = /*MS.numActivatesU20 =*/ RD.numActivatesU20 = OP.numActivatesU20 = TB.numActivatesU20 = CS.numActivatesU20 = EX.numActivatesU20 = SL.numActivatesU20 = HS.numActivatesU20 = CL.numActivatesU20 = 0;
                availRage = origAvailRage;
                availRage += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * (/*1f -*/ percTimeUnder20);

                float acts;
                float Abil_GCDs;
                float CSspace, RDspace, BLSspace, /*MSspace,*/ TFBspace, OPspace, EXspace/*, SLspace*/, HSspace, CLspace;
                // ==== Primary Ability Priorities ====
                // Colossus Smash
                if (CS.ability.Validated)
                {
                    acts = Math.Min(GCDsAvailableU20, CS.ability.Activates * percTimeInDPSAndUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    CS.numActivatesU20 = Abil_GCDs;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= CS.RageU20;
                }
                CSspace = CS.numActivatesU20 / NumGCDs * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated)
                {
                    acts = Math.Min(GCDsAvailableU20, RD.ability.Activates * percTimeInDPSAndUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    RD.numActivatesU20 = Abil_GCDs;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= RD.RageU20;

                }
                RDspace = RD.numActivatesU20 / NumGCDs * RD.ability.UseTime / LatentGCD;

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRage < 0f || PercFailRageUnder20 != 1f)
                {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageUnder20 *= 1f + repassAvailRage / (availRage - repassAvailRage); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                }
                else { PercFailRageUnder20 = 1f; }

                // Bladestorm
                if (BLS.ability.Validated)
                {
                    acts = Math.Min(GCDsAvailableU20, BLS.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    BLS.numActivatesU20 = (1f - RDspace) * Abil_GCDs;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= BLS.RageU20;
                }
                BLSspace = BLS.numActivatesU20 / NumGCDs * BLS.ability.UseTime / LatentGCD;

                // Taste for Blood
                float OPGCDReduc = (OP.ability.Cd < LatentGCD ? (OP.ability.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                if (TB.ability.Validated)
                {
                    acts = Math.Min(GCDsAvailableU20, TB.ability.Activates * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    TB.numActivatesU20 = Abil_GCDs;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= TB.RageU20;
                }
                TFBspace = TB.numActivatesU20 / NumGCDs * TB.ability.UseTime / LatentGCD;
                // Overpower
                if (OP.ability.Validated)
                {
                    OverPower _OP = OP.ability as OverPower;
                    acts = Math.Min(GCDsAvailableU20, _OP.GetActivates(DodgedYellowsOverDur, ParriedYellowsOverDur) * percTimeInDPSAndUnder20 * PercFailRageUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    OP.numActivatesU20 = Abil_GCDs * (1f - TFBspace - RDspace - BLSspace /*- MSspace*/);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= OP.RageU20;
                }
                OPspace = OP.numActivatesU20 / NumGCDs * OP.ability.UseTime / LatentGCD;

                // Heroic Strike/Cleave now that they are on GCDs. These should be rage dumps
                // Computing them together as you use HS for single, CL for Multiple
                if (HS.ability.Validated || CL.ability.Validated)
                {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    CL.numActivatesU20 = Abil_GCDs * (BossOpts.MultiTargsTime / FightDuration);
                    HS.numActivatesU20 = Abil_GCDs - CL.numActivatesU20;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= HS.RageU20 + CL.RageU20;
                }
                HSspace = HS.numActivatesU20 / NumGCDs * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivatesU20 / NumGCDs * CL.ability.UseTime / LatentGCD;

                // Execute for remainder of GCDs
                if (EX.ability.Validated /*&& PercFailRage == 1f*/)
                {
                    acts = Math.Min(GCDsAvailableU20, GCDsAvailableU20/*SL.Activates*/ * percTimeInDPS);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    if (EX.ability.GetRageUseOverDur(Abil_GCDs) > availRage) Abil_GCDs = Math.Max(0f, availRage) / EX.ability.RageCost;
                    EX.numActivatesU20 = Abil_GCDs;
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= EX.RageU20;
                }
                else { EX.numActivatesU20 = 0f; }
                EXspace = EX.numActivatesU20 / NumGCDs * EX.ability.UseTime / LatentGCD;

                float TotalSpace = (CSspace + RDspace + BLSspace /*+ MSspace*/ + OPspace + TFBspace /*+ SLspace*/ + EXspace + HSspace + CLspace);
                repassAvailRage = availRage; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion
            #endregion

            float DPS_TTL = 0f;
            float rageNeeded = 0f, rageGenOther = 0f;
            foreach (AbilWrapper aw in GetAbilityList())
            {
                DPS_TTL += aw.DPSU20;
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
            if (_needDisplayCalcs) GCDUsage += NumGCDs.ToString("000") + " : Total GCDs\n\n";
            
            float TotalPercTimeLost = CalculateTimeLost(GetWrapper<MortalStrike>().ability);
            
            if (_needDisplayCalcs) { GCDUsage += (TotalPercTimeLost != 0f ? "\n" : ""); }

            // ==== Rage Generation Priorities ========
            float availRage = 0f;
            float PercFailRageUnder20 = 1f, PercFailRage = 1f;
            availRage += RageGenOverDur_Other + RageGainedWhileMoving;

            // ==== Standard Priorities ===============
            _DPS_TTL = SettleAll(TotalPercTimeLost, percTimeUnder20, availRage, out PercFailRage);
            if (percTimeUnder20 != 0) { _DPS_TTL_U20 = SettleAll_U20(TotalPercTimeLost, percTimeUnder20, availRage, out PercFailRageUnder20); }

            calcDeepWounds();
            _DPS_TTL += DW.TickSize;
            //_DPS_TTL += GetWrapper<Skills.SpellDamageEffect>().DPS;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs)
            {
                if (PercFailRage != 1.0f || PercFailRageUnder20 != 1.0f)
                {
                    GCDUsage += (PercFailRage < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Starvation before Exec Spam.\n", (1f - PercFailRage)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Starvation during Exec Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                foreach (AbilWrapper aw in GetDamagingAbilities())
                {
                    if (aw.allNumActivates > 0)
                        GCDUsage += aw.allNumActivates.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + aw.ability.Name + (aw.ability.UsesGCD ? "\n" : "(Doesn't use GCDs)\n");
                }
                GCDUsage += "\n" + (GCDsAvailable + GCDsAvailableU20).ToString("000") + " : Avail GCDs";
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
            base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
            float PercTimeUnder20 = 0f;
            if(CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_])
            {
                PercTimeUnder20 = 0f; // We're working on the rotation code, once we settle  >20% we'll re-enable this
                //PercTimeUnder20 = (float)BossOpts.Under20Perc;
            }
            MakeRotationandDoDPS(setCalcs, PercTimeUnder20);
        }
    }
}
