using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphElementPropertyModel
    {
        private sealed class GraphElementPropertyModelImpl : IGraphElementPropertyModel
        {
            public GraphElementPropertyModelImpl(IImmutableDictionary<MemberInfo, MemberMetadata> metadata)
            {
                MemberMetadata = metadata;
            }
            
            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                return new GraphElementPropertyModelImpl(transformation(MemberMetadata));
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MemberMetadata { get; }
        }

        private sealed class InvalidGraphElementPropertyModel : IGraphElementPropertyModel
        {
            public IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureMemberMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{Invalid}. Configure a valid model for the environment first.");
            }

            public IImmutableDictionary<MemberInfo, MemberMetadata> MemberMetadata
            {
                get
                {
                    throw new InvalidOperationException($"{nameof(MemberMetadata)} must not be called on {nameof(GraphElementPropertyModel)}.{Invalid}. Configure a valid model for the environment first.");
                }
            }
        }

        public static readonly IGraphElementPropertyModel Empty = new GraphElementPropertyModelImpl(
            ImmutableDictionary<MemberInfo, MemberMetadata>
                .Empty
                .WithComparers(MemberInfoEqualityComparer.Instance));

        public static readonly IGraphElementPropertyModel Invalid = new InvalidGraphElementPropertyModel();

        public static IGraphElementPropertyModel ConfigureElement<TElement>(this IGraphElementPropertyModel model, Func<IMemberMetadataConfigurator<TElement>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation)
        {
            return model.ConfigureMemberMetadata(
                metadata => transformation(new MemberMetadataConfigurator<TElement>(metadata)));
        }

        internal static Key GetKey(this IGremlinQueryEnvironment environment, Expression expression)
        {
            if (expression is LambdaExpression lambdaExpression)
                return environment.GetKey(lambdaExpression.Body);

            if (expression.Strip() is MemberExpression memberExpression)
            {
                return memberExpression.TryGetWellKnownMember() == WellKnownMember.PropertyValue && memberExpression.Expression is MemberExpression sourceMemberExpression
                    ? environment.GetCache().GetKey(sourceMemberExpression.Member)
                    : environment.GetCache().GetKey(memberExpression.Member);
            }

            throw new ExpressionNotSupportedException(expression);
        }

        internal static IGraphElementPropertyModel FromGraphElementModels(params IGraphElementModel[] models)
        {
            return Empty
                .ConfigureMemberMetadata(_ => _
                    .AddRange(models
                        .SelectMany(model => model.Metadata.Keys)
                        .SelectMany(x => x.GetTypeHierarchy())
                        .Distinct()
                        .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        .Select(property => new KeyValuePair<MemberInfo, MemberMetadata>(property, new MemberMetadata(property.Name)))));
        }
    }
}
