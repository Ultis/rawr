using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public enum Ability
    {
        None,
        Cleave,
        ConcussionBlow,
        DamageShield,
        Devastate,
        DeepWounds,
        HeroicStrike,
        HeroicThrow,
        ShieldBash,
        ShieldSlam,
        Shockwave,
        Slam,
        SunderArmor,
        Rend,
        Revenge,
        ThunderClap,
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
        Basic,
        Devastate,
        SwordAndBoard,
        FullProtection,
        UnrelentingAssault,
    }

    public enum RageModelMode
    {
        Limited,
        Infinite,
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
}