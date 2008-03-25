using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Xml;
using System.Drawing;

/*TODO: Make user actions (not system actions trying to refresh itself) override last fatal error and try again.
 * and if worked, reset fatalerror.  Another option would be to add a status panel like outlook and
 * give the user the option to try to reestablish network connectivity that way 
 * */
namespace Rawr
{
	public class WebRequestWrapper
	{
        private const int RETRY_MAX = 3;

		private class DownloadRequest
		{
		    public string serverPath;
		    public string localPath;
			public string error;
		}
		private Queue<DownloadRequest> _downloadRequests;
		private List<DownloadRequest> _failedRequests;
		private Thread[] _webRequestThreads;
		private string _proxyServer;
		private bool _useDefaultProxy;
		private string _proxyUserName;
		private string _proxyPassword;
        private string _proxyDomain;
        private int _proxyPort;
        
		
		private static bool _fatalError = false;

		public WebRequestWrapper()
		{
			int maxConnections = Rawr.Properties.NetworkSettings.Default.MaxHttpRequests;
			_failedRequests = new List<DownloadRequest>();
			_webRequestThreads = new Thread[maxConnections];
			_downloadRequests = new Queue<DownloadRequest>();
			_useDefaultProxy = Rawr.Properties.NetworkSettings.Default.UseDefaultProxySettings;
            
			_proxyServer = Rawr.Properties.NetworkSettings.Default.ProxyServer;
			_proxyPort = Rawr.Properties.NetworkSettings.Default.ProxyPort;
			_proxyUserName = Rawr.Properties.NetworkSettings.Default.ProxyUserName;
			_proxyPassword = Rawr.Properties.NetworkSettings.Default.ProxyPassword;
            _proxyDomain = Rawr.Properties.NetworkSettings.Default.ProxyDomain;
		}


		public string DownloadClassTalentTree(Character.CharacterClass characterClass)
		{
			//http://www.worldofwarcraft.com/shared/global/talents/{0}/data.js
			return DownloadText(string.Format(Properties.NetworkSettings.Default.ClassTalentURI, characterClass.ToString().ToLower()));
		}

		public XmlDocument DownloadCharacterTalentTree(string characterName, Character.CharacterRegion region, string realm)
		{
			//http://{0}.wowarmory.com/character-talents.xml?r={1}&n={2}
			string domain = region == Character.CharacterRegion.US ? "www" : "eu";
			XmlDocument doc = null;
			if (!String.IsNullOrEmpty(characterName))
			{
				doc = DownloadXml(string.Format(Properties.NetworkSettings.Default.CharacterTalentURI,
													domain, realm, characterName));
			}
			return doc;
		}

		public XmlDocument DownloadCharacterSheet(string characterName, Character.CharacterRegion region, string realm)
		{
			//http://{0}.wowarmory.com/character-sheet.xml?r={1}&n={2}
			string domain = region == Character.CharacterRegion.US ? "www" : "eu";
			XmlDocument doc = null;
			if (!String.IsNullOrEmpty(characterName))
			{
				doc = DownloadXml(string.Format(Properties.NetworkSettings.Default.CharacterSheetURI,
													domain, realm, characterName));
			}
			return doc;
		}

		public XmlDocument DownloadUpgrades(string characterName, Character.CharacterRegion region, string realm, int itemId)
		{
			//http://{0}.wowarmory.com/search.xml?searchType=items&pr={1}&pn={2}&pi={3}
			string domain = region == Character.CharacterRegion.US ? "www" : "eu";
			XmlDocument doc = null;
			if (!String.IsNullOrEmpty(characterName))
			{
				doc = DownloadXml(string.Format(Properties.NetworkSettings.Default.ItemUpgradeURI,
													domain, realm, characterName, itemId.ToString()));
			}
			return doc;
		}

        public XmlDocument DownloadItemInformation(int id)
        {
          return DownloadXml(string.Format(Properties.NetworkSettings.Default.ItemInfoURI, id.ToString()));
        }

		public XmlDocument DownloadItemToolTipSheet(string id)
		{
			XmlDocument doc = null;
			if (!String.IsNullOrEmpty(id))
			{
                int retry = 0;
                bool found = false;
                while (retry < RETRY_MAX && !found)
                {
                    doc = DownloadXml(string.Format(Properties.NetworkSettings.Default.ItemToolTipSheetURI, id));
                    if (doc != null && doc.DocumentElement != null
                                    && doc.DocumentElement.HasChildNodes && doc.DocumentElement.ChildNodes[0].HasChildNodes)
                    {
                        found = true;
                    }
                    else
                    {
                        //No such item exists or armory fail, try a couple times just to be sure
                        retry++;
                    }
                }
            }
			return doc;
		}

