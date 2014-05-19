using System.Collections.Generic;
using System.Linq;

namespace Unity.AOP.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> source, IEnumerable<int> excludeIndices)
        {
            var indexes = new Queue<int>(excludeIndices.OrderBy(i => i));
            foreach (var itemWithIndex in source.Select((item, index) => new { Item = item, Index = index }))
            {
                if (indexes.Count > 0 && indexes.Peek() == itemWithIndex.Index)
                    indexes.Dequeue();
                else
                    yield return itemWithIndex.Item;
            }
        }
    }
}
