using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
	public enum Aspect
	{
        Beast,
		Hawk,
		Viper,
        Monkey,
        Dragonhawk,
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
        ExplosiveShot,
        ChimeraShot_Serpent,
        SerpentSting,
        SteadyShot,
        AimedShot,
        MultiShot,
        ArcaneShot,
        BlackArrow,
        KillShot,
        SilencingShot,
        ChimeraShot,
        ScorpidSting,
        ViperSting,
        RapidFire,
        ImmolationTrap,
        Readiness,
        BeastialWrath,
        BloodFury,
        Berserk
    }

    public enum PetFamily
    {
        None,
        Bat,
        Bear,
        BirdOfPrey,
        Boar,
        CarrionBird,
        Cat,
        Chimera,
        CoreHound,
        Crab,
        Crocolisk,
        Devilsaur,
        Dragonhawk,
        Gorilla,
        Hyena,
        Moth,
        NetherRay,
        Raptor,
        Ravager,
        Rhino,
        Scorpid,
        Serpent,
        Silithid,
        Spider,
        SpiritBeast,
        SporeBat,
        Tallstrider,
        Turtle,
        WarpStalker,
        Wasp,
        WindSerpent,
        Wolf,
        Worm
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
        FireBreath,
        FroststormBreath,
        FuriousHowl,
        Gore,
        Growl,
        LastStand,
        LavaBreath,
        LickYourWounds,
        LightningBreath,
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

    public enum ManaPotionType
    {
        None,
        RunicManaPotion,
        SuperManaPotion
    }

    public enum AspectUsage
    {
        AlwaysOn,
        ViperToOOM,
        ViperRegen
    }

    public enum HeroismUsage
    {
        Repeat,
        Once,
        Never
    }

    public enum PetHappiness
    {
        Happy,
        Content,
        Unhappy
    }

    public static class HunterRatings
    {
        public static double HIT_RATING_PER_PERCENT = 32.78998947;

        public static double AGILITY_PER_CRIT = 83.33; // was previously 83.33333333 - rounded down for spreadsheet
        public static double CRIT_RATING_PER_PERCENT = 45.905985258; // from spreadsheet. we were using 45.90598679                                                       
        public static double HASTE_RATING_PER_PERCENT = 32.78998947; // matches spreadsheet
        public static double ARP_RATING_PER_PERCENT = 12.3162; // from spreadsheet (was 15.39529991)
        public static double INTELLECT_PER_SPELL_CRIT = 166.667; // from spreadsheet 

        public static double BASE_CRIT_PERCENT = -.0153; // Check

        public static double BASE_MISS_PERCENT = 0.05;
        public static double CHAR_LEVEL = 80.0;
        
        public static double HUNTERS_MARK = 500.0;
    }  

}
