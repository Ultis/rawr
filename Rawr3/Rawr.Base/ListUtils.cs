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

        public static int FindLastIndex<T>(this IList<T> source, Predicate<T> match)
        {
            return FindLastIndex(source, source.Count - 1, source.Count, match);
        }

        public static int FindLastIndex<T>(this IList<T> source, int startIndex, Predicate<T> match)
        {
            return FindLastIndex(source, startIndex, startIndex + 1, match);
        }

        public static int FindLastIndex<T>(this IList<T> source, int startIndex, int count, Predicate<T> match)
        {
            int num = startIndex - count;
            for (int i = startIndex; i > num; i--)
            {
                if (match(source[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static List<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> source, Converter<TInput, TOutput> converter)
        {
            return new List<TOutput>((from item in source select converter(item)));
        }

        public static bool Exists<T>(this IList<T> source, Predicate<T> match)
        {
            return FindIndex(source, match) != -1;
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
        {
            return FindIndex(source, 0, source.Count, match);
        }

        public static int FindIndex<T>(this IList<T> source, int startIndex, Predicate<T> match)
        {
            return FindIndex(source, startIndex, source.Count - startIndex, match);
		}

        public static int FindIndex<T>(this IList<T> source, int startIndex, int count, Predicate<T> match)
        {
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(source[i]))
                {
                    return i;
                }
            }
            return -1;
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
