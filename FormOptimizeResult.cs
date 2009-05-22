using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormOptimizeResult : Form 
    {
        private DialogResult result = DialogResult.No;

        public FormOptimizeResult(Character before, Character after)
        {
            InitializeComponent();
            paperDollBefore.Character = before.Clone();
            paperDollAfter.Character = after.Clone();
            RemoveUnchangedItems();
        }

        private void RemoveUnchangedItems()
        {
            Enchant noEnchant = Enchant.FindEnchant(0, Item.ItemSlot.None, paperDollBefore.Character);
            paperDollBefore.Shirt  = null;
            paperDollBefore.Tabard = null;
            paperDollAfter.Shirt = null;
            paperDollAfter.Tabard = null;

            if (paperDollBefore.Head.Equals(paperDollAfter.Head))
            {
                paperDollBefore.Head = null;
                paperDollAfter.Head = null;
            }
            if (paperDollBefore.Neck.Equals(paperDollAfter.Neck))
            {
                paperDollBefore.Neck = null;
                paperDollAfter.Neck = null;
            }
            if (paperDollBefore.Shoulders.Equals(paperDollAfter.Shoulders))
            {
                paperDollBefore.Shoulders = null;
                paperDollAfter.Shoulders = null;
            }
            if (paperDollBefore.Back.Equals(paperDollAfter.Back))
            {
                paperDollBefore.Back = null;
                paperDollAfter.Back = null;
            }
            if (paperDollBefore.Chest.Equals(paperDollAfter.Chest))
            {
                paperDollBefore.Chest = null;
                paperDollAfter.Chest = null;
            }
            if (paperDollBefore.Wrist.Equals(paperDollAfter.Wrist))
            {
                paperDollBefore.Wrist = null;
                paperDollAfter.Wrist = null;
            }
            if (paperDollBefore.MainHand.Equals(paperDollAfter.MainHand))
            {
                paperDollBefore.MainHand = null;
                paperDollAfter.MainHand = null;
            }
            if (paperDollBefore.OffHand.Equals(paperDollAfter.OffHand))
            {
                paperDollBefore.OffHand = null;
                paperDollAfter.OffHand = null;
            }
            if (paperDollBefore.Ranged.Equals(paperDollAfter.Ranged))
            {
                paperDollBefore.Ranged = null;
                paperDollAfter.Ranged = null;
            }
            if (paperDollBefore.Projectile.Equals(paperDollAfter.Projectile))
            {
                paperDollBefore.Projectile = null;
                paperDollAfter.Projectile = null;
            }
            if (paperDollBefore.ProjectileBag.Equals(paperDollAfter.ProjectileBag))
            {
                paperDollBefore.ProjectileBag = null;
                paperDollAfter.ProjectileBag = null;
            }
            if (paperDollBefore.Hands.Equals(paperDollAfter.Hands))
            {
                paperDollBefore.Hands = null;
                paperDollAfter.Hands = null;
            }
            if (paperDollBefore.Waist.Equals(paperDollAfter.Waist))
            {
                paperDollBefore.Waist = null;
                paperDollAfter.Waist = null;
            }
            if (paperDollBefore.Legs.Equals(paperDollAfter.Legs))
            {
                paperDollBefore.Legs = null;
                paperDollAfter.Legs = null;
            }
            if (paperDollBefore.Feet.Equals(paperDollAfter.Feet))
            {
                paperDollBefore.Feet = null;
                paperDollAfter.Feet = null;
            }
            if (paperDollBefore.Finger1.Equals(paperDollAfter.Finger1))
            {
                paperDollBefore.Finger1 = null;
                paperDollAfter.Finger1 = null;
            }
            if (paperDollBefore.Finger2.Equals(paperDollAfter.Finger2))
            {
                paperDollBefore.Finger2 = null;
                paperDollAfter.Finger2 = null;
            }
            if (paperDollBefore.Trinket1.Equals(paperDollAfter.Trinket1))
            {
                paperDollBefore.Trinket1 = null;
                paperDollAfter.Trinket1 = null;
            }
            if (paperDollBefore.Trinket2.Equals(paperDollAfter.Trinket2))
            {
                paperDollBefore.Trinket2 = null;
                paperDollAfter.Trinket2 = null;
            }

            // now check swapped slot rings & trinkets
            if (paperDollBefore.Finger1.Equals(paperDollAfter.Finger2))
            {
                paperDollBefore.Finger1 = null;
                paperDollAfter.Finger2 = null;
            }
            if (paperDollBefore.Finger2.Equals(paperDollAfter.Finger1))
            {
                paperDollBefore.Finger2 = null;
                paperDollAfter.Finger1 = null;
            }
            if (paperDollBefore.Trinket1.Equals(paperDollAfter.Trinket2))
            {
                paperDollBefore.Trinket1 = null;
                paperDollAfter.Trinket2 = null;
            }
            if (paperDollBefore.Trinket2.Equals(paperDollAfter.Trinket1))
            {
                paperDollBefore.Trinket2 = null;
                paperDollAfter.Trinket1 = null;
            }
        }

        public void SetOptimizerScores(float scoreBefore, float scoreAfter)
        {
            paperDollBefore.SetResultText(string.Format("Score Before Optimization :\r\n{0}", scoreBefore));
            if (scoreAfter >= 0)
                paperDollAfter.SetResultText(string.Format("Score After Optimization :\r\n{0}", scoreAfter));
            else
                paperDollAfter.SetResultText("The Optimizer was not able to meet all the requirements. Would you like to equip the gear that is closest to meeting them?");
        }

        public DialogResult LoadOptimizerResult()
        {
            return result;
        }

        private void buttonKeep_Click(object sender, EventArgs e)
        {
            result = DialogResult.No;
            this.Close();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            this.Close();
        }
    }
}
