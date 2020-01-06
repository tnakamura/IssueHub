using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IssueHub.Utils
{
    public static class CollectionEx
    {
        public static void Merge<T>(
            this ObservableCollection<T> @this,
            IEnumerable<T> list)
        {
            Merge(@this, list, EqualityComparer<T>.Default);
        }

        public static void Merge<T>(
            this ObservableCollection<T> @this,
            IEnumerable<T> list,
            IEqualityComparer<T> comparer)
        {
            if (@this.Count == 0)
            {
                foreach (var item in list)
                {
                    @this.Add(item);
                }
            }
            else
            {
                var i = 0;
                foreach (var item in list)
                {
                    if (i < @this.Count)
                    {
                        if (!comparer.Equals(@this[i], item))
                        {
                            @this[i] = item;
                        }
                    }
                    else
                    {
                        @this.Add(item);
                    }
                    i++;
                }
                if (i < @this.Count)
                {
                    for (var j = @this.Count - 1; j >= i; j--)
                    {
                        @this.RemoveAt(j);
                    }
                }
            }
        }
    }
}
