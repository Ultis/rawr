using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Tree
{
    public partial class CalculationOptionsPanelTree : CalculationOptionsPanelBase
    {
        private bool loading = false; 

        public CalculationOptionsPanelTree()
        {
            InitializeComponent();
        }

        protected override void LoadCalculationOptions()
        {
            loading = true;
    
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTree();

            CalculationOptionsTree calcOpts = (CalculationOptionsTree)Character.CalculationOptions;

            cbLevel.SelectedIndex = calcOpts.level == 70 ? 0 : 1;
            cbSchattrathFaction.SelectedIndex = calcOpts.ShattrathFaction == "Aldor" ? 1 : calcOpts.ShattrathFaction == "None" ? 0 : 2;
            cbSpellRotation.SelectedIndex = calcOpts.spellRotationPlaceholder == "Healing Touch" ? 0 : 1;
            tbSurvHAbove.Text = calcOpts.SurvScaleAboveLife.ToString();
            tbSurvHBelow.Text = calcOpts.SurvScaleBelowLife.ToString();
            tbSurvTargetH.Text = calcOpts.SurvTargetLife.ToString();
            tbAverageHealingUsage.Text = calcOpts.averageSpellpowerUsage.ToString();
            chbLivingSeed.Checked = calcOpts.useLivingSeedAsCritMultiplicator;
            chbReplenishment.Checked = calcOpts.haveReplenishSupport;

            loading = false;

            Character.OnCalculationsInvalidated();
        }

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).level = int.Parse((string)cbLevel.Items[cbLevel.SelectedIndex]);
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbSchattrathFaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).ShattrathFaction = cbSchattrathFaction.Items[cbSchattrathFaction.SelectedIndex].ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbSurvHBelow_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp;
                float.TryParse(tbSurvHBelow.Text, out tmp);

                if (tmp > 0)
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvScaleBelowLife = tmp;

                if (tbSurvHBelow.Text == "0")
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvScaleBelowLife = 0f;

                Character.OnCalculationsInvalidated();
            }
        }

        private void tbSurvTargetH_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp;
                float.TryParse(tbSurvTargetH.Text, out tmp);

                if (tmp > 0)
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvTargetLife = tmp;

                if (tbSurvTargetH.Text == "0")
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvTargetLife = 0f;

                Character.OnCalculationsInvalidated();
            }
        }

        private void tbSurvHAbove_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp;
                float.TryParse(tbSurvHAbove.Text, out tmp);

                if (tmp > 0)
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvScaleAboveLife = tmp;

                if (tbSurvHAbove.Text == "0")
                    ((CalculationOptionsTree)Character.CalculationOptions).SurvScaleAboveLife = 0f;
            }
        }

        private void chbReplenishment_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
                ((CalculationOptionsTree)Character.CalculationOptions).haveReplenishSupport = chbReplenishment.Checked;
        }

        private void tbAverageHealingUsage_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                float tmp;
                float.TryParse(tbAverageHealingUsage.Text, out tmp);

                ((CalculationOptionsTree)Character.CalculationOptions).averageSpellpowerUsage = tmp;
            }
        }

        private void cbSpellRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                ((CalculationOptionsTree)Character.CalculationOptions).spellRotationPlaceholder = (string)cbSpellRotation.Items[cbSpellRotation.SelectedIndex];
                Character.OnCalculationsInvalidated();
            }
        }
    }

    [Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase
    {
        //Dummy implementation of future options

        public int level = 70;
        public string ShattrathFaction = "Aldor";
        public bool useLivingSeedAsCritMultiplicator = true;

        //Target Life 8500
        //Points Per Stamina < 8500 = 10
        //Points Per Stamina > 8500 = 1
        public float SurvTargetLife = 8500f;
        public float SurvScaleBelowLife = 10f;
        public float SurvScaleAboveLife = 100f;

        //Add Average Spellpower to Calculation = 0.0f (% used)
        public float averageSpellpowerUsage = 0.0f;

        //'Shadowpriest'-Support
        public bool haveReplenishSupport = true;

        //Spellrotations
        public string spellRotationPlaceholder = "Healing Touch";

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
