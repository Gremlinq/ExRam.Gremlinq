using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GremlinQueryVerifier
    {
        private readonly string _sourceFile;
        private readonly Func<SettingsTask, SettingsTask> _settingsTaskModifier;

        protected GremlinQueryVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier, [CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;
            _settingsTaskModifier = settingsTaskModifier ?? new Func<SettingsTask, SettingsTask>(__ => __);
        }

        public abstract SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected SettingsTask InnerVerify<T>(T value) => InnerVerify(new ValueTask<T>(value));

        protected SettingsTask InnerVerify<T>(Task<T> task) => InnerVerify(new ValueTask<T>(task));

        protected virtual SettingsTask InnerVerify<T>(ValueTask<T> value) => _settingsTaskModifier
            .Invoke(Verifier
                .Verify(value, sourceFile: _sourceFile).AutoVerify());
    }
}
