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
		protected float RipRefreshChanceOnFerociousBiteOnTargetsBelow25Percent { get { return Stats.RipRefreshChanceOnFerociousBiteOnTargetsBelow25Percent; } }
		
		protected float BonusRakeDuration { get { return Stats.BonusRakeDuration; } }
		protected float BonusRipDuration { get { return Stats.BonusRipDuration; } }
		protected float BonusSavageRoarDuration { get { return Stats.BonusSavageRoarDuration; } }
		protected float BonusBerserkDuration { get { return Stats.BonusBerserkDuration; } }
		
		protected float DamageMultiplier { get { return 1f + Stats.BonusDamageMultiplier; } }
        protected float WhiteDamageMultiplier { get { return 1f + Stats.BonusWhiteDamageMultiplier; } }
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

			BaseDamage = WeaponDPS + AttackPower / 14f + stats.WeaponDamage;
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

					float roarMultiplier = 1f + RoarStats.MeleeDamageMultiplier;

					_meleeStats.DamageRaw = BaseDamage * DamageMultiplier * WhiteDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_meleeStats.DamageAverage = ((ChanceAvoided) * 0f) +
												((ChanceCritMelee) * (_meleeStats.DamageRaw * CritMultiplier)) +
												((ChanceGlance) * (_meleeStats.DamageRaw * 0.7f)) +
												((1f - ChanceAvoided - ChanceCritMelee - ChanceGlance) * (_meleeStats.DamageRaw));
					_meleeStats.DPSAverage = _meleeStats.DamageAverage / AttackSpeed;

					_meleeStats.DamageRoarRaw = _meleeStats.DamageRaw * roarMultiplier;
					_meleeStats.DamageRoarAverage = _meleeStats.DamageAverage * roarMultiplier;
					_meleeStats.DPSRoarAverage = _meleeStats.DamageRoarAverage / AttackSpeed;

					_meleeStats.DamageFurySwipesRaw = _meleeStats.DamageRaw * 3.1f;
					_meleeStats.DamageFurySwipesAverage = ChanceNonAvoided * (
																((ChanceCritFurySwipes) * (_meleeStats.DamageFurySwipesRaw * CritMultiplier)) +
																((1f - ChanceCritFurySwipes) * (_meleeStats.DamageFurySwipesRaw))
															);

					
					if (FurySwipesChance > 0)
					{
						int attacksWithinCooldown = (int)Math.Floor(3f / AttackSpeed);
						float attacksPerProc = (1f / FurySwipesChance) + attacksWithinCooldown;
						float timePerProc = attacksPerProc * AttackSpeed;
						_meleeStats.DPSFurySwipesAverage = _meleeStats.DamageFurySwipesAverage / timePerProc;
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
					_biteStats.RipRefreshChanceOnTargetsBelow25Percent = Stats.RipRefreshChanceOnFerociousBiteOnTargetsBelow25Percent;
					
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
					_berserkStats.Duration = BonusBerserkDuration;
					_berserkStats.Cooldown = 300f;
					_berserkStats.EnergyCostMultiplier = 0.5f;
					_berserkStats.MaxEnergyIncrease = MaxEnergyOnTigersFuryBerserk;
				}
				return _berserkStats;
			}
		}


		public static string BuildTooltip(params object[] fields)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < fields.Length; i++)
			{
				if (i % 2 == 0)
					sb.AppendFormat("{0}{1}: ", i == 0 ? "" : "\r\n", fields[i]);
				else
					sb.AppendFormat("{0:N2}", fields[i]);
			}
			return sb.ToString();
		}
	}







	public class MeleeStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float DamageRoarRaw { get; set; }
		public float DamageRoarAverage { get; set; }
		public float DamageFurySwipesRaw { get; set; }
		public float DamageFurySwipesAverage { get; set; }
		public float AttackSpeed { get; set; }
		public float DPSAverage { get; set; }
		public float DPSRoarAverage { get; set; }
		public float DPSFurySwipesAverage { get; set; }
		public float ClearcastChance { get; set; }
		public float EnergyRegenPerSecond { get; set; }

		public override string ToString()
		{
			return string.Format("DPS: {0:N0}*{1}", DPSRoarAverage + DPSFurySwipesAverage,
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Swing", DamageAverage,
					"Damage Per Hit w/ Savage Roar", DamageRoarRaw,
					"Damage Per Swing w/ Savage Roar", DamageRoarAverage,
					"Damage Per Fury Swipes Hit", DamageFurySwipesRaw,
					"Damage Per Fury Swipes Swing", DamageFurySwipesAverage
				));
		}
	}

	public class MangleStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float Duration { get; set; }
		public float ComboPointsGenerated { get; set; }

		public override string ToString()
		{
			return string.Format("DPE: {0:N1}  Dmg: {1:N0}*{2}", DamageAverage / EnergyCost, DamageAverage,
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Landed Attack", DamageAverage,
					"Damage Per Energy", DamageAverage / EnergyCost
				));
		}
	}

	public class ShredStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float ComboPointsGenerated { get; set; }

		public override string ToString()
		{
			return string.Format("DPE: {0:N1}  Dmg: {1:N0}*{2}", DamageAverage / EnergyCost, DamageAverage,
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Landed Attack", DamageAverage,
					"Damage Per Energy", DamageAverage / EnergyCost
				));
		}
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

		public override string ToString()
		{
			return string.Format("DPE: {0:N0}  Dmg: {1:N0}*{2}", DamageAverage / 10f, DamageAverage,
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Landed Attack", DamageAverage,
					"Damage Per Landed Attack Above 80%", DamageAbove80PercentAverage,
					"Damage Per Energy", DamageAverage / EnergyCost,
					"Damage Per Energy Above 80%", DamageAbove80PercentAverage / EnergyCost,
					"Damage Per Energy w/ Stampede", DamageAverage / 10f,
					"Damage Per Energy Above 80% w/ Stampede", DamageAbove80PercentAverage / 10f
				));
		}
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

		public override string ToString()
		{
			return string.Format("DPE: {0:N0}  Dmg: {1:N0}*{2}", (DamageAverage + DamageTickAverage * Duration / 3f )  / EnergyCost, (DamageAverage + DamageTickAverage * Duration / 3f ),
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Landed Attack", DamageAverage,
					"Damage Per Tick Hit", DamageTickRaw,
					"Damage Per Average Tick", DamageTickAverage,
					"Damage Per Energy", (DamageAverage + DamageTickAverage * Duration / 3f) / EnergyCost
				));
		}
	}

	public class RipStats
	{
		public float EnergyCost { get; set; }
		public float Duration { get; set; }
		public float DamageTickRaw { get; set; }
		public float DamageTickAverage { get; set; }
		public float TickClearcastChance { get; set; }

		public override string ToString()
		{
			return string.Format("DPE: {0:N0}  Dmg: {1:N0}*{2}", (DamageTickAverage * Duration / 2f) / EnergyCost, (DamageTickAverage * Duration / 2f),
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Tick Hit", DamageTickRaw,
					"Damage Per Average Tick", DamageTickAverage,
					"Damage Per Energy", (DamageTickAverage * Duration / 2f) / EnergyCost
				));
		}
	}

	public class BiteStats
	{
		public float DamageRaw { get; set; }
		public float DamageAverage { get; set; }
		public float EnergyCost { get; set; }
		public float MaxExtraEnergy { get; set; }
		public float RipRefreshChanceOnTargetsBelow25Percent { get; set; }

		public override string ToString()
		{
			return string.Format("DPE: {0:N1}  Dmg: {1:N0}*{2}", DamageAverage / EnergyCost, DamageAverage,
				CatAbilityBuilder.BuildTooltip(
					"Damage Per Hit", DamageRaw,
					"Damage Per Landed Attack", DamageAverage,
					"Damage Per High Energy Landed Attack", DamageAverage * 2f,
					"Damage Per Energy", DamageAverage / EnergyCost
				));
		}
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
