using System.Collections.Generic;
using System;
using System.IO;

namespace Rawr
{
    public class AltoholicImporter
    {

        public const string cxHead      = "1";
        public const string cxNeck      = "2";
        public const string cxShoulders = "3";
        public const string cxShirt     = "4";
        public const string cxChest     = "5";
        public const string cxWaist     = "6";
        public const string cxLegs      = "7";
        public const string cxFeet      = "8";
        public const string cxWrists    = "9";
        public const string cxHands     = "10";
        public const string cxFinger1   = "11";
        public const string cxFinger2   = "12";
        public const string cxTrinket1  = "13";
        public const string cxTrinket2  = "14";
        public const string cxBack      = "15";
        public const string cxMainHand  = "16";
        public const string cxOffHand   = "17";
        public const string cxRanged    = "18";
        public const string cxTabard    = "19";

        public AltoholicSavedVariables SavedVariables;

        public AltoholicImporter(AltoholicSavedVariables Variables)
        {
            SavedVariables = Variables;
        }

        public AltoholicImporter(SavedVariablesDictionary SVCharacters,
                                 SavedVariablesDictionary SVSkills,
                                 SavedVariablesDictionary SVTalents,
                                 SavedVariablesDictionary SVInventory,
                                 SavedVariablesDictionary SVStats)
        {
            SavedVariables = new AltoholicSavedVariables(SVCharacters,
                                                         SVSkills,
                                                         SVTalents,
                                                         SVInventory,
                                                         SVStats);
        }

        public List<String> GetCaracterList()
        {
            List<string> result = new List<string>();

            if (SavedVariables != null)
            {
                SavedVariablesDictionary Chars = (SavedVariablesDictionary)((SavedVariablesDictionary)SavedVariables.Characters["global"])["Characters"];
                foreach (string Char in Chars.Values)
                {
                    result.Add(Char);
                }
            }
            return result;
        }

        public void addItemToCharacter(KeyValuePair<IComparable, object> item, Character oCharacter)
        {
            switch(item.Key as string)
            {
                case cxBack: 
                    oCharacter.Back         = new ItemInstance(item.Value as string);
                    break;
                case cxChest:
                    oCharacter.Chest        = new ItemInstance(item.Value as string);
                    break;
                case cxFeet:
                    oCharacter.Back         = new ItemInstance(item.Value as string);
                    break;
                case cxFinger1:
                    oCharacter.Finger1      = new ItemInstance(item.Value as string);
                    break;
                case cxFinger2:
                    oCharacter.Finger2      = new ItemInstance(item.Value as string);
                    break;
                case cxHands:
                    oCharacter.Hands        = new ItemInstance(item.Value as string);
                    break;
                case cxHead:
                    oCharacter.Head         = new ItemInstance(item.Value as string);
                    break;
                case cxLegs:
                    oCharacter.Legs         = new ItemInstance(item.Value as string);
                    break;
                case cxMainHand:
                    oCharacter.MainHand     = new ItemInstance(item.Value as string);
                    break;
                case cxNeck:
                    oCharacter.Neck         = new ItemInstance(item.Value as string);
                    break;
                case cxOffHand:
                    oCharacter.OffHand      = new ItemInstance(item.Value as string);
                    break;
                case cxRanged:
                    oCharacter.Ranged       = new ItemInstance(item.Value as string);
                    break;
                case cxShirt:
                    oCharacter.Shirt        = new ItemInstance(item.Value as string);
                    break;
                case cxShoulders:
                    oCharacter.Shoulders    = new ItemInstance(item.Value as string);
                    break;
                case cxTabard:
                    oCharacter.Tabard       = new ItemInstance(item.Value as string);
                    break;
                case cxTrinket1:
                    oCharacter.Trinket1     = new ItemInstance(item.Value as string);
                    break;
                case cxTrinket2:
                    oCharacter.Trinket2     = new ItemInstance(item.Value as string);
                    break;
                case cxWaist:
                    oCharacter.Waist        = new ItemInstance(item.Value as string);
                    break;
                case cxWrists:
                    oCharacter.Wrist        = new ItemInstance(item.Value as string);
                    break;
                default:
                    break;
            }
        }


        public Character GetCharacter(string cCharacterString)
        {
            Character result = new Character();

            char[] acSplitCharacters = { '.' };
            string[] asCharInfo = cCharacterString.Split(acSplitCharacters);

            result.Realm = asCharInfo[asCharInfo.Length - 2];

            //Pull base stats Gender, Class, Level ets out of Character File
            SavedVariablesDictionary CharBase = (SavedVariablesDictionary)(
                                                    (SavedVariablesDictionary)(
                                                        (SavedVariablesDictionary)SavedVariables.Characters["global"])
                                                    ["Characters"])
                                                [cCharacterString];
            //result.Level = (int)CharBase["level"]; // Level not setable
            result.Name = CharBase["name"] as string;
            result.Class = (CharacterClass)Enum.Parse(typeof(CharacterClass), CharBase["englishClass"] as string, true);
            result.Race = (CharacterRace)Enum.Parse(typeof(CharacterRace), CharBase["englishRace"] as string, true); //This sets faction

            //Pull Equipped Invenotry out of Character File
            SavedVariablesDictionary Equiped = (SavedVariablesDictionary)(
                                                   (SavedVariablesDictionary)(
                                                       (SavedVariablesDictionary)(
                                                           (SavedVariablesDictionary)SavedVariables.Inventory["global"])
                                                       ["Characters"])
                                                   [cCharacterString])
                                               ["Inventory"];
            foreach (KeyValuePair<IComparable, object> item in Equiped)
            {
                addItemToCharacter(item, result);
            }

            return result;
        }
        

        /*
         * SavedVariable Container for Easy Use
         */ 
        public class AltoholicSavedVariables
        {
            public SavedVariablesDictionary Characters;
            public SavedVariablesDictionary Skills;
            public SavedVariablesDictionary Talents;
            public SavedVariablesDictionary Inventory;
            public SavedVariablesDictionary Stats;

            public  AltoholicSavedVariables(SavedVariablesDictionary SVCharacters,
                                            SavedVariablesDictionary SVSkills,
                                            SavedVariablesDictionary SVTalents,
                                            SavedVariablesDictionary SVInventory,
                                            SavedVariablesDictionary SVStats)
            {
                
                Characters = SVCharacters;
                Skills = SVSkills;
                Talents = SVTalents;
                Inventory = SVInventory;
                Stats = SVStats;
            }
            
        }

    }

}
