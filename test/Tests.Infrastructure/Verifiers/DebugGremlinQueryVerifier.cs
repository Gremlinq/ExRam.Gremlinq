using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class DebugGremlinQueryVerifier : GremlinQueryVerifier
    {
        public DebugGremlinQueryVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {

        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
    }
}
