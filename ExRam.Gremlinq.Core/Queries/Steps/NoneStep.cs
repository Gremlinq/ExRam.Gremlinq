namespace ExRam.Gremlinq.Core
{
    public sealed class NoneStep : Step, IIsOptimizableInWhere
    {
        public static readonly NoneStep Instance = new NoneStep();
    }
}
