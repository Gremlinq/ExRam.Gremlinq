using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        IGremlinQuery InsertStep(int index, Step step);

        IGraphModel Model { get; }
        IImmutableList<Step> Steps { get; }
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQuery;
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }
}
