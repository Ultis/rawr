using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    /*public enum Spell : int
    {
        Lifebloom,
        LifebloomStack,
        Rejuventation,
        Regrowth,
        HealingTouch
    }

    public class SpellCalculations
    {
        private Spell spell;

        private float baseCastTime;

        private float hotCoef;
        private float directCoef;

        private float hotBaseHeal;
        private float directBaseHeal;

        private float castTime;

        private int baseManaCost;

        public SpellCalculations(Spell spell)
        {
            this.spell = spell;

            if (spell == Spell.Lifebloom)
            {
                hotCoef = 0.249f;
                directCoef = 0;
                hotBaseHeal = 273;
                directBaseHeal = 600;

                baseManaCost = 220;
            }
            else if (spell == Spell.LifebloomStack)
            {
                hotCoef = 0.249f*3;
                directCoef = 0;
                
                hotBaseHeal = 273*3;
                directBaseHeal = 600;

                baseManaCost = 220;
            }
            else if (spell == Spell.Rejuventation)
            {
                hotCoef = 0.8f;
                directCoef = 0;

                hotBaseHeal = 1060;
                directBaseHeal = 0;

                baseManaCost = 415;
            }
            else if (spell == Spell.Regrowth)
            {
                hotCoef = 0.3f;
                directCoef = 0.7f;

                hotBaseHeal = 1274;
                directBaseHeal = 1323.5f;

                baseManaCost = 675;
            }
            else if (spell == Spell.HealingTouch)
            {
                hotCoef = 0;
                directCoef = 1;

                hotBaseHeal = 0;
                directBaseHeal = 2960.5f;

                baseManaCost = 935;
            }
        }


        public int getHealAmount(Stats stats)
        {
            return (int) Math.Floor(hotBaseHeal + hotCoef * stats.Healing + directBaseHeal + directCoef * stats.Healing);
        }

        // TODO
        public void Calculate(Stats stats)
        { }

        public float CastTime { get { return castTime; } }
    }*/
}
