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

    public enum Shots
    {
        None,
        AimedShot,
        ArcaneShot,
        BestialWrath,
        BlackArrow,
        ChimearaShot,
        CobraShot,
        ExplosiveShot,
        ExplosiveTrap,
        FreezingTrap,
        FrostTrap,
        ImmolationTrap,
        KillCommand,
        KillShot,
        MultiShot,
        RapidFire,
        Readiness,
        SerpentSting,
        SilencingShot,
        SnakeTrap,
        SteadyShot,
        WidowVenum,
        WyvernString
    }

    public enum Faction
    {
        None,
        Aldor,
        Scryer
    }

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

    public enum PetHappiness
    {
        Happy,
        Content,
        Unhappy
    }
}
