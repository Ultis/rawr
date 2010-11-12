using System;
using System.Collections.Generic;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public class Rotation
    {
        private List<Spell> spells;
        private List<Spell> casts;

        public List<Spell> Casts
        {
            get
            {
                if (casts == null)
                    calculateCastSpells();
                return casts;
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

        public DevouringPlauge DP;
        public MindBlast MB;
        public MindFlay MF;
        public ShadowFiend Fiend;
        public ShadowWordDeath SWD;
        public ShadowWordPain SWP;
        public VampiricTouch VT;

        public PriestTalents Talents;

        public Rotation()
        {
            spells = new List<Spell>(15);
        }

        public Rotation(SpellBox spellBox, PriestTalents talents, IRotationOptions rotOpt)
            : this()
        {
            //Talents = talents;
            DP = spellBox.DP;
            MB = spellBox.MB;
            MF = spellBox.MF;
            Fiend = spellBox.Fiend;
            SWD = spellBox.SWD;
            SWP = spellBox.SWP;
            VT = spellBox.VT;

            //useXXX = rotOpt.UseDpsFireTotem;

            CalculateRotation();
        }

        /// <summary>
        /// Invalidates the cached values.
        /// </summary>
        private void Invalidate()
        {
            casts = null;
            lastGetTime = float.PositiveInfinity;
            lastDuration = float.PositiveInfinity;
            lastBaseCastTime = float.PositiveInfinity;
            mps = float.PositiveInfinity;
            dps = float.PositiveInfinity;
        }

        int VTdrops = -1, DPdrops = -1, SWPdrops = -1; //count how many dots drop off (starts at -1 because of initial application
        /// <summary>
        /// Calculates the rotation
        /// </summary>
        public void CalculateRotation()
        {
            if (DP == null || MB == null || MF == null || Fiend == null || SWD == null || SWP == null || VT == null)// || Talents == null) 
                return;
            spells.Clear();
            Invalidate();

            float VTreadyAt = 0, DPreadyAt = 0, SWPreadyAt = 0, MBreadyAt = 0, FiendreadyAt = 0, MFreadyAt = 0; //set when spells are ready
            float VTdropsAt = 0, DPdropsAt = 0, SWPdropsAt =0; //set when dots drop

            //MAX DPS Rotation (no SWD): prio - VT, SWP, DP, Fiend, MB, MF
            while (true)
            {
                #region VT
                if (GetTime() >= VTreadyAt) //VT is ready 
                {
                    if (GetTime() + VT.CastTime < VTdropsAt) //is within window for VT refresh
                    {
                        AddSpell(VT);
                        VTreadyAt = GetTime() + (VT.DebuffDuration - VT.TickPeriod);
                        VTdropsAt = GetTime() + VT.DebuffDuration;
                    }
                    else
                    {
                        AddSpell(VT);
                        VTreadyAt = GetTime() + (VT.DebuffDuration - VT.TickPeriod);
                        VTdropsAt = GetTime() + VT.DebuffDuration;
                        VTdrops++;
                    }
                }
                #endregion
                #region SWP
                else if (GetTime() >= SWPreadyAt) //VT didnt need to be cast
                {
                    if (GetTime() + SWP.CastTime < SWPdropsAt) //is within window for SWP refresh
                    {
                        AddSpell(SWP);
                        SWPreadyAt = GetTime() + (SWP.DebuffDuration - SWP.TickPeriod);
                        SWPdropsAt = GetTime() + SWP.DebuffDuration;
                    }
                    else
                    {
                        AddSpell(SWP);
                        SWPreadyAt = GetTime() + (SWP.DebuffDuration - SWP.TickPeriod);
                        SWPdropsAt = GetTime() + VT.DebuffDuration;
                        SWPdrops++;
                    }
                }
                #endregion
                #region DP
                else if (GetTime() >= DPreadyAt) //SWP didnt need to be cast
                {
                    if (GetTime() + DP.CastTime < DPdropsAt) //is within window for SWP refresh
                    {
                        AddSpell(DP);
                        DPreadyAt = GetTime() + (DP.DebuffDuration - DP.TickPeriod);
                        DPdropsAt = GetTime() + DP.DebuffDuration;
                    }
                    else
                    {
                        AddSpell(DP);
                        DPreadyAt = GetTime() + (DP.DebuffDuration - DP.TickPeriod);
                        DPdropsAt = GetTime() + DP.DebuffDuration;
                        DPdrops++;
                    }
                }
                #endregion
                #region Fiend
                else if (GetTime() >= FiendreadyAt) //DP didnt need to be cast
                {
                        AddSpell(Fiend);
                        FiendreadyAt = GetTime() + Fiend.Cooldown + Fiend.CastTime;
                }
                #endregion
                #region MB
                else if (GetTime() >= MBreadyAt) //Fiend didnt need to be cast
                {
                    AddSpell(MB);
                    MBreadyAt = GetTime() + MB.Cooldown + MB.CastTime;
                }
                #endregion
                #region MF
                else //MB didnt need to be cast
                {
                    AddSpell(MF);
                    MFreadyAt = GetTime() + MF.Cooldown + MF.CastTime;

                    //TODO: chance to refresh SWP
                    
                    //TODO: reduce CD on Fiend
                }
                #endregion
                if (GetTime() > 60f) break;

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
        /// Gets the time before spell number i has been cast (with GCD) counting 0 as the first one.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetTime(int i)
        {
            float time = 0f;
            for (int j = 0; j < i; j++)
                time += spells[Mod(j, spells.Count)].CastTime;
            return time;
        }

        private float lastDuration = float.PositiveInfinity;
        /// <summary>
        /// The duration of one rotation pass. The total casting time is modified by totem uptime weighted totem casting times.
        /// </summary>
        public float Duration
        {
            get
            {
                if (lastDuration != float.PositiveInfinity)
                    return lastDuration;
                return lastDuration = GetTime();
            }
        }

        private float lastBaseCastTime = float.PositiveInfinity;
        public float GetBaseCastTime()
        {
            if (lastBaseCastTime != float.PositiveInfinity)
                return lastBaseCastTime;
            float time = 0f;
            for (int j = 0; j < spells.Count; j++)
                time += spells[j].CastTime;
            return lastBaseCastTime = time;
        }

        public List<Spell> getSpells(Type spellType)
        {
            return spells.FindAll(delegate(Spell spell) { return spellType.IsInstanceOfType(spell); });
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
        /// Calculates mana usage, dps and lag per second.
        /// </summary>
        private void calculateProperties()
        {
            mps = 0f; //summing up total manacost
            dps = 0f; //summing up total damage
            lag = 0f;
            spelldps = new SerializableDictionary<Type, float>(); //dps broken up per spell type
            spelltype = new SerializableDictionary<Type, Spell>(); //all used spells by type
            for (int i = -2; i < Casts.Count; i++)
            {
                Spell s = getCast(i);
                if (i >= 0)
                dps += s.AverageDamage;
                mps += s.ManaCost;
                lag += s.Latency;
            }
            mps /= Duration; //divide by rotation time
            dps /= Duration;
            lag /= Duration;
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
            for (i = 0; castNumber >= 0; i++)
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
                    if (prev.Length > 0)
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
                r += "\n" + Math.Round(GetTime(i), 2) + "s\t" + spells[i].ToString();
            }
            r += "\n" + Math.Round(GetTime(), 2) + "s\t...";
            r += "\n" + "VT Drops:" + VTdrops + "\t";
            r += "\n" + "SWP Drops:" + SWPdrops + "\t";
            r += "\n" + "DP Drops:" + DPdrops + "\t";


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
