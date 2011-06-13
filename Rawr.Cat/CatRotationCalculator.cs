using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Cat
{
	public class CatRotationCalculator
	{
		private CatAbilityBuilder Abilities { get; set; }
		private int FightDuration { get; set; }
		private MangleUsage MangleUsage { get; set; }

		private BerserkStats BerserkStats { get { return Abilities.BerserkStats; } }
		private BiteStats BiteStats { get { return Abilities.BiteStats; } }
		private MangleStats MangleStats { get { return Abilities.MangleStats; } }
		private MeleeStats MeleeStats { get { return Abilities.MeleeStats; } }
		private RakeStats RakeStats { get { return Abilities.RakeStats; } }
		private RavageStats RavageStats { get { return Abilities.RavageStats; } }
		private RipStats RipStats { get { return Abilities.RipStats; } }
		private RoarStats RoarStats { get { return Abilities.RoarStats; } }
		private ShredStats ShredStats { get { return Abilities.ShredStats; } }
		private TigersFuryStats TigersFuryStats { get { return Abilities.TigersFuryStats; } }

		private float[] _chanceExtraCP = new float[5];
		public CatRotationCalculator(CatAbilityBuilder abilities, int fightDuration, MangleUsage mangleUsage)
		{
			Abilities = abilities;
			FightDuration = fightDuration;
			MangleUsage = mangleUsage;

			float chanceExtraCPPerHit = abilities.ShredStats.ComboPointsGenerated - 1f;
			float c = chanceExtraCPPerHit, h = (1f - chanceExtraCPPerHit);
			_chanceExtraCP[0] = c;
			_chanceExtraCP[1] = c * h;
			_chanceExtraCP[2] = c * c + c * h * h;
			_chanceExtraCP[3] = 2 * c * c * h + c * h * h * h;
			_chanceExtraCP[4] = c * c * c + 3 * c * c * h * h + c * h * h * h * h;
		}

		public float PercentOfTimeAbove80Percent = 0.17f; //TODO: Make this dynamic/customizable
		public float PercentOfTimeBelow25Percent = 0.22f; //TODO: Make this dynamic/customizable

		public CatRotationCalculation GetOptimalRotation()
		{
			CatRotationCalculation highestDPSRotation = new CatRotationCalculation();
			for (int roarCP = 1; roarCP <= 5; roarCP++)
				for (int poolBites = 0; poolBites <= (BiteStats.MaxExtraEnergy == 0 ? 1 : 2); poolBites++)
				{
					var rotation = CalculateRotation(roarCP, (BiteUsage)poolBites);
					if (rotation.DPS > highestDPSRotation.DPS)
						highestDPSRotation = rotation;
				}

			return highestDPSRotation;
		}

		public float EnergyRemaining = 0f;
		public float EnergyCostMultiplier = 0f;
		public float CPRemaining = 0f;
		public float MangleCount = 0f;
		public float RakeInitCount = 0f;
		public float RakeTickCount = 0f;
		public float RipTickCount = 0f;
		public float ShredCount = 0f;
		public float BiteCount = 0f;
		public float RavageCount = 0f;
		public float RavageAbove80PercentCount = 0f;

		public CatRotationCalculation CalculateRotation(int roarCP, BiteUsage biteUsage)
		{
			float totalEnergy =
				100f + //Starting Energy
				FightDuration * MeleeStats.EnergyRegenPerSecond + //Passive Regen
				(FightDuration / MeleeStats.AttackSpeed) * MeleeStats.ClearcastChance * 40f + //Clearcast Energy
				((FightDuration - 10f) / TigersFuryStats.Cooldown) * (TigersFuryStats.EnergyGenerated + TigersFuryStats.MaxEnergyIncrease / 4f) + //TF Energy
				((FightDuration - 10f) / BerserkStats.Cooldown) * (TigersFuryStats.MaxEnergyIncrease / 4f); //Berserk Energy
			float tfUptime = ((((FightDuration - 10f) / TigersFuryStats.Cooldown) * TigersFuryStats.Duration) / FightDuration);
			float berserkUptime = ((((FightDuration - 10f) / BerserkStats.Cooldown) * BerserkStats.Duration) / FightDuration);
			float energyCostMultiplier = 1f - (1f - BerserkStats.EnergyCostMultiplier) * berserkUptime; //Average energy cost reduction due to Berserk
			float damageMultiplier = 1f + TigersFuryStats.DamageMultiplier * tfUptime;

			//Usage Counts
			MangleCount = 0f;
			RakeInitCount = 0f;
			RakeTickCount = 0f;
			RipTickCount = 0f;
			ShredCount = 0f;
			BiteCount = 0f;
			RavageCount = 0f;
			RavageAbove80PercentCount = 0f;
			EnergyCostMultiplier = energyCostMultiplier;
			EnergyRemaining = totalEnergy / energyCostMultiplier;
			CPRemaining = 0f;

			//Lead up time
			float currentLeadUpTime = 0f;
			float ripLeadUpTime = 0f;
			float rakeLeadUpTime = 0f;
			float roarLeadUpTime = 0f;
			{
				float cpToRip = 5f + _chanceExtraCP[4];

				//Ravage Twice
				Ravage(1, false, true);
				if (RavageStats.FeralChargeCooldown > 0f)
					Ravage(1, true, true);
				currentLeadUpTime += 3f;

				//Mangle, if necessary
				if (MangleUsage != MangleUsage.None)
				{
					Mangle(1);
					currentLeadUpTime++;
				}

				//Rake
				if (CPRemaining < cpToRip)
				{
					float rakesUsed = Math.Min(1f, (cpToRip - CPRemaining) / RakeStats.ComboPointsGenerated);
					Rake(rakesUsed);
					RakeTick(rakesUsed * RakeStats.Duration / 3f);
					rakeLeadUpTime = currentLeadUpTime + rakesUsed * RakeStats.Duration; //Raked [rakesUsed] times to start with, future rakes will start after this one
					currentLeadUpTime += rakesUsed;
				}

				//Shred, if you still need cp
				if (CPRemaining < cpToRip)
				{
					float shredsUsed = (cpToRip - CPRemaining) / ShredStats.ComboPointsGenerated;
					Shred(shredsUsed);
					currentLeadUpTime += shredsUsed;
				}

				ripLeadUpTime = currentLeadUpTime; //Rips will start from here
				if (RakeInitCount == 0)
					rakeLeadUpTime = currentLeadUpTime + 1; //if we haven't Raked already, Rakes will start here
				roarLeadUpTime = currentLeadUpTime + 2f; //Roar will start after the next CPG
			}

			//1. Rip
			float ripUptimeSec = FightDuration - ripLeadUpTime;
			float ripCount = (ripUptimeSec - FightDuration * BiteStats.RipRefreshChanceOnTargetsBelow25Percent * PercentOfTimeBelow25Percent) / RipStats.Duration;
			Rip(ripCount);
			RipTick(ripUptimeSec / 2f);

			//2. Roar
			float roarCPAverage = roarCP + _chanceExtraCP[roarCP - 1];
			float[] roarDurationsByCP = new float[]{RoarStats.Duration1CP, RoarStats.Duration2CP, 
				RoarStats.Duration3CP, RoarStats.Duration4CP, RoarStats.Duration5CP};
			float roarDurationAverage =
				roarDurationsByCP[roarCP - 1] * (1f - _chanceExtraCP[roarCP - 1]) +
				roarDurationsByCP[Math.Min(4, roarCP)] * _chanceExtraCP[roarCP - 1];

			float roarUptimeSec = FightDuration - roarLeadUpTime;
			float meleeDamageMultiplier = 1f + RoarStats.MeleeDamageMultiplier * roarUptimeSec / FightDuration;
			float roarCount = roarUptimeSec / roarDurationAverage;
			Roar(roarCount, roarCPAverage);

			//3. Rake
			float rakeUptimeSec = FightDuration - rakeLeadUpTime;
			float rakeCount = rakeUptimeSec / RakeStats.Duration;
			Rake(rakeCount);
			RakeTick(rakeUptimeSec / 3f);

			//4. Ravage
			if (RavageStats.FeralChargeCooldown > 0f)
			{
				float ravageCount = FightDuration / RavageStats.FeralChargeCooldown;
				Ravage(ravageCount * PercentOfTimeAbove80Percent, true, true);
				Ravage(ravageCount * (1f - PercentOfTimeAbove80Percent), true, false);
			}

			//5. Mangle
			float mangleCount = 0f;
			switch (MangleUsage)
			{
				case MangleUsage.MaintainMangle:
					mangleCount = FightDuration / MangleStats.Duration - 1f; //Minus the one from the lead up
					break;
				case MangleUsage.Maintain4T11:
					mangleCount = FightDuration / 27f - 1f; //Minus the one from the lead up
					break;
			}
			Mangle(mangleCount);

			//6. Shred
			if (CPRemaining < 0)
			{
				float shredCount = -CPRemaining / ShredStats.ComboPointsGenerated;
				Shred(shredCount);
			}

			//7. Bite
			float biteCPAverage = 5f + _chanceExtraCP[4]; //TODO: Need to try 4CP Bites
			float biteExtraEnergyPercent = BiteStats.MaxExtraEnergy == 0f ? 0f : (biteUsage == BiteUsage.HighEnergy ? 1f : 0.1f); //Assume unglyphed that you use an average of 3.5 extra energy per bite
			if (biteUsage != BiteUsage.None)
			{
				if (CPRemaining > 0)
				{
					float biteCount = CPRemaining / biteCPAverage;
					Bite(biteCount, biteCPAverage, biteExtraEnergyPercent);
				}
			}

			//8. Convert Extra Energy to Shreds+Bite
			if (biteUsage == BiteUsage.None)
			{
				float shredCount = EnergyRemaining / ShredStats.EnergyCost;
				Shred(shredCount);
			}
			else
			{
				float shredsPerBite = biteCPAverage / ShredStats.ComboPointsGenerated;
				float setEnergy = shredsPerBite * ShredStats.EnergyCost + BiteStats.EnergyCost + BiteStats.MaxExtraEnergy * biteExtraEnergyPercent;
				float setCount = EnergyRemaining / setEnergy;
				Shred(setCount * shredsPerBite);
				Bite(setCount, biteCPAverage, biteExtraEnergyPercent);
			}

			//Calculate Damage Done
			float totalDamage = 0f;
			totalDamage += MeleeStats.DPSAverage * FightDuration * meleeDamageMultiplier;
			totalDamage += MeleeStats.DPSFurySwipesAverage * FightDuration;
			totalDamage += MangleCount * MangleStats.DamageAverage;
			totalDamage += RakeInitCount * RakeStats.DamageAverage;
			totalDamage += RakeTickCount * RakeStats.DamageTickAverage;
			totalDamage += RipTickCount * RipStats.DamageTickAverage;
			totalDamage += ShredCount * ShredStats.DamageAverage;
			totalDamage += BiteCount * BiteStats.DamageAverage;
			totalDamage += RavageCount * RavageStats.DamageAverage;
			totalDamage += RavageAbove80PercentCount * RavageStats.DamageAbove80PercentAverage;


			return new CatRotationCalculation()
			{
				TotalDamage = totalDamage,
				DPS = totalDamage / FightDuration,
				BiteUsage = biteUsage,
				RoarCP = roarCP,

				MeleeDPS = MeleeStats.DPSAverage * meleeDamageMultiplier + MeleeStats.DPSFurySwipesAverage,
				MangleCount = MangleCount,
				RakeInitCount = RakeInitCount,
				RakeTickCount = RakeTickCount,
				RipTickCount = RipTickCount,
				ShredCount = ShredCount,
				BiteCount = BiteCount,
				RavageCount = RavageCount,
				RavageAbove80PercentCount = RavageAbove80PercentCount,
			};
		}

		private void Mangle(float count)
		{
			EnergyRemaining -= MangleStats.EnergyCost * count;
			CPRemaining += MangleStats.ComboPointsGenerated * count;
			MangleCount += count;
		}

		private void Rake(float count)
		{
			EnergyRemaining -= RakeStats.EnergyCost * count;
			CPRemaining += RakeStats.ComboPointsGenerated * count;
			RakeInitCount += count;
		}

		private void RakeTick(float count)
		{
			RakeTickCount += count;
		}

		private void Ravage(float count, bool fromFeralCharge, bool above80Percent)
		{
			EnergyRemaining -= (fromFeralCharge ? 10f : RavageStats.EnergyCost) * count;
			CPRemaining += (above80Percent ? RavageStats.ComboPointsGeneratedAbove80Percent : RavageStats.ComboPointsGenerated) * count;
			if (above80Percent) RavageAbove80PercentCount += count;
			else RavageCount += count;
		}

		private void Rip(float count)
		{
			EnergyRemaining -= RipStats.EnergyCost * count;
			CPRemaining -= (5f + _chanceExtraCP[4]) * count; //Some CP will be wasted due to critting while at 4cp.
		}

		private void RipTick(float count)
		{
			RipTickCount += count;
		}

		private void Roar(float count, float cp)
		{
			EnergyRemaining -= RoarStats.EnergyCost * count;
			CPRemaining -= cp * count;
		}

		private void Bite(float count, float cp, float extraEnergyPercent)
		{
			EnergyRemaining -= (BiteStats.EnergyCost + BiteStats.MaxExtraEnergy * extraEnergyPercent / EnergyCostMultiplier) * count;
			CPRemaining -= cp * count;
			BiteCount += count * (1f + extraEnergyPercent);
		}

		private void Shred(float count)
		{
			EnergyRemaining -= ShredStats.EnergyCost * count;
			CPRemaining += ShredStats.ComboPointsGenerated * count;
			ShredCount += count;
		}
	}

	public class CatRotationCalculation
	{
		public float DPS { get; set; }
		public float TotalDamage { get; set; }

		public float MeleeDPS { get; set; }
		public float MangleCount { get; set; }
		public float RakeInitCount { get; set; }
		public float RakeTickCount { get; set; }
		public float RipTickCount { get; set; }
		public float ShredCount { get; set; }
		public float BiteCount { get; set; }
		public float RavageCount { get; set; }
		public float RavageAbove80PercentCount { get; set; }

		public int RoarCP { get; set; }
		public BiteUsage BiteUsage { get; set; }

		public override string ToString()
		{
			StringBuilder rotation = new StringBuilder();
			rotation.AppendFormat("{0:N0}dps {1}Roar {2}FB*Priorities:\r\n", DPS, RoarCP, 
				BiteUsage == Cat.BiteUsage.None ? "No" :
				(BiteUsage == Cat.BiteUsage.HighEnergy ? "Hi" : "Low"));

			if (MangleCount > 0) rotation.Append("Keep Mangle up.\r\n");
			rotation.Append("Keep 5cp Rip up.\r\n");
			rotation.Append("Keep Rake up.\r\n");
			rotation.AppendFormat("Keep {0}cp Savage Roar up.\r\n", RoarCP);
			if (BiteUsage != Cat.BiteUsage.None) rotation.AppendFormat("Use {0}energy, 5cp Ferocious Bites to spend extra combo points.\r\n", BiteUsage == Cat.BiteUsage.HighEnergy ? "70" : "35");
			if (RavageAbove80PercentCount + RavageCount > 1f) rotation.Append("Run out and Feral Charge back in, and then Ravage, on cooldown.\r\n");
			rotation.Append("Use Shred for combo points.");
			
			return rotation.ToString();
		}
	}

	public enum MangleUsage
	{
		None,
		MaintainMangle,
		Maintain4T11
	}

	public enum BiteUsage
	{
		None,
		LowEnergy,
		HighEnergy
	}
}
