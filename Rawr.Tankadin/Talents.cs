using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tankadin
{
    public class Talents
    {

        public Talents()
        {
            //Prot Talents
            Deflection = 5;
            Anticipation = 20;
            CombatExpertise = 1.1f;
            SacredDuty = 1.06f;
            ShieldSpecializaiton = 1.3f;
            Thoughness = 1.1f;
            //Holy Talents
            /*Deflection = 0;
            Anticipation = 0;
            CombatExpertise = 1;
            SacredDuty = 1;
            ShieldSpecializaiton = 1;
            Thoughness = 1.08f;*/
        }

        public float Deflection { get; set; }
        public float Anticipation { get; set; }
        public float CombatExpertise { get; set; }
        public float SacredDuty { get; set; }
        public float ShieldSpecializaiton { get; set; }
        public float Thoughness { get; set; }

    }
}
