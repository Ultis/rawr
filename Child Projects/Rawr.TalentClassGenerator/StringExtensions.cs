using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.TalentClassGenerator
{
	public static class StringExtensions
	{
		public static string After(this string a, string b)
		{
			return a.Substring(a.IndexOf(b) + b.Length);
		}

		public static string Before(this string a, string b)
		{
			return a.Substring(0, a.IndexOf(b));
		}

		public static string Between(this string a, string b, string c)
		{
			int start = a.IndexOf(b) + b.Length;
			return a.Substring(start, a.IndexOf(c,start) - start);
		}
	}
}
