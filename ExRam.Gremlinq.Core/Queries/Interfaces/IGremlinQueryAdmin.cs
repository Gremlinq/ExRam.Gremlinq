using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        IGremlinQuery InsertSteps(int index, Step[] steps);
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQuery;

        IImmutableList<Step> Steps { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
