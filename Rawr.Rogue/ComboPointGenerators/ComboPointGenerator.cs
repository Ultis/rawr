using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    [XmlInclude(typeof(Mutilate))]
    [XmlInclude(typeof(Backstab))]
    [XmlInclude(typeof(SinisterStrike))]
    [XmlInclude(typeof(Hemo))]
    [XmlInclude(typeof(HonorAmongThieves))]
    public abstract class ComboPointGenerator 
    {
        public abstract string Name { get; }
        public abstract float CalcCpgDPS(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime);
        public abstract float Crit(CombatFactors combatFactors);

        public virtual float CalcDuration(CalculationOptionsRogue calcOpts, float regen, CombatFactors combatFactors)
        {
            return MhHitsNeeded(calcOpts.ComboPointsNeededForCycle()) * EnergyCost(combatFactors) / regen;
        }

        public virtual float MhHitsNeeded(float numCpg)
        {
            return numCpg / ComboPointsGeneratedPerAttack;    
        }

        public virtual float OhHitsNeeded(float numCpg)
        {
            return 0f;
        }

        public abstract float EnergyCost(CombatFactors combatFactors);
        protected virtual float ComboPointsGeneratedPerAttack
        {
            get { return 1; }
        }
    }

    public class ComboPointGeneratorList : List<ComboPointGenerator>
    {
        public ComboPointGeneratorList()
        {
            Add(new Mutilate());
            Add(new Backstab());
            Add(new SinisterStrike());
            Add(new Hemo());
            Add(new HonorAmongThieves(0f,0f));
        }

        public static ComboPointGenerator Get(string name)
        {
            foreach (var cpGenerator in new ComboPointGeneratorList())
            {
                if (name == cpGenerator.Name)
                {
                    return cpGenerator;
                }
            }
            throw new InvalidDataException("Cannot find: " + name);
        }
    }
}