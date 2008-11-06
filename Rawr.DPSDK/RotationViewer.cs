using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

            txtBS.KeyPress += new KeyPressEventHandler(txtBS_KeyPress);
            txtDC.KeyPress += new KeyPressEventHandler(txtDC_KeyPress);
            txtDuration.KeyPress += new KeyPressEventHandler(txtDuration_KeyPress);
            txtFS.KeyPress += new KeyPressEventHandler(txtFS_KeyPress);
            txtHB.KeyPress += new KeyPressEventHandler(txtHB_KeyPress);
            txtHS.KeyPress += new KeyPressEventHandler(txtHS_KeyPress);
            txtIT.KeyPress += new KeyPressEventHandler(txtIT_KeyPress);
            txtNumDisease.KeyPress += new KeyPressEventHandler(txtNumDisease_KeyPress);
            txtOblit.KeyPress += new KeyPressEventHandler(txtOblit_KeyPress);
            txtPS.KeyPress += new KeyPressEventHandler(txtPS_KeyPress);
            txtSS.KeyPress += new KeyPressEventHandler(txtSS_KeyPress);
            txtUB.KeyPress += new KeyPressEventHandler(txtUB_KeyPress);
            txtUptime.KeyPress += new KeyPressEventHandler(txtUptime_KeyPress);
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
                lockTxt();
                rotation.setRotation(Rotation.Type.Unholy);
                loadCurRotation();
            }
        }

        private void rbFrost_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFrost.Checked)
            {
                lockTxt();
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

        private void rbUnholy_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnholy.Checked)
            {
                lockTxt();
                rotation.setRotation(Rotation.Type.Blood);
                loadCurRotation();
            }
        }

        //text changes, commits VALID entries into memory

        void txtDC_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtBS_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtFS_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtHB_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtHS_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtIT_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtNumDisease_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtOblit_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtPS_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtSS_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtUB_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }

        void txtUptime_KeyPress(object sender, KeyPressEventArgs e)
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
                t.Text = "";
            }
        }
    }
}
