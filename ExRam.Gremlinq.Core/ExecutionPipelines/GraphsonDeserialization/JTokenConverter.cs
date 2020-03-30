using System;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class JTokenConverter
    {
        private sealed class NullJTokenConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                return default;
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

            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                var ret = _converter2
                    .TryConvert(jToken, objectType, recurse);

                return ret.IsSome
                    ? ret
                    : _converter1.TryConvert(jToken, objectType, recurse);
            }
        }

        public static readonly IJTokenConverter Null = new NullJTokenConverter();

        public static IJTokenConverter Combine(this IJTokenConverter converter1, IJTokenConverter converter2)
        {
            return new CombinedJTokenConverter(converter1, converter2);
        }
    }
}