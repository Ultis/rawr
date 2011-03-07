using Rawr;

namespace Tests.Rawr.ShadowPriest
{
    public class TestItemSets
    {
        #region Static Methods

        public static void LoadItemSet1(Character character)
        {
            ItemCacheInstance itemCache = new ItemCacheInstance();
            ItemCache.Instance = itemCache;

            itemCache.AddItem(new Item
                                  {
                                      Id = 63855,
                                      Stats = new Stats
                                                  {
                                                      Armor = 916,
                                                      Stamina = 349,
                                                      Intellect = 233,
                                                      CritRating = 151,
                                                      HasteRating = 158,
                                                  }
                                  });
            character.Head = new ItemInstance(63855, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 63797,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 195,
                                                      Intellect = 130,
                                                      HitRating = 56,
                                                      HasteRating = 36,
                                                      MasteryRating = 78,
                                                  }
                                  });
            character.Neck = new ItemInstance(63797, 0, 0, 0, 0, 0, 82, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 63873,
                                      Stats = new Stats
                                                  {
                                                      Armor = 846,
                                                      Stamina = 260,
                                                      Intellect = 173,
                                                      HitRating = 69,
                                                      CritRating = 46,
                                                      HasteRating = 115,
                                                  }
                                  });
            character.Shoulders = new ItemInstance(63873, 0, 0, 0, 0, 0, 81, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 56107,
                                      Stats = new Stats
                                                  {
                                                      Armor = 572,
                                                      Stamina = 224,
                                                      Intellect = 149,
                                                      Spirit = 100,
                                                      HasteRating = 100,
                                                  }
                                  });
            character.Back = new ItemInstance(56107, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 63786,
                                      Stats = new Stats
                                                  {
                                                      Armor = 1128,
                                                      Stamina = 349,
                                                      Intellect = 233,
                                                      Spirit = 173,
                                                      CritRating = 125,
                                                  }
                                  });
            character.Chest = new ItemInstance(63786, 0, 0, 0, 0, 0, 89, 0);


            ItemCache.AddItem(new Item
                                  {
                                      Id = 63826,
                                      Stats = new Stats
                                                  {
                                                      Armor = 493,
                                                      Stamina = 195,
                                                      Intellect = 130,
                                                      HitRating = 76,
                                                      HasteRating = 93,
                                                  }
                                  });
            character.Wrist = new ItemInstance(63826, 0, 0, 0, 0, 0, 81, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 63814,
                                      Stats = new Stats
                                                  {
                                                      Armor = 705,
                                                      Stamina = 260,
                                                      Intellect = 173,
                                                      HitRating = 98,
                                                      CritRating = 125,
                                                  }
                                  });
            character.Hands = new ItemInstance(63814, 0, 0, 0, 0, 0, 89, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 63921,
                                      Stats = new Stats
                                                  {
                                                      Armor = 664,
                                                      Stamina = 298,
                                                      Intellect = 199,
                                                      CritRating = 101,
                                                      MasteryRating = 151,
                                                  }
                                  });
            character.Waist = new ItemInstance(63921, 0, 0, 0, 0, 0, 111, 0);


            ItemCache.AddItem(new Item
                                  {
                                      Id = 56218,
                                      Stats = new Stats
                                                  {
                                                      Armor = 1002,
                                                      Stamina = 401,
                                                      Intellect = 268,
                                                      Spirit = 178,
                                                      MasteryRating = 157,
                                                  }
                                  });
            character.Legs = new ItemInstance(56218, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 66937,
                                      Stats = new Stats
                                                  {
                                                      Armor = 787,
                                                      Stamina = 298,
                                                      Intellect = 199,
                                                      HitRating = 142,
                                                      CritRating = 116
                                                  }
                                  });
            character.Feet = new ItemInstance(66937, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 55276,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 177,
                                                      Intellect = 118,
                                                      Spirit = 85,
                                                      MasteryRating = 69,
                                                  }
                                  });
            character.Finger1 = new ItemInstance(55276, 0, 0, 0, 0, 0, 111, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 56000,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 224,
                                                      Intellect = 149,
                                                      Spirit = 107,
                                                      CritRating = 87
                                                  }
                                  });
            character.Finger2 = new ItemInstance(56000, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 57346,
                                      Stats = new Stats
                                                  {
                                                      Intellect = 143,
                                                  }
                                  });
            character.Trinket1 = new ItemInstance(57346, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 61411,
                                      Stats = new Stats
                                                  {
                                                      Intellect = 194,
                                                  }
                                  });
            character.Trinket2 = new ItemInstance(61411, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 56097,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 401,
                                                      Intellect = 268,
                                                      Spirit = 178,
                                                      SpellPower = 1533,
                                                      HasteRating = 178
                                                  }
                                  });
            character.MainHand = new ItemInstance(56097, 0, 0, 0, 0, 0, 0, 0);

            ItemCache.AddItem(new Item
                                  {
                                      Id = 56122,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 126,
                                                      Intellect = 84,
                                                      Spirit = 56,
                                                      HasteRating = 56
                                                  }
                                  });
            character.Ranged = new ItemInstance(56122, 0, 0, 0, 0, 0, 0, 0);
        }

        public static void LoadItemSet2(Character character)
        {
            int head = 1;
            int neck = 2;
            int shoulder = 3;
            int back = 4;
            int chest = 5;
            int wrist = 6;
            int hands = 7;
            int waist = 8;
            int legs = 9;
            int feet = 10;
            int finger1 = 11;
            int finger2 = 12;
            int trinket1 = 13;
            int trinket2 = 14;
            int mainhand = 15;
            int offhand = 16;
            int ranged = 17;

            ItemCacheInstance itemCache = new ItemCacheInstance();
            ItemCache.Instance = itemCache;

            itemCache.AddItem(new Item
                                  {
                                      Id = head,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Head,
                                      SocketColor1 = ItemSlot.Meta,
                                      SocketColor2 = ItemSlot.Blue,
                                      SocketBonus = new Stats
                                                        {
                                                            Intellect = 30,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 942,
                                                      Stamina = 454,
                                                      Intellect = 242,
                                                      Spirit = 182,
                                                      HasteRating = 162,
                                                  }
                                  }
                );

            itemCache.AddItem(new Item
                                  {
                                      Id = neck,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Neck,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 112,
                                                      CritRating = 112,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = shoulder,
                                      ItemLevel = 346,
                                      SocketColor1 = ItemSlot.Blue,
                                      SocketBonus = new Stats
                                                        {
                                                            Intellect = 10,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 870,
                                                      Stamina = 337,
                                                      Intellect = 205,
                                                      Spirit = 150,
                                                      CritRating = 130,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = back,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Back,
                                      Stats = new Stats
                                                  {
                                                      Armor = 580,
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 112,
                                                      HasteRating = 112,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = chest,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Chest,
                                      SocketColor1 = ItemSlot.Red,
                                      SocketColor2 = ItemSlot.Yellow,
                                      SocketBonus = new Stats
                                                        {
                                                            Spirit = 20,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 1160,
                                                      Stamina = 454,
                                                      Intellect = 262,
                                                      Spirit = 202,
                                                      CritRating = 162,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = wrist,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Wrist,
                                      Stats = new Stats
                                                  {
                                                      Armor = 507,
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 112,
                                                      CritRating = 112,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = hands,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Hands,
                                      SocketColor1 = ItemSlot.Yellow,
                                      SocketBonus = new Stats
                                                        {
                                                            Spirit = 10,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 725,
                                                      Stamina = 337,
                                                      Intellect = 205,
                                                      Spirit = 130,
                                                      HasteRating = 150,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = waist,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Waist,
                                      SocketColor1 = ItemSlot.Yellow,
                                      SocketBonus = new Stats
                                                        {
                                                            HasteRating = 10,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 652,
                                                      Stamina = 337,
                                                      Intellect = 205,
                                                      Spirit = 150,
                                                      HasteRating = 130,
                                                  },
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = legs,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Legs,
                                      SocketColor1 = ItemSlot.Red,
                                      SocketColor2 = ItemSlot.Yellow,
                                      SocketBonus = new Stats
                                                        {
                                                            Spirit = 20,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 1015,
                                                      Stamina = 454,
                                                      Intellect = 262,
                                                      Spirit = 186,
                                                      CritRating = 167,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = feet,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Feet,
                                      SocketColor1 = ItemSlot.Yellow,
                                      SocketBonus = new Stats
                                                        {
                                                            Intellect = 10,
                                                        },
                                      Stats = new Stats
                                                  {
                                                      Armor = 797,
                                                      Stamina = 337,
                                                      Intellect = 205,
                                                      Spirit = 150,
                                                      CritRating = 130,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = finger1,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Finger,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 112,
                                                      CritRating = 112,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = finger2,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Finger,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 98,
                                                      MasteryRating = 120,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = trinket1,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Trinket,
                                      Stats = new Stats
                                                  {
                                                      Intellect = 285,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = trinket2,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Trinket,
                                      Stats = new Stats
                                                  {
                                                      MasteryRating = 285,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = mainhand,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.MainHand,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 194,
                                                      Intellect = 130,
                                                      CritRating = 98,
                                                      HasteRating = 66,
                                                      SpellPower = 1729
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = offhand,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.OffHand,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 252,
                                                      Intellect = 168,
                                                      Spirit = 120,
                                                      CritRating = 98,
                                                  }
                                  });

            itemCache.AddItem(new Item
                                  {
                                      Id = ranged,
                                      ItemLevel = 346,
                                      Slot = ItemSlot.Ranged,
                                      Stats = new Stats
                                                  {
                                                      Stamina = 143,
                                                      Intellect = 95,
                                                      CritRating = 63,
                                                      HasteRating = 63,
                                                  }
                                  });

            character.Head = new ItemInstance(head, 0, 0, 0, 0, 0, 0, 0);
            character.Neck = new ItemInstance(neck, 0, 0, 0, 0, 0, 0, 0);
            character.Shoulders = new ItemInstance(shoulder, 0, 0, 0, 0, 0, 0, 0);
            character.Back = new ItemInstance(back, 0, 0, 0, 0, 0, 0, 0);
            character.Chest = new ItemInstance(chest, 0, 0, 0, 0, 0, 0, 0);
            character.Wrist = new ItemInstance(wrist, 0, 0, 0, 0, 0, 0, 0);
            character.Hands = new ItemInstance(hands, 0, 0, 0, 0, 0, 0, 0);
            character.Waist = new ItemInstance(waist, 0, 0, 0, 0, 0, 0, 0);
            character.Legs = new ItemInstance(legs, 0, 0, 0, 0, 0, 0, 0);
            character.Feet = new ItemInstance(feet, 0, 0, 0, 0, 0, 0, 0);
            character.Finger1 = new ItemInstance(finger1, 0, 0, 0, 0, 0, 0, 0);
            character.Finger2 = new ItemInstance(finger2, 0, 0, 0, 0, 0, 0, 0);
            character.Trinket1 = new ItemInstance(trinket1, 0, 0, 0, 0, 0, 0, 0);
            character.Trinket2 = new ItemInstance(trinket2, 0, 0, 0, 0, 0, 0, 0);
            character.MainHand = new ItemInstance(mainhand, 0, 0, 0, 0, 0, 0, 0);
            character.OffHand = new ItemInstance(offhand, 0, 0, 0, 0, 0, 0, 0);
            character.Ranged = new ItemInstance(ranged, 0, 0, 0, 0, 0, 0, 0);
        }

        public static void LoadItemSet3(Character character)
        {
            int head = 1;
            int neck = 2;
            int shoulder = 3;
            int back = 4;
            int chest = 5;
            int wrist = 6;
            int hands = 7;
            int waist = 8;
            int legs = 9;
            int feet = 10;
            int finger1 = 11;
            int finger2 = 12;
            int trinket1 = 13;
            int trinket2 = 14;
            int mainhand = 15;
            int offhand = 16;
            int ranged = 17;

            ItemCacheInstance itemCache = new ItemCacheInstance();
            ItemCache.Instance = itemCache;

            itemCache.AddItem(new Item
            {
                Id = head,
                ItemLevel = 346,
                Slot = ItemSlot.Head,
                SocketColor1 = ItemSlot.Meta,
                SocketColor2 = ItemSlot.Yellow,
                SocketBonus = new Stats
                {
                    Intellect = 30,
                },
                Stats = new Stats
                {
                    Armor = 942,
                    Stamina = 454,
                    Intellect = 242,
                    CritRating = 162,
                    MasteryRating = 182,
                }
            }
                );

            itemCache.AddItem(new Item
            {
                Id = neck,
                ItemLevel = 346,
                Slot = ItemSlot.Neck,
                Stats = new Stats
                {
                    Stamina = 252,
                    Intellect = 168,
                    HasteRating = 120,
                    MasteryRating = 98,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = shoulder,
                ItemLevel = 346,
                SocketColor1 = ItemSlot.Yellow,
                SocketBonus = new Stats
                {
                    Intellect = 10,
                },
                Stats = new Stats
                {
                    Armor = 870,
                    Stamina = 337,
                    Intellect = 205,
                    CritRating = 130,
                    MasteryRating = 150,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = back,
                ItemLevel = 346,
                Slot = ItemSlot.Back,
                Stats = new Stats
                {
                    Armor = 580,
                    Stamina = 252,
                    Intellect = 168,
                    HasteRating = 112,
                    MasteryRating = 112,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = chest,
                ItemLevel = 346,
                Slot = ItemSlot.Chest,
                SocketColor1 = ItemSlot.Yellow,
                SocketColor2 = ItemSlot.Blue,
                SocketBonus = new Stats
                {
                    Intellect = 20,
                },
                Stats = new Stats
                {
                    Armor = 1160,
                    Stamina = 454,
                    Intellect = 262,
                    CritRating = 202,
                    HasteRating = 162
                }
            });

            itemCache.AddItem(new Item
            {
                Id = wrist,
                ItemLevel = 346,
                Slot = ItemSlot.Wrist,
                Stats = new Stats
                {
                    Armor = 507,
                    Stamina = 252,
                    Intellect = 168,
                    CritRating = 112,
                    MasteryRating = 112,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = hands,
                ItemLevel = 346,
                Slot = ItemSlot.Hands,
                SocketColor1 = ItemSlot.Yellow,
                SocketBonus = new Stats
                {
                    Intellect = 20,
                },
                Stats = new Stats
                {
                    Armor = 725,
                    Stamina = 337,
                    Intellect = 205,
                    HasteRating = 150,
                    MasteryRating = 130,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = waist,
                ItemLevel = 346,
                Slot = ItemSlot.Waist,
                SocketColor1 = ItemSlot.Red,
                SocketBonus = new Stats
                {
                    HasteRating = 10,
                },
                Stats = new Stats
                {
                    Armor = 652,
                    Stamina = 337,
                    Intellect = 205,
                    HasteRating = 130,
                    CritRating = 150,
                },
            });

            itemCache.AddItem(new Item
            {
                Id = legs,
                ItemLevel = 346,
                Slot = ItemSlot.Legs,
                SocketColor1 = ItemSlot.Red,
                SocketColor2 = ItemSlot.Yellow,
                SocketBonus = new Stats
                {
                    HasteRating = 20,
                },
                Stats = new Stats
                {
                    Armor = 1015,
                    Stamina = 454,
                    Intellect = 262,
                    CritRating = 162,
                    HasteRating = 202
                }
            });

            itemCache.AddItem(new Item
            {
                Id = feet,
                ItemLevel = 346,
                Slot = ItemSlot.Feet,
                SocketColor1 = ItemSlot.Yellow,
                SocketBonus = new Stats
                {
                    CritRating = 10,
                },
                Stats = new Stats
                {
                    Armor = 797,
                    Stamina = 337,
                    Intellect = 205,
                    CritRating = 150,
                    HasteRating = 130,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = finger1,
                ItemLevel = 346,
                Slot = ItemSlot.Finger,
                Stats = new Stats
                {
                    Stamina = 252,
                    Intellect = 168,
                    CritRating = 112,
                    MasteryRating = 112,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = finger2,
                ItemLevel = 346,
                Slot = ItemSlot.Finger,
                Stats = new Stats
                {
                    Stamina = 252,
                    Intellect = 168,
                    CritRating = 112,
                    HasteRating = 112,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = trinket1,
                ItemLevel = 346,
                Slot = ItemSlot.Trinket,
                Stats = new Stats
                {
                    HitRating = 285,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = trinket2,
                ItemLevel = 346,
                Slot = ItemSlot.Trinket,
                Stats = new Stats
                {
                    MasteryRating = 285,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = mainhand,
                ItemLevel = 346,
                Slot = ItemSlot.MainHand,
                Stats = new Stats
                {
                    Stamina = 194,
                    Intellect = 130,
                    CritRating = 98,
                    HasteRating = 66,
                    SpellPower = 1729
                }
            });

            itemCache.AddItem(new Item
            {
                Id = offhand,
                ItemLevel = 346,
                Slot = ItemSlot.OffHand,
                Stats = new Stats
                {
                    Stamina = 252,
                    Intellect = 168,
                    Spirit = 120,
                    CritRating = 98,
                }
            });

            itemCache.AddItem(new Item
            {
                Id = ranged,
                ItemLevel = 346,
                Slot = ItemSlot.Ranged,
                Stats = new Stats
                {
                    Stamina = 143,
                    Intellect = 95,
                    CritRating = 63,
                    HasteRating = 63,
                }
            });

            character.Head = new ItemInstance(head, 0, 0, 0, 0, 0, 0, 0);
            character.Neck = new ItemInstance(neck, 0, 0, 0, 0, 0, 0, 0);
            character.Shoulders = new ItemInstance(shoulder, 0, 0, 0, 0, 0, 0, 0);
            character.Back = new ItemInstance(back, 0, 0, 0, 0, 0, 0, 0);
            character.Chest = new ItemInstance(chest, 0, 0, 0, 0, 0, 0, 0);
            character.Wrist = new ItemInstance(wrist, 0, 0, 0, 0, 0, 0, 0);
            character.Hands = new ItemInstance(hands, 0, 0, 0, 0, 0, 0, 0);
            character.Waist = new ItemInstance(waist, 0, 0, 0, 0, 0, 0, 0);
            character.Legs = new ItemInstance(legs, 0, 0, 0, 0, 0, 0, 0);
            character.Feet = new ItemInstance(feet, 0, 0, 0, 0, 0, 0, 0);
            character.Finger1 = new ItemInstance(finger1, 0, 0, 0, 0, 0, 0, 0);
            character.Finger2 = new ItemInstance(finger2, 0, 0, 0, 0, 0, 0, 0);
            character.Trinket1 = new ItemInstance(trinket1, 0, 0, 0, 0, 0, 0, 0);
            character.Trinket2 = new ItemInstance(trinket2, 0, 0, 0, 0, 0, 0, 0);
            character.MainHand = new ItemInstance(mainhand, 0, 0, 0, 0, 0, 0, 0);
            character.OffHand = new ItemInstance(offhand, 0, 0, 0, 0, 0, 0, 0);
            character.Ranged = new ItemInstance(ranged, 0, 0, 0, 0, 0, 0, 0);
        }
        #endregion
    }
}