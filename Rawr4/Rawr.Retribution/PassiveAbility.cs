using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Retribution.WPF
{

    public abstract class PassiveAbilityBase
    {
        public abstract float? CritBonus { get; }
        public abstract float? MeleeHitBonus { get; }
        public abstract float? SpellHitBonus { get; }
        public abstract float? DamageBonus { get; }
        public abstract float? SpellPowerBonus { get; }
        public abstract float? AttackPowerBonus { get; }
    }
    public class SheathOfLight : PassiveAbilityBase
    {
        public override float? CritBonus { get { return null; } }
        public override float? MeleeHitBonus { get { return null; } }
        public override float? AttackPowerBonus { get { return null; } }
        public override float? DamageBonus { get { return null; } }
        /// <summary>
        /// increase to spell hit - multiplicative
        /// </summary>
        public override float? SpellHitBonus
        {
            get { return PallyConstants.SHEATH_SPHIT_COEFF; }
        }
        /// <summary>
        /// increase to spellpower - additive
        /// </summary>
        public override float? SpellPowerBonus
        {
            get { return CurrentState.AttackPower * PaladinConstants.SHEATH_AP_COEFF; }
        }
    }
    public class TwoHSpecilization : PassiveAbilityBase
    {
        public override float? CritBonus { get { return null; } }
        public override float? MeleeHitBonus { get { return null; } }
        public override float? SpellHitBonus { get { return null; } }
        public override float? SpellPowerBonus { get { return null; } }
        public override float? AttackPowerBonus { get { return null; } }
        /// <summary>
        /// Bonus added to melee strike - Additive
        /// </summary>
        public override float? DamageBonus
        {
            get { return CurrentState.WeaponDamage * PaladinConstants.TWO_H_SPEC; }
        }
    }
    public class HandOfLight : PassiveAbilityBase
    {
        /**Your autoattacks have a 8% chance to grant Hand of Light, causing your next Holy Power ability to consume
         * no Holy Power and to cast as if 3 Holy Power were consumed.  Each point of Mastery increases the chance by an additional 1%. **/
        public override float? CritBonus
        {
            get { throw new NotImplementedException(); }
        }
        public override float? MeleeHitBonus
        {
            get { throw new NotImplementedException(); }
        }
        public override float? SpellHitBonus
        {
            get { throw new NotImplementedException(); }
        }
        public override float? DamageBonus
        {
            get { throw new NotImplementedException(); }
        }
        public override float? SpellPowerBonus
        {
            get { throw new NotImplementedException(); }
        }
        public override float? AttackPowerBonus
        {
            get { throw new NotImplementedException(); }
        }
    }
}
