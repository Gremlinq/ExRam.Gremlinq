using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(object key) : base(key, default)
        {

        }

        public HasStep(object key, P predicate) : base(key, predicate)
        {
        }

        public HasStep(object key, IGremlinQueryBase traversal) : base(key, traversal)
        {
        }
    }
}
