using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core;
using LanguageExt;

namespace System.Linq
{
    public static class ImmutableDictionaryExtensions
    {
        public static TValue GetValue<TValue>(this IImmutableDictionary<GremlinqOption, object> options, GremlinqOption<TValue> option)
        {
            return (options?.TryGetValue(option))
                .ToOption()
                .Bind(x => x)
                .Map(optionValue => (TValue)optionValue)
                .IfNone(option.DefaultValue);
        }

        public static IImmutableDictionary<GremlinqOption, object> ConfigureValue<TValue>(this IImmutableDictionary<GremlinqOption, object> options, GremlinqOption<TValue> option, Func<TValue, TValue> configuration)
        {
            return options.SetItem(option, configuration(options.GetValue(option)));
        }

        internal static IImmutableDictionary<MemberInfo, PropertyMetadata> ConfigureNames(this IImmutableDictionary<MemberInfo, PropertyMetadata> metadata, Func<MemberInfo, string, string> transformation)
        {
            return metadata
                .SetItems(metadata
                    .Select(kvp => new KeyValuePair<MemberInfo, PropertyMetadata>(
                        kvp.Key,
                        new PropertyMetadata(
                            transformation(kvp.Key, kvp.Value.Name),
                            kvp.Value.SerializationBehaviour))));
        }
    }
}
