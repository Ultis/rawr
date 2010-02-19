using System;
using System.Collections.Generic;
using Rawr.Base.Algorithms;
using System.Text;
using Rawr.Base;

namespace Rawr.DPSWarr.Markov
{
    class FuryGenerator : StateSpaceGenerator<Skills.Ability>
    {
        const int NUM_DEC = 3;
        const string FORMAT = "0.000";
        public FuryGenerator(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co)
        {
            Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; WhiteAtks = wa; CalcOpts = co;
            //
            Rot = new FuryRotation(c, s, cf, wa, co);
            Rot.Initialize();
            LatentGCD = Math.Round(1.5 + co.FullLatency, NUM_DEC);
            HastedGCD = Math.Round(1.0 + co.FullLatency, NUM_DEC);
            HSPerc = 1f;
            has4T10 = (s.BonusWarrior_T10_4P_BSSDProcChange > 0);
            BS1Proc = Talents.Bloodsurge / 15f; // 7/13/20%
            BS2Proc = 0f;
            if (has4T10)
            {
                BS2Proc = 0.2f * BS1Proc;
                BS1Proc = BS1Proc - BS2Proc;
            }

            SetChances();

            WhiteInterval = WhiteAtks.MhEffectiveSpeed;
        }

        #region Variables
        protected double LatentGCD;
        protected double HastedGCD;
        protected double HSPerc;
        protected bool has4T10;
        public FuryRotation Rot = null;
        Character Char;
        WarriorTalents Talents;
        Stats StatS;
        CombatFactors combatFactors;
        Skills.WhiteAttacks WhiteAtks;
        CalculationOptionsDPSWarr CalcOpts;
        protected double BS1Proc;
        protected double BS2Proc;

        protected BloodSurgeMatrix HSMatrix, WWMatrix, BTMatrix;

        /*protected double chanceHSProcs0, chanceHSProcs1, chanceHSProcs2;
        protected double chanceWWProcs0, chanceWWProcs1, chanceWWProcs2;
        protected double chanceBTProcs0, chanceBTProcs1, chanceBTProcs2;*/

        protected double WhiteInterval;
        #endregion

        private void SetChances()
        {
            Rawr.DPSWarr.Rotation.AbilWrapper BT = Rot.GetWrapper<Skills.BloodThirst>();
            Rawr.DPSWarr.Rotation.AbilWrapper WW = Rot.GetWrapper<Skills.WhirlWind>();
            Rawr.DPSWarr.Rotation.AbilWrapper BS = Rot.GetWrapper<Skills.BloodSurge>();
            Rawr.DPSWarr.Rotation.AbilWrapper HS = Rot.GetWrapper<Skills.HeroicStrike>();

            // HS
            HSMatrix = new BloodSurgeMatrix();
            HSMatrix.AddAbility(BS1Proc, BS2Proc, HS.ability.MHAtkTable.AnyLand);
            //chanceHSProcs1 = HS.ability.MHAtkTable.AnyLand * BS1Proc;
            //chanceHSProcs2 = HS.ability.MHAtkTable.AnyLand * BS2Proc;
            //chanceHSProcs0 = 1f - chanceHSProcs1 - chanceHSProcs2;

            // WW
            WWMatrix = new BloodSurgeMatrix();
            WWMatrix.AddAbility(BS1Proc, BS2Proc, WW.ability.MHAtkTable.AnyLand);
            WWMatrix.AddAbility(BS1Proc, BS2Proc, WW.ability.OHAtkTable.AnyLand);
            /*double chanceMHProcs1 = WW.ability.MHAtkTable.AnyLand * BS1Proc;
            double chanceMHProcs2 = WW.ability.MHAtkTable.AnyLand * BS2Proc;
            double chanceMHProcs0 = 1f - chanceMHProcs1 - chanceMHProcs2;

            double chanceOHProcs1 = WW.ability.OHAtkTable.AnyLand * BS1Proc;
            double chanceOHProcs2 = WW.ability.OHAtkTable.AnyLand * BS2Proc;
            double chanceOHProcs0 = 1f - chanceOHProcs1 - chanceOHProcs2;

            BloodSurgeMatrix wwMatrix = new BloodSurgeMatrix();
            wwMatrix.AddAbility(new double[] { chanceMHProcs0, chanceMHProcs1, chanceMHProcs2 });
            wwMatrix.AddAbility(new double[] { chanceOHProcs0, chanceOHProcs1, chanceOHProcs2 });
            chanceWWProcs0 = wwMatrix.ProcChances(0);
            chanceWWProcs1 = wwMatrix.ProcChances(1);
            chanceWWProcs2 = wwMatrix.ProcChances(2);*/

            // BT
            BTMatrix = new BloodSurgeMatrix();
            BTMatrix.AddAbility(BS1Proc, BS2Proc, BT.ability.MHAtkTable.AnyLand);
            //chanceBTProcs2 = BT.ability.MHAtkTable.AnyLand * BS2Proc;
            //chanceBTProcs1 = BT.ability.MHAtkTable.AnyLand * BS1Proc;
            //chanceBTProcs0 = 1 - chanceBTProcs1 - chanceBTProcs2;
        }

