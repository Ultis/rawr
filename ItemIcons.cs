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
//using log4net;

namespace Rawr
{
    public static class ItemIcons
    {
		//private static readonly ILog log = LogManager.GetLogger(typeof(ItemIcons));

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

        public static void CacheAllIcons(Item[] items)
        {
            string iconPath;
            string localPath;
			WebRequestWrapper webRequests = new WebRequestWrapper();
			string localImageCacheDir = AppDomain.CurrentDomain.BaseDirectory + "images\\";
			if (!Directory.Exists(localImageCacheDir))
			{
				Directory.CreateDirectory(localImageCacheDir);
			}
			List<string> filesDownloading = new List<string>();
            for(int i=0;i<items.Length && !WebRequestWrapper.LastWasFatalError;i++)
            {
				iconPath = items[i].IconPath.Replace(".png", "").Replace(".jpg", "").ToLower();
				localPath = Path.Combine(localImageCacheDir,iconPath + ".jpg");
                if (!File.Exists(localPath) && !filesDownloading.Contains(localPath))
                {
                    filesDownloading.Add(localPath);
                    BackgroundWorker worker = new BackgroundWorker();
					webRequests.DownloadIconAsync(iconPath, localPath);
                }
            }
            
			//the main GUI thread should not be made to sleep as it will cause white outs, a good solution to prevent 
			//other interaction with the app while performing long running processes is to create a status form that is forced
			//in front of the other forms and updates periodically with information, say on item 400 of 500 or 100 images
			//left to download.  It also has a cancel button if the user doesn't want to wait anymore.
			while (webRequests.RequestQueueCount > 0 && !WebRequestWrapper.LastWasFatalError)
            {
                Thread.Sleep(200);
            }

			//Display any error information appropriotly here,
			//such as, proxy information incorrect or network connection down. Can get pretty detailed about exactly what went 
			//wrong by analyzing the exception.
			if (WebRequestWrapper.LastWasFatalError)
			{
				MessageBox.Show("There was an error trying to retrieve images from the armory.  Please check your proxy settings and network connection.");
			}
        }

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
				string imgDir = AppDomain.CurrentDomain.BaseDirectory + "images\\";
                string localPath =
                    Path.Combine(imgDir, iconPath + ".jpg");
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                if (!File.Exists(localPath))
                {
                    try
                    {
						//log.Debug("Getting icon from the armory");
						if (!WebRequestWrapper.LastWasFatalError)
						{
							WebRequestWrapper wrapper = new WebRequestWrapper();
							wrapper.DownloadIcon(iconPath, localPath);
							//just in case the network code is in a disconnected mode. (e.g. no network traffic sent, so no network exception)
							if (!File.Exists(localPath))
							{
								return null;
							}
						}
						else
						{
							localPath = Path.Combine(imgDir, "temp.jpg");
							if (!File.Exists(localPath))
							{
								return null;
							}
						}
						
                    }
                    catch(Exception ex)
                    {
						//log.Error("Exception trying to retrieve an icon from the armory", ex);
						localPath = Path.Combine(imgDir, "temp.jpg");
						if (!File.Exists(localPath))
						{
							return null;
						}
                    }
                }
                Image fullSizeImage = null;
                try
                {
                    fullSizeImage = Image.FromFile(localPath);
                    if (small) SmallIcons.Images.Add(iconPath, ScaleByPercent(Image.FromFile(localPath), 50));
                    else LargeIcons.Images.Add(iconPath, Image.FromFile(localPath));
                }
                catch(Exception ex)
                {
					//log.Error("Exception trying to load an icon from local", ex);
                    MessageBox.Show(
                        "Rawr encountered an error while attempting to load a saved image. If you encounter this error multiple times, please ensure that Rawr is unzipped in a location that you have full file read/write access, such as your Desktop, or My Documents.");
                }
            }
            return small ? SmallIcons.Images[iconPath] : LargeIcons.Images[iconPath];
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