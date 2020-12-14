using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Core
{
    public sealed class GroovyGremlinQuery
    {
        private static readonly Regex BindingRegex = new Regex("_[a-z]+", RegexOptions.Compiled);

        private readonly bool _isInlined;
        private readonly bool _createdInternally;

        public GroovyGremlinQuery(string script, IReadOnlyDictionary<string, object> bindings) : this(script, bindings, false, false)
        {
        }

        internal GroovyGremlinQuery(string script, IReadOnlyDictionary<string, object> bindings, bool createdInternally, bool isInlined)
        {
            Script = script;
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
                        default:
                        {
                            (newBindings ??= new Dictionary<string, object>())[key] = value;

                            return key;
                        }
                    }
                });

            return new GroovyGremlinQuery(
                newScript,
                ((IReadOnlyDictionary<string, object>?)newBindings) ?? ImmutableDictionary<string, object>.Empty,
                true,
                true);
        }

        public string Script { get; }

        public IReadOnlyDictionary<string, object> Bindings { get; }
    }
}
