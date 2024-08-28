namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class NoneStep : Step, IFilterStep
    {
        public static readonly NoneStep Instance = new();
    }
}
