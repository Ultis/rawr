using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
	public enum Aspect
	{
		Hawk,
		Viper
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
    }

    public enum PetClass
    {
        Cunning,
        Ferocity,
        Tenacity,

        NUM_PetClass, // Always last entry.
    }

    public enum PetTalents
    {
    }

    public enum PetFamily
    {
        Bat,
        Bear,
        BirdofPrey,
        Boar,
        CarrionBird,
        Cat,
        Chimaera,
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
        Worm,

        NUM_PetFamily // Always last entry.
    }

    public enum Faction
	{
		None,
		Aldor,
		Scryer
	}

    public enum PetSkills
    {
        None,                   // 0

        ///////////////////////////
        // Cunning:
        SonicBlast,             // Bat
        Snatch,                 // Birds of prey
        FroststormBreath,       // Chimaeras
        FireBreath,             // Dragonhawks
        NetherShock,            // Nether Ray
        Ravage,                 // Ravager
        PoisonSpit,             // Serpents
        VenomWebSpray,          // Silithids
        Web,                    // Spiders
        SporeCloud,             // Sporebats
        LightningBreath,        // WindSerpents.

        ///////////////////////////
        // Ferocity:
        DemoralizingScreech,    // Carrion Birds
        Prowl,                  // Cats
        Rake,                   // Cats
        LavaBreath,             // CoreHounds
        MonstrousBite,          // Devilsaurs
        TendonRip,              // Hyenas
        SerenityDust,           // Moths
        SavageRend,             // Raptors
        SpiritStrike,           // SpiritBeasts
        DustCloud,              // TallStriders
        Sting,                  // Wasps
        FuriousHowl,            // Wolves

        ///////////////////////////
        // Tenacity:
        Swipe,                  // Bear
        Gore,                   // Boar
        Pin,                    // Crab
        BatAttitude,            // Crocolisks
        Pummel,                 // Gorilla
        Stampede,               // Rhino
        ScorpidPoison,          // Scorpid
        ShellShield,            // Turtle
        Warp,                   // Warp Stalker
        AcidSpit,               // Worms

        ///////////////////////////
        // Focus Dump:
        Bite,
        Claw,
        Smack,

        ///////////////////////////
        // Threat:
        Cower,
        Growl,

        NUM_PetSkills      // always last.
    }

    // TODO: move these to the global values that include class specific values.
    public class HunterRatings
    {
        public double BASE_HIT_PERCENT = .95; // Check
        public double HIT_RATING_PER_PERCENT = 32.78998947;

        public double AGILITY_PER_CRIT = 83.33333333;
        public double BASE_CRIT_PERCENT = -.0153; // Check
        public double CRIT_RATING_PER_PERCENT = 45.90598679;

        public double ARP_RATING_PER_PERCENT = 15.39529991;

        public double HASTE_RATING_PER_PERCENT = 32.78998947;

        public double QUIVER_SPEED_INCREASE = 1.15;


        public double STEADY_AP_SCALE = 0.1;
        public double STEADY_BONUS_DMG = 252.0;

        public double EXPLOSIVE_AP_SCALE = 0.16;
        public double EXPLOSIVE_BONUS_DMG = (428.0 + 516.0) / 2.0;

        public double ARCANE_AP_SCALE = 0.15;
        public double ARCANE_BONUS_DMG = 492.0;

        public double SERPENT_AP_SCALE = 0.2;
        public double SERPENT_BONUS_DMG = 1210.0;

        public double MULTI_BONUS_DMG = 408.0;

        public double AIMED_BONUS_DMG = 408.0;

        public double HAWK_BONUS_AP = 300.0;
    }


}
