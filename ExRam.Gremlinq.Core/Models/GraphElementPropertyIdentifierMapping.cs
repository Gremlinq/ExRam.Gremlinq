using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExRam.Gremlinq.Core.Extensions;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertyIdentifierMapping
    {
        public static IImmutableDictionary<MemberInfo, MemberMetadata> ToCamelCase(this IImmutableDictionary<MemberInfo, MemberMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kpv =>
                    {
                        var retVal = kpv.Value.Identifier;

                        return new MemberMetadata(
                            retVal is string identifier ? identifier.ToCamelCase() : retVal,
                            kpv.Value.IgnoreDirective);
                    });
        }

        public static IImmutableDictionary<MemberInfo, MemberMetadata> ToLowerCase(this IImmutableDictionary<MemberInfo, MemberMetadata> mapping)
        {
            return mapping
                .ToImmutableDictionary(
                    kvp => kvp.Key,
                    kpv =>
                    {
                        var retVal = kpv.Value.Identifier;

                        return new MemberMetadata(
                            retVal is string identifier ? identifier.ToLower() : retVal,
                            kpv.Value.IgnoreDirective);
                    });
        }
    }
}
