using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Rawr
{
    public class TalentItem
    {

        private TalentTree _talentTree;
        public TalentTree TalentTree
        {
            get { return _talentTree; }
            set
            {
                _talentTree = value;
                UpdateIcon();
            }
        }

        private string _talentName;
        public string TalentName
        {
            get { return _talentName; }
            set
            {
                _talentName = value;
                UpdateIcon();
            }
        }

        private int _locked = 0;

        private int _index = -1;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private int _row = 1;
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }
        private int _col = 1;
        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        private string[] _description = new string[] { "" };
        public string[] Description { get { return _description; } set { _description = value; } }

        private int _currentRank = 0;
        public int CurrentRank
        {
            get { return _currentRank; }
            set
            {
                if (_locked == 0 && value >= 0 && value <= _maxRank && _talentTree.TotalPoints() >= _row * 5 && 
                    (_prereq == null || _prereq._currentRank == _prereq._maxRank))
                {
                    if (_prereq != null) {
                        if (_currentRank > 0 && value == 0) _prereq._locked--;
                        else if (_currentRank == 0 && value > 0) _prereq._locked++;
                    }
                    int i = 1;
                    if (value < _currentRank)
                    {
                        int pts = 0;
                        for (i = 0; i <=_row; i++) pts += _talentTree.Points[i];
                        pts = pts - _currentRank + value;
                        for (i = _row + 1; i < 11; i++)
                        {
                            if (_talentTree.Points[i] > 0)
                            {
                                if (pts >= i * 5)
                                {
                                    pts += _talentTree.Points[i];
                                }
                                else
                                {
                                    i = -1;
                                    break;
                                }
                            }
                        }
                    }
                    if (i >= 0)
                    {
                        _talentTree.Points[_row] = _talentTree.Points[_row] - _currentRank + value;
                        _currentRank = value;
                        UpdateOverlay();
                        UpdateIcon();
                        OnCurrentRankChanged();
                    }
                }
            }
        }
        private int _maxRank = 5;
        public int MaxRank
        {
            get { return _maxRank; }
            set
            {
                _maxRank = value;
                UpdateOverlay();
                UpdateIcon();
            }
        }

        private TalentItem _prereq;
        public TalentItem Prereq
        {
            get { return _prereq; }
            set { _prereq = value; }
        }

        private Image _icon;
        private void UpdateIcon() { _icon = ItemIcons.GetTalentIcon(_talentTree.CharacterClass, _talentTree.TreeName, _talentName + (_currentRank == 0 ? "-off" : "")); }

        private Image _overlay;
        private void UpdateOverlay()
        {
            if (_currentRank == 0) _overlay = Properties.Resources.icon_over_grey;
            else if (_currentRank == MaxRank) _overlay = Properties.Resources.icon_over_yellow;
            else _overlay = Properties.Resources.icon_over_green;
        }

        public TalentItem(TalentTree talentTree, string talentName, int row, int col, int index, string[] description, int currentRank, int maxRank)
            : this(talentTree, talentName, row, col, index, description, currentRank, maxRank, null) { }

        public TalentItem(TalentTree talentTree, string talentName, int row, int col, int index, string[] description, int currentRank, int maxRank, TalentItem prereq)
        {
            _talentTree = talentTree;
            _talentName = talentName;
            _row = row;
            _col = col;
            _index = index;
            _description = description;
            _currentRank = currentRank;
            _maxRank = maxRank;
            _prereq = prereq;
            UpdateIcon();
            UpdateOverlay();
        }

        public void Draw(Graphics g)
        {
            int y = 8 + Row * 65;
            int x = 8 + Col * 62;

            String rank = string.Format("{0}/{1}", _currentRank, _maxRank);
            Font font = new Font("Verdana", 8);

            Brush brush;
            if (_currentRank == _maxRank) brush = Brushes.Yellow;
            else if (_talentTree.TotalPoints() < _row * 5) brush = Brushes.Gray;
            else brush = Brushes.Lime;

            if (_icon != null) g.DrawImageUnscaled(_icon, x, y);
            g.DrawImageUnscaled(_overlay, x, y);
            g.DrawString(rank, font, brush, x + 31, y + 39);

            if (_prereq != null)
            {
                Image arrowImage = null;
                int preRow = _row - _prereq.Row;
                int preCol = _col - _prereq.Col;
                if (_prereq._currentRank == _prereq._maxRank && _talentTree.TotalPoints() >= _row * 5)
                {
                    if (preCol == 0)
                    {
                        if (preRow == 1) arrowImage = Properties.Resources.down_1_yellow;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_yellow;
                        if (preRow == 3) arrowImage = Properties.Resources.down_3_yellow;
                        if (preRow == 4) arrowImage = Properties.Resources.down_4_yellow;
                    }
                    else if (preCol == -1)
                    {
                        if (preRow == 0) arrowImage = Properties.Resources.across_left_yellow;
                        if (preRow == 1) arrowImage = Properties.Resources.down_left_yellow;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_left_yellow;
                    }
                    else if (preCol == 1)
                    {
                        if (preRow == 0) arrowImage = Properties.Resources.across_right_yellow;
                        if (preRow == 1) arrowImage = Properties.Resources.down_right_yellow;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_right_yellow;
                    }
                }
                else
                {
                    if (preCol == 0)
                    {
                        if (preRow == 1) arrowImage = Properties.Resources.down_1_grey;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_grey;
                        if (preRow == 3) arrowImage = Properties.Resources.down_3_grey;
                        if (preRow == 4) arrowImage = Properties.Resources.down_4_grey;
                    }
                    else if (preCol == -1)
                    {
                        if (preRow == 0) arrowImage = Properties.Resources.across_left_grey;
                        if (preRow == 1) arrowImage = Properties.Resources.down_left_grey;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_left_grey;
                    }
                    else if (preCol == 1)
                    {
                        if (preRow == 0) arrowImage = Properties.Resources.across_right_grey;
                        if (preRow == 1) arrowImage = Properties.Resources.down_right_grey;
                        if (preRow == 2) arrowImage = Properties.Resources.down_2_right_grey;
                    }
                }
                if (arrowImage != null)
                {
                    if (preRow == 0 && preCol == -1) g.DrawImageUnscaled(arrowImage, x + _icon.Width - 8, y + 13);
                    else if (preRow == 0 && preCol == 1) g.DrawImageUnscaled(arrowImage, x + 8, y + 13);
                    else g.DrawImageUnscaled(arrowImage, x + 13, y - arrowImage.Height + 8);
                    
                }
            }

        }

        public event EventHandler CurrentRankChanged;
        public void OnCurrentRankChanged()
        {
            if (CurrentRankChanged != null)
                CurrentRankChanged(this, EventArgs.Empty);
        }

        public string TooltipText()
        {
            string text = string.Format("{0}\r\nRank {1}/{2}\r\n\r\n", TalentName, CurrentRank, MaxRank);
            if (CurrentRank == 0) text += Description[0];
            else if (CurrentRank >= MaxRank) text += Description[MaxRank - 1];
            else text += string.Format("{0}\r\n\r\nNext Rank:\r\n{1}", Description[CurrentRank - 1], Description[CurrentRank]);

            return text;
        }

    }


}
