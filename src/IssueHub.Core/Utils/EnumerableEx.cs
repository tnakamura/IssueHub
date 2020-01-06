using System.Collections.Generic;

namespace IssueHub
{
    public static class EnumerableEx
    {
        public static int IndexOf<T>(
            this IEnumerable<T> enumerable,
            T item)
        {
            return IndexOf(enumerable, item, EqualityComparer<T>.Default);
        }

        public static int IndexOf<T>(
            this IEnumerable<T> enumerable,
            T item,
            IEqualityComparer<T> equalityComparer)
        {
            var i = 0;
            foreach (var x in enumerable)
            {
                if (equalityComparer.Equals(x, item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}
