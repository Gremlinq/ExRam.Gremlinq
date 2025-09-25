#pragma warning disable IDE0003 // Remove qualification

using System.Collections;
using System.Collections.Immutable;
using System.Text;

using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct GroovyWriter
    {
        private readonly struct Bindings : IEnumerable<KeyValuePair<object, Label>>
        {
            private readonly Dictionary<object, Label>? _dictionary;
            private readonly List<KeyValuePair<object, Label>>? _list;

            public Bindings(Dictionary<object, Label> dictionary)
            {
                _dictionary = dictionary;
            }

            public Bindings(List<KeyValuePair<object, Label>> list)
            {
                _list = list;
            }

            public Label GetOrAdd(object obj)
            {
                if (_list is { } list)
                {
                    var bindingKey = list.Count;
                    list.Add(new KeyValuePair<object, Label>(obj, bindingKey));

                    return bindingKey;
                }

                if (_dictionary is { } dictionary)
                {
                    if (!dictionary.TryGetValue(obj, out var bindingKey))
                    {
                        bindingKey = dictionary.Count;
                        dictionary.Add(obj, bindingKey);
                    }

                    return bindingKey;
                }

                throw new InvalidOperationException();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<KeyValuePair<object, Label>> GetEnumerator()
            {
                if (_list is { } list)
                    return list.GetEnumerator();

                if (_dictionary is { } dictionary)
                    return dictionary.GetEnumerator();

                throw new InvalidOperationException();
            }
        }

        private readonly bool _isEmpty;
        private readonly bool _hasIdentifier;

        private GroovyWriter(bool isEmpty, bool hasIdentifier)
        {
            _isEmpty = isEmpty;
            _hasIdentifier = hasIdentifier;
        }

        public static string ToString(Bytecode bytecode, IGremlinQueryEnvironment environment) => ToGroovyScriptImpl(bytecode, environment, null);

        public static CheapGroovyGremlinScript ToCheapGroovyScript(Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings)
        {
            var bindings = new Bindings(new List<KeyValuePair<object, Label>>());
            var script = ToGroovyScriptImpl(bytecode, environment, bindings);

            return CheapGroovyGremlinScript.From(
                script,
                includeBindings
                    ? bindings.Select(kvp => new KeyValuePair<string, object?>(kvp.Value, kvp.Key))
                    : null);
        }

        public static GroovyGremlinScript ToGroovyScript(Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings)
        {
            var bindings = new Bindings(new Dictionary<object, Label>());
            var script = ToGroovyScriptImpl(bytecode, environment, bindings);

            return GroovyGremlinScript.From(
                script,
                includeBindings
                    ? bindings.ToImmutableDictionary(static kvp => (string)kvp.Value, static kvp => (object?)kvp.Key)
                    : null);
        }

        private static string ToGroovyScriptImpl(Bytecode bytecode, IGremlinQueryEnvironment environment, Bindings? maybeBindings)
        {
            var stringBuilder = new StringBuilder();
            var groovyWriter = new GroovyWriter(true, false);

            groovyWriter
                .Append(bytecode, stringBuilder, maybeBindings, environment);

            return stringBuilder
                .ToString();
        }

        private GroovyWriter Append(
            object? obj,
            StringBuilder stringBuilder,
            Bindings? maybeBindings,
            IGremlinQueryEnvironment environment,
            bool allowEnumerableExpansion = false)
        {
            switch (obj)
            {
                case GroovyExpression expression:
                {
                    var writer = Identifier(expression.Identifier, stringBuilder);

                    foreach (var instruction in expression.Instructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder, maybeBindings, environment);
                    }

                    return writer;
                }
                case Bytecode byteCode:
                {
                    var writer = StartTraversal(stringBuilder);

                    foreach (var instruction in byteCode.SourceInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder, maybeBindings, environment);
                    }

                    foreach (var instruction in byteCode.StepInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder, maybeBindings, environment);
                    }

                    return writer;
                }
                case Instruction instruction:
                {
                    return this
                        .StartOperator(instruction.OperatorName, stringBuilder)
                        .Append(instruction.Arguments, stringBuilder, maybeBindings, environment, true)
                        .EndOperator(stringBuilder);
                }
                case P { Value: P p1, Other: { } otherP, OperatorName: { } operatorName }:
                {
                    return this
                        .Append(p1, stringBuilder, maybeBindings, environment)
                        .StartOperator(operatorName, stringBuilder)
                        .Append(otherP, stringBuilder, maybeBindings, environment)
                        .EndOperator(stringBuilder);
                }
                case P { Value: { } pValue, OperatorName: { } operatorName }:
                {
                    return this
                        .StartOperator(operatorName, stringBuilder)
                        .Append(pValue, stringBuilder, maybeBindings, environment, true)
                        .EndOperator(stringBuilder);
                }
                case EnumWrapper t:
                {
                    return Write(t.EnumValue, stringBuilder);
                }
                case ILambda lambda:
                {
                    return WriteLambda(lambda.LambdaExpression, stringBuilder);
                }
                case string str when maybeBindings == null:
                {
                    return WriteQuoted(str, stringBuilder);
                }
                case DateTimeOffset dateTime when maybeBindings == null:
                {
                    return WriteQuoted(dateTime, "o", stringBuilder);
                }
                case DateTime dateTime when maybeBindings == null:
                {
                    return WriteQuoted(dateTime, "o", stringBuilder);
                }
                case bool b when maybeBindings == null:
                {
                    return Write(b ? "true" : "false", stringBuilder);
                }
                case Type type:
                {
                    return Write(type.Name, stringBuilder);
                }
                case AbstractTraversalStrategy traversalStrategy:
                {
                    return Append(
                        environment.Serializer
                            .TransformTo<GroovyExpression>()
                            .From(traversalStrategy, environment),
                        stringBuilder,
                        maybeBindings,
                        environment,
                        allowEnumerableExpansion);
                }
                case IList list when !environment.SupportsTypeNatively(list.GetType()):
                {
                    var writer = allowEnumerableExpansion
                        ? this
                        : StartArray(stringBuilder);

                    for (var i = 0; i < list.Count; i++)
                    {
                        writer = writer
                            .StartElement(i, stringBuilder)
                            .Append(list[i], stringBuilder, maybeBindings, environment);
                    }

                    return allowEnumerableExpansion
                        ? writer
                        : writer.EndArray(stringBuilder);
                }
                case null:
                    return Write("null", stringBuilder);
                case not null when maybeBindings is { } bindings:
                {
                    stringBuilder
                        .Append(bindings
                            .GetOrAdd(obj));

                    return new();
                }
                default:
                    return Write(obj, stringBuilder);
            }
        }

        private GroovyWriter StartTraversal(StringBuilder stringBuilder) => Identifier(
            _isEmpty
                ? "g"
                : "__",
            stringBuilder);

