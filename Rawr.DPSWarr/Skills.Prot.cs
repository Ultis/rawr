/**********
 * Owner: Jothay
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    public sealed class ShieldSlam : Ability
    {
        /// <summary>
        /// Instant, 6 sec cd, 20 Rage, Melee Range, Shields (Any)
        /// Slam the target with your shield, causing 990 to 1040 damage, modified by you shield block
        /// value, and dispels 1 magic effect on the target. Also causes a high amount of threat.
        /// </summary>
        /// <para>Talents: 
        /// Focused Rage [-(Talents.FocusedRage * 1f) RageCost],
        /// Gag Order [+(5*Pts)% Damage],
        /// OneHandedWeaponSpecialization [+(2*Pts)% Damage]
        /// </para>
        /// <para>Glyphs: </para>
        public ShieldSlam() { }
    }
    public sealed class Revenge : Ability
    {
        /// <summary>
        /// Instant, 1 sec cd, 5 Rage, Melee Range, Melee Weapon (Def)
        /// Instantly counterattack the enemy for 2399 to 2787 damage. Revenge is only usable after the
        /// warrior blocks, dodges or parries an attack.
        /// </summary>
        /// <para>Talents: </para>
        /// <para>Glyphs: </para>
        ///  -(Talents.FocusedRage * 1f) RageCost
        ///  +(10*Pts)% Damage
        public Revenge() { }
    }
    public sealed class ConcussionBlow : Ability
    {
        /// <summary>
        /// Instant, 30 sec cd, 12 Rage, Melee Range, Melee Weapon (Any)
        /// Stuns the opponent for 5 sec and deals 2419 damage (based upon attack power).
        /// </summary>
        /// <para>Talents: Concussion Blow [Requires Talent], Focused Rage [-(Talents.FocusedRage * 1f ) Ragecost]</para>
        /// <para>Glyphs: </para>
        public ConcussionBlow() { }
    }
    public sealed class Devastate : Ability
    {
        /// <summary>
        /// Instant, No Cd, 12 Rage, Melee Range, 1h Melee Weapon (Any)
        /// Sunder the target's armor causing the Sunder Armor effect. In addition, causes 50% of weapon
        /// damage plus 101 for each application of Sunder Armor on the target. The Sunder Armor effect
        /// can stack up to 5 times.
        /// </summary>
        /// <para>Talents: 
        /// Devastate [Requires Talent]
        /// Focused Rage [-(Talents.FocusedRage * 1f) RageCost]
        /// Puncture [-(Talents.Puncture * 1f) RageCost]
        /// Sword and Board [+(5*Pts)% Crit Chance]
        /// </para>
        /// <para>Glyphs: Glyph of Devastate [+1 stack of Sunder Armor]</para>
        public Devastate() { }
    }
    public sealed class Shockwave : Ability
    {
        /// <summary>
        /// Instant, 20 sec Cd, 12 Rage, (Any)
        /// Sends a wave of force in front of the warrior, causing 2419 damage (based upon attack power)
        /// and stunning all enemy targets within 10 yards in a frontal cone for 4 sec.
        /// </summary>
        /// <para>Talents: Shockwave [Requires Talent], Focused Rage [-(Talents.FocusedRage*1f) RageCost]</para>
        /// <para>Glyphs: Glyph of Shockwave [-3 sec Cd]</para>
        public Shockwave() { }
    }
}