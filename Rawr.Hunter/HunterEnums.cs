using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HunterSE
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

    public enum PetHappiness
    {
        Happy,
        Content,
        Unhappy
    }
}
