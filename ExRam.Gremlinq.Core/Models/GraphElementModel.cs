using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModel
    {
        private sealed class GraphElementModelImpl : IGraphElementModel
        {
            public GraphElementModelImpl(IImmutableDictionary<Type, ElementMetadata> metaData)
            {
                Metadata = metaData;
            }

            public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
        }

        public static readonly IGraphElementModel Empty = new GraphElementModelImpl(ImmutableDictionary<Type, ElementMetadata>.Empty);

        private static readonly ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string[]>> DerivedLabels = new ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string[]>>();

        public static IGraphElementModel FromTypes(IEnumerable<Type> types)
        {
            return new GraphElementModelImpl(types
                .Where(x => x.IsClass)
                .Where(type => !type.IsAbstract)
                .ToImmutableDictionary(
                    type => type,
                    type => new ElementMetadata(type.Name)));
        }

        public static IGraphElementModel FromBaseType<TType>(IEnumerable<Assembly>? assemblies, ILogger? logger)
        {
            return FromBaseType(typeof(TType), assemblies, logger);
        }

        public static IGraphElementModel FromBaseType(Type baseType, IEnumerable<Assembly>? assemblies, ILogger? logger)
        {
            return FromTypes((assemblies ?? Enumerable.Empty<Assembly>())
                .Distinct()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly
                            .DefinedTypes
                            .Where(type => type != baseType && !type.IsNestedPrivate && baseType.IsAssignableFrom(type))
                            .Select(typeInfo => typeInfo);
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        logger?.LogWarning(ex, $"{nameof(ReflectionTypeLoadException)} thrown during GraphModel creation.");
                        return Array.Empty<TypeInfo>();
                    }
                })
                .Prepend(baseType));
        }

        public static IGraphElementModel ConfigureLabels(this IGraphElementModel model, Func<Type, string, string> overrideTransformation)
        {
            return model.ConfigureMetadata(_ => _.ToImmutableDictionary(
                kvp => kvp.Key,
                kvp => new ElementMetadata(overrideTransformation(kvp.Key, kvp.Value.Label))));
        }

        public static IGraphElementModel UseCamelCaseLabels(this IGraphElementModel model)
        {
            return model.ConfigureLabels((type, proposedLabel) => proposedLabel.ToCamelCase());
        }

        public static IGraphElementModel UseLowerCaseLabels(this IGraphElementModel model)
        {
            return model.ConfigureLabels((type, proposedLabel) => proposedLabel.ToLower());
        }

        internal static string[] GetFilterLabelsOrDefault(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity)
        {
            return model
                .TryGetFilterLabels(type, verbosity)
                .IfNoneUnsafe(default(string[])) ?? new[] { type.Name };
        }

        public static Option<string[]> TryGetFilterLabels(this IGraphElementModel model, Type type, FilterLabelsVerbosity verbosity)
        {
            var labels = DerivedLabels
                .GetOrCreateValue(model)
                .GetOrAdd(
                    type,
                    (closureType, closureModel) =>
                    {
                        return closureModel.Metadata
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(kvp => kvp.Value.Label)
                            .OrderBy(x => x)
                            .ToArray();
                    },
                    model);


            return labels.Length == 0
                ? default(Option<string[]>)
                : labels.Length == model.Metadata.Count && verbosity == FilterLabelsVerbosity.Minimum
                    ? Array.Empty<string>()
                    : labels;
        }

        private static IGraphElementModel ConfigureMetadata(this IGraphElementModel model, Func<IImmutableDictionary<Type, ElementMetadata>, IImmutableDictionary<Type, ElementMetadata>> transformation)
        {
            return new GraphElementModelImpl(
                transformation(model.Metadata));
        }
    }
}
