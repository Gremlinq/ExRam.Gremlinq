using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class BytecodeExtensions
    {
        private static readonly ThreadLocal<StringBuilder> Builder = new(() => new StringBuilder());

        public static GroovyGremlinQuery ToGroovy(this Bytecode bytecode, GroovyFormatting formatting = GroovyFormatting.WithBindings)
        {
            var builder = Builder.Value!;

            try
            {
                var bindings = new Dictionary<object, BindingKey>();
                var variables = new Dictionary<string, object>();

                void Append(object obj, bool allowEnumerableExpansion = false)
                {
                    switch (obj)
                    {
                        case Bytecode byteCode:
                        {
                            if (builder.Length > 0)
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
                            if (builder.Length != 0)
                                builder.Append('.');

                            builder
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
                        case string str when allowEnumerableExpansion:
                        {
                            // ReSharper disable once TailRecursiveCall
                            Append(str);

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
                        case IEnumerable enumerable when allowEnumerableExpansion:
                        {
                            var comma = false;
                            foreach (var argument in enumerable)
                            {
                                if (comma)
                                    builder.Append(',');
                                else
                                    comma = true;

                                Append(argument);
                            }

                            break;
                        }
                        default:
                        {
                            if (!bindings.TryGetValue(obj, out var bindingKey))
                            {
                                bindingKey = bindings.Count;
                                bindings.Add(obj, bindingKey);

                                variables[bindingKey] = obj;
                            }

                            builder.Append(bindingKey);

                            break;
                        }
                    }
                }

                Append(bytecode);

                var ret = new GroovyGremlinQuery(
                    Guid.NewGuid().ToString(), //TODO: Take over bytecode query id ?
                    builder.ToString(),
                    variables,
                    true,
                    false);

                return formatting == GroovyFormatting.Inline
                    ? ret.Inline()
                    : ret;
            }
            finally
            {
                builder.Clear();
            }
        }
    }
}
