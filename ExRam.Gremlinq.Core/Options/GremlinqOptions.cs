using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public readonly struct GremlinqOptions
    {
        private readonly IImmutableDictionary<IGremlinqOption, object>? _dictionary;

        public GremlinqOptions(IImmutableDictionary<IGremlinqOption, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public TValue GetValue<TValue>(GremlinqOption<TValue> option)
        {
            return (_dictionary != null && _dictionary.TryGetValue(option, out var value))
                ? (TValue)value
                : option.DefaultValue;
        }

        public bool Contains(IGremlinqOption option)
        {
            return (_dictionary?.ContainsKey(option)).GetValueOrDefault();
        }

        public GremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration)
        {
            return new GremlinqOptions((_dictionary ?? ImmutableDictionary<IGremlinqOption, object>.Empty).SetItem(option, configuration(GetValue(option))!));
        }

        public GremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value)
        {
            return new GremlinqOptions((_dictionary ?? ImmutableDictionary<IGremlinqOption, object>.Empty).SetItem(option, value!));
        }

        public GremlinqOptions Remove(IGremlinqOption option)
        {
            return new GremlinqOptions(_dictionary?.Remove(option) ?? ImmutableDictionary<IGremlinqOption, object>.Empty);
        }
    }
}
