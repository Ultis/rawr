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
        VictoryRush,
    }

    public enum AttackModelMode
    {
        Optimal,
        Basic,
        Devastate,
        DevastateRevenge,
        SwordAndBoard,
        SwordAndBoardRevenge,
        FullProtection,
        FullProtectionRevenge,
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