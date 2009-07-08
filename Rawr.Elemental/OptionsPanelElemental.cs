using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public partial class CalculationOptionsPanelElemental : CalculationOptionsPanelBase
    {
        private bool loading = false;

        public CalculationOptionsPanelElemental()
        {
            InitializeComponent();

            // Set about text
            tbModuleNotes.Text =
                "Notes:\r\n" +
                "For the estimator, it is assumed you use Flametongue weapon and Water Shield.\r\n" +
                "The estimator is a simple estimation at the moment and only uses a rotation with Flameshock, Lava Blast and Lightning Bolt.\r\n" +
                "Trinkets, Elemental Mastery and Clearcasting are modelled by calculating their average value during the fight.\r\n" +
                "\r\n" +
                "Assumed rotation:\r\n"+
                "- Lava Burst whenever off cooldown and only with Flame Shock on.\r\n" +
                "- In case of no Glyph of Flameshock, recast Flame Shock immediately after Lava Burst, else cast Flame Shock only when the DoT falls off.\r\n" +
                "- Cast Thunderstorm whenever available if using Thunderstorm\r\n" +
                "- Cast Lightning Bolt whenever there's nothing else to do.";
        }

        protected override void LoadCalculationOptions()
        {
            loading = true;

            CalculationOptionsElemental calcOpts;

            if (Character.CalculationOptions == null)
            {
                calcOpts = new CalculationOptionsElemental();
                Character.CalculationOptions = calcOpts;
            }
            else
                calcOpts = (CalculationOptionsElemental)Character.CalculationOptions;

            tbBSRatio.Value = calcOpts.BSRatio;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";

            trkFightLength.Value = calcOpts.FightDuration / 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: " + m + ":" + s;
            cmbManaAmt.SelectedIndex = calcOpts.ManaPot;
            tkReplenishment.Value = calcOpts.ReplenishmentUptime;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";

            cbThunderstorm.Checked = calcOpts.UseThunderstorm;

            cbNup.Checked = (calcOpts.rotationType & 1) == 0;
            cbMup.Checked = (calcOpts.rotationType & 2) == 0;

            loading = false;
        }


        private float parseFloat(string s)
        {
            float tmp;
            float.TryParse(s, out tmp);

            if (tmp > 0)
                return tmp;
            
            return 0;
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.FightDuration = trkFightLength.Value * 15;
            int m = trkFightLength.Value / 4;
            int s = calcOpts.FightDuration - 60 * m;
            lblFightLength.Text = "Fight duration: "+m+":"+s;
            Character.OnCalculationsInvalidated();
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.ManaPot = cmbManaAmt.SelectedIndex;
            Character.OnCalculationsInvalidated();
        }

        private void tkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.ReplenishmentUptime = tkReplenishment.Value;
            lblReplenishment.Text = tkReplenishment.Value + "% of fight spent with Replenishment.";
            Character.OnCalculationsInvalidated();
        }

        private void tbBSRatio_Scroll(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.BSRatio = tbBSRatio.Value;
            int burst = 100 - tbBSRatio.Value;
            int sust = tbBSRatio.Value;
            lblBSRatio.Text = "Ratio: "+burst + "% Burst, "+sust + "% Sustained.";
            Character.OnCalculationsInvalidated();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            calcOpts.UseThunderstorm = cbThunderstorm.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cbNupdown_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            if (cbNup.Checked) calcOpts.rotationType &= ~1;
            else calcOpts.rotationType |= 1;
            Character.OnCalculationsInvalidated();
        }

        private void cbMupdown_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            CalculationOptionsElemental calcOpts = Character.CalculationOptions as CalculationOptionsElemental;
            if (cbMup.Checked) calcOpts.rotationType &= ~2;
            else calcOpts.rotationType |= 2;
            Character.OnCalculationsInvalidated();
        }
    }

    [Serializable]
    public class CalculationOptionsElemental : ICalculationOptionBase
    {
        [System.Xml.Serialization.XmlIgnore]
        public CharacterCalculationsElemental calculatedStats = null;

        public int BSRatio = 75; // goes from 0 to 100

        public int FightDuration = 300; //5 Minutes
        public int ManaPot = 0; // none
        public int ReplenishmentUptime = 30;
        public bool UseThunderstorm = true;

        public int rotationType = 0;

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsElemental));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
