using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;

namespace Rawr
{
    public static class ListUtils
    {

        public static T Find<T>(this IEnumerable<T> source, Predicate<T> match)
        {
            return source.FirstOrDefault(t => match(t));
        }

        public static int FindLastIndex<T>(this IEnumerable<T> source, Predicate<T> match)
        {
            var v = source
            .Select((item, index) => new { item = item, position = index })
            .Last(x => match(x.item));
            return v.position;
        }

        public static int FindLastIndex<T>(this IEnumerable<T> source, int startIndex, Predicate<T> match)
        {
            var v = source
            .Where((obj, index) => index >= startIndex)
            .Select((item, index) => new { item = item, position = index })
            .Last(x => match(x.item));
            return v.position;
        }

        public static int FindLastIndex<T>(this IEnumerable<T> source, int startIndex, int count, Predicate<T> match)
        {
            var v = source
            .Where((obj, index) => (index >= startIndex) && (index <= count))
            .Select((item, index) => new { item = item, position = index })
            .Last(x => match(x.item));
            return v.position;
        }

        public static List<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> source, Converter<TInput, TOutput> converter)
        {
            return new List<TOutput>((from item in source select converter(item)));
        }


        public static int FindIndex<T>(this IEnumerable<T> source, Predicate<T> match)
        {
            var v = source
            .Select((item, index) => new { Item = item, Position = index })
            .Where(x => match(x.Item))
			.FirstOrDefault();
			return v == null ? -1 : v.Position;
        }

        public static int FindIndex<T>(this IEnumerable<T> source, int startIndex, Predicate<T> match)
        {
            var v = source
            .Select((item, index) => new { Item = item, Position = index })
            .Where(x => (x.Position >= startIndex) && match(x.Item))
			.FirstOrDefault();
			return v == null ? -1 : v.Position;
		}

        public static int FindIndex<T>(this IEnumerable<T> source, int startIndex, int count, Predicate<T> match)
        {
            var v = source
            .Select((item, index) => new { Item = item, Position = index })
			.Where(x => (x.Position >= startIndex) && (x.Position <= startIndex + count) && match(x.Item))
			.FirstOrDefault();
			return v == null ? -1 : v.Position;
        }

        public static int RemoveAll<T>(this List<T> list, Predicate<T> match)
        {
            int index = 0;
            while ((index < list.Count) && !match(list[index]))
            {
                index++;
            }
            if (index >= list.Count)
            {
                return 0;
            }
            int num2 = index + 1;
            while (num2 < list.Count)
            {
                while ((num2 < list.Count) && match(list[num2]))
                {
                    num2++;
                }
                if (num2 < list.Count)
                {
                    list[index++] = list[num2++];
                }
            }
            int num3 = list.Count - index;
            list.RemoveRange(index, list.Count - index);
            return num3;
        }

        public static List<T> FindAll<T>(this List<T> list, Predicate<T> match)
        {
           return new List<T>(list.Where(t => match(t)));
        }
    }
}
