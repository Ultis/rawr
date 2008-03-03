using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Healadin
{


    public class Spell
    {
        private string name;
        private int rank;

        private float baseHeal;
        private float baseMana;
        private float baseCastTime;
        private float coef;
        private float downrank;
        private float bolBonus;
        private float healMultiple;

        private float hps;
        private float mps;
        private float hpm;
        private float spellCrit;
        private float castTime;
        private float avgHeal;

        private static float[] HLMANA = new float[] { 0, 1, 2, 3, 190, 275, 365, 465, 580, 660, 710, 840 };
        private static float[] HLHEAL = new float[] { 0, 1, 2, 3, 345, 537.5f, 758, 1022, 1343, 1709, 1872, 2321 };
        private static float[] HLLEVEL = new float[] { 0, 1, 2, 3, 22, 30, 38, 46, 54, 60, 62, 70 };

        public Spell(string name, int rank)
        {
            this.name = name;
            this.rank = rank;
            healMultiple = 1.12f;
            if (name.Equals("Holy Light"))
            {
                baseMana = HLMANA[rank];
                baseHeal = HLHEAL[rank];
                downrank = Math.Min((HLLEVEL[rank] + 11) / 70f, 1f);
                coef = 2.5f / 3.5f;
                baseCastTime = 2;
                bolBonus = 580;
            }
            else
            {
                baseMana = 180;
                baseHeal = 485.5f;
                baseCastTime = 1.5f;
                coef = 1.5f / 3.5f;
                downrank = 1;
                bolBonus = 185;
            }
        }

        public void Calculate(Stats stats)
        {
            Calculate(stats, false);   
        }

        public void Calculate(Stats stats, bool di)
        {
            spellCrit = .08336f + (stats.Intellect / 8000) + (stats.SpellCritRating / 2208);
            castTime = baseCastTime / (1 + (stats.SpellHasteRating / 1570));
            
            if (name.Equals("Holy Light"))
            {
                spellCrit += .11f;
            }
            else
            {
                healMultiple *= 1.05f;
            }

            avgHeal = healMultiple * (baseHeal + (((stats.Healing * coef) + bolBonus) * downrank));
            hps = avgHeal * (1 + (.5f * spellCrit)) / castTime;
            mps = ((baseMana * (di ? .5f : 1f)) - (.6f * baseMana * spellCrit)) / castTime;
            hpm = hps / mps;
        }

        public string Name { get { return name; } }
        public int Rank { get { return rank; } }

        public float Hps { get { return hps; } }
        public float Mps { get { return Mps; } }
        public float Hpm { get { return hpm; } }
        public float SpellCrit { get { return spellCrit; } }
        public float CastTime { get { return castTime; } }
        public float AverageHeal { get { return avgHeal; } }

    }

}
