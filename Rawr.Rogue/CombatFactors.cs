namespace Rawr.Rogue
{
    public class CombatFactors
    {
        public CombatFactors(Character character, Stats stats)
        {
            _stats = stats;
            _mainHand = character.MainHand ?? new Knuckles();
            _offHand = character.OffHand ?? new Knuckles();
            _talents = character.RogueTalents;
            _calcOpts = character.CalculationOptions as CalculationOptionsRogue;
            _characterRace = character.Race;
        }

        private readonly Stats _stats;
        private readonly Item _mainHand;
        private readonly Item _offHand;
        private readonly RogueTalents _talents;
        private readonly CalculationOptionsRogue _calcOpts;
        private Character.CharacterRace _characterRace;

        public Item MainHand
        {
            get { return _mainHand; }
        }

        public Item OffHand
        {
            get { return _offHand; }
        }

        public float DamageReduction
        {
            get
            {
                var totalArmor = _calcOpts.TargetArmor - _stats.ArmorPenetration;
                return 1f - (totalArmor / (totalArmor + 10557.5f)); 
            }
        } 

        public float AvgMhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(MainHand, _stats); }
        }

        public float AvgOhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(OffHand, _stats); }
        }

        public float YellowMissChance
        {
            get
            {
                var missChance = 5f - HitPercent;
                return missChance < 0f ? 0f : missChance;
            }
        }

        public float WhiteMissChance
        {
            get
            {
                var missChance = 28f - HitPercent;
                return missChance < 0f ? 0f : missChance; 
            }
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

        public float MhCrit
        {
            get { return CalcCrit(MainHand); }
        }

        public float OhCrit
        {
            get { return CalcCrit(OffHand); }
        }

        public float ProbMhWhiteHit
        {
            get { return 1f - WhiteMissChance / 100f - MhDodgeChance / 100f; }
        }

        public float ProbOhWhiteHit
        {
            get { return 1f - WhiteMissChance / 100f - OhDodgeChance / 100f; }
        }
        
        public float TotalHaste
        {
            get
            {   
                //TODO:  Add WindFury Totem (a straight haste bonus as of patch 3.0)
                var sndHaste = .3f;
                sndHaste *= (1f + _stats.BonusSnDHaste);

                var totalHaste = 1f;
                totalHaste *= (1f + sndHaste) * (1f + (_stats.HasteRating * RogueConversions.HasteRatingToHaste) / 100);
                totalHaste *= (1f + .2f * 15f / 120f * _talents.BladeFlurry);
                return totalHaste; 
            }
        }

        public float BonusWhiteCritDmg
        {
            get { return 1f + _stats.BonusCritMultiplier; }
        }

        public float ProbPoisonHit
        {
            get
            {
                var missChance = 17f - HitPercent;
                return missChance < 0f ? 0f : 1f - missChance / 100f;
            }
        }

        public float BaseEnergyRegen
        {
            get
            {
                var energyRegen = 10f;
                if (_talents.AdrenalineRush > 0)
                {
                    energyRegen += .5f;
                }
                return energyRegen;
            }
        }

        public float HitPercent
        {
            get
            {
                return _talents.Precision + _stats.PhysicalHit + _stats.HitRating * RogueConversions.HitRatingToHit;
            }
        }

        private float CalcCrit(Item weapon)
        {
            var crit = _stats.PhysicalCrit + _stats.CritRating * RogueConversions.CritRatingToCrit;
            if (weapon.Type == Item.ItemType.Dagger || weapon.Type == Item.ItemType.FistWeapon)
            {
                crit += _talents.CloseQuartersCombat;
            }
            return crit;
        }

        private float CalcExpertise(Item weapon)
        {
            var baseExpertise = _talents.WeaponExpertise * 5f + _stats.Expertise + _stats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise;

            if (_characterRace == Character.CharacterRace.Human)
            {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandSword || weapon.Type == Item.ItemType.OneHandMace))
                    baseExpertise += 5f;
            }

            return baseExpertise;
        }

        private static float CalcDodgeChance(float mhExpertise)
        {
            var mhDodgeChance = 6.5f - .25f * mhExpertise;

            if (mhDodgeChance < 0f)
                mhDodgeChance = 0f;
            return mhDodgeChance;
        }

        private static float CalcAverageWeaponDamage(Item weapon, Stats stats)
        {
            return (weapon.MinDamage + weapon.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
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