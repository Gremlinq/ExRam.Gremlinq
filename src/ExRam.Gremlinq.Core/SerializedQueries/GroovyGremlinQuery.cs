using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GroovyGremlinQuery : ISerializedGremlinQuery
    {
        private static readonly Regex BindingRegex = new("_[a-z]+", RegexOptions.Compiled);

        private readonly bool _isInlined;
        private readonly bool _createdInternally;

        public GroovyGremlinQuery(string queryId, string script, IReadOnlyDictionary<string, object> bindings) : this(queryId, script, bindings, false, false)
        {
        }

        internal GroovyGremlinQuery(string queryId, string script, IReadOnlyDictionary<string, object> bindings, bool createdInternally, bool isInlined)
        {
            Script = script;
            Id = queryId;
            Bindings = bindings;
            _isInlined = isInlined;
            _createdInternally = createdInternally;
        }

        public override string ToString() => Script;

        public GroovyGremlinQuery Inline()
        {
            if (_isInlined || Bindings.Count == 0)
                return this;

            if (!_createdInternally)
                throw new InvalidOperationException($"Can't inline this {nameof(GroovyGremlinQuery)} since it was created by user code.");

            var newBindings = default(Dictionary<string, object>);

            var newScript = BindingRegex.Replace(
                Script,
                match =>
                {
                    var key = match.Value;
                    var value = Bindings[key];

                    switch (value)
                    {
                        case string str:
                        {
                            return $"'{str}'";
                        }
                        case int number:
                        {
                            return number.ToString();
                        }
                        case long number:
                        {
                            return number.ToString();
                        }
                        case bool boolean:
                        {
                            return boolean.ToString().ToLower();
                        }
                        case { } obj when obj.GetType().IsEnum && obj.GetType().GetEnumUnderlyingType() == typeof(int):
                        {
                            return ((int)obj).ToString();
                        }
                        case string[] { Length: <= 8 } strs:
                        {
                            return $"[{string.Join(",", strs.Select(x => $"'{x}'") )}]";
                        }
                        default:
                        {
                            (newBindings ??= new Dictionary<string, object>())[key] = value;

                            return key;
                        }
                    }
                });

            return new GroovyGremlinQuery(
                Id,
                newScript,
                ((IReadOnlyDictionary<string, object>?)newBindings) ?? ImmutableDictionary<string, object>.Empty,
                true,
                true);
        }

        public string Script { get; }

        public string Id { get; }

        public IReadOnlyDictionary<string, object> Bindings { get; }
    }
}
