using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModel
    {
        private sealed class EmptyGraphElementModel : IGraphElementModel
        {
            public Option<string> TryGetConstructiveLabel(Type elementType) => default;

            public Option<string[]> TryGetFilterLabels(Type elementType) => default;

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public Option<string> IdPropertyName { get; }
        }

        private sealed class InvalidGraphElementModel : IGraphElementModel
        {
            public Option<string> TryGetConstructiveLabel(Type elementType) => throw new InvalidOperationException();

            public Option<string[]> TryGetFilterLabels(Type elementType) => throw new InvalidOperationException();

            public Option<string> IdPropertyName { get => throw new InvalidOperationException(); }
        }

        public static readonly IGraphElementModel Empty = new EmptyGraphElementModel();
        public static readonly IGraphElementModel Invalid = new InvalidGraphElementModel();

        internal static string[] GetValidFilterLabels(this IGraphElementModel model, Type type)
        {
            return model.TryGetFilterLabels(type)
                .Filter(labels =>
                {
                    if (labels.Length == 0)
                        throw new GraphModelException($"Can't determine labels for type {type.FullName}.");

                    return true;
                })
                .IfNone(Array.Empty<string>());
        }
    }
}
