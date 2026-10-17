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

        // A SettingsTask instead of a Task, so that a test can configure its verification further,
        // after ModifySettingsTask: the facts of QueryExecutionTest return it, so a test class that
        // overrides a fact can chain a setting onto base.Fact().
        public abstract SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query);

        protected virtual SettingsTask ModifySettingsTask(SettingsTask task) => task/*.AutoVerify()*/;

        protected SettingsTask InnerVerify<T>(T value) => ModifySettingsTask(Verifier
            .Verify(value, sourceFile: _sourceFile));

        protected SettingsTask InnerVerify<T>(Task<T> value) => ModifySettingsTask(Verifier
            .Verify(value, sourceFile: _sourceFile));
    }
}
