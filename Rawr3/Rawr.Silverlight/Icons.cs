using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;
//using log4net;

namespace Rawr
{
    public static class Icons
    {

        public static BitmapImage ItemIcon(string name)
        {

            return new BitmapImage(new Uri(string.Format("http://www.wowarmory.com/wow-icons/_images/64x64/{0}.jpg", name.ToLower())));
        }

        public static BitmapImage TreeBackground(Character.CharacterClass charClass, string talentTree)
        {
            return TalentIcon(charClass, talentTree, "background", true);
        }

        public static BitmapImage TalentIcon(Character.CharacterClass charClass, string talentTree, string talentName, bool on)
        {
            talentTree = talentTree.Replace(" ", "");
            talentName = talentName.Replace(" ", "");
            talentName = talentName.Replace(":", "");
            return new BitmapImage(new Uri(string.Format("http://www.worldofwarcraft.com/shared/global/talents/{0}/images/{1}/{2}{3}.jpg",
                charClass.ToString().ToLower(), talentTree.ToLower(), talentName.ToLower(), on ? "" : "-off"), UriKind.Absolute));
        }
    }
}