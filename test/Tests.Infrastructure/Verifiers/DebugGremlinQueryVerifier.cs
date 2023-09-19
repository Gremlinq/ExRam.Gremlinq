using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public sealed class DebugGremlinQueryVerifier : GremlinQueryVerifier
    {
        public DebugGremlinQueryVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {

        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.Debug());
    }
}
