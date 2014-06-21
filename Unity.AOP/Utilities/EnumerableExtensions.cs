using System.Collections.Generic;
using System.Linq;

namespace Unity.AOP.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ExcludeSimple<T>(this IList<T> source, IEnumerable<int> excludeIndices)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (!excludeIndices.Contains(i))
                {
                    yield return source[i];
                }
            }
        }

        public static IEnumerable<T> Exclude<T>(this IList<T> source, IEnumerable<int> excludeIndices)
        {
            var result = new List<T>();
            var indexes = new Queue<int>(excludeIndices.OrderBy(i => i));
            for (int i = 0; i < source.Count(); i++)
            {
                if (indexes.Count > 0 && indexes.Peek() == i)
                    indexes.Dequeue();
                else
                    result.Add(source[i]);
            }
            return result;
        }

        public static IEnumerable<T> ExceptIndices<T>(this IList<T> source, IEnumerable<int> excludeIndices)
        {
            return Enumerable.Range(0, source.Count).Except(excludeIndices).Select(i => source[i]);
        }
    }
}
