namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class EdgeProjection : EdgeOrVertexProjection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => environment.Options.GetValue(GremlinqOption.EdgeProjectionSteps);

        public override Projection BaseProjection => EdgeOrVertex;
    }
}
