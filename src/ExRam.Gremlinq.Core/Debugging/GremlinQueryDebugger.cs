#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using Newtonsoft.Json;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryDebugger
    {
        private sealed class DefaultGremlinQueryDebugger : IGremlinQueryDebugger
        {
            public string? TryToString(ISerializedGremlinQuery serializedQuery)
            {
                var indented = true;
                var groovyFormatting = GroovyFormatting.WithBindings;

                var maybeGroovy = serializedQuery
                    .TryToGroovy(groovyFormatting);

                if (maybeGroovy is { } groovy)
                {
                    return JsonConvert.SerializeObject(
                        groovy.Bindings.Count > 0
                            ? new { groovy.Script, groovy.Bindings }
                            : new { groovy.Script },
                        indented
                            ? Formatting.Indented
                            : Formatting.None);
                }

                return default;
            }
        }

        private sealed class OverrideGremlinQueryDebugger : IGremlinQueryDebugger
        {
            private readonly IGremlinQueryDebugger _baseDebugger;
            private readonly IGremlinQueryDebugger _overridingDebugger;

            public OverrideGremlinQueryDebugger(IGremlinQueryDebugger baseDebugger, IGremlinQueryDebugger overridingDebugger)
            {
                _baseDebugger = baseDebugger;
                _overridingDebugger = overridingDebugger;
            }

            public string? TryToString(ISerializedGremlinQuery serializedQuery)
            {
                return _baseDebugger.TryToString(serializedQuery) ?? _overridingDebugger.TryToString(serializedQuery);
            }
        }

        public static readonly IGremlinQueryDebugger Default = new DefaultGremlinQueryDebugger();

        public static IGremlinQueryDebugger Override(this IGremlinQueryDebugger debugger, IGremlinQueryDebugger overridingDebugger) => new OverrideGremlinQueryDebugger(debugger, overridingDebugger);
    }
}
