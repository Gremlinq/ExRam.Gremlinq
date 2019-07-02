using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin : IGremlinQueryEnvironment
    {
        IGremlinQuery InsertStep(int index, Step step);
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQuery;

        IImmutableList<Step> Steps { get; }
    }
}
