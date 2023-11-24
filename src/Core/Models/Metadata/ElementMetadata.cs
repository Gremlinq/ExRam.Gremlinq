using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Models
{
    public readonly struct ElementMetadata : IEquatable<ElementMetadata>
    {
        private readonly string _label;

        public ElementMetadata(string label)
        {
            _label = label;
        }

        public string Label { get => _label ?? throw UninitializedStruct(); }

        public override bool Equals(object? obj) => obj is ElementMetadata metadata && Equals(metadata);

        public bool Equals(ElementMetadata other) => _label == other._label;

        public override int GetHashCode() => HashCode.Combine(Label);

        public static bool operator ==(ElementMetadata left, ElementMetadata right) => left.Equals(right);

        public static bool operator !=(ElementMetadata left, ElementMetadata right) => !(left == right);
    }
}
