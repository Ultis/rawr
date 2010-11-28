using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Rawr.DPSWarr
{
    public static class AbilityIcons
    {
        /// <summary>
        /// Returns an image from the static wowhead database
        /// </summary>
        /// <param name="icon">The name of the icon: "ability_warrior_colossussmash.jpg"</param>
        /// <param name="size">2 large, 1 medium, 0 small, defaults large)))</param>
        /// <returns></returns>
        public static BitmapImage AnIcon(string icon, int size=2)
        {
            string sizee = (size == 2 ? "large" : (size == 1 ? "medium" : (size == 0 ? "small" : "large")));
            Uri uri = new Uri(string.Format(Rawr.Properties.NetworkSettings.Default.WowheadIconURI,
                sizee, icon+(!icon.Contains(".jpg")?".jpg":"")));
            return NewBitmapImage(uri);
        }

        public static BitmapImage NewBitmapImage(Uri uri)
        {
            // this thing is throwing InvalidDeploymentException in WPF when you're catching handled exceptions
            // there's nothing wrong actually, it works when run normally
#if SILVERLIGHT
            return new BitmapImage(uri);
#else
            BitmapImage ret = new BitmapImage();
            ret.BeginInit();
            ret.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            ret.UriSource = uri;
            ret.EndInit();
            return ret;
#endif
        }
    }
}