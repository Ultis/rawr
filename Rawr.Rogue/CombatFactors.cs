using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue
{
    public class CombatFactors
    {
        public CombatFactors(Character character, Stats stats)
        {
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

        public Item MainHand
        {
            get { return _mainHand; }
        }

        public Item OffHand
        {
            get { return _offHand; }
        }

        public float MhNormalizedAttackSpeed
        {
            get { return MainHand.Type == ItemType.Dagger ? 1.7f : 2.4f; }
        }

        public float OhNormalizedAttackSpeed
        {
            get { return OffHand.Type == ItemType.Dagger ? 1.7f : 2.4f; }
        }

        public float DamageReduction
        {
            get { return 1f - ArmorDamageReduction; }
        } 

        public float ArmorDamageReduction
        {
            get { return StatConversion.GetArmorDamageReduction(80, _calcOpts.TargetArmor - Talents.SerratedBlades.ArmorPenetration.Bonus, 0, 0, _stats.ArmorPenetrationRating);}
        }

        public float AvgMhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(MainHand, _stats); }
        }

        public float MhAvgDamage
        {
            get { return AvgMhWeaponDmg + (_stats.AttackPower / 14.0f) * MainHand.Speed; }
        }

        public float MhNormalizedDamage
        {
            get { return AvgMhWeaponDmg + (_stats.AttackPower / 14.0f) * MhNormalizedAttackSpeed; }
        }

        public float AvgOhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(OffHand, _stats); }
        }

        public float OhAvgDamage
        {
            get { return (AvgOhWeaponDmg + (_stats.AttackPower/14.0f)*OffHand.Speed)*OffHandDamagePenalty; }
        }

        public float OhNormalizedDamage
        {
            get { return (AvgOhWeaponDmg + (_stats.AttackPower/14.0f)*OhNormalizedAttackSpeed)*OffHandDamagePenalty; }
        }

        public float OffHandDamagePenalty
        {
            get { return 0.5f + Talents.DualWieldSpecialization.Bonus; }
        }

        public float YellowMissChance
        {
            get { return HitPercent > .08f ? 0f : .08f - HitPercent; }
        }

        public float WhiteMissChance
        {
            get { return HitPercent > .28f ? 0f : .28f - HitPercent; }
        }

        public float BaseExpertise
        {
            get { return Talents.WeaponExpertise.Bonus + _stats.Expertise + (float)Math.Floor(_stats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise); }
        }

        public float MhExpertise
        {
            get { return CalcExpertise(MainHand); }
        }

        public float OhExpertise
        {
            get { return CalcExpertise(OffHand); }
        }
        
        public float MhDodgeChance
        {
            get { return CalcDodgeChance(MhExpertise); }
        }

        public float OhDodgeChance
        {
            get { return CalcDodgeChance(OhExpertise); }
        }

        public float ProbMhCrit
        {
            get { return CalcCrit(MainHand)/100f + BossCriticalReductionChance; }
        }

        public float ProbOhCrit
        {
            get { return CalcCrit(OffHand)/100f + BossCriticalReductionChance; }
        }

        public float BossCriticalReductionChance
        {
            get { return 0.006f * (80 - (_calcOpts.TargetLevel == 83 ? 88 : _calcOpts.TargetLevel)); }
        }

        public float ProbGlancingHit
        {
            get { return (float)Math.Max(0.08 * (_calcOpts.TargetLevel - 80), 0); }
        }

        public float ProbYellowHit
        {
            //assume parry & glance are always 0 for a yellow attack
            get { return 1 - YellowMissChance - MhDodgeChance; }
        }

        public float ProbMhWhiteHit
        {
            get { return 1f - WhiteMissChance - MhDodgeChance - ProbGlancingHit - ProbMhCrit; }
        }

        public float ProbOhWhiteHit
        {
            get { return 1f - WhiteMissChance - OhDodgeChance - ProbGlancingHit - ProbOhCrit; }
        }

        public float BonusWhiteCritDmg
        {
            get { return 1f + _stats.BonusCritMultiplier; }
        }

        public float BaseCritMultiplier
        {
            get { return 2f * BonusWhiteCritDmg; }
        }

        public float BaseEnergyRegen
        {
            get
            {
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
        public float Haste
        {
            get { return _stats.PhysicalHaste; }
            set { _stats.PhysicalHaste = value; }
        }

        public float HitPercent
        {
            get { return _stats.PhysicalHit/100f + _stats.HitRating*RogueConversions.HitRatingToHit/100f; }
        }

        public float ProbPoisonHit
        {
            get
            {
                if(PoisonHitPercent > .17f)
                {
                    return 1f;
                }
                return 1f + PoisonHitPercent - .17f;
            }
        }

        public float PoisonHitPercent
        {
            get { return (_stats.PhysicalHit/100f) + (_stats.HitRating*RogueConversions.HitRatingToSpellHit/100f); }
        }

        public float CritFromCritRating
        {
            get { return _stats.CritRating*RogueConversions.CritRatingToCrit; }
        }

        public float Tier7TwoPieceRuptureBonusDamage
        {
            get { return 1f + _stats.RogueT7TwoPieceBonus * .1f; }    
        }

        public float Tier7FourPieceEnergyCostReduction
        {
            get{ return ( 1f - _stats.RogueT7FourPieceBonus * 0.05f);}
        }

        public float Tier8TwoPieceEnergyBonus
        {
            //1 energy per Deadly Poison tick, 1 tick every 3 seconds
            get { return _stats.RogueT8TwoPieceBonus == 1 ? 1f / 3f : 0f; }
        }

        public float Tier8FourPieceRuptureCrit
        {
            get { return 1 + (_stats.RogueT8FourPieceBonus == 1 ? ProbMhCrit : 0f); }
        }

        private float CalcCrit(Item weapon)
        {
            var crit = _stats.PhysicalCrit + CritFromCritRating;
            if (weapon.Type == ItemType.Dagger || weapon.Type == ItemType.FistWeapon)
            {
                crit += Talents.CloseQuartersCombat.Bonus;
            }
            return crit;
        }

        private float CalcExpertise(Item weapon)
        {
            var baseExpertise = BaseExpertise;

            if (_character.Race == CharacterRace.Human)
            {
                if (weapon != null && (weapon.Type == ItemType.OneHandSword || weapon.Type == ItemType.OneHandMace))
                    baseExpertise += 5f;
            }

            return baseExpertise;
        }

        private static float CalcDodgeChance(float weaponExpertise)
        {
            var mhDodgeChance = 6.5f - .25f * weaponExpertise;

            if (mhDodgeChance < 0f)
                mhDodgeChance = 0f;

            return mhDodgeChance / 100f;
        }

        private static float CalcAverageWeaponDamage(Item weapon, Stats stats)
        {
            return stats.WeaponDamage + (weapon.MinDamage + weapon.MaxDamage) / 2.0f;
        }

        public class Knuckles : Item
        {
            public Knuckles()
            {
                Speed = 2f;
            }
        }
    }
}