using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class Solver
    {
        private DruidTalents talents;
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
        public float ManaProcOnCastMp5
        {
            get;
            private set;
        }
        public float LessMana_15s_1m_Mp5
        {
            get;
            private set;
        }
        public float BlueDragonMp5
        {
            get;
            private set;
        }
        public float BangleMp5
        {
            get;
            private set;
        }
        public float HpS
        {
            get;
            private set;
        }
       
        public Solver(DruidTalents talents, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            this.talents = talents;
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
                ManaProcOnCastMp5 = ManaPerCastInManaPerSecond(bestRotation) * 5f;
                LessMana_15s_1m_Mp5 = LessManaPerCastInManaPerSecond(bestRotation) * 5f;
                BlueDragonMp5 = BlueDragonInManaPerSecond(bestRotation) * 5f;
                BangleMp5 = BangleInManaPerSecond(bestRotation) * 5f;
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
            float secondsPerSpell = rot.currentCycleDuration / rot.numberOfSpells;
            // IED 5% proc-rate, 15 sec cooldown
            // Source: http://elitistjerks.com/f31/t19181-shaman_how_heal_like_pro/p53/#post759589
            float secondsPerProc = secondsPerSpell * 20f + 15f;
            return calculatedStats.BasicStats.ManaRestorePerCast_5_15  / secondsPerProc;
        }

        private float LessManaPerCastInManaPerSecond(SpellRotation rot)
        {
            float secondsPerSpell = rot.currentCycleDuration / rot.numberOfSpells;
            return calculatedStats.BasicStats.ManacostReduceWithin15OnUse1Min / (4 * secondsPerSpell);
        }

        private float BlueDragonInManaPerSecond(SpellRotation rot)
        {
            // Darkmoon Card: Blue Dragon - let's assume no internal cooldown, no proc while the effect is up and no proc during innervate
            float secondsPerSpell = rot.currentCycleDuration / rot.numberOfSpells;
            float secondsPerProc = 100f * secondsPerSpell / calculatedStats.BasicStats.FullManaRegenFor15SecOnSpellcast + 15f;
            return 15f * (calculatedStats.OS5SRRegen - calculatedStats.IS5SRRegen) / secondsPerProc;
        }

        private float BangleInManaPerSecond(SpellRotation rot)
        {
            // Bangle of Endless Blessings - x% mana regen for 15 sec with 10% proc, 45 sec internal cooldown
            float secondsPerSpell = rot.currentCycleDuration / rot.numberOfSpells;
            float secondsPerProc = secondsPerSpell / 0.1f + 45f;
            return 0.15f * calculatedStats.BasicStats.BangleProc * calculatedStats.OS5SRRegen / secondsPerProc;
        }

        private float RotationMultiplier(SpellRotation rot)
        {
            if (rot == null)
                return 0;

            float manaCostPerCycle = rot.manaPerCycle - rot.currentCycleDuration *
                (calculatedStats.Mp5Points / 5 +
                InnervateManaPerSecond(rot) +
                MementoManaPerSecond(rot) +
                LessManaPerCastInManaPerSecond(rot) +
                ManaPerCastInManaPerSecond(rot) +
                BlueDragonInManaPerSecond(rot) +
                BangleInManaPerSecond(rot)
                );

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

            // Filter out rotations that have HPS=0
            spellRotations.RemoveAll(delegate(SpellRotation sr)
            {
                return sr.healPerCycle == 0;
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
                    if (talents.TreeOfLife > 0 && spellList[i][j] is HealingTouch)
                        spellList[i][j] = calculatedStats.Spells.Find(delegate(Spell s) { return s.Name.Equals("Nothing"); });
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
