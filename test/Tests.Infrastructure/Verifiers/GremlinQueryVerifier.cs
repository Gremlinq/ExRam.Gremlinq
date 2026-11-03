using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GremlinQueryVerifier
    {
        // Applies further settings to what another verifier verifies. A test class composes its verifier
        // through WithSettings and WithSnapshotsOf instead of deriving a verifier of its own.
        private sealed class ConfiguredVerifier : GremlinQueryVerifier
        {
            private readonly GremlinQueryVerifier _inner;
            private readonly Func<SettingsTask, SettingsTask> _settings;

            public ConfiguredVerifier(GremlinQueryVerifier inner, Func<SettingsTask, SettingsTask> settings)
            {
                _inner = inner;
                _settings = settings;
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => _settings(_inner.Verify(query));
        }

        private readonly string _sourceFile;

        protected GremlinQueryVerifier([CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;
        }

        // A SettingsTask instead of a Task, so that a test can configure its verification further,
        // after ModifySettingsTask: the facts of QueryExecutionTestsBase return it, so a test class that
        // overrides a fact can chain a setting onto base.Fact().
        public abstract SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query);

        /// <summary>
        /// This verifier with <paramref name="settings"/> applied to every verification, after its own settings.
        /// </summary>
        public GremlinQueryVerifier WithSettings(Func<SettingsTask, SettingsTask> settings) => new ConfiguredVerifier(this, settings);

        /// <summary>
        /// This verifier verifying against the snapshots of <typeparamref name="T"/>: for a test class that answers like
        /// <typeparamref name="T"/>, in another environment.
        /// </summary>
        public GremlinQueryVerifier WithSnapshotsOf<T>() where T : ISourceFileNameProvider<T> => WithSettings(static task => task.UseSnapshotDirectoryAndNameOf<T>());

        protected virtual SettingsTask ModifySettingsTask(SettingsTask task) => task/*.AutoVerify()*/;

        protected SettingsTask InnerVerify<T>(T value) => ModifySettingsTask(Verifier
            .Verify(value, sourceFile: _sourceFile));

        protected SettingsTask InnerVerify<T>(Task<T> value) => ModifySettingsTask(Verifier
            .Verify(value, sourceFile: _sourceFile));
    }
}
