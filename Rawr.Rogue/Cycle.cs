using System;
using System.Collections.Generic;

namespace Rawr.Rogue
{
    [Serializable]
    public class Cycle
    {
        private readonly List<CycleComponent> components = new List<CycleComponent>();
        private readonly string cycle = "";

        public Cycle()
        {
        }

        public Cycle(string c)
        {
            cycle = c;
            Parse(cycle);
        }

        public CycleComponent this[int index]
        {
            get
            {
                if (components.Count < 1 || index < 0 || index > components.Count - 1)
                    return null;
                return components[index];
            }
        }

        public int this[char finisher]
        {
            get
            {
                for (int i = 0; i < components.Count; i++)
                    if ((components[i]).Finisher == finisher)
                        return (components[i]).Rank;
                return 0;
            }
        }

        public int TotalComboPoints
        {
            get
            {
                int ret = 0;
                for (int i = 0; i < components.Count; i++)
                    ret += (components[i]).Rank;
                return ret;
            }
        }

        private void Parse(string c)
        {
            int i;

            for (i = 0; i < (c.Length - 1); i += 2)
            {
                if ((c[i] < '1' || c[i] > '5') || (c[i + 1] != 's' && c[i + 1] != 'r' && c[i + 1] != 'e'))
                {
                    i--;
                    continue;
                }

                components.Add(new CycleComponent(c[i] - '0', c[i + 1]));
            }
        }

        public override string ToString()
        {
            string ret;

            ret = "";
            for (int i = 0; i < components.Count; i++)
                ret += (components[i]).ToString();

            return ret;
        }
    }

    [Serializable]
    public class CycleComponent
    {
        private char finisher;
        private int rank;

        public CycleComponent()
        {
            rank = 0;
            finisher = 's';
        }

        public CycleComponent(int r, char f)
        {
            rank = r;
            finisher = f;
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public char Finisher
        {
            get { return finisher; }
            set { finisher = value; }
        }

        public override string ToString()
        {
            return rank + finisher.ToString();
        }
    }
}