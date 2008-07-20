using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
    internal partial class RaidISB : Form
    {
        private int numWarlocks;
        private int numShadowPriests;
        private CalculationOptionsPanelWarlock basePanel;

        public Character Character
        {
            get
            {
                return basePanel.Character;
            }
        }

        public RaidISB(CalculationOptionsPanelWarlock bp)
        {
            basePanel = bp;
            InitializeComponent();
        }

        private void comboBoxNumWarlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newValue = comboBoxNumWarlocks.SelectedIndex;
            for (int i = 0; i < newValue; i++)
                this.groupBoxWarlocks.Controls["suWarlock" + (i + 1)].Enabled = true;
            for (int i = newValue; i < 5; i++)
                this.groupBoxWarlocks.Controls["suWarlock" + (i + 1)].Enabled = false;

            numWarlocks = newValue;
        }

        private void comboBoxNumShadowPriests_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newValue = comboBoxNumShadowPriests.SelectedIndex;
            for (int i = 0; i < newValue; i++)
                this.groupBoxShadowPriests.Controls["suShadowPriest" + (i + 1)].Enabled = true;
            for (int i = newValue; i < 5; i++)
                this.groupBoxShadowPriests.Controls["suShadowPriest" + (i + 1)].Enabled = false;

            numShadowPriests = newValue;
        }

        public void LoadRaid()
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            numWarlocks = options.NumRaidWarlocks;
            numShadowPriests = options.NumRaidShadowPriests;
            comboBoxNumWarlocks.SelectedIndex = numWarlocks;
            comboBoxNumShadowPriests.SelectedIndex = numShadowPriests;

            for (int i = 0; i < 5; i++)
            {
                WarlockControl control = (WarlockControl)this.groupBoxWarlocks.Controls["suWarlock" + (i + 1)];
                SUWarlock warlock = options.RaidWarlocks[i];

                control.HitPercent = warlock.HitPercent;
                control.CritPercent = warlock.CritPercent;
                control.SbCastTime = warlock.SbCastTime;
                control.SbCastRatio = warlock.SbCastRatio;
                control.ShadowDps = warlock.ShadowDps;
            }

            for (int i = 0; i < 5; i++)
            {
                ShadowPriestControl control = (ShadowPriestControl)this.groupBoxShadowPriests.Controls["suShadowPriest" + (i + 1)];
                SUShadowPriest sp = options.RaidShadowPriests[i];

                control.HitPercent = sp.HitPercent;
                control.MbFrequency = sp.MbFrequency;
                control.ShadowDps = sp.ShadowDps;
            }
        }

        public void SaveRaid()
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            options.NumRaidWarlocks = numWarlocks;
            options.NumRaidShadowPriests = numShadowPriests;

            for (int i = 0; i < 5; i++)
            {
                WarlockControl control = (WarlockControl)this.groupBoxWarlocks.Controls["suWarlock" + (i + 1)];
                SUWarlock warlock = options.RaidWarlocks[i];

                warlock.HitPercent = control.HitPercent;
                warlock.CritPercent = control.CritPercent;
                warlock.SbCastTime = control.SbCastTime;
                warlock.SbCastRatio = control.SbCastRatio;
                warlock.ShadowDps = control.ShadowDps;
            }

            for (int i = 0; i < 5; i++)
            {
                ShadowPriestControl control = (ShadowPriestControl)this.groupBoxShadowPriests.Controls["suShadowPriest" + (i + 1)];
                SUShadowPriest sp = options.RaidShadowPriests[i];

                sp.HitPercent = control.HitPercent;
                sp.MbFrequency = control.MbFrequency;
                sp.ShadowDps = control.ShadowDps;
            }
        }

        public float CalculateISBUptime(CharacterCalculationsWarlock calculations)
        {
            float chanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, calculations.HitPercent);
            float myDirectShadowHitsPerSecond = calculations.SpellRotation.ShadowBoltCastRatio / calculations.SpellRotation.ShadowBoltCastTime * chanceToHit;
            float myEffectiveCritRate = myDirectShadowHitsPerSecond * calculations.CritPercent;

            float raidDirectShadowHitsPerSecond = 0, raidEffectiveCritRate = 0, raidShadowDps = 0;
            //for (int i = 0; i <= lastEnabledWarlock; i++)
            //{
            //    WarlockControl currentWarlock = (WarlockControl)this.groupBoxWarlocks.Controls["suWarlock" + (i + 1)];
            //    currentWarlock.Calculate(calculations);
            //    raidDirectShadowHitsPerSecond += currentWarlock.DirectShadowHitsPerSecond;
            //    raidEffectiveCritRate += currentWarlock.EffectiveCritRate;
            //    raidShadowDps += currentWarlock.ShadowDps;
            //}
            //for (int i = 0; i <= lastEnabledShadowPriest; i++)
            //{
            //    ShadowPriestControl currentSp = (ShadowPriestControl)this.groupBoxShadowPriests.Controls["suShadowPriest" + (i + 1)];
            //    currentSp.Calculate(calculations);
            //    raidDirectShadowHitsPerSecond += currentSp.DirectShadowHitsPerSecond;
            //    raidShadowDps += currentSp.ShadowDps;
            //}

            float raidIsbUptime = 1 - (float)Math.Pow(raidEffectiveCritRate, 4);
            float totalIsbUptime = 1 - (float)Math.Pow(raidEffectiveCritRate + myEffectiveCritRate, 4);

            calculations.RaidDpsFromIsb = raidShadowDps * 1.2f * (totalIsbUptime - raidIsbUptime);

            return totalIsbUptime;
        }
    }
}
