using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class GlyphDataAttribute : Attribute
    {
        public GlyphDataAttribute(int index, string name, bool major, string description)
        {
            _index = index;
            _name = name;
            _major = major;
            _description = description;
        }

        private readonly int _index;
        private readonly string _name;
        private readonly bool _major;
        private readonly string _description;

        public int Index { get { return _index; } }
        public string Name { get { return _name; } }
        public bool Major { get { return _major; } }
        public string Description { get { return _description; } }
    }

    public partial class MageTalents
    {
        private bool[] _glyphData = new bool[16];
        public override bool[] GlyphData { get { return _glyphData; } }

        [GlyphData(0, "Glyph of Fireball", true, @"Increases the critical strike chance of Fireball by 5%, but removes the damage over time effect.")]
        public bool GlyphOfFireball { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, "Glyph of Frostfire", true, @"Increases the initial damage dealt by Frostfire Bolt by 2% and its critical strike chance by 2%.")]
        public bool GlyphOfFrostfire { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, "Glyph of Frostbolt", true, @"Increases the damage dealt by Frostbolt by 5%, but removes the slowing effect.")]
        public bool GlyphOfFrostbolt { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, "Glyph of Ice Armor", true, @"Your Ice Armor and Frost Armor spells grant an additional 20% armor and resistance.")]
        public bool GlyphOfIceArmor { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, "Glyph of Improved Scorch", true, @"The Improved Scorch talent now generates 3 applications of the Improved Scorch effect each time Scorch is cast.")]
        public bool GlyphOfImprovedScorch { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, "Glyph of Mage Armor", true, @"Your Mage Armor spell grants an additional 20% mana regeneration while casting.")]
        public bool GlyphOfMageArmor { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, "Glyph of Mana Gem", true, @"Increases the mana recieved from using a mana gem by 40%.")]
        public bool GlyphOfManaGem { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, "Glyph of Molten Armor", true, @"Your Molten Armor spell grants an additional 15% of your spirit as critical strike rating.")]
        public bool GlyphOfMoltenArmor { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, "Glyph of Water Elemental", true, @"Reduces the cooldown of your Summon Water Elemental spell by 30 sec.")]
        public bool GlyphOfWaterElemental { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, "Glyph of Arcane Explosion", true, @"Reduces mana cost of Arcane Explosion by 10%.")]
        public bool GlyphOfArcaneExplosion { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, "Glyph of Arcane Power", true, @"Increases the duration of Arcane Power by 3 sec.")]
        public bool GlyphOfArcanePower { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, "Glyph of Arcane Blast", true, @"Increases the damage from your Arcane Blast buff by 3%.")]
        public bool GlyphOfArcaneBlast { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, "Glyph of Arcane Missiles", true, @"Increases the critical strike damage bonus of Arcane Missiles by 25%.")]
        public bool GlyphOfArcaneMissiles { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, "Glyph of Arcane Barrage", true, @"Reduces the mana cost of Arcane Barrage by 20%.")]
        public bool GlyphOfArcaneBarrage { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, "Glyph of Living Bomb", true, @"The periodic damage from your Living Bomb can now be critical strikes.")]
        public bool GlyphOfLivingBomb { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, "Glyph of Ice Lance", true, @"Your Ice Lance now causes 4 times damage against frozen targets higher level than you instead of triple damage.")]
        public bool GlyphOfIceLance { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
	}

	public partial class DruidTalents
	{
		private bool[] _glyphData = new bool[24];
		public override bool[] GlyphData { get { return _glyphData; } }

		//Cat Glyphs
		[GlyphData(0, "Glyph of Mangle", true, @"Increases the duration of Mangle by 6 sec.")]
		public bool GlyphOfMangle { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
		[GlyphData(1, "Glyph of Shred", true, @"")]
		public bool GlyphOfShred { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
		[GlyphData(2, "Glyph of Rip", true, @"")]
		public bool GlyphOfRip { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
		[GlyphData(3, "Glyph of Berserk", true, @"")]
		public bool GlyphOfBerserk { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
		[GlyphData(4, "Glyph of Savage Roar", true, @"")]
		public bool GlyphOfSavageRoar { get { return _glyphData[4]; } set { _glyphData[4] = value; } }

		//Bear Glyphs
		[GlyphData(5, "Glyph of Growl", true, @"Increases the chance for your Growl ability to work successfully by 8%.")]
		public bool GlyphOfGrowl { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
		[GlyphData(6, "Glyph of Maul", true, @"Your Maul ability now hits 1 additional target.")]
		public bool GlyphOfMaul { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
		[GlyphData(7, "Glyph of Frenzied Regeneration", true, @"While Frenzied Regeneration is active, healing effects on you are 20% more powerful.")]
		public bool GlyphOfFrenziedRegeneration { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
		
		//Moonkin Glyphs
		[GlyphData(8, "Glyph of Focus", true, @"Increases the damage done by Starfall by 20%, but decreases its radius by 50%.")]
		public bool GlyphOfFocus { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
		[GlyphData(9, "Glyph of Insect Swarm", true, @"Increases the damage of your Insect Swarm ability by 20%, but it no longer affects your victim's chance to hit.")]
		public bool GlyphOfInsectSwarm { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
		[GlyphData(10, "Glyph of Monsoon", true, @"Reduces the cooldown of your Typhoon spell by 3 sec.")]
		public bool GlyphOfMonsoon { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
		[GlyphData(11, "Glyph of Moonfire", true, @"Increases the periodic damage of your Moonfire ability by 75%, but initial damage is decreased by 90%.")]
		public bool GlyphOfMoonfire { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
		[GlyphData(12, "Glyph of Starfall", true, @"Reduces the cooldown of Starfall by 30 sec.")]
		public bool GlyphOfStarfall { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
		[GlyphData(13, "Glyph of Starfire", true, @"Your Starfire ability increases the duration of your Moonfire effect on the target by 3 sec, up to a maximum of 9 additional seconds.")]
		public bool GlyphOfStarfire { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
		[GlyphData(14, "Glyph of Wrath", true, @"Reduces the pushback suffered from damaging attacks while casting your Wrath spell by 50%.")]
		public bool GlyphOfWrath { get { return _glyphData[14]; } set { _glyphData[14] = value; } }

		//Tree Glyphs
		[GlyphData(15, "Glyph of Healing Touch", true, @"Decreases the cast time of Healing Touch by 1.5 sec., the mana cost by 25%, and the amount healed by 50%.")]
		public bool GlyphOfHealingTouch { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
		[GlyphData(16, "Glyph of Innervate", true, @"Innervate now grants the caster full mana regeneration while casting for 20 sec, in addition to the effect on the target. If the caster targets him or herself, the mana regeneration effect of your Innervate is increased by 20%.")]
		public bool GlyphOfInnervate { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
		[GlyphData(17, "Glyph of Lifebloom", true, @"Increases the duration of Lifebloom by 1 sec.")]
		public bool GlyphOfLifebloom { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
		[GlyphData(18, "Glyph of Nourish", true, @"Your Nourish heals for an additional 6% for each of your heal over time effects present on the target.")]
		public bool GlyphOfNourish { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
		[GlyphData(19, "Glyph of Rebirth", true, @"Players resurrected by Rebirth are returned to life with 100% health.")]
		public bool GlyphOfRebirth { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
		[GlyphData(20, "Glyph of Regrowth", true, @"Increases the healing of your Regrowth spell by 20% if your Regrowth effect is still active on the target.")]
		public bool GlyphOfRegrowth { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
		[GlyphData(21, "Glyph of Rejuvenation", true, @"While your Rejuvenation targets are below 50% health, you will heal them for an additional 50% health.")]
		public bool GlyphOfRejuvenation { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
		[GlyphData(22, "Glyph of Swiftmend", true, @"Your Swiftmend ability no longer consumes a Rejuvenation or Regrowth effect from the target.")]
		public bool GlyphOfSwiftmend { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
		[GlyphData(23, "Glyph of Wild Growth", true, @"Wild Growth can affect 1 additional target.")]
		public bool GlyphOfWildGrowth { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
	}

    public partial class PaladinTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }
        
        [GlyphData(0, "Glyph of Avenging Wrath", true, @"Reduces the cooldown of your Hammer of Wrath spell by 50% while Avenging Wrath is active.")]
        public bool GlyphOfAvengingWrath { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, "Glyph of Avenger's Shield", true, @"Your Avenger's Shield hits 2 fewer targets, but for 100% more damage.")]
        public bool GlyphOfAvengersShield { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, "Glyph of Seal of Blood", true, @"When your Seal of Blood, Seal of the Martyr, Judgement of Blood, or Judgement of the Martyr deals damage to you, you gain 11% of the damage done as mana.")]
        public bool GlyphOfSealOfBlood { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, "Glyph of Seal of Righteousness", true, @"Increases the damage done by Seal of Righteousness by 10%.")]
        public bool GlyphOfSealOfRighteousness { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, "Glyph of Seal of Vengeance", true, @"Your Seal of Vengeance or Seal of Corruption also grants 10 expertise while active.")]
        public bool GlyphOfSealOfVengeance { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, "Glyph of Hammer of Wrath", true, @"Reduces the mana cost of Hammer of Wrath by 100%.")]
        public bool GlyphOfHammerOfWrath { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, "Glyph of Beacon of Light", true, @"Increases the duration of Beacon of Light by 30 sec.")]
        public bool GlyphOfBeaconOfLight { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, "Glyph of Divine Plea", true, @"While Divine Plea is active, you take 3% reduced damage from all sources.")]
        public bool GlyphOfDivinePlea { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, "Glyph of Divine Storm", true, @"Your Divine Storm now heals for an additional 15% of the damage it causes.")]
        public bool GlyphOfDivineStorm { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, "Glyph of Hammer of the Righteous", true, @"Your Hammer of the Righteous hits 1 additional target.")]
        public bool GlyphOfHammerOfTheRighteous { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, "Glyph of Holy Shock", true, @"Reduces the cooldown of Holy Shock by 1 sec.")]
        public bool GlyphOfHolyShock { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, "Glyph of Salvation", true, @"When you cast Hand of Salvation on yourself, it also reduces damage taken by 20%.")]
        public bool GlyphOfSalvation  { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, "Glyph of Shield of Righteousness", true, @"Reduces the mana cost of Shield of Righteousness by 80%.")]
        public bool GlyphOfShieldOfRighteousness { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, "Glyph of Seal of Wisdom", true, @"While Seal of Wisdom is active, the cost of your healing spells is reduced by 5%.")]
        public bool GlyphOfSealOfWisdom { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, "Glyph of Cleansing", true, @"Reduces the mana cost of your Cleanse and Purify spells by 20%.")]
        public bool GlyphOfCleansing { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, "Glyph of Seal of Light", true, @"While Seal of Light is active, the effect of your healing spells is increased by 5%.")]
        public bool GlyphOfSealOfLight { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, "Glyph of the Wise", false, @"Reduces the mana cost of your Seal of Wisdom spell by 50%.")]
        public bool GlyphOftheWise { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, "Glyph of Turn Evil", true, @"Reduces the casting time of your Turn Evil spell by 100%, but increases the cooldown by 8 sec.")]
        public bool GlyphOfTurnEvil { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, "Glyph of Consecration", true, @"Increases the duration and cooldown of Consecration by 2 sec.")]
        public bool GlyphOfConsecration { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, "Glyph of Crusader Strike", true, @"Reduces the mana cost of your Crusader Strike ability by 20%.")]
        public bool GlyphOfCrusaderStrike { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, "Glyph of Exorcism", true, @"Increases damage done by Exorcism by 20%.")]
        public bool GlyphOfExorcism { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, "Glyph of Flash of Light", true, @"Your Flash of Light has an additional 5% critical strike chance.")]
        public bool GlyphOfFlashOfLight { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, "Glyph of Seal of Command", true, @"Increases the chance of dealing Seal of Command damage by 20%.")]
        public bool GlyphOfSealOfCommand { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, "Glyph of Blessing of Kings", false, @"Reduces the mana cost of your Blessing of Kings and Greater Blessing of Kings spells by 50%.")]
        public bool GlyphOfBlessingOfKings { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, "Glyph of Sense Undead", false, @"Damage against Undead increased by 1% while your Sense Undead ability is active.")]
        public bool GlyphOfSenseUndead { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, "Glyph of Righteous Defense", true, @"Increases the chance for your Righteous Defense ability to work successfully by 8% on each target.")]
        public bool GlyphOfRighteousDefense { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, "Glyph of Spiritual Attunement", true, @"Increases the amount of mana gained from your Spiritual Attunement spell by an additional 2%.")]
        public bool GlyphOfSpiritualAttunement { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, "Glyph of Divinity", true, @"Your Lay on Hands grants twice as much mana as normal and also grants you as much mana as it grants your target.")]
        public bool GlyphOfDivinity { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, "Glyph of Blessing of Wisdom", false, @"Increases the duration of your Blessing of Wisdom spell by 20 min when cast on yourself.")]
        public bool GlyphOfBlessingOfWisdom { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, "Glyph of Hammer of Justice", true, @"Increases your Hammer of Justice range by 5 yards.")]
        public bool GlyphOfHammerOfJustice { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, "Glyph of Lay on Hands", false, @"Reduces the cooldown of your Lay on Hands spell by 5 min.")]
        public bool GlyphOfLayonHands { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, "Glyph of Judgement", true, @"Your Judgements deal 10% more damage.")]
        public bool GlyphOfJudgement { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, "Glyph of Holy Light", true, @"Your Holy Light grants 10% of its heal amount to up to 5 friendly targets within 8 yards of the initial target.")]
        public bool GlyphOfHolyLight { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        [GlyphData(33, "Glyph of Blessing of Might", false, @"Increases the duration of your Blessing of Might spell by 20 min when cast on yourself.")]
        public bool GlyphOfBlessingOfMight { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
    }

    public partial class WarriorTalents
    {
        private bool[] _glyphData = new bool[27];
        public override bool[] GlyphData { get { return _glyphData; } }

        [GlyphData(0, "Glyph of Battle", false, @"Increases the duration of your Battle Shout by 1 min.")]
        public bool GlyphOfBattle { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData( 1, "Glyph of Heroic Strike", true, @"You gain 10 rage when you critically strike with your Heroic Strike ability.")]
        public bool GlyphOfHeroicStrike { get { return _glyphData[ 1]; } set { _glyphData[ 1] = value; } }
        [GlyphData(2, "Glyph of Rending", true, @"Increases the duration of your Rend ability by 6 sec.")]
        public bool GlyphOfRending { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData( 3, "Glyph of Rapid Charge", true, @"Reduces the cooldown of your Charge ability by 20%.")]
        public bool GlyphOfRapidCharge { get { return _glyphData[ 3]; } set { _glyphData[ 3] = value; } }
        [GlyphData(4, "Glyph of Charge", false, @"Increases the range of your Charge ability by 5 yards.")]
        public bool GlyphOfCharge { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData( 5, "Glyph of Resonating Power", true, @"Reduces the rage cost of your Thunder Clap ability by 5.")]
        public bool GlyphOfResonatingPower { get { return _glyphData[ 5]; } set { _glyphData[ 5] = value; } }
        [GlyphData(6, "Glyph of Thunder Clap", false, @"Increases the radius of your Thunder Clap ability by 2 yards.")]
        public bool GlyphOfThunderClap { get { return _glyphData[ 6]; } set { _glyphData[ 6] = value; } }
        [GlyphData(7, "Glyph of Hamstring", true, @"Gives your Hamstring ability a 10% chance to immobilize the target for 5 sec.")]
        public bool GlyphOfHamstring { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, "Glyph of Taunt", true, @"Increases the change for your Taunt ability to succeed by 8%.")]
        public bool GlyphOfTaunt { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData( 9, "Glyph of Sunder Armor", true, @"Your Sunder Armor ability effects a second nearby target.")]
        public bool GlyphOfSunderArmor { get { return _glyphData[ 9]; } set { _glyphData[ 9] = value; } }
        [GlyphData(10, "Glyph of Bloodrage", false, @"Reduces the health cost of your Bloodrage ability by 100%.")]
        public bool GlyphOfBloodrage { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, "Glyph of Overpower", true, @"Adds a 50% chance to enable your Overpower when your attacks are parried.")]
        public bool GlyphOfOverpower { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, "Glyph of Revenge", true, @"After using Revenge, your next Heroic Strike costs no rage.")]
        public bool GlyphOfRevenge { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, "Glyph of Barbaric Insults", true, @"Your Mocking Blow ability also taunts the target.")]
        public bool GlyphOfBarbaricInsults { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, "Glyph of Mocking Blow", false, @"Increases the damage of your Mocking Blow ability by 25%.")]
        public bool GlyphOfMockingBlow { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, "Glyph of Last Stand", true, @"Reduces the cooldown of your Last Stand ability by 3 min, but also reduces the maximum health gained by 10%.")]
        public bool GlyphOfLastStand { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, "Glyph of Cleaving", true, @"Increases the number of targets your Cleave hits by 1.")]
        public bool GlyphOfCleaving { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, "Glyph of Execution", true, @"Your Execute ability deals damage as if you had 10 additional rage.")]
        public bool GlyphOfExecution { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, "Glyph of Sweeping Strikes", true, @"You generate 30 rage over 12 sec when you use your Sweeping Strieks ability.")]
        public bool GlyphOfSweepingStrikes { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, "Glyph of Whirlwind", true, @"Reduces the cooldown of your Whirlwind ability by 2 sec.")]
        public bool GlyphOfWhirlwind { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, "Glyph of Blocking", true, @"Increases your block value by 10% for 10 sec after using your Shield Slam ability.")]
        public bool GlyphOfBlocking { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, "Glyph of Mortal Strike", true, @"Increases the damage of your Mortal Strike ability by 10%.")]
        public bool GlyphOfMortalStrike { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, "Glyph of Bloodthirst", true, @"Increases the healing your recieve from Bloodthirst ability by 100%.")]
        public bool GlyphOfBloodthirst { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, "Glyph of Devastate", true, @"Your Devastate ability now applies two stacks of Sunder Armor.")]
        public bool GlyphOfDevastate { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, "Glyph of Victory Rush", true, @"Your Victory Ruch ability has a 30% increased critical strike chance against targets above 70% health.")]
        public bool GlyphOfVictoryRush { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, "Glyph of Enduring Victory", false, @"Increases the window of opportunity in which you can use Victory Rush by 5 sec.")]
        public bool GlyphOfEnduringVictory { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, "Glyph of Intervene", true, @"Increases the number of attacks you intercept for your intervene target by 1.")]
        public bool GlyphOfIntervene { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
    }

    public partial class ShamanTalents
    {
        private bool[] _glyphData = new bool[28];
        public override bool[] GlyphData { get { return _glyphData; } }

        [GlyphData(0, "Glyph of Healing Wave", true, @"Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.")]
        public bool GlyphofHealingWave { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, "Glyph of Lightning Bolt", true, @"Increases the damage dealt by Lightning Bolt by 4%.")]
        public bool GlyphofLightningBolt { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, "Glyph of Shocking", true, @"Reduces the global cooldown triggered by your shock spells by 0.5 sec.")]
        public bool GlyphofShocking { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, "Glyph of Lightning Shield", true, @"Increases the damage from Lightning Shield by 20%.")]
        public bool GlyphofLightningShield { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, "Glyph of Flame Shock", true, @"Increases the duration of your Flame Shock ability by 6 sec and it is not consumed by casting Lava Burst.")]
        public bool GlyphofFlameShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, "Glyph of Flametongue Weapon", true, @"Increases spell critical strike chance by 2% while Flametongue Weapon is active.")]
        public bool GlyphofFlametongueWeapon { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, "Glyph of Lava Lash", true, @"Damage on your Lava Lash is increased by an additional 10% if your weapon is enchanted with Flametongue.")]
        public bool GlyphofLavaLash { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, "Glyph of Fire Nova Totem", true, @"Reduces the cooldown of your Fire Nova Totem by 3 seconds.")]
        public bool GlyphofFireNovaTotem { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, "Glyph of Frost Shock", true, @"Increases the duration of your Frost Shock by 2 sec.")]
        public bool GlyphofFrostShock { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, "Glyph of Healing Stream Totem", true, @"Your Healing Stream Totem heals for an additional 20%.")]
        public bool GlyphofHealingStreamTotem { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, "Glyph of Lesser Healing Wave", true, @"Your Lesser Healing Wave heals for 20% more if the target is also affected by your Earth Shield.")]
        public bool GlyphofLesserHealingWave { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, "Glyph of Water Mastery", true, @"Increases the passive mana regeneration of your Water Shield spell by 30%.")]
        public bool GlyphofWaterMastery { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, "Glyph of Earthliving Weapon", true, @"Increases the chance for your Earthliving weapon to trigger by 5%.")]
        public bool GlyphofEarthlivingWeapon { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, "Glyph of Windfury Weapon", true, @"Increases the chance per swing for Windfury Weapon to trigger by 2%.")]
        public bool GlyphofWindfuryWeapon { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, "Glyph of Chain Lightning", true, @"Your Chain Lightning strikes 1 additional target.")]
        public bool GlyphofChainLightning { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, "Glyph of Chain Heal", true, @"Your Chain Heal heals 1 additional target.")]
        public bool GlyphofChainHeal { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, "Glyph of Earth Shield", true, @"Increases the amount healed by your Earth Shield by 20%.")]
        public bool GlyphofEarthShield { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, "Glyph of Feral Spirit", true, @"Your spirit wolves gain an additional 30% of your attack power.")]
        public bool GlyphofFeralSpirit { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, "Glyph of Hex", true, @"Increases the damage your Hex target can take before the Hex effect is removed by 20%.")]
        public bool GlyphofHex { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, "Glyph of Mana Tide Totem", true, @"Your Mana Tide Totem grants an additional 1% of each target's maximum mana each time it pulses.")]
        public bool GlyphofManaTideTotem { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, "Glyph of Riptide", true, @"Increases the duration of Riptide by 6 sec.")]
        public bool GlyphofRiptide { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, "Glyph of Stoneclaw Totem", true, @"Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.")]
        public bool GlyphofStoneclawTotem { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, "Glyph of Stormstrike", true, @"Increases the Nature damage bonus from your Stormstrike ability by an additional 8%.")]
        public bool GlyphofStormstrike { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, "Glyph of Thunder", true, @"Reduces the cooldown on Thunderstorm by 10 sec.")]
        public bool GlyphofThunder { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, "Glyph of Totem of Wrath", true, @"When you cast Totem of Wrath, you gain 30% of the totem's bonus spell power for 5 min.")]
        public bool GlyphofTotemofWrath { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, "Glyph of Elemental Mastery", true, @"Reduces the cooldown of your Elemental Mastery ability by 30 sec.")]
        public bool GlyphofElementalMastery { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, "Glyph of Lava", true, @"Your Lava Burst spell gains an additional 10% of your spellpower.")]
        public bool GlyphofLava { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, "Glyph of Fire Elemental Totem", true, @"Reduces the cooldown of your Fire Elemental Totem by 10 min.")]
        public bool GlyphofFireElementalTotem { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
    }
}
