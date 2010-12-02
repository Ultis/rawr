using System;
using System.Linq;
using System.Collections.Generic;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    public enum SequenceType
    {
        Burst,
        Sustained
    }

    public sealed class StateGenerator : StateSpaceGenerator<Spell>
    {
        private List<Spell> _AvailableSpells;
        private List<RestoShamState> _TargetStates;
        private SequenceType _SequenceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateGenerator"/> class.
        /// </summary>
        public StateGenerator(List<Spell> availableSpells, SequenceType sequence)
            : base()
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            _AvailableSpells = availableSpells;
            _TargetStates = new List<RestoShamState>();
            _SequenceType = sequence;
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
                    if (restoState.TidalWavesBuff > 0)
                    {
                        SpellState ss = null;
                        if (_SequenceType == SequenceType.Burst)
                            ss = restoState.GetNonPrioritySpellStates().Where(i => i.TrackedSpell.ConsumesTidalWaves).OrderByDescending(i => i.TrackedSpell.EPS).First();
                        else
                            ss = restoState.GetNonPrioritySpellStates().Where(i => i.TrackedSpell.ConsumesTidalWaves).OrderByDescending(i => i.TrackedSpell.EPM).First();

                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                    else
                    {
                        SpellState ss = null;
                        if (_SequenceType == SequenceType.Burst)
                            ss = restoState.GetNonPrioritySpellStates().OrderByDescending(i => i.TrackedSpell.EPS).First();
                        else
                            ss = restoState.GetNonPrioritySpellStates().OrderByDescending(i => i.TrackedSpell.EPM).First();

                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                    int transitionCount = transitions.Count;
                    if (transitionCount > 1)
                        transitions.ForEach(i => i.TransitionProbability = (double)(1d / (double)transitionCount));
                }
            }

            return transitions;
        }

        private State<Spell> CastSpell(RestoShamState restoState, SpellState ss)
        {
            RestoShamState next = new RestoShamState(restoState);
            next.CastSpell(ss.TrackedSpell.SpellId);

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
