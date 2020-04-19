using System;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    internal static class JTokenConverter
    {
        private sealed class NullJTokenConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class CombinedJTokenConverter : IJTokenConverter
        {
            private readonly IJTokenConverter _converter1;
            private readonly IJTokenConverter _converter2;

            public CombinedJTokenConverter(IJTokenConverter converter1, IJTokenConverter converter2)
            {
                _converter1 = converter1;
                _converter2 = converter2;
            }

            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                return _converter2.TryConvert(jToken, objectType, recurse, out value) || _converter1.TryConvert(jToken, objectType, recurse, out value);
            }
        }

        public static readonly IJTokenConverter Null = new NullJTokenConverter();

        public static IJTokenConverter Combine(this IJTokenConverter converter1, IJTokenConverter converter2)
        {
            return new CombinedJTokenConverter(converter1, converter2);
        }
    }
}
