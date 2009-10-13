using System;

namespace Rawr.Elemental.Spells
{
    public enum SpellIndex
    {
        LightningBolt,
        ChainLightning,
        ChainLightning2,
        ChainLightning3,
        ChainLightning4,
        LavaBurst,
        LavaBurstFS,
        FlameShock,
        EarthShock,
        FrostShock,
        Thunderstorm
    }

    /// <summary>
    /// A container class to hold an instance of every spell that can be used in a rotation. It also supplies methods to update these spells to new stats and talents.
    /// The idea is to save memory by reusing the spells.
    /// </summary>
    public class SpellBox
    {
        private Spell[] spells;
        private bool EMapplied = false;

        public SpellBox(Stats stats, ShamanTalents talents)
        {
            spells = new Spell[11];
            spells[(int)SpellIndex.LightningBolt] = new LightningBolt(stats, talents);
            spells[(int)SpellIndex.ChainLightning] = new ChainLightning(stats, talents, 0);
            spells[(int)SpellIndex.ChainLightning2] = new ChainLightning(stats, talents, 1);
            spells[(int)SpellIndex.ChainLightning3] = new ChainLightning(stats, talents, 2);
            spells[(int)SpellIndex.ChainLightning4] = new ChainLightning(stats, talents, 3);
            spells[(int)SpellIndex.LavaBurst] = new LavaBurst(stats, talents, 0);
            spells[(int)SpellIndex.LavaBurstFS] = new LavaBurst(stats, talents, 1);
            spells[(int)SpellIndex.FlameShock] = new FlameShock(stats, talents);
            spells[(int)SpellIndex.EarthShock] = new EarthShock(stats, talents);
            spells[(int)SpellIndex.FrostShock] = new FrostShock(stats, talents);
            spells[(int)SpellIndex.Thunderstorm] = new Thunderstorm(stats, talents);
        }

        public void Update(Stats stats, ShamanTalents talents)
        {
            foreach (Spell s in spells)
            {
                if (s != null)
                    s.Update(stats, talents);
            }
            EMapplied = false;
        }

        public void ApplyEM(float modifier)
        {
            if (EMapplied)
                return;

            foreach (Spell s in spells)
            {
                if (s != null)
                    s.ApplyEM(modifier);
            }
            EMapplied = true;
        }

        public Spell Get(SpellIndex spellIndex)
        {
            return spells[(int)spellIndex];
        }

        /// <summary>
        /// The underlying spell array.
        /// This is a reference, not a copy.
        /// </summary>
        public Spell[] Array
        {
            get { return spells; }
        }

        #region Properties for typed access
        public LightningBolt LB
        {
            get { return (LightningBolt)spells[(int)SpellIndex.LightningBolt]; }
        }

        public ChainLightning CL
        {
            get { return (ChainLightning)spells[(int)SpellIndex.ChainLightning]; }
        }

        public ChainLightning CL2
        {
            get { return (ChainLightning)spells[(int)SpellIndex.ChainLightning2]; }
        }

        public ChainLightning CL3
        {
            get { return (ChainLightning)spells[(int)SpellIndex.ChainLightning3]; }
        }

        public ChainLightning CL4
        {
            get { return (ChainLightning)spells[(int)SpellIndex.ChainLightning4]; }
        }

        public LavaBurst LvB
        {
            get { return (LavaBurst)spells[(int)SpellIndex.LavaBurst]; }
        }

        public LavaBurst LvBFS
        {
            get { return (LavaBurst)spells[(int)SpellIndex.LavaBurstFS]; }
        }

        public FlameShock FS
        {
            get { return (FlameShock)spells[(int)SpellIndex.FlameShock]; }
        }

        public EarthShock ES
        {
            get { return (EarthShock)spells[(int)SpellIndex.EarthShock]; }
        }

        public FrostShock FrS
        {
            get { return (FrostShock)spells[(int)SpellIndex.FrostShock]; }
        }
        
        public Thunderstorm TS
        {
            get { return (Thunderstorm)spells[(int)SpellIndex.Thunderstorm]; }
        }
        #endregion
    }
}
