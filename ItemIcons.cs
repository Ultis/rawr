using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Rawr
{
    public static class ItemIcons
    {
        private static ImageList _largeIcons = null;

        private static ImageList LargeIcons
        {
            get
            {
                if (_largeIcons == null)
                {
                    _largeIcons = new ImageList();
                    _largeIcons.ImageSize = new Size(64, 64);
                    _largeIcons.ColorDepth = ColorDepth.Depth24Bit;
                }
                return _largeIcons;
            }
        }

        private static ImageList _smallIcons = null;

        private static ImageList SmallIcons
        {
            get
            {
                if (_smallIcons == null)
                {
                    _smallIcons = new ImageList();
                    _smallIcons.ImageSize = new Size(32, 32);
                    _smallIcons.ColorDepth = ColorDepth.Depth24Bit;
                }
                return _smallIcons;
            }
        }

        private static int _iconsCached = 0;
        private static int _iconsToCache = 0;

        public static void CacheAllIcons(Item[] items)
        {
            string iconPath;
            string localPath;
            _iconsCached = 0;
            _iconsToCache = 0;
            bool firstItem = true;
            List<string> filesDownloading = new List<string>();
            foreach (Item item in items)
            {
                iconPath = item.IconPath.Replace(".png", "").Replace(".jpg", "").ToLower();
                localPath =
                    Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "images\\" + iconPath + ".jpg");
                if (firstItem && !Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                firstItem = false;
                if (!filesDownloading.Contains(localPath) && !File.Exists(localPath))
                {
                    filesDownloading.Add(localPath);
                    _iconsToCache++;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                                         {
                                             string[] args = e.Argument.ToString().Split('#');
                                             DownloadIcon(args[0], args[1]);
                                             _iconsCached++;
                                         };
                    //worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
                    //{
                    //    if (!e.Cancelled && e.Error == null) _iconsCached++;
                    //};
                    worker.RunWorkerAsync(iconPath + "#" + localPath);
                }
            }
            while (_iconsCached < _iconsToCache)
            {
                Thread.Sleep(200);
            }
        }

        private static bool _proxyRequiresAuthentication = false;

        public static Image GetItemIcon(Item item)
        {
            return GetItemIcon(item, false);
        }

        public static Image GetItemIcon(Item item, bool small)
        {
            return GetItemIcon(item.IconPath, small);
        }

        public static Image GetItemIcon(string iconPath)
        {
            return GetItemIcon(iconPath, false);
        }

        public static Image GetItemIcon(string iconPath, bool small)
        {
            iconPath = iconPath.Replace(".png", "").Replace(".jpg", "").ToLower();
            if ((!small && !LargeIcons.Images.ContainsKey(iconPath)) ||
                (small && !SmallIcons.Images.ContainsKey(iconPath)))
            {
                string localPath =
                    Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "images\\" + iconPath + ".jpg");
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                if (!File.Exists(localPath))
                {
                    try
                    {
                        Log.Write("Getting Image from Armory:" + iconPath);
                        DownloadIcon(iconPath, localPath);
                    }
                    catch
                    {
                        localPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "images\\temp.jpg");
                        if (!File.Exists(localPath)) return null;
                    }
                }
                Image fullSizeImage = null;
                try
                {
                    fullSizeImage = Bitmap.FromFile(localPath);
					if (small) SmallIcons.Images.Add(iconPath, ScaleByPercent(Bitmap.FromFile(localPath), 50));
					else LargeIcons.Images.Add(iconPath, Bitmap.FromFile(localPath));
                }
                catch
                {
                    MessageBox.Show(
                        "Rawr encountered an error while attempting to load a saved image, so is retrying. If you encounter this error multiple times, please ensure that Rawr is unzipped in a location that you have full file read/write access, such as your Desktop, or My Documents.");
                    try
                    {
                        File.Delete(localPath);
                    }
                    catch
                    {
                    }
                    return GetItemIcon(iconPath, small);
                }
            }
            return small ? SmallIcons.Images[iconPath] : LargeIcons.Images[iconPath];
        }

        private static void DownloadIcon(string iconPath, string localPath)
        {
            WebClient client = new WebClient();
            try
            {
                if (_proxyRequiresAuthentication)
                {
                    client.Proxy = HttpWebRequest.DefaultWebProxy;
                    client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                }
                client.DownloadFile("http://www.wowarmory.com/images/icons/64x64/" + iconPath + ".jpg",
                                    localPath);
            }
            catch (Exception ex)
            {
                if (!_proxyRequiresAuthentication && ex.Message.Contains("Proxy Authentication Required"))
                {
                    _proxyRequiresAuthentication = true;
                    client.Proxy = HttpWebRequest.DefaultWebProxy;
                    client.Proxy.Credentials = CredentialCache.DefaultCredentials;
					client.DownloadFile("http://www.wowarmory.com/images/icons/64x64/" + iconPath + ".jpg",
                                        localPath);
                }
                else
                {
                    MessageBox.Show("Rawr encountered an error getting Image from Armory: " + localPath +
                                    ". Please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" +
                                    ex.Message + "\r\n\r\n" + ex.StackTrace);
					client.DownloadFile("http://www.wowarmory.com/images/icons/64x64/temp.jpg", localPath);
                }
            }
        }

        private static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float) Percent/100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int) (sourceWidth*nPercent);
            int destHeight = (int) (sourceHeight*nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                        PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                  imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                              new Rectangle(destX, destY, destWidth, destHeight),
                              new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                              GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}