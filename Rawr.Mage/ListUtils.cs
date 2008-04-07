using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
	public class ListUtils
	{
		public static List<T> Intersect<T>(List<T> left, List<T> right)
		{
			List<T> intersect = new List<T>();
			foreach (T item in left)
				if (right.Contains(item))
					intersect.Add(item);
			return intersect;
		}
	}
}
