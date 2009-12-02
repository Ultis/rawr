using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    public class FireElemental
    {
        private float _ap = 0f;
        private float _sp = 0f;
        private float _int = 0f;
        private float totaldps = 0f;

        public FireElemental(float ap, float sp, float intellect)
        {
            _ap = ap; _sp = sp; _int = intellect;
            totaldps = 0f;
        }

        public float getDPS()
        {
            return totaldps;
        }

        public string getDPSOutput()
        {
            return string.Format("{0}*Some dps analysis text will go here", totaldps);
        }
    }
}