		/// <summary>
		/// Downloads the Item icon
		/// </summary>
		/// <param name="iconName">the name of the item icon to download, no extension, no path</param>
		/// <returns>The full path to the downloaded icon.  Null is returned if no icon  could be downloaded</returns>
		public string DownloadItemIcon(string iconName)
		{
			string filePath = Path.Combine(ItemImageCachePath, iconName + ".jpg");
			DownloadFile(Properties.NetworkSettings.Default.WoWItemIconURI + iconName + ".jpg",
							filePath);
			if (!File.Exists(filePath))
			{
				filePath = null;
			}
			return filePath;
		}
		
		/// <summary>
		/// Downloads the icon associated with the talent passed in
		/// </summary>
		/// <param name="charClass">CharacterClass of the given icon</param>
		/// <param name="talentTree">name of the talent tree</param>
		/// <param name="talentName">name of the talent</param>
		/// <returns>The full path to the downloaded icon.  Null is returned if no icon could be downloaded</returns>
		public string DownloadTalentIcon(Character.CharacterClass charClass, string talentTree, string talentName)
		{
			string imageName = talentTree + "_" + talentName + ".jpg";
            string fullPathToSave = Path.Combine(TalentImageCachePath, charClass.ToString().ToLower()+"\\"+imageName);

			if (!String.IsNullOrEmpty(talentTree) && !String.IsNullOrEmpty(talentName))
			{
				//0 = class, 1=tree, 2=talentname - all lowercase
				//@"http://www.worldofwarcraft.com/shared/global/talents/{0}/images/{1}/{2}.jpg";
				string uri = string.Format(Properties.NetworkSettings.Default.WoWTalentIconURI, charClass.ToString().ToLower(),
												talentTree.ToLower(),talentName.ToLower());
				DownloadFile(uri, fullPathToSave);
			}
			if (!File.Exists(fullPathToSave))
			{
				fullPathToSave = null;
			}
			return fullPathToSave;
		}

		/// <summary>
		/// Downloads the temp image for use as an icon.
		/// </summary>
		/// <returns>Full path to the temp image.</returns>
		public string DownloadTempImage()
		{
            return DownloadItemIcon("temp");
		}

		/// <summary>
		/// Gets the number of request failures since the last time the failure list was cleared.
		/// </summary>
		public int QueueFailureCount
		{
			get { return _failedRequests.Count; }
		}


		/// <summary>
		/// Count of the currently queued download requests.
		/// </summary>
		public int RequestQueueCount
		{
			get { return _downloadRequests.Count; }
		}

		/// <summary>
		/// If the last request received a 407 or no response. Used to prevent a lot of bad calls.
		/// It also has the good side effect of not locking someone's account out if they enter the proxy info incorrectly
		/// by sending lots of bad authorization attempts.
		/// </summary>
		public static bool LastWasFatalError
		{
			get { return _fatalError; }
		}

		public static void ResetFatalErrorIndicator()
		{
			_fatalError = false;
		}
		/// <summary>
		/// Downloads an Icon Asyncronously
		/// </summary>
		/// <param name="iconPath">The name of the icon to download.  No extension, No Path.</param>
		public void DownloadItemIconAsync(string iconName)
		{
			string localPath = Path.Combine(ItemImageCachePath, iconName + ".jpg");
			if (!File.Exists(localPath))
			{
				DownloadRequest dl = new DownloadRequest();
				dl.serverPath = Properties.NetworkSettings.Default.WoWItemIconURI + iconName + ".jpg";
				dl.localPath = localPath;
				InitiateRequest(dl);
			}
		}


