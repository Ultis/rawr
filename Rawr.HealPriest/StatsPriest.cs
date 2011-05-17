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

namespace Rawr.HealPriest
{
    public class StatsPriest : Stats
    {
        public ePriestSpec PriestSpec { get; set; }

        public bool InnerFire { get; set; }
        public bool InnerWill { get; set; }

        public float ShieldDiscipline { get; set; }
        public float EchoofLight { get; set; }

        public void Accumulate(StatsPriest data)
        {
            base.Accumulate(data);
            this.PriestSpec = data.PriestSpec;
            this.InnerFire = data.InnerFire;
            this.InnerWill = data.InnerWill;
            this.ShieldDiscipline = data.ShieldDiscipline;
            this.EchoofLight = data.EchoofLight;
        }

        // This was called 'Horrible' by Jothay, but he claimed it would work.
        public new StatsPriest Clone()
        {
            StatsPriest clone = new StatsPriest();
            clone.Accumulate(this);
            return clone;
        }
    }
}
