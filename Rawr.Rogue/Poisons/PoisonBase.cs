using System;
using System.Xml.Serialization;

namespace Rawr.Rogue.Poisons
{
#if (SILVERLIGHT == false)
    [Serializable]
    [XmlInclude(typeof(NoPoison))]
    [XmlInclude(typeof(DeadlyPoison))]
    [XmlInclude(typeof(InstantPoison))]
    [XmlInclude(typeof(WoundPoison))]
#endif
    public abstract class PoisonBase
    {
        public abstract string Name { get; }
        public abstract bool IsDeadlyPoison { get; }
        public abstract float CalcPoisonDps(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, CycleTime cycleTime, Item weapon);

        public static float CalcPoisonDps(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, WhiteAttacks whiteAttacks, CharacterCalculationsRogue calculatedStats, CycleTime cycleTime)
        {
            var mhPoisonDps = calcOpts.TempMainHandEnchant.CalcPoisonDps(stats, calcOpts, combatFactors, whiteAttacks.MhHits + calcOpts.CpGenerator.MhHitsNeeded(combatFactors, calcOpts), cycleTime, combatFactors.MH);
            calculatedStats.AddToolTip(DisplayValue.PoisonDps, "MH Poison DPS: " + Math.Round(mhPoisonDps, 2));

            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison && calcOpts.TempOffHandEnchant.IsDeadlyPoison)
            {
                return mhPoisonDps;
            }

            var ohPoisonDps = calcOpts.TempOffHandEnchant.CalcPoisonDps(stats, calcOpts, combatFactors, whiteAttacks.OhHits, cycleTime, combatFactors.OH);
            calculatedStats.AddToolTip(DisplayValue.PoisonDps, "OH Poison DPS: " + Math.Round(ohPoisonDps, 2));

            return mhPoisonDps + ohPoisonDps;
        }
    }
}