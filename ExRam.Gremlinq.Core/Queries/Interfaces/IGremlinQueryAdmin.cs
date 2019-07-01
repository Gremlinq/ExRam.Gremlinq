using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin : IGremlinQueryEnvironment
    {
        IGremlinQuery InsertStep(int index, Step step);

        IImmutableList<Step> Steps { get; }
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQuery;
    }
}
