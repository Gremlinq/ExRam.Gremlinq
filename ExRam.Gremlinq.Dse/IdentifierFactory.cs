using System.Threading;

namespace ExRam.Gremlinq.Dse
{
    public sealed class IdentifierFactory : IIdentifierFactory
    {
        private int _label;

        private IdentifierFactory()
        {
            
        }

        public StepLabel<TElement> CreateStepLabel<TElement>()
        {
            return new StepLabel<TElement>();
        }

        public string CreateIndexName()
        {
            return "i" + Interlocked.Increment(ref _label);
        }
        
        public static IIdentifierFactory CreateDefault()
        {
            return new IdentifierFactory();
        }
    }
}