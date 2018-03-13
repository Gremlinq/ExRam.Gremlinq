using System.Threading;

namespace ExRam.Gremlinq
{
    public sealed class IdentifierFactory : IIdentifierFactory
    {
        private int _label;

        private IdentifierFactory()
        {
            
        }

        public StepLabel<TElement> CreateStepLabel<TElement>()
        {
            return new StepLabel<TElement>("l" + Interlocked.Increment(ref this._label));
        }

        public string CreateIndexName()
        {
            return "i" + Interlocked.Increment(ref this._label);
        }
        
        public static IIdentifierFactory CreateDefault()
        {
            return new IdentifierFactory();
        }
    }
}