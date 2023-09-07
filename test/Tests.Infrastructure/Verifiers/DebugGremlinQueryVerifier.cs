using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public sealed class DebugGremlinQueryVerifier : GremlinQueryVerifier
    {
        public DebugGremlinQueryVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {

        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
    }
}
