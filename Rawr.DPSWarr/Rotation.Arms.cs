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
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            BossOpts = (bo == null ? new BossOptions() : bo);
            WhiteAtks = wa;

            _cachedLatentGCD = 1.5f + CalcOpts.Latency + CalcOpts.AllowedReact;
            AbilWrapper.LatentGCD = _cachedLatentGCD;
            _cachedNumGCDsO20 = FightDurationO20 / LatentGCD;
            _cachedNumGCDsU20 = FightDurationU20 / LatentGCD;
        }

        #region Charge
        /// <summary>
        /// This is a 2D array because both the Juggernaught talent and the Glyph of Rapid Charge affect this thing
        /// <para>No Jug,No Glyph | No Jug,Glyph</para>
        /// <para>   Jug,No Glyph |    Jug,Glyph</para>
        /// </summary>
        private static readonly SpecialEffect[][] _SE_ChargeUse = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10, ((15f + 0 * 5f) * (1f - (false ? 0.07f : 0f)))), new SpecialEffect(Trigger.Use, null, 10, ((15f + 0 * 5f) * (1f - (true ? 0.07f : 0f)))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10, ((15f + 1 * 5f) * (1f - (false ? 0.07f : 0f)))), new SpecialEffect(Trigger.Use, null, 10, ((15f + 1 * 5f) * (1f - (true ? 0.07f : 0f)))) },
        };
        #endregion

        public float SettleAll(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageO20, out float PercFailRageO20)
        {
            float percTimeO20 = (1f - percTimeU20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndO20 = percTimeInDPS * percTimeO20;
            availRageO20 -= rageUsedByMaintenance * percTimeO20;
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

            float preloopAvailGCDsO20 = GCDsAvailableO20, preloopGCDsUsedO20 = GCDsUsedO20, preloopAvailRageO20 = availRageO20;

            float origNumGCDsO20 = (FightDuration / LatentGCD) * percTimeO20,
                  origavailGCDsO20 = preloopAvailGCDsO20,
                  origGCDsusedO20 = preloopGCDsUsedO20;
            float oldCHActs = 0f, oldBLSGCDs = 0f, oldMSGCDs = 0f, oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f, /*oldEXGCDs = 0f,*/ oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f, oldSoOActs = 0f, oldIRActs = 0f, oldDCActs = 0f;

            AbilWrapper CH = GetWrapper<Charge>();
            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();
            AbilWrapper IR = GetWrapper<InnerRage>();
            AbilWrapper DC = GetWrapper<DeadlyCalm>();

            SL.numActivatesO20 = origavailGCDsO20;
            WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;
            float origAvailRageO20 = preloopAvailRageO20;
            bool hsok = CalcOpts.M_HeroicStrike;
            bool clok = BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                     && CalcOpts.M_Cleave;
            availRageO20 += WhiteAtks.whiteRageGenOverDurO20 * percTimeInDPS;
            availRageO20 -= SL.RageO20;
            float repassAvailRageO20 = 0f;
            PercFailRageO20 = 1f;

            // We want to start the fight with a charge, to give us some starting Rage
            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (false
                     || Math.Abs(CH.numActivatesO20 - oldCHActs) > 0.1f
                     || Math.Abs(IR.numActivatesO20 - oldIRActs) > 0.1f
                     || Math.Abs(DC.numActivatesO20 - oldDCActs) > 0.1f
                     || Math.Abs(CS.numActivatesO20 - oldCSGCDs) > 0.1f
                     || Math.Abs(RD.numActivatesO20 - oldRDGCDs) > 0.1f
                     || Math.Abs(TH.numActivatesO20 - oldTHGCDs) > 0.1f
                     || Math.Abs(BLS.numActivatesO20 - oldBLSGCDs) > 0.1f
                     || Math.Abs(MS.numActivatesO20 - oldMSGCDs) > 0.1f
                     || Math.Abs(TB.numActivatesO20 - oldTBGCDs) > 0.1f
                     || Math.Abs(OP.numActivatesO20 - oldOPGCDs) > 0.1f
                     || Math.Abs(VR.numActivatesO20 - oldVRGCDs) > 0.1f
                     || Math.Abs(HS.numActivatesO20 - oldHSGCDs) > 0.1f
                     || Math.Abs(CL.numActivatesO20 - oldCLGCDs) > 0.1f
                     || Math.Abs(SL.numActivatesO20 - oldSLGCDs) > 0.1f
                     || Math.Abs(SoO.numActivatesO20 - oldSoOActs) > 0.1f
                     ))
            {
                // Store the previous values for CS and OP proc'ing
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;
                oldCHActs = CH.numActivatesO20; oldIRActs = IR.numActivatesO20; oldDCActs = DC.numActivatesO20; oldCSGCDs = CS.numActivatesO20; oldRDGCDs = RD.numActivatesO20;
                oldTHGCDs = TH.numActivatesO20; oldBLSGCDs = BLS.numActivatesO20; oldMSGCDs = MS.numActivatesO20; oldTBGCDs = TB.numActivatesO20; oldOPGCDs = OP.numActivatesO20;
                oldVRGCDs = VR.numActivatesO20; oldHSGCDs = HS.numActivatesO20; oldCLGCDs = CL.numActivatesO20; oldSLGCDs = SL.numActivatesO20; oldSoOActs = SoO.numActivatesO20;
                // Set these all back to 0 so we can start fresh but factor the previous values where needed
                IR.numActivatesO20 = DC.numActivatesO20 = CS.numActivatesO20 = RD.numActivatesO20 = TH.numActivatesO20 = BLS.numActivatesO20 = MS.numActivatesO20 =
                TB.numActivatesO20 = OP.numActivatesO20 = VR.numActivatesO20 = HS.numActivatesO20 = CL.numActivatesO20 = SL.numActivatesO20 = SoO.numActivatesO20 = 0;
                // Reset the Rage
                availRageO20 = origAvailRageO20;
                // TODO: I'd like to cache whiteRageGenOverDur but it changes with slams. Research a better solution
                availRageO20 += WhiteAtks.whiteRageGenOverDurO20 * percTimeInDPS;

                float acts = 0, RDspace = 0, BLSspace = 0, MSspace = 0, TFBspace = 0, OPspace = 0, CSspace = 0,
                    SLspace = 0, HSspace = 0, CLspace = 0, THspace = 0, VRspace = 0, IRspace = 0, DCspace = 0;

                // GCDsAvailableO20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableO20 = GCDsAvailableO20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageO20 < 0f || PercFailRageO20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageO20 *= 1f + repassAvailRageO20 / (availRageO20 - repassAvailRageO20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (PercFailRageO20 > 1f) { PercFailRageO20 = 1f; }
                } else { PercFailRageO20 = 1f; }

                if (false && CalcOpts.M_StartWithCharge) {
                    // Lets make sure that we haven't already used up Charge activates for Movement
                    // There could be a move in the first 30 seconds that we want to save the opening cd for
                    // and this only affects O20
                    if (CH.numActivatesO20 < (CH.ability.Activates * percTimeO20)) {
                        // Add the activate for reporting
                        CH.numActivatesO20 = oldCHActs + 1f;
                        // Add the Rage generated (reverse the negative)
                        availRageO20 += CH.ability.RageCost * -1f;
                        // Since this is before the fight starts, we dont need to take a GCD out
                    }
                }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.ability.Validated && PercFailRageO20 == 1f && gcdsAvailableO20 > 0) {
                    acts = (IR.ability as InnerRage).GetActivates(repassAvailRageO20, percTimeO20) * percTimeInDPS;
                    IR.numActivatesO20 = acts;
                    //availRageO20 -= IR.RageO20 * RageMOD_Total;
                    //gcdsAvailableO20 -= IR.GCDUsageO20;
                }
                float IRUpTime = (IR.numActivatesO20 * IR.ability.Duration) / FightDurationO20;
                IRspace = IR.numActivatesO20 / NumGCDsO20 * IR.ability.UseTime / LatentGCD;

                // Deadly Calm, For 10 sec all abilities have no rage cost, should be used when low on rage
                // Can't be used when Inner Rage is up
                if (DC.ability.Validated && gcdsAvailableO20 > 0) {
                    acts = /*Math.Min(gcdsAvailableO20,*/ DC.ability.Activates * percTimeInDPSAndO20/*)*/;
                    DC.numActivatesO20 = acts * (1f - IRUpTime);
                    //availRageO20 -= DC.RageO20 * RageMOD_Total;
                    //gcdsAvailableO20 -= DC.GCDUsageO20;
                }
                DCspace = DC.numActivatesO20 / NumGCDsO20 * DC.ability.UseTime / LatentGCD;

                float RageMOD_DeadlyCalm = 1f - (CalcOpts.M_DeadlyCalm && Talents.DeadlyCalm > 0 ? /*10f / 120f **/ ((DC.numActivatesO20 * DC.ability.Duration) / FightDurationO20) : 0f);

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated && gcdsAvailableO20 > 0)
                {
                    float landsoverdurPluswhatwehaventprocessedyet = LandedAtksOverDurO20;
                    if (oldRDGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldRDGCDs * RD.ability.AvgTargets * RD.ability.SwingsPerActivate * RD.ability.MHAtkTable.AnyLand; }
                    if (oldTHGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTHGCDs * TH.ability.AvgTargets * TH.ability.SwingsPerActivate * TH.ability.MHAtkTable.AnyLand; }
                    if (oldBLSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldBLSGCDs * BLS.ability.AvgTargets * BLS.ability.SwingsPerActivate * BLS.ability.MHAtkTable.AnyLand; }
                    if (oldMSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldMSGCDs * MS.ability.AvgTargets * MS.ability.SwingsPerActivate * MS.ability.MHAtkTable.AnyLand; }
                    if (oldTBGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTBGCDs * TB.ability.AvgTargets * TB.ability.SwingsPerActivate * TB.ability.MHAtkTable.AnyLand; }
                    if (oldOPGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldOPGCDs * OP.ability.AvgTargets * OP.ability.SwingsPerActivate * OP.ability.MHAtkTable.AnyLand; }
                    if (oldVRGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldVRGCDs * VR.ability.AvgTargets * VR.ability.SwingsPerActivate * VR.ability.MHAtkTable.AnyLand; }
                    if (oldHSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldHSGCDs * HS.ability.AvgTargets * HS.ability.SwingsPerActivate * HS.ability.MHAtkTable.AnyLand; }
                    if (oldCLGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldCLGCDs * CL.ability.AvgTargets * CL.ability.SwingsPerActivate * CL.ability.MHAtkTable.AnyLand; }
                    if (oldSLGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSLGCDs * SL.ability.AvgTargets * SL.ability.SwingsPerActivate * SL.ability.MHAtkTable.AnyLand; }
                    if (oldSoOActs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.ability.AvgTargets * SoO.ability.SwingsPerActivate * SoO.ability.MHAtkTable.AnyLand; }
                    acts = Math.Min(gcdsAvailableO20, (CS.ability as ColossusSmash).GetActivates(landsoverdurPluswhatwehaventprocessedyet/*LandedAtksOverDurO20*/, percTimeO20) * percTimeInDPS * PercFailRageO20);
                    CS.numActivatesO20 = acts;
                    availRageO20 -= CS.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= CS.GCDUsageO20;
                }
                CSspace = CS.numActivatesO20 / NumGCDsO20 * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, RD.ability.Activates * percTimeInDPS * PercFailRageO20 * (Talents.BloodAndThunder < 2 ? percTimeO20 : 1f));
                    RD.numActivatesO20 = acts;
                    availRageO20 -= RD.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= RD.GCDUsageO20;
                }
                RDspace = RD.numActivatesO20 / NumGCDsO20 * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, TH.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    TH.numActivatesO20 = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderAppsO20 = TH.numActivatesO20 * Talents.BloodAndThunder * 0.50f;
                    availRageO20 -= TH.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= TH.GCDUsageO20;
                }
                THspace = TH.numActivatesO20 / NumGCDsO20 * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, BLS.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    BLS.numActivatesO20 = acts * (1f - RDspace);
                    availRageO20 -= BLS.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= BLS.GCDUsageO20;
                }
                BLSspace = BLS.numActivatesO20 / NumGCDsO20 * BLS.ability.UseTime / LatentGCD;

                // Mortal Strike
                if (MS.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, MS.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    MS.numActivatesO20 = acts * (1f - BLSspace - THspace - RDspace - CSspace);
                    availRageO20 -= MS.RageO20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= MS.GCDUsageO20;
                }
                MSspace = MS.numActivatesO20 / NumGCDsO20 * MS.ability.UseTime / LatentGCD;

                // Taste for Blood
                if (TB.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, (TB.ability.Activates) * percTimeInDPSAndO20 * PercFailRageO20);
                    TB.numActivatesO20 = acts * (1f - BLSspace);
                    availRageO20 -= TB.RageO20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= TB.GCDUsageO20;
                }
                TFBspace = TB.numActivatesO20 / NumGCDsO20 * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated && gcdsAvailableO20 > 0)
                {
                    float dodgesoverdurPluswhatwehaventprocessedyet = DodgedAttacksOverDur * percTimeO20;
                    if (oldVRGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldVRGCDs * VR.ability.AvgTargets * VR.ability.SwingsPerActivate * VR.ability.MHAtkTable.Dodge; }
                    if (oldHSGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldHSGCDs * HS.ability.AvgTargets * HS.ability.SwingsPerActivate * HS.ability.MHAtkTable.Dodge; }
                    if (oldCLGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldCLGCDs * CL.ability.AvgTargets * CL.ability.SwingsPerActivate * CL.ability.MHAtkTable.Dodge; }
                    if (oldSLGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSLGCDs * SL.ability.AvgTargets * SL.ability.SwingsPerActivate * SL.ability.MHAtkTable.Dodge; }
                    if (oldSoOActs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.ability.AvgTargets * SoO.ability.SwingsPerActivate * SoO.ability.MHAtkTable.Dodge; }
                    acts = Math.Min(gcdsAvailableO20, (OP.ability as OverPower).GetActivates(dodgesoverdurPluswhatwehaventprocessedyet/*DodgedAttacksOverDur*/, 0/*SoO.numActivatesO20*/) * percTimeInDPSAndO20 * PercFailRageO20);
                    OP.numActivatesO20 = acts * (1f - TFBspace - RDspace - BLSspace - MSspace);
                    availRageO20 -= OP.RageO20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= OP.GCDUsageO20;
                }
                OPspace = OP.numActivatesO20 / NumGCDsO20 * OP.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated && gcdsAvailableO20 > 0)
                {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.ability.Validated && VR.ability.DamageOnUse > SL.ability.DamageOnUse)
                        || (HS.ability.Validated && PercFailRageO20 == 1f && VR.ability.DamageOnUse > SL.ability.DamageOnUse))
                    {
                        acts = Math.Min(gcdsAvailableO20, VR.ability.Activates * percTimeInDPSAndO20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.numActivatesO20 = acts * (1f - BLSspace);
                        //availRage -= VR.Rage; // it's free
                        gcdsAvailableO20 -= VR.GCDUsageO20;
                    }
                }
                VRspace = VR.numActivatesO20 / NumGCDsO20 * VR.ability.UseTime / LatentGCD;
               
                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Slam
                if (SL.ability.Validated && PercFailRageO20 != 1 && gcdsAvailableO20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    if (SL.ability.GetRageUseOverDur(acts) > availRageO20) acts = Math.Max(0f, availRageO20) / SL.ability.RageCost;
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                }
                else if (SL.ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                } else { SL.numActivatesO20 = 0f; }
                SLspace = SL.numActivatesO20 / NumGCDsO20 * SL.ability.UseTime / LatentGCD;
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;

                // Cleaves before Heroic Strikes
                if (PercFailRageO20 >= 1f && CL.ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    acts                    =                 CL.ability.Activates * (MultTargsPerc) * percTimeInDPSAndO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    float clRageLimitedActs = (availRageO20 / CL.ability.RageCost) * (MultTargsPerc) * percTimeInDPSAndO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    CL.numActivatesO20 = Math.Min(clRageLimitedActs, acts);
                    availRageO20 -= CL.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= CL.GCDUsageO20;
                }
                CLspace = CL.numActivatesO20 / NumGCDsO20 * CL.ability.UseTime / LatentGCD;

                // Heroic Strikes, limited by rage and Cleaves
                if (PercFailRageO20 >= 1f && HS.ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/) {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    acts                    =                 HS.ability.Activates /** (1f - MultTargsPerc)*/ * percTimeInDPSAndO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm - CL.numActivatesO20;
                    float hsRageLimitedActs = (availRageO20 / HS.ability.RageCost) /** (1f - MultTargsPerc)*/ * percTimeInDPSAndO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm - CL.numActivatesO20;
                    HS.numActivatesO20 = Math.Min(hsRageLimitedActs, acts);
                    availRageO20 -= HS.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= HS.GCDUsageO20;
                }
                HSspace = HS.numActivatesO20 / NumGCDsO20 * HS.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivatesO20);


