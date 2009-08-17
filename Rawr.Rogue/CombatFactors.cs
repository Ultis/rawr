using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
            _stats = stats;
			_mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
			_offHand = character.OffHand == null ? new Knuckles() : character.OffHand.Item;
            _calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            _character = character;
        }

        private readonly Stats _stats;
        private readonly Item _mainHand;
        private readonly Item _offHand;
        private readonly CalculationOptionsRogue _calcOpts;
        private readonly Character _character;

        public Item MainHand { get { return _mainHand; } }
        public Item OffHand { get { return _offHand; } }

        public float MhNormalizedAttackSpeed { get { return MainHand.Type == ItemType.Dagger ? 1.7f : 2.4f; } }
        public float OhNormalizedAttackSpeed { get { return OffHand.Type == ItemType.Dagger ? 1.7f : 2.4f; } }
        public float DamageReduction { get { return 1f - ArmorDamageReduction; } } 
        public float ArmorDamageReduction { get { return StatConversion.GetArmorDamageReduction(80, _calcOpts.TargetArmor - Talents.SerratedBlades.ArmorPenetration.Bonus, 0, 0, _stats.ArmorPenetrationRating);} }
        public float AvgMhWeaponDmg { get { return CalcAverageWeaponDamage(MainHand, _stats); } }
        public float MhAvgDamage { get { return AvgMhWeaponDmg + (_stats.AttackPower / 14.0f) * MainHand.Speed; } }
        public float MhNormalizedDamage { get { return AvgMhWeaponDmg + (_stats.AttackPower / 14.0f) * MhNormalizedAttackSpeed; } }
        public float AvgOhWeaponDmg { get { return CalcAverageWeaponDamage(OffHand, _stats); } }
        public float OhAvgDamage { get { return (AvgOhWeaponDmg + (_stats.AttackPower/14.0f)*OffHand.Speed)*OffHandDamagePenalty; } }
        public float OhNormalizedDamage { get { return (AvgOhWeaponDmg + (_stats.AttackPower/14.0f)*OhNormalizedAttackSpeed)*OffHandDamagePenalty; } }
        public float OffHandDamagePenalty { get { return 0.5f + Talents.DualWieldSpecialization.Bonus; } }
        public float YellowMissChance { get { return Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[  _calcOpts.TargetLevel - 80] - HitPercent); } }
        public float WhiteMissChance  { get { return Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP_DW[_calcOpts.TargetLevel - 80] - HitPercent); } }
        public float BaseExpertise { get { return Talents.WeaponExpertise.Bonus + _stats.Expertise + (float)Math.Floor(StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating,CharacterClass.Rogue)); } }
        public float MhExpertise { get { return CalcExpertise(MainHand); } }
        public float OhExpertise { get { return CalcExpertise(OffHand); } }
        public float MhDodgeChance { get { return CalcDodgeChance(MhExpertise); } }
        public float OhDodgeChance { get { return CalcDodgeChance(OhExpertise); } }
        public float ProbMhCrit { get { return CalcCrit(MainHand)/100f + StatConversion.NPC_LEVEL_CRIT_MOD[_calcOpts.TargetLevel-80]; } }
        public float ProbOhCrit { get { return CalcCrit(OffHand )/100f + StatConversion.NPC_LEVEL_CRIT_MOD[_calcOpts.TargetLevel-80]; } }
        public float ProbGlancingHit { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[_calcOpts.TargetLevel-80]; } }
        //assume parry & glance are always 0 for a yellow attack
        public float ProbYellowHit { get { return 1 - YellowMissChance - MhDodgeChance; } }
        public float ProbMhWhiteHit { get { return 1f - WhiteMissChance - MhDodgeChance - ProbGlancingHit - ProbMhCrit; } }
        public float ProbOhWhiteHit { get { return 1f - WhiteMissChance - OhDodgeChance - ProbGlancingHit - ProbOhCrit; } }
        public float BonusWhiteCritDmg { get { return 1f + _stats.BonusCritMultiplier; } }
        public float BaseCritMultiplier { get { return 2f * BonusWhiteCritDmg; } }
        public float BaseEnergyRegen {
            get {
                var energyRegen = 10f;
                energyRegen += Talents.Vitality.Bonus;
                energyRegen += Talents.AdrenalineRush.Energy.Bonus;
                energyRegen += Talents.HungerForBlood.EnergyPerSecond.Bonus;
                energyRegen -= Talents.BladeFlurry.EnergyCost.Bonus;
                energyRegen -= _calcOpts.Feint.EnergyCost();
                return energyRegen;
            }
        }
        //Need to Add 30% SnD   formerly:  totalHaste *= (1f + .3f * (1f + _stats.BonusSnDHaste));  //TODO:  change from assuming SnD has a 100% uptime
        public float Haste { get { return _stats.PhysicalHaste; } set { _stats.PhysicalHaste = value; } }
        public float HitPercent { get { return _stats.PhysicalHit/100f + StatConversion.GetHitFromRating(_stats.HitRating); } }
        public float ProbPoisonHit { get { return 1f + (PoisonHitPercent <= .17f ? PoisonHitPercent - 0.17f : 0f); } }
        public float PoisonHitPercent { get { return _stats.PhysicalHit/100f + StatConversion.GetSpellHitFromRating(_stats.HitRating); } }
        public float CritFromCritRating { get { return StatConversion.GetCritFromRating(_stats.CritRating); } }
        public float Tier7TwoPieceRuptureBonusDamage { get { return 1f + _stats.RogueT7TwoPieceBonus * 0.1f; } }
        public float Tier7FourPieceEnergyCostReduction { get{ return ( 1f - _stats.RogueT7FourPieceBonus * 0.05f);} }
        //1 energy per Deadly Poison tick, 1 tick every 3 seconds
        public float Tier8TwoPieceEnergyBonus { get { return _stats.RogueT8TwoPieceBonus == 1 ? 1f / 3f : 0f; } }
        public float Tier8FourPieceRuptureCrit { get { return 1 + (_stats.RogueT8FourPieceBonus == 1 ? ProbMhCrit : 0f); } }
        private float CalcCrit(Item weapon) {
            var crit = _stats.PhysicalCrit + CritFromCritRating;
            if (weapon.Type == ItemType.Dagger || weapon.Type == ItemType.FistWeapon) {
                crit += Talents.CloseQuartersCombat.Bonus;
            }
            return crit;
        }
        private float CalcExpertise(Item weapon) {
            var baseExpertise = BaseExpertise;

            if (_character.Race == CharacterRace.Human) {
                if (weapon != null && (weapon.Type == ItemType.OneHandSword || weapon.Type == ItemType.OneHandMace))
                {
                    baseExpertise += 5f;
                }
            }

            return baseExpertise;
        }
        private static float CalcDodgeChance(float weaponExpertise) {
            weaponExpertise /= 100f;
            float mhDodgeChance = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[83 /*_calcOpts.TargetLevel*/ - 80] - StatConversion.GetDodgeParryReducFromExpertise(weaponExpertise));
            return mhDodgeChance;
        }
        private static float CalcAverageWeaponDamage(Item weapon, Stats stats) { return stats.WeaponDamage + (weapon.MinDamage + weapon.MaxDamage) / 2.0f; }
        public class Knuckles : Item { public Knuckles() { Speed = 2f; } }
    }
}