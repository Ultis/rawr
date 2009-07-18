using System.Collections.Generic;

namespace Rawr.Rogue.BasicStats
{
    public class RelevantItems : List<ItemType>
    {
        public static readonly List<ItemType> List;

        static RelevantItems()
        {
            List = new List<ItemType>(new[]
                                               {
                                                   ItemType.None,
                                                   ItemType.Leather,
                                                   ItemType.Bow,
                                                   ItemType.Crossbow,
                                                   ItemType.Gun,
                                                   ItemType.Thrown,
                                                   ItemType.Dagger,
                                                   ItemType.FistWeapon,
                                                   ItemType.OneHandMace,
                                                   ItemType.OneHandSword
                                               });             
        }
    }
}
