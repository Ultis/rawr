using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
    public partial class TalentDisplay : UserControl
    {


        private TalentTree _tree;

        public Character.CharacterClass CharClass
        {
            get;
            set;
        }
        
        public TalentDisplay()
        {
            InitializeComponent();
        }

        public TalentDisplay(TalentTree tree, Character.CharacterClass charClass) : this()
        {
            CharClass = charClass;
            _tree = tree;
            buildTrees();
            
        }

        public void SetTree(TalentTree tree)
        {
            _tree = tree;
            buildTrees();
        }

        public TalentDisplay(TalentTree trees) : this()
        {
            Trees = trees;
        }

        private void buildTrees()
        {
            if (Trees != null)
            {
                //TalentTree trees, List<TalentPanel> panels, List<Label> labels, Character.CharacterClass charclass
                TalentPanel[] panels = new TalentPanel[] { talentPanel1, talentPanel2, talentPanel3 };
                Label[] labels = new Label[] { label1, label2, label3 };
                int index = 0;
                foreach (string treeName in trees.Trees.Keys)
                {
                    labels[index].Text = treeName;
                    panels[index] = new TalentPanel(trees.Trees[treeName], charclass);
                }
            }
        }
    }
}
