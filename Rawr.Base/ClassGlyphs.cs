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

    public partial class WarriorTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Reduces the cooldown on Bladestorm by 15 sec.</summary>
        [GlyphData(0, 45790, 63324, "Glyph of Bladestorm", GlyphType.Prime,
            @"Reduces the cooldown on Bladestorm by 15 sec.")]
        public bool GlyphOfBladestorm { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage of Bloodthirst by 10%.</summary>
        [GlyphData(1, 43416, 58367, "Glyph of Bloodthirst", GlyphType.Prime,
            @"Increases the damage of Bloodthirst by 10%.")]
        public bool GlyphOfBloodthirst { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of Devastate by 5%.</summary>
        [GlyphData(2, 43415, 58388, "Glyph of Devastate", GlyphType.Prime,
            @"Increases the critical strike chance of Devastate by 5%.")]
        public bool GlyphOfDevastate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the damage of Mortal Strike by 10%.</summary>
        [GlyphData(3, 43421, 58368, "Glyph of Mortal Strike", GlyphType.Prime,
            @"Increases the damage of Mortal Strike by 10%.")]
        public bool GlyphOfMortalStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage of Overpower by 10%.</summary>
        [GlyphData(4, 43422, 58386, "Glyph of Overpower", GlyphType.Prime,
            @"Increases the damage of Overpower by 10%.")]
        public bool GlyphOfOverpower { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the critical strike chance of Raging Blow by 5%.</summary>
        [GlyphData(5, 43432, 58370, "Glyph of Raging Blow", GlyphType.Prime,
            @"Increases the critical strike chance of Raging Blow by 5%.")]
        public bool GlyphOfRagingBlow { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the damage of Revenge by 10%.</summary>
        [GlyphData(6, 43424, 58364, "Glyph of Revenge", GlyphType.Prime,
            @"Increases the damage of Revenge by 10%.")]
        public bool GlyphOfRevenge { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage of Shield Slam by 10%.</summary>
        [GlyphData(7, 43425, 58375, "Glyph of Shield Slam", GlyphType.Prime,
            @"Increases the damage of Shield Slam by 10%.")]
        public bool GlyphOfShieldSlam { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the critical strike chance of Slam by 5%.</summary>
        [GlyphData(8, 43423, 58385, "Glyph of Slam", GlyphType.Prime,
            @"Increases the critical strike chance of Slam by 5%.")]
        public bool GlyphOfSlam { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 9; } }
        /// <summary>Increases the number of targets your Cleave hits by 1.</summary>
        [GlyphData( 9, 43414, 58366, "Glyph of Cleaving", GlyphType.Major,
            @"Increases the number of targets your Cleave hits by 1.")]
        public bool GlyphOfCleaving { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.</summary>
        [GlyphData(10, 63481, 89003, "Glyph of Colossus Smash", GlyphType.Major,
            @"Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.")]
        public bool GlyphOfColossusSmash { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Death Wish no longer increases damage taken.</summary>
        [GlyphData(11, 67483, 94374, "Glyph of Death Wish", GlyphType.Major,
            @"Death Wish no longer increases damage taken.")]
        public bool GlyphOfDeathWish { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Your Heroic Throw applies a stack of Sunder Armor.</summary>
        [GlyphData(12, 43418, 58357, "Glyph of Heroic Throw", GlyphType.Major,
            @"Your Heroic Throw applies a stack of Sunder Armor.")]
        public bool GlyphOfHeroicThrow { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the duration of your Intercept stun by 1 sec.</summary>
        [GlyphData(13, 67482, 94372, "Glyph of Intercept", GlyphType.Major,
            @"Increases the duration of your Intercept stun by 1 sec.")]
        public bool GlyphOfIntercept { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Increases the number of attacks you intercept for your Intervene target by 1.</summary>
        [GlyphData(14, 43419, 58377, "Glyph of Intervene", GlyphType.Major,
            @"Increases the number of attacks you intercept for your Intervene target by 1.")]
        public bool GlyphOfIntervene { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases the range of your Charge ability by 5 yards.</summary>
        [GlyphData(15, 43397, 58097, "Glyph of Long Charge", GlyphType.Major,
            @"Increases the range of your Charge ability by 5 yards.")]
        public bool GlyphOfLongCharge { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the radius of Piercing Howl by 50%.</summary>
        [GlyphData(16, 43417, 58372, "Glyph of Piercing Howl", GlyphType.Major,
            @"Increases the radius of Piercing Howl by 50%.")]
        public bool GlyphOfPiercingHowl { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Reduces the cooldown of your Charge ability by 1 sec.</summary>
        [GlyphData(17, 43413, 58355, "Glyph of Rapid Charge", GlyphType.Major,
            @"Reduces the cooldown of your Charge ability by 1 sec.")]
        public bool GlyphOfRapidCharge { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the rage cost of your Thunder Clap ability by 5.</summary>
        [GlyphData(18, 43430, 58356, "Glyph of Resonating Power", GlyphType.Major,
            @"Reduces the rage cost of your Thunder Clap ability by 5.")]
        public bool GlyphOfResonatingPower { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.</summary>
        [GlyphData(19, 45797, 63329, "Glyph of Shield Wall", GlyphType.Major,
            @"Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.")]
        public bool GlyphOfShieldWall { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the cooldown on Shockwave by 3 sec.</summary>
        [GlyphData(20, 45792, 63325, "Glyph of Shockwave", GlyphType.Major,
            @"Reduces the cooldown on Shockwave by 3 sec.")]
        public bool GlyphOfShockwave { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the cooldown on Spell Reflection by 1 sec.</summary>
        [GlyphData(21, 45795, 63328, "Glyph of Spell Reflection", GlyphType.Major,
            @"Reduces the cooldown on Spell Reflection by 1 sec.")]
        public bool GlyphOfSpellReflection { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>When you apply or refresh Sunder Armor, a second nearby target also receives athe Sunder Armor effect.</summary>
        [GlyphData(22, 43427, 58387, "Glyph of Sunder Armor", GlyphType.Major,
            @"When you apply or refresh Sunder Armor, a second nearby target also receives athe Sunder Armor effect.")]
        public bool GlyphOfSunderArmor { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Reduces the rage cost of Sweeping Strikes ability by 100%.</summary>
        [GlyphData(23, 43428, 58384, "Glyph of Sweeping Strikes", GlyphType.Major,
            @"Reduces the rage cost of Sweeping Strikes ability by 100%.")]
        public bool GlyphOfSweepingStrikes { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Increases the radius of your Thunder Clap ability by 2 yards.</summary>
        [GlyphData(24, 43399, 58098, "Glyph of Thunder Clap", GlyphType.Major,
            @"Increases the radius of your Thunder Clap ability by 2 yards.")]
        public bool GlyphOfThunderClap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the total healing provided by your Victory Rush by 50%.</summary>
        [GlyphData(25, 43431, 58382, "Glyph of Victory Rush", GlyphType.Major,
            @"Increases the total healing provided by your Victory Rush by 50%.")]
        public bool GlyphOfVictoryRush { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 26; } }
        /// <summary>Increases the duration by 2 min and area of effect by 50% of your Battle Shout.</summary>
        [GlyphData(26, 43395, 58095, "Glyph of Battle", GlyphType.Minor,
            @"Increases the duration by 2 min and area of effect by 50% of your Battle Shout.")]
        public bool GlyphOfBattle { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Berserker Rage generates 5 rage when used.</summary>
        [GlyphData(27, 43396, 58096, "Glyph of Berserker Rage", GlyphType.Minor,
            @"Berserker Rage generates 5 rage when used.")]
        public bool GlyphOfBerserkerRage { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Increases the healing your receive from your Bloodthirst ability by 40%.</summary>
        [GlyphData(28, 43412, 58369, "Glyph of Bloody Healing", GlyphType.Minor,
            @"Increases the healing your receive from your Bloodthirst ability by 40%.")]
        public bool GlyphOfBloodyHealing { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.</summary>
        [GlyphData(29, 49084, 68164, "Glyph of Command", GlyphType.Minor,
            @"Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.")]
        public bool GlyphOfCommand { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.</summary>
        [GlyphData(30, 43398, 58099, "Glyph of Demoralizing Shout", GlyphType.Minor,
            @"Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.")]
        public bool GlyphOfDemoralizingShout { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Increases the window of opportunity in which you can use Victory Rush by 5 sec.</summary>
        [GlyphData(31, 43400, 58104, "Glyph of Enduring Victory", GlyphType.Minor,
            @"Increases the window of opportunity in which you can use Victory Rush by 5 sec.")]
        public bool GlyphOfEnduringVictory { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cost of Sunder Armor by 50%.</summary>
        [GlyphData(32, 45793, 63326, "Glyph of Furious Sundering", GlyphType.Minor,
            @"Reduces the cost of Sunder Armor by 50%.")]
        public bool GlyphOfFuriousSundering { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.</summary>
        [GlyphData(33, 45794, 63327, "Glyph of Intimidating Shout", GlyphType.Minor,
            @"Targets of your Intimidating Shout now tremble in place instead of fleeing in fear.")]
        public bool GlyphOfIntimidatingShout { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
    }

    public partial class MageTalents
    {
        private bool[] _glyphData = new bool[33];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the damage of your Arcane Barrage spell by 4%.</summary>
        [GlyphData( 0, 45738, 63092, "Glyph of Arcane Barrage", GlyphType.Prime,
            @"Increases the damage of your Arcane Barrage spell by 4%.")]
        public bool GlyphOfArcaneBarrage { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage from your Arcane Blast buff by 3%.</summary>
        [GlyphData( 1, 44955, 62210, "Glyph of Arcane Blast", GlyphType.Prime,
            @"Increases the damage from your Arcane Blast buff by 3%.")]
        public bool GlyphOfArcaneBlast { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of your Arcane Missiles spell by 5%.</summary>
        [GlyphData( 2, 42735, 56363, "Glyph of Arcane Missiles", GlyphType.Prime,
            @"Increases the critical strike chance of your Arcane Missiles spell by 5%.")]
        public bool GlyphOfArcaneMissiles { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the damage of your Cone of Cold spell by 25%.</summary>
        [GlyphData( 3, 42753, 56364, "Glyph of Cone of Cold", GlyphType.Prime,
            @"Increases the damage of your Cone of Cold spell by 25%.")]
        public bool GlyphOfConeOfCold { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Your Deep Freeze deals 20% additional damage.</summary>
        [GlyphData( 4, 45736, 63090, "Glyph of Deep Freeze", GlyphType.Prime,
            @"Your Deep Freeze deals 20% additional damage.")]
        public bool GlyphOfDeepFreeze { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the critical strike chance of your Fireball spell by 5%.</summary>
        [GlyphData( 5, 42739, 56368, "Glyph of Fireball", GlyphType.Prime,
            @"Increases the critical strike chance of your Fireball spell by 5%.")]
        public bool GlyphOfFireball { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the critical strike chance of your Frostbolt spell by 5%.</summary>
        [GlyphData( 6, 42742, 56370, "Glyph of Frostbolt", GlyphType.Prime,
            @"Increases the critical strike chance of your Frostbolt spell by 5%.")]
        public bool GlyphOfFrostbolt { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage done by your Frostfire Bolt by 15% and your Frostfire Bolt now deals 3% additional damage over 12 sec, stacking up to 3 times, but no longer reduces the victim's movement speed.</summary>
        [GlyphData( 7, 44684, 61205, "Glyph of Frostfire", GlyphType.Prime,
            @"Increases the damage done by your Frostfire Bolt by 15% and your Frostfire Bolt now deals 3% additional damage over 12 sec, stacking up to 3 times, but no longer reduces the victim's movement speed.")]
        public bool GlyphOfFrostfire { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage of your Ice Lance spell by 5%.</summary>
        [GlyphData( 8, 42745, 56377, "Glyph of Ice Lance", GlyphType.Prime,
            @"Increases the damage of your Ice Lance spell by 5%.")]
        public bool GlyphOfIceLance { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage of your Living Bomb spell by 3%.</summary>
        [GlyphData( 9, 63539, 89926, "Glyph of Living Bomb", GlyphType.Prime,
            @"Increases the damage of your Living Bomb spell by 3%.")]
        public bool GlyphOfLivingBomb { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Your Mage Armor regenerates 20% more mana.</summary>
        [GlyphData(10, 42749, 56383, "Glyph of Mage Armor", GlyphType.Prime,
            @"Your Mage Armor regenerates 20% more mana.")]
        public bool GlyphOfMageArmor { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Molten Armor grants an additional 2% spell critical strike chance.</summary>
        [GlyphData(11, 42751, 56382, "Glyph of Molten Armor", GlyphType.Prime,
            @"Your Molten Armor grants an additional 2% spell critical strike chance.")]
        public bool GlyphOfMoltenArmor { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the critical strike chance of your Pyroblast spell by 5%.</summary>
        [GlyphData(12, 42743, 56384, "Glyph of Pyroblast", GlyphType.Prime,
            @"Increases the critical strike chance of your Pyroblast spell by 5%.")]
        public bool GlyphOfPyroblast { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 13; } }
        /// <summary>While Arcane Power is active the global cooldown of your Blink, Mana Shield and Mirror Image is reduced to zero.</summary>
        [GlyphData(13, 42736, 56381, "Glyph of Arcane Power", GlyphType.Major,
            @"While Arcane Power is active the global cooldown of your Blink, Mana Shield and Mirror Image is reduced to zero.")]
        public bool GlyphOfArcanePower { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Increases the duration of Blast Wave's slowing effect by 1 sec.</summary>
        [GlyphData(14, 44920, 62126, "Glyph of Blast Wave", GlyphType.Major,
            @"Increases the duration of Blast Wave's slowing effect by 1 sec.")]
        public bool GlyphOfBlastWave { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases the distance you travel with the Blink spell by 5 yards.</summary>
        [GlyphData(15, 42737, 56365, "Glyph of Blink", GlyphType.Major,
            @"Increases the distance you travel with the Blink spell by 5 yards.")]
        public bool GlyphOfBlink { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Reduces the cooldown of your Dragon's Breath by 3 sec.</summary>
        [GlyphData(16, 42754, 56373, "Glyph of Dragon's Breath", GlyphType.Major,
            @"Reduces the cooldown of your Dragon's Breath by 3 sec.")]
        public bool GlyphOfDragonsBreath { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Your Evocation ability also causes you to regain 40% of your health over its duration.</summary>
        [GlyphData(17, 42738, 56380, "Glyph of Evocation", GlyphType.Major,
            @"Your Evocation ability also causes you to regain 40% of your health over its duration.")]
        public bool GlyphOfEvocation { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Your Frost Armor also causes you to regenerate 2% of your maximum mana every 5 sec.</summary>
        [GlyphData(18, 69773, 98398, "Glyph of Frost Armor", GlyphType.Major,
            @"Your Frost Armor also causes you to regenerate 2% of your maximum mana every 5 sec.")]
        public bool GlyphOfFrostArmor { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Your Frost Nova targets can take an additional 20% damage before the Frost Nova effect automatically breaks.</summary>
        [GlyphData(19, 42741, 56376, "Glyph of Frost Nova", GlyphType.Major,
            @"Your Frost Nova targets can take an additional 20% damage before the Frost Nova effect automatically breaks.")]
        public bool GlyphOfFrostNova { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Increases the amount of damage absorbed by your Ice Barrier by 30%.</summary>
        [GlyphData(20, 45740, 63095, "Glyph of Ice Barrier", GlyphType.Major,
            @"Increases the amount of damage absorbed by your Ice Barrier by 30%.")]
        public bool GlyphOfIceBarrier { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Frost Nova cooldown is now reset every time you use Ice Block.</summary>
        [GlyphData(21, 42744, 56372, "Glyph of Ice Block", GlyphType.Major,
            @"Your Frost Nova cooldown is now reset every time you use Ice Block.")]
        public bool GlyphOfIceBlock { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Icy Veins ability also removes all movement slowing and cast time slowing effects.</summary>
        [GlyphData(22, 42746, 56374, "Glyph of Icy Veins", GlyphType.Major,
            @"Your Icy Veins ability also removes all movement slowing and cast time slowing effects.")]
        public bool GlyphOfIcyVeins { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases your movement speed while Invisible by 40%.</summary>
        [GlyphData(23, 42748, 56366, "Glyph of Invisibility", GlyphType.Major,
            @"Increases your movement speed while Invisible by 40%.")]
        public bool GlyphOfInvisibility { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the cooldown of your Mana Shield by 2 sec.</summary>
        [GlyphData(24, 50045, 70937, "Glyph of Mana Shield", GlyphType.Major,
            @"Reduces the cooldown of your Mana Shield by 2 sec.")]
        public bool GlyphOfManaShield { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Your Polymorph spell also removes all damage over time effects from the target.</summary>
        [GlyphData(25, 42752, 56375, "Glyph of Polymorph", GlyphType.Major,
            @"Your Polymorph spell also removes all damage over time effects from the target.")]
        public bool GlyphOfPolymorph { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Increases the range of your Slow spell by 5 yards.</summary>
        [GlyphData(26, 45737, 63091, "Glyph of Slow", GlyphType.Major,
            @"Increases the range of your Slow spell by 5 yards.")]
        public bool GlyphOfSlow { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 27; } }
        /// <summary>Increases the duration of your Armor spells by 30 min.</summary>
        [GlyphData(27, 63416, 89749, "Glyph of Armors", GlyphType.Minor,
            @"Increases the duration of your Armor spells by 30 min.")]
        public bool GlyphOfArmors { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Reduces the mana cost of your Conjuring spells by 50%.</summary>
        [GlyphData(28, 43359, 57928, "Glyph of Conjuring", GlyphType.Minor,
            @"Reduces the mana cost of your Conjuring spells by 50%.")]
        public bool GlyphOfConjuring { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Your Mirror Images cast Arcane Blast or Fireball instead of Frostbolt depending on your primary talent tree.</summary>
        [GlyphData(29, 45739, 63093, "Glyph of Mirror Image", GlyphType.Minor,
            @"Your Mirror Images cast Arcane Blast or Fireball instead of Frostbolt depending on your primary talent tree.")]
        public bool GlyphOfMirrorImage { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Your Slow Fall spell no longer requires a reagent.</summary>
        [GlyphData(30, 43364, 57925, "Glyph of Slow Fall", GlyphType.Minor,
            @"Your Slow Fall spell no longer requires a reagent.")]
        public bool GlyphOfSlowFall { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a monkey instead.</summary>
        [GlyphData(31, 43360, 57927, "Glyph of the Monkey", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a monkey instead.")]
        public bool GlyphOfTheMonkey { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Your Polymorph: Sheep spell polymorphs the target into a penguin instead.</summary>
        [GlyphData(32, 43361, 52648, "Glyph of the Penquin", GlyphType.Minor,
            @"Your Polymorph: Sheep spell polymorphs the target into a penguin instead.")]
        public bool GlyphOfThePenquin { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        #endregion
    }

    public partial class DruidTalents
    {
        private bool[] _glyphData = new bool[41];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of Berserk by 5 sec.</summary>
        [GlyphData( 0, 45601, 62969, "Glyph of Berserk", GlyphType.Prime,
            @"Increases the duration of Berserk by 5 sec.")]
        public bool GlyphOfBerserk { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage of your Insect Swarm ability by 30%.</summary>
        [GlyphData( 1, 40919, 54830, "Glyph of Insect Swarm", GlyphType.Prime,
            @"Increases the damage of your Insect Swarm ability by 30%.")]
        public bool GlyphOfInsectSwarm { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of your Lacerate ability by 5%.</summary>
        [GlyphData( 2, 67484, 94382, "Glyph of Lacerate", GlyphType.Prime,
            @"Increases the critical strike chance of your Lacerate ability by 5%.")]
        public bool GlyphOfLacerate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the critical effect chance of your Lifebloom 10%.</summary>
        [GlyphData( 3, 40915, 54826, "Glyph of Lifebloom", GlyphType.Prime,
            @"Increases the critical effect chance of your Lifebloom 10%.")]
        public bool GlyphOfLifebloom { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage done by Mangle by 10%.</summary>
        [GlyphData( 4, 40900, 54813, "Glyph of Mangle", GlyphType.Prime,
            @"Increases the damage done by Mangle by 10%.")]
        public bool GlyphOfMangle { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases the periodic damage of your Moonfire ability by 20%.</summary>
        [GlyphData( 5, 40923, 54829, "Glyph of Moonfire", GlyphType.Prime,
            @"Increases the periodic damage of your Moonfire ability by 20%.")]
        public bool GlyphOfMoonfire { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Regrowth heal-over-time will automatically refresh its duration on targets at or below 50% health.</summary>
        [GlyphData( 6, 40912, 54743, "Glyph of Regrowth", GlyphType.Prime,
            @"Your Regrowth heal-over-time will automatically refresh its duration on targets at or below 50% health.")]
        public bool GlyphOfRegrowth { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the healing done by your Rejuvenation by 10%.</summary>
        [GlyphData( 7, 40913, 54754, "Glyph of Rejuvenation", GlyphType.Prime,
            @"Increases the healing done by your Rejuvenation by 10%.")]
        public bool GlyphOfRejuvination { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the periodic damage of your Rip by 15%.</summary>
        [GlyphData( 8, 40902, 54818, "Glyph of Rip", GlyphType.Prime,
            @"Increases the periodic damage of your Rip by 15%.")]
        public bool GlyphOfRip { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Savage Roar ability grants an additional 5% bonus damage done.</summary>
        [GlyphData( 9, 45604, 63055, "Glyph of Savage Roar", GlyphType.Prime,
            @"Your Savage Roar ability grants an additional 5% bonus damage done.")]
        public bool GlyphOfSavageRoar { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Each time you Shred, the duration of your Rip on the target is extended 2 sec, up to a maximum of 6 sec.</summary>
        [GlyphData(10, 40901, 54815, "Glyph of Shred", GlyphType.Prime,
            @"Each time you Shred, the duration of your Rip on the target is extended 2 sec, up to a maximum of 6 sec.")]
        public bool GlyphOfShred { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Starfire ability increases the duration of yoru Moonfire effect on the target by 3 sec, up to a maximum of 9 additional seconds.</summary>
        [GlyphData(11, 40916, 54845, "Glyph of Starfire", GlyphType.Prime,
            @"Your Starfire ability increases the duration of yoru Moonfire effect on the target by 3 sec, up to a maximum of 9 additional seconds.")]
        public bool GlyphOfStarfire { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>When your Starsurge deals damage, the cooldown remaining on your Starfall is reduced by 5 sec.</summary>
        [GlyphData(12, 45603, 62917, "Glyph of Starsurge", GlyphType.Prime,
            @"When your Starsurge deals damage, the cooldown remaining on your Starfall is reduced by 5 sec.")]
        public bool GlyphOfStarsurge { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Your Swiftmend ability no longer consumes a Rejuvenation or Regrowth effect from the target.</summary>
        [GlyphData(13, 40906, 54824, "Glyph of Swiftmend", GlyphType.Prime,
            @"Your Swiftmend ability no longer consumes a Rejuvenation or Regrowth effect from the target.")]
        public bool GlyphOfSwiftmend { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Reduces the cooldown of your Tiger's Fury ability by 3 sec.</summary>
        [GlyphData(14, 67487, 94390, "Glyph of Tiger's Fury", GlyphType.Prime,
            @"Reduces the cooldown of your Tiger's Fury ability by 3 sec.")]
        public bool GlyphOfTigersFury { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases the damage done by your Wrath by 10%.</summary>
        [GlyphData(15, 40922, 54756, "Glyph of Wrath", GlyphType.Prime,
            @"Increases the damage done by your Wrath by 10%.")]
        public bool GlyphOfWrath { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 16; } }
        /// <summary>Reduces the chance you'll be critically hit by melee attacks by 25% while Barkskin is active.</summary>
        [GlyphData(16, 45623, 63057, "Glyph of Barkskin", GlyphType.Major,
            @"Reduces the chance you'll be critically hit by melee attacks by 25% while Barkskin is active.")]
        public bool GlyphOfBarkskin { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Reduces the cast time of your Entangling Roots by 0.2 sec.</summary>
        [GlyphData(17, 40924, 54760, "Glyph of Entangling Roots", GlyphType.Major,
            @"Reduces the cast time of your Entangling Roots by 0.2 sec.")]
        public bool GlyphOfEntanglingRoots { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the range of your Faerie Fire and Feral Faerie Fire abilities by 10 yds.</summary>
        [GlyphData(18, 67485, 94386, "Glyph of Faerie Fire", GlyphType.Major,
            @"Increases the range of your Faerie Fire and Feral Faerie Fire abilities by 10 yds.")]
        public bool GlyphOfFaerieFire { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the cooldown of yoru Feral Charge (Cat) ability by 2 sec and the cooldown of your Feral Charge (Bear) ability by 1 sec.</summary>
        [GlyphData(19, 67486, 94388, "Glyph of Feral Charge", GlyphType.Major,
            @"Reduces the cooldown of yoru Feral Charge (Cat) ability by 2 sec and the cooldown of your Feral Charge (Bear) ability by 1 sec.")]
        public bool GlyphOfFeralCharge { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Your Ferocious Bite ability no longer converts extra energy into additional damage.</summary>
        [GlyphData(20, 48720, 67598, "Glyph of Ferocious Bite", GlyphType.Major,
            @"Your Ferocious Bite ability no longer converts extra energy into additional damage.")]
        public bool GlyphOfFerociousBite { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the damage done by Starfall by 10%, but decreases its radius by 50%.</summary>
        [GlyphData(21, 44928, 62080, "Glyph of Focus", GlyphType.Major,
            @"Increases the damage done by Starfall by 10%, but decreases its radius by 50%.")]
        public bool GlyphOfFocus { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>While Frenzied Regeneration is active, healing effects on you are 30% more powerful but causes your Frenzied Regeneration to longer convert rage into health.</summary>
        [GlyphData(22, 40896, 54810, "Glyph of Frenzied Regeneration", GlyphType.Major,
            @"While Frenzied Regeneration is active, healing effects on you are 30% more powerful but causes your Frenzied Regeneration to longer convert rage into health.")]
        public bool GlyphOfFrenziedRegeneration { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>When you Healing Touch, the cooldown on your Nature's Swiftness is reduced by 10 sec.</summary>
        [GlyphData(23, 40914, 54825, "Glyph of Healing Touch", GlyphType.Major,
            @"When you Healing Touch, the cooldown on your Nature's Swiftness is reduced by 10 sec.")]
        public bool GlyphOfHealingTouch { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Hurricane ability now also slows the movement speed of its victims by 50%.</summary>
        [GlyphData(24, 40920, 54831, "Glyph of Hurricane", GlyphType.Major,
            @"Your Hurricane ability now also slows the movement speed of its victims by 50%.")]
        public bool GlyphOfHurricane { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>When Innervate is cast on a friendly target other than the caster, the caster will gain 50% of Innervate's effect.</summary>
        [GlyphData(25, 40908, 54832, "Glyph of Innervate", GlyphType.Major,
            @"When Innervate is cast on a friendly target other than the caster, the caster will gain 50% of Innervate's effect.")]
        public bool GlyphOfInnervate { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Your Maul ability now hits 1 additional target for 50% damage.</summary>
        [GlyphData(26, 40897, 54811, "Glyph of Maul", GlyphType.Major,
            @"Your Maul ability now hits 1 additional target for 50% damage.")]
        public bool GlyphOfMaul { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Reduces the cooldown of your Typhoon spell by 3 sec.</summary>
        [GlyphData(27, 45622, 63056, "Glyph of Monsoon", GlyphType.Major,
            @"Reduces the cooldown of your Typhoon spell by 3 sec.")]
        public bool GlyphOfMonsoon { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Increases the range of your Pounce by 3 yards.</summary>
        [GlyphData(28, 40903, 54821, "Glyph of Pounce", GlyphType.Major,
            @"Increases the range of your Pounce by 3 yards.")]
        public bool GlyphOfPounce { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Players resurrected by Rebirth are returned to life with 100% health.</summary>
        [GlyphData(29, 40909, 54733, "Glyph of Rebirth", GlyphType.Major,
            @"Players resurrected by Rebirth are returned to life with 100% health.")]
        public bool GlyphOfRebirth { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the duration of your Solar Beam silence effect by 5 sec.</summary>
        [GlyphData(30, 40899, 54812, "Glyph of Solar Beam", GlyphType.Major,
            @"Increases the duration of your Solar Beam silence effect by 5 sec.")]
        public bool GlyphOfSolarBeam { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Reduces the cooldown of Starfall by 30 sec.</summary>
        [GlyphData(31, 40921, 54828, "Glyph of Starfall", GlyphType.Major,
            @"Reduces the cooldown of Starfall by 30 sec.")]
        public bool GlyphOfStarfall { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cooldown of your Thorns spell by 20 sec.</summary>
        [GlyphData(32, 43332, 57862, "Glyph of Thorns", GlyphType.Major,
            @"Reduces the cooldown of your Thorns spell by 20 sec.")]
        public bool GlyphOfThorns { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Wild Growth can affect 1 additional target.</summary>
        [GlyphData(33, 45602, 62970, "Glyph of Wild Growth", GlyphType.Major,
            @"Wild Growth can affect 1 additional target.")]
        public bool GlyphOfWildGrowth { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 34; } }
        /// <summary>Increases your swim speed by 50% while in Aquatic Form.</summary>
        [GlyphData(34, 43316, 57856, "Glyph of Aquatic Form", GlyphType.Minor,
            @"Increases your swim speed by 50% while in Aquatic Form.")]
        public bool GlyphOfAquaticForm { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        /// <summary>Reduces the cooldown of your Challenging Roar by 30 sec.</summary>
        [GlyphData(35, 43334, 57858, "Glyph of Challenging Roar", GlyphType.Minor,
            @"Reduces the cooldown of your Challenging Roar by 30 sec.")]
        public bool GlyphOfChallengingRoar { get { return _glyphData[35]; } set { _glyphData[35] = value; } }
        /// <summary>Reduces the cooldown of your Dash ability by 20%.</summary>
        [GlyphData(36, 43674, 59219, "Glyph of Dash", GlyphType.Minor,
            @"Reduces the cooldown of your Dash ability by 20%.")]
        public bool GlyphOfDash { get { return _glyphData[36]; } set { _glyphData[36] = value; } }
        /// <summary>Mana cost of your Mark of the Wild reduced by 50%.</summary>
        [GlyphData(37, 43335, 57855, "Glyph of Mark of the Wild", GlyphType.Minor,
            @"Mana cost of your Mark of the Wild reduced by 50%.")]
        public bool GlyphOfTheWild { get { return _glyphData[37]; } set { _glyphData[37] = value; } }
        /// <summary>Your Tree of Life Form now resembles a Treant.</summary>
        [GlyphData(38, 68039, 95212, "Glyph of the Treant", GlyphType.Minor,
            @"Your Tree of Life Form now resembles a Treant.")]
        public bool GlyphOfTheTreant { get { return _glyphData[38]; } set { _glyphData[38] = value; } }
        /// <summary>Reduces the cost of your Typhoon spell by 8% and increases its radius by 10 yards, but it no longer knocks enemies back.</summary>
        [GlyphData(39, 44922, 62135, "Glyph of Typhoon", GlyphType.Minor,
            @"Reduces the cost of your Typhoon spell by 8% and increases its radius by 10 yards, but it no longer knocks enemies back.")]
        public bool GlyphOfTyphoon { get { return _glyphData[39]; } set { _glyphData[39] = value; } }
        /// <summary>Your Rebirth spell no longer requires a reagent.</summary>
        [GlyphData(40, 43331, 57857, "Glyph of Unburdened Rebirth", GlyphType.Minor,
            @"Your Rebirth spell no longer requires a reagent.")]
        public bool GlyphOfUnburdenedRebirth { get { return _glyphData[40]; } set { _glyphData[40] = value; } }
        #endregion
    }

    public partial class PaladinTalents
    {
        private bool[] _glyphData = new bool[33];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the critical strike chance of Crusader Strike by 5%.</summary>
        [GlyphData( 0, 41098, 54927, "Glyph of Crusader Strike", GlyphType.Prime,
            @"Increases the critical strike chance of Crusader Strike by 5%.")]
        public bool GlyphOfCrusaderStrike { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the duration of Divine Favor by 10 sec.</summary>
        [GlyphData( 1, 41106, 54937, "Glyph of Divine Favor", GlyphType.Prime,
            @"Increases the duration of Divine Favor by 10 sec.")]
        public bool GlyphOfDivineFavor { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Your Exorcism causes an additional 20% of its damage over 6 sec.</summary>
        [GlyphData( 2, 41103, 54934, "Glyph of Exorcism", GlyphType.Prime,
            @"Your Exorcism causes an additional 20% of its damage over 6 sec.")]
        public bool GlyphOfExorcism { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the damage of both the physical and Holy components of Hammer of the Righteous by 10%.</summary>
        [GlyphData( 3, 45742, 63219, "Glyph of Hammer of the Righteous", GlyphType.Prime,
            @"Increases the damage of both the physical and Holy components of Hammer of the Righteous by 10%.")]
        public bool GlyphOfHammerOfTheRighteous { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the critical effect chance of Holy Shock by 5%.</summary>
        [GlyphData( 4, 45746, 63224, "Glyph of Holy Shock", GlyphType.Prime,
            @"Increases the critical effect chance of Holy Shock by 5%.")]
        public bool GlyphOfHolyShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Your Judgement deals 10% more damage.</summary>
        [GlyphData( 5, 41092, 54922, "Glyph of Judgement", GlyphType.Prime,
            @"Your Judgement deals 10% more damage.")]
        public bool GlyphOfJudgement { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>While Seal of Insight is active, the effect of your healing spells is increased by 5%.</summary>
        [GlyphData( 6, 41110, 54943, "Glyph of Seal of Insight", GlyphType.Prime,
            @"While Seal of Insight is active, the effect of your healing spells is increased by 5%.")]
        public bool GlyphOfSealOfInsight { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Your Seal of Truth also grants 10 expertise while active.</summary>
        [GlyphData( 7, 43869, 56416, "Glyph of Seal of Truth", GlyphType.Prime,
            @"Your Seal of Truth also grants 10 expertise while active.")]
        public bool GlyphOfSealOfTruth { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage of Shield of the Righteous by 10%.</summary>
        [GlyphData( 8, 45744, 63222, "Glyph of Shield of the Righteous", GlyphType.Prime,
            @"Increases the damage of Shield of the Righteous by 10%.")]
        public bool GlyphOfShieldOfTheRighteous { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage of Templar's Verdict by 15%.</summary>
        [GlyphData( 9, 45743, 63220, "Glyph of Templar's Verdict", GlyphType.Prime,
            @"Increases the damage of Templar's Verdict by 15%.")]
        public bool GlyphOfTemplarsVerdict { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the healing done by Word of Glory by 10%.</summary>
        [GlyphData(10, 41105, 54936, "Glyph of Word of Glory", GlyphType.Prime,
            @"Increases the healing done by Word of Glory by 10%.")]
        public bool GlyphOfWordOfGlory { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 11; } }
        /// <summary>Your Beacon of Light costs no mana.</summary>
        [GlyphData(11, 45741, 63218, "Glyph of Beacon of Light", GlyphType.Major,
            @"Your Beacon of Light costs no mana.")]
        public bool GlyphOfBeaconOfLight { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Reduces the mana cost of your Cleanse by 20%.</summary>
        [GlyphData(12, 41104, 54935, "Glyph of Cleansing", GlyphType.Major,
            @"Reduces the mana cost of your Cleanse by 20%.")]
        public bool GlyphOfCleansing { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the duration and cooldown of Consecration by 20%.</summary>
        [GlyphData(13, 41099, 54928, "Glyph of Consecration", GlyphType.Major,
            @"Increases the duration and cooldown of Consecration by 20%.")]
        public bool GlyphOfConsecration { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Avenger's Shield now also dazes targets.</summary>
        [GlyphData(14, 43868, 56414, "Glyph of Dazing Shield", GlyphType.Major,
            @"Your Avenger's Shield now also dazes targets.")]
        public bool GlyphOfDazingShield { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Divine Plea provides an additional 6% of your total mana.</summary>
        [GlyphData(15, 45745, 63223, "Glyph of Divine Plea", GlyphType.Major,
            @"Your Divine Plea provides an additional 6% of your total mana.")]
        public bool GlyphOfDivinePlea { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Removes the physical damage reduction of your Divine Protection, but increases the magical damage reduction by 20%.</summary>
        [GlyphData(16, 41096, 54924, "Glyph of Divine Protection", GlyphType.Major,
            @"Removes the physical damage reduction of your Divine Protection, but increases the magical damage reduction by 20%.")]
        public bool GlyphOfDivineProtection { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>When you use Lay on Hands, you also gain 10% of your maximum mana.</summary>
        [GlyphData(17, 41108, 54939, "Glyph of Divinity", GlyphType.Major,
            @"When you use Lay on Hands, you also gain 10% of your maximum mana.")]
        public bool GlyphOfDivinity { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Your Avenger's Shield hits 2 fewer targets, but for 30% more damage.</summary>
        [GlyphData(18, 41101, 54930, "Glyph of Focused Shield", GlyphType.Major,
            @"Your Avenger's Shield hits 2 fewer targets, but for 30% more damage.")]
        public bool GlyphOfFocusedShield { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Increases your Hammer of Justice range by 5 yards.</summary>
        [GlyphData(19, 41095, 54923, "Glyph of Hammer of Justice", GlyphType.Major,
            @"Increases your Hammer of Justice range by 5 yards.")]
        public bool GlyphOfHammerOfJustice { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the mana cost of Hammer of Wrath by 100%.</summary>
        [GlyphData(20, 41097, 54926, "Glyph of Hammer of Wrath", GlyphType.Major,
            @"Reduces the mana cost of Hammer of Wrath by 100%.")]
        public bool GlyphOfHammerOfWrath { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Holy Wrath now also stuns Elementals and Dragonkin.</summary>
        [GlyphData(21, 43867, 56420, "Glyph of Holy Wrath", GlyphType.Major,
            @"Your Holy Wrath now also stuns Elementals and Dragonkin.")]
        public bool GlyphOfHolyWrath { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Reduces the cooldown of your Lay on Hands spell by 3 min.</summary>
        [GlyphData(22, 43367, 57955, "Glyph of Lay on Hands", GlyphType.Major,
            @"Reduces the cooldown of your Lay on Hands spell by 3 min.")]
        public bool GlyphOfLayOnHands { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Light of Dawn can affect 1 additional target.</summary>
        [GlyphData(23, 41109, 54940, "Glyph of Light of Dawn", GlyphType.Major,
            @"Light of Dawn can affect 1 additional target.")]
        public bool GlyphOfLightOfDawn { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the mana cost of Rebuke by 100%.</summary>
        [GlyphData(24, 41094, 54925, "Glyph of Rebuke", GlyphType.Major,
            @"Reduces the mana cost of Rebuke by 100%.")]
        public bool GlyphOfRebuke { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Hand of Salvation no longer permanently reduces threat over time but instead reduces all threat as long as Hand of Salvation lasts.</summary>
        [GlyphData(23, 45747, 63225, "Glyph of Salvation", GlyphType.Major,
            @"Hand of Salvation no longer permanently reduces threat over time but instead reduces all threat as long as Hand of Salvation lasts.")]
        public bool GlyphOfSalvation { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the mana cost of your Crusader Strike by 30%.</summary>
        [GlyphData(24, 41107, 54938, "Glyph of the Ascetic Crusader", GlyphType.Major,
            @"Reduces the mana cost of your Crusader Strike by 30%.")]
        public bool GlyphOfAsceticCrusader { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Your Word of Glory heals for 50% less up front, but provides an additional 50% healing over 6 sec.</summary>
        [GlyphData(25, 66918, 63466, "Glyph of the Long Word", GlyphType.Major,
            @"Your Word of Glory heals for 50% less up front, but provides an additional 50% healing over 6 sec.")]
        public bool GlyphOfTheLongWord { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Reduces the casting time of your Turn Evil spell by 100%, but increases the cooldown by 8 sec.</summary>
        [GlyphData(26, 41102, 54931, "Glyph of Turn Evil", GlyphType.Major,
            @"Reduces the casting time of your Turn Evil spell by 100%, but increases the cooldown by 8 sec.")]
        public bool GlyphOfTurnEvil { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 27; } }
        /// <summary>Reduces the mana cost of Blessing of Kings by 50%.</summary>
        [GlyphData(27, 43365, 57937, "Glyph of Blessing of Kings", GlyphType.Minor,
            @"Reduces the mana cost of Blessing of Kings by 50%.")]
        public bool GlyphOfBlessingOfKings { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Reduces the mana cost of your Blessing of Might by 50%.</summary>
        [GlyphData(28, 43340, 57958, "Glyph of Blessing of Might", GlyphType.Minor,
            @"Reduces the mana cost of your Blessing of Might by 50%.")]
        public bool GlyphOfBlessingOfMight { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the mana cost of Seal of Insight by 50%.</summary>
        [GlyphData(29, 43366, 57979, "Glyph of Insight", GlyphType.Minor,
            @"Reduces the mana cost of Seal of Insight by 50%.")]
        public bool GlyphOfInsight { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the mana cost of Seal of Justice by 50%.</summary>
        [GlyphData(30, 43369, 57954, "Glyph of Justice", GlyphType.Minor,
            @"Reduces the mana cost of Seal of Justice by 50%.")]
        public bool GlyphOfJustice { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Reduces the mana cost of Seal of Righteousness by 50%.</summary>
        [GlyphData(31, 41100, 89401, "Glyph of Righteousness", GlyphType.Minor,
            @"Reduces the mana cost of Seal of Righteousness by 50%.")]
        public bool GlyphOfRighteousness { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the mana cost of Seal of Truth by 50%.</summary>
        [GlyphData(32, 43368, 57947, "Glyph of Truth", GlyphType.Minor,
            @"Reduces the mana cost of Seal of Truth by 50%.")]
        public bool GlyphOfTruth { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        #endregion
    }

    public partial class ShamanTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the amount healed by your Earth Shield by 20%.</summary>
        [GlyphData(0, 45775, 63279, "Glyph of Earth Shield", GlyphType.Prime,
            @"Increases the amount healed by your Earth Shield by 20%.")]
        public bool GlyphofEarthShield { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the effectiveness of your Earthliving Weapon's periodic healing by 20%.</summary>
        [GlyphData(1, 41527, 55439, "Glyph of Earthliving Weapon", GlyphType.Prime,
            @"Increases the effectiveness of your Earthliving Weapon's periodic healing by 20%.")]
        public bool GlyphofEarthlivingWeapon { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Your spirit wolves gain an additional 30% of your attack power.</summary>
        [GlyphData(2, 45771, 63271, "Glyph of Feral Spirit", GlyphType.Prime,
            @"Your spirit wolves gain an additional 30% of your attack power.")]
        public bool GlyphofFeralSpirit { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Reduces the cooldown of your Fire Elemental Totem by 5 min.</summary>
        [GlyphData(3, 41529, 55455, "Glyph of Fire Elemental Totem", GlyphType.Prime,
            @"Reduces the cooldown of your Fire Elemental Totem by 5 min.")]
        public bool GlyphofFireElementalTotem { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the duration of your Flame Shock by 50%.</summary>
        [GlyphData(4, 41531, 55447, "Glyph of Flame Shock", GlyphType.Prime,
            @"Increases the duration of your Flame Shock by 50%.")]
        public bool GlyphofFlameShock { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Increases spell critical strike chance by 2% while Flametongue Weapon is active.</summary>
        [GlyphData(5, 41532, 55451, "Glyph of Flametongue Weapon", GlyphType.Prime,
            @"Increases spell critical strike chance by 2% while Flametongue Weapon is active.")]
        public bool GlyphofFlametongueWeapon { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Lava Burst spell deals 10% more damage.</summary>
        [GlyphData(6, 41524, 55454, "Glyph of Lava Burst", GlyphType.Prime,
            @"Your Lava Burst spell deals 10% more damage.")]
        public bool GlyphofLavaBurst { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage dealt by your Lava Lash ability by 20%.</summary>
        [GlyphData(7, 41540, 55444, "Glyph of Lava Lash", GlyphType.Prime,
            @"Increases the damage dealt by your Lava Lash ability by 20%.")]
        public bool GlyphofLavaLash { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage dealt by Lightning Bolt by 4%.</summary>
        [GlyphData(8, 41536, 55453, "Glyph of Lightning Bolt", GlyphType.Prime,
            @"Increases the damage dealt by Lightning Bolt by 4%.")]
        public bool GlyphofLightningBolt { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the duration of Riptide by 40%.</summary>
        [GlyphData(9, 45772, 63273, "Glyph of Riptide", GlyphType.Prime,
            @"Increases the duration of Riptide by 40%.")]
        public bool GlyphofRiptide { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Reduces the global cooldown triggered by your shock spells to 1 sec.</summary>
        [GlyphData(10, 41526, 55442, "Glyph of Shocking", GlyphType.Prime,
            @"Reduces the global cooldown triggered by your shock spells to 1 sec.")]
        public bool GlyphofShocking { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Increases the critical strike chance bonus from your Stormstrike ability by an additional 10%.</summary>
        [GlyphData(11, 41539, 55446, "Glyph of Stormstrike", GlyphType.Prime,
            @"Increases the critical strike chance bonus from your Stormstrike ability by an additional 10%.")]
        public bool GlyphofStormstrike { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the passive mana regeneration of your Water Shield spell by 50%.</summary>
        [GlyphData(12, 41541, 55436, "Glyph of Water Shield", GlyphType.Prime,
            @"Increases the passive mana regeneration of your Water Shield spell by 50%.")]
        public bool GlyphofWaterShield { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the chance per swing for Windfury Weapon to trigger by 2%.</summary>
        [GlyphData(13, 41542, 55445, "Glyph of Windfury Weapon", GlyphType.Prime,
            @"Increases the chance per swing for Windfury Weapon to trigger by 2%.")]
        public bool GlyphofWindfuryWeapon { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 14; } }

        /// <summary>Increases healing done by your Chain Heal to targets beyond the first by 15%, but decreases the amount received by the initial target by 10%.</summary>
        [GlyphData(14, 41517, 55437, "Glyph of Chain Heal", GlyphType.Major,
            @"Increases healing done by your Chain Heal to targets beyond the first by 15%, but decreases the amount received by the initial target by 10%.")]
        public bool GlyphofChainHeal { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Chain Lightning spell now strikes 2 additional targets, but deals 10% less initial damage.</summary>
        [GlyphData(15, 41518, 55449, "Glyph of Chain Lightning", GlyphType.Major,
            @"Your Chain Lightning spell now strikes 2 additional targets, but deals 10% less initial damage.")]
        public bool GlyphofChainLightning { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>While your Elemental Mastery ability is active, you take 20% less damage from all sources.</summary>
        [GlyphData(16, 41552, 55452, "Glyph of Elemental Mastery", GlyphType.Major,
            @"While your Elemental Mastery ability is active, you take 20% less damage from all sources.")]
        public bool GlyphofElementalMastery { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the radius of your Fire Nova spell by 5 yards.</summary>
        [GlyphData(17, 41530, 55450, "Glyph of Fire Nova", GlyphType.Major,
            @"Increases the radius of your Fire Nova spell by 5 yards.")]
        public bool GlyphofFireNova { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the duration of your Frost Shock by 2 sec.</summary>
        [GlyphData(18, 41547, 55443, "Glyph of Frost Shock", GlyphType.Major,
            @"Increases the duration of your Frost Shock by 2 sec.")]
        public bool GlyphofFrostShock { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Your Ghost Wolf form grants an additional 5% movement speed.</summary>
        [GlyphData(19, 43725, 59289, "Glyph of Ghost Wolf", GlyphType.Major,
            @"Your Ghost Wolf form grants an additional 5% movement speed.")]
        public bool GlyphofGhostWolf { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Instead of absorbing a spell, your Grounding Totem reflects the next harmful spell back at its caster, but the cooldown of your Grounding Totem is increased by 35 sec.</summary>
        [GlyphData(20, 41538, 55441, "Glyph of Grounding Totem", GlyphType.Major,
            @"Instead of absorbing a spell, your Grounding Totem reflects the next harmful spell back at its caster, but the cooldown of your Grounding Totem is increased by 35 sec.")]
        public bool GlyphofGroundingTotem { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Healing Stream Totem increases all the resistances of nearby party and raid members within 30 yards  by 130.</summary>
        [GlyphData(21, 41533, 55456, "Glyph of Healing Stream Totem", GlyphType.Major,
            @"Your Healing Stream Totem increases all the resistances of nearby party and raid members within 30 yards  by 130.")]
        public bool GlyphofHealingStreamTotem { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.</summary>
        [GlyphData(22, 41534, 55440, "Glyph of Healing Wave", GlyphType.Major,
            @"Your Healing Wave also heals you for 20% of the healing effect when you heal someone else.")]
        public bool GlyphofHealingWave { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Reduces the cooldown on your Hex spell by 10 seconds.</summary>
        [GlyphData(23, 45777, 63291, "Glyph of Hex", GlyphType.Major,
            @"Reduces the cooldown on your Hex spell by 10 seconds.")]
        public bool GlyphofHex { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Lightning Shield can no longer drop below 3 charges from dealing damage to attackers.</summary>
        [GlyphData(24, 41537, 55448, "Glyph of Lightning Shield", GlyphType.Major,
            @"Your Lightning Shield can no longer drop below 3 charges from dealing damage to attackers.")]
        public bool GlyphofLightningShield { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Activating your Shamanistic Rage ability also cleanses you of all dispellable Magic debuffs.</summary>
        [GlyphData(25, 45776, 63280, "Glyph of Shamanistic Rage", GlyphType.Major,
            @"Activating your Shamanistic Rage ability also cleanses you of all dispellable Magic debuffs.")]
        public bool GlyphofShamanisticRage { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.</summary>
        [GlyphData(26, 45778, 63298, "Glyph of Stoneclaw Totem", GlyphType.Major,
            @"Your Stoneclaw Totem also places a damage absorb shield on you, equal to 4 times the strength of the shield it places on your totems.")]
        public bool GlyphofStoneclawTotem { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Reduces the cooldown on Thunderstorm by 10 sec.</summary>
        [GlyphData(27, 45770, 63270, "Glyph of Thunder", GlyphType.Major,
            @"Reduces the cooldown on Thunderstorm by 10 sec.")]
        public bool GlyphofThunder { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Causes your Totemic Recall ability to return an additional 50% of the mana cost of any recalled totems.</summary>
        [GlyphData(28, 41535, 55438, "Glyph of Totemic Recall", GlyphType.Major,
            @"Causes your Totemic Recall ability to return an additional 50% of the mana cost of any recalled totems.")]
        public bool GlyphofTotemicRecall { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 29; } }
        /// <summary>Reduces the cooldown of your Astral Recall spell by 7.5 min.</summary>
        [GlyphData(29, 43381, 58058, "Glyph of Astral Recall", GlyphType.Minor,
            @"Reduces the cooldown of your Astral Recall spell by 7.5 min.")]
        public bool GlyphofAstralRecall { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Your Reincarnation spell no longer requires a reagent.</summary>
        [GlyphData(30, 43385, 58059, "Glyph of Renewed Life", GlyphType.Minor,
            @"Your Reincarnation spell no longer requires a reagent.")]
        public bool GlyphofRenewedLife { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Alters the appearance of your Ghost Wolf transformation, causing it to resemble an arctic wolf.</summary>
        [GlyphData(31, 43386, 58135, "Glyph of the Arctic Wolf", GlyphType.Minor,
            @"Alters the appearance of your Ghost Wolf transformation, causing it to resemble an arctic wolf.")]
        public bool GlyphofArcticWolf { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Increases the mana you recieve from your Thunderstorm spell by 2%, but it no longer knocks enemies back.</summary>
        [GlyphData(32, 44923, 62132, "Glyph of Thunderstorm", GlyphType.Minor,
            @"Increases the mana you recieve from your Thunderstorm spell by 2%, but it no longer knocks enemies back.")]
        public bool GlyphofThunderstorm { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Your Water Breathing Spell no longer reuires a reagent.</summary>
        [GlyphData(33, 43344, 89646, "Glyph of Water Breathing", GlyphType.Minor,
            @"Your Water Breathing Spell no longer reuires a reagent.")]
        public bool GlyphofWaterBreathing { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        /// <summary>Your Water Walking spell no longer quires a reagent.</summary>
        [GlyphData(34, 43388, 58057, "Glyph of Water Walking", GlyphType.Minor,
            @"Your Water Walking spell no longer quires a reagent.")]
        public bool GlyphofWaterWalking { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion
    }

    public partial class PriestTalents
    {
        private bool[] _glyphData = new bool[35];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Reduces the cooldown of your Dispersion by 45 sec.</summary>
        [GlyphData(0, 45753, 63229, "Glyph of Dispersion", GlyphType.Prime,
            @"Reduces the cooldown of your Dispersion by 45 sec.")]
        public bool GlyphofDispersion { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the critical chance of your Flash Heal on targets below 25% health by 10%.</summary>
        [GlyphData(1, 42400, 55679, "Glyph of Flash Heal", GlyphType.Prime,
            @"Increases the critical chance of your Flash Heal on targets below 25% health by 10%.")]
        public bool GlyphofFlashHeal { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of Guardian Spirit by 30 sec.</summary>
        [GlyphData(2, 45755, 63231, "Glyph of Guardian Spirit", GlyphType.Prime,
            @"Reduces the cooldown of Guardian Spirit by 30 sec.")]
        public bool GlyphofGuardianSpirit { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Increases the total amount of charges on your Lightwell by 5.</summary>
        [GlyphData(3, 42403, 55673, "Glyph of Lightwell", GlyphType.Prime,
            @"Increases the total amount of charges on your Lightwell by 5.")]
        public bool GlyphofLightwell { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage done by your Mind Flay spell by 10%.</summary>
        [GlyphData(4, 42415, 55687, "Glyph of Mind Flay", GlyphType.Prime,
            @"Increases the damage done by your Mind Flay spell by 10%.")]
        public bool GlyphofMindFlay { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the cooldown of Penance by 2 sec.</summary>
        [GlyphData(5, 45756, 63235, "Glyph of Penance", GlyphType.Prime,
            @"Reduces the cooldown of Penance by 2 sec.")]
        public bool GlyphofPenance { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the healing received while under the Barrier by 10%.</summary>
        [GlyphData(6, 42407, 55689, "Glyph of Power Word: Barrier", GlyphType.Prime,
            @"Increases the healing received while under the Barrier by 10%.")]
        public bool GlyphofPowerWordBarrier { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Your Power Word: Shield also heals the target for 20% of the absorption amount.</summary>
        [GlyphData(8, 42408, 55672, "Glyph of Power Word: Shield", GlyphType.Prime,
            @"Your Power Word: Shield also heals the target for 20% of the absorption amount.")]
        public bool GlyphofPowerWordShield { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Prayer of Healing spell also heals an additional 20% of its initial heal over 6 sec.</summary>
        [GlyphData(9, 42409, 55680, "Glyph of Prayer of Healing", GlyphType.Prime,
            @"Your Prayer of Healing spell also heals an additional 20% of its initial heal over 6 sec.")]
        public bool GlyphofPrayerofHealing { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the amount healed by your Renew by an additional 10%.</summary>
        [GlyphData(10, 42411, 55674, "Glyph of Renew", GlyphType.Prime,
            @"Increases the amount healed by your Renew by an additional 10%.")]
        public bool GlyphofRenew { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>If your Shadow Word: Death target fails to kill the target at 25% or less, your Shadow Word: Death cooldown is reset. This effect can not occur more than once every 6 seconds.</summary>
        [GlyphData(12, 42414, 55682, "Glyph of Shadow Word: Death", GlyphType.Prime,
            @"If your Shadow Word: Death target fails to kill the target at 25% or less, your Shadow Word: Death cooldown is reset. This effect can not occur more than once every 6 seconds.")]
        public bool GlyphofShadowWordDeath { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases the periodic damage of your Shadow Word: Pain by 10%.</summary>
        [GlyphData(13, 42406, 55681, "Glyph of Shadow Word: Pain", GlyphType.Prime,
            @"Increases the periodic damage of your Shadow Word: Pain by 10%.")]
        public bool GlyphofShadowWordPain { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 14; } }
        /// <summary></summary>
        [GlyphData(14, 42396, 55675, "Glyph of Circle of Healing", GlyphType.Major,
            @"Your Circle of Healing spell heals 1 additional target.")]
        public bool GlyphofCircleOfHealing { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Allows Pain Suppression and Guardian Spirit to be cast while stunned.</summary>
        [GlyphData(15, 45760, 63248, "Glyph of Desperation", GlyphType.Major,
            @"Allows Pain Suppression and Guardian Spirit to be cast while stunned.")]
        public bool GlyphofDesperation { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Your Dispel Magic spell also heals your target for 3% maximum health when you successfully dispel a magical effect.</summary>
        [GlyphData(16, 42397, 55677, "Glyph of Dispel Magic", GlyphType.Major,
            @"Your Dispel Magic spell also heals your target for 3% maximum health when you successfully dispel a magical effect.")]
        public bool GlyphofDispelMagic { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases your chance to hit with your Smite and Holy Fire by 18%.</summary>
        [GlyphData(17, 45758, 63246, "Glyph of Divine Accuracy", GlyphType.Major,
            @"Increases your chance to hit with your Smite and Holy Fire by 18%.")]
        public bool GlyphofDivineAccuracy { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the cooldown of your Fade spell by 9 sec.</summary>
        [GlyphData(18, 42398, 55684, "Glyph of Fade", GlyphType.Major,
            @"Reduces the cooldown of your Fade spell by 9 sec.")]
        public bool GlyphofFade { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the cooldown and duration of Fear Ward by 60 sec.</summary>
        [GlyphData(19, 42399, 55678, "Glyph of Fear Ward", GlyphType.Major,
            @"Reduces the cooldown and duration of Fear Ward by 60 sec.")]
        public bool GlyphofFearWard { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces the global cooldown of your Holy Nova by 0.5 sec.</summary>
        [GlyphData(20, 42401, 55683, "Glyph of Holy Nova", GlyphType.Major,
            @"Reduces the global cooldown of your Holy Nova by 0.5 sec.")]
        public bool GlyphofHolyNova { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the armor from your Inner Fire spell by 50%.</summary>
        [GlyphData(21, 42402, 55686, "Glyph of Inner Fire", GlyphType.Major,
            @"Increases the armor from your Inner Fire spell by 50%.")]
        public bool GlyphofInnerFire { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Reduces the cast time of Mass Dispel by 1 second.</summary>
        [GlyphData(22, 42404, 55691, "Glyph of Mass Dispel", GlyphType.Major,
            @"Reduces the cast time of Mass Dispel by 1 second.")]
        public bool GlyphofMassDispel { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Your first charge of your Prayer of Mending heals for an additional 60%.</summary>
        [GlyphData(23, 42417, 55685, "Glyph of Prayer of Mending", GlyphType.Major,
            @"Your first charge of your Prayer of Mending heals for an additional 60%.")]
        public bool GlyphofPrayerOfMending { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the cooldown of your Psychic Horror by 30 sec.</summary>
        [GlyphData(24, 42405, 55688, "Glyph of Psychic Horror", GlyphType.Major,
            @"Reduces the cooldown of your Psychic Horror by 30 sec.")]
        public bool GlyphofPsychicHorror { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Targets of your Psychic Scream tremble in place instead of fleeing in fear, but the cooldown of Psychic Scream is increased by 3 sec.</summary>
        [GlyphData(25, 42410, 55676, "Glyph of Psychic Scream", GlyphType.Major,
            @"Targets of your Psychic Scream tremble in place instead of fleeing in fear, but the cooldown of Psychic Scream is increased by 3 sec.")]
        public bool GlyphofPsychicScream { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Reduces the cast time of your Shackle Undead by 1.0 sec.</summary>
        [GlyphData(26, 42412, 55690, "Glyph of Scourge Imprisonment", GlyphType.Major,
            @"Reduces the cast time of your Shackle Undead by 1.0 sec.")]
        public bool GlyphofScourgeImprisonment { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Your Smite spell inflicts an additional 20% damage against targets afflicted by Holy Fire.</summary>
        [GlyphData(27, 42416, 55692, "Glyph of Smite", GlyphType.Major,
            @"Your Smite spell inflicts an additional 20% damage against targets afflicted by Holy Fire.")]
        public bool GlyphofSmite { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>When you kill a target with your Shadow Word: Death and yield experience or honor, you instantly receive 12% of your total mana over 12 sec.</summary>
        [GlyphData(28, 45757, 63237, "Glyph of Spirit Tap", GlyphType.Major,
            @"When you kill a target with your Shadow Word: Death and yield experience or honor, you instantly receive 12% of your total mana over 12 sec.")]
        public bool GlyphofSpiritTap { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 29; } }
        /// <summary>Reduces the mana cost of your Fade spell by 30%.</summary>
        [GlyphData(29, 43342, 57985, "Glyph of Fading", GlyphType.Minor,
            @"Reduces the mana cost of your Fade spell by 30%.")]
        public bool GlyphofFading { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the mana cost of your Power Word: Fortitude by 50%.</summary>
        [GlyphData(30, 43371, 58009, "Glyph of Fortitude", GlyphType.Minor,
            @"Reduces the mana cost of your Power Word: Fortitude by 50%.")]
        public bool GlyphofFortitude { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Your Levitate spell no longer requires a reagent.</summary>
        [GlyphData(31, 43370, 57987, "Glyph of Levitate", GlyphType.Minor,
            @"Your Levitate spell no longer requires a reagent.")]
        public bool GlyphofLevitate { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Increases the range of your Shackle Undead spell by 5 yards.</summary>
        [GlyphData(32, 43373, 57986, "Glyph of Shackle Undead", GlyphType.Minor,
            @"Increases the range of your Shackle Undead spell by 5 yards.")]
        public bool GlyphofShackleUndead { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the duration of your Shadow Protection and Prayer of Shadow Protection spells by 10 min.</summary>
        [GlyphData(33, 43372, 58015, "Glyph of Shadow Protection", GlyphType.Minor,
            @"Increases the duration of your Shadow Protection and Prayer of Shadow Protection spells by 10 min.")]
        public bool GlyphofShadowProtection { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        /// <summary>Receive 5% of your maximum mana if your Shadowfiend dies from damage.</summary>
        [GlyphData(34, 43374, 58228, "Glyph of Shadowfiend", GlyphType.Minor,
            @"Receive 5% of your maximum mana if your Shadowfiend dies from damage.")]
        public bool GlyphofShadowfiend { get { return _glyphData[34]; } set { _glyphData[34] = value; } }
        #endregion

    }

    public partial class DeathKnightTalents
    {
        private bool[] _glyphData = new bool[30];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of your Death and Decay spell by 50%.</summary>
        [GlyphData(0, 43542, 58629, "Glyph of Death and Decay", GlyphType.Prime,
            @"Increases the duration of your Death and Decay spell by 50%.")]
        public bool GlyphofDeathandDecay { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Increases the damage or healing done by Death Coil by 15%.</summary>
        [GlyphData(1, 45804, 63333, "Glyph of Death Coil", GlyphType.Prime,
            @"Increases the damage or healing done by Death Coil by 15%.")]
        public bool GlyphofDeathCoil { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.</summary>
        [GlyphData(2, 43827, 59336, "Glyph of Death Strike", GlyphType.Prime,
            @"Increases your Death Strike's damage by 2% for every 5 runic power you currently have (up to a maximum of 40%). The runic power is not consumed by this effect.")]
        public bool GlyphofDeathStrike { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Reduces the cost of your Frost Strike by 8 Runic Power.</summary>
        [GlyphData(3, 43543, 58647, "Glyph of Frost Strike", GlyphType.Prime,
            @"Reduces the cost of your Frost Strike by 8 Runic Power.")]
        public bool GlyphofFrostStrike { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage of your Heart Strike ability by 30%.</summary>
        [GlyphData(4, 43534, 58616, "Glyph of Heart Strike", GlyphType.Prime,
            @"Increases the damage of your Heart Strike ability by 30%.")]
        public bool GlyphofHeartStrike { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Your Howling Blast ability now infects your targets with Frost Fever.</summary>
        [GlyphData(5, 45806, 63335, "Glyph of Howling Blast", GlyphType.Prime,
            @"Your Howling Blast ability now infects your targets with Frost Fever.")]
        public bool GlyphofHowlingBlast { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Your Frost Fever disease deals 20% additional damage.</summary>
        [GlyphData(6, 43546, 58631, "Glyph of Icy Touch", GlyphType.Prime,
            @"Your Frost Fever disease deals 20% additional damage.")]
        public bool GlyphofIcyTouch { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage of your Obliterate ability by 20%.</summary>
        [GlyphData(7, 43547, 58671, "Glyph of Obliterate", GlyphType.Prime,
            @"Increases the damage of your Obliterate ability by 20%.")]
        public bool GlyphofObliterate { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.</summary>
        [GlyphData(8, 43549, 58686, "Glyph of Raise Dead", GlyphType.Prime,
            @"Your Ghoul receives an additional 40% of your Strength and 40% of your Stamina.")]
        public bool GlyphofRaiseDead { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the critical strike chance of your Rune Strike by 10%.</summary>
        [GlyphData(9, 43550, 58669, "Glyph of Rune Strike", GlyphType.Prime,
            @"Increases the critical strike chance of your Rune Strike by 10%.")]
        public bool GlyphofRuneStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the Shadow damage portion of your Scourge Strike by 30%.</summary>
        [GlyphData(10, 43551, 58642, "Glyph of Scourge Strike", GlyphType.Prime,
            @"Increases the Shadow damage portion of your Scourge Strike by 30%.")]
        public bool GlyphofScourgeStrike { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 11; } }
        /// <summary>Increases the duration of your Anti-Magic Shell by 2 sec.</summary>
        [GlyphData(11, 43533, 58623, "Glyph of Anti-Magic Shell", GlyphType.Major,
            @"Increases the duration of your Anti-Magic Shell by 2 sec.")]
        public bool GlyphofAntiMagicShell { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Increases the radius of your Blood Boil ability by 50%.</summary>
        [GlyphData(12, 43826, 59332, "Glyph of Blood Boil", GlyphType.Major,
            @"Increases the radius of your Blood Boil ability by 50%.")]
        public bool GlyphofBloodBoil { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.</summary>
        [GlyphData(13, 43536, 58673, "Glyph of Bone Shield", GlyphType.Major,
            @"Increases your movement speed by 15% while Bone Shield is active. This does not stack with other movement-speed increasing effects.")]
        public bool GlyphofBoneShield { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.</summary>
        [GlyphData(14, 43537, 58620, "Glyph of Chains of Ice", GlyphType.Major,
            @"Your Chains of Ice also causes 144 to 156 Frost damage, increased by your attack power.")]
        public bool GlyphofChainsofIce { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Increases your threat generation by 50% while your Dancing Rune Weapon is active.</summary>
        [GlyphData(15, 45799, 63330, "Glyph of Dancing Rune Weapon", GlyphType.Major,
            @"Increases your threat generation by 50% while your Dancing Rune Weapon is active.")]
        public bool GlyphofDancingRuneWeapon { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.</summary>
        [GlyphData(16, 68793, 96279, "Glyph of Dark Succor", GlyphType.Major,
            @"Causes your Death Strike ability to always restore at least 15% of your maximum health when used while in Frost or Unholy Presence.")]
        public bool GlyphofDarkSuccor { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the range of your Death Grip ability by 5 yards.</summary>
        [GlyphData(17, 43541, 62259, "Glyph of Death Grip", GlyphType.Major,
            @"Increases the range of your Death Grip ability by 5 yards.")]
        public bool GlyphofDeathGrip { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Your Hungering Cold ability no longer costs runic power.</summary>
        [GlyphData(18, 45800, 63331, "Glyph of Hungering Cold", GlyphType.Major,
            @"Your Hungering Cold ability no longer costs runic power.")]
        public bool GlyphofHungeringCold { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Increases the radius of your Pestilence effect by 5 yards.</summary>
        [GlyphData(19, 43548, 58657, "Glyph of Pestilence", GlyphType.Major,
            @"Increases the radius of your Pestilence effect by 5 yards.")]
        public bool GlyphofPestilence { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.</summary>
        [GlyphData(20, 43553, 58635, "Glyph of Pillar of Frost", GlyphType.Major,
            @"Empowers your Pillar of Frost, making you immune to all effects that cause loss of control of your character, but also freezing you in place while the ability is active.")]
        public bool GlyphofPillarofFrost { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Your Rune Tap also heals your party for 5% of their maximum health.</summary>
        [GlyphData(21, 43825, 59327, "Glyph of Rune Tap", GlyphType.Major,
            @"Your Rune Tap also heals your party for 5% of their maximum health.")]
        public bool GlyphofRuneTap { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.</summary>
        [GlyphData(22, 43552, 58618, "Glyph of Strangulate", GlyphType.Major,
            @"Increases the Silence duration of your Strangulate ability by 2 sec when used on a target who is casting a spell.")]
        public bool GlyphofStrangulate { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.</summary>
        [GlyphData(23, 43554, 58676, "Glyph of Vampiric Blood", GlyphType.Major,
            @"Increases the bonus healing received while your Vampiric Blood is active by an additional 15%, but your Vampiric Blood no longer grants you health.")]
        public bool GlyphofVampiricBlood { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 24; } }
        /// <summary>Your Blood Tap no longer causes damage to you.</summary>
        [GlyphData(24, 43535, 58640, "Glyph of Blood Tap", GlyphType.Minor,
            @"Your Blood Tap no longer causes damage to you.")]
        public bool GlyphofBloodTap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Reduces the cast time of your Death Gate spell by 60%.</summary>
        [GlyphData(25, 43673, 60200, "Glyph of Death Gate", GlyphType.Minor,
            @"Reduces the cast time of your Death Gate spell by 60%.")]
        public bool GlyphofDeathGate { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Your Death Coil refunds 20 runic power when used to heal.</summary>
        [GlyphData(26, 43539, 58677, "Glyph of Death's Embrace", GlyphType.Minor,
            @"Your Death Coil refunds 20 runic power when used to heal.")]
        public bool GlyphofDeathsEmbrace { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Increases the duration of your Horn of Winter ability by 1 min.</summary>
        [GlyphData(27, 43544, 58680, "Glyph of Horn of Winter", GlyphType.Minor,
            @"Increases the duration of your Horn of Winter ability by 1 min.")]
        public bool GlyphofHornofWinter { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Your Path of Frost ability allows you to fall from a greater distance without suffering damage.</summary>
        [GlyphData(28, 43671, 59307, "Glyph of Path of Frost", GlyphType.Minor,
            @"Your Path of Frost ability allows you to fall from a greater distance without suffering damage.")]
        public bool GlyphofPathofFrost { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>When your Death Grip ability fails because its target is immune, its cooldown is reset.</summary>
        [GlyphData(29, 43672, 59309, "Glyph of Resilient Grip", GlyphType.Minor,
            @"When your Death Grip ability fails because its target is immune, its cooldown is reset.")]
        public bool GlyphofResilientGrip { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        #endregion
    } 

    public partial class WarlockTalents
    {
        private bool[] _glyphData = new bool[34];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of your Bane of Agony by 4 sec.</summary>
        [GlyphData(0, 42456, 56241, "Glyph of Bane of Agony", GlyphType.Prime,
            @"Increases the duration of your Bane of Agony by 4 sec.")]
        public bool GlyphOfBaneOfAgony { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Reduces the cooldown on Chaos Bolt by 2 sec.</summary>
        [GlyphData(1, 45781, 63304, "Glyph of Chaos Bolt", GlyphType.Prime,
            @"Reduces the cooldown on Chaos Bolt by 2 sec.")]
        public bool GlyphOfChaosBolt { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of your Conflagrate by 2 sec.</summary>
        [GlyphData(2, 42454, 56235, "Glyph of Conflagrate", GlyphType.Prime,
            @"Reduces the cooldown of your Conflagrate by 2 sec.")]
        public bool GlyphOfConflagrate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Corruption spell has a 4% chance to cause you to enter a Shadow Trance state after damaging the opponent.  The Shadow Trance state reduces the casting time of your next Shadow Bolt spell by 100%.</summary>
        [GlyphData(3, 42455, 56218, "Glyph of Corruption", GlyphType.Prime,
            @"Your Corruption spell has a 4% chance to cause you to enter a Shadow Trance state after damaging the opponent.  The Shadow Trance state reduces the casting time of your next Shadow Bolt spell by 100%.")]
        public bool GlyphOfCorruption { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the damage done by your Felguard's Legion Strike by 5%.</summary>
        [GlyphData(4, 42459, 56246, "Glyph of Felguard", GlyphType.Prime,
            @"Increases the damage done by your Felguard's Legion Strike by 5%.")]
        public bool GlyphOfFelguard { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>The bonus damage granted by your Haunt spell is increased by an additional 3%.</summary>
        [GlyphData(5, 45779, 63302, "Glyph of Haunt", GlyphType.Prime,
            @"The bonus damage granted by your Haunt spell is increased by an additional 3%.")]
        public bool GlyphOfHaunt { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases the periodic damage of your Immolate by 10%.</summary>
        [GlyphData(6, 42464, 56228, "Glyph of Immolate", GlyphType.Prime,
            @"Increases the periodic damage of your Immolate by 10%.")]
        public bool GlyphOfImmolate { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the damage done by your Imp's Firebolt spell by 20%.</summary>
        [GlyphData(7, 42465, 56248, "Glyph of Imp", GlyphType.Prime,
            @"Increases the damage done by your Imp's Firebolt spell by 20%.")]
        public bool GlyphOfImp { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the damage done by Incinerate by 5%.</summary>
        [GlyphData(8, 42453, 56242, "Glyph of Incinerate", GlyphType.Prime,
            @"Increases the damage done by Incinerate by 5%.")]
        public bool GlyphOfIncinerate { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage done by your Succubus' Lash of Pain by 25%.</summary>
        [GlyphData(9, 50077, 79047, "Glyph of Lash of Pain", GlyphType.Prime,
            @"Increases the damage done by your Succubus' Lash of Pain by 25%.")]
        public bool GlyphOfLashPain { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the duration of your Metamorphosis by 6 sec.</summary>
        [GlyphData(10, 45780, 63303, "Glyph of Metamorphosis", GlyphType.Prime,
            @"Increases the duration of your Metamorphosis by 6 sec.")]
        public bool GlyphOfMetamorphosis { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>If your Shadowburn fails to kill the target at or below 20% health, your Shadowburn's cooldown is instantly reset. This effect has a 6 sec cooldown.</summary>
        [GlyphData(11, 42468, 56629, "Glyph of Shadowburn", GlyphType.Prime,
            @"If your Shadowburn fails to kill the target at or below 20% health, your Shadowburn's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphOfShadowburn { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Decreases the casting time of your Unstable Affliction by 0.2 sec.</summary>
        [GlyphData(12, 42472, 56233, "Glyph of Unstable Affliction", GlyphType.Prime,
            @"Decreases the casting time of your Unstable Affliction by 0.2 sec.")]
        public bool GlyphOfUnstableAffliction { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 13; } }
        /// <summary>Increases the duration of your Death Coil by 0.5 sec.</summary>
        [GlyphData(13, 42457, 56232, "Glyph of Death Coil", GlyphType.Major,
            @"Increases the duration of your Death Coil by 0.5 sec.")]
        public bool GlyphOfDeathCoil { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary></summary>
        [GlyphData(14, 45782, 63309, "Glyph of Demonic Circle", GlyphType.Major,
            @"Reduces the cooldown on Demonic Circle by 4 sec.")]
        public bool GlyphOfDemonicCircle { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>Your Fear causes the target to tremble in place instead of fleeing in fear, but now causes Fear to have a 5 sec cooldown.</summary>
        [GlyphData(15, 42458, 56244, "Glyph of Fear", GlyphType.Major,
            @"Your Fear causes the target to tremble in place instead of fleeing in fear, but now causes Fear to have a 5 sec cooldown.")]
        public bool GlyphOfFear { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>When your Felhunter uses Devour Magic, you will also be healed for that amount.</summary>
        [GlyphData(16, 42450, 56249, "Glyph of Felhunter", GlyphType.Major,
            @"When your Felhunter uses Devour Magic, you will also be healed for that amount.")]
        public bool GlyphOfFelhunter { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>You receive 30% more healing from using a healthstone.</summary>
        [GlyphData(17, 42462, 56224, "Glyph of Healthstone", GlyphType.Major,
            @"You receive 30% more healing from using a healthstone.")]
        public bool GlyphOfHealthstone { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Reduces the cooldown on your Howl of Terror spell by 8 sec.</summary>
        [GlyphData(18, 42463, 56217, "Glyph of Howl of Terror", GlyphType.Major,
            @"Reduces the cooldown on your Howl of Terror spell by 8 sec.")]
        public bool GlyphHowlTerror { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Reduces the global cooldown of your Life Tap by .5 sec.</summary>
        [GlyphData(19, 45785, 63320, "Glyph of Life Tap", GlyphType.Major,
            @"Reduces the global cooldown of your Life Tap by .5 sec.")]
        public bool GlyphOfLifeTap { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Your Succubus's Seduction ability also removes all damage over time effects from the target.</summary>
        [GlyphData(20, 42471, 56250, "Glyph of Seduction", GlyphType.Major,
            @"Your Succubus's Seduction ability also removes all damage over time effects from the target.")]
        public bool GlyphOfSeduction { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the mana cost of your Shadow Bolt by 15%.</summary>
        [GlyphData(21, 42467, 56240, "Glyph of Shadow Bolt", GlyphType.Major,
            @"Reduces the mana cost of your Shadow Bolt by 15%.")]
        public bool GlyphOfShadowBolt { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Your Shadowflame also applies a 70% movement speed slow to its victims.</summary>
        [GlyphData(22, 45783, 63310, "Glyph of Shadowflame", GlyphType.Major,
            @"Your Shadowflame also applies a 70% movement speed slow to its victims.")]
        public bool GlyphOfShadowflame { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Increases the percentage of damage shared via your Soul Link by an additional 5%.</summary>
        [GlyphData(23, 45789, 63312, "Glyph of Soul Link", GlyphType.Major,
            @"Increases the percentage of damage shared via your Soul Link by an additional 5%.")]
        public bool GlyphOfSoulLink { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Your Soul Swap leaves your damage-over-time spells behind on the target you Soul Swapped from, but gives Soul Swap a 15 sec cooldown.</summary>
        [GlyphData(24, 42466, 56226, "Glyph of Soul Swap", GlyphType.Major,
            @"Your Soul Swap leaves your damage-over-time spells behind on the target you Soul Swapped from, but gives Soul Swap a 15 sec cooldown.")]
        public bool GlyphOfSoulSwap { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Increases the amount of health you gain from resurrecting via a Soulstone by an additional 40%.</summary>
        [GlyphData(25, 42470, 56231, "Glyph of Soulstone", GlyphType.Major,
            @"Increases the amount of health you gain from resurrecting via a Soulstone by an additional 40%.")]
        public bool GlyphOfSoulstone { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary></summary>
        [GlyphData(26, 42473, 56247, "Glyph of Voidwalker", GlyphType.Major,
            @"Increases your Voidwalker's total health by 20%.")]
        public bool GlyphOfVoidwalker { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 27; } }
        /// <summary>Increases the range of your Curse of Exhaustion spell by 5 yards.</summary>
        [GlyphData(27, 43392, 58080, "Glyph of Curse of Exhaustion", GlyphType.Minor,
            @"Increases the range of your Curse of Exhaustion spell by 5 yards.")]
        public bool GlyphCurseOfExhaustion { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Your Drain Soul restores 10% of your total mana after you kill a target that yields experience or honor.</summary>
        [GlyphData(28, 43390, 58070, "Glyph of Drain Soul", GlyphType.Minor,
            @"Your Drain Soul restores 10% of your total mana after you kill a target that yields experience or honor.")]
        public bool GlyphDrainSoul { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the cast time of your Enslave Demon spell by 50%.</summary>
        [GlyphData(29, 43393, 58107, "Glyph of Enslave Demon", GlyphType.Minor,
            @"Reduces the cast time of your Enslave Demon spell by 50%.")]
        public bool GlyphEnslaveDemon { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Increases the movement speed of your Eye of Kilrogg by 50% and allows it to fly in areas where flying mounts are enabled.</summary>
        [GlyphData(30, 43391, 58081, "Glyph of Eye of Kilrogg", GlyphType.Minor,
            @"Increases the movement speed of your Eye of Kilrogg by 50% and allows it to fly in areas where flying mounts are enabled.")]
        public bool GlyphEyeKilrogg { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while channeling your Health Funnel spell by 100%.</summary>
        [GlyphData(31, 42461, 56238, "Glyph of Health Funnel", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while channeling your Health Funnel spell by 100%.")]
        public bool GlyphHealthFunnel { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the mana cost of your Ritual of Souls spell by 70%.</summary>
        [GlyphData(32, 43394, 58094, "Glyph of Ritual of Souls", GlyphType.Minor,
            @"Reduces the mana cost of your Ritual of Souls spell by 70%.")]
        public bool GlyphRitualSouls { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the swim speed of targets affected by your Unending Breath spell by 20%.</summary>
        [GlyphData(33, 43389, 58079, "Glyph of Unending Breath", GlyphType.Minor,
            @"Increases the swim speed of targets affected by your Unending Breath spell by 20%.")]
        public bool GlyphUnendingBreath { get { return _glyphData[33]; } set { _glyphData[33] = value; } }
        #endregion       
    }

    public partial class RogueTalents
    {
        private bool[] _glyphData = new bool[36];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>Increases the duration of Adrenaline Rush by 5 sec.</summary>
        [GlyphData(0, 42954, 56808, "Glyph of Adrenaline Rush", GlyphType.Prime,
            @"Increases the duration of Adrenaline Rush by 5 sec.")]
        public bool GlyphOfAdrenalineRush { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Your Backstab critical strikes grant you 5 energy.</summary>
        [GlyphData(1, 42956, 56800, "Glyph of Backstab", GlyphType.Prime,
            @"Your Backstab critical strikes grant you 5 energy.")]
        public bool GlyphOfBackstab { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Increases the critical strike chance of Eviscerate by 10%.</summary>
        [GlyphData(2, 42961, 56802, "Glyph of Eviscerate", GlyphType.Prime,
            @"Increases the critical strike chance of Eviscerate by 10%.")]
        public bool GlyphOfEviscerate { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Hemorrhage ability also causes the target to bleed, dealing 40% of the direct strike's damage over 24 sec.</summary>
        [GlyphData(3, 42967, 56807, "Glyph of Hemorrhage", GlyphType.Prime,
            @"Your Hemorrhage ability also causes the target to bleed, dealing 40% of the direct strike's damage over 24 sec.")]
        public bool GlyphOfHemorrhage { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the bonus to your damage while Killing Spree is active by an additional 10%.</summary>
        [GlyphData(4, 45762, 63252, "Glyph of Killing Spree", GlyphType.Prime,
            @"Increases the bonus to your damage while Killing Spree is active by an additional 10%.")]
        public bool GlyphOfKillingSpree { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the cost of Mutilate by 5 energy.</summary>
        [GlyphData(5, 45768, 63268, "Glyph of Mutilate", GlyphType.Prime,
            @"Reduces the cost of Mutilate by 5 energy.")]
        public bool GlyphOfMutilate { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>Increases Revealing Strike's bonus effectiveness to your finishing moves by an additional 10%.</summary>
        [GlyphData(6, 42965, 56814, "Glyph of Revealing Strike", GlyphType.Prime,
            @"Increases Revealing Strike's bonus effectiveness to your finishing moves by an additional 10%.")]
        public bool GlyphOfRevealingStrike { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the duration of Rupture by 4 sec.</summary>
        [GlyphData(7, 42969, 56801, "Glyph of Rupture", GlyphType.Prime,
            @"Increases the duration of Rupture by 4 sec.")]
        public bool GlyphOfRupture { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the duration of Shadow Dance by 2 sec.</summary>
        [GlyphData(8, 45764, 63253, "Glyph of Shadow Dance", GlyphType.Prime,
            @"Increases the duration of Shadow Dance by 2 sec.")]
        public bool GlyphOfShadowDance { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Your Sinister Strikes have a 20% chance to add an additional combo point.</summary>
        [GlyphData(9, 42972, 56821, "Glyph of Sinister Strike", GlyphType.Prime,
            @"Your Sinister Strikes have a 20% chance to add an additional combo point.")]
        public bool GlyphOfSinisterStrike { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the duration of Slice and Dice by 6 sec.</summary>
        [GlyphData(10, 42973, 56810, "Glyph of Slice and Dice", GlyphType.Prime,
            @"Increases the duration of Slice and Dice by 6 sec.")]
        public bool GlyphOfSliceandDice { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Increases the duration of your Vendetta ability by 20%.</summary>
        [GlyphData(11, 45761, 63249, "Glyph of Vendetta", GlyphType.Prime,
            @"Increases the duration of your Vendetta ability by 20%.")]
        public bool GlyphOfVendetta { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 12; } }

        /// <summary>Increases the range on Ambush by 5 yards.</summary>
        [GlyphData(12, 42955, 56813, "Glyph of Ambush", GlyphType.Major,
            @"Increases the range on Ambush by 5 yards.")]
        public bool GlyphOfAmbush { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Reduces the penalty to energy generation while Blade Flurry is active by 50%.</summary>
        [GlyphData(13, 42957, 56818, "Glyph of Blade Flurry", GlyphType.Major,
            @"Reduces the penalty to energy generation while Blade Flurry is active by 50%.")]
        public bool GlyphOfBladeFlurry { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>Your Blind ability also removes all damage over time effects from the target.</summary>
        [GlyphData(14, 64493, 91299, "Glyph of Blind", GlyphType.Major,
            @"Your Blind ability also removes all damage over time effects from the target.")]
        public bool GlyphOfBlind { get { return _glyphData[14]; } set { _glyphData[14] = value; } }
        /// <summary>While Cloak of Shadows is active, you take 40% less physical damage.</summary>
        [GlyphData(15, 45769, 63269, "Glyph of Cloak of Shadows", GlyphType.Major,
            @"While Cloak of Shadows is active, you take 40% less physical damage.")]
        public bool GlyphOfCloakOfShadows { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Increases the chance to inflict your target with Crippling Poison by an additional 20%.</summary>
        [GlyphData(16, 42958, 56820, "Glyph of Crippling Poison", GlyphType.Major,
            @"Increases the chance to inflict your target with Crippling Poison by an additional 20%.")]
        public bool GlyphOfCripplingPoison { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the slowing effect on Deadly Throw by 20%.</summary>
        [GlyphData(17, 42959, 56806, "Glyph of Deadly Throw", GlyphType.Major,
            @"Increases the slowing effect on Deadly Throw by 20%.")]
        public bool GlyphOfDeadlyThrow { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the duration of Evasion by 5 sec.</summary>
        [GlyphData(18, 42960, 56799, "Glyph of Evasion", GlyphType.Major,
            @"Increases the duration of Evasion by 5 sec.")]
        public bool GlyphOfEvasion { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>Increases the duration of Expose Armor by 12 sec.</summary>
        [GlyphData(19, 42962, 56803, "Glyph of Expose Armor", GlyphType.Major,
            @"Increases the duration of Expose Armor by 12 sec.")]
        public bool GlyphOfExposeArmor { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Increases the radius of your Fan of Knives ability by 50%.</summary>
        [GlyphData(20, 45766, 63254, "Glyph of Fan of Knives", GlyphType.Major,
            @"Increases the radius of your Fan of Knives ability by 50%.")]
        public bool GlyphOfFanOfKnives { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Reduces the energy cost of Feint by 20.</summary>
        [GlyphData(21, 42963, 56804, "Glyph of Feint", GlyphType.Major,
            @"Reduces the energy cost of Feint by 20.")]
        public bool GlyphOfFeint { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>Increases the duration of your Garrote ability's silence effect by 1.5 sec.</summary>
        [GlyphData(22, 42964, 56812, "Glyph of Garrote", GlyphType.Major,
            @"Increases the duration of your Garrote ability's silence effect by 1.5 sec.")]
        public bool GlyphOfGarrote { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Your Gouge ability no longer requires that the target be facing you.</summary>
        [GlyphData(23, 42966, 56809, "Glyph of Gouge", GlyphType.Major,
            @"Your Gouge ability no longer requires that the target be facing you.")]
        public bool GlyphOfGouge { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Increases the cooldown of your Kick ability by 4 sec, but this cooldown is reduced by 6 sec when your Kick successfully interrupts a spell.</summary>
        [GlyphData(24, 42971, 56805, "Glyph of Kick", GlyphType.Major,
            @"Increases the cooldown of your Kick ability by 4 sec, but this cooldown is reduced by 6 sec when your Kick successfully interrupts a spell.")]
        public bool GlyphOfKick { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Your Preparation ability also instantly resets the cooldown of Kick, Dismantle, and Smoke Bomb.</summary>
        [GlyphData(25, 42968, 56819, "Glyph of Preparation", GlyphType.Major,
            @"Your Preparation ability also instantly resets the cooldown of Kick, Dismantle, and Smoke Bomb.")]
        public bool GlyphOfPreparation { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        /// <summary>Increases the duration of Sap against non-player targets by 80 sec.</summary>
        [GlyphData(26, 42970, 56798, "Glyph of Sap", GlyphType.Major,
            @"Increases the duration of Sap against non-player targets by 80 sec.")]
        public bool GlyphOfSap { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Increases the movement speed of your Sprint ability by an additional 30%.</summary>
        [GlyphData(27, 42974, 56811, "Glyph of Sprint", GlyphType.Major,
            @"Increases the movement speed of your Sprint ability by an additional 30%.")]
        public bool GlyphOfSprint { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Removes the energy cost of your Tricks of the Trade ability but reduces the recipient's damage bonus by 5%.</summary>
        [GlyphData(28, 45767, 63256, "Glyph of Tricks of the Trade", GlyphType.Major,
            @"Removes the energy cost of your Tricks of the Trade ability but reduces the recipient's damage bonus by 5%.")]
        public bool GlyphOfTricksOfTheTrade { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Increases the duration of your Vanish effect by 2 sec.</summary>
        [GlyphData(29, 63420, 89758, "Glyph of Vanish", GlyphType.Major,
            @"Increases the duration of your Vanish effect by 2 sec.")]
        public bool GlyphOfVanish { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 30; } }
        /// <summary>You gain the ability to walk on water while your Sprint ability is active.</summary>
        [GlyphData(30, 43379, 58039, "Glyph of Blurred Speed", GlyphType.Minor,
            @"You gain the ability to walk on water while your Sprint ability is active.")]
        public bool GlyphOfBlurredSpeed { get { return _glyphData[30]; } set{ _glyphData[30] = value;} }
        /// <summary>Increases the range of your Distract ability by 5 yards.</summary>
        [GlyphData(31, 43376, 58032, "Glyph of Distract", GlyphType.Minor,
            @"Increases the range of your Distract ability by 5 yards.")]
        public bool GlyphOfDistrict { get { return _glyphData[31]; } set { _glyphData[31] = value; } }
        /// <summary>Reduces the cast time of your Pick Lock ability by 100%.</summary>
        [GlyphData(32, 43377, 58027, "Glyph of Pick Lock", GlyphType.Minor,
            @"Reduces the cast time of your Pick Lock ability by 100%.")]
        public bool GlyphOfPickLock { get { return _glyphData[32]; } set { _glyphData[32] = value; } }
        /// <summary>Increases the range of your Pick Pocket ability by 5 yards.</summary>
        [GlyphData(33, 43343, 58017, "Glyph of Pick Pocket", GlyphType.Minor,
            @"Increases the range of your Pick Pocket ability by 5 yards.")]
        public bool GlyphOfPickPocket { get { return _glyphData[33]; } set{ _glyphData[33] = value;} }
        /// <summary>You apply poisons to your weapons 50% faster.</summary>
        [GlyphData(34, 43380, 58038, "Glyph of Poisons", GlyphType.Minor,
            @"You apply poisons to your weapons 50% faster.")]
        public bool GlyphOfPoisons { get { return _glyphData[34]; } set{ _glyphData[34] = value;} }
        /// <summary>Increases the distance your Safe Fall ability allows you to fall without taking damage.</summary>
        [GlyphData(35, 43378, 58033, "Glyph of Safe Fall", GlyphType.Minor,
            @"Increases the distance your Safe Fall ability allows you to fall without taking damage.")]
        public bool GlyphOfSafeFall { get { return _glyphData[35]; } set{ _glyphData[35] = value;} }
        #endregion
    }

    public partial class HunterTalents
    {
        private bool[] _glyphData = new bool[31];
        public override bool[] GlyphData { get { return _glyphData; } }

        #region Prime
        public override int GlyphTreeStartingIndexes_0 { get { return 0; } }
        /// <summary>When you critically hit with Aimed Shot, you instantly gain 5 Focus.</summary>
        [GlyphData(0, 42897, 56824, "Glyph of Aimed Shot", GlyphType.Prime,
            @"When you critically hit with Aimed Shot, you instantly gain 5 Focus.")]
        public bool GlyphOfAimedShot { get { return _glyphData[0]; } set { _glyphData[0] = value; } }
        /// <summary>Your Arcane Shot deals 12% more damage.</summary>
        [GlyphData(1, 42898, 56841, "Glyph of Arcane Shot", GlyphType.Prime,
            @"Your Arcane Shot deals 12% more damage.")]
        public bool GlyphOfArcaneShot { get { return _glyphData[1]; } set { _glyphData[1] = value; } }
        /// <summary>Reduces the cooldown of Chimera Shot by 1 sec.</summary>
        [GlyphData(2, 45625, 63065, "Glyph of Chimera Shot", GlyphType.Prime,
            @"Reduces the cooldown of Chimera Shot by 1 sec.")]
        public bool GlyphOfChimeraShot { get { return _glyphData[2]; } set { _glyphData[2] = value; } }
        /// <summary>Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.</summary>
        [GlyphData(3, 42909, 56856, "Glyph of Dazzled Prey", GlyphType.Prime,
            @"Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.")]
        public bool GlyphOfDazzledPrey { get { return _glyphData[3]; } set { _glyphData[3] = value; } }
        /// <summary>Increases the critical strike chance of Explosive Shot by 6%.</summary>
        [GlyphData(4, 45731, 63066, "Glyph of Explosive Shot", GlyphType.Prime,
            @"Increases the critical strike chance of Explosive Shot by 6%.")]
        public bool GlyphOfExplosiveShot { get { return _glyphData[4]; } set { _glyphData[4] = value; } }
        /// <summary>Reduces the Focus cost of your Kill Command by 3.</summary>
        [GlyphData(5, 42915, 56842, "Glyph of Kill Command", GlyphType.Prime,
            @"Reduces the Focus cost of your Kill Command by 3.")]
        public bool GlyphOfKillCommand { get { return _glyphData[5]; } set { _glyphData[5] = value; } }
        /// <summary>If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.</summary>
        [GlyphData(6, 45732, 63067, "Glyph of Kill Shot", GlyphType.Prime,
            @"If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
        public bool GlyphOfKillShot { get { return _glyphData[6]; } set { _glyphData[6] = value; } }
        /// <summary>Increases the haste from Rapid Fire by an additional 10%.</summary>
        [GlyphData(7, 42911, 56828, "Glyph of Rapid Fire", GlyphType.Prime,
            @"Increases the haste from Rapid Fire by an additional 10%.")]
        public bool GlyphOfRapidFire { get { return _glyphData[7]; } set { _glyphData[7] = value; } }
        /// <summary>Increases the periodic critical strike chance of your Serpent Sting by 6%.</summary>
        [GlyphData(8, 42912, 56832, "Glyph of Serpent Sting", GlyphType.Prime,
            @"Increases the periodic critical strike chance of your Serpent Sting by 6%.")]
        public bool GlyphOfSerpentSting { get { return _glyphData[8]; } set { _glyphData[8] = value; } }
        /// <summary>Increases the damage dealt by Steady Shot by 10%.</summary>
        [GlyphData(9, 42914, 56826, "Glyph of Steady Shot", GlyphType.Prime,
            @"Increases the damage dealt by Steady Shot by 10%.")]
        public bool GlyphOfSteadyShot { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        #endregion
        #region Major
        public override int GlyphTreeStartingIndexes_1 { get { return 10; } }
        /// <summary>Decreases the cooldown of Bestial Wrath by 20 sec.</summary>
        [GlyphData(10, 42902, 56830, "Glyph of Bestial Wrath", GlyphType.Major,
            @"Decreases the cooldown of Bestial Wrath by 20 sec.")]
        public bool GlyphOfBestialWrath { get { return _glyphData[10]; } set { _glyphData[10] = value; } }
        /// <summary>Your Concussive Shot also limits the maximum run speed of your target.</summary>
        [GlyphData(11, 42901, 56851, "Glyph of Concussive Shot", GlyphType.Major,
            @"Your Concussive Shot also limits the maximum run speed of your target.")]
        public bool GlyphOfConcussiveShot { get { return _glyphData[11]; } set { _glyphData[11] = value; } }
        /// <summary>Decreases the cooldown of Deterrence by 10 sec.</summary>
        [GlyphData(12, 42903, 56850, "Glyph of Deterrence", GlyphType.Major,
            @"Decreases the cooldown of Deterrence by 10 sec.")]
        public bool GlyphOfDeterrence { get { return _glyphData[12]; } set { _glyphData[12] = value; } }
        /// <summary>Decreases the cooldown of Disengage by 5 sec.</summary>
        [GlyphData(13, 42904, 56844, "Glyph of Disengage", GlyphType.Major,
            @"Decreases the cooldown of Disengage by 5 sec.")]
        public bool GlyphOfDisengage { get { return _glyphData[13]; } set { _glyphData[13] = value; } }
        /// <summary>When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.</summary>
        [GlyphData(14, 42905, 56845, "Glyph of Freezing Trap", GlyphType.Major,
            @"When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.")]
        public bool GlyphOfFreezingTrap { get { return _glyphData[9]; } set { _glyphData[9] = value; } }
        /// <summary>Increases the radius of the effect from your Ice Trap by 2 yards.</summary>
        [GlyphData(15, 42906, 56847, "Glyph of Ice Trap", GlyphType.Major,
            @"Increases the radius of the effect from your Ice Trap by 2 yards.")]
        public bool GlyphOfFrostTrap { get { return _glyphData[15]; } set { _glyphData[15] = value; } }
        /// <summary>Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.</summary>
        [GlyphData(16, 42908, 56846, "Glyph of Immolation Trap", GlyphType.Major,
            @"Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.")]
        public bool GlyphOfImmolationTrap { get { return _glyphData[16]; } set { _glyphData[16] = value; } }
        /// <summary>Increases the duration of your Master's Call by 4 sec.</summary>
        [GlyphData(17, 45733, 63068, "Glyph of Master's Call", GlyphType.Major,
            @"Increases the duration of your Master's Call by 4 sec.")]
        public bool GlyphOfMastersCall { get { return _glyphData[17]; } set { _glyphData[17] = value; } }
        /// <summary>Increases the total amount of healing done by your Mend Pet ability by 60%.</summary>
        [GlyphData(18, 42900, 56833, "Glyph of Mending", GlyphType.Major,
            @"Increases the total amount of healing done by your Mend Pet ability by 60%.")]
        public bool GlyphOfMending { get { return _glyphData[18]; } set { _glyphData[18] = value; } }
        /// <summary>When you use Misdirection on your pet, the cooldown on your Misdirection is reset.</summary>
        [GlyphData(19, 42907, 56829, "Glyph of Misdirection", GlyphType.Major,
            @"When you use Misdirection on your pet, the cooldown on your Misdirection is reset.")]
        public bool GlyphOfMisdirection { get { return _glyphData[19]; } set { _glyphData[19] = value; } }
        /// <summary>Reduces damage taken by 20% for 5 sec after using Raptor Strike.</summary>
        [GlyphData(20, 45735, 63086, "Glyph of Raptor Strike", GlyphType.Major,
            @"Reduces damage taken by 20% for 5 sec after using Raptor Strike.")]
        public bool GlyphOfRaptorStrike { get { return _glyphData[20]; } set { _glyphData[20] = value; } }
        /// <summary>Increases the range of Scatter Shot by 3 yards.</summary>
        [GlyphData(21, 45734, 63069, "Glyph of Scatter Shot", GlyphType.Major,
            @"Increases the range of Scatter Shot by 3 yards.")]
        public bool GlyphOfScatterShot { get { return _glyphData[21]; } set { _glyphData[21] = value; } }
        /// <summary>When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.</summary>
        [GlyphData(22, 42910, 56836, "Glyph of Silencing Shot", GlyphType.Major,
            @"When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.")]
        public bool GlyphOfSilencingShot { get { return _glyphData[22]; } set { _glyphData[22] = value; } }
        /// <summary>Snakes from your Snake Trap take 90% reduced damage from area of effect spells.</summary>
        [GlyphData(23, 42913, 56849, "Glyph of Snake Trap", GlyphType.Major,
            @"Snakes from your Snake Trap take 90% reduced damage from area of effect spells.")]
        public bool GlyphOfSnakeTrap { get { return _glyphData[23]; } set { _glyphData[23] = value; } }
        /// <summary>Reduces the focus cost of Trap Launcher by 10.</summary>
        [GlyphData(24, 42899, 56857, "Glyph of Trap Launcher", GlyphType.Major,
            @"Reduces the focus cost of Trap Launcher by 10.")]
        public bool GlyphOfTrueshotAura { get { return _glyphData[24]; } set { _glyphData[24] = value; } }
        /// <summary>Decreases the cooldown of your Wyvern Sting by 6 sec.</summary>
        [GlyphData(25, 42917, 56848, "Glyph of Wyvern Sting", GlyphType.Major,
            @"Decreases the cooldown of your Wyvern Sting by 6 sec.")]
        public bool GlyphOfWyvernSting { get { return _glyphData[25]; } set { _glyphData[25] = value; } }
        #endregion
        #region Minor
        public override int GlyphTreeStartingIndexes_2 { get { return 26; } }
        /// <summary>Increases the range of your Aspect of the Pack ability by 15 yards.</summary>
        [GlyphData(26, 43355, 57904, "Glyph of Aspect of the Pack", GlyphType.Minor,
            @"Increases the range of your Aspect of the Pack ability by 15 yards.")]
        public bool GlyphOfAspectofthePack { get { return _glyphData[26]; } set { _glyphData[26] = value; } }
        /// <summary>Reduces the cooldown of your Feign Death spell by 5 sec.</summary>
        [GlyphData(27, 43351, 57903, "Glyph of Feign Death", GlyphType.Minor,
            @"Reduces the cooldown of your Feign Death spell by 5 sec.")]
        public bool GlyphOfFeignDeath { get { return _glyphData[27]; } set { _glyphData[27] = value; } }
        /// <summary>Slightly reduces the size of your Pet.</summary>
        [GlyphData(28, 43350, 57870, "Glyph of Lesser Proportion", GlyphType.Minor,
            @"Slightly reduces the size of your Pet.")]
        public bool GlyphOfLesserProportion { get { return _glyphData[28]; } set { _glyphData[28] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.</summary>
        [GlyphData(29, 43338, 57866, "Glyph of Revive Pet", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.")]
        public bool GlyphOfRevivePet { get { return _glyphData[29]; } set { _glyphData[29] = value; } }
        /// <summary>Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.</summary>
        [GlyphData(30, 43356, 57902, "Glyph of Scare Beast", GlyphType.Minor,
            @"Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.")]
        public bool GlyphOfScareBeast { get { return _glyphData[30]; } set { _glyphData[30] = value; } }
        #endregion
    }

}
