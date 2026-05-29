using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal static class GremlinQueryEnvironmentCache
    {
        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, ICachingGremlinQueryEnvironment> Caches = new();

        public static ICachingGremlinQueryEnvironment GetCache(this IGremlinQueryEnvironment environment) => environment is ICachingGremlinQueryEnvironment caching
            ? caching
            : Caches.GetValue(environment, static closure => new CachingGremlinQueryEnvironmentImpl(closure));
    }
}