#if FALSE
                // Heroic Strikes/Cleaves
                if (PercFailRageO20 >= 1f && (HS.ability.Validated && CL.ability.Validated) && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ HS.ability.Activates * percTimeInDPSAndO20/*)*/;
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = (availRageO20 / CL.ability.RageCost) * (0f + MultTargsPerc) * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    float hsActs = (availRageO20 / HS.ability.RageCost) * (1f - MultTargsPerc) * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    CL.numActivatesO20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesO20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRageO20 -= (HS.RageO20 + CL.RageO20) * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= (HS.GCDUsageO20 + CL.GCDUsageO20);
                }
                else if (PercFailRageO20 >= 1f && (HS.ability.Validated && !CL.ability.Validated) && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ HS.ability.Activates * percTimeInDPSAndO20/*)*/;
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRageO20 / HS.ability.RageCost * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    CL.numActivatesO20 = 0f;
                    HS.numActivatesO20 = Math.Min(hsActs, acts);
                    availRageO20 -= HS.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= (HS.GCDUsageO20);
                }
                else if (PercFailRageO20 >= 1f && (!HS.ability.Validated && CL.ability.Validated) && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ CL.ability.Activates * percTimeInDPSAndO20/*)*/;
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageO20 / CL.ability.RageCost * MultTargsPerc * RageMOD_DeadlyCalm;
                    CL.numActivatesO20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesO20 = 0f;
                    availRageO20 -= CL.RageO20 * RageMOD_Total * RageMOD_BattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= (CL.GCDUsageO20);
                } else { CL.numActivatesO20 = HS.numActivatesO20 = 0f; }
