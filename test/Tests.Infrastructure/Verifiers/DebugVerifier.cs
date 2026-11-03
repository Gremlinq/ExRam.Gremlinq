using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class DebugVerifier : GremlinQueryVerifier
    {
        public DebugVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {

        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
    }
}
