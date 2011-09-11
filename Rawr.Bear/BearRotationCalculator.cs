using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Bear
{
    /// <summary>
    /// Provides interface for and calculates the Bear rotation
    /// </summary>
	public static class BearRotationCalculator
	{
		public static Tuple<BearRotationCalculation, BearRotationCalculation> GetOptimalRotations(BearAbilityBuilder abilities)
		{
			int highestDPSRotation = -1;
			float highestDPSRotationDPS = 0;
			float highestDPSRotationTPS = 0;
			int highestTPSRotation = -1;
			float highestTPSRotationDPS = 0;
			float highestTPSRotationTPS = 0;

			for (int i = 0; i < Rotations.Length; i++)
			{
				var rotation = Rotations[i];
				var dpsTps = EvaluateRotation(rotation.Item2, abilities);

				if (dpsTps.Item1 > highestDPSRotationDPS)
				{
					highestDPSRotation = i;
					highestDPSRotationDPS = dpsTps.Item1;
					highestDPSRotationTPS = dpsTps.Item2;
				}
				if (dpsTps.Item2 > highestTPSRotationTPS)
				{
					highestTPSRotation = i;
					highestTPSRotationDPS = dpsTps.Item1;
					highestTPSRotationTPS = dpsTps.Item2;
				}
			}

			return Tuple.Create(
				highestDPSRotation >= 0 ? new BearRotationCalculation()
				{
					Name = BuildRotationName(Rotations[highestDPSRotation].Item1, highestDPSRotationDPS, highestDPSRotationTPS),
					DPS = highestDPSRotationDPS,
					TPS = highestDPSRotationTPS
				} : new BearRotationCalculation() { Name = "None" },
				highestTPSRotation >= 0 ? new BearRotationCalculation()
				{
					Name = BuildRotationName(Rotations[highestTPSRotation].Item1, highestTPSRotationDPS, highestTPSRotationTPS),
					DPS = highestTPSRotationDPS,
					TPS = highestTPSRotationTPS
				} : new BearRotationCalculation() { Name = "None" }
			);
		}

		private static string BuildRotationName(List<BearAttack> rotation, float dps, float tps)
		{
			StringBuilder sbRotation = new StringBuilder();
			sbRotation.AppendFormat("{0:N1} DPS,  {1:N2} TPS*", dps, tps);
			int i = 0;
			foreach (BearAttack attack in rotation)
			{
				sbRotation.AppendFormat("{0}. ", ++i);
				switch (attack)
				{
					case BearAttack.DemoralizingDebuff:
						sbRotation.AppendLine("Demoralizing Roar is not up");
						break;
					case BearAttack.FFFDamage:
						sbRotation.AppendLine("Faerie Fire (Feral) if off cooldown");
						break;
					case BearAttack.MangleDebuff:
					case BearAttack.MangleDamage:
						sbRotation.AppendLine("Mangle if off cooldown");
						break;
					case BearAttack.LacerateDebuff:
						sbRotation.AppendLine("Lacerate if no Lacerate stacks");
						break;
					case BearAttack.LacerateStack:
						sbRotation.AppendLine("Lacerate if less than three Lacerate stacks");
						break;
					case BearAttack.LacerateDamage:
						sbRotation.AppendLine("Lacerate");
						break;
					case BearAttack.PulverizeBuff:
						sbRotation.AppendLine("Pulverize if three Lacerate stacks and no Pulverize buff");
						break;
					case BearAttack.PulverizeDamage:
						sbRotation.AppendLine("Pulverize if three Lacerate stacks");
						break;
					case BearAttack.ThrashDamage:
						sbRotation.AppendLine("Thrash if off cooldown");
						break;
					case BearAttack.SwipeDamage:
						sbRotation.AppendLine("Swipe if off cooldown");
						break;
				}
			}
			return sbRotation.ToString();
		}

		private static Tuple<float,float> EvaluateRotation(AbilityCounts counts, BearAbilityBuilder abilities)
		{
			float damage = 0f;
			float threat = 0f;

			damage += abilities.MeleeStats.DPSTotalAverage * RotationDuration;
			threat += abilities.MeleeStats.TPSTotalAverage * RotationDuration;
					  
			damage += abilities.MaulStats.DamageAverage * RotationDuration / 3f; //TODO: Limit this by rage; just assume you maul on cooldown for now
			threat += abilities.MaulStats.ThreatAverage * RotationDuration / 3f;
					  
			damage += abilities.FaerieFireStats.DamageAverage * counts.FFF;
			threat += abilities.FaerieFireStats.ThreatAverage * counts.FFF;
					  
			damage += abilities.LacerateStats.DamageAverage * counts.Lacerate;
			threat += abilities.LacerateStats.ThreatAverage * counts.Lacerate;
			damage += abilities.LacerateStats.DamageTick1Raw * counts.LacerateTick;
			threat += abilities.LacerateStats.ThreatTick1Raw * counts.LacerateTick;
					  
			damage += abilities.MangleStats.DamageAverage * counts.Mangle;
			threat += abilities.MangleStats.ThreatAverage * counts.Mangle;
					  
			damage += abilities.PulverizeStats.Damage3Average * counts.Pulverize;
			threat += abilities.PulverizeStats.Threat3Average * counts.FFF;
					  
			damage += abilities.SwipeStats.DamageAverage * counts.Swipe;
			threat += abilities.SwipeStats.ThreatAverage * counts.Swipe;
					  
			damage += abilities.ThrashStats.DamageAverage * counts.Thrash;
			threat += abilities.ThrashStats.ThreatAverage * counts.Thrash;

			return Tuple.Create(damage / RotationDuration, threat / RotationDuration);
		}

		public class BearRotationCalculation
		{
			public string Name { get; set; }
			public float DPS { get; set; }
			public float TPS { get; set; }
		}



		private const float RotationDuration = 300f;
		private static Tuple<List<BearAttack>, AbilityCounts>[] _rotations = null;
		public static Tuple<List<BearAttack>, AbilityCounts>[] Rotations
		{
			get
			{
				if (_rotations == null)
				{
					var rotations = new List<Tuple<List<BearAttack>, AbilityCounts>>();
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       59.4705f,   79.8364f,   16.2089f,   32.4951f,   0f, 10.9891f,   219.512f,  185.0927f, 197.7954f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       59.0508f,   63.6001f,   20.6567f,   44.7116f,   0f, 10.9808f,   153.7712f, 189.1796f, 197.8229f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,        BearAttack.LacerateDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(13.1713f, 59.669f,    53.0519f,   15.759f,    47.3573f,   0f, 10.9915f,   185.3848f, 179.8381f, 197.8076f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                                                          new AbilityCounts(1f,       59.1051f,   97.3475f,   31.8998f,   0f,         0f, 10.6476f,   156.0272f, 192.6106f, 195.6966f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,        BearAttack.LacerateDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(13.0473f, 59.0145f,   52.4794f,   16.8754f,   47.5952f,   0f, 10.9882f,   173.2029f, 184.4161f, 197.8257f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       59.1985f,   78.4518f,   25.5479f,   25.1334f,   0f, 10.6684f,   170.9039f, 192.6106f, 195.6604f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,        BearAttack.LacerateDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       59.5656f,   65.3556f,   15.7962f,   47.2897f,   0f, 10.9929f,   185.7582f, 180.1834f, 197.8281f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(15.063f,  59.777f,    53.6468f,   17.1891f,   43.368f,    0f, 10.9561f,   182.5431f, 184.5616f, 197.5604f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       58.763f,    84.0821f,   16.9192f,   28.4657f,   0f, 10.77f,     220.8326f, 190.469f,  196.4313f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(15.1538f, 60.0482f,   53.9492f,   17.3031f,   42.9775f,   0f, 10.5682f,   182.2697f, 184.4266f, 195.6288f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(19.467f,  58.5958f,   65.819f,    16.9775f,   28.1571f,   0f, 10.9836f,   221.1112f, 190.9246f, 197.8227f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       59.0362f,   77.8298f,   25.313f,    25.8461f,   0f, 10.9749f,   171.0373f, 191.5787f, 197.5416f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       59.9774f,   67.1876f,   15.5378f,   45.4782f,   0f, 10.819f,    193.668f,  178.7075f, 196.1632f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(17.986f,  59.3464f,   62.9878f,   16.1755f,   32.5182f,   0f, 10.9861f,   219.6991f, 184.8842f, 197.7931f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                                                          new AbilityCounts(1f,       58.7227f,   97.3523f,   31.9348f,   0f,         0f, 10.9902f,   156.2512f, 192.698f,  197.7419f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(21.0534f, 58.9111f,   62.8386f,   20.1569f,   26.0707f,   0f, 10.9693f,   202.3156f, 190.8209f, 197.6814f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(17.9018f, 59.6126f,   63.2814f,   16.1154f,   32.3136f,   0f, 10.7752f,   220.2126f, 184.8998f, 196.2099f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       58.7448f,   84.0711f,   16.9816f,   28.2155f,   0f, 10.987f,    221.123f,  190.8937f, 197.8522f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       59.4955f,   67.2811f,   15.604f,    45.6287f,   0f, 10.9907f,   191.7018f, 179.3489f, 197.8537f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(21.5241f, 58.6612f,   63.1521f,   20.3616f,   25.3316f,   0f, 10.9694f,   204.4262f, 192.6899f, 197.7295f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       58.9209f,   78.4737f,   25.5499f,   25.0705f,   0f, 10.985f,    171.5683f, 192.7027f, 197.7058f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       59.3888f,   77.709f,    25.2703f,   25.9001f,   0f, 10.7318f,   172.2207f, 191.1919f, 195.8421f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(21.4417f, 58.8336f,   62.9796f,   20.1632f,   25.7035f,   0f, 10.8784f,   204.024f,  192.5623f, 196.7316f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.PulverizeDamage }),  new AbilityCounts(21.1048f, 58.985f,    62.8169f,   20.046f,    26.1833f,   0f, 10.864f,    204.0086f, 190.5112f, 196.5947f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       59.479f,    63.5017f,   20.6181f,   44.5412f,   0f, 10.86f,     156.4739f, 188.9095f, 196.4161f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.LacerateDebuff,      BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(13.966f,  59.7203f,   54.2764f,   15.5767f,   45.4792f,   0f, 10.9814f,   191.9073f, 178.9438f, 197.8485f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.ThrashDamage,    BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(19.4209f, 58.8954f,   65.6749f,   16.9309f,   28.3185f,   0f, 10.7594f,   221.0829f, 190.4634f, 196.466f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.LacerateStack,   BearAttack.ThrashDamage,    BearAttack.PulverizeBuff,   BearAttack.LacerateDamage }),                               new AbilityCounts(1f,       59.425f,    80.3869f,   16.1149f,   32.3284f,   0f, 10.7448f,   220.2076f, 185.0985f, 196.1774f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.LacerateDebuff,      BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,    BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.FFFDamage,       BearAttack.LacerateDamage }),   new AbilityCounts(13.9547f, 59.9444f,   54.4296f,   15.4811f,   45.4002f,   0f, 10.79f,     194.2781f, 178.3004f, 196.2095f)));
					rotations.Add(Tuple.Create(new List<BearAttack>(new BearAttack[] { BearAttack.MangleDamage, BearAttack.DemoralizingDebuff,  BearAttack.ThrashDamage,        BearAttack.LacerateDebuff,  BearAttack.LacerateStack,   BearAttack.PulverizeBuff,   BearAttack.PulverizeDamage }),                              new AbilityCounts(1f,       58.4628f,   61.7501f,   20.0405f,   47.7597f,   0f, 10.9869f,   143.9962f, 187.8468f, 197.7783f)));
					_rotations = rotations.ToArray();
				}
				return _rotations;
			}
		}

		public enum BearAttack
		{
			DemoralizingDebuff = 0,
			FFFDamage = 1,
			MangleDebuff = 2,
			MangleDamage = 3,
			LacerateDebuff = 4,
			LacerateStack = 5,
			LacerateDamage = 6,
			PulverizeBuff = 7,
			PulverizeDamage = 8,
			ThrashDamage = 9,
			SwipeDamage = 10,
		}

		public class AbilityCounts
		{
			public float FFF;
			public float Mangle;
			public float Lacerate;
			public float Pulverize;
			public float Thrash;
			public float Swipe;
			public float Demoralizing;

			public float LacerateTick;
			public float PulverizeUptimeGCDs;
			public float DemoralizingUptimeGCDs;

			public AbilityCounts() { }
			public AbilityCounts(float fff, float mangle, float lacerate, float pulverize, float thrash, float swipe, float demoralizing,
				float lacerateTick, float pulverizeUptimeGCDs, float demoralizingUptimeGCDs)
			{
				FFF = fff;
				Mangle = mangle;
				Lacerate = lacerate;
				Pulverize = pulverize;
				Thrash = thrash;
				Swipe = swipe;
				Demoralizing = demoralizing;

				LacerateTick = lacerateTick;
				PulverizeUptimeGCDs = pulverizeUptimeGCDs;
				DemoralizingUptimeGCDs = demoralizingUptimeGCDs;
			}
		}
	}
}
