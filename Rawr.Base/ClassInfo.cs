using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Rawr
{
    public class RoleInfo
    {
        public readonly ClassInfo Class;

        public readonly CharacterRole Role;
        public readonly AdditiveStat PrimaryStat;
        public readonly int SpecMask;

        public readonly AdditiveStat UselessPrimaryStat1;
        public readonly AdditiveStat UselessPrimaryStat2;
        public readonly bool[] UsefulItemType;
        public readonly ItemType[] UsefulItemTypes;

        internal RoleInfo(CharacterRole role, AdditiveStat primaryStat, int specMask, ClassInfo cls = null)
        {
            Role = role;
            PrimaryStat = primaryStat;
            SpecMask = specMask;
            Class = cls;

            switch (PrimaryStat)
            {
                case AdditiveStat.Strength:
                    UselessPrimaryStat1 = AdditiveStat.Agility;
                    UselessPrimaryStat2 = AdditiveStat.Intellect;
                    break;
                case AdditiveStat.Agility:
                    UselessPrimaryStat1 = AdditiveStat.Intellect;
                    UselessPrimaryStat2 = AdditiveStat.Strength;
                    break;
                case AdditiveStat.Intellect:
                    UselessPrimaryStat1 = AdditiveStat.Strength;
                    UselessPrimaryStat2 = AdditiveStat.Agility;
                    break;
            }

            if (cls != null)
            {
                bool oneHanded = true;
                if (role == CharacterRole.MeleeDPS)
                    oneHanded = Class.CanDualWield;

                bool twoHanded = true;
                if (role == CharacterRole.Tank)
                {
                    foreach (ItemType type in Class.UsableItemTypes)
                    {
                        if (type == ItemType.Shield)
                        {
                            twoHanded = false;
                            break;
                        }
                    }
                }

                List<ItemType> types = new List<ItemType>();
                bool[] mask = new bool[EnumHelper.GetRange(typeof(ItemType))];

                foreach (ItemType type in Class.UsableItemTypes)
                {
                    bool ok = true;
                    switch (type)
                    {
                        case ItemType.Cloth:
                        case ItemType.Leather:
                        case ItemType.Mail:
                        case ItemType.Plate:
                            ok = type == Class.OptimalArmorType;
                            break;
                        case ItemType.Dagger:
                        case ItemType.FistWeapon:
                        case ItemType.OneHandAxe:
                        case ItemType.OneHandMace:
                        case ItemType.OneHandSword:
                            ok = oneHanded;
                            break;
                        case ItemType.Polearm:
                        case ItemType.Staff:
                        case ItemType.TwoHandAxe:
                        case ItemType.TwoHandMace:
                        case ItemType.TwoHandSword:
                            ok = twoHanded;
                            break;
                        case ItemType.Shield:
                            ok = role != CharacterRole.MeleeDPS;
                            break;
                    }
                    if (ok)
                    {
                        types.Add(type);
                        mask[(int)type] = true;
                    }
                }

                UsefulItemType = mask;
                UsefulItemTypes = types.ToArray();
            }
        }
    }

    public class ClassInfo
    {
        public readonly CharacterClass Class;
        public readonly Color Color;
        public readonly ItemType OptimalArmorType;
        public readonly bool CanDualWield;
        public readonly bool[] AllowedRace;
        public readonly bool[] UsableItemType;
        public readonly ItemType[] UsableItemTypes;
        public readonly RoleInfo[] Roles;

        private ClassInfo(CharacterClass cls, Color color, ItemType optimalArmorType, bool canDualWield, CharacterRace[] disallowedRaces, ItemType[] usableItemTypes, RoleInfo[] roles)
        {
            Class = cls;
            Color = color;
            OptimalArmorType = optimalArmorType;
            CanDualWield = canDualWield;

            AllowedRace = new bool[EnumHelper.GetRange(typeof(CharacterRace))];
            foreach(CharacterRace race in EnumHelper.GetValues<CharacterRace>())
            {
                AllowedRace[(int)race] = true;
            }
            foreach(CharacterRace race in disallowedRaces)
            {
                AllowedRace[(int)race] = false;
            }

            List<ItemType> usableItemTypesList = new List<ItemType>();
            usableItemTypesList.Add(ItemType.None);
            if(optimalArmorType >= ItemType.Cloth)
                usableItemTypesList.Add(ItemType.Cloth);
            if (optimalArmorType >= ItemType.Leather)
                usableItemTypesList.Add(ItemType.Leather);
            if (optimalArmorType >= ItemType.Mail)
                usableItemTypesList.Add(ItemType.Mail);
            if (optimalArmorType >= ItemType.Plate)
                usableItemTypesList.Add(ItemType.Plate);
            usableItemTypesList.AddRange(usableItemTypes);
            UsableItemTypes = usableItemTypesList.ToArray();

            UsableItemType = new bool[EnumHelper.GetRange(typeof(ItemType))];
            foreach (ItemType type in UsableItemTypes)
                UsableItemType[(int)type] = true;

            Roles = new RoleInfo[5];
            foreach (RoleInfo role in roles)
            {
                Roles[(int)role.Role] = new RoleInfo(role.Role, role.PrimaryStat, role.SpecMask, this);
            }
        }

        private static ClassInfo[] InitClasses(ClassInfo[] inv)
        {
            ClassInfo[] outv = new ClassInfo[EnumHelper.GetRange(typeof(CharacterClass))];
            foreach(ClassInfo cls in inv)
            {
                outv[(int)cls.Class] = cls;
            }
            return outv;
        }

        public static readonly ClassInfo[] Classes = InitClasses(new ClassInfo[] {
            new ClassInfo(CharacterClass.Druid, Color.FromRgb(255, 125, 10), ItemType.Leather, false,
                new CharacterRace[] {
                    CharacterRace.Draenei,
                    CharacterRace.Dwarf,
                    CharacterRace.Gnome,
                    CharacterRace.Human,
                    CharacterRace.PandarenAlliance,

                    CharacterRace.BloodElf,
                    CharacterRace.Orc,
                    CharacterRace.Undead,
                    CharacterRace.Goblin,
                    CharacterRace.PandarenHorde,
                },
                new ItemType[] {
                    ItemType.Dagger,
                    ItemType.Staff,
                    ItemType.FistWeapon,
                    ItemType.OneHandMace,
                    ItemType.TwoHandMace,
                    ItemType.Polearm,
                    ItemType.Relic,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Intellect, 1),
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Agility, 2),
                    new RoleInfo(CharacterRole.Healer, AdditiveStat.Intellect, 4),
                    new RoleInfo(CharacterRole.Tank, AdditiveStat.Agility, 2),
                }
            ),
            new ClassInfo(CharacterClass.Hunter, Color.FromRgb(171, 212, 115), ItemType.Mail, true,
                new CharacterRace[] {
                    CharacterRace.Gnome
                },
                new ItemType[] {
                    ItemType.Bow,
                    ItemType.Crossbow,
                    ItemType.Gun,
                    ItemType.FistWeapon,
                    ItemType.Dagger,
                    ItemType.OneHandAxe,
                    ItemType.OneHandSword,
                    ItemType.Polearm,
                    ItemType.Staff,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandSword,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Agility, 7),
                }
            ),
            new ClassInfo(CharacterClass.Mage, Color.FromRgb(105, 204, 240), ItemType.Cloth, false,
                new CharacterRace[] {
                    CharacterRace.Tauren
                },
                new ItemType[] {
                    ItemType.Dagger,
                    ItemType.OneHandSword,
                    ItemType.Staff,
                    ItemType.Wand,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Intellect, 7),
                }
            ),
            new ClassInfo(CharacterClass.Paladin, Color.FromRgb(245, 140, 186), ItemType.Plate, false,
                new CharacterRace[] {
                    CharacterRace.NightElf,
                    CharacterRace.Gnome,
                    CharacterRace.Worgen,
                    CharacterRace.PandarenAlliance,

                    CharacterRace.Orc,
                    CharacterRace.Undead,
                    CharacterRace.Troll,
                    CharacterRace.Goblin,
                    CharacterRace.PandarenHorde,
                },
                new ItemType[] {
                    ItemType.OneHandAxe,
                    ItemType.OneHandMace,
                    ItemType.OneHandSword,
                    ItemType.Polearm,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandMace,
                    ItemType.TwoHandSword,
                    ItemType.Relic,
                    ItemType.Shield,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Strength, 4),
                    new RoleInfo(CharacterRole.Healer, AdditiveStat.Intellect, 1),
                    new RoleInfo(CharacterRole.Tank, AdditiveStat.Strength, 2),
                }
            ),
            new ClassInfo(CharacterClass.Priest, Color.FromRgb(255, 255, 255), ItemType.Cloth, false,
                new CharacterRace[] {
                    CharacterRace.Orc
                },
                new ItemType[] {
                    ItemType.Dagger,
                    ItemType.OneHandMace,
                    ItemType.Staff,
                    ItemType.Wand,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Intellect, 4),
                    new RoleInfo(CharacterRole.Healer, AdditiveStat.Intellect, 3),
                }
            ),
            new ClassInfo(CharacterClass.Rogue, Color.FromRgb(255, 245, 105), ItemType.Leather, true,
                new CharacterRace[] {
                    CharacterRace.Draenei,
                    CharacterRace.Tauren
                },
                new ItemType[] {
                    ItemType.Bow,
                    ItemType.Crossbow,
                    ItemType.Gun,
                    ItemType.Thrown,
                    ItemType.Dagger,
                    ItemType.FistWeapon,
                    ItemType.OneHandAxe,
                    ItemType.OneHandMace,
                    ItemType.OneHandSword
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Agility, 7),
                }
            ),
            new ClassInfo(CharacterClass.Shaman, Color.FromRgb(0, 112, 222), ItemType.Mail, true,
                new CharacterRace[] {
                    CharacterRace.Human,
                    CharacterRace.NightElf,
                    CharacterRace.Gnome,
                    CharacterRace.Worgen,

                    CharacterRace.Undead,
                    CharacterRace.BloodElf,
                },
                new ItemType[] {
                    ItemType.Dagger,
                    ItemType.FistWeapon,
                    ItemType.OneHandAxe,
                    ItemType.OneHandMace,
                    ItemType.Staff,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandMace,
                    ItemType.Relic,
                    ItemType.Shield,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Intellect, 1),
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Agility, 2),
                    new RoleInfo(CharacterRole.Healer, AdditiveStat.Intellect, 4),
                }
            ),
            new ClassInfo(CharacterClass.Warlock, Color.FromRgb(148, 130, 201), ItemType.Cloth, false,
                new CharacterRace[] {
                    CharacterRace.NightElf,
                    CharacterRace.Draenei,
                    CharacterRace.PandarenAlliance,

                    CharacterRace.Tauren,
                    CharacterRace.PandarenHorde,
                },
                new ItemType[] {
                    ItemType.Dagger,
                    ItemType.OneHandSword,
                    ItemType.Staff,
                    ItemType.Wand,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.RangedDPS, AdditiveStat.Intellect, 7),
                }
            ),
            new ClassInfo(CharacterClass.Warrior, Color.FromRgb(199, 156, 110), ItemType.Plate, true,
                new CharacterRace[0],
                new ItemType[] {
                    ItemType.Bow,
                    ItemType.Crossbow,
                    ItemType.Gun,
                    ItemType.Thrown,
                    ItemType.Dagger,
                    ItemType.FistWeapon,
                    ItemType.OneHandMace,
                    ItemType.OneHandSword,
                    ItemType.OneHandAxe,
                    ItemType.Polearm,
                    ItemType.Staff,
                    ItemType.TwoHandMace,
                    ItemType.TwoHandSword,
                    ItemType.TwoHandAxe,
                    ItemType.Shield,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Strength, 3),
                    new RoleInfo(CharacterRole.Tank, AdditiveStat.Strength, 4),
                }
            ),
            new ClassInfo(CharacterClass.DeathKnight, Color.FromRgb(196, 130, 59), ItemType.Plate, true,
                new CharacterRace[] {
                    CharacterRace.PandarenAlliance,

                    CharacterRace.PandarenHorde,
                },
                new ItemType[] {
                    ItemType.Polearm,
                    ItemType.TwoHandAxe,
                    ItemType.TwoHandMace,
                    ItemType.TwoHandSword,
                    ItemType.OneHandAxe,
                    ItemType.OneHandMace,
                    ItemType.OneHandSword,
                    ItemType.Relic,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Strength, 6),
                    new RoleInfo(CharacterRole.Tank, AdditiveStat.Strength, 1),
                }
            ),
            new ClassInfo(CharacterClass.Monk, Color.FromRgb(0, 132, 103), ItemType.Leather, false,
                new CharacterRace[] {
                    CharacterRace.Worgen,

                    CharacterRace.Goblin,
                },
                new ItemType[] { // copy from Druid
                    ItemType.Dagger,
                    ItemType.Staff,
                    ItemType.FistWeapon,
                    ItemType.OneHandMace,
                    ItemType.TwoHandMace,
                    ItemType.Polearm,
                    ItemType.Relic,
                },
                new RoleInfo[] {
                    new RoleInfo(CharacterRole.MeleeDPS, AdditiveStat.Agility, 4),
                    new RoleInfo(CharacterRole.Healer, AdditiveStat.Intellect, 2),
                    new RoleInfo(CharacterRole.Tank, AdditiveStat.Agility, 1),
                }
            ),
        });
    }
}
