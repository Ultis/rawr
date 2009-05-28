using System.Collections.Generic;

namespace Rawr.Rogue.ClassAbilities
{
    public class Glyphs
    {
        private static RogueTalents _talents;

        public static bool GlyphOfBackstab { get { return _talents.GlyphOfBackstab; } }
        public static bool GlyphOfEviscerate { get { return _talents.GlyphOfEviscerate; } }
        public static bool GlyphOfMutilate { get { return _talents.GlyphOfMutilate; } }
        public static bool GlyphOfHungerforBlood { get { return _talents.GlyphOfHungerforBlood; } }
        public static bool GlyphOfKillingSpree { get { return _talents.GlyphOfKillingSpree; } }
        public static bool GlyphOfVigor { get { return _talents.GlyphOfVigor; } }
        public static bool GlyphOfFanOfKnives { get { return _talents.GlyphOfFanOfKnives; } }
        public static bool GlyphOfExposeArmor { get { return _talents.GlyphOfExposeArmor; } }
        public static bool GlyphOfSinisterStrike { get { return _talents.GlyphOfSinisterStrike; } }
        public static bool GlyphOfSliceandDice { get { return _talents.GlyphOfSliceandDice; } }
        public static bool GlyphOfFeint { get { return _talents.GlyphOfFeint; } }
        public static bool GlyphOfGhostlyStrike { get { return _talents.GlyphOfGhostlyStrike; } }
        public static bool GlyphOfRupture { get { return _talents.GlyphOfRupture; } }
        public static bool GlyphOfBladeFlurry { get { return _talents.GlyphOfBladeFlurry; } }
        public static bool GlyphOfAdrenalineRush { get { return _talents.GlyphOfAdrenalineRush; } }

        public static void Initialize(RogueTalents talents)
        {
            _talents = talents;
        }
    }

    public class ModeledGlyphs 
    {
        public static List<string> List;
        static ModeledGlyphs()
        {
            List = new List<string> 
                        {
                            //"Glyph of Backstab",
                            "Glyph of Eviscerate", 
                            "Glyph of Mutilate", 
                            "Glyph of Hunger for Blood", 
                            //"Glyph of Killing Spree",
                            //"Glyph of Vigor",
                            //"Glyph of Fan of Knives",
                            //"Glyph of Expose Armor",
                            "Glyph of Sinister Strike", 
                            "Glyph of Slice and Dice", 
                            "Glyph of Feint", 
                            //"Glyph of Ghostly Strike",
                            //"Glyph of Rupture",
                            "Glyph of Blade Flurry", 
                            "Glyph of Adrenaline Rush"
                        };
        }
    }
}