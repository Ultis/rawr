using System.Collections.Generic;
using System;
using System.IO;
namespace Rawr
{
    /**
     * CharacterProfilerCharacter
     * @author Charinna
     * This class is a subclass of Character that imports its data from a saved variables
     * file created by teh CharacterProfiler mod.
     * It automatically populates the character with the currently equipped gear as
     * well as make available for optimization all items in the character's inventory
     * with their current gemming & enchants.
     */
    public class CharacterProfilerCharacter : Character
    {
        public string Name
        {
            get { return m_sName; }
        }

        public string Summary
        {
            get { return "Level " + m_iLevel + " " + m_sRace + " " + m_sClass; }
        }

        static readonly string [] s_asEquippableTooltipKeywords =
        {
            "<br>Head",
            "<br>Neck",
            "<br>Shoulder",
            "<br>Back",
            "<br>Chest",
            "<br>Wrist",
            "<br>Hands",
            "<br>Waist",
            "<br>Legs",
            "<br>Feet",
            "<br>Finger",
            "<br>Trinket",
            "<br>Main Hand",
            "<br>Off Hand",
            "<br>Two-Hand",
            "<br>Relic",
            "<br>Ranged",
            "<br>Projectile",
        };

        static readonly Dictionary<string, CharacterRace> s_stringToRace;

        static CharacterProfilerCharacter()
        {
            s_stringToRace = new Dictionary<string, CharacterRace>();
            s_stringToRace.Add("Human", CharacterRace.Human);
            s_stringToRace.Add("Orc", CharacterRace.Orc);
            s_stringToRace.Add("Dwarf", CharacterRace.Dwarf);
            s_stringToRace.Add("Night Elf", CharacterRace.NightElf);
            s_stringToRace.Add("Undead", CharacterRace.Undead);
            s_stringToRace.Add("Tauren", CharacterRace.Tauren);
            s_stringToRace.Add("Gnome", CharacterRace.Gnome);
            s_stringToRace.Add("Troll", CharacterRace.Troll);
            s_stringToRace.Add("Blood Elf", CharacterRace.BloodElf);
            s_stringToRace.Add("Draenei", CharacterRace.Draenei);
        }

        static int getEnchant(Dictionary<IComparable, object> item)
        {
            string sItemString = item["Item"] as string;
            char[] acSplitCharacters = { ':' };
            return Int32.Parse(sItemString.Split(acSplitCharacters)[1]);
        }

        static int getEnchantBySlot(Dictionary<IComparable, object> characterInfo, string sSlot)
        {
            Dictionary<IComparable, object> equipment = (Dictionary<IComparable, object>) characterInfo["Equipment"];

            if (equipment.ContainsKey(sSlot))
            {
                Dictionary<IComparable, object> item = equipment[sSlot] as Dictionary<IComparable, object>;
                return getEnchant(item);
            }
            else
            {
                return 0;
            }
        }

        static string getGearString(Dictionary<IComparable, object> item)
        {
            string sItemString = item["Item"] as string;
            char[] acSplitCharacters = { ':' };
            string[] asItemElements = sItemString.Split(acSplitCharacters);

            if (item.ContainsKey("Gem"))
            {
                Dictionary<IComparable, object> gems = item["Gem"] as Dictionary<IComparable, object>;

                string sItemSlotString = asItemElements[0];
                for (int iGemSlot = 1; iGemSlot <= 3; iGemSlot++)
                {
                    sItemSlotString += ".";

                    if (gems.ContainsKey(iGemSlot))
                    {
                        string sGemItemString = (gems[iGemSlot] as Dictionary<IComparable, object>)["Item"] as string;
                        sItemSlotString += sGemItemString.Split(acSplitCharacters)[0];
                    }
                    else
                    {
                        sItemSlotString += "0";
                    }
                }

                return sItemSlotString;
            }
            else
            {
                return asItemElements[0] + ".0.0.0";
            }
        }

        static string getGearStringBySlot(Dictionary<IComparable, object> characterInfo, string sSlot)
        {
            Dictionary<IComparable, object> equipment = (Dictionary<IComparable, object>) characterInfo["Equipment"];

            if (equipment.ContainsKey(sSlot))
            {
                Dictionary<IComparable, object> item = equipment[sSlot] as Dictionary<IComparable, object>;
                return getGearString(item);
            }
            else
            {
                return null;
            }
        }

