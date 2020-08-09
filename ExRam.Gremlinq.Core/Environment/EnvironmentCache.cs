using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class EnvironmentCache
    {
        private sealed class EnvironmentCacheImpl : IEnvironmentCache
        {
            private readonly IGremlinQueryEnvironment _environment;

            public EnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }
        }

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IEnvironmentCache> Caches = new ConditionalWeakTable<IGremlinQueryEnvironment, IEnvironmentCache>();

        public static IEnvironmentCache GetCache(this IGremlinQueryEnvironment environment)
        {
            return Caches.GetValue(
                environment,
                closure => new EnvironmentCacheImpl(closure));
        }
    }
}