        public class StateFury : State<Skills.Ability>
        {
            public double BTCooldown;
            public double WWCooldown;
            public int BSProcced;
            public bool BSHasted;
            //public double WhiteCooldown;
        }

        protected override State<Skills.Ability> GetInitialState()
        {
            return GetState(0, false, 0, 0);
            //return GetState(0, false, 0, 0, WhiteInterval);
        }

        protected override List<StateTransition<Skills.Ability>> GetStateTransitions(State<Skills.Ability> state)
        {
            StateFury s = state as StateFury;
            List<StateTransition<Skills.Ability>> list = new List<StateTransition<Skills.Ability>>();

            Rawr.DPSWarr.Rotation.AbilWrapper BT = Rot.GetWrapper<Skills.BloodThirst>();
            Rawr.DPSWarr.Rotation.AbilWrapper WW = Rot.GetWrapper<Skills.WhirlWind>();
            Rawr.DPSWarr.Rotation.AbilWrapper BS = Rot.GetWrapper<Skills.BloodSurge>();
            Rawr.DPSWarr.Rotation.AbilWrapper HS = Rot.GetWrapper<Skills.HeroicStrike>();

            /*bool forceDelay = false;
            if (s.WWCooldown != 0 && s.BTCooldown != 0 && s.BSProcced > 0)
            {
                double slamDuration = (s.BSHasted ? HastedGCD : LatentGCD);
                if (s.WWCooldown < slamDuration)
                {
                    double WWtimeDelayed = slamDuration - s.WWCooldown;
                    double WWpercentLost = WWtimeDelayed / WW.ability.Cd;
                    double dmgLost = WW.ability.DamageOnUse * WWpercentLost;
                    double numHSesOnDelay = s.WWCooldown / WhiteInterval * HSPerc;
                    
                    if (s.BTCooldown < s.WWCooldown + LatentGCD) // if delaying lets us WW>BT
                    {
                        numHSesOnDelay = (s.WWCooldown + LatentGCD) / WhiteInterval * HSPerc;

                        double BTtimeDelayed = slamDuration + LatentGCD - s.BTCooldown;
                        double BTpercentLost = BTtimeDelayed / BT.ability.Cd;
                        dmgLost += BT.ability.DamageOnUse * BTpercentLost;    
                    }

                    double slamDmgGained = 0;
                }
            }*/

            if (s.WWCooldown <= 0)
            {
                #region Whirlwind
                double transDuration = LatentGCD;
                double newBTCD = Math.Max(0, s.BTCooldown - transDuration);
                double newWWCD = WW.ability.Cd - transDuration;
                
                double numHSes = transDuration / WhiteInterval * HSPerc;
                BloodSurgeMatrix mat = new BloodSurgeMatrix();
                mat.AddAbility(WWMatrix.ProcChances(1), WWMatrix.ProcChances(2));
                mat.AddAbility(HSMatrix.ProcChances(1), HSMatrix.ProcChances(2), numHSes);

                //double chance2Proc = chanceWWProcs2 + (1 - chanceWWProcs2) * (chanceHSProcs2 * numHSes);
                //double chance1Proc = chanceWWProcs0 * (chanceHSProcs1 * numHSes) +
                //                     chanceWWProcs1 * ((1 - chanceHSProcs2) * numHSes);
                //double chance0Proc = 1 - chance2Proc - chance1Proc;

                // if WW is up, use it
                if (mat.ProcChances(2) > 0)
                {
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = WW.ability,
                        TransitionDuration = transDuration,
                        TargetState = GetState(2, true, newBTCD, newWWCD),
                        TransitionProbability = mat.ProcChances(2)
                    });
                }
                // if BS procs 1
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = WW.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState((s.BSHasted ? 2 : 1), s.BSHasted, newBTCD, newWWCD),
                    TransitionProbability = mat.ProcChances(1)
                });

                // if BS doesn't proc
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = WW.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState(s.BSProcced, s.BSHasted, newBTCD, newWWCD),
                    TransitionProbability = mat.ProcChances(0)
                });
                #endregion
            }
            else if (s.BTCooldown <= 0)
            {
                #region BloodThirst
                double transDuration = LatentGCD;
                double newBTCD = BT.ability.Cd - transDuration;
                double newWWCD = Math.Max(0, s.WWCooldown - transDuration);

                double numHSes = transDuration / WhiteInterval * HSPerc;
                
                BloodSurgeMatrix mat = new BloodSurgeMatrix();
                mat.AddAbility(BTMatrix.ProcChances(1), BTMatrix.ProcChances(2));
                mat.AddAbility(HSMatrix.ProcChances(1), HSMatrix.ProcChances(2), numHSes);
                //double chance2Proc = chanceBTProcs2 + (1 - chanceBTProcs2) * (chanceHSProcs2 * numHSes);
                //double chance1Proc = chanceBTProcs0 * (chanceHSProcs1 * numHSes) +
                //                     chanceBTProcs1 * ((1 - chanceHSProcs2) * numHSes);
                //double chance0Proc = 1 - chance2Proc - chance1Proc;
                // if BS procs 2
                if (mat.ProcChances(2) > 0)
                {
                    list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                    {
                        Ability = BT.ability,
                        TransitionDuration = transDuration,
                        TargetState = GetState(2, true, newBTCD, newWWCD),
                        TransitionProbability = mat.ProcChances(2)
                    });
                }
                // if BS procs 1
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = BT.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState((s.BSHasted ? 2 : 1), s.BSHasted, newBTCD, newWWCD),
                    TransitionProbability = mat.ProcChances(1)
                });
                // if BS doesn't proc
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = BT.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState(s.BSProcced, s.BSHasted, newBTCD, newWWCD),
                    TransitionProbability = mat.ProcChances(0)
                });
                #endregion
            }

            else if (s.BSProcced > 0)
            {
                #region Slam

                double transDuration = (s.BSHasted ? HastedGCD : LatentGCD);
                double newBTCD = Math.Max(0, s.BTCooldown - transDuration);
                double newWWCD = Math.Max(0, s.WWCooldown - transDuration);

                double numHSes = transDuration / WhiteInterval * HSPerc;
                
                BloodSurgeMatrix mat = new BloodSurgeMatrix();
                mat.AddAbility(HSMatrix.ProcChances(1), HSMatrix.ProcChances(2), numHSes);
                
                //double chance2Proc = chanceHSProcs2 * numHSes;
                //double chance1Proc = chanceHSProcs1 * numHSes;
                //double chance0Proc = 1 - chance2Proc - chance1Proc;

                if (mat.ProcChances(2) > 0)
                {
                    list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                    {
                        Ability = BS.ability,
                        TransitionDuration = transDuration,
                        TargetState = GetState(2, true, newBTCD, newWWCD),
                        TransitionProbability = mat.ProcChances(2)
                    });
                }

                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = BS.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState(s.BSProcced, // if we had 2 and then procced, we're at 2.  If we had 1 and then procced, we're at 1
                                           (s.BSProcced == 2), // if we're at 2 before, we're still hasted.  If we're at 1 before, we lost the haste before we procced
                                           newBTCD, newWWCD),
                    TransitionProbability = mat.ProcChances(1)
                });

                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = BS.ability,
                    TransitionDuration = transDuration,
                    TargetState = GetState(s.BSProcced - 1, // num procs decreases
                                           s.BSHasted && s.BSProcced != 1, // if this is the last charge, haste falls off
                                           newBTCD, // BT CD
                                           newWWCD), // WW CD
                    TransitionProbability = mat.ProcChances(0)
                });
                #endregion
            }
            else
            {
                double transDuration = Math.Min(s.BTCooldown, s.WWCooldown);

                double numHSes = transDuration / WhiteInterval * HSPerc;
                
                BloodSurgeMatrix mat = new BloodSurgeMatrix();
                mat.AddAbility(HSMatrix.ProcChances(1), HSMatrix.ProcChances(2), numHSes);
                //double chance2Proc = chanceHSProcs2 * numHSes;
                //double chance1Proc = chanceHSProcs1 * numHSes;
                //double chance0Proc = 1 - chance2Proc - chance1Proc;
                
                if (mat.ProcChances(2) > 0)
                {
                    list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                    {
                        Ability = null,
                        TransitionDuration = transDuration,
                        TargetState = GetState(2, true, s.BTCooldown - transDuration, s.WWCooldown - transDuration),
                        TransitionProbability = mat.ProcChances(2)
                    });
                }
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = transDuration,
                    TargetState = GetState((s.BSHasted ? 2 : 1), s.BSHasted, s.BTCooldown - transDuration, s.WWCooldown - transDuration),
                    TransitionProbability = mat.ProcChances(1)
                });
                list.Add(new StateTransition<Rawr.DPSWarr.Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = transDuration,
                    TargetState = GetState(s.BSProcced, s.BSHasted, s.BTCooldown - transDuration, s.WWCooldown - transDuration),
                    TransitionProbability = mat.ProcChances(0)
                });
            }

            // Test to make sure probabilities aren't messed up
            double prob = 0;
            foreach (StateTransition<Skills.Ability> st in list)
            {
                prob += st.TransitionProbability;
                StateFury sf = (StateFury)st.TargetState;
                if (s.BSHasted == sf.BSHasted &&
                    s.BSProcced == sf.BSProcced &&
                    s.BTCooldown == sf.BTCooldown &&
                    //s.WhiteCooldown == sf.WhiteCooldown &&
                    s.WWCooldown == sf.WWCooldown)
                {
                    int j = 0; //break
                }
            }
            if (prob != 1)
            {
                int j = 0; //break
            }

            return list;
        }
        private Dictionary<string, StateFury> stateDictionary = new Dictionary<string, StateFury>();
        public StateFury GetState(int BSProc, bool BSHasted, double BTCD, double WWCD/*, double WhiteCD*/)
        {
            string name = string.Format(
                "BSProc {0}{1},BTCD {2:"+FORMAT+"},WWCD {3:"+FORMAT+"}, WhiteCD{4:"+FORMAT+"}",
                BSProc,
                BSHasted ? "+" : "-",
                BTCD,
                WWCD,
                0);//WhiteCD);
            StateFury state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new StateFury()
                {
                    Name = name,
                    BTCooldown = BTCD,
                    WWCooldown = WWCD,
                    BSHasted = BSHasted,
                    BSProcced = BSProc,
                    //WhiteCooldown = WhiteCD
                };
                stateDictionary[name] = state;
            }
            return state;
        }
        public StateFury GetState(int BSProc, bool BSHasted, float BTCD, float WWCD/*, float WhiteCD*/)
        {
            return GetState(BSProc, BSHasted, (double)BTCD, (double)WWCD/*, (double)WhiteCD*/);
        }
    }

    public class StateSpaceGeneratorFuryTest
    {
        public void StateSpaceGeneratorFuryTest1(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co)
        {
            FuryGenerator gen = new FuryGenerator(c, s, cf, wa, co);
            var stateSpace = gen.GenerateStateSpace();
            string output = "";
            foreach (State<Rawr.DPSWarr.Skills.Ability> a in stateSpace)
            {
                output += a.ToString() + "\n";
            }
            output += "\ndone";
            try
            {
                MarkovProcess<Skills.Ability> mp = new MarkovProcess<Skills.Ability>(stateSpace);

                double averageDamage = 0.0;
                foreach (KeyValuePair<Skills.Ability, double> kvp in mp.AbilityWeight)
                {
                    System.Console.WriteLine("{0} - {1}", kvp.Key.Name, kvp.Key.DamageOnUse * kvp.Value / mp.AverageTransitionDuration);
                    averageDamage += kvp.Key.DamageOnUse * kvp.Value;
                }
                double hsdps = gen.Rot.GetWrapper<Skills.HeroicStrike>().ability.DamageOnUse / wa.MhEffectiveSpeed;
                double ohdps = gen.Rot.WhiteAtks.OhDPS;
                System.Console.WriteLine("HS - {0}", hsdps);
                System.Console.WriteLine("OH - {0}", ohdps);
                double dps = averageDamage / mp.AverageTransitionDuration;
                dps += hsdps + ohdps;
            }
            catch (Exception ex)
            {
                new ErrorBox("Error in creating Arms Markov Calculations",
                    ex.Message, "StateSpaceGeneratorArmsTest1()",
                    "StateSpace Count: " + stateSpace.Count.ToString(),
                    ex.StackTrace);
            }
        }
    }
}