#endif

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivatesO20 = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurO20, percTimeO20);
                }

                float TotalSpace = (RDspace + THspace + BLSspace + MSspace + OPspace + TFBspace + CSspace + SLspace + HSspace + CLspace + VRspace);
                (IR.ability as InnerRage).FreeRageO20 = repassAvailRageO20 = availRageO20; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            // Need to add the special effect from Juggernaut to Mortal Strike, not caring about Slam right now
            if (Talents.Juggernaut > 0 && GetWrapper<Charge>().numActivatesO20 > 0) {
                float uptime = _SE_ChargeUse[Talents.Juggernaut][Talents.GlyphOfRapidCharge?1:0].GetAverageUptime(FightDuration / GetWrapper<Charge>().numActivatesO20, 1f, CombatFactors._c_mhItemSpeed, FightDuration);
                // I'm not sure if this is gonna work, but hell, who knows
                (MS.ability as MortalStrike).JuggernautBonusCritChance = 0.25f * uptime;
                //MS = new Skills.MortalStrike(Char, stats, CombatFactors, WhiteAtks, CalcOpts);
            }

            float DPS_TTL = 0f;
            float rageNeededO20 = 0f, rageGenOtherO20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                if (aw.ability is Rend) {
                    DPS_TTL += (aw.ability as Rend).GetDPS(aw.numActivatesO20, TH.numActivatesO20, percTimeO20);
                } else {
                    DPS_TTL += aw.DPSO20;
                }
                _HPS_TTL += aw.HPSO20;
                if (aw.RageO20 > 0) { rageNeededO20 += aw.RageO20; }
                else { rageGenOtherO20 -= aw.RageO20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndO20;

            return DPS_TTL;
        }

        public float SettleAll_U20(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageU20, out float PercFailRageU20)
        {
            float percTimeO20 = (1f - percTimeU20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndU20 = percTimeU20 * percTimeInDPS;
            availRageU20 -= rageUsedByMaintenance * percTimeU20;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Execute sucks up all the cooldowns
             * Heroic Strike and Cleave won't be used at all
             * Execute FreeRage is 0, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDsU20 = GCDsAvailableU20, preloopGCDsUsedU20 = GCDsUsedU20, preloopAvailRageU20 = availRageU20;

            float origNumGCDsU20 = (FightDuration / LatentGCD) * percTimeU20,
                  origavailGCDsU20 = preloopAvailGCDsU20,
                  origGCDsusedU20 = preloopGCDsUsedU20;
            float oldBLSGCDs = 0f, //oldMSGCDs = 0f,
                  oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f,
                  oldEXGCDs = 0f, //oldSLGCDs = 0f,
                  oldCSGCDs = 0f, /*oldHSGCDs = 0f,*/ oldTHGCDs = 0f, /*oldVRGCDs = 0f,*/ oldSoOActs = 0f/*, oldCLGCDs = 0f*/, oldIRActs = 0, oldDCActs = 0;

            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();
            AbilWrapper IR = GetWrapper<InnerRage>();
            AbilWrapper DC = GetWrapper<DeadlyCalm>();

            Execute EX_ability = EX.ability as Execute;

            EX.numActivatesU20 = origavailGCDsU20 * percTimeInDPS;
            WhiteAtks.Slam_ActsOverDurU20 = 0f;
            float origAvailRageU20 = preloopAvailRageU20;
            availRageU20 += WhiteAtks.whiteRageGenOverDurU20 * percTimeInDPS;
            EX_ability.DumbActivates = EX.numActivatesU20;
            EX_ability.FreeRage = availRageU20;
            availRageU20 -= EX.RageU20;
            availRageU20 += EX.numActivatesU20 * 5 * Talents.SuddenDeath; // adds back [5|10] rage
            float repassAvailRageU20 = 0f;
            PercFailRageU20 = 1f;

            /*
             * There's two major lines of reasoning here
             * - If Execute does more damage, use it instead of suchNsuch ability
             * - If the ability does more damage, use it instead (so that we can gradually shift
             *   from one rotation to the other based on rising gear levels)
             * - However, most of these abilities are only coming up so that we can do Taste for Blood.
             *   If an Overpower isn't doing as much damage as an Execute... there's no point to Rend,
             *   Thunderclap (refreshing rend), or TfB so we might as well turn them all *off* at once
             */

            bool WeWantBLS = BLS.ability.Validated
                && EX.ability.Validated
                && (EX.ability.DamageOnUseOverride / (EX.ability.GCDTime / LatentGCD))
                    < (BLS.ability.DamageOnUseOverride / (BLS.ability.GCDTime / LatentGCD))
                && CalcOpts.M_ExecuteSpamStage2;

            bool WeWantTfB = TB.ability.Validated
                && EX.ability.Validated
                && (EX.ability.DamageOnUseOverride / (EX.ability.GCDTime / LatentGCD))
                    < (TB.ability.DamageOnUseOverride / (TB.ability.GCDTime / LatentGCD))
                && CalcOpts.M_ExecuteSpamStage2;

            int Iterator = 0;
            #region <20%
            while (Iterator < 50 && (false
                     || Math.Abs(IR.numActivatesU20 - oldIRActs) > 0.1f
                     || Math.Abs(DC.numActivatesU20 - oldDCActs) > 0.1f
                     || Math.Abs(CS.numActivatesU20 - oldCSGCDs) > 0.1f
                     || Math.Abs(RD.numActivatesU20 - oldRDGCDs) > 0.1f
                     || Math.Abs(TH.numActivatesU20 - oldTHGCDs) > 0.1f
                     || Math.Abs(BLS.numActivatesU20 - oldBLSGCDs) > 0.1f
                     || Math.Abs(TB.numActivatesU20 - oldTBGCDs) > 0.1f
                     || Math.Abs(OP.numActivatesU20 - oldOPGCDs) > 0.1f
                     || Math.Abs(EX.numActivatesU20 - oldEXGCDs) > 0.1f
                     || Math.Abs(SoO.numActivatesU20 - oldSoOActs) > 0.1f
                     ))
            {
                // Store the previous values for CS and OP proc'ing
                oldIRActs = IR.numActivatesU20; oldDCActs = DC.numActivatesU20; oldCSGCDs = CS.numActivatesU20; oldRDGCDs = RD.numActivatesU20;
                oldTHGCDs = TH.numActivatesU20; oldBLSGCDs = BLS.numActivatesU20; oldTBGCDs = TB.numActivatesU20; oldOPGCDs = OP.numActivatesU20;
                oldEXGCDs = EX.numActivatesU20; oldSoOActs = SoO.numActivatesU20;
                // Set these all back to 0 so we can start fresh but factor the previous values where needed
                IR.numActivatesU20 = DC.numActivatesU20 = CS.numActivatesU20 = RD.numActivatesU20 = TH.numActivatesU20 = BLS.numActivatesU20 =
                    TB.numActivatesU20 = OP.numActivatesU20 = EX.numActivatesU20 = SoO.numActivatesU20 = 0;
                // Reset the Rage
                availRageU20 = origAvailRageU20;
                // TODO: I'd like to cache whiteRageGenOverDur but it changes with slams. Research a better solution
                availRageU20 += WhiteAtks.whiteRageGenOverDurU20 * percTimeInDPS;

                float acts;
                float CSspace, RDspace, BLSspace, /*MSspace,*/ TFBspace, OPspace, EXspace/*, SLspace, HSspace, CLspace*/, THspace/*, VRspace*/, IRspace, DCspace;

                // GCDsAvailableU20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableU20 = GCDsAvailableU20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageU20 < 0f || PercFailRageU20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageU20 *= 1f + repassAvailRageU20 / (availRageU20 - repassAvailRageU20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (PercFailRageU20 > 1f) { PercFailRageU20 = 1f; }
                } else { PercFailRageU20 = 1f; }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.ability.Validated && PercFailRageU20 == 1f) {
                    acts = (IR.ability as InnerRage).GetActivates(repassAvailRageU20, percTimeU20) * percTimeInDPS;
                    IR.numActivatesU20 = acts;
                    //availRageU20 -= IR.RageU20 * RageMOD_Total;
                    //gcdsAvailableU20 -= IR.GCDUsageU20;
                }
                float IRUpTime = (IR.numActivatesU20 * IR.ability.Duration) / FightDurationU20;
                IRspace = IR.numActivatesU20 / NumGCDsU20 * IR.ability.UseTime / LatentGCD;

                // Deadly Calm, For 10 sec all abilities have no rage cost, should be used when low on rage
                // Can't be used when Inner Rage is up
                if (DC.ability.Validated) {
                    acts = /*Math.Min(gcdsAvailableU20,*/ DC.ability.Activates * percTimeInDPSAndU20/*)*/;
                    DC.numActivatesU20 = acts * (1f - IRUpTime);
                    //availRageU20 -= DC.RageU20 * RageMOD_Total;
                    //gcdsAvailableU20 -= DC.GCDUsageU20;
                }
                DCspace = DC.numActivatesU20 / NumGCDsU20 * DC.ability.UseTime / LatentGCD;

                float RageMOD_DeadlyCalm = 1f - (CalcOpts.M_DeadlyCalm && Talents.DeadlyCalm > 0 ? /*10f / 120f **/ ((DC.numActivatesU20 * DC.ability.Duration) / FightDurationU20) : 0f);

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    float landsoverdurPluswhatwehaventprocessedyet = LandedAtksOverDurU20;
                    if (oldRDGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldRDGCDs * RD.ability.AvgTargets * RD.ability.SwingsPerActivate * RD.ability.MHAtkTable.AnyLand; }
                    if (oldTHGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTHGCDs * TH.ability.AvgTargets * TH.ability.SwingsPerActivate * TH.ability.MHAtkTable.AnyLand; }
                    if (oldBLSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldBLSGCDs * BLS.ability.AvgTargets * BLS.ability.SwingsPerActivate * BLS.ability.MHAtkTable.AnyLand; }
                    if (oldTBGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTBGCDs * TB.ability.AvgTargets * TB.ability.SwingsPerActivate * TB.ability.MHAtkTable.AnyLand; }
                    if (oldOPGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldOPGCDs * OP.ability.AvgTargets * OP.ability.SwingsPerActivate * OP.ability.MHAtkTable.AnyLand; }
                    if (oldEXGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldEXGCDs * EX.ability.AvgTargets * EX.ability.SwingsPerActivate * EX.ability.MHAtkTable.AnyLand; }
                    if (oldSoOActs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.ability.AvgTargets * SoO.ability.SwingsPerActivate * SoO.ability.MHAtkTable.AnyLand; }
                    acts = Math.Min(gcdsAvailableU20, (CS.ability as ColossusSmash).GetActivates(landsoverdurPluswhatwehaventprocessedyet/*LandedAtksOverDurU20*/, percTimeU20) * percTimeInDPS * PercFailRageU20);
                    //acts = Math.Min(gcdsAvailableU20, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDurU20, percTimeU20) * percTimeInDPS * PercFailRageU20);
                    CS.numActivatesU20 = acts;
                    availRageU20 -= CS.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= CS.GCDUsageU20;
                }
                CSspace = CS.numActivatesU20 / NumGCDsU20 * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated && WeWantTfB && Talents.BloodAndThunder < 2 && gcdsAvailableU20 > 0)
                { // Ignore Rend when we have BnT at 100%
                    acts = Math.Min(gcdsAvailableU20, RD.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    RD.numActivatesU20 = acts;
                    availRageU20 -= RD.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= RD.GCDUsageU20;
                }
                RDspace = RD.numActivatesU20 / NumGCDsU20 * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated && WeWantTfB && gcdsAvailableU20 > 0)
                {
                    acts = Math.Min(gcdsAvailableU20, TH.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    TH.numActivatesU20 = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderAppsU20 = TH.numActivatesU20 * Talents.BloodAndThunder * 0.50f;
                    availRageU20 -= TH.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= TH.GCDUsageU20;
                }
                THspace = TH.numActivatesU20 / NumGCDsU20 * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (WeWantBLS && gcdsAvailableU20 > 0)
                {
                    // We only want to use Bladestorm during Exec phase IF it is going to do more damage, which requires Multiple Targets to be up
                    acts = Math.Min(gcdsAvailableU20, BLS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    BLS.numActivatesU20 = acts * (1f - RDspace);
                    availRageU20 -= BLS.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= BLS.GCDUsageU20;
                }
                BLSspace = BLS.numActivatesU20 / NumGCDsU20 * BLS.ability.UseTime / LatentGCD;

                /*// Mortal Strike // MS doesn't get used in Exec phase
                if (MS.ability.Validated) {
                    acts = Math.Min(gcdsAvailableU20, MS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    MS.numActivatesU20 = acts * (1f - BLSspace);
                    availRageU20 -= MS.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= MS.GCDUsageU20;
                }
                MSspace = MS.numActivatesU20 / NumGCDsU20 * MS.ability.UseTime / LatentGCD;*/

                // Taste for Blood
                if (WeWantTfB && gcdsAvailableU20 > 0)
                {
                    acts = Math.Min(gcdsAvailableU20, TB.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    TB.numActivatesU20 = acts * (1f - BLSspace);
                    availRageU20 -= TB.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= TB.GCDUsageU20;
                }
                TFBspace = TB.numActivatesU20 / NumGCDsU20 * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated && WeWantTfB && gcdsAvailableU20 > 0)
                { // same check, no need to make it twice
                    float dodgesoverdurPluswhatwehaventprocessedyet = DodgedAttacksOverDur * percTimeO20;
                    if (oldEXGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldEXGCDs * EX.ability.AvgTargets * EX.ability.SwingsPerActivate * EX.ability.MHAtkTable.Dodge; }
                    if (oldSoOActs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.ability.AvgTargets * SoO.ability.SwingsPerActivate * SoO.ability.MHAtkTable.Dodge; }
                    acts = Math.Min(gcdsAvailableU20, (OP.ability as OverPower).GetActivates(dodgesoverdurPluswhatwehaventprocessedyet/*DodgedAttacksOverDur*/, 0/*SoO.numActivatesU20*/) * percTimeInDPSAndU20 * PercFailRageU20);
                    //acts = Math.Min(gcdsAvailableU20, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndU20 * PercFailRageU20);
                    OP.numActivatesU20 = acts * (1f - TFBspace - RDspace - BLSspace /*- MSspace*/);
                    availRageU20 -= OP.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= OP.GCDUsageU20;
                }
                OPspace = OP.numActivatesU20 / NumGCDsU20 * OP.ability.UseTime / LatentGCD;

                // Skip Victory Rush, it's useless here

                // Heroic Strikes/Cleaves
                // I dont think we should HS/CL during this phase because Exec should do more damage for less/same Rage
                /*if (PercFailRageUnder20 == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(gcdsAvailableU20, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRage / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivatesU20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesU20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRage -= (HS.Rage + CL.Rage) * RageMOD_Total;
                    gcdsAvailableU20 -= (HS.GCDUsage20 + CL.GCDUsageU20);
                } else if (PercFailRageUnder20 == 1f && (HS.ability.Validated && !CL.ability.Validated)) {
                    acts = Math.Min(gcdsAvailableU20, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRage / HS.ability.RageCost;
                    CL.numActivatesU20 = 0f;
                    HS.numActivatesU20 = Math.Min(hsActs, acts);
                    availRage -= HS.Rage * RageMOD_Total;
                    gcdsAvailableU20 -= HS.GCDUsageU20;
                } else if (PercFailRageUnder20 == 1f && (!HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(gcdsAvailableU20, CL.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivatesU20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesU20 = 0f;
                    availRage -= CL.Rage * RageMOD_Total;
                    gcdsAvailableU20 -= CL.GCDUsageU20;
                } else { VR.numActivatesU20 = SL.numActivatesU20 = CL.numActivatesU20 = HS.numActivatesU20 = 0f; } */

                // Execute for remainder of GCDs
                if (EX.ability.Validated /*&& PercFailRageUnder20 == 1f*/ && gcdsAvailableU20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableU20,*/ gcdsAvailableU20 * percTimeInDPS/*)*/;
                    //if (EX.ability.GetRageUseOverDur(acts) > availRage) acts = Math.Max(0f, availRage) / EX.ability.RageCost;
                    EX.numActivatesU20 = (EX.ability as Execute).DumbActivates = acts;
                    availRageU20 -= EX.RageU20 * RageMOD_Total * RageMOD_DeadlyCalm;
                    availRageU20 += EX.numActivatesU20 * (Talents.SuddenDeath * 5f);
                    gcdsAvailableU20 -= EX.GCDUsageU20;
                } else { EX.numActivatesU20 = 0f; }
                EXspace = EX.numActivatesU20 / NumGCDsU20 * EX.ability.UseTime / LatentGCD;

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivatesU20 = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurU20, percTimeU20);
                    //availRage -= SoO.RageU20; // Not sure if it should affect Rage
                }

                float TotalSpace = (CSspace + RDspace + THspace + BLSspace /*+ MSspace*/ + OPspace + TFBspace /*+ SLspace*/ + EXspace /*+ HSspace + CLspace*/);
                (IR.ability as InnerRage).FreeRageU20 = (EX.ability as Execute).FreeRage = repassAvailRageU20 = availRageU20; // check for not enough rage to maintain rotation and set Execute's FreeRage to this value
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededU20 = 0f, rageGenOtherU20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                if (aw.ability is Rend) {
                    DPS_TTL += (aw.ability as Rend).GetDPS(aw.numActivatesU20, TH.numActivatesU20, percTimeU20);
                } else {
                    DPS_TTL += aw.DPSU20;
                }
                _HPS_TTL += aw.HPSU20;
                if (aw.RageU20 > 0) { rageNeededU20 += aw.RageU20; }
                else { rageGenOtherU20 -= aw.RageU20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndU20;
            
            return DPS_TTL;
        }

        public void MakeRotationandDoDPS(bool setCalcs, float percTimeUnder20) {
            if (Char.MainHand == null) { return; }
            _HPS_TTL = 0f;
            if (_needDisplayCalcs) { GCDUsage += string.Format("All=Over20%+Under20%. Only applicable if using Exec Spam\n{0:000.000}={1:000.000}+{2:000.000} : Total GCDs\n\n", NumGCDsAll, NumGCDsO20, NumGCDsU20); }

            // ==== Impedences ========================
            if (_needDisplayCalcs) { GCDUsage += "Impedences: Things that prevent you from DPS'g\n"; }
            float TotalPercTimeLost = CalculateTimeLost();
            if (_needDisplayCalcs && TotalPercTimeLost <= 0f) { GCDUsage += "None\n\n"; }
            else if (_needDisplayCalcs) { GCDUsage += (TotalPercTimeLost != 0f ? "\n" : ""); }

            // ==== Rage Generation Priorities ========
            float availRage = 0f;
            float PercFailRageUnder20 = 1f, PercFailRageOver20 = 1f;
            availRage += RageGenOverDur_Other + RageGainedWhileMoving;

            // ==== Maintenance and Anti-Impedence Priorities ====
            if (_needDisplayCalcs) GCDUsage += "Maintenance: Things that you do periodically to Buff yourself or the raid\n";
            float rageUsedByMaintenance = DoMaintenanceActivates(TotalPercTimeLost);

            // ==== Standard Priorities ===============
            if (_needDisplayCalcs) GCDUsage += "Abilities: Things that you do to damage the Target. These are not in order of priority.\n";
            _DPS_TTL_O20 = SettleAll(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageOver20);
            if (percTimeUnder20 != 0f) { _DPS_TTL_U20 = SettleAll_U20(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageUnder20); }
            if (_needDisplayCalcs) {
                // We need to add Inner Rage & Deadly Calm now that we know how many there are
                AbilWrapper aw = GetWrapper<DeadlyCalm>();
                GCDUsage = aw.allNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.Name, (!aw.ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
                AbilWrapper aw2 = GetWrapper<InnerRage>();
                GCDUsage = aw2.allNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw2.allNumActivates, aw2.numActivatesO20, aw2.numActivatesU20,
                                aw2.ability.Name, (!aw2.ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
            }

            calcDeepWounds();
            _DPS_TTL_O20 += DW.TickSize;
            _DPS_TTL_U20 += DW.TickSize;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs) {
                if (PercFailRageOver20 != 1.0f || PercFailRageUnder20 != 1.0f) {
                    GCDUsage += (PercFailRageOver20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation before Execute Spam.\n", (1f - PercFailRageOver20)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation during Execute Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                List<AbilWrapper> dmgAbils = GetDamagingAbilities();
                foreach (AbilWrapper aw in dmgAbils) {
                    if (aw.allNumActivates > 0 && !aw.ability.isMaint && !(aw.ability is HeroicLeap)) {
                        if (aw.ability.GCDTime < 1.5f || aw.ability.GCDTime > 3f) {
                            // Overpower (And TfB procs) use less than a GCD to recouperate.
                            // Bladestorm is channelled over 6 seconds (approx 4 GCDs)
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000} : {5}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.GCDTime,
                                (aw.allNumActivates * aw.ability.GCDTime / (CalcOpts.FullLatency + 1.5f)),
                                aw.ability.Name
                            );
                        } else {
                            GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.Name,
                                aw.ability.UsesGCD ? "" : " (Doesn't use GCDs)");
                        }
                    }
                }
                GCDUsage += string.Format("\n{0:000.000}={1:000.000}+{2:000.000} : Available GCDs (should be at or near zero)",
                    GCDsAvailableO20 + (percTimeUnder20 != 0f ? GCDsAvailableU20 : 0f),
                    GCDsAvailableO20,
                    (percTimeUnder20 != 0f ? GCDsAvailableU20 : 0f));
            }

            // Return result
            if (setCalcs) {
                this.calcs.WhiteDPSMH = WhiteAtks.GetMHDPS(WhiteAtks.MhActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSMHU20 = WhiteAtks.GetMHDPS(WhiteAtks.MhActivatesU20, TimeUndr20Perc);
                {
                    if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSMHU20 > 0) { this.calcs.WhiteDPS = (this.calcs.WhiteDPSMH + this.calcs.WhiteDPSMHU20) / 2f; }
                    else if (this.calcs.WhiteDPSMHU20 > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMHU20; }
                    else if (this.calcs.WhiteDPSMH > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH; }
                    else { this.calcs.WhiteDPS = 0f; }
                }
                this.calcs.WhiteDmg = this.WhiteAtks.MhDamageOnUse;

                {
                    if (_DPS_TTL_O20 > 0 && _DPS_TTL_U20 > 0) { this.calcs.TotalDPS = (_DPS_TTL_O20 + _DPS_TTL_U20) / 2f; }
                    else if (_DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_U20; }
                    else if (_DPS_TTL_O20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20; }
                    else { this.calcs.TotalDPS = 0f; }
                }

                this.calcs.WhiteRageO20 = WhiteAtks.MHRageGenOverDurO20;
                this.calcs.OtherRageO20 = this.RageGenOverDur_OtherO20;
                this.calcs.NeedyRageO20 = this.RageNeededOverDurO20;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;

                this.calcs.WhiteRageU20 = WhiteAtks.MHRageGenOverDurU20;
                this.calcs.OtherRageU20 = this.RageGenOverDur_OtherU20;
                this.calcs.NeedyRageU20 = this.RageNeededOverDurU20;
                this.calcs.FreeRageU20 = calcs.WhiteRageU20 + calcs.OtherRageU20 - calcs.NeedyRageU20;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalcs) {
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalcs);
                MakeRotationandDoDPS(setCalcs, TimeUndr20Perc);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Arms Rotation Details",
                    ex.Message, ex.InnerException,
                    "MakeRotationandDoDPS(...)", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
        }
    }
}
