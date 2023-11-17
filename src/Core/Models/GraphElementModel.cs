using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl<TBaseType> : IGraphElementModel
        {
            public static readonly IGraphElementModel Empty = new GraphElementModelImpl<TBaseType>();

            private readonly IImmutableSet<Assembly> _assemblies;
            private readonly IImmutableDictionary<Type, ElementMetadata> _elementMetadataOverrides;
            private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _memberMetadataOverrides;

            private GraphElementModelImpl() : this(ImmutableHashSet<Assembly>.Empty, ImmutableHashSet<Type>.Empty, ImmutableHashSet<MemberInfo>.Empty, ImmutableDictionary<Type, ElementMetadata>.Empty, ImmutableDictionary<MemberInfo, MemberMetadata>.Empty.WithComparers(MemberInfoEqualityComparer.Instance))
            {

            }

            private GraphElementModelImpl(IImmutableSet<Assembly> assemblies, IImmutableSet<Type> elementTypes, IImmutableSet<MemberInfo> members, IImmutableDictionary<Type, ElementMetadata> elementMetadataOverrides, IImmutableDictionary<MemberInfo, MemberMetadata> memberMetadataOverrides)
            {
                Members = members;
                ElementTypes = elementTypes;

                _assemblies = assemblies;
                _memberMetadataOverrides = memberMetadataOverrides;
                _elementMetadataOverrides = elementMetadataOverrides;
            }

            public IGraphElementModel AddAssemblies(params Assembly[] assemblies)
            {
                var newMembers = Members;
                var newElementTypes = ElementTypes;

                var types = assemblies
                    .Where(assembly => !_assemblies.Contains(assembly))
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
                    _assemblies.AddRange(assemblies),
                    newElementTypes,
                    newMembers,
                    _elementMetadataOverrides,
                    _memberMetadataOverrides);
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

                return new GraphElementModelImpl<TBaseType>(_assemblies, ElementTypes, Members, overrides, _memberMetadataOverrides);
            }

            public IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metadataTransformation) => IsWithinModel(elementType)
                ? new GraphElementModelImpl<TBaseType>(
                    _assemblies,
                    ElementTypes,
                    Members,
                    _elementMetadataOverrides.SetItem(
                        elementType,
                        metadataTransformation(this.GetMetadata(elementType))),
                    _memberMetadataOverrides)
                : ThrowOutsideModel<IGraphElementModel>();

            public IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation)
            {
                var overrides = _memberMetadataOverrides;

                foreach (var member in Members)
                {
                    var newMetadata = transformation(member, this.GetMetadata(member));

                    overrides = newMetadata != new MemberMetadata(member.Name)
                        ? overrides.SetItem(member, newMetadata)
                        : overrides.Remove(member);
                }

                return new GraphElementModelImpl<TBaseType>(_assemblies, ElementTypes, Members, _elementMetadataOverrides, overrides);
            }

            public IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation) => IsWithinModel(member)
                ? new GraphElementModelImpl<TBaseType>(
                    _assemblies,
                    ElementTypes,
                    Members,
                    _elementMetadataOverrides,
                    _memberMetadataOverrides.SetItem(
                        member,
                        transformation(this.GetMetadata(member))))
                : ThrowOutsideModel<IGraphElementModel>();

            public MemberMetadata? TryGetMetadata(MemberInfo member) => _memberMetadataOverrides.TryGetValue(member, out var metadata)
                ? metadata
                : IsWithinModel(member)
                    ? MemberMetadata.Default(member.Name)
                    : default(MemberMetadata?);

            public ElementMetadata? TryGetMetadata(Type elementType) => _elementMetadataOverrides.TryGetValue(elementType, out var elementMetadata)
                ? elementMetadata
                : IsWithinModel(elementType)
                    ? new ElementMetadata(elementType.Name)
                    : default(ElementMetadata?);

            public IImmutableSet<Type> ElementTypes { get; }

            public IImmutableSet<MemberInfo> Members { get; }


            private bool IsWithinModel(MemberInfo memberInfo) => memberInfo.DeclaringType is { } declaringType && (IsWithinModel(declaringType) || declaringType.IsAssignableFrom(typeof(TBaseType)));

            private bool IsWithinModel(Type type) => typeof(TBaseType).IsAssignableFrom(type) && _assemblies.Contains(type.Assembly);

            private static T ThrowOutsideModel<T>() => throw new InvalidOperationException();
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
