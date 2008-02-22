using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.Warlock
{
    internal class WarlockSpellRotation
    {
        private Dictionary<string, Spell> _spellList = new Dictionary<string, Spell>();
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

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public void AddSpell(Spell spell, int priority)
        {
            _spellList.Add(spell.Name, spell);
            _spellPriority.Add(priority, spell);
        }

        public float[] GetDPS
        {
            get
            {
                float gcdDuration = 1.5f;
                Dictionary<Spell, float> currCasting = new Dictionary<Spell, float>();
                Dictionary<Spell, float> currDuration = new Dictionary<Spell, float>();
                float totalDamage = 0;
                float currMana = _stats.Mana;
                bool gcd = false;
                float gcdstart = 0;
                for (float currTime = 0;currTime <= _duration;currTime += 0.1f)
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
                        foreach (int priority in _spellPriority.Keys)
                        {
                            Spell currSpell = _spellPriority[priority];
                            if (!currCasting.ContainsKey(currSpell) && !currDuration.ContainsKey(currSpell))
                            {
                                if (currMana >= currSpell.Cost)
                                {
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
                                    gcd = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (gcd & gcdstart == 0) gcdstart = currTime;
                }
                return new float[] { totalDamage / Duration };
              }

            }
        }
        
    }

