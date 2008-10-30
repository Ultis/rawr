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
    }
}
