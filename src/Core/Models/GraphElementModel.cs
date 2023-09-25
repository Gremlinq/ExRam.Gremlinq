using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl<TBaseType> : IGraphElementModel
        {
            private readonly IImmutableDictionary<Type, ElementMetadata> _metaDataOverrides;

            public GraphElementModelImpl() : this(ImmutableHashSet<Type>.Empty)
            {

            }

            public GraphElementModelImpl(IImmutableSet<Type> elementTypes) : this(elementTypes, ImmutableDictionary<Type, ElementMetadata>.Empty)
            {
            }

            public GraphElementModelImpl(IImmutableSet<Type> elementTypes, IImmutableDictionary<Type, ElementMetadata> metaDataOverrides)
            {
                ElementTypes = elementTypes;
                _metaDataOverrides = metaDataOverrides;
            }

            public IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation)
            {
                var overrides = _metaDataOverrides;

                foreach (var elementType in ElementTypes)
                {
                    var newLabel = overrideTransformation(elementType, GetMetadata(elementType).Label);

                    overrides = newLabel != elementType.Name
                        ? overrides.SetItem(elementType, new ElementMetadata(newLabel))
                        : overrides.Remove(elementType);
                }

                return new GraphElementModelImpl<TBaseType>(ElementTypes, overrides);
            }

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation)
            {
                var overrides = _metaDataOverrides;

                foreach (var elementType in ElementTypes)
                {
                    var newMetadata = metaDataTransformation(elementType, GetMetadata(elementType));

                    overrides = newMetadata.Label != elementType.Name   //TODO: Equality operators
                        ? overrides.SetItem(elementType, newMetadata)
                        : overrides.Remove(elementType);
                }

                throw new NotImplementedException();
            }

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metadataTransformation) => new GraphElementModelImpl<TBaseType>(
                ElementTypes,
                _metaDataOverrides.SetItem(
                    elementType,
                    metadataTransformation(GetMetadata(elementType))));

            public ElementMetadata GetMetadata(Type elementType) => typeof(TBaseType).IsAssignableFrom(elementType)
                ? _metaDataOverrides.TryGetValue(elementType, out var elementMetadata)
                    ? elementMetadata
                    : new ElementMetadata(elementType.Name)
                : throw new ArgumentException();

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies) => new GraphElementModelImpl<TBaseType>(
                ElementTypes.AddRange(assemblies
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
                    .Where(static type => type is { IsNestedPrivate: false, IsClass: true, IsAbstract: false } && type != typeof(TBaseType) && typeof(TBaseType).IsAssignableFrom(type))),
                _metaDataOverrides);

            public IImmutableSet<Type> ElementTypes { get; }
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            public IImmutableSet<Type> ElementTypes => throw new InvalidOperationException($"{nameof(ElementTypes)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies) => throw new InvalidOperationException($"{nameof(AddAssemblies)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation) => throw new InvalidOperationException($"{nameof(ConfigureLabels)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation) => throw new InvalidOperationException($"{nameof(ConfigureMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation) => throw new InvalidOperationException($"{nameof(ConfigureMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");

            public ElementMetadata GetMetadata(Type elementType) => throw new InvalidOperationException($"{nameof(GetMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
        }

        private sealed class EmptyGraphElementModel : IGraphElementModel
        {
            public IImmutableSet<Type> ElementTypes => ImmutableHashSet<Type>.Empty;

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies) => throw new InvalidOperationException($"{nameof(AddAssemblies)} must not be called on {nameof(GraphElementModel)}.{nameof(Empty)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation) => this;

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation) => throw new InvalidOperationException($"{nameof(ConfigureMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Empty)}. Configure a valid model for the environment first.");

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation) => throw new InvalidOperationException($"{nameof(ConfigureMetadata)} must not be called on {nameof(GraphElementModel)}.{nameof(Empty)}. Configure a valid model for the environment first.");

            public ElementMetadata GetMetadata(Type elementType) => new (elementType.Name);
        }

        internal static readonly IGraphElementModel Empty = new EmptyGraphElementModel();
        internal static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        public static IGraphElementModel UseCamelCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToCamelCase());

        public static IGraphElementModel UseLowerCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToLower());

        internal static ImmutableArray<string>? TryGetFilterLabels(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity)
        {
            var labels = model.GetCache().GetDerivedLabels(type);

            return labels.IsEmpty
                ? default(ImmutableArray<string>?)
                : labels.Length == model.ElementTypes.Count && verbosity == FilterLabelsVerbosity.Minimum
                    ? ImmutableArray<string>.Empty
                    : labels;
        }

        internal static IGraphElementModel FromBaseType<TType>() => new GraphElementModelImpl<TType>().AddAssemblies(typeof(TType).Assembly);

        internal static ImmutableArray<string> GetFilterLabelsOrDefault(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity) => model.TryGetFilterLabels(type, verbosity) ?? ImmutableArray.Create(type.Name);
    }
}
