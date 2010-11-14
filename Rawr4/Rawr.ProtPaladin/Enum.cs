using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{

    public enum Ability
    {
        // Spells
        AvengersShield,
        HammerOfWrath,
        HolyWrath,

        // Melee & melee procs
        CrusaderStrike,
        HammerOfTheRighteous,
        HammerOfTheRighteousProc,
        JudgementOfRighteousness,
        JudgementOfTruth,
        MeleeSwing,
        SealOfRighteousness,
        SealOfTruth,
        ShieldOfTheRighteous,

        // Damage over Time
        CensureTick,
        Consecration,

        // Defensive
        RetributionAura,
    }
    
    public enum DamageType
    {
        Physical,
        Holy,
        Fire,
        Frost,
        Arcane,
        Shadow,
        Nature,
    }

    public enum CreatureType
    {
        Unspecified,
        Humanoid,
        Undead,
        Demon,
        Giant,
        Elemental,
        Mechanical,
        Beast,
        Dragonkin,
    }

    public enum AttackType
    {
        Melee,
        Ranged,
        Spell,
        DOT,
    }

}