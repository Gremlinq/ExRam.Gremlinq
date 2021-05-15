namespace ExRam.Gremlinq.Core
{
    public static class TraversalTranslator
    {
        private sealed class DefaultTraversalTranslator : ITraversalTranslator
        {
            public Traversal Translate(IGremlinQueryBase query)
            {
                return query.AsAdmin()
                    .Steps
                    .ToTraversal(query.AsAdmin().Projection);

                //return traversal.IncludeProjection();

                throw new System.NotImplementedException();
                //var projectionIndex = 0;
                //var maybeProjectionStep = default(Step?);
                //var projectionSemantics = steps.InitialSemantics;

                //if ((queryFlags & QueryFlags.SurfaceVisible) == QueryFlags.SurfaceVisible)
                //{
                //    var index = steps.Count;

                //    for (var i = steps.Count - 1; i >= 0; i--)
                //    {
                //        if (steps[i].Semantics is { } semantics)
                //        {
                //            if (!typeof(IArrayGremlinQueryBase).IsAssignableFrom(semantics.QueryType))
                //            {
                //                projectionSemantics = semantics;
                //                projectionIndex = index;

                //                break;
                //            }

                //            index = i;
                //        }
                //    }

                //    if (projectionSemantics.IsVertex)
                //        maybeProjectionStep = ProjectVertexStep.Instance;
                //    else if (projectionSemantics.IsEdge)
                //        maybeProjectionStep = ProjectEdgeStep.Instance;
                //}

                //var ret = new Step[steps.Count + ((maybeProjectionStep is not null) ? 1 : 0)];

                //if (maybeProjectionStep is { } projectionStep)
                //{
                //    steps.CopyTo(ret, 0, 0, projectionIndex);
                //    ret[projectionIndex] = projectionStep;
                //    steps.CopyTo(ret, projectionIndex, projectionIndex + 1, steps.Count - projectionIndex);
                //}
                //else
                //    steps.CopyTo(ret, 0, 0, steps.Count);

                //return new Traversal(ret, true, Projection.None);
            }
        }

        public static readonly ITraversalTranslator Default = new DefaultTraversalTranslator();
    }
}
