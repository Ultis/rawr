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
        Exorcism,

        Last = Exorcism
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
        Magic
    }

    public enum MobType
    {
        Undead = 0,
        Demon,
        Humanoid,
        Elemental,
        Other
    }

    public enum RotState
    {
        CS,
        FillerOne,
        FillerTwo
    }
}
