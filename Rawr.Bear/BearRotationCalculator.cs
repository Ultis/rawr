using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	public class BearRotationCalculator
	{
		public float MeleeDamage { get; set; }
		public float MaulDamage { get; set; }
		public float MangleDamage { get; set; }
		public float SwipeDamage { get; set; }
		public float FaerieFireDamage { get; set; }
		public float LacerateDamage { get; set; }
		public float LacerateDotDamage { get; set; }

		public float MeleeThreat { get; set; }
		public float MaulThreat { get; set; }
		public float MangleThreat { get; set; }
		public float SwipeThreat { get; set; }
		public float FaerieFireThreat { get; set; }
		public float LacerateThreat { get; set; }
		public float LacerateDotThreat { get; set; }

		public float MangleCooldown { get; set; }
		public float MeleeSpeed { get; set; }

		public BearRotationCalculator(float meleeDamage, float maulDamage, float mangleDamage, float swipeDamage, float faerieFireDamage,
			float lacerateDamage, float lacerateDotDamage, float meleeThreat, float maulThreat, float mangleThreat, float swipeThreat,
			float faerieFireThreat, float lacerateThreat, float lacerateDotThreat, float mangleCooldown, float meleeSpeed)
		{
			MeleeDamage = meleeDamage;
			MaulDamage = maulDamage;
			MangleDamage = mangleDamage;
			SwipeDamage = swipeDamage;
			FaerieFireDamage = faerieFireDamage;
			LacerateDamage = lacerateDamage;
			LacerateDotDamage = lacerateDotDamage;

			MeleeThreat = meleeThreat;
			MaulThreat = maulThreat;
			MangleThreat = mangleThreat;
			SwipeThreat = swipeThreat;
			FaerieFireThreat = faerieFireThreat;
			LacerateThreat = lacerateThreat;
			LacerateDotThreat = lacerateDotThreat;

			MangleCooldown = mangleCooldown;
			MeleeSpeed = meleeSpeed;
		}

		private enum BearAttack
		{
			None,
			Mangle,
			Swipe,
			FaerieFire,
			Lacerate
		}

		/// <summary>
		/// Gets 
		/// </summary>
		/// <param name="useMaul">null = no auto or maul, false = auto, true = maul</param>
		/// <param name="useMangle"></param>
		/// <param name="useSwipe"></param>
		/// <param name="useFaerieFire"></param>
		/// <param name="useLacerate"></param>
		public BearRotationCalculation GetRotationCalculations(bool? useMaul, bool useMangle, bool useSwipe, bool useFaerieFire, bool useLacerate)
		{
			//36 GCDs, 54 sec
			BearAttack[] attacks = new BearAttack[36];

			if (useMangle)
			{
				for (int i = 0; i < attacks.Length; i++)
				{
					if (attacks[i] == BearAttack.None)
					{
						attacks[i] = BearAttack.Mangle;
						i += (int)Math.Ceiling((float)MangleCooldown / 1.5f) - 1;
					}
				}
			}

			if (useFaerieFire)
			{
				for (int i = 0; i < attacks.Length; i++)
				{
					if (attacks[i] == BearAttack.None)
					{
						attacks[i] = BearAttack.FaerieFire;
						i += 3;
					}
				}
			}

			if (useLacerate)
			{
				for (int i = 0; i < attacks.Length; i++)
				{
					if (attacks[i] == BearAttack.None)
					{
						attacks[i] = BearAttack.Lacerate;
						if (useSwipe)
							i += 4;
					}
				}
			}

			if (useSwipe)
			{
				for (int i = 0; i < attacks.Length; i++)
				{
					if (attacks[i] == BearAttack.None)
					{
						attacks[i] = BearAttack.Swipe;
					}
				}
			}

			int countMangle = 0, countFaerieFire = 0, countLacerate = 0, countSwipe = 0, countLacerateDot = useLacerate ? 18 : 0;
			for (int i = 0; i < attacks.Length; i++)
			{
				switch (attacks[i])
				{
					case BearAttack.Mangle: countMangle++; break;
					case BearAttack.Swipe: countSwipe++; break;
					case BearAttack.FaerieFire: countFaerieFire++; break;
					case BearAttack.Lacerate: countLacerate++; break;
				}
			}

			float attackDPS = (countMangle * MangleDamage + countSwipe * SwipeDamage + countFaerieFire * FaerieFireDamage +
				countLacerate * LacerateDamage + countLacerateDot * LacerateDotDamage) / 54f;
			float attackTPS = (countMangle * MangleThreat + countSwipe * SwipeThreat + countFaerieFire * FaerieFireThreat +
							countLacerate * LacerateThreat + countLacerateDot * LacerateDotThreat) / 54f;

			if (useMaul.HasValue && !useMaul.Value)
			{ //Just melee attacks, no mauls
				attackDPS += (MeleeDamage / MeleeSpeed);
				attackTPS += (MeleeThreat / MeleeSpeed);
			}
			else if (useMaul.HasValue && useMaul.Value)
			{ //Use mauls
				attackDPS += (MaulDamage / MeleeSpeed);
				attackTPS += (MaulThreat / MeleeSpeed);
			}

			List<string> rotationName = new List<string>();
			if (useMaul.HasValue && !useMaul.Value) rotationName.Add("Melee");
			if (useMaul.HasValue && useMaul.Value) rotationName.Add("Maul");
			if (useMangle) rotationName.Add("Mangle");
			if (useSwipe) rotationName.Add("Swipe");
			if (useLacerate) rotationName.Add("Lacerate");
			if (useFaerieFire) rotationName.Add("FaerieFire");
			if (rotationName.Count == 0) rotationName.Add("None");

			return new BearRotationCalculation() { Name = string.Join("+", rotationName.ToArray()), DPS = attackDPS, TPS = attackTPS };
		}

		public class BearRotationCalculation
		{
			public string Name { get; set; }
			public float DPS { get; set; }
			public float TPS { get; set; }
		}
	}
}
