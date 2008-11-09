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

    public enum PetFamily
    {
        Bat,
        Bear,
        Boar,
        CarrionBird,
        Cat,
        Crab,
        Crocolisk,
        Dragonhawk,
        Gorilla,
        Hyena,
        NetherRay,
        Owl,
        Raptor,
        Ravager,
        Scorpid,
        Serpent,
        Spider,
        SporeBat,
        Tallstrider,
        Turtle,
        WarpStalker,
        WindSerpent,
        Wolf
    }

    public enum Faction
	{
		None,
		Aldor,
		Scryer
	}

	public enum PetAttacks
	{
        Bite,
        Claw,
        Cower,
        FireBreath,
        FuriousHowl,
        Gore,
        Growl,
        LightningBreath,
        None,
        PoisonSpit,
        ScorpidPoison,
        Screech,
        Thunderstomp,
        Warp
    }

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


        public double STEADY_AP_SCALE = 0.2;
        public double STEADY_BONUS_DMG = 252.0;

        public double EXPLOSIVE_AP_SCALE = 0.08;
        public double EXPLOSIVE_BONUS_DMG = (238.0 + 286.0)/2.0;

        public double ARCANE_AP_SCALE = 0.15;
        public double ARCANE_BONUS_DMG = 492.0;

        public double SERPENT_AP_SCALE = 0.2;
        public double SERPENT_BONUS_DMG = 1210.0;

        public double MULTI_BONUS_DMG = 408.0;

        public double AIMED_BONUS_DMG = 408.0;

        public double HAWK_BONUS_AP = 300.0;
    }


}
