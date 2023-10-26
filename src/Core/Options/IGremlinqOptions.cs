namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqOptions
    {
        TValue GetValue<TValue>(GremlinqOption<TValue> option);
        bool Contains<TValue>(GremlinqOption<TValue> option);
        IGremlinqOptions ConfigureValue<TValue>(GremlinqOption<TValue> option, Func<TValue, TValue> configuration);
        IGremlinqOptions SetValue<TValue>(GremlinqOption<TValue> option, TValue value);
        IGremlinqOptions Remove<TValue>(GremlinqOption<TValue> option);
    }
}
