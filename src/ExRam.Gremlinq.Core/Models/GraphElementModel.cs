using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl : IGraphElementModel
        {
            public GraphElementModelImpl(IImmutableDictionary<Type, ElementMetadata> metaData)
            {
                Metadata = metaData;
            }

            public IGraphElementModel ConfigureMetadata(Func<IImmutableDictionary<Type, ElementMetadata>, IImmutableDictionary<Type, ElementMetadata>> transformation) => new GraphElementModelImpl(transformation(Metadata));

            public IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation) => ConfigureMetadata(_ => _.ToImmutableDictionary(
                static kvp => kvp.Key,
                kvp => new ElementMetadata(overrideTransformation(kvp.Key, kvp.Value.Label))));

            public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            public IImmutableDictionary<Type, ElementMetadata> Metadata => throw new InvalidOperationException($"{nameof(Metadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation) => throw new InvalidOperationException($"{nameof(ConfigureLabels)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureMetadata(Func<IImmutableDictionary<Type, ElementMetadata>, IImmutableDictionary<Type, ElementMetadata>> transformation) => throw new InvalidOperationException($"{nameof(ConfigureMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
        }

        public static readonly IGraphElementModel Empty = new GraphElementModelImpl(ImmutableDictionary<Type, ElementMetadata>.Empty);
        public static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        public static IGraphElementModel FromTypes(IEnumerable<Type> types)
        {
            return new GraphElementModelImpl(types
                .Where(static type => type.IsClass && !type.IsAbstract)
                .ToImmutableDictionary(
                    static type => type,
                    static type => new ElementMetadata(type.Name)));
        }

        public static IGraphElementModel FromBaseType<TType>(IEnumerable<Assembly>? assemblies)
        {
            return FromBaseType(typeof(TType), assemblies);
        }

        public static IGraphElementModel FromBaseType(Type baseType, IEnumerable<Assembly>? assemblies)
        {
            return FromTypes((assemblies ?? Enumerable.Empty<Assembly>())
                .Distinct()
                .SelectMany(static assembly =>
                {
                    try
                    {
                        return assembly
                            .DefinedTypes
                            .Select(static x => x.AsType());
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        return ex.Types
                            .Where(static x => x is not null)
                            .Select(static x => x!);
                    }
                })
                .Where(type => type != baseType && !type.IsNestedPrivate && baseType.IsAssignableFrom(type))
                .Prepend(baseType));
        }

        public static IGraphElementModel UseCamelCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToCamelCase());

        public static IGraphElementModel UseLowerCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToLower());

        public static ImmutableArray<string>? TryGetFilterLabels(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity)
        {
            var labels = model.GetCache().GetDerivedLabels(type);

            return labels.IsEmpty
                ? default(ImmutableArray<string>?)
                : labels.Length == model.Metadata.Count && verbosity == FilterLabelsVerbosity.Minimum
                    ? ImmutableArray<string>.Empty
                    : labels;
        }

        internal static ImmutableArray<string> GetFilterLabelsOrDefault(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity) => model.TryGetFilterLabels(type, verbosity) ?? ImmutableArray.Create(type.Name);
    }
}
