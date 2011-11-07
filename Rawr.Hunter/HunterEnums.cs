using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public enum Specialization
    {
        BeastMastery,
        Marksmanship,
        Survival
    }

    public enum Aspect
    {
        Hawk,
        Fox,
        None
    }

    public enum ShotRotation
    {
        AutoShotOnly,
        OneToOne,
        ThreeToTwo
    }

    public enum CDStates_Enum
    {
        FOCUS_GT50 = 0,
        KILLSHOT_CD = 1,
        RAPIDFIRE = 2,
        KILLCOMMAND = 3,
        FOCUSFIRE = 4,
        BM_UNKNOWN = 5,
        IMP_STEADYSHOT = 6,
        MASTER_MM = 7,
        CHIMERASHOT = 8,
        READINESS = 9,
        EXPLOSIVESHOT = 10,
        LOCKNLOAD = 11,
        BLACKARROW = 12,
    }

    [Flags]
    public enum CDStates_Flags
    {
        NONE = 0,
        FOCUS_GT50 = 1 << 0,
        KILLSHOT_CD = 1 << 1,
        RAPIDFIRE = 1 << 2,
        KILLCOMMAND = 1 << 3,
        FOCUSFIRE = 1 << 4,
        BM_UNKNOWN = 1 << 5,
        IMP_STEADYSHOT = 1 << 6,
        MASTER_MM = 1 << 7,
        CHIMERASHOT = 1 << 8,
        READINESS = 1 << 9,
        EXPLOSIVESHOT = 1 << 10,
        LOCKNLOAD = 1 << 11,
        BLACKARROW = 1 << 12,
    }

    // Updated 4.2 from Wowhead talents, Shots & Abilities.
    public enum Shots
    {
        None,
        AimedShot, // MM
        ArcaneShot, //
        BestialWrath, //
        BlackArrow, //
        ChimeraShot, //
        CobraShot, //
        ConcussiveShot, //
        ExplosiveShot, // SV
        ExplosiveTrap, //
        Fervor, //
        FocusFire, // 
        FreezingTrap, //
        DistractingShot, //
        IceTrap, //
        ImmolationTrap, //
        Intimidation, //
        KillCommand, // 
        KillShot, //
        MultiShot, //
        RapidFire, //
        Readiness, //
        ScatterShot, //
        SerpentSting, //
        SilencingShot, //
        SnakeTrap, //
        SteadyShot, //
        TranquilizingShot, //
        WidowVenom, //
        WyvernSting, //
    }

    // TODO: Setup map to go with Petfamily & PetAttack
    public enum PetAttacks
    {
        AcidSpit,
        BadAttitude,
        BadManner,
        Bite,
        Bullheaded,
        CallOfTheWild,
        Charge,
        Claw,
        //Cornered,
        Cower,
        Dash,
        Dive,
        DemoralizingScreech,
        DustCloud,
        EmbraceoftheShaleSpider,
        FireBreath,
        FroststormBreath,
        FuriousHowl,
        Gore,
        Growl,
        HardenCarapace,
        LastStand,
        LavaBreath,
        LickYourWounds,
        LightningBreath,
        LockJaw,
        MonstrousBite,
        NetherShock,
        None,
        Pin,
        PoisonSpit,
        Prowl,
        Pummel,
        Rabid,
        Rake,
        Ravage,
        RoarOfRecovery,
        RoarOfSacrifice,
        SavageRend,
        ScorpidPoison,
        SerenityDust,
        ShellShield,
        Smack,
        Snatch,
        SonicBlast,
        SpiritStrike,
        SporeCloud,
        Stampede,
        Sting,
        Swipe,
        Swoop,
        Tailspin,
        Taunt,
        TendonRip,
        Thunderstomp,
        VenomWebSpray,
        Warp,
        Web,
        WolverineBite
    }

    public enum PetSkillType
    {
        FocusDump,
        NonDamaging,
        SpecialMelee,
        SpecialSpell,
        SpecialUnique,
        Unimplemented
    }

    public enum AspectUsage
    {
        None,
        FoxToRun
    }
}
