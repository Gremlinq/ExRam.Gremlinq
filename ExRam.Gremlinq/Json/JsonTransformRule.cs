using System;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal delegate Option<JToken> JsonTransformRule(JToken token, Func<JToken, Option<JToken>> recurse);
}