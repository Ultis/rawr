using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    class Gemming
    {
        ////Relevant Gem IDs for Enhancement Shamans
        //Red
        private int[] delicate = { 39905, 39997, 40112, 42143 };  // Agility
        private int[] bright = { 39906, 39999, 40114, 36766 };    // Attack Power
        private int[] precise = { 39910, 40003, 40118, 42154 };   // Expertise
        private int[] fractured = { 39909, 40002, 40117, 42153 }; // Armour Penetration

        //Yellow
        private int[] rigid = { 39915, 40014, 40125, 42156 };  // Hit
        private int[] smooth = { 39914, 40013, 40124, 42149 }; // Crit
        private int[] quick = { 39918, 40017, 40128, 42150 };  // Haste

        //Orange
        private int[] deadly = { 39952, 40043, 40147 };   // Agi/Crit
        private int[] glinting = { 39953, 40044, 40148 }; // Agi/Hit
        private int[] deft = { 39955, 40046, 40150 };     // Agi/Haste
        private int[] wicked = { 39960, 40052, 40156 };   // AP/Crit
        private int[] pristine = { 39961, 40053, 40157 }; // AP/Hit
        private int[] stark = { 39963, 40055, 40159 };    // AP/Haste
        private int[] accurate = { 39966, 40058, 40162 }; // Exp/Hit

        //Purple
        private int[] guardian = { 39940, 40034, 40141 }; // Exp/Sta
        private int[] balanced = { 39937, 40029, 40136 }; // Ap/Sta
        private int[] shifting = { 39935, 40023, 40130 }; // Agi/Sta
        private int[] puissant = { 39933, 40033, 40140 }; // ArP/Sta

        //Green
        private int[] vivid = { 39975, 40088, 40166 };    // Hit/Sta
        private int[] jagged = { 39974, 40086, 40165 };   // Crit/Sta
        private int[] forceful = { 39978, 40091, 40169 }; // Haste/Sta

        public List<GemmingTemplate> addTemplates(String group, int rarity, int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() { 
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled, //Max Expertise
					RedId = precise[rarity], YellowId = accurate[rarity], BlueId = guardian[rarity], PrismaticId = precise[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red exp
					RedId = accurate[rarity], YellowId = rigid[rarity], BlueId = vivid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red agi
					RedId = glinting[rarity], YellowId = rigid[rarity], BlueId = vivid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Hit - red ap
					RedId = pristine[rarity], YellowId = rigid[rarity], BlueId = vivid[rarity], PrismaticId = rigid[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Attack Power - yellow hit
					RedId = bright[rarity], YellowId = pristine[rarity], BlueId = balanced[rarity], PrismaticId = bright[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Attack Power - yellow haste
					RedId = bright[rarity], YellowId = stark[rarity], BlueId = balanced[rarity], PrismaticId = bright[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Attack Power - yellow crit
					RedId = bright[rarity], YellowId = wicked[rarity], BlueId = balanced[rarity], PrismaticId = bright[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow hit
					RedId = delicate[rarity], YellowId = glinting[rarity], BlueId = shifting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow haste
					RedId = delicate[rarity], YellowId = deft[rarity], BlueId = shifting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Agility - yellow crit
					RedId = delicate[rarity], YellowId = deadly[rarity], BlueId = shifting[rarity], PrismaticId = delicate[rarity], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - red agi
					RedId = deadly[rarity], YellowId = smooth[rarity], BlueId = jagged[rarity], PrismaticId = smooth[rarity], MetaId = metagem },
            	new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Crit - red ap
					RedId = wicked[rarity], YellowId = smooth[rarity], BlueId = jagged[rarity], PrismaticId = smooth[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red agi
					RedId = deft[rarity], YellowId = quick[rarity], BlueId = forceful[rarity], PrismaticId = quick[rarity], MetaId = metagem },
                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Haste - red ap
					RedId = stark[rarity], YellowId = quick[rarity], BlueId = forceful[rarity], PrismaticId = quick[rarity], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = group, Enabled = enabled,  //Max Armour Penetration
					RedId = fractured[rarity], YellowId = fractured[rarity], BlueId = puissant[rarity], PrismaticId = fractured[rarity], MetaId = metagem },
            };
        }

        public List<GemmingTemplate> addJewelerTemplates(int metagem, bool enabled)
        {
            return new List<GemmingTemplate>() { 
            	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Expertise
					RedId = precise[3], YellowId = precise[3], BlueId = precise[3], PrismaticId = precise[3], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Hit
					RedId = rigid[3], YellowId = rigid[3], BlueId = rigid[3], PrismaticId = rigid[3], MetaId = metagem },

            	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Attack Power
					RedId = bright[3], YellowId = bright[3], BlueId = bright[3], PrismaticId = bright[3], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Agility
					RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = metagem },

                new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Crit
					RedId = smooth[3], YellowId = smooth[3], BlueId = smooth[3], PrismaticId = smooth[3], MetaId = metagem },

              	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Haste
					RedId = quick[3], YellowId = quick[3], BlueId = quick[3], PrismaticId = quick[3], MetaId = metagem },

              	new GemmingTemplate() { Model = "Enhance", Group = "Jeweler", Enabled = enabled, //Max Armour Penetration
					RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = metagem },
             };
        }
    }
}
