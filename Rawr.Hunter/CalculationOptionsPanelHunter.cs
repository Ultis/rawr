using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Hunter
{
    public partial class CalculationOptionsPanelHunter : CalculationOptionsPanelBase
    {

        private bool Running;

        public CalculationOptionsPanelHunter()
		{
			InitializeComponent();
		}

        protected override void LoadCalculationOptions()
        {
            Running = true;
            if (Character.CalculationOptions == null)Character.CalculationOptions = new CalculationOptionsHunter();
 
            CalculationOptionsHunter calcOpts = Character.CalculationOptions as CalculationOptionsHunter;
         
            // options placeholder

            Running = false;
        }

        private void calculationOptionControl_Changed(object sender, EventArgs e)
        {
            if (!Running) 
            { 
                CalculationOptionsHunter calcOpts = Character.CalculationOptions as CalculationOptionsHunter;

                Character.OnItemsChanged();
            }
        }
    }

    [Serializable]
    public class CalculationOptionsHunter : ICalculationOptionBase
    {
        public string GetXml()
            {
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHunter));
                StringBuilder xml = new StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(xml);
                serializer.Serialize(writer, this);
                return xml.ToString();
            }

        public int TargetLevel = 73;
        public int TargetArmor = 7700;
    }
}
