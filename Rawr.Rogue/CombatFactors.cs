namespace Rawr.Rogue
{
    public class CombatFactors
    {
        public CombatFactors(Character character, Stats stats)
        {
            _character = character;
            _stats = stats;
        }

        private readonly Character _character;
        private readonly Stats _stats;

        public float DamageReduction
        {
            get
            {
                var totalArmor = ((CalculationOptionsRogue)_character.CalculationOptions).TargetArmor - _stats.ArmorPenetration;
                return 1f - (totalArmor / (totalArmor + 10557.5f)); 
            }
        } 

        public float AvgMhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(_character.MainHand, _stats); }
        }

        public float AvgOhWeaponDmg
        {
            get { return CalcAverageWeaponDamage(_character.OffHand, _stats); }
        }
        
        public float MissChance
        {
            get
            {
                var missChance = 28f - HitPercent;
                return missChance < 0f ? 0f : missChance; 
            }
        }

        public float MhExpertise
        {
            get { return CalcExpertise(_character.MainHand); }
        }

        public float OhExpertise
        {
            get { return CalcExpertise(_character.OffHand); }
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
            get { return CalcCrit(_character.MainHand); }
        }

        public float OhCrit
        {
            get { return CalcCrit(_character.OffHand); }
        }

        public float ProbMHHit
        {
            get { return 1f - MissChance / 100f - MhDodgeChance / 100f; }
        }

        public float ProbOHHit
        {
            get { return 1f - MissChance / 100f - OhDodgeChance / 100f; }
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
                totalHaste *= (1f + .2f * 15f / 120f * _character.RogueTalents.BladeFlurry);
                return totalHaste; 
            }
        }

        public float BonusWhiteCritDmg
        {
            get { return 1f + _stats.BonusCritMultiplier; }
        }

        public float ProbPoison
        {
            get { return (.83f + .05f * _character.RogueTalents.MasterPoisoner) * (.2f + .02f * _character.RogueTalents.ImprovedPoisons); }
        }

        public float BaseEnergyRegen
        {
            get
            {
                var energyRegen = 10f;
                if (_character.RogueTalents.AdrenalineRush > 0)
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
                return _character.RogueTalents.Precision + _stats.PhysicalHit + _stats.HitRating * RogueConversions.HitRatingToHit;
            }
        }

        private float CalcCrit(Item weapon)
        {
            var crit = _stats.PhysicalCrit + _stats.CritRating * RogueConversions.CritRatingToCrit;
            if (weapon != null && (weapon.Type == Item.ItemType.Dagger || weapon.Type == Item.ItemType.FistWeapon))
                crit += _character.RogueTalents.CloseQuartersCombat;
            return crit;
        }

        private float CalcExpertise(Item weapon)
        {
            var baseExpertise = _character.RogueTalents.WeaponExpertise * 5f + _stats.Expertise + _stats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise;

            if (_character.Race == Character.CharacterRace.Human)
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
            return weapon == null ? 0 : (weapon.MinDamage + weapon.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
        }
    }
}