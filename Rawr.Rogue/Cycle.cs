using System;
using System.Collections;
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
                for (var i = 0; i < components.Count; i++)
                    if (components[i].Finisher.Id == finisher)
                        return components[i].Rank;
                return 0;
            }
        }

        public int TotalComboPoints
        {
            get
            {
                var ret = 0;
                for (var i = 0; i < components.Count; i++)
                    ret += (components[i]).Rank;
                return ret;
            }
        }

        private void Parse(string c)
        {
            int i;

            for (i = 0; i < (c.Length - 1); i += 2)
            {
                if ((c[i] < '1' || c[i] > '5') || Finishers.Get(c[i + 1]).Id == 'z' )
                {
                    i--;
                    continue;
                }

                components.Add(new CycleComponent(c[i] - '0', Finishers.Get(c[i + 1])));
            }
        }

        public override string ToString()
        {
            var ret = "";
            for (var i = 0; i < components.Count; i++)
                ret += (components[i]).ToString();

            return ret;
        }

        public List<CycleComponent> Components
        {
            get
            {
                return components;   
            }
        }
    }

    [Serializable]
    public class CycleComponent
    {
        private IFinisher _finisher;
        private int _rank;

        public CycleComponent()
        {
            _rank = 0;
            _finisher = new SnD();
        }

        public CycleComponent(int r, IFinisher f)
        {
            _rank = r;
            _finisher = f;
        }

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public IFinisher Finisher
        {
            get { return _finisher; }
            set { _finisher = value; }
        }

        public override string ToString()
        {
            return _rank + _finisher.Id.ToString();
        }
    }
}