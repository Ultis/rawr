using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Healadin
{
    public class CalculationOptionsHealadin : ICalculationOptionBase
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHealadin));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public CalculationOptionsHealadin()
        {
            Length = 7;
            ManaAmt = 4300;
            Activity = .85f;
            Replenishment = .9f;
            DivinePlea = 2f;
            BoLUp = 1f;
            BoLEff = .2f;
            HolyShock = .15f;
            BurstScale = .4f;
            GHL_Targets = 1f;
            InfusionOfLight = true;
            IoLHolyLight = .9f;
            JotP = true;
            LoHSelf = false;
            SSUptime = 1f;
        }

        public float Length { get; set; }

        [Display(Name="Mana Potion Amount")]
        public float ManaAmt { get; set; }

        public float Activity { get; set; }
        public float Replenishment { get; set; }
        public float DivinePlea { get; set; }
        public float BoLUp { get; set; }
        public float BoLEff { get; set; }
        public float HolyShock { get; set; }
        public float BurstScale { get; set; }
        public float GHL_Targets { get; set; }

        public bool InfusionOfLight { get; set; }
        public float IoLHolyLight { get; set; }

        public bool JotP { get; set; }
        public bool LoHSelf { get; set; }

        public float SSUptime { get; set; }

    }
}
