using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rawr.Server
{
	public static class Extensions
	{
		public static string EverythingAfter(this string a, string b, bool trim = true)
		{
			if (!a.Contains(b)) return a;
			return trim ? a.Substring(a.IndexOf(b) + b.Length).Trim() : a.Substring(a.IndexOf(b) + b.Length);
		}

		public static string EverythingBefore(this string a, string b, bool trim = true)
		{
			if (!a.Contains(b)) return a;
			return trim ? a.Substring(0, a.IndexOf(b)).Trim() : a.Substring(0, a.IndexOf(b));
		}

		public static string EverythingBetween(this string a, string b, string c, bool trim = true)
		{
			return a.EverythingAfter(b, trim).EverythingBefore(c, trim);
		}
	}
}