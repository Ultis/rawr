using System;
using System.Collections.Generic;
using Rawr.Elemental.Spells;

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
        SerializableDictionary<Type, float> cc = null;
        public SerializableDictionary<Type, float> ClearCasting
        {
            get
            {
                if (cc == null)
                    calculateProperties();
                return cc;
            }
        }

        SerializableDictionary<Type, float> spelldps = null;
        public SerializableDictionary<Type, float> SpellDPS
        {
            get
            {
                if (spelldps == null)
                    calculateProperties();
                return spelldps;
            }
        }

        SerializableDictionary<Type, Spell> spelltype = null;
        public SerializableDictionary<Type, Spell> TypeToSpell
        {
            get
            {
                if (spelltype == null)
                    calculateProperties();
                return spelltype;
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

        float lag = float.PositiveInfinity;
        /// <summary>
        /// Effective time lost due to latency per spellcast.
        /// </summary>
        public float LatencyPerSecond
        {
            get
            {
                if (lag == float.PositiveInfinity)
                    calculateProperties();
                return lag;
            }
            set
            {
                lag = value;
            }
        }

        public LightningBolt LB;
        public ChainLightning CL;
        public LavaBurst LvB;
        public LavaBurst LvBFS;
        public FlameShock FS;
        public EarthShock ES;
        public FrostShock FrS;

        public ShamanTalents Talents;

        public Rotation()
        {
            spells = new List<Spell>(15);
        }

        public Rotation(ShamanTalents talents, SpellBox spellBox)
            : this()
        {
            Talents = talents;
            LB = spellBox.LB;
            CL = spellBox.CL;
            LvB = spellBox.LvB;
            LvBFS = spellBox.LvBFS;
            FS = spellBox.FS;
            ES = spellBox.ES;
            FrS = spellBox.FrS;

            CalculateRotation();
        }

        /// <summary>
        /// Invalidates the cached values.
        /// </summary>
        private void Invalidate()
        {
            casts = null;
            lastGetTime = float.PositiveInfinity;
            lastBaseCastTime = float.PositiveInfinity;
            lastWeightedCritchance = float.PositiveInfinity;
            lastWeightedHitchance = float.PositiveInfinity;
            cc = null;
            mps = float.PositiveInfinity;
            dps = float.PositiveInfinity;
        }

        /// <summary>
        /// Calculates a rotation based on the FS>LvB>LB priority.
        /// </summary>
        public void CalculateRotation()
        {
            if (LB == null || FS == null || LvBFS == null || LvB == null)
                return;

            Spell[][] sp = new Spell[4][];
            float[] dps = new float[4];
            
            CalculateRotation(false, false);
            sp[0] = new Spell[spells.Count];
            spells.CopyTo(sp[0]);
            dps[0] = DPS;
            CalculateRotation(true, false);
            sp[1] = new Spell[spells.Count];
            spells.CopyTo(sp[1]);
            dps[1] = DPS;
            CalculateRotation(false, true);
            sp[2] = new Spell[spells.Count];
            spells.CopyTo(sp[2]);
            dps[2] = DPS;
            CalculateRotation(true, true);
            sp[3] = new Spell[spells.Count];
            spells.CopyTo(sp[3]);
            dps[3] = DPS;

            float sdps = 0;
            int pos = 0;
            for (int i = 0; i < dps.Length; i++)
                if (sdps < dps[i])
                    pos = i;
            spells = new List<Spell>(sp[pos]);
        }

        /// <summary>
        /// Calculates a rotation based on the FS>LvB>LB priority.
        /// </summary>
        public void CalculateRotation(bool addlb1, bool addlb2)
        {
            if (Talents == null || LB == null || FS == null || LvBFS == null || LvB == null)
                return;
            spells.Clear();
            Invalidate();

            float LvBreadyAt = 0, FSdropsAt = 0;
            while (true)
            {
                if (GetTime() >= LvBreadyAt) //LvB is ready
                {
                    if (GetTime() + LvBFS.CastTimeWithoutGCD < FSdropsAt) //the LvB cast will be finished before FS runs out
                    {
                        AddSpell(LvBFS);
                        LvBreadyAt = GetTime() + LvBFS.Cooldown;
                    }
                    else if (FSdropsAt == 0) //the first FS
                    {
                        FSdropsAt = GetTime() + FS.Duration;
                        AddSpell(FS);
                    }
                    else //since FS would run out recast FS now -> Rotation end
                        break; //done
                }
                else //LvB is not ready
                {
                    if (GetTime() + LvB.CastTime > FSdropsAt) //FS will run out
                    {
                        if (LvBreadyAt - (GetTime() + FS.CastTime) > LB.CastTime) //there is enough time to fit in another LB and a FS before LvB is ready
                        {
                            AddSpell(LB);
                        }
                        else if (LvBreadyAt - (GetTime() + FS.CastTime) > 0 && addlb2) //FS would still fit in before LvB is ready
                        {
                            AddSpell(LB);
                            break; //FS recast nescessary -> done
                        }
                        else
                        {
                            float waitTime = LvBreadyAt - (GetTime() + FS.CastTime);
                            if (waitTime > 0f)
                                AddSpell(new Wait(waitTime));
                            break; //FS recast nescessary -> done
                        }
                    }
                    else if (LvBreadyAt - GetTime() <= LB.CastTime && !addlb1) //time before the next LvB is lower than LB cast time
                        AddSpell(new Wait(LvBreadyAt - GetTime()));
                    else //LvB is on cooldown, FS won't run out soon
                        AddSpell(LB);
                }
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
        /// Gets the time after the last spell has been cast and the GCD is ready.
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
            for (int j = 0; j < i; j++ )
                time += spells[Mod(j,spells.Count)].CastTime;
            return time;
        }

        private float lastBaseCastTime = float.PositiveInfinity;
        public float GetBaseCastTime()
        {
            if (lastBaseCastTime != float.PositiveInfinity)
                return lastBaseCastTime;
            float time = 0f;
            for (int j = 0; j < spells.Count; j++)
                time += spells[j].BaseCastTime;
            return lastBaseCastTime = time;
        }

        public float getCastsPerSecond()
        {
            return Casts.Count / GetTime();
        }

        public float getCastsPerSecond(Type spellType)
        {
            return getSpells(spellType).Count / GetTime();
        }

        public float getTicksPerSecond(Type spellType)
        {
            float ticks = 0;
            for (int i = 0; i < spells.Count; i++)
            {
                Spell s = spells[i];
                if (spellType.IsInstanceOfType(s))
                {
                    if (s.Duration <= 0)
                        return 0;
                    float durationActive = getNextCastTime(i) - (GetTime(i) + s.CastTimeWithoutGCD);
                    if (durationActive >= s.Duration)
                        ticks += s.PeriodicTicks;
                    else
                        ticks += (float)Math.Floor(durationActive / s.PeriodicTickTime);
                }
            }
            return ticks / GetTime();
        }

        public List<Spell> getSpells(Type spellType)
        {
            return spells.FindAll(delegate(Spell spell) { return spellType.IsInstanceOfType(spell); });
        }

        private float lastWeightedHitchance = float.PositiveInfinity;
        public float getWeightedHitchance()
        {
            if (lastWeightedHitchance != float.PositiveInfinity)
                return lastWeightedHitchance;
            float hitchance = 0f;
            foreach (Spell s in Casts)
                hitchance += s.HitChance;
            return lastWeightedHitchance = hitchance / Casts.Count;
        }

        private float lastWeightedCritchance = float.PositiveInfinity;
        /// <summary>
        /// Returns the average Critchance with Hitchance factored in.
        /// </summary>
        /// <returns></returns>
        public float getWeightedCritchance()
        {
            if (lastWeightedCritchance != float.PositiveInfinity)
                return lastWeightedCritchance;
            float critLB = LB.CritChance;
            float critCL = LB.CritChance * (1f + .11f * Talents.LightningOverload);
            float critchance = 0f;
            foreach (Spell s in Casts)
                critchance += s.HitChance * s.CCCritChance;
            return lastWeightedCritchance = critchance / Casts.Count;
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
        /// Calculates Clearcasting, mana usage, dps and lag per second.
        /// </summary>
        private void calculateProperties()
        {
            mps = 0f; //summing up total manacost
            dps = 0f; //summing up total damage
            lag = 0f;
            cc = new SerializableDictionary<Type, float>(); //clear casting
            spelldps = new SerializableDictionary<Type, float>(); //dps broken up per spell type
            spelltype = new SerializableDictionary<Type, Spell>(); //all used spells by type
            Dictionary<Type, int> count = new Dictionary<Type, int>(); //counting spells
            Spell prev1 = null, prev2 = null;
            for (int i = -2; i < Casts.Count; i++)
            {
                Spell s = getCast(i);
                if(i>=0) //the first two are just saved in order to calculate clear casting
                {
                    float thisdps = 0f;
                    if(!cc.ContainsKey(s.GetType()))
                    {
                        cc.Add(s.GetType(), 0f);
                        spelldps.Add(s.GetType(), 0f);
                        spelltype.Add(s.GetType(), s);
                        count.Add(s.GetType(), 0);
                    }
                    float ccc = 0f;
                    if (Talents.ElementalFocus > 0)
                        ccc = 1f - (1f - prev1.CCCritChance) * (1f - prev2.CCCritChance);
                    mps += s.ManaCost * (1 - .4f * ccc);
                    if (s.Duration > 0) //dot
                    {
                        int j = getSpellNumber(i);
                        float durationActive = getNextCastTime(j) - (GetTime(j) + s.CastTimeWithoutGCD);
                        thisdps = s.HitChance * (s.AvgDamage * (1 + .05f * Talents.ElementalOath * ccc) + s.PeriodicDamage(durationActive)); //bad for FS ticks.
                    }
                    else
                        thisdps = s.HitChance * s.TotalDamage * (1 + .05f * Talents.ElementalOath * ccc); //bad for FS ticks.
                    dps += thisdps;
                    spelldps[s.GetType()] += thisdps;
                    cc[s.GetType()] += ccc;
                    lag += s.Latency;
                    count[s.GetType()]++;
                }
                prev2 = prev1;
                prev1 = s;
            }
            foreach (Type t in count.Keys)
            {
                spelldps[t] /= GetTime();
                cc[t] /= count[t];
            }
            mps /= GetTime(); //divide by rotation time
            dps /= GetTime();
            lag /= GetTime();
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
        /// Spells based.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="spellType"></param>
        /// <returns></returns>
        public float getNextCastTime(int i)
        {
            Type spellType = spells[Mod(i, spells.Count)].GetType();
            int start = Mod(i, spells.Count);
            float time = GetTime(i);
            i++;
            for (int j = start + 1; true; j = Mod(++i, spells.Count))
            {
                if (!spellType.IsInstanceOfType(spells[j]))
                    if (j == start)
                        return float.MaxValue;
                    else
                        time += spells[j].CastTime;
                else
                    return time;
            }
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
                if (castNumber < 0)
                    break;
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
            r += "\n" + Math.Round(GetTime(), 2) + "s\t...";
            return r.Substring(1);
        }

        /// <summary>
        /// An always positive Modulo.
        /// </summary>
        private static int Mod(int a, int n)
        {
            a = a % n;
            if (a < 0)
                a += n;
            return a;
        }
    }
}
