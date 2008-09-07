using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue {
    /// <summary>
    /// Rogue talent form, based on ProtWarr module
    /// </summary>
    public partial class RogueTalents : Form {
        public RogueTalents() {
            InitializeComponent();
        }

        public Character Char {
            get;
            set;
        }

        public TalentTree Tree {
            get;
            set;
        }

        public Character.CharacterClass CharClass {
            get;
            set;
        }

        public void SetTalents(Character character) {
            Char = character;
            buildTrees(Char.Talents, Char.Class);
        }

        private void buildTrees(TalentTree tree, Character.CharacterClass charClass) {
            if (tree != null) {
                Tree = tree;
                CharClass = charClass;

                Panel[] panels = new Panel[] { panelAssassination, panelCombat, panelSubtlety };
                GroupBox[] groups = new GroupBox[] { groupBoxAssassination, groupBoxCombat, groupBoxSubtlety };

                int index = 0;
                foreach (string treeName in tree.Trees.Keys) {
                    buildPanel(panels[index], tree.Trees[treeName], charClass, groups[index], treeName);
                    index++;
                }

                this.Width = groupBoxSubtlety.Location.X + groupBoxAssassination.Location.X + groupBoxSubtlety.Width;
                this.Height = groupBoxAssassination.Location.Y + groupBoxAssassination.Height + 40;
                this.Text = CharClass + " Talent Tree";
            }
        }

        private void buildPanel(Panel p, List<TalentItem> tals, Character.CharacterClass charClass, GroupBox gb, string tree) {
            int points = 0;
            int _vertbuffer = 5;
            int _horizbuffer = 2;
            int maxVert = 0;
            tals.ForEach(delegate(TalentItem ti) { maxVert = ti.VerticalPosition > maxVert ? ti.VerticalPosition : maxVert; });

            int maxHoriz = 0;
            tals.ForEach(delegate(TalentItem ti) { maxHoriz = ti.HorizontalPosition > maxHoriz ? ti.HorizontalPosition : maxHoriz; });

            /* "talent[i] = [0, \"Improved Eviscerate\", 3, 1, 1]; i++;"
               ??, name, rank, horiz, vert                                  */

            TalentIcon bg = new TalentIcon(new TalentItem("talent[i] = [0, \"background\", 0, 0, 0];", tree), charClass, gb);
            p.BackgroundImage = bg.getIcon();
            TalentIcon temp = new TalentIcon();

            p.Width = ((maxHoriz) * _horizbuffer + maxHoriz * temp.Width);
            p.Height = ((maxVert) * _vertbuffer + (maxVert) * temp.Height);

            int vertoffset = 0;
            for (int row = 1; row <= maxVert; row++) {
                int horizoffset = 0;
                vertoffset += _vertbuffer;
                int maxCurrHoriz = 0;
                tals.ForEach(delegate(TalentItem ti) { if (ti.VerticalPosition == row) maxCurrHoriz = ti.HorizontalPosition > maxCurrHoriz ? ti.HorizontalPosition : maxCurrHoriz; });
                for (int col = 1; col <= maxCurrHoriz; col++) {
                    horizoffset += _horizbuffer;
                    //temp = new TalentIcon(tals.Find(ti => ti.VerticalPosition == row && ti.HorizontalPosition == col), CharClass);
                    TalentItem ti = tals.Find(delegate(TalentItem ti2) { return (ti2.VerticalPosition == row && ti2.HorizontalPosition == col); });
                    if (ti != null) {
                        points += ti.PointsInvested;
                        temp = new TalentIcon(ti, charClass, gb);
                        temp.Location = new Point(horizoffset, vertoffset);
                        p.Controls.Add(temp);
                    }
                    horizoffset += temp.Width;
                }
                vertoffset += temp.Height;
            }

            gb.Text = tree + " (" + points.ToString() + ")";
        }
    }
}
