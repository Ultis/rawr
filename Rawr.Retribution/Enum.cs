using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{

    public enum SealOf
    {
        Righteousness = 0,
        Truth,
        None
    }

    public enum Ability
    {
        Judgement = 0,
        CrusaderStrike,
        DivineStorm,
        Inquisition,
        TemplarsVerdict,
        Consecration,
        HammerOfWrath,
        HolyWrath,
        Exorcism
    }

    public enum DamageAbility
    {
        Judgement = 0,
        CrusaderStrike,
        DivineStorm,
        Inquisition,
        TemplarsVerdict,
        Consecration,
        HammerOfWrath,
        HolyWrath,
        Exorcism,
        HandOfLightCS,
        HandOfLightTV,
        White,
        SoC,
        Seal,
        SealDot
    }

    public enum AbilityType
    {
        Melee = 0,
        Range,
        Spell
    }

    public enum DamageType
    {
        Physical = 0,
        Holy,
        Fire,
        Nature,
        Frost,
        Shadow,
        Arcane,
        NoDD
    }

    public enum RotState
    {
        CS,
        FillerOne,
        FillerTwo
    }

    public enum Multiplier
    {
        Armor = 0, 
        Magical,
        Physical,
        Talents,
        Glyphs,
        Sets,
        Others
    }
}
