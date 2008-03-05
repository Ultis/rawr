using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Xml;
using log4net;

/*TODO: Make user actions (not system actions trying to refresh itself) override last fatal error and try again.
 * and if worked, reset fatalerror.  Another option would be to add a status panel like outlook and
 * give the user the option to try to reestablish network connectivity that way 
 * */
namespace Rawr
{
	public class WebRequestWrapper
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(WebRequestWrapper));

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

		/// <summary>
		/// Download an Icon
		/// </summary>
		/// <param name="iconName">Name of icon to download</param>
		/// <param name="localPath">Local path to save the downloaded file</param>
		public void DownloadIcon(string iconName, string localPath)
		{
			DownloadFile(Properties.NetworkSettings.Default.WoWIconURI + iconName + ".jpg", localPath);
		}

		/// <summary>
		/// Downloads an Icon Asyncronously
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="localPath"></param>
		public void DownloadIconAsync(string iconName, string localPath)
		{
			DownloadRequest dl = new DownloadRequest();
			dl.serverPath = Properties.NetworkSettings.Default.WoWIconURI + iconName + ".jpg";
			dl.localPath = localPath;
			InitiateRequest(dl);
		}

		/// <summary>
		/// Used to set the proxy information on a WebClient Object
		/// </summary>
		/// <param name="client">The WebClient object to modify</param>
		private void SetProxyInformation(WebClient client)
		{
			if (_useDefaultProxy)
			{
				client.Proxy = HttpWebRequest.DefaultWebProxy;
			}
			else if (!String.IsNullOrEmpty(_proxyServer))
			{
				client.Proxy = new WebProxy(_proxyServer, _proxyPort);
			}
			if (client.Proxy != null && !String.IsNullOrEmpty(_proxyUserName) && !String.IsNullOrEmpty(_proxyPassword))
			{
				client.Proxy.Credentials = new NetworkCredential(_proxyUserName, _proxyPassword);
			}
		}


		/// <summary>
		/// Download a given file with the appropriote configuration information
		/// </summary>
		/// <param name="serverPath">URI to download</param>
		/// <param name="localPath">local path, including file name,  where the downloaded file will be saved</param>
		private void DownloadFile(string URI, string localPath)
		{
			if (!LastWasFatalError)
			{
				WebClient client = new WebClient();
				SetProxyInformation(client);
				try
				{
					client.DownloadFile(URI, localPath);
				}
				catch(Exception ex)
				{
					CheckExecptionForFatalError(ex);
					//if on a client file download, there is an exception, 
					//it will create a 0 byte file. We don't want that empty file.
					if (File.Exists(localPath))
					{
						File.Delete(localPath);
					}
					throw;
				}
			}
		}

		/// <summary>
		/// This is used to prevent multiple attempts at network traffic when its not working and 
		/// continuing to issue requests could cause serious problems for the user.
		/// </summary>
		/// <param name="ex"></param>
		private void CheckExecptionForFatalError(Exception ex)
		{
			log.Error("Exception trying to download", ex);
			if (ex.Message.Contains("407") /*proxy auth required */
				|| ex.Message.Contains("403") /*proxy info probably wrong, if we keep issuing requests, they will probably get locked out*/
				|| ex.Message.Contains("timed out") /*either proxy required and firewall dropped the request, or armory is down*/
				)
			{
				_fatalError = true;
			}
		}

		public string DownloadText(string URI)
		{
			WebClient webClient = new WebClient();
			SetProxyInformation(webClient);
			string value = null;
			try
			{
				value = webClient.DownloadString(URI);
			}
			catch (Exception ex)
			{
				CheckExecptionForFatalError(ex);
			}
			return value;
		}

		public XmlDocument DownloadXml(string URI)
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
				if (_webRequestThreads[i] == null || _webRequestThreads[i].ThreadState == ThreadState.Stopped)
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

	}
}
