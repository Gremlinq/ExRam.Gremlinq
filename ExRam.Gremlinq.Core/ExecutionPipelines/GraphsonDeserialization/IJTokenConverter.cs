using System;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public interface IJTokenConverter
    {
        bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value);
    }
}
