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
}