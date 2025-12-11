namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a transformation that can be applied to a Gremlin query source.
    /// </summary>
    public interface IGremlinQuerySourceTransformation
    {
        /// <summary>
        /// Transforms the specified query source.
        /// </summary>
        /// <param name="source">The query source to transform.</param>
        /// <returns>The transformed query source.</returns>
        IGremlinQuerySource Transform(IGremlinQuerySource source);
    }
}
