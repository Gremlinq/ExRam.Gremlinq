namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new();

        public IdentityStep() : base()
        {
        }
    }
}
