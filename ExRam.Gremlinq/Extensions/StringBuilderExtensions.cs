using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    public struct MethodStringBuilder
    {
        public enum MethodStringBuilderState
        {
            Idle,
            Chaining,
        }

        private readonly StringBuilder _builder;
        private readonly MethodStringBuilderState _state;

        public MethodStringBuilder(MethodStringBuilderState state, StringBuilder builder)
        {
            this._state = state;
            this._builder = builder;
        }

        public MethodStringBuilder AppendIdentifier(string className)
        {
            if (this._state != MethodStringBuilderState.Idle)
                throw new InvalidOperationException();

            this._builder.Append(className);
            return new MethodStringBuilder(MethodStringBuilderState.Chaining, this._builder);
        }

        public MethodStringBuilder AppendLambda(string lambda)
        {
            this._builder.Append("{");
            this._builder.Append(lambda);
            this._builder.Append("}");

            return this;
        }

        public MethodStringBuilder AppendMethod(string methodName, IEnumerable<object> parameters, IParameterCache parameterCache)
        {
            var setComma = false;

            if (this._state == MethodStringBuilderState.Chaining)
                this._builder.Append(".");

            this._builder.Append(methodName);
            this._builder.Append("(");

            foreach (var parameter in parameters)
            {
                if (setComma)
                    this._builder.Append(", ");

                var subMethodBuilder = new MethodStringBuilder(MethodStringBuilderState.Idle, this._builder);
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

            return new MethodStringBuilder(MethodStringBuilderState.Chaining, this._builder);
        }

        public MethodStringBuilder AppendField(string fieldName)
        {
            if (this._state != MethodStringBuilderState.Chaining)
                throw new InvalidOperationException();

            this._builder.Append(".");
            this._builder.Append(fieldName);

            return this;
        }

        public MethodStringBuilder AppendConstant(object constant, IParameterCache parameterCache)
        {
            if (this._state == MethodStringBuilderState.Chaining)
                throw new InvalidOperationException();

            if (constant is Enum)
            {
                return this
                    .AppendIdentifier(constant.GetType().Name)
                    .AppendField(Enum.GetName(constant.GetType(), constant));
            }

            this._builder.Append(parameterCache.Cache(constant));
            return this;
        }
    }

    internal static class StringBuilderExtensions
    {
        public static MethodStringBuilder ToMethodStringBuilder(this StringBuilder builder)
        {
            return new MethodStringBuilder(MethodStringBuilder.MethodStringBuilderState.Idle, builder);
        }
    }
}