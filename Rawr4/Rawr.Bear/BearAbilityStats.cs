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
		public float ChanceResisted { get; private set; }
		public float ChanceCrit { get; private set; }
		public float ChanceCritSpell { get; private set; }
		public float ChanceGlance { get; private set; }
		public float CritMultiplier { get; private set; }
		public float SpellCritMultiplier { get; private set; }

		public float ChanceNonAvoided { get { return 1f - ChanceAvoided; } }
		public float ChanceNonResisted { get { return 1f - ChanceResisted; } }
		public float AttackPower { get { return Stats.AttackPower; } }
		public float FurySwipesChance { get { return Stats.FurySwipesChance; } }
		public float BonusFaerieFireStacks { get { return Stats.BonusFaerieFireStacks; } }
		public float DamageMultiplier { get { return 1f + Stats.BonusDamageMultiplier; } }
        public float WhiteDamageMultiplier { get { return 1f + Stats.BonusWhiteDamageMultiplier; } }
		public float PhysicalDamageMultiplier { get { return 1f + Stats.BonusPhysicalDamageMultiplier; } }
		public float NatureDamageMultiplier { get { return 1f + Stats.BonusNatureDamageMultiplier; } }
		public float MaulDamageMultiplier { get { return 1f + Stats.BonusMaulDamageMultiplier; } }
		public float MangleDamageMultiplier { get { return 1f + Stats.BonusMangleDamageMultiplier; } }
		public float BleedDamageMultiplier { get { return 1f + Stats.BonusBleedDamageMultiplier; } }
		public float ThreatMultiplier { get { return 3f * (1f + Stats.ThreatIncreaseMultiplier); } }

		public BearAbilityBuilder(StatsBear stats, float weaponDPS, float attackSpeed, float armorDamageMultiplier,
			float chanceAvoided, float chanceResisted, float chanceCrit, float chanceCritSpell, float chanceGlance, float critMultiplier, float spellCritMultiplier)
		{
			Stats = stats;
			WeaponDPS = weaponDPS;
			AttackSpeed = attackSpeed;
			ArmorDamageMultiplier = armorDamageMultiplier;
			ChanceAvoided = chanceAvoided;
			ChanceResisted = chanceResisted;
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

					_meleeStats.DamageRaw = BaseDamage * DamageMultiplier * WhiteDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_meleeStats.ThreatRaw = _meleeStats.DamageRaw * ThreatMultiplier;
					_meleeStats.DamageAverage = ((ChanceAvoided) * 0f) +
												((ChanceCrit) * (_meleeStats.DamageRaw * CritMultiplier)) +
												((ChanceGlance) * (_meleeStats.DamageRaw * 0.7f)) +
												((1f - ChanceAvoided - ChanceCrit - ChanceGlance) * (_meleeStats.DamageRaw));
					_meleeStats.ThreatAverage = _meleeStats.DamageAverage * ThreatMultiplier;
					
					_meleeStats.DamageFurySwipesRaw = _meleeStats.DamageRaw * 3.1f;
					_meleeStats.ThreatFurySwipesRaw = _meleeStats.DamageFurySwipesRaw * ThreatMultiplier;
					_meleeStats.DamageFurySwipesAverage =	ChanceNonAvoided * (
																((ChanceCrit) * (_meleeStats.DamageFurySwipesRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_meleeStats.DamageFurySwipesRaw))
															);
					_meleeStats.ThreatFurySwipesAverage = _meleeStats.DamageFurySwipesAverage * ThreatMultiplier;

					_meleeStats.DPSTotalAverage = _meleeStats.DamageAverage / AttackSpeed;
					_meleeStats.TPSTotalAverage = _meleeStats.ThreatAverage / AttackSpeed;

					if (FurySwipesChance > 0)
					{
						int attacksWithinCooldown = (int)Math.Floor(3f / AttackSpeed);
						float attacksPerProc = (1f / FurySwipesChance) + attacksWithinCooldown;
						float timePerProc = attacksPerProc * AttackSpeed;
						_meleeStats.DPSTotalAverage += _meleeStats.DamageFurySwipesAverage / timePerProc;
						_meleeStats.TPSTotalAverage += _meleeStats.ThreatFurySwipesAverage / timePerProc;
					}
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
					_thornsStats.DamageRaw = (214f + AttackPower * 0.168f) * DamageMultiplier * NatureDamageMultiplier;
					_thornsStats.ThreatRaw = _thornsStats.DamageRaw * ThreatMultiplier;
					_thornsStats.DamageAverage =	((ChanceCritSpell) * (_thornsStats.DamageRaw * SpellCritMultiplier)) +
													((1f - ChanceCritSpell) * (_thornsStats.DamageRaw));
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
					_faerieFireStats.DamageAverage =	ChanceNonResisted * (
															((ChanceCritSpell) * (_faerieFireStats.DamageRaw * SpellCritMultiplier)) +
															((1f - ChanceCritSpell) * (_faerieFireStats.DamageRaw))
														);
					_faerieFireStats.ThreatAverage = _faerieFireStats.DamageAverage * ThreatMultiplier;
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
					_mangleStats.DamageRaw = (290f + BaseDamage) * 2.35f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier * MangleDamageMultiplier;
					_mangleStats.ThreatRaw = _mangleStats.DamageRaw * ThreatMultiplier;
					_mangleStats.DamageAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_mangleStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_mangleStats.DamageRaw))
															);
					_mangleStats.ThreatAverage = _mangleStats.DamageAverage * ThreatMultiplier;
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
					_maulStats.DamageRaw = (8f + AttackPower * 0.24f) * MaulDamageMultiplier * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_maulStats.ThreatRaw = (30f + _maulStats.DamageRaw) * ThreatMultiplier;
					_maulStats.DamageAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_maulStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_maulStats.DamageRaw))
															);
					_maulStats.ThreatAverage = _maulStats.DamageAverage * ThreatMultiplier;
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
					_pulverizeStats.DamageRaw = ((451f * 0f) + BaseDamage) * 0.8f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.ThreatRaw = _pulverizeStats.DamageRaw * ThreatMultiplier;
					_pulverizeStats.Damage1Raw = ((451f * 1f) + BaseDamage) * 0.8f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat1Raw = _pulverizeStats.Damage1Raw * ThreatMultiplier;
					_pulverizeStats.Damage2Raw = ((451f * 2f) + BaseDamage) * 0.8f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat2Raw = _pulverizeStats.Damage2Raw * ThreatMultiplier;
					_pulverizeStats.Damage3Raw = ((451f * 3f) + BaseDamage) * 0.8f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_pulverizeStats.Threat3Raw = _pulverizeStats.Damage3Raw * ThreatMultiplier;
					_pulverizeStats.DamageAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_pulverizeStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_pulverizeStats.DamageRaw))
															);
					_pulverizeStats.ThreatAverage = _pulverizeStats.DamageAverage * ThreatMultiplier;
					_pulverizeStats.Damage1Average = ChanceNonAvoided * (
																((ChanceCrit) * (_pulverizeStats.Damage1Raw * CritMultiplier)) +
																((1f - ChanceCrit) * (_pulverizeStats.Damage1Raw))
															);
					_pulverizeStats.Threat1Average = _pulverizeStats.Damage1Average * ThreatMultiplier;
					_pulverizeStats.Damage2Average = ChanceNonAvoided * (
																((ChanceCrit) * (_pulverizeStats.Damage2Raw * CritMultiplier)) +
																((1f - ChanceCrit) * (_pulverizeStats.Damage2Raw))
															);
					_pulverizeStats.Threat2Average = _pulverizeStats.Damage2Average * ThreatMultiplier;
					_pulverizeStats.Damage3Average = ChanceNonAvoided * (
																((ChanceCrit) * (_pulverizeStats.Damage3Raw * CritMultiplier)) +
																((1f - ChanceCrit) * (_pulverizeStats.Damage3Raw))
															);
					_pulverizeStats.Threat3Average = _pulverizeStats.Damage3Average * ThreatMultiplier;
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
					_swipeStats.RageCostRaw = 15f;
					_swipeStats.DamageRaw = (710f + AttackPower * 0.073f) * 0.83f * DamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_swipeStats.ThreatRaw = (_swipeStats.DamageRaw * 1.5f) * ThreatMultiplier;
					_swipeStats.DamageAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_swipeStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_swipeStats.DamageRaw))
															);
					_swipeStats.ThreatAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_swipeStats.ThreatRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_swipeStats.ThreatRaw))
															);
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
					_thrashStats.DamageInitialRaw = (225f + AttackPower * 0.16f) * 0.83f * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_thrashStats.ThreatInitialRaw = (_thrashStats.DamageInitialRaw * 1.5f) * ThreatMultiplier;
					_thrashStats.DamageTickRaw = (189f + AttackPower * 0.033f) * 0.83f * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_thrashStats.ThreatTickRaw = (_thrashStats.DamageTickRaw * 1.5f) * ThreatMultiplier;
					_thrashStats.DamageRaw = _thrashStats.DamageInitialRaw + _thrashStats.DamageTickRaw * 3f;
					_thrashStats.ThreatRaw = _thrashStats.ThreatInitialRaw + _thrashStats.ThreatTickRaw * 3f;
					_thrashStats.DamageAverage =	ChanceNonAvoided * (
														((ChanceCrit) * (_thrashStats.DamageRaw * CritMultiplier)) +
														((1f - ChanceCrit) * (_thrashStats.DamageRaw))
													);
					_thrashStats.ThreatAverage =	ChanceNonAvoided * (
														((ChanceCrit) * (_thrashStats.ThreatRaw * CritMultiplier)) +
														((1f - ChanceCrit) * (_thrashStats.ThreatRaw))
													);
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
					_lacerateStats.DamageRaw = (197f + AttackPower * 0.0766f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier * ArmorDamageMultiplier;
					_lacerateStats.ThreatRaw = (2269f + _lacerateStats.DamageRaw) * ThreatMultiplier;
					_lacerateStats.DamageTick1Raw = 1f * (15f + AttackPower * 0.00512f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick1Raw = _lacerateStats.DamageTick1Raw * ThreatMultiplier;
					_lacerateStats.DamageTick2Raw = 2f * (15f + AttackPower * 0.00512f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick2Raw = _lacerateStats.DamageTick2Raw * ThreatMultiplier;
					_lacerateStats.DamageTick3Raw = 3f * (15f + AttackPower * 0.00512f) * DamageMultiplier * BleedDamageMultiplier * PhysicalDamageMultiplier;
					_lacerateStats.ThreatTick3Raw = _lacerateStats.DamageTick3Raw * ThreatMultiplier;
					_lacerateStats.DamageAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_lacerateStats.DamageRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_lacerateStats.DamageRaw))
															);
					_lacerateStats.ThreatAverage = ChanceNonAvoided * (
																((ChanceCrit) * (_lacerateStats.ThreatRaw * CritMultiplier)) +
																((1f - ChanceCrit) * (_lacerateStats.ThreatRaw))
															);
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
			return string.Format("{0:N0}Dmg, {1:N0}Threat*Per Hit: {0:N0} damage, {1:N0} threat\r\nPer Swing: {2:N0} damage, {3:N0} threat{4:N0}",
				DamageRaw, ThreatRaw, DamageAverage, ThreatAverage, RageCostRaw == 0 ? "" :
				(string.Format("\r\nPer Rage: {0:N0} damage, {1:N0} threat", DamageRaw / RageCostRaw, ThreatRaw / RageCostRaw)));
		}
	}

	public class MeleeStats : BearAbilityStats
	{
		public float Speed { get; set; }
		public float DamageFurySwipesRaw { get; set; }
		public float ThreatFurySwipesRaw { get; set; }
		public float DamageFurySwipesAverage { get; set; }
		public float ThreatFurySwipesAverage { get; set; }
		public float DPSTotalAverage { get; set; }
		public float TPSTotalAverage { get; set; }

		public override string ToString()
		{
			return base.ToString() + string.Format("\r\nPer Second: {0:N0} damage, {1:N0} threat\r\nFury Swipes Hit: {2:N0} damage, {3:N0} threat\r\nFury Swipes Swing: {4:N0} damage, {5:N0} threat\r\nTotal: {6:N0} dps, {7:N0} tps", 
				DamageAverage / Speed, ThreatAverage / Speed, DamageFurySwipesRaw, ThreatFurySwipesRaw, DamageFurySwipesAverage, ThreatFurySwipesAverage, DPSTotalAverage, TPSTotalAverage);
		}
	}

	public class ThornsStats : BearAbilityStats
	{}

	public class FaerieFireStats : BearAbilityStats
	{}

	public class MangleStats : BearAbilityStats
	{}

	public class MaulStats : BearAbilityStats
	{}

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

		public override string ToString()
		{
			return string.Format("{3:N0}Dmg, {7:N0}Threat*Per Hit (0|1|2|3 Lacerates): {0:N0}|{1:N0}|{2:N0}|{3:N0} damage, {4:N0}|{5:N0}|{6:N0}|{7:N0} threat\r\nPer Swing (0|1|2|3 Lacerates): {8:N0}|{9:N0}|{10:N0}|{11:N0} damage, {12:N0}|{13:N0}|{14:N0}|{15:N0} threat\r\nPer Rage (0|1|2|3 Lacerates): {16:N0}|{17:N0}|{18:N0}|{19:N0} damage, {20:N0}|{21:N0}|{22:N0}|{23:N0} threat",
				DamageRaw, Damage1Raw, Damage2Raw, Damage3Raw, ThreatRaw, Threat1Raw, Threat2Raw, Threat3Raw,
				DamageAverage, Damage1Average, Damage1Average, Damage3Average, ThreatAverage, Threat1Average, Threat2Average, Threat3Average, 
				DamageRaw / RageCostRaw, Damage1Raw / RageCostRaw, Damage2Raw / RageCostRaw, Damage3Raw / RageCostRaw, ThreatRaw / RageCostRaw, Threat1Raw / RageCostRaw, Threat2Raw / RageCostRaw, Threat3Raw / RageCostRaw);
		}
	}

	public class SwipeStats : BearAbilityStats
	{}

	public class ThrashStats : BearAbilityStats
	{
		public float DamageInitialRaw { get; set; }
		public float ThreatInitialRaw { get; set; }
		public float DamageTickRaw { get; set; }
		public float ThreatTickRaw { get; set; }

		public override string ToString()
		{
			return string.Format("{0:N0}Dmg, {1:N0}Threat*Per Hit: {0:N0} damage, {1:N0} threat\r\nPer Swing: {2:N0} damage, {3:N0} threat{4:N0}",
				DamageRaw, ThreatRaw, DamageAverage, ThreatAverage, RageCostRaw == 0 ? "" :
				(string.Format("\r\nPer Rage: {0:N0} damage, {1:N0} threat", DamageRaw / RageCostRaw, ThreatRaw / RageCostRaw)));
		}
	}

	public class LacerateStats : BearAbilityStats
	{
		public float DamageTick1Raw { get; set; }
		public float ThreatTick1Raw { get; set; }
		public float DamageTick2Raw { get; set; }
		public float ThreatTick2Raw { get; set; }
		public float DamageTick3Raw { get; set; }
		public float ThreatTick3Raw { get; set; }
	}
}
