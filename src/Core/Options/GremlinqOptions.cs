using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinqOptions
    {
        private sealed class GremlinqOptionsImpl : IGremlinqOptions
        {
            private readonly IImmutableDictionary<object, object> _dictionary;

            public GremlinqOptionsImpl(IImmutableDictionary<object, object> dictionary)
            {
                _dictionary = dictionary;
            }

            public TValue GetValue<TValue>(GremlinqOption<TValue> option) => _dictionary
                .Fast()
                .TryGetValue(option, out var value)
                    ? (TValue)value
                    : option.DefaultValue;

            public bool Contains<TValue>(GremlinqOption<TValue> option) => _dictionary
                .Fast()
                .ContainsKey(option);

            public IGremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration) => new GremlinqOptionsImpl(_dictionary
                .SetItem(
                    option,
                    configuration(_dictionary.TryGetValue(option, out var value)
                        ? (TValue)value
                        : option.DefaultValue)!));

            public IGremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value) => new GremlinqOptionsImpl(_dictionary.SetItem(option, value!));

            public IGremlinqOptions Remove<TValue>(GremlinqOption<TValue> option) => new GremlinqOptionsImpl(_dictionary.Remove(option));
        }

        public static readonly IGremlinqOptions Empty = new GremlinqOptionsImpl(ImmutableDictionary<object, object>.Empty);
    }
}
