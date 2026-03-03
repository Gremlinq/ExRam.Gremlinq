namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Transforms a <see cref="IGremlinQuerySource"/> into a configured query source.
    /// </summary>
    public interface IGremlinQuerySourceTransformation
    {
        /// <summary>
        /// Transforms the specified query source.
        /// </summary>
        /// <param name="source">The query source to transform.</param>
        IGremlinQuerySource Transform(IGremlinQuerySource source);
    }
}
