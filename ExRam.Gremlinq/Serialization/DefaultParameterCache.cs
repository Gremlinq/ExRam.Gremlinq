using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ExRam.Gremlinq
{
    public sealed class DefaultParameterCache : IParameterCache
    {
        private int _current;
        private readonly ConcurrentDictionary<object, string> _cache = new ConcurrentDictionary<object, string>();

        public string Cache(object parameter)
        {
            return this._cache.GetOrAdd(parameter, _ => "_P" + Interlocked.Increment(ref this._current));
        }

        public IDictionary<string, object> GetDictionary()
        {
            return this._cache
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }
    }
}