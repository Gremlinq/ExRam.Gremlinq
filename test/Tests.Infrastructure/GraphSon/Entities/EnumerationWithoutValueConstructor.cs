using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // An enumeration shaped like Gremlin.Net's, except for the private constructor taking the value:
    // it has none. GetByValue knows "known", answers "none" with null, and throws for anything else.
    public sealed class EnumerationWithoutValueConstructor : EnumWrapper
    {
        private EnumerationWithoutValueConstructor() : base(nameof(EnumerationWithoutValueConstructor), "known")
        {
        }

        public static EnumerationWithoutValueConstructor Known { get; } = new();

        public static EnumerationWithoutValueConstructor? GetByValue(string value) => value switch
        {
            "known" => Known,
            "none" => null,
            _ => throw new ArgumentException($"No matching {nameof(EnumerationWithoutValueConstructor)} for value '{value}'.")
        };
    }
}
