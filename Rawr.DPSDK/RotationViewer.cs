using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.DPSDK
{
    public partial class RotationViewer : Form
    {
        public Rotation rotation;

        public RotationViewer(Rotation r)
        {
            InitializeComponent();
            rotation = r;

         //   cbPresence.Items.Clear();
         //   cbPresence.Items.Add(CalculationOptionsDPSDK.Presence.Blood.ToString());
         //   cbPresence.Items.Add(CalculationOptionsDPSDK.Presence.Unholy.ToString());

            txtBS.KeyUp += new KeyEventHandler(txtBS_KeyUp);
            txtDC.KeyUp += new KeyEventHandler(txtDC_KeyUp);
            txtDuration.KeyUp += new KeyEventHandler(txtDuration_KeyUp);
            txtFS.KeyUp += new KeyEventHandler(txtFS_KeyUp);
            txtHB.KeyUp += new KeyEventHandler(txtHB_KeyUp);
            txtHS.KeyUp += new KeyEventHandler(txtHS_KeyUp);
            txtIT.KeyUp += new KeyEventHandler(txtIT_KeyUp);
            txtNumDisease.KeyUp += new KeyEventHandler(txtNumDisease_KeyUp);
            txtOblit.KeyUp += new KeyEventHandler(txtOblit_KeyUp);
            txtPS.KeyUp += new KeyEventHandler(txtPS_KeyUp);
            txtSS.KeyUp += new KeyEventHandler(txtSS_KeyUp);
            txtUB.KeyUp += new KeyEventHandler(txtUB_KeyUp);
            txtUptime.KeyUp += new KeyEventHandler(txtUptime_KeyUp);
            txtGargoyleDuration.KeyUp += new KeyEventHandler(txtGargoyleDuration_KeyUp);
        }

        private void loadCurRotation()
        {
            txtBS.Text = rotation.BloodStrike.ToString();
            txtDC.Text = rotation.DeathCoil.ToString();
            txtDuration.Text = rotation.curRotationDuration.ToString();
            txtFS.Text = rotation.FrostStrike.ToString();
            txtHB.Text = rotation.HowlingBlast.ToString();
            txtHS.Text = rotation.HeartStrike.ToString();
            txtIT.Text = rotation.IcyTouch.ToString();
            txtNumDisease.Text = rotation.numDisease.ToString();
            txtOblit.Text = rotation.Obliterate.ToString();
            txtPS.Text = rotation.PlagueStrike.ToString();
            txtSS.Text = rotation.ScourgeStrike.ToString();
            txtUB.Text = rotation.UnholyBlight.ToString();
            txtUptime.Text = rotation.diseaseUptime.ToString();
            txtGargoyleDuration.Text = rotation.GargoyleDuration.ToString();
            if (rotation.presence == CalculationOptionsDPSDK.Presence.Unholy)
            {
                rbUnholyPresence.Checked = true;
            }
            else if (rotation.presence == CalculationOptionsDPSDK.Presence.Blood)
            {
                rbBloodPresence.Checked = true;
            }
        }

        private void lockTxt()
        {
            txtBS.ReadOnly = true;
            txtDC.ReadOnly = true;
            txtDuration.ReadOnly = true;
            txtFS.ReadOnly = true;
            txtHB.ReadOnly = true;
            txtHS.ReadOnly = true;
            txtIT.ReadOnly = true;
            txtNumDisease.ReadOnly = true;
            txtOblit.ReadOnly = true;
            txtPS.ReadOnly = true;
            txtSS.ReadOnly = true;
            txtUB.ReadOnly = true;
            txtUptime.ReadOnly = true;
            txtGargoyleDuration.ReadOnly = true;
        }

        private void unLockTxt()
        {
            txtBS.ReadOnly = false;
            txtDC.ReadOnly = false;
            txtDuration.ReadOnly = false;
            txtFS.ReadOnly = false;
            txtHB.ReadOnly = false;
            txtHS.ReadOnly = false;
            txtIT.ReadOnly = false;
            txtNumDisease.ReadOnly = false;
            txtOblit.ReadOnly = false;
            txtPS.ReadOnly = false;
            txtSS.ReadOnly = false;
            txtUB.ReadOnly = false;
            txtUptime.ReadOnly = false;
            txtGargoyleDuration.ReadOnly = false;
        }

        private void RotationViewer_Load(object sender, EventArgs e)
        {
            if (rotation.curRotationType == Rotation.Type.Blood)
            {
                rbBlood.Checked = true;
            }
            else if (rotation.curRotationType == Rotation.Type.Custom)
            {
                rbCustom.Checked = true;
            }
            else if (rotation.curRotationType == Rotation.Type.Frost)
            {
                rbFrost.Checked = true;
            }
            else if (rotation.curRotationType == Rotation.Type.Unholy)
            {
                rbUnholy.Checked = true;
            }
        }

        private void rbUnholy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnholy.Checked)
            {
                unLockTxt();
                rotation.setRotation(Rotation.Type.Unholy);
                loadCurRotation();
            }
        }

        private void rbFrost_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFrost.Checked)
            {
                unLockTxt();
                rotation.setRotation(Rotation.Type.Frost);
                loadCurRotation();
            }
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCustom.Checked)
            {
                unLockTxt();
                rotation.setRotation(Rotation.Type.Custom);
                loadCurRotation();
            }
        }

        private void rbBlood_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlood.Checked)
            {
                unLockTxt();
                rotation.setRotation(Rotation.Type.Blood);
                loadCurRotation();
            }
        }
        private void rbUnholyPresence_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnholyPresence.Checked)
            {
                rotation.presence = CalculationOptionsDPSDK.Presence.Unholy;
            }
        }
        private void rbBloodPresence_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBloodPresence.Checked)
            {
                rotation.presence = CalculationOptionsDPSDK.Presence.Blood;
            }
        }

        //text changes, commits VALID entries into memory

        void txtDC_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.DeathCoil = f;
            }
            catch
            {
                rotation.DeathCoil = 0f;
                t.Text = "0";
            }
        }

        void txtBS_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.BloodStrike = f;
            }
            catch
            {
                rotation.BloodStrike = 0f;
                t.Text = "0";
            }
        }

        void txtDuration_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.curRotationDuration = f;
            }
            catch
            {
                rotation.curRotationDuration = 0f;
                t.Text = "0";
            }
        }

        void txtFS_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.FrostStrike = f;
            }
            catch
            {
                rotation.FrostStrike = 0f;
                t.Text = "0";
            }
        }

        void txtHB_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.HowlingBlast = f;
            }
            catch
            {
                rotation.HowlingBlast = 0f;
                t.Text = "0";
            }
        }

        void txtHS_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.HeartStrike = f;
            }
            catch
            {
                rotation.HeartStrike = 0f;
                t.Text = "0";
            }
        }

        void txtIT_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.IcyTouch = f;
            }
            catch
            {
                rotation.IcyTouch = 0f;
                t.Text = "0";
            }
        }

        void txtNumDisease_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.numDisease = f;
            }
            catch
            {
                rotation.numDisease = 0f;
                t.Text = "0";
            }
        }

        void txtOblit_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.Obliterate = f;
            }
            catch
            {
                rotation.Obliterate = 0f;
                t.Text = "0";
            }
        }

        void txtPS_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.PlagueStrike = f;
            }
            catch
            {
                rotation.PlagueStrike = 0f;
                t.Text = "0";
            }
        }

        void txtSS_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.ScourgeStrike = f;
            }
            catch
            {
                rotation.ScourgeStrike = 0f;
                t.Text = "0";
            }
        }

        void txtUB_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.UnholyBlight = f;
            }
            catch
            {
                rotation.UnholyBlight = 0f;
                t.Text = "0";
            }
        }

        void txtUptime_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;
                rotation.diseaseUptime = f;
            }
            catch
            {
                rotation.diseaseUptime = 0f;
                t.Text = "0";
            }
        }

        void txtGargoyleDuration_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                double d = double.Parse(t.Text);
                float f = (float)d;

                if (f < 0f)
                {
                    f = 0f;
                }
                else if (f > 30f)
                {
                    f = 30f;
                }
                t.Text = f.ToString();

                rotation.GargoyleDuration = f;
            }
            catch
            {
                rotation.GargoyleDuration = 0f;
                t.Text = "0";
            }
        }
    }
}
