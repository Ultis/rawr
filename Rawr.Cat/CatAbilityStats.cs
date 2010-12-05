using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Cat
{
	public class CatAbilityBuilder
	{
		protected StatsCat Stats { get; private set; }
		protected float WeaponDPS { get; private set; }
		protected float AttackSpeed { get; private set; }
		protected float ArmorDamageMultiplier { get; private set; }
		protected float HasteBonus { get; private set; }
		protected float CritMultiplier { get; private set; }
		protected float ChanceAvoided { get; private set; }
		protected float ChanceCritMelee { get; private set; }
		protected float ChanceCritFurySwipes { get; private set; }
		protected float ChanceCritMangle { get; private set; }
		protected float ChanceCritRavage { get; private set; }
		protected float ChanceCritShred { get; private set; }
		protected float ChanceCritRake { get; private set; }
		protected float ChanceCritRip { get; private set; }
		protected float ChanceCritBite { get; private set; }
		protected float ChanceGlance { get; private set; }
		protected float BaseDamage { get; private set; }
		
		protected float ChanceNonAvoided { get { return 1f - ChanceAvoided; } }
		protected float ChanceCritRavageAbove80Percent { get { return Math.Min(1f, ChanceCritRavage + Stats.RavageCritChanceOnTargetsAbove80Percent); } }
		protected float AttackPower { get { return Stats.AttackPower; } }
		protected float FurySwipesChance { get { return Stats.FurySwipesChance; } }
		protected float CPOnCrit { get { return Stats.CPOnCrit; } }
		protected float SavageRoarMeleeDamageMultiplierIncrease { get { return Stats.SavageRoarDamageMultiplierIncrease; } }
		protected float RipCostReduction { get { return Stats.RipCostReduction; } }
		protected float MangleCatCostReduction { get { return Stats.MangleCatCostReduction; } }
		protected float TigersFuryCooldownReduction { get { return Stats.TigersFuryCooldownReduction; } }
		protected float EnergyOnTigersFury { get { return Stats.EnergyOnTigersFury; } }
		protected float MaxEnergyOnTigersFuryBerserk { get { return Stats.MaxEnergyOnTigersFuryBerserk; } }
		protected float ClearcastOnBleedChance { get { return Stats.ClearcastOnBleedChance; } }
		
		protected float BonusRakeDuration { get { return Stats.BonusRakeDuration; } }
		protected float BonusRipDuration { get { return Stats.BonusRipDuration; } }
		protected float BonusSavageRoarDuration { get { return Stats.BonusSavageRoarDuration; } }
		protected float BonusBerserkDuration { get { return Stats.BonusBerserkDuration; } }
		
		protected float DamageMultiplier { get { return 1f + Stats.BonusDamageMultiplier; } }
		protected float PhysicalDamageMultiplier { get { return 1f + Stats.BonusPhysicalDamageMultiplier; } }
		protected float BleedDamageMultiplier { get { return 1f + Stats.BonusBleedDamageMultiplier; } }
		protected float NonShredBleedDamageMultiplier { get { return 1f + Stats.NonShredBleedDamageMultiplier; } }
		protected float MangleDamageMultiplier { get { return 1f + Stats.MangleDamageMultiplier; } }
		protected float ShredDamageMultiplier { get { return 1f + Stats.ShredDamageMultiplier; } }
		protected float RakeDamageMultiplier { get { return 1f + Stats.BonusRakeDamageMultiplier; } }
		protected float RakeTickDamageMultiplier { get { return 1f + Stats.BonusRakeTickDamageMultiplier; } }
		protected float RipDamageMultiplier { get { return 1f + Stats.BonusRipDamageMultiplier; } }
		protected float FerociousBiteDamageMultiplier { get { return 1f + Stats.FerociousBiteDamageMultiplier; } }

		public CatAbilityBuilder(StatsCat stats, float weaponDPS, float attackSpeed, float armorDamageMultiplier, 
			float hasteBonus, float critMultiplier, float chanceAvoided, float chanceCritMelee, 
			float chanceCritFurySwipes, float chanceCritMangle, float chanceCritRavage, float chanceCritShred, 
			float chanceCritRake, float chanceCritRip, float chanceCritBite, float chanceGlance)
		{
			Stats = stats;
			WeaponDPS = weaponDPS;
			AttackSpeed = attackSpeed;
			ArmorDamageMultiplier = armorDamageMultiplier;
			HasteBonus = hasteBonus;
			CritMultiplier = critMultiplier;
			ChanceAvoided = chanceAvoided;
			ChanceCritMelee = chanceCritMelee;
			ChanceCritFurySwipes = chanceCritFurySwipes;
			ChanceCritMangle = chanceCritMangle;
			ChanceCritRavage = chanceCritRavage;
			ChanceCritShred = chanceCritShred;
			ChanceCritRake = chanceCritRake;
			ChanceCritRip = chanceCritRip;
			ChanceCritBite = chanceCritBite;
			ChanceGlance = chanceGlance;

			BaseDamage = WeaponDPS + AttackPower / 14f;
		}


		private MeleeStats _meleeStats = null;
		public MeleeStats MeleeStats
		{
			get
			{
				if (_meleeStats == null)
				{
					_meleeStats = new MeleeStats();
					_meleeStats.AttackSpeed = AttackSpeed;
					_meleeStats.ClearcastChance = 0.0583333333f * ChanceNonAvoided;
					_meleeStats.EnergyRegenPerSecond = 10f * (1f + HasteBonus);

					_meleeStats.DamageRaw = BaseDamage * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_meleeStats.DamageAverage = ((ChanceAvoided) * 0f) +
												((ChanceCritMelee) * (_meleeStats.DamageRaw * CritMultiplier)) +
												((ChanceGlance) * (_meleeStats.DamageRaw * 0.7f)) +
												((1f - ChanceAvoided - ChanceCritMelee - ChanceGlance) * (_meleeStats.DamageRaw));

					_meleeStats.DamageFurySwipesRaw = _meleeStats.DamageRaw * 3.1f;
					_meleeStats.DamageFurySwipesAverage = ChanceNonAvoided * (
																((ChanceCritFurySwipes) * (_meleeStats.DamageFurySwipesRaw * CritMultiplier)) +
																((1f - ChanceCritFurySwipes) * (_meleeStats.DamageFurySwipesRaw))
															);

					_meleeStats.DPSTotalAverage = _meleeStats.DamageAverage / AttackSpeed;

					if (FurySwipesChance > 0)
					{
						int attacksWithinCooldown = (int)Math.Floor(3f / AttackSpeed);
						float attacksPerProc = (1f / FurySwipesChance) + attacksWithinCooldown;
						float timePerProc = attacksPerProc * AttackSpeed;
						_meleeStats.DPSTotalAverage += _meleeStats.DamageFurySwipesAverage / timePerProc;
					}
				}
				return _meleeStats;
			}
		}

		private MangleStats _mangleStats = null;
		public MangleStats MangleStats
		{
			get
			{
				if (_mangleStats == null)
				{
					_mangleStats = new MangleStats();
					_mangleStats.Duration = 60f - 3f; //Assume 3sec of overlap
					_mangleStats.EnergyCost = 35f - MangleCatCostReduction;
					_mangleStats.EnergyCost += (_mangleStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_mangleStats.ComboPointsGenerated = 1f + (CPOnCrit * ChanceCritMangle);

					_mangleStats.DamageRaw = (BaseDamage + 316f) * 3.6f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier * MangleDamageMultiplier;
					_mangleStats.DamageAverage = ((ChanceCritMangle) * (_mangleStats.DamageRaw * CritMultiplier)) +
												((1f - ChanceCritMangle) * (_mangleStats.DamageRaw));
				}
				return _mangleStats;
			}
		}

		private ShredStats _shredStats = null;
		public ShredStats ShredStats
		{
			get
			{
				if (_shredStats == null)
				{
					_shredStats = new ShredStats();
					_shredStats.EnergyCost = 40f;
					_shredStats.EnergyCost += (_shredStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_shredStats.ComboPointsGenerated = 1f + (CPOnCrit * ChanceCritShred);

					_shredStats.DamageRaw = (BaseDamage + 330f) * 3.5f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier * ShredDamageMultiplier * BleedDamageMultiplier;
					_shredStats.DamageAverage = ((ChanceCritShred) * (_shredStats.DamageRaw * CritMultiplier)) +
												((1f - ChanceCritShred) * (_shredStats.DamageRaw));
				}
				return _shredStats;
			}
		}

		private RavageStats _ravageStats = null;
		public RavageStats RavageStats
		{
			get
			{
				if (_ravageStats == null)
				{
					_ravageStats = new RavageStats();
					_ravageStats.EnergyCost = 60f;
					_ravageStats.EnergyCost += (_ravageStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_ravageStats.ComboPointsGenerated = 1f + (CPOnCrit * ChanceCritRavage);
					_ravageStats.ComboPointsGeneratedAbove80Percent = 1f + (CPOnCrit * ChanceCritRavageAbove80Percent);
					if (Stats.FreeRavageOnFeralChargeChance > 0f)
						_ravageStats.FeralChargeCooldown = (30f - Stats.FeralChargeCatCooldownReduction) / Stats.FreeRavageOnFeralChargeChance;

					_ravageStats.DamageRaw = (BaseDamage + 330f) * 8.5f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_ravageStats.DamageAverage = ((ChanceCritRavage) * (_ravageStats.DamageRaw * CritMultiplier)) +
												((1f - ChanceCritRavage) * (_ravageStats.DamageRaw));
					_ravageStats.DamageAbove80PercentAverage = ((ChanceCritRavageAbove80Percent) * (_ravageStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCritRavageAbove80Percent) * (_ravageStats.DamageRaw));
				}
				return _ravageStats;
			}
		}

		private RakeStats _rakeStats = null;
		public RakeStats RakeStats
		{
			get
			{
				if (_rakeStats == null)
				{
					_rakeStats = new RakeStats();
					_rakeStats.Duration = 9f + BonusRakeDuration;
					_rakeStats.EnergyCost = 35f;
					_rakeStats.EnergyCost += (_rakeStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_rakeStats.ComboPointsGenerated = 1f + (CPOnCrit * ChanceCritRake);
					_rakeStats.TickClearcastChance = ClearcastOnBleedChance;
					
					_rakeStats.DamageRaw = (304f + AttackPower * 0.023f) * DamageMultiplier * PhysicalDamageMultiplier * BleedDamageMultiplier * NonShredBleedDamageMultiplier * RakeDamageMultiplier;
					_rakeStats.DamageAverage = ((ChanceCritRake) * (_rakeStats.DamageRaw * CritMultiplier)) +
												((1f - ChanceCritRake) * (_rakeStats.DamageRaw));

					_rakeStats.DamageTickRaw = (620f + AttackPower * 0.14f) * DamageMultiplier * PhysicalDamageMultiplier * BleedDamageMultiplier * NonShredBleedDamageMultiplier * RakeDamageMultiplier * RakeTickDamageMultiplier;
					_rakeStats.DamageTickAverage = ((ChanceCritRake) * (_rakeStats.DamageTickRaw * CritMultiplier)) +
												((1f - ChanceCritRake) * (_rakeStats.DamageTickRaw));
				}
				return _rakeStats;
			}
		}

		private RipStats _ripStats = null;
		public RipStats RipStats
		{
			get
			{
				if (_ripStats == null)
				{
					_ripStats = new RipStats();
					_ripStats.Duration = 16f + BonusRipDuration;
					_ripStats.EnergyCost = 30f - RipCostReduction;
					_ripStats.EnergyCost += (_ripStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_ripStats.TickClearcastChance = ClearcastOnBleedChance;

					_ripStats.DamageTickRaw = (868f + AttackPower * 0.115f) * DamageMultiplier * PhysicalDamageMultiplier * BleedDamageMultiplier * NonShredBleedDamageMultiplier * RipDamageMultiplier;
					_ripStats.DamageTickAverage = ((ChanceCritRip) * (_ripStats.DamageTickRaw * CritMultiplier)) +
												((1f - ChanceCritRip) * (_ripStats.DamageTickRaw));
				}
				return _ripStats;
			}
		}

		private BiteStats _biteStats = null;
		public BiteStats BiteStats
		{
			get
			{
				if (_biteStats == null)
				{
					_biteStats = new BiteStats();
					_biteStats.EnergyCost = 35f;
					_biteStats.EnergyCost += (_biteStats.EnergyCost * 0.2f) * (1f / ChanceNonAvoided - 1f); //Count avoids as an increase in energy cost
					_biteStats.MaxExtraEnergy = 35f - Stats.FerociousBiteMaxExtraEnergyReduction;
					
					_biteStats.DamageRaw = (2822f + AttackPower * 0.545f) * DamageMultiplier * PhysicalDamageMultiplier * FerociousBiteDamageMultiplier * ArmorDamageMultiplier;
					_biteStats.DamageAverage = ((ChanceCritBite) * (_biteStats.DamageRaw * CritMultiplier)) +
												((1f - ChanceCritBite) * (_biteStats.DamageRaw));
				}
				return _biteStats;
			}
		}

		private RoarStats _roarStats = null;
		public RoarStats RoarStats
		{
			get
			{
				if (_roarStats == null)
				{
					_roarStats = new RoarStats();
					_roarStats.Duration1CP = 14f + BonusSavageRoarDuration;
					_roarStats.Duration2CP = 19f + BonusSavageRoarDuration;
					_roarStats.Duration3CP = 24f + BonusSavageRoarDuration;
					_roarStats.Duration4CP = 29f + BonusSavageRoarDuration;
					_roarStats.Duration5CP = 34f + BonusSavageRoarDuration;
					_roarStats.EnergyCost = 25f;
					_roarStats.MeleeDamageMultiplier = 0.5f + SavageRoarMeleeDamageMultiplierIncrease;
				}
				return _roarStats;
			}
		}

		private TigersFuryStats _tigersFuryStats = null;
		public TigersFuryStats TigersFuryStats
		{
			get
			{
				if (_tigersFuryStats == null)
				{
					_tigersFuryStats = new TigersFuryStats();
					_tigersFuryStats.Duration = 6f;
					_tigersFuryStats.Cooldown = 30f - TigersFuryCooldownReduction;
					_tigersFuryStats.DamageMultiplier = 0.15f;
					_tigersFuryStats.EnergyGenerated = EnergyOnTigersFury;
					_tigersFuryStats.MaxEnergyIncrease = MaxEnergyOnTigersFuryBerserk;
				}
				return _tigersFuryStats;
			}
		}

		private BerserkStats _berserkStats = null;
		public BerserkStats BerserkStats
		{
			get
			{
				if (_berserkStats == null)
				{
					_berserkStats = new BerserkStats();
					_berserkStats.Duration = 15f + BonusBerserkDuration;
					_berserkStats.Cooldown = 300f;
					_berserkStats.EnergyCostMultiplier = 0.5f;
					_berserkStats.MaxEnergyIncrease = MaxEnergyOnTigersFuryBerserk;
				}
				return _berserkStats;
			}
		}


	}







	public class MeleeStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float DamageFurySwipesRaw { get; set; }
		public float DamageFurySwipesAverage { get; set; }
		public float AttackSpeed { get; set; }
		public float DPSTotalAverage { get; set; }
		public float ClearcastChance { get; set; }
		public float EnergyRegenPerSecond { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damageDone = useCount * (DamageAverage + cp * DamagePerSwingPerCP);
		//    string stats = string.Format("{0:F0}x   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Swing:  {3}\r\nDamage Per Hit:  {4}",
		//        useCount, damageDone, damageDone / totalDamage, DamageAverage, DamageRaw);
		//    return stats;
		//}
	}

	public class MangleStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float Duration { get; set; }
		public float ComboPointsGenerated { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damageDone = useCount * (DamageAverage + cp * DamagePerSwingPerCP);
		//    string stats = string.Format("{5:F1} DPE   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Landed Swing:  {3}\r\nDamage Per Hit:  {4}\r\nDamage Per Energy:  {5}",
		//        useCount, damageDone, damageDone / totalDamage, DamageAverage * chanceNonAvoided, DamageRaw, DamageAverage / EnergyCost);
		//    return stats;
		//}
	}

	public class ShredStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float ComboPointsGenerated { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damageDone = useCount * (DamageAverage + cp * DamagePerSwingPerCP);
		//    string stats = string.Format("{5:F1} DPE   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Landed Swing:  {3}\r\nDamage Per Hit:  {4}\r\nDamage Per Energy:  {5}",
		//        useCount, damageDone, damageDone / totalDamage, DamageAverage * chanceNonAvoided, DamageRaw, DamageAverage / EnergyCost);
		//    return stats;
		//}
	}

	public class RavageStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float DamageAbove80PercentAverage { get; set; }
		public float ComboPointsGenerated { get; set; }
		public float ComboPointsGeneratedAbove80Percent { get; set; }
		public float FeralChargeCooldown { get; set; }
	}

	public class RakeStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float Duration { get; set; }
		public float DamageTickRaw { get; set; }
		public float DamageTickAverage { get; set; }
		public float ComboPointsGenerated { get; set; }
		public float TickClearcastChance { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damageDone = useCount * (DamageAverage + cp * DamagePerSwingPerCP);
		//    string stats = string.Format("{5:F1} DPE   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Landed Swing:  {3}\r\nDamage Per Hit:  {4}\r\nDamage Per Energy:  {5}\r\nDuration:  {6}sec\r\nUptime:  {7:P}",
		//        useCount, damageDone, damageDone / totalDamage, DamageAverage * chanceNonAvoided, DamageRaw, DamageAverage / EnergyCost, DurationUptime, (DurationUptime * useCount) / totalDuration);
		//    return stats;
		//}
	}

	public class RipStats
	{
		public float EnergyCost { get; set; }
		public float Duration { get; set; }
		public float DamageTickRaw { get; set; }
		public float DamageTickAverage { get; set; }
		public float TickClearcastChance { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damageDone = useCount * (DamageAverage + cp * DamagePerSwingPerCP);
		//    string stats = string.Format("{5:F1} DPE   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Landed Swing:  {3}\r\nDamage Per Hit:  {4}\r\nDamage Per Energy:  {5}\r\nDuration:  {6}sec\r\nUptime:  {7:P}",
		//        useCount, damageDone, damageDone / totalDamage, DamageAverage * chanceNonAvoided, DamageRaw, DamageAverage / EnergyCost, DurationUptime, (DurationUptime * useCount) / totalDuration);
		//    return stats;
		//}
	}

	public class BiteStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float MaxExtraEnergy { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    float damagePerSwing = (DamageAverage + cp * DamagePerSwingPerCP);
		//    float damagePerHit = (DamageRaw + cp * DamagePerHitPerCP);
		//    float damageDone = useCount * damagePerSwing;
		//    float dpe1 = (DamageAverage + 1f * DamagePerSwingPerCP) / EnergyCost;
		//    float dpe2 = (DamageAverage + 2f * DamagePerSwingPerCP) / EnergyCost;
		//    float dpe3 = (DamageAverage + 3f * DamagePerSwingPerCP) / EnergyCost;
		//    float dpe4 = (DamageAverage + 4f * DamagePerSwingPerCP) / EnergyCost;
		//    float dpe5 = (DamageAverage + 5f * DamagePerSwingPerCP) / EnergyCost;

		//    string stats = string.Format("{5:F1} DPE   ({2:P1})*Use Count:  {0}\r\nDamage Done:  {1}\r\n% of Total Damage:  {2:P}\r\nDamage Per Landed Swing:  {3}\r\nDamage Per Hit:  {4}\r\nDamage Per Energy:  {5}\r\n1cp DPE:  {6}\r\n2cp DPE:  {7}\r\n3cp DPE:  {8}\r\n4cp DPE:  {9}\r\n5cp DPE:  {10}\r\n",
		//        useCount, damageDone, damageDone / totalDamage, damagePerSwing * chanceNonAvoided, damagePerHit, damagePerSwing / EnergyCost, dpe1, dpe2, dpe3, dpe4, dpe5);
		//    return stats;
		//}
	}

	public class RoarStats
	{
		public float EnergyCost { get; set; }
		public float Duration1CP { get; set; }
		public float Duration2CP { get; set; }
		public float Duration3CP { get; set; }
		public float Duration4CP { get; set; }
		public float Duration5CP { get; set; }
		public float MeleeDamageMultiplier { get; set; }
		//public override string GetStatsTexts(float useCount, float cp, float totalDamage, float chanceNonAvoided, float totalDuration)
		//{
		//    string usage = string.Format("{0}x*Use Count: {0}",
		//        useCount);
		//    string stats = string.Format("{0}sec, {1}cp*Duration: {0}\r\nTarget Combo Points: {1}%",
		//        DurationUptime + (cp * DurationPerCP), cp);
		//    return usage + stats;
		//}

	}

	public class TigersFuryStats
	{
		public float DamageMultiplier { get; set; }
		public float MaxEnergyIncrease { get; set; }
		public float EnergyGenerated { get; set; }
		public float Duration { get; set; }
		public float Cooldown { get; set; }
	}

	public class BerserkStats
	{
		public float MaxEnergyIncrease { get; set; }
		public float EnergyCostMultiplier { get; set; }
		public float Duration { get; set; }
		public float Cooldown { get; set; }
	}
}
