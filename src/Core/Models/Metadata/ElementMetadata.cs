using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Metadata associated with a graph element type, such as its label.
    /// </summary>
    public readonly struct ElementMetadata : IEquatable<ElementMetadata>
    {
        private readonly string _label;

        /// <summary>
        /// Initializes a new instance of <see cref="ElementMetadata"/> with the specified label.
        /// </summary>
        /// <param name="label">The label for the graph element.</param>
        public ElementMetadata(string label)
        {
            ArgumentNullException.ThrowIfNull(label);

            _label = label;
        }

        /// <summary>
        /// Gets the label of the graph element.
        /// </summary>
        public string Label => _label ?? throw UninitializedStruct();

        public override bool Equals(object? obj) => obj is ElementMetadata metadata && Equals(metadata);

        public bool Equals(ElementMetadata other) => _label == other._label;

        public override int GetHashCode() => HashCode.Combine(Label);

        public static bool operator ==(ElementMetadata left, ElementMetadata right) => left.Equals(right);

        public static bool operator !=(ElementMetadata left, ElementMetadata right) => !(left == right);
    }
}
