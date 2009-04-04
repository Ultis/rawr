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
}
