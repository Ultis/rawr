using System;
using System.Collections.Generic;
using System.Linq;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    public sealed class RestoShamState : State<Spell>
    {
        private Guid _StateId = Guid.NewGuid();

        public int TidalWavesBuff { get; set; }
        public List<SpellState> SpellStates { get; private set; }
        public List<TemporaryBuff> TemporaryBuffs { get; private set; }

        public SpellState GetSpellState(int spellId)
        {
            return SpellStates.FirstOrDefault(i => i.TrackedSpell.SpellId == spellId);
        }

        public IOrderedEnumerable<SpellState> GetPrioritySpellStates()
        {
            return SpellStates.Where(i => i.HasHot || i.HasCooldown).OrderBy(i => i.Priority);
        }

        public IEnumerable<SpellState> GetNonPrioritySpellStates()
        {
            return SpellStates.Where(i => !i.HasHot && !i.HasCooldown && i.TrackedSpell is HealingSpell);
        }

        public RestoShamState(List<Spell> availableSpells)
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            Index = 0;
            Name = "INITIAL";
            TidalWavesBuff = 0;
            TemporaryBuffs = new List<TemporaryBuff>();
            SpellStates = new List<SpellState>(availableSpells.Count);

            for (int i = 0; i < availableSpells.Count; i++)
            {
                SpellStates.Add(new SpellState(availableSpells[i], (byte)i));
            }
        }

        public RestoShamState(RestoShamState parentState)
        {
            TemporaryBuffs = new List<TemporaryBuff>();
            SpellStates = new List<SpellState>(parentState.SpellStates.Count);
            TidalWavesBuff = parentState.TidalWavesBuff;

            foreach (SpellState ss in parentState.SpellStates)
            {
                SpellStates.Add(ss.Clone());
            }
        }

        internal void CastSpell(int spellId)
        {
            Spell spellCastObject = GetModifiedSpell(spellId);
            
            if (spellCastObject.ConsumesTidalWaves && TidalWavesBuff > 0)
                TidalWavesBuff--;
            else if (spellCastObject.ProvidesTidalWaves)
                TidalWavesBuff = 2;
            
            foreach (SpellState ss in SpellStates)
            {
                if (ss.TrackedSpell.SpellId == spellId)
                {
                    if (ss.TrackedSpell.Instant)
                    {
                        ss.Cast();
                        ss.Advance(spellCastObject.CastTime);
                    }
                    else
                    {
                        ss.Advance(spellCastObject.CastTime);
                        ss.Cast();
                    }
                }
                else
                    ss.Advance(spellCastObject.CastTime);
            }
        }

        internal Spell GetModifiedSpell(int spellId)
        {
            SpellState castSpell = GetSpellState(spellId);
            Spell spellCastObject = castSpell.TrackedSpell.Clone();

            if (spellCastObject.TemporaryBuffs != null && spellCastObject.TemporaryBuffs.Count > 0)
                TemporaryBuffs.AddRange(spellCastObject.TemporaryBuffs);

            if (spellCastObject.ConsumesTidalWaves && TidalWavesBuff > 0)
            {
                if (spellCastObject.TidalWavesBuff.Stats.SpellHaste > 0f)
                    spellCastObject.CastTimeReduction += (spellCastObject.TidalWavesBuff.Stats.SpellHaste * spellCastObject.CastTime);
                else if (spellCastObject.TidalWavesBuff.Stats.SpellCrit > 0f)
                    spellCastObject.CritRateModifier += spellCastObject.TidalWavesBuff.Stats.SpellCrit;

                spellCastObject.Calculate();
            }
            return spellCastObject;
        }

        internal bool CooldownsMatch(RestoShamState next)
        {
            foreach (SpellState ss in GetPrioritySpellStates())
            {
                SpellState st = next.GetSpellState(ss.TrackedSpell.SpellId);
                if (st != ss)
                    return false;
            }
            return true;
        }
    }
}
