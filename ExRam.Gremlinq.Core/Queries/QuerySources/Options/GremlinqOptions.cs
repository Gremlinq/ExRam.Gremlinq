using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct GremlinqOptions
    {
        private readonly IImmutableDictionary<GremlinqOption, object> _options;

        public GremlinqOptions(IImmutableDictionary<GremlinqOption, object> options)
        {
            _options = options;
        }

        public TValue GetValue<TValue>(GremlinqOption<TValue> option)
        {
            return (_options?.TryGetValue(option))
                .ToOption()
                .Bind(x => x)
                .Map(optionValue => (TValue)optionValue)
                .IfNone(option.DefaultValue);
        }

        public GremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value)
        {
            return new GremlinqOptions(_options ?? ImmutableDictionary<GremlinqOption, object>.Empty.SetItem(option, value));
        }
    }
}
