using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace Rawr.UI
{
    public class FileUtils
    {
        public EventHandler StreamReady;
        public string Filename { get; private set; }

        public IsolatedStorageFileStream Reader { get { return FileStream(false); } }
        public IsolatedStorageFileStream Writer { get { return FileStream(true); } }

        public FileUtils(string filename)
        {
            Filename = filename;
        }

        public void DownloadIfNotExists(EventHandler callback)
        {
            StreamReady += callback;
#if WPF
            if (File.Exists(Filename))
            {
                if (StreamReady != null) StreamReady.Invoke(this, EventArgs.Empty);
            }
#else
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Filename))
            {
                if (StreamReady != null) StreamReady.Invoke(this, EventArgs.Empty);
            }
#endif
            else
            {
                Uri url = new Uri(Filename, UriKind.Relative);
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(url);
            }
        }

		public void Delete()
		{
#if WPF
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }
#else
			if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Filename))
			{
				IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(Filename);
			}
#endif
		}

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (StringReader sr = new StringReader(e.Result))
                {
#if WPF
                    StreamWriter sw = new StreamWriter(Filename);
#else
                    IsolatedStorageFileStream isfs = FileStream(true);
                    StreamWriter sw = new StreamWriter(isfs);
#endif
                    sw.Write(sr.ReadToEnd());
                    sw.Close();

                }
            }
            if (StreamReady != null) StreamReady.Invoke(this, EventArgs.Empty);
        }

#if WPF
        private FileStream FileStream()
        {
            return new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
#else
        private IsolatedStorageFileStream FileStream(bool write)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (write) return new IsolatedStorageFileStream(Filename, FileMode.Create, isf);
            else return new IsolatedStorageFileStream(Filename, FileMode.OpenOrCreate, isf);
        }
#endif

        public static bool HasQuota(int kilobytes)
        {
#if WPF 
            return true;
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.Quota >= kilobytes * 1024;
            }
#endif
        }

        public static bool EnsureQuota(int kilobytes)
        {
#if WPF
            return true;
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long newSpace = kilobytes * 1024; 
                try
                {
                    return isf.IncreaseQuotaTo(newSpace);
                }
                catch { }
            }
            return false;
#endif
        }
    }
}
