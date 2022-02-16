using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IConfigurableGremlinQuerySource, IStartGremlinQuery
    {
        IGremlinQuerySource WithoutStrategies(params Type[] strategyTypes);
        IGremlinQuerySource WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value);
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : IGremlinQueryBase;

        IGremlinQueryEnvironment Environment { get; }
    }

    public interface IConfigurableGremlinQuerySource
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
    }
}
