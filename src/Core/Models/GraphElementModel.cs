using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl<TBaseType> : IGraphElementModel
        {
            private readonly IImmutableDictionary<Type, ElementMetadata> _elementMetadataOverrides;
            private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _memberMetadataOverrides;

            public GraphElementModelImpl() : this(ImmutableHashSet<Type>.Empty, ImmutableHashSet<MemberInfo>.Empty)
            {

            }

            public GraphElementModelImpl(IImmutableSet<Type> elementTypes, IImmutableSet<MemberInfo> members) : this(elementTypes, members, ImmutableDictionary<Type, ElementMetadata>.Empty, ImmutableDictionary<MemberInfo, MemberMetadata>.Empty.WithComparers(MemberInfoEqualityComparer.Instance))
            {
            }

            public GraphElementModelImpl(IImmutableSet<Type> elementTypes, IImmutableSet<MemberInfo> members, IImmutableDictionary<Type, ElementMetadata> elementMetadataOverrides, IImmutableDictionary<MemberInfo, MemberMetadata> memberMetadataOverrides)
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
                    var newMetadata = metaDataTransformation(elementType, GetMetadata(elementType));

                    overrides = newMetadata.Label != elementType.Name   //TODO: Equality operators
                        ? overrides.SetItem(elementType, newMetadata)
                        : overrides.Remove(elementType);
                }

                return new GraphElementModelImpl<TBaseType>(ElementTypes, Members, overrides, _memberMetadataOverrides);
            }

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metadataTransformation) => new GraphElementModelImpl<TBaseType>(
                ElementTypes,
                Members,
                _elementMetadataOverrides.SetItem(
                    elementType,
                    metadataTransformation(GetMetadata(elementType))),
                _memberMetadataOverrides);

            public ElementMetadata GetMetadata(Type elementType) => typeof(TBaseType).IsAssignableFrom(elementType)
                ? _elementMetadataOverrides.TryGetValue(elementType, out var elementMetadata)
                    ? elementMetadata
                    : new ElementMetadata(elementType.Name)
                : throw new ArgumentException();

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

            public IGraphElementModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation) => new GraphElementModelImpl<TBaseType>(
                ElementTypes,
                Members,
                _elementMetadataOverrides,
                _memberMetadataOverrides.SetItem(
                    member,
                    transformation(this.GetMetadata(member))));

            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo) => _memberMetadataOverrides.TryGetValue(memberInfo, out var metadata)
                ? metadata
                : typeof(TBaseType).IsAssignableFrom(memberInfo.DeclaringType)
                    ? MemberMetadata.Default(memberInfo.Name)
                    : default(MemberMetadata?);

            public IGraphElementModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation)
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

            public IGraphElementModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation) => throw Throw(nameof(ConfigureMemberMetadata));

            public IGraphElementModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation) => throw Throw(nameof(ConfigureMemberMetadata));

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation) => throw Throw(nameof(ConfigureMetadata));

            public IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation) => throw Throw(nameof(ConfigureMetadata));

            public ElementMetadata GetMetadata(Type elementType) => throw Throw(nameof(GetMetadata));

            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo) => throw Throw(nameof(TryGetMetadata));

            private InvalidOperationException Throw(string name) => throw new InvalidOperationException($"{name} must not be called on {nameof(GraphElementModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
        }

        internal static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        public static IGraphElementModel UseCamelCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToCamelCase());

        public static IGraphElementModel UseLowerCaseLabels(this IGraphElementModel model) => model.ConfigureLabels(static (_, proposedLabel) => proposedLabel.ToLower());

        public static IGraphElementModel ConfigureLabels(this IGraphElementModel model, Func<Type, string, string> overrideTransformation) => model.ConfigureMetadata((type, metadata) => new ElementMetadata(overrideTransformation(type, metadata.Label)));

        public static IGraphElementModel ConfigureElement<TElement>(this IGraphElementModel model, Func<IMemberMetadataConfigurator<TElement>, IMemberMetadataConfigurator<TElement>> transformation) => transformation(new MemberMetadataConfigurator<TElement>()).Transform(model);

        public static IGraphElementModel UseCamelCaseNames(this IGraphElementModel model) => model.ConfigureNames(static (_, key) => key.RawKey is string name
            ? name.ToCamelCase()
            : key);

        public static IGraphElementModel UseLowerCaseNames(this IGraphElementModel model) => model.ConfigureNames(static (_, key) => key.RawKey is string name
            ? name.ToLower()
            : key);

        public static MemberMetadata GetMetadata(this IGraphElementModel model, MemberInfo memberInfo) => model.TryGetMetadata(memberInfo) ?? throw new ArgumentException();

        internal static IGraphElementModel ConfigureNames(this IGraphElementModel model, Func<MemberInfo, Key, Key> transformation) => model.ConfigureMemberMetadata((member, metadata) => new MemberMetadata(
            transformation(member, metadata.Key),
            metadata.SerializationBehaviour));

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
