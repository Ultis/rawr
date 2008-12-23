using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.TankDK
{
    public partial class CalculationOptionsPanelTankDK : Rawr.CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelTankDK()
        {
            InitializeComponent();
        }

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTankDK();

            options = Character.CalculationOptions as CalculationOptionsTankDK;

            for (int i = 0; i < cmbAttackerLevel.Items.Count; i++)
            {
                if (cmbAttackerLevel.Items[i] as string == options.TargetLevel.ToString())
                {
                    cmbAttackerLevel.SelectedItem = cmbAttackerLevel.Items[i];
                    break;
                }
            }

            numThreatWeight.Value = (decimal)options.ThreatWeight;


            _loadingCalculationOptions = false;
        }


        private bool _loadingCalculationOptions = false;
        private CalculationOptionsTankDK options;

        private void cmbAttackerLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                options.TargetLevel = int.Parse(cmbAttackerLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }

        private void numThreatWeight_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                options.ThreatWeight = (float)(numThreatWeight.Value);
                Character.OnCalculationsInvalidated();
            }
        }
    }

    [Serializable]
    public class CalculationOptionsTankDK : ICalculationOptionBase
    {
        public int TargetLevel = 83;
        public float ThreatWeight = 0.25f;

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankDK));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

    }
}
