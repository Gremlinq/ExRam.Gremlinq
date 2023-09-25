using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementPropertyModel
    {
        private sealed class GraphElementPropertyModelImpl : IGraphElementPropertyModel
        {
            private readonly IImmutableDictionary<MemberInfo, MemberMetadata> _metadata;

            public GraphElementPropertyModelImpl(IImmutableSet<MemberInfo> members, IImmutableDictionary<MemberInfo, MemberMetadata> metadata)
            {
                Members = members;
                _metadata = metadata;
            }

            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                return new GraphElementPropertyModelImpl(Members, transformation(_metadata));
            }

            public IGraphElementPropertyModel ConfigureElement<TElement>(Func<IMemberMetadataConfigurator<TElement>, IMemberMetadataConfigurator<TElement>> transformation)
            {
                return ConfigureMemberMetadata(
                    metadata => transformation(new MemberMetadataConfigurator<TElement>()).Transform(metadata));
            }

            public MemberMetadata GetMetadata(MemberInfo memberInfo) => _metadata.TryGetValue(memberInfo, out var metadata)
                ? metadata
                : new MemberMetadata(memberInfo.Name);

            public IImmutableSet<MemberInfo> Members { get; }
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureMemberMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IGraphElementPropertyModel ConfigureElement<TElement>(Func<IMemberMetadataConfigurator<TElement>, IMemberMetadataConfigurator<TElement>> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureElement)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public MemberMetadata GetMetadata(MemberInfo memberInfo)
            {
                throw new InvalidOperationException($"{nameof(GetMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
            }

            public IImmutableSet<MemberInfo> Members
            {
                get
                {
                    throw new InvalidOperationException($"{nameof(Members)} must not be called on {nameof(GraphElementPropertyModel)}.{nameof(Invalid)}. Configure a valid model for the environment first.");
                }
            }
        }

        internal static Key GetKey(this IGremlinQueryEnvironment environment, Expression expression)
        {
            var memberExpression = expression.AssumeMemberExpression();

            return memberExpression.TryGetWellKnownMember() == WellKnownMember.PropertyValue && memberExpression.Expression is MemberExpression sourceMemberExpression
                ? environment.GetCache().GetKey(sourceMemberExpression.Member)
                : environment.GetCache().GetKey(memberExpression.Member);
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return Empty
                .ConfigureMemberMetadata(_ => _
                    .AddRange(models
                        .SelectMany(static model => model.ElementTypes)
                        .SelectMany(static x => x.GetTypeHierarchy())
                        .Distinct()
                        .SelectMany(static type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Select(static property => new KeyValuePair<MemberInfo, MemberMetadata>(property, new MemberMetadata(property.Name)))));
        }

        internal static readonly IGraphElementPropertyModel Empty = new GraphElementPropertyModelImpl(
            ImmutableHashSet<MemberInfo>.Empty,
            ImmutableDictionary<MemberInfo, MemberMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance));

        internal static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();
    }
}
