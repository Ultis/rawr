using System;
using System.Collections.Generic;

namespace Rawr.Elemental
{
    public class Rotation
    {
        private List<Spell> spells;
        private List<Spell> casts;

        public List<Spell> Casts
        {
            get {
                if (casts == null)
                    calculateCastSpells();
                return casts;
            }
        }
        Dictionary<Type, float> cc = null;
        public Dictionary<Type, float> ClearCasting
        {
            get
            {
                if (cc == null)
                    calculateProperties();
                return cc;
            }
        }

        float dps = float.PositiveInfinity;
        /// <summary>
        /// Damage per second from spell casts.
        /// </summary>
        public float DPS
        {
            get
            {
                if (dps == float.PositiveInfinity)
                    calculateProperties();
                return dps;
            }
            set
            {
                dps = value;
            }
        }

        float mps = float.PositiveInfinity;
        /// <summary>
        /// Used Mana per Second
        /// </summary>
        public float MPS
        {
            get 
            {
                if (mps == float.PositiveInfinity)
                    calculateProperties();
                return mps; 
            }
            set
            {
                mps = value;
            }
        }

        public LightningBolt LB;
        public ChainLightning CL;
        public ChainLightning CL3;
        public ChainLightning CL4;
        public LavaBurst LvB;
        public LavaBurst LvBFS;
        public FlameShock FS;
        public EarthShock ES;
        public FrostShock FrS;

        private ShamanTalents talents;

        public Rotation(ShamanTalents talents)
        {
            this.talents = talents;
            spells = new List<Spell>(15);
        }

        public Rotation(ShamanTalents talents, LightningBolt lb, ChainLightning cl, ChainLightning cl3, ChainLightning cl4, LavaBurst lvb, LavaBurst lvbfs, FlameShock fs, EarthShock es, FrostShock frs) : this(talents)
        {
            LB = lb;
            CL = cl;
            CL3 = cl3;
            CL4 = cl4;
            LvB = lvb;
            LvBFS = lvbfs;
            FS = fs;
            ES = es;
            FrS = frs;

            CalculateRotation();
        }

        private void Invalidate()
        {
            casts = null;
            lastGetTime = float.PositiveInfinity;
            cc = null;
            mps = float.PositiveInfinity;
            dps = float.PositiveInfinity;
        }

        /// <summary>
        /// Calculates a rotation based on the FS>LvB>LB priority.
        /// </summary>
        public void CalculateRotation()
        {
            float optimalWait = LvBFS.TotalDamage * (LB.CastTime + LvBFS.CastTime) / (LB.TotalDamage + LvBFS.TotalDamage) - LvBFS.CastTime;
            CalculateRotation(optimalWait);
        }

        /// <summary>
        /// Calculates a rotation based on the FS>LvB>LB priority.
        /// </summary>
        /// <param name="waitThreshold">amount of time before LvB gets ready when a lightning bolt shall be skipped</param>
        public void CalculateRotation(float waitThreshold)
        {
            spells.Clear();
            waitThreshold = Math.Min(waitThreshold, LB.CastTime);

            if (talents.GlyphofFlameShock)
            {
                float LvBreadyAt = 0, FSdropsAt = 0;
                while (true)
                {
                    if (GetTime() >= LvBreadyAt)
                    {
                        if (GetTime() + LvBFS.CastTimeWithoutGCD < FSdropsAt)
                        {
                            AddSpell(LvBFS);
                            LvBreadyAt = GetTime() + LvBFS.Cooldown;
                        }
                        else if (FSdropsAt == 0)
                        {
                            FSdropsAt = GetTime() + FS.Duration;
                            AddSpell(FS);
                        }
                        else
                            break; //done
                    }
                    else
                    {
                        if (GetTime() + LvB.CastTime > FSdropsAt)
                        {
                            if (LvBreadyAt - (GetTime() + FS.CastTime) <= waitThreshold)
                            {
                                float waitTime = LvBreadyAt - (GetTime() + FS.CastTime);
                                if(waitTime>0f)
                                    AddSpell(new Wait(waitTime));
                                break; //done
                            }
                            else
                                AddSpell(LB);
                        }
                        else if (LvBreadyAt - GetTime() <= waitThreshold)
                            AddSpell(new Wait(LvBreadyAt - GetTime()));
                        else
                            AddSpell(LB);
                    }
                }
            }
            else //unglyphed
            {
                AddSpell(LvBFS);
                AddSpell(FS);
                while (LvBFS.Cooldown - GetTime() > waitThreshold)
                    AddSpell(LB);
                if (LvBFS.Cooldown - GetTime() > float.Epsilon) //a little time left
                    AddSpell(new Wait(LvBFS.Cooldown - GetTime()));
            }
        }

        public void AddSpell(Spell spell)
        {
            spells.Add(spell);
            Invalidate();
        }

        public void RemoveSpellAt(int i)
        {
            spells.RemoveAt(i);
            Invalidate();
        }

