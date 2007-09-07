using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Rawr
{
	public class Log
	{
		private static StreamWriter _log = null;
		private static StreamWriter RawrLog
		{
			get
			{
				if (_log == null)
					_log = System.IO.File.CreateText("RawrLog.log");
				return _log;
			}
		}

		public static void Write(string log)
		{
			int ms = DateTime.Now.Millisecond;
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
