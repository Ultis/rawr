using System;
using System.Threading;
using System.Windows.Forms;

namespace Rawr
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                bool bAppFirstInstance;
                Mutex oMutex = new Mutex(true, "Global\\Rawr", out bAppFirstInstance);
                if (bAppFirstInstance)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Rawr encountered a serious error. Please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" +
                    ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
            Log.Close();
        }
    }
}

//cold ground below...
//dissatisfied clouds above...