using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IConfigurableGremlinQuerySource, IStartGremlinQuery
    {
        IEdgeGremlinQuery<object> E(params object[] ids);
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);

        IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);

        IGremlinQuerySource WithoutStrategies(params Type[] strategyTypes);

        IGremlinQueryEnvironment Environment { get; }
    }

    public interface IConfigurableGremlinQuerySource
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
    }
}
