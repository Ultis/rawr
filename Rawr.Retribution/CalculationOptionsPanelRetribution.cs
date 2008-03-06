using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
           
        }
        protected override void LoadCalculationOptions()
        {
            if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
                Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("BossArmor"))
                Character.CalculationOptions["BossArmor"] = "7700";
            if (!Character.CalculationOptions.ContainsKey("FightLength"))
                Character.CalculationOptions["FightLength"] = "10";
            if (!Character.CalculationOptions.ContainsKey("Exorcism"))
                Character.CalculationOptions["Exorcism"] = "0";
            if (!Character.CalculationOptions.ContainsKey("ConsecRank"))
                Character.CalculationOptions["ConsecRank"] = "0";
            if (!Character.CalculationOptions.ContainsKey("Seal"))
                Character.CalculationOptions["Seal"] = "1";
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
                Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

            comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            txtArmor.Text = Character.CalculationOptions["BossArmor"];
            trackBarFightLength.Value = int.Parse(Character.CalculationOptions["FightLength"]);
            if (Character.CalculationOptions["Exorcism"] == "1")
            {
                checkBoxExorcism.Checked = true;
            }
            else
            {
                checkBoxExorcism.Checked = false;
            }
            if (Character.CalculationOptions["ConsecRank"] == "0")
            {
                checkBoxConsecration.Checked = false;
            }
            else
            {
                checkBoxConsecration.Checked = true;
                comboBoxConsRank.SelectedItem = "Rank " + Character.CalculationOptions["ConsecRank"];
            }
            if (Character.CalculationOptions["Seal"] == "1")
            {
                rbSoB.Checked = true;
            }
            else
            {
                rbSoC.Checked = true;
            }
            

        }
        

        private void rbSoC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSoC.Checked)
            {
                Character.CalculationOptions["Seal"] = "0";
            }
            else
            {
                Character.CalculationOptions["Seal"] = "1";
            }
            Character.OnItemsChanged();
        }

        private void rbSoB_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSoB.Checked)
            {
                Character.CalculationOptions["Seal"] = "1";
            }
            else
            {
                Character.CalculationOptions["Seal"] = "0";
            }
            Character.OnItemsChanged();

        }

        private void checkBoxConsecration_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxConsecration.Checked)
            {
                comboBoxConsRank.Enabled = true;
                comboBoxConsRank.SelectedItem = "Rank 1";
                Character.CalculationOptions["ConsecRank"] = "1";
                
            }
            else
            {
                comboBoxConsRank.Enabled = false;
                Character.CalculationOptions["ConsecRank"] = "0";
            }
            Character.OnItemsChanged();
        }

        private void comboBoxConsRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ConsecRank"] = comboBoxConsRank.SelectedItem.ToString().Substring(5, 1);
            Character.OnItemsChanged();
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void trackBarFightLength_Scroll(object sender, EventArgs e)
        {
            Character.CalculationOptions["FightLength"] = trackBarFightLength.Value.ToString();
            Character.OnItemsChanged();
        }

        private void txtArmor_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["BossArmor"] = txtArmor.Text;
            Character.OnItemsChanged();
        }

        private void checkBoxExorcism_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExorcism.Checked)
            {
                Character.CalculationOptions["Exorcism"] = "1";
            }
            else
            {
                Character.CalculationOptions["Exorcism"] = "0";
            }
            Character.OnItemsChanged();
        }

        private void btnTalents_Click(object sender, EventArgs e)
        {
            Talents talents = new Talents(this);
            talents.Show();
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {   
            Bitmap _prerenderedGraph = global::Rawr.Retribution.Properties.Resources.GraphBase;
            Graphics g = Graphics.FromImage(_prerenderedGraph);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            

            Color[] colors = new Color[] {
                Color.FromArgb(127,202,180,96), //Strength
                Color.FromArgb(127,101,225,240), //Agi
                Color.FromArgb(127,206,189,191),//SpellD
                Color.FromArgb(127,45,112,63),//Hit
                Color.FromArgb(127,217,100,54), //Haste
                Color.FromArgb(127,123,238,199),//Crit
                Color.FromArgb(127,210,72,195),//ArP
                Color.FromArgb(127,0,4,3),//AP
                Color.FromArgb(127,121,72,210),//Exp
            };
                
                
                
                


            Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { SpellDamageRating = 11.7f } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetration = 66.67f } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { ExpertiseRating=10 } }
                    };
            string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Spell Damage",
                        "Hit Rating",
                        "Haste Rating",
                        "Crit Rating",
                        "Armor Penetration",
                        "Attack Power",
                        "Expertise Rating"
                    };
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsRetribution baseCalc, currentCalc, calc;
            ComparisonCalculationBase comparison;
            CalculationsRetribution retCalc = new CalculationsRetribution();
            float[] subPoints;
            float graphHeight = 700f;
            float graphStart = 100f;
            baseCalc = retCalc.GetCharacterCalculations(Character) as CharacterCalculationsRetribution;

            for (int index = 0; index < statList.Length; index++)
            {
                Point[] points = new Point[100];
                for (int count = 0; count < 100; count++)
                {
                    Stats newStats = new Stats();
                    newStats.Strength = itemList[index].Stats.Strength * count;
                    newStats.Agility = itemList[index].Stats.Agility * count;
                    newStats.SpellDamageRating = itemList[index].Stats.SpellDamageRating * count;
                    newStats.HitRating = itemList[index].Stats.HitRating * count;
                    newStats.CritRating = itemList[index].Stats.CritRating * count;
                    newStats.ArmorPenetration = itemList[index].Stats.ArmorPenetration * count;
                    newStats.AttackPower = itemList[index].Stats.AttackPower * count;
                    newStats.ExpertiseRating = itemList[index].Stats.ExpertiseRating * count;
                    newStats.HasteRating = itemList[index].Stats.HasteRating * count;
                    calc = retCalc.GetCharacterCalculations(Character, new Item() { Stats = newStats }) as CharacterCalculationsRetribution;
                    float overallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                    if (overallPoints <= 0)
                    {
                        if (count <= 1)
                        {
                            overallPoints = 0;
                        }

                        else
                        {
                            overallPoints = graphHeight - points[count - 1].Y;
                        }
                    }
                    if ((graphHeight - overallPoints) > 16)
                    {
                        points[count] = new Point(Convert.ToInt32(graphStart+ count * 5), (Convert.ToInt32(graphHeight - overallPoints)));
                    }
                    else
                    {
                        points[count]=points[count-1];
                    }

                }
                Brush statBrush = new SolidBrush(colors[index]);
                g.DrawLines(new Pen(statBrush, 3), points);
            }
            #region Graph Ticks
       
            float graphWidth = 500f;// this.Width - 150f;
            float graphEnd = graphStart + graphWidth;
            float graphStartY = 16f;
           
            float maxScale = 100f;
            float[] ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};
          
            Pen black200 = new Pen(Color.FromArgb(200, 0, 0, 0));
            Pen black150 = new Pen(Color.FromArgb(150, 0, 0, 0));
            Pen black75 = new Pen(Color.FromArgb(75, 0, 0, 0));
            Pen black50 = new Pen(Color.FromArgb(50, 0, 0, 0));
            Pen black25 = new Pen(Color.FromArgb(25, 0, 0, 0));
            StringFormat formatTick = new StringFormat();
            formatTick.LineAlignment = StringAlignment.Far;
            formatTick.Alignment = StringAlignment.Center;
            Brush black200brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            Brush black150brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
            Brush black75brush = new SolidBrush(Color.FromArgb(75, 0, 0, 0));
            Brush black50brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            Brush black25brush = new SolidBrush(Color.FromArgb(25, 0, 0, 0));

            g.DrawLine(black200, graphStart - 4, 20, graphEnd + 4, 20);
            g.DrawLine(black200, graphStart, 16, graphStart, _prerenderedGraph.Height - 16);
            g.DrawLine(black200, graphEnd, 16, graphEnd, 19);
            g.DrawLine(black200, ticks[0], 16, ticks[0], 19);
            g.DrawLine(black150, ticks[1], 16, ticks[1], 19);
            g.DrawLine(black150, ticks[2], 16, ticks[2], 19);
            g.DrawLine(black75, ticks[3], 16, ticks[3], 19);
            g.DrawLine(black75, ticks[4], 16, ticks[4], 19);
            g.DrawLine(black75, ticks[5], 16, ticks[5], 19);
            g.DrawLine(black75, ticks[6], 16, ticks[6], 19);
            g.DrawLine(black75, graphEnd, 21, graphEnd, _prerenderedGraph.Height - 4);
            g.DrawLine(black75, ticks[0], 21, ticks[0], _prerenderedGraph.Height - 4);
            g.DrawLine(black50, ticks[1], 21, ticks[1], _prerenderedGraph.Height - 4);
            g.DrawLine(black50, ticks[2], 21, ticks[2], _prerenderedGraph.Height - 4);
            g.DrawLine(black25, ticks[3], 21, ticks[3], _prerenderedGraph.Height - 4);
            g.DrawLine(black25, ticks[4], 21, ticks[4], _prerenderedGraph.Height - 4);
            g.DrawLine(black25, ticks[5], 21, ticks[5], _prerenderedGraph.Height - 4);
            g.DrawLine(black25, ticks[6], 21, ticks[6], _prerenderedGraph.Height - 4);
            g.DrawLine(black200, graphStart - 4, _prerenderedGraph.Height - 20, graphEnd + 4, _prerenderedGraph.Height - 20);

            Font tickFont = new Font("Calibri", 11);
            g.DrawString((0f).ToString(), tickFont, black200brush, graphStart, 16, formatTick);
            g.DrawString((maxScale).ToString(), tickFont, black200brush, graphEnd, 16, formatTick);
            g.DrawString((maxScale * 0.5f).ToString(), tickFont, black200brush, ticks[0], 16, formatTick);
            g.DrawString((maxScale * 0.75f).ToString(), tickFont, black150brush, ticks[1], 16, formatTick);
            g.DrawString((maxScale * 0.25f).ToString(), tickFont, black150brush, ticks[2], 16, formatTick);
            g.DrawString((maxScale * 0.125f).ToString(), tickFont, black75brush, ticks[3], 16, formatTick);
            g.DrawString((maxScale * 0.375f).ToString(), tickFont, black75brush, ticks[4], 16, formatTick);
            g.DrawString((maxScale * 0.625f).ToString(), tickFont, black75brush, ticks[5], 16, formatTick);
            g.DrawString((maxScale * 0.875f).ToString(), tickFont, black75brush, ticks[6], 16, formatTick);

            g.DrawString((0f).ToString(), tickFont, black200brush, graphStart, _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale).ToString(), tickFont, black200brush, graphEnd, _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.5f).ToString(), tickFont, black200brush, ticks[0], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.75f).ToString(), tickFont, black150brush, ticks[1], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.25f).ToString(), tickFont, black150brush, ticks[2], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.125f).ToString(), tickFont, black75brush, ticks[3], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.375f).ToString(), tickFont, black75brush, ticks[4], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.625f).ToString(), tickFont, black75brush, ticks[5], _prerenderedGraph.Height-16, formatTick);
            g.DrawString((maxScale * 0.875f).ToString(), tickFont, black75brush, ticks[6], _prerenderedGraph.Height-16, formatTick);

        


            #endregion
            Graph graph = new Graph(_prerenderedGraph);
            graph.Show();
                     
        }
    }
}
