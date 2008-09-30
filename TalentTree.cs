using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class TalentTree : UserControl
    {
        private TalentItem[,] _talents = new TalentItem[11, 4];

        private Image background;

        private Character.CharacterClass _characterClass;
        public Character.CharacterClass CharacterClass
        {
            get { return _characterClass; }
            set { _characterClass = value; }
        }

        private string _treeName;
        public string TreeName
        {
            get { return _treeName; }
            set { 
                _treeName = value;
                background = ItemIcons.GetTalentTreeBackground(_characterClass, _treeName);
            }
        }

        private int _points;
        public int Points { get { return _points; } set { _points = value; } }

        public TalentTree() : this(Character.CharacterClass.Paladin, "Holy") { }

        public TalentTree(Character.CharacterClass characterClass, string treeName)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            _characterClass = characterClass;
            TreeName = treeName;
        }

        public void AddTalent(TalentItem talent)
        {
            talent.CurrentRankChanged += new EventHandler(item_CurrentRankChanged);
            _talents[talent.Row, talent.Col] = talent;
            _points += talent.CurrentRank;
        }

        private Bitmap _prerenderedGraph = null;
        public Bitmap PrerenderedGraph
        {
            get
            {
                if (_prerenderedGraph == null)
                {
                    if (background != null)
                    {
                        _prerenderedGraph = new Bitmap(Math.Min(32767, Math.Max(1, this.Width)), Math.Min(32767, Math.Max(1, background.Height)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics g = Graphics.FromImage(_prerenderedGraph);
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;


                        g.DrawImageUnscaled(background, 0, 0);

                        foreach (TalentItem talent in _talents)
                        {
                            if (talent != null)
                            {
                                talent.Draw(g);
                            }
                        }

                        g.Dispose();
                        //_scrollBar.Maximum = _prerenderedGraph.Height - 1;
                        //_scrollBar.SmallChange = 32;
                        //_scrollBar.LargeChange = _scrollBar.Height;
                    }
                }
                return _prerenderedGraph;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImageUnscaled(PrerenderedGraph, 0, 0 - 0);//(_scrollBar != null ? _scrollBar.Value : 0));
        }

        private int _mouseRow = -1;
        private int _mouseCol = -1;

        private void TalentTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (_mouseRow >= 0 && _mouseRow <= 10 && _mouseCol >= 0 && _mouseCol <= 3)
            {
                TalentItem talent = _talents[_mouseRow, _mouseCol];
                if (talent != null)
                {
                    if (e.Button == MouseButtons.Right) talent.CurrentRank--;
                    else if (e.Button == MouseButtons.Left) talent.CurrentRank++;
                    Redraw();
                }
            }
        }

        public void Redraw()
        {
            _prerenderedGraph = null;
            this.Invalidate();
        }

        public void Reset()
        {
            _points = 0;
            _prerenderedGraph = null;
            foreach (TalentItem talent in _talents)
            {
                if (talent != null)
                {
                    talent.CurrentRankChanged -= new EventHandler(item_CurrentRankChanged);
                }
            }
            _talents = new TalentItem[11, 4];
        }

        private ToolTip _toolTip = new ToolTip();
        private void TalentTree_MouseMove(object sender, MouseEventArgs e)
        {
            int row =  ((e.Y - 8) % 65 > 48) ? -1 : (e.Y - 8) / 65;
            int col = ((e.X - 8) % 62 > 47) ? -1 : (e.X - 8) / 62;
            if (row >= 0 && row <= 10 && col >= 0 && col <= 3)
            {
                if (row != _mouseRow || col != _mouseCol)
                {
                    TalentItem talent = _talents[row, col];
                    if (talent != null)
                    {
                        _toolTip.Show(talent.TooltipText(), this, col * 63 + 65, row * 65 + 24);
                    }
                    else { _toolTip.Hide(this); }
                }
            }
            else { _toolTip.Hide(this); }
            _mouseRow = row;
            _mouseCol = col;
        }

        void item_CurrentRankChanged(object sender, EventArgs e)
        {
            TalentItem item = sender as TalentItem;
            if (_mouseRow == item.Row && _mouseCol == item.Col) _toolTip.Show(item.TooltipText(), this, _mouseCol * 63 + 65, _mouseRow * 65 + 24);
        }
    }
}
