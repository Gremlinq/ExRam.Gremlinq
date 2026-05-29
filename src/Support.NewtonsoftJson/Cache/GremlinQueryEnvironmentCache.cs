using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryEnvironmentCache
    {
        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, ICachingGremlinQueryEnvironment> Caches = new();

        public static ICachingGremlinQueryEnvironment GetCache(this IGremlinQueryEnvironment environment)
        {
            if (environment is ICachingGremlinQueryEnvironment caching)
                return caching;

            return Caches.GetValue(environment, static closure => new CachingGremlinQueryEnvironmentImpl(closure));
        }
    }
}