		private string ItemImageCachePath
		{
			get
			{
				return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
									Properties.CacheSettings.Default.RelativeItemImageCache));
			}
		}

		private string TalentImageCachePath
		{
			get
			{
				return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
								Properties.CacheSettings.Default.RelativeTalentImageCache));
			}
		}

		/// <summary>
		/// Used to create a web client with all of the appropriote proxy/useragent/etc settings
		/// </summary>
		private WebClient CreateWebClient()
		{
			WebClient client = new WebClient();
			client.Headers.Add("user-agent", Properties.NetworkSettings.Default.UserAgent);
			if (Properties.NetworkSettings.Default.ProxyType == "Http")
			{
				if (_useDefaultProxy)
				{
					client.Proxy = HttpWebRequest.DefaultWebProxy;
				}
				else if (!String.IsNullOrEmpty(_proxyServer))
				{
					client.Proxy = new WebProxy(_proxyServer, _proxyPort);
				}
				if (client.Proxy != null && Rawr.Properties.NetworkSettings.Default.ProxyRequiresAuthentication)
				{
					if (Rawr.Properties.NetworkSettings.Default.UseDefaultAuthenticationForProxy)
					{
						client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
					}
					else
					{
						client.Proxy.Credentials = new NetworkCredential(_proxyUserName, _proxyPassword, _proxyDomain);
					}
				}
			}
			return client;
		}

		/// <summary>
		/// Download a given file with the appropriote configuration information
		/// </summary>
		/// <param name="serverPath">URI to download</param>
		/// <param name="localPath">local path, including file name,  where the downloaded file will be saved</param>
		private void DownloadFile(string URI, string localPath)
		{
			int retry = 0;
			bool success = false;
			if (!File.Exists(localPath))
			{
				do
				{
					if (!LastWasFatalError)
					{
						if (!Directory.Exists(Path.GetDirectoryName(localPath)))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(localPath));
						}
						using (WebClient client = CreateWebClient())
						{
							try
							{
								client.DownloadFile(URI, localPath);
								success = true;
							}
							catch (Exception ex)
							{
								CheckExecptionForFatalError(ex);
								//if on a client file download, there is an exception, 
								//it will create a 0 byte file. We don't want that empty file.
								if (File.Exists(localPath))
								{
									File.Delete(localPath);
								}
								retry++;
								if (retry == RETRY_MAX || LastWasFatalError)
								{
									throw;
								}
							}
						}
					}
				} while (retry <= RETRY_MAX && !success);
			}
		}

		/// <summary>
		/// This is used to prevent multiple attempts at network traffic when its not working and 
		/// continuing to issue requests could cause serious problems for the user.
		/// </summary>
		/// <param name="ex"></param>
		private void CheckExecptionForFatalError(Exception ex)
		{
			Log.Write("Exception trying to download: "+ ex);
            Log.Write(ex.StackTrace);
			if (ex.Message.Contains("407") /*proxy auth required */
				|| ex.Message.Contains("403") /*proxy info probably wrong, if we keep issuing requests, they will probably get locked out*/
				|| ex.Message.Contains("timed out") /*either proxy required and firewall dropped the request, or armory is down*/
				)
			{
				_fatalError = true;
			}
		}

		private string DownloadText(string URI)
		{
			WebClient webClient = CreateWebClient();
			string value = null;
			int retry = 0;
			bool success = false;
			do
			{
				if (!LastWasFatalError)
				{
					try
					{
						value = webClient.DownloadString(URI);
						if (!String.IsNullOrEmpty(value))
						{
							success = true;
						}
					}
					catch (Exception ex)
					{
						CheckExecptionForFatalError(ex);
					}
				}
				retry++;
			} while (retry <= RETRY_MAX && !success);
			return value;
		}

		private XmlDocument DownloadXml(string URI)
		{
			XmlDocument returnDocument = null;
			string xml = DownloadText(URI);
			if (!String.IsNullOrEmpty(xml))
			{
				returnDocument = new XmlDocument();
				returnDocument.LoadXml(xml);
			}
			return returnDocument;
		}

		/// <summary>
		/// Queues up download requests and then starts a new thread if the thread pool is not full.
		/// </summary>
		/// <param name="dl">Download Request to Service</param>
		private void InitiateRequest(DownloadRequest dl)
		{
			lock (_downloadRequests)
			{
				_downloadRequests.Enqueue(dl);
			}

			for (int i = 0; i < _webRequestThreads.Length; i++)
			{
				if (_webRequestThreads[i] == null || _webRequestThreads[i].ThreadState == ThreadState.Stopped || _webRequestThreads[i].ThreadState == ThreadState.Aborted)
				{
					//Thread is either null or terminated, start a new one
					_webRequestThreads[i] = new Thread(new ThreadStart(ThreadDoWork));
					_webRequestThreads[i].Start();
					break;
				}
			}
		}

		/// <summary>
		/// Loop over the queued up download requests and service them.  Terminate thread when queue is empty.
		/// </summary>
		private void ThreadDoWork()
		{
            try
            {
                DownloadRequest dl = null;
                while (_downloadRequests.Count > 0 && !_fatalError)
                {
                    lock (_downloadRequests)
                    {
                        if (_downloadRequests.Count > 0)
                        {
                            dl = _downloadRequests.Dequeue();
                        }
                    }
                    if (dl != null)
                    {
                        try
                        {
                            DownloadFile(dl.serverPath, dl.localPath);
                        }
                        catch (Exception ex)
                        {
                            CheckExecptionForFatalError(ex);
                            dl.error = ex.Message;
                            _failedRequests.Add(dl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.StackTrace);
            }
		}
	}
}
