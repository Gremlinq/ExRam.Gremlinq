namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class NoneStep : Step, IIsOptimizableInWhere
    {
        public static readonly NoneStep Instance = new();
    }
}
