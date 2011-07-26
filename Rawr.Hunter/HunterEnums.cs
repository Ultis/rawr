using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public enum Specialization
    {
        None,
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

    public enum PetFamily
    {
        #region Cunning Pets
        Bat = 24,
        BirdOfPrey = 26,
        Chimaera = 38,
        Dragonhawk = 30,
        Monkey = 51,
        NetherRay = 34,
        Ravager = 31,
        Serpent = 35,
        Silithid = 41,
        Spider = 3,
        SporeBat = 33,
        WindSerpent = 27,
        #endregion
        #region Tenacity Pets
        Bear = 4,
        Beetle = 48,
        Boar = 5,
        Crab = 8,
        Crocolisk = 6,
        Gorilla = 9,
        Rhino = 43,
        Scorpid = 20,
        ShaleSpider = 47,
        Turtle = 21,
        WarpStalker = 32,
        Worm = 42,
        #endregion
        #region Ferocity Pets
        CarrionBird = 7,
        Cat = 2,
        CoreHound = 45,
        Devilsaur = 39,
        Dog = 50,
        Fox = 49,
        Hyena = 25,
        Moth = 37,
        Raptor = 11,
        SpiritBeast = 46,
        Tallstrider = 12,
        Wasp = 44,
        Wolf = 1,
        #endregion
        None = 0,
    }

    public enum PetFamilyTree
    {
        Cunning,
        Ferocity,
        Tenacity,
        None
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
