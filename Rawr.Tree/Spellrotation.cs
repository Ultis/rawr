using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    [Serializable]
    public class Spellcast
    {
        public string spellname;
        public string target;

        public Spellcast()
        {
            spellname = "";
            target = "";
        }

        public Spellcast(string spell, string target)
        {
            this.spellname = spell;
            this.target = target;
        }

        public override string ToString()
        {
            return spellname + " on " + target;
        }
    }

    public class HoTEffect
    {
        public float time = 0f;
        public float healing = 0f;
        public string source = "";
        public string target = "";

        public HoTEffect() { }

        public HoTEffect(float time, float healing, string source, string target)
        {
            this.time = time;
            this.healing = healing;
            this.source = source;
            this.target = target;
        }
        public override string ToString()
        {
            return time + ": " + healing + " from " + source + " to " + target;
        }
    }

    [Serializable]
    public class Spellrotation
    {
        public string Name = "";

        public List<Spellcast> spells = new List<Spellcast>();

        public float HPS
        { get { return cycleheal/cycletime; } }

        [System.Xml.Serialization.XmlIgnore]
        public float cyclemana = 0f;
        [System.Xml.Serialization.XmlIgnore]
        public float cycletime = 0f;
        [System.Xml.Serialization.XmlIgnore]
        public float cycleheal = 0f;

        public float manaPerSecond
        { get { return cyclemana / cycletime; } }

        public Spellrotation()
        { 
        }

        public void CalculateSpellRotaion(CharacterCalculationsTree calculatedStats)
        {
            cycletime = 0f;
            cycleheal = 0f;
            cyclemana = 0f;

            List<HoTEffect> hots = new List<HoTEffect>();
            List<HoTEffect> removelist = new List<HoTEffect>();
            int tmp = 0;

            #region Round one (setting up HoTs)
            foreach (Spellcast spellcast in spells)
            {
                Spell spell = getSpellByName(spellcast.spellname, calculatedStats);
                if (spell == null)
                    continue;
                if (spell.ToString() == "Rawr.Tree.Rejuvenation")
                    cycletime = cycletime;

                #region remove already ticking hots due refreshing
                if (spellcast.spellname == "Regrowth" || spellcast.spellname == "Lifebloom" || spellcast.spellname == "Rejuvenation")
                    removeOverriddenHoTs(calculatedStats, hots, removelist, ref tmp,  spellcast, ref spell);
                #endregion

                #region add new HoTs
                for (int i = 0; i < spell.PeriodicTicks; i++)
                {
                    hots.Add(new HoTEffect(cycletime + spell.castTime + (i + 1) * spell.PeriodicTickTime, spell.PeriodicTick, spell.ToString(), spellcast.target));
                }

                if (spell is Lifebloom && !(spell is LifebloomStack))
                    hots.Add(new HoTEffect(cycletime + spell.PeriodicTickTime * spell.PeriodicTicks, spell.AverageHealingwithCrit, "Lifebloom Final", spellcast.target));
                #endregion

                cycletime += spell.CastTime;
            }

            removelist.Clear();
            foreach (HoTEffect hot in hots)
            {
                if (hot.time <= cycletime)
                {
                    removelist.Add(hot);
                }
            }
            foreach (HoTEffect hot in removelist)
                hots.Remove(hot);
            #endregion

            string calculation = "\r\n";

            #region Round two - calculate real HPS with HoTs from the first round
            //place the hots in front of the cycle
            foreach (HoTEffect hot in hots)
                hot.time -= cycletime;
            cycletime = 0f;

            int lifeblooms = 0;
            foreach (Spellcast spellcast in spells)
            {
                removelist.Clear();

                lifeblooms = 1;

                Spell spell = getSpellByName(spellcast.spellname, calculatedStats);
                if (spell == null)
                    continue;

                #region Empower Hots & clear old ones (refresh) and Nourish
                if (spellcast.spellname == "Regrowth" || spellcast.spellname == "Lifebloom" || spellcast.spellname == "Rejuvenation")
                {
                    removeOverriddenHoTs(calculatedStats, hots, removelist, ref lifeblooms, spellcast, ref spell);
                }
                if (spellcast.spellname == "Nourish")
                {
                    foreach (HoTEffect hot in hots)
                    {
                        if (hot.source == "Rawr.Tree.Rejuvenation" || hot.source == "Rawr.Tree.Regrowth" || hot.source == "Rawr.Tree.Lifebloom")
                            spell = new Nourish(calculatedStats, true);
                    }
                }
                #endregion

                #region add Hots
                for (int i = 0; i < spell.PeriodicTicks; i++)
                {
                    hots.Add(new HoTEffect(cycletime + spell.castTime + (i + 1) * spell.PeriodicTickTime, spell.PeriodicTick * lifeblooms, spell.ToString(), spellcast.target));
                }
                #endregion

                if (spell is Lifebloom && !(spell is LifebloomStack))
                    hots.Add(new HoTEffect(cycletime + spell.PeriodicTickTime * spell.PeriodicTicks, spell.AverageHealingwithCrit, "Rawr.Tree.Lifebloom Final", spellcast.target));
                else
                {
                    cycleheal += spell.AverageHealingwithCrit;
                    calculation += (cycletime + spell.castTime) + ": " + spell.AverageHealingwithCrit + " from " + spell.ToString() + " to " + spellcast.target+"\r\n";
                }

                cycletime += spell.CastTime;
                cyclemana += spell.manaCost;

                #region calculate Hots
                foreach (HoTEffect hot in hots)
                {
                    if ((spell.ToString() == "Rawr.Tree.Regrowth" && hot.time < cycletime) || (spell.ToString() != "Rawr.Tree.Regrowth" && hot.time <= cycletime))
                    {
                        cycleheal += hot.healing;
                        calculation += hot.ToString() +  "\r\n";
                        removelist.Add(hot);
                    }
                }
                foreach (HoTEffect hot in removelist)
                    hots.Remove(hot);
                removelist.Clear();
                #endregion
            }
            #endregion

            if (spells.Count == 0)
                cycletime = 1f; //to avoid division 0
            else
                Log.Write(calculation);
        }

        private void removeOverriddenHoTs(CharacterCalculationsTree calculatedStats, List<HoTEffect> hots, List<HoTEffect> removelist, ref int lifeblooms, Spellcast spellcast, ref Spell spell)
        {
            bool empoweredRG = false;

            removelist.Clear();
            foreach (HoTEffect hot in hots)
            {
                if (hot.target == spellcast.target)
                {
                    if (hot.source == spell.ToString() && hot.time > cycletime)
                        removelist.Add(hot); //throw all hots out, which are overridden
                    if (spell.ToString() == "Rawr.Tree.Regrowth" && hot.source == "Rawr.Tree.Regrowth") 
                    {
                        if (hot.time < cycletime + spell.castTime)
                            removelist.Remove(hot); //get the HoT ticks, while casting, out of the removelist :>
                        else if (!empoweredRG)
                            spell = getSpellByName("RegrowthActive", calculatedStats); //there are ticks (which are overridden) from an old cast of RG
                    }
                    if (hot.source == "Rawr.Tree.Lifebloom Final" && spell.ToString() == "Rawr.Tree.Lifebloom")
                        removelist.Add(hot);
                    if (hot.source == "Rawr.Tree.Lifebloom")
                        lifeblooms = 3;
                }
            }
            foreach (HoTEffect hot in removelist)
                hots.Remove(hot);
            removelist.Clear();
        }

        public void addSpell(Spellcast spellcast)
        {
            spells.Add(spellcast); 
        }

        public Spell getSpellByName(string spellname, CharacterCalculationsTree calculatedStats)
        {
            if (calculatedStats == null)
                throw new Exception("CharacterCalculationsTree is null");

            Spell spell = null;
            switch (spellname)
            {
                case "Healing Touch":
                {
                    spell = new HealingTouch(calculatedStats);
                    break;
                }
                case "Regrowth":
                {
                    spell = new Regrowth(calculatedStats);
                    break;
                }
                case "RegrowthActive":
                {
                    spell = new Regrowth(calculatedStats, true);
                    break;
                }
                case "Lifebloom":
                {
                    spell = new Lifebloom(calculatedStats);
                    break;
                }
                case "Lifebloom Stack":
                {
                    spell = new LifebloomStack(calculatedStats);
                    break;
                }
                case "Rejuvenation":
                {
                    spell = new Rejuvenation(calculatedStats);
                    break;
                }
                case "Wild Growth":
                {
                    spell = new WildGrowth(calculatedStats);
                    break;
                }
                case "Nourish":
                {
                    spell = new Nourish(calculatedStats);
                    break;
                }
                case "NourishHoT":
                {
                    spell = new Nourish(calculatedStats, true);
                    break;
                }
            }
            return spell;
        }
    }
}
