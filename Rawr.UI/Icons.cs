using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Rawr
{
    public static class Icons
    {
        public static BitmapImage TreeBackground(CharacterClass charClass, int Tree)
        {
            try {
#if !SILVERLIGHT
                string RootDir = AppDomain.CurrentDomain.BaseDirectory;
                Uri uriLocal = new Uri(string.Format(RootDir + "images\\{0}\\{1}", "backgrounds", charClass.ToString().ToLower() + ".jpg"), UriKind.Absolute);
                Uri uriWeb = new Uri(string.Format("http://static.wowhead.com/images/wow/talents/backgrounds/live/{0}_{1}.jpg", charClass.ToString().ToLower(), Tree + 1));
                // Ensure directory paths
                if (!Directory.Exists(RootDir + "images\\"                  )) { Directory.CreateDirectory(RootDir + "images\\"                  ); }
                if (!Directory.Exists(RootDir + "images\\" + "backgrounds\\")) { Directory.CreateDirectory(RootDir + "images\\" + "backgrounds\\"); }
                // TODO: Do a network test to see if there's no connectivity, if not don't try to pull the image from wowhead
                BitmapImage retVal;
                if (File.Exists(uriLocal.LocalPath)) {
                    retVal = NewBitmapImage(uriLocal);
                } else {
                    retVal = NewBitmapImage(uriWeb);
                    // Set the file up to save to the directory after it is downloaded
                    Image image = new Image();
                    BitmapImage theicon = new BitmapImage();
                    theicon.BeginInit();
                    theicon.CacheOption = BitmapCacheOption.OnLoad;
                    theicon.UriSource = uriWeb;
                    theicon.DecodePixelHeight = 554;
                    theicon.DecodePixelWidth = 204;
                    theicon.EndInit();
                    image.Source = theicon;
                    theicon.DownloadCompleted += new EventHandler(objImageBg_DownloadCompleted);
                }
                return retVal;
#else
                Uri uri = new Uri(string.Format("http://static.wowhead.com/images/wow/talents/backgrounds/live/{0}_{1}.jpg", charClass.ToString().ToLower(), Tree + 1));
                return NewBitmapImage(uri);
#endif
            } catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Returns an image from the wowhead database
        /// </summary>
        /// <param name="icon">The name of the icon: "ability_warrior_colossussmash.jpg"</param>
        /// <param name="size">2 large, 1 medium, 0 small, defaults large)))</param>
        /// <returns></returns>
        public static BitmapImage AnIcon(string icon, int size = 2)
        {
            if (String.IsNullOrEmpty(icon)) { return null; }
            string sizee = (size == 2 ? "large" : (size == 1 ? "medium" : (size == 0 ? "small" : "large")));
            try {
#if !SILVERLIGHT
                string RootDir = AppDomain.CurrentDomain.BaseDirectory;
                Uri uriLocal = new Uri(string.Format(RootDir + "images\\{0}\\{1}", sizee, icon + (!icon.Contains(".jpg") ? ".jpg" : "")), UriKind.Absolute);
                Uri uriWeb   = new Uri(string.Format(Rawr.Properties.NetworkSettings.Default.WowheadIconURI,
                    sizee, icon + (!icon.Contains(".jpg") ? ".jpg" : "")), UriKind.Absolute);
                // Ensure directory paths
                if (!Directory.Exists(RootDir + "images\\")) { Directory.CreateDirectory(RootDir + "images\\"); }
                if (!Directory.Exists(RootDir + "images\\" + sizee + "\\")) { Directory.CreateDirectory(RootDir + "images\\" + sizee + "\\"); }
                // TODO: Do a network test to see if there's no connectivity, if not don't try to pull the image from wowhead
                BitmapImage retVal;
                if (File.Exists(uriLocal.LocalPath)) {
                    retVal = NewBitmapImage(uriLocal);
                } else {
                    retVal = NewBitmapImage(uriWeb);
                    // Set the file up to save to the directory after it is downloaded
                    Image image = new Image();
                    BitmapImage theicon = new BitmapImage();
                    theicon.BeginInit();
                    theicon.CacheOption = BitmapCacheOption.OnLoad;
                    theicon.UriSource = uriWeb;
                    theicon.DecodePixelHeight = 56;
                    theicon.DecodePixelWidth = 56;
                    theicon.EndInit();
                    image.Source = theicon;
                    theicon.DownloadCompleted += new EventHandler(objImage_DownloadCompleted);
                }
                return retVal;
#else
                Uri uri = new Uri(string.Format(Rawr.Properties.NetworkSettings.Default.WowheadIconURI,
                    sizee, icon + (!icon.Contains(".jpg") ? ".jpg" : "")), UriKind.Absolute);
                return NewBitmapImage(uri);
#endif
            } catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// This function saves the image as a .jpg<br />
        /// * .\images\[size]\ability_name.jpg
        /// </summary>
        private static void objImage_DownloadCompleted(object sender, EventArgs e) {
#if !SILVERLIGHT
            BitmapImage theicon = sender as BitmapImage;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            String photolocation = AppDomain.CurrentDomain.BaseDirectory + "images\\"
                + theicon.UriSource.Segments[theicon.UriSource.Segments.Length - 2] // size (large)
                + theicon.UriSource.Segments[theicon.UriSource.Segments.Length - 1]; // file name
            encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));
            try {
                using (var filestream = new FileStream(photolocation, FileMode.OpenOrCreate))
                {
                    encoder.Save(filestream);
                    filestream.Close();
                }
            } catch (System.IO.FileFormatException) {
                // ignore it, we couldn't save the file because the web server has a corrupted image
            } catch (Exception) {
                // for now, ignore it
            }
#endif
        }
        /// <summary>
        /// This function saves the image as a .jpg<br />
        /// * .\images\backgrounds\classname_[tree+1].jpg
        /// </summary>
        private static void objImageBg_DownloadCompleted(object sender, EventArgs e)
        {
#if !SILVERLIGHT
            BitmapImage theicon = sender as BitmapImage;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            String photolocation = AppDomain.CurrentDomain.BaseDirectory + "images\\"
                + theicon.UriSource.Segments[theicon.UriSource.Segments.Length - 3] // backgrounds
                + theicon.UriSource.Segments[theicon.UriSource.Segments.Length - 1]; // file name
            encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));
            try
            {
                using (var filestream = new FileStream(photolocation, FileMode.OpenOrCreate))
                {
                    encoder.Save(filestream);
                    filestream.Close();
                }
            }
            catch (System.IO.FileFormatException)
            {
                // ignore it, we couldn't save the file because the web server has a corrupted image
            }
            catch (Exception)
            {
                // for now, ignore it
            }
#endif
        }

        public static BitmapImage NewBitmapImage(Uri uri)
        {
            // this thing is throwing InvalidDeploymentException in WPF when you're catching handled exceptions
            // there's nothing wrong actually, it works when run normally
            try {
#if SILVERLIGHT
                return new BitmapImage(uri);
#else
                BitmapImage ret = new BitmapImage();
                ret.BeginInit();
                ret.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                ret.UriSource = uri;
                // EndInit throws an exception about application identiy not being set, find a way to catch this beforehad
                //if (Application.Current. ApplicationIdentity == null) { }
                try { ret.EndInit(); } catch { }
                return ret;
#endif
            } catch (Exception) { return null; }
        }
    }
}
