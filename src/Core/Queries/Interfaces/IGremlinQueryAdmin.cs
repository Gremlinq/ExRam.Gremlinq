using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> configurator, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;
        TTargetQuery AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? projectionTransformation = null) where TTargetQuery : IStartGremlinQuery;

        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IStartGremlinQuery;

        IGremlinQuerySource GetSource();

        Traversal Steps { get; }
        IGremlinQueryEnvironment Environment { get; }
        IImmutableDictionary<object, object?> Metadata { get; }
    }
}
