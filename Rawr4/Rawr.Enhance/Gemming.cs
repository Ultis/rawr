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
        private int[] delicate = { 52082, 52212, 0, 52258 };   // Agility
        private int[] precise = { 52085, 52230, 0, 52260 };    // Expertise

        //Yellow
        private int[] fractured = { 52094, 52219, 0, 52269 };  // Mastery
        private int[] smooth = { 52091, 52241, 0, 52266 };     // Crit
        private int[] quick = { 52093, 52232, 0, 52268 };     // Haste

        //Orange (R&Y)
        private int[] deadly = { 52109, 52209, 0, 0 };    // Agi/Crit
        private int[] deft = { 52112, 52211, 0, 0 };      // Agi/Haste
        private int[] adept = { 52115, 52204, 0, 0 };     // Agi/Mast
        private int[] keen = { 52118, 52224, 0, 0 };      // Exp/Mast

        //Blue
        private int[] rigid = { 52089, 52235, 0, 52264 };     // Hit

        //Purple (B&R)
        private int[] glinting = { 52102, 52220, 0, 0 }; // Agi/Hit
        private int[] accurate = { 52105, 52203, 0, 0 }; // Exp/Hit
        private int[] guardian = { 0, 0, 0, 0 }; // Exp/Sta
        private int[] shifting = { 52096, 52238, 0, 0 }; // Agi/Sta

        //Green (B&Y)
        private int[] lightning = { 52125, 52225, 0, 0 }; // Haste/Hit
        private int[] piercing = { 52122, 52228, 0, 0 }; // Crit/Hit
        private int[] senseis = { 52128, 52237, 0, 0 }; // Mast/Hit
        private int[] puissant = { 52126, 52231, 0, 0 };    // Mast/Sta
        private int[] jagged = { 0, 0, 0, 0 };   // Crit/Sta
        private int[] forceful = { 52124, 52218, 0, 0 }; // Haste/Sta

        //Prismatic
        private int[] tear = { 0, 0, 0, 0 }; // +X to all stats

        public List<GemmingTemplate> addTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() { 
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Temp
                    RedId = 40112, YellowId = 40112, BlueId = 40112, PrismaticId = 49110, MetaId = 41398 },

            	/*new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, // Stats
					RedId = tear[rarity], YellowId = tear[rarity], BlueId = tear[rarity], PrismaticId = tear[rarity], MetaId = metagem },

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
                //new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - red agi, blue stam
				//	  RedId = deadly[rarity], YellowId = smooth[rarity], BlueId = jagged[rarity], PrismaticId = smooth[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - no colour match
					RedId = smooth[rarity], YellowId = smooth[rarity], BlueId = smooth[rarity], PrismaticId = smooth[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red agi, blue hit
					RedId = deft[rarity], YellowId = quick[rarity], BlueId = lightning[rarity], PrismaticId = quick[rarity], MetaId = metagem },
                //new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red agi, blue stam
				//	  RedId = deft[rarity], YellowId = quick[rarity], BlueId = forceful[rarity], PrismaticId = quick[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - no colour match
					RedId = quick[rarity], YellowId = quick[rarity], BlueId = quick[rarity], PrismaticId = quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red agi, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - red exp, blue hit
					RedId = adept[rarity], YellowId = fractured[rarity], BlueId = senseis[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Mastery - no colour match
					RedId = fractured[rarity], YellowId = fractured[rarity], BlueId = fractured[rarity], PrismaticId = fractured[rarity], MetaId = metagem },*/
            };
        }
    }
}
