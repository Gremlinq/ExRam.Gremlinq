using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqOptions
    {
        TValue GetValue<TValue>(GremlinqOption<TValue> option);
        bool Contains(IGremlinqOption option);
        IGremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration);
        IGremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value);
        IGremlinqOptions Remove(IGremlinqOption option);
    }
}