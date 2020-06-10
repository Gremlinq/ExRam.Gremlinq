using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinqOptions
    {
        private sealed class GremlinqOptionsImpl : IGremlinqOptions
        {
            private readonly IImmutableDictionary<IGremlinqOption, object> _dictionary;

            public GremlinqOptionsImpl(IImmutableDictionary<IGremlinqOption, object> dictionary)
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

            public IGremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration)
            {
                return new GremlinqOptionsImpl(_dictionary.SetItem(option, configuration(GetValue(option))!));
            }

            public IGremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value)
            {
                return new GremlinqOptionsImpl(_dictionary.SetItem(option, value!));
            }

            public IGremlinqOptions Remove(IGremlinqOption option)
            {
                return new GremlinqOptionsImpl(_dictionary.Remove(option));
            }
        }

        public static readonly IGremlinqOptions Empty = new GremlinqOptionsImpl(ImmutableDictionary<IGremlinqOption, object>.Empty);
    }
}
