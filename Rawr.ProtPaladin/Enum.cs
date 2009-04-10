using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public enum Ability
    {
        None,
        ShieldOfRighteousness,
        HammerOfTheRighteous,
        SealOfVengeance, 
        HolyVengeance,
        JudgementOfVengeance,
        SealOfRighteousness,
        JudgementOfRighteousness,
        Exorcism,
        HammerOfWrath,
        AvengersShield,
        HolyShield,
        RetributionAura,
        Consecration,
        RighteousDefense,
        HolyWrath,
        HandOfReckoning,
    }
    
    public enum HitResult
    {
        AnyMiss,
        AnyHit,
        Miss,
        Dodge,
        Parry,
        Block,
        Glance,
        Resist,
        Crit,
        Hit,
    }

    public enum AttackModelMode
    {
        BasicSoV,
        BasicSoR,
        
    }

    //public enum RageModelMode
    //{
    //    Limited,
    //    Infinite,
    //}

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