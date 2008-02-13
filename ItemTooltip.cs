using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Rawr
{
    public class ItemToolTip : ToolTip
    {
        private static ItemToolTip _instance = null;
        public static ItemToolTip Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemToolTip();
                }
                return _instance;
            }
        }

		private Character _character = null;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.ItemsChanged -= new EventHandler(CharacterItemCache_ItemsChanged);
				}
				_character = value;
				if (_character != null)
				{
					_character.ItemsChanged += new EventHandler(CharacterItemCache_ItemsChanged);
				}
			}
		}

        public ItemToolTip()
        {
            AutomaticDelay = 0;
            AutoPopDelay = 0;
            Draw += new DrawToolTipEventHandler(ItemToolTip_Draw);
            InitialDelay = 0;
            OwnerDraw = true;
            Popup += new PopupEventHandler(ItemToolTip_Popup);
            ReshowDelay = 0;
            UseAnimation = true;
            UseFading = true;
            LoadGraphicsObjects();

			ItemCache.ItemsChanged += new EventHandler(CharacterItemCache_ItemsChanged);
        }

        private void CharacterItemCache_ItemsChanged(object sender, EventArgs e)
        {
            _currentItem = null;
        }

        protected Item _currentItem = null;

        protected Item CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem != value)
                {
                    _currentItem = value;
                    if (_cachedToolTipImage != null)
                    {
                        _cachedToolTipImage.Dispose();
                        _cachedToolTipImage = null;
                    }
                }
            }
        }

        private Font _fontName;
        private Font _fontStats;

        private void LoadGraphicsObjects()
        {
            _fontName = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            _fontStats = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
        }

        protected Image _cachedToolTipImage = null;

        protected Image CachedToolTipImage
        {
            get
            {
                if (_cachedToolTipImage == null && _currentItem != null)
                {
                    bool hasSockets = CurrentItem.Sockets.Color1 != Item.ItemSlot.None ||
                                      CurrentItem.Sockets.Color2 != Item.ItemSlot.None ||
                                      CurrentItem.Sockets.Color3 != Item.ItemSlot.None;
                    var positiveStats = Calculations.GetRelevantStats(CurrentItem.Stats).Values(x => x > 0);
                    int statHeight = (positiveStats.Count + 2) / 3; // number of lines
                    statHeight *= 17;// * line height
                    _cachedToolTipImage = new Bitmap(309, hasSockets ? 81 + statHeight : 23 + statHeight, PixelFormat.Format32bppArgb);

                    Graphics g = Graphics.FromImage(_cachedToolTipImage);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;

                    Rectangle rectBorder =
                        new Rectangle(0, 0, _cachedToolTipImage.Width - 1, _cachedToolTipImage.Height - 1);
                    g.FillRectangle(new LinearGradientBrush(rectBorder,
                                                            Color.FromArgb(212, 212, 255), Color.FromArgb(192, 192, 255),
                                                            90), rectBorder);
                    g.DrawRectangle(new Pen(Color.FromArgb(128, 0, 0, 0)), rectBorder);

                    Brush nameBrush = null;
                    switch (CurrentItem.Quality)
                    {
						case Item.ItemQuality.Common:
                            nameBrush = new SolidBrush(Color.FromKnownColor(KnownColor.InfoText));
                            break;
						case Item.ItemQuality.Epic:
                            nameBrush = new SolidBrush(Color.Purple);
                            break;
						case Item.ItemQuality.Legendary:
                            nameBrush = new SolidBrush(Color.Orange);
                            break;
						case Item.ItemQuality.Poor:
                            nameBrush = new SolidBrush(Color.Gray);
                            break;
						case Item.ItemQuality.Rare:
                            nameBrush = new SolidBrush(Color.Blue);
                            break;
						case Item.ItemQuality.Uncommon:
                            nameBrush = new SolidBrush(Color.Gray);
                            break;
                    }
                    g.DrawString(CurrentItem.Name, _fontName, nameBrush, 2, 4);
                    nameBrush.Dispose();

                    var xGrid = new
                    {
                        initial = 2,
                        step = 103,
                        max = 218,
                    };

                    var yGrid = new
                    {
                        initial = 21,
                        step = 17,
                        max = statHeight,
                    };

                    int xPos = xGrid.initial;
                    int yPos = yGrid.initial;

                    foreach (System.Reflection.PropertyInfo info in positiveStats.Keys)
                    {
						float value = positiveStats[info];
						if (Stats.IsMultiplicative(info))
							value *= 100;
						value = (float)Math.Round(value * 100f) / 100f;
						g.DrawString(string.Format("{0}{1}", value, Extensions.DisplayName(info)), _fontStats, SystemBrushes.InfoText, xPos, yPos);

                        xPos += xGrid.step;
                        if (xPos > xGrid.max)
                        {
                            xPos = xGrid.initial;
                            yPos += yGrid.step;
                        }
                    }
                    if (hasSockets)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Item.ItemSlot slotColor = (i == 0
                                                           ? CurrentItem.Sockets.Color1
                                                           :
                                                       (i == 1 ? CurrentItem.Sockets.Color2 : CurrentItem.Sockets.Color3));
                            if (slotColor != Item.ItemSlot.None)
                            {
                                Rectangle rectGemBorder = new Rectangle(3 + (103*(i)), 25+statHeight, 35, 35);
                                Brush brushGemBorder = new SolidBrush(Color.Silver);
                                switch (slotColor)
                                {
                                    case Item.ItemSlot.Red:
                                        brushGemBorder = new SolidBrush(Color.Red);
                                        break;
                                    case Item.ItemSlot.Yellow:
                                        brushGemBorder = new SolidBrush(Color.Yellow);
                                        break;
                                    case Item.ItemSlot.Blue:
                                        brushGemBorder = new SolidBrush(Color.Blue);
                                        break;
                                }
                                g.FillRectangle(brushGemBorder, rectGemBorder);

                                Item gem = (i == 0 ? CurrentItem.Gem1 : (i == 1 ? CurrentItem.Gem2 : CurrentItem.Gem3));
                                if (gem != null)
                                {
                                    g.DrawImageUnscaled(ItemIcons.GetItemIcon(gem, true), rectGemBorder.X + 2,
                                                        rectGemBorder.Y + 2);

									Character characterWithItemEquipped = Character.Clone();
									characterWithItemEquipped[Character.CharacterSlot.Head] = CurrentItem;
									bool active = gem.MeetsRequirements(characterWithItemEquipped);

                                    string[] stats = gem.Stats.ToString().Split(',');
                                    if (stats.Length > 0)
                                        g.DrawString(stats[0].Trim(), _fontStats, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
                                                     rectGemBorder.X + 39, rectGemBorder.Y + 3);

                                    if (stats.Length > 1)
                                        g.DrawString(stats[1].Trim(), _fontStats, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
                                                     rectGemBorder.X + 39, rectGemBorder.Y + 20);

									if (!active)
										g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Silver)), rectGemBorder);
                                }
                            }
                        }

                        Brush brushBonus = SystemBrushes.InfoText;
                        if (!Item.GemMatchesSlot(CurrentItem.Gem1, CurrentItem.Sockets.Color1) ||
                            !Item.GemMatchesSlot(CurrentItem.Gem2, CurrentItem.Sockets.Color2) ||
                            !Item.GemMatchesSlot(CurrentItem.Gem3, CurrentItem.Sockets.Color3))
                            brushBonus = SystemBrushes.GrayText;

                        g.DrawString(
                            "Socket Bonus: " +
                            (CurrentItem.Sockets.Stats.ToString().Length == 0
                                 ? "None"
                                 : CurrentItem.Sockets.Stats.ToString()),
                            _fontStats, brushBonus, 2, 63 + statHeight);
                    }

                    g.Dispose();
                }
                return _cachedToolTipImage;
            }
        }

		public void Show(Item item, IWin32Window window, Point point)
		{
			CurrentItem = item;
			CachedToolTipImage.ToString();
			base.Show(item.Name, window, point);
		}

        private void ItemToolTip_Popup(object sender, PopupEventArgs e)
        {
			e.ToolTipSize = CachedToolTipImage.Size;
		}

        private void ItemToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (CachedToolTipImage != null)
                //the emotion opens up swallow you
                e.Graphics.DrawImageUnscaled(CachedToolTipImage, 0, 0);
        }
    }
}