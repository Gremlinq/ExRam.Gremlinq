using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        private static readonly ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string>> Cache = new ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string>>();

        protected AddElementStep(IGraphElementModel elementModel, object value)
        {
            Label = Cache
                .GetOrCreateValue(elementModel)
                .GetOrAdd(
                    value.GetType(),
                    (closureType, closureModel) => closureType
                        .GetTypeHierarchy()
                        .Where(type => !type.IsAbstract)
                        .Select(type => closureModel.Metadata.TryGetValue(type, out var metadata)
                            ? metadata.Label
                            : null)
                        .FirstOrDefault() ?? closureType.Name,
                    elementModel);
        }

        public string Label { get; }
    }
}
