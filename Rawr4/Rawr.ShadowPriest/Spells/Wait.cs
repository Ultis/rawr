using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.ShadowPriest.Spells
{
    public class Wait : Spell
    {
        public Wait()
            : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            gcd = 0f;
            missChance = 0f;
            spCoef = 0f;
        }

        public void Initialize(float duration)
        {
            castTime = duration;
            latencyCast = 0;
            latencyGcd = 0;
        }

        public Wait(float duration)
            : this()
        {
            Initialize(duration);
        }

        public override string ToString()
        {
            return "W(" + Math.Round(castTime, 2) + ")";
        }
    }
}

