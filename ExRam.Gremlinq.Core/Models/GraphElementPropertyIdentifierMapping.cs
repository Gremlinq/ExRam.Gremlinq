using System;
using System.Reflection;
using ExRam.Gremlinq.Core.Extensions;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertyIdentifierMapping
    {
        private sealed class DefaultGraphElementPropertyIdentifierMapping : IGraphElementPropertyIdentifierMapping
        {
            public object ToIdentifier(MemberInfo memberInfo)
            {
                var memberName = memberInfo.Name;

                if (string.Equals(memberName, "id", StringComparison.OrdinalIgnoreCase))
                    return T.Id;

                if (string.Equals(memberName, "label", StringComparison.OrdinalIgnoreCase))
                    return T.Label;

                return memberName;
            }
        }

        private sealed class InvalidGraphElementPropertyIdentifierMapping : IGraphElementPropertyIdentifierMapping
        {
            public object ToIdentifier(MemberInfo memberInfo)
            {
                throw new InvalidOperationException();//TODO
            }
        }

        private sealed class CamelCaseGraphElementPropertiesModel : IGraphElementPropertyIdentifierMapping
        {
            private readonly IGraphElementPropertyIdentifierMapping _mapping;

            public CamelCaseGraphElementPropertiesModel(IGraphElementPropertyIdentifierMapping mapping)
            {
                _mapping = mapping;
            }

            public object ToIdentifier(MemberInfo memberInfo)
            {
                var retVal = _mapping.ToIdentifier(memberInfo);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }
        }

        private sealed class LowerCaseGraphElementPropertiesModel : IGraphElementPropertyIdentifierMapping
        {
            private readonly IGraphElementPropertyIdentifierMapping _mapping;

            public LowerCaseGraphElementPropertiesModel(IGraphElementPropertyIdentifierMapping mapping)
            {
                _mapping = mapping;
            }

            public object ToIdentifier(MemberInfo memberInfo)
            {
                var retVal = _mapping.ToIdentifier(memberInfo);

                return retVal is string identifier ? identifier.ToLower() : retVal;
            }
        }

        public static IGraphElementPropertyIdentifierMapping ToCamelCase(this IGraphElementPropertyIdentifierMapping mapping)
        {
            return new CamelCaseGraphElementPropertiesModel(mapping);
        }

        public static IGraphElementPropertyIdentifierMapping ToLowerCase(this IGraphElementPropertyIdentifierMapping mapping)
        {
            return new LowerCaseGraphElementPropertiesModel(mapping);
        }

        public static readonly IGraphElementPropertyIdentifierMapping Default = new DefaultGraphElementPropertyIdentifierMapping();
        public static readonly IGraphElementPropertyIdentifierMapping Invalid = new InvalidGraphElementPropertyIdentifierMapping();
    }
}