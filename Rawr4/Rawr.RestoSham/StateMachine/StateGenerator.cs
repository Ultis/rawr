using System;
using System.Linq;
using System.Collections.Generic;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    public sealed class StateGenerator : StateSpaceGenerator<Spell>
    {
        private List<Spell> _AvailableSpells;
        private List<RestoShamState> _TargetStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateGenerator"/> class.
        /// </summary>
        public StateGenerator(List<Spell> availableSpells)
            : base()
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            _AvailableSpells = availableSpells;
            _TargetStates = new List<RestoShamState>();
        }

        protected override State<Spell> GetInitialState()
        {
            if (_TargetStates.Count == 0)
                _TargetStates.Add(new RestoShamState(_AvailableSpells));

            return _TargetStates.FirstOrDefault(i => i.Name == "INITIAL");
        }

        protected override List<StateTransition<Spell>> GetStateTransitions(State<Spell> state)
        {
            RestoShamState restoState = state as RestoShamState;
            List<StateTransition<Spell>> transitions = new List<StateTransition<Spell>>();

            lock (state)
            {
                bool prioritySpellCast = false;
                foreach (SpellState ss in restoState.GetPrioritySpellStates())
                {
                    if (!ss.IsActive && !ss.IsOnCooldown)
                    {
                        // cast this spell
                        prioritySpellCast = true;
                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                        break;
                    }
                }

                if (!prioritySpellCast)
                {
                    // Create a transition for each "spammable" spell
                    foreach (SpellState ss in restoState.GetNonPrioritySpellStates())
                    {
                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                }
            }

            return transitions;
        }

        private State<Spell> CastSpell(RestoShamState restoState, SpellState ss)
        {
            RestoShamState next = new RestoShamState(restoState);
            if (ss.TrackedSpell.ProvidesTidalWaves)
                next.TidalWavesBuff = 2;
            else if (ss.TrackedSpell.ConsumesTidalWaves && restoState.TidalWavesBuff > 0)
                next.TidalWavesBuff = restoState.TidalWavesBuff - 1;

            if (ss.TrackedSpell.TemporaryBuffs != null && ss.TrackedSpell.TemporaryBuffs.Count > 0)
                next.TemporaryBuffs.AddRange(ss.TrackedSpell.TemporaryBuffs);

            next.CastSpell(ss.TrackedSpell.SpellId, ss.TrackedSpell.CastTime);

            // Does this state already exist?
            lock (_TargetStates)
            {
                foreach (RestoShamState rss in _TargetStates.Where(i => i.TidalWavesBuff == next.TidalWavesBuff && i.TemporaryBuffs.Count == next.TemporaryBuffs.Count))
                {
                    if (rss.CooldownsMatch(next))
                        return rss;
                }
                _TargetStates.Add(next);
            }
            return next;
        }
    }
}
