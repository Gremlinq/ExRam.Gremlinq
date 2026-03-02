namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Represents a traversal path, consisting of labels and the objects encountered at each step.
    /// </summary>
    public sealed class Path
    {
        /// <summary>
        /// Gets or sets the labels at each step of the path.
        /// </summary>
        public string[][] Labels { get; set; } = [];

        /// <summary>
        /// Gets or sets the objects encountered at each step of the path.
        /// </summary>
        public object[] Objects { get; set; } = [];
    }
}
