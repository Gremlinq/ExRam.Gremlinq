using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Extensions;

namespace System.Linq
{
    public static class ImmutableDictionaryExtensions
    {
        internal static IImmutableDictionary<MemberInfo, PropertyMetadata> WithCamelCaseIdentifiers(this IImmutableDictionary<MemberInfo, PropertyMetadata> metadata)
        {
            return metadata
                .OverrideIdentifier(x => x.ToCamelCase());
        }

        internal static IImmutableDictionary<MemberInfo, PropertyMetadata> WithLowerCaseIdentifiers(this IImmutableDictionary<MemberInfo, PropertyMetadata> metadata)
        {
            return metadata
                .OverrideIdentifier(x => x.ToLower());
        }

        private static IImmutableDictionary<MemberInfo, PropertyMetadata> OverrideIdentifier(this IImmutableDictionary<MemberInfo, PropertyMetadata> metadata, Func<string, string> transformation)
        {
            return metadata
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kvp => new PropertyMetadata(
                        transformation(kvp.Value.IdentifierOverride.IfNone(kvp.Key.Name)),
                        kvp.Value.IgnoreDirective));
        }
    }
}
