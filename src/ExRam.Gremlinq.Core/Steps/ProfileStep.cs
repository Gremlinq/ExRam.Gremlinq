namespace ExRam.Gremlinq.Core
{
    public sealed class ProfileStep : Step
    {
        public static readonly ProfileStep Instance = new();

        public ProfileStep() : base()
        {
        }
    }
}
