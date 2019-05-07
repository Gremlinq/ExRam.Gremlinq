using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Extensions;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModel
    {
        private sealed class EmptyGraphElementModel : IGraphElementModel
        {
            public IImmutableDictionary<Type, string> Labels => ImmutableDictionary<Type, string>.Empty;
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphElementModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IImmutableDictionary<Type, string> Labels => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(Labels)));
        }

        private sealed class CamelcaseGraphElementModel : IGraphElementModel
        {
            public CamelcaseGraphElementModel(IGraphElementModel baseModel)
            {
                Labels = baseModel.Labels
                    .ToImmutableDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.ToCamelCase());
            }

            public IImmutableDictionary<Type, string> Labels { get; }
        }

        private sealed class LowercaseGraphElementModel : IGraphElementModel
        {
            public LowercaseGraphElementModel(IGraphElementModel baseModel)
            {
                Labels = baseModel.Labels
                    .ToImmutableDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.ToLower());
            }

            public IImmutableDictionary<Type, string> Labels { get; }
        }

        public static readonly IGraphElementModel Empty = new EmptyGraphElementModel();
        public static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        public static IGraphElementModel WithCamelCaseLabels(this IGraphElementModel model)
        {
            return new CamelcaseGraphElementModel(model);
        }

        public static IGraphElementModel WithLowerCaseLabels(this IGraphElementModel model)
        {
            return new LowercaseGraphElementModel(model);
        }

        internal static string[] GetValidFilterLabels(this IGraphElementModel model, Type type)
        {
            return model
                .TryGetFilterLabels(type)
                .IfNone(new[] { type.Name });   //TODO: What if type is abstract?
        }
    }
}