#pragma warning disable CA1822 // Mark members as static
        private GroovyWriter Identifier(string identifier, StringBuilder stringBuilder)
#pragma warning restore CA1822 // Mark members as static
        {
            stringBuilder.Append(identifier);

            return new(false, true);
        }

        private GroovyWriter StartOperator(string operatorName, StringBuilder stringBuilder)
        {
            if (_hasIdentifier)
                stringBuilder.Append('.');

            stringBuilder
                .Append(operatorName)
                .Append('(');

            return new();
        }

        private GroovyWriter StartArray(StringBuilder stringBuilder)
        {
            stringBuilder
                .Append('[');

            return new();
        }

        private GroovyWriter EndArray(StringBuilder stringBuilder)
        {
            stringBuilder
                .Append(']');

            return new();
        }

        private GroovyWriter StartElement(int elementIndex, StringBuilder stringBuilder)
        {
            if (elementIndex > 0)
                stringBuilder.Append(',');

            return new(false, _hasIdentifier);
        }

        private GroovyWriter WriteLambda(string lambda, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append('{')
                .Append(lambda)
                .Append('}');

            return new(false, _hasIdentifier);
        }

#pragma warning disable CA1822 // Mark members as static
        private GroovyWriter EndOperator(StringBuilder stringBuilder)
#pragma warning restore CA1822 // Mark members as static
        {
            stringBuilder.Append(')');

            return new(false, true);
        }

        private GroovyWriter WriteQuoted(string value, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append('\'')
                .Append(value)
                .Append('\'');

            return new(false, _hasIdentifier);
        }

        private GroovyWriter WriteQuoted<T>(T value, string format, StringBuilder stringBuilder)
        {
            var handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
            handler.AppendLiteral("'");
            handler.AppendFormatted(value, format: format);
            handler.AppendLiteral("'");

            stringBuilder.Append(ref handler);

            return new(false, _hasIdentifier);
        }

        private GroovyWriter Write(string value, StringBuilder stringBuilder)
        {
            stringBuilder.Append(value);

            return new(false, _hasIdentifier);
        }

        private GroovyWriter Write(object value, StringBuilder stringBuilder)
        {
            var handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder);
            handler.AppendFormatted(value);

            stringBuilder.Append(ref handler);

            return new(false, _hasIdentifier);
        }
    }
}
