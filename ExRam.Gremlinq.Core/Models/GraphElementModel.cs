using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Extensions;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModel
    {
        private sealed class EmptyGraphElementModel : IGraphElementModel
        {
            public IImmutableDictionary<Type, ElementMetadata> Metadata => ImmutableDictionary<Type, ElementMetadata>.Empty;
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphElementModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<Type, ElementMetadata> Metadata => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(Metadata)));
        }

        private sealed class CamelcaseGraphElementModel : IGraphElementModel
        {
            public CamelcaseGraphElementModel(IGraphElementModel baseModel)
            {
                Metadata = baseModel.Metadata
                    .ToImmutableDictionary(
                        kvp => kvp.Key,
                        kvp => new ElementMetadata(kvp.Value.LabelOverride.IfNone(kvp.Key.Name).ToCamelCase()));
            }

            public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
        }

        private sealed class LowercaseGraphElementModel : IGraphElementModel
        {
            public LowercaseGraphElementModel(IGraphElementModel baseModel)
            {
                Metadata = baseModel.Metadata
                    .ToImmutableDictionary(
                        kvp => kvp.Key,
                        kvp => new ElementMetadata(kvp.Value.LabelOverride.IfNone(kvp.Key.Name).ToLower()));
            }

            public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
        }

        public static readonly IGraphElementModel Empty = new EmptyGraphElementModel();
        public static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        private static readonly ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>> DerivedLabels = new ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>>();

        public static IGraphElementModel WithCamelCaseLabels(this IGraphElementModel model)
        {
            return new CamelcaseGraphElementModel(model);
        }

        public static IGraphElementModel WithLowerCaseLabels(this IGraphElementModel model)
        {
            return new LowercaseGraphElementModel(model);
        }

        public static Option<string[]> TryGetFilterLabels(this IGraphElementModel model, Type type)
        {
            return DerivedLabels
                .GetOrCreateValue(model)
                .GetOrAdd(
                    type,
                    closureType =>
                    {
                        var labels = model.Metadata
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(kvp => kvp.Value.LabelOverride.IfNone(kvp.Key.Name))
                            .OrderBy(x => x)
                            .ToArray();

                        return labels.Length == 0
                            ? default(Option<string[]>)
                            : labels.Length == model.Metadata.Count
                                ? Array.Empty<string>()
                                : labels;
                    });
        }

        internal static string[] GetValidFilterLabels(this IGraphElementModel model, Type type)
        {
            return model
                .TryGetFilterLabels(type)
                .IfNone(new[] { type.Name });   //TODO: What if type is abstract?
        }
    }
}
