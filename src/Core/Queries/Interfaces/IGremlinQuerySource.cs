namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IStartGremlinQuery
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);

        IGremlinQuerySource WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value);

        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : IGremlinQueryBase;
    }
}
