using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class HonorAmongThieves : ComboPointGenerator
    {
        private readonly float _hemosPerCycle;
        private readonly float _cpsPersecond;

        public HonorAmongThieves():this(.1f, 0f){}
        public HonorAmongThieves(float cpsPersecond, float hemosPerCycle)
        {
            _hemosPerCycle = (hemosPerCycle < 0f ? 0f : hemosPerCycle);
            _cpsPersecond = (cpsPersecond < .1f ? .1f : cpsPersecond);
        }

        public override string Name
        {
            get { return "HAT"; }
        }

        public override float CalcCpgDps(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime)
        {
            return 0f;
        }

        public override float Crit( CombatFactors combatFactors, CalculationOptionsRogue calcOpts )
        {
            return 0;
        }

        public override float EnergyCost(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return new Hemo().EnergyCost(combatFactors, calcOpts) * _hemosPerCycle;
        }

        public override float CalcDuration(CalculationOptionsRogue calcOpts, float regen, CombatFactors combatFactors)
        {
            return (calcOpts.ComboPointsNeededForCycle() - _hemosPerCycle) /_cpsPersecond;
        }
    }
}
