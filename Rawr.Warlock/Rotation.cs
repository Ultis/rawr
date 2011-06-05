using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.Warlock
{
    public class Rotation
    {
        public string Name = "Dummy";
        public string Filler = "Shadow Bolt";
        public string Execute = "";
        public List<string> SpellPriority = new List<string>();

        public Rotation() { } // here for XML deserialization
        public Rotation(string name, string filler, string execute, params string[] priorities)
        {
            Name = name;
            Filler = filler;
            Execute = execute;
            SpellPriority = new List<string>(priorities);
        }

        public string GetError()
        {
            bool foundCurse = false;
            foreach (string spell in SpellPriority)
            {
                if (!Spell.ALL_SPELLS.Contains(spell))
                {
                    return spell + " can no longer be prioritized.";
                }

                if (spell.StartsWith("Bane"))
                {
                    if (foundCurse)
                    {
                        return "You may only include one bane.";
                    }
                    foundCurse = true;
                }
            }

            int immo = SpellPriority.IndexOf("Immolate");
            int conf = SpellPriority.IndexOf("Conflagrate");
            if (conf >= 0 && conf < immo)
            {
                return "Conflagrate may only appear after Immolate.";
            }

            if (immo >= 0 && SpellPriority.Contains("Unstable Affliction"))
            {
                return "Unstable Affliction and Immolate don't mix.";
            }

            return null;
        }

        /// <summary>
        /// Gets a modified version of the user's spell priorities, for internal
        /// purposes.
        /// </summary>
        /// <param name="spellPriority"></param>
        /// <returns></returns>
        public List<string> GetPrioritiesForCalcs(WarlockTalents talents, bool execute)
        {
            List<string> forCalcs = new List<string>(SpellPriority);
            if (talents.Backdraft > 0 && !SpellPriority.Contains("Incinerate (Under Backdraft)"))
            {
                forCalcs.Insert(forCalcs.Count, "Incinerate (Under Backdraft)");
            }
            if (!execute
                && Filler.Equals("Shadow Bolt")
                && !forCalcs.Contains("Shadow Bolt (Instant)")
                && ShadowBolt_Instant.IsCastable(talents, forCalcs))
            {
                forCalcs.Insert(forCalcs.Count, "Shadow Bolt (Instant)");
            }
            return forCalcs;
        }

        public bool Contains(string spellname)
        {
            return SpellPriority.Contains(spellname)
                    ||
                   Filler.Equals(spellname);
        }
    }
}
