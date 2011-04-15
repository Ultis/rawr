using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public enum GlyphType { Minor = 0, Major, Prime, }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class GlyphDataAttribute : Attribute
    {
        public GlyphDataAttribute(int index, int spellid, int effectid, string name, GlyphType type, string description)
        {
            _index = index;
            _spellid = spellid;
            _effectid = effectid;
            _name = name;
            _type = type;
            _description = description;
        }

        /// <summary>This function is Obsolete and should be removed.</summary>
        [Obsolete]
        public GlyphDataAttribute(int index, int spellid, string name, GlyphType type, string description)
        {
            _index = index;
            _spellid = spellid;
            _effectid = -1;
            _name = name;
            _type = type;
            _description = description;
        }

        /// <summary>This function is Obsolete and should be removed.</summary>
        [Obsolete]
        public GlyphDataAttribute(int index, string name, GlyphType type, string description)
        {
            _index = index;
            _spellid = -1;
            _effectid = -1;
            _name = name;
            _type = type;
            _description = description;
        }

        private readonly int _index;
        private readonly int _spellid;
        private readonly int _effectid;
        private readonly string _name;
        private readonly GlyphType _type;
        private readonly string _description;

        public int Index { get { return _index; } }
        /// <summary>The Spell that applies the Glyph:<br/>E.g.- Glyph of Judgement: http://www.wowhead.com/item=41092 </summary>
        public int SpellID { get { return _spellid; } }
        /// <summary>The Glyph Effect itself:<br/>E.g.- Glyph of Judgement: http://www.wowhead.com/spell=54922 </summary>
        public int EffectID { get { return _effectid; } }
        public string Name { get { return _name; } }
        public GlyphType Type { get { return _type; } }
        public string Description { get { return _description; } }
    }

    public partial class MageTalents
    {
        private bool[] _glyphData = new bool[20];
        public override bool[] GlyphData { get { return _glyphData; } }

        [GlyphData(0, 42739, "Glyph of Fireball", GlyphType.Prime, @"Increases the critical strike chance of your Fireball spell by 5%.")]
        public bool GlyphOfFireball { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 44684, "Glyph of Frostfire", GlyphType.Prime, @"Increases the damage done by your Frostfire Bolt by 15% and your Frostfire Bolt now deals 3% additional damage over 12 sec, stacking up to 3 times, but no longer reduces the victim's movement speed.")]
        public bool GlyphOfFrostfire { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 42742, "Glyph of Frostbolt", GlyphType.Prime, @"Increases the critical strike chance of your Frostbolt spell by 5%.")]
        public bool GlyphOfFrostbolt { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        //[GlyphData(3, "Glyph of Ice Armor", GlyphType.Major, @"Your Ice Armor and Frost Armor spells grant an additional 50% armor and resistance.")]
        //public bool GlyphOfIceArmor { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, 42743, "Glyph of Pyroblast", GlyphType.Prime, @"Increases the critical strike chance of your Pyroblast spell by 5%.")]
        public bool GlyphOfPyroblast { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, 42749, "Glyph of Mage Armor", GlyphType.Prime, @"Your Mage Armor regenerates 20% more mana.")]
        public bool GlyphOfMageArmor { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        //[GlyphData(6, "Glyph of Mana Gem", GlyphType.Major, @"Increases the mana recieved from using a mana gem by 40%.")]
        //public bool GlyphOfManaGem { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, 42751, "Glyph of Molten Armor", GlyphType.Prime, @"Your Molten Armor grants an additional 2%  spell critical strike chance.")]
        public bool GlyphOfMoltenArmor { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        //[GlyphData(8, "Glyph of Water Elemental", GlyphType.Major, @"Reduces the cooldown of your Summon Water Elemental spell by 30 sec.")]
        //public bool GlyphOfWaterElemental { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        //[GlyphData(9, "Glyph of Arcane Explosion", GlyphType.Major, @"Reduces mana cost of Arcane Explosion by 10%.")]
        //public bool GlyphOfArcaneExplosion { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        //[GlyphData(10, "Glyph of Arcane Power", GlyphType.Major, @"Increases the duration of Arcane Power by 3 sec.")]
        //public bool GlyphOfArcanePower { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, 44955, "Glyph of Arcane Blast", GlyphType.Prime, @"Increases the damage from your Arcane Blast buff by 3%.")]
        public bool GlyphOfArcaneBlast { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, 42735, "Glyph of Arcane Missiles", GlyphType.Prime, @"Increases the critical strike chance of your Arcane Missiles spell by 5%.")]
        public bool GlyphOfArcaneMissiles { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, 45738, "Glyph of Arcane Barrage", GlyphType.Prime, @"Increases the damage of your Arcane Barrage spell by 4%.")]
        public bool GlyphOfArcaneBarrage { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 63539, "Glyph of Living Bomb", GlyphType.Prime, @"Increases the damage of your Living Bomb spell by 3%.")]
        public bool GlyphOfLivingBomb { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, 42745, "Glyph of Ice Lance", GlyphType.Prime, @"Increases the damage of your Ice Lance spell by 5%.")]
        public bool GlyphOfIceLance { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, 45739, "Glyph of Mirror Image", GlyphType.Minor, @"Your Mirror Images cast Arcane Blast or Fireball instead of Frostbolt depending on your primary talent tree.")]
        public bool GlyphOfMirrorImage { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 45736, "Glyph of Deep Freeze", GlyphType.Prime, @"Your Deep Freeze deals 20% additional damage.")]
        public bool GlyphOfDeepFreeze { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        //[GlyphData(18, "Glyph of Eternal Water", GlyphType.Major, @"Your Summon Water Elemental now lasts indefinitely, but your Water Elemental can no longer cast Freeze.")]
        //public bool GlyphOfEternalWater { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 42753, "Glyph of Cone of Cold", GlyphType.Prime, @"Increases the damage of your Cone of Cold spell by 25%.")]
        public bool GlyphOfConeOfCold { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
    }

    public partial class DruidTalents
    {
        private bool[] _glyphData = new bool[40];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        [GlyphData( 0, 45601, 62969, "Glyph of Berserk", GlyphType.Prime, @"Increases the duration of Berserk by 5 sec.")]
        public bool GlyphOfBerserk { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData( 1, 40919, 54830, "Glyph of Insect Swarm", GlyphType.Prime, @"Increases the damage of your Insect Swarm ability by 30%.")]
        public bool GlyphOfInsectSwarm { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData( 2, 67484, 94382, "Glyph of Lacerate", GlyphType.Prime, @"Increases the critical strike chance of your Lacerate ability by 5%.")]
        public bool GlyphOfLacerate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData( 3, 40915, 54826, "Glyph of Lifebloom", GlyphType.Prime, @"Increases the critical effect chance of your Lifebloom 10%.")]
        public bool GlyphOfLifebloom { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData( 4, 40900, 54813, "Glyph of Mangle", GlyphType.Prime, @"Increases the damage done by Mangle by 10%.")]
        public bool GlyphOfMangle { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData( 5, 40923, 54829, "Glyph of Moonfire", GlyphType.Prime, @"Increases the periodic damage of your Moonfire ability by 20%.")]
        public bool GlyphOfMoonfire { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData( 6, 40912, 54743, "Glyph of Regrowth", GlyphType.Prime, @"Your Regrowth heal-over-time will automatically refresh its duration on targets at or below 25% health.")]
        public bool GlyphOfRegrowth { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData( 7, 40913, 54754, "Glyph of Rejuvenation", GlyphType.Prime, @"Increases the healing done by your Rejuvenation by 10%.")]
        public bool GlyphOfRejuvination { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData( 8, 40902, 54818, "Glyph of Rip", GlyphType.Prime, @"Increases the periodic damage of your Rip by 15%.")]
        public bool GlyphOfRip { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData( 9, 45604, 63055, "Glyph of Savage Roar", GlyphType.Prime, @"Your Savage Roar ability grants an additional 5% bonus damage done")]
        public bool GlyphOfSavageRoar { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, 40901, 54815, "Glyph of Shred", GlyphType.Prime, @"Each time you Shred, the duration of your Rip on the target is extended 2 sec, up to a maximum of 6 sec.")]
        public bool GlyphOfShred { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, 40916, 54845, "Glyph of Starfire", GlyphType.Prime, @"Your Starfire ability increases the duration of yoru Moonfire effect on the target by 3 sec, up to a maximum of 9 additional seconds.")]
        public bool GlyphOfStarfire { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, 45603, 62917, "Glyph of Starsurge", GlyphType.Prime, @"When your Starsurge deals damage, the cooldown remaining on your Starfall is reduced by 5 sec.")]
        public bool GlyphOfStarsurge { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, 40906, 54824, "Glyph of Swiftmend", GlyphType.Prime, @"Your Swiftmend ability no longer consumes a Rejuvenation or Regrowth effect from the target.")]
        public bool GlyphOfSwiftmend { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 67487, 94390, "Glyph of Tiger's Fury", GlyphType.Prime, @"Reduces the cooldown of your Tiger's Fury ability by 3 sec.")]
        public bool GlyphOfTigersFury { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, 40922, 54756, "Glyph of Wrath", GlyphType.Prime, @"Your Wrath does 10% additional damage to targets afflicted by your Insect Swarm")]
        public bool GlyphOfWrath { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        #endregion
        #region Major
        [GlyphData(16, 45623, 63057, "Glyph of Barkskin", GlyphType.Major, @"Reduces the chance you'll be critically hit by melee attacks by 25% while Barkskin is active")]
        public bool GlyphOfBarkskin { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 40924, 54760, "Glyph of Entangling Roots", GlyphType.Major, @"Reduces the cast time of your Entangling Roots by 100%, but increases its cooldown by 10 sec.")]
        public bool GlyphOfEntanglingRoots { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, 67485, 94386, "Glyph of Faerie Fire", GlyphType.Major, @"Increases the range of your Faerie Fire and Feral Faerie Fire abilities by 10 yds.")]
        public bool GlyphOfFaerieFire { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 67486, 94388, "Glyph of Feral Charge", GlyphType.Major, @"Reduces the cooldown of yoru Feral Charge (Cat) ability by 2 sec and the cooldown of your Feral Charge (Bear) ability by 1 sec.")]
        public bool GlyphOfFeralCharge { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, 48720, 67598, "Glyph of Ferocious Bite", GlyphType.Major, @"Your Ferocious Bite ability no longer converts extra energy into additional damage.")]
        public bool GlyphOfFerociousBite { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, 44928, 62080, "Glyph of Focus", GlyphType.Major, @"Increases the damage done by Starfall by 10%, but decreases its radius by 50%")]
        public bool GlyphOfFocus { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, 40896, 54810, "Glyph of Frenzied Regeneration", GlyphType.Major, @"While Frenzied Regeneration is active, healing effects on you are 30% more powerful but causes your Frenzied Regeneration to longer convert rage into health.")]
        public bool GlyphOfFrenziedRegeneration { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, 40914, 54825, "Glyph of Healing Touch", GlyphType.Major, @"When you Healing Touch, the cooldown on your Nature's Swiftness is reduced by 5 sec.")]
        public bool GlyphOfHealingTouch { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 40920, 54831, "Glyph of Hurricane", GlyphType.Major, @"Your Hurricane ability now also slows the movement speed of its victims by 50%.")]
        public bool GlyphOfHurricane { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 40908, 54832, "Glyph of Innervate", GlyphType.Major, @"When Innervate is cast on a friendly target other than the caster, the caster will gain 50% of Innervate's effect.")]
        public bool GlyphOfInnervate { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, 40897, 54811, "Glyph of Maul", GlyphType.Major, @"Your Maul ability now hits 1 additional target for 50% damage.")]
        public bool GlyphOfMaul { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, 45622, 63056, "Glyph of Monsoon", GlyphType.Major, @"Reduces the cooldown of your Typhoon spell by 3 sec.")]
        public bool GlyphOfMonsoon { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, 40903, 54821, "Glyph of Rake", GlyphType.Major, @"Your Rake ability prevents targets from fleeing.")]
        public bool GlyphOfRake { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, 40909, 54733, "Glyph of Rebirth", GlyphType.Major, @"Players resurrected by Rebirth are returned to life with 100% health.")]
        public bool GlyphOfRebirth { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 40899, 54812, "Glyph of Solar Beam", GlyphType.Major, @"Increases the duration of your Solar Beam silence effect by 5 sec.")]
        public bool GlyphOfSolarBeam { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, 40921, 54828, "Glyph of Starfall", GlyphType.Major, @"Reduces the cooldown of Starfall by 30 sec.")]
        public bool GlyphOfStarfall { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, 43332, 57862, "Glyph of Thorns", GlyphType.Major, @"Reduces the cooldown of your Thorns spell by 20 sec.")]
        public bool GlyphOfThorns { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        [GlyphData(33, 45602, 62970, "Glyph of Wild Growth", GlyphType.Major, @"Wild Growth can affect 1 additional target.")]
        public bool GlyphOfWildGrowth { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
        #region Minor
        [GlyphData(34, 43316, 57856, "Glyph of Aquatic Form", GlyphType.Major, @"Increases your swim speed by 50% while in Aquatic Form.")]
        public bool GlyphOfAquaticForm { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        [GlyphData(35, 43334, 57858, "Glyph of Challenging Roar", GlyphType.Major, @"Reduces the cooldown of your Challenging Roar by 30 sec.")]
        public bool GlyphOfChallengingRoar { get { return _glyphData[35]; } set { _glyphData[35] = value; } }
        [GlyphData(36, 43674, 59219, "Glyph of Dash", GlyphType.Major, @"Reduces the cooldown of your Dash ability by 20%.")]
        public bool GlyphOfDash { get { return _glyphData[36]; } set { _glyphData[36] = value; } }
        [GlyphData(37, 44922, 62135, "Glyph of Typhoon", GlyphType.Major, @"Reduces the cost of your Typhoon spell by 8% and increases its radius by 10 yards, but it no longer knocks enemies back.")]
        public bool GlyphOfTyphoon { get { return _glyphData[37]; } set { _glyphData[37] = value; } }
        [GlyphData(38, 43331, 57857, "Glyph of Unburdened Rebirth", GlyphType.Major, @"Your Rebirth spell no longer requires a reagent.")]
        public bool GlyphOfUnburdenedRebirth { get { return _glyphData[38]; } set { _glyphData[38] = value; } }
        [GlyphData(39, 43335, 57855, "Glyph of Mark of the Wild", GlyphType.Major, @"Mana cost of your Mark of the Wild reduced by 50%.")]
        public bool GlyphOfTheWild { get { return _glyphData[39]; } set { _glyphData[39] = value; } }
        #endregion
    }

    public partial class PaladinTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        [GlyphData(0, 41098, "Glyph of Crusader Strike", GlyphType.Prime, @"Increases the critical strike chance of Crusader Strike by 5%.")]
        public bool GlyphOfCrusaderStrike { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 41106, "Glyph of Divine Favor", GlyphType.Prime, @"Increases the duration of Divine Favor by 10 sec.")]
        public bool GlyphOfDivineFavor { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 41103, "Glyph of Exorcism", GlyphType.Prime, @"Your Exorcism causes an additional 20% of its damage over 6 sec.")]
        public bool GlyphOfExorcism { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, 45742, "Glyph of Hammer of the Righteous", GlyphType.Prime, @"Increases the damage of both the physical and Holy components of Hammer of the Righteous by 10%.")]
        public bool GlyphOfHammerOfTheRighteous { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, 45746, "Glyph of Holy Shock", GlyphType.Prime, @"Increases the critical effect chance of Holy Shock by 5%.")]
        public bool GlyphOfHolyShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, 41092, "Glyph of Judgement", GlyphType.Prime, @"Your Judgement deals 10% more damage.")]
        public bool GlyphOfJudgement { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, 41110, "Glyph of Seal of Insight", GlyphType.Prime, @"While Seal of Insight is active, the effect of your healing spells is increased by 5%.")]
        public bool GlyphOfSealOfInsight { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, 43869, "Glyph of Seal of Truth", GlyphType.Prime, @"Your Seal of Truth also grants 10 expertise while active.")]
        public bool GlyphOfSealOfTruth { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 45744, "Glyph of Shield of the Righteous", GlyphType.Prime, @"Increases the damage of Shield of the Righteous by 10%.")]
        public bool GlyphOfShieldOfTheRighteous { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, 45743, "Glyph of Templar's Verdict", GlyphType.Prime, @"Increases the damage of Templar's Verdict by 15%.")]
        public bool GlyphOfTemplarsVerdict { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, 41105, "Glyph of Word of Glory", GlyphType.Prime, @"Increases the healing done by Word of Glory by 10%.")]
        public bool GlyphOfWordOfGlory { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        #endregion
        #region Major
        [GlyphData(11, 41107, "Glyph of Ascetic Crusader", GlyphType.Major, @"Reduces the mana cost of your Crusader Strike by 30%.")]
        public bool GlyphOfAsceticCrusader { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(12, 45741, "Glyph of Beacon of Light", GlyphType.Major, @"Your Beacon of Light costs no mana.")]
        public bool GlyphOfBeaconOfLight { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, 41104, "Glyph of Cleansing", GlyphType.Major, @"Reduces the mana cost of your Cleanse by 20%.")]
        public bool GlyphOfCleansing { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 41099, "Glyph of Consecration", GlyphType.Major, @"Increases the duration and cooldown of Consecration by 20%.")]
        public bool GlyphOfConsecration { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, 43868, "Glyph of Dazing Shield", GlyphType.Major, @"Your Avenger's Shield now also dazes targets.")]
        public bool GlyphOfDazingShield { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, 45745, "Glyph of Divine Plea", GlyphType.Major, @"Your Divine Plea provides an additional 5% of your total mana.")]
        public bool GlyphOfDivinePlea { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 41096, "Glyph of Divine Protection", GlyphType.Major, @"Removes the physical damage reduction of your Divine Protection, but increases the magical damage reduction by 20%.")]
        public bool GlyphOfDivineProtection { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, 41108, "Glyph of Divinity", GlyphType.Major, @"When you use Lay on Hands, you also gain 10% of your maximum mana.")]
        public bool GlyphOfDivinity { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 41101, "Glyph of Focused Shield", GlyphType.Major, @"Your Avenger's Shield hits 2 fewer targets, but for 30% more damage.")]
        public bool GlyphOfFocusedShield { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, 41095, "Glyph of Hammer of Justice", GlyphType.Major, @"Increases your Hammer of Justice range by 5 yards.")]
        public bool GlyphOfHammerOfJustice { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, 41097, "Glyph of Hammer of Wrath", GlyphType.Major, @"Reduces the mana cost of Hammer of Wrath by 100%.")]
        public bool GlyphOfHammerOfWrath { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, 45747, "Glyph of Salvation", GlyphType.Major, @"Hand of Salvation no longer permanently reduces threat over time but instead reduces all threat as long as Hand of Salvation lasts.")]
        public bool GlyphOfSalvation { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, 43867, "Glyph of Holy Wrath", GlyphType.Major, @"Your Holy Wrath now also stuns Elementals and Dragonkin.")]
        public bool GlyphOfHolyWrath { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 43367, "Glyph of Lay on Hands", GlyphType.Major, @"Reduces the cooldown of your Lay on Hands spell by 3 min.")]
        public bool GlyphOfLayOnHands { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 41109, "Glyph of Light of Dawn", GlyphType.Major, @"Light of Dawn can affect 1 additional target.")]
        public bool GlyphOfLightOfDawn { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, 66918, "Glyph of the Long Word", GlyphType.Major, @"Your Word of Glory heals for 50% less up front, but provides an additional 50% healing over 6 sec.")]
        public bool GlyphOfTheLongWord { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, 41094, "Glyph of Rebuke", GlyphType.Major, @"Reduces the mana cost of Rebuke by 100%.")]
        public bool GlyphOfRebuke { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, 41102, "Glyph of Turn Evil", GlyphType.Major, @"Reduces the casting time of your Turn Evil spell by 100%, but increases the cooldown by 8 sec.")]
        public bool GlyphOfTurnEvil { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        #region Minor
        [GlyphData(29, 43365, "Glyph of Blessing of Kings", GlyphType.Minor, @"Reduces the mana cost of Blessing of Kings by 50%.")]
        public bool GlyphOfBlessingOfKings { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 43340, "Glyph of Blessing of Might", GlyphType.Minor, @"Reduces the mana cost of your Blessing of Might by 50%.")]
        public bool GlyphOfBlessingOfMight { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, 43366, "Glyph of Insight", GlyphType.Minor, @"Reduces the mana cost of Seal of Insight by 50%.")]
        public bool GlyphOfInsight { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, 43369, "Glyph of Justice", GlyphType.Minor, @"Reduces the mana cost of Seal of Justice by 50%.")]
        public bool GlyphOfJustice { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        [GlyphData(33, 41100, "Glyph of Righteousness", GlyphType.Minor, @"Reduces the mana cost of Seal of Righteousness by 50%.")]
        public bool GlyphOfRighteousness { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        [GlyphData(34, 43368, "Glyph of Truth", GlyphType.Minor, @"Reduces the mana cost of Seal of Truth by 50%.")]
        public bool GlyphOfTruth { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion
    }

    public partial class WarriorTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Reduces the cooldown on Bladestorm by 15 sec.</summary>
        [GlyphData(0, 45790, 63324, "Glyph of Bladestorm", GlyphType.Prime, @"Reduces the cooldown on Bladestorm by 15 sec.")]
        public bool GlyphOfBladestorm { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage of Bloodthirst by 10%.</summary>
        [GlyphData(1, 43416, 58367, "Glyph of Bloodthirst", GlyphType.Prime, @"Increases the damage of Bloodthirst by 10%.")]
        public bool GlyphOfBloodthirst { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of Devastate by 5%.</summary>
        [GlyphData(2, 43415, 58388, "Glyph of Devastate", GlyphType.Prime, @"Increases the critical strike chance of Devastate by 5%.")]
        public bool GlyphOfDevastate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the damage of Mortal Strike by 10%.</summary>
        [GlyphData(3, 43421, 58368, "Glyph of Mortal Strike", GlyphType.Prime, @"Increases the damage of Mortal Strike by 10%.")]
        public bool GlyphOfMortalStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage of Overpower by 10%.</summary>
        [GlyphData(4, 43422, 58386, "Glyph of Overpower", GlyphType.Prime, @"Increases the damage of Overpower by 10%.")]
        public bool GlyphOfOverpower { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the critical strike chance of Raging Blow by 5%.</summary>
        [GlyphData(5, 43432, 58370, "Glyph of Raging Blow", GlyphType.Prime, @"Increases the critical strike chance of Raging Blow by 5%.")]
        public bool GlyphOfRagingBlow { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the damage of Revenge by 10%.</summary>
        [GlyphData(6, 43424, 58364, "Glyph of Revenge", GlyphType.Prime, @"Increases the damage of Revenge by 10%.")]
        public bool GlyphOfRevenge { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage of Shield Slam by 10%.</summary>
        [GlyphData(7, 43425, 58375, "Glyph of Shield Slam", GlyphType.Prime, @"Increases the damage of Shield Slam by 10%.")]
        public bool GlyphOfShieldSlam { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the critical strike chance of Slam by 5%.</summary>
        [GlyphData(8, 43423, 58385, "Glyph of Slam", GlyphType.Prime, @"Increases the critical strike chance of Slam by 5%.")]
        public bool GlyphOfSlam { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 9; } }
        /// <summary>Increases the number of targets your Cleave hits by 1.</summary>
        [GlyphData( 9, 43414, 58366, "Glyph of Cleaving", GlyphType.Major, @"Increases the number of targets your Cleave hits by 1.")]
        public bool GlyphOfCleaving { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.</summary>
        [GlyphData(10, 63481, 89003, "Glyph of Colossus Smash", GlyphType.Major, @"Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.")]
        public bool GlyphOfColossusSmash { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Death Wish no longer increases damage taken.</summary>
        [GlyphData(11, 67483, 94374, "Glyph of Death Wish", GlyphType.Major, @"Death Wish no longer increases damage taken.")]
        public bool GlyphOfDeathWish { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Your Heroic Throw applies a stack of Sunder Armor.</summary>
        [GlyphData(12, 43418, 58357, "Glyph of Heroic Throw", GlyphType.Major, @"Your Heroic Throw applies a stack of Sunder Armor.")]
        public bool GlyphOfHeroicThrow { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the duration of your Intercept stun by 1 sec.</summary>
        [GlyphData(13, 67482, 94372, "Glyph of Intercept", GlyphType.Major, @"Increases the duration of your Intercept stun by 1 sec.")]
        public bool GlyphOfIntercept { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Increases the number of attacks you intercept for your Intervene target by 1.</summary>
        [GlyphData(14, 43419, 58377, "Glyph of Intervene", GlyphType.Major, @"Increases the number of attacks you intercept for your Intervene target by 1.")]
        public bool GlyphOfIntervene { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases the range of your Charge ability by 5 yards.</summary>
        [GlyphData(15, 43397, 58097, "Glyph of Long Charge", GlyphType.Major, @"Increases the range of your Charge ability by 5 yards.")]
        public bool GlyphOfLongCharge { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the radius of Piercing Howl by 50%.</summary>
        [GlyphData(16, 43417, 58372, "Glyph of Piercing Howl", GlyphType.Major, @"Increases the radius of Piercing Howl by 50%.")]
        public bool GlyphOfPiercingHowl { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Reduces the cooldown of your Charge ability by 7%.</summary>
        [GlyphData(17, 43413, 58355, "Glyph of Rapid Charge", GlyphType.Major, @"Reduces the cooldown of your Charge ability by 7%.")]
        public bool GlyphOfRapidCharge { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the rage cost of your Thunder Clap ability by 5.</summary>
        [GlyphData(18, 43430, 58356, "Glyph of Resonating Power", GlyphType.Major, @"Reduces the rage cost of your Thunder Clap ability by 5.")]
        public bool GlyphOfResonatingPower { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.</summary>
        [GlyphData(19, 45797, 63329, "Glyph of Shield Wall", GlyphType.Major, @"Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.")]
        public bool GlyphOfShieldWall { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the cooldown on Shockwave by 3 sec.</summary>
        [GlyphData(20, 45792, 63325, "Glyph of Shockwave", GlyphType.Major, @"Reduces the cooldown on Shockwave by 3 sec.")]
        public bool GlyphOfShockwave { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the cooldown on Spell Reflection by 1 sec.</summary>
        [GlyphData(21, 45795, 63328, "Glyph of Spell Reflection", GlyphType.Major, @"Reduces the cooldown on Spell Reflection by 1 sec.")]
        public bool GlyphOfSpellReflection { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Sunder Armor ability effects a second nearby target.</summary>
        [GlyphData(22, 43427, 58387, "Glyph of Sunder Armor", GlyphType.Major, @"Your Sunder Armor ability effects a second nearby target.")]
        public bool GlyphOfSunderArmor { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Reduces the rage cost of Sweeping Strikes ability by 100%.</summary>
        [GlyphData(23, 43428, 58384, "Glyph of Sweeping Strikes", GlyphType.Major, @"Reduces the rage cost of Sweeping Strikes ability by 100%.")]
        public bool GlyphOfSweepingStrikes { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Increases the radius of your Thunder Clap ability by 2 yards.</summary>
        [GlyphData(24, 43399, 58098, "Glyph of Thunder Clap", GlyphType.Major, @"Increases the radius of your Thunder Clap ability by 2 yards.")]
        public bool GlyphOfThunderClap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the total healing provided by your Victory Rush by 50%.</summary>
        [GlyphData(25, 43431, 58382, "Glyph of Victory Rush", GlyphType.Major, @"Increases the total healing provided by your Victory Rush by 50%.")]
        public bool GlyphOfVictoryRush { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 26; } }
        /// <summary>Increases the duration by 2 min and area of effect by 50% of your Battle Shout.</summary>
        [GlyphData(26, 43395, 58095, "Glyph of Battle", GlyphType.Minor, @"Increases the duration by 2 min and area of effect by 50% of your Battle Shout.")]
        public bool GlyphOfBattle { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Berserker Rage generates 5 rage when used.</summary>
        [GlyphData(27, 43396, 58096, "Glyph of Berserker Rage", GlyphType.Minor, @"Berserker Rage generates 5 rage when used.")]
        public bool GlyphOfBerserkerRage { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Increases the healing your receive from your Bloodthirst ability by 100%.</summary>
        [GlyphData(28, 43412, 58369, "Glyph of Bloody Healing", GlyphType.Minor, @"Increases the healing your receive from your Bloodthirst ability by 100%.")]
        public bool GlyphOfBloodyHealing { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.</summary>
        [GlyphData(29, 49084, 68164, "Glyph of Command", GlyphType.Minor, @"Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.")]
        public bool GlyphOfCommand { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.</summary>
        [GlyphData(30, 43398, 58099, "Glyph of Demoralizing Shout", GlyphType.Minor, @"Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.")]
        public bool GlyphOfDemoralizingShout { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Increases the window of opportunity in which you can use Victory Rush by 5 sec.</summary>
        [GlyphData(31, 43400, 58104, "Glyph of Enduring Victory", GlyphType.Minor, @"Increases the window of opportunity in which you can use Victory Rush by 5 sec.")]
        public bool GlyphOfEnduringVictory { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cost of Sunder Armor by 50%.</summary>
        [GlyphData(32, 45793, 63326, "Glyph of Furious Sundering", GlyphType.Minor, @"Reduces the cost of Sunder Armor by 50%.")]
        public bool GlyphOfFuriousSundering { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.</summary>
        [GlyphData(33, 45794, 63327, "Glyph of Intimidating Shout", GlyphType.Minor, @"Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.")]
        public bool GlyphOfIntimidatingShout { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
    }

    public partial class ShamanTalents
    {
        private bool[] _glyphData = new bool[32];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime Glyphs
        [GlyphData(16, 45775, "Glyph of Earth Shield", GlyphType.Prime, @"Increases the amount healed by your Earth Shield by 20%.")]
        public bool GlyphofEarthShield { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(20, 45772, "Glyph of Riptide", GlyphType.Prime, @"Increases the duration of Riptide by 40%.")]
        public bool GlyphofRiptide { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(12, 41527, "Glyph of Earthliving Weapon", GlyphType.Prime, @"Increases the effectiveness of your Earthliving Weapon's periodic healing by 20%.")]
        public bool GlyphofEarthlivingWeapon { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(11, 41541, "Glyph of Water Shield", GlyphType.Prime, @"Increases the passive mana regeneration of your Water Shield spell by 50%.")]
        public bool GlyphofWaterMastery { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(5, 41532, "Glyph of Flametongue Weapon", GlyphType.Prime, @"Increases spell critical strike chance by 2% while Flametongue Weapon is active.")]
        public bool GlyphofFlametongueWeapon { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(17, 45771, "Glyph of Feral Spirit", GlyphType.Prime, @"Your spirit wolves gain an additional 30% of your attack power.")]
        public bool GlyphofFeralSpirit { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(27, 41529, "Glyph of Fire Elemental Totem", GlyphType.Prime, @"Reduces the cooldown of your Fire Elemental Totem by 5 min.")]
        public bool GlyphofFireElementalTotem { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(4, 41531, "Glyph of Flame Shock", GlyphType.Prime, @"Increases the duration of your Flame Shock by 50%.")]
        public bool GlyphofFlameShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(6, 41540, "Glyph of Lava Lash", GlyphType.Prime, @"Increases the damage dealt by your Lava Lash ability by 20%.")]
        public bool GlyphofLavaLash { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(26, 41524, "Glyph of Lava Burst", GlyphType.Prime, @"Your Lava Burst spell deals 10% more damage.")] //Consider renaming Lava Burst
        public bool GlyphofLava { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(1, 41536, "Glyph of Lightning Bolt", GlyphType.Prime, @"Increases the damage dealt by Lightning Bolt by 4%.")]
        public bool GlyphofLightningBolt { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 41526, "Glyph of Shocking", GlyphType.Prime, @"Reduces the global cooldown triggered by your shock spells to 1 sec.")]
        public bool GlyphofShocking { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(22, 41539, "Glyph of Stormstrike", GlyphType.Prime, @"Increases the critical strike chance bonus from your Stormstrike ability by an additional 10%.")]
        public bool GlyphofStormstrike { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(13, 41542, "Glyph of Windfury Weapon", GlyphType.Prime, @"Increases the chance per swing for Windfury Weapon to trigger by 2%.")]
        public bool GlyphofWindfuryWeapon { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        #endregion
        #region Major Glyphs
        [GlyphData(0, 41534, "Glyph of Healing Wave", GlyphType.Major, @"Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.")]
        public bool GlyphofHealingWave { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(9, 41533, "Glyph of Healing Stream Totem", GlyphType.Major, @"Your Healing Stream Totem increases all the resistances of nearby party and raid members by 130.")]
        public bool GlyphofHealingStreamTotem { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(15, 41517, "Glyph of Chain Heal", GlyphType.Major, @"Increases healing done by your Chain Heal to targets beyond the first by 15%, but decreases the amount received by the initial target by 10%.")]
        public bool GlyphofChainHeal { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(3, 41537, "Glyph of Lightning Shield", GlyphType.Major, @"Your Lightning Shield can no longer drop below 3 charges from dealing damage to attackers.")]
        public bool GlyphofLightningShield { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(7, 41530, "Glyph of Fire Nova", GlyphType.Major, @"Increases the radius of your Fire Nova spell by 5 yards.")]
        public bool GlyphofFireNova { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 41547, "Glyph of Frost Shock", GlyphType.Major, @"Increases the duration of your Frost Shock by 2 sec.")]
        public bool GlyphofFrostShock { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(14, 41518, "Glyph of Chain Lightning", GlyphType.Major, @"Your Chain Lightning spell now strikes 2 additional targets, but deals 10% less damage.")]
        public bool GlyphofChainLightning { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(18, 45777, "Glyph of Hex", GlyphType.Major, @"Reduces the cooldown on Hex by 15 seconds.")]
        public bool GlyphofHex { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(21, 45778, "Glyph of Stoneclaw Totem", GlyphType.Major, @"Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.")]
        public bool GlyphofStoneclawTotem { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(23, 45770, "Glyph of Thunder", GlyphType.Major, @"Reduces the cooldown on Thunderstorm by 10 sec.")]
        public bool GlyphofThunder { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 43725, "Glyph of Ghost Wolf", GlyphType.Major, @"Your Ghost Wolf form grants an additional 5% movement speed.")]
        public bool GlyphofTotemofWrath { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 41552, "Glyph of Elemental Mastery", GlyphType.Major, @"While your Elemental Mastery ability is active, you take 20% less damage from all sources.")]
        public bool GlyphofElementalMastery { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(29, 41538, "Glyph of Grounding Totem", GlyphType.Major, @"Instead of absorbing a spell, your Grounding Totem reflects the next harmful spell back at its caster, but the cooldown of your Grounding Totem is increased by 45 sec.")]
        public bool GlyphofGroundingTotem { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 45776, "Glyph of Shamanistic Rage", GlyphType.Major, @"Activating your Shamanistic Rage ability also cleanses you of all dispellable Magic debuffs.")]
        public bool GlyphofShamanisticRage { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, 41535, "Glyph of Totemic Recall", GlyphType.Major, @"Causes your Totemic Recall ability to return an additional 50% of the mana cost of any recalled totems.")]
        public bool GlyphofTotemicRecall { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        #endregion
        #region Minor Glyphs
        [GlyphData(28, 44923, "Glyph of Thunderstorm", GlyphType.Minor, @"Increases the mana you recieve from your Thunderstorm spell by 2%, but it no longer knocks enemies back.")]
        public bool GlyphofThunderstorm { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion

    }

    public partial class PriestTalents
    {
        private bool[] _glyphData = new bool[37];
        public override bool[] GlyphData { get { return _glyphData; } }
        #region Prime
        [GlyphData(2, 45753, "Glyph of Dispersion", GlyphType.Prime, @"Reduces the cooldown of your Dispersion by 45 sec.")]
        public bool GlyphofDispersion { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(5, 42400, "Glyph of Flash Heal", GlyphType.Prime, @"Increases the critical chance of your Flash Heal spell on targets below 25% by 10%.")]
        public bool GlyphofFlashHeal { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, 45755, "Glyph of Guardian Spirit", GlyphType.Prime, @"Reduces the cooldown of Guardian Spirit by 30 sec.")]
        public bool GlyphofGuardianSpirit { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(10, 42403, "Glyph of Lightwell", GlyphType.Prime, @"Increases the total amount of charges on your Lightwell by 5.")]
        public bool GlyphofLightwell { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(13, 42415, "Glyph of Mind Flay", GlyphType.Prime, @"Increases the damage done by your Mind Flay spell by 10%.")]
        public bool GlyphofMindFlay { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(16, 45756, "Glyph of Penance", GlyphType.Prime, @"Reduces the cooldown of Penance by 2 sec.")]
        public bool GlyphofPenance { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(33, 42407, "Glyph of Power Word: Barrier", GlyphType.Prime, @"Increases the healing received while under the Barrier by 10%.")]
        public bool GlyphofPowerWordBarrier { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        [GlyphData(17, 42408, "Glyph of Power Word: Shield", GlyphType.Prime, @"Your Power Word: Shield also heals the target for 20% of absorption amount.")]
        public bool GlyphofPowerWordShield { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, 42409, "Glyph of Prayer of Healing", GlyphType.Prime, @"Your Prayer of Healing spell also heals an additional 20% of its initial heal over 6 sec.")]
        public bool GlyphofPrayerofHealing { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(20, 42411, "Glyph of Renew", GlyphType.Prime, @"Increases the amount healed by your Renew by 10%.")]
        public bool GlyphofRenew { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(23, 42414, "Glyph of Shadow Word: Death", GlyphType.Prime, @"If your Shadow Word: Death target fails to kill the target at 25% or less, your Shadow Word: Death cooldown is reset. This effect can not occur more than once every 6 seconds.")]
        public bool GlyphofShadowWordDeath { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 42406, "Glyph of Shadow Word: Pain", GlyphType.Prime, @"Increases the periodic damage of your Shadow Word: Pain by 10%.")]
        public bool GlyphofShadowWordPain { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        #endregion
        #region Major
        [GlyphData(0, 42396, "Glyph of Circle of Healing", GlyphType.Major, @"Your Circle of Healing spell heals 1 additional target.")]
        public bool GlyphofCircleOfHealing { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 42397, "Glyph of Dispel Magic", GlyphType.Major, @"Your Dispel Magic spell also heals your target for 3% maximum health.")]
        public bool GlyphofDispelMagic { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(3, 42398, "Glyph of Fade", GlyphType.Major, @"Reduces the cooldown of your Fade spell by 9 sec.")]
        public bool GlyphofFade { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, 42399, "Glyph of Fear Ward", GlyphType.Major, @"Reduces the cooldown and duration of Fear Ward by 60 sec.")]
        public bool GlyphofFearWard { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(7, 42401, "Glyph of Holy Nova", GlyphType.Major, @"Reduces the global cooldown of your Holy Nova by 0.5 sec.")]
        public bool GlyphofHolyNova { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 42417, "Glyph of Prayer of Mending", GlyphType.Major, @"Your first charge of your Prayer of Mending heals for an additional 60%")]
        public bool GlyphofPrayerOfMending { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, 42402, "Glyph of Inner Fire", GlyphType.Major, @"Increases the armor from your Inner Fire spell by 50%.")]
        public bool GlyphofInnerFire { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(11, 42404, "Glyph of Mass Dispel", GlyphType.Major, @"Reduces the cast time of Mass Dispel by 1 second.")]
        public bool GlyphofMassDispel { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(15, 45760, "Glyph of Desperation", GlyphType.Major, @"Allows Pain Suppression and Guardian Spirit to be cast while stunned.")]
        public bool GlyphofDesperation { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(19, 42410, "Glyph of Psychic Scream", GlyphType.Major, @"Targets of your Psychic Scream tremble in place instead of fleeing in horror. Also increases the cooldown by 3 seconds.")]
        public bool GlyphofPsychicScream { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(21, 42412, "Glyph of Scourge Imprisonment", GlyphType.Major, @"Reduces the cast time of your Shackle Undead by 1.0 sec.")]
        public bool GlyphofScourgeImprisonment { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(25, 42416, "Glyph of Smite", GlyphType.Major, @"Your Smite spell inflicts an additional 20% damage against targets afflicted by Holy Fire.")]
        public bool GlyphofSmite { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(34, 45758, "Glyph of Divine Accuracy", GlyphType.Major, @"Increases your chance to hit with your Smite and Holy Fire by 18%.")]
        public bool GlyphofDivineAccuracy { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        [GlyphData(35, 42405, "Glyph of Psychic Horror", GlyphType.Major, @"Reduces the cooldown of your Psychic Horror by 60 sec.")]
        public bool GlyphofPsychicHorror { get { return _glyphData[35]; } set { _glyphData[35] = value; } }
        [GlyphData(36, 45757, "Glyph of Spirit Tap", GlyphType.Major, @"When you kill a target with your Shadow Word: Death and yield experience or honor, you instantly receive 12% of your total mana over 12 sec.")]
        public bool GlyphofSpiritTap { get { return _glyphData[36]; } set { _glyphData[36] = value; } }
        #endregion
        #region Minor
        [GlyphData(27, 43342, "Glyph of Fading", GlyphType.Minor, @"Reduces the mana cost of your Fade spell by 30%.")]
        public bool GlyphofFading { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, 43371, "Glyph of Fortitude", GlyphType.Minor, @"Reduces the mana cost of your Power Word: Fortitude by 50%.")]
        public bool GlyphofFortitude { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, 43370, "Glyph of Levitate", GlyphType.Minor, @"Your Levitate spell no longer requires a reagent.")]
        public bool GlyphofLevitate { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 43373, "Glyph of Shackle Undead", GlyphType.Minor, @"Increases the range of your Shackle Undead spell by 5 yards.")]
        public bool GlyphofShackleUndead { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, 43372, "Glyph of Shadow Protection", GlyphType.Minor, @"Increases the duration of your Shadow Protection and Prayer of Shadow Protection spells by 10 min.")]
        public bool GlyphofShadowProtection { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, 43374, "Glyph of Shadowfiend", GlyphType.Minor, @"Receive 5% of your maximum mana if your Shadowfiend dies from damage.")]
        public bool GlyphofShadowfiend { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        #endregion

    }

    public partial class DeathKnightTalents
    {
        private bool[] _glyphData = new bool[11 + 13 + 6]; // Prime + Major + Minor
        public override bool[] GlyphData { get { return _glyphData; } }
        #region Prime
        /// <summary>Death Coil - Increases the damage or healing done by Death Coil by 15%.</summary>
        [GlyphData(0, 45804, "Glyph of Death Coil", GlyphType.Prime, @"Increases the damage or healing done by Death Coil by 15%.")]
        public bool GlyphofDeathCoil { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Death Strike - Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.</summary>
        [GlyphData(1, 43827,"Glyph of Death Strike", GlyphType.Prime, @"Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.")]
        public bool GlyphofDeathStrike { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Death and Decay - Increases the duration of your Death and Decay spell by 50%.</summary>
        [GlyphData(2, 43542, "Glyph of Death and Decay", GlyphType.Prime, @"Increases the duration of your Death and Decay spell by 50%.")]
        public bool GlyphofDeathandDecay { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Frost Strike - Reduces the cost of your Frost Strike by 8 Runic Power.</summary>
        [GlyphData(3, 43543, "Glyph of Frost Strike", GlyphType.Prime, @"Reduces the cost of your Frost Strike by 8 Runic Power.")]
        public bool GlyphofFrostStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Heart Strike - Increases the damage of your Heart Strike ability by 30%.</summary>
        [GlyphData(4, 43534, "Glyph of Heart Strike", GlyphType.Prime, @"Increases the damage of your Heart Strike ability by 30%.")]
        public bool GlyphofHeartStrike { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Howling Blast - Your Howling Blast ability now infects your targets with Frost Fever.</summary>
        [GlyphData(5, 45806, "Glyph of Howling Blast", GlyphType.Prime, @"Your Howling Blast ability now infects your targets with Frost Fever.")]
        public bool GlyphofHowlingBlast { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Icy Touch - Your Frost Fever disease deals 20% additional damage.</summary>
        [GlyphData(6, 43546, "Glyph of Icy Touch", GlyphType.Prime, @"Your Frost Fever disease deals 20% additional damage.")]
        public bool GlyphofIcyTouch { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Obliterate - Increases the damage of your Obliterate ability by 20%.</summary>
        [GlyphData(7, 43547, "Glyph of Obliterate", GlyphType.Prime, @"Increases the damage of your Obliterate ability by 20%.")]
        public bool GlyphofObliterate { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Raise Dead - Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.</summary>
        [GlyphData(8, 43549, "Glyph of Raise Dead", GlyphType.Prime, @"Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.")]
        public bool GlyphofRaiseDead { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Rune Strike - Increases the critical strike chance of your Rune Strike by 10%.</summary>
        [GlyphData(9, 43550, "Glyph of Rune Strike", GlyphType.Prime, @"Increases the critical strike chance of your Rune Strike by 10%.")]
        public bool GlyphofRuneStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Scourge Strike - Increases the Shadow damage portion of your Scourge Strike by 30%.</summary>
        [GlyphData(10, 43551, "Glyph of Scourge Strike", GlyphType.Prime, @"Increases the Shadow damage portion of your Scourge Strike by 30%.")]
        public bool GlyphofScourgeStrike { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        #endregion
        #region Major
        /// <summary>Increases the duration of your Anti-Magic Shell by 2 sec.</summary>
        [GlyphData(11, 43533, "Glyph of Anti-Magic Shell", GlyphType.Major, @"Increases the duration of your Anti-Magic Shell by 2 sec.")]
        public bool GlyphofAntiMagicShell { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the radius of your Blood Boil ability by 50%.</summary>
        [GlyphData(12, 43826, "Glyph of Blood Boil", GlyphType.Major, @"Increases the radius of your Blood Boil ability by 50%.")]
        public bool GlyphofBloodBoil { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.</summary>
        [GlyphData(13, 43536, "Glyph of Bone Shield", GlyphType.Major, @"Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.")]
        public bool GlyphofBoneShield { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.</summary>
        [GlyphData(14, 43537, "Glyph of Chains of Ice", GlyphType.Major, @"Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.")]
        public bool GlyphofChainsofIce { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases your threat generation by 50% while your Dancing Rune Weapon is active.</summary>
        [GlyphData(15, 45799, "Glyph of Dancing Rune Weapon", GlyphType.Major, @"Increases your threat generation by 50% while your Dancing Rune Weapon is active.")]
        public bool GlyphofDancingRuneWeapon { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        // Note: Glyph data index out of order to make for better group by category. 
        // New glyphs need to have higher indexes as to not break old character XMLs 
        /// <summary>Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.</summary>
        [GlyphData(29, 68793, "Glyph of Dark Succor", GlyphType.Major, @"Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.")]
        public bool GlyphofDarkSuccor { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the range of your Death Grip ability by 5 yards.</summary>
        [GlyphData(16, 43541, "Glyph of Death Grip", GlyphType.Major, @"Increases the range of your Death Grip ability by 5 yards.")]
        public bool GlyphofDeathGrip { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Your Hungering Cold ability no longer costs runic power.</summary>
        [GlyphData(17, 45800, "Glyph of Hungering Cold", GlyphType.Major, @"Your Hungering Cold ability no longer costs runic power.")]
        public bool GlyphofHungeringCold { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the radius of your Pestilence effect by 5 yards.</summary>
        [GlyphData(18, 43548, "Glyph of Pestilence", GlyphType.Major, @"Increases the radius of your Pestilence effect by 5 yards.")]
        public bool GlyphofPestilence { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.</summary>
        [GlyphData(19, 43553, "Glyph of Pillar of Frost", GlyphType.Major, @"Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.")]
        public bool GlyphofPillarofFrost { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Your Rune Tap also heals your party for 10% of their maximum health.</summary>
        [GlyphData(20, 43825, "Glyph of Rune Tap", GlyphType.Major, @"Your Rune Tap also heals your party for 10% of their maximum health.")]
        public bool GlyphofRuneTap { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.</summary>
        [GlyphData(21, 43552, "Glyph of Strangulate", GlyphType.Major, @"Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.")]
        public bool GlyphofStrangulate { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.</summary>
        [GlyphData(22, 43554, "Glyph of Vampiric Blood", GlyphType.Major, @"Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.")]
        public bool GlyphofVampiricBlood { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        #endregion
        #region Minor
        /// <summary>Your Blood Tap no longer causes damage to you.</summary>
        [GlyphData(23, 43535, "Glyph of Blood Tap", GlyphType.Minor, @"Your Blood Tap no longer causes damage to you.")]
        public bool GlyphofBloodTap { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Death Coil refunds 20 runic power when used to heal.</summary>
        [GlyphData(24, 43539, "Glyph of Death's Embrace", GlyphType.Minor, @"Your Death Coil refunds 20 runic power when used to heal.")]
        public bool GlyphofDeathsEmbrace { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the duration of your Horn of Winter ability by 1 min.</summary>
        [GlyphData(25, 43544, "Glyph of Horn of Winter", GlyphType.Minor, @"Increases the duration of your Horn of Winter ability by 1 min.")]
        public bool GlyphofHornofWinter { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Your Path of Frost ability allows you to fall from a greater distance without suffering damage.</summary>
        [GlyphData(26, 43671, "Glyph of Path of Frost", GlyphType.Minor, @"Your Path of Frost ability allows you to fall from a greater distance without suffering damage.")]
        public bool GlyphofPathofFrost { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Increases the health of your Risen Ally by 25% and its run speed by 15%.</summary>
        [GlyphData(27, 43673, "Glyph of Raise Ally", GlyphType.Minor, @"Increases the health of your Risen Ally by 25% and its run speed by 15%.")]
        public bool GlyphofRaiseAlly { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>When your Death Grip ability fails because its target is immune, its cooldown is reset.</summary>
        [GlyphData(28, 43672, "Glyph of Resilient Grip", GlyphType.Minor, @"When your Death Grip ability fails because its target is immune, its cooldown is reset.")]
        public bool GlyphofResilientGrip { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        // NOTE: Current Max index is 29.
        // Next new glyph should be 30.
    } 

    public partial class WarlockTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        [GlyphData(3, 42456, "Glyph of Bane of Agony", GlyphType.Prime, @"Increases the duration of your Bane of Agony by 4 sec.")]
        public bool GlyphCoA { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(0, 45781, "Glyph of Chaos Bolt", GlyphType.Prime, @"Reduces the cooldown on Chaos Bolt by 2 sec.")]
        public bool GlyphChaosBolt { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 42454, "Glyph of Conflagrate", GlyphType.Prime, @"Reduces the cooldown of your Conflagrate by 2 sec.")]
        public bool GlyphConflag { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 42455, "Glyph of Corruption", GlyphType.Prime, @"Your Corruption spell has a 4% chance to cause you to enter a Shadow Trance state" +
                    " after damaging the opponent.  The Shadow Trance state reduces the casting time " +
                    "of your next Shadow Bolt spell by 100%.")]
        public bool GlyphCorruption { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(4, 42459, "Glyph of Felguard", GlyphType.Prime, @"Increases the damage done by your Felguard's Legion Strike by 5%.")]
        public bool GlyphFelguard { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, 45779, "Glyph of Haunt", GlyphType.Prime, @"The bonus damage granted by your Haunt spell is increased by an additional 3%.")]
        public bool GlyphHaunt { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, 42464, "Glyph of Immolate", GlyphType.Prime, @"Increases the periodic damage of your Immolate by 10%.")]
        public bool GlyphImmolate { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, 42465, "Glyph of Imp", GlyphType.Prime, @"Increases the damage done by your Imp's Firebolt spell by 20%.")]
        public bool GlyphImp { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 42453, "Glyph of Incinerate", GlyphType.Prime, @"Increases the damage done by Incinerate by 5%.")]
        public bool GlyphIncinerate { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, 50077, "Glyph of Lash of Pain", GlyphType.Prime, @"Increases the damage done by your Succubus' Lash of Pain by 25%.")]
        public bool GlyphLashPain { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(11, 45780, "Glyph of Metamorphosis", GlyphType.Prime, @"Increases the duration of your Metamorphosis by 6 sec.")]
        public bool GlyphMetamorphosis { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        [GlyphData(13, 42468, "Glyph of Shadowburn", GlyphType.Prime, @"If your Shadowburn fails to kill the target at or below 20% health, " +
                    "your Shadowburn's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphShadowburn { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 42472, "Glyph of Unstable Affliction", GlyphType.Prime, @"Decreases the casting time of your Unstable Affliction by 0.2 sec.")]
        public bool GlyphUA { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        #endregion
        #region Major
        [GlyphData(16, 42457, "Glyph of Death Coil", GlyphType.Major, @"Increases the duration of your Death Coil by 0.5 sec.")]
        public bool GlyphDeathCoil { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 45782, "Glyph of Demonic Circle", GlyphType.Major, @"Reduces the cooldown on Demonic Circle by 4 sec.")]
        public bool GlyphDemonicCircle { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(21, 42458, "Glyph of Fear", GlyphType.Major, @"Your Fear causes the target to tremble in place instead of fleeing in fear, but now causes Fear to have a 5 sec cooldown.")]
        public bool GlyphFear { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, 42450, "Glyph of Felhunter", GlyphType.Major, @"When your Felhunter uses Devour Magic, you will also be healed for that amount.")]
        public bool GlyphFelhunter { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(24, 42462, "Glyph of Healthstone", GlyphType.Major, @"You receive 30% more healing from using a healthstone.")]
        public bool GlyphHealthstone { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 42463, "Glyph of Howl of Terror", GlyphType.Major, @"Reduces the cooldown on your Howl of Terror spell by 8 sec.")]
        public bool GlyphHowlTerror { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(10, 45785, "Glyph of Life Tap", GlyphType.Major, @"Reduces the global cooldown of your Life Tap by .5 sec.")]
        public bool GlyphLifeTap { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(27, 42471, "Glyph of Seduction", GlyphType.Major, @"Your Succubus's Seduction ability also removes all damage over time effects from the target.")]
        public bool GlyphSeduction { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(12, 42467, "Glyph of Shadow Bolt", GlyphType.Major, @"Reduces the mana cost of your Shadow Bolt by 15%.")]
        public bool GlyphSB { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(28, 45783, "Glyph of Shadowflame", GlyphType.Major, @"Your Shadowflame also applies a 70% movement speed slow to its victims.")]
        public bool GlyphShadowflame { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, 45789, "Glyph of Soul Link", GlyphType.Major, @"Increases the percentage of damage shared via your Soul Link by an additional 5%.")]
        public bool GlyphSoulLink { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        [GlyphData(30, 42466, "Glyph of Soul Swap", GlyphType.Major, @"Your Soul Swap leaves your damage-over-time spells behind on the target you Soul Swapped from, "
            + "but gives Soul Swap a 10 sec. cooldown.")]
        public bool GlyphSoulSwap { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        [GlyphData(31, 42470, "Glyph of Soulstone", GlyphType.Major, @"Increases the amount of health you gain from resurrecting via a Soulstone by an additional 40%.")]
        public bool GlyphSoulstone { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(33, 42473, "Glyph of Voidwalker", GlyphType.Major, @"Increases your Voidwalker's total health by 20%.")]
        public bool GlyphVoidwalker { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
        #region Minor
        [GlyphData(15, 43392, "Glyph of Curse of Exhaustion", GlyphType.Minor, @"Increases the range of your Curse of Exhaustion spell by 5 yards.")]
        public bool GlyphCurseOfExhaustion { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(18, 43390, "Glyph of Drain Soul", GlyphType.Minor, @"Your Drain Soul restores 10% of your total mana after you kill a target that yields experience or honor.")]
        public bool GlyphDrainSoul { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 43393, "Glyph of Enslave Demon", GlyphType.Minor, @"Reduces the cast time of your Enslave Demon spell by 50%.")]
        public bool GlyphEnslaveDemon { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, 43391, "Glyph of Eye of Kilrogg", GlyphType.Minor, @"Increases the movement speed of your Eye of Kilrogg by 50% and allows it to fly in areas where "
            + "flying mounts are enabled.")]
        public bool GlyphEyeKilrogg { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(23, 42461, "Glyph of Health Funnel", GlyphType.Minor, @"Reduces the pushback suffered from damaging attacks while channeling your Health Funnel spell by 100%.")]
        public bool GlyphHealthFunnel { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(26, 43394, "Glyph of Ritual of Souls", GlyphType.Minor, @"Reduces the mana cost of your Ritual of Souls spell by 70%.")]
        public bool GlyphRitualSouls { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(32, 43389, "Glyph of Unending Breath", GlyphType.Minor, @"Increases the swim speed of targets affected by your Unending Breath spell by 20%.")]
        public bool GlyphUnendingBreath { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        #endregion       
    }

    public partial class RogueTalents
    {
        private bool[] _glyphData = new bool[36];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        [GlyphData(0, 42954, "Glyph of Adrenaline Rush", GlyphType.Prime, @"Increases the duration of Adrenaline Rush by 5 sec.")]
        public bool GlyphOfAdrenalineRush { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        [GlyphData(1, 42956, "Glyph of Backstab", GlyphType.Prime, @"Your Backstab critical strikes grant you 5 energy.")]
        public bool GlyphOfBackstab { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        [GlyphData(2, 42961, "Glyph of Eviscerate", GlyphType.Prime, @"Increases the critical strike chance of Eviscerate by 10%.")]
        public bool GlyphOfEviscerate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        [GlyphData(3, 42967, "Glyph of Hemorrhage", GlyphType.Prime, @"Your Hemorrhage ability also causes the target to bleed, dealing 40% of the direct strike's damage over 24 sec.")]
        public bool GlyphOfHemorrhage { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        [GlyphData(4, 45762, "Glyph of Killing Spree", GlyphType.Prime, @"Increases the bonus to your damage while Killing Spree is active by an additional 10%.")]
        public bool GlyphOfKillingSpree { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        [GlyphData(5, 45768, "Glyph of Mutilate", GlyphType.Prime, @"Reduces the cost of Mutilate by 5 energy.")]
        public bool GlyphOfMutilate { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        [GlyphData(6, 42965, "Glyph of Revealing Strike", GlyphType.Prime, @"Increases Revealing Strike's bonus effectiveness to your finishing moves by an additional 10%.")]
        public bool GlyphOfRevealingStrike { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        [GlyphData(7, 42969, "Glyph of Rupture", GlyphType.Prime, @"Increases the duration of Rupture by 4 sec.")]
        public bool GlyphOfRupture { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        [GlyphData(8, 45764, "Glyph of Shadow Dance", GlyphType.Prime, @"Increases the duration of Shadow Dance by 2 sec.")]
        public bool GlyphOfShadowDance { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        [GlyphData(9, 42972, "Glyph of Sinister Strike", GlyphType.Prime, @"Your Sinister Strikes have a 20% chance to add an additional combo point.")]
        public bool GlyphOfSinisterStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        [GlyphData(10, 42973, "Glyph of Slice and Dice", GlyphType.Prime, @"Increases the duration of Slice and Dice by 3 sec.")]
        public bool GlyphOfSliceandDice { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        [GlyphData(11, 45761, "Glyph of Vendetta", GlyphType.Prime, @"Increases the duration of your Vendetta ability by 20%.")]
        public bool GlyphOfVendetta { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        #endregion
        #region Major
        [GlyphData(12, 42955, "Glyph of Ambush", GlyphType.Major, @"Increases the range on Ambush by 5 yards.")]
        public bool GlyphOfAmbush { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        [GlyphData(13, 42957, "Glyph of Blade Flurry", GlyphType.Major, @"Reduces the penalty to energy generation while Blade Flurry is active by 50%.")]
        public bool GlyphOfBladeFlurry { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        [GlyphData(14, 64493, "Glyph of Blind", GlyphType.Major, @"Your Blind ability also removes all damage over time effects from the target.")]
        public bool GlyphOfBlind { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        [GlyphData(15, 45769, "Glyph of Cloak of Shadows", GlyphType.Major, @"While Cloak of Shadows is active, you take 40% less physical damage.")]
        public bool GlyphOfCloakOfShadows { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        [GlyphData(16, 42958, "Glyph of Crippling Poison", GlyphType.Major, @"Increases the chance to trigger Crippling Poison by 20%.")]
        public bool GlyphOfCripplingPoison { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        [GlyphData(17, 42959, "Glyph of Deadly Throw", GlyphType.Major, @"Increases the slowing effect on Deadly Throw by 20%.")]
        public bool GlyphOfDeadlyThrow { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        [GlyphData(18, 42960, "Glyph of Evasion", GlyphType.Major, @"Increases the duration of Evasion by 5 sec.")]
        public bool GlyphOfEvasion { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        [GlyphData(19, 42962, "Glyph of Expose Armor", GlyphType.Major, @"Increases the duration of Expose Armor by 12 sec.")]
        public bool GlyphOfExposeArmor { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        [GlyphData(20, 45766, "Glyph of Fan of Knives", GlyphType.Major, @"Increases the radius of your Fan of Knives ability by 50%.")]
        public bool GlyphOfFanOfKnives { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        [GlyphData(21, 42963, "Glyph of Feint", GlyphType.Major, @"Reduces the energy cost of Feint by 20.")]
        public bool GlyphOfFeint { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        [GlyphData(22, 42964, "Glyph of Garrote", GlyphType.Major, @"Increases the duration of your Garrote ability's silence effect by 2 sec.")]
        public bool GlyphOfGarrote { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        [GlyphData(23, 42966, "Glyph of Gouge", GlyphType.Major, @"Your Gouge ability no longer requires that the target be facing you.")]
        public bool GlyphOfGouge { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        [GlyphData(24, 42971, "Glyph of Kick", GlyphType.Major, @"Increases the cooldown of your Kick ability by 4 sec, but this cooldown is reduced by 6 sec when your Kick successfully interrupts a spell.")]
        public bool GlyphOfKick { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        [GlyphData(25, 42968, "Glyph of Preparation", GlyphType.Major, @"Your Preparation ability also instantly resets the cooldown of Kick, Dismantle, and Smoke Bomb.")]
        public bool GlyphOfPreparation { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        [GlyphData(26, 42970, "Glyph of Sap", GlyphType.Major, @"Increases the duration of Sap against non-player targets by 80 sec.")]
        public bool GlyphOfSap { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        [GlyphData(27, 42974, "Glyph of Sprint", GlyphType.Major, @"Increases the movement speed of your Sprint ability by an additional 30%.")]
        public bool GlyphOfSprint { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        [GlyphData(28, 45767, "Glyph of Tricks of the Trade", GlyphType.Major, @"Removes the energy cost of your Tricks of the Trade ability but reduces the recipient's damage bonus by 5%.")]
        public bool GlyphOfTricksOfTheTrade { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        [GlyphData(29, 63420, "Glyph of Vanish", GlyphType.Major, @"Increases the duration of your Vanish effect by 2 sec.")]
        public bool GlyphOfVanish { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        #endregion
        #region Minor
        [GlyphData(30, 43379, "Glyph of Blurred Speed", GlyphType.Minor, @"You gain the ability to walk on water while your Sprint ability is active.")]
        public bool GlyphOfBlurredSpeed { get { return _glyphData[30]; } set{ _glyphData[30] = value;} }
        [GlyphData(31, 43376, "Glyph of Distract", GlyphType.Minor, @"Increases the range of your Distract ability by 5 yards.")]
        public bool GlyphOfDistrict { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        [GlyphData(32, 43377, "Glyph of Pick Lock", GlyphType.Minor, @"Reduces the cast time of your Pick Lock ability by 100%.")]
        public bool GlyphOfPickLock { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        [GlyphData(33, 43343, "Glyph of Pick Pocket", GlyphType.Minor, @"Increases the range of your Pick Pocket ability by 5 yards.")]
        public bool GlyphOfPickPocket { get { return _glyphData[33]; } set{ _glyphData[33] = value;} }
        [GlyphData(34, 43380, "Glyph of Poisons", GlyphType.Minor, @"You apply poisons to your weapons 50% faster.")]
        public bool GlyphOfPoisons { get { return _glyphData[34]; } set{ _glyphData[34] = value;} }
        [GlyphData(35, 43378, "Glyph of Safe Fall", GlyphType.Minor, @"Increases the distance your Safe Fall ability allows you to fall without taking damage.")]
        public bool GlyphOfSafeFall { get { return _glyphData[35]; } set{ _glyphData[35] = value;} }
        #endregion
    }

    public partial class HunterTalents
    {
        private bool[] _glyphData = new bool[33]; // Set this to the final value of how many glyphs there end up being.
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        /// <summary>When you critically hit with Aimed Shot, you instantly gain 5 Focus.</summary>
        [GlyphData(0, 42897, "Glyph of Aimed Shot", GlyphType.Prime, @"When you critically hit with Aimed Shot, you instantly gain 5 Focus.")]
        public bool GlyphOfAimedShot { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Your Arcane Shot deals 12% more damage.</summary>
        [GlyphData(1, 42898, "Glyph of Arcane Shot", GlyphType.Prime, @"Your Arcane Shot deals 12% more damage.")]
        public bool GlyphOfArcaneShot { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of Chimera Shot by 1 sec.</summary>
        [GlyphData(2, 45625, "Glyph of Chimera Shot", GlyphType.Prime, @"Reduces the cooldown of Chimera Shot by 1 sec.")]
        public bool GlyphOfChimeraShot { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the critical strike chance of Explosive Shot by 6%.</summary>
        [GlyphData(3, 45731, "Glyph of Explosive Shot", GlyphType.Prime, @"Increases the critical strike chance of Explosive Shot by 6%.")]
        public bool GlyphOfExplosiveShot { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Reduces the Focus cost of your Kill Command by 3.</summary>
        [GlyphData(4, 42915, "Glyph of Kill Command", GlyphType.Prime, @"Reduces the Focus cost of your Kill Command by 3.")]
        public bool GlyphOfKillCommand { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.</summary>
        [GlyphData(5, 45732, "Glyph of Kill Shot", GlyphType.Prime, @"If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphOfKillShot { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the haste from Rapid Fire by an additional 10%.</summary>
        [GlyphData(6, 42911, "Glyph of Rapid Fire", GlyphType.Prime, @"Increases the haste from Rapid Fire by an additional 10%.")]
        public bool GlyphOfRapidFire { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the periodic critical strike chance of your Serpent Sting by 6%.</summary>
        [GlyphData(7, 42912, "Glyph of Serpent Sting", GlyphType.Prime, @"Increases the periodic critical strike chance of your Serpent Sting by 6%.")]
        public bool GlyphOfSerpentSting { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage dealt by Steady Shot by 10%.</summary>
        [GlyphData(8, 42914, "Glyph of Steady Shot", GlyphType.Prime, @"Increases the damage dealt by Steady Shot by 10%.")]
        public bool GlyphOfSteadyShot { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.</summary>
        [GlyphData(9, 42909, "Glyph of Dazzled Prey", GlyphType.Prime, @"Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.")]
        public bool GlyphOfDazzledPrey { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        #endregion
        #region Major
        /// <summary>Decreases the cooldown of Bestial Wrath by 20 sec.</summary>
        [GlyphData(10, 42902, "Glyph of Bestial Wrath", GlyphType.Major, @"Decreases the cooldown of Bestial Wrath by 20 sec.")]
        public bool GlyphOfBestialWrath { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Concussive Shot also limits the maximum run speed of your target.</summary>
        [GlyphData(11, 42901, "Glyph of Concussive Shot", GlyphType.Major, @"Your Concussive Shot also limits the maximum run speed of your target.")]
        public bool GlyphOfConcussiveShot { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Decreases the cooldown of Deterrence by 10 sec.</summary>
        [GlyphData(12, 42903, "Glyph of Deterrence", GlyphType.Major, @"Decreases the cooldown of Deterrence by 10 sec.")]
        public bool GlyphOfDeterrence { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Decreases the cooldown of Disengage by 5 sec.</summary>
        [GlyphData(13, 42904, "Glyph of Disengage", GlyphType.Major, @"Decreases the cooldown of Disengage by 5 sec.")]
        public bool GlyphOfDisengage { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.</summary>
        [GlyphData(14, 42905, "Glyph of Freezing Trap", GlyphType.Major, @"When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.")]
        public bool GlyphOfFreezingTrap { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the radius of the effect from your Ice Trap by 2 yards.</summary>
        [GlyphData(15, 42906, "Glyph of Ice Trap", GlyphType.Major, @"Increases the radius of the effect from your Ice Trap by 2 yards.")]
        public bool GlyphOfFrostTrap { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.</summary>
        [GlyphData(16, 42908, "Glyph of Immolation Trap", GlyphType.Major, @"Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.")]
        public bool GlyphOfImmolationTrap { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the duration of your Master's Call by 4 sec.</summary>
        [GlyphData(17, 45733, "Glyph of Master's Call", GlyphType.Major, @"Increases the duration of your Master's Call by 4 sec.")]
        public bool GlyphOfMastersCall { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the total amount of healing done by your Mend Pet ability by 60%.</summary>
        [GlyphData(18, 42900, "Glyph of Mending", GlyphType.Major, @"Increases the total amount of healing done by your Mend Pet ability by 60%.")]
        public bool GlyphOfMending { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>When you use Misdirection on your pet, the cooldown on your Misdirection is reset.</summary>
        [GlyphData(19, 42907, "Glyph of Misdirection", GlyphType.Major, @"When you use Misdirection on your pet, the cooldown on your Misdirection is reset.")]
        public bool GlyphOfMisdirection { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces damage taken by 20% for 5 sec after using Raptor Strike.</summary>
        [GlyphData(20, 45735, "Glyph of Raptor Strike", GlyphType.Major, @"Reduces damage taken by 20% for 5 sec after using Raptor Strike.")]
        public bool GlyphOfRaptorStrike { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the range of Scatter Shot by 3 yards.</summary>
        [GlyphData(21, 45734, "Glyph of Scatter Shot", GlyphType.Major, @"Increases the range of Scatter Shot by 3 yards.")]
        public bool GlyphOfScatterShot { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.</summary>
        [GlyphData(22, 42910, "Glyph of Silencing Shot", GlyphType.Major, @"When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.")]
        public bool GlyphOfSilencingShot { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases the attack power bonus of Aspect of the Beast for you and your pet by an additional 2%.</summary>
        //[GlyphData(23, "Glyph Of The Beast", GlyphType.Major, @"N/A")]
        //public bool GlyphOfTheBeast { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Snakes from your Snake Trap take 90% reduced damage from area of effect spells.</summary>
        [GlyphData(24, 42913, "Glyph of Snake Trap", GlyphType.Major, @"Snakes from your Snake Trap take 90% reduced damage from area of effect spells.")]
        public bool GlyphOfSnakeTrap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Reduces the focus cost of Trap Launcher by 10.</summary>
        [GlyphData(25, 42899, "Glyph of Trap Launcher", GlyphType.Major, @"Reduces the focus cost of Trap Launcher by 10.")]
        public bool GlyphOfTrueshotAura { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Decreases the cooldown of your Wyvern Sting by 6 sec.</summary>
        [GlyphData(26, 42917, "Glyph of Wyvern Sting", GlyphType.Major, @"Decreases the cooldown of your Wyvern Sting by 6 sec.")]
        public bool GlyphOfWyvernSting { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        #endregion
        #region Minor
        /// <summary>Increases the range of your Aspect of the Pack ability by 15 yards.</summary>
        [GlyphData(27, 43355, "Glyph of Aspect of the Pack", GlyphType.Minor, @"Increases the range of your Aspect of the Pack ability by 15 yards.")]
        public bool GlyphOfAspectofthePack { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Reduces the cooldown of your Feign Death spell by 5 sec.</summary>
        [GlyphData(28, 43351, "Glyph of Feign Death", GlyphType.Minor, @"Reduces the cooldown of your Feign Death spell by 5 sec.")]
        public bool GlyphOfFeignDeath { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Your Mend Pet spell increases your pet's happiness slightly.</summary>
        [GlyphData(29, 43350, "Glyph of Mend Pet", GlyphType.Minor, @"Your Mend Pet spell increases your pet's happiness slightly.")]
        public bool GlyphOfMendPet { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.</summary>
        [GlyphData(30, 43338, "Glyph of Revive Pet", GlyphType.Minor, @"Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.")]
        public bool GlyphOfRevivePet { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.</summary>
        [GlyphData(31, 43356, "Glyph of Scare Beast", GlyphType.Minor, @"Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.")]
        public bool GlyphOfScareBeast { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        #endregion
    }

}
