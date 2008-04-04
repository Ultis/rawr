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

            StatusMessaging.UpdateStatus("Cache Item Icons", "Caching all Item Icons");
			WebRequestWrapper webRequests = new WebRequestWrapper();
			List<string> filesDownloading = new List<string>();
			for(int i=0;i<items.Length && !WebRequestWrapper.LastWasFatalError;i++)
            {
				string iconName = items[i].IconPath.Replace(".png", "").Replace(".jpg", "").ToLower();
			    webRequests.DownloadItemIconAsync(iconName);
            }
            
			while (webRequests.RequestQueueCount > 0 && !WebRequestWrapper.LastWasFatalError)
            {
                Thread.Sleep(200);
            }

			//TODO: Display any error information appropriotly here,
			//such as, proxy information incorrect or network connection down. Can get pretty detailed about exactly what went 
			//wrong by analyzing the exception.
			if (WebRequestWrapper.LastWasFatalError)
			{
                StatusMessaging.ReportError("Cache All Icons", null, "There was an error trying to retrieve images from the armory.  Please check your proxy settings and network connection.");
			}
            StatusMessaging.UpdateStatusFinished("Cache Item Icons");
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

        public static Image GetItemIcon(string iconName, bool small)
        {
            Image returnImage = null;
            iconName = iconName.Replace(".png", "").Replace(".jpg", "").ToLower();
            if (small && SmallIcons.Images.ContainsKey(iconName))
            {
                returnImage = SmallIcons.Images[iconName];
            }
            else if (!small && LargeIcons.Images.ContainsKey(iconName))
            {
                returnImage = LargeIcons.Images[iconName];
            }
            else
            {
                string pathToIcon = null;
                try
                {
                    WebRequestWrapper wrapper = new WebRequestWrapper();
                    if (!String.IsNullOrEmpty(iconName))
                    {

                        pathToIcon = wrapper.DownloadItemIcon(iconName);
                        //just in case the network code is in a disconnected mode. (e.g. no network traffic sent, so no network exception)
                    }

                    if (pathToIcon == null)
                    {
                        pathToIcon = wrapper.DownloadTempImage();
                    }
                }
                catch (Exception)
                {
                    //Log.Write(ex.Message);
                    //Log.Write(ex.StackTrace);
                    //log.Error("Exception trying to retrieve an icon from the armory", ex);
                }
                if (!String.IsNullOrEmpty(pathToIcon))
                {
                    int retry = 0;
                    do
                    {
                        try
                        {
                            using (Stream fileStream = File.Open(pathToIcon, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                returnImage = Image.FromStream(fileStream);
                            }
                        }
                        catch
                        {
                            returnImage = null;
                            //possibly still downloading, give it a second
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            if (retry >= 3)
                            {
                                //log.Error("Exception trying to load an icon from local", ex);
                                MessageBox.Show(
                                    "Rawr encountered an error while attempting to load a saved image. If you encounter this error multiple times, please ensure that Rawr is unzipped in a location that you have full file read/write access, such as your Desktop, or My Documents.");
                                //Log.Write(ex.Message);
                                //Log.Write(ex.StackTrace);
                                #if DEBUG
                                throw;
                                #endif
                            }
                        }
                        retry++;
                    } while (returnImage == null && retry < 5);

                    if (returnImage != null)
                    {
                        if (small)
                        {
                            returnImage = ScaleByPercent(returnImage, 50);
                            SmallIcons.Images.Add(iconName, returnImage);
                        }
                        else
                        {
                            LargeIcons.Images.Add(iconName, returnImage);
                        }
                    }
                }
            }
            return returnImage;
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