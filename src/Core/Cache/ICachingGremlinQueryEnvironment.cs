namespace ExRam.Gremlinq.Core
{
    internal interface ICachingGremlinQueryEnvironment : IGremlinQueryEnvironment, IGremlinQueryEnvironmentCache
    {
        IGremlinQueryEnvironment InnerEnvironment { get; }
    }
}
