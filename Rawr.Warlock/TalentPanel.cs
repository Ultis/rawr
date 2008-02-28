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
    public partial class TalentPanel : UserControl 
    {

        private SerializableDictionary<TalentItem, TalentIcon> _icons = new Dictionary<TalentItem, TalentIcon>();
        private int _vertbuffer = 15;
        private int _horizbuffer = 25;
        private List<TalentItem> _talents = new List<TalentItem>();

        public TalentPanel()
        {
            InitializeComponent();
            CharClass = Character.CharacterClass.Warlock;
        }

        public TalentPanel(List<TalentItem> talents, Character.CharacterClass charclass) : this()
        {
            CharClass = charclass;
            Talents = talents;
        }

        public Character.CharacterClass CharClass
        {
            get;
            set;
        }


        public string Name
        {
            get;
            set;
        }

        public List<TalentItem> Talents
        {
            get { return _talents; }
            set
            {
                _talents = value;
                buildTree(_talents);
            }
        }

        private void buildTree(List<TalentItem> tals)
        {
            int maxVert = 0;
            tals.ForEach(ti => maxVert = ti.VerticalPosition > maxVert ? ti.VerticalPosition : maxVert);

            int maxHoriz = 0;
            tals.ForEach(ti => maxHoriz = ti.HorizontalPosition> maxHoriz ? ti.HorizontalPosition : maxHoriz);

            TalentIcon temp = new TalentIcon();

            this.Width =  (maxHoriz + 1 * _horizbuffer + maxHoriz * temp.Width);
            this.Height = (maxVert + 1 * _vertbuffer + maxVert * temp.Height);

            int vertoffset = 0;
            for (int row = 0; row <= maxVert; row++)
            {
                int horizoffset = 0;
                vertoffset += _vertbuffer;
                int maxCurrHoriz = 0;
                tals.ForEach(delegate(TalentItem ti) { if (ti.VerticalPosition == row) maxCurrHoriz = ti.HorizontalPosition > maxCurrHoriz ? ti.HorizontalPosition : maxCurrHoriz; });
                for (int col = 0;col<=maxCurrHoriz;col++)
                {
                    horizoffset += _horizbuffer;
                    temp = new TalentIcon(tals.Find(ti => ti.VerticalPosition == row && ti.HorizontalPosition == col), CharClass);
                    temp.Location = new Point(horizoffset, vertoffset);
                    horizoffset += temp.Width;
                    Controls.Add(temp);
                }
                _vertbuffer += temp.Height;
            }
        }
    }
}
