using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class SerializingVerifier<TSerialized> : GremlinQueryVerifier
    {
        public SerializingVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return this
                .InnerVerify(env
                    .Serializer
                    .TransformTo<TSerialized>()
                    .From(query, env))
                .DontScrubGuids()
                .ScrubGuids();
        }
    }
}
