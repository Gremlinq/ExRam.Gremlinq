using System;

namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQuery
    {
        IChooseBuilderWithCondition<TSourceQuery, TElement> On<TElement>(Func<TSourceQuery, IGremlinQuery<TElement>> chooseTraversal);
    }
}
