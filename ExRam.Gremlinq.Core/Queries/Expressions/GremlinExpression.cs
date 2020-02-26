using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal struct GremlinExpression
    {
        public GremlinExpression(Expression key, P predicate)
        {
            Key = key;
            Predicate = predicate;
        }

        public P Predicate { get; }
        public Expression Key { get; }
    }
}
