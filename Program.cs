using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr
{
	
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
		static void Main()
		{
#if !DEBUG
            try
            {
#endif
                bool bAppFirstInstance;
                //use the app domain base directory to allow a second copy running in a different folder.
				System.Threading.Mutex oMutex = new System.Threading.Mutex(true, "Global\\Rawr-" + AppDomain.CurrentDomain.BaseDirectory.Replace('\\','|'), out bAppFirstInstance);
                if (bAppFirstInstance)
                {
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
#if !DEBUG
            }
            catch (Exception ex)
            {
               MessageBox.Show(GetErrorMessage(ex), "Rawr Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
#endif
        }

		private static string GetErrorMessage(Exception ex)
		{
			string error = "\r\n\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace;
			if (error.Contains("Could not load file or assembly 'Rawr.Base") ||
				error.Contains("Rawr.FormMain.LoadModel(String"))
			{
				return "Rawr was unable to find its required data files. Please make sure that you have fully unzipped Rawr to a location that you have full permissions to (such as My Documents). " +
					"If you still have this problem, please copy (CTRL-C) and paste this into an e-mail to cnervig@hotmail.com. Thanks!" + error;
			}
			if (error.Contains("Error creating window handle."))
			{
				return "Rawr encounted a serious error caused by an old version of the .NET Framework. Please download and install the latest version of the .NET Framework from Microsoft (http://www.microsoft.com/Net/Download.aspx)." +
					"If you still have this problem, please copy (CTRL-C) and paste this into an e-mail to cnervig@hotmail.com. Thanks!" + error;
			}
			return "Rawr encountered a serious error. Please copy (CTRL-C) and paste this into an e-mail to cnervig@hotmail.com. Thanks!" + error;
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