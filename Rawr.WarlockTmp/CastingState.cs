using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.WarlockTmp {
    
    public class CastingState {

        public CharacterCalculationsWarlock Mommy;
        public float Elapsed;
        public float Probability;
        public Dictionary<Spell, float> Cooldowns;
        public List<Spell> Series;
        public List<int> SeriesPriorities;
        public List<float> SeriesTimes;

        public Dictionary<string, object> ExtraState;

        public CastingState(CharacterCalculationsWarlock mommy, Spell filler) {

            Mommy = mommy;
            Elapsed = filler.GetAvgTimeUsed() / 2f;
            Probability = 1f;
            Cooldowns = new Dictionary<Spell, float>();
            SeriesPriorities = new List<int>();
            Series = new List<Spell>();
            SeriesTimes = new List<float>();
            ExtraState = new Dictionary<string, object>();
        }

        public CastingState(CastingState toCopy) {

            Mommy = toCopy.Mommy;
            Probability = toCopy.Probability;
            Cooldowns = new Dictionary<Spell, float>(toCopy.Cooldowns);
            SeriesPriorities = new List<int>(toCopy.SeriesPriorities);
            Series = new List<Spell>(toCopy.Series);
            SeriesTimes = new List<float>(toCopy.SeriesTimes);
            ExtraState = new Dictionary<string, object>(toCopy.ExtraState);
        }

        public float GetAvgTimeSinceQueued(Spell spell) {

            if (Cooldowns.ContainsKey(spell)) {
                Debug.Assert(Cooldowns[spell] <= 0);
                return -Cooldowns[spell];
            }

            // This method answers the qusetion, "My spell is being cast now, so
            // how long has it been since it queued up to be cast?"
            // 
            // We know if there are lower priority spells that have already been
            // cast, it was not queued before that.  So we backtrack until we
            // find a lower priority spell.  We know it could have come off
            // cooldown anywhere during that period, so on average, take that
            // time & divide by 2.
            // 
            // Lower index == higher priority.

            float t = 0;
            int newPriority = Mommy.Priorities.IndexOf(spell);
            for (int i = SeriesTimes.Count; --i >= 0; ) {
                t += SeriesTimes[i];
                if (SeriesPriorities[i] > newPriority) {
                    return t / 2f;
                }
            }
            return Elapsed / 2f;
        }

        public void AddSpell(Spell spell, float timeAdvance) {

            Elapsed += timeAdvance;
            Series.Add(spell);
            SeriesTimes.Add(timeAdvance);
            SeriesPriorities.Add(Mommy.Priorities.IndexOf(spell));
            foreach (Spell key in new List<Spell>(Cooldowns.Keys)) {
                Cooldowns[key] -= timeAdvance;
            }
        }

        public string ToString() {

            string str = string.Format("{0:0.00000}:", Probability);
            foreach (Spell spell in Series) {
                str += " " + spell.GetType().Name;
            }
            return str;
        }
    }
}
