using System.Collections.Generic;
using System;
using System.IO;

namespace Rawr
{
    public class AltoholicImporter
    {

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

        public String[] GetCaracterList()
        {
            if (SavedVariables != null)
            {
                SavedVariables.Characters           
            }
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
