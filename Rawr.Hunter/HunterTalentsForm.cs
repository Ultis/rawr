using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Hunter
{
    public partial class HunterTalentsForm : Form
    {
        #region Instance Variables

        private CalculationOptionsPanelHunter basePanel;
        private bool calculationSuspended = false;

        #endregion

        #region Constructors

        public HunterTalentsForm(CalculationOptionsPanelHunter basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        #endregion
        
        #region Properties

        public Character Character
        {
            get { return basePanel.Character; }
        }

        #endregion

        #region Event Handlers

        private void HunterTalentsForm_Load(object sender, EventArgs e)
        {
            LoadTalents();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Tag as string;

			//if (Character.AllTalents.Trees.ContainsKey(cb.Parent.Text) && !String.IsNullOrEmpty(talent))
			//{
			//    List<TalentItem> talents = Character.AllTalents.Trees[cb.Parent.Text];
			//    TalentItem item = null;
			//    for (int i = 0; i < Character.AllTalents.Trees[cb.Parent.Text].Count; i++)
			//    {
			//        if (string.Compare(talents[i].Name.Trim(), talent.Trim(), true) == 0)
			//        {
			//            item = talents[i];
			//            break;
			//        }
			//    }

			//    if (item != null)
			//    {
			//        item.PointsInvested = Convert.ToInt32(cb.SelectedItem);
			//        if (!calculationSuspended)
			//        {
			//            Character.OnItemsChanged();
			//        }
			//    }
			//}
        }

        private void HunterTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        #endregion


		private void LoadTalents()
		{
			calculationSuspended = true;
			foreach (Control c in Controls)
			{
				if (c is GroupBox)
				{
					foreach (Control cc in c.Controls)
					{
						if (cc is ComboBox)
						{
							ComboBox cb = (ComboBox)cc;
							string talent = cb.Tag as string;

							//if (Character.AllTalents.Trees.ContainsKey(c.Text) && !String.IsNullOrEmpty(talent))
							//{
							//    List<TalentItem> talents =  Character.AllTalents.Trees[c.Text];
							//    TalentItem ti = null;
							//    for(int i=0;i<talents.Count;i++)
							//    {
							//        if (string.Compare(talents[i].Name.Trim(), talent.Trim(), true) == 0)
							//        {
							//            ti = talents[i];
							//            break;
							//        }
							//    }
							//    if (ti != null)
							//    {
							//        cb.SelectedItem = ti.PointsInvested.ToString();
							//    }
							//}
						}
					}
				}
			}
			calculationSuspended = false;
			ComputeTalentTotals();
		}

		private void ComputeTalentTotals()
		{
			List<string> totals = new List<string>();
			foreach (Control c in Controls)
			{
				if (c is GroupBox)
				{
					int total = 0;
					foreach (Control cc in c.Controls)
					{
						if (cc is ComboBox)
						{
							ComboBox cb = (ComboBox)cc;
							string talent = cb.Tag as string;
							//if (Character.AllTalents.Trees.ContainsKey(c.Text) && !String.IsNullOrEmpty(talent))
							//{
							//    List<TalentItem> talents = Character.AllTalents.Trees[c.Text];
							//    TalentItem ti = null;
							//    for (int i = 0; i < talents.Count; i++)
							//    {
							//        if (string.Compare(talents[i].Name.Trim(), talent.Trim(), true) == 0)
							//        {
							//            ti = talents[i];
							//            break;
							//        }
							//    }
							//    if (ti != null)
							//    {
							//        total += ti.PointsInvested;
							//    }
							//}
						}
					}
					totals.Add(total.ToString());
				}
			}
			totals.Reverse();
			Text = "Hunter Talents (" + string.Join("/", totals.ToArray()) + ")";
		}

    }
}
