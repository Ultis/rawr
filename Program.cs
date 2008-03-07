using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Rawr
{
	
    public static class Program
    {
		public static readonly ILog log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
		static void Main()
		{
            try
            {
                bool bAppFirstInstance;
				log.Debug("Grabbing Mutex");
                //use the app domain base directory to allow a second copy running in a different folder.
				System.Threading.Mutex oMutex = new System.Threading.Mutex(true, "Global\\Rawr-" + AppDomain.CurrentDomain.BaseDirectory.Replace('\\','|'), out bAppFirstInstance);
                if (bAppFirstInstance)
                {
                    log.Debug("Mutex Aquired, first instance");
                    //RawrCatIntro();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                else
                {
                    MessageBox.Show("Rawr is already running and cannot be opened a second time.");
                }
                //In release mode, the GC will collect oMutex right after it is created since it is not referenced anywhere else.
                //This line prevents the GC from collecting the object until the App closes.
                GC.KeepAlive(oMutex);
            }
            catch (TypeLoadException ex)
            {
				log.Error("Type Load Exception", ex);
                MessageBox.Show(ex.Message, "when cat durid is FITE do not ask for HEEL and NINIRVATE!"); //Heh
            }
            catch (Exception ex)
            {
				log.Error("Top Level exception caught", ex);
                #if DEBUG
                    //rethrow if debug mode
                    throw;
                #endif
                //MessageBox.Show("Rawr encountered a serious error. Please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }

		}

		//private static void RawrCatIntro()
		//{
		//    if (!System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "BuffCache.xml")))
		//    { //If they don't have a BuffCache, this is probably the first run, so display an intro to b10 message
		//        string message = "Welcome to Rawr b10!\r\n\r\n" +
		//            " Rawr b10 is probably the biggest update to Rawr in a while. " +
		//            "There's tons of new stuff, including (finally) Cat support! " +
		//            "It's also got several holes where there are features that I " +
		//            "haven't had a chance to finish, but I thought weren't worth " +
		//            "delaying the release of b10 further for. Since this is the " +
		//            "first release with Cat support, and a major upgrade as well, " +
		//            "I expect there to be a few bugs, which I hope to fix quickly. " +
		//            "\r\n\r\n I strongly encourage you to read the ReadMe in order " +
		//            "to see what's new, what's incomplete, etc. Would you like to " +
		//            "view the ReadMe now?";
		//        if (MessageBox.Show(message, "Welcome to Rawr b10!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
		//        {
		//            try
		//            {
		//                System.Diagnostics.Process.Start(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "ReadMe.txt"));
		//            }
		//            catch { }
		//        }
		//    }
		//} 
    }
}

//cold ground below...
//dissatisfied clouds above...