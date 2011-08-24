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
