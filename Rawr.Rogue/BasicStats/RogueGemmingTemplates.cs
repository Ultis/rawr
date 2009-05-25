using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue.BasicStats
{
    public class RogueGemmingTemplates
    {
        public static readonly List<GemmingTemplate> List;

        static RogueGemmingTemplates()
        {
            ////Relevant Gem IDs for Rogues
            //Red
            int[] bold = { 39900, 39996, 40111, 42142 };
            int[] delicate = { 39905, 39997, 40112, 42143 };

            //Purple
            int[] shifting = { 39935, 40023, 40130 };
            int[] sovereign = { 39934, 40022, 40129 };

            //Blue
            int[] solid = { 39919, 40008, 40119, 36767 };

            //Green
            int[] enduring = { 39976, 40089, 40167 };

            //Yellow
            int[] thick = { 39916, 40015, 40126, 42157 };

            //Orange
            int[] etched = { 39948, 40038, 40143 };
            int[] fierce = { 39951, 40041, 40146 };
            int[] glinting = { 39953, 40044, 40148 };
            int[] stalwart = { 39964, 40056, 40160 };
            int[] deadly = { 39952, 40043, 40147 };

            //Meta
            // int austere = 41380;
            int relentless = 41398;

            List = new List<GemmingTemplate>
				{
					new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Max Agility
						RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Uncommon", //Agi/Crit
						RedId = delicate[0], YellowId = deadly[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Rare", Enabled = true, //Max Agility
						RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Rare", Enabled = true, //Agi/Crit 
						RedId = delicate[1], YellowId = deadly[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Epic", //Max Agility
						RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Epic", //Agi/Crit 
						RedId = delicate[2], YellowId = deadly[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Max Agility
						RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless },
					new GemmingTemplate { Model = "Rogue", Group = "Jeweler", //Agility Heavy
						RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless },
				};
        }
    }
}
