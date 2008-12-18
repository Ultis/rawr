using System;
using System.Diagnostics;
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

        private Font _fontName = null;
        private Font _fontStats = null;
        private Font _fontStatsSmall = null;
        private Font _fontTinyName = null;

        private void LoadGraphicsObjects()
        {
            Debug.Assert(_fontName == null);
            Debug.Assert(_fontTinyName == null);
            Debug.Assert(_fontStats == null);
            Debug.Assert(_fontStatsSmall == null);

            _fontName = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((0)));
            _fontTinyName = new Font("Microsoft Sans Serif", 6.00F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
			_fontStats = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
			_fontStatsSmall = new Font("Microsoft Sans Serif", 7.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
		}

        protected Image _cachedToolTipImage = null;

        private static Graphics _dummyBitmap = Graphics.FromImage(new Bitmap(1, 1));

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
                            SizeF locationSize = _dummyBitmap.MeasureString(location, _fontStats);
                            if (locationSize.Width > 300)
                            {
                                extraLocation = (int)locationSize.Height;
                            }

                            int enchantSize = 0;
                            if (CurrentItemEnchant != null)
                            {
                                enchantSize = 17;
                            }

                            int gemInfoSize = 0;
                            if (CurrentItem.IsGem)
                            {
                                SizeF gem_info_size = _dummyBitmap.MeasureString("Matches:", _fontStats);
                                gemInfoSize = (int)gem_info_size.Height;
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

                            _cachedToolTipImage = new Bitmap(309, (hasSockets ? 96 + statHeight : 38 + statHeight) + extraLocation + enchantSize + gemInfoSize + numTinyItems * tinyItemSize, PixelFormat.Format32bppArgb);

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
                            lgBrush = null;

                            Pen pen = new Pen(Color.FromArgb(128, 0, 0, 0));
                            g.DrawRectangle(pen, rectBorder);
                            pen.Dispose();
                            pen = null;

                            { // hide the name brush scope to reduce bugs
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
                                nameBrush = null;
                            }

                            // item level drawing
                            if (CurrentItem.ItemLevel > 0)
                            {
                                SizeF name_size = g.MeasureString(CurrentItem.Name, _fontName);
                                Brush ilvlBrush = new SolidBrush(Color.Gray);
                                g.DrawString(string.Format("[{0}]", CurrentItem.ItemLevel), _fontName, ilvlBrush, name_size.Width + 2, 4);
                                ilvlBrush.Dispose();
                            }

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

                            int ypos_nextline = 0;
                            foreach (System.Reflection.PropertyInfo info in positiveStats.Keys)
                            {
                                float value = positiveStats[info];
                                if (Stats.IsPercentage(info))
                                    value *= 100;
                                value = (float)Math.Round(value * 100f) / 100f;
                                g.DrawString(string.Format("{0}{1}", value, Extensions.DisplayName(info)), _fontStats, SystemBrushes.InfoText, xPos, yPos);
                                // once we write on a line store where the next line would be
                                // (alternatively we could put the yPos update code below this at the top of the loop)
                                ypos_nextline = yPos + yGrid.step;

                                xPos += xGrid.step;
                                if (xPos > xGrid.max)
                                {
                                    xPos = xGrid.initial;
                                    yPos += yGrid.step;
                                }
                            }
                            // this is the next clean/empty line after we wrote the stats
                            yPos = ypos_nextline;

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
                                        brushGemBorder = null;

                                        Item gem = (i == 0 ? _currentItem.Gem1 : (i == 1 ? _currentItem.Gem2 : _currentItem.Gem3));
                                        if (gem != null)
                                        {
                                            Image icon = ItemIcons.GetItemIcon(gem, true);
                                            if (icon != null)
                                            {
                                                g.DrawImageUnscaled(icon, rectGemBorder.X + 2, rectGemBorder.Y + 2);
                                                icon.Dispose();
                                                icon = null;
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

                                // update cursor position (the magic number 63 comes from right above)
                                SizeF socket_info_size = g.MeasureString("Socket Bonus:", _fontStats);
                                yPos = 63 + statHeight + ((int)socket_info_size.Height);
                            }
                            // draw information about gem color (for dummies like me)
                            if (CurrentItem.IsGem)
                            {
                                // create a string that gives us some information about the gem
                                string colors = "Matches: ";
                                if (Item.GemMatchesSlot(CurrentItem, Item.ItemSlot.Meta))
                                    colors += "Meta ";
                                if (Item.GemMatchesSlot(CurrentItem, Item.ItemSlot.Red))
                                    colors += "Red ";
                                if (Item.GemMatchesSlot(CurrentItem, Item.ItemSlot.Yellow))
                                    colors += "Yellow ";
                                if (Item.GemMatchesSlot(CurrentItem, Item.ItemSlot.Blue))
                                    colors += "Blue ";
                                // put the info on the tooltip
                                Brush gemBrush = new SolidBrush(Color.Gray);
                                g.DrawString(colors, _fontStats, gemBrush, 2, yPos);
                                gemBrush.Dispose();
                                // update our cursor position
                                SizeF gem_info_size = g.MeasureString(colors, _fontStats);
                                yPos += (int)gem_info_size.Height;
                            }

                            if (_currentItem.Quality != Item.ItemQuality.Temp)
                            {
                                PointF draw_pos = new PointF(2, yPos);
                                //Rectangle textRec = new Rectangle(2, (hasSockets ? 76 : 18) + statHeight + 4, _cachedToolTipImage.Width - 4, (int)locationSize.Height + extraLocation);
                                g.DrawString(location, _fontStats, SystemBrushes.InfoText, draw_pos);
                                // update yPos here
                                SizeF quality_size = g.MeasureString(location, _fontStats);
                                yPos += (int)quality_size.Height;
                            }

                            xPos = 2;                           
                            //yPos = (hasSockets ? 76 : 18) + statHeight + 4 + (int)locationSize.Height + extraLocation + 2;

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

                                        { // hide the name brush scope to reduce bugs
                                            Brush nameBrush = null;
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
                            }

                            g.Dispose();
                            g = null;
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