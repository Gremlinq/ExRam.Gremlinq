using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestBase : VerifyBase
    {
        private static readonly AsyncLocal<GremlinqTestBase> CurrentTestBase = new();

        protected GremlinqTestBase(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(CreateSettings(), sourceFile)
        {
            CurrentTestBase.Value = this;
            XunitContext.Register(testOutputHelper, sourceFile);
        }

        public virtual async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var serialized = JsonConvert.SerializeObject(
                await query
                    .ToArrayAsync(),
                Formatting.Indented);

            var scrubbed = this
                .Scrubbers()
                .Aggregate(serialized, (s, func) => func(s));

            await Verify(scrubbed);
        }

        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;

        private static VerifySettings CreateSettings()
        {
            var settings = new VerifySettings();

            settings.DisableDiff();

#if DEBUG
            settings.AutoVerify();
#endif

            return settings;
        }

        public static GremlinqTestBase Current { get => CurrentTestBase.Value ?? throw new InvalidOperationException(); }
    }
}
