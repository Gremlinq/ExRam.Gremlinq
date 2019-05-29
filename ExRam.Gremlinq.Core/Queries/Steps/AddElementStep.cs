using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using LanguageExt;

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
                    closureType => closureType
                        .GetTypeHierarchy()
                        .Where(type => !type.IsAbstract)
                        .SelectMany(type => elementModel.Metadata
                            .TryGetValue(type)
                            .Map(x => x.Label))
                        .HeadOrNone()
                        .IfNone(closureType.Name));
        }

        public string Label { get; }
    }
}
