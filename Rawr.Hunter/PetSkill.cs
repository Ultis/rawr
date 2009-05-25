using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public class PetSkill
    {
        public double CD;
        public int Cost;
        public int Talent;
        public int Type;
        public int Min;
        public int Max;

        public PetSkill(double cd, int cost, int talent, int type)
        {
            CD = cd;
            Cost = cost;
            Talent = talent;
            Type = type;
            Min = 0;
            Max = 0;
        }

        public PetSkill(double cd, int cost, int talent, int type, int min, int max)
        {
            CD = cd;
            Cost = cost;
            Talent = talent;
            Type = type;
            Min = min;
            Max = max;
        }
    }
}
