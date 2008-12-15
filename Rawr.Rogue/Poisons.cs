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
	public abstract class PoisonBase
    {
		public abstract string Name { get; }
		public abstract bool IsDeadlyPoison { get; }
		public abstract float CalcPoisonDPS(RogueTalents talents, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits);
    }

	[Serializable]
	public class NoPoison : PoisonBase
    {
		public override string Name { get { return "None"; } }

		public override bool IsDeadlyPoison { get { return false; } }

		public override float CalcPoisonDPS(RogueTalents talents, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 0f;
        }
    }

	[Serializable]
	public class DeadlyPoison : PoisonBase
    {
		public override string Name { get { return "Deadly Poison"; } }

        private const float _stackSize = 5f;
        private const float _duration = 12f;

		public override bool IsDeadlyPoison { get { return true; } }

		public override float CalcPoisonDPS(RogueTalents talents, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return _stackSize * (296f + .08f * stats.AttackPower) * VilePoison.DamageMultiplier(talents) / _duration;
        }
    }

	[Serializable]
	public class InstantPoison : PoisonBase
    {
		public override string Name { get { return "Instant Poison"; } }

		public override bool IsDeadlyPoison { get { return false; } }

		public override float CalcPoisonDPS(RogueTalents talents, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return hits*combatFactors.ProbPoisonHit*(300f + .1f*stats.AttackPower)*VilePoison.DamageMultiplier(talents)*(.2f + ImprovedPoisons.IncreasedApplicationChance(talents));
        }
    }

	[Serializable]
	public static class VilePoison
    {
        private static readonly float[] _multipliers = new[] { 1f, 1.07f, 1.14f, 1.2f };
        public static float DamageMultiplier(RogueTalents talents)
        {
            return _multipliers[talents.VilePoisons];
        }
    }

	[Serializable]
	public static class ImprovedPoisons
    {
        public static float IncreasedApplicationChance(RogueTalents talents)
        {
            return talents.ImprovedPoisons*.02f;
        }
    }
}