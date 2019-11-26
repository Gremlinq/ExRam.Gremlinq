using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(object key, P predicate) : base(key, predicate)
        {
        }

        public HasStep(object key, IGremlinQuery traversal) : base(key, traversal)
        {
        }
    }
}
