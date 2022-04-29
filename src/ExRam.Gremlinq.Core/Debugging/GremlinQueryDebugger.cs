#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryDebugger
    {
        private sealed class OverrideGremlinQueryDebugger : IGremlinQueryDebugger
        {
            private readonly IGremlinQueryDebugger _baseDebugger;
            private readonly IGremlinQueryDebugger _overridingDebugger;

            public OverrideGremlinQueryDebugger(IGremlinQueryDebugger baseDebugger, IGremlinQueryDebugger overridingDebugger)
            {
                _baseDebugger = baseDebugger;
                _overridingDebugger = overridingDebugger;
            }

            public string? TryToString(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _overridingDebugger.TryToString(serializedQuery, environment) ?? _baseDebugger.TryToString(serializedQuery, environment);
            }
        }

        public static readonly IGremlinQueryDebugger Groovy = new GroovyGremlinQueryDebugger();

        public static IGremlinQueryDebugger Override(this IGremlinQueryDebugger debugger, IGremlinQueryDebugger overridingDebugger) => new OverrideGremlinQueryDebugger(debugger, overridingDebugger);
    }
}
