using System;

namespace System.Collections.Concurrent
{
    internal static class ConcurrentDictionaryExtensions
    {
#if NETSTANDARD2_0
        public static TValue GetOrAdd<TKey, TValue, TArg>(this ConcurrentDictionary<TKey,TValue> dict, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
        {
            return dict.GetOrAdd(key, closureKey => valueFactory(closureKey, factoryArgument));
        }
#endif
    }
}
