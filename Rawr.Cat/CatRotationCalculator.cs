using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Cat
{
	public class CatRotationCalculator
	{
		public Stats Stats { get; set; }
		public float Duration { get; set; }
		public float CPPerCPG { get; set; }
		public bool MaintainMangle { get; set; }
		public bool GlyphOfShred { get; set; }
		public float AttackSpeed { get; set; }
		public bool OmenOfClarity { get; set; }
		public float AvoidedAttacks { get; set; }
		public float CPGEnergyCostMultiplier { get; set; }
		public float ClearcastOnBleedChance { get; set; }

		public float MangleDuration { get; set; }
		public float RipDuration { get; set; }
		public float RakeDuration { get; set; }
		public float SavageRoarBonusDuration { get; set; }
		public float BerserkDuration { get; set; }

		public float MeleeDamage { get; set; }
		public float MangleDamage { get; set; }
		public float ShredDamage { get; set; }
		public float RakeDamage { get; set; }
		public float RipDamage { get; set; }
		public float BiteDamage { get; set; }

		public float MangleEnergy { get; set; }
		public float ShredEnergy { get; set; }
		public float RakeEnergy { get; set; }
		public float RipEnergy { get; set; }
		public float BiteEnergy { get; set; }
		public float RoarEnergy { get; set; }

		public CatRotationCalculator(Stats stats, float duration, float cpPerCPG, bool maintainMangle, float mangleDuration,
			float ripDuration, float rakeDuration, float savageRoarBonusDuration, float berserkDuration, float attackSpeed, 
			bool omenOfClarity, bool glyphOfShred, float avoidedAttacks, float cpgEnergyCostMultiplier, float clearcastOnBleedChance,
			float meleeDamage, float mangleDamage, float shredDamage, float rakeDamage, float ripDamage, float biteDamage, 
			float mangleEnergy, float shredEnergy, float rakeEnergy, float ripEnergy, float biteEnergy, float roarEnergy)
		{
			Stats = stats;
			Duration = duration;
			CPPerCPG = cpPerCPG;
			MaintainMangle = maintainMangle;
			AttackSpeed = attackSpeed;
			OmenOfClarity = omenOfClarity;
			GlyphOfShred = glyphOfShred;
			AvoidedAttacks = avoidedAttacks;
			CPGEnergyCostMultiplier = cpgEnergyCostMultiplier;
			ClearcastOnBleedChance = clearcastOnBleedChance;

			MangleDuration = mangleDuration;
			RipDuration = ripDuration;
			RakeDuration = rakeDuration;
			SavageRoarBonusDuration = savageRoarBonusDuration;
			BerserkDuration = berserkDuration;
			
			MeleeDamage = meleeDamage;
			MangleDamage = mangleDamage;
			ShredDamage = shredDamage;
			RakeDamage = rakeDamage;
			RipDamage = ripDamage;
			BiteDamage = biteDamage;
			
			MangleEnergy = mangleEnergy;
			ShredEnergy = shredEnergy;
			RakeEnergy = rakeEnergy;
			RipEnergy = ripEnergy;
			BiteEnergy = biteEnergy;
			RoarEnergy = roarEnergy;
		}

		public CatRotationCalculation GetRotationCalculations(bool useShred, bool useRip, bool useFerociousBite, int roarCP)
		{
			float totalEnergyAvailable = 100f + (10f * Duration) + ((float)Math.Ceiling((Duration - 10f) / (30f - Stats.TigersFuryCooldownReduction)) * Stats.BonusEnergyOnTigersFury);
			if (BerserkDuration > 0)
				totalEnergyAvailable += (BerserkDuration + 7f) * 10f; //Assume 70 energy when you activate Berserk
			if (OmenOfClarity)
			{
				float oocProcs = ((3.5f * (Duration / 60f)) / AttackSpeed) * (1f - AvoidedAttacks); //Counts all OOCs as being used on the CPG. Should be made more accurate than that, but that's close at least
				if (ClearcastOnBleedChance > 0)
				{
					float dotTicks = (1f / 3f + 1f / 2f) * Duration;
					oocProcs += dotTicks * ClearcastOnBleedChance;
				}
				float cpgEnergyRaw = (useShred ? ShredEnergy : MangleEnergy) / CPGEnergyCostMultiplier;
				totalEnergyAvailable += oocProcs * (cpgEnergyRaw * (1f - AvoidedAttacks) + cpgEnergyRaw * AvoidedAttacks * 0.2f);
			}
			
			float totalCPAvailable = 0f;

			#region Melee
			float meleeCount = Duration / AttackSpeed;
			#endregion

			#region Rake
			float rakeCount = Duration / RakeDuration;
			float rakeTotalEnergy = rakeCount * RakeEnergy;
			float rakeCP = rakeCount * CPPerCPG;
			totalCPAvailable += rakeCP;
			totalEnergyAvailable -= rakeTotalEnergy;
			#endregion

			#region Mangle
			float mangleCount = 0f;
			float mangleTotalEnergy = 0f;
			float mangleCP = 0f;
			if (MaintainMangle)
			{
				mangleCount = Duration / MangleDuration;
				mangleTotalEnergy = mangleCount * MangleEnergy;
				mangleCP = mangleCount * CPPerCPG;
			}
			totalCPAvailable += mangleCP;
			totalEnergyAvailable -= mangleTotalEnergy;
			#endregion

			#region Combo Point Generator
			float cpgCount = 0f;
			float cpgEnergy = useShred ? ShredEnergy : MangleEnergy;
			float shredCount = 0f;
			#endregion

			#region Savage Roar
			float roarDuration = 9f + 5f * roarCP + SavageRoarBonusDuration;
			float roarCount = Duration / roarDuration;
			float roarTotalEnergy = roarCount * RoarEnergy;
			float roarCPRequired = roarCount * roarCP;
			if (totalCPAvailable < roarCPRequired)
			{
				float cpToGenerate = roarCPRequired - totalCPAvailable;
				float cpgToUse = cpToGenerate / CPPerCPG;
				cpgCount += cpgToUse;
				totalEnergyAvailable -= cpgToUse * cpgEnergy;
				totalCPAvailable += cpToGenerate;
			}
			totalCPAvailable -= roarCPRequired;
			totalEnergyAvailable -= roarTotalEnergy;
			#endregion

			#region Damage Finishers
			float ripDuration = GlyphOfShred && useShred ? RipDuration + 6 : RipDuration;
			float ripCount = 0f;
			float biteCount = 0f;
			if (useRip && !useFerociousBite)
			{
				#region Rip
				float ripCyclesFromAvailableCP = totalCPAvailable / 5f;
				ripCount += ripCyclesFromAvailableCP;
				totalCPAvailable = 0;
				totalEnergyAvailable -= RipEnergy * ripCyclesFromAvailableCP;

				float ripCycleEnergy = (5f / CPPerCPG) * cpgEnergy + RipEnergy;
				float ripCycleCountMax = Duration / ripDuration;
				float ripCycleCount = Math.Min(ripCycleCountMax - ripCyclesFromAvailableCP, totalEnergyAvailable / ripCycleEnergy);
				
				ripCount += ripCycleCount;
				cpgCount += (5f / CPPerCPG) * ripCycleCount;
				totalEnergyAvailable -= ripCycleEnergy * ripCycleCount;
				#endregion
			}
			else if (!useRip && useFerociousBite)
			{
				#region Ferocious Bite
				float biteCyclesFromAvailableCP = totalCPAvailable / 5f;
				biteCount += biteCyclesFromAvailableCP;
				totalCPAvailable = 0;
				totalEnergyAvailable -= BiteEnergy * biteCyclesFromAvailableCP;

				float biteCycleEnergy = (5f / CPPerCPG) * cpgEnergy + BiteEnergy;
				float biteCycleCount = totalEnergyAvailable / biteCycleEnergy;

				biteCount += biteCycleCount;
				cpgCount += biteCycleCount * (5f / CPPerCPG);
				totalEnergyAvailable = 0f;
				#endregion
			}
			else if (useRip && useFerociousBite)
			{
				#region Rip & Ferocious Bite
				float ripCyclesFromAvailableCP = totalCPAvailable / 5f;
				ripCount += ripCyclesFromAvailableCP;
				totalCPAvailable = 0;
				totalEnergyAvailable -= RipEnergy * ripCyclesFromAvailableCP;

				float ripCycleEnergy = (5f / CPPerCPG) * cpgEnergy + RipEnergy;
				float ripCycleCountMax = Duration / ripDuration;
				float ripCycleCount = Math.Min(ripCycleCountMax - ripCyclesFromAvailableCP, totalEnergyAvailable / ripCycleEnergy);

				ripCount += ripCycleCount;
				cpgCount += ripCycleCount * (5f / CPPerCPG);
				totalEnergyAvailable -= ripCycleEnergy * ripCycleCount;

				if (ripCycleCount > 0 && totalEnergyAvailable / ripCycleCount > BiteEnergy)
				{
					float energyAvailablePerRipCycle = totalEnergyAvailable / ripCycleCount;
					float biteCycleCP = CPPerCPG * ((energyAvailablePerRipCycle - BiteEnergy) / cpgEnergy);
					float biteDamageMultiplier = (1f + biteCycleCP) / 6f; //A 1pt Bite does 2/6 of full dmaage, 2pt does 3/6, 3pt does 4/6, 4pt does 5/6, 5pt does 6/6

					biteCount += ripCycleCount * biteDamageMultiplier; //ie, count it as however many full damage bites it's equivalent to. 
					cpgEnergy += ripCycleCount * (biteCycleCP / CPPerCPG);
					totalEnergyAvailable = 0f;
				}
				#endregion
			}
			#endregion

			#region Extra Energy turned into Combo Point Generators
			if (totalEnergyAvailable > 0)
			{
				cpgCount += totalEnergyAvailable / cpgEnergy;
				totalEnergyAvailable = 0f;
			}
			#endregion

			#region Damage Totals
			if (useShred) shredCount += cpgCount;
			else mangleCount += cpgCount;
			
			float meleeDamageTotal = meleeCount * MeleeDamage;
			float mangleDamageTotal = mangleCount * MangleDamage;
			float rakeDamageTotal = rakeCount * RakeDamage;
			float shredDamageTotal = shredCount * ShredDamage;
			float ripDamageTotal = ripCount * RipDamage * (ripDuration / 12f);
			float biteDamageTotal = biteCount * BiteDamage;

			float damageTotal = meleeDamageTotal + mangleDamageTotal + rakeDamageTotal + shredDamageTotal + ripDamageTotal + biteDamageTotal;
			#endregion

			StringBuilder rotationName = new StringBuilder();
			if (MaintainMangle || !useShred) rotationName.Append("Mangle+");
			if (useShred) rotationName.Append("Shred+");
			if (useRip) rotationName.Append("Rip+");
			if (useFerociousBite) rotationName.Append("Bite+");
			rotationName.Append("Roar" + roarCP.ToString());

			return new CatRotationCalculation()
			{
				Name = rotationName.ToString(),
				DPS = damageTotal / Duration,

				MeleeDamageTotal = meleeDamageTotal,
				MangleDamageTotal = mangleDamageTotal,
				RakeDamageTotal = rakeDamageTotal,
				ShredDamageTotal = shredDamageTotal,
				RipDamageTotal = ripDamageTotal,
				BiteDamageTotal = biteDamageTotal,
				DamageTotal = damageTotal,

				RoarCP = roarCP,
			};

			//List<string> rotationName = new List<string>();
			//if (MaintainMangle || !useShred) rotationName.Add("Mangle");
			//if (useShred) rotationName.Add("Shred");
			//if (useRip) rotationName.Add("Rip");
			//if (useFerociousBite) rotationName.Add("Bite");
			//rotationName.Add("Roar" + roarCP.ToString());
			
			//return new CatRotationCalculation()
			//{ 
			//    Name = string.Join(" + ", rotationName.ToArray()),
			//    DPS = damageTotal / Duration,
				
			//    MeleeDamageTotal = meleeDamageTotal,
			//    MangleDamageTotal = mangleDamageTotal,
			//    RakeDamageTotal = rakeDamageTotal,
			//    ShredDamageTotal = shredDamageTotal,
			//    RipDamageTotal = ripDamageTotal,
			//    BiteDamageTotal = biteDamageTotal,
			//    DamageTotal = damageTotal,

			//    RoarCP = roarCP,
			//};
		}

		public class CatRotationCalculation
		{
			public string Name { get; set; }
			public float DPS { get; set; }

			//public Stats Stats { get; set; }
			//public float Duration { get; set; }
			//public float CPPerCPG { get; set; }
			//public bool MaintainMangle { get; set; }
			//public float MangleDuration { get; set; }
			//public float RipDuration { get; set; }
			//public float AttackSpeed { get; set; }
			//public bool OmenOfClarity { get; set; }

			//public float MeleeDamage { get; set; }
			//public float MangleDamage { get; set; }
			//public float ShredDamage { get; set; }
			//public float RakeDamage { get; set; }
			//public float RipDamage { get; set; }
			//public float BiteDamage { get; set; }

			//public float MangleEnergy { get; set; }
			//public float ShredEnergy { get; set; }
			//public float RakeEnergy { get; set; }
			//public float RipEnergy { get; set; }
			//public float BiteEnergy { get; set; }
			//public float RoarEnergy { get; set; }

			public float MeleeDamageTotal { get; set; }
			public float MangleDamageTotal { get; set; }
			public float RakeDamageTotal { get; set; }
			public float ShredDamageTotal { get; set; }
			public float RipDamageTotal { get; set; }
			public float BiteDamageTotal { get; set; }
			public float DamageTotal { get; set; }
			public int RoarCP { get; set; }
		}
	}
}
