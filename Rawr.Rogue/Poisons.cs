using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Rawr.Rogue
{
	public class PoisonList : List<PoisonBase>
    {
        public PoisonList()
        {
            Add(new NoPoison());
            Add(new DeadlyPoison());
            Add(new InstantPoison());
            Add(new WoundPoison());
        }

		public static PoisonBase Get(string poisonName)
        {
            foreach(var poison in new PoisonList())
            {
                if(poison.Name == poisonName)
                {
                    return poison;
                }
            }

            return new NoPoison();
        }
    }

	[Serializable]
	[XmlInclude(typeof(NoPoison))]
	[XmlInclude(typeof(DeadlyPoison))]
	[XmlInclude(typeof(InstantPoison))]
    [XmlInclude(typeof(WoundPoison))]
	public abstract class PoisonBase
    {
		public abstract string Name { get; }
		public abstract bool IsDeadlyPoison { get; }
		public abstract float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits);
    }

	[Serializable]
	public class NoPoison : PoisonBase
    {
		public override string Name { get { return "None"; } }

		public override bool IsDeadlyPoison { get { return false; } }

		public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 0f;
        }
    }

	[Serializable]
	public class DeadlyPoison : PoisonBase
    {
		public override string Name { get { return "Deadly Poison"; } }

        private const float STACK_SIZE = 5f;
        private const float DURATION = 12f;

		public override bool IsDeadlyPoison { get { return true; } }

		public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return STACK_SIZE * (296f + .12f * stats.AttackPower) * Talents.VilePoisons.Multiplier / DURATION;
        }
    }

	[Serializable]
	public class InstantPoison : PoisonBase
    {
		public override string Name { get { return "Instant Poison"; } }

		public override bool IsDeadlyPoison { get { return false; } }

		public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
			var damage = hits * (300f + .10f * stats.AttackPower);
			damage *= (.2f + Talents.ImprovedPoisons.InstantPoison.Bonus);
			damage *= combatFactors.ProbPoisonHit;
			damage *= Talents.VilePoisons.Multiplier;
			return damage;
        }
    }

    [Serializable]
    public class WoundPoison : PoisonBase
    {
        public override string Name { get { return "Wound Poison"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return hits * combatFactors.ProbPoisonHit * (231f + 0.04f * stats.AttackPower) * 0.5f * Talents.VilePoisons.Multiplier;
        }
    }
}