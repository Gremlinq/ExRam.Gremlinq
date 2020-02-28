using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class BytecodeExtensions
    {
        public static GroovySerializedGremlinQuery ToGroovy(this Bytecode bytecode)
        {
            var builder = new StringBuilder();
            var bindings = new Dictionary<object, string>();
            var variables = new Dictionary<string, object>();

            void Append(object obj, bool allowEnumerableExpansion = false)
            {
                if (obj is Bytecode bytecode)
                {
                    if (builder.Length > 0)
                        builder.Append("__");

                    foreach (var instruction in bytecode.StepInstructions)
                    {
                        builder.Append(builder.Length != 0
                            ? $".{instruction.OperatorName}("
                            : $"{instruction.OperatorName}(");

                        Append(instruction.Arguments, true);

                        builder.Append(")");
                    }
                }
                else if (obj is P p)
                {
                    if (p.Value is P p1)
                    {
                        Append(p1);
                        builder.Append($".{p.OperatorName}(");
                        Append(p.Other);

                        builder.Append(")");
                    }
                    else
                    {
                        builder.Append($"{p.OperatorName}(");

                        Append(p.Value, true);

                        builder.Append(")");
                    }
                }
                else if (obj is EnumWrapper t)
                {
                    builder.Append($"{t.EnumValue}");
                }
                else if (obj is ILambda lambda)
                {
                    builder.Append($"{{{lambda.LambdaExpression}}}");
                }
                else if (obj is string str && allowEnumerableExpansion)
                {
                    Append(str);
                }
                else if (obj is Type type)
                {
                    builder.Append(type.Name);
                }
                else if (obj is IEnumerable enumerable && allowEnumerableExpansion)
                {
                    var comma = false;
                    foreach (var argument in enumerable)
                    {
                        if (comma)
                            builder.Append(", ");
                        else
                            comma = true;

                        Append(argument);
                    }
                }
                else
                {
                    if (!bindings.TryGetValue(obj, out var bindingKey))
                    {
                        var next = bindings.Count;

                        do
                        {
                            bindingKey = (char)('a' + next % 26) + bindingKey;
                            next /= 26;
                        }
                        while (next > 0);

                        bindingKey = "_" + bindingKey;
                        bindings.Add(obj, bindingKey);

                        variables[bindingKey] = obj;
                    }

                    builder.Append(bindingKey);
                }
            }

            Append(bytecode);

            return new GroovySerializedGremlinQuery(
                builder.ToString(),
                variables);
        }
    }
}
