using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{
    public class ShotPriority
    {
        public int latency;
        public ShotData[] priorities = new ShotData[10];
        public double GCD = 1.5;

        public double DPS = 0;
        public double MPS = 0;

        public void validateShots(HunterTalents hunterTalents)
        { 
            // check each shot in the priority is allowed to be there.
            // this means removing dupes and anything after steady shot.
            // it also means removing shots which have already had their cooldowns used

            bool removeAll = false;
            bool used_arcane_explosive = false;
            bool used_aimed_multi = false;
            bool used_black_immo = false;

            for (int i = 0; i < priorities.Length; i++)
            {
                // we've already seen steadyShot - remove everything else
                if (priorities[i] != null && removeAll) priorities[i] = null;

                // this shot has been used aleady
                if (priorities[i] != null && priorities[i].used) priorities[i] = null;                

                // we've already used a shot which shares cooldown with this one
                if (priorities[i] != null && (priorities[i].type == Shots.ArcaneShot && used_arcane_explosive)) priorities[i] = null;
                if (priorities[i] != null && (priorities[i].type == Shots.ExplosiveShot && used_arcane_explosive)) priorities[i] = null;
                if (priorities[i] != null && (priorities[i].type == Shots.AimedShot && used_aimed_multi)) priorities[i] = null;
                if (priorities[i] != null && (priorities[i].type == Shots.MultiShot && used_aimed_multi)) priorities[i] = null;
                if (priorities[i] != null && (priorities[i].type == Shots.BlackArrow && used_black_immo)) priorities[i] = null;
                if (priorities[i] != null && (priorities[i].type == Shots.ImmolationTrap && used_black_immo)) priorities[i] = null;

                // shots which require talents
                if (priorities[i] != null && priorities[i].type == Shots.BlackArrow && hunterTalents.BlackArrow == 0) priorities[i] = null;
                if (priorities[i] != null && priorities[i].type == Shots.ChimeraShot && hunterTalents.ChimeraShot == 0) priorities[i] = null;
                if (priorities[i] != null && priorities[i].type == Shots.AimedShot && hunterTalents.AimedShot == 0) priorities[i] = null;
                if (priorities[i] != null && priorities[i].type == Shots.ExplosiveShot && hunterTalents.ExplosiveShot == 0) priorities[i] = null;

                if (priorities[i] != null)
                {
                    priorities[i].used = true;
                    if (priorities[i].type == Shots.SteadyShot) removeAll = true;

                    if (priorities[i].type == Shots.ArcaneShot) used_arcane_explosive = true;
                    if (priorities[i].type == Shots.ExplosiveShot) used_arcane_explosive = true;
                    if (priorities[i].type == Shots.AimedShot) used_aimed_multi = true;
                    if (priorities[i].type == Shots.MultiShot) used_aimed_multi = true;
                    if (priorities[i].type == Shots.BlackArrow) used_black_immo = true;
                    if (priorities[i].type == Shots.ImmolationTrap) used_black_immo = true;
                }
            }       
        }

        public bool containsShot(Shots aType)
        {
            for (int i=0; i<priorities.Length; i++)
            {
                if (priorities[i] != null)
                {
                    if (priorities[i].type == aType) return true;
                }
            }

            return false;
        }

        public void calculateRotationDPS(){

            double CastLag = (latency / 1000.0);
            ShotData PrevShot = null;

            bool ChimeraRefreshesViper = false; // TODO
            bool ChimeraRefreshesSerpent = false; // TODO
            double LALExplosiveFrequency = 0; // TODO
            double LALArcaneFrequency = 0; // TODO
            bool UseKillShot = false; // TODO
            double ViperDamagePenalty = 0; // TODO
            bool UseRotationTest = false; // TODO

            MPS = 0;
            DPS = 0;

            for (int i=0; i<priorities.Length; i++)
            {
                if (priorities[i] != null){
                    ShotData s = priorities[i];

                    #region Timing Calculations

                    // cooldown
                    s.rotation_cooldown = priorities[i].cooldown;
                    if (s.type == Shots.SerpentSting)
                    {
                        s.rotation_cooldown = priorities[i].duration;
                    }

                    // time used
                    switch (s.type)
                    {
                        case Shots.None:
                            s.time_used = 0;
                            break;

                        case Shots.ImmolationTrap:
                            s.time_used = 0; // 1.5 * ImmoTrapCastTime
                            break;

                        case Shots.SteadyShot:
                            s.time_used = s.cooldown < GCD ? GCD : s.cooldown;
                            break;

                        default:
                            s.time_used = s.gcd ? GCD : 0;
                            break;
                    }

                    if (s.type != Shots.None){
                        double lag = s.gcd ? CastLag : 0.001; // non GCD = 1ms

                        s.time_used += lag;
                    }

                    //Debug.WriteLine("Cooldown " + i + " is " + s.rotation_cooldown);
                    //Debug.WriteLine("Time used " + i + " is " + s.time_used);

                    #endregion
                    #region Spreadsheet Calculations

                    // the hidden shot calculations!
                    // G  (RotationCooldown)
                    // H  (Mana)
                    // I  (Damage)
                    // J  (TimeUsed)
                    // K  (Ratio) =IF(OR(AND(ChimeraRefreshesViper,E30="Viper Sting"),AND(ChimeraRefreshesSerpent,E30="Serpent Sting")),"Refreshed",AB30)
                    // N  (MPS) =IF(AND(NOT(UseRotationTest),OR(E30="Explosive Shot",E30="Arcane Shot)),V30,IF(L30="Refreshed","Refreshed",AD30))
                    // O  (StartFreq) =IF(AJ30=TRUE,0,IF(G30>J30,ROUNDUP((G30-J30)/(GCD+CastLag),0)*(GCD+CastLag)+J30,GCD+CastLag))*IF(VLOOKUP(E30,ShotsTable,8,FALSE)<>FALSE,1,0)
                    // P  (GCDLeft)
                    // Q  (GCDNeeded) =IF(AND(NOT(UseKillShot,E30="Kill Shot"),0,IF(OR(AND(ChimeraRefreshesViper,E30="Viper Sting"),AND(ChimeraRefreshesSerpent,E30="Serpent Sting")),0,IF(O30>0,IF(J30>O30,1,J30/O30),0)))
                    // R  (GCDUsed)
                    // S  (InBetFreq) =IF(AND(O30>0,R30>0),J30/R30,0)
                    // T  (InBetStings)
                    // W  (LALFreq) =IF(OR(O30=0,T30=2),0,IF(E30="Explosive Shot",IF(LALExplosiveFrequency>0,LALExplosiveFrequency,S30),IF(E30="Arcane Shot",IF(LALArcaneFrequency>0,LALArcaneFrequency,S30),S30)))
                    // X  (LALGCDLeft)
                    // Y  (LALGCDNeeded) =IF(AND(NOT(UseKillShot),E30="Kill Shot"),0,IF(AND(ChimeraRefreshesSerpent,E30="Serpent Sting"),0,IF(W30>0,IF(J30>W30,1,J30/W30),0)))
                    // Z  (LALGCDUsed)
                    // AA (ActualFreq) =IF(UseRotationTest,'Rotation Test'!D11,IF(AND(W30>0,Z30>0),J3/Z30,0))
                    // AB (ActualRatio) =IF(AND(NOT(UseKillShot),E30="Kill Shot"),0,IF(AND(ChimeraRefereshesSerpent,E30="Serpent Sting"),0,IF(AA30>0,IF(J30>AA30,1,J30/AA30),0)))
                    // AC (ActualDPS) =IF(OR(AND(ChimeraRefreshesViper,E30="Viper Sting"),AND(ChimeraRefreshesSerpent,E30="Serpent Sting")),I30/G30,IF(AA30>0,I30/AA30,0))
                    // AD (ActualMPS) =IF(OR(AND(ChimeraRefreshesViper,E30="Viper Sting"),AND(ChimeraRefreshesSerpent,E30="Serpent Sting")),0,IF(AA30>0,H30/AA30,0))
                    // AJ (InvalidShotPriority) =IF(VLOOKUP(E30,ShotPriorityList,2,FALSE)<>F30,TRUE,OR(IF(AND(E30="Arcane Shot",ExplosiveInRotation),IF(VLOOKUP("Explosive Shot",ShotPriorityList,2,FALSE)<F30,TRUE,FALSE),FALSE,IF(AND(

                    #endregion
                    #region Starting Calculations

                    double start_freq = (s.rotation_cooldown > s.time_used) ? Math.Ceiling((s.rotation_cooldown - s.time_used) / (GCD + CastLag)) * (GCD + CastLag) + s.time_used : GCD + CastLag;

                    s.start_gcd_needed = (start_freq > 0) ? (s.time_used > start_freq ? 1 : s.time_used / start_freq) : 0;

                    if (ChimeraRefreshesViper && s.type == Shots.ViperSting) s.start_gcd_needed = 0;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) s.start_gcd_needed = 0;
                    if (!UseKillShot && s.type == Shots.KillShot) s.start_gcd_needed = 0;

                    s.start_gcd_left = 1;
                    if (PrevShot != null)
                    {
                        s.start_gcd_left = PrevShot.start_gcd_left - PrevShot.start_gcd_used;
                    }

                    s.start_gcd_used = 1 - (s.start_gcd_left - s.start_gcd_needed);
                    if (PrevShot != null)
                    {
                        s.start_gcd_used = s.start_gcd_left > s.start_gcd_needed ? s.start_gcd_needed : s.start_gcd_left;
                    }

                    //Debug.WriteLine("Start Freq " + i + " is " + start_freq);
                    //Debug.WriteLine("GCD Left " + i + " is " + s.start_gcd_left);
                    //Debug.WriteLine("GCD Needed " + i + " is " + s.start_gcd_needed);
                    //Debug.WriteLine("GCD Used " + i + " is " + s.start_gcd_used);

                    #endregion
                    #region In-Between Calculations

                    double inbet_freq = (start_freq > 0 && s.start_gcd_used > 0) ? s.time_used / s.start_gcd_used : 0;

                    if (PrevShot != null)
                    {
                        if (s.type == Shots.SerpentSting || s.type == Shots.ScorpidSting)
                        {
                            s.sting_count = PrevShot.sting_count < 2 ? PrevShot.sting_count + 1 : PrevShot.sting_count;
                        }
                        else 
                        {
                            s.sting_count = PrevShot.sting_count == 2 ? 3 : PrevShot.sting_count;
                        }
                    }
                    else
                    {
                        s.sting_count = (s.type == Shots.SerpentSting || s.type == Shots.ScorpidSting) ? 1 : 0;
                    }

                    double inbet_dps = inbet_freq > 0 ? s.damage / inbet_freq : 0;

                    if (ChimeraRefreshesViper && s.type == Shots.ViperSting) inbet_dps = s.damage / start_freq;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) inbet_dps = s.damage / start_freq;

                    double inbet_mps = inbet_freq > 0 ? s.mana / inbet_freq : 0;


                    //Debug.WriteLine("InBet Freq " + i + " is " + inbet_freq);
                    //Debug.WriteLine("Sting Count " + i + " is " + s.sting_count);
                    //Debug.WriteLine("InBet DPS " + i + " is " + inbet_dps);
                    //Debug.WriteLine("InBet MPS " + i + " is " + inbet_mps);

                    #endregion
                    #region Lock-and-Load Calculations

                    double lal_freq = inbet_freq;
                    if (s.type == Shots.ExplosiveShot && LALExplosiveFrequency > 0) lal_freq = LALExplosiveFrequency;
                    if (s.type == Shots.ArcaneShot && LALArcaneFrequency > 0) lal_freq = LALArcaneFrequency;
                    if (start_freq == 0 || s.sting_count == 2) lal_freq = 0;

                    s.lal_gcd_left = 1;
                    if (PrevShot != null)
                    {
                        s.lal_gcd_left = PrevShot.lal_gcd_left - PrevShot.lal_gcd_used;
                    }

                    s.lal_gcd_needed = lal_freq > 0 ? (s.time_used > lal_freq ? 1 : s.time_used / lal_freq) : 0;
                    if (!UseKillShot && s.type == Shots.KillShot) s.lal_gcd_needed = 0;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) s.lal_gcd_needed = 0;

                    s.lal_gcd_used = 1 - (s.lal_gcd_left - s.lal_gcd_needed);
                    if (PrevShot != null)
                    {
                        s.lal_gcd_used = s.lal_gcd_left > s.lal_gcd_needed ? s.lal_gcd_needed : s.lal_gcd_left;
                    }
                    
                    //Debug.WriteLine("LAL Freq " + i + " is " + lal_freq);
                    //Debug.WriteLine("LAL GCD Left " + i + " is " + s.lal_gcd_left);
                    //Debug.WriteLine("LAL GCD Needed " + i + " is " + s.lal_gcd_needed);
                    //Debug.WriteLine("LAL GCD Used " + i + " is " + s.lal_gcd_used);

                    #endregion
                    #region Final/Actual Calculations

                    // TODO: insert rotation test data here...
                    double final_freq = (lal_freq > 0 && s.lal_gcd_used > 0) ? s.time_used / s.lal_gcd_used : 0;

                    double final_ratio = final_freq > 0 ? (s.time_used > final_freq ? 1 : s.time_used / final_freq) : 0;
                    if (!UseKillShot && s.type == Shots.KillShot) final_ratio = 0;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) final_ratio = 0;

                    double final_dps = final_freq > 0 ? s.damage / final_freq : 0;
                    if (ChimeraRefreshesViper && s.type == Shots.ViperSting) final_dps = s.damage / s.rotation_cooldown;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) final_dps = s.damage / s.rotation_cooldown;

                    double final_mps = final_freq > 0 ? s.mana / final_freq : 0;
                    if (ChimeraRefreshesViper && s.type == Shots.ViperSting) final_mps = 0;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) final_mps = 0;

                    //Debug.WriteLine("Final Freq " + i + " is " + final_freq);
                    //Debug.WriteLine("Final Ratio " + i + " is " + final_ratio);
                    //Debug.WriteLine("Final DPS " + i + " is " + final_dps);
                    //Debug.WriteLine("Final MPS " + i + " is " + final_mps);

                    #endregion
                    #region Output Calculations

                    s.is_refreshed = false;
                    s.ratio = 0;
                    s.freq = 0;

                    if (ChimeraRefreshesViper && s.type == Shots.ViperSting) s.is_refreshed = true;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting) s.is_refreshed = true;

                    if (!s.is_refreshed)
                    {
                        s.ratio = final_ratio;
                        s.freq = final_freq;
                    }

                    s.dps = final_dps;
                    if (ChimeraRefreshesSerpent && s.type == Shots.SerpentSting)
                    {
                        // immune to AspectOfViper
                    }
                    else
                    {
                        s.dps *= 1 - ViperDamagePenalty;
                    }

                    if (!UseRotationTest && (s.type == Shots.ExplosiveShot || s.type == Shots.ArcaneShot))
                    {
                        s.mps = inbet_mps;
                    }
                    else 
                    {
                        if (s.is_refreshed)
                        {
                            s.mps = 0;
                        }
                        else
                        {
                            s.mps = final_mps;
                        }
                    }

                    //Debug.WriteLine("DPS " + i + " is " + s.dps);
                    //Debug.WriteLine("MPS " + i + " is " + s.mps);

                    #endregion

                    DPS += s.dps;
                    MPS += s.mps;

                    PrevShot = s;
                }
            }
        }
    }

    public class ShotData
    {
        public Shots type = Shots.None;
        public double damage = 0;
        public double damageNormal = 0;
        public double damageCrit = 0;
        public double mana = 0;
        public double cooldown = 0;
        public double duration = 0;
        public bool critProcs = false;
        public bool gcd = false;
        
        public bool used = false; // for de-duping the priority list

        public double rotation_cooldown = 0;
        public double time_used = 0;

        public double start_gcd_left = 0;
        public double start_gcd_needed = 0;
        public double start_gcd_used = 0;
        public int sting_count = 0;
        public double lal_gcd_left = 0;
        public double lal_gcd_needed = 0;
        public double lal_gcd_used = 0;

        public double ratio = 0;
        public bool is_refreshed = false;
        public double freq = 0;
        public double dps = 0;
        public double mps = 0;


        public ShotData(Shots aType)
        {
            type = aType;
        }

        public void Dump(string label)
        {
            Debug.WriteLine(label + " damage = " + damage);
            Debug.WriteLine(label + " mana cost = " + mana);
            Debug.WriteLine(label + " cooldown = " + cooldown);
            Debug.WriteLine(label + " duration = " + duration);
            Debug.WriteLine(label + " critProcs = " + critProcs);
            Debug.WriteLine(label + " gcd = " + gcd);
        }
    }

}
