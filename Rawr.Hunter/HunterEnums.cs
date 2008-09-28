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
        public double BASE_HIT_PERCENT = .95;
        public double HIT_RATING_PER_PERCENT = 15.76;

        public double AGILITY_PER_CRIT = 40;
        public double BASE_CRIT_PERCENT = -.0153;
        public double CRIT_RATING_PER_PERCENT = 22.0765;

        public double HASTE_RATING_PER_PERCENT = 15.70;

        public double QUIVER_SPEED_INCREASE = 1.15;
        public double STEADYSHOT_BASE_DAMAGE = 150;
        public double STEADYSHOT_BASE_MANA = 110;

        public double AUTO_SHOT_CAST_TIME = .5;
        public double STEADY_SHOT_CAST_TIME = 1.5;

        public int MAX_SHOT_TABLE_LOOPS = 50;
    }


}
