using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace ExRam.Gremlinq
{
    public sealed class DefaultParameterCache : IParameterCache
    {
        private int _current;

        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;
        private readonly ConcurrentDictionary<object, string> _cache = new ConcurrentDictionary<object, string>();
       
        public DefaultParameterCache(IImmutableDictionary<StepLabel, string> stepLabelMappings)
        {
            this._stepLabelMappings = stepLabelMappings;
        }

        public string Cache(object parameter)
        {
            return this._cache.GetOrAdd(
                parameter, 
                _ =>
                {
                    if (_ is StepLabel stepLabel)
                    {
                        return this._stepLabelMappings.TryGetValue(stepLabel, out var mapping) 
                            ? this.Cache(mapping) 
                            : this.Cache("l" + Interlocked.Increment(ref this._current));
                    }

                    return "_P" + Interlocked.Increment(ref this._current);
                });
        }

        public IDictionary<string, object> GetDictionary()
        {
            return this._cache
                .Where(kvp => !(kvp.Key is StepLabel))
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }
    }
}