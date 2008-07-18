using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class Solver
    {
        private CalculationOptionsTree calcOpts;
        private CharacterCalculationsTree calculatedStats;
        private List<SpellRotation> baseSpellCycles;
        private List<SpellRotation> baseRotations;
        public SpellRotation bestRotation
        {
            get;
            private set;
        }
        public float FightFraction
        {
            get;
            private set;
        }
        public float InnervateMp5
        {
            get;
            private set;
        }
        public float MementoMp5
        {
            get;
            private set;
        }
        public float ManaPerCastMp5
        {
            get;
            private set;
        }
        public float HpS
        {
            get;
            private set;
        }
       
        public Solver(CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            this.calcOpts = calcOpts;
            this.calculatedStats = calculatedStats;
            baseSpellCycles = CalculateBaseCycles();
            if (baseSpellCycles.Count != 0)
            {
                baseRotations = CalculateBaseRotations();
                bestRotation = GetBestRotation();
                FightFraction = RotationMultiplier(bestRotation);
                InnervateMp5 = InnervateManaPerSecond(bestRotation) * 5f;
                MementoMp5 = MementoManaPerSecond(bestRotation) * 5f;
                ManaPerCastMp5 = ManaPerCastInManaPerSecond(bestRotation) * 5f;
                HpS = bestRotation.healPerCycle / bestRotation.bestCycleDuration * FightFraction +
                    // 1 proc / 50 sec
                    (calculatedStats.BasicStats.ShatteredSunRestoProc > 0 && calcOpts.ShattrathFaction == "Scryer" ? (618 + 682) / 2f * (1 + calculatedStats.BasicStats.SpellCrit/100f) / 50 : 0);
            }
        }

        private SpellRotation GetBestRotation()
        {
            SpellRotation bestRotation = null;
            float bestScore = 0f;
            foreach (SpellRotation rot in baseRotations)
            {
                float maxLength = calcOpts.MaxCycleDuration * calcOpts.NumCyclesPerRotation;
                float bestLocalScore = 0f;
                float granularity = 0.1f;

                rot.currentCycleDuration = ((float)Math.Ceiling(rot.tightCycleDuration * 10)) / 10f;

                // No use trying all combinations if we already have a better score than the theoretical max
                if (bestLocalScore > rot.healPerCycle / rot.tightCycleDuration)
                    continue;

                float multiplier;
                do
                {
                    multiplier = RotationMultiplier(rot);
                    float score = rot.healPerCycle / rot.currentCycleDuration * multiplier;

                    if (score > bestLocalScore)
                    {
                        bestLocalScore = score;
                        rot.bestCycleDuration = rot.currentCycleDuration;
                    }

                    rot.currentCycleDuration += granularity;
                } while (rot.currentCycleDuration < maxLength && multiplier < 1f);

                if (bestLocalScore > bestScore)
                {
                    bestRotation = rot;
                    bestScore = bestLocalScore;
                }
            }
            bestRotation.currentCycleDuration = bestRotation.bestCycleDuration;

            return bestRotation;
        }

        private float InnervateManaPerSecond(SpellRotation rot)
        {
            float innervateMana = 0f;
            if (calcOpts.InnervateSelf)
            {
                float manaGain = calculatedStats.OS5SRRegen * 5 * 20 - calculatedStats.IS5SRRegen * 20;
                float manaUsed = rot.manaPerCycle / rot.currentCycleDuration * 20;
                if (manaGain > calculatedStats.BasicStats.Mana + manaUsed)
                    manaGain = calculatedStats.BasicStats.Mana + manaUsed;
                innervateMana = manaGain / (calcOpts.InnervateDelay * 60);
            }
            return innervateMana;
        }

        private float MementoManaPerSecond(SpellRotation rot)
        {
            return calculatedStats.BasicStats.MementoProc * 3 / (45 + rot.currentCycleDuration / rot.numberOfSpells * 5);
        }

        private float ManaPerCastInManaPerSecond(SpellRotation rot)
        {
            return rot.numberOfSpells * calculatedStats.BasicStats.ManaRestorePerCast_5_15 * 0.05f / rot.currentCycleDuration;
        }

        private float RotationMultiplier(SpellRotation rot)
        {
            if (rot == null)
                return 0;

            float manaCostPerCycle = rot.manaPerCycle - rot.currentCycleDuration *
                (calculatedStats.Mp5Points / 5 +
                InnervateManaPerSecond(rot) +
                MementoManaPerSecond(rot) +
                ManaPerCastInManaPerSecond(rot));

            float HPSMultiplier = 1.0f;

            if (manaCostPerCycle > 0)
            {
                HPSMultiplier = (rot.currentCycleDuration * calculatedStats.BasicStats.Mana / manaCostPerCycle) / (calcOpts.FightLength * 60);
                if (HPSMultiplier > 1.0f)
                    HPSMultiplier = 1.0f;
            }

            return HPSMultiplier;
        }

        private List<SpellRotation> FilterSpellRotations(List<SpellRotation> spellRotations)
        {
            // If we can't keep the rotation up, there's no use keeping it
            spellRotations.RemoveAll(delegate(SpellRotation sr)
            {
                sr.currentCycleDuration = sr.maxCycleDuration;
                return RotationMultiplier(sr) < 0.7f;
            });

            // If something heals for both more per second and for less mana/cycle, there's no use keeping a rotation
            spellRotations.RemoveAll(delegate(SpellRotation sr2)
            {
                return spellRotations.Exists(delegate(SpellRotation sr)
                {
                    return sr2.manaPerCycle > sr.manaPerCycle && sr.healPerCycle > sr2.healPerCycle;
                });
            });

            List<SpellRotation> res = new List<SpellRotation>();

            // Remove duplicates (we get a lot due to being able to pick LB-RG-LB, LB-LB-RG, RG-LB-LB and so on)
            foreach (SpellRotation cycle in spellRotations)
            {
                if (!res.Exists(delegate(SpellRotation sr)
                {
                    return cycle.manaPerCycle == sr.manaPerCycle && cycle.healPerCycle == sr.healPerCycle;
                }))
                {
                    res.Add(cycle);
                }
            }

            return res;
        }

        private List<SpellRotation> CalculateBaseCycles()
        {
            Spell[][] spellList = new Spell[calcOpts.availableSpells.GetLength(0)][];

            for (int i = 0; i < calcOpts.availableSpells.GetLength(0); i++)
            {
                spellList[i] = new Spell[calcOpts.availableSpells[i].GetLength(0)];
                for (int j = 0; j < calcOpts.availableSpells[i].GetLength(0); j++)
                {
                    spellList[i][j] = calculatedStats.Spells.Find(delegate(Spell s) { return s.Name.Equals(calcOpts.availableSpells[i][j]); });
                }
            }

            List<SpellRotation> spellCyclesBeforeFilter = new List<SpellRotation>();

            foreach (List<Spell> spells in allCombinations(spellList))
            {
                spellCyclesBeforeFilter.Add(new SpellRotation(spells, calcOpts.MaxCycleDuration));
            }

            return FilterSpellRotations(spellCyclesBeforeFilter);
        }

        private List<List<Spell>> allCombinations(Spell[][] spellList)
        {
            List<List<Spell>> cur = new List<List<Spell>>();

            for (int j = 0; j < spellList[0].GetLength(0); j++)
            {
                List<Spell> l = new List<Spell>();
                l.Add(spellList[0][j]);
                cur.Add(l);
            }

            for (int i = 1; i < spellList.GetLength(0); i++)
            {
                List<List<Spell>> newList = new List<List<Spell>>();

                for (int j = 0; j < spellList[i].GetLength(0); j++)
                {
                    foreach (List<Spell> l in cur)
                    {
                        List<Spell> nl = new List<Spell>(l);
                        nl.Add(spellList[i][j]);
                        newList.Add(nl);
                    }
                }
                cur = newList;
            }

            return cur;
        }

        private List<SpellRotation> CalculateBaseRotations()
        {
            List<SpellRotation> possibleRotations = new List<SpellRotation>();
            possibleRotations.Add(new SpellRotation(new List<Spell>(), 0f));

            // Builds up the possible spell rotations by adding one spell at a time, and then filtering out the bad rotations
            for (int i = 0; i < calcOpts.NumCyclesPerRotation; i++)
            {
                List<SpellRotation> tmpPossibleRotations = new List<SpellRotation>();
                foreach (SpellRotation sr in possibleRotations)
                {
                    foreach (SpellRotation cycle in baseSpellCycles)
                    {
                        List<SpellRotation> tmpList = new List<SpellRotation>();
                        tmpList.Add(sr);
                        tmpList.Add(cycle);
                        tmpPossibleRotations.Add(new SpellRotation(tmpList, calcOpts.MaxCycleDuration * (i + 1), i + 1));
                    }
                }

                possibleRotations = FilterSpellRotations(tmpPossibleRotations);
            }

            return possibleRotations;
        }
    }
}
