using System;

namespace Rawr.Retribution
{
    public abstract class RetAbilityBase
    {
        public abstract int Id { get; }
        public abstract bool OnCooldown { get; set; }
        public abstract float CooldownDuration { get; }

        public abstract double Fire();
    }

    public sealed class SealOfTruth : RetAbilityBase
    {
        public override int Id { get { return (int)PaladinConstants.SpellIds.SOT_ID; } }
        public override bool OnCooldown { get { return OnCooldown; } set { OnCooldown = value; } }
        public override float CooldownDuration { get { return -1; } }
        public override double Fire()
        {
            /**
             *   EJ:        (((AP/14) * Weaponspeed) + Weapon damage) * 15%
             *   Tooltip:   ((0.01 * holy power.0193 * AP) * 5 * 100 / 100)
             *    [0.013*holy power0.025*AP*5] 
             */
            throw new NotImplementedException();
        }
    }

    public sealed class TemplarsVerdict : RetAbilityBase
    {
        public override int Id
        {
            get { return (int)PaladinConstants.SpellIds.TV_ID; }
        }

        public override bool OnCooldown
        {
            get { return OnCooldown; } set { OnCooldown = value; }
        }

        public override float CooldownDuration
        {
            get { return -1; }
        }

        /// <summary>
        /// (((AP/14) * Weaponspeed) + Weapon damage) * 235%
        /// </summary>
        /// <returns></returns>
        public override double Fire()
        {
            double retVal = ((CurrentState.AP14 * CurrentState.MaxWeaponSpeed) + CurrentState.WeaponDamage) * PaladinConstants.TV_DMG_BONUS;
            return retVal;
        }
    }

    public sealed class Judgement
    {
        public Judgement(int sealID)
        {
            switch (sealID)
            {
                case (int)PaladinConstants.SpellIds.SOT_ID:
                    break;
                case (int)PaladinConstants.SpellIds.SOR_ID:
                    break;
                default:
                    break;
            }
        }
    }
}
