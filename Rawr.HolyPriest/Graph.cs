using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Rawr.HolyPriest
{
    public partial class Graph : Form
    {
        private List<Spell>[] spellList;

        public Graph(List<Spell>[] spellList)
        {
            this.spellList = spellList;
            InitializeComponent();
        }

       
        private void Graph_Load(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void RefreshGraph()
        {
            GraphPane gp1 = zg1.GraphPane;
            GraphPane gp2 = zg2.GraphPane;

            gp1.Title.Text = "Healing per second";
            gp1.XAxis.Title.Text = "Rank";
            gp1.YAxis.Title.Text = "HpS";
            gp2.Title.Text = "Healing per mana";
            gp2.XAxis.Title.Text = "Rank";
            gp2.YAxis.Title.Text = "HpM";

            for (int i = 0; i < spellList.Length; i++)
            {
                if (spellList[i][0].AvgHeal == 0)
                    continue;

                PointPairList list = new PointPairList();
                PointPairList list2 = new PointPairList();

                foreach (Spell spell in spellList[i])
                {
                    double x = spell.Rank;
                    double y = spell.HpS;
                    double y2 = spell.HpM;
                    list.Add(x, y);
                    list2.Add(x, y2);
                }

                LineItem li = gp1.AddCurve(spellList[i][0].Name,
                list, spellList[i][0].GraphColor, SymbolType.Circle);
                li.Symbol.Fill = new Fill(spellList[i][0].GraphColor);
                li = gp2.AddCurve(spellList[i][0].Name,
                    list2, spellList[i][0].GraphColor, SymbolType.Circle);
                li.Symbol.Fill = new Fill(spellList[i][0].GraphColor);
                chLines.Items.Add(spellList[i][0].Name, true);

                if (spellList[i][0].Targets > 3 || spellList[i][0].Name == "Heal")
                {
                    zg1.GraphPane.CurveList[i].IsVisible = false;
                    chLines.SetItemChecked(i, false);
                }
            }

            gp1.XAxis.MajorGrid.IsVisible = true;
            gp2.XAxis.MajorGrid.IsVisible = true;

            zg1.IsShowPointValues = true;
            zg1.PointValueFormat = "0.00";
            zg1.AxisChange();
            zg1.Invalidate();
            zg2.IsShowPointValues = true;
            zg2.PointValueFormat = "0.00";
            zg2.AxisChange();
            zg2.Invalidate();
        }

        private void chLines_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!(sender is CheckedListBox))
                return;
            
            zg1.GraphPane.CurveList[e.Index].IsVisible = e.NewValue == CheckState.Checked;
            zg2.GraphPane.CurveList[e.Index].IsVisible = e.NewValue == CheckState.Checked;
            zg1.AxisChange();
            zg1.Invalidate();
            zg2.AxisChange();
            zg2.Invalidate();
        }
    }
}
