using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class GremlinQueryVerifier
    {
        private readonly string _sourceFile;

        protected GremlinQueryVerifier([CallerFilePath] string sourceFile = "")
        {
            _sourceFile = sourceFile;
        }

        public abstract Task Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected virtual SettingsTask ModifySettingsTask(SettingsTask task) => task;

        protected virtual Task InnerVerify<T>(T value) => InnerVerify(new ValueTask<T>(value));

        protected virtual Task InnerVerify<T>(ValueTask<T> value) => ModifySettingsTask(Verifier
            .Verify(value, sourceFile: _sourceFile));
    }
}
