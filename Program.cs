using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr
{
    static class Program
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
#endif //DEBUG
				bool bAppFirstInstance;
				System.Threading.Mutex oMutex = new System.Threading.Mutex(true, "Global\\Rawr", out bAppFirstInstance);
				if (bAppFirstInstance)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new FormMain());
				}
                
#if !DEBUG 
			}
			catch (Exception ex)
			{
				MessageBox.Show("Rawr encountered a serious error. Please copy and paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace);
			}
			Log.Close();
#endif //DEBUG
        } 
    }
}

//cold ground below...
//dissatisfied clouds above...