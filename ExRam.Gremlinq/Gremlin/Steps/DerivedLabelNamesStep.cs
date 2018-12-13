using System;
using System.Collections.Concurrent;

namespace ExRam.Gremlinq
{
    public abstract class DerivedLabelNamesStep : Step
    {
        private static readonly ConcurrentDictionary<(IGraphModel model, Type type), string[]> TypeLabelDict = new ConcurrentDictionary<(IGraphModel, Type), string[]>();

        protected DerivedLabelNamesStep(IGraphModel model, Type type)
        {
            Labels = TypeLabelDict
                .GetOrAdd(
                    (model, type),
                    tuple => tuple.model.TryGetDerivedLabels(tuple.type));
        }

        public string[] Labels { get; }
    }
}
