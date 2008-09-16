using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.HolyPriest
{
	public partial class TalentForm : Form
	{
		private CalculationOptionsPanelHolyPriest optionsPanel;
		public Character Character
		{
			get
			{
				return optionsPanel.Character;
			}
			set
			{
				optionsPanel.Character = value;
			}
		}
		public TalentForm(CalculationOptionsPanelHolyPriest optionsPanel)
		{
			this.optionsPanel = optionsPanel;
			InitializeComponent();
			panel1.Resize += new EventHandler(panel1_Resize);
			panel2.Resize += new EventHandler(panel1_Resize);
			panel3.Resize += new EventHandler(panel1_Resize);

		}
		void panel1_Resize(object sender, EventArgs e)
		{
			int xbuffer = 10;
			int y = panel1.Location.Y;
			panel2.Location = new Point(panel1.Location.X + panel1.Width + 10, y);
			panel3.Location = new Point(panel2.Location.X + panel2.Width + 10, y);
			label1.Location = new Point(panel1.Location.X, panel1.Location.Y - label1.Height - 2);
			label2.Location = new Point(panel2.Location.X, panel1.Location.Y - label1.Height - 2);
			label3.Location = new Point(panel3.Location.X, panel1.Location.Y - label1.Height - 2);
		}

		//public void SetParameters(TalentTree tree, Character.CharacterClass charClass)
		//{
		//    buildTrees(tree, charClass);
		//}


		//private void buildTrees(TalentTree tree, Character.CharacterClass CharClass)
		//{
		//    if (tree != null)
		//    {
		//        //TalentTree trees, List<TalentPanel> panels, List<Label> labels, Character.CharacterClass charclass
		//        Panel[] panels = new Panel[] { panel1, panel2, panel3 };
		//        Label[] labels = new Label[] { label1, label2, label3 };
		//        int index = 0;
		//        foreach (string treeName in tree.Trees.Keys)
		//        {
		//            buildPanel(panels[index], tree.Trees[treeName], CharClass, labels[index], treeName);
		//            index++;
		//        }
		//        this.Width = panel3.Location.X + panel1.Location.X + panel3.Width; ;
		//        this.Height = panel1.Location.Y  + panel1.Height + 40;
		//        this.Text = CharClass + " talent tree";

		//    }
		//}

		//private void buildPanel(Panel p, List<TalentItem> tals, Character.CharacterClass charclass, Label treeLabel, string treeName)
		//{
		//    int points = 0;
		//    int _vertbuffer = 5;
		//    int _horizbuffer = 2;
		//    int maxVert = 0;
		//    tals.ForEach(delegate(TalentItem ti) { maxVert = ti.VerticalPosition > maxVert ? ti.VerticalPosition : maxVert; });

		//    int maxHoriz = 0;
		//    tals.ForEach(delegate(TalentItem ti) { maxHoriz = ti.HorizontalPosition > maxHoriz ? ti.HorizontalPosition : maxHoriz; });

		//    TalentIcon temp = new TalentIcon(this);

		//    p.Width = ((maxHoriz) * _horizbuffer + maxHoriz * temp.Width);
		//    p.Height = ((maxVert) * _vertbuffer + (maxVert) * temp.Height) ;


		//    int vertoffset = 0;
		//    for (int row = 1; row <= maxVert; row++)
		//    {
		//        int horizoffset = 0;
		//        vertoffset += _vertbuffer;
		//        int maxCurrHoriz = 0;
		//        tals.ForEach(delegate(TalentItem ti) { if (ti.VerticalPosition == row) maxCurrHoriz = ti.HorizontalPosition > maxCurrHoriz ? ti.HorizontalPosition : maxCurrHoriz; });
		//        for (int col = 1; col <= maxCurrHoriz; col++)
		//        {
		//            horizoffset += _horizbuffer;
		//            //temp = new TalentIcon(tals.Find(ti => ti.VerticalPosition == row && ti.HorizontalPosition == col), CharClass);
		//            TalentItem ti = tals.Find(delegate(TalentItem ti2) { return (ti2.VerticalPosition == row && ti2.HorizontalPosition == col); });
		//            if (ti != null)
		//            {
		//                points += ti.PointsInvested;
		//                temp = new TalentIcon(this, ti, charclass);
		//                temp.Location = new Point(horizoffset, vertoffset);
		//                p.Controls.Add(temp);
		//            }
		//            horizoffset += temp.Width;
		//        }
		//        vertoffset += temp.Height;
		//    }
		//    treeLabel.Text = treeName + " (" + points.ToString() + ")";
		//}

	}

}
