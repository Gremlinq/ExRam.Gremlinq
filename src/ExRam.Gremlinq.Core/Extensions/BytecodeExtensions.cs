using System.Collections.Immutable;
using System.Text;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class BytecodeExtensions
    {
        private static readonly ThreadLocal<StringBuilder> Builder = new(static () => new StringBuilder());

        public static GroovyGremlinQuery ToGroovy(this Bytecode bytecode) => bytecode.ToGroovy(true);

        internal static GroovyGremlinQuery ToGroovy(this Bytecode bytecode, bool includeBindings)
        {
            var builder = Builder.Value!;

            builder.Append("g");

            try
            {
                var bindings = new Dictionary<object, Label>();
                var variables = includeBindings
                    ? new Dictionary<string, object>()
                    : default;

                void Append(object obj, bool allowEnumerableExpansion = false)
                {
                    switch (obj)
                    {
                        case Bytecode byteCode:
                        {
                            if (builder.Length > 1)
                                builder.Append("__");

                            foreach (var instruction in byteCode.SourceInstructions)
                            {
                                Append(instruction);
                            }

                            foreach (var instruction in byteCode.StepInstructions)
                            {
                                Append(instruction);
                            }

                            break;
                        }
                        case Instruction instruction:
                        {
                            builder
                                .Append('.')
                                .Append(instruction.OperatorName)
                                .Append('(');

                            Append(instruction.Arguments, true);

                            builder
                                .Append(')');

                            break;
                        }
                        case P {Value: P p1} p:
                        {
                            Append(p1);

                            builder
                                .Append('.')
                                .Append(p.OperatorName)
                                .Append('(');

                            Append(p.Other);

                            builder
                                .Append(')');

                            break;
                        }
                        case P p:
                        {
                            builder
                                .Append(p.OperatorName)
                                .Append('(');

                            Append(p.Value, true);

                            builder
                                .Append(')');

                            break;
                        }
                        case EnumWrapper t:
                        {
                            builder.Append(t.EnumValue);

                            break;
                        }
                        case ILambda lambda:
                        {
                            builder
                                .Append('{')
                                .Append(lambda.LambdaExpression)
                                .Append('}');

                            break;
                        }
                        case Type type:
                        {
                            builder.Append(type.Name);

                            break;
                        }
                        case object[] objectArray when allowEnumerableExpansion:
                        {
                            for (var i = 0; i < objectArray.Length; i++)
                            {
                                if (i != 0)
                                    builder.Append(',');

                                Append(objectArray[i]);
                            }

                            break;
                        }
                        case { } o:
                        {
                            if (!bindings.TryGetValue(o, out var bindingKey))
                            {
                                bindingKey = bindings.Count;
                                bindings.Add(o, bindingKey);

                                variables?.Add(bindingKey, o);
                            }

                            builder.Append(bindingKey);

                            break;
                        }
                        case null:
                        {
                            builder.Append("null");

                            break;
                        }
                    }
                }

                Append(bytecode);

                return new GroovyGremlinQuery(
                    builder.ToString(),
                    (IReadOnlyDictionary<string, object>?)variables ?? ImmutableDictionary<string, object>.Empty);
            }
            finally
            {
                builder.Clear();
            }
        }
    }
}
