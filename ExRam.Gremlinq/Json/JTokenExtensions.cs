using System;
using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal static class JTokenExtensions
    {
        public static Option<JToken> Transform(this JToken token, JsonTransformRule rule)
        {
            Option<JToken> Recurse(JToken ast) => rule(ast, Recurse);

            return Recurse(token);
        }
    }
}
