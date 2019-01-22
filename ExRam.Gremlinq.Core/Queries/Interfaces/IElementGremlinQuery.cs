using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public partial interface IElementGremlinQuery : IGremlinQuery
    {
        IValueGremlinQuery<object> Id();

        IValueGremlinQuery<string> Label();

        IValueGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
        IValueGremlinQuery<object> Values(params string[] keys);

        IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params string[] keys);
        IValueGremlinQuery<IDictionary<string, object>> ValueMap(params string[] keys);
    }

    public partial interface IElementGremlinQuery<TElement> : IElementGremlinQuery, IGremlinQuery<TElement>
    {
    }
}
