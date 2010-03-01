using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public class WarlockSpell {

        public String Name { get; private set; }
        public float ManaCost { get; private set; }
        public float CastTime { get; private set; }
        public float CritChance { get; private set; }

        public float DamageOnHit { get; private set; }
        public float CritDamageOnHit { get; private set; }

        public float DamageOnTick { get; private set; }
        public int NumTicks { get; set; }

        public float NumCasts { get; set; }

        private CharacterCalculationsWarlock Calculations;

        public WarlockSpell(
            CharacterCalculationsWarlock calculations,
            String name,
            float percentBaseMana,
            float baseMana,
            float costMultiplier,
            float baseCastTime,
            float hasteDivisor,
            float critChance) {

            Calculations = calculations;
            Name = name;
            // TODO factor in "mana cost reduction" proc trinket(s?)
            // TODO factor in mana restore procs (as cost reduction)
            ManaCost = baseMana * percentBaseMana * costMultiplier;
            CastTime = Math.Max(1, baseCastTime / hasteDivisor);
            CritChance = critChance;
        }

        public void SetAverageDamageOnHit(
            float spellPower,
            float lowBase,
            float highBase,
            float coefficient,
            float multiplier,
            float critMultiplier) {

            DamageOnHit 
                = multiplier 
                    * ((lowBase + highBase) / 2f + coefficient * spellPower);
            CritDamageOnHit = DamageOnHit * critMultiplier;
        }

        public void SetNumCasts(
            float fightLength, float timeRemaining, float delayPerSpell) {

            // TODO factor in estimated collisions
            NumCasts = timeRemaining / (CastTime + delayPerSpell);
        }

        public float GetAvgDamagePerCast() {

            float hitChance
                = Calculations.Options.GetBaseHitRate() / 100f
                    + Calculations.TotalStats.SpellHit;
            return hitChance 
                * Utilities.GetWeightedSum(
                    CritDamageOnHit, CritChance, DamageOnHit, 1 - CritChance);
        }
    }
}
