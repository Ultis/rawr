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
        Unknown
    }

    public enum Faction
	{
		None,
		Aldor,
		Scryer
	}

	public enum PetAttacks
	{
        Acid,
        Attitude,
        Bite,
        Claw,
        Cornered,
        Cower,
        Dive,
        Dust,
        FireBreath,
        Frost,
        FuriousHowl,
        Gore,
        Growl,
        Howl,
        Lava,
        LightningBreath,
        Monstrous,
        None,
        Pin,
        Poison,
        PoisonSpit,
        Pummel,
        Rabid,
        Rake,
        Ravage,
        Savage,
        Screech,
        Scorpid,
        Serenity,
        Shell,
        Shock,
        Smack,
        Snatch,
        SonicBlast,
        Spirit,
        Spore,
        Stampede,
        Sting,
        Swipe,
        Tendon,
        Thunderstomp,
        Warp,
        Web,
        Wolverine
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

    public static class HunterRatings
    {
        public static double BASE_HIT_PERCENT = .95; // Check
        public static double HIT_RATING_PER_PERCENT = 32.78998947;

        public static double AGILITY_PER_CRIT = 83.33333333;
        public static double BASE_CRIT_PERCENT = -.0153; // Check
        public static double CRIT_RATING_PER_PERCENT = 45.90598679;

        public static double ARP_RATING_PER_PERCENT = 15.39529991;

        public static double HASTE_RATING_PER_PERCENT = 32.78998947;

        public static double QUIVER_SPEED_INCREASE = 1.15;


        public static double STEADY_AP_SCALE = 0.1;
        public static double STEADY_BONUS_DMG = 252.0;

        public static double EXPLOSIVE_AP_SCALE = 0.16;
        public static double EXPLOSIVE_BONUS_DMG = (428.0 + 516.0) / 2.0;

        public static double ARCANE_AP_SCALE = 0.15;
        public static double ARCANE_BONUS_DMG = 492.0;

        public static double SERPENT_AP_SCALE = 0.2;
        public static double SERPENT_BONUS_DMG = 1210.0;

        public static double MULTI_BONUS_DMG = 408.0;

        public static double AIMED_BONUS_DMG = 408.0;

        public static double HAWK_BONUS_AP = 300.0;
        
        
        
        public static double BASE_MISS_PERCENT = 0.05;
        public static double CHAR_LEVEL = 80.0;
        
        public static double HUNTERS_MARK = 500.0;
    }
    
   
   

    public enum PetTrees
    { 
        CunningGround,
        CunningFlying,
        FerocityGround,
        FerocityFlying,
        Tenacity
    }
}
