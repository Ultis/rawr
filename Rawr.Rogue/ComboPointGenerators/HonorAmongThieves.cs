using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class HonorAmongThieves : ComboPointGenerator
    {
        private readonly float _cpsPersecond;

        public HonorAmongThieves():this(.1f){}
        public HonorAmongThieves(float cpsPersecond)
        {
            _cpsPersecond = (cpsPersecond < .1f ? .1f : cpsPersecond);
        }

        public override string Name
        {
            get { return "HAT"; }
        }

        public override float CalcCpgDPS(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime)
        {
            return 0f;
        }

        public override float Crit(CombatFactors combatFactors)
        {
            return 0;
        }

        protected override float EnergyCost(CombatFactors combatFactors)
        {
            return 0;
        }

        public override float CalcDuration(CalculationOptionsRogue calcOpts, float regen, CombatFactors combatFactors)
        {
            return calcOpts.ComboPointsNeededForCycle() /_cpsPersecond;
        }
    }
}
