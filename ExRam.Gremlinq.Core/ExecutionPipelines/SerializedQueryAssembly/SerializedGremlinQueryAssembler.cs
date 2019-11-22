using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class SerializedGremlinQueryAssembler
    {
        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName)
        {
            assembler.Method(methodName);
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter, Action<object> recurse)
        {
            assembler.Method(
                methodName,
                () => recurse(parameter));
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter1, object parameter2, Action<object> recurse)
        {
            assembler.Method(
                methodName,
                () => recurse(parameter1),
                () => recurse(parameter2));
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter1, object parameter2, object parameter3, Action<object> recurse)
        {
            assembler.Method(
                methodName,
                () => recurse(parameter1),
                () => recurse(parameter2),
                () => recurse(parameter3));
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, IEnumerable<object> parameters, Action<object> recurse)
        {
            assembler.Method(
                methodName,
                parameters
                    .Select(parameter => new Action(() => recurse(parameter)))
                    .ToArray());
        }
    }
}