        private float lastGetTime = float.PositiveInfinity;
        /// <summary>
        /// Gets the time after the last spell has been cast and the GCD is done.
        /// </summary>
        /// <returns></returns>
        public float GetTime()
        {
            if (lastGetTime != float.PositiveInfinity)
                return lastGetTime;
            return lastGetTime = GetTime(spells.Count);
        }

        /// <summary>
        /// Gets the time after spell number i has been cast (with GCD) counting 0 as the first one.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetTime(int i)
        {
            float time = 0f;
            for (int j = 0; j < i && j < spells.Count; j++ )
                time += spells[j].CastTime;
            return time;
        }

        public float getCastsPerSecond()
        {
            return Casts.Count / GetTime();
        }

        public float getCastsPerSecond(Type spellType)
        {
            return getSpells(spellType).Count / GetTime();
        }

        public List<Spell> getSpells(Type spellType)
        {
            return spells.FindAll(delegate(Spell spell) { return spellType.IsInstanceOfType(spell); });
        }

        public float getWeightedHitchance()
        {
            float hitchance = 0f;
            foreach (Spell s in Casts)
                hitchance += s.HitChance;
            return hitchance / Casts.Count;
        }

        /// <summary>
        /// Returns the average Critchance with Hitchance factored in.
        /// </summary>
        /// <returns></returns>
        public float getWeightedCritchance()
        {
            float critLB = LB.CritChance;
            float critCL = LB.CritChance * (1f + .11f * talents.LightningOverload);
            float critchance = 0f;
            foreach (Spell s in Casts)
                critchance += s.HitChance * s.CCCritChance;
            return critchance / Casts.Count;
        }

        public float waitingPercentage()
        {
            List<Spell> waits = getSpells(typeof(Wait));
            float d = 0f;
            foreach (Spell s in waits)
                d += s.CastTime;
            return d / GetTime();
        }

        private void calculateCastSpells()
        {
            casts = spells.FindAll(delegate(Spell s) { return !(s is Wait); });
        }

        /// <summary>
        /// Calculates Clearcasting, mana usage and dps.
        /// </summary>
        private void calculateProperties()
        {
            mps = 0f; //summing up total manacost
            dps = 0f; //summing up total damage
            cc = new Dictionary<Type, float>(); //clear casting
            Dictionary<Type, int> count = new Dictionary<Type, int>(); //counting spells
            Spell prev1 = null, prev2 = null;
            for (int i = -2; i < Casts.Count; i++)
            {
                Spell s = getCast(i);
                if(i>=0) //the first two are just saved in order to calculate clear casting
                {
                    if(!cc.ContainsKey(s.GetType()))
                    {
                        cc.Add(s.GetType(), 0f);
                        count.Add(s.GetType(), 0);
                    }
                    float ccc = 0f;
                    if (talents.ElementalFocus > 0)
                        ccc = 1f - (1f - prev1.CCCritChance) * (1f - prev2.CCCritChance);
                    mps += s.ManaCost * (1 - .4f * ccc);
                    dps += s.HitChance * s.TotalDamage * (1 + .05f * talents.ElementalOath * ccc); //bad for FS ticks. FS ticks calculation would need to be added if the rotation can be changed by everyone.
                    cc[s.GetType()] += ccc;
                    count[s.GetType()]++;
                }
                prev2 = prev1;
                prev1 = s;
            }
            foreach (Type t in count.Keys)
                cc[t] /= count[t];
            mps /= GetTime(); //divide by rotation time
            dps /= GetTime();
        }

        /// <summary>
        /// Return the i-th spell cast.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Spell getCast(int i)
        {
            return Casts[Mod(i, Casts.Count)]; ;
        }

        /// <summary>
        /// Return the spell number based on it's cast number. This is ignoring Wait.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int getSpellNumber(int castNumber)
        {
            int i;
            for (i=0; castNumber >= 0; i++ )
            {
                if (!(spells[i] is Wait))
                    castNumber--;
            }
            return i;
        }

        public override string ToString()
        {
            string r = "";
            string prev = "";
            int count = 1;
            foreach (Spell s in spells)
            {
                if (s.ToString().Equals(prev))
                    count++;
                else
                {
                    if(prev.Length>0)
                        r += "/" + ((count > 1) ? count.ToString() : "") + prev;
                    prev = s.ToString();
                    count = 1;
                }
            }
            if (prev.Length > 0)
                r += "/" + ((count > 1) ? count.ToString() : "") + prev;
            return r.Substring(1);
        }

        public string ToDetailedString()
        {
            string r = "";
            for (int i = 0; i < spells.Count; i++)
            {
                r += "\n" + Math.Round(GetTime(i),2) + "s\t" + spells[i].ToString();
            }
            r += "\n" + Math.Round(GetTime() + spells[spells.Count-1].CastTime, 2) + "s\t...";
            return r.Substring(1);
        }

        private static int Mod(int a, int n)
        {
            a = a % n;
            if (a < 0)
                a += n;
            return a;
        }
    }
}
