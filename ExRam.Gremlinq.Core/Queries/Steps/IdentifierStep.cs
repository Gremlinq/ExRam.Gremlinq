namespace ExRam.Gremlinq.Core
{
    public sealed class IdentifierStep : Step
    {
        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public static readonly IdentifierStep g = new IdentifierStep("g");
#pragma warning restore IDE1006 // Naming Styles
        public static readonly IdentifierStep __ = new IdentifierStep("__");

        public IdentifierStep(string identifier)
        {
            Identifier = identifier;
        }

        public static IdentifierStep Create(string name)
        {
            return name == "g"
                ? g
                : name == "__"
                    ? __
                    : new IdentifierStep(name);
        }

        public string Identifier { get; }
    }
}
