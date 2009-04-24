using System;

namespace Rawr.Rogue
{
    public class CombatFactors
    {
        public CombatFactors(Character character, Stats stats)
        {
            _stats = stats;
			_mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
			_offHand = character.OffHand == null ? new Knuckles() : character.OffHand.Item;
			//_talents = character.RogueTalents;
            _calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            _characterRace = character.Race;
        }

        private readonly Stats _stats;
        private readonly Item _mainHand;
        private readonly Item _offHand;
        //private readonly RogueTalents _talents;
        private readonly CalculationOptionsRogue _calcOpts;
        private readonly Character.CharacterRace _characterRace;

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
            get { return MainHand.Type == Item.ItemType.Dagger ? 1.7f : 2.4f; }
        }

        public float OhNormalizedAttackSpeed
        {
            get { return OffHand.Type == Item.ItemType.Dagger ? 1.7f : 2.4f; }
        }

        public float TotalArmorPenetration
        {
			get
			{
				var armorReductionPercent = (1f - _stats.ArmorPenetration) * (1f - _stats.ArmorPenetrationRating / 1539.529991f);
				var reducedArmor = _calcOpts.TargetArmor * (armorReductionPercent);
				return _calcOpts.TargetArmor - reducedArmor;
			}
        }

        public float DamageReduction
        {
            get
            {
				return 1f - StatConversion.GetArmorDamageReduction(80, _calcOpts.TargetArmor, 0, 0, _stats.ArmorPenetrationRating);
            }
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

        public float ProbPoisonHit
        {
            get
            {
                var missChance = .17f - HitPercent;
                return missChance < 0f ? 0f : 1f - missChance / 100f;
            }
        }

        public float BaseEnergyRegen
        {
            get
            {
                var energyRegen = 10f*Talents.Multiply(Talents.Vitality, Talents.AdrenalineRush);
                energyRegen += Talents.HungerForBlood.EnergyPerSecond.Bonus;
                return energyRegen;
            }
        }

        public float BaseHaste
        {
            get
            {
                var totalHaste = 1f;
                totalHaste *= (1f + .3f * (1f + _stats.BonusSnDHaste));
                totalHaste *= (1f + (_stats.HasteRating * RogueConversions.HasteRatingToHaste) / 100);
                totalHaste *= Talents.BladeFlurry.Bonus;
                return totalHaste;
            }
        }

        public float HitPercent
        {
            get { return _stats.PhysicalHit/100f + _stats.HitRating*RogueConversions.HitRatingToHit/100f; }
        }

        public float CritFromCritRating
        {
            get { return _stats.CritRating*RogueConversions.CritRatingToCrit; }
        }

        private float CalcCrit(Item weapon)
        {
            var crit = _stats.PhysicalCrit + CritFromCritRating;
            if (weapon.Type == Item.ItemType.Dagger || weapon.Type == Item.ItemType.FistWeapon)
            {
                crit += Talents.CloseQuartersCombat.Bonus;
            }
            return crit;
        }

        private float CalcExpertise(Item weapon)
        {
            var baseExpertise = BaseExpertise;

            if (_characterRace == Character.CharacterRace.Human)
            {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandSword || weapon.Type == Item.ItemType.OneHandMace))
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