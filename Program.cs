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
                DG_RawrCrashReports report = new DG_RawrCrashReports() { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, SuggestedFix = GetSuggestedFix(ex) };
                report.ShowDialog();
                //MessageBox.Show(GetErrorMessage(ex), "Rawr Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
#endif
        }

        private static string GetSuggestedFix(Exception ex) {
            string error = ex.Message + ex.StackTrace;
            string SugFix = "";
            //
            if (error.Contains("Could not load file or assembly 'Rawr.Base")
                || error.Contains("Rawr.FormMain.LoadModel(String")) {
                SugFix = "Rawr was unable to find its required data files. Please make sure that you have "
                       + "fully unzipped Rawr to a location that you have full permissions to (such as My "
                       + "Documents or the Desktop). ";
            } else if (error.Contains("Access to the path")) {
                SugFix = "If you unzipped Rawr to the 'Program Files' directory on your system you need to "
                       + "move it to somewhere that you have full permissions, such as the Desktop.";
            } else  if (error.Contains("Error creating window handle.")) {
                SugFix = "Rawr encountered a serious error caused by an old version of the .NET Framework. "
                       + "Please download and install the latest version of the .NET Framework from Microsoft "
                       + "(http://www.microsoft.com/Net/Download.aspx).";
            } else  if (error.Contains("Verdana")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted copy of the 'Verdana' font. "
                       + "Try reinstalling .NET Framework 3.5SP1 (http://www.microsoft.com/Net/Download.aspx), "
                       + "or getting a copy from a friend.";
            } else if (error.Contains("Segou UI")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted copy of the 'Segou UI' font. "
                       + "Try reinstalling .NET Framework 3.5SP1 (http://www.microsoft.com/Net/Download.aspx), "
                       + "or getting a copy from a friend.";
            } else if (error.Contains("Configuration system failed")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted user settings file. Please "
                       + "try clearing your local settings by closing Rawr, and then deleting any folders "
                       + "whose name starts with 'Rawr' at C:\\Documents and Settings\\[User]\\AppData\\Local\\ "
                       + "or C:\\Users\\[User]\\AppData\\Local\\. You will need to display hidden folders to "
                       + "find those folders.";
            }
            //
            return SugFix;
        }
        private static string GetErrorMessage(Exception ex)
        {
            string errorFormat = "{0}== Error Message ==\r\n{1}\r\n\r\n== StackTrace ==\r\n{2}";
            string SugFix = "";
            string SugFix2 = "\r\n\r\nYou can also check the Troubleshooting Guide at http://rawr.codeplex.com/wikipage?title=Troubleshooting for more steps on getting Rawr to run smoothly."
                           + "\r\n\r\nIf you still have this problem after performing the suggested fix, please copy (CTRL-C) and paste this into an e-mail to WarcraftRawr@gmail.com. Thanks!\r\n\r\n\r\n";
            //
            if (ex.Message.Contains("Could not load file or assembly 'Rawr.Base")
                || ex.Message.Contains("Rawr.FormMain.LoadModel(String")) {
                SugFix = "Rawr was unable to find its required data files. Please make sure that you have "
                       + "fully unzipped Rawr to a location that you have full permissions to (such as My "
                       + "Documents or the Desktop). ";
            } else if (ex.Message.Contains("Access to the path")) {
                SugFix = "If you unzipped Rawr to the 'Program Files' directory on your system you need to "
                       + "move it to somewhere that you have full permissions, such as the Desktop.";
            } else  if (ex.Message.Contains("Error creating window handle.")) {
                SugFix = "Rawr encountered a serious error caused by an old version of the .NET Framework. "
                       + "Please download and install the latest version of the .NET Framework from Microsoft "
                       + "(http://www.microsoft.com/Net/Download.aspx).";
            } else  if (ex.Message.Contains("Verdana")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted copy of the 'Verdana' font. "
                       + "Try reinstalling .NET Framework 3.5SP1 (http://www.microsoft.com/Net/Download.aspx), "
                       + "or getting a copy from a friend.";
            } else if (ex.Message.Contains("Segou UI")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted copy of the 'Segou UI' font. "
                       + "Try reinstalling .NET Framework 3.5SP1 (http://www.microsoft.com/Net/Download.aspx), "
                       + "or getting a copy from a friend.";
            } else if (ex.Message.Contains("Configuration system failed")) {
                SugFix = "Rawr encountered a serious error caused by a corrupted user settings file. Please "
                       + "try clearing your local settings by closing Rawr, and then deleting any folders "
                       + "whose name starts with 'Rawr' at C:\\Documents and Settings\\[User]\\AppData\\Local\\ "
                       + "or C:\\Users\\[User]\\AppData\\Local\\. You will need to display hidden folders to "
                       + "find those folders.";
            }
            //
            return string.Format(errorFormat,
                                (SugFix == "" ? "Rawr encountered a serious error.\r\n\r\n" : SugFix + SugFix2),
                                ex.Message,
                                ex.StackTrace);
        }
    }
}

//cold ground below...
//dissatisfied clouds above...