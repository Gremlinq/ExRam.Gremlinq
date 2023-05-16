using System.Text;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct GroovyWriter
    {
        private readonly bool _hasIdentifier;

        private GroovyWriter(bool hasIdentifier)
        {
            _hasIdentifier = hasIdentifier;
        }

        public static string ToString(Bytecode bytecode)
        {
            var groovyWriter = new GroovyWriter();
            var stringBuilder = new StringBuilder();

            groovyWriter.Append(bytecode, stringBuilder);

            return stringBuilder.ToString();
        }

        private GroovyWriter Append(
            object obj,
            StringBuilder stringBuilder,
            bool allowEnumerableExpansion = false)
        {
            switch (obj)
            {
                case Bytecode byteCode:
                {
                    var writer = StartTraversal(stringBuilder);

                    foreach (var instruction in byteCode.SourceInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder);
                    }

                    foreach (var instruction in byteCode.StepInstructions)
                    {
                        writer = writer
                            .Append(instruction, stringBuilder);
                    }

                    return writer;
                }
                case Instruction instruction:
                {
                    return this
                        .StartOperator(instruction.OperatorName, stringBuilder)
                        .Append(instruction.Arguments, stringBuilder, true)
                        .EndOperator(stringBuilder);
                }
                case P { Value: P p1 } p:
                {
                    return this
                        .Append(p1, stringBuilder)
                        .StartOperator(p.OperatorName, stringBuilder)
                        .Append(p.Other, stringBuilder)
                        .EndOperator(stringBuilder);
                }
                case P p:
                {
                    return this
                        .StartOperator(p.OperatorName, stringBuilder)
                        .Append((object)p.Value, stringBuilder, true)
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
                case string str:
                {
                    return WriteQuoted(str, stringBuilder);
                }
                case DateTimeOffset dateTime:
                {
                    return WriteQuoted(dateTime.ToString("o"), stringBuilder);
                }
                case DateTime dateTime:
                {
                    return WriteQuoted(dateTime.ToString("o"), stringBuilder);
                }
                case bool b:
                {
                    return Write(b ? "true" : "false", stringBuilder);
                }
                case Type type:
                {
                    return Write(type.Name, stringBuilder);
                }
                case object[] objectArray when allowEnumerableExpansion:
                {
                    var writer = this;

                    for (var i = 0; i < objectArray.Length; i++)
                    {
                        writer = writer
                            .StartParameter(i, stringBuilder)
                            .Append(objectArray[i], stringBuilder);
                    }

                    return writer;
                }
                case null:
                    return Write("null", stringBuilder);
                default:
                    return Write(obj, stringBuilder);
            }
        }

        private GroovyWriter StartTraversal(StringBuilder stringBuilder) => Identifier(
            stringBuilder.Length == 0
                ? "g"
                : "__",
            stringBuilder);

        private GroovyWriter Identifier(string identifier, StringBuilder stringBuilder)
        {
            stringBuilder.Append(identifier);

            return new(true);
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

            return new(_hasIdentifier);
        }

        private GroovyWriter WriteLambda(string lambda, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append('{')
                .Append(lambda)
                .Append('}');

            return new(_hasIdentifier);
        }

        private GroovyWriter EndOperator(StringBuilder stringBuilder)
        {
            stringBuilder.Append(')');

            return new(true);
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

            return new(_hasIdentifier);
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

            return new(_hasIdentifier);
        }
    }
}
