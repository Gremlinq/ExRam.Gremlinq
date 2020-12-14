using System;

namespace ExRam.Gremlinq.Core
{
    public interface IFeatureSet
    {
        IFeatureSet ConfigureGraphFeatures(Func<GraphFeatures, GraphFeatures> config);
        IFeatureSet ConfigureVariableFeatures(Func<VariableFeatures, VariableFeatures> config);
        IFeatureSet ConfigureVertexFeatures(Func<VertexFeatures, VertexFeatures> config);
        IFeatureSet ConfigureVertexPropertyFeatures(Func<VertexPropertyFeatures, VertexPropertyFeatures> config);
        IFeatureSet ConfigureEdgeFeatures(Func<EdgeFeatures, EdgeFeatures> config);
        IFeatureSet ConfigureEdgePropertyFeatures(Func<EdgePropertyFeatures, EdgePropertyFeatures> config);

        GraphFeatures GraphFeatures { get; }
        EdgePropertyFeatures EdgePropertyFeatures { get; }
        EdgeFeatures EdgeFeatures { get; }
        VariableFeatures VariableFeatures { get; }
        VertexFeatures VertexFeatures { get; }
        VertexPropertyFeatures VertexPropertyFeatures { get; }
    }
}