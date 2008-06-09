using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Rawr.WCFService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class RawrService : IRawrService
    {
        public Dictionary<string, string> GetCharacterDisplayCalculationValues(string character, string model)
        {
            if (!Calculations.Models.ContainsKey(model)) throw new ArgumentException("Model does not exist.");
            Calculations.LoadModel(Calculations.Models[model]);
            Character c = Character.LoadFromXml(character);
            return Calculations.GetCharacterCalculations(c).GetCharacterDisplayCalculationValues();
        }

        public string[] GetSupportedModels()
        {
            return Calculations.Models.Keys.ToArray();
        }

        static RawrService()
        {
            WebRequestWrapper.NetworkSettingsProvider = new WebServiceNetworksSettingsProvider();
            WebRequestWrapper.CacheSettingsProvider = new WebServiceCacheSettingsProvider();
        }

        private class WebServiceNetworksSettingsProvider : WebRequestWrapper.INetworkSettingsProvider
        {
            #region INetworkSettingsProvider Members

            public int MaxHttpRequests
            {
                get { return 5; }
            }

            public bool UseDefaultProxySettings
            {
                get { return true; }
            }

            public string ProxyServer
            {
                get { return ""; }
            }

            public int ProxyPort
            {
                get { return 0; }
            }

            public string ProxyUserName
            {
                get { return ""; }
            }

            public string ProxyPassword
            {
                get { return ""; }
            }

            public string ProxyDomain
            {
                get { return ""; }
            }

            public string ProxyType
            {
                get { return "None"; }
            }

            public string UserAgent
            {
                get
                {
                    return "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                }
            }

            public bool DownloadItemInfo
            {
                get { return false; }
            }

            public bool ProxyRequiresAuthentication
            {
                get { return false; }
            }

            public bool UseDefaultAuthenticationForProxy
            {
                get { return false; }
            }

            public string WoWItemIconURI
            {
                get { return "http://www.wowarmory.com/images/icons/64x64/"; }
            }

            public string UserAgent_IE7
            {
                get { return "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; bgft) "; }
            }

            public string UserAgent_IE6
            {
                get { return "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) "; }
            }

            public string UserAgent_FireFox2
            {
                get
                {
                    return "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                }
            }

            public string ClassTalentURI
            {
                get { return "http://www.worldofwarcraft.com/shared/global/talents/{0}/data.js"; }
            }

            public string CharacterTalentURI
            {
                get { return "http://{0}.wowarmory.com/character-talents.xml?r={1}&n={2}"; }
            }

            public string CharacterSheetURI
            {
                get { return "http://{0}.wowarmory.com/character-sheet.xml?r={1}&n={2}"; }
            }

            public string ItemToolTipSheetURI
            {
                get { return "http://www.wowarmory.com/item-tooltip.xml?i={0}"; }
            }

            public string ItemUpgradeURI
            {
                get { return "http://{0}.wowarmory.com/search.xml?searchType=items&pr={1}&pn={2}&pi={3}"; }
            }

            public string WoWTalentIconURI
            {
                get { return "http://www.worldofwarcraft.com/shared/global/talents/{0}/images/{1}/{2}.jpg"; }
            }

            public string ItemInfoURI
            {
                get { return "http://www.wowarmory.com/item-info.xml?i={0}"; }
            }

            #endregion
        }

        private class WebServiceCacheSettingsProvider : WebRequestWrapper.ICacheSettingsProvider
        {
            #region ICacheSettingsProvider Members

            public string RelativeItemImageCache
            {
                get { return "images"; }
            }

            public string RelativeTalentImageCache
            {
                get { return "talent_images"; }
            }

            #endregion
        }

    }
}
