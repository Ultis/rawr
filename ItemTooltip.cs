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

            ItemCache.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
        }

        private void ItemCache_ItemsChanged(object sender, EventArgs e)
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
                    _cachedToolTipImage = new Bitmap(249, hasSockets ? 115 : 57, PixelFormat.Format32bppArgb);
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
                        case Quality.Common:
                            nameBrush = new SolidBrush(Color.FromKnownColor(KnownColor.InfoText));
                            break;
                        case Quality.Epic:
                            nameBrush = new SolidBrush(Color.Purple);
                            break;
                        case Quality.Legendary:
                            nameBrush = new SolidBrush(Color.Orange);
                            break;
                        case Quality.Poor:
                            nameBrush = new SolidBrush(Color.Gray);
                            break;
                        case Quality.Rare:
                            nameBrush = new SolidBrush(Color.Blue);
                            break;
                        case Quality.Uncommon:
                            nameBrush = new SolidBrush(Color.Gray);
                            break;
                    }
                    g.DrawString(CurrentItem.Name, _fontName, nameBrush, 2, 4);
                    nameBrush.Dispose();

                    g.DrawString(CurrentItem.Stats.Armor.ToString() + " Armor", _fontStats, SystemBrushes.InfoText, 2,
                                 21);
                    g.DrawString(CurrentItem.Stats.Stamina.ToString() + " Stamina", _fontStats, SystemBrushes.InfoText,
                                 85, 21);
                    g.DrawString(CurrentItem.Stats.Agility.ToString() + " Agility", _fontStats, SystemBrushes.InfoText,
                                 168, 21);
                    g.DrawString(CurrentItem.Stats.DodgeRating.ToString() + " Dodge", _fontStats, SystemBrushes.InfoText,
                                 2, 38);
                    g.DrawString(CurrentItem.Stats.DefenseRating.ToString() + " Defense", _fontStats,
                                 SystemBrushes.InfoText, 85, 38);
                    g.DrawString(CurrentItem.Stats.Resilience.ToString() + " Resilience", _fontStats,
                                 SystemBrushes.InfoText, 168, 38);

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
                                Rectangle rectGemBorder = new Rectangle(3 + (83*(i)), 59, 35, 35);
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

                                    string[] stats = gem.Stats.ToString().Split(',');
                                    if (stats.Length > 0)
                                        g.DrawString(stats[0].Trim(), _fontStats, SystemBrushes.InfoText,
                                                     rectGemBorder.X + 39, rectGemBorder.Y + 3);

                                    if (stats.Length > 1)
                                        g.DrawString(stats[1].Trim(), _fontStats, SystemBrushes.InfoText,
                                                     rectGemBorder.X + 39, rectGemBorder.Y + 20);
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
                            _fontStats, brushBonus, 2, 97);
                    }

                    g.Dispose();
                }
                return _cachedToolTipImage;
            }
        }

        private void ItemToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (e.AssociatedControl is IItemProvider)
            {
                //with hands held high into the sky so blue
                CurrentItem = (e.AssociatedControl as IItemProvider).GetItem();
                if (CachedToolTipImage != null)
                    e.ToolTipSize = CachedToolTipImage.Size;
            }
        }

        private void ItemToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (CachedToolTipImage != null)
                //the emotion opens up swallow you
                e.Graphics.DrawImageUnscaled(CachedToolTipImage, 0, 0);
        }
    }

    public interface IItemProvider
    {
        Item GetItem();
    }
}