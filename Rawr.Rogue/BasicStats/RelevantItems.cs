using System.Collections.Generic;

namespace Rawr.Rogue.BasicStats
{
    public class RelevantItems : List<Item.ItemType>
    {
        public static readonly List<Item.ItemType> List;

        static RelevantItems()
        {
            List = new List<Item.ItemType>(new[]
                                               {
                                                   Item.ItemType.None,
                                                   Item.ItemType.Leather,
                                                   Item.ItemType.Bow,
                                                   Item.ItemType.Crossbow,
                                                   Item.ItemType.Gun,
                                                   Item.ItemType.Thrown,
                                                   Item.ItemType.Dagger,
                                                   Item.ItemType.FistWeapon,
                                                   Item.ItemType.OneHandMace,
                                                   Item.ItemType.OneHandSword
                                               });             
        }
    }
}
