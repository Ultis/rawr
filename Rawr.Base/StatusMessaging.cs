using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	public class StatusEventArgs : EventArgs
	{
		private string _key;
		private string _description;
		private bool _done;

		public StatusEventArgs(string key, string description)
		{
			_key = key;
			_description = description;
			_done = false;
		}

		public StatusEventArgs(string key, string description, bool done)
		{
			_key = key;
			_description = description;
			_done = done;
		}

		public string Key
		{
			get { return _key; }
		}

		public string Description
		{
			get { return _description; }
		}

		public bool Done
		{
			get { return _done; }
		}
	}

	public static class StatusMessaging
	{
		public delegate void StatusUpdateDelegate(StatusEventArgs args);
		public static event StatusUpdateDelegate StatusUpdate;

		public static void UpdateStatus(string key, string description)
		{
			StatusUpdateDelegate del = StatusMessaging.StatusUpdate;
			if (del != null)
			{
				del(new StatusEventArgs(key, description));
			}
		}

		public static void UpdateStatusFinished(string key)
		{
			StatusUpdateDelegate del = StatusMessaging.StatusUpdate;
			if (del != null)
			{
				del(new StatusEventArgs(key, "Done!",true));
			}
		}
	}
}
