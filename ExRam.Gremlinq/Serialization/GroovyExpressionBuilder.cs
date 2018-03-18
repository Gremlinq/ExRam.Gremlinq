using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    public struct GroovyExpressionBuilder
    {
        public enum State
        {
            Idle,
            Chaining,
        }

        private readonly State _state;
        private readonly StringBuilder _stringBuilder;

        public GroovyExpressionBuilder(State state, StringBuilder stringBuilder)
        {
            this._state = state;
            this._stringBuilder = stringBuilder;
        }

        public GroovyExpressionBuilder AppendIdentifier(string className)
        {
            if (this._state != State.Idle)
                throw new InvalidOperationException();

            this._stringBuilder.Append(className);
            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder);
        }

        public GroovyExpressionBuilder AppendLambda(string lambda)
        {
            this._stringBuilder.Append("{");
            this._stringBuilder.Append(lambda);
            this._stringBuilder.Append("}");

            return this;
        }

        public GroovyExpressionBuilder AppendMethod(string methodName, IEnumerable<object> parameters, IParameterCache parameterCache)
        {
            var setComma = false;

            if (this._state == State.Chaining)
                this._stringBuilder.Append(".");

            this._stringBuilder.Append(methodName);
            this._stringBuilder.Append("(");

            foreach (var parameter in parameters)
            {
                if (setComma)
                    this._stringBuilder.Append(", ");

                var subMethodBuilder = new GroovyExpressionBuilder(State.Idle, this._stringBuilder);
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

            this._stringBuilder.Append(")");

            return new GroovyExpressionBuilder(State.Chaining, this._stringBuilder);
        }

        public GroovyExpressionBuilder AppendField(string fieldName)
        {
            if (this._state != State.Chaining)
                throw new InvalidOperationException();

            this._stringBuilder.Append(".");
            this._stringBuilder.Append(fieldName);

            return this;
        }

        public GroovyExpressionBuilder AppendConstant(object constant, IParameterCache parameterCache)
        {
            if (this._state == State.Chaining)
                throw new InvalidOperationException();

            if (constant is Enum)
            {
                return this
                    .AppendIdentifier(constant.GetType().Name)
                    .AppendField(Enum.GetName(constant.GetType(), constant));
            }

            this._stringBuilder.Append(parameterCache.Cache(constant));
            return this;
        }
    }
}