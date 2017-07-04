using System.Reactive;

namespace ExRam.Gremlinq
{
    public interface IHasTraversalSource
    {
        IGremlinQuery<Unit> TraversalSource { get; }
    }
}