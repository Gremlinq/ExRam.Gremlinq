using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl<TBaseType> : IGraphElementModel
        {
            public static readonly IGraphElementModel Empty = new GraphElementModelImpl<TBaseType>();

            private readonly IImmutableDictionary<Type, ElementMetadata> _elementMetadataOverrides;
            private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _memberMetadataOverrides;

            private GraphElementModelImpl() : this(ImmutableHashSet<Type>.Empty, ImmutableHashSet<MemberInfo>.Empty, ImmutableDictionary<Type, ElementMetadata>.Empty, ImmutableDictionary<MemberInfo, MemberMetadata>.Empty.WithComparers(MemberInfoEqualityComparer.Instance))
            {

            }

            private GraphElementModelImpl(IImmutableSet<Type> elementTypes, IImmutableSet<MemberInfo> members, IImmutableDictionary<Type, ElementMetadata> elementMetadataOverrides, IImmutableDictionary<MemberInfo, MemberMetadata> memberMetadataOverrides)
            {
                Members = members;
                ElementTypes = elementTypes;
                _memberMetadataOverrides = memberMetadataOverrides;
                _elementMetadataOverrides = elementMetadataOverrides;
            }

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation)
            {
                var overrides = _elementMetadataOverrides;

                foreach (var elementType in ElementTypes)
                {
                    var newMetadata = metaDataTransformation(elementType, this.GetMetadata(elementType));

                    overrides = newMetadata.Label != elementType.Name   //TODO: Equality operators
                        ? overrides.SetItem(elementType, newMetadata)
                        : overrides.Remove(elementType);
                }

                return new GraphElementModelImpl<TBaseType>(ElementTypes, Members, overrides, _memberMetadataOverrides);
            }

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metadataTransformation) => typeof(TBaseType).IsAssignableFrom(elementType)
                ? new GraphElementModelImpl<TBaseType>(
                    ElementTypes,
                    Members,
                    _elementMetadataOverrides.SetItem(
                        elementType,
                        metadataTransformation(this.GetMetadata(elementType))),
                    _memberMetadataOverrides)
                : throw new InvalidOperationException();

            public ElementMetadata? TryGetMetadata(Type elementType) => typeof(TBaseType).IsAssignableFrom(elementType)
                ? _elementMetadataOverrides.TryGetValue(elementType, out var elementMetadata)
                    ? elementMetadata
                    : new ElementMetadata(elementType.Name)
                : default(ElementMetadata?);

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies)
            {
                var newElementTypes = ElementTypes;
                var newMembers = Members;

                var types = assemblies
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
                    .Where(static type => type is { IsNestedPrivate: false, IsClass: true, IsAbstract: false } && type != typeof(TBaseType) && typeof(TBaseType).IsAssignableFrom(type));

                foreach (var type in types)
                {
                    newElementTypes = newElementTypes.Add(type);
                    newMembers = newMembers.AddRange(type.GetSerializableProperties());
                }

                return new GraphElementModelImpl<TBaseType>(
                    newElementTypes,
                    newMembers,
                    _elementMetadataOverrides,
                    _memberMetadataOverrides);
            }

            public IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation) => typeof(TBaseType).IsAssignableFrom(member.DeclaringType)
                ? new GraphElementModelImpl<TBaseType>(
                    ElementTypes,
                    Members,
                    _elementMetadataOverrides,
                    _memberMetadataOverrides.SetItem(
                        member,
                        transformation(this.GetMetadata(member))))
                : throw new InvalidOperationException();

            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo) => _memberMetadataOverrides.TryGetValue(memberInfo, out var metadata)
                ? metadata
                : typeof(TBaseType).IsAssignableFrom(memberInfo.DeclaringType)
                    ? MemberMetadata.Default(memberInfo.Name)
                    : default(MemberMetadata?);

            public IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation)
            {
                var overrides = _memberMetadataOverrides;

                foreach (var member in Members)
                {
                    var newMetadata = transformation(member, this.GetMetadata(member));

                    overrides = newMetadata.Key != member.Name || newMetadata.SerializationBehaviour != SerializationBehaviour.Default  //TODO: Equality operators
                        ? overrides.SetItem(member, newMetadata)
                        : overrides.Remove(member);
                }

                return new GraphElementModelImpl<TBaseType>(ElementTypes, Members, _elementMetadataOverrides, overrides);
            }

            public IImmutableSet<Type> ElementTypes { get; }

            public IImmutableSet<MemberInfo> Members { get; }
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            public IImmutableSet<Type> ElementTypes => throw Throw(nameof(ElementTypes));

            public IImmutableSet<MemberInfo> Members => throw Throw(nameof(Members));

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies) => throw Throw(nameof(AddAssemblies));

            public IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation) => throw Throw(nameof(ConfigureMetadata));

            public IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation) => throw Throw(nameof(ConfigureMetadata));

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation) => throw Throw(nameof(ConfigureMetadata));

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation) => throw Throw(nameof(ConfigureMetadata));

            public ElementMetadata? TryGetMetadata(Type elementType) => throw Throw(nameof(TryGetMetadata));

            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo) => throw Throw(nameof(TryGetMetadata));

            private InvalidOperationException Throw(string name) => throw new InvalidOperationException($"{name} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
        }

        internal static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        public static IGraphElementModel UseCamelCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToCamelCase());

        public static IGraphElementModel UseLowerCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToLower());

        public static IGraphElementModel ConfigureLabels(this IGraphElementModel model, Func<Type, string, string> overrideTransformation) => model.ConfigureMetadata((type, metadata) => new ElementMetadata(overrideTransformation(type, metadata.Label)));

        public static IGraphElementModel ConfigureElement<TElement>(this IGraphElementModel model, Func<IMemberMetadataConfigurator<TElement>, IMemberMetadataConfigurator<TElement>> transformation) => transformation(MemberMetadataConfigurator<TElement>.Identity).Transform(model);

        public static IGraphElementModel UseCamelCaseMemberNames(this IGraphElementModel model) => model.ConfigureMemberNames(static (_, name) => name.ToCamelCase());

        public static IGraphElementModel UseLowerCaseMemberNames(this IGraphElementModel model) => model.ConfigureMemberNames(static (_, name) => name.ToLower());

        public static IGraphElementModel ConfigureMemberNames(this IGraphElementModel model, Func<MemberInfo, string, string> transformation) => model.ConfigureMetadata((member, metadata) => metadata.Key.RawKey is string name
            ? new MemberMetadata(
                transformation(member, name),
                metadata.SerializationBehaviour)
            : metadata);

        internal static ImmutableArray<string>? TryGetFilterLabels(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity)
        {
            var labels = model.GetCache().GetDerivedLabels(type);

            return labels.IsEmpty
                ? default(ImmutableArray<string>?)
                : labels.Length == model.ElementTypes.Count && verbosity == FilterLabelsVerbosity.Minimum
                    ? ImmutableArray<string>.Empty
                    : labels;
        }

        internal static IGraphElementModel FromBaseType<TType>() => GraphElementModelImpl<TType>.Empty.AddAssemblies(typeof(TType).Assembly);

        internal static ImmutableArray<string> GetFilterLabelsOrDefault(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity) => model.TryGetFilterLabels(type, verbosity) ?? ImmutableArray.Create(type.Name);
    }
}
