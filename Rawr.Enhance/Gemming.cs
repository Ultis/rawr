using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class Gemming
    {
        ////Relevant Gem IDs for Enhancement Shamans
        // { Uncommon, Rare, Epic, Jeweler }

        //Red
        private int[] delicate  = { 52082, 52212, 71879, 52258 };         // Agility
        private int[] precise   = { 52085, 52230, 71880, 52260 };         // Expertise

        //Yellow
        private int[] fractured = { 52094, 52219, 71877, 52269 };         // Mastery
        private int[] smooth    = { 52091, 52241, 71874, 52266 };         // Crit
        private int[] quick     = { 52093, 52232, 71876, 52268 };         // Haste

        //Orange (R&Y)
        private int[] deadly    = { 52109, 52209, 71840, 52209 };         // Agi/Crit
        private int[] deft      = { 52112, 52211, 71848, 52211 };         // Agi/Haste
        private int[] adept     = { 52115, 52204, 71852, 52204 };         // Agi/Mast
        private int[] keen      = { 52118, 52224, 71853, 52224 };         // Exp/Mast

        //Blue
        private int[] rigid     = { 52089, 52235, 71817, 52264 };         // Hit

        //Purple (B&R)
        private int[] glinting  = { 52102, 52220, 71862, 52220 };         // Agi/Hit
        private int[] accurate  = { 52105, 52203, 71863, 52203 };         // Exp/Hit
        private int[] shifting  = { 52096, 52238, 71869, 52238 };         // Agi/Sta
        private int[] guardian  = { 52099, 52221, 71870, 52221 };         // Exp/Sta

        //Green (B&Y)
        private int[] lightning = { 52125, 52225, 71824, 52225 };         // Haste/Hit
        private int[] senseis   = { 52128, 52237, 71825, 52237 };         // Mast/Hit
        private int[] piercing  = { 52122, 52228, 71823, 52228 };         // Crit/Hit
        private int[] forceful  = { 52124, 52218, 71836, 52218 };         // Haste/Sta
        private int[] puissant  = { 52126, 52231, 71838, 52231 };         // Mast/Sta
        private int[] jagged    = { 52121, 52223, 71834, 52223 };         // Crit/Sta

        //Prismatic
        private int[] tear      = { 42701, 42702, 49110, 49110 };         // +X to all stats

        //Cogwheel
        private int[] cog_fractured = { 59480 };                          // Mastery
        private int[] cog_precise   = { 59489 };                          // Expertise
        private int[] cog_quick     = { 59479 };                          // Haste
        private int[] cog_rigid     = { 59493 };                          // Hit
        private int[] cog_smooth    = { 59478 };                          // Crit

        public List<GemmingTemplate> addTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() 
            {

            	/*new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, // Stats
					RedId = tear[rarity], YellowId = tear[rarity], BlueId = tear[rarity], PrismaticId = tear[rarity], MetaId = metagem },*/

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Max Expertise - colour match
					RedId = precise[rarity], YellowId = keen[rarity], BlueId = accurate[rarity], PrismaticId = precise[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Max Expertise - no colour match
					RedId = precise[rarity], YellowId = precise[rarity], BlueId = precise[rarity], PrismaticId = precise[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red exp, yellow mast
					RedId = accurate[rarity], YellowId = senseis[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red agi, yellow mast
					RedId = glinting[rarity], YellowId = senseis[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - no colour match
					RedId = rigid[rarity], YellowId = rigid[rarity], BlueId = rigid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow mast
					RedId = delicate[rarity], YellowId = adept[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow haste
					RedId = delicate[rarity], YellowId = deft[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow crit
					RedId = delicate[rarity], YellowId = deadly[rarity], BlueId = glinting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - no colour match
					RedId = delicate[rarity], YellowId = delicate[rarity], BlueId = delicate[rarity], PrismaticId = delicate[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - red agi, blue hit
					RedId = deadly[rarity], YellowId = smooth[rarity], BlueId = piercing[rarity], PrismaticId = smooth[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - no colour match
					RedId = smooth[rarity], YellowId = smooth[rarity], BlueId = smooth[rarity], PrismaticId = smooth[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red agi, blue hit
					RedId = deft[rarity], YellowId = quick[rarity], BlueId = lightning[rarity], PrismaticId = quick[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - no colour match
					RedId = quick[rarity], YellowId = quick[rarity], BlueId = quick[rarity], PrismaticId = quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red agi, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red exp, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - no colour match
					RedId = fractured[rarity], YellowId = fractured[rarity], BlueId = fractured[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
            };
        }

        public List<GemmingTemplate> addCogwheelTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>()
            {
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_rigid[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_precise[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_rigid[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_smooth[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_fractured[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,
                    CogwheelId = cog_smooth[rarity], Cogwheel2Id = cog_quick[rarity], MetaId = metagem },
            };
        }
    }
}
