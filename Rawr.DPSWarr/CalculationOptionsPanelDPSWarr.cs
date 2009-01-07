using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Rawr.DPSWarr
{
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelDPSWarr()
        {
            InitializeComponent();
        }
        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsDPSWarr(Character);
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            TargetArmorEdit.Text = calcOpts.TargetArmor.ToString();
            FightLengthEdit.Text = calcOpts.FightLength.ToString();
            HeroicStrikeRageEdit.Text = calcOpts.HeroicStrikeRage.ToString();
            SimModeCombo.SelectedIndex = calcOpts.SimMode;
        }

        private void FightLengthEdit_TextChanged(object sender, EventArgs e)
        {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (FightLengthEdit.TextLength > 0)
                calcOpts.FightLength = int.Parse(FightLengthEdit.Text);
            else
                calcOpts.FightLength = 1;
            Character.OnCalculationsInvalidated();
        }

        private void HeroicStrikeRageEdit_TextChanged(object sender, EventArgs e)
        {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (HeroicStrikeRageEdit.TextLength > 0)
                calcOpts.HeroicStrikeRage = int.Parse(HeroicStrikeRageEdit.Text);
            else
                calcOpts.HeroicStrikeRage = 0;
            Character.OnCalculationsInvalidated();
        }

        private void TargetArmorEdit_TextChanged(object sender, EventArgs e)
        {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            if (TargetArmorEdit.TextLength > 0)
                calcOpts.TargetArmor = int.Parse(TargetArmorEdit.Text);
            else
                calcOpts.TargetArmor = 0;
            Character.OnCalculationsInvalidated();
        }

        private void SimModeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.SimMode = SimModeCombo.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }
    }


	[Serializable]
	public class CalculationOptionsDPSWarr : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public CalculationOptionsDPSWarr() {}
		public CalculationOptionsDPSWarr(Character character) : this()
		{
		}

		public int TargetArmor = 13000;
		public int FightLength = 240;
        public int SimMode = 0;
        public int HeroicStrikeRage = 60;

	}
}
