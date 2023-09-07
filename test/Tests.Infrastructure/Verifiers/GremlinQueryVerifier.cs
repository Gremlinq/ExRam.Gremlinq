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

        protected SettingsTask InnerVerify<T>(T value) => InnerVerify(ValueTask.FromResult(value));

        protected SettingsTask InnerVerify<T>(Task<T> task) => InnerVerify(new ValueTask<T>(task));

        protected virtual SettingsTask InnerVerify<T>(ValueTask<T> value) => Verifier
            .Verify(value, sourceFile: _sourceFile);
    }
}
