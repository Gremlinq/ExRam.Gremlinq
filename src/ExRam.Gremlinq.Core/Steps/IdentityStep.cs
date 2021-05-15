namespace ExRam.Gremlinq.Core
{
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new();

        public IdentityStep() : base()
        {
        }
    }
}
