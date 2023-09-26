using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementPropertyModel
    {
        private sealed class GraphElementPropertyModelImpl : IGraphElementPropertyModel
        {
            private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _metadataOverrides;

            public GraphElementPropertyModelImpl(IImmutableSet<MemberInfo> members, IImmutableDictionary<MemberInfo, MemberMetadata> metadataOverrides)
            {
                Members = members;
                _metadataOverrides = metadataOverrides;
            }

            public IGraphElementPropertyModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation)
            {
                return new GraphElementPropertyModelImpl(
                    Members,
                    _metadataOverrides.SetItem(
                        member,
                        transformation(this.GetMetadata(member))));
            }

            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo) => _metadataOverrides.TryGetValue(memberInfo, out var metadata)
                ? metadata
                : MemberMetadata.Default(memberInfo.Name);

            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation)
            {
                var overrides = _metadataOverrides;

                foreach (var member in Members)
                {
                    var newMetadata = transformation(member, this.GetMetadata(member));

                    overrides = newMetadata.Key != member.Name || newMetadata.SerializationBehaviour != SerializationBehaviour.Default  //TODO: Equality operators
                        ? overrides.SetItem(member, newMetadata)
                        : overrides.Remove(member);
                }

                return new GraphElementPropertyModelImpl(Members, overrides);
            }

            public IGraphElementPropertyModel AddType(Type type)
            {
                return new GraphElementPropertyModelImpl(
                    Members.AddRange(type.GetSerializableProperties()),
                    _metadataOverrides);
            }

            public IImmutableSet<MemberInfo> Members { get; }
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public MemberMetadata? TryGetMetadata(MemberInfo memberInfo)
            {
                throw new InvalidOperationException($"{nameof(TryGetMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureMemberMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IGraphElementPropertyModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureMemberMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IGraphElementPropertyModel AddType(Type type)
            {
                throw new InvalidOperationException($"{nameof(AddType)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IImmutableSet<MemberInfo> Members
            {
                get
                {
                    throw new InvalidOperationException($"{nameof(Members)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
                }
            }
        }

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel propertiesModel, Func<IMemberMetadataConfigurator<TElement>, IMemberMetadataConfigurator<TElement>> transformation)
        {
            return transformation(new MemberMetadataConfigurator<TElement>()).Transform(propertiesModel);
        }

        public static IGraphElementPropertyModel UseCamelCaseNames(this IGraphElementPropertyModel model)
        {
            return model.ConfigureNames(static (_, key) => key.RawKey is string name
                ? name.ToCamelCase()
                : key);
        }

        public static IGraphElementPropertyModel UseLowerCaseNames(this IGraphElementPropertyModel model)
        {
            return model.ConfigureNames(static (_, key) => key.RawKey is string name
                ? name.ToLower()
                : key);
        }

        public static MemberMetadata GetMetadata(this IGraphElementPropertyModel model, MemberInfo memberInfo) => model.TryGetMetadata(memberInfo) ?? throw new ArgumentException();

        internal static IGraphElementPropertyModel ConfigureNames(this IGraphElementPropertyModel model, Func<MemberInfo, Key, Key> transformation)
        {
            return model.ConfigureMemberMetadata((member, metadata) => new MemberMetadata(
                transformation(member, metadata.Key),
                metadata.SerializationBehaviour));
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return models
                .Aggregate(Empty, static (propertiesModel, elementModel) => elementModel.ElementTypes
                    .Aggregate(propertiesModel, static (propertiesModel, type) => propertiesModel.AddType(type)));
        }

        internal static readonly IGraphElementPropertyModel Empty = new GraphElementPropertyModelImpl(
            ImmutableHashSet<MemberInfo>.Empty,
            ImmutableDictionary<MemberInfo, MemberMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance));

        internal static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();
    }
}
