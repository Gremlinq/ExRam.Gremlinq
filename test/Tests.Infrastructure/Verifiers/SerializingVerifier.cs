using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Tests.Verifiers
{
    public sealed class SerializingVerifier<TSerialized> : GremlinQueryVerifier
    {
        public SerializingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return InnerVerify(env
                .Serializer
                .TransformTo<TSerialized>()
                .From(query, env));
        }
    }
}
