using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//i... hurt myself today
namespace Rawr
{
	//to see if i still feel
	public class Log
	{
		//i focus on the pain
		private static StreamWriter _log = null;
		//the only thing that's real
		private static StreamWriter RawrLog
		{
			get
			{
				//the needle tears a hole
				if (_log == null)
					_log = System.IO.File.CreateText("RawrLog.log");
				//the old familiar sting
				return _log;
			}
		}

		//try to kill it all away
		public static void Write(string log)
		{
			int ms = DateTime.Now.Millisecond;

			//but i remember... everything
			RawrLog.WriteLine("{0} {1}:{2}:{3}.{4}: {5}", DateTime.Now.ToShortDateString(), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, 
				(ms < 100 ? "0" : "") + (ms < 10 ? "0" : "") + ms.ToString(), log);
		}

		public static void Close()
		{
			if (_log != null)
			{
				_log.Flush();
				_log.Close();
			}
		}
	}
}
