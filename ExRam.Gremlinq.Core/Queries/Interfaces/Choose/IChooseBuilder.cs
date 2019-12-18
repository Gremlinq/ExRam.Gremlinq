using System;

namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        IChooseBuilderWithCondition<TSourceQuery, TElement> On<TElement>(Func<TSourceQuery, IGremlinQueryBase<TElement>> chooseTraversal);
    }
}
