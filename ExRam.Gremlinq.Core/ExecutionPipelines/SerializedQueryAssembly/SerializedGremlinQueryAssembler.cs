using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public static class SerializedGremlinQueryAssembler
    {
        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName)
        {
            assembler.OpenMethod(methodName);
            assembler.CloseMethod();
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter, Action<object> recurse)
        {
            assembler.OpenMethod(methodName);
            {
                assembler.StartParameter();
                recurse(parameter);
                assembler.EndParameter();
            }
            assembler.CloseMethod();
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter1, object parameter2, Action<object> recurse)
        {
            assembler.OpenMethod(methodName);
            {
                assembler.StartParameter();
                recurse(parameter1);
                assembler.EndParameter();

                assembler.StartParameter();
                recurse(parameter2);
                assembler.EndParameter();
            }
            assembler.CloseMethod();
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, object parameter1, object parameter2, object parameter3, Action<object> recurse)
        {
            assembler.OpenMethod(methodName);
            {
                assembler.StartParameter();
                recurse(parameter1);
                assembler.EndParameter();

                assembler.StartParameter();
                recurse(parameter2);
                assembler.EndParameter();

                assembler.StartParameter();
                recurse(parameter3);
                assembler.EndParameter();
            }
            assembler.CloseMethod();
        }

        public static void Method(this ISerializedGremlinQueryAssembler assembler, string methodName, IEnumerable<object> parameters, Action<object> recurse)
        {
            assembler.OpenMethod(methodName);
            {
                foreach (var parameter in parameters)
                {
                    assembler.StartParameter();
                    recurse(parameter);
                    assembler.EndParameter();
                }
            }
            assembler.CloseMethod();
        }
    }
}
