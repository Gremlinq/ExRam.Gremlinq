namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IStartGremlinQuery
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);

        IGremlinQuerySource WithStrategy<TStrategy>(Func<IGremlinQuerySource, TStrategy> factory)
            where TStrategy : IGremlinQueryStrategy;

        IGremlinQuerySource WithStrategy<TStrategy>(TStrategy strategy)
            where TStrategy : IGremlinQueryStrategy;

        IGremlinQuerySource WithoutStrategies(params Type[] strategyTypes);

        IGremlinQuerySource WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value);
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : IGremlinQueryBase;

        IGremlinQueryEnvironment Environment { get; }
    }
}
