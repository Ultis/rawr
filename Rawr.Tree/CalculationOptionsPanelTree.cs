using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
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
                Character.CalculationOptions = new CalculationOptionsTree(Character);

            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            cmbLength.Value = (decimal)calcOpts.FightLength;
            cmbManaAmt.Text = calcOpts.ManaPotAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaPotDelay;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            chkInnervate.Checked = calcOpts.InnervateSelf;
            upDownInnervate.Value = (decimal) calcOpts.InnervateDelay;

            upDownTargetHealth.Value = (decimal)calcOpts.TargetHealth;
            upDownSurvScalingAbove.Value = (decimal)calcOpts.SurvScalingAbove;
            upDownSurvScalingBelow.Value = (decimal)calcOpts.SurvScalingBelow;

            upDownAvgHeal.Value = (decimal)calcOpts.AverageHealingScaling;

            cmbNumCyclesPerRotation.Text = calcOpts.NumCyclesPerRotation.ToString();
            cmbSpellNum.Text = "1";

            upDownMaxCycleDuration.Value = (decimal) calcOpts.MaxCycleDuration;

            if (calcOpts.ShattrathFaction == "Scryer")
            {
                shattScryer.Checked = true;
            }
            else if (calcOpts.ShattrathFaction == "Aldor")
            {
                shattAldor.Checked = true;
            }
            else
            {
                calcOpts.ShattrathFaction = "None";
                shattNone.Checked = true;
            }

            loading = false;

            cmbSpellNum_SelectedIndexChanged(null, null);
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaPotAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.ManaPotDelay = (float)cmbManaTime.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaPotAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownSurvScalingAbove_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.SurvScalingAbove = (float)upDownSurvScalingAbove.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownSurvScalingBelow_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.SurvScalingBelow = (float)upDownSurvScalingBelow.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownInnervate_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.InnervateDelay = (float)upDownInnervate.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownTargetHealth_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.TargetHealth = (float) upDownTargetHealth.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbSpellNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                int selIx = int.Parse(cmbSpellNum.Text) - 1; // arrays are zero-based

                loading = true; // disable updates while changing the spell listBox

                spellList.ClearSelected();

                if (calcOpts.availableSpells[selIx] != null)
                {

                    foreach (String s in calcOpts.availableSpells[selIx])
                    {
                        int ix = spellList.FindStringExact(s);
                        if (ix != -1)
                            spellList.SetSelected(ix, true);
                    }
                }

                loading = false;
            }
        }

        private void spellList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                int selIx = int.Parse(cmbSpellNum.Text) - 1; // arrays are zero-based

                if (spellList.SelectedItems.Count == 0)
                    return;

                String[] spells = new String[spellList.SelectedItems.Count];

                for(int i=0; i<spells.Length; i++) {
                    spells[i] = spellList.SelectedItems[i].ToString();
                }

                calcOpts.availableSpells[selIx] = spells;
                
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkInnervate_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.InnervateSelf = chkInnervate.Checked;

                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownMaxCycleDuration_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.MaxCycleDuration = (float) upDownMaxCycleDuration.Value;

                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbNumCyclesPerRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.NumCyclesPerRotation = int.Parse(cmbNumCyclesPerRotation.Text);

                Character.OnCalculationsInvalidated();
            }
        }

        private void shattNone_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && shattNone.Checked)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.ShattrathFaction = "None";

                Character.OnCalculationsInvalidated();
            }
        }

        private void shattScryer_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && shattScryer.Checked)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.ShattrathFaction = "Scryer";

                Character.OnCalculationsInvalidated();
            }
        }

        private void shattAldor_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && shattAldor.Checked)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.ShattrathFaction = "Aldor";

                Character.OnCalculationsInvalidated();
            }
        }

        private void upDownAvgHeal_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;

                calcOpts.AverageHealingScaling = (float)upDownAvgHeal.Value;

                Character.OnCalculationsInvalidated();
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

        public float FightLength = 5;
        public float ManaPotAmt = 2400;
        public float ManaPotDelay = 2.5f;
        public float TargetHealth = 8500;
        public float SurvScalingAbove = 1000;
        public float SurvScalingBelow = 100;
        public float Spriest = 0;
        public float InnervateDelay = 6.5f;
        public float MaxCycleDuration = 6.5f;
        public int   NumCyclesPerRotation = 2;
        public String[][] availableSpells = new String[][] {
            new String[] {"Lifebloom Stack"},
            new String[] {"Rejuvenation", "Regrowth"},
            new String[] {"Regrowth", "Lifebloom (no aura)", "Rejuvenation (no aura)", "Regrowth (no aura)", "Healing Touch", "Nothing"},
            new String[] {"Lifebloom (no aura)", "Rejuvenation (no aura)", "Regrowth (no aura)", "Healing Touch", "Nothing"},
            new String[] {"Nothing"},
            new String[] {"Nothing"},
        };
        public Boolean InnervateSelf = true;
        public string ShattrathFaction = "None";
        public float AverageHealingScaling = 0.8f;

        public CalculationOptionsTree()
        {
        }

        public CalculationOptionsTree(Character character)
        {
        }
    }
}
