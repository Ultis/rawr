using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class SpellRotation
    {
        public SpellRotation(List<Spell> spells, float maxCycleDuration)
        {
            float sum = 0f;
            this.spells = new List<Spell>();
            foreach (Spell s in spells)
            {
                if (sum + s.CastTime <= maxCycleDuration)
                {
                    sum += s.CastTime;
                    this.spells.Add(s);
                }
            }

            this.maxCycleDuration = maxCycleDuration;
        }

        public float currentCycleDuration
        {
            get;
            set;
        }

        public float bestCycleDuration
        {
            get;
            set;
        }

        public float maxCycleDuration
        {
            get;
            private set;
        }

        private List<Spell> spells;

        public String cycleSpells
        {
            get
            {
                String sum = "";
                foreach (Spell s in spells)
                {
                    if (sum.Length != 0)
                        sum += ", ";
                    sum += s.Name;
                }
                return sum;
            }
        }

        public int numberOfSpells
        {
            get
            {
                return spells.Count;
            }
        }

        public float tightCycleDuration
        {
            get
            {
                float sum = 0f;
                foreach (Spell s in spells)
                {
                    sum += s.CastTime;
                }
                return sum;
            }
        }

        public float manaPerCycle
        {
            get
            {
                float sum = 0f;
                foreach (Spell s in spells)
                {
                    sum += s.Cost;
                }
                return sum;
            }
        }

        public float healPerCycle
        {
            get
            {
                float sum = 0f;
                foreach (Spell s in spells)
                {
                    // Stacked lifebloom is the only heal expected not to run out
                    // All other heals are expected to run out, providing their maximum possible heal
                    if (s is LifebloomStack)
                        sum += s.PeriodicTick * currentCycleDuration;
                    else
                        sum += s.AverageTotalHeal;
                }
                return sum;
            }
        }

        public float HPM
        {
            get
            {
                return healPerCycle / manaPerCycle;
            }
        }
    }
}
