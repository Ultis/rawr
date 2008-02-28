using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.Warlock
{
    internal class WarlockSpellRotation
    {
        private Dictionary<int, Spell> _spellPriority = new Dictionary<int, Spell>();
        private int _duration;
        private Stats _stats;
        private int _lifetap;
        private Character _character;

        private int _lifetapBase = 582;
        private float _lifetapCoeffecient = 0.8f;



        public WarlockSpellRotation(Stats totalStats, Character character, int duration)
        {
            stats = totalStats;
            _character = character;
            _duration = duration;
            calcLifeTap();
        }

     

        private void calcLifeTap()
        {
            float damage = _stats.SpellShadowDamageRating + _stats.SpellDamageRating;
            _lifetap = (int)(_lifetapBase + (damage * _lifetapCoeffecient));
            if (_character != null && _stats != null)
                _lifetap *= (int)Math.Floor(_character.Talents.GetTalent("Improved Lifetap").PointsInvested * 0.1f + 1);
        }

        public Stats stats
        {
            set {_stats = value;
                  calcLifeTap();
            }
        }

        public int ManaPerLifetap
        {
            get { return _lifetap; }
        }

        public Dictionary<int, Spell> Spells
        {
            get { return _spellPriority; }
            set { _spellPriority = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public void AddSpell(Spell spell, int priority)
        {
            _spellPriority.Add(priority, spell);
        }


        public Dictionary<Spell, int> NumCasts
        {
            get;
            set;
        }

        public int NumLifetaps
        {
            get;
            set;
        }

        public float[] GetDPS
        {
            get
            {
                NumCasts = new Dictionary<Spell, int>();
                NumLifetaps = 0;
                float gcd = 1.5f;
                float gainedMana = _stats.Mana;
                float lifetap = _lifetap;
                float durationLeft = Duration;
                Spell filler = null;


                //calc all dots
                foreach (Spell currSpell in _spellPriority.Values) 
                {
                    //Spell currSpell = _spellPriority[x];
                    if (!NumCasts.ContainsKey(currSpell))
                        NumCasts.Add(currSpell, 0);
                    if (currSpell.PeriodicDuration > 0)
                    {
                        NumCasts[currSpell] = Convert.ToInt32(Math.Floor(Duration / currSpell.PeriodicDuration));
                        durationLeft -= NumCasts[currSpell] * (currSpell.CastTime < gcd ? gcd : currSpell.CastTime);
                    }
                    else
                    {
                        if (filler != null)
                            throw new Exception("Error, multiple filler spells");
                        filler = currSpell;
                    }
                }


                //calc filler
                NumCasts[filler] = Convert.ToInt32(Math.Floor(durationLeft / (filler.CastTime < gcd ? gcd : filler.CastTime) ));

                List<Spell> daSpells = new List<Spell>(NumCasts.Keys);

                //calc Lifetaps
                float manaSpent = 0;
                daSpells.ForEach(delegate(Spell s) { manaSpent += NumCasts[s] * s.Cost; });

                while (gainedMana < manaSpent)
                {
                    durationLeft -= gcd;
                    gainedMana += lifetap;
                    NumLifetaps++;
                    NumCasts[filler] = Convert.ToInt32(Math.Floor(durationLeft / (filler.CastTime < gcd ? gcd : filler.CastTime)));
                    manaSpent = 0;
                    daSpells.ForEach(delegate(Spell s) { manaSpent += NumCasts[s] * s.Cost; });
                }

                //calcTotalDamage
                float totalDamage = 0;
                daSpells.ForEach(delegate(Spell s) { totalDamage += NumCasts[s] * s.AverageDamage; });
                return new float[] { totalDamage / Duration };

                //wierd bug with this model (on trinkets would sometimes report battlemasters audacity > hex shrunken head (47sd > 53sd) - wierd
                /*
                NumCasts = new Dictionary<Spell, int>();
                NumLifetaps = 0;
                List<int> daSpells = new List<int>(_spellPriority.Keys);
                daSpells.Sort();
                float gcdDuration = 1.5f;
                Dictionary<Spell, float> currCasting = new Dictionary<Spell, float>();
                Dictionary<Spell, float> currDuration = new Dictionary<Spell, float>();
                float totalDamage = 0;
                float currMana = _stats.Mana;
                bool gcd = false;
                float gcdstart = 0;
                for (float currTime = 0;currTime <= _duration;currTime += 0.05f)
                {
                    //check gcd over
                    if (gcd && (currTime - gcdstart) >= gcdDuration)
                    {
                        gcd = false;
                        gcdstart = 0;
                    }

                    //check if spell casting finished
                    List<Spell> toRemove = new List<Spell>();
                    foreach (Spell s in currCasting.Keys)
                    {
                        if ((currTime - currCasting[s]) >= s.CastTime)
                            toRemove.Add(s);
                    }

                    foreach (Spell s in toRemove)
                        currCasting.Remove(s);

                    toRemove.Clear();


                    //check if spell duration elapsed
                    foreach (Spell s in currDuration.Keys)
                    {
                        if ((currTime - currDuration[s]) >= s.PeriodicDuration)
                            toRemove.Add(s);
                    }

                    foreach (Spell s in toRemove)
                        currDuration.Remove(s);

                    if (!gcd)
                    {
                        foreach (int priority in daSpells)
                        {
                            Spell currSpell = _spellPriority[priority];
                            if (!currCasting.ContainsKey(currSpell) && !currDuration.ContainsKey(currSpell))
                            {
                                if (currMana >= currSpell.Cost)
                                {
                                    if (!NumCasts.ContainsKey(currSpell))
                                        NumCasts.Add(currSpell, 0);
                                    NumCasts[currSpell]++;
                                    totalDamage += currSpell.AverageDamage;
                                    currMana -= currSpell.Cost;
                                    currCasting.Add(currSpell, currTime);
                                    currDuration.Add(currSpell, currTime);
                                    gcd = true;
                                    break;
                                }
                                else
                                {
                                    currMana += _lifetap;
                                    NumLifetaps++;
                                    gcd = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (gcd & gcdstart == 0) gcdstart = currTime;
                }
                return new float[] { (totalDamage) / Duration };
                 */
              }

            }
        }
        
    }

