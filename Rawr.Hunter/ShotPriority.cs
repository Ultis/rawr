using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter {
    public class ShotPriority {
        public ShotData[] priorities = new ShotData[10];

        public CalculationOptionsHunter options;

        public bool chimeraRefreshesSerpent = false;
        public bool chimeraRefreshesViper = false;
        public float LALExplosiveFrequency = 0f;
        public float LALArcaneFrequency = 0f;
        public bool useKillShot = false; // TODO
        public float viperDamagePenalty = 0f;

        public float specialShotsPerSecond = 0f;
        public float critSpecialShotsPerSecond = 0f;

        public float critsRatioSum = 0f;
        public float critsCompositeSum = 0f;

        public float DPS = 0f;
        public float MPS = 0f;

        public ShotPriority(CalculationOptionsHunter options) { this.options = options; }

        public void validateShots(HunterTalents hunterTalents) { 
            // check each shot in the priority is allowed to be there.
            // this means removing dupes and anything after steady shot.
            // it also means removing shots which have already had their cooldowns used

            bool removeAll = false;
            bool used_arcane_explosive = false;
            bool used_aimed_multi = false;
            bool used_black_immo = false;

            for (int i = 0; i < priorities.Length; i++) {
                // we've already seen steadyShot - remove everything else
                if (priorities[i] != null && removeAll) { priorities[i].steadyBefore = true; priorities[i] = null; }

                // this shot has been used aleady
                if (priorities[i] != null && priorities[i].used) priorities[i] = null;                

                // we've already used a shot which shares cooldown with this one
                if (priorities[i] != null && (priorities[i].type == Shots.ArcaneShot && used_arcane_explosive)) { priorities[i].cooldownUsed = true; priorities[i] = null; }
                if (priorities[i] != null && (priorities[i].type == Shots.ExplosiveShot && used_arcane_explosive)){ priorities[i].cooldownUsed = true; priorities[i] = null;}
                if (priorities[i] != null && (priorities[i].type == Shots.AimedShot && used_aimed_multi)){ priorities[i].cooldownUsed = true; priorities[i] = null;}
                if (priorities[i] != null && (priorities[i].type == Shots.MultiShot && used_aimed_multi)){ priorities[i].cooldownUsed = true; priorities[i] = null;}
                if (priorities[i] != null && (priorities[i].type == Shots.BlackArrow && used_black_immo)){ priorities[i].cooldownUsed = true; priorities[i] = null;}
                if (priorities[i] != null && (priorities[i].type == Shots.ImmolationTrap && used_black_immo)){ priorities[i].cooldownUsed = true; priorities[i] = null;}

                // shots which require talents
                if (priorities[i] != null && priorities[i].type == Shots.BlackArrow && hunterTalents.BlackArrow == 0){ priorities[i].lackTalent = true; priorities[i] = null; }
                if (priorities[i] != null && priorities[i].type == Shots.ChimeraShot && hunterTalents.ChimeraShot == 0) { priorities[i].lackTalent = true; priorities[i] = null; }
                if (priorities[i] != null && priorities[i].type == Shots.AimedShot && hunterTalents.AimedShot == 0) { priorities[i].lackTalent = true; priorities[i] = null; }
                if (priorities[i] != null && priorities[i].type == Shots.ExplosiveShot && hunterTalents.ExplosiveShot == 0){priorities[i].lackTalent = true; priorities[i] = null;}
                if (priorities[i] != null && priorities[i].type == Shots.BeastialWrath && hunterTalents.BestialWrath == 0) { priorities[i].lackTalent = true; priorities[i] = null; }
                if (priorities[i] != null && priorities[i].type == Shots.SilencingShot && hunterTalents.SilencingShot == 0) { priorities[i].lackTalent = true; priorities[i] = null; }

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
   
            // store some derived facts about the rotation for later use
            chimeraRefreshesSerpent = false;
            chimeraRefreshesViper = false;
            if (containsShot(Shots.ChimeraShot))
            {
                if (containsShot(Shots.SerpentSting)) chimeraRefreshesSerpent = true;
                if (containsShot(Shots.ViperSting)) chimeraRefreshesViper = true;
            }
        }

        public void initializeTimings()
        {
            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] != null) priorities[i].initializeTimings(this);
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

        public ShotData getShotInRotation(Shots aType)
        {
            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] != null)
                {
                    if (priorities[i].type == aType) return priorities[i];
                }
            }

            return null;
        }

        public void calculateFrequencies()
        {
            // This function calculates the frequencies for each shot.
            // We have already calculated all shot cooldowns and validated the rotation by this point.

            // THIS FUNCTION IS CALLED THREE TIMES - TRY TO REMEMBER THIS!
            // 1) When we have mana and basic timings
            // 2) When we have LAL proc info
            // 3) When we have steady shot speed

            ShotData PrevShot = null;

            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] == null) continue;
                ShotData s = priorities[i];
                s.calculateTimings(this, PrevShot);

                PrevShot = s;
            }
        }

        public void recalculateRatios()
        {
            // this function is called when we're using the rotation test.
            // we call it once we know the cast time for steady shot, to 
            // get the correct ratio numbers.

            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] == null) continue;
                priorities[i].finishCalculateTimings(this);
            }
        }

        public void calculateFrequencySums()
        {
            specialShotsPerSecond = 0f;
            critSpecialShotsPerSecond = 0f;
            critsRatioSum = 0f;

            // 31-10-2009 Drizz: Updated to follow 92b
            // I still don't understand the method of how critSpecialShotsPerSecond is calculated.

            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] == null) continue;
                ShotData s = priorities[i];

                specialShotsPerSecond += 1 / s.final_freq;
                
                if (s.final_freq > 0)
                    if (s.type == Shots.SerpentSting)
                        critSpecialShotsPerSecond += 1 / (s.final_freq/ s.rotation_cooldown * 3);
                    else
                        critSpecialShotsPerSecond += 1 / s.final_freq;
               
                critsRatioSum += s.crits_ratio;
            }
        }

        public void calculateCrits()
        {
            // called after we find out shot crit rates.
            // we calculated frequencies and ratios in earlier calls.

            critsCompositeSum = 0f;

            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] == null) continue;
                ShotData s = priorities[i];

                s.calculateComposites(this);
                critsCompositeSum += s.crits_composite;
            }
        }

        public void calculateLALProcs(Character character)
        {
            float lal_trigger_freq = 0f;
            float lal_trigger_duration = 0f;

            ShotData proc_shot = null;
            if (proc_shot == null) proc_shot = getShotInRotation(Shots.BlackArrow);
            if (proc_shot == null) proc_shot = getShotInRotation(Shots.ImmolationTrap);

            if (proc_shot != null)
            {
                lal_trigger_freq = proc_shot.lal_freq;
                lal_trigger_duration = proc_shot.duration;
            }

            float lal_proc_chance = 0.02f * character.HunterTalents.LockAndLoad;
            //lal_proc_chance = 0.06; //isolation testing for LAL
            float lal_proc_freq = 0f;

            if (options.useRotationTest) {
                // the spreadsheet gets the LAL proc frequency from the rotation test,
                // but it doesn't really matter because it's only used to calculate some
                // stats we'll throw away (final_freq via lal_freq).
            } else {
                if (lal_trigger_freq > 0 && lal_proc_chance > 0)
                {
                    lal_proc_freq = (lal_trigger_duration > lal_trigger_freq ? 1f : lal_trigger_freq / lal_trigger_duration) * 3f / lal_proc_chance;
                }
            }

            ShotData lalExplosive = getShotInRotation(Shots.ExplosiveShot);
            ShotData lalArcane = getShotInRotation(Shots.ArcaneShot);

            float pre_lal_explosive_freq = (lalExplosive != null && options.LALShotToUse == Shots.ExplosiveShot) ? lalExplosive.inbet_freq : 0;
            float pre_lal_arcane_freq = (lalArcane != null && options.LALShotToUse == Shots.ArcaneShot) ? lalArcane.inbet_freq : 0;

            LALExplosiveFrequency = 0;
            if (lal_proc_freq > 0 && pre_lal_explosive_freq > 0) {
                LALExplosiveFrequency = 1f / (1f / (lal_proc_freq / (pre_lal_explosive_freq / (pre_lal_arcane_freq + pre_lal_explosive_freq) * options.LALShotsReplaced)) + 1f / pre_lal_explosive_freq);
            }

            LALArcaneFrequency = 0;
            if (lal_proc_freq > 0 && pre_lal_arcane_freq > 0) {
                LALArcaneFrequency = 1f / (1f / (lal_proc_freq / (pre_lal_arcane_freq / (pre_lal_explosive_freq + pre_lal_arcane_freq) * options.LALShotsReplaced)) + 1f / pre_lal_arcane_freq);
            }
        }

        public void calculateRotationMPS()
        {
            MPS = 0;
            for (int i = 0; i < priorities.Length; i++)
            {
                if (priorities[i] != null)
                {
                    ShotData s = priorities[i];

                    s.calculateMPS(this);

                    MPS += s.mps;
                }
            }
        }

        public void calculateRotationDPS()
        {
            bool debug_shot_rotation = false;

            DPS = 0;

            for (int i=0; i<priorities.Length; i++)
            {
                if (priorities[i] != null){
                    ShotData s = priorities[i];

                    s.calculateDPS(this);

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

                    DPS += s.dps;

                    if (debug_shot_rotation) {
                        string col1 = String.Format("{0,6:0.00}", s.rotation_cooldown);
                        string col2 = String.Format("{0,3:0}", s.mana);
                        string col3 = String.Format("{0,4:0}", s.damage);
                        string col4 = String.Format("{0:0.00}", s.time_used);
                        string col5 = String.Format("{0,6:0.00}%", 100*s.ratio);
                        string col6 = String.Format("{0,6:0.00}", s.freq);
                        string col7 = String.Format("{0,6:0.00}", s.dps);
                        string col8 = String.Format("{0,6:0.00}", s.mps);

                        Debug.WriteLine("Shot: |" + col1 + "|" + col2 + "|" + col3 + "|" + col4 + "|" + col5 + "|" + col6 + "|" + col7 + "|" + col8 + "|");
                    }
                } else {
                    if (debug_shot_rotation) {
                        Debug.WriteLine("None: |------|---|----|----|-------|------|  0.00|  0.00|");
                    }
                }
            }

            if (debug_shot_rotation) {
                string debug_dps = String.Format("{0,6:0.00}", DPS);
                string debug_mps = String.Format("{0,6:0.00}", MPS);

                Debug.WriteLine("TOTAL:                                    |"+debug_dps+"|"+debug_mps+"|");
            }
        }
    }

    public class ShotData
    {
        public const float GCD = 1.5f;

        public Shots type = Shots.None;
        public float damage = 0f;
        public float damageNormal = 0f;
        public float damageCrit = 0f;
        public float mana = 0f;
        public float cooldown = 0f;
        public float duration = 0f;
        public bool critProcs = false;
        public bool gcd = false;
        public float critChance = 0f;

        public bool steadyBefore = false;
        public bool cooldownUsed = false; // used in tooltip display        
        public bool lackTalent = false; // used in tooltip display        
        public bool used = false; // for de-duping the priority list

        public float rotation_cooldown = 0f;
        public float time_used = 0f;
        public float ratio = 0f;
        public bool is_refreshed = false;
        public float freq = 0f;
        public float dps = 0f;
        public float mps = 0f;

        // used in the 4 intermediate stages of calculations (start, inbet, lal, final)
        public float start_freq = 0f;
        protected float start_gcd_left = 0f;
        protected float start_gcd_needed = 0f;
        protected float start_gcd_used = 0f;

        public float inbet_freq = 0f;
        protected int sting_count = 0;

        public float lal_freq = 0;
        protected float lal_gcd_left = 0f;
        protected float lal_gcd_needed = 0f;
        protected float lal_gcd_used = 0f;

        public float final_freq = 0f; // the rotation test can set this
        protected float final_ratio = 0f;

        public float crits_per_sec = 0f;
        public float crits_ratio = 0f;
        public float crits_composite = 0f;

        public ShotData(Shots aType, bool aCritProcs, bool aGcd)
        {
            type = aType;
            critProcs = aCritProcs;
            gcd = aGcd;
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

        public void initializeTimings(ShotPriority Priority)
        {
            float CastLag = Priority.options.Latency;

            #region Timing Calculations

            // rotation cooldown
            rotation_cooldown = cooldown;
            if (type == Shots.SerpentSting)
            {
                rotation_cooldown = duration;
            }

            // time used
            switch (type)
            {
                case Shots.None:
                    time_used = 0f;
                    break;

                case Shots.ImmolationTrap:
                    time_used = Priority.options.gcdsToLayImmoTrap * GCD;
                    break;

                case Shots.SteadyShot:
                    time_used = cooldown < GCD ? GCD : cooldown;
                    break;

                default:
                    time_used = gcd ? GCD : 0f;
                    break;
            }

            if (type != Shots.None)
            {
                float lag = gcd ? CastLag : 0.001f; // non GCD = 1ms

                time_used += lag;
            }

            #endregion
        }

        public void calculateTimings(ShotPriority Priority, ShotData PrevShot)
        {
            float CastLag = Priority.options.Latency;

            #region Starting Calculations

            start_freq = (rotation_cooldown > time_used) ? (float)Math.Ceiling((rotation_cooldown - time_used) / (GCD + CastLag)) * (GCD + CastLag) + time_used : GCD + CastLag;

            start_gcd_needed = (start_freq > 0) ? (time_used > start_freq ? 1f : time_used / start_freq) : 0f;

            if (Priority.chimeraRefreshesViper && type == Shots.ViperSting) start_gcd_needed = 0f;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) start_gcd_needed = 0f;
            if (!Priority.useKillShot && type == Shots.KillShot) start_gcd_needed = 0f;

            start_gcd_left = 1f;
            if (PrevShot != null) {
                start_gcd_left = PrevShot.start_gcd_left - PrevShot.start_gcd_used;
            }

            start_gcd_used = 1f - (start_gcd_left - start_gcd_needed);
            if (PrevShot != null) {
                start_gcd_used = start_gcd_left > start_gcd_needed ? start_gcd_needed : start_gcd_left;
            }

            //Debug.WriteLine("Start Freq is " + start_freq);
            //Debug.WriteLine("GCD Left is " + start_gcd_left);
            //Debug.WriteLine("GCD Needed is " + start_gcd_needed);
            //Debug.WriteLine("GCD Used is " + start_gcd_used);

            #endregion
            #region In-Between Calculations

            inbet_freq = (start_freq > 0 && start_gcd_used > 0) ? time_used / start_gcd_used : 0;

            if (PrevShot != null) {
                if (type == Shots.SerpentSting || type == Shots.ScorpidSting) {
                    sting_count = PrevShot.sting_count < 2 ? PrevShot.sting_count + 1 : PrevShot.sting_count;
                } else {
                    sting_count = PrevShot.sting_count == 2 ? 3 : PrevShot.sting_count;
                }
            } else {
                sting_count = (type == Shots.SerpentSting || type == Shots.ScorpidSting) ? 1 : 0;
            }

            //Debug.WriteLine("Pre-LAL freq = "+inbet_freq);
            //Debug.WriteLine("Pre-LAL sting count = " + sting_count);

            #endregion
            #region Lock-and-Load Calculations

            lal_freq = inbet_freq;
            if (type == Shots.ExplosiveShot && Priority.LALExplosiveFrequency > 0f) lal_freq = Priority.LALExplosiveFrequency;
            if (type == Shots.ArcaneShot && Priority.LALArcaneFrequency > 0f) lal_freq = Priority.LALArcaneFrequency;
            if (start_freq == 0f || sting_count == 2f) lal_freq = 0f;

            lal_gcd_left = 1;
            if (PrevShot != null) {
                lal_gcd_left = PrevShot.lal_gcd_left - PrevShot.lal_gcd_used;
            }

            lal_gcd_needed = lal_freq > 0f ? (time_used > lal_freq ? 1f : time_used / lal_freq) : 0f;
            if (!Priority.useKillShot && type == Shots.KillShot) lal_gcd_needed = 0f;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) lal_gcd_needed = 0f;

            lal_gcd_used = 1f - (lal_gcd_left - lal_gcd_needed);
            if (PrevShot != null) {
                lal_gcd_used = lal_gcd_left > lal_gcd_needed ? lal_gcd_needed : lal_gcd_left;
            }

            #endregion

            // RotationTest may come in later and override this value,
            // hence the split-function that we call here
            final_freq = (lal_freq > 0 && lal_gcd_used > 0) ? time_used / lal_gcd_used : 0;

            finishCalculateTimings(Priority);
        }

        public void finishCalculateTimings(ShotPriority Priority)
        {
            final_ratio = final_freq > 0f ? (time_used > final_freq ? 1f : time_used / final_freq) : 0f;
            if (!Priority.useKillShot && type == Shots.KillShot) final_ratio = 0;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) final_ratio = 0f;
            
            #region Output Calculations

            is_refreshed = false;
            ratio = 0;
            freq = 0;

            if (Priority.chimeraRefreshesViper && type == Shots.ViperSting) is_refreshed = true;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) is_refreshed = true;

            if (!is_refreshed) {
                ratio = final_ratio;
                freq = final_freq;
            }

            crits_per_sec = (critProcs && final_freq > 0f) ? 1f / final_freq : 0f;
            crits_ratio = (critProcs) ? ratio : 0f;

            #endregion
        }

        public void setFrequency(ShotPriority Priority, float new_freq) {
            // called by the RotationTest after it calculates real frequencies
            start_freq = new_freq;
            final_freq = new_freq;
            finishCalculateTimings(Priority);
        }

        public void calculateComposites(ShotPriority Priority)
        {
            if (crits_ratio == 0)
            {
                crits_composite = 0;
                return;
            }
            crits_composite = crits_ratio > 0 ? critChance * (crits_ratio / Priority.critsRatioSum) : 0;
        }

        public void calculateMPS(ShotPriority Priority)
        {
            float inbet_mps = inbet_freq > 0 ? mana / inbet_freq : 0;

            float final_mps = final_freq > 0 ? mana / final_freq : 0;

            if (Priority.chimeraRefreshesViper && type == Shots.ViperSting) final_mps = 0;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) final_mps = 0;

            if (!Priority.options.useRotationTest && (type == Shots.ExplosiveShot || type == Shots.ArcaneShot)) {
                mps = inbet_mps;
            } else {
                if (is_refreshed) {
                    mps = 0;
                } else {
                    mps = final_mps;
                }
            }
        }

        public void calculateDPS(ShotPriority Priority)
        {
            #region In-Between Calculations

            float inbet_dps = inbet_freq > 0 ? damage / inbet_freq : 0;

            if (Priority.chimeraRefreshesViper && type == Shots.ViperSting) inbet_dps = damage / start_freq;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) inbet_dps = damage / start_freq;

            //Debug.WriteLine("InBet DPS is " + inbet_dps);

            #endregion
            #region Final/Actual Calculations

            float final_dps = final_freq > 0 ? damage / final_freq : 0;
            if (Priority.chimeraRefreshesViper && type == Shots.ViperSting) final_dps = damage / rotation_cooldown;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) final_dps = damage / rotation_cooldown;

            //Debug.WriteLine("Final DPS is " + final_dps);

            #endregion
            #region Output Calculations

            dps = final_dps;
            if (Priority.chimeraRefreshesSerpent && type == Shots.SerpentSting) {
                // immune to AspectOfViper
            } else {
                dps *= 1 - Priority.viperDamagePenalty;
            }

            //Debug.WriteLine("DPS is " + dps);

            #endregion
        }

        public string formatTooltip()
        {
            string ret = damage.ToString("00000") + " : " + dps.ToString("0000.00") + " : " + freq.ToString("00.00") + "*";

            ret += "Damage: " + damage.ToString("F2") + "\n";
            ret += "Mana: " + mana.ToString("F2") + "\n";
            ret += "Cooldown: "+cooldown.ToString("F2")+"\n";
            if (duration > 0) ret += "Duration: " + duration.ToString("F2") + "\n";
            if (critProcs) ret += "Crit Chance: " + critChance.ToString("P2") + "\n";

            if (freq > 0) {
                ret += "Rotation Freqency: " + freq.ToString("F2") + "\n";
                ret += "Rotation DPS: " + dps.ToString("F2") + "\n";
                ret += "Rotation MPS: " + mps.ToString("F2");
            } else if (is_refreshed) {
                ret += "Rotation Freqency: " + freq.ToString("F2") + " (Refreshed by Chimera)\n";
                ret += "Rotation DPS: " + dps.ToString("F2") + "\n";
                ret += "Rotation MPS: " + mps.ToString("F2") + " (Refreshed by Chimera)";
            } else {
                ret += cooldownUsed ? "Not being used in rotation:\n  Shares a cooldown with a higher\n  priority shot" : 
                    lackTalent      ? "Not being used in rotation:\n  You lack the needed talent" :
                    steadyBefore    ? "Not being used in rotation:\n  Steady shot has a higher\n  priority" :
                    "(Not in rotation)";
            }
            /*
            if (false) // yet another rotation debug block
            {
                ret += "\n-\n";
                //ret += "start_freq = " + start_freq.ToString("F2") + "\n";
                //ret += "inbet_freq = " + inbet_freq.ToString("F2") + "\n";
                //ret += "lal_freq = " + lal_freq.ToString("F2") + "\n";
                //ret += "final_freq = " + final_freq.ToString("F2") + "\n";
                ret += "crits_per_sec = " + crits_per_sec.ToString("P2") + "\n";
                ret += "crits_ratio = " + crits_ratio.ToString("P2") + "\n";
                ret += "crits_composite = " + crits_composite.ToString("P2") + "\n";
                //ret += "ratio = " + ratio.ToString("P2") + "\n";
                //ret += "time_used = " + time_used.ToString("F2") + "\n";
            }
            */
            return ret;
        }
    }

}
