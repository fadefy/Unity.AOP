using System.Collections.Concurrent;

namespace Unity.AOP.Utilities
{
    public class DualKeyDictionary<K1, K2, T>
    {
        private ConcurrentDictionary<K1, ConcurrentDictionary<K2, T>> _topMap = new ConcurrentDictionary<K1, ConcurrentDictionary<K2, T>>();

        public bool TryAdd(K1 key1, K2 key2, T value)
        {
            return GetSubMap(key1).TryAdd(key2, value);
        }

        public T Get(K1 key1, K2 key2)
        {
            T value;
            if (GetSubMap(key1).TryGetValue(key2, out value))
                return value;

            return default(T);
        }

        public Optional<T> TryRemove(K1 key1, K2 key2)
        {
            T value;
            if (GetSubMap(key1).TryRemove(key2, out value))
                return new Optional<T>(value);
            return new Optional<T>();
        }

        public ConcurrentDictionary<K2, T> GetSubMap(K1 key1)
        {
            return _topMap.GetOrAdd(key1, _ => new ConcurrentDictionary<K2, T>());
        }
    }
}
