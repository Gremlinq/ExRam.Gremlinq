#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using Newtonsoft.Json;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryDebugger
    {
        private sealed class GroovyGremlinQueryDebugger : IGremlinQueryDebugger
        {
            private readonly bool _indented;
            private readonly GroovyFormatting _formatting;

            public GroovyGremlinQueryDebugger(GroovyFormatting formatting, bool indented)
            {
                _indented = indented;
                _formatting = formatting;
            }

            public string? TryToString(ISerializedGremlinQuery serializedQuery)
            {
                var maybeGroovy = serializedQuery
                    .TryToGroovy(_formatting);

                if (maybeGroovy is { } groovy)
                {
                    return JsonConvert.SerializeObject(
                        groovy.Bindings.Count > 0
                            ? new { groovy.Script, groovy.Bindings }
                            : new { groovy.Script },
                        _indented
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
                return _overridingDebugger.TryToString(serializedQuery) ?? _baseDebugger.TryToString(serializedQuery);
            }
        }

        public static IGremlinQueryDebugger Groovy(GroovyFormatting formatting, bool indented) => new GroovyGremlinQueryDebugger(formatting, indented);

        public static IGremlinQueryDebugger Override(this IGremlinQueryDebugger debugger, IGremlinQueryDebugger overridingDebugger) => new OverrideGremlinQueryDebugger(debugger, overridingDebugger);
    }
}
