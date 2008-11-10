using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

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
					_character.CalculationsInvalidated -= new EventHandler(CharacterItemCache_ItemsChanged);
				}
				_character = value;
				if (_character != null)
				{
					_character.CalculationsInvalidated += new EventHandler(CharacterItemCache_ItemsChanged);
				}
			}
		}

        private bool resetItem = false;
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

			ItemCache.Instance.ItemsChanged += new EventHandler(CharacterItemCache_ItemsChanged);
        }

        private void CharacterItemCache_ItemsChanged(object sender, EventArgs e)
        {
            //with the way this event can be fired, it can set the current item null while the item image is being generated and cause errors
            resetItem = true;
        }

        private Item _currentItem = null;

        private Item CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (resetItem)
                {
                    _currentItem = null;
                    resetItem = false;
                }
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

        private Character CurrentItemCharacter { get; set; }
        private Enchant CurrentItemEnchant { get; set; }

        private Font _fontName;
		private Font _fontStats;
		private Font _fontStatsSmall;
        private Font _fontTinyName;

        private void LoadGraphicsObjects()
        {
            _fontName = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            _fontTinyName = new Font("Microsoft Sans Serif", 6.00F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
			_fontStats = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
			_fontStatsSmall = new Font("Microsoft Sans Serif", 7.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
		}

        protected Image _cachedToolTipImage = null;

        private static Graphics _sizeTest = Graphics.FromImage(new Bitmap(309, 50));

        protected Image CachedToolTipImage
        {
            get
            {
                if (_cachedToolTipImage == null && _currentItem != null)
                {
                    lock (_currentItem)
                    {
                        if (_currentItem != null)
                        {
                            bool hasSockets = _currentItem.Sockets.Color1 != Item.ItemSlot.None ||
                                              _currentItem.Sockets.Color2 != Item.ItemSlot.None ||
                                              _currentItem.Sockets.Color3 != Item.ItemSlot.None;
                            var positiveStats = Calculations.GetRelevantStats(_currentItem.Stats).Values(x => x > 0);
                            int statHeight = (positiveStats.Count + 2) / 3; // number of lines
                            statHeight *= 17;// * line height
                            int extraLocation = 0;

                            string location = "Unknown source";
                            if (_currentItem.LocationInfo != null)
                            {
                                location = _currentItem.LocationInfo.Description;
                            }
                            SizeF locationSize = _sizeTest.MeasureString(location, _fontStats);
                            if (locationSize.Width > 300)
                            {
                                extraLocation = (int)locationSize.Height;
                            }

                            int enchantSize = 0;
                            if (CurrentItemEnchant != null)
                            {
                                enchantSize = 17;
                            }

                            int numTinyItems = 0;
                            int tinyItemSize = 18;
                            if (CurrentItemCharacter != null)
                            {
                                foreach (Character.CharacterSlot slot in Character.CharacterSlots)
                                {
                                    if (CurrentItemCharacter[slot] != null && CurrentItemCharacter[slot].GemmedId != _currentItem.GemmedId) numTinyItems++;
                                }
                            }

                            _cachedToolTipImage = new Bitmap(309, (hasSockets ? 96 + statHeight : 38 + statHeight) + extraLocation + enchantSize + numTinyItems * tinyItemSize, PixelFormat.Format32bppArgb);

                            Graphics g = Graphics.FromImage(_cachedToolTipImage);
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.TextRenderingHint = TextRenderingHint.AntiAlias;

                            Rectangle rectBorder =
                                new Rectangle(0, 0, _cachedToolTipImage.Width - 1, _cachedToolTipImage.Height - 1);
                            LinearGradientBrush lgBrush = new LinearGradientBrush(rectBorder,
                                                                    Color.FromArgb(212, 212, 255), Color.FromArgb(192, 192, 255),
                                                                    90);
                            g.FillRectangle(lgBrush, rectBorder);
                            lgBrush.Dispose();
                            Pen pen = new Pen(Color.FromArgb(128, 0, 0, 0));
                            g.DrawRectangle(pen, rectBorder);
                            pen.Dispose();
                            Brush nameBrush = null;
                            switch (_currentItem.Quality)
                            {
                                case Item.ItemQuality.Common:
                                case Item.ItemQuality.Temp:
                                    nameBrush = new SolidBrush(Color.FromKnownColor(KnownColor.InfoText));
                                    break;
                                case Item.ItemQuality.Epic:
                                    nameBrush = new SolidBrush(Color.Purple);
                                    break;
                                case Item.ItemQuality.Legendary:
                                    nameBrush = new SolidBrush(Color.Orange);
									break;
								case Item.ItemQuality.Heirloom:
								case Item.ItemQuality.Artifact:
									nameBrush = new SolidBrush(Color.Gold);
									break;
                                case Item.ItemQuality.Poor:
                                    nameBrush = new SolidBrush(Color.Gray);
                                    break;
                                case Item.ItemQuality.Rare:
                                    nameBrush = new SolidBrush(Color.Blue);
                                    break;
                                case Item.ItemQuality.Uncommon:
                                    nameBrush = new SolidBrush(Color.Green);
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
                                if (Stats.IsPercentage(info))
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
                                                                   ? _currentItem.Sockets.Color1
                                                                   :
                                                               (i == 1 ? _currentItem.Sockets.Color2 : _currentItem.Sockets.Color3));
                                    if (slotColor != Item.ItemSlot.None)
                                    {
                                        Rectangle rectGemBorder = new Rectangle(3 + (103 * (i)), 25 + statHeight, 35, 35);
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
                                        brushGemBorder.Dispose();
                                        Item gem = (i == 0 ? _currentItem.Gem1 : (i == 1 ? _currentItem.Gem2 : _currentItem.Gem3));
                                        if (gem != null)
                                        {
                                            Image icon = ItemIcons.GetItemIcon(gem, true);
                                            if (icon != null)
                                            {
                                                g.DrawImageUnscaled(icon, rectGemBorder.X + 2, rectGemBorder.Y + 2);
                                                icon.Dispose();
                                            }
                                            
                                            Character characterWithItemEquipped = Character.Clone();
                                            characterWithItemEquipped[Character.CharacterSlot.Head] = CurrentItem;
                                            bool active = gem.MeetsRequirements(characterWithItemEquipped);

                                            string[] stats = gem.Stats.ToString().Split(',');

											switch (stats.Length)
											{
												case 1:
													g.DrawString(stats[0].Trim(), _fontStats, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 3);
													break;

												case 2:
													g.DrawString(stats[0].Trim(), _fontStats, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 3);
													g.DrawString(stats[1].Trim(), _fontStats, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 20);
													break;

												case 3:
													g.DrawString(stats[0].Trim(), _fontStatsSmall, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 0);
													g.DrawString(stats[1].Trim(), _fontStatsSmall, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 12);
													g.DrawString(stats[2].Trim(), _fontStatsSmall, active ? SystemBrushes.InfoText : SystemBrushes.GrayText,
														rectGemBorder.X + 39, rectGemBorder.Y + 24);
													break;
											}

                                            if (!active)
                                            {
                                                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(128, Color.Silver));
                                                g.FillRectangle(solidBrush, rectGemBorder);
                                                solidBrush.Dispose();
                                            }
                                        }
                                    }
                                }

                                Brush brushBonus = SystemBrushes.InfoText;
                                if (!Item.GemMatchesSlot(_currentItem.Gem1, _currentItem.Sockets.Color1) ||
                                    !Item.GemMatchesSlot(_currentItem.Gem2, _currentItem.Sockets.Color2) ||
                                    !Item.GemMatchesSlot(_currentItem.Gem3, _currentItem.Sockets.Color3))
                                    brushBonus = SystemBrushes.GrayText;

                                g.DrawString(
                                    "Socket Bonus: " +
                                    (CurrentItem.Sockets.Stats.ToString().Length == 0
                                         ? "None"
                                         : CurrentItem.Sockets.Stats.ToString()),
                                    _fontStats, brushBonus, 2, 63 + statHeight);
                            }
                            if (_currentItem.Quality != Item.ItemQuality.Temp)
                            {
                                Rectangle textRec = new Rectangle(2, (hasSockets ? 76 : 18) + statHeight + 4, _cachedToolTipImage.Width - 4, (int)locationSize.Height + extraLocation);
                                g.DrawString(location, _fontStats, SystemBrushes.InfoText, textRec);
                            }
                            xPos = 2;
                            yPos = (hasSockets ? 76 : 18) + statHeight + 4 + (int)locationSize.Height + extraLocation + 2;

                            if (CurrentItemEnchant != null)
                            {
                                g.DrawString(CurrentItemEnchant.ToString(), _fontStats, SystemBrushes.InfoText, xPos, yPos + 1);
                                yPos += enchantSize;
                            }

                            if (CurrentItemCharacter != null)
                            {
                                foreach (Character.CharacterSlot slot in Character.CharacterSlots)
                                {
                                    Item tinyItem = CurrentItemCharacter[slot];
                                    if (tinyItem != null && tinyItem.GemmedId != _currentItem.GemmedId)
                                    {
                                        Image icon = ItemIcons.GetItemIcon(tinyItem, true);
                                        if (icon != null)
                                        {
                                            g.DrawImage(icon, xPos + 1, yPos + 1, 16, 16);
                                            icon.Dispose();
                                            xPos += tinyItemSize;
                                        }
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            Item gem = tinyItem.GetGem(i);
                                            if (gem != null)
                                            {
                                                icon = ItemIcons.GetItemIcon(gem, true);
                                                if (icon != null)
                                                {
                                                    g.DrawImage(icon, xPos + 1, yPos + 1, 16, 16);
                                                    icon.Dispose();
                                                    xPos += tinyItemSize;
                                                }
                                            }
                                        }

                                        xPos = 2;
                                        nameBrush = null;
                                        switch (tinyItem.Quality)
                                        {
                                            case Item.ItemQuality.Common:
                                            case Item.ItemQuality.Temp:
                                                nameBrush = SystemBrushes.InfoText;
                                                break;
                                            case Item.ItemQuality.Epic:
                                                nameBrush = Brushes.Purple;
                                                break;
                                            case Item.ItemQuality.Legendary:
                                                nameBrush = Brushes.Orange;
                                                break;
                                            case Item.ItemQuality.Poor:
                                                nameBrush = Brushes.Gray;
                                                break;
                                            case Item.ItemQuality.Rare:
                                                nameBrush = Brushes.Blue;
                                                break;
                                            case Item.ItemQuality.Uncommon:
                                                nameBrush = Brushes.Gray;
                                                break;
                                        }
                                        string label = tinyItem.Name;
                                        Enchant tinyEnchant = CurrentItemCharacter.GetEnchantBySlot(slot);
                                        if (tinyEnchant != null && tinyEnchant.Id != 0) label = label + " (" + tinyEnchant.ToString() + ")";
                                        g.DrawString(label, _fontTinyName, nameBrush, xPos + 76, yPos + 1);
                                        yPos += tinyItemSize;
                                    }
                                }
                            }

                            g.Dispose();
                        }
                    }
                }
                if (resetItem)
                {
                    CurrentItem = null;
                    resetItem = false;
                }
                return _cachedToolTipImage;
            }
        }

        public void Show(Item item, IWin32Window window, Point point)
        {
            Show(item, null, null, window, point);
        }

		public void Show(Item item, Character itemCharacter, Enchant itemEnchant, IWin32Window window, Point point)
		{
			CurrentItem = item;
            CurrentItemCharacter = itemCharacter;
            CurrentItemEnchant = itemEnchant;
            if (CachedToolTipImage != null)
            {
                base.Show(item.Name, window, point);
            }
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