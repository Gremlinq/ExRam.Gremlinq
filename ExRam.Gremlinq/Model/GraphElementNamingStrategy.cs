using System;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GraphElementNamingStrategy
    {
        private sealed class SimpleGraphElementNamingStrategy : IGraphElementNamingStrategy
        {
            public string GetLabelForType(Type type)
            {
                return type.Name;
            }
        }

        public static readonly IGraphElementNamingStrategy Simple = new SimpleGraphElementNamingStrategy();
    }
}