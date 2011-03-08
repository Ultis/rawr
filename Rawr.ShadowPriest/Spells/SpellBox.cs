using System;

namespace Rawr.ShadowPriest.Spells
{
    public enum SpellIndex
    {
        DevouringPlauge,
        MindBlast,
        MindFlay,
        ShadowFiend,
        ShadowWordDeath,
        ShadowWordPain,
        VampiricTouch,
        PowerWordShield,
        MindSpike,
    }

    /// <summary>
    /// A container class to hold an instance of every spell that can be used in a rotation. It also supplies methods to update these spells to new stats and talents.
    /// The idea is to save memory by reusing the spells.
    /// </summary>
    public class SpellBox
    {
        private Spell[] spells;

        public SpellBox() //ISpellArgs args)
        {
            spells = new Spell[9];
            spells[(int)SpellIndex.DevouringPlauge] = new DevouringPlauge();
            spells[(int)SpellIndex.MindBlast] = new MindBlast();
            spells[(int)SpellIndex.MindFlay] = new MindFlay();
            spells[(int)SpellIndex.ShadowFiend] = new ShadowFiend();
            spells[(int)SpellIndex.ShadowWordDeath] = new ShadowWordDeath();
            spells[(int)SpellIndex.ShadowWordPain] = new ShadowWordPain();
            spells[(int)SpellIndex.VampiricTouch] = new VampiricTouch();
            spells[(int)SpellIndex.PowerWordShield] = new PowerWordShield();
            spells[(int)SpellIndex.MindSpike] = new MindSpike();

        }
        public SpellBox(ISpellArgs args)
        {
            spells = new Spell[10];
            spells[(int)SpellIndex.DevouringPlauge] = new DevouringPlauge(args);
            spells[(int)SpellIndex.MindBlast] = new MindBlast(args);
            spells[(int)SpellIndex.MindFlay] = new MindFlay(args);
            spells[(int)SpellIndex.ShadowFiend] = new ShadowFiend(args);
            spells[(int)SpellIndex.ShadowWordDeath] = new ShadowWordDeath(args);
            spells[(int)SpellIndex.ShadowWordPain] = new ShadowWordPain(args);
            spells[(int)SpellIndex.VampiricTouch] = new VampiricTouch(args);
            spells[(int)SpellIndex.PowerWordShield] = new PowerWordShield(args);
            spells[(int)SpellIndex.MindSpike] = new MindSpike(args);

        }


        public void Update(ISpellArgs args)
        {
            foreach (Spell s in spells)
            {
                if (s != null)
                    s.Update(args);
            }
        }
        
        public Spell Get(SpellIndex spellIndex)
        {
            return spells[(int)spellIndex];
        }

        /// <summary>
        /// The underlying spell array.
        /// This is a reference, not a copy.
        /// </summary>
        public Spell[] Spells
        {
            get { return spells; }
        }

        #region Properties for typed access
        public MindSpike Spike
        {
            get { return (MindSpike)spells[(int)SpellIndex.MindSpike]; }
        }

        public DevouringPlauge DP
        {
            get { return (DevouringPlauge)spells[(int)SpellIndex.DevouringPlauge]; }
        }

        public MindBlast MB
        {
            get { return (MindBlast)spells[(int)SpellIndex.MindBlast]; }
        }

        public MindFlay MF
        {
            get { return (MindFlay)spells[(int)SpellIndex.MindFlay]; }
        }

        public ShadowFiend Fiend
        {
            get { return (ShadowFiend)spells[(int)SpellIndex.ShadowFiend]; }
        }

        public ShadowWordDeath SWD
        {
            get { return (ShadowWordDeath)spells[(int)SpellIndex.ShadowWordDeath]; }
        }

        public ShadowWordPain SWP
        {
            get { return (ShadowWordPain)spells[(int)SpellIndex.ShadowWordPain]; }
        }

        public VampiricTouch VT
        {
            get { return (VampiricTouch)spells[(int)SpellIndex.VampiricTouch]; }
        }

        public PowerWordShield PWS
        {
            get { return (PowerWordShield)spells[(int)SpellIndex.PowerWordShield]; }
        }
        #endregion
    }
}
