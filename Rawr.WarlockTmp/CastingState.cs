using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {
    
    public class CastingState {

        public CharacterCalculationsWarlock Mommy;
        public float Elapsed;
        public float Probability;
        public Dictionary<Spell, float> Cooldowns;
        public List<int> SeriesPriorities;
        public List<float> SeriesTimes;

        public CastingState(CharacterCalculationsWarlock mommy) {

            Mommy = mommy;
            Elapsed = mommy.AvgTimeUsed / 2f;
            Probability = 1f;
            Cooldowns = new Dictionary<Spell, float>();
            SeriesPriorities = new List<int>();
            SeriesTimes = new List<float>();
        }

        public CastingState(CastingState toCopy) {

            Mommy = toCopy.Mommy;
            Probability = toCopy.Probability;
            Cooldowns = new Dictionary<Spell, float>(toCopy.Cooldowns);
            SeriesPriorities = new List<int>(toCopy.SeriesPriorities);
            SeriesTimes = new List<float>(toCopy.SeriesTimes);
        }

        public float GetAvgTimeSinceQueued(Spell spell) {

            if (Cooldowns.ContainsKey(spell)) {
                return -Cooldowns[spell] / 2f;
            }

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
            SeriesTimes.Add(timeAdvance);
            SeriesPriorities.Add(Mommy.Priorities.IndexOf(spell));
            foreach (Spell key in new List<Spell>(Cooldowns.Keys)) {
                Cooldowns[key] -= timeAdvance;
            }
        }
    }
}