        /**
         * This function is used to help populate the optimizer list.
         * Rather than add every item in every bag slot, this filter is used
         * to try to limit the items only to equippable ones.
         * Note however that the current implentation is a bit of a hack
         * that involves searching item Tooltips.
         * Returns true if the item is equippable.
         */
        static bool isEquippable(Dictionary<IComparable, object> itemInfo)
        {
            if (itemInfo != null && itemInfo.ContainsKey("Tooltip"))
            {
                string sTooltip = itemInfo["Tooltip"] as string;

                foreach (string sNeedle in s_asEquippableTooltipKeywords)
                {
                    if (sTooltip.IndexOf(sNeedle) != -1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool addEquippedItemForOptimization(List<string> asOptimizableItems, 
            Dictionary<IComparable, object> characterInfo, string sSlot)
        {
            string sItem = getGearStringBySlot(characterInfo, sSlot);

            if (sItem != null)
            {
                asOptimizableItems.Add(sItem + "." + getEnchantBySlot(characterInfo, sSlot));
                return true;
            }

            return false;
        }

        static void addPossessionsForOptimization(List<string> asOptimizableItems,
            Dictionary<IComparable, object> characterInfo)
        {
            string [] asSources = { "Inventory", "Bank" };

            foreach (string sSource in asSources)
            {
                if (characterInfo.ContainsKey(sSource))
                {
                    Dictionary<IComparable, object> bags = characterInfo[sSource] as Dictionary<IComparable, object>;

                    foreach (object oBag in bags.Values)
                    {
                        Dictionary<IComparable, object> bag = oBag as Dictionary<IComparable, object>;
                        Dictionary<IComparable, object> contents = bag["Contents"] as Dictionary<IComparable, object>;

                        foreach (object oItem in contents.Values)
                        {
                            Dictionary<IComparable, object> item = oItem as Dictionary<IComparable, object>;

                            if (isEquippable(item))
                            {
                                asOptimizableItems.Add(getGearString(item) + "." + getEnchant(item));
                            }
                        }
                    }
                }
            }
        }


        /*
		public Character(string name, string realm, Character.CharacterRegion region, CharacterRace race,
         * string head, string neck, string shoulders, string back, string chest, string shirt, string tabard,
				string wrist, string hands, string waist, string legs, string feet, string finger1, string finger2, string trinket1, string trinket2,
         * string mainHand, string offHand, string ranged, string projectile, string projectileBag,
			int enchantHead, int enchantShoulders, int enchantBack, int enchantChest, int enchantWrist, 
         * int enchantHands, int enchantLegs, int enchantFeet, int enchantFinger1, int enchantFinger2, int enchantMainHand, int enchantOffHand, int enchantRanged)
         * */

        public CharacterProfilerCharacter(string sName, string sRealm, Dictionary<IComparable, object> characterInfo)
            : base(sName, sRealm, 
            CharacterRegion.US, // Have to figure out if I know the region
            s_stringToRace[characterInfo["Race"] as string],
            getGearStringBySlot(characterInfo, "Head"),
            getGearStringBySlot(characterInfo, "Neck"),
            getGearStringBySlot(characterInfo, "Shoulder"),
            getGearStringBySlot(characterInfo, "Back"),
            getGearStringBySlot(characterInfo, "Chest"),
            getGearStringBySlot(characterInfo, "Shirt"),
            getGearStringBySlot(characterInfo, "Tabard"),
            getGearStringBySlot(characterInfo, "Wrist"),
            getGearStringBySlot(characterInfo, "Hands"),
            getGearStringBySlot(characterInfo, "Waist"),
            getGearStringBySlot(characterInfo, "Legs"),
            getGearStringBySlot(characterInfo, "Feet"),
            getGearStringBySlot(characterInfo, "Finger0"),
            getGearStringBySlot(characterInfo, "Finger1"),
            getGearStringBySlot(characterInfo, "Trinket0"),
            getGearStringBySlot(characterInfo, "Trinket1"),
            getGearStringBySlot(characterInfo, "MainHand"),
            getGearStringBySlot(characterInfo, "SecondaryHand"),
            getGearStringBySlot(characterInfo, "Ranged"),
            getGearStringBySlot(characterInfo, "Ammo"),
            null, // Not sure what projectile bag is called
            getEnchantBySlot(characterInfo, "Head"),
            getEnchantBySlot(characterInfo, "Shoulder"),
            getEnchantBySlot(characterInfo, "Back"),
            getEnchantBySlot(characterInfo, "Chest"),
            getEnchantBySlot(characterInfo, "Wrist"),
            getEnchantBySlot(characterInfo, "Hands"),
            getEnchantBySlot(characterInfo, "Legs"),
            getEnchantBySlot(characterInfo, "Feet"),
            getEnchantBySlot(characterInfo, "Finger0"),
            getEnchantBySlot(characterInfo, "Finger1"),
            getEnchantBySlot(characterInfo, "MainHand"),
            getEnchantBySlot(characterInfo, "SecondaryHand"),
            getEnchantBySlot(characterInfo, "Ranged"))
        {
            m_characterInfo = characterInfo;
            m_sName = sName;
            m_iLevel = (int) (characterInfo["Level"] as long?);
            m_sRace = (string)characterInfo["Race"];
            m_sClass = (string)characterInfo["Class"];

            // Populate available items
            // Note that some of these items cannot be enchanted
            // But they should correctly return ".0" for their enchants.
            List<string> asOptimizableItems = new List<string>();
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Head");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Neck");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Shoulder");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Back");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Chest");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Shirt");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Tabard");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Wrist");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Hands");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Waist");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Legs");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Feet");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Finger0");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Finger1");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Trinket0");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Trinket1");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "MainHand");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "SecondaryHand");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Ranged");
            addEquippedItemForOptimization(asOptimizableItems, characterInfo, "Ammo");

            addPossessionsForOptimization(asOptimizableItems, characterInfo);

            AvailableItems = asOptimizableItems;
        }

        string m_sName;
        string m_sRace;
        string m_sClass;
        int m_iLevel;
        Dictionary<IComparable, object> m_characterInfo;
    }

    public class CharacterProfilerRealm
    {
        string m_sName;
        List<CharacterProfilerCharacter> m_aCharacters = new List<CharacterProfilerCharacter>();

        public CharacterProfilerRealm(string sName)
        {
            m_sName = sName;
        }

        public string Name
        {
            get { return m_sName; }
        }

        public List<CharacterProfilerCharacter> Characters
        {
            get { return m_aCharacters; }
        }
    }

    public class CharacterProfilerData
    {
        List<CharacterProfilerRealm> m_realms = new List<CharacterProfilerRealm>();

        public List<CharacterProfilerRealm> Realms
        {
            get { return m_realms; }
        }

        public CharacterProfilerData(string sFileName)
        {
            Dictionary<string, object> savedVariables = SavedVariablesParser.parse(sFileName);

            if (!savedVariables.ContainsKey("myProfile"))
            {
                throw new InvalidDataException("Expected myProfile variable in file.");
            }

            Dictionary<IComparable, object> realms = (Dictionary<IComparable, object>)savedVariables["myProfile"];

            foreach (string sRealm in realms.Keys)
            {
                bool bHaveCharacters = false;

                CharacterProfilerRealm realm = new CharacterProfilerRealm(sRealm);

                Dictionary<IComparable, object> characterContainer = (Dictionary<IComparable, object>)realms[sRealm];
                Dictionary<IComparable, object> characters = (Dictionary<IComparable, object>)characterContainer["Character"];

                foreach (string sCharacter in characters.Keys)
                {
                    Dictionary<IComparable, object> characterInfo = (Dictionary<IComparable, object>)characters[sCharacter];
                    CharacterProfilerCharacter character = new CharacterProfilerCharacter(sCharacter, sRealm, characterInfo);
                    realm.Characters.Add(character);
                    bHaveCharacters = true;
                }

                if (bHaveCharacters)
                {
                    m_realms.Add(realm);
                }
           }
        }
    }

}
