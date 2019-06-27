using System;

namespace ExRam.Gremlinq.Core
{
    public interface IStage1ChooseBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQuery
    {
        IStage2ChooseBuilder<TSourceQuery, TElement> On<TElement>(Func<TSourceQuery, IGremlinQuery<TElement>> chooseTraversal);
    }
}
