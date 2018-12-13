using System;
using System.Collections.Concurrent;
using System.Linq;

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
                    tuple => tuple.model.GetDerivedTypes(tuple.type, true)
                        .Select(closureType => tuple.model.TryGetLabelOfType(closureType)
                            .IfNone(() => throw new InvalidOperationException()))
                        .OrderBy(x => x)
                        .ToArray());
        }

        public string[] Labels { get; }
    }
}
