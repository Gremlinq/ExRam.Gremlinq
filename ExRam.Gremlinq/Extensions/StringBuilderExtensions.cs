using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    internal static class StringBuilderExtensions
    {
        private sealed class MethodStringBuilder : IMethodStringBuilder
        {
            private enum MethodStringBuilderState
            {
                Idle,
                Chaining,
            }

            private MethodStringBuilderState _state = MethodStringBuilderState.Idle;
            private readonly StringBuilder _builder;

            public MethodStringBuilder(StringBuilder builder)
            {
                this._builder = builder;
            }

            public void AppendIdentifier(string className)
            {
                if (this._state != MethodStringBuilderState.Idle)
                    throw new InvalidOperationException();

                this._state = MethodStringBuilderState.Chaining;
                this._builder.Append(className);
            }

            public void AppendLambda(string lambda)
            {
                this._builder.Append("{");
                this._builder.Append(lambda);
                this._builder.Append("}");
            }

            public void AppendMethod(string methodName, IEnumerable<object> parameters, IParameterCache parameterCache)
            {
                var setComma = false;

                if (this._state == MethodStringBuilderState.Chaining)
                    this._builder.Append(".");
                else
                    this._state = MethodStringBuilderState.Chaining;

                this._builder.Append(methodName);
                this._builder.Append("(");

                foreach(var parameter in parameters)
                {
                    if (setComma)
                        this._builder.Append(", ");

                    using (var subMethodBuilder = new MethodStringBuilder(this._builder))
                    {
                        if (parameter is IGremlinSerializable serializable)
                        {
                            serializable.Serialize(subMethodBuilder, parameterCache);
                        }
                        else
                            subMethodBuilder.AppendConstant(parameter, parameterCache);
                    }

                    setComma = true;
                }

                this._builder.Append(")");
            }

            public void AppendField(string fieldName)
            {
                if (this._state != MethodStringBuilderState.Chaining)
                    throw new InvalidOperationException();

                this._builder.Append(".");
                this._builder.Append(fieldName);
            }

            public void AppendConstant(object constant, IParameterCache parameterCache)
            {
                if (this._state == MethodStringBuilderState.Chaining)
                    throw new InvalidOperationException();

                if (constant is Enum)
                {
                    this.AppendIdentifier(constant.GetType().Name);
                    this.AppendField(Enum.GetName(constant.GetType(), constant));
                }
                else
                    this._builder.Append(parameterCache.Cache(constant));
            }
            
            public void Dispose()
            {

            }
        }

        public static IMethodStringBuilder ToMethodStringBuilder(this StringBuilder builder)
        {
            return new MethodStringBuilder(builder);
        }
    }
}