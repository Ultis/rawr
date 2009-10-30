using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.DPSWarr.Markov
{
    public class MarkovProcessArms
    {
        public class Ability : Skills.Ability {
            public double TheDamage { get; set; }
            public double TheDuration { get; set; }
        }

        public class AbilitySequence : Ability
        {
            public AbilitySequence(params Ability[] abilities)
            {
                foreach (Ability ability in abilities)
                {
                    TheDamage += ability.DamageOnUse;
                    TheDuration += ability.Duration;
                }
            }
        }

        public void MarkovProcessTest1()
        {
            //S0:
            //A => S0 0.5
            //  => S1 0.5
            //S1:
            //B => S0 1

            Ability A = new Ability() { TheDamage = 100, Duration = 2 };
            Ability B = new Ability() { TheDamage = 200, Duration = 1 };

            State<Ability> S0 = new State<Ability>() { Name = "S0" };
            State<Ability> S1 = new State<Ability>() { Name = "S1" };

            S0.Transitions = new List<StateTransition<Ability>>()
            {
                new StateTransition<Ability>() { Ability = A, TargetState = S0, TransitionDuration = A.Duration, TransitionProbability = 0.5 },
                new StateTransition<Ability>() { Ability = A, TargetState = S1, TransitionDuration = A.Duration, TransitionProbability = 0.5 },
            };
            S1.Transitions = new List<StateTransition<Ability>>()
            {
                new StateTransition<Ability>() { Ability = B, TargetState = S0, TransitionDuration = B.Duration, TransitionProbability = 1.0 },
            };
            List<State<Ability>> stateSpace = new List<State<Ability>>() { S0, S1 };

            MarkovProcess<Ability> mp = new MarkovProcess<Ability>(stateSpace);

            //Assert.AreEqual(mp.StateWeight[0], 2.0 / 3.0, 0.000000000001, "S0");
            //Assert.AreEqual(mp.StateWeight[1], 1.0 / 3.0, 0.000000000001, "S1");
            //Assert.AreEqual(mp.AverageTransitionDuration, 5.0 / 3.0, 0.000000000001, "time");

            double averageDamage = 0.0;
            foreach (KeyValuePair<Ability, double> kvp in mp.AbilityWeight)
            {
                averageDamage += kvp.Key.TheDamage * kvp.Value;
            }
            //Assert.AreEqual(averageDamage, 100.0 * 4.0 / 3.0, 0.000000000001, "damage");

            //Assert.AreEqual(averageDamage / mp.AverageTransitionDuration, 80.0, 0.000000000001, "dps");
        }
    }
}
