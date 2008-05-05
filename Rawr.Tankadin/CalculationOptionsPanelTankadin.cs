using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Tankadin
{
    public partial class CalculationOptionsPanelTankadin  : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelTankadin()
		{
			InitializeComponent();
		}

        protected override void LoadCalculationOptions()
        {

            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTankadin();

            CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;

            comboBoxTargetLevel.SelectedItem = (int)calcOpts.TargetLevel;

        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
            calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
            Character.OnItemsChanged();
        }

    }

    [Serializable]
    public class CalculationOptionsTankadin : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankadin));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public bool EnforceMetagemRequirements = false;
        public int TargetLevel = 73;
    }

}
