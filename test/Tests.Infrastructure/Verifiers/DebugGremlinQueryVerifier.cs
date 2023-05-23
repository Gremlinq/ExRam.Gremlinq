using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DebugGremlinQueryVerifier : GremlinQueryVerifier
    {
        public DebugGremlinQueryVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {

        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
    }
}
