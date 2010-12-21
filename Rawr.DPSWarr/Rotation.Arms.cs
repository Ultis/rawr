/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class ArmsRotation : Rotation {
        public ArmsRotation(DPSWarrCharacter dpswarrchar) {
            DPSWarrChar = dpswarrchar;

            _cachedLatentGCD = 1.5f + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact;
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

        public float SettleAll(float totalPercTimeLost, float rageUsedByMaintenance, float availRageO20, out float percFailRageO20)
        {
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndO20 = percTimeInDPS * TimeOver20Perc;
            availRageO20 -= rageUsedByMaintenance * TimeOver20Perc;
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

            float preloopAvailGCDsO20 = GCDsAvailableO20, /*preloopGCDsUsedO20 = GCDsUsedO20,*/ preloopAvailRageO20 = availRageO20;

            float //origNumGCDsO20 = (FightDuration / LatentGCD) * TimeOver20Perc,
                  origavailGCDsO20 = preloopAvailGCDsO20//,
                  //origGCDsusedO20 = preloopGCDsUsedO20
                  ;
            float oldCHActs = 0f, oldBLSGCDs = 0f, oldMSGCDs = 0f, oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f, /*oldEXGCDs = 0f,*/ oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f, oldSoOActs = 0f, oldIRActs = 0f, oldDCActs = 0f;

            AbilWrapper CH = GetWrapper<Charge>();
            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<Overpower>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();
            AbilWrapper IR = GetWrapper<InnerRage>();
            AbilWrapper DC = GetWrapper<DeadlyCalm>();

            SL.NumActivatesO20 = origavailGCDsO20;
            DPSWarrChar.Whiteattacks.SlamActsOverDurO20 = SL.NumActivatesO20;
            float origAvailRageO20 = preloopAvailRageO20;
            //bool hsok = CalcOpts.M_HeroicStrike;
            //bool clok = BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0 && CalcOpts.M_Cleave;
            availRageO20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurO20 * percTimeInDPS;
            availRageO20 -= SL.RageO20;
            float repassAvailRageO20 = 0f;
            percFailRageO20 = 1f;

            // We want to start the fight with a charge, to give us some starting Rage
            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (false
                     || Math.Abs(CH.NumActivatesO20 - oldCHActs) > 0.1f
                     || Math.Abs(IR.NumActivatesO20 - oldIRActs) > 0.1f
                     || Math.Abs(DC.NumActivatesO20 - oldDCActs) > 0.1f
                     || Math.Abs(CS.NumActivatesO20 - oldCSGCDs) > 0.1f
                     || Math.Abs(RD.NumActivatesO20 - oldRDGCDs) > 0.1f
                     || Math.Abs(TH.NumActivatesO20 - oldTHGCDs) > 0.1f
                     || Math.Abs(BLS.NumActivatesO20 - oldBLSGCDs) > 0.1f
                     || Math.Abs(MS.NumActivatesO20 - oldMSGCDs) > 0.1f
                     || Math.Abs(TB.NumActivatesO20 - oldTBGCDs) > 0.1f
                     || Math.Abs(OP.NumActivatesO20 - oldOPGCDs) > 0.1f
                     || Math.Abs(VR.NumActivatesO20 - oldVRGCDs) > 0.1f
                     || Math.Abs(HS.NumActivatesO20 - oldHSGCDs) > 0.1f
                     || Math.Abs(CL.NumActivatesO20 - oldCLGCDs) > 0.1f
                     || Math.Abs(SL.NumActivatesO20 - oldSLGCDs) > 0.1f
                     || Math.Abs(SoO.NumActivatesO20 - oldSoOActs) > 0.1f
                     ))
            {
                // Store the previous values for CS and OP proc'ing
                DPSWarrChar.Whiteattacks.SlamActsOverDurO20 = SL.NumActivatesO20;
                oldCHActs = CH.NumActivatesO20; oldIRActs = IR.NumActivatesO20; oldDCActs = DC.NumActivatesO20; oldCSGCDs = CS.NumActivatesO20; oldRDGCDs = RD.NumActivatesO20;
                oldTHGCDs = TH.NumActivatesO20; oldBLSGCDs = BLS.NumActivatesO20; oldMSGCDs = MS.NumActivatesO20; oldTBGCDs = TB.NumActivatesO20; oldOPGCDs = OP.NumActivatesO20;
                oldVRGCDs = VR.NumActivatesO20; oldHSGCDs = HS.NumActivatesO20; oldCLGCDs = CL.NumActivatesO20; oldSLGCDs = SL.NumActivatesO20; oldSoOActs = SoO.NumActivatesO20;
                // Set these all back to 0 so we can start fresh but factor the previous values where needed
                IR.NumActivatesO20 = DC.NumActivatesO20 = CS.NumActivatesO20 = RD.NumActivatesO20 = TH.NumActivatesO20 = BLS.NumActivatesO20 = MS.NumActivatesO20 =
                TB.NumActivatesO20 = OP.NumActivatesO20 = VR.NumActivatesO20 = HS.NumActivatesO20 = CL.NumActivatesO20 = SL.NumActivatesO20 = SoO.NumActivatesO20 = 0;
                // Reset the Rage
                availRageO20 = origAvailRageO20;
                // TODO: I'd like to cache whiteRageGenOverDur but it changes with slams. Research a better solution
                availRageO20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurO20 * percTimeInDPS;

                float acts = 0, RDspace = 0, BLSspace = 0, MSspace = 0, TFBspace = 0, /*OPspace = 0,*/ CSspace = 0,
                    /*SLspace = 0, HSspace = 0, CLspace = 0,*/ THspace = 0/*, VRspace = 0, IRspace = 0, DCspace = 0*/;

                // GCDsAvailableO20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableO20 = GCDsAvailableO20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageO20 < 0f || percFailRageO20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    percFailRageO20 *= 1f + repassAvailRageO20 / (availRageO20 - repassAvailRageO20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (percFailRageO20 > 1f) { percFailRageO20 = 1f; }
                } else { percFailRageO20 = 1f; }

                /*if (false && CalcOpts.M_StartWithCharge) {
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
                }*/

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.Ability.Validated && percFailRageO20 == 1f && gcdsAvailableO20 > 0) {
                    acts = (IR.Ability as InnerRage).GetActivates(repassAvailRageO20, TimeOver20Perc) * percTimeInDPS;
                    IR.NumActivatesO20 = acts;
                    //availRageO20 -= IR.RageO20 * RageMOD_Total;
                    //gcdsAvailableO20 -= IR.GCDUsageO20;
                }
                float IRUpTime = (IR.NumActivatesO20 * IR.Ability.Duration) / FightDurationO20;
                //IRspace = IR.numActivatesO20 / NumGCDsO20 * IR.ability.UseTime / LatentGCD;

                // Deadly Calm, For 10 sec all abilities have no rage cost, should be used when low on rage
                // Can't be used when Inner Rage is up
                if (DC.Ability.Validated && gcdsAvailableO20 > 0) {
                    acts = /*Math.Min(gcdsAvailableO20,*/ DC.Ability.Activates * percTimeInDPSAndO20/*)*/;
                    DC.NumActivatesO20 = acts * (1f - IRUpTime);
                    //availRageO20 -= DC.RageO20 * RageMOD_Total;
                    //gcdsAvailableO20 -= DC.GCDUsageO20;
                }
                //DCspace = DC.numActivatesO20 / NumGCDsO20 * DC.ability.UseTime / LatentGCD;

                float RageMOD_DeadlyCalm = 1f - (DPSWarrChar.CalcOpts.M_DeadlyCalm && DPSWarrChar.Talents.DeadlyCalm > 0 ? /*10f / 120f **/ ((DC.NumActivatesO20 * DC.Ability.Duration) / FightDurationO20) : 0f);

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    float landsoverdurPluswhatwehaventprocessedyet = LandedAtksOverDurO20;
                    if (oldRDGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldRDGCDs * RD.Ability.AvgTargets * RD.Ability.SwingsPerActivate * RD.Ability.MHAtkTable.AnyLand; }
                    if (oldTHGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTHGCDs * TH.Ability.AvgTargets * TH.Ability.SwingsPerActivate * TH.Ability.MHAtkTable.AnyLand; }
                    if (oldBLSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldBLSGCDs * BLS.Ability.AvgTargets * BLS.Ability.SwingsPerActivate * BLS.Ability.MHAtkTable.AnyLand; }
                    if (oldMSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldMSGCDs * MS.Ability.AvgTargets * MS.Ability.SwingsPerActivate * MS.Ability.MHAtkTable.AnyLand; }
                    if (oldTBGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTBGCDs * TB.Ability.AvgTargets * TB.Ability.SwingsPerActivate * TB.Ability.MHAtkTable.AnyLand; }
                    if (oldOPGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldOPGCDs * OP.Ability.AvgTargets * OP.Ability.SwingsPerActivate * OP.Ability.MHAtkTable.AnyLand; }
                    if (oldVRGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldVRGCDs * VR.Ability.AvgTargets * VR.Ability.SwingsPerActivate * VR.Ability.MHAtkTable.AnyLand; }
                    if (oldHSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldHSGCDs * HS.Ability.AvgTargets * HS.Ability.SwingsPerActivate * HS.Ability.MHAtkTable.AnyLand; }
                    if (oldCLGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldCLGCDs * CL.Ability.AvgTargets * CL.Ability.SwingsPerActivate * CL.Ability.MHAtkTable.AnyLand; }
                    if (oldSLGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSLGCDs * SL.Ability.AvgTargets * SL.Ability.SwingsPerActivate * SL.Ability.MHAtkTable.AnyLand; }
                    if (oldSoOActs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.Ability.AvgTargets * SoO.Ability.SwingsPerActivate * SoO.Ability.MHAtkTable.AnyLand; }
                    acts = Math.Min(gcdsAvailableO20, (CS.Ability as ColossusSmash).GetActivates(landsoverdurPluswhatwehaventprocessedyet/*LandedAtksOverDurO20*/, TimeOver20Perc) * percTimeInDPS * percFailRageO20);
                    CS.NumActivatesO20 = acts;
                    availRageO20 -= CS.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= CS.GCDUsageO20;
                }
                CSspace = CS.NumActivatesO20 / NumGCDsO20 * CS.Ability.UseTime / LatentGCD;

                // Rend
                if (RD.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, RD.Ability.Activates * percTimeInDPS * percFailRageO20 * (DPSWarrChar.Talents.BloodAndThunder < 2 ? TimeOver20Perc : 1f));
                    RD.NumActivatesO20 = acts;
                    availRageO20 -= RD.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= RD.GCDUsageO20;
                }
                RDspace = RD.NumActivatesO20 / NumGCDsO20 * RD.Ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, TH.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    TH.NumActivatesO20 = acts * (1f - RDspace);
                    (RD.Ability as Rend).ThunderAppsO20 = TH.NumActivatesO20 * DPSWarrChar.Talents.BloodAndThunder * 0.50f;
                    availRageO20 -= TH.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= TH.GCDUsageO20;
                }
                THspace = TH.NumActivatesO20 / NumGCDsO20 * TH.Ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, BLS.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    BLS.NumActivatesO20 = acts * (1f - RDspace);
                    availRageO20 -= BLS.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= BLS.GCDUsageO20;
                }
                BLSspace = BLS.NumActivatesO20 / NumGCDsO20 * BLS.Ability.UseTime / LatentGCD;

                // Mortal Strike
                if (MS.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, MS.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    MS.NumActivatesO20 = acts * (1f - BLSspace - THspace - RDspace - CSspace);
                    availRageO20 -= MS.RageO20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= MS.GCDUsageO20;
                }
                MSspace = MS.NumActivatesO20 / NumGCDsO20 * MS.Ability.UseTime / LatentGCD;

                // Taste for Blood
                if (TB.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = Math.Min(gcdsAvailableO20, (TB.Ability.Activates) * percTimeInDPSAndO20 * percFailRageO20);
                    TB.NumActivatesO20 = acts * (1f - BLSspace);
                    availRageO20 -= TB.RageO20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= TB.GCDUsageO20;
                }
                TFBspace = TB.NumActivatesO20 / NumGCDsO20 * TB.Ability.UseTime / LatentGCD;

                // Overpower
                if (OP.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    float dodgesoverdurPluswhatwehaventprocessedyet = DodgedAttacksOverDur * TimeOver20Perc;
                    if (oldVRGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldVRGCDs * VR.Ability.AvgTargets * VR.Ability.SwingsPerActivate * VR.Ability.MHAtkTable.Dodge; }
                    if (oldHSGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldHSGCDs * HS.Ability.AvgTargets * HS.Ability.SwingsPerActivate * HS.Ability.MHAtkTable.Dodge; }
                    if (oldCLGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldCLGCDs * CL.Ability.AvgTargets * CL.Ability.SwingsPerActivate * CL.Ability.MHAtkTable.Dodge; }
                    if (oldSLGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSLGCDs * SL.Ability.AvgTargets * SL.Ability.SwingsPerActivate * SL.Ability.MHAtkTable.Dodge; }
                    if (oldSoOActs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.Ability.AvgTargets * SoO.Ability.SwingsPerActivate * SoO.Ability.MHAtkTable.Dodge; }
                    acts = Math.Min(gcdsAvailableO20, (OP.Ability as Overpower).GetActivates(dodgesoverdurPluswhatwehaventprocessedyet/*DodgedAttacksOverDur*/, 0/*SoO.numActivatesO20*/) * percTimeInDPSAndO20 * percFailRageO20);
                    OP.NumActivatesO20 = acts * (1f - TFBspace - RDspace - BLSspace - MSspace);
                    availRageO20 -= OP.RageO20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= OP.GCDUsageO20;
                }
                //OPspace = OP.numActivatesO20 / NumGCDsO20 * OP.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.Ability.Validated && VR.Ability.DamageOnUse > SL.Ability.DamageOnUse)
                        || (HS.Ability.Validated && percFailRageO20 == 1f && VR.Ability.DamageOnUse > SL.Ability.DamageOnUse))
                    {
                        acts = Math.Min(gcdsAvailableO20, VR.Ability.Activates * percTimeInDPSAndO20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.NumActivatesO20 = acts * (1f - BLSspace);
                        //availRage -= VR.Rage; // it's free
                        gcdsAvailableO20 -= VR.GCDUsageO20;
                    }
                }
                //VRspace = VR.numActivatesO20 / NumGCDsO20 * VR.ability.UseTime / LatentGCD;
               
                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Slam
                if (SL.Ability.Validated && percFailRageO20 != 1 && gcdsAvailableO20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    if (SL.Ability.GetRageUseOverDur(acts) > availRageO20) acts = Math.Max(0f, availRageO20) / SL.Ability.RageCost;
                    SL.NumActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                }
                else if (SL.Ability.Validated && gcdsAvailableO20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    SL.NumActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                } else { SL.NumActivatesO20 = 0f; }
                //SLspace = SL.numActivatesO20 / NumGCDsO20 * SL.ability.UseTime / LatentGCD;
                DPSWarrChar.Whiteattacks.SlamActsOverDurO20 = SL.NumActivatesO20;

                // Cleaves before Heroic Strikes
                if (percFailRageO20 >= 1f && CL.Ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float MultTargsPerc = DPSWarrChar.BossOpts.MultiTargsTime / FightDuration;
                    acts                    =                 CL.Ability.Activates * (MultTargsPerc) * percTimeInDPSAndO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    float clRageLimitedActs = (availRageO20 / CL.Ability.RageCost) * (MultTargsPerc) * percTimeInDPSAndO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    CL.NumActivatesO20 = Math.Min(clRageLimitedActs, acts);
                    availRageO20 -= CL.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= CL.GCDUsageO20;
                }
                //CLspace = CL.numActivatesO20 / NumGCDsO20 * CL.ability.UseTime / LatentGCD;

                // Heroic Strikes, limited by rage and Cleaves
                if (percFailRageO20 >= 1f && HS.Ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/) {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float MultTargsPerc = DPSWarrChar.BossOpts.MultiTargsTime / FightDuration;
                    acts                    =                 HS.Ability.Activates /** (1f - MultTargsPerc)*/ * percTimeInDPSAndO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm - CL.NumActivatesO20;
                    float hsRageLimitedActs = (availRageO20 / HS.Ability.RageCost) /** (1f - MultTargsPerc)*/ * percTimeInDPSAndO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm - CL.NumActivatesO20;
                    HS.NumActivatesO20 = Math.Min(hsRageLimitedActs, acts);
                    availRageO20 -= HS.RageO20 * RageModTotal * RageModBattleTrance * RageMOD_DeadlyCalm;
                    //gcdsAvailableO20 -= HS.GCDUsageO20;
                }
                //HSspace = HS.numActivatesO20 / NumGCDsO20 * HS.ability.UseTime / LatentGCD;
                (HS.Ability as HeroicStrike).InciteBonusCrits(HS.NumActivatesO20);


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
                if (SoO.Ability.Validated) {
                    SoO.NumActivatesO20 = (SoO.Ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurO20, TimeOver20Perc);
                }

                //float TotalSpace = (RDspace + THspace + BLSspace + MSspace + OPspace + TFBspace + CSspace + SLspace + HSspace + CLspace + VRspace);
                (IR.Ability as InnerRage).FreeRageO20 = repassAvailRageO20 = availRageO20; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            // Need to add the special effect from Juggernaut to Mortal Strike, not caring about Slam right now
            if (DPSWarrChar.Talents.Juggernaut > 0 && GetWrapper<Charge>().NumActivatesO20 > 0)
            {
                float uptime = _SE_ChargeUse[DPSWarrChar.Talents.Juggernaut][DPSWarrChar.Talents.GlyphOfRapidCharge ? 1 : 0].GetAverageUptime(FightDuration / GetWrapper<Charge>().NumActivatesO20, 1f, DPSWarrChar.CombatFactors.CMHItemSpeed, FightDuration);
                // I'm not sure if this is gonna work, but hell, who knows
                (MS.Ability as MortalStrike).JuggernautBonusCritChance = 0.25f * uptime;
                //MS = new Skills.MortalStrike(Char, stats, CombatFactors, Whiteattacks, CalcOpts);
            }

            float DPS_TTL = 0f;
            float rageNeededO20 = 0f, rageGenOtherO20 = 0f;
            foreach (AbilWrapper aw in TheAbilityList) {
                if (aw.Ability is Rend) {
                    DPS_TTL += (aw.Ability as Rend).GetDPS(aw.NumActivatesO20, TH.NumActivatesO20, TimeOver20Perc);
                } else {
                    DPS_TTL += aw.DPS_O20;
                }
                _HPS_TTL += aw.HPS_O20;
                if (aw.RageO20 > 0) { rageNeededO20 += aw.RageO20; }
                else { rageGenOtherO20 -= aw.RageO20; }
            }

            DPS_TTL += DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesO20, TimeOver20Perc) * percTimeInDPS;

            return DPS_TTL;
        }

        public float SettleAllU20(float totalPercTimeLost, float rageUsedByMaintenance, float availRageU20, out float percFailRageU20)
        {
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndU20 = TimeUndr20Perc * percTimeInDPS;
            availRageU20 -= rageUsedByMaintenance * TimeUndr20Perc;
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

            float preloopAvailGCDsU20 = GCDsAvailableU20, /*preloopGCDsUsedU20 = GCDsUsedU20,*/ preloopAvailRageU20 = availRageU20;

            float //origNumGCDsU20 = (FightDuration / LatentGCD) * TimeUndr20Perc,
                  origavailGCDsU20 = preloopAvailGCDsU20//,
                  //origGCDsusedU20 = preloopGCDsUsedU20
                  ;
            float oldBLSGCDs = 0f, //oldMSGCDs = 0f,
                  oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f,
                  oldEXGCDs = 0f, //oldSLGCDs = 0f,
                  oldCSGCDs = 0f, /*oldHSGCDs = 0f,*/ oldTHGCDs = 0f, /*oldVRGCDs = 0f,*/ oldSoOActs = 0f/*, oldCLGCDs = 0f*/, oldIRActs = 0, oldDCActs = 0;

            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper OP = GetWrapper<Overpower>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();
            AbilWrapper IR = GetWrapper<InnerRage>();
            AbilWrapper DC = GetWrapper<DeadlyCalm>();

            Execute EX_ability = EX.Ability as Execute;

            EX.NumActivatesU20 = origavailGCDsU20 * percTimeInDPS;
            DPSWarrChar.Whiteattacks.SlamActsOverDurU20 = 0f;
            float origAvailRageU20 = preloopAvailRageU20;
            availRageU20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurU20 * percTimeInDPS;
            EX_ability.DumbActivates = EX.NumActivatesU20;
            EX_ability.FreeRage = availRageU20;
            availRageU20 -= EX.RageU20;
            availRageU20 += EX.NumActivatesU20 * 5 * DPSWarrChar.Talents.SuddenDeath; // adds back [5|10] rage
            float repassAvailRageU20 = 0f;
            percFailRageU20 = 1f;

            /*
             * There's two major lines of reasoning here
             * - If Execute does more damage, use it instead of suchNsuch ability
             * - If the ability does more damage, use it instead (so that we can gradually shift
             *   from one rotation to the other based on rising gear levels)
             * - However, most of these abilities are only coming up so that we can do Taste for Blood.
             *   If an Overpower isn't doing as much damage as an Execute... there's no point to Rend,
             *   Thunderclap (refreshing rend), or TfB so we might as well turn them all *off* at once
             */

            bool WeWantBLS = BLS.Ability.Validated
                && EX.Ability.Validated
                && (EX.Ability.DamageOnUseOverride / (EX.Ability.GCDTime / LatentGCD))
                    < (BLS.Ability.DamageOnUseOverride / (BLS.Ability.GCDTime / LatentGCD))
                && DPSWarrChar.CalcOpts.M_ExecuteSpamStage2;

            bool WeWantTfB = TB.Ability.Validated
                && EX.Ability.Validated
                && (EX.Ability.DamageOnUseOverride / (EX.Ability.GCDTime / LatentGCD))
                    < (TB.Ability.DamageOnUseOverride / (TB.Ability.GCDTime / LatentGCD))
                && DPSWarrChar.CalcOpts.M_ExecuteSpamStage2;

            int Iterator = 0;
            #region <20%
            while (Iterator < 50 && (false
                     || Math.Abs(IR.NumActivatesU20 - oldIRActs) > 0.1f
                     || Math.Abs(DC.NumActivatesU20 - oldDCActs) > 0.1f
                     || Math.Abs(CS.NumActivatesU20 - oldCSGCDs) > 0.1f
                     || Math.Abs(RD.NumActivatesU20 - oldRDGCDs) > 0.1f
                     || Math.Abs(TH.NumActivatesU20 - oldTHGCDs) > 0.1f
                     || Math.Abs(BLS.NumActivatesU20 - oldBLSGCDs) > 0.1f
                     || Math.Abs(TB.NumActivatesU20 - oldTBGCDs) > 0.1f
                     || Math.Abs(OP.NumActivatesU20 - oldOPGCDs) > 0.1f
                     || Math.Abs(EX.NumActivatesU20 - oldEXGCDs) > 0.1f
                     || Math.Abs(SoO.NumActivatesU20 - oldSoOActs) > 0.1f
                     ))
            {
                // Store the previous values for CS and OP proc'ing
                oldIRActs = IR.NumActivatesU20; oldDCActs = DC.NumActivatesU20; oldCSGCDs = CS.NumActivatesU20; oldRDGCDs = RD.NumActivatesU20;
                oldTHGCDs = TH.NumActivatesU20; oldBLSGCDs = BLS.NumActivatesU20; oldTBGCDs = TB.NumActivatesU20; oldOPGCDs = OP.NumActivatesU20;
                oldEXGCDs = EX.NumActivatesU20; oldSoOActs = SoO.NumActivatesU20;
                // Set these all back to 0 so we can start fresh but factor the previous values where needed
                IR.NumActivatesU20 = DC.NumActivatesU20 = CS.NumActivatesU20 = RD.NumActivatesU20 = TH.NumActivatesU20 = BLS.NumActivatesU20 =
                    TB.NumActivatesU20 = OP.NumActivatesU20 = EX.NumActivatesU20 = SoO.NumActivatesU20 = 0;
                // Reset the Rage
                availRageU20 = origAvailRageU20;
                // TODO: I'd like to cache whiteRageGenOverDur but it changes with slams. Research a better solution
                availRageU20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurU20 * percTimeInDPS;

                float acts;
                float CSspace, RDspace, BLSspace, /*MSspace,*/ TFBspace/*, OPspace, EXspace, SLspace, HSspace, CLspace, THspace, VRspace, IRspace, DCspace*/;

                // GCDsAvailableU20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableU20 = GCDsAvailableU20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageU20 < 0f || percFailRageU20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    percFailRageU20 *= 1f + repassAvailRageU20 / (availRageU20 - repassAvailRageU20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (percFailRageU20 > 1f) { percFailRageU20 = 1f; }
                } else { percFailRageU20 = 1f; }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.Ability.Validated && percFailRageU20 == 1f) {
                    acts = (IR.Ability as InnerRage).GetActivates(repassAvailRageU20, TimeUndr20Perc) * percTimeInDPS;
                    IR.NumActivatesU20 = acts;
                    //availRageU20 -= IR.RageU20 * RageMOD_Total;
                    //gcdsAvailableU20 -= IR.GCDUsageU20;
                }
                float IRUpTime = (IR.NumActivatesU20 * IR.Ability.Duration) / FightDurationU20;
                //IRspace = IR.numActivatesU20 / NumGCDsU20 * IR.ability.UseTime / LatentGCD;

                // Deadly Calm, For 10 sec all abilities have no rage cost, should be used when low on rage
                // Can't be used when Inner Rage is up
                if (DC.Ability.Validated) {
                    acts = /*Math.Min(gcdsAvailableU20,*/ DC.Ability.Activates * percTimeInDPSAndU20/*)*/;
                    DC.NumActivatesU20 = acts * (1f - IRUpTime);
                    //availRageU20 -= DC.RageU20 * RageMOD_Total;
                    //gcdsAvailableU20 -= DC.GCDUsageU20;
                }
                //DCspace = DC.numActivatesU20 / NumGCDsU20 * DC.ability.UseTime / LatentGCD;

                float RageMOD_DeadlyCalm = 1f - (DPSWarrChar.CalcOpts.M_DeadlyCalm && DPSWarrChar.Talents.DeadlyCalm > 0 ? /*10f / 120f **/ ((DC.NumActivatesU20 * DC.Ability.Duration) / FightDurationU20) : 0f);

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.Ability.Validated) {
                    float landsoverdurPluswhatwehaventprocessedyet = LandedAtksOverDurU20;
                    if (oldRDGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldRDGCDs * RD.Ability.AvgTargets * RD.Ability.SwingsPerActivate * RD.Ability.MHAtkTable.AnyLand; }
                    if (oldTHGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTHGCDs * TH.Ability.AvgTargets * TH.Ability.SwingsPerActivate * TH.Ability.MHAtkTable.AnyLand; }
                    if (oldBLSGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldBLSGCDs * BLS.Ability.AvgTargets * BLS.Ability.SwingsPerActivate * BLS.Ability.MHAtkTable.AnyLand; }
                    if (oldTBGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldTBGCDs * TB.Ability.AvgTargets * TB.Ability.SwingsPerActivate * TB.Ability.MHAtkTable.AnyLand; }
                    if (oldOPGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldOPGCDs * OP.Ability.AvgTargets * OP.Ability.SwingsPerActivate * OP.Ability.MHAtkTable.AnyLand; }
                    if (oldEXGCDs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldEXGCDs * EX.Ability.AvgTargets * EX.Ability.SwingsPerActivate * EX.Ability.MHAtkTable.AnyLand; }
                    if (oldSoOActs != 0) { landsoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.Ability.AvgTargets * SoO.Ability.SwingsPerActivate * SoO.Ability.MHAtkTable.AnyLand; }
                    acts = Math.Min(gcdsAvailableU20, (CS.Ability as ColossusSmash).GetActivates(landsoverdurPluswhatwehaventprocessedyet/*LandedAtksOverDurU20*/, TimeUndr20Perc) * percTimeInDPS * percFailRageU20);
                    //acts = Math.Min(gcdsAvailableU20, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDurU20, percTimeU20) * percTimeInDPS * PercFailRageU20);
                    CS.NumActivatesU20 = acts;
                    availRageU20 -= CS.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= CS.GCDUsageU20;
                }
                CSspace = CS.NumActivatesU20 / NumGCDsU20 * CS.Ability.UseTime / LatentGCD;

                // Rend
                if (RD.Ability.Validated && WeWantTfB && DPSWarrChar.Talents.BloodAndThunder < 2 && gcdsAvailableU20 > 0)
                { // Ignore Rend when we have BnT at 100%
                    acts = Math.Min(gcdsAvailableU20, RD.Ability.Activates * percTimeInDPSAndU20 * percFailRageU20);
                    RD.NumActivatesU20 = acts;
                    availRageU20 -= RD.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= RD.GCDUsageU20;
                }
                RDspace = RD.NumActivatesU20 / NumGCDsU20 * RD.Ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.Ability.Validated && WeWantTfB && gcdsAvailableU20 > 0)
                {
                    acts = Math.Min(gcdsAvailableU20, TH.Ability.Activates * percTimeInDPSAndU20 * percFailRageU20);
                    TH.NumActivatesU20 = acts * (1f - RDspace - CSspace);
                    (RD.Ability as Rend).ThunderAppsU20 = TH.NumActivatesU20 * DPSWarrChar.Talents.BloodAndThunder * 0.50f;
                    availRageU20 -= TH.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= TH.GCDUsageU20;
                }
                //THspace = TH.numActivatesU20 / NumGCDsU20 * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (WeWantBLS && gcdsAvailableU20 > 0)
                {
                    // We only want to use Bladestorm during Exec phase IF it is going to do more damage, which requires Multiple Targets to be up
                    acts = Math.Min(gcdsAvailableU20, BLS.Ability.Activates * percTimeInDPSAndU20 * percFailRageU20);
                    BLS.NumActivatesU20 = acts * (1f - RDspace - CSspace);
                    availRageU20 -= BLS.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= BLS.GCDUsageU20;
                }
                BLSspace = BLS.NumActivatesU20 / NumGCDsU20 * BLS.Ability.UseTime / LatentGCD;

                /*// Mortal Strike // MS doesn't get used in Exec phase
                if (MS.ability.Validated) {
                    acts = Math.Min(gcdsAvailableU20, MS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    MS.numActivatesU20 = acts * (1f - BLSspace - CSspace);
                    availRageU20 -= MS.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= MS.GCDUsageU20;
                }
                MSspace = MS.numActivatesU20 / NumGCDsU20 * MS.ability.UseTime / LatentGCD;*/

                // Taste for Blood
                if (WeWantTfB && gcdsAvailableU20 > 0)
                {
                    acts = Math.Min(gcdsAvailableU20, TB.Ability.Activates * percTimeInDPSAndU20 * percFailRageU20);
                    TB.NumActivatesU20 = acts * (1f - BLSspace - CSspace);
                    availRageU20 -= TB.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= TB.GCDUsageU20;
                }
                TFBspace = TB.NumActivatesU20 / NumGCDsU20 * TB.Ability.UseTime / LatentGCD;

                // Overpower
                if (OP.Ability.Validated && WeWantTfB && gcdsAvailableU20 > 0)
                { // same check, no need to make it twice
                    float dodgesoverdurPluswhatwehaventprocessedyet = DodgedAttacksOverDur * TimeUndr20Perc;
                    if (oldEXGCDs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldEXGCDs * EX.Ability.AvgTargets * EX.Ability.SwingsPerActivate * EX.Ability.MHAtkTable.Dodge; }
                    if (oldSoOActs != 0) { dodgesoverdurPluswhatwehaventprocessedyet += oldSoOActs * SoO.Ability.AvgTargets * SoO.Ability.SwingsPerActivate * SoO.Ability.MHAtkTable.Dodge; }
                    acts = Math.Min(gcdsAvailableU20, (OP.Ability as Overpower).GetActivates(dodgesoverdurPluswhatwehaventprocessedyet/*DodgedAttacksOverDur*/, 0/*SoO.numActivatesU20*/) * percTimeInDPSAndU20 * percFailRageU20);
                    //acts = Math.Min(gcdsAvailableU20, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndU20 * PercFailRageU20);
                    OP.NumActivatesU20 = acts * (1f - TFBspace - RDspace - BLSspace /*- MSspace*/ - CSspace);
                    availRageU20 -= OP.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    gcdsAvailableU20 -= OP.GCDUsageU20;
                }
                //OPspace = OP.numActivatesU20 / NumGCDsU20 * OP.ability.UseTime / LatentGCD;

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
                if (EX.Ability.Validated /*&& PercFailRageUnder20 == 1f*/ && gcdsAvailableU20 > 0)
                {
                    acts = /*Math.Min(gcdsAvailableU20,*/ gcdsAvailableU20 * percTimeInDPS/*)*/;
                    //if (EX.ability.GetRageUseOverDur(acts) > availRage) acts = Math.Max(0f, availRage) / EX.ability.RageCost;
                    EX.NumActivatesU20 = (EX.Ability as Execute).DumbActivates = acts;
                    availRageU20 -= EX.RageU20 * RageModTotal * RageMOD_DeadlyCalm;
                    availRageU20 += EX.NumActivatesU20 * (DPSWarrChar.Talents.SuddenDeath * 5f);
                    gcdsAvailableU20 -= EX.GCDUsageU20;
                } else { EX.NumActivatesU20 = 0f; }
                //EXspace = EX.numActivatesU20 / NumGCDsU20 * EX.ability.UseTime / LatentGCD;

                // Strikes of Opportunity Procs
                if (SoO.Ability.Validated) {
                    SoO.NumActivatesU20 = (SoO.Ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurU20, TimeUndr20Perc);
                    //availRage -= SoO.RageU20; // Not sure if it should affect Rage
                }

                //float TotalSpace = (CSspace + RDspace + THspace + BLSspace /*+ MSspace*/ + OPspace + TFBspace /*+ SLspace*/ + EXspace /*+ HSspace + CLspace*/);
                (IR.Ability as InnerRage).FreeRageU20 = (EX.Ability as Execute).FreeRage = repassAvailRageU20 = availRageU20; // check for not enough rage to maintain rotation and set Execute's FreeRage to this value
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededU20 = 0f, rageGenOtherU20 = 0f;
            foreach (AbilWrapper aw in TheAbilityList) {
                if (aw.Ability is Rend) {
                    DPS_TTL += (aw.Ability as Rend).GetDPS(aw.NumActivatesU20, TH.NumActivatesU20, TimeUndr20Perc);
                } else {
                    DPS_TTL += aw.DPS_U20;
                }
                _HPS_TTL += aw.HPS_U20;
                if (aw.RageU20 > 0) { rageNeededU20 += aw.RageU20; }
                else { rageGenOtherU20 -= aw.RageU20; }
            }

            DPS_TTL += DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesU20, TimeUndr20Perc) * percTimeInDPS;
            
            return DPS_TTL;
        }

        public void MakeRotationAndDoDPS(bool setCalcs, float percTimeUnder20) {
            if (DPSWarrChar.Char.MainHand == null) { return; }
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
            availRage += RageGenOverDurOther + RageGainedWhileMoving;

            // ==== Maintenance and Anti-Impedence Priorities ====
            if (_needDisplayCalcs) GCDUsage += "Maintenance: Things that you do periodically to Buff yourself or the raid\n";
            float rageUsedByMaintenance = DoMaintenanceActivates(TotalPercTimeLost);

            // ==== Standard Priorities ===============
            if (_needDisplayCalcs) GCDUsage += "Abilities: Things that you do to damage the Target. These are not in order of priority.\n";
            _DPS_TTL_O20 = SettleAll(TotalPercTimeLost, rageUsedByMaintenance, availRage, out PercFailRageOver20);
            if (percTimeUnder20 != 0f) { _DPS_TTL_U20 = SettleAllU20(TotalPercTimeLost, rageUsedByMaintenance, availRage, out PercFailRageUnder20); }
            if (_needDisplayCalcs) {
                // We need to add Inner Rage & Deadly Calm now that we know how many there are
                AbilWrapper aw = GetWrapper<DeadlyCalm>();
                GCDUsage = aw.AllNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                                aw.Ability.Name, (!aw.Ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
                AbilWrapper aw2 = GetWrapper<InnerRage>();
                GCDUsage = aw2.AllNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw2.AllNumActivates, aw2.NumActivatesO20, aw2.NumActivatesU20,
                                aw2.Ability.Name, (!aw2.Ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
            }

            CalcDeepWounds();
            _DPS_TTL_O20 += DW.TickSize;
            _DPS_TTL_U20 += DW.TickSize;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs) {
                if (PercFailRageOver20 != 1.0f || PercFailRageUnder20 != 1.0f) {
                    GCDUsage += (PercFailRageOver20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation before Execute Spam.\n", (1f - PercFailRageOver20)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation during Execute Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                List<AbilWrapper> dmgAbils = DamagingAbilities;
                foreach (AbilWrapper aw in dmgAbils) {
                    if (aw.AllNumActivates > 0 && !aw.Ability.IsMaint && !(aw.Ability is HeroicLeap)) {
                        if (aw.Ability.GCDTime < 1.5f || aw.Ability.GCDTime > 3f) {
                            // Overpower (And TfB procs) use less than a GCD to recouperate.
                            // Bladestorm is channelled over 6 seconds (approx 4 GCDs)
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000} : {5}\n",
                                aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                                aw.Ability.GCDTime,
                                (aw.AllNumActivates * aw.Ability.GCDTime / (DPSWarrChar.CalcOpts.FullLatency + 1.5f)),
                                aw.Ability.Name
                            );
                        } else {
                            GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                                aw.Ability.Name,
                                aw.Ability.UsesGCD ? "" : " (Doesn't use GCDs)");
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
                this.calcs.WhiteDPSMH = DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSMH_U20 = DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesU20, TimeUndr20Perc);
                {
                    if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSMH_U20 > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH * (1f - percTimeUnder20) + this.calcs.WhiteDPSMH_U20 * (percTimeUnder20); }
                    else if (this.calcs.WhiteDPSMH_U20 > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH_U20; }
                    else if (this.calcs.WhiteDPSMH > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH; }
                    else { this.calcs.WhiteDPS = 0f; }
                }
                this.calcs.WhiteDmg = this.DPSWarrChar.Whiteattacks.MHDamageOnUse;

                {
                    if (_DPS_TTL_O20 > 0 && _DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20 * (1f - percTimeUnder20) + _DPS_TTL_U20 * (percTimeUnder20); }
                    else if (_DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_U20; }
                    else if (_DPS_TTL_O20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20; }
                    else { this.calcs.TotalDPS = 0f; }
                }

                this.calcs.WhiteRageO20 = DPSWarrChar.Whiteattacks.MHRageGenOverDurO20;
                this.calcs.OtherRageO20 = this.RageGenOverDurOtherO20;
                this.calcs.NeedyRageO20 = this.RageNeededOverDurO20;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;

                this.calcs.WhiteRageU20 = DPSWarrChar.Whiteattacks.MHRageGenOverDurU20;
                this.calcs.OtherRageU20 = this.RageGenOverDurOtherU20;
                this.calcs.NeedyRageU20 = this.RageNeededOverDurU20;
                this.calcs.FreeRageU20 = calcs.WhiteRageU20 + calcs.OtherRageU20 - calcs.NeedyRageU20;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
                MakeRotationAndDoDPS(setCalcs, TimeUndr20Perc);
            } catch (Exception ex) {
                new Rawr.Base.ErrorBox("Error in creating Arms Rotation Details",
                    ex, "MakeRotationandDoDPS(...)", "No Additional Info", "").Show();
            }
        }
    }
}
