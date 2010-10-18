using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Bear
{
	public class BearAbilityBuilder
	{
		public StatsBear Stats { get; private set; }
		public float WeaponDPS { get; private set; }
		public float AttackSpeed { get; private set; }
		public float ArmorDamageMultiplier { get; private set; }
		public float BaseDamage { get; private set; }
		public float ChanceAvoided { get; private set; }
		public float ChanceCrit { get; private set; }
		public float ChanceCritSpell { get; private set; }
		public float ChanceGlance { get; private set; }
		public float CritMultiplier { get; private set; }
		public float SpellCritMultiplier { get; private set; }

		public float ChanceNonAvoided { get { return 1f - ChanceAvoided; } }
		public float AttackPower { get { return Stats.AttackPower; } }
		public float BonusFaerieFireStacks { get { return Stats.BonusFaerieFireStacks; } }
		public float DamageMultiplier { get { return 1f + Stats.BonusDamageMultiplier; } }
		public float PhysicalDamageMultiplier { get { return 1f + Stats.BonusPhysicalDamageMultiplier; } }
		public float NatureDamageMultiplier { get { return 1f + Stats.BonusNatureDamageMultiplier; } }
		public float MaulDamageMultiplier { get { return 1f + Stats.BonusMaulDamageMultiplier; } }
		public float BleedDamageMultiplier { get { return 1f + Stats.BonusBleedDamageMultiplier; } }
		public float ThreatMultiplier { get { return 2f * (1f + Stats.ThreatIncreaseMultiplier); } }

		public BearAbilityBuilder(StatsBear stats, float weaponDPS, float attackSpeed, float armorDamageMultiplier,
			float chanceAvoided, float chanceCrit, float chanceCritSpell, float chanceGlance, float critMultiplier, float spellCritMultiplier)
		{
			Stats = stats;
			WeaponDPS = weaponDPS;
			AttackSpeed = attackSpeed;
			ArmorDamageMultiplier = armorDamageMultiplier;
			ChanceAvoided = chanceAvoided;
			ChanceCrit = chanceCrit;
			ChanceCritSpell = chanceCritSpell;
			ChanceGlance = chanceGlance;
			CritMultiplier = critMultiplier;
			SpellCritMultiplier = spellCritMultiplier;
			BaseDamage = (WeaponDPS + AttackPower / 14f) * 2.5f;
		}

		private MeleeStats _meleeStats = null;
		public MeleeStats MeleeStats
		{
			get
			{
				if (_meleeStats == null)
				{
					_meleeStats = new MeleeStats();
					_meleeStats.Speed = AttackSpeed;

					_meleeStats.DamageRaw = BaseDamage * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_meleeStats.ThreatRaw = _meleeStats.DamageRaw * ThreatMultiplier;
					_meleeStats.DamageAverage = ((ChanceAvoided) * 0f) +
												((ChanceCrit) * (_meleeStats.DamageRaw * CritMultiplier)) +
												((ChanceGlance) * (_meleeStats.DamageRaw * 0.7f)) +
												((1f - ChanceAvoided - ChanceCrit - ChanceGlance) * (_meleeStats.DamageRaw));
					_meleeStats.ThreatAverage = _meleeStats.DamageAverage * ThreatMultiplier;
					
					_meleeStats.DamageFurySwipesRaw = _meleeStats.DamageRaw * 2f;
					_meleeStats.ThreatFurySwipesRaw = _meleeStats.DamageFurySwipesRaw * ThreatMultiplier;
					_meleeStats.DamageFurySwipesAverage = ChanceNonAvoided *
															((ChanceCrit) * (_meleeStats.DamageFurySwipesRaw * CritMultiplier)) +
															((1f - ChanceCrit) * (_meleeStats.DamageFurySwipesRaw));
					_meleeStats.ThreatFurySwipesAverage = _meleeStats.DamageFurySwipesAverage * ThreatMultiplier;
				}
				return _meleeStats;
			}
		}

		private ThornsStats _thornsStats = null;
		public ThornsStats ThornsStats
		{
			get
			{
				if (_thornsStats == null)
				{
					_thornsStats = new ThornsStats();
					_thornsStats.DamageRaw = (447f + AttackPower * 0.421f) * DamageMultiplier * NatureDamageMultiplier;
					_thornsStats.ThreatRaw = _thornsStats.DamageRaw * ThreatMultiplier;
					_thornsStats.DamageAverage =	((ChanceCritSpell) * _thornsStats.DamageRaw * SpellCritMultiplier) +
													((1f - ChanceCritSpell) * _thornsStats.DamageRaw);
					_thornsStats.ThreatAverage = _thornsStats.DamageAverage * ThreatMultiplier;
				}
				return _thornsStats;
			}
		}

		private FaerieFireStats _faerieFireStats = null;
		public FaerieFireStats FaerieFireStats
		{
			get
			{
				if (_faerieFireStats == null)
				{
					_faerieFireStats = new FaerieFireStats();
					_faerieFireStats.DamageRaw = (1f + AttackPower * 0.15f) * DamageMultiplier * NatureDamageMultiplier;
					_faerieFireStats.ThreatRaw = (774f + _faerieFireStats.DamageRaw + (BonusFaerieFireStacks * 48f)) * ThreatMultiplier;

					//TODO: Need to add ChanceResisted
				}
				return _faerieFireStats;
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
					_mangleStats.RageCostRaw = 15f;
					_mangleStats.DamageRaw = (667f + BaseDamage * 2.3f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_mangleStats.ThreatRaw = _mangleStats.DamageRaw * ThreatMultiplier;
				}
				return _mangleStats;
			}
		}

		private MaulStats _maulStats = null;
		public MaulStats MaulStats
		{
			get
			{
				if (_maulStats == null)
				{
					_maulStats = new MaulStats();
					_maulStats.RageCostRaw = 30f;
					_maulStats.DamageRaw = (8f + AttackPower * 0.36f) * MaulDamageMultiplier * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_maulStats.ThreatRaw = (30f + _maulStats.DamageRaw) * ThreatMultiplier;
				}
				return _maulStats;
			}
		}

		private PulverizeStats _pulverizeStats = null;
		public PulverizeStats PulverizeStats
		{
			get
			{
				if (_pulverizeStats == null)
				{
					_pulverizeStats = new PulverizeStats();
					_pulverizeStats.RageCostRaw = 15f;
					_pulverizeStats.DamageRaw = ((451f * 0f) + BaseDamage * 1.2f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.ThreatRaw = _pulverizeStats.DamageRaw * ThreatMultiplier;
					_pulverizeStats.Damage1Raw = ((451f * 1f) + BaseDamage * 1.2f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat1Raw = _pulverizeStats.Damage1Raw * ThreatMultiplier;
					_pulverizeStats.Damage2Raw = ((451f * 2f) + BaseDamage * 1.2f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat2Raw = _pulverizeStats.Damage2Raw * ThreatMultiplier;
					_pulverizeStats.Damage3Raw = ((451f * 3f) + BaseDamage * 1.2f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat3Raw = _pulverizeStats.Damage3Raw * ThreatMultiplier;
				}
				return _pulverizeStats;
			}
		}

		private SwipeStats _swipeStats = null;
		public SwipeStats SwipeStats
		{
			get
			{
				if (_swipeStats == null)
				{
					_swipeStats = new SwipeStats();
					_swipeStats.RageCostRaw = 30f;
					_swipeStats.DamageRaw = (427f + AttackPower * 0.3416f) * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_swipeStats.ThreatRaw = (_swipeStats.DamageRaw * 1.5f) * ThreatMultiplier;
				}
				return _swipeStats;
			}
		}

		private ThrashStats _thrashStats = null;
		public ThrashStats ThrashStats
		{
			get
			{
				if (_thrashStats == null)
				{
					_thrashStats = new ThrashStats();
					_thrashStats.RageCostRaw = 25f;
					_thrashStats.DamageInitialRaw = (324f + AttackPower * 0.192f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_thrashStats.ThreatInitialRaw = (_thrashStats.DamageInitialRaw * 1.5f) * ThreatMultiplier;
					_thrashStats.DamageTickRaw = (189f + AttackPower * 0.033f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_thrashStats.ThreatTickRaw = (_thrashStats.DamageTickRaw * 1.5f) * ThreatMultiplier;
					_thrashStats.DamageRaw = _thrashStats.DamageInitialRaw + _thrashStats.DamageTickRaw * 3f;
					_thrashStats.ThreatRaw = _thrashStats.ThreatInitialRaw + _thrashStats.ThreatTickRaw * 3f;
				}
				return _thrashStats;
			}
		}

		private LacerateStats _lacerateStats = null;
		public LacerateStats LacerateStats
		{
			get
			{
				if (_lacerateStats == null)
				{
					_lacerateStats = new LacerateStats();
					_lacerateStats.RageCostRaw = 15f;
					_lacerateStats.DamageRaw = (280f + AttackPower * 0.14f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_lacerateStats.ThreatRaw = (2269f + _lacerateStats.DamageRaw) * ThreatMultiplier;
					_lacerateStats.DamageTick1Raw = 1f * (23f + AttackPower * 0.00776f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick1Raw = _lacerateStats.DamageTick1Raw * ThreatMultiplier;
					_lacerateStats.DamageTick2Raw = 2f * (23f + AttackPower * 0.00776f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick2Raw = _lacerateStats.DamageTick2Raw * ThreatMultiplier;
					_lacerateStats.DamageTick3Raw = 3f * (23f + AttackPower * 0.00776f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick3Raw = _lacerateStats.DamageTick3Raw * ThreatMultiplier;
				}
				return _lacerateStats;
			}
		}
	}

	public abstract class BearAbilityStats
	{
		public float RageCostRaw { get; set; }
		public float DamageRaw { get; set; }
		public float ThreatRaw { get; set; }
		public float RageCostAverage { get; set; }
		public float DamageAverage { get; set; }
		public float ThreatAverage { get; set; }

		public override string ToString()
		{
			return string.Format("{0}dmg, {1}thr, {2}dpr, {3}tpr", DamageRaw, ThreatRaw, DamageRaw/RageCostRaw, ThreatRaw/RageCostRaw);
		}
	}

	public class MeleeStats : BearAbilityStats
	{
		public float Speed { get; set; }
		public float DamageFurySwipesRaw { get; set; }
		public float ThreatFurySwipesRaw { get; set; }
		public float DamageFurySwipesAverage { get; set; }
		public float ThreatFurySwipesAverage { get; set; }
	}

	public class ThornsStats : BearAbilityStats
	{
	}

	public class FaerieFireStats : BearAbilityStats
	{
	}

	public class MangleStats : BearAbilityStats
	{
	}

	public class MaulStats : BearAbilityStats
	{
	}

	public class PulverizeStats : BearAbilityStats
	{
		public float Damage1Raw { get; set; }
		public float Threat1Raw { get; set; }
		public float Damage2Raw { get; set; }
		public float Threat2Raw { get; set; }
		public float Damage3Raw { get; set; }
		public float Threat3Raw { get; set; }
		public float Damage1Average { get; set; }
		public float Threat1Average { get; set; }
		public float Damage2Average { get; set; }
		public float Threat2Average { get; set; }
		public float Damage3Average { get; set; }
		public float Threat3Average { get; set; }
	}

	public class SwipeStats : BearAbilityStats
	{
	}

	public class ThrashStats : BearAbilityStats
	{
		public float DamageInitialRaw { get; set; }
		public float ThreatInitialRaw { get; set; }
		public float DamageTickRaw { get; set; }
		public float ThreatTickRaw { get; set; }
		public float DamageInitialAverage { get; set; }
		public float ThreatInitialAverage { get; set; }
		public float DamageTickAverage { get; set; }
		public float ThreatTickAverage { get; set; }
	}

	public class LacerateStats : BearAbilityStats
	{
		public float DamageTick1Raw { get; set; }
		public float ThreatTick1Raw { get; set; }
		public float DamageTick2Raw { get; set; }
		public float ThreatTick2Raw { get; set; }
		public float DamageTick3Raw { get; set; }
		public float ThreatTick3Raw { get; set; }
		public float DamageTick1Average { get; set; }
		public float ThreatTick1Average { get; set; }
		public float DamageTick2Average { get; set; }
		public float ThreatTick2Average { get; set; }
		public float DamageTick3Average { get; set; }
		public float ThreatTick3Average { get; set; }
	}
}
