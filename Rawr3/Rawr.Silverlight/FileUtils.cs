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

namespace Rawr.Silverlight
{
    public class FileUtils
    {
        public EventHandler StreamReady;
        public string Filename { get; private set; }

        public StreamReader Reader { get { return new StreamReader(ISFileStream(), Encoding.UTF8); } }
        public StreamWriter Write { get { return new StreamWriter(ISFileStream(), Encoding.UTF8); } }

        public FileUtils(string filename, EventHandler callback)
        {
            StreamReady += callback;
            Filename = filename;

            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(Filename))
            {
                if (StreamReady != null) StreamReady.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Uri url = new Uri(filename, UriKind.Relative);
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
                client.DownloadStringAsync(url);
            }
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                using (StringReader sr = new StringReader(e.Result))
                {
                    IsolatedStorageFileStream isfs = ISFileStream();
                    StreamWriter sw = new StreamWriter(isfs);
                    sw.Write(sr.ReadToEnd());
                    sw.Close();

                }
            }
            if (StreamReady != null) StreamReady.Invoke(this, EventArgs.Empty);
        }

        private IsolatedStorageFileStream ISFileStream()
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (isf.FileExists(Filename)) return new IsolatedStorageFileStream(Filename, FileMode.Open, isf);
            else return new IsolatedStorageFileStream(Filename, FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication());
        }

        public static bool HasQuota(int kilobytes)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.Quota >= kilobytes * 1024;
            }
        }

        public static bool EnsureQuota(int kilobytes)
        {
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
        }
    }
}
