using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Rawr
{
    public enum RawrAddonImportType { EquippedBagsBank, EquippedBags, Equipped }
    public class RawrAddonCharacter
    {
        public RawrAddonCharacter(string xmlDump, RawrAddonImportType importType)
        {
            XMLDump = xmlDump;
            ImportType = importType;
            loadFromXML();
        }

        #region Variables
        private RawrAddonImportType _ImportType = RawrAddonImportType.EquippedBagsBank;
        private string _XMLDump = "";
        private Character _character = null;
        //
        public RawrAddonImportType ImportType { get { return _ImportType; } set { _ImportType = value; } }
        public string XMLDump { get { return _XMLDump; } set { _XMLDump = value; } }
        public Character Character { get { return _character; } set { _character = value; } }
        #endregion

        public void loadFromXML()
        {
            try
            {
                XDocument xdoc = XDocument.Parse(XMLDump);

                #region Pull out the list of items from your Bank
                List<int> BankList = new List<int>();
                foreach (XElement node in xdoc.SelectNodes("Rawr/Character/Bank/AvailableItem"))
                {
                    BankList.Add(int.Parse(node.Value));
                }
                xdoc.SelectSingleNode("Rawr/Character/Bank").Remove();
                #endregion

                #region Pull out the list of items from your Bags
                List<int> BagsList = new List<int>();
                foreach (XElement node in xdoc.SelectNodes("Rawr/Character/Bags/AvailableItem"))
                {
                    BagsList.Add(int.Parse(node.Value));
                }
                xdoc.SelectSingleNode("Rawr/Character/Bags").Remove();
                #endregion

                #region Pull out the list of Glyphs equipped
                List<int> GlyphsList = new List<int>();
                foreach (XElement node in xdoc.SelectNodes("Rawr/Character/AvailableGlyphs/Glyph"))
                {
                    GlyphsList.Add(int.Parse(node.Value));
                }
                xdoc.SelectSingleNode("Rawr/Character/AvailableGlyphs").Remove();
                #endregion

                #region Try and create the Character since we got the pullable stuff taken out
                string charXML = xdoc.SelectSingleNode("Rawr/Character").ToString();
                charXML = charXML.Replace("\r\n", "")
                                 .Replace("DEATHKNIGHT", "DeathKnight")
                                 .Replace("DRUID", "Druid")
                                 .Replace("HUNTER", "Hunter") 
                                 .Replace("MAGE", "Mage")
                                 .Replace("PALADIN", "Paladin")
                                 .Replace("PRIEST", "Priest")
                                 .Replace("ROGUE", "Rogue")
                                 .Replace("SHAMAN", "Shaman")
                                 .Replace("WARLOCK", "Warlock")
                                 .Replace("WARRIOR", "Warrior");
                Character character = Character.LoadFromXml(charXML);
                #endregion

                #region Put the Glyph data into the Talents
                ParseGlyphsIntoTalents(GlyphsList, character);
                #endregion

                #region Add Equipped Items and their enchants as available to the Optimizer
                foreach (CharacterSlot cs in Character.CharacterSlots)
                {
                    ItemInstance toMakeAvail = null;
                    if ((toMakeAvail = character[cs]) != null)
                    {
                        character.ToggleItemAvailability(toMakeAvail, true);
                        character.ToggleItemAvailability(toMakeAvail.Enchant);
                    }
                }
                #endregion

                #region Filter the Bags & Bank lists for items you can't equip
                List<Item> relevantItems = new List<Item>(ItemCache.GetRelevantItems(character.CurrentCalculations, character));
                List<int> relevantItemIDs = new List<int>();
                foreach (Item i in relevantItems) {
                    if (relevantItemIDs.Contains(i.Id)) { continue; }
                    relevantItemIDs.Add(i.Id);
                }
                #endregion

                #region Add Bag Items as available to the Optimizer, under option only
                if (ImportType == RawrAddonImportType.EquippedBags || ImportType == RawrAddonImportType.EquippedBagsBank)
                {
                    foreach (int id in BagsList) {
                        if (relevantItemIDs.Contains(id))
                        {
                            character.ToggleItemAvailability(id, true);
                        }// else don't add it because it is an item that shouldn't matter to this character
                    }
                }
                #endregion

                #region Add Bank Items as available to the Optimizer, under option only
                if (ImportType == RawrAddonImportType.EquippedBagsBank)
                {
                    foreach (int id in BankList)
                    {
                        if (relevantItemIDs.Contains(id))
                        {
                            character.ToggleItemAvailability(id, true);
                        }// else don't add it because it is an item that shouldn't matter to this character
                    }
                }
                #endregion

                #region Send the result as ready for the Main Form
                Character = character;
                #endregion
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error Importing Character from Rawr Addon",
                    Function = "loadFromXML()",
                    TheException = ex,
                }.Show();
            }
        }

        private void ParseGlyphsIntoTalents(List<int> glyphsList, Character character)
        {
            foreach (int glyphId in glyphsList) {
                foreach (PropertyInfo pi in character.CurrentTalents.GetType().GetProperties()) {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    if (glyphDatas.Length > 0) {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (glyphData.ID == glyphId) { character.CurrentTalents.GlyphData[glyphData.Index] = true; }
                    }
                }
            }
        }

#if FALSE
        /*
         * This function is used to help populate the optimizer list.
         * Rather than add every item in every bag slot, this filter is used
         * to try to limit the items only to equippable ones.
         * Note however that the current implentation is a bit of a hack
         * that involves searching item Tooltips.
         * Returns true if the item is equippable.
         */
        /*static bool isEquippable(int itemID)
        {
            if (itemID > 0 && ItemCache.ContainsItemId(itemID))
            {
                ItemCache.Items[itemID].Slot
                string sTooltip = itemInfo["Tooltip"] as string;
            }

            return false;
        }*/

        static int getEnchant(SavedVariablesDictionary item)
        {
            string sItemString = item["Item"] as string;
            char[] acSplitCharacters = { ':' };
            return Int32.Parse(sItemString.Split(acSplitCharacters)[1]);
        }

        static int getEnchantBySlot(SavedVariablesDictionary characterInfo, string sSlot)
        {
            SavedVariablesDictionary equipment = (SavedVariablesDictionary)characterInfo["Equipment"];

            if (equipment.ContainsKey(sSlot))
            {
                SavedVariablesDictionary item = equipment[sSlot] as SavedVariablesDictionary;
                return getEnchant(item);
            }
            else
            {
                return 0;
            }
        }

        static string getGearString(SavedVariablesDictionary item, bool replaceAsterisks)
        {
            string sItemString = item["Item"] as string;
            char[] acSplitCharacters = { ':' };
            string[] asItemElements = sItemString.Split(acSplitCharacters);

            if (item.ContainsKey("Gem"))
            {
                SavedVariablesDictionary gems = item["Gem"] as SavedVariablesDictionary;

                string sItemSlotString = "";
                for (long lGemSlot = 1; lGemSlot <= 3; lGemSlot++)
                {
                    sItemSlotString += ".";

                    if (gems.ContainsKey(lGemSlot))
                    {
                        string sGemItemString = (gems[lGemSlot] as SavedVariablesDictionary)["Item"] as string;
                        sItemSlotString += sGemItemString.Split(acSplitCharacters)[0];
                    }
                    else
                    {
                        sItemSlotString += "0";
                    }
                }

                if (replaceAsterisks && sItemSlotString == ".0.0.0")
                {
                    return asItemElements[0] + ".*.*.*";
                }
                else
                {
                    return asItemElements[0] + sItemSlotString;
                }
            }
            else
            {
                if (replaceAsterisks)
                {
                    return asItemElements[0] + ".*.*.*";
                }
                else
                {
                    return asItemElements[0] + ".0.0.0";
                }
            }
        }

        static string getGearStringBySlot(SavedVariablesDictionary characterInfo, string sSlot, bool replaceAsterisks)
        {
            SavedVariablesDictionary equipment = (SavedVariablesDictionary)characterInfo["Equipment"];

            if (equipment.ContainsKey(sSlot))
            {
                SavedVariablesDictionary item = equipment[sSlot] as SavedVariablesDictionary;
                string enchant = getEnchant(item).ToString();
                if (enchant == "0" && replaceAsterisks)
                {
                    enchant = "*";
                }
                return getGearString(item, replaceAsterisks) + "." + enchant;
            }
            else
            {
                return null;
            }
        }

        static bool addEquippedItemForOptimization(List<string> asOptimizableItems,
            SavedVariablesDictionary characterInfo, string sSlot)
        {
            string sItem = getGearStringBySlot(characterInfo, sSlot, true);

            if (sItem != null)
            {
                char[] acSplitCharacters = { '.' };
                string[] asItemElements = sItem.Split(acSplitCharacters);
                if (sItem.Substring(asItemElements[0].Length) == ".*.*.*.*")
                {
                    sItem = asItemElements[0];
                }
                asOptimizableItems.Add(sItem);
                return true;
            }

            return false;
        }

        static void addPossessionsForOptimization(List<string> asOptimizableItems,
            SavedVariablesDictionary characterInfo)
        {
            string[] asSources = { "Inventory", "Bank" };

            foreach (string sSource in asSources)
            {
                if (characterInfo.ContainsKey(sSource))
                {
                    SavedVariablesDictionary bags = characterInfo[sSource] as SavedVariablesDictionary;

                    foreach (object oBag in bags.Values)
                    {
                        SavedVariablesDictionary bag = oBag as SavedVariablesDictionary;
                        SavedVariablesDictionary contents = bag["Contents"] as SavedVariablesDictionary;

                        foreach (object oItem in contents.Values)
                        {
                            SavedVariablesDictionary item = oItem as SavedVariablesDictionary;

                            if (isEquippable(item))
                            {
                                string enchant = getEnchant(item).ToString();
                                if (enchant == "0")
                                {
                                    enchant = "*";
                                }
                                string item2 = getGearString(item, true) + "." + enchant;
                                char[] acSplitCharacters = { '.' };
                                string[] asItemElements = item2.Split(acSplitCharacters);
                                if (item2.Substring(asItemElements[0].Length) == ".*.*.*.*")
                                {
                                    item2 = asItemElements[0];
                                }
                                asOptimizableItems.Add(item2);
                            }
                        }
                    }
                }
            }
        }

        static int getTalentPointsFromTree(SavedVariablesDictionary talent_tree, string spec, TalentDataAttribute talent)
        {
            int points = 0;

            if (talent_tree.ContainsKey(spec))
            {
                SavedVariablesDictionary spec_tree = talent_tree[spec] as SavedVariablesDictionary;

                if (spec_tree.ContainsKey(talent.Name))
                {
                    SavedVariablesDictionary talent_info = spec_tree[talent.Name] as SavedVariablesDictionary;

                    string rank_info = talent_info["Rank"] as string;

                    int split_pos = rank_info.IndexOf(':');
                    string points_str = rank_info.Remove(split_pos);

                    points = (int)Int32.Parse(points_str);
                }
                else
                {
                    Debug.WriteLine("Talent Not Found: " + talent);
                }
            }
            else
            {
                // we're most likely dealing with non-English data, try to determine by position in tree
                foreach (SavedVariablesDictionary tree in talent_tree.Values)
                {
                    if ((long)tree["Order"] == talent.Tree + 1)
                    {
                        string loc = talent.Row + ":" + talent.Column;
                        foreach (object t in tree.Values)
                        {
                            SavedVariablesDictionary td = t as SavedVariablesDictionary;
                            if (td != null)
                            {
                                if ((string)td["Location"] == loc)
                                {
                                    string rank_info = td["Rank"] as string;

                                    int split_pos = rank_info.IndexOf(':');
                                    string points_str = rank_info.Remove(split_pos);

                                    points = (int)Int32.Parse(points_str);
                                    return points;
                                }
                            }
                        }
                    }
                }
                Debug.WriteLine("Talent Tree Not Found: " + spec);
            }

            return points;
        }

        void setTalentsFromTree(SavedVariablesDictionary characterInfo)
        {
            if (!characterInfo.ContainsKey("Talents"))
            {
                return;
            }

            SavedVariablesDictionary talent_tree = characterInfo["Talents"] as SavedVariablesDictionary;

            TalentsBase Talents = Character.CurrentTalents;

            if (Talents != null)
            {
                List<string> treeNames = new List<string>((string[])Talents.GetType().GetField("TreeNames").GetValue(Talents));

                //TalentTree currentTree;
                foreach (PropertyInfo pi in Talents.GetType().GetProperties())
                {
                    TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                    if (talentDatas.Length > 0)
                    {
                        TalentDataAttribute talentData = talentDatas[0];

                        int points = getTalentPointsFromTree(talent_tree, treeNames[talentData.Tree], talentData);
                        Character.CurrentTalents.Data[talentData.Index] = points;
                    }
                }
            }
        }
#endif
    }
}