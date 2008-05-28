using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Tree
{
    public partial class CalculationOptionsPanelTree : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelTree()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTree();

            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            cmbLength.Value = (decimal)calcOpts.Length;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;

            trkActivity.Value = (int)calcOpts.Activity;
            lblActivity.Text = trkActivity.Value + "%";

            loading = false;
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.Length = (float)cmbLength.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void trkActivity_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                lblActivity.Text = trkActivity.Value + "%";
                calcOpts.Activity = trkActivity.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnItemsChanged();
            }
        }
    }
    [Serializable]
	public class CalculationOptionsTree : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public bool EnforceMetagemRequirements = false;
		public float Length = 5;
		public float ManaAmt = 2400;
		public float ManaTime = 2.5f;
		public float Activity = 80;
		public float Spriest = 0;
	}
}
