namespace ExRam.Gremlinq.Core.GraphElements
{
    /// <summary>
    /// Represents a path in a graph traversal, containing the sequence of objects and their labels encountered during traversal.
    /// </summary>
    public sealed class Path
    {
        /// <summary>
        /// Gets or sets the array of label sets for each step in the path.
        /// Each element is an array of labels that were active at that step.
        /// </summary>
        public string[][] Labels { get; set; } = [];
        
        /// <summary>
        /// Gets or sets the array of objects encountered at each step in the path.
        /// </summary>
        public object[] Objects { get; set; } = [];
    }
}
