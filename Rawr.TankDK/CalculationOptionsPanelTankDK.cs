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

            _loadingCalculationOptions = false;
        }


        private bool _loadingCalculationOptions = false;
    }

    [Serializable]
    public class CalculationOptionsTankDK : ICalculationOptionBase
    {
        public int TargetLevel = 83;

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
