using System;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public interface IJTokenConverter
    {
        OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse);
    }
}