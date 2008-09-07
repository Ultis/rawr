using System;
using System.Collections;

namespace Rawr.Rogue {
    [Serializable]
    public class Cycle {
        private string cycle;
        private ArrayList components;

        public Cycle() {
            cycle = "";
            components = new ArrayList();
        }

        public Cycle(string c) {
            cycle = c;
            components = new ArrayList();
            Parse(cycle);
        }

        public CycleComponent this[int index] {
            get {
                if (components.Count < 1 || index < 0 || index > components.Count - 1)
                    return null;
                return (CycleComponent)components[index];
            }
        }

        public int this[char finisher] {
            get {
                for (int i = 0; i < components.Count; i++)
                    if (((CycleComponent)components[i]).Finisher == finisher)
                        return ((CycleComponent)components[i]).Rank;
                return 0;
            }
        }

        public int TotalComboPoints {
            get {
                int ret = 0;
                for (int i = 0; i < components.Count; i++)
                    ret += ((CycleComponent)components[i]).Rank;
                return ret;
            }
        }

        private void Parse(string c) {
            int i;

            for(i = 0 ; i < (c.Length - 1) ; i += 2) {
                if ((c[i] < '1' || c[i] > '5') || (c[i + 1] != 's' && c[i + 1] != 'r' && c[i + 1] != 'e')){
                    i--;
                    continue;
                }

                components.Add(new CycleComponent(c[i] - '0', c[i + 1]));
           }
        }

        public override string ToString() {
            string ret;

            ret = "";
            for (int i = 0; i < components.Count; i++)
                ret += ((CycleComponent)components[i]).ToString();

            return ret;
        }
    }

    [Serializable]
    public class CycleComponent {
        private int rank;
        private char finisher;

        public CycleComponent() {
            rank = 0;
            finisher = 's';
        }

        public CycleComponent(int r, char f) {
            rank = r;
            finisher = f;
        }

        public int Rank {
            get { return rank; }
            set { rank = value; }
        }

        public char Finisher {
            get { return finisher; }
            set { finisher = value; }
        }

        public override string ToString() {
            return rank.ToString() + finisher.ToString();
        }
    }
}