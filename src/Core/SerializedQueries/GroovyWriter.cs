using System.Collections.Immutable;
using System.Text;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct GroovyWriter
    {
        private readonly bool _isEmpty;
        private readonly bool _hasIdentifier;

        private GroovyWriter(bool isEmpty, bool hasIdentifier)
        {
            _isEmpty = isEmpty;
            _hasIdentifier = hasIdentifier;
        }

        public static string ToString(Bytecode bytecode, IGremlinQueryEnvironment environment)
        {
            var groovyWriter = new GroovyWriter(true, false);
            var stringBuilder = new StringBuilder();

            groovyWriter.Append(bytecode, stringBuilder, null, environment);

            return stringBuilder.ToString();
        }

        public static GroovyGremlinQuery ToGroovyGremlinQuery(Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings)
        {
            var stringBuilder = new StringBuilder();
            var bindings = new Dictionary<object, Label>();
            var groovyWriter = new GroovyWriter(true, false);

            groovyWriter.Append(bytecode, stringBuilder, bindings, environment);

            return new GroovyGremlinQuery(
                stringBuilder.ToString(),
                includeBindings
                    ? bindings.ToDictionary(static kvp => (string)kvp.Value, static kvp => kvp.Key)
                    : ImmutableDictionary<string, object>.Empty);
        }

        private GroovyWriter Append(
            object obj,
            StringBuilder stringBuilder,
            Dictionary<object, Label>? bindings,
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
                            .Append(instruction, stringBuilder, bindings, environment);
                    }

                    return writer;
                }
                case Bytecode byteCode:
                {
                    var writer = StartTraversal(stringBuilder);

                    foreach (var instruction in byteCode.SourceInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder, bindings, environment);
                    }

                    foreach (var instruction in byteCode.StepInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder, bindings, environment);
                    }

                    return writer;
                }
                case Instruction instruction:
                {
                    return this
                        .StartOperator(instruction.OperatorName, stringBuilder)
                        .Append(instruction.Arguments, stringBuilder, bindings, environment, true)
                        .EndOperator(stringBuilder);
                }
                case P { Value: P p1 } p:
                {
                    return this
                        .Append(p1, stringBuilder, bindings, environment)
                        .StartOperator(p.OperatorName, stringBuilder)
                        .Append(p.Other, stringBuilder, bindings, environment)
                        .EndOperator(stringBuilder);
                }
                case P p:
                {
                    return this
                        .StartOperator(p.OperatorName, stringBuilder)
                        .Append((object)p.Value, stringBuilder, bindings, environment, true)
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
                case string str when bindings == null:
                {
                    return WriteQuoted(str, stringBuilder);
                }
                case DateTimeOffset dateTime when bindings == null:
                {
                    return WriteQuoted(dateTime.ToString("o"), stringBuilder);
                }
                case DateTime dateTime when bindings == null:
                {
                    return WriteQuoted(dateTime.ToString("o"), stringBuilder);
                }
                case bool b when bindings == null:
                {
                    return Write(b ? "true" : "false", stringBuilder);
                }
                case Type type:
                {
                    return Write(type.Name, stringBuilder);
                }
                case AbstractTraversalStrategy traversalStrategy:
                {
                    if (environment.Serializer.TryTransform(traversalStrategy, environment, out GroovyExpression groovyExpression))
                        return Append(groovyExpression, stringBuilder, bindings, environment, allowEnumerableExpansion);

                    throw new NotSupportedException("Cannot ");
                }
                case object[] objectArray when allowEnumerableExpansion:
                {
                    var writer = this;

                    for (var i = 0; i < objectArray.Length; i++)
                    {
                        writer = writer
                            .StartParameter(i, stringBuilder)
                            .Append(objectArray[i], stringBuilder, bindings, environment);
                    }

                    return writer;
                }
                case null:
                    return Write("null", stringBuilder);
                case not null when bindings != null:
                {
                    if (!bindings.TryGetValue(obj, out var bindingKey))
                    {
                        bindingKey = bindings.Count;
                        bindings.Add(obj, bindingKey);
                    }

                    stringBuilder.Append(bindingKey);

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

        private GroovyWriter Identifier(string identifier, StringBuilder stringBuilder)
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

        private GroovyWriter StartParameter(int parameterIndex, StringBuilder stringBuilder)
        {
            if (parameterIndex > 0)
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

        private GroovyWriter EndOperator(StringBuilder stringBuilder)
        {
            stringBuilder.Append(')');

            return new(false, true);
        }

        private GroovyWriter WriteQuoted(object value, StringBuilder stringBuilder)
        {
#if NET6_0_OR_GREATER
            var handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
            handler.AppendLiteral("'");
            handler.AppendFormatted(value);
            handler.AppendLiteral("'");

            stringBuilder.Append(ref handler);
#else
            stringBuilder
                .Append('\'')
                .Append(value)
                .Append('\'');
#endif

            return new(false, _hasIdentifier);
        }

        private GroovyWriter Write(object value, StringBuilder stringBuilder)
        {
#if NET6_0_OR_GREATER
            var handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder);
            handler.AppendFormatted(value);

            stringBuilder.Append(ref handler);
#else
            stringBuilder.Append(value);
#endif

            return new(false, _hasIdentifier);
        }
    }
}
