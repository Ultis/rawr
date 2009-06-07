using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Rawr.Silverlight
{
	public partial class TalentItem : UserControl
	{

        public TalentTree TalentTree { get { return (TalentTree)(((Grid)Parent).Parent); } }

        private TalentDataAttribute talentData;
        public TalentDataAttribute TalentData
        {
            get { return talentData; }
            set
            {
                talentData = value;
                current =  talentData == null ? 0 : (int)Math.Min(TalentTree.Talents.Data[talentData.Index], talentData.MaxPoints);
            }
        }

        public int current;
        public int Current
        {
            get { return current; }
            set
            {
                if (talentData != null && value >= 0 && value <= talentData.MaxPoints && CanPutPoints())
                {
                    current = value;
                    TalentTree.Talents.Data[TalentData.Index] = current;
                    TalentTree.RankChanged();
                }
            }
        }

        public bool IsMaxRank() { return current == talentData.MaxPoints; }

        public bool CanPutPoints()
        {
            return TalentTree.PointsBelowRow(talentData.Row) >= (talentData.Row - 1) * 5 &&
                (talentData.Prerequisite < 0
                || TalentTree.Talents.Data[talentData.Prerequisite] == TalentTree.GetAttribute(talentData.Prerequisite).MaxPoints);
        }

        public void Update()
        {
            if (talentData != null)
            {
                Brush b;
                if (Current == talentData.MaxPoints)
                {
                    b = new SolidColorBrush(Colors.Yellow);
                    OverlayImage.Source = new BitmapImage(new Uri("../Images/icon-over-yellow.png", UriKind.Relative));
                }
                else
                {
                    if (CanPutPoints()) b = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                    else b = new SolidColorBrush(Colors.White);

                    if (Current > 0) OverlayImage.Source = new BitmapImage(new Uri("../Images/icon-over-green.png", UriKind.Relative));
                    else OverlayImage.Source = new BitmapImage(new Uri("../Images/icon-over-grey.png", UriKind.Relative));
                }

                RankLabel.Text = string.Format("{0}/{1}", Current, talentData.MaxPoints);
                RankLabel.Foreground = b;

                TalentImage.Source = Icons.TalentIcon(TalentTree.Class, TalentTree.TreeName, talentData.Name, Current > 0);

                ToolTipService.SetToolTip(this, GetTooltipString());
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        public string GetTooltipString()
        {
            if (Current == 0)
            {
                return string.Format("Next Rank:\n{0}",talentData.Description[0]);
            }
            else if (Current == talentData.MaxPoints)
            {
                return talentData.Description[talentData.MaxPoints - 1];
            }
            else
            {
                return string.Format("{0}\n\nNext Rank:\n{1}",talentData.Description[Current - 1], talentData.Description[Current]);
            }
        }

		public TalentItem()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void TalentClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) Current--;
			else Current++;
		}
		
	}
}